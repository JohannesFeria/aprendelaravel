Imports System.Data
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities

Partial Class Modulos_Parametria_Tablas_Valores_frmPatrimonioEmisor
    Inherits BasePage

#Region "Eventos de la página"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombo()
                CargarFecha()
                CargarRegistro()
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                Select Case hdTipoModal.Value
                    Case "EMI"
                        tbEmisor.Text = CType(Session("SS_DatosModal"), String())(0)
                        tbEmisorDesc.Text = CType(Session("SS_DatosModal"), String())(1)
                End Select
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página: " & Replace(ex.Message, "'", ""))
        Finally
            If Session("SS_DatosModal") IsNot Nothing Then
                Session.Remove("SS_DatosModal")
            End If
        End Try
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            SaveRegister()
            LimpiarRegistro()
            AlertaJS("El registro se guardó correctamente")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al guardar el registro: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("frmBusquedaPatrimonioValor.aspx")
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub

#End Region

#Region "Métodos de la página"

    Private Sub CargarCombo()
        CargarComboTipoValor()
    End Sub

    Private Sub CargarComboTipoValor()
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim dt As DataTable = Nothing
        dt = oParametrosGenerales.SeleccionarPorFiltro("PATEMI_TIPVAL", "", "", "", DatosRequest)
        HelpCombo.LlenarComboBox(ddlTipoValor, dt, "Valor", "Nombre", True)
    End Sub

    Private Sub CargarFecha()
        txtFecha.Text = FechaActual
    End Sub

    Private Function CreateObjectPatrimonioEmisor(ByRef objPE_BE As PatrimonioEmisorBE) As PatrimonioEmisorBE
        objPE_BE = New PatrimonioEmisorBE
        If Request.QueryString("Id") IsNot Nothing Then
            objPE_BE.id = Integer.Parse(Request.QueryString("Id"))
        End If
        If tbEmisor.Text.Equals("") Then
            Throw New Exception("Seleccione un emisor")
        Else
            objPE_BE.codigoEntidad = tbEmisor.Text
        End If
        objPE_BE.codigoTercero = ""
        If ddlTipoValor.SelectedValue.Equals("") Then
            Throw New Exception("Seleccione el tipo valor")
        Else
            objPE_BE.tipoValor = ddlTipoValor.SelectedValue
        End If
        objPE_BE.valor = Decimal.Parse(txtValor.Text)
        If IsDate(UIUtility.ConvertirStringaFecha(txtFecha.Text)) Then
            objPE_BE.fecha = UIUtility.ConvertirFechaaDecimal(txtFecha.Text)
        Else
            Throw New Exception("Seleccione una fecha válida")
        End If
        Return objPE_BE
    End Function

    Private Sub SaveRegister()
        Dim objPatriminioEmisorBE As PatrimonioEmisorBE = Nothing
        Dim objPatrimonioEmisorBM As New PatrimonioEmisorBM
        objPatriminioEmisorBE = CreateObjectPatrimonioEmisor(objPatriminioEmisorBE)
        If Request.QueryString("Operacion") = "Ingresar" Then
            objPatrimonioEmisorBM.Insertar(objPatriminioEmisorBE, DatosRequest)
        Else
            objPatrimonioEmisorBM.Actualizar(objPatriminioEmisorBE, DatosRequest)
        End If
    End Sub

    Private Sub LimpiarRegistro()
        tbEmisor.Text = String.Empty
        tbEmisorDesc.Text = String.Empty
        ddlTipoValor.SelectedValue = ""
        CargarFecha()
        txtValor.Text = String.Empty
    End Sub

    Private Sub CargarRegistro()
        If Request.QueryString("Operacion").ToString() = "Modificar" Then
            Dim objPE_BM As New PatrimonioValorBM
            Dim dt As DataTable = Nothing
            Dim fecha As Integer = Integer.Parse(Request.QueryString("Fecha"))
            dt = objPE_BM.SeleccionarPatrimonioEmisor(Integer.Parse(Request.QueryString("Id")), "", "", "", fecha, fecha)
            If dt.Rows.Count > 0 Then
                tbEmisor.Text = dt.Rows(0)("CodigoEntidad")
                tbEmisorDesc.Text = dt.Rows(0)("DescripcionEmisor")
                ddlTipoValor.SelectedValue = dt.Rows(0)("TipoValor")
                txtValor.Text = dt.Rows(0)("Valor")
                txtFecha.Text = UIUtility.ConvertirFechaaString(dt.Rows(0)("Fecha").ToString())
            End If
        End If
    End Sub

#End Region

End Class
