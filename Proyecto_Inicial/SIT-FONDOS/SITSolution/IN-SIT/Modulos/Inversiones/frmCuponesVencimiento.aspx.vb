Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Text
Imports System.Drawing
Partial Class Modulos_Inversiones_frmCuponesVencimiento
    Inherits BasePage
    Dim oOrdenInversionBE As New OrdenPreOrdenInversionBE
    Dim oOrdenInversionBM As New OrdenPreOrdenInversionBM
#Region " /* Eventos de Página */ "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        Try
            If Not Page.IsPostBack Then
                Response.Cache.SetCacheability(HttpCacheability.NoCache)
                Dim monto As String = String.Empty
                Dim fecha As Decimal
                Dim nemonico As String = String.Empty
                Dim portafolio As String = String.Empty
                Dim secuencial As String = String.Empty
                Dim orden As String = String.Empty
                If Not Request.QueryString("montoNominalLocal") Is Nothing And Not Request.QueryString("fechaVencimiento") Is Nothing And Not Request.QueryString("codigoNemonico") Is Nothing Then
                    monto = Request.QueryString("montoNominalLocal")
                    fecha = Request.QueryString("fechaVencimiento")
                    nemonico = Request.QueryString("codigoNemonico")
                    portafolio = Request.QueryString("codigoPortafolio")
                    If CType(Session("TipoInstrumento_OI"), String) = "BS" Then
                        Dim dtCuponeraBonoSwap As DataTable = oOrdenInversionBM.ConsultaCuponera_Bono_Swap(String.Empty, Request.QueryString("ordenInversion"))
                        If dtCuponeraBonoSwap.Rows.Count > 0 Then
                            secuencial = dtCuponeraBonoSwap(0)(0)
                        Else
                            AlertaJS("No se encontró cuponera swap para el instrumento seleccionado")
                            Exit Sub
                        End If
                    Else
                        secuencial = New CuponeraBM().SecuenciaCuponera(Request.QueryString("ordenInversion"), Request.QueryString("codigoPortafolio"))
                    End If

                    orden = Request.QueryString("ordenInversion")
                    ViewState("Monto") = monto
                    ViewState("Nemonico") = nemonico
                    ViewState("Secuencial") = secuencial
                    ViewState("Portafolio") = portafolio
                    ViewState("OrdenInversion") = orden
                    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF010 - Cuando el formulario viene de Confirmaciones se cambia label de botón aceptar a Grabar y Confirmar y si tiene fecha anterior se bloquea campos | 13/07/18 
                    btnAceptar.Text = "Grabar y Confirmar"
                    If Session("ValidarFecha").ToString = "FECHADIFERENTE" Then
                        UIUtility.deshabilitarCampos(form1)
                        btnAceptar.Enabled = False
                    End If
                    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF010 - Cuando el formulario viene de Confirmaciones se cambia label de botón aceptar a Grabar y Confirmar y si tiene fecha anterior se bloquea campos | 13/07/18 

                End If
                CargarGrillaOICuponerasVencimiento(nemonico, Convert.ToDecimal(monto), fecha, secuencial)
                ViewState("FechaVencimiento") = Request.QueryString("fechaVencimiento")
                SeleccionarCuponesVencidos(Request.QueryString("fechaVencimiento"))
                CargarDatosOrdenInversion(orden, portafolio)
                btnAceptar.Attributes.Add("onClick", "javascript:return Confirmar();")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página/ " + ex.Message.ToString)
        End Try
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(2).Controls(1), TextBox).Text = e.Row.Cells(4).Text
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try
    End Sub
    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim montoNominalLocal As Decimal
            Dim fechaVenc As String = String.Empty
            Dim cnt As Int32 = 0
            If txtFechaIDI.Text.Trim = String.Empty Then
                AlertaJS("Falta ingresar la Fecha de Operación.")
                UIUtility.ResaltaCajaTexto(txtFechaIDI, True)
            ElseIf txtFechaPago.Text.Trim = String.Empty Then
                AlertaJS("Falta ingresar la Fecha de Vencimiento.")
                UIUtility.ResaltaCajaTexto(txtFechaPago, True)
            Else
                For cnt = 0 To dgLista.Rows.Count - 1
                    fechaVenc = dgLista.Rows(cnt).Cells(1).Text.ToString()
                    If ViewState("Secuencial") = dgLista.Rows(cnt).Cells(8).Text.ToString() Then
                        montoNominalLocal = Convert.ToDecimal(CType(dgLista.Rows(cnt).Cells(2).Controls(1), TextBox).Text.ToString())
                    End If
                Next
                If txtMontoOperacionDestino.Text.Trim.Equals("") Or txtMontoOperacionDestino.Text.Trim.Equals("0") Then
                    ConfirmarCuponVencimiento(ViewState("Portafolio").ToString(), ViewState("Nemonico").ToString(), ViewState("FechaVencimiento").ToString(), montoNominalLocal, ViewState("Secuencial").ToString(), ViewState("OrdenInversion").ToString(), "N", 0)
                Else
                    ConfirmarCuponVencimiento(ViewState("Portafolio").ToString(), ViewState("Nemonico").ToString(), ViewState("FechaVencimiento").ToString(), txtMontoOperacionDestino.Text.Replace(",", ""), ViewState("Secuencial").ToString(), ViewState("OrdenInversion").ToString(), "Y", montoNominalLocal)
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Aceptar")
        End Try
    End Sub
