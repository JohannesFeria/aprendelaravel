Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports Sit.BusinessEntities

Public Class ReporteVLMidasDAM

    Public Function Insertar(ByVal ReporteVLMidas As ReporteVLMidasBE, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("dbo.SP_SIT_INS_ReporteVLMidas")

        db.AddInParameter(dbCommand, "@p_TipoRegistro", DbType.String, ReporteVLMidas.TipoRegistro)
        db.AddInParameter(dbCommand, "@p_Administradora", DbType.String, ReporteVLMidas.Administradora)
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, ReporteVLMidas.Fondo)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, ReporteVLMidas.Fecha)
        db.AddInParameter(dbCommand, "@p_TipoCodigoValor", DbType.String, ReporteVLMidas.TipoCodigoValor)
        db.AddInParameter(dbCommand, "@p_CodigoValor", DbType.String, ReporteVLMidas.CodigoValor)
        db.AddInParameter(dbCommand, "@p_IdentificadorOperacion", DbType.String, ReporteVLMidas.IdentificadorOperacion)
        db.AddInParameter(dbCommand, "@p_FormaValorizacion", DbType.String, ReporteVLMidas.FormaValorizacion)
        db.AddInParameter(dbCommand, "@p_MontoNominal", DbType.Decimal, ReporteVLMidas.MontoNominal)
        db.AddInParameter(dbCommand, "@p_PrecioTasa", DbType.Decimal, ReporteVLMidas.PrecioTasa)
        db.AddInParameter(dbCommand, "@p_TipoCambio", DbType.Decimal, ReporteVLMidas.TipoCambio)
        db.AddInParameter(dbCommand, "@p_MontoFinal", DbType.Decimal, ReporteVLMidas.MontoFinal)
        db.AddInParameter(dbCommand, "@p_MontoInversion", DbType.Decimal, ReporteVLMidas.MontoInversion)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, IIf(ReporteVLMidas.FechaOperacion = 0, DBNull.Value, ReporteVLMidas.FechaOperacion))
        db.AddInParameter(dbCommand, "@p_FechaInicioPagaIntereses", DbType.Decimal, IIf(ReporteVLMidas.FechaInicioPagaIntereses = 0, DBNull.Value, ReporteVLMidas.FechaInicioPagaIntereses))
        db.AddInParameter(dbCommand, "@p_FechaVencimiento", DbType.Decimal, IIf(ReporteVLMidas.FechaVencimiento = 0, DBNull.Value, ReporteVLMidas.FechaVencimiento))
        db.AddInParameter(dbCommand, "@p_InteresesCorrido", DbType.Decimal, ReporteVLMidas.InteresesCorrido)
        db.AddInParameter(dbCommand, "@p_InteresesGanado", DbType.Decimal, ReporteVLMidas.InteresesGanado)
        db.AddInParameter(dbCommand, "@p_Ganancia_Perdida", DbType.Decimal, ReporteVLMidas.Ganancia_Perdida)
        db.AddInParameter(dbCommand, "@p_Valorizacion", DbType.Decimal, ReporteVLMidas.Valorizacion)
        db.AddInParameter(dbCommand, "@p_TipoInstrumento", DbType.String, ReporteVLMidas.TipoInstrumento)
        db.AddInParameter(dbCommand, "@p_Clasificacion", DbType.String, ReporteVLMidas.Clasificacion)
        db.AddInParameter(dbCommand, "@p_ComisionContado", DbType.Decimal, ReporteVLMidas.ComisionContado)
        db.AddInParameter(dbCommand, "@p_ComisionPlazo", DbType.Decimal, ReporteVLMidas.ComisionPlazo)
        db.AddInParameter(dbCommand, "@p_TIR", DbType.Decimal, ReporteVLMidas.TIR)
        db.AddInParameter(dbCommand, "@p_Duracion", DbType.Decimal, ReporteVLMidas.Duracion)
        db.ExecuteNonQuery(dbCommand)

        Return "True"
    End Function

    Public Function Elminar(ByVal ReporteVLMidas As ReporteVLMidasBE, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("dbo.SP_SIT_DEL_ReporteVLMidas")

        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, ReporteVLMidas.Fecha)
        db.ExecuteNonQuery(dbCommand)

        Return "True"
    End Function

    Public Function CompararVL(ByVal ReporteVLMidas As ReporteVLMidasBE) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("dbo.SP_SIT_SEL_CompararVL")

        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, ReporteVLMidas.Fecha)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function ValidarVL(ByVal ReporteVLMidas As ReporteVLMidasBE) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("dbo.SP_SIT_OBT_ValidarCargaVL")

        Dim Resultado As String = ""
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, ReporteVLMidas.Fecha)
        db.AddOutParameter(dbCommand, "@p_Resultado", DbType.String, 100)
        db.ExecuteNonQuery(dbCommand)

        Return db.GetParameterValue(dbCommand, "@p_Resultado")

    End Function
    
End Class
