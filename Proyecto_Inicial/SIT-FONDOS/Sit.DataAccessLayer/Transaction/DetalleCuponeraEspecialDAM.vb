Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql



	''' <summary>
	''' Clase para el acceso de los datos para DetalleCuponeraEspecial tabla.
	''' </summary>
	Public class DetalleCuponeraEspecialDAM
	
		Public Sub New()

		End Sub

		''' <summary>
		''' Inserta un expediente en DetalleCuponeraEspecial tabla.
		''' <summary>
		''' <param name="codigoISIN"></param>
		''' <param name="codigoNemonico"></param>
		''' <param name="fechaInicio"></param>
		''' <param name="fechaTermino"></param>
		''' <param name="de"></param>
		''' <param name="a"></param>
		''' <param name="diferenciaDias"></param>
		''' <param name="tasaCupon"></param>
		''' <param name="base"></param>
		''' <param name="diasPago"></param>
		''' <param name="usuarioCreacion"></param>
		''' <param name="fechaCreacion"></param>
		''' <param name="horaCreacion"></param>
		''' <param name="usuarioModificacion"></param>
		''' <param name="fechaModificacion"></param>
		''' <param name="horaModificacion"></param>
		''' <param name="usuarioEliminacion"></param>
		''' <param name="fechaEliminacion"></param>
		''' <param name="horaEliminacion"></param>
		''' <param name="host"></param>
		''' <param name="codigoAmortizacion"></param>
		''' <returns></returns>
		Public  Function Insertar(ByVal codigoISIN As String, ByVal codigoNemonico As String, ByVal fechaInicio As Decimal, ByVal fechaTermino As Decimal, ByVal de As Decimal, ByVal a As Decimal, ByVal diferenciaDias As Decimal, ByVal tasaCupon As Decimal, ByVal base As Decimal, ByVal diasPago As Decimal, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String, ByVal host As String, ByVal codigoAmortizacion As String)as String 
			Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleCuponeraEspecial_Insertar")

        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, fechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaTermino", DbType.Decimal, fechaTermino)
        db.AddInParameter(dbCommand, "@p_De", DbType.Decimal, de)
        db.AddInParameter(dbCommand, "@p_A", DbType.Decimal, a)
        db.AddInParameter(dbCommand, "@p_DiferenciaDias", DbType.Decimal, diferenciaDias)
        db.AddInParameter(dbCommand, "@p_TasaCupon", DbType.Decimal, tasaCupon)
        db.AddInParameter(dbCommand, "@p_Base", DbType.Decimal, base)
        db.AddInParameter(dbCommand, "@p_DiasPago", DbType.Decimal, diasPago)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)
        db.AddInParameter(dbCommand, "@p_CodigoAmortizacion", DbType.String, codigoAmortizacion)

        db.ExecuteNonQuery(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona un solo expediente de DetalleCuponeraEspecial tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Seleccionar(ByVal codigoISIN As String, ByVal codigoNemonico As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleCuponeraEspecial_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de DetalleCuponeraEspecial tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoAmortizacion"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoAmortizacion(ByVal codigoAmortizacion As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleCuponeraEspecial_SeleccionarPorCodigoAmortizacion")

        db.AddInParameter(dbCommand, "@p_CodigoAmortizacion", DbType.String, codigoAmortizacion)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de DetalleCuponeraEspecial tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoISIN"></param>
    ''' <param name="codigoNemonico"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleCuponeraEspecial_SeleccionarPorCodigoISIN_CodigoNemonico")

        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Lista todos los expedientes de DetalleCuponeraEspecial tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleCuponeraEspecial_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Midifica un expediente en DetalleCuponeraEspecial tabla.
    ''' <summary>
    ''' <param name="codigoISIN"></param>
    ''' <param name="codigoNemonico"></param>
    ''' <param name="fechaInicio"></param>
    ''' <param name="fechaTermino"></param>
    ''' <param name="de"></param>
    ''' <param name="a"></param>
    ''' <param name="diferenciaDias"></param>
    ''' <param name="tasaCupon"></param>
    ''' <param name="base"></param>
    ''' <param name="diasPago"></param>
    ''' <param name="usuarioCreacion"></param>
    ''' <param name="fechaCreacion"></param>
    ''' <param name="horaCreacion"></param>
    ''' <param name="usuarioModificacion"></param>
    ''' <param name="fechaModificacion"></param>
    ''' <param name="horaModificacion"></param>
    ''' <param name="usuarioEliminacion"></param>
    ''' <param name="fechaEliminacion"></param>
    ''' <param name="horaEliminacion"></param>
    ''' <param name="host"></param>
    ''' <param name="codigoAmortizacion"></param>
    Public Function Modificar(ByVal codigoISIN As String, ByVal codigoNemonico As String, ByVal fechaInicio As Decimal, ByVal fechaTermino As Decimal, ByVal de As Decimal, ByVal a As Decimal, ByVal diferenciaDias As Decimal, ByVal tasaCupon As Decimal, ByVal base As Decimal, ByVal diasPago As Decimal, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String, ByVal host As String, ByVal codigoAmortizacion As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleCuponeraEspecial_Modificar")

        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, fechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaTermino", DbType.Decimal, fechaTermino)
        db.AddInParameter(dbCommand, "@p_De", DbType.Decimal, de)
        db.AddInParameter(dbCommand, "@p_A", DbType.Decimal, a)
        db.AddInParameter(dbCommand, "@p_DiferenciaDias", DbType.Decimal, diferenciaDias)
        db.AddInParameter(dbCommand, "@p_TasaCupon", DbType.Decimal, tasaCupon)
        db.AddInParameter(dbCommand, "@p_Base", DbType.Decimal, base)
        db.AddInParameter(dbCommand, "@p_DiasPago", DbType.Decimal, diasPago)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)
        db.AddInParameter(dbCommand, "@p_CodigoAmortizacion", DbType.String, codigoAmortizacion)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de DetalleCuponeraEspecial table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoISIN As String, ByVal codigoNemonico As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleCuponeraEspecial_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de DetalleCuponeraEspecial table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoAmortizacion(ByVal codigoAmortizacion As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleCuponeraEspecial_EliminarPorCodigoAmortizacion")

        db.AddInParameter(dbCommand, "@p_CodigoAmortizacion", DbType.String, codigoAmortizacion)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de DetalleCuponeraEspecial table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleCuponeraEspecial_EliminarPorCodigoISIN_CodigoNemonico")

        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
	End Class

