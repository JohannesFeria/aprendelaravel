Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
'--------Orden de trabajo: en OT-10795  -----------
'--------Método creados: Page_Load(), CargarPagina(), CargarCombos(), cargarRegistro(), btnAceptar_Click(), btnCancelar_Click(),
'  Aceptar(), ExisteRegistro(), CambiarCuenta(), crearObjeto() -----------
'--------Fecha de Modificación: 15/09/2017 -----------
'--------Tipo: Modificación -----------
'--------Descripción: Se Generó el todo el formulario -----------

Partial Class Modulos_Parametria_Tablas_Generales_frmCambiarCuentaEconomica
    'Inherits System.Web.UI.Page
    Inherits BasePage
    Dim oPortafolioBM As New PortafolioBM
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        Try
            CargarPagina()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

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

    Private Sub CargarCombos()

        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM

        Dim tablaMoneda As New Data.DataTable
        Dim oMoneda As New MonedaBM
        tablaMoneda = oMoneda.Listar("A").Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlMoneda, tablaMoneda, "CodigoMoneda", "CodigoMoneda", True)

        Dim tablaPortafolio As New Data.DataTable
        Dim oPortafolio As New PortafolioBM
        tablaPortafolio = oPortafolio.Listar(DatosRequest, "A").Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlPortafolio, tablaPortafolio, "CodigoPortafolioSBS", "CodigoPortafolioSBS", True) '20140909 Se desea obtener la descripcion
        HelpCombo.LlenarComboBox(Me.ddlPortafolio, tablaPortafolio, "CodigoPortafolioSBS", "Descripcion", True)

        ddlPortafolio.DataSource = oPortafolioBM.ObtenerDatosPortafolio(DatosRequest, "A")
        ddlPortafolio.DataValueField = "CodigoPortafolioSBS"
        ddlPortafolio.DataTextField = "Descripcion"
        ddlPortafolio.DataBind()
        ddlPortafolio.Items.Insert(0, "--SELECCIONE--")

        Dim dtEntidad As DataTable
        Dim oEntidadBM As New EntidadBM
        dtEntidad = oEntidadBM.ListarEntidadFinanciera(Me.DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlENtidadFinanciera, dtEntidad, "CodigoEntidad", "NombreCompleto", True)
    End Sub

    Private Sub cargarRegistro(ByVal codigo As String)
        Dim oCuentaEconomicaBM As New CuentaEconomicaBM
        Dim oCuentaEconomica As New CuentaEconomicaBE
        Dim oRow As CuentaEconomicaBE.CuentaEconomicaRow
        Dim strCodigoPortafolio, strCuentaContable, strNumeroCuenta, strMoneda, strEntidadFinanciera As String

        strCodigoPortafolio = codigo.Split(","c)(0)
        strCuentaContable = codigo.Split(","c)(1)
        strNumeroCuenta = codigo.Split(","c)(2)
        strMoneda = codigo.Split(","c)(3)
        strEntidadFinanciera = codigo.Split(","c)(4)

        oCuentaEconomica = oCuentaEconomicaBM.SeleccionarPorFiltroMant(strCodigoPortafolio, String.Empty, String.Empty, String.Empty, String.Empty, strNumeroCuenta, Me.DatosRequest)
        oRow = DirectCast(oCuentaEconomica.CuentaEconomica.Rows(0), CuentaEconomicaBE.CuentaEconomicaRow)
        txtCuentaContable.Enabled = False
        ddlPortafolio.Enabled = False
        ddlMoneda.Enabled = False
        tbNumeroCuenta.Enabled = False
        ddlENtidadFinanciera.Enabled = False

        hd.Value = codigo
        txtCuentaContable.Text = oRow.CuentaContable
        tbNumeroCuenta.Text = oRow.NumeroCuenta
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
            ddlPortafolio.SelectedValue = oRow.CodigoPortafolioSBS
        Catch ex As Exception
            ddlPortafolio.SelectedIndex = 0
        End Try

    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Aceptar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("frmBusquedaCuentasEconomicas.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cancelar la operación")
        End Try
    End Sub


    Private Sub Aceptar()
        If hd.Value.Equals(String.Empty) Then
            Dim blnExisteEntidad As Boolean
            Dim Situacion As String = String.Empty
            ExisteRegistro(blnExisteEntidad, Situacion)
            If blnExisteEntidad Then
                If Situacion = "A" Then
                    AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
                Else
                    AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE & ", está inactivo")
                End If
            Else
                CambiarCuenta()
            End If
        Else
            CambiarCuenta()
        End If
    End Sub

    Private Sub ExisteRegistro(ByRef Existe As Boolean, ByRef Situacion As String)
        Dim oCuentaEconomicaBM As New CuentaEconomicaBM
        Situacion = oCuentaEconomicaBM.ObtenerSituacionNuevo(Me.ddlPortafolio.SelectedValue, _
                                                        txtCuentaContable.Text, _
                                                        ddlENtidadFinanciera.SelectedValue, _
                                                        ddlMoneda.SelectedValue,
                                                        tbNuevoNumeroCuenta.Text)
        Existe = IIf(Situacion = "", False, True)
    End Sub

    Private Sub CambiarCuenta()
        Dim oCuentaEconomicaBM As New CuentaEconomicaBM
        Dim oCuentaEconomicaBE As New CuentaEconomicaBE

        oCuentaEconomicaBE = crearObjeto()
        oCuentaEconomicaBM.CambiarCuenta(oCuentaEconomicaBE, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
        LimpiarCampos()
    End Sub

    Private Function crearObjeto() As CuentaEconomicaBE
        Dim oCuentaEconomicaBE As New CuentaEconomicaBE
        Dim oRow As CuentaEconomicaBE.CuentaEconomicaRow

        oRow = DirectCast(oCuentaEconomicaBE.CuentaEconomica.NewCuentaEconomicaRow(), CuentaEconomicaBE.CuentaEconomicaRow)

        oRow.CuentaContable = txtCuentaContable.Text.ToUpper.Trim.ToString
        oRow.CodigoPortafolioSBS = ddlPortafolio.SelectedValue.ToString
        oRow.EntidadFinanciera = Me.ddlENtidadFinanciera.SelectedValue
        'oRow.CodigoTercero = String.Empty
        oRow.NumeroCuenta = tbNumeroCuenta.Text.Trim
        oRow.NuevoNumeroCuenta = tbNuevoNumeroCuenta.Text.Trim
        oRow.MotivoCambio = tbMotivoCambio.Text.Trim
        oRow.Descripcion = ""
        'oRow.TipoCuenta = String.Empty

        oRow.CodigoMoneda = Me.ddlMoneda.SelectedValue.ToString
        oCuentaEconomicaBE.CuentaEconomica.AddCuentaEconomicaRow(oRow)
        oCuentaEconomicaBE.CuentaEconomica.AcceptChanges()

        Return oCuentaEconomicaBE
    End Function



    Private Sub LimpiarCampos()
        tbNuevoNumeroCuenta.Text = ""
        tbMotivoCambio.Text = ""
        btnAceptar.Enabled = False
    End Sub

End Class