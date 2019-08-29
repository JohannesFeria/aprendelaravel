Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class MesDAM
    Private sqlCommand As String = ""
    Private oRow As MesBE.MesRow
    Public Sub New()

    End Sub
#Region " /* Funciones Seleccionar */ "

    Public Function Listar(ByVal dataRequest As DataSet) As MesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Mes_Listar")

        Dim oMesBE As New MesBE
        db.LoadDataSet(dbCommand, oMesBE, "Mes")
        Return oMesBE

    End Function
#End Region
End Class
