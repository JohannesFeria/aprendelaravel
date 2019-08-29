Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Data.SqlClient
Imports Sit.BusinessEntities

Public Class GrupoPortafolioDAM

    Private sqlCommand As String = ""
    Private oEntidadRow As EntidadBE.EntidadRow
    Private oEntidadExcesosRow As EntidadExcesosBE.EntidadExcesosRow    'HDG OT 60022 20100709
    Private strVacio As String = ""
    Private decVacio As String = 0
    Public Sub New()

    End Sub


    Public Function Listar(ByVal eGrupoPortafolio As GrupoPortafolioBE) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_Listar")
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, eGrupoPortafolio.Situacion)

        Dim objeto As New DataTable

        Try
            objeto = db.ExecuteDataSet(dbCommand).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

        Return objeto
    End Function

    Public Function CodigoPortafolioListar() As DataTable
        Try
            Dim bd As Database = DatabaseFactory.CreateDatabase()
            Dim dbcomand As DbCommand = bd.GetStoredProcCommand("GrupoPortafolio_listarCodigoGrupoPortafolio")
            Return bd.ExecuteDataSet(dbcomand).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class

