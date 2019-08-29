Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports Sit.BusinessEntities

Public Class RatingDAM
    Private oRating As RatingBE.RegistroRatingRow

    Public Sub New()
    End Sub

    Public Sub Borrar_Rating(ByVal FechaProceso As Decimal, ByVal tipoArchivo As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_Rating_Eliminar")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, FechaProceso)
        db.AddInParameter(dbCommand, "@p_TipoInformacion", DbType.String, tipoArchivo)
        db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.ExecuteNonQuery(dbCommand)
    End Sub

    Public Function Insertar(ByVal orowRating As RatingBE.RegistroRatingRow, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_Rating_Insertar")
        dbCommand.CommandTimeout = 1020
        oRating = CType(orowRating, RatingBE.RegistroRatingRow)
        db.AddInParameter(dbCommand, "@p_Codigo", DbType.String, Left(Trim(oRating.Codigo), 20))
        db.AddInParameter(dbCommand, "@p_Rating", DbType.String, Left(Trim(oRating.Rating), 10))
        db.AddInParameter(dbCommand, "@p_RatingInterno", DbType.String, Left(Trim(oRating.RatingInterno), 10))
        db.AddInParameter(dbCommand, "@p_RatingFF", DbType.String, Left(Trim(oRating.RatingFF), 10))
        db.AddInParameter(dbCommand, "@p_LineaPlazo", DbType.String, Left(Trim(oRating.LineaPlazo), 15))
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, oRating.Fecha)

        db.AddInParameter(dbCommand, "@p_TipoNegocio", DbType.String, Left(Trim(oRating.TipoNegocio), 5))
        db.AddInParameter(dbCommand, "@p_Clasificadora", DbType.String, Left(Trim(oRating.Clasificadora), 100))
        db.AddInParameter(dbCommand, "@p_FechaClasificacion", DbType.Decimal, oRating.FechaClasificacion)

        db.AddInParameter(dbCommand, "@p_TipoInformacion", DbType.String, Trim(oRating.Tipo))
        db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)
        Return ""
    End Function

    Function SeleccionarPorFecha(ByVal fechaProceso As Decimal, ByVal tipoArchivo As String, ByVal dataRequest As DataSet) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_Rating_Seleccionar")

        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fechaProceso)
        db.AddInParameter(dbCommand, "@p_TipoInformacion", DbType.String, tipoArchivo)
        db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))

        Dim ds As New DataSet
        db.LoadDataSet(dbCommand, ds, "Rating")
        Return ds.Tables("Rating")

    End Function

    Function ObtenerCodigoRatingxNombre(ByVal Rating As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("ObtenerCodigoRating")

        db.AddInParameter(dbCommand, "@Rating", DbType.String, Rating)

        Dim ds As New DataSet
        db.LoadDataSet(dbCommand, ds, "Rating")
        Return ds.Tables("Rating")

    End Function
End Class
