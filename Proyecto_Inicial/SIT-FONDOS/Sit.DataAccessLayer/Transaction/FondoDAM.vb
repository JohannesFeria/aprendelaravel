Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class FondoDAM
    Public Function ListarPrecierreFondos() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Using DbCommand As DbCommand = db.GetStoredProcCommand("INGF_LIS_PRECIERRE_FONDOS")
                Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                    Return ds
                End Using
            End Using
    End Function
End Class
