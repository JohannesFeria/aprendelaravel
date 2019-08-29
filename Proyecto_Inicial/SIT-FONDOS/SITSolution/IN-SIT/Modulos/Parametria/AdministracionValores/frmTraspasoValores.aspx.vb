Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Partial Class Modulos_Parametria_AdministracionValores_frmTraspasoValores
    Inherits BasePage
    Protected Sub btnAceptar_Click(sender As Object, e As System.EventArgs) Handles btnAceptar.Click
        Try
            If txtCodigoIsinOrigen.Text = "" Then
                AlertaJS("Ingrese el Codigo Isin de Origen.")
            ElseIf txtCodigoIsinDestino.Text = "" Then
                AlertaJS("Ingrese el Codigo Isin de Destino.")
            Else
                Dim ovaloresBM As New ValoresBM
                Dim Mensaje As String = ovaloresBM.ValidaTraspaso(txtCodigoIsinOrigen.Text, txtCodigoIsinDestino.Text)
                If Mensaje <> "" Then
                    AlertaJS(Mensaje)
                Else
                    Dim dt As DataTable = ovaloresBM.InsertaTraspasoValores(txtCodigoIsinOrigen.Text, txtCodigoIsinDestino.Text, UIUtility.ConvertirFechaaDecimal(txtFechaProceso.Text), DatosRequest)
                    dgLista.DataSource = dt
                    dgLista.DataBind()
                    AlertaJS("Se relizo el traspaso correctamente.")
                End If
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Session("SS_DatosModal") Is Nothing Then
            Dim datos As String() = CType(Session("SS_DatosModal"), String())
            If hdBusqueda.Value = "Origen" Then
                txtCodigoIsinOrigen.Text = HttpUtility.HtmlDecode(CType(Session("SS_DatosModal"), String())(0).ToString())
                txtCodigoNemonicoOrigen.Text = CType(Session("SS_DatosModal"), String())(1).ToString()
            Else
                txtCodigoIsinDestino.Text = HttpUtility.HtmlDecode(CType(Session("SS_DatosModal"), String())(0).ToString())
                txtCodigoNemonicoDestino.Text = CType(Session("SS_DatosModal"), String())(1).ToString()
            End If
            Session.Remove("SS_DatosModal")
        End If
        If Not IsPostBack Then
            txtFechaProceso.Text = UIUtility.ConvertirDecimalAStringFormatoFecha(UIUtility.ObtenerFechaMaximaNegocio())
        End If
    End Sub
End Class