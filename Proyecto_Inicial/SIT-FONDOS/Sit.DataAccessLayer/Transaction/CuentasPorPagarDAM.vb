Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql



	''' <summary>
	''' Clase para el acceso de los datos para CuentasPorPagar tabla.
	''' </summary>
	Public class CuentasPorPagarDAM
	
		Public Sub New()

		End Sub

		''' <summary>
		''' Inserta un expediente en CuentasPorPagar tabla.
		''' <summary>
		''' <param name="codigoClaseCuenta"></param>
		''' <param name="codigoMoneda"></param>
		''' <param name="codigoPortafolio"></param>
		''' <param name="codigoMercado"></param>
		''' <param name="codigoTipoOperacion"></param>
		''' <param name="numeroCuenta"></param>
		''' <param name="codigoCuenta"></param>
		''' <param name="referencia"></param>
		''' <param name="situacion"></param>
		''' <param name="importe"></param>
		''' <param name="fechaIngreso"></param>
		''' <param name="fechaOperacion"></param>
		''' <param name="fechaPago"></param>
		''' <param name="usuarioCreacion"></param>
		''' <param name="fechaCreacion"></param>
		''' <param name="host"></param>
		''' <param name="horaCreacion"></param>
		''' <param name="usuarioModificacion"></param>
		''' <param name="fechaModificacion"></param>
		''' <param name="horaModificacion"></param>
		''' <param name="usuarioEliminacion"></param>
		''' <param name="fechaEliminacion"></param>
		''' <param name="codigoOrden"></param>
		''' <param name="codigoOperacion"></param>
		''' <param name="tipoMovimiento"></param>
		''' <param name="horaEliminacion"></param>
		''' <returns></returns>
		Public  Function Insertar(ByVal codigoClaseCuenta As String, ByVal codigoMoneda As String, ByVal codigoPortafolio As String, ByVal codigoMercado As String, ByVal codigoTipoOperacion As String, ByVal numeroCuenta As Decimal, ByVal codigoCuenta As String, ByVal referencia As String, ByVal situacion As String, ByVal importe As Decimal, ByVal fechaIngreso As Decimal, ByVal fechaOperacion As Decimal, ByVal fechaPago As Decimal, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal host As String, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal codigoOrden As String, ByVal codigoOperacion As String, ByVal tipoMovimiento As String, ByVal horaEliminacion As String)as String 
			Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorPagar_Insertar")

        db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, codigoClaseCuenta)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)
        db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.Decimal, numeroCuenta)
        db.AddInParameter(dbCommand, "@p_CodigoCuenta", DbType.String, codigoCuenta)
        db.AddInParameter(dbCommand, "@p_Referencia", DbType.String, referencia)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, importe)
        db.AddInParameter(dbCommand, "@p_FechaIngreso", DbType.Decimal, fechaIngreso)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaOperacion)
        db.AddInParameter(dbCommand, "@p_FechaPago", DbType.Decimal, fechaPago)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codigoOperacion)
        db.AddInParameter(dbCommand, "@p_TipoMovimiento", DbType.String, tipoMovimiento)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)

        db.ExecuteNonQuery(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona un solo expediente de CuentasPorPagar tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Seleccionar(ByVal codigoCuenta As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorPagar_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoCuenta", DbType.String, codigoCuenta)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de CuentasPorPagar tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoClaseCuenta"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoClaseCuenta(ByVal codigoClaseCuenta As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorPagar_SeleccionarPorCodigoClaseCuenta")

        db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, codigoClaseCuenta)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de CuentasPorPagar tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoMercado"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoMercado(ByVal codigoMercado As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorPagar_SeleccionarPorCodigoMercado")

        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de CuentasPorPagar tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoTipoOperacion"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoTipoOperacion(ByVal codigoTipoOperacion As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorPagar_SeleccionarPorCodigoTipoOperacion")

        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de CuentasPorPagar tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoOperacion"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoOperacion(ByVal codigoOperacion As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorPagar_SeleccionarPorCodigoOperacion")

        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codigoOperacion)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de CuentasPorPagar tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoPortafolio"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoPortafolio(ByVal codigoPortafolio As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorPagar_SeleccionarPorCodigoPortafolio")

        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de CuentasPorPagar tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoMoneda"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoMoneda(ByVal codigoMoneda As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorPagar_SeleccionarPorCodigoMoneda")

        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de CuentasPorPagar tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoOrden"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoOrden(ByVal codigoOrden As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorPagar_SeleccionarPorCodigoOrden")

        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Lista todos los expedientes de CuentasPorPagar tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorPagar_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Midifica un expediente en CuentasPorPagar tabla.
    ''' <summary>
    ''' <param name="codigoClaseCuenta"></param>
    ''' <param name="codigoMoneda"></param>
    ''' <param name="codigoPortafolio"></param>
    ''' <param name="codigoMercado"></param>
    ''' <param name="codigoTipoOperacion"></param>
    ''' <param name="numeroCuenta"></param>
    ''' <param name="codigoCuenta"></param>
    ''' <param name="referencia"></param>
    ''' <param name="situacion"></param>
    ''' <param name="importe"></param>
    ''' <param name="fechaIngreso"></param>
    ''' <param name="fechaOperacion"></param>
    ''' <param name="fechaPago"></param>
    ''' <param name="usuarioCreacion"></param>
    ''' <param name="fechaCreacion"></param>
    ''' <param name="host"></param>
    ''' <param name="horaCreacion"></param>
    ''' <param name="usuarioModificacion"></param>
    ''' <param name="fechaModificacion"></param>
    ''' <param name="horaModificacion"></param>
    ''' <param name="usuarioEliminacion"></param>
    ''' <param name="fechaEliminacion"></param>
    ''' <param name="codigoOrden"></param>
    ''' <param name="codigoOperacion"></param>
    ''' <param name="tipoMovimiento"></param>
    ''' <param name="horaEliminacion"></param>
    Public Function Modificar(ByVal codigoClaseCuenta As String, ByVal codigoMoneda As String, ByVal codigoPortafolio As String, ByVal codigoMercado As String, ByVal codigoTipoOperacion As String, ByVal numeroCuenta As Decimal, ByVal codigoCuenta As String, ByVal referencia As String, ByVal situacion As String, ByVal importe As Decimal, ByVal fechaIngreso As Decimal, ByVal fechaOperacion As Decimal, ByVal fechaPago As Decimal, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal host As String, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal codigoOrden As String, ByVal codigoOperacion As String, ByVal tipoMovimiento As String, ByVal horaEliminacion As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorPagar_Modificar")

        db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, codigoClaseCuenta)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)
        db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.Decimal, numeroCuenta)
        db.AddInParameter(dbCommand, "@p_CodigoCuenta", DbType.String, codigoCuenta)
        db.AddInParameter(dbCommand, "@p_Referencia", DbType.String, referencia)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, importe)
        db.AddInParameter(dbCommand, "@p_FechaIngreso", DbType.Decimal, fechaIngreso)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaOperacion)
        db.AddInParameter(dbCommand, "@p_FechaPago", DbType.Decimal, fechaPago)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codigoOperacion)
        db.AddInParameter(dbCommand, "@p_TipoMovimiento", DbType.String, tipoMovimiento)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de CuentasPorPagar table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoCuenta As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorPagar_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoCuenta", DbType.String, codigoCuenta)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de CuentasPorPagar table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoClaseCuenta(ByVal codigoClaseCuenta As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorPagar_EliminarPorCodigoClaseCuenta")

        db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, codigoClaseCuenta)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de CuentasPorPagar table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoMercado(ByVal codigoMercado As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorPagar_EliminarPorCodigoMercado")

        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de CuentasPorPagar table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoTipoOperacion(ByVal codigoTipoOperacion As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorPagar_EliminarPorCodigoTipoOperacion")

        db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de CuentasPorPagar table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoOperacion(ByVal codigoOperacion As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorPagar_EliminarPorCodigoOperacion")

        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codigoOperacion)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de CuentasPorPagar table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoPortafolio(ByVal codigoPortafolio As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorPagar_EliminarPorCodigoPortafolio")

        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de CuentasPorPagar table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoMoneda(ByVal codigoMoneda As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorPagar_EliminarPorCodigoMoneda")

        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de CuentasPorPagar table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoOrden(ByVal codigoOrden As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentasPorPagar_EliminarPorCodigoOrden")

        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
	End Class

