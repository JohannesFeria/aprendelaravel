Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

	Public class CodigoPostalDAM
    
    Private sqlCommand As String = ""
    Private oCodigoPostalRow As CodigoPostalBE.CodigoPostalRow
		Public Sub New()

		End Sub
#Region " /* Funciones Seleccionar */ "

    Public Function SeleccionarPorFiltros(ByVal codigoCodigoPostal As String, ByVal descripcion As String, ByVal dataRequest As DataSet) As CodigoPostalBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CodigoPostal_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoPostal", DbType.String, codigoCodigoPostal)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)

        Dim objeto As New CodigoPostalBE
        db.LoadDataSet(dbCommand, objeto, "CodigoPostalBE")
        Return objeto

    End Function
    Public Function SeleccionarPorFiltro(ByVal codigoPostal As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As CodigoPostalBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CodigoPostal_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoPostal", DbType.String, codigoPostal)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        Dim objeto As New CodigoPostalBE
        db.LoadDataSet(dbCommand, objeto, "CodigoPostal")
        Return objeto

    End Function

    Public Function Seleccionar(ByVal codigoCodigoPostal As String, ByVal dataRequest As DataSet) As CodigoPostalBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CodigoPostal_Seleccionar")
        db.AddInParameter(dbCommand, "@p_CodigoPostal", DbType.String, codigoCodigoPostal)
        Dim objeto As New CodigoPostalBE
        db.LoadDataSet(dbCommand, objeto, "CodigoPostal")
        Return objeto

    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As CodigoPostalBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CodigoPostal_Listar")

        Dim objeto As New CodigoPostalBE
        db.LoadDataSet(dbCommand, objeto, "CodigoPostal")
        Return objeto

    End Function
#End Region

    Public Function Insertar(ByVal ob As CodigoPostalBE, ByVal dataRequest As DataSet)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CodigoPostal_Insertar")
        oCodigoPostalRow = CType(ob.CodigoPostal.Rows(0), CodigoPostalBE.CodigoPostalRow)

        db.AddInParameter(dbCommand, "@p_CodigoPostal", DbType.String, oCodigoPostalRow.CodigoPostal)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oCodigoPostalRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oCodigoPostalRow.Situacion)

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

    End Function

    Public Function Modificar(ByVal ob As CodigoPostalBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CodigoPostal_Modificar")
        oCodigoPostalRow = CType(ob.CodigoPostal.Rows(0), CodigoPostalBE.CodigoPostalRow)

        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oCodigoPostalRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oCodigoPostalRow.Situacion)

        db.AddInParameter(dbCommand, "@p_CodigoPostal", DbType.String, oCodigoPostalRow.CodigoPostal)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Eliminar(ByVal codigoPostal As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CodigoPostal_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoPostal", DbType.String, codigoPostal)

        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
End Class

