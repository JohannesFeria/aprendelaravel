Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports System.Data
Imports UIUtility
Partial Class Modulos_Contabilidad_frmMatrizContableFondoListar
    Inherits BasePage
    Dim oCabeceraMatrizContableBM As New CabeceraMatrizContableBM
    Dim oCabeceraMatrizContableBE As New CabeceraMatrizContableBE
    Private dtTblCabecera As New DataTable
    Dim oMatrizContableBM As New MatrizContableBM
    Dim oNegocioBM As New NegocioBM
    Dim oPortafolio As New PortafolioBM
    Private Sub CargarNegocio()
        HelpCombo.LlenarComboBox(ddlNegocio, oNegocioBM.Listar(DatosRequest).Tables(0), "CodigoNegocio", "Descripcion", True)
    End Sub
    Private Sub CargarSeries()
        HelpCombo.LlenarComboBox(ddlSerie, oPortafolio.ListarSeries(""), "CodigoSerie", "NombreSerie", True)
    End Sub
    Sub CargaMatriz()
        HelpCombo.LlenarComboBox(ddlMatriz, oMatrizContableBM.SeleccionarPorFiltros("", "A", "1").Tables(0), "CodigoMatrizContable", "Descripcion", True)
    End Sub
    Private Sub InicializarCabecera()
        Dim dt As DataTable
        dt = New DataTable
        dt.Columns.Add(New DataColumn("MatrizContable", GetType(String)))
        dt.Columns.Add(New DataColumn("CodigoPortafolio", GetType(String)))
        dt.Columns.Add(New DataColumn("Fondo", GetType(String)))
        dt.Columns.Add(New DataColumn("Moneda", GetType(String)))
        dt.Columns.Add(New DataColumn("Operacion", GetType(String)))
        dt.Columns.Add(New DataColumn("ClaseInstrumento", GetType(String)))
        dt.Columns.Add(New DataColumn("ModalidadPago", GetType(String)))
        dt.Columns.Add(New DataColumn("TipoInstrumento", GetType(String)))
        dt.Columns.Add(New DataColumn("SectorEmpresarial", GetType(String)))
        dt.Columns.Add(New DataColumn("NumeroCuentaIngreso", GetType(String)))
        dt.GetChanges()
        Me.dgCabecera.DataSource = dt
        Me.dgCabecera.DataBind()
    End Sub
    Private Function CrearFiltroBusquedaM() As CabeceraMatrizContableBE.CabeceraMatrizContableRow
        Dim oRow As CabeceraMatrizContableBE.CabeceraMatrizContableRow
        oRow = oCabeceraMatrizContableBE.CabeceraMatrizContable.NewCabeceraMatrizContableRow()
        oRow.CodigoMoneda = ""
        oRow.CodigoClaseInstrumento = String.Empty
        oRow.CodigoOperacion = ""
        oRow.CodigoTipoInstrumento = ""
        oRow.CodigoModalidadPago = String.Empty
        oRow.CodigoSectorEmpresarial = String.Empty
        oRow.CodigoPortafolioSBS = ""
        oRow.CodigoMatrizContable = ddlMatriz.SelectedValue
        oRow.NumeroCuentaIngreso = String.Empty
        oRow.CodigoSBSBanco = String.Empty
        oRow.Situacion = ddlSituacion.SelectedValue
        oRow.CodigoCabeceraMatriz = If(tbCodigoCabeceraM.Text.Trim().Equals(String.Empty), "0", tbCodigoCabeceraM.Text.Trim())
        oRow.CodigoSerie = ddlSerie.SelectedValue
        oRow.CodigoNegocio = ddlnegocio.SelectedValue
        Return oRow
    End Function
    Private Sub CargarGrilla()
        Dim oRow As CabeceraMatrizContableBE.CabeceraMatrizContableRow
        oRow = oCabeceraMatrizContableBE.CabeceraMatrizContable.NewCabeceraMatrizContableRow()
        oRow = CrearFiltroBusquedaM()
        oCabeceraMatrizContableBE.CabeceraMatrizContable.AddCabeceraMatrizContableRow(oRow)
        oCabeceraMatrizContableBE.CabeceraMatrizContable.AcceptChanges()
        InicializarCabecera()
        dtTblCabecera = oCabeceraMatrizContableBM.SeleccionarPorFiltro(oCabeceraMatrizContableBE, "1").Tables(0).Copy
        ViewState("GrillaCabeceraMatriz") = dtTblCabecera
        dgCabecera.DataSource = dtTblCabecera
        dgCabecera.DataBind()
        dgCabecera.SelectedIndex = -1
        ViewState("CabeceraSeleccionada") = Nothing
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + MostrarResultadoBusqueda(dtTblCabecera.Rows.Count) + "');")
    End Sub
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Try
                CargarNegocio()
                CargarSeries()
                CargaMatriz()
                CargarGrilla()
            Catch ex As Exception
                AlertaJS(Replace(ex.Message, "'", ""))
            End Try
        End If
    End Sub
    Protected Sub ibtnBuscar_Click(sender As Object, e As System.EventArgs) Handles ibtnBuscar.Click
        CargarGrilla()
    End Sub
    Protected Sub dgCabecera_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCabecera.RowCommand
        Try
            Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
            Dim estado As String = ""
            If e.CommandName = "Modificar" Then
                Response.Redirect("frmMatrizContableFondo.aspx?cod=" & e.CommandArgument.ToString())
            Else
                oCabeceraMatrizContableBM.Eliminar(e.CommandArgument, DatosRequest)
                CargarGrilla()
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub ibtnIngresar_Click(sender As Object, e As System.EventArgs) Handles ibtnIngresar.Click
        Response.Redirect("frmMatrizContableFondo.aspx")
    End Sub
End Class