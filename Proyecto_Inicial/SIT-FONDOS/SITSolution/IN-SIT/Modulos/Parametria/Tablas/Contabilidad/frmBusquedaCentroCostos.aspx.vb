Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data
Partial Class Modulos_Parametria_Tablas_Contabilidad_frmBusquedaCentroCostos
    Inherits BasePage
#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ddlSituacion As System.Web.UI.WebControls.DropDownList

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarDatosGrilla()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub
#Region " /* Metodos de Pagina */ "

    Protected Sub ibBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibBuscar.Click
        Try
            dgLista.PageIndex = 0
            CargarDatosGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la búsqueda")
        End Try
    End Sub

    Protected Sub ibConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibConsultar.Click
        Try
            tbCodigo.Text = Constantes.M_STR_TEXTO_INICIAL
            tbDescripcion.Text = Constantes.M_STR_TEXTO_INICIAL
            ddlSituacion.SelectedValue = Constantes.M_STR_TEXTO_TODOS
        Catch ex As Exception
            AlertaJS("Ocurrió un error al consultar la operación")
        End Try
    End Sub

    Protected Sub ibIngresar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibIngresar.Click
        Try
            Response.Redirect("CentroCostos.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al ingresar a la página")
        End Try
    End Sub

    Protected Sub ibSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibSalir.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al salir de la página")
        End Try
    End Sub
#End Region
#Region " /* Funciones Modificar */"

    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim codigo As String = e.CommandArgument
            Response.Redirect("CentroCostos.aspx?cod=" & codigo)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al modificar el registro")
        End Try
    End Sub

#End Region
#Region " /* Funciones Eliminar */"
    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Dim oCentroCostosBM As New CentroCostosBM
            Dim codigo As String = e.CommandArgument
            oCentroCostosBM.Eliminar(codigo, DatosRequest)
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al eliminar el registro")
        End Try
    End Sub

#End Region
#Region " /* Funciones Personalizadas*/"

    Private Sub CargarDatosGrilla()
        CargarGrilla()
        If Me.dgLista.Rows.Count = 0 Then
            AlertaJS("No se encontraron Registros")
        End If
    End Sub

    Private Sub CargarGrilla()

        Dim oCentroCostosBM As New CentroCostosBM

        Dim strCodigo As String
        Dim strDescripcion As String

        strCodigo = Me.tbCodigo.Text.Trim().ToUpper
        strDescripcion = Me.tbDescripcion.Text.Trim().ToUpper

        Dim dtblDatos As DataTable = oCentroCostosBM.SeleccionarPorFiltros(strCodigo, strDescripcion, DatosRequest).Tables(0)
        Me.dgLista.DataSource = dtblDatos
        Me.dgLista.DataBind()
        Me.lbContador.Text = MostrarResultadoBusqueda(dtblDatos)
    End Sub

#End Region
    Private Sub ActualizarIndice(ByVal nuevoIndice As Integer)
        dgLista.PageIndex = nuevoIndice
        CargarGrilla()
    End Sub
    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            ActualizarIndice(e.NewPageIndex)
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub
End Class