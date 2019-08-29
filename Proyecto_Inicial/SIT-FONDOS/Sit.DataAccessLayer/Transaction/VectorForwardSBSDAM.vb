Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class VectorForwardSBSDAM
    Private oVectorForwardSBSBE As VectorForwardSBSBE.VectorForwardSBSRow
    Public Function Insertar(ByVal objVectorForwardSBS As VectorForwardSBSBE.VectorForwardSBSRow, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("VectorForwardSBS_Insertar") 'OT10709 Refactorizar código
            dbCommand.CommandTimeout = 1020
            oVectorForwardSBSBE = CType(objVectorForwardSBS, VectorForwardSBSBE.VectorForwardSBSRow)
            db.AddInParameter(dbCommand, "@p_NumeroPoliza", DbType.String, oVectorForwardSBSBE.NumeroPoliza)
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, oVectorForwardSBSBE.Fecha)
            db.AddInParameter(dbCommand, "@p_PrecioForward", DbType.Decimal, oVectorForwardSBSBE.PrecioForward)
            db.AddInParameter(dbCommand, "@p_Mtm", DbType.Decimal, oVectorForwardSBSBE.Mtm)
            db.AddInParameter(dbCommand, "@p_MtmDestino", DbType.Decimal, oVectorForwardSBSBE.MtmDestino)
            db.AddInParameter(dbCommand, "@p_PrecioVector", DbType.Decimal, oVectorForwardSBSBE.PrecioVector)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Function
    Public Function CargarPrecioFWD(ByVal decFecha As Decimal) As DataSet
        CargarPrecioFWD = Nothing
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_CargarVectorForwardSBS") 'OT10709 Refactorizar código
            dbCommand.CommandTimeout = 1020
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, decFecha)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                CargarPrecioFWD = ds
            End Using
        End Using
    End Function
    Public Function EliminatmpVectorForwardSBS() As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_del_Elimina_tmpVectorForwardSBS") 'OT10709 Refactorizar código
            dbCommand.CommandTimeout = 1020
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Function
End Class