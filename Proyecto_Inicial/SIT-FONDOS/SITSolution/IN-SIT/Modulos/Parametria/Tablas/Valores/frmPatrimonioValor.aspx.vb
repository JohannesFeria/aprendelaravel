Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data.SqlTypes
Imports System.Collections
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmPatrimonioValor
    Inherits BasePage

    Private operacion As String
    Private tipoPatrimonioValor As String
    Private CodigoTipoInstrumento As String
    Private CodigoMnemonico As String
    Private FechaVigencia As String
    Private oPatrimonioValorBE As PatrimonioValorBE

#Region "/* Métodos de la Página */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            operacion = Request.QueryString("ope")
            If (operacion = "mod") Then
                CodigoTipoInstrumento = Request.QueryString("codTI")
                CodigoMnemonico = Request.QueryString("codMne")
                FechaVigencia = Request.QueryString("fecVig")
                hdTipoOperacion.Value = "mod"
            Else
                hdTipoOperacion.Value = "ins"
            End If

            If Not Page.IsPostBack Then
                Me.CargarCombos()
                Me.hd.Value = ""
                If (operacion = "mod") Then
                    Me.cargarRegistro()
                End If
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                If hdTipoBusqueda.Value = "I" Then
                    tbCodigoTipoInstrumento.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                    tbDescTipoInstrumento.Text = CType(Session("SS_DatosModal"), String())(1).ToString()
                Else
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
            Me.oPatrimonioValorBE = Me.obtenerInstancia()
            If Me.operacion = "reg" Then
                resultado = New PatrimonioValorBM().Insertar(Me.oPatrimonioValorBE, Me.DatosRequest)
                If (resultado) Then
                    Me.AlertaJS("Se registro el Patrimonio Valor satisfactoriamente.")
                    Me.limpiarCampos()
                Else
                    Me.AlertaJS("El registro ya existe!!.")
                    Exit Sub
                End If
            Else
                resultado = New PatrimonioValorBM().Modificar(Me.oPatrimonioValorBE, Me.DatosRequest)
                Me.AlertaJS("Se modifico el Patrimonio Valor satisfactoriamente.")
            End If
        Catch ex As Exception
            Me.AlertaJS("Ocurrió un error al Aceptar")
        End Try
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaPatrimonioValor.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar")
        End Try        
    End Sub

#End Region

#Region "/* Métodos Personalizados */"

    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
    End Sub

    Private Sub cargarRegistro()
        Dim oTipoInstrumentoBM As New TipoInstrumentoBM
        Dim oTipoInstrumentoBE As New TipoInstrumentoBE
        Dim oRow As PatrimonioValorBE.PatrimonioValorRow

        oPatrimonioValorBE = New PatrimonioValorBM().Seleccionar(CodigoTipoInstrumento, CodigoMnemonico, FechaVigencia, Me.DatosRequest)
        oRow = DirectCast(oPatrimonioValorBE.PatrimonioValor.Rows(0), PatrimonioValorBE.PatrimonioValorRow)

        Me.ddlSituacion.SelectedValue = oRow.Situacion.ToString()
        Me.tbCodigoTipoInstrumento.Text = oRow.CodigoTipoInstrumento.ToString
        Me.tbCodigoMnemonico.Text = oRow.CodigoMnemonico.ToString
        Me.tbPatrimonio.Text = Format(oRow.Patrimonio, "##,##0.0000000")
        Me.tbFechaVigencia.Text = UIUtility.ConvertirFechaaString(oRow.FechaVigencia)

        Me.tbDescMnemonico.Text = oRow.DescripcionMnemonico.ToString
        Me.tbDescTipoInstrumento.Text = oRow.DescripcionTipoInstrumento.ToString

        habilitarControles(False)
    End Sub

    Private Sub habilitarControles(ByVal flag As Boolean)
        Me.lkbBuscarInstrumento.Enabled = flag
        Me.lkbBuscarMnemonico.Enabled = flag
        Me.tbCodigoTipoInstrumento.Enabled = flag
        Me.tbCodigoMnemonico.Enabled = flag
        Me.tbDescMnemonico.Enabled = flag
        Me.tbDescTipoInstrumento.Enabled = flag
    End Sub

    Private Function obtenerInstancia() As PatrimonioValorBE
        Dim oPatrimonioValorBE As New PatrimonioValorBE
        Dim oRow As PatrimonioValorBE.PatrimonioValorRow
        oRow = CType(oPatrimonioValorBE.PatrimonioValor.NewRow(), PatrimonioValorBE.PatrimonioValorRow)

        oRow.CodigoTipoInstrumento = Me.tbCodigoTipoInstrumento.Text
        oRow.CodigoMnemonico = Me.tbCodigoMnemonico.Text
        oRow.Patrimonio = CType(Me.tbPatrimonio.Text, Decimal)
        oRow.FechaVigencia = UIUtility.ConvertirFechaaDecimal(Me.tbFechaVigencia.Text)
        oRow.Situacion = Me.ddlSituacion.SelectedValue

        oPatrimonioValorBE.PatrimonioValor.AddPatrimonioValorRow(oRow)
        oPatrimonioValorBE.PatrimonioValor.AcceptChanges()
        Return oPatrimonioValorBE
    End Function

    Private Sub limpiarCampos()
        Me.ddlSituacion.SelectedIndex = 0
        Me.tbCodigoMnemonico.Text = ""
        Me.tbCodigoTipoInstrumento.Text = ""
        Me.tbPatrimonio.Text = ""
        Me.hd.Value = ""
        Me.tbDescMnemonico.Text = ""
        Me.tbDescTipoInstrumento.Text = ""
        Me.tbFechaVigencia.Text = ""
    End Sub

#End Region

End Class
