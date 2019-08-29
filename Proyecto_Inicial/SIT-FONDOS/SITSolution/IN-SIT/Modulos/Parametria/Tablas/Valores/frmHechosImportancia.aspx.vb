'Creado por: HDG OT 64769-4 20120404
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports ParametrosSIT

Partial Class Modulos_Parametria_Tablas_Valores_frmHechosImportancia
    Inherits BasePage

#Region "/* Eventos de la Pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                If Not (Request.QueryString("codigo") = Nothing) Then
                    hdCodigo.Value = Request.QueryString("codigo")
                    CargarControles(hdCodigo.Value)
                Else
                    hdCodigo.Value = String.Empty
                End If
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                tbNemonico.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Aceptar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al grabar los datos")
        End Try
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("frmBusquedaHechosImportancia.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al retornar la página")
        End Try
    End Sub

#End Region

#Region "/* Metodos Personalizados */"

    Private Sub Aceptar()
        If hdCodigo.Value.Equals(String.Empty) Then
            Insertar()
        Else
            Modificar()
        End If
    End Sub

    Private Sub Insertar()
        Dim oHechosImportanciaBM As New HechosImportanciaBM
        Dim oHechosImportanciaBE As HechosImportanciaBE

        oHechosImportanciaBE = ObtenerInstancia()
        oHechosImportanciaBM.Insertar(oHechosImportanciaBE, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
        LimpiarCampos()
    End Sub
    Private Sub Modificar()
        Dim oHechosImportanciaBM As New HechosImportanciaBM
        Dim oHechosImportanciaBE As HechosImportanciaBE
        oHechosImportanciaBE = ObtenerInstancia()
        oHechosImportanciaBM.Modificar(oHechosImportanciaBE, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)
    End Sub
    Private Sub CargarControles(ByVal codigo As String)
        Dim oHechosImportanciaBM As New HechosImportanciaBM
        Dim oHechosImportanciaBE As HechosImportanciaBE
        Dim oRow As HechosImportanciaBE.HechosImportanciaRow
        Dim strCodigo As String
        strCodigo = codigo.Split(","c)(0)
        oHechosImportanciaBE = oHechosImportanciaBM.Seleccionar(strCodigo, DatosRequest)
        oRow = DirectCast(oHechosImportanciaBE.HechosImportancia.Rows(0), HechosImportanciaBE.HechosImportanciaRow)
        hdCodigo.Value = oRow.CodigoHechos
        ddlFondo.SelectedValue = oRow.CodigoPortafolioSBS
        tbNemonico.Text = oRow.CodigoNemonico
        tbFecha.Text = UIUtility.ConvertirFechaaString(oRow.Fecha)
        tbDescripcion.Text = oRow.Hecho
        ddlSituacion.SelectedValue = oRow.Situacion
    End Sub
    Private Function ObtenerInstancia() As HechosImportanciaBE
        Dim oHechosImportanciaBE As New HechosImportanciaBE
        Dim oRow As HechosImportanciaBE.HechosImportanciaRow
        oRow = oHechosImportanciaBE.HechosImportancia.NewHechosImportanciaRow()
        oRow.CodigoHechos = Val(hdCodigo.Value.ToString)
        oRow.CodigoPortafolioSBS = ddlFondo.SelectedValue
        oRow.CodigoNemonico = tbNemonico.Text.ToString
        oRow.Fecha = UIUtility.ConvertirFechaaDecimal(tbFecha.Text.ToString)
        oRow.Hecho = tbDescripcion.Text.ToString
        oRow.Situacion = ddlSituacion.SelectedValue
        oHechosImportanciaBE.HechosImportancia.AddHechosImportanciaRow(oRow)
        oHechosImportanciaBE.HechosImportancia.AcceptChanges()
        Return oHechosImportanciaBE
    End Function
    Private Sub CargarCombos()
        Dim oPortafolioBM As New PortafolioBM
        ddlFondo.DataSource = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        ddlFondo.DataValueField = "CodigoPortafolio"
        ddlFondo.DataTextField = "Descripcion"
        ddlFondo.DataBind()
        Dim it As ListItem = New ListItem("Todos", "Todos")
        ddlFondo.Items.Insert(0, it)
        ddlFondo.SelectedValue = 1

        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
        ddlSituacion.SelectedValue = ESTADO_ACTIVO
        Dim dFecha As Decimal = UIUtility.ObtenerFechaNegocio(ddlFondo.SelectedValue)
        dFecha = UIUtility.ObtenerFechaNegocio(ddlFondo.SelectedValue)
        tbFecha.Text = UIUtility.ConvertirFechaaString(dFecha)
    End Sub
    Public Sub LimpiarCampos()
        ddlFondo.SelectedIndex = 0
        tbNemonico.Text = Constantes.M_STR_TEXTO_INICIAL
        tbFecha.Text = Constantes.M_STR_TEXTO_INICIAL
        tbDescripcion.Text = Constantes.M_STR_TEXTO_INICIAL
        ddlSituacion.SelectedIndex = 0
    End Sub
#End Region
End Class