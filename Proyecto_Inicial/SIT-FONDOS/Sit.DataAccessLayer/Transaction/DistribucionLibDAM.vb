Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

''' <summary>
''' Clase para el acceso de los datos para tabla DistribucionLib.
''' 'OT10927 - 21/11/2017 - Hanz Cocchi. Insertar distribución de cuotas liberadas
''' </summary>
Public Class DistribucionLibDAM

    Public Sub New()

    End Sub

    Public Function Insertar(ByVal oDis As DistribucionLibBE, ByVal dataRequest As DataSet) As Boolean
        Insertar = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("InsertarDistribucionLib")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oDis.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, oDis.FechaOperacion)
            db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, oDis.CodigoNemonico)
            db.AddInParameter(dbCommand, "@p_Serie", DbType.String, oDis.Serie)
            db.AddInParameter(dbCommand, "@p_Monto", DbType.Decimal, oDis.Monto)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            Insertar = True
        End Using
    End Function

    'OT10989 - 12/12/2017 - Hanz Cocchi. Eliminar rentabilidad distribucción
    Public Function Eliminar(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal) As Boolean
        Eliminar = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Eliminar_DistribucionLib")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
            db.ExecuteNonQuery(dbCommand)
            Eliminar = True
        End Using
    End Function

End Class
