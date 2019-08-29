Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmFeriado
    Inherits BasePage

#Region " /* Metodos de Pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try            
            If Not Page.IsPostBack Then
                CargarCombos()
                If Not Request.QueryString("cod") Is Nothing Then
                    hd.Value = Request.QueryString("cod")
                    Me.hdMerca.Value = Request.QueryString("merca")
                    cargarRegistro(hd.Value, Me.hdMerca.Value)
                Else
                    hd.Value = String.Empty
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Protected Sub btnSalir_Click(sender As Object, e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("frmBusquedaFeriados.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al salir de la página")
        End Try
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim blnExisteEntidad As Boolean
            If Me.hd.Value.Equals(String.Empty) Then
                blnExisteEntidad = Me.verificarExistenciaFeriado()
                If blnExisteEntidad Then
                    AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
                Else
                    Insertar()
                End If
            Else
                Modificar()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub

#End Region

#Region " /* Funciones Personalizadas */"

    Private Sub Insertar()
        Dim oFeriadoBM As New FeriadoBM
        Dim oFeriadoBE As New FeriadoBE
        oFeriadoBE = crearObjeto()
        oFeriadoBM.Insertar(oFeriadoBE, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
        LimpiarCampos()
    End Sub

    Private Sub Modificar()
        Dim oFeriadoBM As New FeriadoBM
        Dim oFeriadoBE As New FeriadoBE
        oFeriadoBE = crearObjeto()
        oFeriadoBM.Modificar(oFeriadoBE, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)
    End Sub

    Private Sub cargarRegistro(ByVal codigo As String, ByVal mercado As String)
        Dim oFeriadoBM As New FeriadoBM
        Dim oFeriado As New FeriadoBE
        Dim oRow As FeriadoBE.FeriadoRow

        oFeriado = oFeriadoBM.Seleccionar(Convert.ToInt32(codigo), mercado, DatosRequest)
        oRow = DirectCast(oFeriado.Feriado.Rows(0), FeriadoBE.FeriadoRow)
        Me.tbFechaControl.Enabled = False
        Me.spCalendar.Style("Display") = "none"
        Me.hd.Value = oRow.Fecha.ToString()
        Me.tbFechaControl.Text = UIUtility.ConvertirFechaaString(Me.hd.Value)
        Me.ddlMercado.SelectedValue = oRow.CodigoMercado
        Me.ddlMercado.Enabled = False
        Me.ddlSituacion.SelectedValue = oRow.Situacion.ToString()
    End Sub

    Private Function crearObjeto() As FeriadoBE
        Dim oFeriadoBE As New FeriadoBE
        Dim oRow As FeriadoBE.FeriadoRow
        oRow = DirectCast(oFeriadoBE.Feriado.NewFeriadoRow(), FeriadoBE.FeriadoRow)
        oRow.Fecha = UIUtility.ConvertirFechaaDecimal(Me.tbFechaControl.Text)
        oRow.Descripcion = String.Empty
        oRow.CodigoMercado = Me.ddlMercado.SelectedValue
        oRow.Situacion = Me.ddlSituacion.SelectedValue
        oFeriadoBE.Feriado.AddFeriadoRow(oRow)
        oFeriadoBE.Feriado.AcceptChanges()
        Return oFeriadoBE
    End Function

    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim dtMercado As DataTable
        Dim oMercadoBM As New MercadoBM
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)

        dtMercado = oMercadoBM.ListarActivos(Me.DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlMercado, dtMercado, "CodigoMercado", "Descripcion", True)

    End Sub

    Private Sub LimpiarCampos()
        Me.tbFechaControl.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.ddlMercado.SelectedIndex = 0
        Me.ddlSituacion.SelectedIndex = 0
    End Sub

    Private Function verificarExistenciaFeriado() As Boolean
        Dim oFeriadoBM As New FeriadoBM
        Dim oFeriadoBE As New FeriadoBE
        oFeriadoBE = oFeriadoBM.Seleccionar(Convert.ToInt32(Me.tbFechaControl.Text.Substring(6, 4) & Me.tbFechaControl.Text.Substring(3, 2) & Me.tbFechaControl.Text.Substring(0, 2)), Me.ddlMercado.SelectedValue, DatosRequest)
        If oFeriadoBE.Feriado.Rows.Count > 0 Then
            verificarExistenciaFeriado = True
        Else
            verificarExistenciaFeriado = False
        End If
    End Function

#End Region

End Class
