Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class ReportesLimitesDAM
    Public Sub New()
    End Sub
    Public Function SeleccionarPorEmisionAccionesFloat(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Inversiones_Reportes_Limites_PorEmisionAccionesFloat")
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, param1)
        Dim oReporte As New ReportesLimitesBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteLimites")
        Return oReporte
    End Function
    Public Function SeleccionarEnElExterior(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Inversiones_Reportes_Limites_EnElExteriorGeneral")
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, param1)
        Dim oReporte As New ReportesLimitesBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteLimites")
        Return oReporte
    End Function
    Public Function SeleccionarEnElExteriorDetallado(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Inversiones_Reportes_Limites_EnElExteriorDetallado")
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, param1)
        Dim oReporte As New ReportesLimitesBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteLimites")
        Return oReporte
    End Function
    Public Function SeleccionarPorEmisionAccionesFactorLiquidez(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Inversiones_Reportes_Limites_PorEmisionAccionesFactorLiquidez")
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, param1)
        Dim oReporte As New ReportesLimitesBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteLimites")
        Return oReporte
    End Function
    Public Function SeleccionarPorEmisorDeudaFactorRiesgo(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Inversiones_Reportes_Limites_PorEmisorDeudaFactorRiesgo")
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, param1)
        Dim oReporte As New ReportesLimitesBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteLimites")
        Return oReporte
    End Function
    Public Function SeleccionarValorContablePatrimonio(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Inversiones_Reportes_Limites_ValorContablePatrimonio")
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, param1)
        Dim oReporte As New ReportesLimitesBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteLimites")
        Return oReporte
    End Function
    Public Function SeleccionarPasivoValorContable(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Inversiones_Reportes_Limites_PasivoValorContable")
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, param1)
        Dim oReporte As New ReportesLimitesBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteLimites")
        Return oReporte
    End Function
    Public Function SeleccionarValorContableActivo(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Inversiones_Reportes_Limites_ValorContableActivo")
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, param1)
        Dim oReporte As New ReportesLimitesBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteLimites")
        Return oReporte
    End Function
    Public Function SeleccionarTrading(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Inversiones_Reportes_Limites_Trading")
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, param1)
        Dim oReporte As New ReportesLimitesBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteLimites")
        Return oReporte
    End Function
    Public Function SeleccionarBVL(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Inversiones_Reportes_Limites_BVL")
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, param1)
        Dim oReporte As New ReportesLimitesBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteLimites")
        Return oReporte
    End Function
    Public Function SeleccionarPorEmisionSerieAcciones(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Inversiones_Reportes_Limites_PorEmisionSerieAcciones")
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, param1)
        Dim oReporte As New ReportesLimitesBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteLimites")
        Return oReporte
    End Function
    Public Function SeleccionarPorEmisorDeuda(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Inversiones_Reportes_Limites_PorEmisorDeuda")
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, param1)
        Dim oReporte As New ReportesLimitesBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteLimites")
        Return oReporte
    End Function
    Public Function SeleccionarPorEmisionSerieRentaFija(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Inversiones_Reportes_Limites_PorEmisionSerieRentaFija")
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, param1)
        Dim oReporte As New ReportesLimitesBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteLimites")
        Return oReporte
    End Function
    Public Function SeleccionarPorExtranjeroRespectoAlInstrumento(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Inversiones_Reportes_Limites_PorExtranjeroRespectoAlInstrumento")
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, param1)
        Dim oReporte As New ReportesLimitesBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteLimites")
        Return oReporte
    End Function
    Public Function SeleccionarActivosTitulizados(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Inversiones_Reportes_Limites_ActivosTitulizados")
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, param1)
        Dim oReporte As New ReportesLimitesBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteLimites")
        Return oReporte
    End Function
    Public Function SeleccionarEstructurados(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Inversiones_Reportes_Limites_Estructurados")
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, param1)
        Dim oReporte As New ReportesLimitesBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteLimites")
        Return oReporte
    End Function
    Public Function SeleccionarSociedadAdministradoraFondos(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Inversiones_Reportes_Limites_SociedadAdministradoraFondos")
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, param1)
        Dim oReporte As New ReportesLimitesBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteLimites")
        Return oReporte
    End Function
    Public Function SeleccionarInversiones(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Inversiones_Reportes_Limites_Inversion")
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, param1)
        Dim oReporte As New ReportesLimitesBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteLimites")
        Return oReporte
    End Function
    Public Function SeleccionarIndividuales(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Inversiones_Reportes_Limites_Individuales")
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, param1)
        Dim oReporte As New ReportesLimitesBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteLimites")
        Return oReporte
    End Function
    Public Function SeleccionarGlobales(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Inversiones_Reportes_Limites_Globales")
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, param1)
        Dim oReporte As New ReportesLimitesBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteLimites")
        Return oReporte
    End Function
    Public Function SeleccionarGrupoEconomico(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Inversiones_Reportes_Limites_GrupoEconomico")
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, param1)
        Dim oReporte As New ReportesLimitesBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteLimites")
        Return oReporte
    End Function
    Public Function SeleccionarPorEmisorAcciones(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Inversiones_Reportes_Limites_PorEmisorAcciones")
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, param1)
        Dim oReporte As New ReportesLimitesBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteLimites")
        Return oReporte
    End Function
    Public Function SeleccionarPorEmisor(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Inversiones_Reportes_Limites_PorEmisor")
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, param1)
        Dim oReporte As New ReportesLimitesBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteLimites")
        Return oReporte
    End Function
    Public Function SeleccionarPorInstrumentoGestor(ByVal param1 As Decimal, ByVal dataRequest As DataSet) As ReportesLimitesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Inversiones_Reportes_Limites_PorInstrumentoGestor")
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, param1)
        Dim oReporte As New ReportesLimitesBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteLimites")
        Return oReporte
    End Function
    Public Function Nivel4(ByVal codigoLimite As String, ByVal CodigoLimiteCaracteristica As String, _
    ByVal FechaLimite As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Limite_Reporte_Nivel4")
        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, codigoLimite)
        db.AddInParameter(dbCommand, "@p_CodigoLimiteCaracteristica", DbType.String, CodigoLimiteCaracteristica)
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, FechaLimite)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    '------------------ Reporte Limites -------------------
    Public Function Seleccionar_ReporteLimites(ByVal codLimite As String, ByVal codLimiteCaracteristica As String, _
        ByVal fecha As Decimal, ByVal procesar As Decimal, ByVal escenario As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Limite_Reporte")
        dbCommand.CommandTimeout = 600
        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, codLimite)
        db.AddInParameter(dbCommand, "@p_CodigoLimiteCaracteristica", DbType.String, codLimiteCaracteristica)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_procesar", DbType.Decimal, procesar)
        db.AddInParameter(dbCommand, "@p_Escenario", DbType.String, escenario) 'RGF 20081031
        Dim DstTabla As New DataSet
        DstTabla = db.ExecuteDataSet(dbCommand)
        Return DstTabla
    End Function
    Public Function Seleccionar_ReporteLimite_Trading_Diario(ByVal fechaLimite As Decimal, ByVal fondo As String, ByRef montoTotal3Fondos As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Limite_Reporte_Traiding_Diario")
        dbCommand.CommandTimeout = 300
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, fechaLimite)
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, fondo)
        db.AddOutParameter(dbCommand, "@p_MontoTotal3Fondos", DbType.Decimal, montoTotal3Fondos)
        Dim DstTabla As New DataSet
        DstTabla = db.ExecuteDataSet(dbCommand)
        montoTotal3Fondos = Convert.ToDecimal(db.GetParameterValue(dbCommand, "@p_MontoTotal3Fondos"))
        Return DstTabla
    End Function
    Public Function Seleccionar_ReporteLimite_Trading_Mensual(ByVal fechaLimite As Decimal, ByVal fondo As String, ByRef montoTotal3Fondos As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Limite_Reporte_Traiding_Mensual1")
        dbCommand.CommandTimeout = 300
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, fechaLimite)
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, fondo)
        db.AddOutParameter(dbCommand, "@p_MontoTotal3Fondos", DbType.Decimal, montoTotal3Fondos)
        Dim DstTabla As New DataSet
        DstTabla = db.ExecuteDataSet(dbCommand)
        montoTotal3Fondos = Convert.ToDecimal(db.GetParameterValue(dbCommand, "@p_MontoTotal3Fondos"))
        Return DstTabla
    End Function
    'RGF 20080711
    Public Function Seleccionar_ReporteLimite_BVL(ByVal fechaLimite As Decimal, ByVal fondo As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Limite_Reporte_BVL")
        dbCommand.CommandTimeout = 300
        db.AddInParameter(dbCommand, "@p_FechaLimite", DbType.Decimal, fechaLimite)
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, fondo)
        Dim DstTabla As New DataSet
        DstTabla = db.ExecuteDataSet(dbCommand)
        Return DstTabla
    End Function
    Public Function Obtener_LimiteExterior_SaldoBancos(ByVal fechaLimite As Decimal, ByVal escenario As String) As Decimal
        Dim saldoBancos As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("LimiteExterior_SaldoBancos")
        dbCommand.CommandTimeout = 300
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fechaLimite)
        db.AddInParameter(dbCommand, "@p_Escenario", DbType.String, escenario)   'HDG INC 59836	20100617
        db.AddOutParameter(dbCommand, "@p_saldo", DbType.Decimal, saldoBancos)
        db.ExecuteNonQuery(dbCommand)
        saldoBancos = Convert.ToDecimal(db.GetParameterValue(dbCommand, "@p_saldo"))
        Return saldoBancos
    End Function
    Public Function ValidarSaldoBancos(ByVal Fecha As Decimal, ByVal Portafolio As String, ByVal Banco As String, ByVal Escenario As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_Limite_ValidarSaldoBancos")
        dbCommand.CommandTimeout = 300
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, Fecha)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, Portafolio)
        db.AddInParameter(dbCommand, "@p_Banco", DbType.String, Banco)
        db.AddInParameter(dbCommand, "@p_Escenario", DbType.String, Escenario)
        If db.ExecuteScalar(dbCommand).ToString = 1 Then Return True
        Return False
    End Function
    Public Function ValidarGeneraTodoFondos(ByVal fecha As Decimal, ByVal Escenario As String, ByVal Portafolio As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_ValidarGenConsolidaLimites")
        db.AddInParameter(dbCommand, "@p_FechaReporte", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_Escenario", DbType.String, Escenario)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio) 'RGF 20101111
        Return db.ExecuteScalar(dbCommand)
    End Function
    Public Function ListaEmailUsuarioNotifica() As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ListaEmailUsuarioNotifica")
        dbCommand.CommandTimeout = 100
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
    Public Function Seleccionar_ReporteLimite_Moneda(ByVal codlimite As String, ByVal fechaLimite As Decimal, ByVal fondo As String, ByVal escenario As String, ByVal procesar As Integer) As DataSet   'HDG INC 62882	20110406
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_Limites_ProcesarDivisas")
        dbCommand.CommandTimeout = 300
        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, codlimite)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fechaLimite)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, fondo)
        db.AddInParameter(dbCommand, "@p_Escenario", DbType.String, escenario)
        db.AddInParameter(dbCommand, "@p_procesar", DbType.Decimal, procesar)    'HDG INC 62882	20110406
        Dim DstTabla As New DataSet
        DstTabla = db.ExecuteDataSet(dbCommand)
        Return DstTabla
    End Function
    Public Function Obtener_LimiteNivelMaximo(ByVal CodigoLimiteCaracteristica As String) As Decimal
        Dim nivelMaximo As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Limite_NivelMaximo")
        dbCommand.CommandTimeout = 300
        db.AddInParameter(dbCommand, "@p_CodigoLimiteCaracteristica", DbType.String, CodigoLimiteCaracteristica)
        db.AddOutParameter(dbCommand, "@p_NivelMaximo", DbType.Decimal, nivelMaximo)
        db.ExecuteNonQuery(dbCommand)
        nivelMaximo = Convert.ToDecimal(db.GetParameterValue(dbCommand, "@p_NivelMaximo"))
        Return nivelMaximo
    End Function
    Public Function Seleccionar_ConsolidadoLimitesExcedidos(ByVal fecha As Decimal, ByVal portafolio As String, ByVal escenario As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("LimiteExcesoConsolidado_Seleccionar")
        dbCommand.CommandTimeout = 600
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_Escenario", DbType.String, escenario) 'RGF 20090924
        Dim DstTabla As New DataSet
        DstTabla = db.ExecuteDataSet(dbCommand)
        Return DstTabla
    End Function
    Public Function SeleccionarLimitesPorPortafolio(ByVal portafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("LimitesPorPortafolio_Seleccionar")
        dbCommand.CommandTimeout = 300
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
        Dim DstTabla As New DataSet
        DstTabla = db.ExecuteDataSet(dbCommand)
        Return DstTabla
    End Function
    Public Function ObtenerUltimaFecha_ReporteLimite(ByVal limite As String, ByVal portafolio As String, ByVal fecha As Decimal, ByVal escenario As String, ByVal codmnemonico As String)
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ObtenerUltimaFecha_ReporteLimite")
        dbCommand.CommandTimeout = 300
        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, limite)
        db.AddInParameter(dbCommand, "@p_CodigoLimiteCaracteristica", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_Escenario", DbType.String, escenario)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, codmnemonico)
        Dim dstTabla As New DataSet
        dstTabla = db.ExecuteDataSet(dbCommand)
        Return dstTabla
    End Function
    Public Function SeleccionarLimitesPorForward(ByVal portafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_LimitesPorForward_Seleccionar")
        dbCommand.CommandTimeout = 300
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
        Dim dstTabla As New DataSet
        dstTabla = db.ExecuteDataSet(dbCommand)
        Return dstTabla
    End Function
    Public Function Seleccionar_ReporteLimitesPorInstrumento(ByVal portafolio As String, ByVal fecha As Decimal, ByVal escenario As String, ByVal valorNivel As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_LimitesPorInstrumento_Seleccionar")
        dbCommand.CommandTimeout = 300
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_Escenario", DbType.String, escenario)
        db.AddInParameter(dbCommand, "@p_ValorNivel", DbType.String, valorNivel)
        Dim dstTabla As New DataSet
        dstTabla = db.ExecuteDataSet(dbCommand)
        Return dstTabla
    End Function
    Public Function ValidarInstrumentosParametrizados(ByVal fecha As Decimal, ByVal escenario As String, ByVal portafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Limite_ValidarInstrumentos")
        db.AddInParameter(dbCommand, "@p_FechaValoracion", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_Escenario", DbType.String, escenario)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
        Dim dstTabla As New DataSet
        dstTabla = db.ExecuteDataSet(dbCommand)
        Return dstTabla
    End Function
    Public Function LimiteValoresInicializa(ByVal Portafolio As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_del_LimiteValoresInicializa")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio)
        Return db.ExecuteScalar(dbCommand)
    End Function
    Public Function TieneCalculoExistente(ByVal codLimite As String, ByVal codLimiteCaracteristica As String, ByVal fecha As Decimal, ByVal escenario As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Limite_BuscarCalculoExistente")
        dbCommand.CommandTimeout = 600 '(10min)
        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, codLimite)
        db.AddInParameter(dbCommand, "@p_CodigoLimiteCaracteristica", DbType.String, codLimiteCaracteristica)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_Escenario", DbType.String, escenario) 'RGF 20081031
        If db.ExecuteScalar(dbCommand).ToString = 1 Then Return True
        Return False
    End Function
    Public Function ProcesarLimite(ByVal codLimite As String, ByVal codLimiteCaracteristica As String, ByVal fecha As Decimal, ByVal escenario As String, ByVal procesar As Integer) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Limite_ProcesarLimite")
        dbCommand.CommandTimeout = 1620  'HDG 20110905
        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, codLimite)
        db.AddInParameter(dbCommand, "@p_CodigoLimiteCaracteristica", DbType.String, codLimiteCaracteristica)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_Escenario", DbType.String, escenario) 'RGF 20081031
        db.AddInParameter(dbCommand, "@p_procesar", DbType.Decimal, procesar)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
    Public Function TotalSaldoBancos(ByVal fecha As Decimal, ByVal portafolio As String, ByVal escenario As String) As Decimal ' ini OT 62839 20110408
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_LimiteExterior_TotalSaldoBancos")
        Dim saldoBancos As Decimal
        dbCommand.CommandTimeout = 900
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_Escenario", DbType.String, escenario)
        db.AddOutParameter(dbCommand, "@p_Saldo", DbType.Decimal, saldoBancos)
        db.ExecuteNonQuery(dbCommand)
        saldoBancos = Convert.ToDecimal(db.GetParameterValue(dbCommand, "@p_Saldo"))
        Return saldoBancos
    End Function
    Public Function TotalPatrimonioRentaFija(ByVal fecha As Decimal, ByVal portafolio As String) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_PatrimonioInstrumentosRentaFija")
        Dim totalPatrimonioRF As Decimal
        dbCommand.CommandTimeout = 900
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
        db.AddOutParameter(dbCommand, "@p_TotalInversionesRF", DbType.Decimal, totalPatrimonioRF)
        db.ExecuteNonQuery(dbCommand)
        totalPatrimonioRF = Convert.ToDecimal(db.GetParameterValue(dbCommand, "@p_TotalInversionesRF"))
        Return totalPatrimonioRF
    End Function
    Public Function ValidarValorizacion(ByVal fecha As Decimal, ByVal Portafolio As String, ByVal Escenario As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_ValidarValorizacion")
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio)
        db.AddInParameter(dbCommand, "@p_Escenario", DbType.String, Escenario)
        Return db.ExecuteScalar(dbCommand)
    End Function
    Public Function ObtenerReporteLimite_PorPortafolio(ByVal p_CodigoPortafolio As String, ByVal p_FechaDecimal As Integer, ByVal p_FechaString As String, ByVal p_Mandato As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Pr_Generar_ReporteLimites")
            dbCommand.CommandTimeout = 1800
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, p_CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Int32, p_FechaDecimal)
            db.AddInParameter(dbCommand, "@p_FechaCadena", DbType.String, p_FechaString)
            db.AddInParameter(dbCommand, "@p_ClienteMandato", DbType.String, p_Mandato)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
End Class