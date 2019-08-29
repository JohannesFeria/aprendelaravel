Option Explicit On
Option Strict Off
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports ParametrosSIT
Partial Class Modulos_Parametria_Tablas_Entidades_frmCustodios
    Inherits BasePage
#Region "/* Enumerados */"

    Public Enum TIPO_ACCION As Byte
        [NUEVO] = 1
        [MODIFICAR] = 2
    End Enum
#End Region
#Region "/* Propiedades */"

    Private Property VistaDetalleCuentas() As CustodioCuentaDepositariaBE
        Get
            Return DirectCast(ViewState("Cuentas"), CustodioCuentaDepositariaBE)
        End Get
        Set(ByVal Value As CustodioCuentaDepositariaBE)
            ViewState("Cuentas") = Value
        End Set
    End Property

#End Region
#Region " /* Metodos de Pagina */"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                ddlFisicoAnotacion.SelectedIndex = 1

                If Not Request.QueryString("cod") Is Nothing Then
                    hd.Value = Request.QueryString("cod")
                    cargarRegistro(hd.Value)
                Else
                    hd.Value = String.Empty
                    VistaDetalleCuentas = New CustodioCuentaDepositariaBE
                    divDetalle.Visible = False
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub
    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            If e.CommandName.Equals("Modificar") Then
                EditarGrilla(e.CommandArgument.ToString().Split("|")(1))
            End If
            If e.CommandName.Equals("Eliminar") Then
                EliminarFilaCuentaDepositaria(e.CommandArgument.ToString().Split("|")(1))
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la operación en la Grilla")
        End Try
    End Sub
    Private Sub btnModificarDetalle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModificarDetalle.Click
        Try
            ModificarDetalle()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al modificar el detalle")
        End Try
    End Sub
    Private Sub btnAgregarDetalle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarDetalle.Click
        Try
            If Not VerificarCustodioCuentaDepositaria(tbCodigo.Text, txtCuentaDepositaria.Text, ddlPortafolio.SelectedValue) Then
                AlertaJS("Existe Numero de Cuenta, Portafolio")
                Exit Sub
            End If
            Agregar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al agregar detalle")
        End Try
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Aceptar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al aceptar")
        End Try
    End Sub
    Private Sub btnRetornarDetalle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornarDetalle.Click
        Try
            Response.Redirect("frmBusquedaCustodios.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error el retornar")
        End Try
    End Sub
    Private Sub btnMostrarDetalle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMostrarDetalle.Click
        Try
            Ingresar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al mostrar el detalle")
        End Try
    End Sub
