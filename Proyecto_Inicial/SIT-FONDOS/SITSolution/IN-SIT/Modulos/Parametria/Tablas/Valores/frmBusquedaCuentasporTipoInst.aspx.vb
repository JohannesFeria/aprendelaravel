Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data
Imports ParametrosSIT

Partial Class Modulos_Parametria_Tablas_Valores_frmBusquedaCuentasporTipoInst
    Inherits BasePage

#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                CargarDatosGrilla()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            dgLista.PageIndex = 0
            CargarDatosGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmCuentasPorTipoInst.aspx", False)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al ingresar a la página")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al salir de la página")
        End Try
    End Sub
#End Region

#Region " /* Funciones Modificar */"

    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim secuencial As Int32 = Int32.Parse(e.CommandArgument)
            Response.Redirect("frmCuentasPorTipoInst.aspx?cod=" & secuencial)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al modificar el registro")
        End Try
    End Sub

#End Region

#Region " /* Funciones Eliminar */"
    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oCuentaPorTipoInstrumentoBM As New CuentaPorTipoInstrumentoBM
            Dim secuencial As Int32 = Int32.Parse(e.CommandArgument)
            oCuentaPorTipoInstrumentoBM.Eliminar(secuencial, DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al eliminar el registro")
        End Try
    End Sub

#End Region


#Region " /* Funciones Personalizadas*/"
    Private Sub CargarDatosGrilla()
        CargarGrilla()
        If dgLista.Rows.Count = 0 Then
            AlertaJS("No se encontraron Registros")
        End If
    End Sub
    Private Sub CargarGrilla()
        Dim oCuentaPorTipoInstrumentoBM As New CuentaPorTipoInstrumentoBM
        Dim strTipoInstrumentoSBS, strSituacion, strCodigoMoneda, strGrupoContable, strPortafolio As String
        strTipoInstrumentoSBS = IIf(ddlTipoInstrumento.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, ddlTipoInstrumento.SelectedValue).ToString()
        strSituacion = IIf(ddlSituacion.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, ddlSituacion.SelectedValue).ToString()
        strCodigoMoneda = IIf(ddlMoneda.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, ddlMoneda.SelectedValue).ToString()
        strGrupoContable = IIf(ddlGrupoContable.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), String.Empty, ddlGrupoContable.SelectedValue).ToString()
        strPortafolio = IIf(ddlPortafolio.SelectedValue.Equals(Constantes.M_STR_TEXTO_TODOS), ConfigurationManager.AppSettings("MULTIFONDO"), ddlPortafolio.SelectedValue)
        Dim dtblDatos As DataTable = oCuentaPorTipoInstrumentoBM.SeleccionarPorFiltro(strTipoInstrumentoSBS, strCodigoMoneda, strGrupoContable, strSituacion, strPortafolio, DatosRequest).Tables(0)
        dgLista.DataSource = dtblDatos
        dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dtblDatos) + "');")
    End Sub
    Public Sub CargarCombos()
        Dim tablaTipoInstrumento As New Data.DataTable
        Dim tablaMoneda As New Data.DataTable
        Dim oTipoInstrumentoBM As New TipoInstrumentoBM
        Dim oPortafolio As New PortafolioBM
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
        tablaTipoInstrumento = oTipoInstrumentoBM.Listar(DatosRequest).Tables(0)
        Dim oMonedaBM As New MonedaBM
        tablaMoneda = oMonedaBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBoxBusquedas(ddlTipoInstrumento, tablaTipoInstrumento, "CodigoTipoInstrumentoSBS", "CodigoMasDescripcion", True)
        HelpCombo.LlenarComboBoxBusquedas(ddlMoneda, tablaMoneda, "CodigoMoneda", "Descripcion", True)
        CargarGrupoContable()
        Dim CodigoPortafolio As String = "" 'Nuevo
        Dim tablaPortafolio As New DataTable 'Nuevo
        Dim oPortafolioBM As New PortafolioBM 'Nuevo
        'CodigoPortafolio = New ParametrosGeneralesBM().SeleccionarPorFiltro(GRUPO_FONDO, MULTIFONDO, "", "", Nothing).Rows(0)("Valor").ToString.Trim 'Nuevo
        tablaPortafolio = oPortafolioBM.PortafolioCodigoListar(CodigoPortafolio) 'Nuevo 
        ddlPortafolio.DataSource = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        ddlPortafolio.DataValueField = "CodigoPortafolio"
        ddlPortafolio.DataTextField = "Descripcion"
        ddlPortafolio.DataBind()
    End Sub
    Private Sub CargarGrupoContable()
        'If Not ddlTipoInstrumento.SelectedValue.Equals("") Then
        Dim DtGrupoContable As DataTable
        DtGrupoContable = New ParametrosGeneralesBM().ListarGrupoContable(ddlTipoInstrumento.SelectedValue, DatosRequest)
        HelpCombo.LlenarComboBox(ddlGrupoContable, DtGrupoContable, "Valor", "Nombre", True)
        If ddlGrupoContable.Items.Count > 0 Then
            ddlGrupoContable.SelectedIndex = ddlGrupoContable.Items.IndexOf(ddlGrupoContable.Items.FindByValue("PM"))
        Else
            ddlGrupoContable.SelectedIndex = 0
        End If
    End Sub
#End Region
    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarDatosGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub
    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
        End Try
    End Sub

    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand

    End Sub
End Class
