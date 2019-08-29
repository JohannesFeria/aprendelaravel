Imports System.Data
Imports Sit.BusinessLayer

Partial Class Modulos_Inversiones_frmAgruparAcciones
    Inherits BasePage

#Region "Variables"

    Dim oOrdenPreOrdenInversionBM As New OrdenPreOrdenInversionBM

#End Region

#Region "Eventos"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Not (Page.IsPostBack) Then
            CargarPagina()
        End If

    End Sub

    Public Sub dgListaOC_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim chkTemp As CheckBox = CType(sender, CheckBox)
            Dim dgi As GridViewRow
            Dim i As Int32 = 0
            Dim nSeleccionadas As Int64 = 0
            Dim valor As String = String.Empty
            If hdnSelect.Value = String.Empty Then
                dgi = CType(chkTemp.Parent.Parent, GridViewRow)
                i = dgi.RowIndex
                If chkTemp.Checked = True Then
                    dgListaOC.SelectedIndex = dgi.RowIndex
                    
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación")
        End Try
    End Sub

    Private Sub ddlFondoOE_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlFondoOE.SelectedIndexChanged
        Try
            If Me.ddlFondoOE.SelectedIndex > 0 Then
                Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(ddlFondoOE.SelectedValue.ToString))
                ViewState("Fecha") = Me.tbFechaOperacion.Text.Trim
            Else
                Me.tbFechaOperacion.Text = UIUtility.ConvertirDecimalAStringFormatoFecha(String.Format("{0:yyyyMMdd}", DateTime.Today))
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Selección")
        End Try
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As System.EventArgs) Handles btnBuscar.Click
        Buscar()
    End Sub

    Protected Sub btnAgrupar_Click(sender As Object, e As System.EventArgs) Handles btnAgrupar.Click
        Agrupado("S")
    End Sub

    Protected Sub btnDesagrupar_Click(sender As Object, e As System.EventArgs) Handles btnDesagrupar.Click
        Agrupado("N")
    End Sub

    Protected Sub btnRegresar_Click(sender As Object, e As System.EventArgs) Handles btnRegresar.Click
        Response.Redirect("frmConfirmarOrdenesDeInversion.aspx")
    End Sub

    Private Sub btnSeleccionar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        Try
            Dim grilla As GridView
            Dim chkControl As String = String.Empty
            Dim bBotones As String = String.Empty

            grilla = dgListaOC
            chkControl = "chkSelectOC"

            grilla.SelectedIndex = -1
            Dim iCont As Int64 = grilla.Rows.Count - 1
            Dim chk As CheckBox
            If hdnSelect.Value = "2" Then
                While iCont >= 0
                    If grilla.Rows(iCont).FindControl(chkControl).GetType Is GetType(CheckBox) Then
                        chk = CType(grilla.Rows(iCont).FindControl(chkControl), CheckBox)
                        If chk.Checked = False And chk.Visible = True Then
                            If grilla.Rows(iCont).RowType = DataControlRowType.DataRow Then
                                grilla.Rows(iCont).BackColor = System.Drawing.Color.White 'Smoke
                            ElseIf grilla.Rows(iCont).RowType = DataControlRowType.DataRow Then
                                grilla.Rows(iCont).BackColor = System.Drawing.Color.White
                            End If
                        End If
                    End If
                    iCont = iCont - 1
                End While
            Else
                While iCont >= 0
                    If grilla.Rows(iCont).FindControl(chkControl).GetType Is GetType(CheckBox) Then
                        chk = CType(grilla.Rows(iCont).FindControl(chkControl), CheckBox)
                        If chk.Checked = True Then
                            grilla.Rows(iCont).BackColor = System.Drawing.Color.LemonChiffon
                            bBotones = "SI"
                        Else
                            grilla.Rows(iCont).BackColor = System.Drawing.Color.White
                        End If
                    End If
                    iCont = iCont - 1
                End While
            End If

            hdnSelect.Value = String.Empty
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Selección")
        End Try
    End Sub

#End Region

