Imports SIT.BusinessLayer
Imports SIT.BusinessEntities
Imports System.Data
Imports System.Text
Partial Class Modulos_ValorizacionCustodia_Valorizacion_frmIngresoPrecios
    Inherits BasePage
#Region " /* Declaración Variables */ "
    Dim oUIUtil As New UIUtility
    Dim oVectorPrecioBM As New VectorPrecioBM
    Dim oVectorPrecioBE As VectorPrecioBE
    Dim oUtil As New UtilDM
    Private campos() As String = {"Descripción del Valor", "Precio Anterior", "Precio Actual"}
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                'ViewState("vsFuenteInformacion") = "Real"
                tbFechaOperacion.Text = oUtil.RetornarFechaSistema
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                txtMnemonico.Text = CType(Session("SS_DatosModal"), String())(1).ToString()
                txtISIN.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                txtSBS.Text = CType(Session("SS_DatosModal"), String())(2).ToString()
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub
    Private Function GetISIN() As String
        Return txtISIN.Text
    End Function
    Private Function GetSBS() As String
        Return txtSBS.Text
    End Function
    Private Function GetMNEMONICO() As String
        Return txtMnemonico.Text
    End Function
    Public Sub SeleccionarSBS(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim strcadena As String
            Dim strcodSBS As String
            strcadena = e.CommandArgument
            strcodSBS = strcadena.Split(",").GetValue(0)
            ViewState("vscodSBS") = strcodSBS
        Catch ex As Exception
            AlertaJS("Ocurrió un error al seleccionar el código SBS")
        End Try
    End Sub
    Public Sub LimpiarCampos()
        Me.txtMnemonico.Text = String.Empty
        Me.txtISIN.Text = String.Empty
        Me.txtPrecioT_1.Text = String.Empty
        Me.txtSBS.Text = String.Empty
        Me.txtPrecioSucio.Text = String.Empty
        Me.txtPorcPrecioLimpio.Text = String.Empty
        Me.txtPorcPrecioSucio.Text = String.Empty
        Me.ddlFuenteVecPre.SelectedValue = ""
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim oVectorPrecioBM As New VectorPrecioBM
        Try
            Dim dtblDatos As DataTable
            dtblDatos = New VectorPrecioBM().ListarRango(UIUtility.ConvertirFechaaDecimal(Me.tbFechaOperacion.Text), _
            UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), txtMnemonico.Text, txtISIN.Text, ddlFuenteVecPre.SelectedValue, DatosRequest).Tables(0)
            If (dtblDatos.Rows.Count >= 1) Then
                AlertaJS("Precio ya ingresado para la fecha y fuente seleccionada.")
                Exit Sub
            End If
            'OT 10238 - 18/04/2017 - Carlos Espejo
            'Descripcion: Se agrega el precio sucio
            oVectorPrecioBM.IngresarNuevo(Me.txtMnemonico.Text, UIUtility.ConvertirFechaaDecimal(Me.tbFechaOperacion.Text), ddlFuenteVecPre.SelectedValue, _
            Convert.ToDecimal(txtPrecioT_1.Text), CDec(txtPrecioSucio.Text), Decimal.Parse(txtPorcPrecioLimpio.Text), Decimal.Parse(txtPorcPrecioSucio.Text), DatosRequest, "S")
            'OT 10238 Fin
            AlertaJS("Precio ingresado satisfactoriamente")
            LimpiarCampos()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al grabar los datos")
        End Try
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("frmMantenimientoPrecios.aspx", False)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al retornar la página")
        End Try
    End Sub
    Private Sub CargarCombos()
        Dim dtParametrosGenarales As DataTable
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        dtParametrosGenarales = oParametrosGeneralesBM.Listar(ParametrosSIT.VECTOR_PRECIO_VALORIZACION, DatosRequest)
        HelpCombo.LlenarComboBox(ddlFuenteVecPre, dtParametrosGenarales, "Valor", "Nombre", True)
    End Sub
End Class