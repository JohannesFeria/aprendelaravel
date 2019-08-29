Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
'--------Orden de trabajo: en OT-10795  -----------
'--------Nombre Método con cambios: crearObjetoOI()-----------
'--------Fecha de Modificación: 15/09/2017 -----------
'--------Tipo: Modificación -----------
'--------Descripción: Se válido Set al atritubo FechaLiquidación  con el valor del control tbFechaContrato cuando el control ddlOperacion tiene valor 6 -----------

Partial Class Modulos_Parametria_Tablas_Generales_frmCuentaEconomica
    Inherits BasePage
    Dim oPortafolioBM As New PortafolioBM
#Region " */ Metodos de Pagina */"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
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
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("frmBusquedaCuentasEconomicas.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cancelar la operación")
        End Try
    End Sub

    Protected Sub ddlENtidadFinanciera_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlENtidadFinanciera.SelectedIndexChanged
        CargarCuentaContable()
    End Sub

    Protected Sub ddlMoneda_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMoneda.SelectedIndexChanged
        CargarCuentaContable()
    End Sub

#End Region

#Region " */ Funciones Personalizadas */"
    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            CargarCombos()
            If Not Request.QueryString("cod") Is Nothing Then
                hd.Value = Request.QueryString("cod")
                cargarRegistro(hd.Value)
            Else
                hd.Value = String.Empty
            End If
        End If
    End Sub

    Private Sub cargarRegistro(ByVal codigo As String)
        Dim oCuentaEconomicaBM As New CuentaEconomicaBM
        Dim oCuentaEconomica As New CuentaEconomicaBE
        Dim oRow As CuentaEconomicaBE.CuentaEconomicaRow
        Dim strCodigoPortafolio, strCuentaContable, strNumeroCuenta As String

        strCodigoPortafolio = codigo.Split(","c)(0)
        strCuentaContable = codigo.Split(","c)(1)
        strNumeroCuenta = codigo.Split(","c)(2)

        oCuentaEconomica = oCuentaEconomicaBM.SeleccionarPorFiltroMant(strCodigoPortafolio, String.Empty, String.Empty, String.Empty, String.Empty, strNumeroCuenta, Me.DatosRequest)
        oRow = DirectCast(oCuentaEconomica.CuentaEconomica.Rows(0), CuentaEconomicaBE.CuentaEconomicaRow)
        txtCuentaContable.Enabled = False
        ddlPortafolio.Enabled = False
        ddlMoneda.Enabled = False
        ' Inicio de Cambio OT-10795
        tbNumeroCuenta.Enabled = False
        ddlENtidadFinanciera.Enabled = False
        ' Fin de Cambio OT-10795

        hd.Value = codigo
        txtCuentaContable.Text = oRow.CuentaContable
        tbNumeroCuenta.Text = oRow.NumeroCuenta
        tbNumeroCuentaInterbancaria.Text = oRow.CuentaInterbancaria
        Try
            ddlENtidadFinanciera.SelectedValue = oRow.EntidadFinanciera
        Catch ex As Exception
            ddlENtidadFinanciera.SelectedIndex = 0
        End Try
        Try
            ddlMoneda.SelectedValue = oRow.CodigoMoneda
        Catch ex As Exception
            ddlMoneda.SelectedIndex = 0
        End Try
        Try
            ddlClaseCuenta.SelectedValue = oRow.CodigoClaseCuenta
        Catch ex As Exception
            ddlClaseCuenta.SelectedIndex = 0
        End Try

        Try
            ddlPortafolio.SelectedValue = oRow.CodigoPortafolioSBS
        Catch ex As Exception
            ddlPortafolio.SelectedIndex = 0
        End Try
        Try
            Me.ddlMercado.SelectedValue = oRow.CodigoMercado
        Catch ex As Exception
            Me.ddlMercado.SelectedIndex = 0
        End Try
        tbTasa.Text = oRow.Tasa.ToString().Replace(UIUtility.DecimalSeparator(), ".")
        tbComision.Text = oRow.Comision.ToString().Replace(UIUtility.DecimalSeparator(), ".")
        tbPortes.Text = oRow.Portes.ToString().Replace(UIUtility.DecimalSeparator(), ".")
        ddlSituacion.SelectedValue = oRow.Situacion
        'ini HDG OT 63380 20110905
        If Not oRow.IsNroDocumentoNull Then
            tbNroDoc.Text = oRow.NroDocumento.ToString
        End If
        'fin HDG OT 63380 20110905
    End Sub

    Private Function crearObjeto() As CuentaEconomicaBE
        Dim oCuentaEconomicaBE As New CuentaEconomicaBE
        Dim oRow As CuentaEconomicaBE.CuentaEconomicaRow

        oRow = DirectCast(oCuentaEconomicaBE.CuentaEconomica.NewCuentaEconomicaRow(), CuentaEconomicaBE.CuentaEconomicaRow)

        oRow.CuentaContable = txtCuentaContable.Text.ToUpper.Trim.ToString
        oRow.CodigoPortafolioSBS = ddlPortafolio.SelectedValue.ToString
        oRow.EntidadFinanciera = Me.ddlENtidadFinanciera.SelectedValue
        oRow.CodigoTercero = String.Empty
        oRow.NumeroCuenta = tbNumeroCuenta.Text.Trim
        oRow.Descripcion = ""
        oRow.TipoCuenta = String.Empty

        If tbNumeroCuentaInterbancaria.Text.Trim.Equals(String.Empty) Then
            oRow.CuentaInterbancaria = 0
        Else
            oRow.CuentaInterbancaria = tbNumeroCuentaInterbancaria.Text.Trim
        End If

        oRow.CodigoMoneda = Me.ddlMoneda.SelectedValue.ToString
        oRow.CodigoClaseCuenta = ddlClaseCuenta.SelectedValue.ToString
        oRow.CodigoMercado = ddlMercado.SelectedValue.ToString
        oRow.Tasa = IIf(tbTasa.Text.ToString.Trim <> "", Me.tbTasa.Text.Replace(".", UIUtility.DecimalSeparator()), 0)
        oRow.Portes = IIf(tbPortes.Text.ToString.Trim <> "", Me.tbPortes.Text.Replace(".", UIUtility.DecimalSeparator()), 0)
        oRow.Comision = IIf(tbComision.Text.ToString.Trim <> "", Me.tbComision.Text.Replace(".", UIUtility.DecimalSeparator()), 0)
        oRow.Situacion = Me.ddlSituacion.SelectedValue
        oRow.NroDocumento = tbNroDoc.Text.Trim.ToString 'HDG OT 63380 20110905

        oCuentaEconomicaBE.CuentaEconomica.AddCuentaEconomicaRow(oRow)
        oCuentaEconomicaBE.CuentaEconomica.AcceptChanges()

        Return oCuentaEconomicaBE
    End Function

    Private Sub CargarCombos()

        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)

        Dim tablaMoneda As New Data.DataTable
        Dim oMoneda As New MonedaBM
        tablaMoneda = oMoneda.Listar("A").Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlMoneda, tablaMoneda, "CodigoMoneda", "CodigoMoneda", True)

        Dim tablaPortafolio As New Data.DataTable
        Dim oPortafolio As New PortafolioBM
        tablaPortafolio = oPortafolio.Listar(DatosRequest, "A").Tables(0)
        'HelpCombo.LlenarComboBox(Me.ddlPortafolio, tablaPortafolio, "CodigoPortafolioSBS", "CodigoPortafolioSBS", True) '20140909 Se desea obtener la descripcion
        'HelpCombo.LlenarComboBox(Me.ddlPortafolio, tablaPortafolio, "CodigoPortafolioSBS", "Descripcion", True)

        ddlPortafolio.DataSource = oPortafolioBM.ObtenerDatosPortafolio(DatosRequest, "A")
        ddlPortafolio.DataValueField = "CodigoPortafolioSBS"
        ddlPortafolio.DataTextField = "Descripcion"
        ddlPortafolio.DataBind()
        ddlPortafolio.Items.Insert(0, "--SELECCIONE--")

        Dim tablaClaseCuenta As New Data.DataTable
        Dim oClaseCuenta As New ClaseCuentaBM
        tablaClaseCuenta = oClaseCuenta.Listar(DatosRequest).Tables(0)

        HelpCombo.LlenarComboBox(Me.ddlClaseCuenta, tablaClaseCuenta, "CodigoClaseCuenta", "Descripcion", True)

        Dim tablaMercado As New Data.DataTable
        Dim oMercado As New MercadoBM
        tablaMercado = oMercado.ListarActivos(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlMercado, tablaMercado, "CodigoMercado", "Descripcion", True)

        Dim dtEntidad As DataTable
        Dim oEntidadBM As New EntidadBM
        dtEntidad = oEntidadBM.ListarEntidadFinanciera(Me.DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlENtidadFinanciera, dtEntidad, "CodigoEntidad", "NombreCompleto", True)
    End Sub

    Private Sub LimpiarCampos()
        txtCuentaContable.Text = Constantes.M_STR_TEXTO_INICIAL
        tbComision.Text = "0"
        tbNumeroCuenta.Text = Constantes.M_STR_TEXTO_INICIAL
        tbNumeroCuentaInterbancaria.Text = Constantes.M_STR_TEXTO_INICIAL
        tbPortes.Text = Constantes.M_STR_TEXTO_INICIAL
        tbTasa.Text = Constantes.M_STR_TEXTO_INICIAL
        ddlENtidadFinanciera.SelectedIndex = 0
        ddlClaseCuenta.SelectedIndex = 0
        ddlMercado.SelectedIndex = 0
        ddlMoneda.SelectedIndex = 0
        ddlPortafolio.SelectedIndex = 0
        ddlSituacion.SelectedIndex = 0
    End Sub
    Private Sub ExisteRegistro(ByRef Existe As Boolean, ByRef Situacion As String)
        Dim oCuentaEconomicaBM As New CuentaEconomicaBM
        Situacion = oCuentaEconomicaBM.ObtenerSituacion(Me.ddlPortafolio.SelectedValue, _
                                                        txtCuentaContable.Text, _
                                                        ddlENtidadFinanciera.SelectedValue, _
                                                        ddlMoneda.SelectedValue)
        Existe = IIf(Situacion = "", False, True)
    End Sub
    Private Sub Aceptar()
        If hd.Value.Equals(String.Empty) Then
            'Dim blnExisteEntidad As Boolean
            'Dim Situacion As String = String.Empty
            'ExisteRegistro(blnExisteEntidad, Situacion)
            'If blnExisteEntidad Then
            '    If Situacion = "A" Then
            '        AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
            '    Else
            '        AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE & ", está inactivo")
            '    End If
            'Else
            '    Insertar()
            'End If
            Insertar()
        Else
            Modificar()
        End If
    End Sub

    Private Sub Insertar()
        Dim oCuentaEconomicaBM As New CuentaEconomicaBM
        Dim oCuentaEconomicaBE As New CuentaEconomicaBE
        oCuentaEconomicaBE = crearObjeto()
        'Inicio OT-10795
        Dim rp As String = String.Empty
        rp = oCuentaEconomicaBM.Insertar(oCuentaEconomicaBE, DatosRequest)
        If rp = "1" Then
            AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
        Else
            AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
        End If 'Fin OT-10795
        LimpiarCampos()
    End Sub

    Private Sub Modificar()
        Dim oCuentaEconomicaBM As New CuentaEconomicaBM
        Dim oCuentaEconomicaBE As New CuentaEconomicaBE
        oCuentaEconomicaBE = crearObjeto()
        If Request.QueryString("cod") IsNot Nothing Then
            Dim codigo, strCodigoPortafolio, strCuentaContable, strNumeroCuenta As String
            codigo = Request.QueryString("cod")
            strCodigoPortafolio = codigo.Split(","c)(0)
            strCuentaContable = codigo.Split(","c)(1)
            strNumeroCuenta = codigo.Split(","c)(2)
            oCuentaEconomicaBM.Modificar(oCuentaEconomicaBE, strCodigoPortafolio, strCuentaContable, strNumeroCuenta, DatosRequest)
        Else
            oCuentaEconomicaBM.Modificar(oCuentaEconomicaBE, DatosRequest)
        End If
        AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)
    End Sub

    Private Sub CargarCuentaContable()
        Dim CuentaEconomicaBM As New CuentaEconomicaBM
        Dim CuentaContable As String = String.Empty
        If (ddlENtidadFinanciera.SelectedValue = "--SELECCIONE--") Then
            AlertaJS("Seleccione el Banco")
            Exit Sub
        End If
        If (ddlMoneda.SelectedValue = "") Then
            AlertaJS("Seleccione el Moneda")
            Exit Sub
        End If
        CuentaContable = CuentaEconomicaBM.ObtenerCuentaContable(ddlENtidadFinanciera.SelectedValue, ddlMoneda.SelectedValue)
        txtCuentaContable.Text = CuentaContable
    End Sub

#End Region


End Class
