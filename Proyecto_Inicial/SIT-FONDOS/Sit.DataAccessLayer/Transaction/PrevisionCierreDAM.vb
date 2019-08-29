Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class PrevisionCierreDAM

    Public Shared Function ObtenerPrevisionCierre() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("prov.SP_PROV_SEL_ObtenerPrevisionCierre")
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Shared Function ActualizarPrevisionCierre(ByVal HoraCierre1 As String, ByVal TipoCierre1 As String, ByVal HoraCierre2 As String, ByVal TipoCierre2 As String, ByVal UsuarioModificacion As String) As Boolean
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("prov.SP_PROV_UPD_ActualizarPrevisionCierre")
            db.AddInParameter(dbCommand, "@HoraCierre1", DbType.String, HoraCierre1)
            db.AddInParameter(dbCommand, "@TipoCierre1", DbType.String, TipoCierre1)
            db.AddInParameter(dbCommand, "@HoraCierre2", DbType.String, HoraCierre2)
            db.AddInParameter(dbCommand, "@TipoCierre2", DbType.String, TipoCierre2)
            db.AddInParameter(dbCommand, "@UsuarioModificacion", DbType.String, UsuarioModificacion)
            db.ExecuteNonQuery(dbCommand)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
