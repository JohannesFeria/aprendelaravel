Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Data.SqlClient
Imports Sit.BusinessEntities

''' <summary>
''' Clase para el acceso de los datos para ClaseCuenta tabla.
''' </summary>
Public Class ClaseCuentaDAM

    Public Sub New()

    End Sub
    Public Function Listar(ByVal dataRequest As DataSet) As ClaseCuentaBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ClaseCuenta_Listar")
        Dim objeto As New ClaseCuentaBE
        db.LoadDataSet(dbCommand, objeto, "ClaseCuenta")
        Return objeto
    End Function
    Public Function SeleccionarPorFiltro(ByVal codigoClaseCuenta As String, ByVal descripcion As String, ByVal situacion As String) As ClaseCuentaBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ClaseCuenta_SeleccionarPorFiltro")

        Dim oClaseCuentaBE As New ClaseCuentaBE

        db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, codigoClaseCuenta)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        db.LoadDataSet(dbCommand, oClaseCuentaBE, "ClaseCuenta")

        Return oClaseCuentaBE

    End Function

    Public Function Insertar(ByVal oClaseCuentaBE As ClaseCuentaBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ClaseCuenta_Insertar")

        Dim oClaseCuentaRow As ClaseCuentaBE.ClaseCuentaRow

        oClaseCuentaRow = DirectCast(oClaseCuentaBE.ClaseCuenta.Rows(0), ClaseCuentaBE.ClaseCuentaRow)

        db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, oClaseCuentaRow.CodigoClaseCuenta)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oClaseCuentaRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oClaseCuentaRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, "")

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

    ''' <summary>
    ''' Selecciona un solo expediente de ClaseCuenta tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Seleccionar(ByVal codigoClaseCuenta As String) As DataSet

        Dim oClaseCuentaBE As New ClaseCuentaBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ClaseCuenta_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, codigoClaseCuenta)

        db.LoadDataSet(dbCommand, oClaseCuentaBE, "ClaseCuenta")

        Return oClaseCuentaBE

    End Function

    ''' <summary>
    ''' Lista todos los expedientes de ClaseCuenta tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar() As ClaseCuentaBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ClaseCuenta_Listar")
        Dim objeto As New ClaseCuentaBE
        db.LoadDataSet(dbCommand, objeto, "ClaseCuenta")
        Return objeto
    End Function

    Public Function ClaseCuentaXBanco_Listar(ByVal sCodigoTercero As String) As ClaseCuentaBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ClaseCuentaXBanco_Listar")

        db.AddInParameter(dbCommand, "@CodigoTercero", DbType.String, sCodigoTercero)

        Dim objeto As New ClaseCuentaBE
        db.LoadDataSet(dbCommand, objeto, "ClaseCuenta")
        Return objeto
    End Function

    ''' <summary>
    ''' Midifica un expediente en ClaseCuenta tabla.
    ''' <summary>
    ''' <param name="codigoClaseCuenta"></param>
    ''' <param name="usuarioCreacion"></param>
    ''' <param name="descripcion"></param>
    ''' <param name="fechaCreacion"></param>
    ''' <param name="usuarioModificacion"></param>
    ''' <param name="horaCreacion"></param>
    ''' <param name="fechaModificacion"></param>
    ''' <param name="situacion"></param>
    ''' <param name="horaModificacion"></param>
    ''' <param name="host"></param>
    ''' <param name="usuarioEliminacion"></param>
    ''' <param name="fechaEliminacion"></param>
    ''' <param name="horaEliminacion"></param>
    Public Function Modificar(ByVal oClaseCuentaBE As ClaseCuentaBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ClaseCuenta_Modificar")

        Dim oClaseCuentaRow As ClaseCuentaBE.ClaseCuentaRow

        oClaseCuentaRow = DirectCast(oClaseCuentaBE.ClaseCuenta.Rows(0), ClaseCuentaBE.ClaseCuentaRow)

        db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, oClaseCuentaRow.CodigoClaseCuenta)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oClaseCuentaRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oClaseCuentaRow.Situacion)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de ClaseCuenta table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoClaseCuenta As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ClaseCuenta_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, codigoClaseCuenta)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function
End Class

