Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports Sit.BusinessEntities


Public Class GenerarAccessDAM

    Public Sub New()

    End Sub

    Public USUARIO_JOB As String = System.Configuration.ConfigurationSettings.AppSettings.Item("USUARIO_JOB")
    Public NombreServidor As String = System.Configuration.ConfigurationSettings.AppSettings.Item("SERVIDOR")


    Public Function EjecutarDTSAccess(ByVal Paquete As String, ByVal Servidor As String, ByVal RutaArchivo As String, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_EjecutarDTS_ACCESS")

        Dim strGenerarAccess As String

        db.AddInParameter(dbCommand, "@pi_vc_Paquete", DbType.String, Paquete)
        db.AddInParameter(dbCommand, "@pi_vc_Servidor", DbType.String, NombreServidor)
        db.AddInParameter(dbCommand, "@pi_vc_RutaArchivo", DbType.String, RutaArchivo)
        db.AddInParameter(dbCommand, "@pi_vc_Propietariobd", DbType.String, USUARIO_JOB)

        strGenerarAccess = db.ExecuteScalar(dbCommand)
        Return strGenerarAccess

    End Function

    Public Function InsertarControl(ByVal RutaArchivo As String, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("InsertarControlDTS")

        Dim strGenerarAccess As String

        db.AddInParameter(dbCommand, "@pi_vc_RutaArchivo", DbType.String, RutaArchivo)

        strGenerarAccess = db.ExecuteScalar(dbCommand)
        Return strGenerarAccess

    End Function

    Public Function ListarControl(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ListaControlDTS")

        Return db.ExecuteDataSet(dbCommand)

    End Function
End Class

