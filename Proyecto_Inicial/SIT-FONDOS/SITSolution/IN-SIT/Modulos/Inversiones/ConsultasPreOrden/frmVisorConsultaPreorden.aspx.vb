Imports System.Data
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Partial Class Modulos_Inversiones_ConsultasPreOrden_frmVisorConsultaPreorden
    Inherits BasePage
    Dim Cro As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Request.QueryString("pIndica").ToString = "1" Then
            Dim vportafolio, vcodigooperacion, vmnemonico, visin, vsbs, vestado As String
            Dim vfechaasignacion As Decimal
            vportafolio = Request.QueryString("pportafolio")
            vcodigooperacion = Request.QueryString("pcodigooperacion")
            vmnemonico = Request.QueryString("pmnemonico")
            visin = Request.QueryString("pisin")
            vsbs = Request.QueryString("psbs")
            vfechaasignacion = Convert.ToDecimal(Request.QueryString("pfechaasignacion"))
            vestado = Request.QueryString("pestado")
            Dim dtconsultapreorden As New DataTable
            Dim dsconsulta As New dsconsultaPreorden
            Dim drconsulta As DataRow
            Dim objOrdeninversionBM As New OrdenPreOrdenInversionBM
            dtconsultapreorden = objOrdeninversionBM.ConsultaHistoricaPreOrdenes(vportafolio, vfechaasignacion, vcodigooperacion, vmnemonico, visin, vsbs, vestado, DatosRequest)
            For Each drv As DataRow In dtconsultapreorden.Rows
                drconsulta = dsconsulta.Tables(0).NewRow()
                drconsulta("Fecha") = drv("FechaOperacion")
                drconsulta("Hora") = drv("HoraOperacion")
                drconsulta("Portafolio") = drv("CodigoPortafolioSBS")
                drconsulta("Moneda") = drv("Moneda")
                drconsulta("Mnemonico") = drv("CodigoMnemonico")
                drconsulta("tipoInstrumento") = drv("TipoInstrumento")
                drconsulta("InteresCorrido") = drv("InteresCorridoNegociacion")
                drconsulta("Precio") = drv("Precio")
                drconsulta("Monto") = drv("MontoNetoOperacion")
                drconsulta("Intermediario") = drv("Intermediario")
                drconsulta("estado") = drv("Estado")
                dsconsulta.Tables(0).Rows.Add(drconsulta)
            Next
            Dim oStream As New System.IO.MemoryStream
            Dim Archivo As String = System.AppDomain.CurrentDomain.BaseDirectory() & "Modulos/Inversiones/ConsultasPreOrden/RptConsultaHistoricaPreOrden.rpt"
            Dim StrNombre As String = "Usuario"
            Dim strusuario As String
            Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
            strusuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & StrNombre & "'")(0)(1), String)
            Cro.Load(Archivo)
            Cro.SetDataSource(dsconsulta)
            Cro.SetParameterValue("@Usuario", strusuario)
            Cro.SetParameterValue("@FechaAsignacion", UIUtility.ConvertirFechaaString(vfechaasignacion))
            Cro.SetParameterValue("@RutaImagen", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
            crvisor.ReportSource = Cro
        Else
            Dim vcorrelativo, vcodigooperacion, vcodigotipoinstrumento, vmnemonico, visin, vsbs, vtipoRenta, vPortafolio As String
            Dim vfechainicio, vfechafin As Decimal
            vcorrelativo = Request.QueryString("pcorrelativo")
            vcodigooperacion = Request.QueryString("pcodigooperacion")
            vcodigotipoinstrumento = Request.QueryString("pcodigotipoinstrumento")
            vmnemonico = Request.QueryString("pmnemonico")
            visin = Request.QueryString("pisin")
            vsbs = Request.QueryString("psbs")
            vfechainicio = Convert.ToDecimal(Request.QueryString("pfechainicio"))
            vfechafin = Convert.ToDecimal(Request.QueryString("pfechafin"))
            vtipoRenta = Request.QueryString("pTipoRenta")
            vPortafolio = Request.QueryString("pPortafolio")
            Dim dtconsultapreorden As New DataTable
            dtconsultapreorden = CType(Session("TablaConsulta"), DataTable)
            Dim dsconsulta As New DsConsultaPOrden
            Dim drconsulta As DataRow
            Dim objOrdeninversionBM As New OrdenPreOrdenInversionBM
            dtconsultapreorden = objOrdeninversionBM.ConsultaOrdenesPreOrdenes(vfechainicio, vfechafin, vcodigooperacion, vcodigotipoinstrumento, vmnemonico, visin, vsbs, vtipoRenta, vPortafolio, "", DatosRequest).Tables(0)
            For Each drv As DataRow In dtconsultapreorden.Rows
                drconsulta = dsconsulta.Tables(0).NewRow()
                drconsulta("Fecha") = UIUtility.ConvertirStringaFecha(drv("FechaOperacion"))
                drconsulta("Hora") = drv("HoraOperacion")
                drconsulta("Portafolio") = drv("CodigoPortafolioSBS")
                drconsulta("Moneda") = drv("Moneda")
                drconsulta("Mnemonico") = drv("CodigoMnemonico")
                drconsulta("tipoInstrumento") = drv("DescOperacion") + "/" + drv("TipoInstrumento")
                drconsulta("InteresCorrido") = drv("InteresCorridoNegociacion")
                drconsulta("Precio") = drv("Precio")
                drconsulta("Monto") = drv("MontoNetoOperacion")
                drconsulta("Intermediario") = drv("Intermediario")
                drconsulta("estado") = drv("Estado")
                drconsulta("Cantidad") = drv("CantidadOperacion")
                drconsulta("TipoCambio") = drv("TipoCambio")
                drconsulta("FechaLiquidacion") = drv("FechaLiquidacion")
                drconsulta("FechaContrato") = drv("FechaContrato")
                drconsulta("Tasa") = drv("Tasa")
                drconsulta("MontoNominal") = drv("MontoNominal")
                drconsulta("CodigoOrden") = drv("CodigoOrden")
                dsconsulta.Tables(0).Rows.Add(drconsulta)
            Next
            Dim oStream As New System.IO.MemoryStream            
            Dim Archivo As String = System.AppDomain.CurrentDomain.BaseDirectory() & "Modulos/Inversiones/ConsultasPreOrden/RptConsultaPreOrden.rpt"
            Try
                Dim StrNombre As String = "Usuario"
                Dim strusuario, strcorrelativo As String
                If vcorrelativo = "OI" Then
                    strcorrelativo = "Ordenes de Inversion"
                Else
                    strcorrelativo = "PreOrdenes de Inversion"
                End If
                Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
                strusuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & StrNombre & "'")(0)(1), String)
                Cro.Load(Archivo)
                Cro.SetDataSource(dsconsulta)
                Cro.SetParameterValue("@FechaInicio", UIUtility.ConvertirFechaaString(vfechainicio))
                Cro.SetParameterValue("@Usuario", strusuario)
                Cro.SetParameterValue("@correlativo", strcorrelativo)
                Cro.SetParameterValue("@RutaImagen", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
                Me.crvisor.ReportSource = Cro
            Catch ex As Exception
            End Try
        End If
    End Sub
    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        EjecutarJS("<script language=javascript>window.close();</script>", False)
    End Sub
    Protected Sub Modulos_Inversiones_ConsultasPreOrden_frmVisorConsultaPreorden_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Cro.Close()
        Cro.Dispose()
    End Sub
End Class