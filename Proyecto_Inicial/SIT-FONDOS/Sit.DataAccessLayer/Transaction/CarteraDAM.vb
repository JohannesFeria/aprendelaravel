Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

''' <summary>
	''' Clase para el acceso de los datos para Cartera tabla.
	''' </summary>
	Public class CarteraDAM
	
		Public Sub New()

		End Sub

    Public Function GeneraSaldosCarteraTitulo(ByVal fechaInicial As Decimal, ByVal fechaFin As Decimal, ByVal codigoPortafolioSBS As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("GeneraSaldosCarteraTitulo_Insertar")
            dbCommand.CommandTimeout = 1020  'HDG 20110905
            db.AddInParameter(dbCommand, "@p_FechaInicial", DbType.Decimal, fechaInicial)
            db.AddInParameter(dbCommand, "@p_FechaFinal", DbType.Decimal, fechaFin)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function

    ''' <summary>
    ''' Inserta un expediente en Cartera tabla.
    ''' <summary>
    ''' <param name="codigoCartera"></param>
    ''' <param name="fecha"></param>
    ''' <param name="codigoPortafolio"></param>
    ''' <returns></returns>
    Public Function Insertar(ByVal codigoCartera As Decimal, ByVal fecha As Decimal, ByVal codigoPortafolio As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Cartera_Insertar")

        db.AddInParameter(dbCommand, "@p_CodigoCartera", DbType.Decimal, codigoCartera)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)

        db.ExecuteNonQuery(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona un solo expediente de Cartera tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Seleccionar(ByVal codigoCartera As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Cartera_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoCartera", DbType.Decimal, codigoCartera)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de Cartera tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoPortafolio"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoPortafolio(ByVal codigoPortafolio As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Cartera_SeleccionarPorCodigoPortafolio")

        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Lista todos los expedientes de Cartera tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Cartera_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Midifica un expediente en Cartera tabla.
    ''' <summary>
    ''' <param name="codigoCartera"></param>
    ''' <param name="fecha"></param>
    ''' <param name="codigoPortafolio"></param>
    Public Function Modificar(ByVal codigoCartera As Decimal, ByVal fecha As Decimal, ByVal codigoPortafolio As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Cartera_Modificar")

        db.AddInParameter(dbCommand, "@p_CodigoCartera", DbType.Decimal, codigoCartera)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de Cartera table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoCartera As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Cartera_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoCartera", DbType.Decimal, codigoCartera)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de Cartera table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoPortafolio(ByVal codigoPortafolio As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Cartera_EliminarPorCodigoPortafolio")

        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
End Class

