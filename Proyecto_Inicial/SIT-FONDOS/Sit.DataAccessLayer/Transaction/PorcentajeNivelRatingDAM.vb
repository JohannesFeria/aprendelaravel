Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class PorcentajeNivelRatingDAM
    Private sqlCommand As String = ""
    Private oRow As PorcentajeNivelRatingBE.PorcentajeNivelRatingRow
    Dim DECIMAL_NULO As Decimal = -1
    Public Sub New()

    End Sub
    Public Sub InicializarPorcentajeNivelRating(ByRef oRow As PorcentajeNivelRatingBE.PorcentajeNivelRatingRow)
        oRow.ValorCaracteristica = String.Empty
        oRow.ValorPorcentaje = DECIMAL_NULO
        oRow.Situacion = String.Empty
        oRow.CategInver = String.Empty
        oRow.CodigoPortafolioSBS = String.Empty
    End Sub

#Region " /* Funciones No Transaccionales */ "
    Public Function SeleccionarPorFiltro(ByVal oPorcentajeNivelRatingBE As PorcentajeNivelRatingBE) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_PorcentajeNivelRating_SeleccionarPorFiltro")

        oRow = CType(oPorcentajeNivelRatingBE.PorcentajeNivelRating.Rows(0), PorcentajeNivelRatingBE.PorcentajeNivelRatingRow)
        db.AddInParameter(dbCommand, "@p_DescRating", DbType.String, oRow.ValorCaracteristica)
        db.AddInParameter(dbCommand, "@p_CategInver", DbType.String, oRow.CategInver)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oRow.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_GrupoRating", DbType.String, oRow.GrupoRating) 'HDG 20121002 rating
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function SeleccionarCategoriaInversiones() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_CategoriaInversiones_Seleccionar")

        Return db.ExecuteDataSet(dbCommand)
    End Function
#End Region

#Region " /* Funciones Transaccionales */ "
    Public Function Insertar(ByVal oPorcentajeNivelRatingBE As PorcentajeNivelRatingBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_ins_PorcentajeNivelRating_Insertar")

        oRow = CType(oPorcentajeNivelRatingBE.PorcentajeNivelRating.Rows(0), PorcentajeNivelRatingBE.PorcentajeNivelRatingRow)

        db.AddInParameter(dbCommand, "@p_ValorCaracteristica", DbType.String, oRow.ValorCaracteristica)
        db.AddInParameter(dbCommand, "@p_CategInver", DbType.String, oRow.CategInver)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oRow.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_ValorPorcentaje", DbType.Decimal, oRow.ValorPorcentaje)
        db.AddInParameter(dbCommand, "@p_GrupoRating", DbType.String, oRow.GrupoRating) 'HDG 20121002 rating
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Modificar(ByVal oPorcentajeNivelRatingBE As PorcentajeNivelRatingBE, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_upd_PorcentajeNivelRating_Modificar")
        oRow = CType(oPorcentajeNivelRatingBE.PorcentajeNivelRating.Rows(0), PorcentajeNivelRatingBE.PorcentajeNivelRatingRow)

        db.AddInParameter(dbCommand, "@p_ValorCaracteristica", DbType.String, oRow.ValorCaracteristica)
        db.AddInParameter(dbCommand, "@p_CategInver", DbType.String, oRow.CategInver)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oRow.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_ValorPorcentaje", DbType.Decimal, oRow.ValorPorcentaje)
        db.AddInParameter(dbCommand, "@p_GrupoRating", DbType.String, oRow.GrupoRating) 'HDG 20121002 rating
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True
    End Function

    Public Function Eliminar(ByVal oPorcentajeNivelRatingBE As PorcentajeNivelRatingBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_del_PorcentajeNivelRating_Eliminar")
        oRow = CType(oPorcentajeNivelRatingBE.PorcentajeNivelRating.Rows(0), PorcentajeNivelRatingBE.PorcentajeNivelRatingRow)

        db.AddInParameter(dbCommand, "@p_ValorCaracteristica", DbType.Decimal, oRow.ValorCaracteristica)
        db.AddInParameter(dbCommand, "@p_CategInver", DbType.String, oRow.CategInver)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oRow.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_GrupoRating", DbType.String, oRow.GrupoRating) 'HDG 20121002 rating
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
#End Region

End Class
