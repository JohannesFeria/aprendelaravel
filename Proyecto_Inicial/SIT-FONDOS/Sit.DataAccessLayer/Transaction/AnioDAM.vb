Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class AnioDAM
    Private sqlCommand As String = ""
    Private oRow As AnhioBE.AnioRow
    Public Sub New()

    End Sub
#Region " /* Funciones Seleccionar */ "

    Public Function Listar(ByVal dataRequest As DataSet) As AnhioBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Anio_Listar")

        Dim oAnhioBE As New AnhioBE
        db.LoadDataSet(dbCommand, oAnhioBE, "Anio")
        Return oAnhioBE

    End Function
#End Region
End Class
