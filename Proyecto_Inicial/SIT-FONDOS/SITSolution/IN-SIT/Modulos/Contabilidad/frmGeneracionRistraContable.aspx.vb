Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports ParametrosSIT
Imports System.IO

Partial Class Modulos_Contabilidad_frmGeneracionRistraContable
    Inherits BasePage


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPortafolio()
                tbFechaOperacionDesde.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaAperturaContable(ddlPortafolio.SelectedValue))
                tbFechaOperacionHasta.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaAperturaContable(ddlPortafolio.SelectedValue))
                Dim fecha As String = IIf(tbFechaOperacionDesde.Text.Length = 0, Date.Today.ToShortDateString, tbFechaOperacionDesde.Text)

                BuscarRuta()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
        
    End Sub

    Private Sub ibGeneraRistra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGeneraRistra.Click
        'HelpRistra.GenerarRistraContable(Me)
    End Sub

    Private Sub CargarPortafolio()
        Dim oPortafolio As New PortafolioBM
        Dim dsPortafolio As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        ddlPortafolio.Items.Clear()
        ddlPortafolio.DataSource = dsPortafolio
        ddlPortafolio.DataValueField = "CodigoPortafolio"
        ddlPortafolio.DataTextField = "Descripcion"
        ddlPortafolio.DataBind()
    End Sub

    Private Sub BuscarRuta()
        Dim dtConsulta As DataTable
        dtConsulta = New ParametrosGeneralesBM().SeleccionarPorFiltro("RISTRACONT", "", "", "", DatosRequest)

        If dtConsulta.Rows.Count > 0 Then
            Me.tbRuta.Text = CType(dtConsulta.Rows(0).Item("Valor"), String)
        End If
    End Sub

    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        tbFechaOperacionDesde.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaAperturaContable(ddlPortafolio.SelectedValue))
        tbFechaOperacionHasta.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaAperturaContable(ddlPortafolio.SelectedValue))
        lblError.Text = ""
    End Sub

    Private Sub ibConsolidarVax_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConsolidarVAX.Click
        Try
            ConsolidarVAX()
        Catch
            AlertaJS("Ocurrió un error al consolidar")
        End Try
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub

    Public Sub ConsolidarVAX()
        Dim strRutaFinal As String = ConfigurationManager.AppSettings("ARCHIVOS_UNIFICADO")
        Dim strRutaVAX As String = ConfigurationManager.AppSettings("RUTA_VAX")
        Dim strRutaSIT As String = Server.MapPath(Request.ApplicationPath)
        Dim oAsientoContableBM As New AsientoContableBM
        Dim FechaArchivoRistra2 As String = ""
        Dim FechaArchivoRistra As String = ""
        Dim nombreArchivo2 As String = ""
        Dim nombreArchivo As String = ""
        Dim extension As String = "txt"
        Dim sFileName As String = ""
        Dim dsConsulta As DataSet
        Try

            If ddlPortafolio.SelectedValue.Equals("ADMINISTRA") Then
                FechaArchivoRistra = DateTime.Now.ToString("yyyyMMdd")
            Else
                FechaArchivoRistra = HelpRistra.RetornarFechaContableUtilAnterior(DateTime.Now)
            End If

            nombreArchivo = "pefp_ha_fix_f" + FechaArchivoRistra.Substring(2) + "_intapl"
            If ddlPortafolio.SelectedValue.Equals("ADMINISTRA") Then
                nombreArchivo = nombreArchivo + "1_teso_a9" + "." + extension
            Else
                nombreArchivo = nombreArchivo + "2_teso_f" + ddlPortafolio.SelectedValue + "." + extension
            End If

            'Ruta de archivo VAX
            strRutaVAX = strRutaVAX + nombreArchivo
            'Ruta final
            strRutaFinal = strRutaFinal + nombreArchivo
            'Ruta de archivo SIT
            strRutaSIT = strRutaSIT + "\" + nombreArchivo
            'Verificar existencia de archivo
            If File.Exists(strRutaVAX) = True Then
                'Copiar archivo a SIT
                File.Copy(strRutaVAX, strRutaSIT, True)
                'Seleccionar ristra contable
                dsConsulta = oAsientoContableBM.Seleccionar_RistraContable(ddlPortafolio.SelectedValue().ToString, UIUtility.ConvertirFechaaDecimal(tbFechaOperacionDesde.Text), UIUtility.ConvertirFechaaDecimal(tbFechaOperacionHasta.Text), FechaArchivoRistra, DatosRequest)
                If dsConsulta.Tables(0).Rows.Count > 0 Then
                    'Fecha archivo ristra contable
                    If ddlPortafolio.SelectedValue.Equals("ADMINISTRA") Then
                        FechaArchivoRistra2 = DateTime.Now.ToString("yyyyMMdd")
                    Else
                        FechaArchivoRistra2 = HelpRistra.RetornarFechaContableUtilAnterior(DateTime.Now)
                    End If
                    'Nombre del archivo ristra contable debe contener la fecha de operación
                    nombreArchivo2 = "pefp_ha_fix_f" + FechaArchivoRistra2.Substring(2) + "_intapl"
                    If ddlPortafolio.SelectedValue.Equals("ADMINISTRA") Then
                        nombreArchivo2 = nombreArchivo2 + "1_sit_a9" + "." + extension
                    Else
                        nombreArchivo2 = nombreArchivo2 + "2_sit_f" + ddlPortafolio.SelectedValue + "." + extension
                    End If
                    'sFileName = tbRuta.Text + nombreArchivo2
                    sFileName = ConfigurationManager.AppSettings("RUTA_VAX") + tbRuta.Text + nombreArchivo2
                    'Verificar existencia de archivo
                    If File.Exists(sFileName) = True Then
                        'Unificar archivos VAX con SIT
                        sbUnirArchivos(sFileName, strRutaSIT)
                        'Si existe archivo eliminar para mover nuevamente
                        If File.Exists(strRutaFinal) = True Then
                            File.Delete(strRutaFinal)
                        End If
                        'Mover a ruta especificada
                        'File.Move(strRutaSIT, strRutaFinal)
                        'Mostrar mensaje de unificación satisfactoria
                        AlertaJS("El archivo " + nombreArchivo + " se creo correctamente...!")
                    Else
                        sFileName = Replace(sFileName, "\", "\\")
                        AlertaJS("No se encontro el archivo en la siguiente ruta:" & sFileName)
                    End If
                Else
                    AlertaJS("No se encontro datos de Ristras contables de SIT...!")
                End If
            Else
                'strRutaVAX = Replace(strRutaVAX, "\", "\\")
                AlertaJS("No se encontro el archivo en la siguiente ruta:" & strRutaVAX)
            End If
        Catch ex As Exception
            lblError.Visible = True
            lblError.Text = "Error al consolidar VAX: " + ex.Message
        Finally
            oAsientoContableBM = Nothing
            dsConsulta = Nothing
            GC.Collect()
        End Try
    End Sub

    Public Sub sbUnirArchivos(ByVal strArchivoOrigen As String, ByVal strArchivoDestino As String)
        Dim strContext As String = ""
        Dim oSR As StreamReader
        Dim oSW As StreamWriter
        Try
            'Establecer el encoding a 1252 para soportar caracteres en Español
            oSR = New StreamReader(strArchivoOrigen, Encoding.UTF8)
            oSW = New StreamWriter(strArchivoDestino, True, Encoding.GetEncoding(1252))
            While Not strContext Is Nothing
                strContext = oSR.ReadLine
                oSW.WriteLine(strContext)
            End While
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        Finally
            oSW.Close()
            oSR.Close()
            oSR = Nothing
            oSW = Nothing
            GC.Collect()
        End Try
    End Sub
End Class
