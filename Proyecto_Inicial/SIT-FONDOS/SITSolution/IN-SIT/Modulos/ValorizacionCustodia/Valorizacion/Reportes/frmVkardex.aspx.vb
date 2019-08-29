Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Partial Class Modulos_Valorizacion_y_Custodia_Valorizacion_Reportes_frmVkardex
    Inherits BasePage
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim vReporte, vFondo, vTipoInstrumento, vmnemonico, visin, usuario, nombre, sportafolio As String
            Dim vfechainicio, vfechafin As Decimal
            Dim vsbs As String  'HDG INC 64085	20111011
            vReporte = Request.QueryString("pReporte")
            vFondo = IIf(Request.QueryString("pportafolio") = "Todos", "", Request.QueryString("pportafolio"))
            vTipoInstrumento = IIf(Request.QueryString("pTipoInstrumento") = "Todos", "", Request.QueryString("pportafolio"))
            vmnemonico = IIf(Request.QueryString("pmnemonico") Is Nothing, "", Request.QueryString("pmnemonico"))
            visin = IIf(Request.QueryString("pisin") Is Nothing, "", Request.QueryString("pisin"))
            vsbs = IIf(Request.QueryString("psbs") Is Nothing, "", Request.QueryString("psbs")) 'HDG INC 64085	20111011
            vfechainicio = Convert.ToDecimal(Request.QueryString("pFechaIni"))
            vfechafin = Convert.ToDecimal(Request.QueryString("pFechaFin"))
            nombre = "Usuario"
            Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
            usuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & nombre & "'")(0)(1), String)
            Dim dtPortafolio As PortafolioBE = New PortafolioBM().Seleccionar(vFondo, Nothing)
            sportafolio = dtPortafolio.Tables(0).Rows(0)("Descripcion").ToString.Trim
            Dim dtconsultaKardex As New DataTable            
            Select Case vReporte
                Case "IC"
                    Dim dsconsulta As New DsKardexCartera
                    Dim drconsulta As DataRow
                    oReport.Load(Server.MapPath("Kardex.rpt"))
                    Dim objOrdeninversionBM As New OrdenPreOrdenInversionBM
                    If Not (Page.IsPostBack) Then
                        dtconsultaKardex = objOrdeninversionBM.ConsultaKardex(vFondo, vfechainicio, vfechafin, vmnemonico, visin, vsbs, DatosRequest).Tables(0) 'HDG INC 64085	20111011
                        Session("dtConsulta") = dtconsultaKardex
                    Else
                        dtconsultaKardex = CType(Session("dtConsulta"), DataTable)
                    End If
                    For Each drv As DataRow In dtconsultaKardex.Rows
                        drconsulta = dsconsulta.Tables(0).NewRow()
                        drconsulta("FechaOperacion") = UIUtility.ConvertirFechaaString(drv("FechaOperacion"))
                        drconsulta("CodigoMnemonico") = drv("CodigoMnemonico")
                        drconsulta("CodigoPortafoliosbs") = drv("Portafolio")
                        drconsulta("SaldoInicial") = drv("SaldoInicial")
                        drconsulta("CodigoSBS") = drv("CodigoSBS")
                        drconsulta("Unidades") = drv("Unidades")
                        drconsulta("Saldodisponible") = drv("SaldoFinal")
                        drconsulta("TipoOperacion") = drv("Operacion")
                        drconsulta("SaldoCustodio") = drv("CustodioSaldo")
                        drconsulta("Custodio") = drv("Custodio")
                        drconsulta("CodigoISIN") = drv("CodigoISIN")
                        drconsulta("Diferencia") = drv("Diferencia")
                        If Not drv("FechaLiquidacion") Is DBNull.Value Then
                            If drv("FechaLiquidacion") <> 0 Then 'RGF 20090319
                                drconsulta("FechaLiquidacion") = UIUtility.ConvertirFechaaString(drv("FechaLiquidacion"))
                            End If
                        End If
                        dsconsulta.Tables(0).Rows.Add(drconsulta)
                    Next
                    dsconsulta.Merge(dsconsulta, False, System.Data.MissingSchemaAction.Ignore)
                    oReport.SetDataSource(dsconsulta)
                    oReport.SetParameterValue("@FechaInicio", UIUtility.ConvertirFechaaString(vfechainicio))
                    oReport.SetParameterValue("@FechaFin", UIUtility.ConvertirFechaaString(vfechafin))
                    oReport.SetParameterValue("@Usuario", usuario)
                    oReport.SetParameterValue("@Portafolio", sportafolio)
                    Me.crKardex.ReportSource = oReport
                Case "OR"
                    Dim dsconsulta As New DsKardexOR
                    Dim drconsulta As DataRow
                    oReport.Load(Server.MapPath("OperacionesRealizadas.rpt"))
                    Dim objOrdeninversionBM As New OrdenPreOrdenInversionBM
                    dtconsultaKardex = objOrdeninversionBM.ConsultaOperacionesRealizadas(vFondo, vfechainicio, vfechafin, vmnemonico, visin, vsbs, DatosRequest).Tables(0) 'HDG INC 64085	20111011
                    For Each drv As DataRow In dtconsultaKardex.Rows
                        drconsulta = dsconsulta.Tables(0).NewRow()
                        drconsulta("CodigoMnemonico") = drv("CodigoMnemonico")
                        drconsulta("CodigoPortafolio") = drv("Portafolio")
                        drconsulta("Empresa") = drv("Empresa")
                        drconsulta("CodigoSBS") = drv("CodigoSBS")
                        drconsulta("SaldoInicial") = drv("SaldoInicial")
                        drconsulta("Unidades") = drv("Unidades")
                        drconsulta("SaldoFinal") = drv("SaldoFinal")
                        drconsulta("TipoOperacion") = drv("Operacion")
                        drconsulta("TipoCambio") = drv("TipoCambio")
                        drconsulta("PrecioVector") = drv("PrecioVector")
                        drconsulta("Valoracion") = drv("Valoracion")
                        drconsulta("PrecioTransaccion") = drv("PrecioTransaccion")
                        drconsulta("CodigoMoneda") = drv("Moneda")
                        dsconsulta.Tables(0).Rows.Add(drconsulta)
                    Next
                    dsconsulta.Merge(dsconsulta, False, System.Data.MissingSchemaAction.Ignore)
                    oReport.SetDataSource(dsconsulta)
                    oReport.SetParameterValue("@Fecha", UIUtility.ConvertirFechaaString(vfechainicio))
                    oReport.SetParameterValue("@Usuario", usuario)
                    oReport.SetParameterValue("@Portafolio", sportafolio)
                    Me.crKardex.ReportSource = oReport
            End Select
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub
    Protected Sub Modulos_Valorizacion_y_Custodia_Valorizacion_Reportes_frmVkardex_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class