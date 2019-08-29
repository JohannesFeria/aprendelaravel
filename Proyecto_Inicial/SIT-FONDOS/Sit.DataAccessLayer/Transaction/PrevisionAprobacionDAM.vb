Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data.SqlClient
Imports System.Data.Common

Public Class PrevisionAprobacionDAM

    Public Shared Function ListarPagosAprobar(ByVal Fecha As Decimal, ByVal Usuario As String, ByVal idTipoOperacion As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As Common.DbCommand = db.GetStoredProcCommand("prov.SP_PROV_SEL_ListarPagosAprobar")
        db.AddInParameter(dbCommand, "@Fecha", DbType.Decimal, Fecha)
        db.AddInParameter(dbCommand, "@Usuario", DbType.String, Usuario)
        db.AddInParameter(dbCommand, "@TipoOperacion", DbType.String, idTipoOperacion)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Shared Function ActualizarAprobacion(ByVal CodigoPago As String, ByVal UsuarioAprobacion As String, ByVal Estado As String) As Boolean
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("prov.SP_PROV_UPD_ActualizarPagoAprobacion")
            db.AddInParameter(dbCommand, "@CodigoPago", DbType.String, CodigoPago)
            db.AddInParameter(dbCommand, "@UsuarioAprobacion", DbType.String, UsuarioAprobacion)
            db.AddInParameter(dbCommand, "@Estado", DbType.String, Estado)
            db.ExecuteNonQuery(dbCommand)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
