Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data.SqlTypes
Imports System.Collections
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmPatrimonioFideicomiso
    Inherits BasePage
    Private CodigoNemonico As String
    Private oPatriFideiBE As PatrimonioFideicomisoBE

#Region "/* Métodos de la Página */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                ViewState("Operacion") = Request.QueryString("ope")
                If (ViewState("Operacion") = "mod") Then
                    hdCodigoPatrimonioFideicomiso.Value = Request.QueryString("codigo")
                End If
                CargarCombos()
                hd.Value = ""
                ViewState("consecutivo") = 1
                ViewState("detalles") = Nothing
                If (ViewState("Operacion") = "mod") Then
                    cargarRegistro()
                Else
                    CargarDetalle("")
                End If
                btnModificarDetalle.Visible = False
                btnAgregarDetalle.Visible = True
            End If

            If Not Session("SS_DatosModal") Is Nothing Then
                tbCodigoMnemonicoDetalle.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                tbDescripcionDetalle.Text = CType(Session("SS_DatosModal"), String())(1).ToString().ToUpper
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try        
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click        
        Try
            Dim resultado As Boolean
            oPatriFideiBE = obtenerInstancia()
            If Not ValidarPatrimonio() Then Exit Sub
            If Not ValidarDetalle() Then Exit Sub
            Dim objPatriFideiBM As New PatrimonioFideicomisoBM
            If ViewState("Operacion") = "reg" Then
                hdCodigoPatrimonioFideicomiso.Value = objPatriFideiBM.Insertar(oPatriFideiBE, DatosRequest)
                If hdCodigoPatrimonioFideicomiso.Value = "" Then
                    resultado = False
                Else
                    resultado = objPatriFideiBM.InsertarDetalle(CrearObjetoDetalle(CType(ViewState("detalles"), PatrimonioFideicomisoDetalleBE)), DatosRequest)
                End If
                If resultado Then
                    AlertaJS("Se registró el Patrimonio Fideicomiso satisfactoriamente.")
                    limpiarCampos()
                    limpiarCamposDetalle()
                    CargarDetalle("")
                    ViewState("Operacion") = "reg"
                    hdCodigoPatrimonioFideicomiso.Value = ""
                    hdCodigoPatrimonioFideicomisoDetalle.Value = ""
                Else
                    AlertaJS("Hubo un error al registrar el patrimonio fideicomiso")
                    Exit Sub
                End If
            Else
                resultado = objPatriFideiBM.Modificar(oPatriFideiBE, DatosRequest)
                If resultado Then resultado = objPatriFideiBM.InsertarModificarDetalle(CrearObjetoDetalle(CType(ViewState("detalles"), PatrimonioFideicomisoDetalleBE)), DatosRequest)
                If resultado Then
                    AlertaJS("Se registró/modificó el Patrimonio Fideicomiso satisfactoriamente.")
                    limpiarCampos()
                    limpiarCamposDetalle()
                    CargarDetalle("")
                    ViewState("Operacion") = "reg"
                    hdCodigoPatrimonioFideicomiso.Value = ""
                    hdCodigoPatrimonioFideicomisoDetalle.Value = ""
                Else
                    AlertaJS("Hubo un error al registrar/modificar el patrimonio fideicomiso")
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Aceptar")
        End Try
    End Sub

    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            Dim index As Integer = CType(e.CommandArgument.ToString(), Integer)
            Dim CodigoPatrimonioFideicomisoDetalle As String
            CodigoPatrimonioFideicomisoDetalle = DirectCast(dgLista.Rows(index).FindControl("_CodigoPatrimonioFideicomisoDetalle"), HiddenField).Value
            hdCodigoPatrimonioFideicomisoDetalle.Value = CodigoPatrimonioFideicomisoDetalle
            If CodigoPatrimonioFideicomisoDetalle <> "" Then
                Select Case e.CommandName
                    Case "Eliminar"
                        cargarRegistroDetalle("eliminar", index)
                    Case "Modificar"
                        cargarRegistroDetalle("modificar", index)
                        btnModificarDetalle.Visible = True
                        btnAgregarDetalle.Visible = False
                End Select
            End If
        Catch ex As Exception
            AlertaJS("Ocuerrió un error en la Operación de la Grilla")
        End Try        
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("ibnEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?');")
                If (e.Row.Cells(7).Text = "A") Then
                    e.Row.Cells(7).Text = "Activo"
                ElseIf (e.Row.Cells(7).Text = "I") Then
                    e.Row.Cells(7).Text = "Inactivo"
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try        
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarDetalle(hdCodigoPatrimonioFideicomiso.Value)
        Catch ex As Exception
            AlertaJS("Ocurrió un error el la Paginación")
        End Try        
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("frmBusquedaPatrimonioFideicomiso.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar")
        End Try        
    End Sub

    Private Sub btnAgregarDetalle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarDetalle.Click
        Try
            If Not ValidarExistenciaLocal() Then Exit Sub
            Dim dsDetalleBE As PatrimonioFideicomisoDetalleBE
            dsDetalleBE = CType(ViewState("detalles"), PatrimonioFideicomisoDetalleBE)
            Dim drDetalle As PatrimonioFideicomisoDetalleBE.PatrimonioFideicomisoDetalleRow
            drDetalle = dsDetalleBE.Tables(0).NewRow
            drDetalle.CodigoPatrimonioFideicomisoDetalle = "TEMP" & ViewState("consecutivo").ToString
            ViewState("consecutivo") = CType(ViewState("consecutivo"), Integer) + 1
            drDetalle.CodigoMnemonico = tbCodigoMnemonicoDetalle.Text

            Dim strAuxIsin As String = ""
            Dim strAuxEmisor As String = ""
            ObtenerIsinEmisor(strAuxIsin, strAuxEmisor)
            drDetalle.CodigoIsin = strAuxIsin
            drDetalle.Emisor = strAuxEmisor
            drDetalle.Descripcion = tbDescripcionDetalle.Text.ToUpper
            drDetalle.Situacion = ddlSituacionDetalle.Items(ddlSituacionDetalle.SelectedIndex).Text
            dsDetalleBE.Tables(0).Rows.Add(drDetalle)

            ViewState("detalles") = dsDetalleBE
            dgLista.DataSource = dsDetalleBE
            dgLista.DataBind()
            limpiarCamposDetalle()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Agregar Detalle")
        End Try        
    End Sub

    Private Sub btnModificarDetalle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificarDetalle.Click
        Try
            If Not ValidarExistenciaLocal(hdCodigoPatrimonioFideicomisoDetalle.Value) Then Exit Sub
            Dim dsDetalleBE As PatrimonioFideicomisoDetalleBE
            dsDetalleBE = CType(ViewState("detalles"), PatrimonioFideicomisoDetalleBE)
            Dim drDetalle As PatrimonioFideicomisoDetalleBE.PatrimonioFideicomisoDetalleRow
            For Each drDetalle In dsDetalleBE.Tables(0).Rows
                If drDetalle.CodigoPatrimonioFideicomisoDetalle = hdCodigoPatrimonioFideicomisoDetalle.Value Then
                    If hdCodigoPatrimonioFideicomisoDetalle.Value Is Nothing Then
                        drDetalle.CodigoPatrimonioFideicomisoDetalle = ""
                    Else
                        drDetalle.CodigoPatrimonioFideicomisoDetalle = hdCodigoPatrimonioFideicomisoDetalle.Value
                    End If
                    drDetalle.CodigoMnemonico = tbCodigoMnemonicoDetalle.Text
                    Dim strAuxIsin As String = ""
                    Dim strAuxEmisor As String = ""
                    ObtenerIsinEmisor(strAuxIsin, strAuxEmisor)
                    drDetalle.CodigoIsin = strAuxIsin
                    drDetalle.Emisor = strAuxEmisor
                    drDetalle.Descripcion = tbDescripcionDetalle.Text.ToUpper
                    drDetalle.Situacion = ddlSituacionDetalle.Items(ddlSituacionDetalle.SelectedIndex).Text
                End If
            Next

            ViewState("detalles") = dsDetalleBE
            dgLista.DataSource = dsDetalleBE
            dgLista.DataBind()
            limpiarCamposDetalle()
            btnModificarDetalle.Visible = False
            btnAgregarDetalle.Visible = True
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Modificar Detalle")
        End Try       
    End Sub

