Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class ParametrosMigracionDAM

    Public Sub New()

    End Sub

    Public Function SeleccionarPorFiltro(ByVal CodigoParametro As String, ByVal secuencial As Decimal, ByVal nombre As String, ByVal valor As String, ByVal situacion As String) As DataSet

        Dim oParametrosMigracionBE As New DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosMigracion_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoParametro", DbType.String, CodigoParametro)
        db.AddInParameter(dbCommand, "@p_Secuencial", DbType.Decimal, secuencial)
        db.AddInParameter(dbCommand, "@p_Nombre", DbType.String, nombre)
        db.AddInParameter(dbCommand, "@p_Valor", DbType.String, valor)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        db.LoadDataSet(dbCommand, oParametrosMigracionBE, "ParametrosMigracion")

        Return oParametrosMigracionBE

    End Function



End Class
