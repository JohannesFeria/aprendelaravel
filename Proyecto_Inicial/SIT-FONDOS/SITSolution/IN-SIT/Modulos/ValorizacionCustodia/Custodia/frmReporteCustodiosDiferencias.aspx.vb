Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports UIUtility
Imports ParametrosSIT
Imports CrystalDecisions.CrystalReports.Engine
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports System.IO

Partial Class Modulos_ValorizacionCustodia_Custodia_frmReporteCustodiosDiferencias
    Inherits BasePage

    Dim sPortafolioCodigo As String = String.Empty
    Dim sPortafolioDescripcion As String = String.Empty
    Dim sCodigoCustodio As String = String.Empty
    Dim oCustodioArchivoBM As New CustodioArchivoBM
    Dim oUtil As New UtilDM


#Region " /* Funciones Personalizadas*/"

    Private Function VerificaExistenDiferencias() As Boolean
        Dim sFechaOperacion As String = tbFechaOperacion.Text
        sPortafolioCodigo = ddlPortafolio.SelectedValue.ToString
        sCodigoCustodio = dlCustodio.SelectedValue.ToString
        Dim oInfCusDS As New DataTable
        Dim objInformacionCustodio As New CustodioArchivoBM
        oInfCusDS = objInformacionCustodio.InstrumentosDiferencias(sFechaOperacion.Substring(6, 4) & sFechaOperacion.Substring(3, 2) & sFechaOperacion.Substring(0, 2), sPortafolioCodigo, sCodigoCustodio, DatosRequest).Tables(0)

        If oInfCusDS.Rows.Count > 0 Then
            Return True
            Exit Function
        End If

        objInformacionCustodio = Nothing
        oInfCusDS = Nothing
        Return False
    End Function

    Private Sub CargaPortafolio(ByVal drlista As DropDownList)
        Dim objportafolio As New PortafolioBM
        drlista.DataSource = objportafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        drlista.DataTextField = "Descripcion"
        drlista.DataValueField = "CodigoPortafolio"
        drlista.DataBind()
        UIUtility.InsertarElementoSeleccion(drlista)
        objportafolio = Nothing
    End Sub

    Private Sub CargaCustodios()
        Dim objCustodio As New CustodioBM
        dlCustodio.DataTextField = "Descripcion"
        dlCustodio.DataValueField = "CodigoCustodio"
        dlCustodio.DataSource = objCustodio.Listar(DatosRequest)
        dlCustodio.DataBind()
        UIUtility.InsertarElementoSeleccion(dlCustodio, "", "--VARIOS--")
        objCustodio = Nothing
    End Sub

    Private Function Validar() As Boolean
        Dim sFechaOperacion As String = tbFechaOperacion.Text.Trim

        If sFechaOperacion.Trim = "" Then
            AlertaJS(ObtenerMensaje("ALERT49"))
            Return False
            Exit Function
        ElseIf sFechaOperacion.Length < 10 Then
            AlertaJS(ObtenerMensaje("ALERT50"))
            Return False
            Exit Function
        End If

        If sFechaOperacion.Trim <> "" Then
            If Not IsDate(sFechaOperacion) Then
                AlertaJS(ObtenerMensaje("ALERT51"))
                Return False
                Exit Function
            End If
        End If

        Return True
    End Function
#End Region


    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            dtDestino.LoadDataRow(dr.ItemArray, False)
        Next
    End Sub

    Function CargarRuta() As String
        Dim oArchivoPlanoBM As New ArchivoPlanoBM
        Dim oArchivoPlanoBE As New DataSet
        oArchivoPlanoBE = oArchivoPlanoBM.Seleccionar("012", MyBase.DatosRequest())
        Return (oArchivoPlanoBE.Tables(0).Rows(0).Item(4))
    End Function

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                tbFechaOperacion.Text = oUtil.RetornarFechaSistema
                Call CargaPortafolio(ddlPortafolio)
                Call CargaCustodios()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub
    
    Private Sub btnGenerarReporte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerarReporte.Click
        Try
          
            Dim sFechaOperacion As String = tbFechaOperacion.Text
            sPortafolioCodigo = ddlPortafolio.SelectedValue.ToString
            sPortafolioDescripcion = ddlPortafolio.SelectedItem.Text.Trim
            sCodigoCustodio = dlCustodio.SelectedValue.ToString

            EjecutarJS(UIUtility.MostrarPopUp("Reportes/frmDiferenciaInstrumentosCDet.aspx?nFechaOperacion=" & sFechaOperacion.Substring(6, 4) & sFechaOperacion.Substring(3, 2) & sFechaOperacion.Substring(0, 2) & "&sPortafolioCodigo=" & sPortafolioCodigo & "&sPortafolioDescripcion=" & sPortafolioDescripcion & "&sCodigoCustodio=" & sCodigoCustodio & "&sNombreCustodio=" & dlCustodio.SelectedItem.Text, "no", 1010, 670, 0, 0, "no", "yes", "yes", "yes"), False)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Generar el Reporte")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al salir de la página")
        End Try
    End Sub

    Private Function VerificaCustodioInformacion() As Boolean
        Dim sFechaCorte As String

        sFechaCorte = tbFechaOperacion.Text
        sFechaCorte = sFechaCorte.Substring(6, 4) & sFechaCorte.Substring(3, 2) & sFechaCorte.Substring(0, 2)
        Dim oCustodioInformacion As DataSet = oCustodioArchivoBM.VerificaCustodioInformacion(CLng(sFechaCorte), DatosRequest)

        If oCustodioInformacion.Tables(0).Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Function ActualizaSaldosInfCustodio(ByVal sCodigoMnemonico As String, ByVal sCodigoISIN As String, ByVal sCodigoPortafolioSBS As String, ByVal sCodigoCustodio As String) As Boolean
        Dim sFechaOperacion As String = tbFechaOperacion.Text
        sFechaOperacion = sFechaOperacion.Substring(6, 4) & sFechaOperacion.Substring(3, 2) & sFechaOperacion.Substring(0, 2)
        Dim oInfCusDS As New DataTable
        Dim objInformacionCustodio As New CustodioArchivoBM

        Return objInformacionCustodio.ActualizaSaldosInfCustodio(sFechaOperacion, sCodigoMnemonico, sCodigoISIN, sCodigoPortafolioSBS, sCodigoCustodio, DatosRequest)
    End Function

    Private Sub tbFechaOperacion_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            If VerificaExistenDiferencias() Then
                _Validacion.Value = "1"
            Else
                _Validacion.Value = "0"
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Fecha")
        End Try
    End Sub

    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        Try
            If VerificaExistenDiferencias() Then
                _Validacion.Value = "1"
            Else
                _Validacion.Value = "0"
            End If
            EstablecerFecha()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al seleccionar el Portafolio")
        End Try
    End Sub

    Private Sub dlCustodio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dlCustodio.SelectedIndexChanged
        Try
            If VerificaExistenDiferencias() Then
                _Validacion.Value = "1"
            Else
                _Validacion.Value = "0"
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al seleccionar el Custodio")
        End Try
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        oCustodioArchivoBM = Nothing
    End Sub

    Private Sub EstablecerFecha()
        If ddlPortafolio.SelectedIndex > 0 Then
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(ddlPortafolio.SelectedValue))
        Else
            tbFechaOperacion.Text = Now.ToString("dd/MM/yyyy")
        End If
    End Sub

End Class