#End Region

#Region "/* Métodos de la Página */"

    Private Sub CargarDetalle(ByVal strCodigoFideicomiso As String)
        Dim dsAux As New PatrimonioFideicomisoDetalleBE
        Dim oPatriFideiBM As New PatrimonioFideicomisoBM
        dsAux = oPatriFideiBM.SeleccionarPorCodigoDetalle(strCodigoFideicomiso, DatosRequest)
        ViewState("detalles") = dsAux
        dgLista.DataSource = dsAux
        dgLista.DataBind()
    End Sub

    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
        HelpCombo.LlenarComboBox(ddlSituacionDetalle, tablaSituacion, "Valor", "Nombre", False)
    End Sub

    Private Sub cargarRegistro()
        Dim oPatrimonioFideicomisoBM As New PatrimonioFideicomisoBM
        Dim oPatrimonioFideicomisoBE As New PatrimonioFideicomisoBE
        Dim oRow As PatrimonioFideicomisoBE.PatrimonioFideicomisoBERow

        oPatrimonioFideicomisoBE = New PatrimonioFideicomisoBM().SeleccionarPorFiltro(hdCodigoPatrimonioFideicomiso.Value, "", "", DatosRequest)
        oRow = DirectCast(oPatrimonioFideicomisoBE.PatrimonioFideicomisoBE.Rows(0), PatrimonioFideicomisoBE.PatrimonioFideicomisoBERow)

        ddlSituacion.SelectedValue = oRow.Situacion.ToString()
        tbDescripcion.Text = oRow.Descripcion.ToString
        tbTotalActivo.Text = Format(oRow.TotalActivo, "###,###,##0.0000000")
        tbTotalPasivo.Text = Format(oRow.TotalPasivo, "###,###,##0.0000000")
        tbPatrimonio.Text = Format(oRow.Patrimonio, "###,###,##0.0000000")
        tbFechaVigencia.Text = oRow.FechaVigencia
        hdCodigoPatrimonioFideicomiso.Value = oRow.CodigoPatrimonioFideicomiso
        txtFactorRiesgo.Text = oRow.FactorRiesgo
        txtFactorLiquidez.Text = oRow.FactorLiquidez

        CargarDetalle(hdCodigoPatrimonioFideicomiso.Value)
    End Sub

    Private Function obtenerInstancia() As PatrimonioFideicomisoBE
        Dim oPatrimonioFideicomisoBE As New PatrimonioFideicomisoBE
        Dim oRow As PatrimonioFideicomisoBE.PatrimonioFideicomisoBERow
        oRow = CType(oPatrimonioFideicomisoBE.PatrimonioFideicomisoBE.NewRow(), PatrimonioFideicomisoBE.PatrimonioFideicomisoBERow)

        If hdCodigoPatrimonioFideicomiso.Value Is Nothing Then
            oRow.CodigoPatrimonioFideicomiso = ""
        Else
            oRow.CodigoPatrimonioFideicomiso = hdCodigoPatrimonioFideicomiso.Value
        End If
        oRow.TotalActivo = CType(tbTotalActivo.Text, Decimal)
        oRow.TotalPasivo = CType(tbTotalPasivo.Text, Decimal)
        oRow.Patrimonio = CType(tbPatrimonio.Text, Decimal)
        oRow.FechaVigencia = UIUtility.ConvertirFechaaDecimal(tbFechaVigencia.Text)
        oRow.Situacion = ddlSituacion.SelectedValue
        oRow.Descripcion = tbDescripcion.Text
        oRow.FactorRiesgo = CDec(txtFactorRiesgo.Text)
        oRow.FactorLiquidez = CDec(txtFactorLiquidez.Text)


        oPatrimonioFideicomisoBE.PatrimonioFideicomisoBE.AddPatrimonioFideicomisoBERow(oRow)
        oPatrimonioFideicomisoBE.PatrimonioFideicomisoBE.AcceptChanges()
        Return oPatrimonioFideicomisoBE
    End Function

    Private Function CrearObjetoDetalle(ByVal objDetalle As PatrimonioFideicomisoDetalleBE) As PatrimonioFideicomisoDetalleBE
        Dim objRow As PatrimonioFideicomisoDetalleBE.PatrimonioFideicomisoDetalleRow
        For Each objRow In objDetalle.Tables(0).Rows
            objRow.CodigoPatrimonioFideicomiso = hdCodigoPatrimonioFideicomiso.Value
            objRow.Situacion = objRow.Situacion.Substring(0, 1)
        Next
        Return objDetalle
    End Function

    Private Function ValidarPatrimonio() As Boolean
        Dim objPatrimonioBM As New PatrimonioFideicomisoBM
        If objPatrimonioBM.ValidarPatrimonio(hdCodigoPatrimonioFideicomiso.Value, tbDescripcion.Text, DatosRequest) Then
            AlertaJS("El patrimonio fideicomiso ya se encuentra registrado")
            Return False
        End If
        Return True
    End Function

    Private Function ValidarDetalle() As Boolean
        Dim dsDetalleBE As PatrimonioFideicomisoDetalleBE
        dsDetalleBE = CType(ViewState("detalles"), PatrimonioFideicomisoDetalleBE)
        Dim i As Integer
        If dsDetalleBE.Tables(0).Rows.Count > 0 Then
            With dsDetalleBE.Tables(0)
                For i = 0 To .Rows.Count - 1
                    Dim objValidarExistencia As New PatrimonioFideicomisoBM
                    If objValidarExistencia.ValidarExistencia(.Rows(i)("CodigoPatrimonioFideicomisoDetalle"), .Rows(i)("CodigoMnemonico"), DatosRequest) Then
                        AlertaJS("El código mnemónico " & .Rows(i)("CodigoMnemonico") & " ya se encuentra registrado")
                        Return False
                    End If
                Next
            End With
        End If
        Return True
    End Function

    Private Function ValidarExistenciaLocal(Optional ByVal codigoRow As String = "") As Boolean
        Dim dsDetalleBE As PatrimonioFideicomisoDetalleBE
        dsDetalleBE = CType(ViewState("detalles"), PatrimonioFideicomisoDetalleBE)
        Dim i As Integer
        If dsDetalleBE.Tables(0).Rows.Count > 0 Then
            With dsDetalleBE.Tables(0)
                If codigoRow = "" Then
                    For i = 0 To .Rows.Count - 1
                        If .Rows(i)("CodigoMnemonico") = tbCodigoMnemonicoDetalle.Text Then
                            AlertaJS("Ya se ingresó el mnemónico")
                            Return False
                        End If
                    Next
                Else
                    For i = 0 To .Rows.Count - 1
                        If .Rows(i)("CodigoPatrimonioFideicomisoDetalle") <> codigoRow _
                        And .Rows(i)("CodigoMnemonico") = tbCodigoMnemonicoDetalle.Text Then
                            AlertaJS("Ya se ingresó el mnemónico")
                            Return False
                        End If
                    Next
                End If
            End With
        End If
        Return True
    End Function

    Private Sub limpiarCampos()
        ddlSituacion.SelectedIndex = 0
        tbTotalActivo.Text = ""
        tbTotalPasivo.Text = ""
        tbPatrimonio.Text = ""
        tbFechaVigencia.Text = ""
        hd.Value = ""
        tbDescripcion.Text = ""
        txtFactorLiquidez.Text = ""
        txtFactorRiesgo.Text = ""
    End Sub

    Private Sub limpiarCamposDetalle()
        ddlSituacionDetalle.SelectedIndex = 0
        tbCodigoMnemonicoDetalle.Text = ""
        tbDescripcionDetalle.Text = ""
    End Sub

    Private Sub ObtenerIsinEmisor(ByRef strIsin As String, ByRef strEmisor As String)
        Dim objPatri As New PatrimonioFideicomisoBM
        Dim dsAux As DataSet = objPatri.SeleccionarNemonicoCaracteristicas(tbCodigoMnemonicoDetalle.Text, DatosRequest)
        If dsAux.Tables(0).Rows.Count > 0 Then
            strIsin = dsAux.Tables(0).Rows(0)("CodigoISIN")
            strEmisor = dsAux.Tables(0).Rows(0)("Emisor")
        End If
    End Sub

    Private Sub cargarRegistroDetalle(ByVal tipo As String, ByVal index As Integer)
        If tipo = "eliminar" Then
            dgLista.Rows(index).Cells(7).Text = "Inactivo"
        ElseIf tipo = "modificar" Then
            tbCodigoMnemonicoDetalle.Text = dgLista.Rows(index).Cells(6).Text
            tbDescripcionDetalle.Text = dgLista.Rows(index).Cells(5).Text
            ddlSituacionDetalle.SelectedValue = dgLista.Rows(index).Cells(7).Text.Substring(0, 1)
        End If
    End Sub

#End Region

End Class
