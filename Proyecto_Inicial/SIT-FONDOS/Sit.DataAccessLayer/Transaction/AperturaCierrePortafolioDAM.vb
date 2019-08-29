Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Data.SqlClient
Imports Sit.BusinessEntities



	''' <summary>
	''' Clase para el acceso de los datos para AperturaCierrePortafolio tabla.
	''' </summary>
	Public class AperturaCierrePortafolioDAM
	
    'Private oAperturaCierrePortafolioRow As AperturaCierrePortafolio.AperturaCierrePortafolioRow
	
		Private Sub New()

		End Sub

		''' <summary>
		''' Inserta un expediente en AperturaCierrePortafolio tabla.
		''' <summary>
		''' <param name="codigoAperturaCierre"></param>
		''' <param name="codigoPortafolio"></param>
		''' <param name="fecha"></param>
		''' <param name="estado"></param>
		''' <param name="usuario"></param>
		''' <param name="fechaOperacion"></param>
		''' <param name="horaOperacion"></param>
		''' <param name="host"></param>
		''' <param name="accion"></param>
		''' <returns></returns>
    'Public Function Insertar(ByVal ob As AperturaCierrePortafolio, ByRef dataRequest As DataSet) As String
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim dbCommand As dbCommand = db.GetStoredProcCommand("AperturaCierrePortafolio_Insertar")

    '    Dim Codigo As String = String.Empty


    '    oAperturaCierrePortafolioRow = CType(ob.AperturaCierrePortafolio.Rows(0), AperturaCierrePortafolio.AperturaCierrePortafolioRow)

    '    db.AddInParameter(dbCommand, "@p_CodigoAperturaCierre", DbType.String, oAperturaCierrePortafolioRow.CodigoAperturaCierre)
    '    db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, oAperturaCierrePortafolioRow.CodigoPortafolio)
    '    db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, oAperturaCierrePortafolioRow.Fecha)
    '    db.AddInParameter(dbCommand, "@p_Estado", DbType.String, oAperturaCierrePortafolioRow.Estado)
    '    db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
    '    db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, oAperturaCierrePortafolioRow.FechaOperacion)
    '    db.AddInParameter(dbCommand, "@p_HoraOperacion", DbType.String, oAperturaCierrePortafolioRow.HoraOperacion)
    '    db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
    '    db.AddInParameter(dbCommand, "@p_Accion", DbType.String, oAperturaCierrePortafolioRow.Accion)

    '    db.ExecuteNonQuery(dbCommand)
    '    Return Codigo = True

    '    Return Codigo = False

    '    Return Codigo = False

    'End Function

    ''' <summary>
    ''' Selecciona un solo expediente de AperturaCierrePortafolio tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    'Public Function Seleccionar(ByVal codigoAperturaCierre As String) As AperturaCierrePortafolio
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim dbCommand As dbCommand = db.GetStoredProcCommand("AperturaCierrePortafolio_Seleccionar")

    '    Dim objeto As New AperturaCierrePortafolio

    '    db.AddInParameter(dbCommand, "@p_CodigoAperturaCierre", DbType.String, codigoAperturaCierre)

    '    db.LoadDataSet(dbCommand, objeto, "AperturaCierrePortafolio")
    '    Return objeto

    '    Return Nothing

    '    Return Nothing

    'End Function

    ''' <summary>
    ''' Selecciona expedientes de AperturaCierrePortafolio tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoPortafolio"></param>
    ''' <returns>DataSet</returns>
    'Public Function SeleccionarPorCodigoPortafolio(ByVal codigoPortafolio As String) As AperturaCierrePortafolio

    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim dbCommand As dbCommand = db.GetStoredProcCommand("AperturaCierrePortafolio_SeleccionarPorCodigoPortafolio")

    '    Dim objeto As New AperturaCierrePortafolio


    '    db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)

    '    db.LoadDataSet(dbCommand, objeto, "AperturaCierrePortafolio")
    '    Return objeto

    '    Return Nothing

    '    Return Nothing

    'End Function

    ''' <summary>
    ''' Lista todos los expedientes de AperturaCierrePortafolio tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    'Public Function Listar() As AperturaCierrePortafolio
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim dbCommand As dbCommand = db.GetStoredProcCommand("AperturaCierrePortafolio_Listar")

    '    Dim objeto As New AperturaCierrePortafolio


    '    db.LoadDataSet(dbCommand, objeto, "AperturaCierrePortafolio")
    '    Return objeto

    '    Return Nothing

    '    Return Nothing

    'End Function

    ''' <summary>
    ''' Midifica un expediente en AperturaCierrePortafolio tabla.
    ''' <summary>
    ''' <param name="codigoAperturaCierre"></param>
    ''' <param name="codigoPortafolio"></param>
    ''' <param name="fecha"></param>
    ''' <param name="estado"></param>
    ''' <param name="usuario"></param>
    ''' <param name="fechaOperacion"></param>
    ''' <param name="horaOperacion"></param>
    ''' <param name="host"></param>
    ''' <param name="accion"></param>
    'Public Function Modificar(ByVal ob As AperturaCierrePortafolio, ByRef dataRequest As DataSet) As Boolean
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim dbCommand As dbCommand = db.GetStoredProcCommand("AperturaCierrePortafolio_Modificar")



    '    oAperturaCierrePortafolioRow = CType(ob.AperturaCierrePortafolio.Rows(0), AperturaCierrePortafolio.AperturaCierrePortafolioRow)

    '    db.AddInParameter(dbCommand, "@p_CodigoAperturaCierre", DbType.String, oAperturaCierrePortafolioRow.CodigoAperturaCierre)
    '    db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, oAperturaCierrePortafolioRow.CodigoPortafolio)
    '    db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, oAperturaCierrePortafolioRow.Fecha)
    '    db.AddInParameter(dbCommand, "@p_Estado", DbType.String, oAperturaCierrePortafolioRow.Estado)
    '    db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
    '    db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, oAperturaCierrePortafolioRow.FechaOperacion)
    '    db.AddInParameter(dbCommand, "@p_HoraOperacion", DbType.String, oAperturaCierrePortafolioRow.HoraOperacion)
    '    db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
    '    db.AddInParameter(dbCommand, "@p_Accion", DbType.String, oAperturaCierrePortafolioRow.Accion)

    '    db.ExecuteNonQuery(dbCommand)
    '    Return True





    'End Function

    ''' <summary>
    ''' Elimina un expediente de AperturaCierrePortafolio table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoAperturaCierre As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("AperturaCierrePortafolio_Eliminar")


        db.AddInParameter(dbCommand, "@p_CodigoAperturaCierre", DbType.String, codigoAperturaCierre)

        db.ExecuteNonQuery(dbCommand)
        Return True





    End Function

    ''' <summary>
    ''' Elimina un expediente de AperturaCierrePortafolio table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoPortafolio(ByVal codigoPortafolio As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("AperturaCierrePortafolio_EliminarPorCodigoPortafolio")


        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)

        db.ExecuteNonQuery(dbCommand)
        Return True





    End Function
End Class