#Region "Metodos"

    Public Sub CargarPagina()


        tbFechaOperacion.Text = UIUtility.ConvertirDecimalAStringFormatoFecha(String.Format("{0:yyyyMMdd}", DateTime.Today))
        CargarPortafolio()
        CargarMercado()

    End Sub

    Public Sub CargarPortafolio()

        Dim tablaClaseInstrumento As New DataTable
        Dim oPortafolioBM As New PortafolioBM
        HelpCombo.LlenarComboBox(ddlFondoOE, oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS), "CodigoPortafolio", "Descripcion", True, "TODOS")

    End Sub

    Public Sub CargarMercado()
        Dim DtablaMercado As New DataTable
        Dim oMercadoBM As New MercadoBM
        Try
            DtablaMercado = oMercadoBM.ListarActivos(DatosRequest).Tables(0)
            DtablaMercado.DefaultView.Sort = "CodigoMercado"
            HelpCombo.LlenarComboBoxBusquedas(Me.ddlMercado, DtablaMercado, "CodigoMercado", "Descripcion", True)
        Finally
            oMercadoBM = Nothing
            GC.Collect()
        End Try
    End Sub

    Public Sub Buscar()

        Dim fondo As String = IIf(ddlFondoOE.SelectedIndex = 0, "", ddlFondoOE.SelectedValue)
        Dim fecha As String = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text)
        Dim codigoMercado As String = IIf(ddlMercado.SelectedIndex = 0, "", ddlMercado.SelectedValue)

        Try
            Dim dtblDatos As DataTable = oOrdenPreOrdenInversionBM.ListarOIConfirmadasAgrupados(fondo, fecha, codigoMercado)
            If Not (dtblDatos Is Nothing) Then
                Me.lblCantidadOIC.Text = String.Format("({0})", dtblDatos.Rows.Count) + " - " + ddlFondoOE.SelectedItem.Text
            Else
                Me.lblCantidadOIC.Text = String.Format("({0})", 0)
            End If
            dgListaOC.DataSource = dtblDatos
            dgListaOC.DataBind()
            ViewState("OIConfirmadas") = dtblDatos
        Catch ex As Exception
            Me.dgListaOC.DataSource = Nothing
            Me.dgListaOC.DataBind()
        End Try

    End Sub

    Public Sub Agrupado(ByVal Opcion As String)

        Dim respuesta As String
        Dim Ordenes As String = ObtieneOrdenes()

        If (Ordenes.Length > 0) Then

            If (ValidaMercado()) Then

                respuesta = oOrdenPreOrdenInversionBM.AgruparDesagruparAccionesOI(Ordenes, Opcion)

                If (respuesta.Length = 0) Then 'Exito
                    If (Opcion.Equals("S")) Then

                        AlertaJS("Se realizo la agrupación correctamente.")
                    Else

                        AlertaJS("Se realizo la desagrupación correctamente.")
                    End If
                Else
                    AlertaJS(respuesta)
                End If

                Buscar()
            Else
                AlertaJS("No se puede agrupar acciones de mercado local con mercado extranjero, favor de validar.")
            End If
        Else
            AlertaJS("Seleccione una orden.")
        End If
    End Sub


    Function ObtieneOrdenes() As String
        Dim Ordenes As String = ""
        Dim iCont As Integer = dgListaOC.Rows.Count - 1
        Dim chk As CheckBox
        Dim chkBuscar As String = "chkSelectOC"

        While iCont >= 0
            If dgListaOC.Rows(iCont).FindControl(chkBuscar).GetType Is GetType(CheckBox) Then
                chk = CType(dgListaOC.Rows(iCont).FindControl(chkBuscar), CheckBox)
                If chk.Checked = True Then
                    Ordenes = Ordenes + dgListaOC.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("CodigoOrden", dgListaOC)).Text + ","
                End If
            End If
            iCont = iCont - 1
        End While
        Return Ordenes
    End Function

    Function ValidaMercado() As Boolean
        Dim iCont As Integer = dgListaOC.Rows.Count - 1
        Dim chk As CheckBox
        Dim chkBuscar As String = "chkSelectOC"
        Dim Mercado As String = ""
        Dim MercadoSeleccion As String = ""
        Dim Valida As Boolean = True
        While iCont >= 0
            If dgListaOC.Rows(iCont).FindControl(chkBuscar).GetType Is GetType(CheckBox) Then
                chk = CType(dgListaOC.Rows(iCont).FindControl(chkBuscar), CheckBox)
                If chk.Checked = True Then

                    If (Mercado = "") Then
                        Mercado = dgListaOC.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("CodigoMercado", dgListaOC)).Text
                    Else
                        MercadoSeleccion = dgListaOC.Rows.Item(iCont).Cells(obtenerIndiceColumna_Grilla("CodigoMercado", dgListaOC)).Text
                        If Not (MercadoSeleccion.Equals(Mercado)) Then
                            Valida = False
                            Return Valida
                        End If
                    End If
                End If
            End If
            iCont = iCont - 1
        End While
        Return Valida
    End Function

