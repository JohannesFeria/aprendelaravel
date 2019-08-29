Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class AprobadorCartaDAM
    Private sqlCommand As String = ""
    Dim DECIMAL_NULO As Decimal = -1
    Public oAprobadorCartaRow As AprobadorCartaBE.AprobadorCartaRow

    Public Sub New()
    End Sub

    Public Sub InicializarAprobadorCarta(ByRef oRow As AprobadorCartaBE.AprobadorCartaRow)
        oRow.CodigoInterno = ""
        oRow.Firma = ""
        oRow.Rol = ""
        oRow.Situacion = ""
        oRow.UsuarioCreacion = ""
        oRow.FechaCreacion = DECIMAL_NULO
        oRow.HoraCreacion = ""
        oRow.UsuarioModificacion = ""
        oRow.FechaModificacion = DECIMAL_NULO
        oRow.HoraModificacion = ""
        oRow.Host = ""
    End Sub

#Region "Funciones Seleccionar"
    Public Function SeleccionarPorFiltro(ByVal codigoInterno As String, ByVal rol As String, ByVal situacion As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_SeleccionarPorFiltro_AprobadorCarta")
            db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, codigoInterno)
            db.AddInParameter(dbCommand, "@p_Rol", DbType.String, rol)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function

    Public Function GeneraClaves(ByVal longitud As Decimal, ByVal upper As Boolean) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_GeneraClave_AprobadorCarta")
            db.AddInParameter(dbCommand, "@p_Longitud", DbType.Decimal, longitud)
            db.AddInParameter(dbCommand, "@p_Upper", DbType.Boolean, upper)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function

    Public Function ObtenerRutaReporteAprobacion(ByVal codigoUsuario As String, ByVal CodigoOperacionCaja As String, ByVal indReporte As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ObtenerRuta_ReporteAprobacion")
            dbCommand.CommandTimeout = 300
            db.AddInParameter(dbCommand, "@p_CodigoUsuario", DbType.String, codigoUsuario)
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, CodigoOperacionCaja)
            db.AddInParameter(dbCommand, "@p_IndReporte", DbType.String, indReporte)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Dim rutaReporte As String = ""
                If ds.Tables(0).Rows.Count > 0 Then rutaReporte = ds.Tables(0).Rows(0)("RutaReporte")
                Return rutaReporte
            End Using
        End Using
    End Function
#End Region

#Region "Funciones CRUD"
    Public Function Insertar(ByVal oAprobadorCartaBE As AprobadorCartaBE, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim db As Database = DatabaseFactory.CreateDatabase
        oAprobadorCartaRow = CType(oAprobadorCartaBE.AprobadorCarta.Rows(0), AprobadorCartaBE.AprobadorCartaRow)
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_Insertar_AprobadorCarta")
            db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, oAprobadorCartaRow.CodigoInterno)
            db.AddInParameter(dbCommand, "@p_Rol", DbType.String, oAprobadorCartaRow.Rol)
            db.AddInParameter(dbCommand, "@p_Firma", DbType.String, IIf(oAprobadorCartaRow.Firma = "", DBNull.Value, oAprobadorCartaRow.Firma))
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oAprobadorCartaRow.Situacion)
            db.AddInParameter(dbCommand, "@p_TipoFirmante", DbType.String, oAprobadorCartaRow.TipoFirmante) 'OT-10795
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddOutParameter(dbCommand, "@p_Valida", DbType.Boolean, bolResult)
            db.ExecuteNonQuery(dbCommand)
            Return CType(db.GetParameterValue(dbCommand, "@p_Valida"), Boolean)
        End Using
    End Function

    Public Function Modificar(ByVal oAprobadorCartaBE As AprobadorCartaBE, ByVal dataRequest As DataSet) As Boolean
        Modificar = False
        Dim db As Database = DatabaseFactory.CreateDatabase
        oAprobadorCartaRow = CType(oAprobadorCartaBE.AprobadorCarta.Rows(0), AprobadorCartaBE.AprobadorCartaRow)
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_Actualizar_AprobadorCarta")
            db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, oAprobadorCartaRow.CodigoInterno)
            db.AddInParameter(dbCommand, "@p_Rol", DbType.String, oAprobadorCartaRow.Rol)
            db.AddInParameter(dbCommand, "@p_Firma", DbType.String, oAprobadorCartaRow.Firma)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oAprobadorCartaRow.Situacion)
            db.AddInParameter(dbCommand, "@p_TipoFirmante", DbType.String, oAprobadorCartaRow.TipoFirmante) 'OT-10795
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            Modificar = True
        End Using
    End Function
#End Region

End Class
