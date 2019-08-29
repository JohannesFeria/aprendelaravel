Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.IO
Imports System.Data
Partial Class Modulos_Contabilidad_Reportes_frmVisorAsientosContables
    Inherits BasePage
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
#Region "/* Metodos Personalizados */"
    Private Sub VisualizarReportesContables()
        Dim StrOpcion, StrUsuario, StrNombre, Strfondo, StrDescripcionFondo, StrEstadoPortafolio As String
        Dim DatFechaOperacionInicio, DatFechaOperacionFin As Date
        Dim DecFechaOperacionInicio, DecFechaOperacionFin As Decimal
        DatFechaOperacionInicio = CType(Session("FechaOperacionInicio"), Date)
        DatFechaOperacionFin = CType(Session("FechaOperacionFin"), Date)
        Strfondo = Session("ReporteContabilidad_Fondo")
        StrDescripcionFondo = Session("ReporteContabilidad_DescripcionFondo")
        StrOpcion = Session("titulo")
        StrNombre = "Usuario"
        Dim StrCodigoMercado As String = Session("CodigoMercado")
        Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
        StrUsuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & StrNombre & "'")(0)(1), String)
        DecFechaOperacionInicio = CType(DatFechaOperacionInicio.ToString("yyyyMMdd"), Decimal)
        DecFechaOperacionFin = CType(DatFechaOperacionFin.ToString("yyyyMMdd"), Decimal)
        If UIUtility.ObtenerFechaAperturaContable(Strfondo) <= DecFechaOperacionInicio Then
            StrEstadoPortafolio = "Abierto"
        Else
            StrEstadoPortafolio = "Cerrado"
        End If
        Dim fecha_ope As String = Convert.ToString(DecFechaOperacionInicio)
        Dim lote As String = fecha_ope.Substring(0, 4) + fecha_ope.Substring(4, 2) + "0001"
        Dim tipoLote As String
        Dim dtConsulta As New DataTable
        Dim dsconsulta As New DsRptAsientosContablesTipoT
        Dim drconsulta As DataRow
        oReport.Load(Server.MapPath("ReporteAsientosContablesTipoT.rpt"))
        Select Case StrOpcion
            Case "CVI"
                tipoLote = "I"
            Case "VC"
                tipoLote = "V"
            Case "CCI"
                tipoLote = "T"
            Case "PP"
                tipoLote = "P"
            Case "PI"
                tipoLote = "R"
            Case "COM"
                tipoLote = "C"
        End Select
        dtConsulta = New ReporteContabilidadBM().CobranzaCancelacionInversiones(DecFechaOperacionInicio, Strfondo, DecFechaOperacionFin, tipoLote, DatosRequest, StrCodigoMercado).Tables(0)
        For Each drv As DataRow In dtConsulta.Rows
            drconsulta = dsconsulta.Tables(0).NewRow()
            drconsulta("Asiento") = drv("Asiento")
            drconsulta("DetalleAsiento") = drv("DetalleAsiento")
            drconsulta("DescripcionCuenta") = drv("DescripcionCuenta")
            drconsulta("NumeroCuenta") = drv("NumeroCuenta")
            drconsulta("DescripcionOperacion") = drv("DescripcionOperacion")
            drconsulta("TipoImporte") = drv("TipoImporte")
            drconsulta("DescripcionCentroCosto") = drv("DescripcionCentroCosto")
            drconsulta("TipoContable") = drv("TipoContable")
            drconsulta("Divisa") = drv("Divisa")
            drconsulta("MontoDebe") = drv("MontoDebe")
            drconsulta("MontoHaber") = drv("MontoHaber")
            drconsulta("Referencia") = drv("Referencia")
            drconsulta("Transaccion") = drv("Transaccion")
            drconsulta("OperacionContable") = drv("OperacionContable")
            drconsulta("ImporteOrigen") = drv("ImporteOrigen")
            drconsulta("FechaCreacion") = drv("FechaCreacion")
            drconsulta("HoraCreacion") = drv("HoraCreacion")
            drconsulta("FechaModificacion") = drv("FechaModificacion")
            drconsulta("HoraModificacion") = drv("HoraModificacion")
            drconsulta("UsuarioModificacion") = drv("UsuarioModificacion")
            dsconsulta.Tables(0).Rows.Add(drconsulta)
        Next
        dsconsulta.Merge(dsconsulta, False, System.Data.MissingSchemaAction.Ignore)
        oReport.SetDataSource(dsconsulta)
        oReport.SetParameterValue("@Lote", lote)
        oReport.SetParameterValue("@FechaContable", UIUtility.ConvertirFechaaString(DecFechaOperacionInicio))
        oReport.SetParameterValue("@Fondo", StrDescripcionFondo)
        oReport.SetParameterValue("@EstadoPeriodo", StrEstadoPortafolio)
        Select Case StrOpcion
            Case "CVI"
                oReport.SetParameterValue("@TituloReporte", "REPORTE DE LOTE CONTABLE DE COMPRA Y VENTA DE INVERSIONES")
            Case "VC"
                oReport.SetParameterValue("@TituloReporte", "REPORTE DE LOTE CONTABLE DE VALORIZACION DE LA CARTERA DE INVERSIONES")
            Case "CCI"
                oReport.SetParameterValue("@TituloReporte", "REPORTE DE LOTE CONTABLE DE COBRANZA Y CANCELACION DE INVERSIONES")
            Case "PP"
                oReport.SetParameterValue("@TituloReporte", "REPORTE DE LOTE CONTABLE DE PROVISIÓN DE POLIZAS AGENTES DE BOLSA")
            Case "PI"
                oReport.SetParameterValue("@TituloReporte", "REPORTE DE LOTE CONTABLE DE PROVISIÓN DE INTERESES")
            Case "COM"
                oReport.SetParameterValue("@TituloReporte", "REPORTE DE LOTE CONTABLE DE COMISIÓN SAFM")
        End Select
        oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
        Me.CrystalReportViewer1.ReportSource = oReport
    End Sub
    Private Sub VisualizarReportesResuPUADM_Y_FONFO()
        Dim arrayNameFile() As String = Split(ConfigurationManager.AppSettings("ARCHIVOS_VAX"), ",")
        Dim strRutaVAX As String = ConfigurationManager.AppSettings("UBICACION_VAX")
        Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim DecFechaOperacionInicio As Decimal, DecFechaOperacionFin As Decimal
        Dim DatFechaOperacionInicio As Date, DatFechaOperacionFin As Date
        Dim Titulo As String = "RESUMEN ENVIO PU FONDO"
        Dim dsconsulta As New DsRptResumenEnvioPUADM
        Dim FechaArchivoRistra As String = ""
        Dim nombreArchivo As String = ""
        Dim dtConsulta As New DataTable
        Dim extension As String = "txt"
        Dim MontoHaber As Decimal = 0
        Dim MontoDebe As Decimal = 0
        Dim FileName As String = ""
        Dim sufijo As String = ""
        Dim drconsulta As DataRow
        Dim Strfondo As String
        Dim StrDescripcionFondo As String
        Dim strContext As String
        Dim montoHaberF17 As Decimal = 0
        Dim MontoDebeF17 As Decimal = 0
        Dim montoHaberC07 As Decimal = 0
        Dim MontoDebeC07 As Decimal = 0
        Dim montoHaberF18VAX As Decimal = 0
        Dim MontoDebeF18VAX As Decimal = 0
        Dim glosaCyC = "COBRANZA Y CANCELACION"
        Try
            DatFechaOperacionInicio = CType(Session("FechaOperacionInicio"), Date)
            DatFechaOperacionFin = CType(Session("FechaOperacionFin"), Date)
            DecFechaOperacionInicio = CType(DatFechaOperacionInicio.ToString("yyyyMMdd"), Decimal)
            DecFechaOperacionFin = CType(DatFechaOperacionFin.ToString("yyyyMMdd"), Decimal)
            Strfondo = Session("ReporteContabilidad_Fondo")
            StrDescripcionFondo = Session("ReporteContabilidad_DescripcionFondo")
            If Strfondo = ParametrosSIT.PORTAFOLIO_ADMINISTRA Then
                sufijo = "_A9"
            Else
                sufijo = "_F" & Strfondo
            End If
            If Strfondo.Equals(ParametrosSIT.PORTAFOLIO_ADMINISTRA) Then
                oReport.Load(Server.MapPath("ReporteResumenEnvioPUADM.rpt"))
                FechaArchivoRistra = DateTime.Now.ToString("yyyyMMdd")
            Else
                oReport.Load(Server.MapPath("ReporteResumenEnvioPUFONDOS.rpt"))
                FechaArchivoRistra = HelpRistra.RetornarFechaContableUtilAnterior(DateTime.Now)
            End If
            For iCont As Int16 = 0 To arrayNameFile.Length - 1
                If Strfondo = ParametrosSIT.PORTAFOLIO_ADMINISTRA Then
                    nombreArchivo = "pefp_ha_fix_f" + FechaArchivoRistra.Substring(2) + "_intapl1_" + arrayNameFile(iCont) + sufijo + "." + extension
                Else
                    nombreArchivo = "pefp_ha_fix_f" + FechaArchivoRistra.Substring(2) + "_intapl2_" + arrayNameFile(iCont) + sufijo + "." + extension
                End If
                'Ruta de archivo VAX
                FileName = strRutaVAX + nombreArchivo
                'Verificar existencia de archivo
                If File.Exists(FileName) = True Then
                    strContext = ""
                    Dim oSR As StreamReader
                    Try
                        oSR = New StreamReader(FileName)
                        While Not strContext Is Nothing
                            strContext = oSR.ReadLine
                            If strContext <> "" Then
                                'If Strfondo = "ADMINISTRA" Then
                                If Strfondo = ParametrosSIT.PORTAFOLIO_ADMINISTRA Then
                                    MontoHaber = MontoHaber + CDec(Mid(strContext, 140, 13) + "." + Mid(strContext, 153, 2))
                                    MontoDebe = MontoDebe + CDec(Mid(strContext, 125, 13) + "." + Mid(strContext, 138, 2))
                                Else 'RGF 20090709
                                    Select Case strContext.Substring(4, 3)
                                        Case "F17"
                                            montoHaberF17 = montoHaberF17 + CDec(Mid(strContext, 140, 13) + "." + Mid(strContext, 153, 2))
                                            MontoDebeF17 = MontoDebeF17 + CDec(Mid(strContext, 125, 13) + "." + Mid(strContext, 138, 2))
                                        Case "F18"
                                            montoHaberF18VAX = montoHaberF18VAX + CDec(Mid(strContext, 140, 13) + "." + Mid(strContext, 153, 2))
                                            MontoDebeF18VAX = MontoDebeF18VAX + CDec(Mid(strContext, 125, 13) + "." + Mid(strContext, 138, 2))
                                        Case "C07"
                                            montoHaberC07 = montoHaberC07 + CDec(Mid(strContext, 140, 13) + "." + Mid(strContext, 153, 2))
                                            MontoDebeC07 = MontoDebeC07 + CDec(Mid(strContext, 125, 13) + "." + Mid(strContext, 138, 2))
                                    End Select
                                End If
                            End If
                        End While
                    Catch ex As Exception
                        AlertaJS(ex.Message.ToString)
                    Finally
                        oSR.Close()
                        oSR = Nothing
                        GC.Collect()
                    End Try
                End If
            Next iCont
            '******************************Agregar resumen de VAX en primera fila************************
            drconsulta = dsconsulta.Tables(0).NewRow()
            drconsulta("FechaContable") = DateTime.Now.ToString("dd/MM/yyyy")

            If Strfondo = ParametrosSIT.PORTAFOLIO_ADMINISTRA Then
                drconsulta("MontoHaber") = CStr(MontoHaber)
                drconsulta("MontoDebe") = CStr(MontoDebe)
                drconsulta("Glosa") = "VAX"
                dsconsulta.Tables(0).Rows.Add(drconsulta)
            Else 'RGF 20090709
                drconsulta("MontoHaber") = CStr(montoHaberF17)
                drconsulta("MontoDebe") = CStr(MontoDebeF17)
                drconsulta("Glosa") = "RECAUDACION"
                dsconsulta.Tables(0).Rows.Add(drconsulta)

                'RGF 20090713
                drconsulta = dsconsulta.Tables(0).NewRow()
                drconsulta("FechaContable") = DateTime.Now.ToString("dd/MM/yyyy")
                drconsulta("MontoHaber") = CStr(montoHaberF18VAX)
                drconsulta("MontoDebe") = CStr(MontoDebeF18VAX)
                drconsulta("Glosa") = glosaCyC 'RGF 20090714
                dsconsulta.Tables(0).Rows.Add(drconsulta)
                'If Strfondo.ToUpper.Equals("HO-FONDO2") Then
                'If Strfondo.ToUpper.Equals(ParametrosSIT.PORTAFOLIO_FONDO2) Then
                '    drconsulta = dsconsulta.Tables(0).NewRow()
                '    drconsulta("FechaContable") = DateTime.Now.ToString("dd/MM/yyyy")
                '    drconsulta("MontoHaber") = CStr(montoHaberC07)
                '    drconsulta("MontoDebe") = CStr(MontoDebeC07)
                '    drconsulta("Glosa") = "CARGOS A LA CIC" 'RGF colocar nombre de lote
                '    dsconsulta.Tables(0).Rows.Add(drconsulta)
                'End If
            End If
            '******************************Resumen de envio de PU ADM (Del SIT)**************************
            dtConsulta = Session("dtConsultaRPA")

            For Each drv As DataRow In dtConsulta.Rows
                'If Not (Strfondo.StartsWith("HO-FONDO") And drv("Glosa").Equals(glosaCyC)) Then 'RGF 20090714 La ristra pefp_ha_fix_f090713_intapl2_teso_f2 ya contiene el lote de CyC del SIT
                If Not (Strfondo.StartsWith(StrDescripcionFondo) And drv("Glosa").Equals(glosaCyC)) Then 'RGF 20090714 La ristra pefp_ha_fix_f090713_intapl2_teso_f2 ya contiene el lote de CyC del SIT
                    drconsulta = dsconsulta.Tables(0).NewRow()
                    drconsulta("FechaContable") = drv("FechaContable")
                    drconsulta("Glosa") = drv("Glosa")
                    drconsulta("MontoDebe") = drv("MontoDebe")
                    drconsulta("MontoHaber") = drv("MontoHaber")
                    dsconsulta.Tables(0).Rows.Add(drconsulta)
                End If
            Next
            dsconsulta.Merge(dsconsulta, False, System.Data.MissingSchemaAction.Ignore)
            oReport.SetDataSource(dsconsulta)
            If Not Strfondo.Equals(ParametrosSIT.PORTAFOLIO_ADMINISTRA) Then
                oReport.SetParameterValue("@FechaOperacion", "Desde: " & DatFechaOperacionInicio.ToString("dd/MM/yyyy") & " Hasta: " & DatFechaOperacionFin.ToString("dd/MM/yyyy"))
            End If
            oReport.SetParameterValue("@Usuario", Usuario)
            oReport.SetParameterValue("@Fondo", StrDescripcionFondo)
            oReport.SetParameterValue("@Titulo", Titulo)
            oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
            CrystalReportViewer1.ReportSource = oReport
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
#End Region
#Region "/* Eventos del WebForm */"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Session("titulo") = "RPA" Then
            VisualizarReportesResuPUADM_Y_FONFO()
        Else
            VisualizarReportesContables()
        End If
    End Sub
    Private Sub CopiarTabla(ByRef dtOrigen As DataTable, ByRef dtDestino As DataTable)
        For Each dr As DataRow In dtOrigen.Rows
            dtDestino.LoadDataRow(dr.ItemArray, False)
        Next
    End Sub
#End Region
    Protected Sub Modulos_Contabilidad_Reportes_frmVisorAsientosContables_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class