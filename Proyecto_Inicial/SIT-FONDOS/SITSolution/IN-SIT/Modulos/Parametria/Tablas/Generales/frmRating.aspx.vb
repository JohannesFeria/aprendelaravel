Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmRating
    Inherits BasePage

    Private Sub ConstruirObjeto()
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim dtblDatos As DataTable
        dtblDatos = oParametrosGenerales.ListarRating("", hddValor.Value.ToString, "", "", DatosRequest).Tables(0)
        ddlTipoRating.SelectedValue = IIf(IsDBNull(dtblDatos.Rows(0).Item("Comentario")), "--Seleccione--", dtblDatos.Rows(0).Item("Comentario"))
        tbDescripcion.Text = CType(dtblDatos.Rows(0).Item("Nombre"), String).Replace("(L)", "").Trim
        If dtblDatos.Rows(0)("factor").ToString <> "" Then tbFactor.Text = Format(Convert.ToDecimal(dtblDatos.Rows(0)("factor").ToString), "##,##0.00") 'HDG OT 62087 Nro14-R23 20110222
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                hddAction.Value = Request.QueryString("accion").ToString

                Select Case hddAction.Value.ToString
                    Case "upd"
                        hddValor.Value = Request.QueryString("valor").ToString
                        ConstruirObjeto()
                    Case "new"
                        hddValor.Value = ""
                End Select
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("frmBusquedaRating.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cancelar la operación")
        End Try
    End Sub

    Private Sub InsertarRating()
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim strTipoRating As String = IIf(ddlTipoRating.SelectedValue.ToString.Equals("--Seleccione--"), "", ddlTipoRating.SelectedValue.ToString)
        Dim Descripcion As String = tbDescripcion.Text.Replace("(L)", "").Trim
        Dim Factor As String = tbFactor.Text.Trim

        Select Case strTipoRating
            Case "LOC"
                Descripcion &= " (L)"
        End Select
        Dim intResult As Integer

        intResult = oParametrosGenerales.InsertarRating(Descripcion.ToUpper, strTipoRating, Factor, DatosRequest)
        If intResult = 0 Then
            AlertaJS("El Rating ya existe.\nIngrese uno nuevo.")
        Else
            AlertaJS("Los datos fueron grabados correctamente")
        End If
    End Sub

    Private Sub ModificarRating(ByVal valor As String)
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim strTipoRating As String = IIf(ddlTipoRating.SelectedValue.ToString.Equals("--Seleccione--"), "", ddlTipoRating.SelectedValue.ToString)
        Dim Descripcion As String = tbDescripcion.Text.Replace("(L)", "").Trim
        Dim Factor As String = tbFactor.Text.Trim   'HDG OT 62087 Nro14-R23 20110223

        Select Case strTipoRating
            Case "LOC"
                Descripcion &= " (L)"
        End Select

        Dim intResult As Integer
        intResult = oParametrosGenerales.ModificarRating(Descripcion.ToUpper, valor, strTipoRating, Factor, DatosRequest)
        If intResult = 0 Then
            AlertaJS("El Rating ya existe.\nIngrese uno nuevo.")
        Else
            AlertaJS("Los datos fueron modificados correctamente.")
        End If
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Select Case hddAction.Value.ToString
                Case "upd"
                    ModificarRating(hddValor.Value.ToString)
                Case "new"
                    InsertarRating()
            End Select
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub

End Class
