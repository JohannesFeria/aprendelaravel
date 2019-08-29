Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Public Class Modulos_PrevisionPagos_frmParametriaMemo
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarTipoOperacion()
            CargarTipoReporte()
        End If
    End Sub

    Protected Sub ddlTipoReporte_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlTipoReporte.SelectedIndexChanged
        If ddlTipoReporte.SelectedValue.ToString() = "TIP1" Then
            ddlTipoOperacion.Enabled = True
        Else
            ddlTipoOperacion.SelectedIndex = 0
            ddlTipoOperacion.Enabled = False
            CargarDatosMemo()
        End If
    End Sub

    Protected Sub ddlTipoOperacion_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlTipoOperacion.SelectedIndexChanged
        CargarDatosMemo()
    End Sub

    Protected Sub bSalir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bSalir.Click
        'Response.Redirect("../Bienvenida.aspx", False)
        Response.Redirect("frmGenerarMemo.aspx", False)
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        Dim objBM As New PrevisionMemoBM()
        Dim objMemoBE As New PrevisionMemo()

        objMemoBE.TipoReporte = ddlTipoReporte.SelectedValue()
        objMemoBE.IdTipoOperacion = ddlTipoOperacion.SelectedValue()
        objMemoBE.DescripcionA = tbDescripcionA.Text.Trim()
        objMemoBE.DescripcionDe = tbDescripcionDe.Text.Trim()
        objMemoBE.Referencia = tbReferencia.Text.Trim()
        objMemoBE.Contenido = tbContenido.Text.Trim()
        objMemoBE.Despedida = tbDespedida.Text.Trim()
        objMemoBE.UsuarioFirma = tbUsuarioFirma.Text.Trim()
        objMemoBE.AreaUsuarioFirma = tbAreaUsuarioFirma.Text.Trim()
        objMemoBE.InicialesDocumentador = tbInicialesDocumentador.Text.Trim()

        Dim result As Integer
        Try
            result = objBM.InsertarParametriaMemo(objMemoBE)
            If result = 1 Then
                AlertaJS("Datos registrados correctamente.")
            Else
                AlertaJS("Ocurrió un error en el sistema.")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en el sistema.")
        End Try
    End Sub

    Private Sub CargarTipoReporte()
        Dim objBM As New PrevisionParametroBM()
        Dim ds As New DataSet()
        ds = objBM.SeleccionarPorCodigo("11")
        HelpCombo.LlenarComboBox(ddlTipoReporte, ds.Tables(0), "Valor", "descripcion", True)
    End Sub

    Private Sub CargarTipoOperacion()
        Dim objBM As New PrevisionParametroBM()
        Dim ds As New DataSet()
        ds = objBM.SeleccionarPorCodigo("4")
        HelpCombo.LlenarComboBox(ddlTipoOperacion, ds.Tables(0), "Valor", "descripcion", True)
    End Sub

    Private Sub CargarDatosMemo()
        Dim objBM As New PrevisionMemoBM()
        Dim objMemoBE As PrevisionMemo
        objMemoBE = objBM.ListarDatosMemo(ddlTipoReporte.SelectedValue, ddlTipoOperacion.SelectedValue)

        If Not objMemoBE Is Nothing Then
            tbDescripcionA.Text = objMemoBE.DescripcionA
            tbDescripcionDe.Text = objMemoBE.DescripcionDe
            tbReferencia.Text = objMemoBE.Referencia
            tbContenido.Text = objMemoBE.Contenido
            tbDespedida.Text = objMemoBE.Despedida
            tbUsuarioFirma.Text = objMemoBE.UsuarioFirma
            tbAreaUsuarioFirma.Text = objMemoBE.AreaUsuarioFirma
            tbInicialesDocumentador.Text = objMemoBE.InicialesDocumentador
        End If

    End Sub

    Protected Sub Alerta(ByVal mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), New Guid().ToString(), "<script language='javascript'>alert('" & mensaje & "');</script>", False)
    End Sub
End Class
