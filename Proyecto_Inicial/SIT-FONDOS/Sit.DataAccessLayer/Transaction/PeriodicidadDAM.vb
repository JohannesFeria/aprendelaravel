Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql



	''' <summary>
	''' Clase para el acceso de los datos para Periodicidad tabla.
	''' </summary>
	Public class PeriodicidadDAM
    Private sqlCommand As String = ""
    Private oRow As PeriodicidadBE.PeriodicidadRow
		Public Sub New()

		End Sub
#Region " /* Funciones Seleccionar */ "



    Public Function SeleccionarPorFiltros(ByVal codigoPeriodicidad As String, ByVal descripcion As String, ByVal dataRequest As DataSet) As PeriodicidadBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Periodicidad_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoPeriodicidad", DbType.String, codigoPeriodicidad)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)



        Dim oPeriodicidadBE As New PeriodicidadBE
        db.LoadDataSet(dbCommand, oPeriodicidadBE, "Periodicidad")
        Return oPeriodicidadBE

    End Function
    Public Function SeleccionarPorFiltros(ByVal descripcion As String, ByVal dias As Integer, ByVal situacion As String, ByVal dataRequest As DataSet) As PeriodicidadBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Periodicidad_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_DiasPeriodo", DbType.Decimal, dias)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        Dim oPeriodicidadBE As New PeriodicidadBE
        db.LoadDataSet(dbCommand, oPeriodicidadBE, "Periodicidad")
        Return oPeriodicidadBE

    End Function
    Public Function Seleccionar(ByVal codigoPeriodicidad As String, ByVal dataRequest As DataSet) As PeriodicidadBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Periodicidad_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoPeriodicidad", DbType.String, codigoPeriodicidad)
        Dim oPeriodicidadBE As New PeriodicidadBE
        db.LoadDataSet(dbCommand, oPeriodicidadBE, "Periodicidad")
        Return oPeriodicidadBE

    End Function
    ''' <summary>
    ''' Lista todos los expedientes de PeriodicidadBE tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar(ByVal dataRequest As DataSet) As PeriodicidadBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Periodicidad_Listar")

        Dim oPeriodicidadBE As New PeriodicidadBE
        db.LoadDataSet(dbCommand, oPeriodicidadBE, "Periodicidad")
        Return oPeriodicidadBE

    End Function
#End Region

#Region " /* Funciones Insertar */ "

    Public Function Insertar(ByVal oPeriodicidadBE As PeriodicidadBE, ByVal dataRequest As DataSet) As String

        Dim Codigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Periodicidad_Insertar")

        oRow = CType(oPeriodicidadBE.Periodicidad.Rows(0), PeriodicidadBE.PeriodicidadRow)

        db.AddInParameter(dbCommand, "@p_CodigoPeriodicidad", DbType.String, oRow.CodigoPeriodicidad)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_DiasPeriodo", DbType.Decimal, oRow.DiasPeriodo)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return Codigo

    End Function

#End Region

#Region " /* Funciones Modificar */"

    Public Function Modificar(ByVal oPeriodicidadBE As PeriodicidadBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Periodicidad_Modificar")
        oRow = CType(oPeriodicidadBE.Periodicidad.Rows(0), PeriodicidadBE.PeriodicidadRow)

        db.AddInParameter(dbCommand, "@p_CodigoPeriodicidad", DbType.String, oRow.CodigoPeriodicidad)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_DiasPeriodo", DbType.Decimal, oRow.DiasPeriodo)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function


#End Region

#Region " /* Funciones Eliminar */"

    Public Function Eliminar(ByVal codigoPeriodicidad As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Periodicidad_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoPeriodicidad", DbType.String, codigoPeriodicidad)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

#End Region

#Region " /* Funciones Personalizadas*/"

#End Region


End Class

