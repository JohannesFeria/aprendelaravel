Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql



	''' <summary>
	''' Clase para el acceso de los datos para Ejecuciones tabla.
	''' </summary>
	Public class EjecucionesDAM
	
		Public Sub New()

		End Sub

		''' <summary>
		''' Inserta un expediente en Ejecuciones tabla.
		''' <summary>
		''' <param name="codigoEjecucion"></param>
		''' <param name="fecha"></param>
		''' <param name="estado"></param>
		''' <param name="usuario"></param>
		''' <param name="codigoTransaccion"></param>
		''' <returns></returns>
		Public  Function Insertar(ByVal codigoEjecucion As String, ByVal fecha As Decimal, ByVal estado As String, ByVal usuario As String, ByVal codigoTransaccion As String)as String 
			Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Ejecuciones_Insertar")

        db.AddInParameter(dbCommand, "@p_CodigoEjecucion", DbType.String, codigoEjecucion)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_Estado", DbType.String, estado)
        db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, usuario)
        db.AddInParameter(dbCommand, "@p_CodigoTransaccion", DbType.String, codigoTransaccion)

        db.ExecuteNonQuery(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona un solo expediente de Ejecuciones tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Seleccionar(ByVal codigoEjecucion As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Ejecuciones_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoEjecucion", DbType.String, codigoEjecucion)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de Ejecuciones tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoTransaccion"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoTransaccion(ByVal codigoTransaccion As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Ejecuciones_SeleccionarPorCodigoTransaccion")

        db.AddInParameter(dbCommand, "@p_CodigoTransaccion", DbType.String, codigoTransaccion)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Lista todos los expedientes de Ejecuciones tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Ejecuciones_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Midifica un expediente en Ejecuciones tabla.
    ''' <summary>
    ''' <param name="codigoEjecucion"></param>
    ''' <param name="fecha"></param>
    ''' <param name="estado"></param>
    ''' <param name="usuario"></param>
    ''' <param name="codigoTransaccion"></param>
    Public Function Modificar(ByVal codigoEjecucion As String, ByVal fecha As Decimal, ByVal estado As String, ByVal usuario As String, ByVal codigoTransaccion As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Ejecuciones_Modificar")

        db.AddInParameter(dbCommand, "@p_CodigoEjecucion", DbType.String, codigoEjecucion)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_Estado", DbType.String, estado)
        db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, usuario)
        db.AddInParameter(dbCommand, "@p_CodigoTransaccion", DbType.String, codigoTransaccion)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de Ejecuciones table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoEjecucion As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Ejecuciones_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoEjecucion", DbType.String, codigoEjecucion)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de Ejecuciones table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoTransaccion(ByVal codigoTransaccion As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Ejecuciones_EliminarPorCodigoTransaccion")

        db.AddInParameter(dbCommand, "@p_CodigoTransaccion", DbType.String, codigoTransaccion)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
	End Class

