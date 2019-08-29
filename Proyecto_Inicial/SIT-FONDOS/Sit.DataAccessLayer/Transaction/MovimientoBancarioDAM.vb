Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Data.SqlClient
Imports Sit.BusinessEntities

''' <summary>
''' Clase para el acceso de los datos para MovimientoBancario tabla.
''' </summary>
Public Class MovimientoBancarioDAM
    Private oMovimientoBancarioRow As MovimientoBancarioBE.MovimientoBancarioRow

    Public Sub New()
    End Sub

#Region " /* Seleccionar Functions */ "
    ''' <summary>
    ''' Selecciona una lista Filtrada de los expedientes de MovimientoBancario tabla , segun los paramepros .
    ''' <summary>
    ''' <param name="descripcion">String</param>
    ''' <param name="situacion">String</param>
    ''' <returns>MovimientoBancarioBE</returns>
    Public Function SeleccionarPorFiltros(ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As MovimientoBancarioBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientoBancario_SeleccionarPorFiltro")
        Dim objeto As New MovimientoBancarioBE

        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.LoadDataSet(dbCommand, objeto, "MovimientoBancario")
        Return objeto
    End Function
    ''' <summary>
    ''' Selecciona un solo expediente de MovimientoBancario tabla.
    ''' <summary>
    ''' <param name="codigoMovimiento">String</param>
    ''' <returns>MovimientoBancarioBE</returns>
    Public Function Seleccionar(ByVal codigoMovimiento As String, ByVal dataRequest As DataSet) As MovimientoBancarioBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientoBancario_Seleccionar")
        Dim objeto As New MovimientoBancarioBE
        db.AddInParameter(dbCommand, "@p_CodigoMovimiento", DbType.String, codigoMovimiento)
        db.LoadDataSet(dbCommand, objeto, "MovimientoBancario")
        Return objeto

    End Function

    ''' <summary>
    ''' Lista todos los expedientes de MovimientoBancario tabla.
    ''' <summary>
    ''' <returns>MovimientoBancarioBE</returns>
    Public Function Listar(ByVal dataRequest As DataSet) As MovimientoBancarioBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientoBancario_Listar")
        Dim objeto As New MovimientoBancarioBE

        db.LoadDataSet(dbCommand, objeto, "MovimientoBancario")

        Return objeto

    End Function


#End Region

#Region " /* Insertar Functions */ "

    ''' <summary>
    ''' Inserta un expediente en MovimientoBancario tabla.
    ''' <summary>
    ''' <param name="ob">MovimientoBancarioBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>String</returns>
    Public Function Insertar(ByVal ob As MovimientoBancarioBE, ByVal dataRequest As DataSet) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientoBancario_Insertar")
        Dim Codigo As String = String.Empty

        oMovimientoBancarioRow = CType(ob.MovimientoBancario.Rows(0), MovimientoBancarioBE.MovimientoBancarioRow)

        db.AddInParameter(dbCommand, "@p_CodigoMovimiento", DbType.String, oMovimientoBancarioRow.CodigoMovimiento)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oMovimientoBancarioRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_Naturaleza", DbType.String, oMovimientoBancarioRow.Naturaleza)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oMovimientoBancarioRow.Situacion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return Codigo = "true"

    End Function

#End Region

#Region " /* Modificar Functions */"

    ''' <summary>
    ''' Midifica un expediente en MovimientoBancario tabla.
    ''' <summary>
    ''' <param name="ob">MovimientoBancarioBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Modificar(ByVal ob As MovimientoBancarioBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientoBancario_Modificar")
        oMovimientoBancarioRow = CType(ob.MovimientoBancario.Rows(0), MovimientoBancarioBE.MovimientoBancarioRow)
        db.AddInParameter(dbCommand, "@p_CodigoMovimiento", DbType.String, oMovimientoBancarioRow.CodigoMovimiento)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oMovimientoBancarioRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Naturaleza", DbType.String, oMovimientoBancarioRow.Naturaleza)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oMovimientoBancarioRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Extornar(ByVal CodigoOperacionCaja As String, ByVal codigoPortafolio As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientoBancario_Extornar")
        db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, CodigoOperacionCaja)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

#End Region

#Region " /* Eliminar Functions */"
    ''' <summary>
    ''' Elimina un expediente de MovimientoBancario table por una llave primaria compuesta.
    ''' <summary>
    ''' <param name="codigoMovimiento">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Eliminar(ByVal codigoMovimiento As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientoBancario_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoMovimiento", DbType.String, codigoMovimiento)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function


#End Region


End Class

