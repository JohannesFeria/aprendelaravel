Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports ParametrosSIT
Imports System.Data
Imports System.IO

Partial Class Modulos_Contabilidad_frmRevisionAsientosContables
    Inherits BasePage

#Region "/* Declaracion de Variables */"

    Dim oUtil As New UtilDM

    Public Enum TIPO_ACCION As Byte
        [NUEVO] = 1
        [MODIFICAR] = 2
    End Enum
    Dim oAsientoContableBE As New AsientoContableBE
    Dim oAsientoContableBM As New AsientoContableBM
    Dim strmensaje As String
    Dim strMensajeObli As String = ""
    Dim CodigoPortafolioSBSKey As String = ""
    Dim NumeroAsientoKey As String = ""
    Dim SecuenciaKey As String = ""
    Private campos() As String = {"NumeroAsiento", "CodigoMoneda", "Glosa", "DebeHaber", "CuentaContable", "Importe"}
    Private tipos() As String = {"System.String", "System.String", "System.String", "System.String", "System.String", "System.String"}

#End Region

#Region "/* Propiedades */"

    Private Property VistaAsientoContable() As AsientoContableBE
        Get
            Return DirectCast(ViewState("AsientoContable"), AsientoContableBE)
        End Get
        Set(ByVal Value As AsientoContableBE)
            ViewState("AsientoContable") = Nothing
            ViewState("AsientoContable") = Value
        End Set
    End Property

#End Region


#Region "/* Eventos de la Pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.btModal.Style.Add("display", "none")
            If Not Session("SS_DatosModal") Is Nothing Then
                Dim sModal As String() = CType(Session("SS_DatosModal"), String())
                Select Case hdModal.Value
                    Case "1"
                        txtNroCuenta.Text = sModal(0)
                End Select
                Session.Remove("SS_DatosModal")
            End If

            If Not Page.IsPostBack Then
                CargarPagina()
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub ibListar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibListar.Click
        Try
            Listar()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub ibAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibAceptar.Click
        Try
            Aceptar()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub ibSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibSalir.Click
        Response.Redirect("../../frmDefault.aspx")
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex
        CargarDatosGrilla()
    End Sub

    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Dim fechaApertura As String = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaAperturaContable(Me.ddlFondo.SelectedValue))

        Try
            Dim decFechaApertura = UIUtility.ConvertirFechaaDecimal(fechaApertura)
            Dim decFechaOperacion = UIUtility.ConvertirFechaaDecimal(Me.txtFechaOperacion.Text)
            Dim fila As Integer
            'If IsNumeric(e.CommandArgument) Then
            '    fila = Convert.ToInt32(e.CommandArgument)
            'End If
            Select Case e.CommandName
                Case "Modificar"
                    Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                    fila = row.DataItemIndex 'row.RowIndex
                    If (decFechaApertura > decFechaOperacion) Then
                        AlertaJS("No se puede modificar Asientos contables generados en un periodo cerrado.")
                        Return
                    End If

                    Me.ibIngresar.Visible = False
                    Me.ibAceptar.Visible = True
                    Me.ibAceptar.Attributes.Add("onclick", "javascript:return ValidarDetalle();")

                    EditarGrilla(fila)
                Case "Eliminar"
                    Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                    fila = row.DataItemIndex 'row.RowIndex
                    If (decFechaApertura > decFechaOperacion) Then
                        AlertaJS("No se puede eliminar Asientos contables generados en un periodo cerrado.")
                        Return
                    End If

                    oAsientoContableBE = Me.VistaAsientoContable
                    CodigoPortafolioSBSKey = oAsientoContableBE.AsientoContable.Rows(fila)("CodigoPortafolioSBS").ToString()
                    NumeroAsientoKey = oAsientoContableBE.AsientoContable.Rows(fila)("NumeroAsiento").ToString()
                    SecuenciaKey = oAsientoContableBE.AsientoContable.Rows(fila)("Secuencia").ToString()
                    ViewState("IndiceSeleccionado") = CodigoPortafolioSBSKey + "," + NumeroAsientoKey + "," + SecuenciaKey
                    Eliminar(fila)
            End Select

        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try

    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        EditarCeldaGrilla(e.Row)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim btnEliminar As ImageButton = e.Row.Cells(1).Controls(1)

            btnEliminar.Attributes.Add("onclick", "return confirm('¿Desea eliminar el registro seleccionado?');")
            If e.Row.Cells(4).Text = "0" Then
                Dim btnModificar As ImageButton = e.Row.Cells(0).Controls(1)
                btnEliminar.Visible = False
                btnModificar.Visible = False
                e.Row.Cells(3).Text = ""
                e.Row.Cells(7).Text = ""
                e.Row.Cells(9).Text = "Total Diferencia :"
                e.Row.Cells(10).Text = ""
            End If
        End If
    End Sub

    Private Sub ddlMatriz_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMatriz.SelectedIndexChanged
        CargarComboNumeroAsiento()
    End Sub

