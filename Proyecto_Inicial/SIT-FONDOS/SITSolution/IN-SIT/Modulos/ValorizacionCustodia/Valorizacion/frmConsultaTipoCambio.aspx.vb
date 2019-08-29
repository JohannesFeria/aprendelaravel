Imports SIT.BusinessEntities
Imports SIT.BusinessLayer
Imports System.Data
Imports System.Text

Partial Class Modulos_Valorizacion_y_Custodia_Valorizacion_frmConsultaTipoCambio
    Inherits BasePage

#Region " /* Declaración Variables */ "
    Dim oVectorTipoCambioBM As New VectorTipoCambioBM
    Dim oVectorTipoCambioBE As New VectorTipoCambio
    Dim oUtil As New UtilDM
    Private campos() As String = {"Moneda", "Tipo de Cambio"}
#End Region

#Region "/* Métodos de la Página */"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                'ViewState("vsFuenteInformacion") = "Real"
                tbFechaOperacion.Text = Me.FechaActual
                UIUtility.CargarMonedaOI(dlMoneda)
                'txtActual.Text = "Vector"
                Call CargaEntidadExterna()
                Call CargaTipoCambio()
                Call DescargaGrillaLista()
                ViewState("vsConsulta") = 1
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub ddlEntidadExterna_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlEntidadExterna.SelectedIndexChanged
        Try
            ViewState("vsFuenteInformacion") = ddlEntidadExterna.SelectedValue.Trim
            ViewState("vsConsulta") = 0
            Call DescargaGrillaLista()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar")
        End Try
    End Sub
    Private Sub btnConsulta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConsulta.Click
        Try
            If ddlEntidadExterna.SelectedValue.Trim <> "" Then
                Call CargaTipoDeCambio()
                ViewState("vsConsulta") = 1
            Else
                AlertaJS("Seleccione un tipo de fuente")
                ViewState("vsConsulta") = 0
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Consultar: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            If tbFechaOperacion.Text.Length = 0 Then
                AlertaJS("Seleccione una fecha")
                Exit Sub
            ElseIf ddlEntidadExterna.SelectedValue = "" Then
                AlertaJS("Seleccione un tipo de fuente")
                Exit Sub
            Else
                ViewState("vsModo") = "I"
                Call HabilitaBotones(False, False, True, True, False, False, False, False)
                Call HabilitaControlesEdicion(True, True, True)
                Call VisibilidadControlesEdicion(False, True)
                Call LimpiarControlesEdicion()
                'txtfecha.Text = tbFechaOperacion.Text
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub btnModificar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificar.Click
        Try
            If tbFechaOperacion.Text.Length = 0 Then
                AlertaJS("Seleccione una fecha")
                Exit Sub
            ElseIf ddlEntidadExterna.SelectedValue = "" Then
                AlertaJS("Seleccione un tipo de fuente")
                Exit Sub
            Else
                ViewState("vsModo") = "M"
                Call HabilitaBotones(False, False, True, True, False, False, False, False)
                Call HabilitaControlesEdicion(True, True, True)
                Call VisibilidadControlesEdicion(True, False)
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Modificar: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim operacion As String = ""
        Try
            Select Case ViewState("vsModo")
                Case "I"
                    operacion = "Insertar"
                    If IngresarTipoCambioSBS() Then
                        AlertaJS(ObtenerMensaje("ALERT87"))
                        Call LimpiarControlesEdicion()
                        Call CargaTipoCambio()
                    Else
                        AlertaJS(ObtenerMensaje("ALERT86"))
                        Exit Sub
                    End If
                Case "M"
                    operacion = "Modificar"
                    If ModificarTipoCambioSBS() Then
                        AlertaJS(ObtenerMensaje("ALERT85"))
                        Call LimpiarControlesEdicion()
                        Call CargaTipoCambio()
                    Else
                        AlertaJS(ObtenerMensaje("ALERT86"))
                        Exit Sub
                    End If
            End Select
            Call DescargaGrillaLista()
            Call CargaTipoCambio()
            Call HabilitaBotones(True, True, False, False, True, True, True, True)
            Call HabilitaControlesEdicion(False, False, False)
            ViewState("vsCodigoMoneda") = ""
            ViewState("vsFecha") = ""
            ViewState("vsEntidadExt") = ""
        Catch ex As Exception
            AlertaJS("Ocurrió un error al " + operacion & ": " & Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            ViewState("vsModo") = ""
            Call HabilitaBotones(True, True, False, False, True, True, True, True)
            Call HabilitaControlesEdicion(False, False, False)
            Call VisibilidadControlesEdicion(True, False)
            Call LimpiarControlesEdicion()
            ViewState("vsCodigoMoneda") = ""
            ViewState("vsFecha") = ""
            ViewState("vsEntidadExt") = ""
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Cancelar")
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir de la Página")
        End Try
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try
            Dim sFechaOperacion As String = tbFechaOperacion.Text
            Dim sFuenteInformacion As String
            sFechaOperacion = sFechaOperacion.Substring(6, 4) & sFechaOperacion.Substring(3, 2) & sFechaOperacion.Substring(0, 2)
            sFuenteInformacion = ViewState("vsFuenteInformacion").ToString()
            EjecutarJS("showPopup('" + sFechaOperacion + "','" + sFuenteInformacion + "');")
        Catch ex As Exception
            AlertaJS("Ocurrió un error el Imprimir")
        End Try
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        Try
            oVectorTipoCambioBM = Nothing
            oVectorTipoCambioBE = Nothing
            oUtil = Nothing
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el Proceso")
        End Try
    End Sub

    Protected Sub dgTipoCambio_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgTipoCambio.RowCommand
        Try
            Dim index As Int32 = 0
            If e.CommandName = "Seleccionar" Then
                index = Convert.ToInt32(e.CommandArgument)
                Dim row As GridViewRow = dgTipoCambio.Rows(index)
                Dim var = dgTipoCambio.DataKeys(index).Values
                dgTipoCambio.SelectedIndex = index

                ViewState("vsCodigoMoneda") = var(0).ToString()
                ViewState("vsFecha") = var(1).ToString()
                ViewState("vsEntidadExt") = var(2).ToString()

                txtMoneda.Text = row.Cells(1).Text
                txtTipoCambio.Text = row.Cells(2).Text
                txtTipoCambioPrimario.Text = row.Cells(3).Text
                txtMoneda.Enabled = False
                txtTipoCambio.Enabled = False
                txtTipoCambioPrimario.Enabled = False
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Protected Sub dgTipoCambio_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgTipoCambio.PageIndexChanging
        Try
            dgTipoCambio.PageIndex = e.NewPageIndex
            CargaTipoCambio()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try
    End Sub

#End Region

#Region " /* Métodos Personalizados*/"

    Private Sub DescargaGrillaLista()
        dgTipoCambio.DataSource = UIUtility.GetStructureTablebase(campos)
        dgTipoCambio.DataBind()
    End Sub

    Private Sub CargaEntidadExterna()
        'Dim objParametrosGenerales As New ParametrosGeneralesBM
        'ddlEntidadExterna.DataTextField = "Nombre"
        'ddlEntidadExterna.DataValueField = "Valor"
        'ddlEntidadExterna.DataSource = objParametrosGenerales.ListarEntidadesExternas("PrecioEExt", DatosRequest)
        'ddlEntidadExterna.DataBind()
        'UIUtility.InsertarElementoSeleccion(ddlEntidadExterna)
        'objParametrosGenerales = Nothing
        Dim dtParametrosGenarales As DataTable
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        dtParametrosGenarales = oParametrosGeneralesBM.Listar(ParametrosSIT.VECTOR_PRECIO_VALORIZACION, DatosRequest)
        HelpCombo.LlenarComboBox(ddlEntidadExterna, dtParametrosGenarales, "Valor", "Nombre", True)
    End Sub

    Private Sub CargaTipoCambio()
        'Dim sFechaOperacion As String
        'If Not (String.IsNullOrEmpty(tbFechaOperacion.Text)) Then
        '    sFechaOperacion = tbFechaOperacion.Text.Trim
        '    sFechaOperacion = sFechaOperacion.Substring(6, 4) & sFechaOperacion.Substring(3, 2) & sFechaOperacion.Substring(0, 2)
        'Else
        '    sFechaOperacion = UIUtility.ObtenerFechaMaximaNegocio()
        'End If
        Dim dtblDatos As DataTable
        'dtblDatos = New VectorTipoCambioBM().Listar(sFechaOperacion, sFuenteInformacion, "", DatosRequest).Tables(0)
        dtblDatos = New VectorTipoCambioBM().Listar(UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), ddlEntidadExterna.SelectedValue, "", DatosRequest).Tables(0)
        dgTipoCambio.DataSource = dtblDatos
        dgTipoCambio.DataBind()
    End Sub

    Private Sub HabilitaBotones(ByVal pIngresar As Boolean, ByVal pModificar As Boolean, ByVal pAceptar As Boolean, ByVal pCancelar As Boolean, ByVal pImagenCalendario As Boolean, ByVal pFechaOperacion As Boolean, ByVal pBloomberg As Boolean, ByVal pEntidadExterna As Boolean)
        btnIngresar.Visible = pIngresar
        btnModificar.Visible = pModificar
        btnAceptar.Visible = pAceptar
        btnCancelar.Visible = pCancelar
        tbFechaOperacion.Enabled = pFechaOperacion
        btnBloomberg.Enabled = pBloomberg
        ddlEntidadExterna.Enabled = pEntidadExterna

        If (pFechaOperacion) Then
            ingFecha.Attributes.Add("class", "input-append date")
        Else
            ingFecha.Attributes.Add("class", "input-append")
        End If
    End Sub

    Private Sub HabilitaControlesEdicion(ByVal bTipoCambioPrimario As Boolean, ByVal bTipoCambio As Boolean, ByVal bMoneda As Boolean)
        txtTipoCambio.Enabled = bTipoCambio
        txtTipoCambioPrimario.Enabled = bTipoCambioPrimario
        dlMoneda.Enabled = bMoneda
    End Sub

    Private Sub VisibilidadControlesEdicion(ByVal bLabelMoneda As Boolean, ByVal bComboMoneda As Boolean)
        txtMoneda.Visible = bLabelMoneda
        dlMoneda.Visible = bComboMoneda
    End Sub

    Private Function VerificaTipoCambio(ByVal sFecha As String, ByVal sEntidadExt As String, ByVal sCodigoMoneda As String) As Boolean
        Dim oTipoCambioBE As DataSet = oVectorTipoCambioBM.SeleccionarTipoCambio(sFecha, sEntidadExt, sCodigoMoneda, DatosRequest)
        If oTipoCambioBE.Tables(0).Rows.Count > 0 Then
            Return False
            Exit Function
        End If
        oTipoCambioBE = Nothing
        Return True
    End Function

    Private Function ValidarTipoCambioSBS() As Boolean
        Dim sFecha As String = tbFechaOperacion.Text
        Dim sTipoCambio As String = "0" & txtTipoCambio.Text.Trim
        Dim sTipoCambioPrimario As String = "0" & txtTipoCambioPrimario.Text.Trim

        If ViewState("vsModo") = "I" Then
            If dlMoneda.SelectedValue.Trim = "" Then
                AlertaJS(ObtenerMensaje("ALERT82"))
                Return False
                Exit Function
            End If
            If tbFechaOperacion.Text.Trim = "" Then
                AlertaJS(ObtenerMensaje("ALERT83"))
                Return False
                Exit Function
            End If
            If Not VerificaTipoCambio(sFecha.Substring(6, 4) & sFecha.Substring(3, 2) & sFecha.Substring(0, 2), ViewState("vsFuenteInformacion"), dlMoneda.SelectedValue.Trim) Then
                AlertaJS(ObtenerMensaje("ALERT84"))
                Return False
                Exit Function
            End If
        End If

        If txtTipoCambioPrimario.Text.Trim = "." Or txtTipoCambioPrimario.Text.Trim = "," Then
            AlertaJS(ObtenerMensaje("ALERT79"))
            Return False
            Exit Function
        ElseIf CDec("0" & txtTipoCambioPrimario.Text.Trim) < 0 Then
            AlertaJS(ObtenerMensaje("ALERT80"))
            Return False
            Exit Function
        ElseIf txtTipoCambioPrimario.Text.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT79"))
            Return False
            Exit Function
        ElseIf CDec(sTipoCambioPrimario) > 99 Then
            AlertaJS(ObtenerMensaje("ALERT81"))
            Return False
            Exit Function
        End If

        Return True
    End Function

    Private Function IngresarTipoCambioSBS() As Boolean
        If ValidarTipoCambioSBS() Then
            oVectorTipoCambioBE = CrearObjetoTipoCambioSBS(dlMoneda.SelectedValue.Trim, _
                                                    UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), _
                                                    ddlEntidadExterna.SelectedValue, _
                                                    Decimal.Parse(txtTipoCambio.Text.Trim), _
                                                    Decimal.Parse(txtTipoCambioPrimario.Text))
            Return oVectorTipoCambioBM.InsertarPorMantenimiento(oVectorTipoCambioBE, DatosRequest)
        Else
            Return False
        End If
    End Function

    Private Function ModificarTipoCambioSBS() As Boolean
        If ValidarTipoCambioSBS() Then
            oVectorTipoCambioBE = CrearObjetoTipoCambioSBS(ViewState("vsCodigoMoneda"), _
                                                    UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), _
                                                    ddlEntidadExterna.SelectedValue, _
                                                    Decimal.Parse(txtTipoCambio.Text.Trim), _
                                                    Decimal.Parse(txtTipoCambioPrimario.Text))
            Return oVectorTipoCambioBM.Modificar(oVectorTipoCambioBE, DatosRequest)
        Else
            Return False
        End If
    End Function

    Private Function CrearObjetoTipoCambioSBS( _
                                                ByVal CodigoMoneda As String, _
                                                ByVal Fecha As Decimal, _
                                                ByVal EntidadExt As String, _
                                                ByVal Valor As Decimal, _
                                                ByVal ValorPrimario As Decimal) As VectorTipoCambio
        Dim oVectorTipoCambioBE As New VectorTipoCambio
        Dim oRow As VectorTipoCambio.VectorTipoCambioRow
        oRow = CType(oVectorTipoCambioBE.VectorTipoCambio.NewRow(), VectorTipoCambio.VectorTipoCambioRow)

        oRow.CodigoMoneda() = CodigoMoneda
        oRow.Fecha() = Fecha
        oRow.EntidadExt() = EntidadExt
        oRow.Valor() = Valor
        oRow.ValorPrimario() = ValorPrimario
        oRow.Manual = "S"

        oVectorTipoCambioBE.VectorTipoCambio.AddVectorTipoCambioRow(oRow)
        oVectorTipoCambioBE.VectorTipoCambio.AcceptChanges()
        Return oVectorTipoCambioBE
    End Function

    Private Sub LimpiarControlesEdicion()
        dlMoneda.SelectedIndex = 0
        txtTipoCambio.Text = ""
        txtTipoCambioPrimario.Text = ""
        'txtfecha.Text = ""
        txtMoneda.Text = ""
    End Sub

    Public Sub SeleccionarTipoCambio(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim strcadena As String
        Dim strCodigoMoneda As String
        Dim strFecha As String
        Dim strEntidadExt As String

        strcadena = e.CommandArgument
        strCodigoMoneda = strcadena.Split(",").GetValue(0)
        strFecha = strcadena.Split(",").GetValue(1)
        strEntidadExt = strcadena.Split(",").GetValue(2)

        ViewState("vsCodigoMoneda") = strCodigoMoneda
        ViewState("vsFecha") = strFecha
        ViewState("vsEntidadExt") = strEntidadExt
    End Sub

    Private Sub CargaTipoDeCambio()
        Call DescargaGrillaLista()
        Call CargaTipoCambio()
        Call LimpiarControlesEdicion()
        Call HabilitaBotones(True, True, False, False, True, True, True, True)
        Call HabilitaControlesEdicion(False, False, False)
        ViewState("vsCodigoMoneda") = ""
        ViewState("vsFecha") = ""
        ViewState("vsEntidadExt") = ""
    End Sub
#End Region

End Class
