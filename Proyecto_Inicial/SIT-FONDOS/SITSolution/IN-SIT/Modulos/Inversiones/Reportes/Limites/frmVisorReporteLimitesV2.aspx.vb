Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports ParametrosSIT
Partial Class Modulos_Inversiones_Reportes_Limites_frmVisorReporteLimitesV2
    Inherits BasePage
    Dim oReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
#Region "Variables"
    Dim oLimiteBM As New LimiteBM
    Public _FolderReportes As String = ""
    Private _CodigoValorBase As String
    Public _PorcentajeCercaLimite As Decimal
#End Region
#Region "Eventos de Pagina"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim visor As New ReporteLimites
            visor._CodLimite = Session("pCodLimite")
            visor._CodLimiteCaracteristica = Session("pCodLimiteCaracteristica")
            visor._FechaOperacion = Session("pFechaOperacion")
            visor._DetalladoPorFondo = Session("pDetalladoPorFondo")
            visor._Escenario = Session("pEscenario")
            visor._ProcesarLimite = Session("pProcesarLimite")
            visor._FolderReportes = ""
            visor._Portafolio = Session("pPortafolio")
            visor._DescPortafolio = New PortafolioBM().Seleccionar(Session("pPortafolio"), Nothing).Tables(0)(0)("Descripcion")
            oReport = visor.GeneraLimite(DatosRequest, Me.Usuario)
            If oReport Is Nothing Then
                AlertaJS("No existe información para mostrar.", "window.close();")
            Else
                CrystalReportViewer1.Visible = True
                CrystalReportViewer1.ReportSource = oReport
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message.ToString(), "'", ""))
        End Try
    End Sub
#End Region
    Protected Sub Modulos_Inversiones_Reportes_Limites_frmVisorReporteLimitesV2_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If Not oReport Is Nothing Then
            oReport.Close()
            oReport.Dispose()
        End If
    End Sub
End Class