Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Inversiones_frmPopupBusquedaOI
    Inherits BasePage

#Region "Variables"
    Dim oPortafolio As New PortafolioBM
    Dim oOrdenPreOrdenInversion As New OrdenPreOrdenInversionBM
    Dim oTipoRenta As New TipoRentaBM
    Dim objutil As New UtilDM
#End Region

#Region "Eventos de la Pagina"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            CargarPagina()
        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        dgLista.PageIndex = 0
        CargarGrilla()
    End Sub

#End Region

#Region "Metodos Personalizados"
    Private Sub CargarPagina()
        CargarFechaNegocio()
        CargarPortafolio()
        CargarTipoRenta()
        tbFechaOperacion.Enabled = False
    End Sub

    Private Sub CargarFechaNegocio()
        tbFechaOperacion.Text = objutil.RetornarFechaNegocio
    End Sub

    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oPortafolio As New PortafolioBM
            Dim dsPortafolio As DataSet = oPortafolio.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
            ddlPortafolio.Items.Clear()
            ddlPortafolio.DataSource = dsPortafolio
            ddlPortafolio.DataValueField = "CodigoPortafolioSBS"
            ddlPortafolio.DataTextField = "Descripcion"
            ddlPortafolio.DataBind()
            ddlPortafolio.Items.Insert(0, New ListItem("--TODOS--", ""))
        Else
            ddlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(ddlPortafolio)
        End If
        ddlPortafolio.Enabled = enabled
    End Sub

    Private Sub CargarTipoRenta()
        Dim DtTablaTipoRenta As New DataTable
        DtTablaTipoRenta = oTipoRenta.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlTipoRenta, DtTablaTipoRenta, "CodigoRenta", "Descripcion", True, "TODOS")
        ddlTipoRenta.SelectedIndex = -1
    End Sub

    Private Sub CargarGrilla()
        Dim fechaOperacion As Decimal
        Dim tipoRenta As String
        Dim portafolio As String
        Dim codigoOrden As String
        Dim oDt As New DataTable
        fechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        tipoRenta = ddlTipoRenta.SelectedValue
        portafolio = ddlPortafolio.SelectedValue
        codigoOrden = tbCodigo.Text
        oDt = oOrdenPreOrdenInversion.ConsultaOrdenesPreOrdenes(fechaOperacion, fechaOperacion, _
                                                        String.Empty, String.Empty, String.Empty, _
                                                        String.Empty, String.Empty, tipoRenta, _
                                                        portafolio, codigoOrden, DatosRequest).Tables(0)
        dgLista.DataSource = oDt
        dgLista.DataBind()
    End Sub

    Private Sub ActualizarIndice(ByVal nuevoIndice As Integer)
        dgLista.PageIndex = nuevoIndice
        CargarGrilla()
    End Sub
#End Region

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        ActualizarIndice(e.NewPageIndex)
    End Sub

    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Dim codigo As string
        If e.CommandName = "Seleccionar" Then
            codigo = dgLista.Rows(e.CommandArgument).Cells(1).Text.ToString()

            If Not Session("SS_DatosModal") Is Nothing Then
                Session.Remove("SS_DatosModal")
            End If

            Dim arraySesiones As String() = New String(1) {}
            arraySesiones(0) = codigo

            Session("SS_DatosModal") = arraySesiones
            EjecutarJS("window.close();")
        End If
    End Sub
End Class
