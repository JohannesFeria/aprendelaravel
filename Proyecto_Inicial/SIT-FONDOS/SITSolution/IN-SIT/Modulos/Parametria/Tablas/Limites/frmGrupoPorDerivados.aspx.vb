Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Text
Imports System.Data
Partial Class Modulos_Parametria_Tablas_Limites_frmGrupoPorDerivados
    Inherits BasePage
    Sub Limpiar()
        txtCodigoGrupo.Text = ""
        txtDescripcion.Text = ""
        ddlSituacion.SelectedValue = "1"
        chklOpciones.Items(0).Selected = 0
        chklOpciones.Items(1).Selected = 0
        chklOpciones.Items(2).Selected = 0
        chklOpciones.Items(3).Selected = 0
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                If Not Session("Codigo") Is Nothing Then
                    Dim oLimParametria As New LimiteParametriaBM
                    Dim dtdatos As DataTable = oLimParametria.GrupoPorDerivados_Seleccionar(Session("Codigo"))
                    txtCodigoGrupo.Text = dtdatos(0)("CodigoGrupoDerivado").ToString()
                    txtDescripcion.Text = dtdatos(0)("Descripcion").ToString()
                    ddlSituacion.SelectedValue = dtdatos(0)("Situacion").ToString()
                    chklOpciones.Items(0).Selected = IIf(dtdatos(0)("MonedaPortafolio").ToString() = "S", True, False)
                    chklOpciones.Items(1).Selected = IIf(dtdatos(0)("Forward").ToString() = "S", True, False)
                    chklOpciones.Items(2).Selected = IIf(dtdatos(0)("Swap").ToString() = "S", True, False)
                    chklOpciones.Items(3).Selected = IIf(dtdatos(0)("NominalRecibir").ToString() = "S", True, False)
                    If Session("EstadoPag") = "M" Then
                        txtCodigoGrupo.Enabled = False
                    End If
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página: " & Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim oLimParametria As New LimiteParametriaBM
            If Session("Codigo") Is Nothing Then
                oLimParametria.GrupoPorDerivados_Insertar(txtCodigoGrupo.Text, txtDescripcion.Text,
                IIf(chklOpciones.Items(0).Selected, "S", "N"),IIf(chklOpciones.Items(1).Selected, "S", "N"),
                IIf(chklOpciones.Items(2).Selected, "S", "N"),IIf(chklOpciones.Items(3).Selected, "S", "N"),
                ddlSituacion.SelectedValue, DatosRequest)
                Limpiar()
            Else
                oLimParametria.GrupoPorDerivados_Modificar(txtCodigoGrupo.Text, txtDescripcion.Text,
                IIf(chklOpciones.Items(0).Selected, "S", "N"), IIf(chklOpciones.Items(1).Selected, "S", "N"),
                IIf(chklOpciones.Items(2).Selected, "S", "N"), IIf(chklOpciones.Items(3).Selected, "S", "N"),
                ddlSituacion.SelectedValue, DatosRequest)
            End If
            AlertaJS("Operacion ejecutada correctamente")
        Catch ex As Exception
            AlertaJS("Error:" & Replace(ex.Message, "'", ""))
        End Try
    End Sub
    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Response.Redirect("frmBusquedaLimiteParametria.aspx")
    End Sub
End Class
