Imports Sit.BusinessLayer
Imports System.Data
Imports Sit.BusinessEntities
Imports System.Configuration
Partial Class Modulos_Inversiones_frmSubNivelesRentaVariable
    Inherits BasePage
#Region "Rutinas"
    Private Sub CargarGrillaNiveles()
        Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
        Dim dsDatos As DataSet, dtNivel2 As New DataTable, tbTotalF As WebControls.TextBox
        dtNivel2.Columns.Add("CodigoPrevOrdenDet")
        dtNivel2.Columns.Add("CantidadOperacion")
        dtNivel2.Columns.Add("PrecioOperacion")
        dtNivel2.Columns.Add("TotalOperacion")
        dsDatos = oPrevOrdenInversionBM.SeleccionarOperaciones(CType(Request.QueryString("Codigo"), Decimal), DatosRequest)
        Dim dtDatos As DataTable = dsDatos.Tables(0)
        Dim dtAsignacion As DataTable = dsDatos.Tables(1)
        If dtDatos.Rows.Count > 0 Then
            txtCorrelativo.Text = dtDatos.Rows(0)(0)
            txtCantidad.Text = dtDatos.Rows(0)(1)
            txtPrecio.Text = dtDatos.Rows(0)(2)
            txtTotal.Text = dtDatos.Rows(0)(3)
            hdCodigoOperacion.Value = dtDatos.Rows(0)(4)
        End If
        If dtAsignacion.Rows.Count = 0 Then
            llenarFilaVacia(dtNivel2)
            dgNivel2.DataSource = dtNivel2
            dgNivel2.DataBind()
            dgNivel2.Rows(0).Visible = False
        Else
            dgNivel2.DataSource = dtAsignacion
            dgNivel2.DataBind()
        End If
        tbTotalF = CType(dgNivel2.FooterRow.FindControl("tbPrecioEF"), WebControls.TextBox)
        tbTotalF.Text = txtPrecio.Text
    End Sub
    Private Sub llenarFilaVacia(ByRef table As DataTable)
        Dim row As DataRow = table.NewRow()
        For Each item As DataColumn In table.Columns
            Select Case item.DataType
                Case GetType(String)
                    row(item.ColumnName) = String.Empty
                Case GetType(Decimal)
                    row(item.ColumnName) = 0.0
            End Select
        Next
        table.Rows.Add(row)
    End Sub
