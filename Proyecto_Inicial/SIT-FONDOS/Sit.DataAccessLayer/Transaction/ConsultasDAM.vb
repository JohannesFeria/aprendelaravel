Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class ConsultasDAM
    Public Function EjecutarDataSet(ByVal query As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Return db.ExecuteDataSet(CommandType.Text, query).Tables(0)
    End Function

    Public Function EjecutarNonQuery(ByVal query As String) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Return db.ExecuteNonQuery(CommandType.Text, query)
    End Function
End Class
