Imports SIT.BusinessEntities
Imports SIT.BusinessLayer
Imports System.Data.SqlTypes
Imports System.Collections
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmFactor
    Inherits BasePage

    Private operacion As String
    Private tipoFactor As String
    Private codEntidad As String
    Private codMnemonico As String
    Private oFactorBE As FactorBE

#Region "/* Métodos de la Página */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            operacion = Request.QueryString("ope")
            If (operacion = "mod") Then
                tipoFactor = Request.QueryString("tipo")
                codEntidad = Request.QueryString("codEnt")
                codMnemonico = Request.QueryString("codMne")
            End If
            If Not Page.IsPostBack Then
                Me.CargarCombos()
                If (operacion = "mod") Then
                    Me.cargarRegistro()
                End If
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                Dim arraySesiones As String() = New String(2) {}
                arraySesiones = DirectCast(Session("SS_DatosModal"), String())
                Me.tbCodigoEntidad.Text = arraySesiones(0)
                Me.tbDescEntidad.Text = arraySesiones(1)
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Private Sub ddlTipoFactor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTipoFactor.SelectedIndexChanged
        Try

        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click        
        Try
            Dim resultado As Boolean
            Me.oFactorBE = Me.obtenerInstancia()
            If Me.operacion = "reg" Then
                resultado = New FactorBM().Insertar(Me.oFactorBE, Me.DatosRequest)
                If resultado Then
                    AlertaJS("Se registro el Factor satisfactoriamente.")
                    Me.limpiarCampos()
                Else
                    AlertaJS("El Factor ingresado ya existe!!.")
                End If
            Else
                resultado = New FactorBM().Modificar(Me.oFactorBE, Me.DatosRequest)
                AlertaJS("Se modifico el Factor satisfactoriamente.")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Aceptar")
        End Try
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaFactor.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error la Retornar")
        End Try        
    End Sub

#End Region

#Region "/* Métodos Personalizados */"

    Private Sub cargarRegistro()
        Dim oTipoInstrumentoBM As New TipoInstrumentoBM
        Dim oTipoInstrumentoBE As New TipoInstrumentoBE
        Dim oRow As FactorBE.FactorRow

        oFactorBE = New FactorBM().SeleccionarPorFiltro(tipoFactor, codMnemonico, codEntidad, "", ParametrosSIT.GRUPO_FACTOR_EMISOR, Me.DatosRequest) 'CMB OT 61566
        oRow = DirectCast(oFactorBE.Factor.Rows(0), FactorBE.FactorRow)

        Me.ddlSituacion.SelectedValue = oRow.Situacion.ToString()
        Me.ddlTipoFactor.SelectedValue = oRow.TipoFactor.ToString

        Me.tbCodigoEntidad.Text = oRow.CodigoEntidad
        Me.tbDescEntidad.Text = oRow.DescripcionEntidad
        Try
            Me.tbFactor.Text = Format(oRow.Factor, "##,##0.0000000")
        Catch ex As Exception
            Me.tbFactor.Text = ""
        End Try
        Try
            Me.tbFechaVigencia.Text = UIUtility.ConvertirFechaaString(oRow.FechaVigencia)
        Catch ex As Exception
            Me.tbFechaVigencia.Text = ""
        End Try

        Me.ddlTipoFactor.Enabled = False

        Me.tbCodigoEntidad.Enabled = False
        Me.lkbBuscar.Enabled = False
    End Sub

    Private Sub CargarCombos()
        Dim tablaTipoFactor As New DataTable
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        tablaTipoFactor = oParametrosGenerales.ListarTipoFactor(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlTipoFactor, tablaTipoFactor, "Valor", "Nombre", True) 'RGF 20080806
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
    End Sub

    Private Function obtenerInstancia() As FactorBE
        Dim oFactorBE As New FactorBE
        Dim oRow As FactorBE.FactorRow
        oRow = CType(oFactorBE.Factor.NewRow(), FactorBE.FactorRow)
        oRow.TipoFactor = Me.ddlTipoFactor.SelectedValue
        oRow.Factor = IIf(Me.tbFactor.Text.Replace(".", UIUtility.DecimalSeparator()).Length = 0, 0, Convert.ToDecimal(Me.tbFactor.Text.Replace(".", UIUtility.DecimalSeparator())))
        oRow.CodigoEntidad = Me.tbCodigoEntidad.Text
        oRow.CodigoMnemonico = Nothing
        oRow.AccionesCirculacionRV = Nothing
        oRow.FloatOficioMultiple = Nothing
        oRow.MarketShare = Nothing
        oRow.Alerta = Nothing
        oRow.FechaVigencia = UIUtility.ConvertirFechaaDecimal(Me.tbFechaVigencia.Text.ToString())
        oRow.Situacion = Me.ddlSituacion.SelectedValue()
        oRow.GrupoFactor = ParametrosSIT.GRUPO_FACTOR_EMISOR
        oRow.Situacion = Me.ddlSituacion.SelectedValue()
        'FIN CMB OT 61566
        oFactorBE.Factor.AddFactorRow(oRow)
        oFactorBE.Factor.AcceptChanges()
        Return oFactorBE
    End Function

    Private Sub limpiarCampos()
        Me.ddlTipoFactor.SelectedIndex = 0
        Me.ddlSituacion.SelectedIndex = 0
        Me.tbCodigoEntidad.Text = ""
        Me.tbFactor.Text = ""
        Me.tbDescEntidad.Text = ""
        Me.tbFechaVigencia.Text = ""
    End Sub

#End Region

End Class
