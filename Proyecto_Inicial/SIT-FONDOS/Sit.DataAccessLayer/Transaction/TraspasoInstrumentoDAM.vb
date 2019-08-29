Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql



	''' <summary>
	''' Clase para el acceso de los datos para TraspasoInstrumento tabla.
	''' </summary>
	Public class TraspasoInstrumentoDAM
	
		Public Sub New()

		End Sub

		''' <summary>
		''' Inserta un expediente en TraspasoInstrumento tabla.
		''' <summary>
		''' <param name="codigoTraspaso"></param>
		''' <param name="fondoOrigen"></param>
		''' <param name="fondoDestino"></param>
		''' <param name="codigoMoneda"></param>
		''' <param name="codigoSBS"></param>
		''' <param name="horaCreacion"></param>
		''' <param name="usuarioCreacion"></param>
		''' <param name="fechaCreacion"></param>
		''' <param name="usuarioModificacion"></param>
		''' <param name="horaModificacion"></param>
		''' <param name="fechaModificacion"></param>
		''' <param name="usuarioEliminacion"></param>
		''' <param name="codigoISIN"></param>
		''' <param name="fechaEliminacion"></param>
		''' <param name="codigoNemonico"></param>
		''' <param name="horaEliminacion"></param>
		''' <param name="host"></param>
		''' <param name="codigoTipoOperacion"></param>
		''' <returns></returns>
		Public  Function Insertar(ByVal codigoTraspaso As String, ByVal fondoOrigen As String, ByVal fondoDestino As String, ByVal codigoMoneda As String, ByVal codigoSBS As Decimal, ByVal horaCreacion As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal usuarioModificacion As String, ByVal horaModificacion As String, ByVal fechaModificacion As Decimal, ByVal usuarioEliminacion As String, ByVal codigoISIN As String, ByVal fechaEliminacion As Decimal, ByVal codigoNemonico As String, ByVal horaEliminacion As String, ByVal host As String, ByVal codigoTipoOperacion As String)as String 
			Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TraspasoInstrumento_Insertar")

        db.AddInParameter(dbCommand, "@p_CodigoTraspaso", DbType.String, codigoTraspaso)
        db.AddInParameter(dbCommand, "@p_FondoOrigen", DbType.String, fondoOrigen)
        db.AddInParameter(dbCommand, "@p_FondoDestino", DbType.String, fondoDestino)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.Decimal, codigoSBS)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)
        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)

        db.ExecuteNonQuery(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona un solo expediente de TraspasoInstrumento tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Seleccionar(ByVal codigoTraspaso As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TraspasoInstrumento_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoTraspaso", DbType.String, codigoTraspaso)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de TraspasoInstrumento tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="fondoOrigen"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorFondoOrigen(ByVal fondoOrigen As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TraspasoInstrumento_SeleccionarPorFondoOrigen")

        db.AddInParameter(dbCommand, "@p_FondoOrigen", DbType.String, fondoOrigen)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de TraspasoInstrumento tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoTipoOperacion"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoTipoOperacion(ByVal codigoTipoOperacion As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TraspasoInstrumento_SeleccionarPorCodigoTipoOperacion")

        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de TraspasoInstrumento tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoMoneda"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoMoneda(ByVal codigoMoneda As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TraspasoInstrumento_SeleccionarPorCodigoMoneda")

        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de TraspasoInstrumento tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoISIN"></param>
    ''' <param name="codigoNemonico"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TraspasoInstrumento_SeleccionarPorCodigoISIN_CodigoNemonico")

        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de TraspasoInstrumento tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="fondoDestino"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorFondoDestino(ByVal fondoDestino As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TraspasoInstrumento_SeleccionarPorFondoDestino")

        db.AddInParameter(dbCommand, "@p_FondoDestino", DbType.String, fondoDestino)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Lista todos los expedientes de TraspasoInstrumento tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TraspasoInstrumento_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Midifica un expediente en TraspasoInstrumento tabla.
    ''' <summary>
    ''' <param name="codigoTraspaso"></param>
    ''' <param name="fondoOrigen"></param>
    ''' <param name="fondoDestino"></param>
    ''' <param name="codigoMoneda"></param>
    ''' <param name="codigoSBS"></param>
    ''' <param name="horaCreacion"></param>
    ''' <param name="usuarioCreacion"></param>
    ''' <param name="fechaCreacion"></param>
    ''' <param name="usuarioModificacion"></param>
    ''' <param name="horaModificacion"></param>
    ''' <param name="fechaModificacion"></param>
    ''' <param name="usuarioEliminacion"></param>
    ''' <param name="codigoISIN"></param>
    ''' <param name="fechaEliminacion"></param>
    ''' <param name="codigoNemonico"></param>
    ''' <param name="horaEliminacion"></param>
    ''' <param name="host"></param>
    ''' <param name="codigoTipoOperacion"></param>
    Public Function Modificar(ByVal codigoTraspaso As String, ByVal fondoOrigen As String, ByVal fondoDestino As String, ByVal codigoMoneda As String, ByVal codigoSBS As Decimal, ByVal horaCreacion As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal usuarioModificacion As String, ByVal horaModificacion As String, ByVal fechaModificacion As Decimal, ByVal usuarioEliminacion As String, ByVal codigoISIN As String, ByVal fechaEliminacion As Decimal, ByVal codigoNemonico As String, ByVal horaEliminacion As String, ByVal host As String, ByVal codigoTipoOperacion As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TraspasoInstrumento_Modificar")

        db.AddInParameter(dbCommand, "@p_CodigoTraspaso", DbType.String, codigoTraspaso)
        db.AddInParameter(dbCommand, "@p_FondoOrigen", DbType.String, fondoOrigen)
        db.AddInParameter(dbCommand, "@p_FondoDestino", DbType.String, fondoDestino)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.Decimal, codigoSBS)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)
        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de TraspasoInstrumento table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoTraspaso As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TraspasoInstrumento_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoTraspaso", DbType.String, codigoTraspaso)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de TraspasoInstrumento table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorFondoOrigen(ByVal fondoOrigen As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TraspasoInstrumento_EliminarPorFondoOrigen")

        db.AddInParameter(dbCommand, "@p_FondoOrigen", DbType.String, fondoOrigen)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de TraspasoInstrumento table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoTipoOperacion(ByVal codigoTipoOperacion As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TraspasoInstrumento_EliminarPorCodigoTipoOperacion")

        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de TraspasoInstrumento table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoMoneda(ByVal codigoMoneda As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TraspasoInstrumento_EliminarPorCodigoMoneda")

        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de TraspasoInstrumento table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TraspasoInstrumento_EliminarPorCodigoISIN_CodigoNemonico")

        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de TraspasoInstrumento table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorFondoDestino(ByVal fondoDestino As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TraspasoInstrumento_EliminarPorFondoDestino")

        db.AddInParameter(dbCommand, "@p_FondoDestino", DbType.String, fondoDestino)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
	End Class

