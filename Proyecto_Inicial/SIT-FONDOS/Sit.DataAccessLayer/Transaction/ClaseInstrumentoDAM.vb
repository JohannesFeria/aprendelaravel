Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql



''' <summary>
''' Clase para el acceso de los datos para ClaseInstrumento tabla.
''' </summary>
Public Class ClaseInstrumentoDAM
    Private sqlCommand As String = ""
    Private oRow As ClaseInstrumentoBE.ClaseInstrumentoRow
    Public Sub New()

    End Sub


#Region " /* Funciones Seleccionar */ "



    Public Function SeleccionarPorFiltro(ByVal codigoClaseInstrumento As String, ByVal descripcion As String, ByVal dataRequest As DataSet) As ClaseInstrumentoBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ClaseInstrumento_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoClaseInstrumento", DbType.String, codigoClaseInstrumento)
        db.AddInParameter(dbCommand, "@p_Sinonimo", DbType.String, descripcion)

        Dim oClaseInstrumentoBE As New ClaseInstrumentoBE
        db.LoadDataSet(dbCommand, oClaseInstrumentoBE, "ClaseInstrumento")
        Return oClaseInstrumentoBE

    End Function

    Public Function Seleccionar(ByVal codigoClaseInstrumento As String, ByVal dataRequest As DataSet) As ClaseInstrumentoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ClaseInstrumento_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoClaseInstrumento", DbType.String, codigoClaseInstrumento)
        Dim oClaseInstrumentoBE As New ClaseInstrumentoBE
        db.LoadDataSet(dbCommand, oClaseInstrumentoBE, "ClaseInstrumento")
        Return oClaseInstrumentoBE

    End Function

    Public Function SeleccionarDataSet(ByVal codigoClaseInstrumento As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ClaseInstrumento_SeleccionarPorCodigo")
        db.AddInParameter(dbCommand, "@p_CodigoClaseInstrumento", DbType.String, codigoClaseInstrumento)
        Dim oClaseInstrumentoBE As New DataSet
        db.LoadDataSet(dbCommand, oClaseInstrumentoBE, "ClaseInstrumento")
        Return oClaseInstrumentoBE
    End Function

    ''' <summary>
    ''' Lista todos los expedientes de ClaseInstrumentoBE tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar(ByVal dataRequest As DataSet) As ClaseInstrumentoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ClaseInstrumento_Listar")

        Dim oClaseInstrumentoBE As New ClaseInstrumentoBE
        db.LoadDataSet(dbCommand, oClaseInstrumentoBE, "ClaseInstrumento")
        Return oClaseInstrumentoBE

    End Function




    Public Function Seleccionar(Optional ByVal situacion As String = "A") As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ClaseInstrumento_SeleccionarPorSituacion")
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        Dim dstClaseInstrumento As New DataSet
        db.LoadDataSet(dbCommand, dstClaseInstrumento, "ClaseInstrumento")
        Return dstClaseInstrumento

    End Function

    'CMB OT 62087 20110114 Nro 6
    Public Function SeleccionarClaseInstrumentoPorTipoRenta(ByVal tipoRenta As Integer) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_ObtenerClaseInstrumentoPorTipoRenta")
        db.AddInParameter(dbCommand, "@p_TipoRenta", DbType.Int32, tipoRenta)
        Dim dstClaseInstrumento As New DataSet
        db.LoadDataSet(dbCommand, dstClaseInstrumento, "ClaseInstrumento")
        Return dstClaseInstrumento
    End Function

    'CMB OT 62254 20110415
    Public Function SeleccionarPorCategoria(ByVal categoria As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_SeleccionarPorCategoria_ClaseInstrumento")
        db.AddInParameter(dbCommand, "@p_CategoriaInstrumento", DbType.String, categoria)
        Return db.ExecuteDataSet(dbCommand)
    End Function

#End Region

#Region " /* Funciones Insertar */ "

    Public Function Insertar(ByVal oClaseInstrumentoBE As ClaseInstrumentoBE, ByVal dataRequest As DataSet) As String
        Dim Codigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ClaseInstrumento_Insertar")

        oRow = CType(oClaseInstrumentoBE.ClaseInstrumento.Rows(0), ClaseInstrumentoBE.ClaseInstrumentoRow)

        db.AddInParameter(dbCommand, "@p_CodigoClaseInstrumento", DbType.Decimal, oRow.CodigoClaseInstrumento)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, oRow.UsuarioCreacion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        ' db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return Codigo

    End Function

#End Region

#Region " /* Funciones Modificar */"

    ''' <summary>
    ''' Midifica un expediente en ClaseInstrumentoBE tabla.
    ''' <summary>
    ''' <param name="codigoClaseInstrumento"></param>
    ''' <param name="descripcion"></param>
    ''' <param name="usuarioCreacion"></param>
    ''' <param name="onservaciones"></param>
    ''' <param name="fechaCreacion"></param>
    ''' <param name="usuarioModificacion"></param>
    ''' <param name="fechaModificacion"></param>
    ''' <param name="horaCreacion"></param>
    ''' <param name="situacion"></param>
    ''' <param name="host"></param>
    ''' <param name="horaModificacion"></param>
    ''' <param name="usuarioEliminacion"></param>
    ''' <param name="fechaEliminacion"></param>
    ''' <param name="horaEliminacion"></param>
    Public Function Modificar(ByVal oClaseInstrumentoBE As ClaseInstrumentoBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ClaseInstrumento_Modificar")
        oRow = CType(oClaseInstrumentoBE.ClaseInstrumento.Rows(0), ClaseInstrumentoBE.ClaseInstrumentoRow)

        db.AddInParameter(dbCommand, "@p_CodigoClaseInstrumento", DbType.Decimal, oRow.CodigoClaseInstrumento)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        'db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)
        Return True


    End Function


#End Region

#Region " /* Funciones Eliminar */"

    ''' <summary>
    ''' Elimina un expediente de ClaseInstrumentoBE table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoClaseInstrumento As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ClaseInstrumento_Eliminar")
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_CodigoClaseInstrumento", DbType.Decimal, codigoClaseInstrumento)

        db.ExecuteNonQuery(dbCommand)

        Return True
    End Function
#End Region

#Region " /* Funciones Personalizadas*/"

#End Region

End Class