#End Region
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            ibRetornar.Attributes.Add("onclick", "javascript:Salir();")
            If Not Page.IsPostBack Then
                CargarGrillaNiveles()
            End If
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub
    Protected Sub dgNivel2_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgNivel2.RowCommand
        Try
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            If e.CommandName = "Add" Then
                Dim tbCantidadEF As TextBox
                Dim tbPrecioEF As TextBox
                Dim bolValidar As Boolean = False
                Dim oPrevOrdenInversionDetalleBE As New PrevOrdenInversionDetalleBE
                Dim oRow As PrevOrdenInversionDetalleBE.PrevOrdenInversionDetalleRow
                Dim sumTotalEjecucion As Decimal = 0
                Dim bolValidaOperacion As Boolean = False
                Dim cantidadOrden As Decimal
                Dim precioOrden As Decimal
                Dim codigoOperacion As String
                tbCantidadEF = CType(dgNivel2.FooterRow.FindControl("tbCantidadEF"), TextBox)
                tbPrecioEF = CType(dgNivel2.FooterRow.FindControl("tbPrecioEF"), TextBox)
                If Val(tbCantidadEF.Text) <> 0 And Val(tbPrecioEF.Text) <> 0 Then                    
                    cantidadOrden = CDec(txtCantidad.Text)
                    precioOrden = CDec(txtPrecio.Text)
                    codigoOperacion = hdCodigoOperacion.Value
                    bolValidaOperacion = oPrevOrdenInversionBM.ValidarOperaciones(CType(Request.QueryString("Codigo"), Decimal), _
                    CType(tbCantidadEF.Text, Decimal), bolValidaOperacion)
                    'OT 10090 - 26/07/2017 - Carlos Espejo
                    'Descripcion: Validacion para que no exceda la cantidad de la orden
                    If Not bolValidaOperacion Then
                        AlertaJS("El numero de instrumentos de la operación excede al numero de instrumentos de la orden!")
                        Exit Sub
                    End If
                    'OT 10090 Fin
                Else
                    AlertaJS("Debe ingresar un valor distinto de cero!")
                    Exit Sub
                End If
                oRow = CType(oPrevOrdenInversionDetalleBE.PrevOrdenInversionDetalle.NewRow(), PrevOrdenInversionDetalleBE.PrevOrdenInversionDetalleRow)
                oRow.CodigoPrevOrden = CType(Request.QueryString("Codigo"), Decimal)
                oRow.CantidadOperacion = CType(tbCantidadEF.Text, Decimal)
                oRow.PrecioOperacion = CType(tbPrecioEF.Text, Decimal)
                oPrevOrdenInversionDetalleBE.PrevOrdenInversionDetalle.AddPrevOrdenInversionDetalleRow(oRow)
                oPrevOrdenInversionDetalleBE.AcceptChanges()
                oPrevOrdenInversionBM.InsertarOperacion(oPrevOrdenInversionDetalleBE, DatosRequest)
                CargarGrillaNiveles()
            End If
            If e.CommandName = "_Delete" Then
                oPrevOrdenInversionBM.EliminarOperacion(CType(e.CommandArgument.ToString(), Decimal), DatosRequest)
                CargarGrillaNiveles()
            End If
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub
    Protected Sub ibGrabar_Click(sender As Object, e As System.EventArgs) Handles ibGrabar.Click
        Try
            Dim sumaCantidad As Decimal = 0
            Dim cantidadOrden As Decimal
            Dim totalOrden As Decimal = 0
            Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
            Dim oPrevOrdenInversionDetalleBE As New PrevOrdenInversionDetalleBE
            Dim oRow As PrevOrdenInversionDetalleBE.PrevOrdenInversionDetalleRow
            For Each fila As GridViewRow In dgNivel2.Rows
                If fila.RowType = DataControlRowType.DataRow Or fila.RowType = DataControlRowType.EmptyDataRow Then
                    Dim tbCantidadE As TextBox
                    Dim tbPrecioE As TextBox
                    Dim lbCodigoPrevOrdenDet As Label
                    tbCantidadE = CType(fila.FindControl("tbCantidadE"), TextBox)
                    tbPrecioE = CType(fila.FindControl("tbPrecioE"), TextBox)
                    lbCodigoPrevOrdenDet = CType(fila.FindControl("lbCodigoPrevOrdenDet"), Label)
                    If Val(tbCantidadE.Text) <> 0 And Val(tbPrecioE.Text) <> 0 Then
                        sumaCantidad = sumaCantidad + tbCantidadE.Text
                        oRow = CType(oPrevOrdenInversionDetalleBE.PrevOrdenInversionDetalle.NewRow(), PrevOrdenInversionDetalleBE.PrevOrdenInversionDetalleRow)
                        oRow.CantidadOperacion = CType(tbCantidadE.Text, Decimal)
                        oRow.PrecioOperacion = CType(tbPrecioE.Text, Decimal)
                        oRow.CodigoPrevOrdenDet = CType(lbCodigoPrevOrdenDet.Text, Decimal)
                        oPrevOrdenInversionDetalleBE.PrevOrdenInversionDetalle.AddPrevOrdenInversionDetalleRow(oRow)
                        oPrevOrdenInversionDetalleBE.AcceptChanges()
                    Else
                        AlertaJS("Ingrese correctamente el registro! ")
                        Exit Sub
                    End If
                End If
            Next
            'OT 10090 - 26/07/2017 - Carlos Espejo
            'Descripcion: Validacion para que no exceda la cantidad de la orden
            cantidadOrden = txtCantidad.Text
            If sumaCantidad <= cantidadOrden Then
                oPrevOrdenInversionBM.ModificarOperacion(oPrevOrdenInversionDetalleBE, DatosRequest)
                CargarGrillaNiveles()
                AlertaJS("Operacion Exitosa")
            Else
                AlertaJS("No puede continuar. Las cantidad total supera la cantidad ordenada!")
            End If
            'OT 10090 Fin
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub
    Protected Sub dgNivel2_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgNivel2.RowDataBound
        If e.Row.RowType = ListItemType.Item Then
            If Request.QueryString("Estado").ToString = "EJE" Or Request.QueryString("Estado").ToString = "APR" Then
                CType(e.Row.FindControl("ImageButton1"), ImageButton).Enabled = False
            End If
        End If
        If e.Row.RowType = ListItemType.Footer Then
            If Request.QueryString("Estado").ToString = "EJE" Or Request.QueryString("Estado").ToString = "APR" Then
                CType(e.Row.FindControl("ImageButton2"), ImageButton).Enabled = False
            End If
        End If
    End Sub
End Class