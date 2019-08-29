Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class VectorPrecioSBSDAM
    Private oVectorPrecioSBS As VectorPrecioSBSBE.VectorPrecioSBSRow
    Private oVectorPrecioPIP As VectorPrecioPIP.VectorPrecioPIPRow
    Public Sub delVectorPrecioPIP(ByVal Fecha As Decimal, ByVal Manual As String, ByVal Escenario As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_delVectorPrecioPIP")
            db.AddInParameter(dbCommand, "@Fecha", DbType.Decimal, Fecha)
            db.AddInParameter(dbCommand, "@Manual", DbType.String, Manual)
            db.AddInParameter(dbCommand, "@Escenario", DbType.String, Escenario)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Sub Update_RatingValores(p_Rating As String,p_CodigoIsin As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_up_RatingValores")
            db.AddInParameter(dbCommand, "@p_Rating", DbType.String, p_Rating)
            db.AddInParameter(dbCommand, "@p_CodigoIsin", DbType.String, p_CodigoIsin)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Sub VectorPrecioPIP(ByVal objVectorPrecioPIP As VectorPrecioPIP.VectorPrecioPIPRow, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_insVectorPrecioPIP")
            oVectorPrecioPIP = CType(objVectorPrecioPIP, VectorPrecioPIP.VectorPrecioPIPRow)
            db.AddInParameter(dbCommand, "@Fecha", DbType.Decimal, IIf(oVectorPrecioPIP.Fecha = "", 0, oVectorPrecioPIP.Fecha))
            db.AddInParameter(dbCommand, "@Hora", DbType.String, oVectorPrecioPIP.Hora)
            db.AddInParameter(dbCommand, "@CodigoNemonico", DbType.String, oVectorPrecioPIP.CodigoNemonico)
            db.AddInParameter(dbCommand, "@CodigoISIN", DbType.String, oVectorPrecioPIP.CodigoISIN)
            db.AddInParameter(dbCommand, "@DescripcionEmisor", DbType.String, oVectorPrecioPIP.DescripcionEmisor)
            db.AddInParameter(dbCommand, "@Moneda", DbType.String, oVectorPrecioPIP.Moneda)
            db.AddInParameter(dbCommand, "@TipoTasa", DbType.String, oVectorPrecioPIP.TipoTasa)
            db.AddInParameter(dbCommand, "@TasaCupon", DbType.Decimal, IIf(oVectorPrecioPIP.TasaCupon = "", 0, oVectorPrecioPIP.TasaCupon))
            db.AddInParameter(dbCommand, "@FechaVencimiento", DbType.Decimal, IIf(oVectorPrecioPIP.FechaVencimiento = "", 0, oVectorPrecioPIP.FechaVencimiento))
            db.AddInParameter(dbCommand, "@ClasificacionRiesgo", DbType.String, oVectorPrecioPIP.ClasificacionRiesgo)
            db.AddInParameter(dbCommand, "@ValorNominal", DbType.Decimal, IIf(oVectorPrecioPIP.ValorNominal = "", 0, oVectorPrecioPIP.ValorNominal))
            db.AddInParameter(dbCommand, "@PorcPrecioLimpio", DbType.Decimal, IIf(oVectorPrecioPIP.PorcPrecioLimpio = "", 0, oVectorPrecioPIP.PorcPrecioLimpio))
            db.AddInParameter(dbCommand, "@TIR", DbType.Decimal, IIf(oVectorPrecioPIP.TIR = "", 0, oVectorPrecioPIP.TIR))
            db.AddInParameter(dbCommand, "@PorcPrecioSucio", DbType.Decimal, IIf(oVectorPrecioPIP.PorcPrecioSucio = "", 0, oVectorPrecioPIP.PorcPrecioSucio))
            db.AddInParameter(dbCommand, "@SobreTasa", DbType.Decimal, IIf(oVectorPrecioPIP.SobreTasa = "", 0, oVectorPrecioPIP.SobreTasa))
            db.AddInParameter(dbCommand, "@PrecioLimpio", DbType.Decimal, IIf(oVectorPrecioPIP.PrecioLimpio = "", 0, oVectorPrecioPIP.PrecioLimpio))
            db.AddInParameter(dbCommand, "@PrecioSucio", DbType.Decimal, IIf(oVectorPrecioPIP.PrecioSucio = "", 0, oVectorPrecioPIP.PrecioSucio))
            db.AddInParameter(dbCommand, "@IC", DbType.Decimal, IIf(oVectorPrecioPIP.IC = "", 0, oVectorPrecioPIP.IC))
            db.AddInParameter(dbCommand, "@Duracion", DbType.Decimal, IIf(oVectorPrecioPIP.Duracion = "", 0, oVectorPrecioPIP.Duracion))
            db.AddInParameter(dbCommand, "@DetalleMoneda", DbType.String, oVectorPrecioPIP.DetalleMoneda)
            db.AddInParameter(dbCommand, "@FechaCarga", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@Escenario", DbType.String, oVectorPrecioPIP.Escenario)
            db.AddInParameter(dbCommand, "@UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@Manual", DbType.String, oVectorPrecioPIP.Manual)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Function Insertar(ByVal objVectorPrecioSBS As VectorPrecioSBSBE.VectorPrecioSBSRow, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("VectorPrecioSBS_Insertar")
            oVectorPrecioSBS = CType(objVectorPrecioSBS, VectorPrecioSBSBE.VectorPrecioSBSRow)
            db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, oVectorPrecioSBS.CodigoSBS)
            db.AddInParameter(dbCommand, "@p_FechaCarga", DbType.Decimal, oVectorPrecioSBS.FechaCarga)
            db.AddInParameter(dbCommand, "@p_ValorPrecio", DbType.Decimal, oVectorPrecioSBS.ValorPrecio)
            db.AddInParameter(dbCommand, "@p_Devengo", DbType.Decimal, oVectorPrecioSBS.Devengo)
            db.AddInParameter(dbCommand, "@p_TIR", DbType.Decimal, oVectorPrecioSBS.TIR)
            db.AddInParameter(dbCommand, "@p_SignoTir", DbType.String, oVectorPrecioSBS.SignoTIR)
            db.AddInParameter(dbCommand, "@p_VariacionPrecio", DbType.Decimal, oVectorPrecioSBS.VariacionPrecio)
            db.AddInParameter(dbCommand, "@p_SignoVariacion", DbType.String, oVectorPrecioSBS.SignoVariacion)
            db.AddInParameter(dbCommand, "@p_IndicadorVariacion", DbType.String, oVectorPrecioSBS.IndicadorVariacion)
            db.AddInParameter(dbCommand, "@p_Categoria", DbType.String, oVectorPrecioSBS.Categoria)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_Manual", DbType.String, oVectorPrecioSBS.Manual)
            db.ExecuteNonQuery(dbCommand)
            Return oVectorPrecioSBS.CodigoSBS & " - " & System.Convert.ToString(oVectorPrecioSBS.FechaCarga)
        End Using
    End Function
    Public Function EliminarPrecioSBS(ByVal FechaCarga As Date) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VectorPrecioSBS_Eliminar")
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(FechaCarga))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Sub InsertarVectorWarrant(CodigoIsin As String, Subyacente As Decimal, FechaProceso As Decimal, MarkToModel As Decimal,
        StrikePrice As Decimal, SpotPrice As Decimal, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ins_VectorWarrant")
        db.AddInParameter(dbCommand, "@p_CodigoIsin", DbType.String, CodigoIsin)
        db.AddInParameter(dbCommand, "@p_Subyacente", DbType.Decimal, Subyacente)
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, FechaProceso)
        db.AddInParameter(dbCommand, "@p_MarkToModel", DbType.Decimal, MarkToModel)
        db.AddInParameter(dbCommand, "@p_StrikePrice", DbType.Decimal, StrikePrice)
        db.AddInParameter(dbCommand, "@p_SpotPrice", DbType.Decimal, SpotPrice)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Sub BorrarVectorWarrant(FechaProceso As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_del_VectorWarrant")
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, FechaProceso)
        db.ExecuteNonQuery(dbCommand)
    End Sub
End Class