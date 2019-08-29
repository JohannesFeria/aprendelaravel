Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Data.SqlClient
Imports Sit.BusinessEntities
Public Class OperacionDAM
    Public Function SeleccionarPorCodigoTipoOperacion(ByVal codigoTipoOperacion As String, ByVal Egreso As String, ByVal CodigoClaseCuenta As String) As OperacionBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim oOperacionBE As New OperacionBE
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Operacion_SeleccionarPorCodigoTipoOperacion")
        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)
        db.AddInParameter(dbCommand, "@p_Egreso", DbType.String, Egreso)
        db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, CodigoClaseCuenta)
        db.LoadDataSet(dbCommand, oOperacionBE, "Operacion")
        Return oOperacionBE
    End Function
    Public Function SeleccionarporClaseinstrumento(ByVal clase As String, ByVal situacion As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Operacion_ListarporClaseInstrumento")
        db.AddInParameter(dbCommand, "@p_clase", DbType.String, clase)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarPorTrasladoOI() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Operacion_ListarPorTrasladoOI")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarOperacionesSwap(ByVal codigooperacion As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Operacion_ListarSwap")
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codigooperacion)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarOperacionesFx() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ListarOperacionFX_Operacion")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Lista operacion por categoria
    Public Function ListarOperacion_Categoria(Categoria As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_lis_ListarOperacion_CCI")
        db.AddInParameter(dbCommand, "@p_Categoria", DbType.String, Categoria)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
    Public Function SeleccionarOperacionOpuesta(ByVal strOperacion As String, ByVal clase As String, ByVal situacion As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Operacion_SeleccionarOperacionOpuesta")
        db.AddInParameter(dbCommand, "@p_operacion", DbType.String, strOperacion)
        db.AddInParameter(dbCommand, "@p_clase", DbType.String, clase)
        db.AddInParameter(dbCommand, "@p_situacion", DbType.String, situacion)
        Return CType(db.ExecuteScalar(dbCommand), String)
    End Function
    Public Function SeleccionarPorFiltro(ByVal descripcion As String, ByVal situacion As String) As OperacionBE
        Dim oOperacionBE As New OperacionBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Operacion_SeleccionarPorFiltro")
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.LoadDataSet(dbCommand, oOperacionBE, "Operacion")
        Return oOperacionBE
    End Function
    Public Function Seleccionar(ByVal codigoOperacion As String) As OperacionBE
        Dim oOperacionBE As New OperacionBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Operacion_Seleccionar")
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codigoOperacion)
        db.LoadDataSet(dbCommand, oOperacionBE, "Operacion")
        Return oOperacionBE
    End Function
    Public Sub Insertar(ByVal oOperacionBE As OperacionBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Operacion_Insertar")
        Dim oRow As OperacionBE.OperacionRow
        oRow = DirectCast(oOperacionBE.Operacion.Rows(0), OperacionBE.OperacionRow)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, oRow.CodigoOperacion)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, oRow.CodigoTipoOperacion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, oRow.CodigoClaseCuenta)
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Function Modificar(ByVal oOperacionBE As OperacionBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Operacion_Modificar")
        Dim oRow As OperacionBE.OperacionRow
        oRow = DirectCast(oOperacionBE.Operacion.Rows(0), OperacionBE.OperacionRow)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, oRow.CodigoOperacion)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, oRow.CodigoTipoOperacion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, oRow.CodigoClaseCuenta)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function Eliminar(ByVal codigoOperacion As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Operacion_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codigoOperacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function Listar() As OperacionBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Operacion_Listar")
        Dim oOperacionBE As New OperacionBE
        db.LoadDataSet(dbCommand, oOperacionBE, "Operacion")
        Return oOperacionBE
    End Function
    Public Function ListarDataTable() As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Operacion_Listar")
        Dim oOperacionBE As New DataSet
        db.LoadDataSet(dbCommand, oOperacionBE, "Operacion")
        Return oOperacionBE.Tables(0)
    End Function
    Public Function Listar_ClaseInstrumento() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Operacion_ListarOperacionesOI")
        Dim oOperacionBE As New DataSet
        db.LoadDataSet(dbCommand, oOperacionBE, "Operacion")
        Return oOperacionBE
    End Function
    Public Function ListaModeloCartaOperacion(ByVal codigoOperacion As String, ByVal CodigoOrden As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("usp_ListaModeloCartaOperacion")
        db.AddInParameter(dbCommand, "@nvcCodigoOperacion", DbType.String, codigoOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
        Dim ds As New DataSet
        db.LoadDataSet(dbCommand, ds, "Operacion")
        Return ds.Tables(0)
    End Function
    Public Function ListaOperacionLlamado() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_OperacionesLlamados")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function OperacionCodAuto() As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("SP_OperacionCodAuto")
        Dim ds As New DataSet
        db.LoadDataSet(dbCommand, ds, "Operacion")
        Return ds.Tables(0).Rows(0).Item(0)
    End Function
End Class