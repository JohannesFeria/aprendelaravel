Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_AdministracionValores_frmBusquedaValores
    Inherits BasePage

#Region " /* Eventos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarLoading("btnBuscar")
                CargarCombos()
                CargarFiltrosExistentes()
                CargarGrilla()
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                tbEmisor.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try        
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            'System.Threading.Thread.Sleep(4000)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Búsqueda")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Session("accionValor") = "INGRESAR"
            Response.Redirect("frmAdministracionValores.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar")
        End Try        
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try        
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            dgLista.DataSource = CType(Session("TablaBusquedaValores"), DataTable)
            dgLista.DataBind()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try
    End Sub

#End Region

#Region " /* Funciones Modificar */"
    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Session("accionValor") = "MODIFICAR"
            Dim StrCodigoMnemonico As String = e.CommandArgument
            Response.Redirect("frmAdministracionValores.aspx?cod=" & Server.UrlEncode(StrCodigoMnemonico) & "&nemonico=" & Server.UrlEncode(tbMnemonico.Text) & "&tipoRenta=" & ddlTipoRenta.SelectedValue)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Modificar Registro")
        End Try        
    End Sub

#End Region

#Region " /* Funciones Eliminar */"
    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oValoresBM As New ValoresBM
            Dim oCuponeraBM As New CuponeraBM
            Dim oInsCom As New InstrumentosCompuestosBM
            Dim oInsEst As New InstrumentosEstructuradosBM
            Dim StrCodigoMnemonico As String = e.CommandArgument
            oValoresBM.Eliminar(StrCodigoMnemonico, DatosRequest)
            oCuponeraBM.EliminarCuponeraEspecial(StrCodigoMnemonico, DatosRequest)
            oCuponeraBM.EliminarCuponeraNormal(StrCodigoMnemonico, DatosRequest)
            oInsCom.Eliminar(StrCodigoMnemonico, DatosRequest)
            oInsEst.Eliminar(StrCodigoMnemonico, DatosRequest)
            oValoresBM.EliminarDetalleCustodios(StrCodigoMnemonico, DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Eliminar Registro")
        End Try
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"
    Private Sub CargarGrilla()

        Dim oValoresBM As New ValoresBM
        Dim StrCodigoSBS As String
        Dim StrCodigoIsin As String
        Dim StrCodigoMnemonico As String
        Dim StrTipoRenta As String
        Dim StrMoneda As String
        Dim StrEmisor As String

        StrCodigoSBS = tbCodigoSBS.Text.ToString
        StrCodigoIsin = tbCodigoIsin.Text.ToString
        StrCodigoMnemonico = tbMnemonico.Text.ToString
        StrTipoRenta = ddlTipoRenta.SelectedValue.ToString
        StrMoneda = ddlMoneda.SelectedValue
        StrEmisor = tbEmisor.Text
        If StrMoneda = "Todos" Then
            StrMoneda = ""
        End If
        If StrTipoRenta = "Todos" Then
            StrTipoRenta = ""
        End If
        Session("TablaBusquedaValores") = oValoresBM.SeleccionarPorFiltro2(StrCodigoSBS, StrCodigoIsin, StrCodigoMnemonico, StrMoneda, StrTipoRenta, StrEmisor, DatosRequest).Tables(0)

        dgLista.PageIndex = 0
        dgLista.DataSource = CType(Session("TablaBusquedaValores"), DataTable)
        dgLista.DataBind()
        Session("DetalleAgrupacionE") = Nothing
        Session("DetalleAgrupacionC") = Nothing
        Session("Custodios") = Nothing
        Session("cuponeraNormal") = Nothing
        Session("cuponeraEspecial") = Nothing
        Session("TablaDetalle") = Nothing
        Session("agrupacionIE") = Nothing
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(CType(Session("TablaBusquedaValores"), DataTable).Rows.Count) + "')")


        If dgLista.Rows.Count = 0 Then
            AlertaJS("No se encontraron registros")
        End If
    End Sub

    Public Sub CargarCombos()
        Dim DtTablaMoneda As DataTable
        Dim DtTablaTipoRenta As DataTable
        Dim oTipoRentaBM As New TipoRentaBM
        Dim oMonedaBM As New MonedaBM
        DtTablaTipoRenta = oTipoRentaBM.Listar(DatosRequest).Tables(0)

        'INICIO | ZOLUXIONES | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se filtra en dataTable Tipo Renta por RENTA FIJA y VARIABLE para mostrarse en Combo SOLO cuando es un registro nuevo (Botón Ingresar) | 16/05/18
        DtTablaTipoRenta = DtTablaTipoRenta.Select("Descripcion in ('RENTA FIJA','RENTA VARIABLE')").CopyToDataTable()
        'FIN | ZOLUXIONES | ZOLUXIONES | RCE | ProyFondosII - RF001 - Se filtra en dataTable Tipo Renta por RENTA FIJA y VARIABLE para mostrarse en Combo SOLO cuando es un registro nuevo (Botón Ingresar) | 16/05/18

        DtTablaMoneda = oMonedaBM.Listar(DatosRequest).Tables(0)


        HelpCombo.LlenarComboBoxBusquedas(ddlTipoRenta, DtTablaTipoRenta, "CodigoRenta", "Descripcion", True)

        HelpCombo.LlenarComboBoxBusquedas(ddlMoneda, DtTablaMoneda, "CodigoMoneda", "CodigoMoneda", True)

    End Sub

    Public Sub CargarFiltrosExistentes()
        Dim strCodigoNemonico, strTipoRenta, strRetorno As String

        If Not Request.QueryString("tipoRenta") Is Nothing Then
            strCodigoNemonico = Server.UrlDecode(Request.QueryString("nemonico")) 'RGF 20090325
            strTipoRenta = Request.QueryString("tipoRenta")
            strRetorno = Request.QueryString("retorno")

            tbMnemonico.Text = strCodigoNemonico

            If Not ddlTipoRenta.Items.FindByValue(strTipoRenta) Is Nothing Then
                ddlTipoRenta.Items.FindByValue(strTipoRenta).Selected = True
            End If

            CargarGrilla()

            If (strRetorno = "yes") Then
                AlertaJS("Los Datos fueron grabados correctamente")
            End If

        End If
    End Sub
#End Region

End Class
