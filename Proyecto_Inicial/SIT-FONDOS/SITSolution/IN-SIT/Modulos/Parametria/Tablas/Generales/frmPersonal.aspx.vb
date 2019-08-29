Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports UIUtility
Imports System.Data
Partial Class Modulos_Parametria_Tablas_Generales_frmPersonal
    Inherits BasePage
    Private oCentroCostosBM As New CentroCostosBM
    Private oCargoBM As New CargoBM
    Private oPersonalBM As New PersonalBM

#Region " /* Metodos de Pagina */ "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            CargarPagina()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub
    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaPersonal.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al retornar la página")
        End Try
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try

            Aceptar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub
#End Region

    Private Sub Modificar()
        Dim oPersonalBM As New PersonalBM
        Dim oPersonalBE As New PersonalBE
        oPersonalBE = crearObjeto()
        oPersonalBM.Modificar(oPersonalBE, DatosRequest)
        AlertaJS("Los datos fueron modificados correctamente")
    End Sub

    Private Function crearObjeto() As PersonalBE
        Dim oPersonalBE As New PersonalBE
        Dim oRow As PersonalBE.PersonalRow
        oRow = DirectCast(oPersonalBE.Personal.NewPersonalRow(), PersonalBE.PersonalRow)
        oRow.CodigoInterno = txtCodigoInterno.Text
        oRow.CodigoUsuario = txtCodigoInterno.Text
        oRow.email_personal = txtEmailPersonal.Text
        oRow.email_trabajo = txtEmailTrabajo.Text
        oRow.FechaIngreso = txtFechaIngreso.Text
        oRow.LibretaElectoral = txtLibretaElectoral.Text
        If String.IsNullOrEmpty(txtFechaCese.Text) = False Then
            oRow.FechaCese = UIUtility.ConvertirFechaaDecimal(txtFechaCese.Text)
        Else
            oRow.FechaCese = String.Empty
        End If

        oRow.FechaIngreso = UIUtility.ConvertirFechaaDecimal(txtFechaIngreso.Text)
        oRow.PrimerNombre = txtNombre.Text
        oRow.ApellidoMaterno = txtApellidoMaterno.Text
        oRow.ApellidoPaterno = txtApellidoPaterno.Text
        oRow.matricula = txtMatricula.Text
        oRow.CodigoCentroCosto = ddlCentroCosto.SelectedValue
        oRow.SegundoNombre = ""
        oRow.CodigoCargo = ddlCargo.SelectedValue

        IIf(Not hd.Value.Equals(String.Empty), oRow.CodigoInterno = hd.Value, oRow.CodigoInterno = Me.txtCodigoInterno.Text.Trim)
        oPersonalBE.Personal.AddPersonalRow(oRow)
        oPersonalBE.Personal.AcceptChanges()
        Return oPersonalBE
    End Function

    Private Sub Aceptar()
        Dim blnExisteEntidad As Boolean
        If Me.hd.Value.Equals(String.Empty) Then
            blnExisteEntidad = verificarExistenciaPersonal()
            If blnExisteEntidad Then
                Me.AlertaJS(Constantes.M_STR_MENSAJE_ENTIDAD_EXISTE)
            End If
            Insertar()
        Else
            Modificar()
        End If
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

        Dim dtCdntroCosto As DataTable = oCentroCostosBM.Listar(DatosRequest).Tables(0)
        Dim dtCargo As DataTable = oCargoBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlCentroCosto, dtCdntroCosto, "CodigoCentroCosto", "NombreCentroCosto", True)
        HelpCombo.LlenarComboBox(Me.ddlCargo, dtCargo, "CodigoCargo", "Descripcion", True)

    End Sub

    Private Sub cargarRegistro(ByVal codigo As String)
        Dim oPersonalBE As New PersonalBE

        oPersonalBE = oPersonalBM.SeleccionarPorFiltro(codigo, String.Empty, DatosRequest)
        Dim personalFila As PersonalBE.PersonalRow = oPersonalBE.Personal.Rows(0)
        txtCodigoInterno.Text = If(personalFila.IsCodigoInternoNull(), "", personalFila.CodigoInterno.ToString.Trim())
        txtApellidoPaterno.Text = If(personalFila.IsApellidoPaternoNull(), "", personalFila.ApellidoPaterno.ToString().Trim())
        txtNombre.Text = If(personalFila.IsPrimerNombreNull(), "", personalFila.PrimerNombre.ToString.Trim())
        txtApellidoMaterno.Text = If(personalFila.IsApellidoMaternoNull(), "", personalFila.ApellidoMaterno.ToString().Trim())
        ddlCentroCosto.SelectedValue = If(personalFila.IsCodigoCentroCostoNull(), "", personalFila.CodigoCentroCosto)
        ddlCargo.SelectedValue = If(personalFila.IsCodigoCargoNull(), "", personalFila.CodigoCargo)
        txtEmailPersonal.Text = If(personalFila.Isemail_personalNull(), "", personalFila.email_personal)
        txtEmailTrabajo.Text = If(personalFila.Isemail_trabajoNull(), "", personalFila.email_trabajo)
        txtMatricula.Text = If(personalFila.IsmatriculaNull(), "", personalFila.matricula)
        txtLibretaElectoral.Text = If(personalFila.IsLibretaElectoralNull(), "", personalFila.LibretaElectoral)
        txtFechaIngreso.Text = If(personalFila.IsFechaIngresoNull(), "", UIUtility.ConvertirFechaaString(personalFila.FechaIngreso))
        txtFechaCese.Text = If(personalFila.IsFechaCeseNull(), "", UIUtility.ConvertirFechaaString(personalFila.FechaCese))
        
    End Sub

    Private Function verificarExistenciaPersonal() As Boolean
        Dim oPersonalBE As New PersonalBE
        oPersonalBE = oPersonalBM.SeleccionarPorFiltro(txtCodigoInterno.Text, String.Empty, DatosRequest)
        Return oPersonalBE.Tables(0).Rows.Count > 0
    End Function


    Private Sub LimpiarCampos()
        Me.txtCodigoInterno.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.txtNombre.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.txtApellidoMaterno.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.txtApellidoPaterno.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.txtMatricula.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.txtLibretaElectoral.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.txtFechaIngreso.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.txtFechaCese.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.txtEmailPersonal.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.txtEmailTrabajo.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.ddlCargo.SelectedIndex = 0
        Me.ddlCentroCosto.SelectedIndex = 0
    End Sub

    Private Sub Insertar()
        Dim oPersonalBE As New PersonalBE
        oPersonalBE = crearObjeto()
        oPersonalBM.Insertar(oPersonalBE, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
        LimpiarCampos()
    End Sub

End Class
