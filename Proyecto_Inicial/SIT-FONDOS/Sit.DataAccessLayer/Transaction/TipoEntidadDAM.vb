Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer

Public Class TipoEntidadDAM

    Public Sub New()

    End Sub

    Public Function Listar() As TipoEntidadBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoEntidad_Listar")

        Dim oTipoEntidadBE As New TipoEntidadBE

        db.LoadDataSet(dbCommand, oTipoEntidadBE, "TipoEntidad")

        Return oTipoEntidadBE

    End Function

End Class
