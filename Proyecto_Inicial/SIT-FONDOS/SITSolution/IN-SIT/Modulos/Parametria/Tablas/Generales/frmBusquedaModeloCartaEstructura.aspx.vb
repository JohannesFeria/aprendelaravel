Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmBusquedaModeloCartaEstructura
    Inherits BasePage


#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            tbCodigo.Text = Request.QueryString("cod")
            CargarGrilla()
        End If
    End Sub

    Private Sub ibCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibCancelar.Click
        Response.Redirect("frmModeloCarta.aspx?cod=" & Request.QueryString("cod"))
    End Sub

    Private Sub ibIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibIngresar.Click
        MostrarCampos()
        hdOperacion.Value = "I"
    End Sub

    Protected Sub dgLista_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        dgLista.PageIndex = e.NewPageIndex
        CargarGrilla()
    End Sub

    Protected Sub ibtnAceptar_Click(sender As Object, e As System.EventArgs) Handles ibtnAceptar.Click
        Dim oModeloCartaEstructura As New ModeloCartaEstructuraBM
        Dim oModeloCartaEstructuraBE As New ModeloCartaEstructuraBE
        oModeloCartaEstructuraBE = crearObjeto()

        If hdOperacion.Value = "I" Then
            'manda campos para ingresar nuevo
            oModeloCartaEstructura.Insertar(oModeloCartaEstructuraBE, DatosRequest)
            AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)

        ElseIf hdOperacion.Value = "M" Then
            oModeloCartaEstructura.Modificar(oModeloCartaEstructuraBE, DatosRequest)
            AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)
        End If

        OcultarCampos()
        CargarGrilla()
    End Sub

    Protected Sub dgLista_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.Cells(1).FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
        End If
    End Sub

    Protected Sub dgLista_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Dim fila As Integer
        Dim texto As String

        If (hdOperacion.Value = "E") Then

            fila = e.CommandArgument.ToString().Split(",")(3).Trim
            'intA = e.Item.ItemIndex
            'texto = dgLista.Items.Item(intA).Cells(2).Text
            texto = dgLista.Rows(fila).Cells(2).Text
            If texto <> "&nbsp;" Then
                hdNombre.Value = texto
            Else
                hdNombre.Value = ""
            End If

            texto = dgLista.Rows(fila).Cells(3).Text
            If texto <> "&nbsp;" Then
                hdOrigen.Value = texto
            Else
                hdOrigen.Value = ""
            End If
            Eliminar_Modelo()

        End If
    End Sub

#End Region

#Region " /* Funciones Modificar */"
    Public Sub Modificar(ByVal sender As Object, ByVal e As CommandEventArgs)
        hdOperacion.Value = "M"
        'Dim oModeloCartaEstructura As New Sit.BusinessLayer.ModeloCartaEstructura
        'Dim modificar As SalirDelegate = New SalirDelegate(AddressOf oModeloCartaEstructura.Actualizar)
        'modificar.BeginInvoke(DatosRequest, Nothing, Nothing)

        Dim codigo As String = e.CommandArgument
        hdNombre.Value = e.CommandArgument.ToString().Split(","c)(1)
        hdOrigen.Value = e.CommandArgument.ToString().Split(","c)(2)
        txtNombre.Text = e.CommandArgument.ToString().Split(","c)(1)
        txtOrigen.Text = e.CommandArgument.ToString().Split(","c)(2)
        txtNombre.Visible = True
        txtOrigen.Visible = True
        lblNombre.Visible = True
        lblOrigen.Visible = True
        ibtnAceptar.Visible = True

    End Sub

    Sub MostrarCampos()
        txtNombre.Text = ""
        txtOrigen.Text = ""
        lblNombre.Visible = True
        txtNombre.Visible = True
        lblOrigen.Visible = True
        txtOrigen.Visible = True
        ibtnAceptar.Visible = True
    End Sub

    Sub OcultarCampos()
        txtNombre.Text = ""
        txtOrigen.Text = ""
        lblNombre.Visible = False
        txtNombre.Visible = False
        lblOrigen.Visible = False
        txtOrigen.Visible = False
        ibtnAceptar.Visible = False
    End Sub

#End Region

#Region " /* Funciones Eliminar */"
    Public Sub Eliminar(ByVal sender As Object, ByVal e As CommandEventArgs)
        hdOperacion.Value = "E"
    End Sub
#End Region

#Region " /* Funciones Personalizadas*/"
    Private Sub CargarGrilla()
        Dim oModeloCartaEstructura As New ModeloCartaEstructuraBM
        Dim dsModeloCartaEstructura As New DataSet
        Dim dtblDatos As DataTable = oModeloCartaEstructura.SeleccionarPorFiltro(tbCodigo.Text, dsModeloCartaEstructura).Tables(0)
        Me.dgLista.DataSource = dtblDatos
        Me.dgLista.DataBind()
        Me.lbContador.Text = MostrarResultadoBusqueda(dtblDatos)
    End Sub


#End Region

    Sub Eliminar_Modelo()
        Dim oModeloCartaEstructura As New ModeloCartaEstructuraBM
        Dim oModeloCartaEstructuraBE As New ModeloCartaEstructuraBE
        oModeloCartaEstructuraBE = crearObjeto()
        Try
            oModeloCartaEstructura.Eliminar(oModeloCartaEstructuraBE, DatosRequest)
            AlertaJS(Constantes.M_STR_MENSAJE_ELIMINAR_ENTIDAD)
            CargarGrilla()
        Catch ex As Exception
            'Las excepciones deben ser enviadas a la clase base con el método ManejarError,esta clase se encarga de mostrar los mensajes correspondientes
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Function crearObjeto() As ModeloCartaEstructuraBE

        Dim oModeloCartaEstructura As New ModeloCartaEstructuraBE
        Dim oRow As ModeloCartaEstructuraBE.ModeloCartaEstructuraRow

        oRow = DirectCast(oModeloCartaEstructura._ModeloCartaEstructura.NewModeloCartaEstructuraRow(), ModeloCartaEstructuraBE.ModeloCartaEstructuraRow)

        If Me.tbCodigo.Text.ToString.TrimStart.TrimEnd <> "" Then
            oRow.CodigoModelo = Me.tbCodigo.Text.ToString.TrimStart.TrimEnd
        Else
            oRow.CodigoModelo = Me.hdCodigoModelo.Value.TrimStart.TrimEnd
        End If

        If hdOperacion.Value = "I" Then
            oRow.OrigenCampo = Me.txtOrigen.Text.ToString.TrimStart.TrimEnd
            oRow.NombreCampo = Me.txtNombre.Text.ToString.TrimStart.TrimEnd
        Else
            oRow.OrigenCampo = Me.hdOrigen.Value.ToString.TrimStart.TrimEnd
            oRow.NombreCampo = Me.hdNombre.Value.ToString.TrimStart.TrimEnd

        End If

        oRow.OrigenCampoNuevo = Me.txtOrigen.Text.ToString.TrimStart.TrimEnd
        oRow.NombreCampoNuevo = Me.txtNombre.Text.ToString.TrimStart.TrimEnd

        oModeloCartaEstructura._ModeloCartaEstructura.AddModeloCartaEstructuraRow(oRow)
        oModeloCartaEstructura._ModeloCartaEstructura.AcceptChanges()

        Return oModeloCartaEstructura

    End Function

End Class
