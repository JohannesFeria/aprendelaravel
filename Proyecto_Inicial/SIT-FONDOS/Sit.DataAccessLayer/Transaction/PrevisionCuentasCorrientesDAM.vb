Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class PrevisionCuentasCorrientesDAM

    Public Shared Function ObtenerBanco(ByVal IdEntidad As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As Common.DbCommand = db.GetStoredProcCommand("prov.SP_PROV_SEL_ObtenerBanco")
        db.AddInParameter(dbCommand, "@IdEntidad", DbType.String, IdEntidad)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Shared Function ObtenerBanco_x_IdBanco(ByVal IdEntidad As String, ByVal IdMoneda As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As Common.DbCommand = db.GetStoredProcCommand("prov.SP_PROV_SEL_ObtenerBanco_x_TipoMoneda")
        db.AddInParameter(dbCommand, "@IdEntidad", DbType.String, IdEntidad)
        db.AddInParameter(dbCommand, "@p_IdMoneda", DbType.String, IdMoneda)
        Return db.ExecuteDataSet(dbCommand)
    End Function
End Class
