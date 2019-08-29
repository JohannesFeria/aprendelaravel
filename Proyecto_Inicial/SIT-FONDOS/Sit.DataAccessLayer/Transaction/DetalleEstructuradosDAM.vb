Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql



	''' <summary>
	''' Clase para el acceso de los datos para DetalleEstructurados tabla.
	''' </summary>
	Public class DetalleEstructuradosDAM
	
		Public Sub New()

		End Sub

		''' <summary>
		''' Inserta un expediente en DetalleEstructurados tabla.
		''' <summary>
		''' <param name="codigoTipoInstrumento"></param>
		''' <param name="monto"></param>
		''' <param name="cantidad"></param>
		''' <param name="usuarioCreacion"></param>
		''' <param name="horaCreacion"></param>
		''' <param name="fechaCreacion"></param>
		''' <param name="horaModificacion"></param>
		''' <param name="usuarioModificacion"></param>
		''' <param name="usuarioEliminacion"></param>
		''' <param name="fechaModificacion"></param>
		''' <param name="codigoISIN"></param>
		''' <param name="codigoNemonico"></param>
		''' <param name="fechaEliminacion"></param>
		''' <param name="host"></param>
		''' <param name="horaEliminacion"></param>
		''' <returns></returns>
		Public  Function Insertar(ByVal codigoTipoInstrumento As Decimal, ByVal monto As Decimal, ByVal cantidad As Decimal, ByVal usuarioCreacion As String, ByVal horaCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaModificacion As String, ByVal usuarioModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaModificacion As Decimal, ByVal codigoISIN As String, ByVal codigoNemonico As String, ByVal fechaEliminacion As Decimal, ByVal host As String, ByVal horaEliminacion As String)as String 
			Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleEstructurados_Insertar")

        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.Decimal, codigoTipoInstrumento)
        db.AddInParameter(dbCommand, "@p_Monto", DbType.Decimal, monto)
        db.AddInParameter(dbCommand, "@p_Cantidad", DbType.Decimal, cantidad)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)

        db.ExecuteNonQuery(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona un solo expediente de DetalleEstructurados tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Seleccionar(ByVal codigoISIN As String, ByVal codigoNemonico As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleEstructurados_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de DetalleEstructurados tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoTipoInstrumento"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoTipoInstrumento(ByVal codigoTipoInstrumento As Decimal) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleEstructurados_SeleccionarPorCodigoTipoInstrumento")

        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.Decimal, codigoTipoInstrumento)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de DetalleEstructurados tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoISIN"></param>
    ''' <param name="codigoNemonico"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleEstructurados_SeleccionarPorCodigoISIN_CodigoNemonico")

        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Lista todos los expedientes de DetalleEstructurados tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleEstructurados_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Midifica un expediente en DetalleEstructurados tabla.
    ''' <summary>
    ''' <param name="codigoTipoInstrumento"></param>
    ''' <param name="monto"></param>
    ''' <param name="cantidad"></param>
    ''' <param name="usuarioCreacion"></param>
    ''' <param name="horaCreacion"></param>
    ''' <param name="fechaCreacion"></param>
    ''' <param name="horaModificacion"></param>
    ''' <param name="usuarioModificacion"></param>
    ''' <param name="usuarioEliminacion"></param>
    ''' <param name="fechaModificacion"></param>
    ''' <param name="codigoISIN"></param>
    ''' <param name="codigoNemonico"></param>
    ''' <param name="fechaEliminacion"></param>
    ''' <param name="host"></param>
    ''' <param name="horaEliminacion"></param>
    Public Function Modificar(ByVal codigoTipoInstrumento As Decimal, ByVal monto As Decimal, ByVal cantidad As Decimal, ByVal usuarioCreacion As String, ByVal horaCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaModificacion As String, ByVal usuarioModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaModificacion As Decimal, ByVal codigoISIN As String, ByVal codigoNemonico As String, ByVal fechaEliminacion As Decimal, ByVal host As String, ByVal horaEliminacion As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleEstructurados_Modificar")

        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.Decimal, codigoTipoInstrumento)
        db.AddInParameter(dbCommand, "@p_Monto", DbType.Decimal, monto)
        db.AddInParameter(dbCommand, "@p_Cantidad", DbType.Decimal, cantidad)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de DetalleEstructurados table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoISIN As String, ByVal codigoNemonico As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleEstructurados_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de DetalleEstructurados table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoTipoInstrumento(ByVal codigoTipoInstrumento As Decimal) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleEstructurados_EliminarPorCodigoTipoInstrumento")

        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.Decimal, codigoTipoInstrumento)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de DetalleEstructurados table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleEstructurados_EliminarPorCodigoISIN_CodigoNemonico")

        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
	End Class

