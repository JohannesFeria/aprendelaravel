Imports SIT.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports ParametrosSIT

Partial Class Modulos_Parametria_Tablas_Generales_frmLocacionBBH
    Inherits BasePage

#Region "/* Metodos Personalizados */"

    Private Sub Aceptar()
        If Me.hdCodigo.Value.Equals(String.Empty) Then
            Insertar()
        Else
            Modificar()
        End If
    End Sub

    Private Sub Insertar()
        Dim oLocacionBBHBM As New LocacionBBHBM
        Dim oLocacionBBHBE As LocacionBBHBE
        oLocacionBBHBE = Me.ObtenerInstancia()
        oLocacionBBHBM.Insertar(oLocacionBBHBE, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_INSERTAR_ENTIDAD)
        LimpiarCampos()
    End Sub

    Private Sub Modificar()
        Dim oLocacionBBHBM As New LocacionBBHBM
        Dim oLocacionBBHBE As LocacionBBHBE
        oLocacionBBHBE = Me.ObtenerInstancia()
        oLocacionBBHBM.Modificar(oLocacionBBHBE, DatosRequest)
        AlertaJS(Constantes.M_STR_MENSAJE_ACTUALIZAR_ENTIDAD)
    End Sub

    Private Sub CargarControles(ByVal codigo As String)

        Dim oLocacionBBHBM As New LocacionBBHBM
        Dim oLocacionBBHBE As LocacionBBHBE
        Dim oRow As LocacionBBHBE.LocacionBBHRow
        Dim strCodigo As String

        strCodigo = codigo.Split(","c)(0)
        oLocacionBBHBE = oLocacionBBHBM.Seleccionar(strCodigo, Me.DatosRequest)
        oRow = DirectCast(oLocacionBBHBE.LocacionBBH.Rows(0), LocacionBBHBE.LocacionBBHRow)
        Me.hdCodigo.Value = oRow.CodigoLocacion
        Me.tbMercado.Text = oRow.Mercado
        Me.tbLocacion.Text = oRow.SOD_Name
        Me.tbLocacionCustodio.Text = oRow.NemoLocacion
        Me.tbSetLocCusatodio.Text = oRow.Settlement_location
        Me.tbCostoTrans.Text = Format(oRow.Precio_Trans, "##,##0.0000000")
        Me.tbTasa.Text = Format(oRow.Tasa_Custodio, "##,##0.0000000")
        Me.ddlSituacion.SelectedValue = oRow.Situacion
    End Sub

    Private Function ObtenerInstancia() As LocacionBBHBE
        Dim oLocacionBBHBE As New LocacionBBHBE
        Dim oRow As LocacionBBHBE.LocacionBBHRow
        oRow = oLocacionBBHBE.LocacionBBH.NewLocacionBBHRow()
        oRow.CodigoLocacion = Val(Me.hdCodigo.Value.ToString)
        oRow.Mercado = Me.tbMercado.Text.ToString
        oRow.SOD_Name = Me.tbLocacion.Text.ToString
        oRow.NemoLocacion = Me.tbLocacionCustodio.Text.ToString
        oRow.Settlement_location = Me.tbSetLocCusatodio.Text.ToString
        oRow.Precio_Trans = Me.tbCostoTrans.Text.ToString.Replace(",", "")
        oRow.Tasa_Custodio = Me.tbTasa.Text.ToString.Replace(",", "")
        oRow.Situacion = Me.ddlSituacion.SelectedValue
        oLocacionBBHBE.LocacionBBH.AddLocacionBBHRow(oRow)
        oLocacionBBHBE.LocacionBBH.AcceptChanges()
        Return oLocacionBBHBE
    End Function

    Private Sub CargarCombos()

        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
        ddlSituacion.SelectedValue = ESTADO_ACTIVO

    End Sub

    Public Sub LimpiarCampos()

        Me.tbMercado.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.tbLocacion.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.tbLocacionCustodio.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.tbSetLocCusatodio.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.tbCostoTrans.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.tbTasa.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.ddlSituacion.SelectedIndex = 0

    End Sub

    Private Sub CargarPagina()
        If Not Page.IsPostBack Then
            CargarCombos()
            If Not (Request.QueryString("codigo") = Nothing) Then
                Me.hdCodigo.Value = Request.QueryString("codigo")
                CargarControles(Me.hdCodigo.Value)
            Else
                Me.hdCodigo.Value = String.Empty
            End If
        End If
    End Sub

#End Region

#Region "/* Eventos de la Pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            CargarPagina()
        Catch ex As Exception            
            AlertaJS("Ocurrió un error al cargar la página")
        End Try

    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Aceptar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaLocacionBBH.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al retornar la página")
        End Try
    End Sub

#End Region

End Class
