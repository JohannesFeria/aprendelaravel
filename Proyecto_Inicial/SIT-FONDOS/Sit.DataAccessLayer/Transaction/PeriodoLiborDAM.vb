Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

	Public class PeriodoLiborDAM
        Private sqlCommand As String = ""
    Private oRow As PeriodoLiborBE.PeriodoLiborRow
		Public Sub New()

		End Sub

#Region " /* Funciones Seleccionar */ "



    Public Function SeleccionarPorFiltro(ByVal codigoPeriodoLibor As String, ByVal descripcion As String, ByVal dataRequest As DataSet) As PeriodoLiborBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PeriodoLibor_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoPeriodoLibor", DbType.String, codigoPeriodoLibor)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)



        Dim oPeriodoLiborBE As New PeriodoLiborBE
        db.LoadDataSet(dbCommand, oPeriodoLiborBE, "PeriodoLibor")
        Return oPeriodoLiborBE

    End Function
    Public Function SeleccionarPorFiltro(ByVal codigoPeriodoLibor As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As PeriodoLiborBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PeriodoLibor_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoPeriodoLibor", DbType.String, codigoPeriodoLibor)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)



        Dim oPeriodoLiborBE As New PeriodoLiborBE
        db.LoadDataSet(dbCommand, oPeriodoLiborBE, "PeriodoLibor")
        Return oPeriodoLiborBE

    End Function
    Public Function Seleccionar(ByVal codigoPeriodoLibor As String, ByVal dataRequest As DataSet) As PeriodoLiborBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PeriodoLibor_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoPeriodoLibor", DbType.String, codigoPeriodoLibor)
        Dim oPeriodoLiborBE As New PeriodoLiborBE
        db.LoadDataSet(dbCommand, oPeriodoLiborBE, "PeriodoLibor")
        Return oPeriodoLiborBE

    End Function
    ''' <summary>
    ''' Lista todos los expedientes de PeriodoLiborBE tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar(ByVal dataRequest As DataSet) As PeriodoLiborBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PeriodoLibor_Listar")

        Dim oPeriodoLiborBE As New PeriodoLiborBE
        db.LoadDataSet(dbCommand, oPeriodoLiborBE, "PeriodoLibor")
        Return oPeriodoLiborBE

    End Function
#End Region

#Region " /* Funciones Insertar */ "

    Public Function Insertar(ByVal oPeriodoLiborBE As PeriodoLiborBE, ByVal dataRequest As DataSet) As String

        Dim Codigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PeriodoLibor_Insertar")

        oRow = CType(oPeriodoLiborBE.PeriodoLibor.Rows(0), PeriodoLiborBE.PeriodoLiborRow)

        db.AddInParameter(dbCommand, "@p_CodigoPeriodoLibor", DbType.String, oRow.CodigoPeriodoLibor)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return Codigo

    End Function

#End Region

#Region " /* Funciones Modificar */"

    Public Function Modificar(ByVal oPeriodoLiborBE As PeriodoLiborBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PeriodoLibor_Modificar")
        oRow = CType(oPeriodoLiborBE.PeriodoLibor.Rows(0), PeriodoLiborBE.PeriodoLiborRow)

        db.AddInParameter(dbCommand, "@p_CodigoPeriodoLibor", DbType.String, oRow.CodigoPeriodoLibor)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
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

    Public Function Eliminar(ByVal codigoPeriodoLibor As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PeriodoLibor_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoPeriodoLibor", DbType.String, codigoPeriodoLibor)
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