#End Region


#Region "/* Metodos Personalizados */"

#Region "/* Validacion */"
    Private Function ValidarDataEntry() As Boolean
        Return Me.VistaAsientoContable.AsientoContable.Rows.Count > 0
    End Function

    Private Function ValidarBusqueda() As Boolean
        Dim ok As Boolean = True
        If (txtFechaOperacion.Text = "") Then
            ok = False
            strMensajeObli = strMensajeObli + "- Fecha <br />"
        End If
        If (ddlFondo.SelectedItem.Text = "--Seleccione--") Then
            ok = False
            strMensajeObli = strMensajeObli + "- Fondo <br />"
        End If
        Return ok
    End Function

    Private Function ValidarIngreso() As Boolean
        Dim ok As Boolean = True
        If (txtFechaOperacion.Text = "") Then
            ok = False
            strMensajeObli = strMensajeObli + "- Fecha de Operacion <br />"
        End If
        If (ddlFondo.SelectedItem.Text = "--Seleccione--") Then
            ok = False
            strMensajeObli = strMensajeObli + "- Fondo <br />"
        End If
        If (ddlMatriz.SelectedItem.Text = "--Seleccione--") Then
            ok = False
            strMensajeObli = strMensajeObli + "- Matriz <br />"
        End If
        If (ddlNumeroAsiento.SelectedItem.Text = "--Seleccione--") Then
            ok = False
            strMensajeObli = strMensajeObli + "- Numero Asiento <br />"
        End If
        If (ddlDebeHaber.SelectedItem.Text = "--Seleccione--") Then
            ok = False
            strMensajeObli = strMensajeObli + "- Debe/Haber <br />"
        End If
        If (txtNroCuenta.Text = "") Then
            ok = False
            strMensajeObli = strMensajeObli + "- Nro de Cuenta <br />"
        End If
        If (txtGlosa.Text = "") Then
            ok = False
            strMensajeObli = strMensajeObli + "- Glosa <br />"
        End If
        If (txtImporteSoles.Text = "") Then
            ok = False
            strMensajeObli = strMensajeObli + "- Importe Soles <br />"
        End If
        If (txtImporteOrigen.Text = "") Then
            ok = False
            strMensajeObli = strMensajeObli + "- Importe Origen <br />"
        End If
        If (ddlMoneda.SelectedItem.Text = "--Seleccione--") Then
            ok = False
            strMensajeObli = strMensajeObli + "- Moneda Origen <br />"
        End If
        Return ok

    End Function
#End Region

#Region "/* Cargar */"

    Private Sub CargarComboNumeroAsiento()
        Dim dtMatriz As DataTable
        Dim oMatrizContable As New MatrizContableBM
        dtMatriz = oMatrizContable.ListarTablaMatriz(Me.DatosRequest).MatrizContable
        Dim tablaMatriz As String = dtMatriz.Select("CodigoMatrizContable=" & ddlMatriz.SelectedValue)(0)("TablaMatriz")
        Dim dtNumeroAsiento As DataTable
        Dim oAsientoContable As New AsientoContableBM
        If (ddlMatriz.SelectedItem.Text <> "--Seleccione--") Then
            dtNumeroAsiento = oAsientoContable.ListarNumeroAsiento(tablaMatriz, ddlFondo.SelectedValue, UIUtility.ConvertirFechaaDecimal(txtFechaOperacion.Text), DatosRequest)
            HelpCombo.LlenarComboBox(Me.ddlNumeroAsiento, dtNumeroAsiento, "NumeroAsiento", "NumeroAsiento", True)
            ddlNumeroAsiento.SelectedIndex = 1
        End If
    End Sub

    Private Sub CargarCombos()

        Dim dtPortafolio As DataTable
        Dim dtMoneda As DataTable
        Dim dtParametros As DataTable
        Dim dtMatriz As DataTable

        Dim oPortafolioBM As New PortafolioBM
        Dim oMonedaBM As New MonedaBM
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim oMatrizContable As New MatrizContableBM

        dtPortafolio = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBox(Me.ddlFondo, dtPortafolio, "CodigoPortafolio", "Descripcion", True)
        'ddlFondo.Items.Remove(PORTAFOLIO_MULTIFONDOS)

        dtMoneda = oMonedaBM.Listar(Me.DatosRequest).Moneda
        HelpCombo.LlenarComboBox(Me.ddlMoneda, dtMoneda, "CodigoMoneda", "Descripcion", True)

        dtParametros = oParametrosGenerales.Listar("MatrizCont", Me.DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlDebeHaber, dtParametros, "Valor", "Nombre", True)

        dtMatriz = oMatrizContable.ListarTablaMatriz(Me.DatosRequest).MatrizContable
        HelpCombo.LlenarComboBox(Me.ddlMatriz, dtMatriz, "CodigoMatrizContable", "Descripcion", True)

    End Sub

    Private Sub InicializarGrilla()
        Dim dtblGenerico As DataTable = UIUtility.GetStructureTablebase(campos, tipos)
        dgLista.DataSource = dtblGenerico : dgLista.DataBind()
    End Sub

    Private Sub CargarPagina()
        Me.ibListar.Attributes.Add("onclick", "javascript:return ValidarCabecera();")
        Me.ibIngresar.Attributes.Add("onclick", "javascript:return ValidarDetalle();")

        Me.txtFechaOperacion.Text = oUtil.RetornarFechaSistema
        CargarCombos()
        Me.txtFechaOperacion.Text = ""
        InicializarGrilla()
    End Sub
