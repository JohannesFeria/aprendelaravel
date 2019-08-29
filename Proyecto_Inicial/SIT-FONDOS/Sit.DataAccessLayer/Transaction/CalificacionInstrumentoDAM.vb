Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

	Public class CalificacionInstrumentoDAM
        Private sqlCommand As String = ""
    Private oRow As CalificacionInstrumentoBE.CalificacionInstrumentoRow
    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "


    Public Function SeleccionarPorFiltro(ByVal descripcion As String, ByVal plazo As String, ByVal situacion As String, ByVal dataRequest As DataSet) As CalificacionInstrumentoBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CalificacionInstrumento_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Plazo", DbType.String, plazo)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        Dim oCalificacionInstrumentoBE As New CalificacionInstrumentoBE
        db.LoadDataSet(dbCommand, oCalificacionInstrumentoBE, "CalificacionInstrumento")
        Return oCalificacionInstrumentoBE
    End Function
    Public Function SeleccionarPorFiltro(ByVal codigoCalificacionInstrumento As String, ByVal descripcion As String, ByVal dataRequest As DataSet) As CalificacionInstrumentoBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CalificacionInstrumento_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoCalificacionInstrumento", DbType.String, codigoCalificacionInstrumento)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)



        Dim oCalificacionInstrumentoBE As New CalificacionInstrumentoBE
        db.LoadDataSet(dbCommand, oCalificacionInstrumentoBE, "CalificacionInstrumento")
        Return oCalificacionInstrumentoBE

    End Function
    Public Function SeleccionarPorFiltro(ByVal descripcion As String, ByVal tipoRenta As String, ByVal situacion As String, ByVal maduracion As String, ByVal dataRequest As DataSet) As CalificacionInstrumentoBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CalificacionInstrumento_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_Maduracion", DbType.String, maduracion)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, tipoRenta)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)



        Dim oCalificacionInstrumentoBE As New CalificacionInstrumentoBE
        db.LoadDataSet(dbCommand, oCalificacionInstrumentoBE, "CalificacionInstrumento")
        Return oCalificacionInstrumentoBE

    End Function
    Public Function Seleccionar(ByVal codigoCalificacionInstrumento As String, ByVal dataRequest As DataSet) As CalificacionInstrumentoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CalificacionInstrumento_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoCalificacionInstrumento", DbType.String, codigoCalificacionInstrumento)
        Dim oCalificacionInstrumentoBE As New CalificacionInstrumentoBE
        db.LoadDataSet(dbCommand, oCalificacionInstrumentoBE, "CalificacionInstrumento")
        Return oCalificacionInstrumentoBE

    End Function
    ''' <summary>
    ''' Lista todos los expedientes de CalificacionInstrumentoBE tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar(ByVal dataRequest As DataSet) As CalificacionInstrumentoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CalificacionInstrumento_Listar")

        Dim oCalificacionInstrumentoBE As New CalificacionInstrumentoBE
        db.LoadDataSet(dbCommand, oCalificacionInstrumentoBE, "CalificacionInstrumento")
        Return oCalificacionInstrumentoBE

    End Function
#End Region

#Region " /* Funciones Insertar */ "

    Public Function Insertar(ByVal oCalificacionInstrumentoBE As CalificacionInstrumentoBE, ByVal dataRequest As DataSet) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CalificacionInstrumento_Insertar")

        oRow = DirectCast(oCalificacionInstrumentoBE.CalificacionInstrumento.Rows(0), CalificacionInstrumentoBE.CalificacionInstrumentoRow)

        db.AddInParameter(dbCommand, "@p_CodigoCalificacion", DbType.String, oRow.CodigoCalificacion)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Prioridad", DbType.Int32, oRow.Prioridad)
        db.AddInParameter(dbCommand, "@p_Maduracion", DbType.String, oRow.Maduracion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_Plazo", DbType.String, oRow.Plazo)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

#End Region

#Region " /* Funciones Modificar */"

    Public Function Modificar(ByVal oCalificacionInstrumentoBE As CalificacionInstrumentoBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CalificacionInstrumento_Modificar")

        oRow = DirectCast(oCalificacionInstrumentoBE.CalificacionInstrumento.Rows(0), CalificacionInstrumentoBE.CalificacionInstrumentoRow)

        db.AddInParameter(dbCommand, "@p_CodigoCalificacion", DbType.String, oRow.CodigoCalificacion)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Prioridad", DbType.Int32, oRow.Prioridad)
        db.AddInParameter(dbCommand, "@p_Maduracion", DbType.String, oRow.Maduracion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Plazo", DbType.String, oRow.Plazo)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

#End Region

#Region " /* Funciones Eliminar */"

    Public Function Eliminar(ByVal codigoCalificacionInstrumento As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CalificacionInstrumento_Eliminar")

        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_CodigoCalificacionInstrumento", DbType.String, codigoCalificacionInstrumento)

        db.ExecuteNonQuery(dbCommand)

        Return True
    End Function
#End Region

#Region " /* Funciones Personalizadas*/"

#End Region

End Class