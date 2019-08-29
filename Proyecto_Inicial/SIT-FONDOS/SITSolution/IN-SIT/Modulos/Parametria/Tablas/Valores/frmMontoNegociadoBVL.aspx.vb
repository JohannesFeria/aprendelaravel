Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data.SqlTypes
Imports System.Collections
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmMontoNegociadoBVL
    Inherits BasePage

    Private operacion As String
    Private NumeroOperacion As Decimal
    Private FechaOperacion As Decimal
    Private oMontoNegociadoBVLBE As MontoNegociadoBVLBE

#Region "/* Métodos de la Página */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            operacion = Request.QueryString("ope")
            If (operacion = "mod") Then
                NumeroOperacion = CType(Request.QueryString("numOP"), Decimal)
                FechaOperacion = CType(Request.QueryString("fecOP"), Decimal)
            End If

            If Not Page.IsPostBack Then
                Me.CargarCombos()
                Me.hd.Value = ""
                If (operacion = "mod") Then
                    Me.cargarRegistro()
                Else
                    Me.limpiarCampos()
                End If
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                If hdTipoBusqueda.Value = "C" Then
                    tbCodigoComprador.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                    tbDescripcionComprador.Text = CType(Session("SS_DatosModal"), String())(1).ToString()
                ElseIf hdTipoBusqueda.Value = "V" Then
                    tbCodigoVendedor.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                    tbDescripcionVendedor.Text = CType(Session("SS_DatosModal"), String())(1).ToString()
                ElseIf hdTipoBusqueda.Value = "M" Then
                    tbCodigoMnemonico.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                    tbDescMnemonico.Text = CType(Session("SS_DatosModal"), String())(1).ToString()
                End If
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try        
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click        
        Try
            Dim resultado As Boolean
            Me.oMontoNegociadoBVLBE = Me.obtenerInstancia()
            If Me.operacion = "reg" Then
                resultado = New MontoNegociadoBVLBM().Insertar(Me.oMontoNegociadoBVLBE, Me.DatosRequest)
                If (resultado) Then
                    Me.AlertaJS("Se registro el Monto Negociado BVL satisfactoriamente.")
                    Me.limpiarCampos()
                Else
                    AlertaJS("El registro ya existe!!.")
                    Exit Sub
                End If
            Else
                resultado = New MontoNegociadoBVLBM().Modificar(Me.oMontoNegociadoBVLBE, Me.DatosRequest)
                Me.AlertaJS("Se modifico el Monto Negociado BVL satisfactoriamente.")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Aceptar")
        End Try
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaMontoNegociadoBVL.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retorna")
        End Try        
    End Sub

#End Region

#Region "/* Métodos de la Página */"

    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
    End Sub

    Private Sub cargarRegistro()
        Dim oTipoInstrumentoBM As New TipoInstrumentoBM
        Dim oTipoInstrumentoBE As New TipoInstrumentoBE
        Dim oRow As MontoNegociadoBVLBE.MontoNegociadoBVLRow

        oMontoNegociadoBVLBE = New MontoNegociadoBVLBM().SeleccionarPorFiltro(Me.FechaOperacion, Me.NumeroOperacion, "", "", Me.DatosRequest)
        oRow = DirectCast(oMontoNegociadoBVLBE.MontoNegociadoBVL.Rows(0), MontoNegociadoBVLBE.MontoNegociadoBVLRow)

        Me.tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
        Me.tbNumeroOperacion.Text = oRow.NumeroOperacion

        Me.tbCodigoMnemonico.Text = oRow.CodigoMnemonico.ToString
        Me.tbDescMnemonico.Text = oRow.DescripcionMnemonico.ToString
        Me.tbMontoEfectivo.Text = Format(oRow.MontoEfectivo, "##,##0.0000000")
        Me.tbPrecio.Text = Format(oRow.Precio, "##,##0.0000000")
        Me.tbCantidad.Text = Format(oRow.Cantidad, "##,##0.0000000")

        Me.tbDescripcionComprador.Text = oRow.DescripcionComprador
        Me.tbDescripcionVendedor.Text = oRow.DescripcionVendedor
        Me.tbCodigoComprador.Text = oRow.Comprador
        Me.tbCodigoVendedor.Text = oRow.Vendedor

        Me.ddlSituacion.SelectedValue = oRow.Situacion.ToString()

        habilitarControles(False)
    End Sub

    Private Sub habilitarControles(ByVal flag As Boolean)
        'spanFecha.Visible = flag
        If flag Then
            spanFecha.Attributes.Add("class", "input-append date")
        Else
            spanFecha.Attributes.Add("class", "input-append")
        End If
        tbFechaOperacion.Enabled = flag
        Me.tbFechaOperacion.Enabled = flag
        Me.tbNumeroOperacion.Enabled = flag
    End Sub

    Private Function obtenerInstancia() As MontoNegociadoBVLBE
        Dim oMontoNegociadoBVLBE As New MontoNegociadoBVLBE
        Dim oRow As MontoNegociadoBVLBE.MontoNegociadoBVLRow
        oRow = CType(oMontoNegociadoBVLBE.MontoNegociadoBVL.NewRow(), MontoNegociadoBVLBE.MontoNegociadoBVLRow)

        oRow.FechaOperacion = UIUtility.ConvertirFechaaDecimal(Me.tbFechaOperacion.Text)
        oRow.NumeroOperacion = CType(Me.tbNumeroOperacion.Text, Decimal)
        oRow.CodigoMnemonico = Me.tbCodigoMnemonico.Text
        oRow.Comprador = Me.tbCodigoComprador.Text
        oRow.Vendedor = Me.tbCodigoVendedor.Text
        oRow.Precio = CType(Me.tbPrecio.Text, Decimal)
        oRow.Cantidad = CType(Me.tbCantidad.Text, Decimal)
        oRow.MontoEfectivo = CType(Me.tbMontoEfectivo.Text, Decimal)
        oRow.Situacion = Me.ddlSituacion.SelectedValue

        oMontoNegociadoBVLBE.MontoNegociadoBVL.AddMontoNegociadoBVLRow(oRow)
        oMontoNegociadoBVLBE.MontoNegociadoBVL.AcceptChanges()
        Return oMontoNegociadoBVLBE
    End Function

    Private Sub limpiarCampos()
        Me.tbFechaOperacion.Text = ""
        Me.tbNumeroOperacion.Text = ""
        Me.tbCodigoMnemonico.Text = ""
        Me.tbDescMnemonico.Text = ""
        Me.tbCodigoComprador.Text = ""
        Me.tbDescripcionComprador.Text = ""
        Me.tbCodigoVendedor.Text = ""
        Me.tbDescripcionVendedor.Text = ""
        Me.tbPrecio.Text = ""
        Me.tbCantidad.Text = ""
        Me.tbMontoEfectivo.Text = ""
        Me.ddlSituacion.SelectedIndex = 0
        Me.hd.Value = ""
        Me.habilitarControles(True)
    End Sub

#End Region

End Class
