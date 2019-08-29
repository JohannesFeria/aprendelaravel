Imports System.Text
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Inversiones_frmDividendosRebatesLiberadasVencimiento
    Inherits BasePage

#Region " /* Eventos de Página */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                Response.Cache.SetCacheability(HttpCacheability.NoCache)
                Dim monto As String = String.Empty
                Dim fecha As String = String.Empty
                Dim identificador As String = String.Empty
                Dim nemonico As String = String.Empty
                Dim portafolio As String = String.Empty
                Dim nportafolio As String = String.Empty
                Dim estado As String = String.Empty
                Dim moneda As String = String.Empty
                Dim dtConsulta As New DataTable
                Dim tipo As String = ""
                Dim codigoSBS As String = ""
                If Not Request.QueryString("montoNominalLocal") Is Nothing And Not Request.QueryString("fechaVencimiento") Is Nothing And Not Request.QueryString("codigoNemonico") Is Nothing Then
                    monto = Request.QueryString("montoNominalLocal")
                    fecha = Request.QueryString("fechaVencimiento")
                    nemonico = Request.QueryString("codigoNemonico")
                    identificador = Request.QueryString("identificador")
                    codigoSBS = CType(Session("CodigoSBS_DRL"), String)
                    dtConsulta = New DividendosRebatesLiberadasBM().Seleccionar(codigoSBS, CType(identificador, Decimal), "A", DatosRequest).Tables(0)
                    If (dtConsulta.Rows.Count > 0) Then
                        Me.tbFactor.Text = CType(dtConsulta.Rows(0).Item("Factor"), String).Substring(0, CType(dtConsulta.Rows(0).Item("Factor"), String).Length - 2)
                        Me.tbFechaCorte.Text = UIUtility.ConvertirFechaaString(CType(dtConsulta.Rows(0).Item("FechaCorte"), Decimal))
                        Me.tbFechaIDI.Text = UIUtility.ConvertirFechaaString(CType(dtConsulta.Rows(0).Item("FechaIDI"), Decimal))
                        Me.tbFechaEntrega.Text = UIUtility.ConvertirFechaaString(CType(dtConsulta.Rows(0).Item("FechaEntrega"), Decimal))
                        tbUnidades.Text = Format(Math.Floor(dtConsulta.Rows(0).Item("NroCuotasUnidades")), "#,##0") 'HDG 20110831

                        tipo = dtConsulta.Rows(0).Item("TipoDistribucion")
                        If (tipo = "D") Then
                            Me.tbTipoDistribucion.Text = "Dividendo"
                        Else
                            If (tipo = "L") Then
                                Me.tbTipoDistribucion.Text = "Liberada"
                            Else
                                If (tipo = "R") Then
                                    Me.tbTipoDistribucion.Text = "Rebate"
                                End If
                            End If
                        End If
                    End If

                    portafolio = Request.QueryString("codigoPortafolio")
                    nportafolio = Request.QueryString("Portafolio")
                    estado = Request.QueryString("estado")
                    moneda = Request.QueryString("moneda")

                    ViewState("monto") = monto
                    ViewState("fecha") = fecha
                    ViewState("nemonico") = nemonico
                    ViewState("identificador") = identificador
                    ViewState("portafolio") = portafolio
                    ViewState("nportafolio") = nportafolio
                    ViewState("estado") = estado
                    ViewState("moneda") = moneda
                    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Cuando el formulario viene de Confirmaciones se cambia label de botón aceptar a Grabar y Confirmar y si tiene fecha anterior se bloquea campos | 13/07/18 
                    btnAceptar.Text = "Grabar y Confirmar"
                    If Session("ValidarFecha").ToString = "FECHADIFERENTE" Then
                        UIUtility.deshabilitarCampos(form1)
                        btnAceptar.Enabled = False
                    End If
                    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Cuando el formulario viene de Confirmaciones se cambia label de botón aceptar a Grabar y Confirmar y si tiene fecha anterior se bloquea campos | 13/07/18 
                End If
                CargarDatosDividendoRebateLiberadaVencimiento(nportafolio, nemonico, fecha, identificador, Convert.ToDecimal(monto), estado, moneda)
                btnAceptar.Attributes.Add("onClick", "javascript:return Confirmar();")
                Session("CodigoSBS_DRL") = Nothing
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim montoConfimar As Decimal
            If txtMontoConfirmar.Text = Nothing Or txtMontoConfirmar.Text = "" Then
                AlertaJS("Ingrese un monto")
                Exit Sub
            End If
            montoConfimar = Convert.ToDecimal(txtMontoConfirmar.Text.ToString())
            ConfirmarDividendoRebateLiberada(ViewState("portafolio").ToString(), ViewState("nemonico").ToString(), ViewState("fecha").ToString(), ViewState("identificador").ToString(), Convert.ToDecimal(montoConfimar), ViewState("estado").ToString())
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación")
        End Try
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            EjecutarJS("Cerrar();")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar")
        End Try
    End Sub

#End Region

#Region " /* Métodos y Funciones Personalizadas * /"
    Private Sub ConfirmarDividendoRebateLiberada(ByVal fondo As String, ByVal nemonico As String, ByVal fechavencimiento As String, ByVal identificador As String, ByVal monto As Decimal, ByVal estado As String)
        Try
            Dim confirmacion As Boolean = False
            confirmacion = New DividendosRebatesLiberadasBM().ConfirmarDividendoRebateLiberada(fondo, nemonico, fechavencimiento, identificador, monto, DatosRequest)
            If confirmacion Then
                'AlertaJS("La operación se ha confirmado exitosamente")
                'EjecutarJS("Cerrar();")
                AlertaJS("La operación se ha confirmado exitosamente", "window.close()")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación")
        End Try
    End Sub
    Private Sub CargarDatosDividendoRebateLiberadaVencimiento(ByVal fondo As String, ByVal nemonico As String, ByVal fechavencimiento As String, ByVal identificador As String, ByVal monto As Decimal, ByVal estado As String, ByVal moneda As String)
        lbIdentificador.Text = identificador
        lblEstado.Text = estado
        txtMontoConfirmar.Text = monto.ToString("#,##0.00")
        lblNemonico.Text = nemonico
        lblPortafolio.Text = fondo
        lMoneda.Text = moneda
        lFechaVencimiento.Text = UIUtility.ConvertirFechaaString(fechavencimiento)
        lMontoNominalLocal.Text = monto
    End Sub

#End Region
End Class
