Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql



	''' <summary>
	''' Clase para el acceso de los datos para MovimientosCartaInstruccion tabla.
	''' </summary>
	Public class MovimientosCartaInstruccionDAM
	
		Public Sub New()

		End Sub

		''' <summary>
		''' Inserta un expediente en MovimientosCartaInstruccion tabla.
		''' <summary>
		''' <param name="numeroCarta"></param>
		''' <param name="importe"></param>
		''' <param name="numeroOperacion"></param>
		''' <param name="numeroCuenta"></param>
		''' <param name="fechaRegistro"></param>
		''' <param name="horaRegistro"></param>
		''' <param name="estado"></param>
		''' <param name="situacion"></param>
		''' <param name="codigoModelo"></param>
		''' <param name="codigoTipoOperacion"></param>
		''' <param name="codigoTerceroSBS"></param>
		''' <param name="codigoPortafolio"></param>
		''' <param name="codigoRenta"></param>
		''' <param name="codigoModalidadPago"></param>
		''' <param name="codigoMoneda"></param>
		''' <param name="descripcion"></param>
		''' <param name="codigoNegocio"></param>
		''' <param name="codigoClaseCuenta"></param>
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
		''' <param name="codigoContacto"></param>
		''' <param name="codigoIntermediario"></param>
		''' <returns></returns>
		Public  Function Insertar(ByVal numeroCarta As String, ByVal importe As Decimal, ByVal numeroOperacion As String, ByVal numeroCuenta As String, ByVal fechaRegistro As Decimal, ByVal horaRegistro As String, ByVal estado As String, ByVal situacion As String, ByVal codigoModelo As String, ByVal codigoTipoOperacion As String, ByVal codigoTerceroSBS As Decimal, ByVal codigoPortafolio As String, ByVal codigoRenta As String, ByVal codigoModalidadPago As String, ByVal codigoMoneda As String, ByVal descripcion As String, ByVal codigoNegocio As String, ByVal codigoClaseCuenta As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String, ByVal host As String, ByVal codigoContacto As String, ByVal codigoIntermediario As String)as String 
			Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientosCartaInstruccion_Insertar")

        db.AddInParameter(dbCommand, "@p_NumeroCarta", DbType.String, numeroCarta)
        db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, importe)
        db.AddInParameter(dbCommand, "@p_NumeroOperacion", DbType.String, numeroOperacion)
        db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, numeroCuenta)
        db.AddInParameter(dbCommand, "@p_FechaRegistro", DbType.Decimal, fechaRegistro)
        db.AddInParameter(dbCommand, "@p_HoraRegistro", DbType.String, horaRegistro)
        db.AddInParameter(dbCommand, "@p_Estado", DbType.String, estado)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, codigoModelo)
        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoTerceroSBS", DbType.Decimal, codigoTerceroSBS)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, codigoRenta)
        db.AddInParameter(dbCommand, "@p_CodigoModalidadPago", DbType.String, codigoModalidadPago)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, codigoNegocio)
        db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, codigoClaseCuenta)
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
        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, codigoContacto)
        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, codigoIntermediario)

        db.ExecuteNonQuery(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona un solo expediente de MovimientosCartaInstruccion tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Seleccionar(ByVal numeroCarta As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientosCartaInstruccion_Seleccionar")

        db.AddInParameter(dbCommand, "@p_NumeroCarta", DbType.String, numeroCarta)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de MovimientosCartaInstruccion tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoNegocio"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoNegocio(ByVal codigoNegocio As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientosCartaInstruccion_SeleccionarPorCodigoNegocio")

        db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, codigoNegocio)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de MovimientosCartaInstruccion tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoMoneda"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoMoneda(ByVal codigoMoneda As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientosCartaInstruccion_SeleccionarPorCodigoMoneda")

        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de MovimientosCartaInstruccion tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoTerceroSBS"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoTerceroSBS(ByVal codigoTerceroSBS As Decimal) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientosCartaInstruccion_SeleccionarPorCodigoTerceroSBS")

        db.AddInParameter(dbCommand, "@p_CodigoTerceroSBS", DbType.Decimal, codigoTerceroSBS)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de MovimientosCartaInstruccion tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoRenta"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoRenta(ByVal codigoRenta As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientosCartaInstruccion_SeleccionarPorCodigoRenta")

        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, codigoRenta)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de MovimientosCartaInstruccion tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoIntermediario"></param>
    ''' <param name="codigoContacto"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoIntermediario_CodigoContacto(ByVal codigoIntermediario As String, ByVal codigoContacto As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientosCartaInstruccion_SeleccionarPorCodigoIntermediario_CodigoContacto")

        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, codigoIntermediario)
        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, codigoContacto)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de MovimientosCartaInstruccion tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoModalidadPago"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoModalidadPago(ByVal codigoModalidadPago As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientosCartaInstruccion_SeleccionarPorCodigoModalidadPago")

        db.AddInParameter(dbCommand, "@p_CodigoModalidadPago", DbType.String, codigoModalidadPago)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de MovimientosCartaInstruccion tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoPortafolio"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoPortafolio(ByVal codigoPortafolio As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientosCartaInstruccion_SeleccionarPorCodigoPortafolio")

        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de MovimientosCartaInstruccion tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoModelo"></param>
    ''' <param name="codigoTipoOperacion"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoModelo_CodigoTipoOperacion(ByVal codigoModelo As String, ByVal codigoTipoOperacion As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientosCartaInstruccion_SeleccionarPorCodigoModelo_CodigoTipoOperacion")

        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, codigoModelo)
        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de MovimientosCartaInstruccion tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoClaseCuenta"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoClaseCuenta(ByVal codigoClaseCuenta As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientosCartaInstruccion_SeleccionarPorCodigoClaseCuenta")

        db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, codigoClaseCuenta)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Lista todos los expedientes de MovimientosCartaInstruccion tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientosCartaInstruccion_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Midifica un expediente en MovimientosCartaInstruccion tabla.
    ''' <summary>
    ''' <param name="numeroCarta"></param>
    ''' <param name="importe"></param>
    ''' <param name="numeroOperacion"></param>
    ''' <param name="numeroCuenta"></param>
    ''' <param name="fechaRegistro"></param>
    ''' <param name="horaRegistro"></param>
    ''' <param name="estado"></param>
    ''' <param name="situacion"></param>
    ''' <param name="codigoModelo"></param>
    ''' <param name="codigoTipoOperacion"></param>
    ''' <param name="codigoTerceroSBS"></param>
    ''' <param name="codigoPortafolio"></param>
    ''' <param name="codigoRenta"></param>
    ''' <param name="codigoModalidadPago"></param>
    ''' <param name="codigoMoneda"></param>
    ''' <param name="descripcion"></param>
    ''' <param name="codigoNegocio"></param>
    ''' <param name="codigoClaseCuenta"></param>
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
    ''' <param name="codigoContacto"></param>
    ''' <param name="codigoIntermediario"></param>
    Public Function Modificar(ByVal numeroCarta As String, ByVal importe As Decimal, ByVal numeroOperacion As String, ByVal numeroCuenta As String, ByVal fechaRegistro As Decimal, ByVal horaRegistro As String, ByVal estado As String, ByVal situacion As String, ByVal codigoModelo As String, ByVal codigoTipoOperacion As String, ByVal codigoTerceroSBS As Decimal, ByVal codigoPortafolio As String, ByVal codigoRenta As String, ByVal codigoModalidadPago As String, ByVal codigoMoneda As String, ByVal descripcion As String, ByVal codigoNegocio As String, ByVal codigoClaseCuenta As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String, ByVal host As String, ByVal codigoContacto As String, ByVal codigoIntermediario As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientosCartaInstruccion_Modificar")

        db.AddInParameter(dbCommand, "@p_NumeroCarta", DbType.String, numeroCarta)
        db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, importe)
        db.AddInParameter(dbCommand, "@p_NumeroOperacion", DbType.String, numeroOperacion)
        db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, numeroCuenta)
        db.AddInParameter(dbCommand, "@p_FechaRegistro", DbType.Decimal, fechaRegistro)
        db.AddInParameter(dbCommand, "@p_HoraRegistro", DbType.String, horaRegistro)
        db.AddInParameter(dbCommand, "@p_Estado", DbType.String, estado)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, codigoModelo)
        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoTerceroSBS", DbType.Decimal, codigoTerceroSBS)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, codigoRenta)
        db.AddInParameter(dbCommand, "@p_CodigoModalidadPago", DbType.String, codigoModalidadPago)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, codigoNegocio)
        db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, codigoClaseCuenta)
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
        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, codigoContacto)
        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, codigoIntermediario)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de MovimientosCartaInstruccion table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal numeroCarta As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientosCartaInstruccion_Eliminar")

        db.AddInParameter(dbCommand, "@p_NumeroCarta", DbType.String, numeroCarta)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de MovimientosCartaInstruccion table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoNegocio(ByVal codigoNegocio As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientosCartaInstruccion_EliminarPorCodigoNegocio")

        db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, codigoNegocio)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de MovimientosCartaInstruccion table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoMoneda(ByVal codigoMoneda As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientosCartaInstruccion_EliminarPorCodigoMoneda")

        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de MovimientosCartaInstruccion table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoTerceroSBS(ByVal codigoTerceroSBS As Decimal) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientosCartaInstruccion_EliminarPorCodigoTerceroSBS")

        db.AddInParameter(dbCommand, "@p_CodigoTerceroSBS", DbType.Decimal, codigoTerceroSBS)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de MovimientosCartaInstruccion table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoRenta(ByVal codigoRenta As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientosCartaInstruccion_EliminarPorCodigoRenta")

        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, codigoRenta)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de MovimientosCartaInstruccion table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoIntermediario_CodigoContacto(ByVal codigoIntermediario As String, ByVal codigoContacto As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientosCartaInstruccion_EliminarPorCodigoIntermediario_CodigoContacto")

        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, codigoIntermediario)
        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, codigoContacto)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de MovimientosCartaInstruccion table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoModalidadPago(ByVal codigoModalidadPago As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientosCartaInstruccion_EliminarPorCodigoModalidadPago")

        db.AddInParameter(dbCommand, "@p_CodigoModalidadPago", DbType.String, codigoModalidadPago)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de MovimientosCartaInstruccion table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoPortafolio(ByVal codigoPortafolio As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientosCartaInstruccion_EliminarPorCodigoPortafolio")

        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de MovimientosCartaInstruccion table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoModelo_CodigoTipoOperacion(ByVal codigoModelo As String, ByVal codigoTipoOperacion As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientosCartaInstruccion_EliminarPorCodigoModelo_CodigoTipoOperacion")

        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, codigoModelo)
        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de MovimientosCartaInstruccion table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoClaseCuenta(ByVal codigoClaseCuenta As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MovimientosCartaInstruccion_EliminarPorCodigoClaseCuenta")

        db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, codigoClaseCuenta)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
	End Class