#End Region
#Region "*/ Funciones Personalizadas */"
    Private Sub Aceptar()
        Dim blnExisteEntidad As Boolean
        Dim blnDataCorrecta As Boolean

        If hd.Value.Equals(String.Empty) Then
            blnExisteEntidad = ExisteEntidad()

            If blnExisteEntidad Then
                AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
            Else
                blnDataCorrecta = ValidarDataEntry()

                If blnDataCorrecta Then
                    Insertar()
                    LimpiarControlesCabecera()
                    LimpiarControlesDetalle()
                    Cancelar()
                Else
                    AlertaJS("Debe ingresar al menos un registro en el detalle.")
                End If
            End If
        Else
            Modificar()
        End If
    End Sub
    Private Sub Insertar()
        Dim oCustodioBM As New CustodioBM
        Dim oCustodioBE As New CustodioBE
        oCustodioBE = crearObjeto()
        oCustodioBM.Insertar(oCustodioBE, VistaDetalleCuentas, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
    End Sub
    Private Sub Modificar()
        Dim oCustodioBM As New CustodioBM
        Dim oCustodioBE As New CustodioBE
        oCustodioBE = crearObjeto()
        oCustodioBM.Modificar(oCustodioBE, VistaDetalleCuentas, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)
    End Sub
    Private Sub cargarRegistro(ByVal codigo As String)
        Dim oCustodioBM As New CustodioBM
        Dim oCustodio As New CustodioBE
        Dim oCuentas As CustodioCuentaDepositariaBE
        Dim oRow As CustodioBE.CustodioRow
        oCustodio = oCustodioBM.Seleccionar1(codigo, String.Empty, String.Empty, DatosRequest)
        tbCodigo.Enabled = False
        oRow = DirectCast(oCustodio.Custodio.Rows(0), CustodioBE.CustodioRow)
        hd.Value = oRow.CodigoCustodio.ToString()
        tbCodigo.Text = oRow.CodigoCustodio.ToString()
        tbDescripcion.Text = oRow.Descripcion.ToString()
        ddlSituacion.SelectedValue = oRow.Situacion.ToString()
        oCuentas = oCustodioBM.SeleccionarCuentasDepositaria(codigo, DatosRequest)
        VistaDetalleCuentas = oCuentas
        CargarGrilla()
        divDetalle.Visible = True
        btnMostrarDetalle.Visible = False
        btnAgregarDetalle.Visible = False
        btnModificarDetalle.Visible = False
        DeshabilitarControlesCabecera(2)
        btnAgregarDetalle.Visible = True
    End Sub
    Private Function crearObjeto() As CustodioBE
        Dim oCustodioBE As New CustodioBE
        Dim oRow As CustodioBE.CustodioRow
        oRow = DirectCast(oCustodioBE.Custodio.NewCustodioRow(), CustodioBE.CustodioRow)
        oRow.CodigoCustodio = tbCodigo.Text.ToUpper.Trim.ToString
        oRow.Descripcion = tbDescripcion.Text.ToUpper.TrimStart.TrimEnd.ToString
        oRow.Situacion = ddlSituacion.SelectedValue
        IIf(Not hd.Value.Equals(String.Empty), oRow.CodigoCustodio = hd.Value, oRow.CodigoCustodio = tbCodigo.Text.Trim)
        oCustodioBE.Custodio.AddCustodioRow(oRow)
        oCustodioBE.Custodio.AcceptChanges()
        Return oCustodioBE
    End Function
    Private Sub CargarGrilla()
        dgLista.DataSource = VistaDetalleCuentas
        dgLista.DataBind()
    End Sub
    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
        'situacion de cuenta dep.
        HelpCombo.LlenarComboBox(ddlSituacionCuentaDep, tablaSituacion, "Valor", "Nombre", False)
        tablaSituacion = oParametrosGenerales.Listar("FisAnot", DatosRequest)
        HelpCombo.LlenarComboBox(ddlFisicoAnotacion, tablaSituacion, "Valor", "Nombre", True)
        Dim CodigoPortafolio As String = "" 'Nuevo
        Dim dtPortafolio As DataTable
        Dim oPortafolioBM As New PortafolioBM
        'CodigoPortafolio = New ParametrosGeneralesBM().SeleccionarPorFiltro(GRUPO_FONDO, MULTIFONDO, "", "", Nothing).Rows(0)("Valor").ToString.Trim
        dtPortafolio = oPortafolioBM.PortafolioCodigoListar(CodigoPortafolio)
        HelpCombo.LlenarComboBox(ddlPortafolio, dtPortafolio, "CodigoPortafolio", "Descripcion", True) 'Nuevo
    End Sub
    Private Sub LimpiarControlesCabecera()
        tbCodigo.Text = String.Empty
        tbDescripcion.Text = String.Empty
        ddlSituacion.SelectedIndex = 0
    End Sub
    Private Sub LimpiarControlesDetalle()
        txtCuentaDepositaria.Text = Constantes.M_STR_TEXTO_INICIAL
        txtNombreCuenta.Text = Constantes.M_STR_TEXTO_INICIAL
        txtObservaciones.Text = Constantes.M_STR_TEXTO_INICIAL
        ddlFisicoAnotacion.SelectedIndex = 1
        ddlPortafolio.SelectedIndex = 0
        ddlSituacionCuentaDep.SelectedIndex = 0
    End Sub
    Private Function ExisteEntidad() As Boolean
        Dim oCustodioBM As New CustodioBM
        Dim oCustodioBE As New CustodioBE
        oCustodioBE = oCustodioBM.SeleccionarPorFiltro(tbCodigo.Text.Trim, String.Empty, String.Empty, DatosRequest)
        Return oCustodioBE.Custodio.Rows.Count > 0
    End Function
    Private Function ValidarDataEntry() As Boolean
        Return VistaDetalleCuentas.CustodioCuentaDepositaria.Rows.Count > 0
    End Function
    Private Sub InsertarFilaCuentas()
        Dim oCuentas As CustodioCuentaDepositariaBE
        Dim oRow As CustodioCuentaDepositariaBE.CustodioCuentaDepositariaRow
        oCuentas = VistaDetalleCuentas
        oRow = oCuentas.CustodioCuentaDepositaria.NewCustodioCuentaDepositariaRow()
        oRow.CodigoCustodio = tbCodigo.Text.ToUpper.Trim.ToString
        oRow.CodigoCuentaDepositaria = txtCuentaDepositaria.Text
        oRow.NombreCuenta = txtNombreCuenta.Text
        If ddlFisicoAnotacion.SelectedIndex <= 0 Then
            oRow.FisicoAnotacion = ""
            oRow.NombreFisicoAnotacion = ""
        Else
            oRow.FisicoAnotacion = ddlFisicoAnotacion.SelectedValue
            oRow.NombreFisicoAnotacion = ddlFisicoAnotacion.SelectedItem.Text
        End If

        oRow.CodigoPortafolioSBS = IIf(ddlPortafolio.SelectedIndex = 0, "", ddlPortafolio.SelectedValue)
        oRow.Observaciones = txtObservaciones.Text
        oRow.Situacion = ddlSituacionCuentaDep.SelectedValue
        oRow.NombreSituacion = ddlSituacionCuentaDep.SelectedItem.Text
        oRow.Estado = TIPO_ACCION.NUEVO
        oRow.Descripcion = ddlPortafolio.SelectedItem.Text
        oCuentas.CustodioCuentaDepositaria.AddCustodioCuentaDepositariaRow(oRow)
        oCuentas.CustodioCuentaDepositaria.AcceptChanges()
        VistaDetalleCuentas = oCuentas
    End Sub
    Private Sub ModificarFilaCuentas()
        Dim oCuentas As CustodioCuentaDepositariaBE
        Dim oRow As CustodioCuentaDepositariaBE.CustodioCuentaDepositariaRow
        Dim intIndice As Integer
        oCuentas = VistaDetalleCuentas
        intIndice = Convert.ToInt32(ViewState("IndiceSeleccionado"))
        oRow = DirectCast(oCuentas.CustodioCuentaDepositaria.Rows(intIndice), CustodioCuentaDepositariaBE.CustodioCuentaDepositariaRow)
        oRow.BeginEdit()
        oRow.CodigoCustodio = tbCodigo.Text.ToUpper.Trim.ToString
        If oRow.Estado = 3 Then
            oRow.Estado = TIPO_ACCION.MODIFICAR
        End If
        oRow.CodigoCuentaDepositaria = txtCuentaDepositaria.Text
        oRow.NombreCuenta = txtNombreCuenta.Text
        If ddlFisicoAnotacion.SelectedIndex <= 0 Then
            oRow.FisicoAnotacion = ""
            oRow.NombreFisicoAnotacion = ""
        Else
            oRow.FisicoAnotacion = ddlFisicoAnotacion.SelectedValue
            oRow.NombreFisicoAnotacion = ddlFisicoAnotacion.SelectedItem.Text
        End If
        oRow.CodigoPortafolioSBS = IIf(ddlPortafolio.SelectedIndex = 0, "", ddlPortafolio.SelectedValue)
        oRow.Observaciones = txtObservaciones.Text
        oRow.Situacion = ddlSituacionCuentaDep.SelectedValue
        oRow.NombreSituacion = ddlSituacionCuentaDep.SelectedItem.Text
        oRow.EndEdit()
        oRow.AcceptChanges()
        oCuentas.CustodioCuentaDepositaria.AcceptChanges()
        VistaDetalleCuentas = oCuentas
    End Sub
    Private Sub DeshabilitarControlesCabecera(ByVal opcion As Integer)
        If opcion = 1 Then 'ingreso
            tbCodigo.Enabled = False
            tbDescripcion.Enabled = False
            ddlSituacion.Enabled = False
        ElseIf opcion = 2 Then 'modificacion
            tbCodigo.Enabled = False
            tbDescripcion.Enabled = True
            ddlSituacion.Enabled = True
        End If
    End Sub
    Private Sub HabilitarControlesCabecera()
        tbCodigo.Enabled = True
        tbDescripcion.Enabled = True
        ddlSituacion.Enabled = True
    End Sub

    Private Sub DeshabilitarControlesDetalle()
        txtCuentaDepositaria.Enabled = False
        ddlPortafolio.Enabled = False
    End Sub

    Private Sub ActualizarIndiceGrilla(ByVal nuevoIndice As Integer)
        dgLista.PageIndex = nuevoIndice
        If Not Request.QueryString("cod") Is Nothing Then
            CargarGrilla()
        Else
            hd.Value = String.Empty
        End If
    End Sub
    Private Sub HabilitarControlesDetalle()
        txtCuentaDepositaria.Enabled = True
        ddlPortafolio.Enabled = True
    End Sub
    Private Function ExisteCuenta() As Boolean
        Dim dwCuentas As DataView
        dwCuentas = VistaDetalleCuentas.CustodioCuentaDepositaria.DefaultView
        dwCuentas.RowFilter = "CodigoCustodio = '" & tbCodigo.Text & "' AND CodigoCuentaDepositaria = '" & txtCuentaDepositaria.Text & "'"
        Return dwCuentas.Count > 0
    End Function
    Private Sub Ingresar()
        Dim blnExisteEntidad As Boolean
        blnExisteEntidad = ExisteEntidad()
        If Not blnExisteEntidad Then
            divDetalle.Visible = True
            btnMostrarDetalle.Visible = False
            btnModificarDetalle.Visible = False
            DeshabilitarControlesCabecera(1)
            HabilitarControlesDetalle()
        Else
            AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
        End If
    End Sub

    Private Sub Agregar()
        InsertarFilaCuentas()
        CargarGrilla()
        LimpiarControlesDetalle()
    End Sub

    Private Sub ModificarDetalle()
        ModificarFilaCuentas()
        CargarGrilla()
        LimpiarControlesDetalle()
        btnModificarDetalle.Visible = False
        btnAgregarDetalle.Visible = True
        HabilitarControlesDetalle()

        If hd.Value.Equals(String.Empty) Then
            btnAgregarDetalle.Visible = True
        Else
            ' DeshabilitarControlesDetalle()
        End If
    End Sub

    Private Sub EditarGrilla(ByVal index As Int16)
        Dim oCuentas As CustodioCuentaDepositariaBE

        oCuentas = VistaDetalleCuentas

        btnModificarDetalle.Enabled = True
        ddlPortafolio.Enabled = False

        txtCuentaDepositaria.Text = oCuentas.CustodioCuentaDepositaria.Rows(index)("CodigoCuentaDepositaria").ToString()
        txtNombreCuenta.Text = oCuentas.CustodioCuentaDepositaria.Rows(index)("NombreCuenta").ToString()
        txtObservaciones.Text = oCuentas.CustodioCuentaDepositaria.Rows(index)("Observaciones").ToString()
        ddlFisicoAnotacion.SelectedValue = oCuentas.CustodioCuentaDepositaria.Rows(index)("FisicoAnotacion").ToString()

        ddlSituacionCuentaDep.SelectedValue = oCuentas.CustodioCuentaDepositaria.Rows(index)("Situacion").ToString()

        If oCuentas.CustodioCuentaDepositaria.Rows(index)("CodigoPortafolioSBS").ToString = "" Then
            ddlPortafolio.SelectedIndex = 0
        Else
            Try
                ddlPortafolio.SelectedValue = oCuentas.CustodioCuentaDepositaria.Rows(index)("CodigoPortafolioSBS").ToString()
            Catch ex As Exception
                ddlPortafolio.SelectedIndex = 0
            End Try
        End If

        ViewState("IndiceSeleccionado") = index

        btnModificarDetalle.Visible = True
        btnAgregarDetalle.Visible = False

    End Sub

    Private Sub EliminarFilaCuentaDepositaria(ByVal index As Int16)
        Dim oCuentas As CustodioCuentaDepositariaBE
        Dim oRow As CustodioCuentaDepositariaBE.CustodioCuentaDepositariaRow

        oCuentas = VistaDetalleCuentas
        oRow = DirectCast(oCuentas.CustodioCuentaDepositaria.Rows(index), CustodioCuentaDepositariaBE.CustodioCuentaDepositariaRow)

        oRow.BeginEdit()
        Dim estado As Integer = oRow.Estado
        If (oRow.Estado = 3) Then
            oRow.Estado = 2
        End If
        oRow.Situacion = "I"
        oRow.NombreSituacion = "Inactivo"

        oRow.EndEdit()
        oRow.AcceptChanges()
        oCuentas.CustodioCuentaDepositaria.AcceptChanges()

        VistaDetalleCuentas = oCuentas
        CargarGrilla()
    End Sub

    Private Sub Cancelar()
        Dim oCuentas As CustodioCuentaDepositariaBE

        oCuentas = VistaDetalleCuentas
        oCuentas.CustodioCuentaDepositaria.Clear()
        oCuentas.CustodioCuentaDepositaria.AcceptChanges()

        VistaDetalleCuentas = oCuentas
        CargarGrilla()

        divDetalle.Visible = False

        btnMostrarDetalle.Visible = True
        HabilitarControlesCabecera()
    End Sub

    Private Function VerificarCustodioCuentaDepositaria(ByVal strCodigoCustodio As String, ByVal strCodigoCuentaDepositaria As String, ByVal strCodigoPortafolioSBS As String) As Boolean
        Dim oCustodioBM As New CustodioBM
        Dim LngContarCuentaTerceros As Long
        LngContarCuentaTerceros = oCustodioBM.VerificarCustodioCuentaDepositaria(strCodigoCustodio, strCodigoCuentaDepositaria, strCodigoPortafolioSBS)
        If LngContarCuentaTerceros > 0 Then
            Return False
        Else
            Return True
        End If
    End Function
#End Region

    Private Sub dgLista_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        ActualizarIndiceGrilla(e.NewPageIndex)
    End Sub
End Class
