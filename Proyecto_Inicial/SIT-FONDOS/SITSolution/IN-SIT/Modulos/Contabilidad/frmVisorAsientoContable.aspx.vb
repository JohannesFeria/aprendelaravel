Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Partial Class Modulos_Contabilidad_frmVisorAsientoContable
    Inherits BasePage
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim vFondo, vdescripcionFondo, vFecha, vMoneda, usuario, nombre As String
            Dim decFechaAsiento As Decimal
            Dim StrEstadoPortafolio As String
            vFondo = Request.QueryString("pFondo")
            vdescripcionFondo = Request.QueryString("pDescripcionFondo")
            vFecha = Request.QueryString("pFecha")
            vMoneda = "NSOL"
            decFechaAsiento = UIUtility.ConvertirFechaaDecimal(vFecha)
            If Not UIUtility.ValidarPortafolioAperturado(vFondo) Then
                StrEstadoPortafolio = "Cerrado"
            Else
                StrEstadoPortafolio = "Abierto"
            End If
            nombre = "Usuario"
            Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
            usuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & nombre & "'")(0)(1), String)
            Dim dtConsulta As New DataTable
            Dim dsconsulta As New DsRptAsientosContablesTipoT
            Dim drconsulta As DataRow            
            oReport.Load(Server.MapPath("RptAsientoContable.rpt"))
            Dim oAsientoContableBM As New AsientoContableBM
            dtConsulta = oAsientoContableBM.AsientoContable_SeleccionarPorFiltroRevisionCabecera(decFechaAsiento, vFondo, vMoneda, Me.DatosRequest).Tables(0)
            For Each drv As DataRow In dtConsulta.Rows
                drconsulta = dsconsulta.Tables(0).NewRow()
                drconsulta("Asiento") = drv("Asiento")
                drconsulta("DetalleAsiento") = drv("DetalleAsiento")
                drconsulta("DescripcionCuenta") = drv("DescripcionCuenta")
                drconsulta("TipoImporte") = drv("TipoImporte")
                drconsulta("DescripcionCentroCosto") = drv("DescripcionCentroCosto")
                drconsulta("TipoContable") = drv("TipoContable")
                drconsulta("Divisa") = drv("Divisa")
                drconsulta("MontoDebe") = drv("MontoDebe")
                drconsulta("MontoHaber") = drv("MontoHaber")
                drconsulta("Referencia") = drv("Referencia")
                drconsulta("Transaccion") = drv("Transaccion")
                drconsulta("OperacionContable") = drv("OperacionContable")
                dsconsulta.Tables(0).Rows.Add(drconsulta)
            Next
            dsconsulta.Merge(dsconsulta, False, System.Data.MissingSchemaAction.Ignore)
            oReport.SetDataSource(dsconsulta)
            oReport.SetParameterValue("@Usuario", usuario)
            oReport.SetParameterValue("@Fondo", vdescripcionFondo)
            oReport.SetParameterValue("@Fecha", vFecha)
            oReport.SetParameterValue("@EstadoPeriodo", StrEstadoPortafolio)
            oReport.SetParameterValue("Ruta_Logo", Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO")))
            Me.crAsientoContable.ReportSource = oReport
        Catch ex As Exception
            AlertaJS("Ocurrió un error al imprimir el reporte")
        End Try
    End Sub
    Protected Sub Modulos_Contabilidad_frmVisorAsientoContable_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class