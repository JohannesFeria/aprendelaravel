Option Explicit On
Option Strict Off

Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmTiposTitulos
    Inherits BasePage


#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            CargarPagina()
        Catch ex As Exception
            Dim str As String = String.Empty
            Replace(str, Chr(13), "\n")
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

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("frmBusquedaTiposTitulos.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al retornar la página")
        End Try
    End Sub

    Private Sub ddlTipoRenta_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoRenta.SelectedIndexChanged
        Try
            If ddlTipoRenta.SelectedValue.ToString = "1" Then
                ddlBaseTir.Enabled = True
                ddlDiasBaseTir.Enabled = True
                ddlBaseCupon.Enabled = True
                ddlDiasBaseCupon.Enabled = True
                ddlPeriodicidad.Enabled = True
                ddlAmortizacion.Enabled = True
                ddlCupon.Enabled = True
                hdTipoRenta.Value = "SI"
            Else
                ddlBaseTir.Enabled = False
                ddlDiasBaseTir.Enabled = False
                ddlBaseCupon.Enabled = False
                ddlDiasBaseCupon.Enabled = False
                ddlPeriodicidad.Enabled = False
                ddlAmortizacion.Enabled = False
                ddlCupon.Enabled = False
                hdTipoRenta.Value = "NO"
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al seleccionar el Tipo de Renta")
        End Try
    End Sub

#End Region

