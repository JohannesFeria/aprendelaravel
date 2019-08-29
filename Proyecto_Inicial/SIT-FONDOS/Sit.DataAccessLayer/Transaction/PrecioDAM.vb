Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql



	''' <summary>
	''' Clase para el acceso de los datos para Precio tabla.
	''' </summary>
	Public class PrecioDAM
	
		Public Sub New()

		End Sub

		''' <summary>
		''' Inserta un expediente en Precio tabla.
		''' <summary>
		''' <param name="codigoPrecio"></param>
		''' <param name="tipoPrecio"></param>
		''' <param name="fecha"></param>
		''' <param name="ultimoPrecio"></param>
		''' <param name="precioActual"></param>
		''' <param name="codigoISIN"></param>
		''' <param name="codigoNemonico"></param>
		''' <param name="usuarioCreacion"></param>
		''' <param name="codigoClaseInstrumento"></param>
		''' <param name="fechaCreacion"></param>
		''' <param name="horaCreacion"></param>
		''' <param name="usuarioModificacion"></param>
		''' <param name="fechaModificacion"></param>
		''' <param name="horaModificacion"></param>
		''' <param name="usuarioEliminacion"></param>
		''' <param name="fechaEliminacion"></param>
		''' <param name="horaEliminacion"></param>
		''' <param name="host"></param>
		''' <returns></returns>
		Public  Function Insertar(ByVal codigoPrecio As String, ByVal tipoPrecio As String, ByVal fecha As Decimal, ByVal ultimoPrecio As Decimal, ByVal precioActual As Decimal, ByVal codigoISIN As String, ByVal codigoNemonico As String, ByVal usuarioCreacion As String, ByVal codigoClaseInstrumento As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String, ByVal host As String)as String 
			Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Precio_Insertar")

        db.AddInParameter(dbCommand, "@p_CodigoPrecio", DbType.String, codigoPrecio)
        db.AddInParameter(dbCommand, "@p_TipoPrecio", DbType.String, tipoPrecio)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_UltimoPrecio", DbType.Decimal, ultimoPrecio)
        db.AddInParameter(dbCommand, "@p_PrecioActual", DbType.Decimal, precioActual)
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_CodigoClaseInstrumento", DbType.String, codigoClaseInstrumento)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)

        db.ExecuteNonQuery(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona un solo expediente de Precio tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Seleccionar(ByVal codigoPrecio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Precio_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoPrecio", DbType.String, codigoPrecio)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de Precio tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoClaseInstrumento"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoClaseInstrumento(ByVal codigoClaseInstrumento As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Precio_SeleccionarPorCodigoClaseInstrumento")

        db.AddInParameter(dbCommand, "@p_CodigoClaseInstrumento", DbType.String, codigoClaseInstrumento)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de Precio tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoISIN"></param>
    ''' <param name="codigoNemonico"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Precio_SeleccionarPorCodigoISIN_CodigoNemonico")

        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Lista todos los expedientes de Precio tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Precio_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Midifica un expediente en Precio tabla.
    ''' <summary>
    ''' <param name="codigoPrecio"></param>
    ''' <param name="tipoPrecio"></param>
    ''' <param name="fecha"></param>
    ''' <param name="ultimoPrecio"></param>
    ''' <param name="precioActual"></param>
    ''' <param name="codigoISIN"></param>
    ''' <param name="codigoNemonico"></param>
    ''' <param name="usuarioCreacion"></param>
    ''' <param name="codigoClaseInstrumento"></param>
    ''' <param name="fechaCreacion"></param>
    ''' <param name="horaCreacion"></param>
    ''' <param name="usuarioModificacion"></param>
    ''' <param name="fechaModificacion"></param>
    ''' <param name="horaModificacion"></param>
    ''' <param name="usuarioEliminacion"></param>
    ''' <param name="fechaEliminacion"></param>
    ''' <param name="horaEliminacion"></param>
    ''' <param name="host"></param>
    Public Function Modificar(ByVal codigoPrecio As String, ByVal tipoPrecio As String, ByVal fecha As Decimal, ByVal ultimoPrecio As Decimal, ByVal precioActual As Decimal, ByVal codigoISIN As String, ByVal codigoNemonico As String, ByVal usuarioCreacion As String, ByVal codigoClaseInstrumento As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String, ByVal host As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Precio_Modificar")

        db.AddInParameter(dbCommand, "@p_CodigoPrecio", DbType.String, codigoPrecio)
        db.AddInParameter(dbCommand, "@p_TipoPrecio", DbType.String, tipoPrecio)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_UltimoPrecio", DbType.Decimal, ultimoPrecio)
        db.AddInParameter(dbCommand, "@p_PrecioActual", DbType.Decimal, precioActual)
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_CodigoClaseInstrumento", DbType.String, codigoClaseInstrumento)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de Precio table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoPrecio As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Precio_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoPrecio", DbType.String, codigoPrecio)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de Precio table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoClaseInstrumento(ByVal codigoClaseInstrumento As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Precio_EliminarPorCodigoClaseInstrumento")

        db.AddInParameter(dbCommand, "@p_CodigoClaseInstrumento", DbType.String, codigoClaseInstrumento)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de Precio table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Precio_EliminarPorCodigoISIN_CodigoNemonico")

        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
	End Class

