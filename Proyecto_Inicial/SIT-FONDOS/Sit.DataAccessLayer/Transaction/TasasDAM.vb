Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql



	''' <summary>
	''' Clase para el acceso de los datos para Tasas tabla.
	''' </summary>
	Public class TasasDAM
	
		Public Sub New()

		End Sub

		''' <summary>
		''' Inserta un expediente en Tasas tabla.
		''' <summary>
		''' <param name="codigoTasa"></param>
		''' <param name="codigoEmisor"></param>
		''' <param name="codigoMercado"></param>
		''' <param name="codigoCalificacion"></param>
		''' <param name="vigencia"></param>
		''' <param name="tasaEncaje"></param>
		''' <param name="referencia"></param>
		''' <param name="usuarioCreacion"></param>
		''' <param name="fechaCreacion"></param>
		''' <param name="horaCreacion"></param>
		''' <param name="codigoMoneda"></param>
		''' <param name="codigoISIN"></param>
		''' <param name="usuarioEliminacion"></param>
		''' <param name="codigoNemonico"></param>
		''' <param name="fechaEliminacion"></param>
		''' <param name="horaModificacion"></param>
		''' <param name="horaEliminacion"></param>
		''' <param name="usuarioModificacion"></param>
		''' <param name="host"></param>
		''' <param name="fechaModificacion"></param>
		''' <returns></returns>
		Public  Function Insertar(ByVal codigoTasa As String, ByVal codigoEmisor As Decimal, ByVal codigoMercado As String, ByVal codigoCalificacion As String, ByVal vigencia As Decimal, ByVal tasaEncaje As Decimal, ByVal referencia As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal codigoMoneda As String, ByVal codigoISIN As String, ByVal usuarioEliminacion As String, ByVal codigoNemonico As String, ByVal fechaEliminacion As Decimal, ByVal horaModificacion As String, ByVal horaEliminacion As String, ByVal usuarioModificacion As String, ByVal host As String, ByVal fechaModificacion As Decimal)as String 
			Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Tasas_Insertar")

        db.AddInParameter(dbCommand, "@p_CodigoTasa", DbType.String, codigoTasa)
        db.AddInParameter(dbCommand, "@p_CodigoEmisor", DbType.Decimal, codigoEmisor)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
        db.AddInParameter(dbCommand, "@p_CodigoCalificacion", DbType.String, codigoCalificacion)
        db.AddInParameter(dbCommand, "@p_Vigencia", DbType.Decimal, vigencia)
        db.AddInParameter(dbCommand, "@p_TasaEncaje", DbType.Decimal, tasaEncaje)
        db.AddInParameter(dbCommand, "@p_Referencia", DbType.String, referencia)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)

        db.ExecuteNonQuery(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona un solo expediente de Tasas tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Seleccionar(ByVal codigoTasa As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Tasas_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoTasa", DbType.String, codigoTasa)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de Tasas tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoMoneda"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoMoneda(ByVal codigoMoneda As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Tasas_SeleccionarPorCodigoMoneda")

        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de Tasas tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoEmisor"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoEmisor(ByVal codigoEmisor As Decimal) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Tasas_SeleccionarPorCodigoEmisor")

        db.AddInParameter(dbCommand, "@p_CodigoEmisor", DbType.Decimal, codigoEmisor)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de Tasas tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoISIN"></param>
    ''' <param name="codigoNemonico"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Tasas_SeleccionarPorCodigoISIN_CodigoNemonico")

        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de Tasas tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoMercado"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoMercado(ByVal codigoMercado As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Tasas_SeleccionarPorCodigoMercado")

        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de Tasas tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoCalificacion"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoCalificacion(ByVal codigoCalificacion As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Tasas_SeleccionarPorCodigoCalificacion")

        db.AddInParameter(dbCommand, "@p_CodigoCalificacion", DbType.String, codigoCalificacion)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Lista todos los expedientes de Tasas tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Tasas_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Midifica un expediente en Tasas tabla.
    ''' <summary>
    ''' <param name="codigoTasa"></param>
    ''' <param name="codigoEmisor"></param>
    ''' <param name="codigoMercado"></param>
    ''' <param name="codigoCalificacion"></param>
    ''' <param name="vigencia"></param>
    ''' <param name="tasaEncaje"></param>
    ''' <param name="referencia"></param>
    ''' <param name="usuarioCreacion"></param>
    ''' <param name="fechaCreacion"></param>
    ''' <param name="horaCreacion"></param>
    ''' <param name="codigoMoneda"></param>
    ''' <param name="codigoISIN"></param>
    ''' <param name="usuarioEliminacion"></param>
    ''' <param name="codigoNemonico"></param>
    ''' <param name="fechaEliminacion"></param>
    ''' <param name="horaModificacion"></param>
    ''' <param name="horaEliminacion"></param>
    ''' <param name="usuarioModificacion"></param>
    ''' <param name="host"></param>
    ''' <param name="fechaModificacion"></param>
    Public Function Modificar(ByVal codigoTasa As String, ByVal codigoEmisor As Decimal, ByVal codigoMercado As String, ByVal codigoCalificacion As String, ByVal vigencia As Decimal, ByVal tasaEncaje As Decimal, ByVal referencia As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal codigoMoneda As String, ByVal codigoISIN As String, ByVal usuarioEliminacion As String, ByVal codigoNemonico As String, ByVal fechaEliminacion As Decimal, ByVal horaModificacion As String, ByVal horaEliminacion As String, ByVal usuarioModificacion As String, ByVal host As String, ByVal fechaModificacion As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Tasas_Modificar")

        db.AddInParameter(dbCommand, "@p_CodigoTasa", DbType.String, codigoTasa)
        db.AddInParameter(dbCommand, "@p_CodigoEmisor", DbType.Decimal, codigoEmisor)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
        db.AddInParameter(dbCommand, "@p_CodigoCalificacion", DbType.String, codigoCalificacion)
        db.AddInParameter(dbCommand, "@p_Vigencia", DbType.Decimal, vigencia)
        db.AddInParameter(dbCommand, "@p_TasaEncaje", DbType.Decimal, tasaEncaje)
        db.AddInParameter(dbCommand, "@p_Referencia", DbType.String, referencia)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de Tasas table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoTasa As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Tasas_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoTasa", DbType.String, codigoTasa)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de Tasas table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoMoneda(ByVal codigoMoneda As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Tasas_EliminarPorCodigoMoneda")

        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de Tasas table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoEmisor(ByVal codigoEmisor As Decimal) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Tasas_EliminarPorCodigoEmisor")

        db.AddInParameter(dbCommand, "@p_CodigoEmisor", DbType.Decimal, codigoEmisor)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de Tasas table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Tasas_EliminarPorCodigoISIN_CodigoNemonico")

        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de Tasas table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoMercado(ByVal codigoMercado As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Tasas_EliminarPorCodigoMercado")

        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de Tasas table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoCalificacion(ByVal codigoCalificacion As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Tasas_EliminarPorCodigoCalificacion")

        db.AddInParameter(dbCommand, "@p_CodigoCalificacion", DbType.String, codigoCalificacion)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
	End Class

