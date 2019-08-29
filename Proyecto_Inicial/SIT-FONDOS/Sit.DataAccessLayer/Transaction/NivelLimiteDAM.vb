Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql



	''' <summary>
	''' Clase para el acceso de los datos para NivelLimite tabla.
	''' </summary>
	Public class NivelLimiteDAM
	
		Public Sub New()

		End Sub

		''' <summary>
		''' Inserta un expediente en NivelLimite tabla.
		''' <summary>
		''' <param name="codigoNivel"></param>
		''' <param name="porcentaje"></param>
		''' <param name="descripcion"></param>
		''' <param name="usuarioCreacion"></param>
		''' <param name="fechaCreacion"></param>
		''' <param name="fechaModificacion"></param>
		''' <param name="horaCreacion"></param>
		''' <param name="host"></param>
		''' <param name="usuarioModificacion"></param>
		''' <param name="horaModificacion"></param>
		''' <param name="usuarioEliminacion"></param>
		''' <param name="fechaEliminacion"></param>
		''' <param name="horaEliminacion"></param>
		''' <returns></returns>
		Public  Function Insertar(ByVal codigoNivel As Decimal, ByVal porcentaje As Decimal, ByVal descripcion As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal fechaModificacion As Decimal, ByVal horaCreacion As String, ByVal host As String, ByVal usuarioModificacion As String, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String)as String 
			Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("NivelLimite_Insertar")

        db.AddInParameter(dbCommand, "@p_CodigoNivel", DbType.Decimal, codigoNivel)
        db.AddInParameter(dbCommand, "@p_Porcentaje", DbType.Decimal, porcentaje)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)

        db.ExecuteNonQuery(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona un solo expediente de NivelLimite tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Seleccionar(ByVal codigoNivel As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("NivelLimite_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoNivel", DbType.Decimal, codigoNivel)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Lista todos los expedientes de NivelLimite tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("NivelLimite_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Midifica un expediente en NivelLimite tabla.
    ''' <summary>
    ''' <param name="codigoNivel"></param>
    ''' <param name="porcentaje"></param>
    ''' <param name="descripcion"></param>
    ''' <param name="usuarioCreacion"></param>
    ''' <param name="fechaCreacion"></param>
    ''' <param name="fechaModificacion"></param>
    ''' <param name="horaCreacion"></param>
    ''' <param name="host"></param>
    ''' <param name="usuarioModificacion"></param>
    ''' <param name="horaModificacion"></param>
    ''' <param name="usuarioEliminacion"></param>
    ''' <param name="fechaEliminacion"></param>
    ''' <param name="horaEliminacion"></param>
    Public Function Modificar(ByVal codigoNivel As Decimal, ByVal porcentaje As Decimal, ByVal descripcion As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal fechaModificacion As Decimal, ByVal horaCreacion As String, ByVal host As String, ByVal usuarioModificacion As String, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("NivelLimite_Modificar")

        db.AddInParameter(dbCommand, "@p_CodigoNivel", DbType.Decimal, codigoNivel)
        db.AddInParameter(dbCommand, "@p_Porcentaje", DbType.Decimal, porcentaje)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de NivelLimite table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoNivel As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("NivelLimite_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoNivel", DbType.Decimal, codigoNivel)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
	End Class

