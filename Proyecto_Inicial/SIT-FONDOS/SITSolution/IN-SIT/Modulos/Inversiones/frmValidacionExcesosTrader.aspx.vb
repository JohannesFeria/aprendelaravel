Imports Sit.BusinessLayer
Imports System.Text
Imports System.Data
Imports ParametrosSIT
'OT 10090 - 26/07/2017 - Carlos Espejo
'Descripcion: Se ordeno el formulario
Partial Class Modulos_Inversiones_frmValidacionExcesosTrader
    Inherits BasePage
    Dim oLimiteTradingBM As New LimiteTradingBM
    Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
    Dim decNProceso As Decimal = 0
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            btnAprobar.Attributes.Add("onclick", "javascript: return confirm('Desea aprobar la siguiente negociación ? ');")
            If Not Session("dtValidaTrader") Is Nothing And Not Request.QueryString("TipoRenta") Is Nothing Then
                CargarGrilla()
            Else
                Call Retornar()
            End If
        End If
    End Sub
    Private Sub Imprimir()
        EjecutarJS("window.print();")
    End Sub
    Private Sub Retornar()
        decNProceso = Convert.ToDecimal(Request.QueryString("nProc"))
        oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
        EjecutarJS("window.close();")
    End Sub
    Private Sub CargarGrilla()
        Dim dtAux As New DataTable
        dtAux = CType(Session("dtValidaTrader"), DataTable)
        dgLista.DataSource = dtAux
        dgLista.DataBind()
    End Sub
    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Call Imprimir()
    End Sub
    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Call Retornar()
    End Sub
    Private Sub btnAprobar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAprobar.Click
        Try
            Dim dtExcesos As New DataTable, dtAprobadores As New DataTable
            Dim tipoCargo, usuarioAprob, tipoRenta, mensaje, CodigoGrupLimTrader As String, codigoPrevOrden As String = "",
                CodigoPortafolio As String, PorcentajeExcedido As Decimal
            Dim dsExcesos As DataSet
            decNProceso = Convert.ToDecimal(Request.QueryString("nProc"))
            tipoRenta = Request.QueryString("TipoRenta")
            'OT 10090 - 26/07/2017 - Carlos Espejo
            'Descripcion: Se aprueba la negociacion de trader
            oPrevOrdenInversionBM.AprobarNegociacionTrader(DatosRequest, tipoRenta, decNProceso)
            'OT 10090 Fin
            dtExcesos = oPrevOrdenInversionBM.ObtenerExcesosTrader(tipoRenta, DatosRequest, decNProceso).Tables(0)
            If dtExcesos.Rows.Count > 0 Then
                For Each fila As DataRow In dtExcesos.Rows
                    CodigoGrupLimTrader = fila("CodigoGrupLimTrader").ToString()
                    tipoCargo = fila("TipoCargo").ToString()
                    usuarioAprob = fila("Usuario").ToString()
                    codigoPrevOrden = ""
                    dsExcesos = oPrevOrdenInversionBM.ExcesosTrader(CodigoGrupLimTrader, tipoRenta, DatosRequest, decNProceso)
                    'OT 10090 - 26/07/2017 - Carlos Espejo
                    'Descripcion: Otros datos para los aprobadores
                    CodigoPortafolio = dsExcesos.Tables(0).Rows(0)("CodigoPortafolioSBS").ToString()
                    PorcentajeExcedido = CDec(dsExcesos.Tables(0).Rows(0)("PorcentajeNegociado"))
                    'Recorre las ordenes que esten dentro del grupo
                    For i As Integer = 0 To dsExcesos.Tables(1).Rows.Count - 1
                        If codigoPrevOrden = "" Then
                            codigoPrevOrden = dsExcesos.Tables(1).Rows(i)("CodigoPrevOrden").ToString()
                        Else
                            codigoPrevOrden = codigoPrevOrden + "," + dsExcesos.Tables(1).Rows(i)("CodigoPrevOrden").ToString()
                        End If
                    Next
                    'Envia los correos a los aprobadores
                    mensaje = MensajeExcesoTrader(dsExcesos, codigoPrevOrden, usuarioAprob, tipoRenta)
                    dtAprobadores = oPrevOrdenInversionBM.AprobarNegociacionTrader(CodigoGrupLimTrader, CodigoPortafolio, PorcentajeExcedido)
                    'OT 10090 Fin
                    For Each dr As DataRow In dtAprobadores.Rows
                        UIUtility.EnviarMail(dr("email_trabajo"), "", "SIT-Fondos - Exceso por Trader - Pendiente de Aprobación", mensaje, DatosRequest)
                    Next
                Next
            End If
            oPrevOrdenInversionBM.EliminarProcesoMasivo(decNProceso)
            AlertaJS("Proceso realizado satisfactoriamente! ")
            Call Retornar()
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
    Private Function MensajeExcesoTrader(ByVal dsResult As DataSet, ByVal codigoPrevOrden As String, ByVal usuarioAprob As String, ByVal tipoRenta As String, _
    Optional ByVal Riesgos As String = "") As String
        Dim mensaje As New StringBuilder
        Dim dtResultHeader As New DataTable
        Dim dtResultDetail As New DataTable
        dtResultHeader = dsResult.Tables(0)
        dtResultDetail = dsResult.Tables(1)
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim dtParametros As New DataTable
        Dim ruta As String
        Dim CabMontoCant As String = ""
        dtParametros = oParametrosGeneralesBM.Listar(ParametrosSIT.RUTA_APROB_EXC_TRADER, DatosRequest)
        ruta = dtParametros.Rows(0)("Valor").ToString()
        With mensaje
            .Append("<table cellspacing='1' cellpadding='0' border='0' width='650' style='FONT-FAMILY: Verdana; COLOR: #000000; FONT-SIZE: 11px; FONT-WEIGHT: normal'>")
            If Riesgos = "" Then
                .Append("<tr><td>La siguiente negociación requiere de su aprobación:</td></tr>")
                .Append("<tr><td></td></tr>")
                .Append("<tr><td>Para aprobar la siguiente negociación seleccione <b><a href='" + ruta + "?CodigoPrevOrden=" + codigoPrevOrden + "&amp;Usuario=" + _
                usuarioAprob + "&amp;TipoRenta=" + tipoRenta + "'>aqu&iacute;.</a></b></td></tr>")
            Else
                .Append("<tr><td>La siguiente negociación requiere de aprobación.</td></tr>")
            End If
            .Append("<tr><td></td></tr>")
            .Append("<tr><td><b>Detalle de la negociación realizada: </b></td></tr>")
            .Append("</table><br/>")
            If dtResultHeader.Rows.Count > 0 Then
                .Append("<table cellspacing='1' cellpadding='1' border='1' width='650' bordercolor='#000000' style='FONT-FAMILY: Verdana; COLOR: #000000; FONT-SIZE: 11px; FONT-WEIGHT: normal; TEXT-DECORATION: none'><tr>" _
                & "<td align=""center"">Tipo Instrumento</td>" _
                & "<td align=""center"">Monto Negociado<br>(S/.)</td>" _
                & "<td align=""center"">Tamaño del Fondo<br>(S/.)</td>" _
                & "<td align=""center"">Porcentaje<br>Negociado</td>" _
                & "<td align=""center"">Porcentaje<br>Limite</td>" _
                & "<td align=""center"">Estado</td>" _
                & "<td align=""center"">Fondo</td>" _
                & "</tr>")
                CabMontoCant = "Cant."
                For Each fila As DataRow In dtResultHeader.Rows
                    .Append("<tr>" _
                    & "<td align=""center"">" + fila("GrupoInstrumento") + "</td>" _
                    & "<td align=""center"">" + String.Format("{0:###,##0}", fila("PosicionNegociado")) + "</td>" _
                    & "<td align=""center"">" + String.Format("{0:###,##0}", fila("PosicionLimite")) + "</td>" _
                    & "<td align=""center"">" + String.Format("{0:###,##0.00}", fila("PorcentajeNegociado")) + "%</td>" _
                    & "<td align=""center"">" + String.Format("{0:###,##0.00}", fila("PorcentajeLimite")) + "%</td>" _
                    & "<td align=""center"">" + fila("Estado") + "</td>" _
                    & "<td align=""center"">" + fila("Portafolio") + "</td>" _
                    & "</tr>")
                    If fila("CodigoGrupLimTrader") = GRUPO_LIMTRD_CURRENCY Or fila("CodigoGrupLimTrader") = GRUPO_LIMTRD_DEPOSITOSON Then
                        CabMontoCant = "Monto"
                    End If
                Next
                .Append("</table><br>")
            End If
            'Detalle
            If dtResultDetail.Rows.Count > 0 Then
                .Append("<table cellspacing='1' cellpadding='1' border='1' width='650' bordercolor='#000000' style='FONT-FAMILY: Verdana; COLOR: #000000; FONT-SIZE: 11px; FONT-WEIGHT: normal; TEXT-DECORATION: none'><tr>" _
                & "<td align=""center"">Nemonico</td>" _
                & "<td align=""center"">Operación</td>" _
                & "<td align=""center"">Intermediario</td>" _
                & "<td align=""center"">Precio Ejecución</td>" _
                & "<td align=""center"">" & CabMontoCant & " Ejecución</td>" _
                & "<td align=""center"">Fondo</td>" _
                & "<td align=""center"">Asignación</td>" _
                & "</tr>")
                For Each fila As DataRow In dtResultDetail.Rows
                    .Append("<tr>" _
                        & "<td align=""center"">" + fila("CodigoNemonico") + "</td>" _
                        & "<td align=""center"">" + fila("Operacion") + "</td>" _
                        & "<td align=""center"">" + fila("Intermediario") + "</td>" _
                        & "<td align=""center"">" + String.Format("{0:###,##0.0000}", fila("PrecioOperacion")) + "</td>" _
                        & "<td align=""center"">" + String.Format("{0:###,##0}", fila("CantidadOperacion")) + "</td>" _
                        & "<td align=""center"">" + fila("Fondo") + "</td>" _
                        & "<td align=""center"">" + String.Format("{0:###,##0.00}", fila("Asignacion")) + "</td>" _
                        & "</tr>")
                Next
                .Append("</table><br>")
            End If
            .Append("<table cellspacing='1' cellpadding='0' border='0' width='650' style='FONT-FAMILY: Verdana; COLOR: #000000; FONT-SIZE: 11px; FONT-WEIGHT: normal; TEXT-DECORATION: none'>")
            .Append("<tr><td><b>Fondos Sura</b></td></tr>")
            .Append("<tr><td><b>Grupo SURA</b></td></tr></table>")
        End With
        Return mensaje.ToString
    End Function
    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en al Paginación")
        End Try
    End Sub
End Class
