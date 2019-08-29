Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

	Public class SectorEmpresarialDAM

    Private sqlCommand As String = ""
    Public oSectorEmpresarialRow As SectorEmpresarialBE.SectorEmpresarialRow
    Public Sub New()

    End Sub
#Region " /* Funciones Seleccionar */ "
    Public Function SeleccionarPorFiltros(ByVal codigoSectorEmpresarial As String, ByVal descripcion As String, ByVal dataRequest As DataSet) As SectorEmpresarialBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SectorEmpresarial_Seleccionar")
            db.AddInParameter(dbCommand, "@p_CodigoSectorEmpresarial", DbType.String, codigoSectorEmpresarial)
            db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)

            Dim objeto As New SectorEmpresarialBE
            db.LoadDataSet(dbCommand, objeto, "CodigoPostalBE")
            Return objeto
        End Using
    End Function
    Public Function SeleccionarPorFiltro(ByVal codigoSectorEmpresarial As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As SectorEmpresarialBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "SectorEmpresarial_SeleccionarPorFiltro"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@p_CodigoSectorEmpresarial", DbType.String, codigoSectorEmpresarial)
            db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
            Dim objeto As New SectorEmpresarialBE
            db.LoadDataSet(dbCommand, objeto, "SectorEmpresarial")
            Return objeto
        End Using
    End Function
    Public Function Seleccionar(ByVal codigoSectorEmpresarial As String, ByVal dataRequest As DataSet) As SectorEmpresarialBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "SectorEmpresarial_Seleccionar"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@p_CodigoSectorEmpresarial", DbType.String, codigoSectorEmpresarial)
            Dim objeto As New SectorEmpresarialBE
            db.LoadDataSet(dbCommand, objeto, "SectorEmpresarial")
            Return objeto
        End Using
    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As SectorEmpresarialBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SectorEmpresarial_Listar")
            'Return db.ExecuteDataSet(dbCommand)
            Dim objeto As New SectorEmpresarialBE
            db.LoadDataSet(dbCommand, objeto, "SectorEmpresarial")
            Return objeto
        End Using
    End Function
#End Region

    Public Function Insertar(ByVal ob As SectorEmpresarialBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SectorEmpresarial_Insertar")
            oSectorEmpresarialRow = CType(ob.SectorEmpresarial.Rows(0), SectorEmpresarialBE.SectorEmpresarialRow)
            db.AddInParameter(dbCommand, "@p_CodigoSectorEmpresarial", DbType.String, oSectorEmpresarialRow.CodigoSectorEmpresarial)
            db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oSectorEmpresarialRow.Descripcion)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oSectorEmpresarialRow.Situacion)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Function

    Public Function Modificar(ByVal ob As SectorEmpresarialBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SectorEmpresarial_Modificar")
            oSectorEmpresarialRow = CType(ob.SectorEmpresarial.Rows(0), SectorEmpresarialBE.SectorEmpresarialRow)
            db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oSectorEmpresarialRow.Descripcion)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oSectorEmpresarialRow.Situacion)
            db.AddInParameter(dbCommand, "@p_CodigoSectorEmpresarial", DbType.String, oSectorEmpresarialRow.CodigoSectorEmpresarial)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function

    Public Function Eliminar(ByVal codigoSectorEmpresarial As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SectorEmpresarial_Eliminar")
            db.AddInParameter(dbCommand, "@p_CodigoSectorEmpresarial", DbType.String, codigoSectorEmpresarial)
            db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function

End Class

