'Creado por: HDG OT 62087 Nro3-R09 20110110
Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class IndicadorBenchmarkDAM
    Private oIndicadorBenchmarkBE As IndicadorBenchmarkBE.IndicadorBenchmarkRow

    Public Sub Actualizar(ByVal objIndicadorBenchmark As IndicadorBenchmarkBE.IndicadorBenchmarkRow, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_upd_IndicadoresBenchmark")
        oIndicadorBenchmarkBE = CType(objIndicadorBenchmark, IndicadorBenchmarkBE.IndicadorBenchmarkRow)

        db.AddInParameter(dbCommand, "@p_CodigoIndicadorRent", DbType.String, oIndicadorBenchmarkBE.CodigoIndicadorRent)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oIndicadorBenchmarkBE.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_PorcInverObj", DbType.Decimal, oIndicadorBenchmarkBE.PorcInverObj)
        db.AddInParameter(dbCommand, "@p_PorcInverMax", DbType.Decimal, oIndicadorBenchmarkBE.PorcInverMax)
        db.AddInParameter(dbCommand, "@p_PorcInverMin", DbType.Decimal, oIndicadorBenchmarkBE.PorcInverMin)
        db.AddInParameter(dbCommand, "@p_CodigoSubGrupo", DbType.String, oIndicadorBenchmarkBE.CodigoSubGrupo)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)

    End Sub
End Class
