Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data.SqlTypes
Imports System.Collections
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmBalanceContable
    Inherits BasePage
    Private Operacion As String
    Private CodigoEmisor As String
    Private oBalanceContableBE As BalanceContableBE

#Region "/*Métodos de la página*/"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Operacion = Request.QueryString("ope")
            If (Operacion = "mod") Then
                CodigoEmisor = Request.QueryString("codEm")
            End If

            If Not Page.IsPostBack Then
                Me.CargarCombos()
                Me.hd.Value = ""
                If (Operacion = "mod") Then
                    hdTipoOperacion.Value = "mod"
                    Me.cargarRegistro()
                Else
                    hdTipoOperacion.Value = "ins"
                End If
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                tbCodigoEmisor.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                tbDescEmisor.Text = CType(Session("SS_DatosModal"), String())(1).ToString()
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try        
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click                
        Try
            Dim resultado As Boolean
            Me.oBalanceContableBE = Me.obtenerInstancia()
            If Me.Operacion = "reg" Then
                resultado = New BalanceContableBM().Insertar(Me.oBalanceContableBE, Me.DatosRequest)
                If (resultado) Then
                    Me.AlertaJS("Se registro el Balance Contable satisfactoriamente.")
                    Me.limpiarCampos()
                Else
                    Me.AlertaJS("El registro ya existe!!.")
                    Exit Sub
                End If
            Else
                resultado = New BalanceContableBM().Modificar(Me.oBalanceContableBE, Me.DatosRequest)
                Me.AlertaJS("Se modifico el Balance Contable satisfactoriamente.")
            End If
        Catch ex As Exception
            Me.AlertaJS("Ocurrió un error al Aceptar")
        End Try
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaBalanceContable.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar")
        End Try        
    End Sub

#End Region

#Region "/*Métodos personalizados*/"

    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
    End Sub

    Private Sub cargarRegistro()
        Dim oBalanceContableBM As New BalanceContableBM
        Dim oBalanceContableBE As New BalanceContableBE
        Dim oRow As BalanceContableBE.BalanceContableRow

        oBalanceContableBE = New BalanceContableBM().SeleccionarPorFiltro(CodigoEmisor, "", Me.DatosRequest)
        oRow = DirectCast(oBalanceContableBE.BalanceContable.Rows(0), BalanceContableBE.BalanceContableRow)

        Me.ddlSituacion.SelectedValue = oRow.Situacion.ToString()
        'Format(oRow.MontoNominalOperacion, "##,##0.0000000")
        Me.tbCodigoEmisor.Text = oRow.CodigoEmisor.ToString
        Me.tbDescEmisor.Text = oRow.DescripcionEmisor.ToString
        Me.tbTotalActivo.Text = Format(oRow.TotalActivo, "##,##0.0000000")
        Me.tbTotalPasivo.Text = Format(oRow.TotalPasivo, "##,##0.0000000")
        Me.tbPatrimonio.Text = Format(oRow.Patrimonio, "##,##0.0000000")
        Me.tbFechaVigencia.Text = UIUtility.ConvertirFechaaString(oRow.FechaVigencia)
        habilitarControles(False)
    End Sub

    Private Sub habilitarControles(ByVal flag As Boolean)
        Me.lkbBuscar.Enabled = flag
        Me.tbCodigoEmisor.Enabled = flag
        Me.tbDescEmisor.Enabled = flag
    End Sub

    Private Function obtenerInstancia() As BalanceContableBE
        Dim oBalanceContableBE As New BalanceContableBE
        Dim oRow As BalanceContableBE.BalanceContableRow
        oRow = CType(oBalanceContableBE.BalanceContable.NewRow(), BalanceContableBE.BalanceContableRow)

        oRow.CodigoEmisor = Me.tbCodigoEmisor.Text
        oRow.TotalActivo = CType(Me.tbTotalActivo.Text, Decimal)
        oRow.TotalPasivo = CType(Me.tbTotalPasivo.Text, Decimal)
        oRow.Patrimonio = CType(Me.tbPatrimonio.Text, Decimal)
        oRow.FechaVigencia = UIUtility.ConvertirFechaaDecimal(Me.tbFechaVigencia.Text)
        oRow.Situacion = Me.ddlSituacion.SelectedValue

        oBalanceContableBE.BalanceContable.AddBalanceContableRow(oRow)
        oBalanceContableBE.BalanceContable.AcceptChanges()
        Return oBalanceContableBE
    End Function

    Private Sub limpiarCampos()
        Me.ddlSituacion.SelectedIndex = 0
        Me.tbTotalActivo.Text = ""
        Me.tbTotalPasivo.Text = ""
        Me.tbPatrimonio.Text = ""
        Me.tbFechaVigencia.Text = ""
        Me.hd.Value = ""
        Me.tbCodigoEmisor.Text = ""
        Me.tbDescEmisor.Text = ""
    End Sub

#End Region

End Class
