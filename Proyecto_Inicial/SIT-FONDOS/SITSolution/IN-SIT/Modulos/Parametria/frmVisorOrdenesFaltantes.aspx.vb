Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Partial Class Modulos_Parametria_frmVisorOrdenesFaltantes
    Inherits BasePage
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim vFondo, usuario, nombre, vFecha As String
            Dim tipoOperacion As String = ""
            vFondo = Request.QueryString("pFondo")
            vFecha = Request.QueryString("pFecha")
            If Not (Request.QueryString()("tipoOperacion") Is Nothing) Then
                tipoOperacion = Request.QueryString()("tipoOperacion")
            End If
            nombre = "Usuario"
            Dim columnName As String = DatosRequest.Tables(0).Columns(0).ColumnName
            usuario = CType(DatosRequest.Tables(0).Select(columnName & "='" & nombre & "'")(0)(1), String)
            Dim dtConsultaCxC As New DataTable
            Dim dtConsultaOI As New DataTable
            Dim dtConsultaPOI As New DataTable
            Dim dsConsultaOI As New DSOI1
            Dim dsConsultaCxC As New DsCXC
            Dim dsConsultaPOI As New DSPOI1
            Dim drconsultaOI As DataRow
            Dim drconsultaCXC As DataRow
            Dim drconsultaPOI As DataRow            
            oReport.Load(Server.MapPath("RptCierrePortafolio.rpt"))
            Dim oPortafolioBM As New PortafolioBM
            Dim dsOrdenesFaltantes As DataSet
            If tipoOperacion = "A" Then
                dsOrdenesFaltantes = oPortafolioBM.ListaOrdenesFaltantesApertura(vFondo, DatosRequest)
                dtConsultaCxC = dsOrdenesFaltantes.Tables(0)
                dtConsultaOI = dsOrdenesFaltantes.Tables(1)
                dtConsultaPOI = dsOrdenesFaltantes.Tables(2)
            Else
                If tipoOperacion = "VAL" Then
                    dtConsultaCxC = New DataTable
                    dsOrdenesFaltantes = oPortafolioBM.ListarOrdenesFaltantesValoracion(vFondo, UIUtility.ConvertirFechaaDecimal(vFecha))
                    dtConsultaOI = dsOrdenesFaltantes.Tables(0)
                    dtConsultaPOI = dsOrdenesFaltantes.Tables(1)
                ElseIf tipoOperacion = "VALEST" Then
                    dsOrdenesFaltantes = oPortafolioBM.ListarOrdenesFaltantesValoracionEstimada(vFondo, UIUtility.ConvertirFechaaDecimal(vFecha))
                    dtConsultaOI = dsOrdenesFaltantes.Tables(0)
                Else
                    dsOrdenesFaltantes = oPortafolioBM.ListaOrdenesFaltantes(vFondo, DatosRequest)
                    dtConsultaOI = dsOrdenesFaltantes.Tables(0)
                End If
            End If
            For Each drv As DataRow In dtConsultaOI.Rows
                drconsultaOI = dsConsultaOI.Tables("DSOI1").NewRow()
                drconsultaOI("FechaOperacion") = drv("FechaOperacion")
                drconsultaOI("CodigoOrden") = drv("CodigoOrden")
                drconsultaOI("CodigoSBS") = drv("CodigoSBS")
                drconsultaOI("CodigoISIN") = drv("CodigoISIN")
                drconsultaOI("CodigoMnemonico") = drv("CodigoMnemonico")
                drconsultaOI("CodigoMoneda") = drv("CodigoMoneda")
                drconsultaOI("Operacion") = drv("Operacion")
                drconsultaOI("Monto") = drv("Monto")
                dsConsultaOI.Tables("DSOI1").Rows.Add(drconsultaOI)
            Next
            For Each drv As DataRow In dtConsultaPOI.Rows
                drconsultaPOI = dsConsultaPOI.Tables("DSPOI1").NewRow()
                drconsultaPOI("FechaOperacion") = drv("FechaOperacion")
                drconsultaPOI("CodigoPreOrden") = drv("CodigoPreOrden")
                drconsultaPOI("CodigoSBS") = drv("CodigoSBS")
                drconsultaPOI("CodigoISIN") = drv("CodigoISIN")
                drconsultaPOI("CodigoMnemonico") = drv("CodigoMnemonico")
                drconsultaPOI("CodigoMoneda") = drv("CodigoMoneda")
                drconsultaPOI("Operacion") = drv("Operacion")
                drconsultaPOI("Monto") = drv("Monto")
                dsConsultaPOI.Tables("DSPOI1").Rows.Add(drconsultaPOI)
            Next
            For Each drv As DataRow In dtConsultaCxC.Rows
                drconsultaCXC = dsConsultaCxC.Tables("CxC").NewRow()
                drconsultaCXC("FechaOperacion") = drv("FechaOperacion")
                drconsultaCXC("FechaVencimiento") = drv("FechaVencimiento")
                drconsultaCXC("NumeroOperacion") = drv("NumeroOperacion")
                drconsultaCXC("Referencia") = drv("Referencia")
                drconsultaCXC("CodigoOrden") = drv("CodigoOrden")
                drconsultaCXC("NumeroCuenta") = drv("NumeroCuenta")
                drconsultaCXC("Operacion") = drv("Operacion")
                'RGF 20080715
                drconsultaCXC("CodigoMoneda") = drv("CodigoMoneda")
                drconsultaCXC("Mercado") = drv("Mercado")
                dsConsultaCxC.Tables("CxC").Rows.Add(drconsultaCXC)
            Next
            dsConsultaCxC.Merge(dsConsultaCxC, False, System.Data.MissingSchemaAction.Ignore)
            dsConsultaPOI.Merge(dsConsultaPOI, False, System.Data.MissingSchemaAction.Ignore)
            dsConsultaOI.Merge(dsConsultaOI, False, System.Data.MissingSchemaAction.Ignore)
            oReport.OpenSubreport("OI").SetDataSource(dsConsultaOI)
            oReport.OpenSubreport("POI").SetDataSource(dsConsultaPOI)
            oReport.OpenSubreport("CxC").SetDataSource(dsConsultaCxC)
            oReport.SetParameterValue("@Usuario", usuario)
            If tipoOperacion = "VAL" Then
                oReport.SetParameterValue("@Fondo", "Inconsistencias para la valoracion del Portafolio " & vFondo)
                oReport.SetParameterValue("@Fecha", "Fecha de Valoracion : " & vFecha)
            ElseIf tipoOperacion = "A" Then 'RGF 20080804
                oReport.SetParameterValue("@Fondo", "Inconsistencias para la Apertura del Portafolio " & vFondo)
                oReport.SetParameterValue("@Fecha", "Fecha de Apertura : " & vFecha)
            ElseIf tipoOperacion = "VALEST" Then 'LETV 20100113 ,Para la Valoracion Estimada debe de estar confirmado los DRL
                oReport.SetParameterValue("@Fondo", "Inconsistencias para la valoracion Estimada del Portafolio " & vFondo)
                oReport.SetParameterValue("@Fecha", "Fecha de Valoracion : " & vFecha)
            Else
                oReport.SetParameterValue("@Fondo", "Inconsistencias para el Cierre del Portafolio " & vFondo)
                oReport.SetParameterValue("@Fecha", "Fecha de Cierre : " & vFecha)
            End If
            Me.CrystalReportViewer1.ReportSource = oReport
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Public Sub InicializarValoresOI(ByRef oRow As DsOI.OIRow)
        oRow.CodigoOrden = ""
        oRow.FechaOperacion = ""
        oRow.CodigoSBS = ""
        oRow.CodigoISIN = ""
        oRow.CodigoMnemonico = ""
        oRow.Operacion = ""
        oRow.CodigoMoneda = ""
    End Sub
    Protected Sub Modulos_Parametria_frmVisorOrdenesFaltantes_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oReport.Close()
        oReport.Dispose()
    End Sub
End Class