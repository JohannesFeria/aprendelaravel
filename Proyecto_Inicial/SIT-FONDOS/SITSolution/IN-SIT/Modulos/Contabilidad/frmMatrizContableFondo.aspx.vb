Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Contabilidad_frmMatrizContableFondo
    Inherits BasePage
    Dim oPortafolio As New PortafolioBM
    Dim oMatrizContableBM As New MatrizContableBM
    Dim oCabeceraMatrizContableBM As New CabeceraMatrizContableBM
    Dim oDetalleMatrizContableBM As New DetalleMatrizContableBM
    Dim oCabeceraMatrizContableBE As New CabeceraMatrizContableBE
    Dim oDetalleMatrizContableBE As New DetalleMatrizContableBE
    Dim oNegocioBM As New NegocioBM
    Private Sub CargarRegistro(ByVal codigoCabecera As String)
        Dim oRow As CabeceraMatrizContableBE.CabeceraMatrizContableRow
        oCabeceraMatrizContableBE = oCabeceraMatrizContableBM.Seleccionar(codigoCabecera, DatosRequest)
        oRow = DirectCast(oCabeceraMatrizContableBE.CabeceraMatrizContable.Rows(0), CabeceraMatrizContableBE.CabeceraMatrizContableRow)
        ddlNegocio.SelectedValue = oRow.CodigoNegocio
        ddlMatriz.SelectedValue = oRow.CodigoMatrizContable
        ddlSerie.SelectedValue = oRow.CodigoSerie
        txtCodigo.Text = oRow.CodigoCabeceraMatriz
    End Sub
    Private Sub CargarGrilla(ByVal sCodigoCabeceraContable As String)
        ViewState("Detalle_MatrizContable") = oDetalleMatrizContableBM.Seleccionar(sCodigoCabeceraContable, DatosRequest).DetalleMatrizContable
        dgLista.DataSource = DirectCast(ViewState("Detalle_MatrizContable"), DataTable)
        dgLista.DataBind()
    End Sub
    Sub CargarPagina()
        If Not Request.QueryString("cod") Is Nothing Then
            Try
                CargarRegistro(Request.QueryString("cod"))
                CargarGrilla(Request.QueryString("cod"))
            Catch ex As Exception
                AlertaJS(Replace(ex.Message, "'", ""))
            End Try
        End If
    End Sub
    Private Function CrearObjetoCabecera() As CabeceraMatrizContableBE
        Dim oRow As CabeceraMatrizContableBE.CabeceraMatrizContableRow
        oRow = DirectCast(oCabeceraMatrizContableBE.CabeceraMatrizContable.NewRow(), CabeceraMatrizContableBE.CabeceraMatrizContableRow)
        oRow.CodigoPortafolioSBS = ""
        oRow.CodigoOperacion = ""
        oRow.CodigoClaseInstrumento = String.Empty
        oRow.CodigoModalidadPago = String.Empty
        oRow.CodigoTipoInstrumento = ""
        oRow.CodigoSectorEmpresarial = String.Empty
        oRow.CodigoMoneda = ""
        oRow.CodigoMatrizContable = ddlMatriz.SelectedValue()
        oRow.Situacion = "A"
        oRow.NumeroCuentaIngreso = String.Empty
        oRow.CodigoSBSBanco = String.Empty
        oRow.CodigoSerie = ddlSerie.SelectedValue
        oRow.CodigoNegocio = ddlNegocio.SelectedValue
        If Not Request.QueryString("cod") Is Nothing Then
            oRow.CodigoCabeceraMatriz = Request.QueryString("cod")
        Else
            oRow.CodigoCabeceraMatriz = "0"
        End If
        oCabeceraMatrizContableBE.CabeceraMatrizContable.AddCabeceraMatrizContableRow(oRow)
        oCabeceraMatrizContableBE.CabeceraMatrizContable.AcceptChanges()
        Return oCabeceraMatrizContableBE
    End Function
    Private Function CrearObjetoDetalle(ByVal sCodigoCabeceraMatriz As String) As DetalleMatrizContableBE
        Dim oDetalleMatrizContableBE As New DetalleMatrizContableBE
        If Not ViewState("Detalle_MatrizContable") Is Nothing Then
            Dim oDetalleMatrizContableRow As DetalleMatrizContableBE.DetalleMatrizContableRow
            Dim oRow As DataRow
            Dim dt As New DataTable
            dt = CType(ViewState("Detalle_MatrizContable"), DataTable)
            For Each oRow In dt.Rows
                oDetalleMatrizContableRow = DirectCast(oDetalleMatrizContableBE.DetalleMatrizContable.NewDetalleMatrizContableRow, DetalleMatrizContableBE.DetalleMatrizContableRow)
                oDetalleMatrizContableRow.CodigoCabeceraMatriz = sCodigoCabeceraMatriz
                oDetalleMatrizContableRow.Aplicar = oRow("Aplicar")
                oDetalleMatrizContableRow.DebeHaber = oRow("DebeHaber")
                oDetalleMatrizContableRow.Glosa = oRow("Glosa")
                oDetalleMatrizContableRow.NumeroCuentaContable = oRow("NumeroCuentaContable")
                oDetalleMatrizContableRow.Secuencia = 0
                oDetalleMatrizContableRow.TipoContabilidad = String.Empty
                oDetalleMatrizContableRow.CodigoTercero = String.Empty
                oDetalleMatrizContableRow.IndicaNroDocumento = String.Empty
                oDetalleMatrizContableRow.CodigoCentroCosto = String.Empty
                oDetalleMatrizContableBE.DetalleMatrizContable.AddDetalleMatrizContableRow(oDetalleMatrizContableRow)
                oDetalleMatrizContableBE.AcceptChanges()
            Next
        End If
        Return oDetalleMatrizContableBE
    End Function
    Private Sub Insertar()
        Dim sCodigoCabeceraMatriz As String = "0"
        oCabeceraMatrizContableBE = CrearObjetoCabecera()
        sCodigoCabeceraMatriz = oCabeceraMatrizContableBM.Insertar_1(oCabeceraMatrizContableBE, DatosRequest)
        oDetalleMatrizContableBE = CrearObjetoDetalle(sCodigoCabeceraMatriz)
        oDetalleMatrizContableBM.Insertar(oDetalleMatrizContableBE, "", DatosRequest)
    End Sub
    Private Sub Modificar()        
        Dim esEliminado As Boolean = False
        oCabeceraMatrizContableBE = CrearObjetoCabecera()
        oCabeceraMatrizContableBM.Modificar(oCabeceraMatrizContableBE, DatosRequest)
        esEliminado = oDetalleMatrizContableBM.Eliminar(Request.QueryString("cod"))
        If (esEliminado) Then
            oDetalleMatrizContableBE = CrearObjetoDetalle(Request.QueryString("cod"))
            oDetalleMatrizContableBM.Insertar(oDetalleMatrizContableBE, "", DatosRequest)
        End If
    End Sub
    Private Sub CargarNegocio()
        HelpCombo.LlenarComboBox(ddlNegocio, oNegocioBM.Listar(DatosRequest).Tables(0), "CodigoNegocio", "Descripcion", True)
    End Sub
    Private Sub CargarSeries()
        HelpCombo.LlenarComboBox(ddlSerie, oPortafolio.ListarSeries(""), "CodigoSerie", "NombreSerie", True)
    End Sub
    Sub CargaMatriz()
        HelpCombo.LlenarComboBox(ddlMatriz, oMatrizContableBM.SeleccionarPorFiltros("", "A", "1").Tables(0), "CodigoMatrizContable", "Descripcion", True)
    End Sub
    Function ValidarExistencias() As Boolean
        Try
            If oCabeceraMatrizContableBM.ValidaExistenciaMatrizFondo(CInt(txtCodigo.Text), ddlNegocio.SelectedValue, ddlSerie.SelectedValue) > 0 Then
                AlertaJS("Ya esxiste una parametria con esta configuración para este negocio.")
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
            Return False
        End Try
    End Function
    Private Sub LimpiarControlesDetalle()
        tbNumeroCuentaContable.Text = String.Empty
        ddlDebeHaber.SelectedIndex = 0
    End Sub
    Private Function InicializarCabecera() As DataTable
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("CodigoCabeceraMatriz", GetType(String)))
        dt.Columns.Add(New DataColumn("NumeroCuentaContable", GetType(String)))
        dt.Columns.Add(New DataColumn("DebeHaber", GetType(String)))
        dt.Columns.Add(New DataColumn("TipoContabilidad", GetType(String)))
        dt.Columns.Add(New DataColumn("Glosa", GetType(String)))
        dt.Columns.Add(New DataColumn("CodigoTercero", GetType(String)))
        dt.Columns.Add(New DataColumn("Tercero", GetType(String)))
        dt.Columns.Add(New DataColumn("Aplicar", GetType(String)))
        dt.Columns.Add(New DataColumn("Secuencia", GetType(String)))
        dt.GetChanges()
        Return dt
    End Function
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CargarNegocio()
            CargarSeries()
            CargaMatriz()
            CargarPagina()
        End If
    End Sub
    Protected Sub btnAgregar_Click(sender As Object, e As System.EventArgs) Handles btnAgregar.Click
        
        Try
            Dim dt As DataTable
            Dim oRow As DataRow
            If hdGrilla.Value = "" Then
                Dim sGlosa, sAplicar As String
                dt = CType(ViewState("Detalle_MatrizContable"), DataTable)
                If dt Is Nothing Then
                    dt = InicializarCabecera()
                End If
                Select Case ddlMatriz.SelectedValue
                    Case "4"
                        sGlosa = "COMISION SAF"
                        sAplicar = "IMPORTE"
                    Case Else
                        sGlosa = ""
                        sAplicar = ""
                End Select
                oRow = dt.NewRow
                oRow(0) = "0"
                oRow(1) = tbNumeroCuentaContable.Text
                oRow(2) = ddlDebeHaber.SelectedValue
                oRow(3) = ""
                oRow(4) = sGlosa
                oRow(5) = ""
                oRow(6) = ""
                oRow(7) = sAplicar
                oRow(8) = ""
                dt.Rows.Add(oRow)
                ViewState("Detalle_MatrizContable") = dt
                dgLista.DataSource = dt
                dgLista.DataBind()
                LimpiarControlesDetalle()
            Else
                dt = DirectCast(ViewState("Detalle_MatrizContable"), DataTable)
                Dim Indice As Integer = CInt(hdGrilla.Value)
                dt.Rows(Indice)("DebeHaber") = ddlDebeHaber.SelectedValue
                dt.Rows(Indice)("NumeroCuentaContable") = tbNumeroCuentaContable.Text
                ViewState("Detalle_MatrizContable") = dt
                dgLista.DataSource = dt
                dgLista.DataBind()
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub btnAceptar_Click(sender As Object, e As System.EventArgs) Handles btnAceptar.Click
        If ValidarExistencias() Then
            If Request.QueryString("cod") Is Nothing Then
                Insertar()
                Response.Redirect("frmMatrizContableFondoListar.aspx")
            Else
                Modificar()
                AlertaJS("Modificado correctamente.")
            End If
        End If
    End Sub
    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            Dim gvr As GridViewRow = CType(CType(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
            Dim estado As String = ""
            If e.CommandName = "Modificar" Then
                btnAgregar.Text = "Modificar"
                hdGrilla.Value = gvr.RowIndex
                ddlDebeHaber.SelectedValue = gvr.Cells(4).Text
                tbNumeroCuentaContable.Text = gvr.Cells(3).Text
            Else
                Dim dt As DataTable = DirectCast(ViewState("Detalle_MatrizContable"), DataTable)
                dt.Rows.RemoveAt(gvr.RowIndex)
                ViewState("Detalle_MatrizContable") = dt
                dgLista.DataSource = dt
                dgLista.DataBind()
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub btnRetornar_Click(sender As Object, e As System.EventArgs) Handles btnRetornar.Click
        Response.Redirect("frmMatrizContableFondoListar.aspx")
    End Sub
End Class