#End Region

#Region "/* Operacion de Mantenimiento */"

    Private Sub Insertar()
        Try

            strMensajeObli = "Los siguientes campos son obligatorios: <br />"
            Dim swDatoOK As Boolean
            If ValidarIngreso() Then
                swDatoOK = ValidarCamposDetalleImporte()
                If swDatoOK = True Then
                    oAsientoContableBE = New AsientoContableBE

                    Dim oAsientoContableRow As AsientoContableBE.AsientoContableRow
                    oAsientoContableRow = oAsientoContableBE.AsientoContable.NewAsientoContableRow()
                    oAsientoContableRow.CodigoPortafolioSBS = ddlFondo.SelectedValue().ToString
                    oAsientoContableRow.NumeroAsiento = ddlNumeroAsiento.SelectedValue().ToString
                    oAsientoContableRow.FechaAsiento = Convert.ToDecimal(txtFechaOperacion.Text.Substring(6, 4) + txtFechaOperacion.Text.Substring(3, 2) + txtFechaOperacion.Text.Substring(0, 2))
                    oAsientoContableRow.CodigoMatrizContable = ddlMatriz.SelectedValue().ToString
                    oAsientoContableRow.TipoAsiento = "M"
                    oAsientoContableRow.CodigoMoneda = ddlMoneda.SelectedValue().ToString
                    oAsientoContableRow.Glosa = txtGlosa.Text
                    oAsientoContableRow.DebeHaber = ddlDebeHaber.SelectedValue().ToString
                    oAsientoContableRow.Importe = Convert.ToDecimal(Me.txtImporteSoles.Text.Replace(".", UIUtility.DecimalSeparator()))
                    oAsientoContableRow.ImporteOrigen = Convert.ToDecimal(Me.txtImporteOrigen.Text.Replace(".", UIUtility.DecimalSeparator()))
                    oAsientoContableRow.CuentaContable = Me.txtNroCuenta.Text
                    oAsientoContableBM.InsertarRevision(oAsientoContableRow, DatosRequest)
                    oAsientoContableBE.AsientoContable.AddAsientoContableRow(oAsientoContableRow)
                    oAsientoContableBE.AsientoContable.AcceptChanges()
                    Me.VistaAsientoContable = oAsientoContableBE

                    CargarDatosGrilla()
                    LimpiarControlesDetalle()

                    AlertaJS("Se grabó correctamente el Asiento")

                Else
                    AlertaJS("Importe ingresado inválido")
                End If
            Else
                AlertaJS(strMensajeObli)
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub Aceptar()
        Dim codigo As String
        codigo = ViewState("IndiceSeleccionado")
        If (codigo = "") Then
            AlertaJS("Debe seleccionar un registro.")
        Else
            Modificar()
        End If
    End Sub

    Private Sub Modificar()

        Try
            strMensajeObli = "Los siguientes campos son obligatorios: <br />"
            If ValidarIngreso() Then
                If ValidarCamposDetalleImporte() Then
                    Dim codigo As String

                    oAsientoContableBE = New AsientoContableBE
                    Dim oAsientoContableRow As AsientoContableBE.AsientoContableRow
                    oAsientoContableRow = oAsientoContableBE.AsientoContable.NewAsientoContableRow()
                    oAsientoContableRow.CodigoPortafolioSBS = ddlFondo.SelectedValue().ToString

                    oAsientoContableRow.FechaAsiento = Convert.ToDecimal(txtFechaOperacion.Text.Substring(6, 4) + txtFechaOperacion.Text.Substring(3, 2) + txtFechaOperacion.Text.Substring(0, 2))
                    oAsientoContableRow.CodigoMoneda = ddlMoneda.SelectedValue.ToString
                    oAsientoContableRow.Glosa = txtGlosa.Text

                    oAsientoContableRow.DebeHaber = ddlDebeHaber.SelectedValue().ToString
                    oAsientoContableRow.Importe = Convert.ToDecimal(Me.txtImporteSoles.Text.Replace(".", UIUtility.DecimalSeparator()))
                    oAsientoContableRow.ImporteOrigen = Convert.ToDecimal(Me.txtImporteOrigen.Text.Replace(".", UIUtility.DecimalSeparator()))
                    oAsientoContableRow.CuentaContable = Me.txtNroCuenta.Text
                    codigo = ViewState("IndiceSeleccionado")

                    CodigoPortafolioSBSKey = codigo.Split(",")(0)
                    NumeroAsientoKey = codigo.Split(",")(1)
                    SecuenciaKey = codigo.Split(",")(2)


                    oAsientoContableBM.ModificarRevision(CodigoPortafolioSBSKey, NumeroAsientoKey, SecuenciaKey, oAsientoContableRow, DatosRequest)

                    LimpiarControlesDetalle()
                    CargarDatosGrilla()
                    GenerarArchivo() 'HSP 20151027. Agregado
                    AlertaJS("Se actualizó correctamente el Asiento")
                    Me.ibAceptar.Visible = False
                    Me.ibIngresar.Visible = True
                Else
                    AlertaJS("Importe ingresado inválido")
                End If

            Else
                AlertaJS(strMensajeObli)
            End If

        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    'HSP 20151027. Inicio agregado
    Private Sub GenerarArchivo(Optional ByVal pTipoProceso As String = "")
        Dim oAsientoContable As New AsientoContableBM
        Dim dsArchivo As DataSet
        Dim registro As String = ""
        Dim PrefijoArchivo As String = "Lote"
        Dim sFileName As String = ""
        Dim sRutaFile As String = ""
        Dim sFecha As String = ""
        Dim FechaProceso As Decimal
        Dim errorGenerar As Boolean = False
        Dim i As Integer
        Dim extension As String = ViewState("Extension")
        Dim longitudRegistro As Integer = ViewState("LongitudRegistro")
        Dim auxformato As Decimal = 0.0
        Dim sRuta As String = String.Empty
        Dim sTipoLote As String = String.Empty
        errorGenerar = False

        FechaProceso = UIUtility.ConvertirFechaaDecimal(txtFechaOperacion.Text)
        Select Case ddlTipoLote.SelectedValue
            Case "I"
                sTipoLote = "CVI"
            Case "T"
                sTipoLote = "CCI"
            Case "V"
                sTipoLote = "VC"
        End Select

        dsArchivo = oAsientoContable.AsientoContable_Interface(ddlFondo.SelectedValue, FechaProceso, sTipoLote)

        If dsArchivo.Tables(0).Rows.Count > 0 Then
            Dim oArchivoPlanoBM As New ArchivoPlanoBM
            Dim dtArchivoPlano As New DataSet
            dtArchivoPlano = oArchivoPlanoBM.Seleccionar(COD_ARCHIVO_LOTES, MyBase.DatosRequest())
            extension = dtArchivoPlano.Tables(0).Rows(0).Item(3).ToString()
            longitudRegistro = dtArchivoPlano.Tables(0).Rows(0).Item(7).ToString()
            sRuta = dtArchivoPlano.Tables(0).Rows(0).Item(4)
            sFecha = Convert.ToDateTime(txtFechaOperacion.Text).ToString("yyyyMMdd")
            sFileName = PrefijoArchivo & pTipoProceso & "_" & sTipoLote & ddlFondo.SelectedValue & "_" & sFecha & "." & extension
            sRutaFile = sRuta & sFileName

            Dim tw As TextWriter

            'Modificado 
            If (File.Exists(sRutaFile)) Then File.Delete(sRutaFile)
            tw = New StreamWriter(sRutaFile, True, Encoding.ASCII)

            i = 0
            For i = 0 To dsArchivo.Tables(0).Rows.Count - 1
                registro = ""
                registro += dsArchivo.Tables(0).Rows(i).Item("PerVou").ToString().Trim().PadRight(4)
                registro += dsArchivo.Tables(0).Rows(i).Item("CodEmp").ToString().Trim().PadLeft(2)
                registro += dsArchivo.Tables(0).Rows(i).Item("CodLib").ToString().Trim().PadRight(2)
                registro += dsArchivo.Tables(0).Rows(i).Item("NroVou").ToString().Trim().PadRight(5)
                registro += dsArchivo.Tables(0).Rows(i).Item("CorVou").ToString().Trim().PadRight(5)
                registro += dsArchivo.Tables(0).Rows(i).Item("FecVou").ToString().Trim().PadRight(8)
                registro += dsArchivo.Tables(0).Rows(i).Item("AsiVou").ToString().Trim().PadRight(5)
                registro += dsArchivo.Tables(0).Rows(i).Item("CtaVou").ToString().Trim().PadRight(15)
                registro += dsArchivo.Tables(0).Rows(i).Item("CenCos").ToString().Trim().PadRight(12)
                registro += dsArchivo.Tables(0).Rows(i).Item("CtaCte").ToString().Trim().PadRight(16)
                registro += dsArchivo.Tables(0).Rows(i).Item("TipCta").ToString().Trim().PadRight(2)
                registro += dsArchivo.Tables(0).Rows(i).Item("TipDoc").ToString().Trim().PadRight(2)
                registro += dsArchivo.Tables(0).Rows(i).Item("Nrodoc").ToString().Trim().PadRight(12)
                registro += dsArchivo.Tables(0).Rows(i).Item("FecVen").ToString().Trim().PadRight(8)
                If pTipoProceso = String.Empty Then
                    If dsArchivo.Tables(0).Rows(i).Item("Debe").ToString.Trim = "" Then
                        registro += auxformato.ToString("0.00").PadLeft(13)
                    Else
                        registro += ToNullDecimal(dsArchivo.Tables(0).Rows(i).Item("Debe")).ToString("0.00").PadLeft(13)
                    End If
                    If dsArchivo.Tables(0).Rows(i).Item("Haber").ToString.Trim = "" Then
                        registro += auxformato.ToString("0.00").PadLeft(13)
                    Else
                        registro += ToNullDecimal(dsArchivo.Tables(0).Rows(i).Item("Haber")).ToString("0.00").PadLeft(13)
                    End If
                Else
                    registro += ToNullDecimal(dsArchivo.Tables(0).Rows(i).Item("Haber")).ToString("0.00").PadLeft(13)
                    registro += ToNullDecimal(dsArchivo.Tables(0).Rows(i).Item("Debe")).ToString("0.00").PadLeft(13)
                End If
                registro += dsArchivo.Tables(0).Rows(i).Item("Moneda").ToString().PadRight(1)
                registro += ToNullDecimal(dsArchivo.Tables(0).Rows(i).Item("Tipcam")).ToString("0.0000").PadLeft(13)
                registro += ToNullDecimal(dsArchivo.Tables(0).Rows(i).Item("ValOtraMon")).ToString("0.00").PadLeft(13)
                registro += dsArchivo.Tables(0).Rows(i).Item("CenGes").ToString().PadRight(6)
                registro += dsArchivo.Tables(0).Rows(i).Item("NombreTercero").ToString().PadRight(50)
                registro += dsArchivo.Tables(0).Rows(i).Item("Glosa").ToString().PadRight(80)
                If (dsArchivo.Tables(0).Rows(i).Item("DescripcionArchivo").ToString().Trim().Length > 50) Then
                    registro += dsArchivo.Tables(0).Rows(i).Item("DescripcionArchivo").ToString().Trim().Substring(1, 50)
                Else
                    registro += dsArchivo.Tables(0).Rows(i).Item("DescripcionArchivo").ToString().Trim().PadRight(50)
                End If
                registro += dsArchivo.Tables(0).Rows(i).Item("Per_Trans").ToString().PadRight(6)
                registro += dsArchivo.Tables(0).Rows(i).Item("FecDoc").ToString().PadRight(10)

                If ((longitudRegistro - registro.Length) < 3 And (longitudRegistro - registro.Length) >= 0) Then 'HSP 20150721.Para montos grandes en relacion a JPY
                    tw.WriteLine(registro)
                Else
                    errorGenerar = True
                    Exit For
                End If
            Next

            tw.Close()

            If Not errorGenerar Then
                AlertaJS("Se ha generado correctamente el archivo Interfaz.") ' + sFileName
            Else
                AlertaJS("Error al generar el archivo Interfaz.") ' + sFileName
            End If
        Else
            AlertaJS("No existen datos para el archivo Interfaz de lote.")
        End If

    End Sub
    'HSP 20151027. Fin agregado

    Private Sub Eliminar(ByVal fila As Integer)
        Dim codigo As String
        codigo = ViewState("IndiceSeleccionado")
        CodigoPortafolioSBSKey = codigo.Split(",")(0)
        NumeroAsientoKey = codigo.Split(",")(1)
        SecuenciaKey = codigo.Split(",")(2)

        oAsientoContableBM.EliminarRevision(CodigoPortafolioSBSKey, NumeroAsientoKey, SecuenciaKey, UIUtility.ConvertirFechaaDecimal(txtFechaOperacion.Text), DatosRequest)

        LimpiarControlesDetalle()
        CargarDatosGrilla()
        AlertaJS("El registro ha sido eliminado correctamente.")
    End Sub

