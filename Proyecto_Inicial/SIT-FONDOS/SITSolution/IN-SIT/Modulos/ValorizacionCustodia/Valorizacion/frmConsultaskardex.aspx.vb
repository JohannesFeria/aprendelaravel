Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Text
Imports System.Data

Partial Class Modulos_Valorizacion_y_Custodia_Valorizacion_frmConsultaskardex
    Inherits BasePage

    Dim oUtil As New UtilDM

#Region "/* Métodos de la Página */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                Me.tbFechaInicio.Text = oUtil.RetornarFechaSistema
                Me.tbFechaFin.Text = oUtil.RetornarFechaSistema
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                txtISIN.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                txtMnemonico.Text = CType(Session("SS_DatosModal"), String())(1).ToString()
                txtsbs.Text = CType(Session("SS_DatosModal"), String())(2).ToString()
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try        
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Try
            Dim mensaje As String
            mensaje = Validar()
            If mensaje <> "" Then
                AlertaJS(mensaje)
                Exit Sub
            Else
                Dim decfechainicio, decfechafin As Decimal
                Dim sportafolio As String
                decfechainicio = UIUtility.ConvertirFechaaDecimal(Me.tbFechaInicio.Text)
                decfechafin = UIUtility.ConvertirFechaaDecimal(Me.tbFechaFin.Text)
                If ddlFondo.SelectedValue <> "" Then
                    sportafolio = ddlFondo.SelectedValue
                Else
                    AlertaJS("Seleccione un portafolio")
                    Exit Sub
                End If
                EjecutarJS("showPopup('" + sportafolio + "','" + Me.ddlTipoInstrumento.SelectedValue + "','" + Me.txtMnemonico.Text + "','" + Me.txtISIN.Text + "','" + Me.txtsbs.Text + "','" + Convert.ToString(decfechainicio) + "','" + Convert.ToString(decfechafin) + "','" + Me.rbtReporte.SelectedValue + "')")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Imprimir")
        End Try       
    End Sub

    Protected Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try
    End Sub

#End Region

#Region "/* Métodos Personalizados */"

    Public Function Validar() As String
        Dim msg As String = ""
        Dim strMensajeError As String = ""

        If Me.tbFechaInicio.Text.Trim = "" Then
            msg += "\t-Fecha Inicio\n"
        End If
        If Me.ddlFondo.SelectedValue = "--Seleccione--" Then
            msg += "\t-Fondo\n"
        End If
        If (msg <> "") Then
            strMensajeError = "Los siguientes campos son obligatorios:\n" + msg + "\n"
            Return strMensajeError
        Else
            Return ""
        End If
    End Function

    Public Sub CargarCombos()
        Dim DtTablaTipoInstrumento As New DataTable
        Dim Dtablafondo As New DataTable
        Dim oPortafolioBM As New PortafolioBM
        Dim oTipoInstrumentoBM As New TipoInstrumentoBM

        DtTablaTipoInstrumento = oTipoInstrumentoBM.Listar(DatosRequest).Tables(0)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlTipoInstrumento, DtTablaTipoInstrumento, "CodigoTipoInstrumentoSBS", "Descripcion", True)

        Dtablafondo = oPortafolioBM.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
        HelpCombo.LlenarComboBox(Me.ddlFondo, Dtablafondo, "CodigoPortafolio", "Descripcion", True)
    End Sub

#End Region

End Class
