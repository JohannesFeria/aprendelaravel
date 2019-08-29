Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

	Public class GrupoEconomicoDAM

    Private sqlCommand As String = ""
    Private oGrupoEconomicoRow As GrupoEconomicoBE.GrupoEconomicoRow

		Public Sub New()

		End Sub
#Region " /* Funciones Seleccionar */ "
    Public Function Seleccionar(ByVal codigoGrupoEconomico As String) As GrupoEconomicoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoEconomico_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoGrupoEconomico", DbType.String, codigoGrupoEconomico)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function SeleccionarPorCodigoEmisor(ByVal codigoEmisor As Decimal) As GrupoEconomicoBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoEconomico_SeleccionarPorCodigoEmisor")

        db.AddInParameter(dbCommand, "@p_CodigoEmisor", DbType.Decimal, codigoEmisor)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function Listar() As GrupoEconomicoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoEconomico_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function Listar(ByVal dataRequest As DataSet) As GrupoEconomicoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoEconomico_Listar")

        Dim objeto As New GrupoEconomicoBE
        db.LoadDataSet(dbCommand, objeto, "GrupoEconomico")
        Return objeto
    End Function
    Public Function SeleccionarPorFiltro(ByVal codigoGrupoEconomico As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As GrupoEconomicoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoEconomico_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoGrupoEconomico", DbType.String, codigoGrupoEconomico)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        Dim objeto As New GrupoEconomicoBE
        db.LoadDataSet(dbCommand, objeto, "GrupoEconomico")
        Return objeto
    End Function

#End Region

    Public Function Insertar(ByVal ob As GrupoEconomicoBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoEconomico_Insertar")
        oGrupoEconomicoRow = CType(ob.GrupoEconomico.Rows(0), GrupoEconomicoBE.GrupoEconomicoRow)

        db.AddInParameter(dbCommand, "@p_CodigoGrupoEconomico", DbType.String, oGrupoEconomicoRow.CodigoGrupoEconomico)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oGrupoEconomicoRow.Descripcion)

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oGrupoEconomicoRow.Situacion)
        db.AddInParameter(dbCommand, "@p_EntidadVinculada", DbType.String, oGrupoEconomicoRow.EntidadVinculada)

        db.ExecuteNonQuery(dbCommand)
    End Function

    Public Function Modificar(ByVal ob As GrupoEconomicoBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoEconomico_Modificar")
        oGrupoEconomicoRow = CType(ob.GrupoEconomico.Rows(0), GrupoEconomicoBE.GrupoEconomicoRow)

        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oGrupoEconomicoRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_CodigoGrupoEconomico", DbType.String, oGrupoEconomicoRow.CodigoGrupoEconomico)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oGrupoEconomicoRow.Situacion)
        db.AddInParameter(dbCommand, "@p_EntidadVinculada", DbType.String, oGrupoEconomicoRow.EntidadVinculada)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Eliminar(ByVal codigoGrupoEconomico As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoEconomico_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoGrupoEconomico", DbType.String, codigoGrupoEconomico)

        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function EliminarPorCodigoEmisor(ByVal codigoEmisor As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GrupoEconomico_EliminarPorCodigoEmisor")

        db.AddInParameter(dbCommand, "@p_CodigoEmisor", DbType.Decimal, codigoEmisor)

        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
End Class

