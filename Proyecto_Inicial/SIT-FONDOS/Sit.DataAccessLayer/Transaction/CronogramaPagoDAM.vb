Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data.Common

Public Class CronogramaPagosDAM
#Region "Variables"
    Private sqlCommand As String = String.Empty
    Private oRow As AumentoCapitalBE
    Dim DECIMAL_NULO As Decimal = 0.0
#End Region
    Public Sub New()

    End Sub
#Region " /* Funciones Seleccionar */ "

    Public Function CronogramaPagos_ListarbyRangoFechaPortafolio(ByVal CodigoPortafolioSBS As String, ByVal FechaIni As Decimal, ByVal FechaFin As Decimal) As DataTable
        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("CronogramaPagos_ListarbyRangoFechaPortafolio")
            bd.AddInParameter(dbcomand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            bd.AddInParameter(dbcomand, "@p_FechaINI", DbType.Decimal, FechaIni)
            bd.AddInParameter(dbcomand, "@p_FechaFIN", DbType.Decimal, FechaFin)
            Using ds As DataSet = bd.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function CronogramaPagos_ListarbyDetalleInstrumento(ByVal CodigoPortafolioSBS As String, ByVal fechaPago As Decimal) As DataTable
        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("CronogramaPagos_ListarbyDetalleInstrumento")
            bd.AddInParameter(dbcomand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            bd.AddInParameter(dbcomand, "@p_fechaPago", DbType.Decimal, fechaPago)
            Using ds As DataSet = bd.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
#End Region

#Region " /* Funciones Insertar */ "
    Public Function CronogramaPagos_Insertar(ByVal oRowCP As CronogramaPagosBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CronogramaPagos_Insertar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oRowCP.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_FechaCronogramaPagos", DbType.Decimal, oRowCP.FechaCronogramaPagos)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Function
#End Region

#Region " /* Funciones Modificar */"
    Public Function CronogramaPagos_Modificar(ByVal oRowCP As CronogramaPagosBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CronogramaPagos_Modificar")
            db.AddInParameter(dbCommand, "@p_idCronogramaPagos", DbType.String, oRowCP.IDCronogramaPagos)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.Decimal, oRowCP.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_FechaCronogramaPagos", DbType.Decimal, oRowCP.FechaCronogramaPagos)
            db.AddInParameter(dbCommand, "@p_Estado", DbType.String, IIf(oRowCP.Estado = "E", "A", "I"))
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Function
#End Region

#Region " /* Funciones Eliminar */"
   
#End Region

#Region " /* Funciones Personalizadas*/"
    Public Sub InicializarCronogramaPagos(ByRef oRowCP As CronogramaPagosBE)
        oRowCP.IDCronogramaPagos = DECIMAL_NULO
        oRowCP.CodigoPortafolioSBS = String.Empty
        oRowCP.FechaCronogramaPagos = DECIMAL_NULO
        oRowCP.Estado = String.Empty
        oRowCP.UsuarioCreacion = String.Empty
        oRowCP.FechaCreacion = DECIMAL_NULO
        oRowCP.HoraCreacion = String.Empty
        oRowCP.UsuarioModificacion = String.Empty
        oRowCP.FechaModificacion = DECIMAL_NULO
        oRowCP.HoraModificacion = String.Empty
        oRowCP.Host = String.Empty
        oRowCP.codigoMnemonico = String.Empty
        oRowCP.fechaLiquidacion = DECIMAL_NULO
    End Sub

#End Region
End Class
