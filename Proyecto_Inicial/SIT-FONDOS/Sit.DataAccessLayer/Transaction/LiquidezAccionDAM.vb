Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class LiquidezAccionDAM
    Private sqlCommand As String = ""
    Private oLiquidezAccionRow As LiquidezAccionBE.LiquidezAccionRow

    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "

    Public Function Listar() As LiquidezAccionBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("LiquidezAccion_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As LiquidezAccionBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("LiquidezAccion_Listar")

        Dim objeto As New LiquidezAccionBE
        db.LoadDataSet(dbCommand, objeto, "LiquidezAccion")
        Return objeto
    End Function


    Public Function SeleccionarPorFiltro(ByVal CodigoMnemonico As String, ByVal Situacion As String, ByVal dataRequest As DataSet) As LiquidezAccionBE
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("LiquidezAccion_SeleccionarPorFiltro")

            db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, CodigoMnemonico)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, Situacion)

            Dim objeto As New LiquidezAccionBE
            db.LoadDataSet(dbCommand, objeto, "LiquidezAccion")
            Return objeto
        Catch ex As Exception
            Throw ex
        End Try
    End Function

#End Region


    Public Function Insertar(ByVal ob As LiquidezAccionBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("LiquidezAccion_Insertar")
        oLiquidezAccionRow = CType(ob.LiquidezAccion.Rows(0), LiquidezAccionBE.LiquidezAccionRow)

        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, oLiquidezAccionRow.CodigoMnemonico)
        db.AddInParameter(dbCommand, "@p_CriterioLiquidez", DbType.String, oLiquidezAccionRow.CriterioLiquidez)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oLiquidezAccionRow.Situacion)

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
    End Function


    Public Function Modificar(ByVal ob As LiquidezAccionBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("LiquidezAccion_Modificar")
        oLiquidezAccionRow = CType(ob.LiquidezAccion.Rows(0), LiquidezAccionBE.LiquidezAccionRow)

        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, oLiquidezAccionRow.CodigoMnemonico)
        db.AddInParameter(dbCommand, "@p_CriterioLiquidez", DbType.String, oLiquidezAccionRow.CriterioLiquidez)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oLiquidezAccionRow.Situacion)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function


    Public Function Eliminar(ByVal CodigoMnemonico As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("LiquidezAccion_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, CodigoMnemonico)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
End Class
