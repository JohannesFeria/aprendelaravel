Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.IO
Imports System.Data

Partial Class Modulos_Contabilidad_Reportes_frmReversionRistraContable
    Inherits BasePage


#Region " /* Funciones Personalizadas*/"
    Private Sub CargarPortafolio()
        Dim oPortafolio As New PortafolioBM
        'Dim dsPortafolio As DataSet = oPortafolio.Listar(DatosRequest, ParametrosSIT.ESTADO_ACTIVO)
        Dim dsPortafolio As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        ddlPortafolio.Items.Clear()
        ddlPortafolio.DataSource = dsPortafolio
        ddlPortafolio.DataValueField = "CodigoPortafolio"
        ddlPortafolio.DataTextField = "Descripcion"
        ddlPortafolio.DataBind()
    End Sub
#End Region

#Region " /* Eventos de Página */ "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPortafolio()
                Session("Ruta") = HelpRistra.fnBuscarRuta(DatosRequest)
                tbFechaOperacionDesde.Text = DateTime.Now.ToString("dd/MM/yyyy")

                tbFechaContable.Text = DateTime.Now.ToString("dd/MM/yyyy")
                tbRuta.Enabled = False
                tbRuta.Text = Session("Ruta")
                Call tbFechaOperacionDesde_TextChanged(sender, e)
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        Try
            Call tbFechaOperacionDesde_TextChanged(sender, e)
            lblError.Text = ""
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub btnRevertirRistra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRevertirRistra.Click
        Dim Ruta As String = ""
        Try
            lblError.Text = ""
            If Trim(tbFechaOperacionDesde.Text) <> "" Then
                If IsDate(tbFechaOperacionDesde.Text) = True Then
                    If File.Exists(tbRuta.Text) = True Then
                        Dim result As string
                        Call tbFechaOperacionDesde_TextChanged(sender, e)
                        result = HelpRistra.RevertirRistraContable(ddlPortafolio.SelectedValue, tbFechaContable.Text, tbRuta.Text, DatosRequest)

                        If Not String.IsNullOrEmpty(result) Then
                            lblError.Visible = True
                            lblError.Text = result
                        End If
                    Else
                        Ruta = Replace(tbRuta.Text, "\", "\\")
                        AlertaJS("No existe el archivo seleccionado: " + Ruta)
                    End If
                Else
                    AlertaJS("Fecha invalida.")
                End If
            Else
                AlertaJS("Debe ingresar la fecha de operación.")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub tbFechaOperacionDesde_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFechaOperacionDesde.TextChanged
        Try
            lblError.Text = ""
            If Trim(tbFechaOperacionDesde.Text) <> "" Then
                Dim FechaArchivoRistra As String = Microsoft.VisualBasic.Right(tbFechaOperacionDesde.Text, 4) & Mid(tbFechaOperacionDesde.Text, 4, 2) & Microsoft.VisualBasic.Left(tbFechaOperacionDesde.Text, 2)
                Dim NombreArchivo As String = "pefp_ha_fix_f" + FechaArchivoRistra.Substring(2) + "_intapl"
                Dim extension As String = "txt"
                NombreArchivo = NombreArchivo + "2_sit_f" + ddlPortafolio.SelectedValue + "." + extension
                tbRuta.Text = Session("Ruta") + NombreArchivo
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("~/frmDefault.aspx")
    End Sub

#End Region
End Class