#End Region

#Region "Funciones"

    Private Sub displayBotones(ByVal divNombre As System.Web.UI.HtmlControls.HtmlGenericControl, ByVal opcion As String)
        divNombre.Attributes.Add("style", "text-align:right;display:" + opcion)
    End Sub

    Protected Sub dgListaOC_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgListaOC.RowCommand
        Dim Row As GridViewRow
        Dim i As Integer
        Try
            If e.CommandName = "Seleccionar" Then
                Row = DirectCast(DirectCast(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                i = Row.RowIndex
                Dim codigoOperacion As String = String.Empty
                Dim iCont As Int64 = dgListaOC.Rows.Count - 1
                dgListaOC.SelectedIndex = Row.RowIndex
                seleccionarMasivoRow_Grilla(dgListaOC, "Seleccionar")
                'If i >= 0 And i <= iCont Then
                '    dgListaOC.Rows.Item(i).BackColor = System.Drawing.Color.LemonChiffon
                'End If
            End If

        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla")
        End Try
    End Sub

    Private Function retornarNumSeleccionados(ByVal grilla As GridView, ByVal chkNombre As String) As Integer
        Dim iCont As Int64 = grilla.Rows.Count - 1
        Dim chk As CheckBox
        retornarNumSeleccionados = 0
        While iCont >= 0 And retornarNumSeleccionados < 1
            If grilla.Rows(iCont).FindControl(chkNombre).GetType Is GetType(CheckBox) Then
                chk = CType(grilla.Rows(iCont).FindControl(chkNombre), CheckBox)
                If chk.Checked = True Then
                    retornarNumSeleccionados += 1
                End If
            End If
            iCont = iCont - 1
        End While
        Return retornarNumSeleccionados
    End Function

    Private Sub seleccionarMasivoRow_Grilla(ByVal grilla As GridView, ByVal chkBuscar As String)
        Dim iCont As Integer = grilla.Rows.Count - 1
        Dim chk As CheckBox
        While iCont >= 0
            If grilla.Rows(iCont).FindControl(chkBuscar).GetType Is GetType(CheckBox) Then
                chk = CType(grilla.Rows(iCont).FindControl(chkBuscar), CheckBox)
                If chk.Checked = True Then
                    chk.Checked = False
                    If grilla.Rows(iCont).RowType = DataControlRowType.DataRow Then
                        grilla.Rows(iCont).BackColor = System.Drawing.Color.White
                    ElseIf grilla.Rows(iCont).RowType = DataControlRowType.DataRow Then
                        grilla.Rows(iCont).BackColor = System.Drawing.Color.White
                    End If
                End If
            End If
            iCont = iCont - 1
        End While
    End Sub

    Private Function obtenerIndiceColumna_Grilla(ByVal nomCol As String, ByVal grilla As GridView) As Integer
        Dim columna As DataControlFieldCollection = grilla.Columns
        Dim indiceCol As Integer = -1
        For Each celda As DataControlField In columna
            If TypeOf celda Is System.Web.UI.WebControls.BoundField Then
                If CType(celda, BoundField).DataField = nomCol Then
                    indiceCol = columna.IndexOf(celda)
                    Exit For
                End If
            End If
        Next
        Return indiceCol
    End Function

#End Region

End Class
