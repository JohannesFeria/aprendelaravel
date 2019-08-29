Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmTipoCambio
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load        
        If Not Page.IsPostBack Then
            CargarCombos()
            If Not (Request.QueryString("cod") = Nothing) Then
                hd.Value = Request.QueryString("cod")
                CargarRegistro(hd.Value)
            Else
                hd.Value = ""
            End If
        End If
    End Sub

    Public Sub CargarRegistro(ByVal codigo As String)
        Dim oTipoCambioDI_BM As New TipoCambioDI_BM
        Dim oDS As DataSet
        oDS = oTipoCambioDI_BM.SeleccionarPorFiltros(codigo, "", "", "", "", DatosRequest)
        ddlMonedaOrigen.SelectedValue = oDS.Tables(0).Rows(0)("CodigoMonedaOrigen")
        ddlMonedaDestino.SelectedValue = oDS.Tables(0).Rows(0)("CodigoMonedaDestino")
        ddlTipo.SelectedValue = oDS.Tables(0).Rows(0)("Tipo")
        ddlSituacion.SelectedValue = oDS.Tables(0).Rows(0)("Situacion").ToString.Substring(0, 1)
        txtDescripcion.Text = oDS.Tables(0).Rows(0)("Observaciones")
    End Sub

    Sub CargarCombos()
        UIUtility.CargarMonedaSituacionOI(ddlMonedaOrigen, "")
        UIUtility.CargarMonedaSituacionOI(ddlMonedaDestino, "")

        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)

        ddlTipo.Items.Add("Directo")
        ddlTipo.Items.Add("Indirecto")
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Response.Redirect("frmBusquedaTipoCambio.aspx")
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim oTipoCambioDI_BM As New TipoCambioDI_BM
        Dim oTipoCambioDI_BE As New TipoCambioDI_BE
        Try
            oTipoCambioDI_BE = crearObjeto()
            If ddlMonedaOrigen.SelectedValue = ddlMonedaDestino.SelectedValue Then                
                AlertaJS("No puede ingresar cuando la divisa y moneda son iguales")
                Exit Sub
            End If

            If Me.hd.Value = "" Then
                If verificarExistenciaTipoCambio() = True Then                    
                    AlertaJS("Este registro ya existe")
                    Exit Sub
                End If
                oTipoCambioDI_BM.Insertar(oTipoCambioDI_BE, DatosRequest)                
                AlertaJS("Los datos fueron grabados correctamente")
                LimpiarCampos()
            Else
                If verificarExistenciaTipoCambioModificado() = True Then                    
                    AlertaJS("Este registro ya existe")
                    Exit Sub
                End If
                oTipoCambioDI_BM.Modificar(oTipoCambioDI_BE, DatosRequest)
                AlertaJS("Los datos fueron modificados correctamente")
            End If
        Catch ex As Exception            
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

    Sub LimpiarCampos()
        Me.ddlMonedaOrigen.SelectedIndex = 0
        Me.ddlMonedaDestino.SelectedIndex = 0
        Me.ddlTipo.SelectedIndex = 0
        Me.ddlSituacion.SelectedIndex = 0
    End Sub

    Public Function crearObjeto() As TipoCambioDI_BE
        Dim oTipoCambioDI_BE As New TipoCambioDI_BE
        Dim oRow As TipoCambioDI_BE.TipoCambioDIRow
        oRow = CType(oTipoCambioDI_BE.TipoCambioDI.NewTipoCambioDIRow(), TipoCambioDI_BE.TipoCambioDIRow)

        oRow.CodigoMonedaOrigen = ddlMonedaOrigen.SelectedValue
        oRow.CodigoMonedaDestino = ddlMonedaDestino.SelectedValue
        oRow.Tipo = ddlTipo.SelectedValue.Substring(0, 1)
        oRow.Situacion = Me.ddlSituacion.SelectedValue
        oRow.Observaciones = Me.txtDescripcion.Text

        If hd.Value <> "" Then
            oRow.CodigoTipoCambio = hd.Value
        Else
            oRow.CodigoTipoCambio = ""
        End If

        oTipoCambioDI_BE.TipoCambioDI.AddTipoCambioDIRow(oRow)
        oTipoCambioDI_BE.TipoCambioDI.AcceptChanges()

        Return oTipoCambioDI_BE
    End Function

    Private Function verificarExistenciaTipoCambio() As Boolean
        Dim oTipoCambioDI_BM As New TipoCambioDI_BM
        Dim oDS As DataSet
        oDS = oTipoCambioDI_BM.SeleccionarPorFiltros("", ddlMonedaOrigen.SelectedValue, ddlMonedaDestino.SelectedValue, "", "", datosrequest)
        If oDS.Tables(0).Rows.Count > 0 Then
            verificarExistenciaTipoCambio = True
        Else
            verificarExistenciaTipoCambio = False
        End If
    End Function

    Private Function verificarExistenciaTipoCambioModificado() As Boolean
        Dim oTipoCambioDI_BM As New TipoCambioDI_BM
        Dim oDS As DataSet
        oDS = oTipoCambioDI_BM.SeleccionarPorFiltros("", ddlMonedaOrigen.SelectedValue, ddlMonedaDestino.SelectedValue, "", "", datosrequest)
        If oDS.Tables(0).Rows.Count > 0 Then
            If hd.Value = oDS.Tables(0).Rows(0)("CodigoTipoCambio") Then
                verificarExistenciaTipoCambioModificado = False
            Else
                verificarExistenciaTipoCambioModificado = True
            End If
        Else
            verificarExistenciaTipoCambioModificado = False
        End If
    End Function

End Class