#End Region

    Private Sub LimpiarControlesDetalle()
        Me.ddlDebeHaber.SelectedIndex = 0
        Me.ddlMatriz.SelectedIndex = 0
        Me.ddlMatriz.Enabled = True
        Me.txtNroCuenta.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.txtGlosa.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.txtImporteSoles.Text = Constantes.M_STR_TEXTO_INICIAL
        txtImporteOrigen.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.ddlMoneda.SelectedValue = "NSOL"
        Me.ddlNumeroAsiento.Enabled = True
        Me.ddlNumeroAsiento.Items.Clear()
    End Sub

    Private Sub LimpiarControlesCabecera()

        Me.txtFechaOperacion.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.ddlFondo.SelectedIndex = 0

    End Sub

    Private Sub InsertarFilaAsientoContable()

        Dim oAsientoContableBE As AsientoContableBE
        Dim oRow As AsientoContableBE.AsientoContableRow

        oAsientoContableBE = Me.VistaAsientoContable

        oRow = oAsientoContableBE.AsientoContable.NewAsientoContableRow()
        oRow.CodigoMoneda = Me.ddlMoneda.SelectedValue
        oRow.DebeHaber = Me.ddlDebeHaber.SelectedValue
        oRow.CuentaContable = Me.txtNroCuenta.Text
        oRow.Glosa = Me.txtGlosa.Text
        oRow.Importe = Me.txtImporteSoles.Text.Replace(".", UIUtility.DecimalSeparator())
        oRow.ImporteOrigen = Me.txtImporteOrigen.Text.Replace(".", UIUtility.DecimalSeparator())
        oAsientoContableBE.AsientoContable.AddAsientoContableRow(oRow)
        oAsientoContableBE.AsientoContable.AcceptChanges()

        Me.VistaAsientoContable = oAsientoContableBE

    End Sub

    Private Sub DeshabilitarControlesCabecera()
        Me.txtFechaOperacion.Enabled = False
        Me.imgCalendar.Visible = False
        Me.ddlFondo.Enabled = False
    End Sub

    Private Sub HabilitarControlesCabecera()
        Me.txtFechaOperacion.Enabled = True
        Me.imgCalendar.Visible = True
        Me.ddlFondo.Enabled = True
    End Sub

    Private Sub DeshabilitarControlesDetalle()
        Me.txtGlosa.ReadOnly = True
        Me.ddlDebeHaber.Enabled = False
        Me.txtNroCuenta.ReadOnly = True
        txtImporteOrigen.ReadOnly = True
        txtImporteSoles.ReadOnly = True
    End Sub

    Private Sub HabilitarControlesDetalle()
        Me.txtGlosa.ReadOnly = False
        Me.ddlDebeHaber.Enabled = True
        Me.txtNroCuenta.ReadOnly = False
        Me.txtImporteOrigen.ReadOnly = False
        Me.txtImporteSoles.ReadOnly = False
    End Sub

    Private Sub CargarGrilla()
        Dim oAsientoContableBM As New AsientoContableBM
        Dim oAsientoContableBE As AsientoContableBE
        Dim decFechaAsiento As Decimal
        Dim filtroFecha As Decimal = 0
        Dim filtroFondo As String = ""
        Dim filtroMoneda As String = ""
        Dim filtroTipoLote As String = ""

        decFechaAsiento = UIUtility.ConvertirFechaaDecimal(Me.txtFechaOperacion.Text)

        filtroFondo = Me.ddlFondo.SelectedValue
        filtroFecha = decFechaAsiento
        filtroTipoLote = ddlTipoLote.SelectedValue

        filtroMoneda = ""


        oAsientoContableBE = oAsientoContableBM.SeleccionarPorFiltroRevision(filtroFecha, filtroFondo, filtroMoneda, filtroTipoLote, "", Me.DatosRequest)

        Me.VistaAsientoContable = oAsientoContableBE
        Me.dgLista.DataSource = Me.VistaAsientoContable
        Me.dgLista.DataBind()

    End Sub

    Private Sub EditarGrilla(ByVal fila As Integer)

        Dim oAsientoContableBE As New AsientoContableBE
        Dim aux As String
        oAsientoContableBE = Me.VistaAsientoContable

        CodigoPortafolioSBSKey = dgLista.Rows(fila).Cells(2).Text.Trim
        NumeroAsientoKey = dgLista.Rows(fila).Cells(3).Text.Trim
        SecuenciaKey = dgLista.Rows(fila).Cells(4).Text.Trim
        aux = oAsientoContableBE.AsientoContable.Rows(fila)("FechaAsiento").ToString()
        Me.txtFechaOperacion.Text = aux.Substring(6, 2) + "/" + aux.Substring(4, 2) + "/" + aux.Substring(0, 4)
        Me.ddlFondo.SelectedValue = oAsientoContableBE.AsientoContable.Rows(fila)("CodigoPortafolioSBS").ToString()
        Me.ddlMatriz.SelectedValue = oAsientoContableBE.AsientoContable.Rows(fila)("CodigoMatrizContable").ToString()
        CargarComboNumeroAsiento()
        Me.ddlNumeroAsiento.SelectedValue = oAsientoContableBE.AsientoContable.Rows(fila)("NumeroAsiento").ToString()

        Me.ddlMoneda.SelectedValue = oAsientoContableBE.AsientoContable.Rows(fila)("CodigoMoneda").ToString()
        Me.txtGlosa.Text = oAsientoContableBE.AsientoContable.Rows(fila)("Glosa").ToString()
        If (oAsientoContableBE.AsientoContable.Rows(fila)("DebeHaber").ToString() = "") Then
            Me.ddlDebeHaber.SelectedIndex = 0
        Else
            Me.ddlDebeHaber.SelectedValue = oAsientoContableBE.AsientoContable.Rows(fila)("DebeHaber").ToString()
        End If

        Me.txtNroCuenta.Text = oAsientoContableBE.AsientoContable.Rows(fila)("CuentaContable").ToString()
        Me.txtImporteSoles.Text = dgLista.Rows.Item(fila).Cells(11).Text
        Dim dtAuxLocal As DataTable = oAsientoContableBM.RetornarTipoCambio(ddlMoneda.SelectedValue, UIUtility.ConvertirFechaaDecimal(txtFechaOperacion.Text).ToString, DatosRequest)
        If dtAuxLocal.Rows.Count = 1 Then
            txtImporteOrigen.Text = Format(Convert.ToDecimal(Me.txtImporteSoles.Text.Replace(".", UIUtility.DecimalSeparator())) / Convert.ToDecimal(dtAuxLocal.Rows(0)(0)), "###,###,##0.00")
        End If
        oAsientoContableBE.AsientoContable.Rows(fila)("Importe").ToString().Replace(UIUtility.DecimalSeparator(), ".")

        HabilitarControlesDetalle()
        ddlFondo.Enabled = False
        ddlMatriz.Enabled = False
        ddlNumeroAsiento.Enabled = False
        ViewState("IndiceSeleccionado") = CodigoPortafolioSBSKey + "," + NumeroAsientoKey + "," + SecuenciaKey

    End Sub

    Private Sub EditarCeldaGrilla(ByVal oGridViewRow As GridViewRow)
        Dim valor As Decimal
        Dim valorcad As String

        If oGridViewRow.RowType = DataControlRowType.DataRow Then
            valor = oGridViewRow.Cells(11).Text
            valor = Math.Round(valor, 2)
            valorcad = valor.ToString
            oGridViewRow.Cells(11).Text = valorcad.Replace(UIUtility.DecimalSeparator(), ".")
        End If

    End Sub

    Private Sub CalcularSubTotales()
        Dim debe, haber, diferencia As Decimal
        debe = 0
        haber = 0
        diferencia = 0
        Dim i As Integer = 0
        While (i < VistaAsientoContable.AsientoContable.Rows.Count)
            If Not VistaAsientoContable.AsientoContable.Rows(i).Item("Importe") Is DBNull.Value Then
                If (VistaAsientoContable.AsientoContable.Rows(i).Item("DebeHaber").ToString = "D") Or (VistaAsientoContable.AsientoContable.Rows(i).Item("DebeHaber").ToString = "Debe") Then
                    debe = debe + Convert.ToDecimal(VistaAsientoContable.AsientoContable.Rows(i).Item("Importe"))
                End If
                If (VistaAsientoContable.AsientoContable.Rows(i).Item("DebeHaber").ToString = "H") Or (VistaAsientoContable.AsientoContable.Rows(i).Item("DebeHaber").ToString = "Haber") Then
                    haber = haber + Convert.ToDecimal(VistaAsientoContable.AsientoContable.Rows(i).Item("Importe"))
                End If
            End If
            i = i + 1
        End While

        Me.txtTobDeb.Text = debe.ToString().Replace(UIUtility.DecimalSeparator(), ".")
        Me.txtTotHab.Text = haber.ToString().Replace(UIUtility.DecimalSeparator(), ".")
        diferencia = debe - haber
        Me.txtTotDif.Text = diferencia.ToString().Replace(UIUtility.DecimalSeparator(), ".")
        ddlMoneda.SelectedValue = "NSOL"
    End Sub



    Private Sub Listar()
        Me.ibIngresar.Visible = True
        Me.ibAceptar.Visible = False
        Me.ibIngresar.Attributes.Add("onclick", "javascript:return ValidarDetalle();")
        Me.dgLista.PageIndex = 0
        CargarDatosGrilla()
        'CalcularSubTotales()
    End Sub

    Private Sub CargarDatosGrilla()
        CargarGrilla()
        CalcularSubTotales()
        If Me.dgLista.Rows.Count = 0 Then
            AlertaJS("No se encontraron Registros")
        Else
            Me.ibImprimir.Visible = True
            Me.divBusqueda.Visible = True
        End If
    End Sub

    Public Function ValidarCamposDetalleImporte() As Boolean
        Dim Resultado As Boolean = True
        Resultado = ValidarNumero(Me.txtImporteSoles.Text.Trim(), 2)
        If Resultado Then
            Resultado = ValidarNumero(Me.txtImporteOrigen.Text.Trim(), 2)
        End If

        Return Resultado

    End Function
    Public Function ValidarNumero(ByVal Cadena() As Char, ByVal Tipo As Byte) As Boolean
        Dim Estado As Boolean
        Dim CadBase As String = "0123456789"
        Dim posic, Largo, Ind As Integer
        Dim Eval As Char

        If Tipo = 2 Then
            CadBase = CadBase + "." + ","
        End If
        If Tipo = 3 Then
            CadBase = CadBase + "/"
        End If
        Estado = True
        Largo = Len(Cadena)
        Ind = 0
        Do While Ind < Largo
            Eval = Cadena(Ind)
            posic = InStr(CadBase, Eval)
            If posic = 0 Then
                Estado = False
                Ind = Largo
            End If
            Ind = Ind + 1
        Loop
        Return Estado
    End Function

