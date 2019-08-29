Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class LimiteTradingDAM
    Private sqlCommand As String = ""
    Private oRow As LimiteTradingBE.LimiteTradingRow
    Dim DECIMAL_NULO As Decimal = -1
    Public Sub New()
    End Sub
    Public Sub InicializarLimiteTrading(ByRef oRow As LimiteTradingBE.LimiteTradingRow)
        oRow.CodigoTrading = DECIMAL_NULO
        oRow.CodigoPortafolioSBS = ""
        oRow.CodigoGrupLimTrader = DECIMAL_NULO
        oRow.Porcentaje = DECIMAL_NULO
        oRow.Situacion = ""
        oRow.UsuarioCreacion = ""
        oRow.FechaCreacion = DECIMAL_NULO
        oRow.HoraCreacion = ""
        oRow.UsuarioModificacion = ""
        oRow.FechaModificacion = DECIMAL_NULO
        oRow.HoraModificacion = ""
    End Sub
#Region " /* Funciones No Transaccionales */ "
    Public Function Seleccionar(ByVal codigoTrading As Decimal) As LimiteTradingBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Seleccionar_LimiteTrading")
        Dim objeto As New LimiteTradingBE
        db.AddInParameter(dbCommand, "@p_CodigoTrading", DbType.Decimal, codigoTrading)
        db.LoadDataSet(dbCommand, objeto, "LimiteTrading")
        Return objeto
    End Function
    Public Function SeleccionarPorFiltro(ByVal strCodigoRenta As String, ByVal decGrupoLimite As Decimal, ByVal strTipoCargo As String,
    ByVal strPortafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_SeleccionarPorFiltro_LimiteTrading")
        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, strCodigoRenta)
        db.AddInParameter(dbCommand, "@p_CodigoGrupLimTrader", DbType.Decimal, decGrupoLimite)
        db.AddInParameter(dbCommand, "@p_TipoCargo", DbType.String, strTipoCargo)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, strPortafolio)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ReporteLimitesTrading() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Reporte_LimiteTrading")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarValidacionExcesosTrader(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, ByVal strUsuario As String,
    ByVal claseInstrumento As String, Optional ByVal decNProceso As Decimal = 0) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ValidarExcesosTrader_LimiteTrading")
        db.AddInParameter(dbCommand, "@p_TipoRenta", DbType.String, strTipoRenta)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, decFechaOperacion)
        db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, strUsuario)
        db.AddInParameter(dbCommand, "@p_claseInstrumento", DbType.String, claseInstrumento)
        db.AddInParameter(dbCommand, "@p_NProceso", DbType.Decimal, decNProceso)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Validacion de trader - Version Fondos
    Public Function SeleccionarValidacionExcesosTrader_Sura(ByVal strTipoRenta As String, ByVal decFechaOperacion As Decimal, ByVal strUsuario As String,
    ByVal claseInstrumento As String, Optional ByVal decNProceso As Decimal = 0) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ValidarExcesosTrader")
            db.AddInParameter(dbCommand, "@p_TipoRenta", DbType.String, strTipoRenta)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, decFechaOperacion)
            db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, strUsuario)
            db.AddInParameter(dbCommand, "@p_claseInstrumento", DbType.String, claseInstrumento)
            db.AddInParameter(dbCommand, "@p_NProceso", DbType.Decimal, decNProceso)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
#End Region
#Region " /* Funciones Transaccionales */ "
    Public Function Insertar(ByVal oLimiteTradingBE As LimiteTradingBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_Insertar_LimiteTrading")
        Dim bolResult As Boolean = False
        oRow = CType(oLimiteTradingBE.LimiteTrading.Rows(0), LimiteTradingBE.LimiteTradingRow)
        db.AddInParameter(dbCommand, "@p_CodigoGrupLimTrader", DbType.Decimal, oRow.CodigoGrupLimTrader)
        db.AddInParameter(dbCommand, "@p_TipoCargo", DbType.String, oRow.TipoCargo)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oRow.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_Porcentaje", DbType.Decimal, oRow.Porcentaje)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddOutParameter(dbCommand, "@p_ValidaIngreso", DbType.Boolean, bolResult)
        db.ExecuteNonQuery(dbCommand)
        bolResult = CType(db.GetParameterValue(dbCommand, "@p_ValidaIngreso"), Boolean)
        Return bolResult
    End Function
    Public Function Modificar(ByVal oLimiteTradingBE As LimiteTradingBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_Actualizar_LimiteTrading")
        Dim bolResult As Boolean = False
        oRow = CType(oLimiteTradingBE.LimiteTrading.Rows(0), LimiteTradingBE.LimiteTradingRow)
        db.AddInParameter(dbCommand, "@p_CodigoTrading", DbType.Decimal, oRow.CodigoTrading)
        db.AddInParameter(dbCommand, "@p_CodigoGrupLimTrader", DbType.Decimal, oRow.CodigoGrupLimTrader)
        db.AddInParameter(dbCommand, "@p_TipoCargo", DbType.String, oRow.TipoCargo)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oRow.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_Porcentaje", DbType.String, oRow.Porcentaje)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddOutParameter(dbCommand, "@p_ValidaIngreso", DbType.Boolean, bolResult)
        db.ExecuteNonQuery(dbCommand)
        bolResult = CType(db.GetParameterValue(dbCommand, "@p_ValidaIngreso"), Boolean)
        Return bolResult
    End Function
    Public Function Eliminar(ByVal decCodigoTrading As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_del_Eliminar_LimiteTrading")
        db.AddInParameter(dbCommand, "@p_CodigoTrading", DbType.Decimal, decCodigoTrading)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
#End Region
End Class
