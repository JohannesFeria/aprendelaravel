Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Partial Class Modulos_Tesoreria_Encaje_frmVisorEncaje
    Inherits BasePage
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim vFondo, usuario, nombre, fecha As String
            Dim DecFechaProceso As Decimal
            Dim dFecha As Date
            Dim opcion As String
            Dim objutilbm As New UtilDM
            Dim oArchivosVaxBCOSBM As New ArchivosVAXBCOSBM
            Dim oArchivosVaxBCOSBE As New DataSet
            Dim EncajeMantenido As String
            Dim ValorFondo As String
            Dim IndicadorFondo As Decimal
            Dim IndicadorEncaje As Decimal
            Dim Nemonico As String
            Dim oParametrosMigracionBM As New ParametrosMigracionBM
            Dim oParametrosMigracionBE As New DataSet
            Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
            opcion = Request.QueryString("pReporte")
            vFondo = IIf(Request.QueryString("pportafolio") = "Todos", "", Request.QueryString("pportafolio"))
            fecha = Request.QueryString("pFechaIni")
            Nemonico = Request.QueryString("pNemonico")
            Dim cls As New PortafolioBM
            Dim obj As PortafolioBE
            Dim ds As New DataSet
            obj = cls.Seleccionar(vFondo, ds)
            dFecha = fecha            
            Select Case opcion
                Case "CE"
                    nombre = "Usuario"
                    Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
                    usuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & nombre & "'")(0)(1), String)
                    Dim dtConsulta As New DataTable
                    Dim dsconsulta As New DsEncaje
                    Dim drconsulta As DataRow
                    oReport.Load(Server.MapPath("RptConsultaEncaje.rpt"))
                    Dim oEncajeBM As New EncajeBM
                    dtConsulta = oEncajeBM.SeleccionarPorFiltro(vFondo, dFecha, DatosRequest).Tables(0)
                    For Each drv As DataRow In dtConsulta.Rows
                        drconsulta = dsconsulta.Tables(0).NewRow()
                        drconsulta("FechaEncaje") = drv("FechaEncaje")
                        drconsulta("ValorMantenido") = drv("ValorMantenido")
                        drconsulta("ValorRequerido") = drv("ValorRequerido")
                        drconsulta("DiferenciaEncaje") = drv("DiferenciaEncaje")
                        drconsulta("Estado") = drv("Estado")
                        drconsulta("ValorRentabilidad") = drv("ValorRentabilidad")
                        drconsulta("CodigoPortafolioSBS") = drv("CodigoPortafolioSBS") 'RGF 20080910
                        dsconsulta.Tables(0).Rows.Add(drconsulta)
                    Next
                    dsconsulta.Merge(dsconsulta, False, System.Data.MissingSchemaAction.Ignore)
                    oReport.SetDataSource(dsconsulta)
                    oReport.SetParameterValue("@FechaInicio", fecha)
                    oReport.SetParameterValue("@Usuario", usuario)
                    oReport.SetParameterValue("@Portafolio", obj.Tables(0).Rows(0)("Descripcion").ToString)
                    oReport.SetParameterValue("@rutaLogo", ConfigurationManager.AppSettings("RUTA_LOGO"))
                    Me.crEncaje.ReportSource = oReport
                Case "RE"
                    oReport.Load(Server.MapPath("RptResultadoEncaje.rpt"))
                    nombre = "Usuario"
                    Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
                    usuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & nombre & "'")(0)(1), String)
                    DecFechaProceso = objutilbm.RetornarFechaValoradaAnterior(UIUtility.ConvertirFechaaDecimal(fecha), vFondo)
                    oParametrosMigracionBE = oParametrosMigracionBM.SeleccionarPorFiltro("ENCA", 7, "", "", "", DatosRequest)
                    If (oParametrosMigracionBE.Tables(0).Rows.Count <> 0) Then
                        EncajeMantenido = oParametrosMigracionBE.Tables(0).Rows(0).Item("Valor").ToString()
                    End If
                    'IndicadorEncaje
                    If (oArchivosVaxBCOSBM.SeleccionarPorFiltro(EncajeMantenido, vFondo, DecFechaProceso, DatosRequest).Tables(0).Rows.Count <> 0) Then
                        IndicadorEncaje = Convert.ToDecimal(oArchivosVaxBCOSBM.SeleccionarPorFiltro(EncajeMantenido, vFondo, DecFechaProceso, DatosRequest).Tables(0).Rows(0).Item("Valor"))
                    End If
                    'ValorFondo
                    'RGF 20090121 No estaba mostrando el "Valor Cartera Administrada" correcto
                    oParametrosMigracionBE = oParametrosMigracionBM.SeleccionarPorFiltro("ENCA", 8, "", "", "", DatosRequest)
                    'oParametrosMigracionBE = oParametrosMigracionBM.SeleccionarPorFiltro("ENCA", 6, "", "", "", datosrequest)
                    If (oParametrosMigracionBE.Tables(0).Rows.Count <> 0) Then
                        ValorFondo = oParametrosMigracionBE.Tables(0).Rows(0).Item("Valor").ToString()
                    End If
                    'IndicadorFondo
                    If (oArchivosVaxBCOSBM.SeleccionarPorFiltro(ValorFondo, vFondo, DecFechaProceso, DatosRequest).Tables(0).Rows.Count <> 0) Then
                        IndicadorFondo = Convert.ToDecimal(oArchivosVaxBCOSBM.SeleccionarPorFiltro(ValorFondo, vFondo, DecFechaProceso, DatosRequest).Tables(0).Rows(0).Item("Valor"))
                    End If
                    '------------------------
                    Dim oEncajeDetalleBM As New EncajeDetalleBM
                    Dim dtConsulta As New DataTable
                    Dim dsconsulta As New DsResultadoEncaje
                    Dim drconsulta As DataRow
                    Dim total_encaje, vector_factor As Decimal
                    dtConsulta = oEncajeDetalleBM.ReporteResultadosEncaje(vFondo, UIUtility.ConvertirFechaaDecimal(fecha), DatosRequest).Tables(0)
                    If dtConsulta.Rows.Count > 0 Then
                        For Each drv As DataRow In dtConsulta.Rows
                            drconsulta = dsconsulta.Tables(0).NewRow()
                            drconsulta("CodigoSBS") = drv("CodigoSBS")
                            drconsulta("TipoInstrumento") = drv("TipoInstrumento")
                            drconsulta("CodigoTipoInstrumento") = drv("CodigoTipoInstrumento")
                            drconsulta("Emision") = drv("Emision")
                            drconsulta("Emisor") = drv("Emisor")
                            drconsulta("SumaValorizados") = Format(Convert.ToDecimal(drv("SumaValorizados")), "###,##0.0000000")
                            drconsulta("Categoria") = drv("Categoria")
                            drconsulta("ValorNominal") = Format(Convert.ToDecimal(drv("ValorNominal")), "###,##0") 'RGF 20090116
                            drconsulta("Descripcion") = drv("Descripcion")
                            drconsulta("FechaEncaje") = drv("FechaEncaje")
                            drconsulta("NumeroTitulo") = drv("NumeroTitulo")
                            drconsulta("NumeroDias") = drv("NumeroDias")
                            drconsulta("PromedioValoracion") = Format(Convert.ToDecimal(drv("PromedioValoracion")), "###,##0.0000000")
                            drconsulta("ValorTasa") = Format(Convert.ToDecimal(drv("ValorTasa")), "###,##0.0000000")
                            drconsulta("ValorEncaje") = Format(Convert.ToDecimal(drv("ValorEncaje")), "###,##0.0000000")
                            drconsulta("ValorPromedioMantenido") = Format(Convert.ToDecimal(drv("ValorPromedioMantenido")), "###,##0.0000000")
                            drconsulta("CodigoCalificacion") = drv("CodigoCalificacion")
                            drconsulta("FechaVencimiento") = drv("FechaVencimiento")
                            drconsulta("ValorMantenido") = Format(Convert.ToDecimal(drv("ValorMantenido")), "###,##0.0000000")
                            drconsulta("ValorRequerido") = Format(Convert.ToDecimal(drv("ValorRequerido")), "###,##0.0000000")
                            drconsulta("VectorFactor") = Format(Convert.ToDecimal(drv("VectorFactor")), "###,##0.0000000")
                            drconsulta("SumaValorEncaje") = Format(Convert.ToDecimal(drv("SumaValorEncaje")), "###,##0.0000000")
                            drconsulta("TotalEncaje") = Format(Convert.ToDecimal(drv("TotalEncaje")), "###,##0.0000000")
                            drconsulta("VPNLocal") = Format(Convert.ToDecimal(drv("VPNLocal")), "###,##0.0000000")
                            dsconsulta.Tables(0).Rows.Add(drconsulta)
                        Next
                        dsconsulta.Merge(dsconsulta, False, System.Data.MissingSchemaAction.Ignore)
                        oReport.SetDataSource(dsconsulta)
                        total_encaje = Convert.ToDecimal(dtConsulta.Rows(0).Item(dtConsulta.Columns.IndexOf("TotalEncaje")))
                        vector_factor = Convert.ToDecimal(dtConsulta.Rows(0).Item(dtConsulta.Columns.IndexOf("VectorFactor")))
                        oReport.SetParameterValue("@FechaProceso", fecha)
                        oReport.SetParameterValue("@Usuario", usuario)
                        oReport.SetParameterValue("@Portafolio", obj.Tables(0).Rows(0)("Descripcion").ToString())
                        oReport.SetParameterValue("@EncajeMantenido", Format(IndicadorEncaje, "###,##0.0000000"))
                        oReport.SetParameterValue("@CarteraAdministrada", Format(IndicadorFondo, "###,##0.0000000"))
                        oReport.SetParameterValue("@TotalEncajeRequerido", Format(total_encaje * vector_factor, "###,##0.0000000"))
                        oReport.SetParameterValue("@rutaLogo", ConfigurationManager.AppSettings("RUTA_LOGO"))
                        Me.crEncaje.ReportSource = oReport
                    Else
                        AlertaJS("No existen registros")
                    End If
                Case "REN"
                    nombre = "Usuario"
                    Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
                    Dim drconsulta As DataRow
                    usuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & nombre & "'")(0)(1), String)
                    oReport.Load(Server.MapPath("RptRentabilidadFondoEncaje.rpt"))
                    Dim FechaFin As Decimal = Request.QueryString("pFechaFin")
                    Dim obm As New ReporteGestionBM
                    'RGF 20090108
                    Dim dtConsulta As DataTable = obm.GenerarReporteUtilidad(UIUtility.ConvertirFechaaDecimal(fecha), FechaFin, vFondo, DatosRequest).Tables(0)
                    Dim dsconsulta As New dsRentabilidadFondoEncaje
                    For Each drv As DataRow In dtConsulta.Rows
                        drconsulta = dsconsulta.Tables(0).NewRow()
                        drconsulta("TipoInstrumento") = drv("TipoInstrumento")
                        drconsulta("CodigoMNemonico") = drv("CodigoNemonico") 'RGF 20090108
                        'drconsulta("CodigoMNemonico") = drv("CodigoMNemonico")
                        drconsulta("UtilidadTotal") = drv("Utilidad")
                        drconsulta("UtilidadFondo") = 0 'RGF 20090108
                        'drconsulta("UtilidadFondo") = drv("UtilidadFondo") 
                        drconsulta("UtilidadEncaje") = drv("UtilidadEncaje")
                        dsconsulta.Tables(0).Rows.Add(drconsulta)
                    Next
                    dsconsulta.Merge(dsconsulta, False, System.Data.MissingSchemaAction.Ignore)
                    oReport.SetDataSource(dsconsulta)
                    oReport.SetParameterValue("@p_Usuario", usuario)
                    oReport.SetParameterValue("@p_FechaFin", UIUtility.ConvertirFechaaString(FechaFin))
                    oReport.SetParameterValue("@p_FechaInicio", fecha)
                    oReport.SetParameterValue("@p_Portafolio", obj.Tables(0).Rows(0)("Descripcion").ToString())
                    oReport.SetParameterValue("@rutaLogo", ConfigurationManager.AppSettings("RUTA_LOGO"))
                    Me.crEncaje.ReportSource = oReport
                Case "REN_ENCAJE"
                    nombre = "Usuario"
                    Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
                    Dim drconsulta As DataRow
                    usuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & nombre & "'")(0)(1), String)
                    oReport.Load(Server.MapPath("RptRentabilidadEncaje.rpt"))
                    Dim FechaFin As Decimal = UIUtility.ConvertirFechaaDecimal(Request.QueryString("pFechaFin"))
                    Dim obm As New ReporteGestionBM
                    Dim dtConsulta As DataTable = obm.GenerarReporteEncaje(UIUtility.ConvertirFechaaDecimal(fecha), FechaFin, vFondo, Nemonico, DatosRequest).Tables(0)
                    Dim dsconsulta As New dsRentabilidadEncaje
                    For Each drv As DataRow In dtConsulta.Rows
                        drconsulta = dsconsulta.Tables(0).NewRow()
                        drconsulta("TipoInstrumento") = drv("TipoInstrumento")
                        drconsulta("CodigoMNemonico") = drv("CodigoNemonico")
                        drconsulta("UtilidadTotal") = drv("Utilidad")
                        drconsulta("UtilidadFondo") = 0
                        drconsulta("UtilidadEncaje") = drv("UtilidadEncaje")
                        drconsulta("Fecha") = drv("Fecha")
                        dsconsulta.Tables(0).Rows.Add(drconsulta)
                    Next
                    dsconsulta.Merge(dsconsulta, False, System.Data.MissingSchemaAction.Ignore)
                    oReport.SetDataSource(dsconsulta)
                    oReport.SetParameterValue("@p_Usuario", usuario)
                    oReport.SetParameterValue("@p_FechaFin", UIUtility.ConvertirFechaaString(FechaFin))
                    oReport.SetParameterValue("@p_FechaInicio", fecha)
                    oReport.SetParameterValue("@p_Portafolio", obj.Tables(0).Rows(0)("Descripcion").ToString.Trim)
                    oReport.SetParameterValue("@rutaLogo", ConfigurationManager.AppSettings("RUTA_LOGO"))
                    Me.crEncaje.ReportSource = oReport
            End Select
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try        
    End Sub
    Protected Sub Modulos_Tesoreria_Encaje_frmVisorEncaje_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class