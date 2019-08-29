Imports Sit.BusinessLayer
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports CrystalDecisions.Shared
Imports System.Data
Imports ParametrosSIT
Partial Class Modulos_Gestion_Reportes_frmVisorReportesCartera
    Inherits BasePage
    Dim pRutas As String
    Dim rutas As New System.Text.StringBuilder
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
        Dim StrOpcion, StrUsuario, StrNombre, Strfondo As String
        Dim StrEstadoPortafolio As String = ""
        Dim DatFechaOperacionIni As Date
        Dim DecFechaOperacionIni As Decimal

        DatFechaOperacionIni = Request.QueryString("vfechainicio")
        Strfondo = Session("ReporteContabilidad_Fondo")
        StrOpcion = Request.QueryString("vopcion")
        StrNombre = "Usuario"

        Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
        StrUsuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & StrNombre & "'")(0)(1), String)

        DecFechaOperacionIni = CType(DatFechaOperacionIni.ToString("yyyyMMdd"), Decimal)

        Dim drfila As DataRow

        Select Case StrOpcion
            Case ANEXO_A3A_03

                oReport.Load(Server.MapPath("InformeDiario_AnexoIIIA.rpt"))

                Dim dsReporte As New AnexoIDI3A

                Dim oreporte As DataSet

                oreporte = New ReporteGestionBM().ReporteAnexoIDI3A(DecFechaOperacionIni, Strfondo, DatosRequest)
                For Each dr As DataRow In oreporte.Tables(0).Rows
                    drfila = dsReporte.Tables(0).NewRow
                    drfila("CodigoPortafolioSBS") = dr("CodigoPortafolioSBS")
                    drfila("FechaProceso") = dr("FechaProceso")
                    drfila("CodigoSBS") = dr("CodigoSBS")
                    drfila("Consecutivo") = dr("Consecutivo")
                    drfila("FechaNegociacion") = dr("FechaNegociacion")
                    drfila("HoraNegociacion") = dr("HoraNegociacion")
                    drfila("NumeroSecuencia") = dr("NumeroSecuencia")
                    drfila("Movimiento") = dr("Movimiento")
                    drfila("UnidadesTransadas") = dr("UnidadesTransadas")
                    drfila("PrecioTransaccion") = dr("PrecioTransaccion")
                    drfila("FechaLiquidacion") = dr("FechaLiquidacion")
                    drfila("PlazoLiquidacion") = dr("PlazoLiquidacion")
                    drfila("CodigoIntermediario") = dr("CodigoIntermediario")
                    drfila("NombreIntermediario") = dr("NombreIntermediario")
                    drfila("IndicaCajaBanco") = dr("IndicaCajaBanco")
                    drfila("NumeroOperacion") = dr("NumeroOperacion")
                    drfila("Plaza") = dr("Plaza")
                    dsReporte.Tables(0).Rows.Add(drfila)
                Next

                oReport.SetDataSource(dsReporte)

                oReport.SetParameterValue("@FechaOperacionIni", DatFechaOperacionIni.Day.ToString().PadLeft(2, "0") + "/" + DatFechaOperacionIni.Month.ToString().PadLeft(2, "0") + "/" + DatFechaOperacionIni.Year.ToString())
                oReport.SetParameterValue("@Usuario", StrUsuario)
                oReport.SetParameterValue("@Fondo", Strfondo)
                oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))

                Me.CrystalReportViewer1.ReportSource = oReport

            Case ANEXO_A3B_04

                oReport.Load(Server.MapPath("InformeDiario_AnexoIIIB.rpt"))

                Dim dsReporte As New AnexoIDI3B

                Dim oreporte As DataSet

                oreporte = New ReporteGestionBM().ReporteAnexoIDI3B(DecFechaOperacionIni, Strfondo, DatosRequest)
                For Each dr As DataRow In oreporte.Tables(0).Rows
                    drfila = dsReporte.Tables(0).NewRow
                    drfila("CodigoPortafolioSBS") = dr("CodigoPortafolioSBS")
                    drfila("FechaProceso") = dr("FechaProceso")
                    drfila("CodigoSBS") = dr("CodigoSBS")
                    drfila("Consecutivo") = dr("Consecutivo")
                    drfila("FechaNegociacion") = dr("FechaNegociacion")
                    drfila("HoraNegociacion") = dr("HoraNegociacion")
                    drfila("FechaEmision") = dr("FechaEmision")
                    drfila("Movimiento") = dr("Movimiento")
                    drfila("FechaVencimiento") = dr("FechaVencimiento")
                    drfila("ValorInicial") = dr("ValorInicial")
                    drfila("ValorAlVencimiento") = dr("ValorAlVencimiento")
                    drfila("TipoTasa") = dr("TipoTasa")
                    drfila("NombreTasa") = dr("NombreTasa")
                    drfila("TasaInteresAnual") = dr("TasaInteresAnual")
                    drfila("PlazoLiquidacion") = dr("PlazoLiquidacion")
                    drfila("BaseAnual") = dr("BaseAnual")
                    drfila("MetodoValoracion") = dr("MetodoValoracion")
                    drfila("CodigoIntermediario") = dr("CodigoIntermediario")
                    drfila("NombreIntermediario") = dr("NombreIntermediario")
                    drfila("IndicaCajaBanco") = dr("IndicaCajaBanco")
                    drfila("Plaza") = dr("Plaza")

                    dsReporte.Tables(0).Rows.Add(drfila)
                Next

                oReport.SetDataSource(dsReporte)

                oReport.SetParameterValue("@FechaOperacionIni", DatFechaOperacionIni.Day.ToString().PadLeft(2, "0") + "/" + DatFechaOperacionIni.Month.ToString().PadLeft(2, "0") + "/" + DatFechaOperacionIni.Year.ToString())
                oReport.SetParameterValue("@Usuario", StrUsuario)
                oReport.SetParameterValue("@Fondo", Strfondo)
                oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))

                Me.CrystalReportViewer1.ReportSource = oReport

            Case ANEXO_A6_08

                oReport.Load(Server.MapPath("InformeDiario__AnexoVI.rpt"))

                Dim dsReporte As New AnexoIDI6

                Dim oreporte As DataSet

                oreporte = New ReporteGestionBM().ReporteAnexoIDI6(DecFechaOperacionIni, Strfondo, DatosRequest)
                For Each dr As DataRow In oreporte.Tables(0).Rows
                    drfila = dsReporte.Tables(0).NewRow
                    drfila("CodigoPortafolioSBS") = dr("CodigoPortafolioSBS")
                    drfila("FechaProceso") = dr("FechaProceso")
                    drfila("CodigoSBS") = dr("CodigoSBS")
                    drfila("Consecutivo") = dr("Consecutivo")
                    drfila("FechaLiquidacion") = dr("FechaLiquidacion")
                    drfila("PlazoLiquidacion") = dr("PlazoLiquidacion")
                    drfila("Tenencia") = dr("Tenencia")
                    drfila("Movimiento") = dr("Movimiento")
                    drfila("Factor") = dr("Factor")
                    drfila("CodigoMoneda") = dr("CodigoMoneda")
                    drfila("DescripcionMoneda") = dr("DescripcionMoneda")
                    drfila("TotalMonedaOrigen") = dr("TotalMonedaOrigen")
                    drfila("TotalMonedaLocal") = dr("TotalMonedaLocal")
                    drfila("IndicaCajaBanco") = dr("IndicaCajaBanco")
                    dsReporte.Tables(0).Rows.Add(drfila)
                Next

                oReport.SetDataSource(dsReporte)

                oReport.SetParameterValue("@FechaOperacionIni", DatFechaOperacionIni.Day.ToString().PadLeft(2, "0") + "/" + DatFechaOperacionIni.Month.ToString().PadLeft(2, "0") + "/" + DatFechaOperacionIni.Year.ToString())
                oReport.SetParameterValue("@Usuario", StrUsuario)
                oReport.SetParameterValue("@Fondo", Strfondo)
                oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))

                Me.CrystalReportViewer1.ReportSource = oReport

            Case ANEXO_A7_09

                oReport.Load(Server.MapPath("InformeDiario_AnexoVII.rpt"))

                Dim dsReporte As New AnexoIDI7

                Dim oreporte As DataSet

                oreporte = New ReporteGestionBM().ReporteAnexoIDI7(DecFechaOperacionIni, Strfondo, DatosRequest)
                For Each dr As DataRow In oreporte.Tables(0).Rows
                    drfila = dsReporte.Tables(0).NewRow
                    drfila("CodigoPortafolioSBS") = dr("CodigoPortafolioSBS")
                    drfila("FechaProceso") = dr("FechaProceso")
                    drfila("Emisor") = dr("Emisor")
                    drfila("Consecutivo") = dr("Consecutivo")
                    drfila("FechaNegociacion") = dr("FechaNegociacion")
                    drfila("HoraNegociacion") = dr("HoraNegociacion")
                    drfila("CodigoMonedaCompra") = dr("CodigoMonedaCompra")
                    drfila("CodigoMonedaVenta") = dr("CodigoMonedaVenta")
                    drfila("DescripcionMonedaCompra") = dr("DescripcionMonedaCompra")
                    drfila("DescripcionMonedaVenta") = dr("DescripcionMonedaVenta")
                    drfila("IndiceCajaBanco") = dr("IndiceCajaBanco")
                    drfila("MontoVenta") = dr("MontoVenta")
                    drfila("MontoCompra") = dr("MontoCompra")
                    drfila("TipoCambio") = dr("TipoCambio")
                    drfila("FechaLiquidacion") = dr("FechaLiquidacion")
                    drfila("PlazoLiquidacion") = dr("PlazoLiquidacion")
                    drfila("Plaza") = dr("Plaza")
                    dsReporte.Tables(0).Rows.Add(drfila)
                Next

                oReport.SetDataSource(dsReporte)

                oReport.SetParameterValue("@FechaOperacionIni", DatFechaOperacionIni.Day.ToString().PadLeft(2, "0") + "/" + DatFechaOperacionIni.Month.ToString().PadLeft(2, "0") + "/" + DatFechaOperacionIni.Year.ToString())
                oReport.SetParameterValue("@Usuario", StrUsuario)
                oReport.SetParameterValue("@Fondo", Strfondo)
                oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))

                Me.CrystalReportViewer1.ReportSource = oReport

            Case ANEXO_A8_10

                oReport.Load(Server.MapPath("InformeDiario_AnexoVIII.rpt"))

                Dim dsReporte As New AnexoIDI8

                Dim oreporte As DataSet

                oreporte = New ReporteGestionBM().ReporteAnexoIDI8(DecFechaOperacionIni, Strfondo, DatosRequest)
                For Each dr As DataRow In oreporte.Tables(0).Rows
                    drfila = dsReporte.Tables(0).NewRow
                    drfila("CodigoPortafolioSBS") = dr("CodigoPortafolioSBS")
                    drfila("FechaProceso") = dr("FechaProceso")
                    drfila("CodigoSBS") = dr("CodigoSBS")
                    drfila("Consecutivo") = dr("Consecutivo")
                    drfila("CantidadInstrumento") = dr("CantidadInstrumento")
                    drfila("InstrumentosTransito") = dr("InstrumentosTransito")
                    drfila("TotalInstrumentos") = dr("TotalInstrumentos")
                    drfila("ValorizacionInstrumento") = dr("ValorizacionInstrumento")
                    drfila("CodigoCustodio") = dr("CodigoCustodio")
                    drfila("DescripcionCustodio") = dr("DescripcionCustodio")
                    dsReporte.Tables(0).Rows.Add(drfila)
                Next

                oReport.SetDataSource(dsReporte)

                oReport.SetParameterValue("@FechaOperacionIni", DatFechaOperacionIni.Day.ToString().PadLeft(2, "0") + "/" + DatFechaOperacionIni.Month.ToString().PadLeft(2, "0") + "/" + DatFechaOperacionIni.Year.ToString())
                oReport.SetParameterValue("@Usuario", StrUsuario)
                oReport.SetParameterValue("@Fondo", Strfondo)
                oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))

                Me.CrystalReportViewer1.ReportSource = oReport

            Case ANEXO_A9_11

                oReport.Load(Server.MapPath("InformeDiario_AnexoIX.rpt"))

                Dim dsReporte As New AnexoIDI9
                Dim oReporte As DataSet

                oReporte = New ReporteGestionBM().ReporteAnexoIDI9(DecFechaOperacionIni, Strfondo, DatosRequest)
                For Each dr As DataRow In oReporte.Tables(0).Rows
                    drfila = dsReporte.Tables(0).NewRow
                    drfila("CodigoSBS") = dr("CodigoSBS")
                    drfila("HoraNegociacion") = dr("HoraNegociacion")
                    drfila("CodigoMonedaVenta") = dr("CodigoMonedaVenta")
                    drfila("Movimiento") = dr("Movimiento")
                    drfila("IndicadorForward") = dr("IndicadorForward")
                    drfila("MontoForward") = dr("MontoForward")
                    drfila("PrecioTransaccion") = dr("PrecioTransaccion")
                    drfila("FechaVencimiento") = dr("FechaVencimiento")
                    drfila("PlazoVencimiento") = dr("PlazoVencimiento")
                    drfila("Modalidad") = dr("Modalidad")
                    drfila("TipoCambio") = dr("TipoCambio")
                    drfila("IndicadorCaja") = dr("IndicadorCaja")
                    drfila("Plaza") = dr("Plaza")
                    drfila("DescripcionMoneda") = dr("DescripcionMoneda")
                    drfila("FechaEmision") = dr("FechaEmision")
                    drfila("FechaLiquidacion") = dr("FechaLiquidacion")
                    drfila("Intermediario") = dr("Intermediario")
                    dsReporte.Tables(0).Rows.Add(drfila)
                Next

                oReport.SetDataSource(dsReporte)

                oReport.SetParameterValue("@FechaOperacionIni", DatFechaOperacionIni.Day.ToString().PadLeft(2, "0") + "/" + DatFechaOperacionIni.Month.ToString().PadLeft(2, "0") + "/" + DatFechaOperacionIni.Year.ToString())
                oReport.SetParameterValue("@Usuario", StrUsuario)
                oReport.SetParameterValue("@Fondo", Strfondo)
                oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))

                Me.CrystalReportViewer1.ReportSource = oReport

            Case Constantes.M_STR_TEXTO_TODOS
                Dim rutaArchivo As String = ""
                Dim Anexos As String = ""
                Dim destinoPdf As String = ""
                Dim nombreNuevoArchivo As String = ""
                Dim PrefijoFolder As String = "Anexos_"
                Dim fechaActual As String = System.DateTime.Now.ToString("yyyyMMdd")
                Dim RutaTemp As String = New ParametrosGeneralesBM().Listar("RUTA_TEMP", DatosRequest).Rows(0)("Valor")
                Dim foldersAsesoria() As String = Directory.GetDirectories(RutaTemp, PrefijoFolder & "*")
                Dim folderActual As String = RutaTemp & PrefijoFolder & fechaActual
                Dim cont As Integer

                Try
                    For cont = 0 To foldersAsesoria.Length - 1
                        If Not foldersAsesoria(cont).Equals(folderActual) Then
                            Try
                                Directory.Delete(foldersAsesoria(cont), True)
                            Catch ex As Exception
                            End Try
                        End If
                    Next
                    If Not Directory.Exists(folderActual) Then
                        Directory.CreateDirectory(folderActual)
                    End If
                    folderActual = folderActual & "\"

                    Dim dsReporte As Object
                    Dim oreporte As DataSet

                    oReport.Load(Server.MapPath("InformeDiario_AnexoIIIA.rpt"))
                    dsReporte = New AnexoIDI3A

                    oreporte = New ReporteGestionBM().ReporteAnexoIDI3A(DecFechaOperacionIni, Strfondo, DatosRequest)
                    For Each dr As DataRow In oreporte.Tables(0).Rows
                        drfila = dsReporte.Tables(0).NewRow
                        drfila("CodigoPortafolioSBS") = dr("CodigoPortafolioSBS")
                        drfila("FechaProceso") = dr("FechaProceso")
                        drfila("CodigoSBS") = dr("CodigoSBS")
                        drfila("Consecutivo") = dr("Consecutivo")
                        drfila("FechaNegociacion") = dr("FechaNegociacion")
                        drfila("HoraNegociacion") = dr("HoraNegociacion")
                        drfila("NumeroSecuencia") = dr("NumeroSecuencia")
                        drfila("Movimiento") = dr("Movimiento")
                        drfila("UnidadesTransadas") = dr("UnidadesTransadas")
                        drfila("PrecioTransaccion") = dr("PrecioTransaccion")
                        drfila("FechaLiquidacion") = dr("FechaLiquidacion")
                        drfila("PlazoLiquidacion") = dr("PlazoLiquidacion")
                        drfila("CodigoIntermediario") = dr("CodigoIntermediario")
                        drfila("NombreIntermediario") = dr("NombreIntermediario")
                        drfila("IndicaCajaBanco") = dr("IndicaCajaBanco")
                        drfila("NumeroOperacion") = dr("NumeroOperacion")
                        drfila("Plaza") = dr("Plaza")
                        dsReporte.Tables(0).Rows.Add(drfila)
                    Next

                    oReport.SetDataSource(dsReporte)

                    oReport.SetParameterValue("@FechaOperacionIni", DatFechaOperacionIni.Day.ToString().PadLeft(2, "0") + "/" + DatFechaOperacionIni.Month.ToString().PadLeft(2, "0") + "/" + DatFechaOperacionIni.Year.ToString())
                    oReport.SetParameterValue("@Usuario", StrUsuario)
                    oReport.SetParameterValue("@Fondo", Strfondo)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))

                    If Not (oReport Is Nothing) Then
                        rutaArchivo = folderActual & System.Guid.NewGuid().ToString() & ".pdf"
                        oReport.ExportToDisk(ExportFormatType.PortableDocFormat, rutaArchivo)
                        rutas.Append("&" & rutaArchivo)
                        pRutas = rutas.ToString()
                    End If

                    oReport.Load(Server.MapPath("InformeDiario_AnexoIIIB.rpt"))
                    dsReporte = New AnexoIDI3B

                    oreporte = New ReporteGestionBM().ReporteAnexoIDI3B(DecFechaOperacionIni, Strfondo, DatosRequest)
                    For Each dr As DataRow In oreporte.Tables(0).Rows
                        drfila = dsReporte.Tables(0).NewRow
                        drfila("CodigoPortafolioSBS") = dr("CodigoPortafolioSBS")
                        drfila("FechaProceso") = dr("FechaProceso")
                        drfila("CodigoSBS") = dr("CodigoSBS")
                        drfila("Consecutivo") = dr("Consecutivo")
                        drfila("FechaNegociacion") = dr("FechaNegociacion")
                        drfila("HoraNegociacion") = dr("HoraNegociacion")
                        drfila("FechaEmision") = dr("FechaEmision")
                        drfila("Movimiento") = dr("Movimiento")
                        drfila("FechaVencimiento") = dr("FechaVencimiento")
                        drfila("ValorInicial") = dr("ValorInicial")
                        drfila("ValorAlVencimiento") = dr("ValorAlVencimiento")
                        drfila("TipoTasa") = dr("TipoTasa")
                        drfila("NombreTasa") = dr("NombreTasa")
                        drfila("TasaInteresAnual") = dr("TasaInteresAnual")
                        drfila("PlazoLiquidacion") = dr("PlazoLiquidacion")
                        drfila("BaseAnual") = dr("BaseAnual")
                        drfila("MetodoValoracion") = dr("MetodoValoracion")
                        drfila("CodigoIntermediario") = dr("CodigoIntermediario")
                        drfila("NombreIntermediario") = dr("NombreIntermediario")
                        drfila("IndicaCajaBanco") = dr("IndicaCajaBanco")
                        drfila("Plaza") = dr("Plaza")

                        dsReporte.Tables(0).Rows.Add(drfila)
                    Next

                    oReport.SetDataSource(dsReporte)

                    oReport.SetParameterValue("@FechaOperacionIni", DatFechaOperacionIni.Day.ToString().PadLeft(2, "0") + "/" + DatFechaOperacionIni.Month.ToString().PadLeft(2, "0") + "/" + DatFechaOperacionIni.Year.ToString())
                    oReport.SetParameterValue("@Usuario", StrUsuario)
                    oReport.SetParameterValue("@Fondo", Strfondo)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))

                    If Not (oReport Is Nothing) Then
                        rutaArchivo = folderActual & System.Guid.NewGuid().ToString() & ".pdf"
                        oReport.ExportToDisk(ExportFormatType.PortableDocFormat, rutaArchivo)
                        rutas.Append("&" & rutaArchivo)
                        pRutas = rutas.ToString()
                    End If

                    oReport.Load(Server.MapPath("InformeDiario__AnexoVI.rpt"))
                    dsReporte = New AnexoIDI6

                    oreporte = New ReporteGestionBM().ReporteAnexoIDI6(DecFechaOperacionIni, Strfondo, DatosRequest)
                    For Each dr As DataRow In oreporte.Tables(0).Rows
                        drfila = dsReporte.Tables(0).NewRow
                        drfila("CodigoPortafolioSBS") = dr("CodigoPortafolioSBS")
                        drfila("FechaProceso") = dr("FechaProceso")
                        drfila("CodigoSBS") = dr("CodigoSBS")
                        drfila("Consecutivo") = dr("Consecutivo")
                        drfila("FechaLiquidacion") = dr("FechaLiquidacion")
                        drfila("PlazoLiquidacion") = dr("PlazoLiquidacion")
                        drfila("Tenencia") = dr("Tenencia")
                        drfila("Movimiento") = dr("Movimiento")
                        drfila("Factor") = dr("Factor")
                        drfila("CodigoMoneda") = dr("CodigoMoneda")
                        drfila("DescripcionMoneda") = dr("DescripcionMoneda")
                        drfila("TotalMonedaOrigen") = dr("TotalMonedaOrigen")
                        drfila("TotalMonedaLocal") = dr("TotalMonedaLocal")
                        drfila("IndicaCajaBanco") = dr("IndicaCajaBanco")
                        dsReporte.Tables(0).Rows.Add(drfila)
                    Next

                    oReport.SetDataSource(dsReporte)

                    oReport.SetParameterValue("@FechaOperacionIni", DatFechaOperacionIni.Day.ToString().PadLeft(2, "0") + "/" + DatFechaOperacionIni.Month.ToString().PadLeft(2, "0") + "/" + DatFechaOperacionIni.Year.ToString())
                    oReport.SetParameterValue("@Usuario", StrUsuario)
                    oReport.SetParameterValue("@Fondo", Strfondo)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))

                    If Not (oReport Is Nothing) Then
                        rutaArchivo = folderActual & System.Guid.NewGuid().ToString() & ".pdf"
                        oReport.ExportToDisk(ExportFormatType.PortableDocFormat, rutaArchivo)
                        rutas.Append("&" & rutaArchivo)
                        pRutas = rutas.ToString()
                    End If

                    oReport.Load(Server.MapPath("InformeDiario_AnexoVII.rpt"))
                    dsReporte = New AnexoIDI7

                    oreporte = New ReporteGestionBM().ReporteAnexoIDI7(DecFechaOperacionIni, Strfondo, DatosRequest)
                    For Each dr As DataRow In oreporte.Tables(0).Rows
                        drfila = dsReporte.Tables(0).NewRow
                        drfila("CodigoPortafolioSBS") = dr("CodigoPortafolioSBS")
                        drfila("FechaProceso") = dr("FechaProceso")
                        drfila("Emisor") = dr("Emisor")
                        drfila("Consecutivo") = dr("Consecutivo")
                        drfila("FechaNegociacion") = dr("FechaNegociacion")
                        drfila("HoraNegociacion") = dr("HoraNegociacion")
                        drfila("CodigoMonedaCompra") = dr("CodigoMonedaCompra")
                        drfila("CodigoMonedaVenta") = dr("CodigoMonedaVenta")
                        drfila("DescripcionMonedaCompra") = dr("DescripcionMonedaCompra")
                        drfila("DescripcionMonedaVenta") = dr("DescripcionMonedaVenta")
                        drfila("IndiceCajaBanco") = dr("IndiceCajaBanco")
                        drfila("MontoVenta") = dr("MontoVenta")
                        drfila("MontoCompra") = dr("MontoCompra")
                        drfila("TipoCambio") = dr("TipoCambio")
                        drfila("FechaLiquidacion") = dr("FechaLiquidacion")
                        drfila("PlazoLiquidacion") = dr("PlazoLiquidacion")
                        drfila("Plaza") = dr("Plaza")
                        dsReporte.Tables(0).Rows.Add(drfila)
                    Next

                    oReport.SetDataSource(dsReporte)

                    oReport.SetParameterValue("@FechaOperacionIni", DatFechaOperacionIni.Day.ToString().PadLeft(2, "0") + "/" + DatFechaOperacionIni.Month.ToString().PadLeft(2, "0") + "/" + DatFechaOperacionIni.Year.ToString())
                    oReport.SetParameterValue("@Usuario", StrUsuario)
                    oReport.SetParameterValue("@Fondo", Strfondo)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))

                    If Not (oReport Is Nothing) Then
                        rutaArchivo = folderActual & System.Guid.NewGuid().ToString() & ".pdf"
                        oReport.ExportToDisk(ExportFormatType.PortableDocFormat, rutaArchivo)
                        rutas.Append("&" & rutaArchivo)
                        pRutas = rutas.ToString()
                    End If

                    oReport.Load(Server.MapPath("InformeDiario_AnexoVIII.rpt"))
                    dsReporte = New AnexoIDI8

                    oreporte = New ReporteGestionBM().ReporteAnexoIDI8(DecFechaOperacionIni, Strfondo, DatosRequest)
                    For Each dr As DataRow In oreporte.Tables(0).Rows
                        drfila = dsReporte.Tables(0).NewRow
                        drfila("CodigoPortafolioSBS") = dr("CodigoPortafolioSBS")
                        drfila("FechaProceso") = dr("FechaProceso")
                        drfila("CodigoSBS") = dr("CodigoSBS")
                        drfila("Consecutivo") = dr("Consecutivo")
                        drfila("CantidadInstrumento") = dr("CantidadInstrumento")
                        drfila("InstrumentosTransito") = dr("InstrumentosTransito")
                        drfila("TotalInstrumentos") = dr("TotalInstrumentos")
                        drfila("ValorizacionInstrumento") = dr("ValorizacionInstrumento")
                        drfila("CodigoCustodio") = dr("CodigoCustodio")
                        drfila("DescripcionCustodio") = dr("DescripcionCustodio")
                        dsReporte.Tables(0).Rows.Add(drfila)
                    Next

                    oReport.SetDataSource(dsReporte)

                    oReport.SetParameterValue("@FechaOperacionIni", DatFechaOperacionIni.Day.ToString().PadLeft(2, "0") + "/" + DatFechaOperacionIni.Month.ToString().PadLeft(2, "0") + "/" + DatFechaOperacionIni.Year.ToString())
                    oReport.SetParameterValue("@Usuario", StrUsuario)
                    oReport.SetParameterValue("@Fondo", Strfondo)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))

                    If Not (oReport Is Nothing) Then
                        rutaArchivo = folderActual & System.Guid.NewGuid().ToString() & ".pdf"
                        oReport.ExportToDisk(ExportFormatType.PortableDocFormat, rutaArchivo)
                        rutas.Append("&" & rutaArchivo)
                        pRutas = rutas.ToString()
                    End If

                    oReport.Load(Server.MapPath("InformeDiario_AnexoIX.rpt"))
                    dsReporte = New AnexoIDI9

                    oreporte = New ReporteGestionBM().ReporteAnexoIDI9(DecFechaOperacionIni, Strfondo, DatosRequest)
                    For Each dr As DataRow In oreporte.Tables(0).Rows
                        drfila = dsReporte.Tables(0).NewRow
                        drfila("CodigoSBS") = dr("CodigoSBS")
                        drfila("HoraNegociacion") = dr("HoraNegociacion")
                        drfila("CodigoMonedaVenta") = dr("CodigoMonedaVenta")
                        drfila("Movimiento") = dr("Movimiento")
                        drfila("IndicadorForward") = dr("IndicadorForward")
                        drfila("MontoForward") = dr("MontoForward")
                        drfila("PrecioTransaccion") = dr("PrecioTransaccion")
                        drfila("FechaVencimiento") = dr("FechaVencimiento")
                        drfila("PlazoVencimiento") = dr("PlazoVencimiento")
                        drfila("Modalidad") = dr("Modalidad")
                        drfila("TipoCambio") = dr("TipoCambio")
                        drfila("IndicadorCaja") = dr("IndicadorCaja")
                        drfila("Plaza") = dr("Plaza")
                        drfila("DescripcionMoneda") = dr("DescripcionMoneda")
                        drfila("FechaEmision") = dr("FechaEmision")
                        drfila("FechaLiquidacion") = dr("FechaLiquidacion")
                        drfila("Intermediario") = dr("Intermediario")
                        dsReporte.Tables(0).Rows.Add(drfila)
                    Next

                    oReport.SetDataSource(dsReporte)

                    oReport.SetParameterValue("@FechaOperacionIni", DatFechaOperacionIni.Day.ToString().PadLeft(2, "0") + "/" + DatFechaOperacionIni.Month.ToString().PadLeft(2, "0") + "/" + DatFechaOperacionIni.Year.ToString())
                    oReport.SetParameterValue("@Usuario", StrUsuario)
                    oReport.SetParameterValue("@Fondo", Strfondo)
                    oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))

                    If Not (oReport Is Nothing) Then
                        rutaArchivo = folderActual & System.Guid.NewGuid().ToString() & ".pdf"
                        oReport.ExportToDisk(ExportFormatType.PortableDocFormat, rutaArchivo)
                        rutas.Append("&" & rutaArchivo)
                        pRutas = rutas.ToString()
                    End If

                    Anexos = rutas.ToString()
                    nombreNuevoArchivo = "Anexos_" & Usuario.ToString() & "_" & String.Format("{0:yyyyMMdd}", DateTime.Today) & String.Format("{0:HHMMss}", DateTime.Now) & ".pdf"
                    destinoPdf = folderActual & nombreNuevoArchivo

                    Dim sourceFiles() As String = Anexos.Substring(1).Split("&")
                    Dim f As Integer = 0
                    Dim reader As PdfReader = New PdfReader(sourceFiles(f))
                    Dim n As Integer = reader.NumberOfPages
                    Dim document As Document = New Document(reader.GetPageSizeWithRotation(1))
                    Dim writer As PdfWriter = PdfWriter.GetInstance(document, New FileStream(destinoPdf, FileMode.Create))

                    document.Open()

                    Dim cb As PdfContentByte = writer.DirectContent
                    Dim page As PdfImportedPage
                    Dim rotation As Integer

                    While (f < sourceFiles.Length)
                        Dim i As Integer = 0
                        While (i < n)
                            i += 1
                            document.SetPageSize(reader.GetPageSizeWithRotation(i))
                            document.NewPage()
                            page = writer.GetImportedPage(reader, i)
                            rotation = reader.GetPageRotation(i)
                            If rotation = 90 Or rotation = 270 Then
                                cb.AddTemplate(page, 0, -1.0F, 1.0F, 0, 0, reader.GetPageSizeWithRotation(i).Height)
                            Else
                                cb.AddTemplate(page, 1.0F, 0, 0, 1.0F, 0, 0)
                            End If
                        End While
                        f += 1
                        If f < sourceFiles.Length Then
                            reader = New PdfReader(sourceFiles(f))
                            n = reader.NumberOfPages
                        End If
                    End While

                    document.Close()

                    For Each savedDoc As String In Anexos.Split(New Char() {"&"})
                        If File.Exists(savedDoc) Then
                            File.Delete(savedDoc)
                        End If
                    Next

                Catch ex As Exception
                    UIUtility.PublicarEvento("Anexos Todos - ex.Message = " & ex.Message & " ++ ex.StackTrace = " & ex.StackTrace)
                    AlertaJS(ex.Message.ToString())
                    For Each savedDoc As String In pRutas.Split(New Char() {"&"})
                        If File.Exists(savedDoc) Then
                            File.Delete(savedDoc)
                        End If
                    Next
                End Try

                Dim strFl As New System.IO.FileStream(destinoPdf, FileMode.Open)
                Dim bytes() As Byte = New Byte(strFl.Length) {}

                strFl.Read(bytes, 0, strFl.Length)
                strFl.Flush()
                strFl.Close()
                Response.Clear()
                Response.Buffer = True

                Response.ContentType = "application/pdf"
                Response.AddHeader("Content-Disposition", "inline;filename=" + nombreNuevoArchivo)

                Response.BinaryWrite(bytes)
                Response.Flush()
                Response.End()

        End Select
        Catch ex As Exception
            EjecutarJS("alert(" + ex.Message + ");")
        End Try
    End Sub
    Protected Sub Modulos_Gestion_Reportes_frmVisorReportesCartera_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class