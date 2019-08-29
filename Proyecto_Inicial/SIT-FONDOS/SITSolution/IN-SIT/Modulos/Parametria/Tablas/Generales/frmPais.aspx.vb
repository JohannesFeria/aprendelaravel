Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmPais
    Inherits BasePage


#Region " */ Metodos de Pagina */"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                If Not (Request.QueryString("cod") = Nothing) Then
                    hd.Value = Request.QueryString("cod")
                    cargarRegistro(hd.Value)
                Else
                    hd.Value = ""
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim oPaisBM As New PaisBM
        Dim oPaisBE As New PaisBE
        'Si no tiene nada es porque es un nuevo registro
        Try
            oPaisBE = crearObjeto()
            If Me.hd.Value = "" Then
                If verificarExistenciaPais() = True Then
                    AlertaJS("Este registro ya existe")
                    Exit Sub
                End If

                oPaisBM.Insertar(oPaisBE, DatosRequest)
                AlertaJS("Los datos fueron grabados correctamente")
                LimpiarCampos()
            Else
                oPaisBM.Modificar(oPaisBE, DatosRequest)
                AlertaJS("Los datos fueron modificados correctamente")
            End If
            'Me.ibAceptar.Visible = False
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub
    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("frmBusquedaPaises.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cancelar la operación")
        End Try
    End Sub
#End Region

#Region " */ Funciones Personalizadas */"
    Public Sub cargarRegistro(ByVal codigo As String)
        Dim oPaisBM As New PaisBM
        Dim oPais As New PaisBE
        oPais = oPaisBM.SeleccionarPorFiltro(codigo, "", "", DatosRequest)
        tbCodigo.Enabled = False

        tbCodigo.Text = CType(oPais.Pais.Rows(0), PaisBE.PaisRow).CodigoPais.ToString()
        tbDescripcion.Text = CType(oPais.Pais.Rows(0), PaisBE.PaisRow).Descripcion.ToString()
        ddlSituacion.SelectedValue = CType(oPais.Pais.Rows(0), PaisBE.PaisRow).Situacion.ToString()

        'MC 10.06.14
        Dim paraiso As String, dobleimposicion As String

        paraiso = IIf(IsDBNull(CType(oPais.Pais.Rows(0), PaisBE.PaisRow).Paraiso), String.Empty, CType(oPais.Pais.Rows(0), PaisBE.PaisRow).Paraiso.ToString())
        dobleimposicion = IIf(IsDBNull(CType(oPais.Pais.Rows(0), PaisBE.PaisRow).DobleImposicion), String.Empty, CType(oPais.Pais.Rows(0), PaisBE.PaisRow).DobleImposicion.ToString())

        'If CType(oPais.Pais.Rows(0), PaisBE.PaisRow).Paraiso.ToString() <> "" Then 'JRM 20100518
        If Not String.IsNullOrEmpty(paraiso) Then
            ddlParaiso.SelectedValue = CType(oPais.Pais.Rows(0), PaisBE.PaisRow).Paraiso.ToString()
        End If

        'If CType(oPais.Pais.Rows(0), PaisBE.PaisRow).DobleImposicion.ToString() <> "" Then 'JLPA 20100602
        If Not String.IsNullOrEmpty(dobleimposicion) Then
            ddlDobleImposicion.SelectedValue = CType(oPais.Pais.Rows(0), PaisBE.PaisRow).DobleImposicion.ToString()
        End If
        'MC 10.06.14
    End Sub
    Public Function crearObjeto() As PaisBE
        Dim oPaisBE As New PaisBE
        Dim oRow As PaisBE.PaisRow
        oRow = CType(oPaisBE.Pais.NewPaisRow(), PaisBE.PaisRow)

        oRow.CodigoPais = tbCodigo.Text.ToUpper.Trim.ToString
        oRow.Descripcion = Me.tbDescripcion.Text.TrimStart.TrimEnd.ToString
        oRow.Situacion = Me.ddlSituacion.SelectedValue
        oRow.Paraiso = Me.ddlParaiso.SelectedValue 'JRM 20100427
        oRow.DobleImposicion = Me.ddlDobleImposicion.SelectedValue 'JLPA 20100602

        IIf(hd.Value <> "", oRow.CodigoPais = hd.Value, oRow.CodigoPais = Me.tbCodigo.Text.Trim)

        oPaisBE.Pais.AddPaisRow(oRow)
        oPaisBE.Pais.AcceptChanges()

        Return oPaisBE

    End Function
    Public Sub CargarCombos()
        Dim tablaSituacion As New DataTable

        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)

        'JRM 20100518
        Dim tablaCondicion As New DataTable
        tablaCondicion = oParametrosGenerales.ListarParametros("Condicion", "", "", "", New DataSet("Condicion")).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlParaiso, tablaCondicion, "Valor", "Nombre", False)
        'JLPA 20100602
        HelpCombo.LlenarComboBox(Me.ddlDobleImposicion, tablaCondicion, "Valor", "Nombre", False)
    End Sub
    Sub LimpiarCampos()
        Me.tbCodigo.Text = ""
        Me.tbDescripcion.Text = ""
        Me.ddlSituacion.SelectedIndex = 0
        Me.ddlParaiso.SelectedIndex = 0 'JRM 20100518
        Me.ddlDobleImposicion.SelectedIndex = 0 'JLPA 20100602
    End Sub
    Private Function verificarExistenciaPais() As Boolean
        Dim oPaisBM As New PaisBM
        Dim oPaisBE As New PaisBE
        oPaisBE = oPaisBM.SeleccionarPorFiltro(Me.tbCodigo.Text.Trim, "", "", DatosRequest)
        If oPaisBE.Pais.Rows.Count > 0 Then
            verificarExistenciaPais = True
        Else
            verificarExistenciaPais = False
        End If
    End Function
#End Region
End Class
