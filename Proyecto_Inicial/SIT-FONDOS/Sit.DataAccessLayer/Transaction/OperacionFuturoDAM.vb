'JCH OT 66056 Acceso de los base de datos para OperacionFuturo tabla.
Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class OperacionFuturoDAM

    Private oVectorPrecio As VectorPrecioBE.VectorPrecioRow

    Public Function SeleccionarPorFechaAnterior(ByVal fechaOperacion As Decimal, ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_sel_OperacionFutura_FechaAnterior")
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.String, fechaOperacion)
        Return db.ExecuteDataSet(dbCommand)

    End Function

End Class