#End Region

#Region " /* Funciones y Métodos Personalizados */ "
    Private Function CargarGrillaOICuponerasVencimiento(ByVal codigoNemonico As String, ByVal valorNominal As Decimal, ByVal fecha As String, ByVal secuencial As String) As Decimal
        Dim cuponera As DataSet = Nothing
        Dim saldo As Decimal = 0
        Dim codigoISIN As String = String.Empty
        Dim codigoSBS As String = String.Empty
        Dim i As Integer
        cuponera = New CuponeraBM().SeleccionarVencimientos(codigoNemonico, valorNominal, fecha, _
                                                            CType(ViewState("Portafolio"), String), _
                                                            saldo, Request.QueryString("ordenInversion"), _
                                                            codigoISIN, codigoSBS, CargarGrillaOICuponerasVencimiento, _
                                                            Me.DatosRequest, secuencial)

        dgLista.DataSource = cuponera.Tables(0)
        dgLista.DataBind()

        For i = 0 To dgLista.Rows.Count - 1
            If dgLista.Rows(i).Cells(1).Text.Trim.Length = 8 Then dgLista.Rows(i).Cells(1).Text = UIUtility.ConvertirFechaaString(dgLista.Rows(i).Cells(1).Text)
            If dgLista.Rows(i).Cells(0).Text.Trim.Length = 8 Then dgLista.Rows(i).Cells(0).Text = UIUtility.ConvertirFechaaString(dgLista.Rows(i).Cells(0).Text)
        Next

        tbUnidades.Text = Format(saldo, "##,##0.0000000")
        tbUnidades.Enabled = False

        lCodigoISIN.Text = codigoISIN
        lCodigoSBS.Text = codigoSBS
    End Function

    Private Function SeleccionarCuponesVencidos(ByVal fechaVencimiento As String) As Boolean

        Dim cnt As Int32 = 0
        'Dim fechaVenc As Decimal = 0
        Dim strSituacion As String
        Dim ingreso As String = "no"
        '  Dim ts As TimeSpan 'RGF 20080911

        For cnt = 0 To dgLista.Rows.Count - 1
            strSituacion = dgLista.Rows(cnt).Cells(5).Text.ToString()
            '  ts = DateTime.Parse(UIUtility.ConvertirFechaaString(dgLista.Rows(cnt).Cells(1).Text)).Subtract(DateTime.Parse(UIUtility.ConvertirFechaaString(fechaVencimiento)))
            'If Math.Abs(ts.Days) <= 2 And strSituacion = "PENDIENTE" Then
            lFechaVencimiento.Text = dgLista.Rows(cnt).Cells(1).Text.ToString()
            CType(dgLista.Rows(cnt).Cells(2).Controls(1), TextBox).Text = Format(Convert.ToDouble(ViewState("Monto")), "#0.0000000")
            lMontoNominalLocal.Text = Convert.ToString(ViewState("Monto"))
            ' txtFechaIDI.Text = UIUtility.ConvertirFechaaString(dgLista.Rows(cnt).Cells(1).Text.ToString()).ToString()
            txtFechaPago.Text = dgLista.Rows(cnt).Cells(1).Text.ToString()
            txtFechaIDI.Text = UIUtility.ConvertirFechaaString(Request.QueryString("fechaVencimiento"))
            'txtFechaPago.Text = UIUtility.ConvertirFechaaString(Request.QueryString("fechaVencimiento"))
            dgLista.Rows(cnt).Enabled = True
            dgLista.Rows(cnt).BackColor = Color.Beige
            ViewState("Secuencial") = dgLista.Rows(cnt).Cells(8).Text.ToString()
            'Else
            'dgLista.Rows(cnt).Cells(4).Text = "0"
            'dgLista.Rows(cnt).Enabled = False
            'End If
        Next
    End Function
    Private Function ConfirmarCuponVencimiento(ByVal portafolio As String, ByVal codigoNemonico As String, ByVal fechaVencimiento As String, ByVal montoNominalLocal As Decimal, ByVal secuencial As String, ByVal orden As String, ByVal monedaDest As String, ByVal montoOrigen As Decimal) As Boolean
        Try
            Dim fechaIDI As Decimal
            Dim fechaPago As Decimal
            Dim confirmacion As Boolean = False
            fechaIDI = UIUtility.ConvertirFechaaDecimal(txtFechaIDI.Text)
            fechaPago = UIUtility.ConvertirFechaaDecimal(txtFechaPago.Text)
            secuencial = IIf(HttpUtility.HtmlDecode(secuencial).Trim = "", "0", HttpUtility.HtmlDecode(secuencial))
            confirmacion = New CuponeraBM().ConfirmarVencimientoCuponeraOI(portafolio, codigoNemonico, fechaVencimiento, montoNominalLocal, HttpUtility.HtmlDecode(secuencial), fechaIDI,
            fechaPago, orden, monedaDest, montoOrigen, DatosRequest)
            If confirmacion Then
                'AlertaJS("El cupón se ha confirmado exitosamente")
                'EjecutarJS("Cerrar();")
                AlertaJS("El cupón se ha confirmado exitosamente", "window.close()")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación")
        End Try
    End Function
    Private Sub CargarDatosOrdenInversion(ByVal orden As String, ByVal portafolio As String)
        oOrdenInversionBE = oOrdenInversionBM.ListarOrdenesInversionPorCodigoOrden(orden, portafolio, DatosRequest, PORTAFOLIO_MULTIFONDOS)
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oRow = oOrdenInversionBE.Tables(0).Rows(0)
        ' If oRow.CodigoMoneda <> oRow.CodigoMonedaDestino Then 'RGF 20090525
        Me.tblDestino.Attributes.Add("Style", "Visibility:visible")
        If oRow.CodigoOperacion = "35" Or oRow.CodigoOperacion = "22" Or oRow.CodigoOperacion = "24" Then
            txtMontoOperacionDestino.Text = Format(oRow.MontoOperacion, "##,##0.0000000")
        Else
            txtMontoOperacionDestino.Text = Format(oRow.MontoDestino, "##,##0.0000000")
        End If
        Me.lblMDest2.Text = Trim(oRow.CodigoMonedaDestino)
        ' End If
    End Sub
#End Region
End Class
