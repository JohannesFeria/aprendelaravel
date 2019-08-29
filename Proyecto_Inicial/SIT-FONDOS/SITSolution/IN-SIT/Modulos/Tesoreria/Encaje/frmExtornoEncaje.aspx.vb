Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Tesoreria_Encaje_frmExtornoEncaje
    Inherits BasePage

#Region " /* Métodos de la Página*/ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPortafolio()
                Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(Me.ddlPortafolio.SelectedValue))
                Me.tbFechaUltimoEncaje.Text = UIUtility.ConvertirFechaaString(New EncajeBM().ObtenerFechaUltimoEncaje(Me.ddlPortafolio.SelectedValue, 0, DatosRequest))
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try        
    End Sub

    Private Sub btnExtornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExtornar.Click
        Try
            If Me.tbFechaOperacion.Text.Trim <> "" Then
                If Me.ddlPortafolio.SelectedValue <> "--SELECCIONE--" Then
                    Try
                        Dim oencaje As New EncajeBM
                        If (oencaje.ExisteEncajePeru(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaUltimoEncaje.Text), DatosRequest) = 0) Then
                            AlertaJS(ObtenerMensaje("ALERT138"))
                        Else
                            oencaje.ExtornoEncajePeru(ddlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaUltimoEncaje.Text), DatosRequest)
                            AlertaJS(ObtenerMensaje("ALERT139"))
                        End If
                    Catch ex As Exception
                        AlertaJS(ex.ToString)
                    End Try
                Else
                    AlertaJS(ObtenerMensaje("ALERT152"))
                End If
            Else
                AlertaJS(ObtenerMensaje("ALERT153"))
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error Extornar")
        End Try        
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try        
    End Sub

    Private Sub ddlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPortafolio.SelectedIndexChanged
        Try
            Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(Me.ddlPortafolio.SelectedValue))
            Me.tbFechaUltimoEncaje.Text = UIUtility.ConvertirFechaaString(New EncajeBM().ObtenerFechaUltimoEncaje(Me.ddlPortafolio.SelectedValue, 0, DatosRequest))
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar")
        End Try        
    End Sub

#End Region

#Region " /* Métodos Personalizados*/ "

    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oPortafolio As New PortafolioBM
            Dim dsPortafolio As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            ddlPortafolio.Items.Clear()
            ddlPortafolio.DataSource = dsPortafolio
            ddlPortafolio.DataValueField = "CodigoPortafolio"
            ddlPortafolio.DataTextField = "Descripcion"
            ddlPortafolio.DataBind()
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled

    End Sub

#End Region

End Class