#Region " /* Funciones Insertar */ "

    Private Sub Insertar()
        Dim oTipoTituloBM As New TipoTituloBM
        Dim oTipoTituloBE As New TipoTituloBE

        oTipoTituloBE = crearObjeto()
        oTipoTituloBM.Insertar(oTipoTituloBE, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
        LimpiarCampos()
    End Sub

#End Region

#Region " /* Funciones Modificar */"
    Private Sub Modificar()
        Dim oTipoTituloBM As New TipoTituloBM
        Dim oTipoTituloBE As New TipoTituloBE

        oTipoTituloBE = crearObjeto()
        oTipoTituloBM.Modificar(oTipoTituloBE, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)
    End Sub

#End Region

#Region " /* Funciones Personalizadas */"

    Private Sub Aceptar()
        Dim blnExisteEntidad As Boolean

        If hd.Value.Equals(String.Empty) Then
            blnExisteEntidad = verificarExistencia()
            If Not blnExisteEntidad Then
                Insertar()
                LimpiarCampos()
            Else
                AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
            End If
        Else
            Modificar()
        End If
    End Sub

    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            CargarCombos()
            If Not Request.QueryString("cod") Is Nothing Then
                hd.Value = Request.QueryString("cod")
                CargarRegistro(hd.Value)
                tbCodigo.Enabled = False
            Else
                tbCodigo.Enabled = True
                hd.Value = String.Empty
            End If
        End If
    End Sub

    Private Sub CargarRegistro(ByVal Codigo As String)
        Dim oTipoTituloBM As New TipoTituloBM
        Dim oTipoTituloBE As New TipoTituloBE
        Dim oRow As TipoTituloBE.TipoTituloRow

        oTipoTituloBE = oTipoTituloBM.Seleccionar(Codigo, DatosRequest)

        If oTipoTituloBE.TipoTitulo.Rows.Count = 0 Then
            Exit Sub
        End If

        oRow = DirectCast(oTipoTituloBE.TipoTitulo.Rows(0), TipoTituloBE.TipoTituloRow)
        ddlSituacion.SelectedValue = oRow.Situacion.ToString()

        If oRow.CodigoPeriodicidad.Equals(String.Empty) Then
            ddlPeriodicidad.SelectedIndex = 0
        Else
            ddlPeriodicidad.SelectedValue = oRow.CodigoPeriodicidad
        End If

        If oRow.CodigoTipoRenta.Equals(String.Empty) Then
            ddlTipoRenta.SelectedIndex = 0
        Else
            ddlTipoRenta.SelectedValue = oRow.CodigoTipoRenta
            If (ddlTipoRenta.SelectedValue.ToString = "2") Then
                ddlBaseTir.Enabled = False
                ddlDiasBaseTir.Enabled = False
                ddlbaseCupon.Enabled = False
                ddlDiasBaseCupon.Enabled = False
                ddlPeriodicidad.Enabled = False
                hdTipoRenta.Value = "NO"
            Else
                hdTipoRenta.Value = "SI"
            End If
        End If

        If oRow.CodigoIndicadorVAC.Equals(String.Empty) Then
            ddlIndicadorVAC.SelectedIndex = 0
        Else
            ddlIndicadorVAC.SelectedValue = oRow.CodigoIndicadorVAC
        End If

        tbDescripcion.Text = oRow.Descripcion.ToString().Trim.ToUpper
        tbCodigo.Text = oRow.CodigoTipoTitulo.ToString()

        Try
            If oRow.CodigoTipoInstrumentoSBS.Equals(String.Empty) Then
                ddlTipoInstrumento.SelectedIndex = 0
            Else
                ddlTipoInstrumento.SelectedValue = oRow.CodigoTipoInstrumentoSBS.ToString()
            End If
        Catch ex As Exception
            ddlTipoInstrumento.SelectedIndex = 0
        End Try

        Try
            ddlMoneda.SelectedValue = oRow.CodigoMoneda.ToString()
        Catch ex As Exception
            ddlMoneda.SelectedIndex = 0
        End Try

        If oRow.CodigoTipoAmortizacion.Equals(String.Empty) Then
            ddlAmortizacion.SelectedIndex = 0
        Else
            ddlAmortizacion.SelectedValue = oRow.CodigoTipoAmortizacion.ToString()
        End If

        Try
            ddlCupon.SelectedValue = oRow.CodigoTipoCupon.ToString()
        Catch ex As Exception
            ddlCupon.SelectedIndex = 0
        End Try

        txtTasaSpread.Text = oRow.TasaSpread.ToString().Replace(UIUtility.DecimalSeparator(), ".")

        If oRow.BaseTir = 0 Then
            ddlBaseTir.SelectedIndex = 0
        Else
            ddlBaseTir.SelectedValue = oRow.BaseTir.ToString().Replace(UIUtility.DecimalSeparator(), ".")
        End If
        If oRow.BaseTir = 0 Then
            ddlDiasBaseTir.SelectedIndex = 0
        Else
            ddlDiasBaseTir.SelectedValue = oRow.BaseTirDias.ToString().Replace(UIUtility.DecimalSeparator(), ".")
        End If
        If oRow.BaseTir = 0 Then
            ddlbaseCupon.SelectedIndex = 0
        Else
            ddlbaseCupon.SelectedValue = oRow.BaseCupon.ToString().Replace(UIUtility.DecimalSeparator(), ".")
        End If
        If oRow.BaseTir = 0 Then
            ddlDiasBaseCupon.SelectedIndex = 0
        Else
            ddlDiasBaseCupon.SelectedValue = oRow.BaseCuponDias.ToString().Replace(UIUtility.DecimalSeparator(), ".")
        End If

        txtObservaciones.Text = oRow.Observacion.ToString()
        hd.Value = oRow.CodigoTipoTitulo.ToString()
    End Sub

    Private Function crearObjeto() As TipoTituloBE
        Dim oTipoTituloBE As New TipoTituloBE
        Dim oRow As TipoTituloBE.TipoTituloRow

        oRow = DirectCast(oTipoTituloBE.TipoTitulo.NewRow(), TipoTituloBE.TipoTituloRow)
        oRow.Situacion = ddlSituacion.SelectedValue()
        oRow.Descripcion = tbDescripcion.Text.ToString.Trim.ToUpper
        oRow.CodigoTipoInstrumentoSBS = ddlTipoInstrumento.SelectedValue
        oRow.CodigoMoneda = ddlMoneda.SelectedValue
        oRow.CodigoTipoRenta = ddlTipoRenta.SelectedValue

        oRow.CodigoPeriodicidad = IIf(ddlPeriodicidad.SelectedValue.Equals(Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO), String.Empty, ddlPeriodicidad.SelectedValue).ToString()

        oRow.CodigoIndicadorVAC = IIf(ddlIndicadorVAC.SelectedValue.Equals(Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO), String.Empty, ddlIndicadorVAC.SelectedValue).ToString()
        oRow.CodigoTipoAmortizacion = IIf(ddlAmortizacion.SelectedValue.Equals(Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO), String.Empty, ddlAmortizacion.SelectedValue).ToString()
        oRow.CodigoTipoCupon = IIf(ddlCupon.SelectedValue.Equals(Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO), String.Empty, ddlCupon.SelectedValue).ToString()

        oRow.TasaSpread = IIf(txtTasaSpread.Text.Length = 0, 0, txtTasaSpread.Text.Replace(".", UIUtility.DecimalSeparator()))

        If (hdTipoRenta.Value = "SI") Then 'no es tipo renta variable
            oRow.BaseTir = ddlBaseTir.SelectedValue.ToString().Replace(".", UIUtility.DecimalSeparator())
            oRow.BaseTirDias = ddlDiasBaseTir.SelectedValue.ToString().Replace(".", UIUtility.DecimalSeparator())
            oRow.BaseCupon = ddlbaseCupon.SelectedValue.ToString().Replace(".", UIUtility.DecimalSeparator())
            oRow.BaseCuponDias = ddlDiasBaseCupon.SelectedValue.ToString().Replace(".", UIUtility.DecimalSeparator())
        Else 'si es tipo renta variable
            oRow.BaseTir = 0 'ddlBaseTir.SelectedValue.ToString().Replace(".", UIUtility.DecimalSeparator())
            oRow.BaseTirDias = 0 'ddlDiasBaseTir.SelectedValue.ToString().Replace(".", UIUtility.DecimalSeparator())
            oRow.BaseCupon = 0 'ddlBaseCupon.SelectedValue.ToString().Replace(".", UIUtility.DecimalSeparator())
            oRow.BaseCuponDias = 0 'ddlDiasBaseCupon.SelectedValue.ToString().Replace(".", UIUtility.DecimalSeparator())
        End If

        oRow.Observacion = txtObservaciones.Text.ToUpper

        If Not hd.Value.Equals(String.Empty) Then
            oRow.CodigoTipoTitulo = hd.Value
        Else
            oRow.CodigoTipoTitulo = tbCodigo.Text.ToString.Trim.ToUpper
        End If

        oTipoTituloBE.TipoTitulo.AddTipoTituloRow(oRow)
        oTipoTituloBE.TipoTitulo.AcceptChanges()

        Return oTipoTituloBE
    End Function

    Private Sub CargarCombos()
        Dim tablaTipoInstrumento As New Data.DataTable
        Dim oTipoInstrumentoBM As New TipoInstrumentoBM

        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM

        Dim dtMoneda As DataTable
        Dim oMonedaBM As New MonedaBM

        Dim dtAmortizacion As DataTable
        Dim oAmorticacionBM As New TipoAmortizacionBM

        Dim dtCupon As DataTable
        Dim oCuponBM As New TipoCuponBM

        Dim dtPeriodicidad As DataTable
        Dim oPeriodicidadBM As New PeriodicidadBM

        Dim dtTipoRenta As DataTable
        Dim oTipoRentaBM As New TipoRentaBM

        Dim dtIndicador As DataTable
        Dim oIndicadorBM As New IndicadorBM

        tablaTipoInstrumento = oTipoInstrumentoBM.Listar(DatosRequest).Tables(0)
        'HelpCombo.LlenarComboBox(ddlTipoInstrumento, tablaTipoInstrumento, "CodigoTipoInstrumentoSBS", "Descripcion", True)
        ddlTipoInstrumento.Items.Clear()
        ddlTipoInstrumento.DataSource = tablaTipoInstrumento
        ddlTipoInstrumento.DataValueField = "CodigoTipoInstrumentoSBS"
        ddlTipoInstrumento.DataTextField = "DescripcionCompleta"
        ddlTipoInstrumento.DataBind()
        ddlTipoInstrumento.Items.Insert(0, New ListItem("--Seleccione--"))


        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, tablaSituacion, "Valor", "Nombre", False)

        dtMoneda = oMonedaBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlMoneda, dtMoneda, "CodigoMoneda", "Descripcion", True)

        dtAmortizacion = oAmorticacionBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlAmortizacion, dtAmortizacion, "CodigoTipoAmortizacion", "Descripcion", True)

        dtCupon = oCuponBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlCupon, dtCupon, "CodigoTipoCupon", "Descripcion", True)

        dtPeriodicidad = oPeriodicidadBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlPeriodicidad, dtPeriodicidad, "CodigoPeriodicidad", "Descripcion", True)

        dtTipoRenta = oTipoRentaBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(ddlTipoRenta, dtTipoRenta, "CodigoRenta", "Descripcion", True)

        tablaSituacion = oParametrosGenerales.ListarBaseTir(DatosRequest)
        HelpCombo.LlenarComboBox(ddlBaseTir, tablaSituacion, "Valor", "Nombre", True)

        tablaSituacion = oParametrosGenerales.ListarBaseTirNDias(DatosRequest)
        HelpCombo.LlenarComboBox(ddlDiasBaseTir, tablaSituacion, "Valor", "Nombre", True)

        tablaSituacion = oParametrosGenerales.ListarBaseCupon(DatosRequest)
        HelpCombo.LlenarComboBox(ddlbaseCupon, tablaSituacion, "Valor", "Nombre", True)

        tablaSituacion = oParametrosGenerales.ListarBaseCuponNDias(DatosRequest)
        HelpCombo.LlenarComboBox(ddlDiasBaseCupon, tablaSituacion, "Valor", "Nombre", True)

        dtIndicador = oIndicadorBM.Listar(DatosRequest).Indicador
        HelpCombo.LlenarComboBox(ddlIndicadorVAC, dtIndicador, "CodigoIndicador", "NombreIndicador", True)
    End Sub

    Private Sub LimpiarCampos()
        tbCodigo.Text = Constantes.M_STR_TEXTO_INICIAL
        tbDescripcion.Text = Constantes.M_STR_TEXTO_INICIAL
        txtObservaciones.Text = Constantes.M_STR_TEXTO_INICIAL
        txtTasaSpread.Text = Constantes.M_STR_TEXTO_INICIAL
        ddlAmortizacion.SelectedIndex = 0
        ddlbaseCupon.SelectedIndex = 0
        ddlBaseTir.SelectedIndex = 0
        ddlIndicadorVAC.SelectedIndex = 0
        ddlCupon.SelectedIndex = 0
        ddlDiasBaseCupon.SelectedIndex = 0
        ddlDiasBaseTir.SelectedIndex = 0
        ddlMoneda.SelectedIndex = 0
        ddlPeriodicidad.SelectedIndex = 0
        ddlSituacion.SelectedIndex = 0
        ddlTipoInstrumento.SelectedIndex = 0
        ddlTipoRenta.SelectedIndex = 0

        ddlTipoInstrumento.SelectedValue = "--Seleccione--"
        ddlSituacion.SelectedValue = "A"

        ddlBaseTir.Enabled = True
        ddlDiasBaseTir.Enabled = True
        ddlbaseCupon.Enabled = True
        ddlDiasBaseCupon.Enabled = True
        ddlPeriodicidad.Enabled = True
        hdTipoRenta.Value = "SI"
    End Sub

    Private Function verificarExistencia() As Boolean
        Dim oTipoTituloBM As New TipoTituloBM
        Dim oTipoTituloBE As New TipoTituloBE

        oTipoTituloBE = oTipoTituloBM.Seleccionar(tbCodigo.Text, DatosRequest)

        Return oTipoTituloBE.TipoTitulo.Rows.Count > 0
    End Function

#End Region
End Class
