Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class PlazaDAM
    Private sqlCommand As String = ""
    Private oPlazaRow As PlazaBE.PlazaRow


    Public Sub New()
    End Sub

    Public Function Listar(ByVal dataRequest As DataSet) As PlazaBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Plaza_Listar")
        Dim objeto As New PlazaBE
        db.LoadDataSet(dbCommand, objeto, "Plaza")
        Return objeto
    End Function
    'CMB OT 64034 20111202
    Public Function ListarxOrden() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_SeleccionarxOrden_Plaza")
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function ListarDataset(ByVal dataRequest As DataSet) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Plaza_Listar")
        Dim objeto As New DataSet
        db.LoadDataSet(dbCommand, objeto, "Plaza")
        Return objeto.Tables(0)
    End Function



End Class
