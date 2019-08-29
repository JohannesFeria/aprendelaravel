Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class CaracteristicaGrupoDAM
    Private sqlCommand As String = ""

    Public Sub New()

    End Sub

    Public Function Listar(ByVal eCaracteristicaGrupo As CaracteristicaGrupoBE) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CaracteristicaGrupo_Buscar")
        db.AddInParameter(dbCommand, "@P_CodigoCaracteristica", DbType.String, eCaracteristicaGrupo.CodigoCaracteristica)
        db.AddInParameter(dbCommand, "@P_Descripcion", DbType.String, eCaracteristicaGrupo.Descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, eCaracteristicaGrupo.Situacion)

        Dim objeto As New DataTable
        Try
            objeto = db.ExecuteDataSet(dbCommand).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

        Return objeto
    End Function

    Public Function Seleccionar(ByVal eCaracteristicaGrupo As CaracteristicaGrupoBE) As CaracteristicaGrupoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CaracteristicaGrupo_Seleccionar")
        db.AddInParameter(dbCommand, "@P_CodigoCaracteristica", DbType.String, eCaracteristicaGrupo.CodigoCaracteristica)

        Try

            'Using resource As New db.ExecuteReader(dbCommand)

            'End Using
            Return Nothing
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ListarVistaCodigoCaracteristica(ByVal CodigoCaracteristica As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("SP_SIT_ListaVistaCaracteristica")
        db.AddInParameter(dbCommand, "@p_CodigoCaracteristica", DbType.String, CodigoCaracteristica)
        Dim objeto As New DataTable
        Try
            objeto = db.ExecuteDataSet(dbCommand).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

        Return objeto
    End Function
End Class
