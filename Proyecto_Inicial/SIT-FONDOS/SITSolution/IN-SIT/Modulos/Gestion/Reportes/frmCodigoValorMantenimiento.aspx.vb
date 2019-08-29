Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Partial Class Modulos_Gestion_Reportes_frmCodigoValorMantenimiento
    Inherits BasePage
    Dim oCodigoValoBM As New CodigoValorBM
#Region "Métodos de la página"
    Function Validar() As String
        Dim resultado As String = String.Empty
        If hdemisor.Value = "" Then
            resultado = "Seleccione un emisor"
        ElseIf txtcodigovalor.Text = "" Then
            resultado = "Ingrese el codigo valor"
        End If
        Return resultado
    End Function
    Sub MuestraOpcion()
        If ddlopcion.SelectedValue = "0" Then
            rowTipoTasa.Visible = True
        Else
            rowTipoTasa.Visible = False
        End If
    End Sub
    Sub CargaTipoInstrumento()
        Dim dtTipoInstrumento As DataTable
        Dim oCodigoValor As New CodigoValorBM
        dtTipoInstrumento = oCodigoValor.TipoInstrumento_SMV()
        HelpCombo.LlenarComboBox(ddlTipoInstumentoSMV, dtTipoInstrumento, "CodigoTipoInstrumentoSMV", "Descripcion", True, "Seleccione")
    End Sub
    Private Function CrearObjetoCodigoValor() As CodigoValorBE
        Using oCVBE As New CodigoValorBE
            Dim id As Integer = 0
            Dim estadoCV As String = Request.QueryString("estadoCV")
            id = Integer.Parse(IIf(estadoCV = "Modificar", Request.QueryString("id"), "0"))
            Dim oRow As CodigoValorBE.CodigoValorBERow = oCVBE._CodigoValorBE.NewCodigoValorBERow
            oRow.Id = id
            oRow.CodigoValor = txtcodigovalor.Text
            oRow.CodigoEntidad = hdemisor.Value
            oRow.CodigoMoneda = ""
            oRow.CodigoNemonico = txtnemonico.Text
            oRow.Situacion = ddlestado.SelectedValue
            oRow.TipoInstrumento = ddlTipoInstumentoSMV.SelectedValue
            oRow.Opcion = ddlopcion.SelectedValue
            oRow.CodigoTipoCupon = ddltipoTasa.SelectedValue
            oCVBE._CodigoValorBE.AddCodigoValorBERow(oRow)
            oCVBE._CodigoValorBE.AcceptChanges()
            Return oCVBE
        End Using
    End Function
    Private Sub CargarRegistro()
        Dim id As Integer = Decimal.Parse(Request.QueryString("id"))
        Dim oCVBE As CodigoValorBE = oCodigoValoBM.ListarCodigoValor(id, "", "", "", "", "", "")
        Dim oRow As CodigoValorBE.CodigoValorBERow = DirectCast(oCVBE._CodigoValorBE.Rows(0), CodigoValorBE.CodigoValorBERow)
        txtcodigovalor.Text = oRow.CodigoValor
        txtCodIsin.Text = oRow.CodigoISIN
        txtnemonico.Text = oRow.CodigoNemonico
        hdemisor.Value = oRow.CodigoEntidad
        ddlopcion.SelectedValue = oRow.Opcion
        ddlTipoInstumentoSMV.SelectedValue = oRow.TipoInstrumento
        txtEmisor.Text = oRow.Sinonimo
        ddlestado.SelectedValue = oRow.Situacion
        ddltipoTasa.SelectedValue = oRow.CodigoTipoCupon
    End Sub
#End Region
#Region "Eventos de la página"
    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim validacion As String = Validar()
        Try
            If validacion = String.Empty Then
                Dim estadoCV As String = Request.QueryString("estadoCV")
                Using oCVBE As CodigoValorBE = CrearObjetoCodigoValor()
                    If estadoCV = "Nuevo" Then
                        oCodigoValoBM.InsertarCodigoValor(oCVBE, DatosRequest)
                        'Session.Remove("EstadoCV")
                        Response.Redirect("frmCodigoValor.aspx")
                    ElseIf estadoCV = "Modificar" Then
                        oCodigoValoBM.ModificarCodigoValor(oCVBE, DatosRequest)
                        AlertaJS("Se modifico correctamente el registro")
                    End If
                End Using
                'Dim oCVBE As New CodigoValorBE
                'Dim oRow As CodigoValorBE.CodigoValorBERow = oCVBE._CodigoValorBE.NewCodigoValorBERow
                'oRow.CodigoValor = txtcodigovalor.Text
                'oRow.CodigoEntidad = hdemisor.Value
                'oRow.CodigoMoneda = ""
                'oRow.CodigoNemonico = txtnemonico.Text
                'oRow.Situacion = ddlestado.SelectedValue
                'oRow.TipoInstrumento = ddlTipoInstumentoSMV.SelectedValue
                'oRow.Opcion = ddlopcion.SelectedValue
                'oRow.CodigoTipoCupon = ddltipoTasa.SelectedValue
                'oCVBE._CodigoValorBE.AddCodigoValorBERow(oRow)
                'oCVBE._CodigoValorBE.AcceptChanges()
                'If Session("EstadoCV") = "Nuevo" Then
                '    oCodigoValoBM.InsertarCodigoValor(oCVBE, DatosRequest)
                '    Session.Remove("EstadoCV")
                '    Response.Redirect("frmCodigoValor.aspx")
                'ElseIf Session("EstadoCV") = "Modificar" Then
                '    oCodigoValoBM.ModificarCodigoValor(oCVBE, DatosRequest)
                '    AlertaJS("Se modifico correctamente el registro")
                'End If
            Else
                AlertaJS(validacion)
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Session("SS_DatosModal") Is Nothing Then
                If hdBusqueda.Value = "Emisor" Then
                    txtEmisor.Text = CType(Session("SS_DatosModal"), String())(1).ToString()
                    hdemisor.Value = CType(Session("SS_DatosModal"), String())(0).ToString()
                Else
                    Dim datos As String()
                    datos = CType(Session("SS_DatosModal"), String())
                    txtCodIsin.Text = HttpUtility.HtmlDecode(CType(Session("SS_DatosModal"), String())(0).ToString())
                    If ddlopcion.SelectedIndex = "1" Then
                        txtcodigovalor.Text = HttpUtility.HtmlDecode(CType(Session("SS_DatosModal"), String())(0).ToString())
                    Else
                        txtcodigovalor.Text = ""
                    End If
                    txtnemonico.Text = CType(Session("SS_DatosModal"), String())(1).ToString()
                End If
                Session.Remove("SS_DatosModal")
            End If
            If Not Page.IsPostBack Then
                Dim estadoCV As String = Request.QueryString("estadoCV")
                CargaTipoInstrumento()
                If estadoCV = "Modificar" Then
                    CargarRegistro()
                    txtnemonico.Enabled = False
                    txtCodIsin.Enabled = False
                End If
                MuestraOpcion()
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub btnRegresar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRegresar.Click
        Response.Redirect("frmCodigoValor.aspx")
        'Session.Remove("EstadoCV")
        'Session.Remove("CV")
    End Sub
    Protected Sub ddlopcion_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlopcion.SelectedIndexChanged
        MuestraOpcion()
        If ddlopcion.SelectedIndex = "1" Then
            txtcodigovalor.Text = txtCodIsin.Text
        Else
            txtcodigovalor.Text = ""
        End If
    End Sub
#End Region
End Class