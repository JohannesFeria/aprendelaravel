Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Partial Class Modulos_Contabilidad_frmVisorMatrizContable
    Inherits BasePage
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim vCodigo, vMatriz, usuario, nombre, vCodigoMoneda, vCodigoClaseInstrumento, vCodigoOperacion, vCodigoTipoInstrumento, vCodigoModalidadPago, vCodigoSectorEmpresarial As String
            vCodigo = Request.QueryString("pFondo")
            vMatriz = Request.QueryString("pMatriz")
            vCodigoMoneda = Request.QueryString("pCodigoMoneda")
            vCodigoClaseInstrumento = Request.QueryString("pCodigoClaseInstrumento")
            vCodigoOperacion = Request.QueryString("pCodigoOperacion")
            vCodigoTipoInstrumento = Request.QueryString("pCodigoTipoInstrumento")
            vCodigoModalidadPago = Request.QueryString("pCodigoModalidadPago")
            vCodigoSectorEmpresarial = Request.QueryString("pCodigoSectorEmpresarial")
            nombre = "Usuario"
            Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
            usuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & nombre & "'")(0)(1), String)
            Dim dtConsulta As New DataTable
            Dim dsconsulta As New DsMatrizContable
            Dim drconsulta As DataRow            
            oReport.Load(Server.MapPath("RptMatrizContable.rpt"))
            Dim oDetalleMatrizContableBM As New DetalleMatrizContableBM
            dtConsulta = oDetalleMatrizContableBM.SeleccionarReporteMatrizContable(vCodigo, vMatriz, vCodigoMoneda, vCodigoClaseInstrumento, vCodigoOperacion, vCodigoTipoInstrumento, vCodigoModalidadPago, vCodigoSectorEmpresarial, DatosRequest).Tables(0)
            For Each drv As DataRow In dtConsulta.Rows
                drconsulta = dsconsulta.Tables(0).NewRow()
                drconsulta("CodigoMatrizContable") = drv("CodigoMatrizContable")
                drconsulta("MatrizContable") = drv("MatrizContable")
                drconsulta("ClaseInstrumento") = drv("ClaseInstrumento")
                drconsulta("Operacion") = drv("Operacion")
                drconsulta("TipoInstrumento") = drv("TipoInstrumento")
                drconsulta("ModalidadPago") = drv("ModalidadPago")
                drconsulta("SectorEmpresarial") = drv("SectorEmpresarial")
                drconsulta("CodigoMoneda") = drv("CodigoMoneda")
                drconsulta("Fondo") = drv("Fondo")
                drconsulta("CodigoCabeceraMatriz") = drv("CodigoCabeceraMatriz")
                drconsulta("NumeroCuentaContable") = drv("NumeroCuentaContable")
                drconsulta("DebeHaber") = drv("DebeHaber")
                drconsulta("TipoContabilidad") = drv("TipoContabilidad")
                drconsulta("Glosa") = drv("Glosa")
                drconsulta("CodigoTercero") = drv("CodigoTercero")
                drconsulta("Tercero") = drv("Tercero")
                drconsulta("Aplicar") = drv("Aplicar")
                drconsulta("CodigoCentroCosto") = drv("CodigoCentroCosto")
                drconsulta("CentroCosto") = drv("CentroCosto")
                drconsulta("SituacionCabecera") = drv("SituacionCabecera")
                drconsulta("SituacionDetalle") = drv("SituacionDetalle")
                drconsulta("Secuencia") = drv("Secuencia")
                drconsulta("CodigoOperacion") = drv("CodigoOperacion")
                drconsulta("CodigoSectorEmpresarial") = drv("CodigoSectorEmpresarial")
                drconsulta("CodigoTipoInstrumento") = drv("CodigoTipoInstrumento")
                dsconsulta.Tables(0).Rows.Add(drconsulta)
            Next
            dsconsulta.Merge(dsconsulta, False, System.Data.MissingSchemaAction.Ignore)
            oReport.SetDataSource(dsconsulta)
            oReport.SetParameterValue("@Usuario", usuario)
            oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
            Me.crMatrizContable.ReportSource = oReport
        Catch ex As Exception
            AlertaJS("Ocurrió un error al imprimir")
        End Try
    End Sub
    Protected Sub Modulos_Contabilidad_frmVisorMatrizContable_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class