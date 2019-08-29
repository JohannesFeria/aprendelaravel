Imports Sit.BusinessEntities
Imports SIT.BusinessLayer
Imports System.Globalization
Imports System.Collections.Generic
Imports System.Data

Public Class Modulos_PrevisionPagos_frmPagosDetalle
    Inherits BasePage
    'Dim IdUsuario As String = Usuario.ToString

    Private Function Validar2() As Boolean

        Dim fechaHoraHoy As DateTime = DateTime.ParseExact(System.DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
        Dim fechaHoraParametro As DateTime = Convert.ToDateTime(Convert.ToString(System.DateTime.Now().ToString("dd/MM/yyyy")).Substring(0, 10) + " " + lblHoraCierre.Text.ToString + ":00")
        Dim fechaFutura As DateTime = Convert.ToDateTime(System.DateTime.Now().ToString("dd/MM/yyyy")).AddDays(1)
        If UIUtility.ConvertirFechaaDecimal(txtFechaPago.Text.Trim) < UIUtility.ConvertirFechaaDecimal(System.DateTime.Now.ToString("dd/MM/yyyy")) Then
            'Response.Write("<script>window.alert('La fecha de pago no debe ser anterior a la de hoy')</script>")
            AlertaJS("La fecha de pago no debe ser anterior a la de hoy.")
            Return False
        ElseIf fechaHoraHoy > fechaHoraParametro And (UIUtility.ConvertirFechaaDecimal(txtFechaPago.Text) < UIUtility.ConvertirFechaaDecimal(fechaFutura)) Then
            'Response.Write("<script>window.alert('La hora actual supera la hora de cierre, eliga una fecha futura.')</script>")
            AlertaJS("La hora actual supera la hora de cierre, elija una fecha futura.")
            Return False
        End If
        Return True
    End Function

    Private Function ValidarFormato() As Boolean
        Dim dato As DateTime
        Dim estado As Boolean
        estado = DateTime.TryParseExact(txtFechaPago.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, dato)
        If estado = False Then
            Return False
        End If
        Return True
    End Function

    Private Function Validar() As Boolean
        If txtFechaPago.Text = String.Empty Then
            Return False
        End If
        If ddlMoneda.SelectedIndex = 0 Then
            Return False
        End If
        If txtImporte.Text = String.Empty Then
            Return False
        End If
        If ddlCtaOrigen.SelectedIndex = 0 Then
            Return False
        Else
            If ddlBancoOrigen.Items.Count > 0 And ddlBancoOrigen.SelectedIndex = 0 Then
                Return False
            End If
        End If
        If ddlCtaDestino.SelectedIndex = 0 Then
            Return False
        Else
            If ddlBancoDestino.Items.Count > 0 And ddlBancoDestino.SelectedIndex = 0 Then
                Return False
            End If
        End If
        Return True
    End Function

    Private Sub CargarCombos(ByVal ddl As DropDownList, ByVal Parametro As Int32)
        Dim tablaListaParametria As New Data.DataTable
        Dim oTipoDocumento As New TipoDocumentoBM
        tablaListaParametria = PrevisionParametriaBM.ListarParametria(Parametro).Tables(0)
        HelpCombo.LlenarComboBox(ddl, tablaListaParametria, "Valor", "Descripcion", True)
    End Sub

    Private Sub CargarHoraCierreREgistro()
        Dim ObtenerCierre As New DataSet
        ObtenerCierre = PrevisionCierreBM.ObtenerPrevisionCierre()
        lblHoraCierre.Text = ObtenerCierre.Tables(0).Rows(0)(0)
        Session("sTipoCierre1") = ObtenerCierre.Tables(0).Rows(0)(1)
    End Sub

    Private Sub CargarControles()
        ddlTipoOperacion.Items.Clear()
        ddlCtaOrigen.Items.Clear()
        ddlCtaDestino.Items.Clear()
        ddlMoneda.Items.Clear()
        CargarCombos(ddlCtaOrigen, 2)
        CargarCombos(ddlCtaDestino, 2)
        CargarCombos(ddlMoneda, 6)
        ddlBancoOrigen.Items.Clear()
        ddlBancoDestino.Items.Clear()
        ddlBancoOrigen.Enabled = False
        ddlBancoDestino.Enabled = False
        txtFechaPago.Text = String.Empty
        txtImporte.Text = String.Empty
        'CargarCombos(ddlTipoOperacion, 4)

        If ddlTipoOperacion.Items.Count = 0 Then
            HelpCombo.LlenarComboBox(ddlTipoOperacion, PrevisionPagoDetalleBM.ListarOperacionxUsuario(Usuario).Tables(0), "Valor", "Descripcion", True)
        Else
            ddlTipoOperacion.SelectedIndex = 0
        End If
    End Sub

    Private Sub PosicionarComboxToIndex(ByVal IdTipoOperacion As String, ByVal IdTipoMoneda As String, ByVal CtaOrigen As String, ByVal BancoOrigen As String, ByVal CtaDestino As String, ByVal BancoDestino As String, ByVal IdEstado As String)
        For i As Integer = 0 To ddlTipoOperacion.Items.Count - 1
            If ddlTipoOperacion.Items(i).Value = IdTipoOperacion.Trim Then
                ddlTipoOperacion.SelectedIndex = i
            End If
        Next
        For i As Integer = 0 To ddlMoneda.Items.Count - 1
            If ddlMoneda.Items(i).Value = IdTipoMoneda.Trim Then
                ddlMoneda.SelectedIndex = i
            End If
        Next
        For i As Integer = 0 To ddlCtaOrigen.Items.Count - 1
            If ddlCtaOrigen.Items(i).Value = CtaOrigen.Trim Then
                ddlCtaOrigen.SelectedIndex = i
            End If
        Next
        For i As Integer = 0 To ddlCtaDestino.Items.Count - 1
            If ddlCtaDestino.Items(i).Value = CtaDestino.Trim Then
                ddlCtaDestino.SelectedIndex = i
            End If
        Next

        ddlBancoOrigen.DataSource = PrevisionCuentasCorrientesBM.ObtenerBanco(ddlCtaOrigen.SelectedValue.ToString)
        ddlBancoOrigen.DataValueField = "Codigo"
        ddlBancoOrigen.DataTextField = "BancoCta"
        ddlBancoOrigen.DataBind()
        ddlBancoDestino.Enabled = True
        ddlBancoDestino.DataSource = PrevisionCuentasCorrientesBM.ObtenerBanco(ddlCtaDestino.SelectedValue.ToString)
        ddlBancoDestino.DataValueField = "Codigo"
        ddlBancoDestino.DataTextField = "BancoCta"
        ddlBancoDestino.DataBind()

        For i As Integer = 0 To ddlBancoOrigen.Items.Count - 1
            If ddlBancoOrigen.Items(i).Value = BancoOrigen.Trim Then
                ddlBancoOrigen.SelectedIndex = i
            End If
        Next
        For i As Integer = 0 To ddlBancoDestino.Items.Count - 1
            If ddlBancoDestino.Items(i).Value = BancoDestino.Trim Then
                ddlBancoDestino.SelectedIndex = i
            End If
        Next

        ddlEstado.Items.Remove(0)
        ddlEstado.DataBind()

        If Session("sEstado").ToString = "APROBADO" Then
            Dim n1 As Int32 = ddlEstado.Items.Count
            For i As Integer = 0 To ddlEstado.Items.Count - 1
                Dim item As String = ddlEstado.Items(i).ToString
                If ddlEstado.Items(i).Value = "PEN" Then
                    ddlEstado.Items.RemoveAt(i)
                    'ddlEstado.DataBind()
                    Exit For
                End If
            Next
        ElseIf Session("sEstado").ToString = "PENDIENTE" Then
            Dim n1 As Int32 = ddlEstado.Items.Count
            For i As Integer = 0 To ddlEstado.Items.Count - 1
                Dim item As String = ddlEstado.Items(i).ToString
                If ddlEstado.Items(i).Value = "APR" Then
                    ddlEstado.Items.RemoveAt(i)
                    'ddlEstado.DataBind()
                    Exit For
                End If
            Next
        End If

        For i As Integer = 0 To ddlEstado.Items.Count - 1
            If ddlEstado.Items(i).Value = IdEstado.Trim Then
                ddlEstado.SelectedIndex = i
            End If
        Next

    End Sub

    Private Sub InhabilitarControles()
        txtFechaPago.Enabled = False
        ddlTipoOperacion.Enabled = False
        ddlMoneda.Enabled = False
        txtImporte.Enabled = False
        ddlCtaOrigen.Enabled = False
        ddlCtaDestino.Enabled = False
        ddlBancoOrigen.Enabled = False
        ddlBancoDestino.Enabled = False
        txtUsuarioAprobacion.Enabled = False
        txtUsuarioAnulacion.Enabled = False
        txtUsuarioProvision.Enabled = False
        txtFechaAprobacion.Enabled = False
        txtFechaAnulacion.Enabled = False
        txtFechaProvision.Enabled = False
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then
                CargarHoraCierreREgistro()
                CargarControles()
                If Session("sCodigoPago") <> Nothing Then
                    CargarCombos(ddlEstado, 5)
                    Dim oDataSet As DataSet = PrevisionPagoBM.ObtenerPrevisionPago(Session("sCodigoPago").ToString)
                    With oDataSet.Tables(0) '20140319
                        Dim fecha As String = .Rows(0)(0)
                        Dim fechaProvision As String = .Rows(0)(10).ToString
                        Dim fechaAprobacion As String = .Rows(0)(12).ToString
                        Dim fechaAnulacion As String = .Rows(0)(14).ToString

                        Dim strFecha As String = fecha.Substring(6, 2) + "/" + fecha.Substring(4, 2) + "/" + fecha.Substring(0, 4)
                        Dim strFechaProvision As String '= IIf(fechaProvision.Equals("") Or fechaProvision.Equals(Nothing), String.Empty, fechaProvision.Substring(6, 2) + "/" + fechaProvision.Substring(4, 2) + "/" + fechaProvision.Substring(0, 4))
                        Dim strFechaAprobacion As String '= IIf(fechaAprobacion.Equals("") Or fechaAprobacion.Equals(Nothing), String.Empty, fechaAprobacion.Substring(6, 2) + "/" + fechaAprobacion.Substring(4, 2) + "/" + fechaAprobacion.Substring(0, 4))
                        Dim strFechaAnulacion As String '= IIf(fechaAnulacion.Equals("") Or fechaAnulacion.Equals(Nothing), String.Empty, fechaAnulacion.Substring(6, 2) + "/" + fechaAnulacion.Substring(4, 2) + "/" + fechaAnulacion.Substring(0, 4))

                        If fechaProvision.Equals("") Or fechaProvision.Equals(Nothing) Then
                            strFechaProvision = ""
                        Else
                            strFechaProvision = fechaProvision.Substring(6, 2) + "/" + fechaProvision.Substring(4, 2) + "/" + fechaProvision.Substring(0, 4)
                        End If
                        If fechaAprobacion.Equals("") Or fechaAprobacion.Equals(Nothing) Then
                            strFechaAprobacion = ""
                        Else
                            strFechaAprobacion = fechaAprobacion.Substring(6, 2) + "/" + fechaAprobacion.Substring(4, 2) + "/" + fechaAprobacion.Substring(0, 4)
                        End If
                        If fechaAnulacion.Equals("") Or fechaAnulacion.Equals(Nothing) Then
                            strFechaAnulacion = ""
                        Else
                            strFechaAnulacion = fechaAnulacion.Substring(6, 2) + "/" + fechaAnulacion.Substring(4, 2) + "/" + fechaAnulacion.Substring(0, 4)
                        End If

                        txtFechaPago.Text = strFecha
                        'txtImporte.Text = .Rows(0)(3).ToString
                        txtImporte.Text = String.Format("{0:0.00}", Double.Parse(.Rows(0)(3).ToString))
                        txtUsuarioProvision.Text = .Rows(0)(9).ToString
                        txtFechaProvision.Text = strFechaProvision
                        txtUsuarioAprobacion.Text = .Rows(0)(11).ToString
                        txtFechaAprobacion.Text = strFechaAprobacion
                        txtUsuarioAnulacion.Text = .Rows(0)(13).ToString
                        txtFechaAnulacion.Text = strFechaAnulacion
                        PosicionarComboxToIndex(.Rows(0)(1), .Rows(0)(2), .Rows(0)(4), .Rows(0)(5), .Rows(0)(6), .Rows(0)(7), .Rows(0)(8))
                    End With
                    InhabilitarControles()
                    If Session("sEstado").ToString = "ANULADO" Then
                        ddlEstado.Enabled = False
                        btnGrabar.Enabled = False
                    End If
                    divPanelInferior.Style("display") = "block"
                Else

                    ddlCtaOrigen.Items.Clear()
                    ddlCtaDestino.Items.Clear()
                    ddlCtaOrigen.Enabled = False
                    ddlCtaDestino.Enabled = False
                    divPanelInferior.Style("display") = "none"
                End If
            End If
        Catch ex As Exception
            'Response.Write("<script>window.alert('Error: " + ex.Message + "')</script>")
            AlertaJS(ex.Message)
        End Try
    End Sub


    Protected Sub txtFechaPago_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtFechaPago.TextChanged
        If txtFechaPago.Text.Length = 10 Then
            If Validar2() = False Then
                Me.txtFechaPago.Text = ""
                Exit Sub
            End If
        End If
    End Sub


    Protected Sub ddlCtaOrigen_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCtaOrigen.SelectedIndexChanged
        'Dim oDataSet As DataSet = PrevisionCuentasCorrientesBM.ObtenerBanco(ddlCtaOrigen.SelectedValue.ToString)
        Dim oDataSet As DataSet = PrevisionCuentasCorrientesBM.ObtenerBanco_x_IdBanco(ddlCtaOrigen.SelectedValue.ToString, ddlMoneda.SelectedValue.Trim)
        If oDataSet.Tables(0).Rows.Count > 0 Then
            ddlBancoOrigen.Enabled = True
            ddlBancoOrigen.DataSource = oDataSet
            ddlBancoOrigen.DataValueField = "Codigo"
            ddlBancoOrigen.DataTextField = "BancoCta"
            ddlBancoOrigen.DataBind()
            ddlBancoOrigen.Items.Insert(0, New ListItem("--Seleccione--", ""))
        Else
            ddlBancoOrigen.Items.Clear()
            ddlBancoOrigen.Enabled = False
        End If
    End Sub

    Protected Sub ddlCtaDestino_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCtaDestino.SelectedIndexChanged
        'Dim oDataSet As DataSet = PrevisionCuentasCorrientesBM.ObtenerBanco(ddlCtaDestino.SelectedValue.ToString)
        Dim oDataSet As DataSet = PrevisionCuentasCorrientesBM.ObtenerBanco_x_IdBanco(ddlCtaDestino.SelectedValue.ToString, ddlMoneda.SelectedValue.Trim)
        If oDataSet.Tables(0).Rows.Count > 0 Then
            ddlBancoDestino.Enabled = True
            ddlBancoDestino.DataSource = oDataSet
            ddlBancoDestino.DataValueField = "Codigo"
            ddlBancoDestino.DataTextField = "BancoCta"
            ddlBancoDestino.DataBind()
            ddlBancoDestino.Items.Insert(0, New ListItem("--Seleccione--", ""))
        Else
            ddlBancoDestino.Items.Clear()
            ddlBancoDestino.Enabled = False
        End If
    End Sub

    Protected Sub btnGrabar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGrabar.Click
        Try
            If Session("sCodigoPago") = Nothing Then
                Dim IdTabla1, IdTabla2 As Integer
                If ddlCtaOrigen.SelectedValue.ToString = "" Then
                    IdTabla1 = 0
                Else
                    IdTabla1 = Convert.ToInt32(ddlCtaOrigen.SelectedValue.ToString)
                End If

                If ddlCtaDestino.SelectedValue.ToString = "" Then
                    IdTabla2 = 0
                Else
                    IdTabla2 = Convert.ToInt32(ddlCtaDestino.SelectedValue.ToString)
                End If

                'Se agrego para validar afiliado

                Dim dt As New Data.DataTable
                dt = PrevisionParametriaBM.ListarParametria("15").Tables(0)
                Dim Valor As String = ""

                If dt.Rows.Count > 0 Then
                    Valor = dt.Rows(0)("Valor").ToString()
                End If


                If Validar() = False Then
                    AlertaJS("Debe ingresar todos los campos.")
                ElseIf IsNumeric(txtImporte.Text.Trim) = False Then
                    AlertaJS("El importe debe ser solo números.")
                ElseIf Convert.ToDouble(txtImporte.Text.Trim) < 1 Then
                    AlertaJS("El importe no debe ser igual ni menor a cero(0).")
                ElseIf ValidarFormato() = False Then
                    AlertaJS("Formato incorrecto. Debe ser dd/MM/yyyy.")
                ElseIf Validar2() = True Then
                    'If (((IdTabla1 = 1 Or IdTabla1 = 2 Or IdTabla1 = 3 Or IdTabla1 = 4 Or IdTabla1 = 5) And ddlBancoOrigen.SelectedIndex <= 0) Or ((IdTabla2 = 1 Or IdTabla2 = 2 Or IdTabla2 = 3 Or IdTabla2 = 4 Or IdTabla2 = 5) And ddlBancoDestino.SelectedIndex <= 0)) Then
                    If (((IdTabla1 <> Valor) And ddlBancoOrigen.SelectedIndex <= 0) Or ((IdTabla2 <> Valor) And ddlBancoDestino.SelectedIndex <= 0)) Then
                        AlertaJS("Las cuentas con entidades diferentes de Afiliado \n requieren banco y cuenta corriente.")
                    ElseIf ddlCtaOrigen.SelectedValue = ddlCtaDestino.SelectedValue And ddlBancoOrigen.SelectedValue = ddlBancoDestino.SelectedValue Then
                        AlertaJS("Las operaciones con mismos bancos y cuentas no son permitidas.")
                    Else

                        Dim CodigoBancoOrigen, CodigoBancoDestino As Integer
                        Dim oPago As New PrevisionPago
                        oPago.FechaPago = UIUtility.ConvertirFechaaDecimal(txtFechaPago.Text.Trim)
                        oPago.IdTipoOperacion = ddlTipoOperacion.SelectedValue
                        oPago.UsuarioProvision = Usuario

                        Dim oPagoDetalle1 As New PrevisionPagoDetalle
                        oPagoDetalle1.IdEntidad = ddlCtaOrigen.SelectedValue
                        'oPagoDetalle1.IdBanco = ddlBancoOrigen.SelectedValue
                        CodigoBancoOrigen = IIf(ddlBancoOrigen.SelectedValue = "", 0, ddlBancoOrigen.SelectedValue)
                        oPagoDetalle1.TipoMovimiento = "2"
                        oPagoDetalle1.Importe = txtImporte.Text.Trim
                        oPagoDetalle1.IdTipoMoneda = ddlMoneda.SelectedValue.ToString

                        Dim oPagoDetalle2 As New PrevisionPagoDetalle
                        oPagoDetalle2.IdEntidad = ddlCtaDestino.SelectedValue
                        'oPagoDetalle2.IdBanco = ddlBancoDestino.SelectedValue
                        CodigoBancoDestino = IIf(ddlBancoDestino.SelectedValue = "", 0, ddlBancoDestino.SelectedValue)
                        oPagoDetalle2.TipoMovimiento = "1"
                        oPagoDetalle2.Importe = txtImporte.Text.Trim
                        oPagoDetalle2.IdTipoMoneda = ddlMoneda.SelectedValue.ToString

                        If PrevisionPagoBM.InsertarPRevisionPago(CodigoBancoOrigen, CodigoBancoDestino, oPago, oPagoDetalle1, oPagoDetalle2) = True Then
                            CargarControles()
                            AlertaJS("Grabado correctamente.")
                        Else
                            AlertaJS("Error al grabar.")
                        End If

                    End If
                End If
            Else
                If PrevisionPagoBM.EliminarPrevisionPago(Session("sCodigoPago").ToString, Usuario) = True Then
                    AlertaJS("Registro anulado.")
                    Session("sCodigoPago") = Nothing
                    Response.Redirect("frmPagos.aspx")
                Else
                    AlertaJS("Error al registrar.")
                End If
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

    Protected Sub btnSalir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSalir.Click
        Session("sCodigoPago") = Nothing
        Response.Redirect("frmPagos.aspx")
    End Sub

    Protected Sub ddlMoneda_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlMoneda.SelectedIndexChanged
        If ddlMoneda.SelectedIndex = 0 Then
            ddlCtaOrigen.Items.Clear()
            ddlCtaDestino.Items.Clear()
            ddlCtaOrigen.Enabled = False
            ddlCtaDestino.Enabled = False
        Else
            Dim dt As New DataTable
            dt = PrevisionPagoDetalleBM.ConsultaEntidad_x_TipoMoneda(ddlMoneda.SelectedValue.Trim).Tables(0)

            ddlCtaOrigen.Items.Clear()
            ddlCtaOrigen.DataSource = dt
            ddlCtaOrigen.DataValueField = "IdEntidad"
            ddlCtaOrigen.DataTextField = "Descripcion"
            ddlCtaOrigen.DataBind()
            ddlCtaOrigen.Items.Insert(0, New ListItem("--Seleccione--", ""))
            ddlCtaOrigen.Enabled = True

            ddlCtaDestino.Items.Clear()
            ddlCtaDestino.DataSource = dt
            ddlCtaDestino.DataValueField = "IdEntidad"
            ddlCtaDestino.DataTextField = "Descripcion"
            ddlCtaDestino.DataBind()
            ddlCtaDestino.Items.Insert(0, New ListItem("--Seleccione--", ""))
            ddlCtaDestino.Enabled = True

            ddlBancoOrigen.Items.Clear()
            ddlBancoOrigen.Enabled = False
            ddlBancoDestino.Items.Clear()
            ddlBancoDestino.Enabled = False
        End If
    End Sub

End Class