Imports SIT.BusinessEntities
Imports SIT.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmMedioTransmision
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load        
        Try
            If Not Page.IsPostBack Then
                CargarTipoRenta()
                CargarPagina()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            If Not Request.QueryString("cod") Is Nothing Then
                Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
                oParametrosGeneralesBM.ActualizarMedioTransmision(txtCodigo.Text.ToUpper(), txtDescripcion.Text.ToUpper(), ddlTipoRenta.SelectedValue, DatosRequest)
                AlertaJS("Los cambios se han registrado satisfactoriamente!")
            Else
                Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
                oParametrosGeneralesBM.InsertarMedioTransmision(txtCodigo.Text.ToUpper(), txtDescripcion.Text.ToUpper(), ddlTipoRenta.SelectedValue, DatosRequest)
                AlertaJS("Los cambios se han registrado satisfactoriamente!")
            End If
            LimpiarControles()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaMedioTransmision.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al retornar la página")
        End Try
    End Sub

    Private Sub CargarPagina()
        If Not Request.QueryString("cod") Is Nothing Then
            Dim dt As DataTable
            dt = New ParametrosGeneralesBM().SeleccionarMedioTransmision("", Request.QueryString("cod"), "", DatosRequest).Tables(0)
            txtCodigo.Text = CType(dt.Rows(0)("Valor"), String)
            txtDescripcion.Text = CType(dt.Rows(0)("Nombre"), String)
            ddlTipoRenta.SelectedValue = CType(dt.Rows(0)("Valor2"), String)
            txtCodigo.Enabled = False
        End If
    End Sub

    Public Sub CargarTipoRenta()
        Dim tablaTipoRenta As New Data.DataTable
        Dim oTipoRentaBM As New TipoRentaBM

        tablaTipoRenta = oTipoRentaBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlTipoRenta, tablaTipoRenta, "CodigoRenta", "Descripcion", False)
        Me.ddlTipoRenta.SelectedValue = ParametrosSIT.TR_RENTA_VARIABLE
    End Sub

    Private Sub LimpiarControles()
        txtCodigo.Text = ""
        txtDescripcion.Text = ""
        ddlTipoRenta.SelectedIndex = 0
    End Sub

End Class
