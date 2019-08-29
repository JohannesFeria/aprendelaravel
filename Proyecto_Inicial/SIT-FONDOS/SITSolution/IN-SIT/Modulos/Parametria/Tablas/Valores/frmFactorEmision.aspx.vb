Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data.SqlTypes
Imports System.Collections
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmFactorEmision
    Inherits BasePage

    Private operacion As String
    Private tipoFactor As String
    Private codEntidad As String
    Private codMnemonico As String
    Private oFactorBE As FactorBE

#Region " Eventos de la Pagina "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            operacion = Request.QueryString("ope")
            If (operacion = "mod") Then
                tipoFactor = Request.QueryString("tipo")
                codEntidad = Request.QueryString("codEnt")
                codMnemonico = Request.QueryString("codMne")
            End If
            If Not Page.IsPostBack Then
                CargarCombos()
                If (operacion = "mod") Then
                    cargarRegistro()
                End If
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                tbCodigoMnemonico.Text = CType(Session("SS_DatosModal")(0), String)
                tbDescMnemonico.Text = CType(Session("SS_DatosModal")(1), String)
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try        
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click       
        Try
            Dim resultado As Boolean
            oFactorBE = obtenerInstancia()
            If operacion = "reg" Then
                resultado = New FactorBM().Insertar(oFactorBE, DatosRequest)
                If resultado Then
                    AlertaJS("Se registro el Factor satisfactoriamente.")
                    limpiarCampos()
                Else
                    AlertaJS("El Factor ingresado ya existe!!.")
                End If
            Else
                resultado = New FactorBM().Modificar(oFactorBE, DatosRequest)
                AlertaJS("Se modifico el Factor satisfactoriamente.")
            End If
        Catch ex As Exception
            AlertaJS("Ocueerió un error al Aceptar")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("frmBusquedaFactorEmision.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar")
        End Try        
    End Sub

#End Region

#Region " Funciones Personalizadas "

    Private Sub CargarCombos()
        Dim tablaTipoFactor As New DataTable
        Dim tablaSituacion As New DataTable

        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        tablaTipoFactor = oParametrosGenerales.ListarTipoFactor(DatosRequest)

        HelpCombo.LlenarComboBox(ddlTipoFactor, tablaTipoFactor, "Valor", "Nombre", True) 'RGF 20080806
        HelpCombo.LlenarComboBox(ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
    End Sub

    Private Sub cargarRegistro()
        Dim oTipoInstrumentoBM As New TipoInstrumentoBM
        Dim oTipoInstrumentoBE As New TipoInstrumentoBE
        Dim oRow As FactorBE.FactorRow

        oFactorBE = New FactorBM().SeleccionarPorFiltro(tipoFactor, codMnemonico, codEntidad, "", ParametrosSIT.GRUPO_FACTOR_EMISION, DatosRequest)
        oRow = DirectCast(oFactorBE.Factor.Rows(0), FactorBE.FactorRow)
        tbCodigoMnemonico.Text = oRow.CodigoMnemonico.ToString()
        tbDescMnemonico.Text = oRow.DescripcionMnemonico.ToString()
        ddlSituacion.SelectedValue = oRow.Situacion.ToString()
        ddlTipoFactor.SelectedValue = oRow.TipoFactor.ToString()
        Try
            tbFactor.Text = Format(oRow.FloatOficioMultiple, "##,##0.0000000")
        Catch ex As Exception
            tbFactor.Text = ""
        End Try
        Try
            tbFechaVigencia.Text = UIUtility.ConvertirFechaaString(oRow.FechaVigencia)
        Catch ex As Exception
            tbFechaVigencia.Text = ""
        End Try
        ddlTipoFactor.Enabled = False
        tbCodigoMnemonico.Enabled = False
    End Sub

    Private Function obtenerInstancia() As FactorBE
        Dim oFactorBE As New FactorBE
        Dim oRow As FactorBE.FactorRow
        oRow = CType(oFactorBE.Factor.NewRow(), FactorBE.FactorRow)

        oRow.TipoFactor = ddlTipoFactor.SelectedValue
        oRow.CodigoMnemonico = tbCodigoMnemonico.Text.Trim
        oRow.CodigoEntidad = ""
        oRow.Factor = Nothing
        oRow.MarketShare = Nothing
        oRow.FloatOficioMultiple = IIf(tbFactor.Text.Replace(".", UIUtility.DecimalSeparator()).Length = 0, 0, Convert.ToDecimal(tbFactor.Text.Replace(".", UIUtility.DecimalSeparator())))
        oRow.AccionesCirculacionRV = Nothing
        oRow.Alerta = Nothing
        oRow.Situacion = ddlSituacion.SelectedValue()
        oRow.FechaVigencia = UIUtility.ConvertirFechaaDecimal(tbFechaVigencia.Text)
        oRow.GrupoFactor = ParametrosSIT.GRUPO_FACTOR_EMISION
        oFactorBE.Factor.AddFactorRow(oRow)
        oFactorBE.Factor.AcceptChanges()
        Return oFactorBE
    End Function

    Private Sub limpiarCampos()
        ddlTipoFactor.SelectedIndex = 0
        ddlSituacion.SelectedIndex = 0
        tbFactor.Text = ""
        tbCodigoMnemonico.Text = ""
        tbFechaVigencia.Text = ""
        tbDescMnemonico.Text = ""
    End Sub

#End Region

End Class
