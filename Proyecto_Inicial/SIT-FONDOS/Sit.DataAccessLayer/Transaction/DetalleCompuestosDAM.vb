Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql



	''' <summary>
	''' Clase para el acceso de los datos para DetalleCompuestos tabla.
	''' </summary>
	Public class DetalleCompuestosDAM
	
		Public Sub New()

		End Sub

		''' <summary>
		''' Inserta un expediente en DetalleCompuestos tabla.
		''' <summary>
		''' <param name="usuarioCreacion"></param>
		''' <param name="porcentaje"></param>
		''' <param name="fechaCreacion"></param>
		''' <param name="horaCreacion"></param>
		''' <param name="usuarioModificacion"></param>
		''' <param name="codigoTipoInstrumento"></param>
		''' <param name="horaModificacion"></param>
		''' <param name="fechaModificacion"></param>
		''' <param name="usuarioEliminacion"></param>
		''' <param name="host"></param>
		''' <param name="codigoISIN"></param>
		''' <param name="codigoNemonico"></param>
		''' <param name="fechaEliminacion"></param>
		''' <param name="horaEliminacion"></param>
		''' <returns></returns>
		Public  Function Insertar(ByVal usuarioCreacion As String, ByVal porcentaje As Decimal, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal codigoTipoInstrumento As Decimal, ByVal horaModificacion As String, ByVal fechaModificacion As Decimal, ByVal usuarioEliminacion As String, ByVal host As String, ByVal codigoISIN As String, ByVal codigoNemonico As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String)as String 
			Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleCompuestos_Insertar")

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_Porcentaje", DbType.Decimal, porcentaje)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.Decimal, codigoTipoInstrumento)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)

        db.ExecuteNonQuery(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona un solo expediente de DetalleCompuestos tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Seleccionar(ByVal codigoISIN As String, ByVal codigoNemonico As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleCompuestos_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de DetalleCompuestos tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoISIN"></param>
    ''' <param name="codigoNemonico"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleCompuestos_SeleccionarPorCodigoISIN_CodigoNemonico")

        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de DetalleCompuestos tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoTipoInstrumento"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoTipoInstrumento(ByVal codigoTipoInstrumento As Decimal) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleCompuestos_SeleccionarPorCodigoTipoInstrumento")

        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.Decimal, codigoTipoInstrumento)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Lista todos los expedientes de DetalleCompuestos tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleCompuestos_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Midifica un expediente en DetalleCompuestos tabla.
    ''' <summary>
    ''' <param name="usuarioCreacion"></param>
    ''' <param name="porcentaje"></param>
    ''' <param name="fechaCreacion"></param>
    ''' <param name="horaCreacion"></param>
    ''' <param name="usuarioModificacion"></param>
    ''' <param name="codigoTipoInstrumento"></param>
    ''' <param name="horaModificacion"></param>
    ''' <param name="fechaModificacion"></param>
    ''' <param name="usuarioEliminacion"></param>
    ''' <param name="host"></param>
    ''' <param name="codigoISIN"></param>
    ''' <param name="codigoNemonico"></param>
    ''' <param name="fechaEliminacion"></param>
    ''' <param name="horaEliminacion"></param>
    Public Function Modificar(ByVal usuarioCreacion As String, ByVal porcentaje As Decimal, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal codigoTipoInstrumento As Decimal, ByVal horaModificacion As String, ByVal fechaModificacion As Decimal, ByVal usuarioEliminacion As String, ByVal host As String, ByVal codigoISIN As String, ByVal codigoNemonico As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleCompuestos_Modificar")

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_Porcentaje", DbType.Decimal, porcentaje)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.Decimal, codigoTipoInstrumento)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de DetalleCompuestos table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoISIN As String, ByVal codigoNemonico As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleCompuestos_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de DetalleCompuestos table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleCompuestos_EliminarPorCodigoISIN_CodigoNemonico")

        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de DetalleCompuestos table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoTipoInstrumento(ByVal codigoTipoInstrumento As Decimal) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleCompuestos_EliminarPorCodigoTipoInstrumento")

        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.Decimal, codigoTipoInstrumento)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
	End Class

