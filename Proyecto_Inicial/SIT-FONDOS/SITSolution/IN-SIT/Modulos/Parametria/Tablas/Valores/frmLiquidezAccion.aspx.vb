Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data.SqlTypes
Imports System.Collections
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmLiquidezAccion
    Inherits BasePage

    Private operacion As String
    Private CodigoMnemonico As String
    Private oLiquidezAccionBE As LiquidezAccionBE

#Region "/* Métodos de la Página */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            operacion = Request.QueryString("ope")
            If (operacion = "mod") Then
                CodigoMnemonico = Request.QueryString("codMne")
                tipoOperacion.Value = "mod"
            End If

            If Not Page.IsPostBack Then
                CargarCombos()
                hd.Value = ""
                If (operacion = "mod") Then
                    cargarRegistro()
                End If
            End If

            If Not Session("SS_DatosModal") Is Nothing Then
                tbCodigoMnemonico.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                tbDescMnemonico.Text = CType(Session("SS_DatosModal"), String())(1).ToString()
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try        
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click        
        Try
            Dim resultado As Boolean
            Dim oLiqBE As LiquidezAccionBE
            oLiquidezAccionBE = obtenerInstancia()
            If operacion = "reg" Then
                oLiqBE = New LiquidezAccionBM().SeleccionarPorFiltro(tbCodigoMnemonico.Text, "", DatosRequest)
                If Not IsNothing(oLiqBE) And oLiqBE.Tables(0).Rows.Count = 0 Then
                    resultado = New LiquidezAccionBM().Insertar(oLiquidezAccionBE, DatosRequest)
                    If (resultado) Then
                        AlertaJS("Se Registro la Liquidez Accion satisfactoriamente.")
                        limpiarCampos()
                    Else
                        AlertaJS("El Registro ya existe!!.")
                        Exit Sub
                    End If
                Else
                    AlertaJS("El Registro ya existe!!.")
                    Exit Sub
                End If
            Else
                resultado = New LiquidezAccionBM().Modificar(oLiquidezAccionBE, DatosRequest)
                AlertaJS("Se Modifico la Liquidez Accion satisfactoriamente.")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Aceptar")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("frmBusquedaLiquidezAccion.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try        
    End Sub

#End Region

#Region "/* Métodos de la Página */"

    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
    End Sub

    Private Sub cargarRegistro()
        Dim oTipoInstrumentoBM As New TipoInstrumentoBM
        Dim oTipoInstrumentoBE As New TipoInstrumentoBE
        Dim oRow As LiquidezAccionBE.LiquidezAccionRow

        oLiquidezAccionBE = New LiquidezAccionBM().SeleccionarPorFiltro(CodigoMnemonico, "", DatosRequest)
        oRow = DirectCast(oLiquidezAccionBE.LiquidezAccion.Rows(0), LiquidezAccionBE.LiquidezAccionRow)

        ddlSituacion.SelectedValue = oRow.Situacion.ToString()
        tbCodigoMnemonico.Text = oRow.CodigoMnemonico.ToString
        tbLiquidez.Text = oRow.CriterioLiquidez.ToString

        tbDescMnemonico.Text = oRow.DescripcionMnemonico.ToString

        habilitarControles(False)
    End Sub

    Private Sub habilitarControles(ByVal flag As Boolean)
        lkbModal.Enabled = flag
        tbCodigoMnemonico.Enabled = flag
        tbDescMnemonico.Enabled = flag
    End Sub

    Private Function obtenerInstancia() As LiquidezAccionBE
        Dim oLiquidezAccionBE As New LiquidezAccionBE
        Dim oRow As LiquidezAccionBE.LiquidezAccionRow
        oRow = CType(oLiquidezAccionBE.LiquidezAccion.NewRow(), LiquidezAccionBE.LiquidezAccionRow)

        oRow.CodigoMnemonico = tbCodigoMnemonico.Text
        oRow.CriterioLiquidez = tbLiquidez.Text.ToUpper
        oRow.Situacion = ddlSituacion.SelectedValue

        oLiquidezAccionBE.LiquidezAccion.AddLiquidezAccionRow(oRow)
        oLiquidezAccionBE.LiquidezAccion.AcceptChanges()
        Return oLiquidezAccionBE
    End Function

    Private Sub limpiarCampos()
        ddlSituacion.SelectedIndex = 0
        tbCodigoMnemonico.Text = ""
        tbLiquidez.Text = ""
        hd.Value = ""
        tbDescMnemonico.Text = ""
    End Sub

#End Region

End Class
