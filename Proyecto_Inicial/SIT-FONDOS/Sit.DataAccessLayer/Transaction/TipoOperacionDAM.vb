Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Data.SqlClient
Imports Sit.BusinessEntities



''' <summary>
''' Clase para el acceso de los datos para TipoOperacion tabla.
''' </summary>
Public Class TipoOperacionDAM

    Private oTipoOperacionRow As TipoOperacionBE.TipoOperacionRow

    Public Sub New()

    End Sub


#Region " /* Seleccionar Functions */ "


    ''' <summary>
    ''' Selecciona un solo expediente de TipoOperacion tabla.
    ''' <summary>
    ''' <param name="descripcion">String</param>
    ''' <param name="situacion">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>TipoOperacionBE</returns>
    Public Function SeleccionarPorFiltros(ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As TipoOperacionBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoOperacion_SeleccionarPorFiltro")
        Dim objeto As New TipoOperacionBE

        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.LoadDataSet(dbCommand, objeto, "TipoOperacion")
        Return objeto
    End Function

    Public Function SeleccionarPorFiltros(ByVal codigoTipoOperacion As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As TipoOperacionBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoOperacion_SeleccionarPorFiltro")
        Dim objeto As New TipoOperacionBE

        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.LoadDataSet(dbCommand, objeto, "TipoOperacion")
        Return objeto
    End Function
    ''' <summary>
    ''' Selecciona un solo expediente de TipoOperacion tabla.
    ''' <summary>
    ''' <param name="codigoTipoOperacion">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>TipoOperacionBE</returns>
    Public Function Seleccionar(ByVal codigoTipoOperacion As String, ByVal dataRequest As DataSet) As TipoOperacionBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoOperacion_Seleccionar")
        Dim objeto As New TipoOperacionBE

        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)
        db.LoadDataSet(dbCommand, objeto, "TipoOperacion")
        Return objeto
    End Function

    ''' <summary>
    ''' Lista todos los expedientes de TipoOperacion tabla.
    ''' <summary>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>TipoOperacionBE</returns>
    Public Function Listar(Optional ByVal situacion As String = "", Optional ByVal egreso As String = "") As TipoOperacionBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoOperacion_Listar")
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_Egreso", DbType.String, egreso)
        Dim objeto As New TipoOperacionBE
        db.LoadDataSet(dbCommand, objeto, "TipoOperacion")
        Return objeto
    End Function


#End Region

#Region " /* Insertar Functions */ "

    ''' <summary>
    ''' Inserta un expediente en TipoOperacion tabla.
    ''' <summary>
    ''' <param name="ob">TipoOperacionBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>String</returns>
    Public Function Insertar(ByVal ob As TipoOperacionBE, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoOperacion_Insertar")

        Dim Codigo As String = String.Empty

        oTipoOperacionRow = CType(ob.TipoOperacion.Rows(0), TipoOperacionBE.TipoOperacionRow)

        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, oTipoOperacionRow.CodigoTipoOperacion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oTipoOperacionRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oTipoOperacionRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return Codigo = "True"

    End Function
#End Region

#Region " /* Modificar Functions */"
    ''' <summary>
    ''' Midifica un expediente en TipoOperacion tabla.
    ''' <summary>
    ''' <param name="ob">TipoOperacionBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Modificar(ByVal ob As TipoOperacionBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoOperacion_Modificar")
        oTipoOperacionRow = CType(ob.TipoOperacion.Rows(0), TipoOperacionBE.TipoOperacionRow)

        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, oTipoOperacionRow.CodigoTipoOperacion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oTipoOperacionRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oTipoOperacionRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function


#End Region

#Region " /* Eliminar Functions */"
    ''' <summary>
    ''' Elimina un expediente de TipoOperacion table por una llave primaria compuesta.
    ''' <summary>
    ''' <param name="codigoTipoOperacion">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Eliminar(ByVal codigoTipoOperacion As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoOperacion_Eliminar")


        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
#End Region



    Public Function SeleccionarporClaseinstrumento(ByVal clase As String, ByVal situacion As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoOperacion_ListarporClaseInstrumento")
        db.AddInParameter(dbCommand, "@p_clase", DbType.String, clase)
        Return db.ExecuteDataSet(dbCommand)
    End Function








End Class

