Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql



	''' <summary>
	''' Clase para el acceso de los datos para CaracteristicaLimiteNivel tabla.
	''' </summary>
	Public class CaracteristicaLimiteNivelDAM
	
		Public Sub New()

		End Sub

		''' <summary>
		''' Inserta un expediente en CaracteristicaLimiteNivel tabla.
		''' <summary>
		''' <param name="codigoNivel"></param>
		''' <param name="codigoLimite"></param>
		''' <returns></returns>
		Public  Function Insertar(ByVal codigoNivel As Decimal, ByVal codigoLimite As String)as String 
			Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CaracteristicaLimiteNivel_Insertar")

        db.AddInParameter(dbCommand, "@p_CodigoNivel", DbType.Decimal, codigoNivel)
        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, codigoLimite)

        db.ExecuteNonQuery(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de CaracteristicaLimiteNivel tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoNivel"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoNivel(ByVal codigoNivel As Decimal) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CaracteristicaLimiteNivel_SeleccionarPorCodigoNivel")

        db.AddInParameter(dbCommand, "@p_CodigoNivel", DbType.Decimal, codigoNivel)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de CaracteristicaLimiteNivel tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoLimite"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoLimite(ByVal codigoLimite As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CaracteristicaLimiteNivel_SeleccionarPorCodigoLimite")

        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, codigoLimite)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Elimina un expediente de CaracteristicaLimiteNivel table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoNivel As Decimal, ByVal codigoLimite As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CaracteristicaLimiteNivel_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoNivel", DbType.Decimal, codigoNivel)
        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, codigoLimite)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de CaracteristicaLimiteNivel table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoNivel(ByVal codigoNivel As Decimal) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CaracteristicaLimiteNivel_EliminarPorCodigoNivel")

        db.AddInParameter(dbCommand, "@p_CodigoNivel", DbType.Decimal, codigoNivel)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de CaracteristicaLimiteNivel table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoLimite(ByVal codigoLimite As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CaracteristicaLimiteNivel_EliminarPorCodigoLimite")

        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, codigoLimite)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
	End Class

