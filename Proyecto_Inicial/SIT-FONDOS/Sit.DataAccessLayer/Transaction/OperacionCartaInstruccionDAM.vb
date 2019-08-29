Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql



	''' <summary>
	''' Clase para el acceso de los datos para OperacionCartaInstruccion tabla.
	''' </summary>
	Public class OperacionCartaInstruccionDAM
	
		Public Sub New()

		End Sub

		''' <summary>
		''' Inserta un expediente en OperacionCartaInstruccion tabla.
		''' <summary>
		''' <param name="codigoModelo"></param>
		''' <param name="codigoTipoOperacion"></param>
		''' <param name="situacion"></param>
		''' <param name="usuarioCreacion"></param>
		''' <param name="fechaCreacion"></param>
		''' <param name="horaCreacion"></param>
		''' <param name="usuarioModificacion"></param>
		''' <param name="fechaModificacion"></param>
		''' <param name="host"></param>
		''' <param name="horaModificacion"></param>
		''' <param name="usuarioEliminacion"></param>
		''' <param name="fechaEliminacion"></param>
		''' <param name="horaEliminacion"></param>
		''' <returns></returns>
		Public  Function Insertar(ByVal codigoModelo As String, ByVal codigoTipoOperacion As String, ByVal situacion As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal host As String, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String)as String 
			Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("OperacionCartaInstruccion_Insertar")

        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, codigoModelo)
        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)

        db.ExecuteNonQuery(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona un solo expediente de OperacionCartaInstruccion tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Seleccionar(ByVal codigoModelo As String, ByVal codigoTipoOperacion As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("OperacionCartaInstruccion_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, codigoModelo)
        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de OperacionCartaInstruccion tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoModelo"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoModelo(ByVal codigoModelo As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("OperacionCartaInstruccion_SeleccionarPorCodigoModelo")

        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, codigoModelo)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de OperacionCartaInstruccion tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoTipoOperacion"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoTipoOperacion(ByVal codigoTipoOperacion As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("OperacionCartaInstruccion_SeleccionarPorCodigoTipoOperacion")

        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Lista todos los expedientes de OperacionCartaInstruccion tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("OperacionCartaInstruccion_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Midifica un expediente en OperacionCartaInstruccion tabla.
    ''' <summary>
    ''' <param name="codigoModelo"></param>
    ''' <param name="codigoTipoOperacion"></param>
    ''' <param name="situacion"></param>
    ''' <param name="usuarioCreacion"></param>
    ''' <param name="fechaCreacion"></param>
    ''' <param name="horaCreacion"></param>
    ''' <param name="usuarioModificacion"></param>
    ''' <param name="fechaModificacion"></param>
    ''' <param name="host"></param>
    ''' <param name="horaModificacion"></param>
    ''' <param name="usuarioEliminacion"></param>
    ''' <param name="fechaEliminacion"></param>
    ''' <param name="horaEliminacion"></param>
    Public Function Modificar(ByVal codigoModelo As String, ByVal codigoTipoOperacion As String, ByVal situacion As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal host As String, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("OperacionCartaInstruccion_Modificar")

        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, codigoModelo)
        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de OperacionCartaInstruccion table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoModelo As String, ByVal codigoTipoOperacion As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("OperacionCartaInstruccion_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, codigoModelo)
        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de OperacionCartaInstruccion table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoModelo(ByVal codigoModelo As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("OperacionCartaInstruccion_EliminarPorCodigoModelo")

        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, codigoModelo)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de OperacionCartaInstruccion table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoTipoOperacion(ByVal codigoTipoOperacion As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("OperacionCartaInstruccion_EliminarPorCodigoTipoOperacion")

        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
	End Class

