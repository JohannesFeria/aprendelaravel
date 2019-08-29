Option Explicit On
Option Strict Off

Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmCuentasPorTipoInst
    Inherits BasePage


#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            CargarPagina()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Aceptar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al grabar los datos")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("frmBusquedaCuentasPorTipoInst.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al retornar la página")
        End Try
    End Sub

#End Region

#Region " /* Funciones Insertar */ "

    Private Sub Insertar()
        Dim oCuentaPorTipoInstrumentoBM As New CuentaPorTipoInstrumentoBM
        Dim oCuentaPorTipoInstrumentoBE As New CuentasPorTipoInstrumentoBE

        oCuentaPorTipoInstrumentoBE = crearObjeto()
        oCuentaPorTipoInstrumentoBM.Insertar(oCuentaPorTipoInstrumentoBE, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
        LimpiarCampos()
    End Sub

#End Region

#Region " /* Funciones Modificar */"

    Private Sub Modificar()

        Dim oCuentaPorTipoInstrumentoBM As New CuentaPorTipoInstrumentoBM
        Dim oCuentaPorTipoInstrumentoBE As New CuentasPorTipoInstrumentoBE

        oCuentaPorTipoInstrumentoBE = crearObjeto()
        oCuentaPorTipoInstrumentoBM.Modificar(oCuentaPorTipoInstrumentoBE, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            CargarCombos()
            If Not Request.QueryString("cod") Is Nothing Then

                ddlTipoInstrumento.Enabled = False
                hd.Value = Request.QueryString("cod")
                CargarRegistro(Int32.Parse(hd.Value))
            Else
                ddlTipoInstrumento.Enabled = True
                hd.Value = String.Empty
            End If
        End If
    End Sub

    Private Sub Aceptar()
        Dim blnExisteEntidad As Boolean

        If hd.Value.Equals(String.Empty) Then
            blnExisteEntidad = verificarExistencia()
            If blnExisteEntidad Then
                AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
            Else
                Insertar()
            End If
        Else
            Modificar()
        End If
    End Sub

    Private Sub CargarRegistro(ByVal Secuencial As Int32)
        Dim oCuentaPorTipoInstrumentoBM As New CuentaPorTipoInstrumentoBM
        Dim oCuentaPorTipoInstrumentoBE As New CuentasPorTipoInstrumentoBE
        Dim oRow As CuentasPorTipoInstrumentoBE.CuentasPorTipoInstrumentoRow

        oCuentaPorTipoInstrumentoBE = oCuentaPorTipoInstrumentoBM.Seleccionar(Secuencial, DatosRequest)
        oRow = DirectCast(oCuentaPorTipoInstrumentoBE.CuentasPorTipoInstrumento.Rows(0), CuentasPorTipoInstrumentoBE.CuentasPorTipoInstrumentoRow)
        ddlSituacion.SelectedValue = oRow.Situacion.ToString()

        Try
            ddlTipoInstrumento.SelectedValue = oRow.CodigoTipoInstrumentoSBS.ToString()
            LlenarComboGrupoContable()
        Catch ex As Exception
            ddlTipoInstrumento.SelectedIndex = 0
        End Try

        Try
            ddlMoneda.SelectedValue = oRow.CodigoMoneda.ToString()
        Catch ex As Exception
            ddlMoneda.SelectedIndex = 0
        End Try

        If oRow.GrupoContable.ToString <> "" Then
            ddlGrupoContable.SelectedIndex = ddlGrupoContable.Items.IndexOf(ddlGrupoContable.Items.FindByValue(oRow.GrupoContable.ToString()))
        Else
            ddlGrupoContable.SelectedIndex = 0
        End If

        ddlPortafolio.SelectedValue = oCuentaPorTipoInstrumentoBE.Tables(0).Rows(0)("CodigoPortafolio").ToString

        tbCuentaContableFondo1.Text = oRow.CuentaContable.ToString().Trim.ToUpper

        hd.Value = oRow.Secuencial.ToString()
    End Sub

    Private Function crearObjeto() As CuentasPorTipoInstrumentoBE

        Dim oCuentaPorTipoInstrumentoBE As New CuentasPorTipoInstrumentoBE
        Dim oRow As CuentasPorTipoInstrumentoBE.CuentasPorTipoInstrumentoRow

        If ddlGrupoContable.SelectedIndex = 0 Then
            AlertaJS("Ingrese Grupo Contable")
            Exit Function
        End If

        oRow = DirectCast(oCuentaPorTipoInstrumentoBE.CuentasPorTipoInstrumento.NewRow(), CuentasPorTipoInstrumentoBE.CuentasPorTipoInstrumentoRow)

        oRow.CodigoTipoInstrumentoSBS = ddlTipoInstrumento.SelectedValue()
        oRow.CodigoMoneda = ddlMoneda.SelectedValue()
        oRow.CuentaContable = tbCuentaContableFondo1.Text.ToString.Trim.ToUpper
        oRow.CodigoPortafolio = ddlPortafolio.SelectedValue
        oRow.GrupoContable = ddlGrupoContable.SelectedValue.ToString()
        oRow.Situacion = ddlSituacion.SelectedValue

        If Not hd.Value.Equals(String.Empty) Then
            oRow.Secuencial = hd.Value
        Else
            oRow.Secuencial = 0
        End If

        oCuentaPorTipoInstrumentoBE.CuentasPorTipoInstrumento.AddCuentasPorTipoInstrumentoRow(oRow)
        oCuentaPorTipoInstrumentoBE.CuentasPorTipoInstrumento.AcceptChanges()

        Return oCuentaPorTipoInstrumentoBE
    End Function

    Private Sub CargarCombos()
        Dim tablaMoneda As Data.DataTable
        Dim tablaInstrumento As Data.DataTable
        Dim tablaSituacion As DataTable


        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim oMonedaBM As New MonedaBM
        Dim oTipoInstrumentoBM As New TipoInstrumentoBM

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        tablaInstrumento = oTipoInstrumentoBM.Listar(DatosRequest).Tables(0)
        tablaMoneda = oMonedaBM.Listar(DatosRequest).Tables(0)


        'Dim tablaPortafolio As DataTable
        'tablaPortafolio = New PortafolioBM().PortafolioCodigoListar(ConfigurationManager.AppSettings("MULTIFONDO"))
        'HelpCombo.LlenarComboBox(ddlPortafolio, tablaPortafolio, "CodigoPortafolio", "Descripcion", True)

        Dim oPortafolioBM As New PortafolioBM
        ddlPortafolio.DataSource = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        ddlPortafolio.DataValueField = "CodigoPortafolio"
        ddlPortafolio.DataTextField = "Descripcion"
        ddlPortafolio.DataBind()

        HelpCombo.LlenarComboBox(ddlTipoInstrumento, tablaInstrumento, "CodigoTipoInstrumentoSBS", "CodigoMasDescripcion", True)
        HelpCombo.LlenarComboBox(ddlMoneda, tablaMoneda, "CodigoMoneda", "Descripcion", True)
        HelpCombo.LlenarComboBox(ddlSituacion, tablaSituacion, "Valor", "Nombre", False)

    End Sub

    Private Sub LimpiarCampos()
        hd.Value = Constantes.M_STR_TEXTO_INICIAL
        ddlTipoInstrumento.SelectedValue = ""
        ddlMoneda.SelectedValue = ""
        tbCuentaContableFondo1.Text = Constantes.M_STR_TEXTO_INICIAL
        ddlSituacion.SelectedValue = "A"
    End Sub

    Private Function verificarExistencia() As Boolean
        Dim oCuentaPorTipoInstrumentosBM As New CuentaPorTipoInstrumentoBM
        Dim oCuentaPorTipoInstrumentosBE As New CuentasPorTipoInstrumentoBE

        oCuentaPorTipoInstrumentosBE = oCuentaPorTipoInstrumentosBM.SeleccionarPorFiltro(ddlTipoInstrumento.SelectedValue, ddlMoneda.SelectedValue, ddlGrupoContable.SelectedValue, "A", ConfigurationManager.AppSettings("MULTIFONDO"), DatosRequest)

        If oCuentaPorTipoInstrumentosBE.CuentasPorTipoInstrumento.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

#End Region

    'RGF 20080919

    Private Sub LlenarComboGrupoContable()
        Dim DtGrupoContable As DataTable
        DtGrupoContable = New ParametrosGeneralesBM().ListarGrupoContable(ddlTipoInstrumento.SelectedValue, DatosRequest)
        HelpCombo.LlenarComboBox(ddlGrupoContable, DtGrupoContable, "Valor", "Nombre", True)
    End Sub

    Protected Sub ddlTipoInstrumento_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlTipoInstrumento.SelectedIndexChanged
        Try
            If Not ddlTipoInstrumento.SelectedValue.Equals("") Then
                LlenarComboGrupoContable()

                If ddlGrupoContable.Items.Count > 0 Then
                    ddlGrupoContable.SelectedIndex = ddlGrupoContable.Items.IndexOf(ddlGrupoContable.Items.FindByValue("PM"))
                Else
                    ddlGrupoContable.SelectedIndex = 0
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar el Tipo de Instrumento")
        End Try
    End Sub
End Class
