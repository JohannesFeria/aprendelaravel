Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data

Partial Class Modulos_Tesoreria_Encaje_frmCapturaTasasEncaje
    Inherits BasePage

#Region "/* Propiedades */"

    Private Property VistaTasaEncaje() As DataSet
        Get
            Return DirectCast(ViewState("TasaEncaje"), DataSet)
        End Get

        Set(ByVal Value As DataSet)
            ViewState("TasaEncaje") = Value
        End Set
    End Property

#End Region

#Region "/* Variables */"

    Dim oTasaEncajeBE As New TasaEncajeBE
    Dim oTasaEncajeBM As New TasaEncajeBM

#End Region

#Region "/* Funciones Personalizadas*/"

#Region " /* Funciones Modificar */"

#End Region

#Region " /* Funciones Eliminar */"

    Public Sub Eliminar(ByVal secuencia As String)
        oTasaEncajeBM.Eliminar(secuencia, DatosRequest)
        CargarGrilla()
    End Sub

#End Region

    Private Sub CargarGrilla()
        Dim strCalificacion As String
        Dim strEmisor As String
        Dim strMnemonico As String
        Dim decFechaVigencia As Decimal
        Dim decTasasVigentes As Decimal
        If Me.tbemisor.Text.Equals(String.Empty) Then
            strEmisor = String.Empty
        Else
            strEmisor = Me.tbemisor.Text
        End If
        If Me.tbNemonico.Text.Equals(String.Empty) Then
            strMnemonico = String.Empty
        Else
            strMnemonico = Me.tbNemonico.Text
        End If

        If Me.ddlCalificacion.SelectedItem.Text.Equals(Constantes.M_STR_TEXTO_SELECCIONAR_EN_COMBO) Then
            strCalificacion = String.Empty
        Else
            strCalificacion = Me.ddlCalificacion.SelectedValue
        End If
        If Me.tbVigenciaDesde.Text.Equals(String.Empty) Then
            decFechaVigencia = 0
        Else
            decFechaVigencia = UIUtility.ConvertirFechaaDecimal(Me.tbVigenciaDesde.Text)
        End If
        If Me.tbVigenciaDesde.Text.Equals(String.Empty) Then
            decTasasVigentes = 0
        Else
            decTasasVigentes = UIUtility.ConvertirFechaaDecimal(Me.tbVigenciaDesde.Text)
        End If
        Dim dsDatos As New DataSet
        dsDatos = oTasaEncajeBM.SeleccionarPorFiltro(strEmisor, strCalificacion, decFechaVigencia, strMnemonico, decTasasVigentes)
        Dim dtblDatos As DataTable = dsDatos.Tables(0)
        Me.dgLista.DataSource = dtblDatos
        Me.dgLista.DataBind()
        EjecutarJS("$('#" + Me.lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dtblDatos) + "')")
        VistaTasaEncaje = dsDatos
    End Sub

    Public Sub CargarCombos()
        Dim DtTablaCalificacionSBS As DataTable
        Dim DtTablaSituacion As DataTable
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim oCalificacionSBS As New CalificacionInstrumentoBM
        DtTablaCalificacionSBS = oCalificacionSBS.Listar(DatosRequest).Tables(0)
        DtTablaSituacion = oParametrosGeneralesBM.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlCalificacion, DtTablaCalificacionSBS, "CodigoCalificacion", "CodigoCalificacion", True)
    End Sub

    Private Sub ObtenerEmisorSBS()
        Dim oEntidad As New EntidadBM
        Dim oDT As DataTable
        oDT = oEntidad.Seleccionar(Me.tbemisor.Text, DatosRequest).Tables(0)
        If oDT.Rows.Count > 0 Then
            Me.hdEmisor.Value = oDT.Rows(0)("CodigoSBS")
        End If
    End Sub

#End Region

#Region "/* Funciones Pagina*/"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                Me.dgLista.PageIndex = 0
                CargarGrilla()
                If Me.dgLista.Rows.Count = 0 Then
                    AlertaJS("No se encontraron Registros")
                End If
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                If hdTipoBusqueda.Value = "E" Then
                    tbemisor.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                ElseIf hdTipoBusqueda.Value = "M" Then
                    tbNemonico.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                End If
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Me.dgLista.PageIndex = 0
            CargarGrilla()
            If Me.dgLista.Rows.Count = 0 Then
                AlertaJS("No se encontraron Registros")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmMantenimientoTasasEncaje.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar a la página")
        End Try
    End Sub

    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        Try
            Response.Redirect("frmMantenimientoTasasEncaje.aspx?cod=" & e.CommandArgument)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Modificar el registro")
        End Try
    End Sub

    Private Sub EditarCeldaGrilla(ByVal oDataGridItem As GridViewRow)
        Dim valor As Decimal
        Dim valorcad As String
        If oDataGridItem.RowType = DataControlRowType.DataRow Then
            CType(oDataGridItem.Cells(1).FindControl("ibEliminar"), ImageButton).Attributes.Add("OnClick", "return confirm('¿Desea eliminar registro?')")
            valor = oDataGridItem.Cells(6).Text
            valor = Math.Round(valor, 7)
            valorcad = valor.ToString
            oDataGridItem.Cells(6).Text = valorcad.Replace(UIUtility.DecimalSeparator(), ".")
            If oDataGridItem.Cells(7).Text.Trim = "A" Then
                oDataGridItem.Cells(7).Text = "Activo"
            Else
                oDataGridItem.Cells(7).Text = "Inactivo"
            End If
        End If
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir de la página")
        End Try
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            Me.EditarCeldaGrilla(e.Row)
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
        End Try
    End Sub

    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            Dim secuencia As String
            secuencia = e.CommandArgument
            If e.CommandName.Equals("Eliminar") Then
                Eliminar(secuencia)
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar una operación en la Grilla")
        End Try
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la paginación")
        End Try
    End Sub

#End Region

End Class
