Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System
Imports System.IO
Imports System.Data
Partial Class Modulos_Parametria_Tablas_Generales_frmModeloCarta
    Inherits BasePage
#Region "/* Variables */"
    Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
#End Region
#Region "/* Metodos de Pagina */"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                Dim datosModal As String() = CType(Session("SS_DatosModal"), String())
                If hfModal.Value = 1 Then
                    txtUsuarioValidador1.Text = datosModal(0).ToString()
                    lblNombreUsuario1.Text = datosModal(1).ToString()
                ElseIf hfModal.Value = 2 Then
                    txtUsuarioValidador2.Text = datosModal(0).ToString()
                    lblNombreUsuario2.Text = datosModal(1).ToString()
                End If
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Protected Sub bAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibAceptar.Click
        Try
            Aceptar()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Protected Sub btnSalir_Click(sender As Object, e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("frmBusquedaModeloCartas.aspx")
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
#End Region
#Region "/* Funciones Personalizadas */"
    Private Sub cargarRegistro(ByVal codigo As String)
        Dim oModeloCartaBM As New ModeloCartaBM
        Dim oModeloCarta As New ModeloCartaBE
        oModeloCarta = oModeloCartaBM.SeleccionarPorFiltro(codigo, String.Empty, String.Empty, DatosRequest)
        Me.tbCodigo.Enabled = False
        Me.hd.Value = DirectCast(oModeloCarta.ModeloCarta.Rows(0), ModeloCartaBE.ModeloCartaRow).CodigoModelo.ToString()
        Me.tbCodigo.Text = DirectCast(oModeloCarta.ModeloCarta.Rows(0), ModeloCartaBE.ModeloCartaRow).CodigoModelo.ToString()
        Me.tbDescripcion.Text = DirectCast(oModeloCarta.ModeloCarta.Rows(0), ModeloCartaBE.ModeloCartaRow).Descripcion.ToString()
        Dim ruta As String
        If DirectCast(oModeloCarta.ModeloCarta.Rows(0), ModeloCartaBE.ModeloCartaRow).ArchivoPlantilla Is DBNull.Value Then
            ruta = ""
        Else
            ruta = DirectCast(oModeloCarta.ModeloCarta.Rows(0), ModeloCartaBE.ModeloCartaRow).ArchivoPlantilla.ToString
        End If
        If ruta.Length = 0 Then
            Me.lblRutaAnterior.Visible = False
            Me.tbRutaAnterior.Visible = False
            Me.tbRutaAnterior.Text = ""
            Me.hdruta.Value = ""
        Else
            Me.lblRutaAnterior.Visible = True
            Me.tbRutaAnterior.Visible = True
            Me.tbRutaAnterior.Text = ruta
            Me.tbRutaAnterior.Enabled = False
            Me.hdruta.Value = ruta
            If Not (ddlArchivo.Items.FindByValue(Path.GetFileName(ruta)) Is Nothing) Then
                ddlArchivo.SelectedValue = Path.GetFileName(ruta)
            End If
        End If
        Me.ddlOperacion.SelectedValue = DirectCast(oModeloCarta.ModeloCarta.Rows(0), ModeloCartaBE.ModeloCartaRow).CodigoOperacion.ToString()
        Me.ddlSituacion.SelectedValue = DirectCast(oModeloCarta.ModeloCarta.Rows(0), ModeloCartaBE.ModeloCartaRow).Situacion.ToString()
        Me.txtUsuarioValidador1.Text = DirectCast(oModeloCarta.ModeloCarta.Rows(0), ModeloCartaBE.ModeloCartaRow).UsuarioValidador1.ToString()
        Me.txtUsuarioValidador2.Text = DirectCast(oModeloCarta.ModeloCarta.Rows(0), ModeloCartaBE.ModeloCartaRow).UsuarioValidador2.ToString()
        Me.lblNombreUsuario1.Text = New PersonalBM().SeleccionarPorCodigoInterno(Me.txtUsuarioValidador1.Text, DatosRequest)
        Me.lblNombreUsuario2.Text = New PersonalBM().SeleccionarPorCodigoInterno(Me.txtUsuarioValidador2.Text, DatosRequest)
    End Sub
    Private Function crearObjeto() As ModeloCartaBE
        Dim oModeloCartaBE As New ModeloCartaBE
        Dim oRow As ModeloCartaBE.ModeloCartaRow
        oRow = DirectCast(oModeloCartaBE.ModeloCarta.NewModeloCartaRow(), ModeloCartaBE.ModeloCartaRow)
        oRow.CodigoModelo = Me.tbCodigo.Text.ToString.ToUpper.TrimStart.TrimEnd
        oRow.Descripcion = Me.tbDescripcion.Text.ToString.ToUpper.TrimStart.TrimEnd
        oRow.UsuarioValidador1 = Me.txtUsuarioValidador1.Text.ToUpper.TrimStart().TrimEnd()
        oRow.UsuarioValidador2 = Me.txtUsuarioValidador2.Text.ToUpper.TrimStart().TrimEnd()
        oRow.CodigoOperacion = Me.ddlOperacion.SelectedValue
        If Me.ddlArchivo.SelectedIndex = 0 And Me.tbRutaAnterior.Visible Then
            oRow.ArchivoPlantilla = Me.tbRutaAnterior.Text
        End If
        If ddlArchivo.SelectedIndex <> 0 Then
            oRow.ArchivoPlantilla = oParametrosGeneralesBM.ObtenerRutaModeloCarta(DatosRequest).Rows(0).Item("Comentario").ToString + ddlArchivo.SelectedItem.Text
            Me.hdruta.Value = oParametrosGeneralesBM.ObtenerRutaModeloCarta(DatosRequest).Rows(0).Item("Comentario").ToString + ddlArchivo.SelectedItem.Text
            If Not Me.hd.Value.Equals(String.Empty) Then
                Me.tbRutaAnterior.Visible = True
                Me.lblRutaAnterior.Visible = True
                Me.tbRutaAnterior.Text = oParametrosGeneralesBM.ObtenerRutaModeloCarta(DatosRequest).Rows(0).Item("Comentario").ToString + ddlArchivo.SelectedItem.Text
            End If
        End If
        oRow.Situacion = Me.ddlSituacion.SelectedValue
        IIf(Not hd.Value.Equals(String.Empty), oRow.CodigoModelo = hd.Value, oRow.CodigoModelo = Me.tbCodigo.Text.Trim)
        oModeloCartaBE.ModeloCarta.AddModeloCartaRow(oRow)
        oModeloCartaBE.ModeloCarta.AcceptChanges()
        Return oModeloCartaBE
    End Function
    Private Sub CargarModelosCarta()
        Dim archivo As String 'para el nombre de archivos y carpetas
        Dim sArchivos() As String 'array con los nombres de archivos y directorios
        Dim archivoInfo As FileInfo 'objeto para extraer propiedades de los archivos
        Dim i, cont As Integer 'para llevar la cuenta del nº de archivos
        Dim ruta As String
        ruta = oParametrosGeneralesBM.ObtenerRutaModeloCarta(DatosRequest).Rows(0).Item("Comentario").ToString
        Dim dtTabla As New DataTable
        dtTabla.Columns.Add("Codigo")
        dtTabla.Columns.Add("Descripcion")
        sArchivos = Directory.GetFiles(ruta)
        i = sArchivos.Length
        cont = 0
        For Each archivo In sArchivos
            archivoInfo = New FileInfo(archivo)

            If (archivoInfo.Name.Substring(0, 1) <> "~") Then
                Dim row As DataRow = dtTabla.NewRow()
                row(0) = cont
                row(1) = archivoInfo.Name  'Nombre Archivo
                dtTabla.Rows.Add(row)
                cont = cont + 1
            End If
        Next
        Dim dwdatos As New DataView
        dwdatos = dtTabla.DefaultView
        dwdatos.Sort = "Descripcion DESC"
        ddlArchivo.DataSource = dwdatos.Table
        ddlArchivo.DataValueField = "Descripcion"
        ddlArchivo.DataTextField = "Descripcion"
        ddlArchivo.DataBind()
        ddlArchivo.Items.Insert(0, New ListItem("--Seleccione--", ""))
    End Sub
    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim tablaRuta As New DataTable
        Dim tablaOperacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim oOperacion As New OperacionBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        tablaOperacion = oOperacion.SeleccionarPorCodigoTipoOperacion("", "", "20").Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
        HelpCombo.LlenarComboBox(Me.ddlOperacion, tablaOperacion, "CodigoOperacion", "Descripcion", False)
        CargarModelosCarta()
    End Sub
    Private Sub LimpiarCampos()
        Me.tbCodigo.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.tbDescripcion.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.txtUsuarioValidador1.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.txtUsuarioValidador2.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.ddlArchivo.SelectedIndex = 0
        Me.ddlSituacion.SelectedIndex = 0
    End Sub
    Private Function verificarExistenciaModeloCarta() As Boolean
        Dim oModeloCartaBM As New ModeloCartaBM
        Dim oModeloCartaBE As New ModeloCartaBE
        oModeloCartaBE = oModeloCartaBM.SeleccionarPorFiltro(Me.tbCodigo.Text.Trim, String.Empty, String.Empty, DatosRequest)
        Return oModeloCartaBE.ModeloCarta.Rows.Count > 0
    End Function
    Private Sub Insertar()
        Dim oModeloCartaBM As New ModeloCartaBM
        Dim oModeloCartaBE As New ModeloCartaBE
        oModeloCartaBE = crearObjeto()
        oModeloCartaBM.Insertar(oModeloCartaBE, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
        LimpiarCampos()
    End Sub
    Private Sub Modificar()
        Dim oModeloCartaBM As New ModeloCartaBM
        Dim oModeloCartaBE As New ModeloCartaBE
        oModeloCartaBE = crearObjeto()
        oModeloCartaBM.Modificar(oModeloCartaBE, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)
    End Sub
    Private Sub Aceptar()
        Dim blnExisteEntidad As Boolean
        If Me.hd.Value.Equals(String.Empty) Then
            blnExisteEntidad = verificarExistenciaModeloCarta()
            If blnExisteEntidad Then
                AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
            Else
                Insertar()
            End If
        Else
            Modificar()
            Me.lblNombreUsuario1.Text = New PersonalBM().SeleccionarPorCodigoInterno(Me.txtUsuarioValidador1.Text, DatosRequest)
            Me.lblNombreUsuario2.Text = New PersonalBM().SeleccionarPorCodigoInterno(Me.txtUsuarioValidador2.Text, DatosRequest)
        End If
    End Sub
    Private Sub CargarPagina()
        CargarCombos()
        If Not (Request.QueryString("cod") = Nothing) Then
            hd.Value = Request.QueryString("cod")
            cargarRegistro(hd.Value)
        Else
            Me.lblRutaAnterior.Visible = False
            Me.tbRutaAnterior.Visible = False
            Me.tbRutaAnterior.Text = ""
            Me.hdruta.Value = ""
            hd.Value = String.Empty
        End If
    End Sub
    Private Sub ibtnModeloCartaEstructura_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibtnModeloCartaEstructura.Click
        Response.Redirect("frmBusquedaModeloCartaEstructura.aspx?cod=" & tbCodigo.Text)
    End Sub
#End Region
End Class