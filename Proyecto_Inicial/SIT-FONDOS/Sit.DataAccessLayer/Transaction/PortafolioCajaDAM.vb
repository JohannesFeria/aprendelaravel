Imports System.Data.SqlClient
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Data.Common
Imports Sit.BusinessEntities

Public Class PortafolioCajaDAM

    Public Function ObtenerFechaCajaOperaciones(ByVal CodigoPortafolioSBS As String, ByVal CodigoClaseCuenta As String) As DataTable
        'OT10916 - 06/11/2017 - Ian Pastor M. Refactorizar y ordenar código.
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Using dbcomand As DbCommand = db.GetStoredProcCommand("PortafolioCaja_ObtenerFechaCajaOperaciones")
            db.AddInParameter(dbcomand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbcomand, "@p_CodigoClaseCuenta", DbType.String, CodigoClaseCuenta)
            Using ds As DataSet = db.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function

    Public Function Insertar(ByVal CodigoPortafolioSBS As String, ByVal CodigoClaseCuenta As String, ByVal FechaOperacion As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PortafolioCaja_Insertar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, CodigoClaseCuenta)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)

            db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.ExecuteNonQuery(dbCommand)


        End Using

    End Function
End Class