#End Region

    Protected Sub imbNroCuenta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imbNroCuenta.Click
        MostrarCuentaContable(Me.txtNroCuenta.Text)
    End Sub

    Private Sub MostrarCuentaContable(ByVal NroCuenta As String)

        Dim script As New StringBuilder
        Dim portafolio As String
        If ddlFondo.SelectedIndex <= 0 Then
            portafolio = PORTAFOLIO_MULTIFONDOS
        Else
            portafolio = ddlFondo.SelectedValue
        End If

        With script
            .Append("showCuentaContable('" & NroCuenta & "','" & portafolio & "')")
        End With
        EjecutarJS(script.ToString())

    End Sub

    Private Sub ibImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibImprimir.Click
        EjecutarJS(UIUtility.MostrarPopUp("frmVisorAsientoContable.aspx?pFondo=" + Me.ddlFondo.SelectedValue + "&pDescripcionFondo=" + Me.ddlFondo.SelectedItem.Text.Trim + "&pFecha=" + txtFechaOperacion.Text.Trim(), "no", 800, 600, 10, 10, "no", "yes", "yes", "yes"), False)
    End Sub

    Private Sub ddlFondo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlFondo.SelectedIndexChanged
        If Me.ddlFondo.SelectedItem.Text <> "--Seleccione--" Then
            Me.txtFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaAperturaContable(Me.ddlFondo.SelectedValue))
        End If
    End Sub

    Private Sub ibIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibIngresar.Click
        Try
            Dim fechaApertura As String = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaAperturaContable(Me.ddlFondo.SelectedValue))
            If (fechaApertura <> Me.txtFechaOperacion.Text) Then
                AlertaJS("No se puede modificar Asientos contables generados en un periodo cerrado.")
                Return
            End If
            Insertar()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub ibCalcular_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibCalcular.Click
        If ddlMoneda.SelectedValue <> "--Seleccione--" Then
            Dim dtAuxLocal As DataTable = oAsientoContableBM.RetornarTipoCambio(ddlMoneda.SelectedValue, UIUtility.ConvertirFechaaDecimal(txtFechaOperacion.Text).ToString, DatosRequest)
            If dtAuxLocal.Rows.Count = 1 Then
                If txtImporteOrigen.Text <> "" Then
                    txtImporteSoles.Text = Format(Convert.ToDecimal(Me.txtImporteOrigen.Text.Replace(".", UIUtility.DecimalSeparator())) * Convert.ToDecimal(dtAuxLocal.Rows(0)(0)), "###,###,##0.00")
                    Me.txtImporteOrigen.Text = Format(Convert.ToDecimal(Me.txtImporteOrigen.Text.Replace(".", UIUtility.DecimalSeparator())), "###,###,##0.00")
                ElseIf txtImporteSoles.Text <> "" Then
                    txtImporteOrigen.Text = Format(Convert.ToDecimal(Me.txtImporteSoles.Text.Replace(".", UIUtility.DecimalSeparator())) / Convert.ToDecimal(dtAuxLocal.Rows(0)(0)), "###,###,##0.00")
                    Me.txtImporteSoles.Text = Format(Convert.ToDecimal(Me.txtImporteSoles.Text.Replace(".", UIUtility.DecimalSeparator())), "###,###,##0.00")
                End If
            End If
        Else
            If txtImporteOrigen.Text <> "" Then
                Me.txtImporteOrigen.Text = Format(Convert.ToDecimal(Me.txtImporteOrigen.Text.Replace(".", UIUtility.DecimalSeparator())), "###,###,##0.00")
                txtImporteSoles.Text = txtImporteOrigen.Text
            ElseIf txtImporteSoles.Text <> "" Then
                Me.txtImporteSoles.Text = Format(Convert.ToDecimal(Me.txtImporteSoles.Text.Replace(".", UIUtility.DecimalSeparator())), "###,###,##0.00")
                txtImporteOrigen.Text = txtImporteSoles.Text
            End If
        End If
    End Sub

End Class
