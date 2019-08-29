Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Data.SqlClient
Imports Sit.BusinessEntities
Imports System.Configuration
Public Class CarteraTituloValoracionDAM
    Public Sub New()
    End Sub
    Public Function SeleccionarDuraciones(ByVal CodigoPortafolioSBS As String, ByVal CodigoMnemonico As String, ByVal fechaValoracion As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CarteraTituloValoracion_Duraciones")
            Dim objeto As New DataSet
            db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@CodigoMnemonico", DbType.String, CodigoMnemonico)
            db.AddInParameter(dbCommand, "@fechaValoracion", DbType.Decimal, fechaValoracion)
            db.LoadDataSet(dbCommand, objeto, "CarteraTituloValoracion")
            Return objeto
        End Using
    End Function
    Public Function SeleccionarDetalleDuraciones(ByVal CodigoPortafolioSBS As String, ByVal fechaValoracion As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CarteraTituloValoracion_DetalleDuraciones")
            Dim objeto As New DataSet
            db.AddInParameter(dbCommand, "@FechaValoracion", DbType.Decimal, fechaValoracion)
            db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.LoadDataSet(dbCommand, objeto, "CarteraTituloValoracion")
            Return objeto
        End Using
    End Function
    Public Function ObtenerFechaValoracion(ByVal CodigoPortafolioSBS As String, ByVal Escenario As String, ByVal Indicador As Boolean) As String
        'OT10916 - 06/11/2017 - Ian Pastor M. Refactorizar y ordenar código.
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim strFecha As String
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CarteraTituloValoracion_ObtenerFechaValoracion")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_Escenario", DbType.String, Escenario)
            If Indicador = True Then
                db.AddInParameter(dbCommand, "@p_Indica", DbType.String, "2")
            Else
                db.AddInParameter(dbCommand, "@p_Indica", DbType.String, "1")
            End If
            strFecha = db.ExecuteScalar(dbCommand)
            Return strFecha
        End Using
    End Function
    Public Function ObtenerFechaValoracion(ByVal codigoPortafolioSBS As String) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim fechaValorizacion As Decimal
        Using dbCommand As DbCommand = db.GetStoredProcCommand("usp_GetFechaValorizacion")
            db.AddInParameter(dbCommand, "@nvcCodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
            fechaValorizacion = db.ExecuteScalar(dbCommand)
            Return fechaValorizacion
        End Using
    End Function
    Public Function UltimaValoracion(ByVal fechaValoracion As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "ValorizacionCartera_UltimaFecha"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@FechaValoracion", DbType.Decimal, fechaValoracion)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function Validar(ByVal CodigoPortafolioSBS As String, ByVal fechaValoracion As Decimal) As Boolean
        Dim resultado As Boolean = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "CarteraTituloValoracion_Validar"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_FechaValoracion", DbType.Decimal, fechaValoracion)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Dim data As DataTable = ds.Tables(0)
                If (data.Rows.Count > 0) Then
                    resultado = True
                End If
            End Using
        End Using
        Return resultado
    End Function
    Public Function ReporteVL(ByVal FechaValorizacion As String) As DataTable
        Dim resultado As Boolean = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "sp_SIT_sel_ReporteVL_2"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@Fecha", DbType.String, FechaValorizacion)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Sub ReporteVLGenerar(ByVal FechaValorizacion As String, ByVal CodigoPortafolio As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "GenerarReporteVL"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@Fecha", DbType.String, FechaValorizacion)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.Decimal, CodigoPortafolio)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Function ReporteVLObtener(ByVal FechaValorizacion As String, ByVal Privados As Integer) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "ObtenerReporteVL"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@Fecha", DbType.Decimal, FechaValorizacion)
            db.AddInParameter(dbCommand, "@p_Privados", DbType.Decimal, Privados)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'Inicio OT10902 - 23/10/2017 - Jorge Benites
    'Reporte de diferencias en el proceso de valorización
    Public Function DiferenciaReporteVLObtener(ByVal FechaValorizacion As String, ByVal Privados As Integer) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "ObtenerDiferenciaReporteVL"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@Fecha", DbType.Decimal, FechaValorizacion)
            db.AddInParameter(dbCommand, "@p_Privados", DbType.Decimal, Privados)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'Fin OT10902
    Public Function ValidadFondos(ByVal FechaValorizacion As String, ByVal Privados As Integer) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "ValidadFondos"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@Fecha", DbType.Decimal, FechaValorizacion)
            db.AddInParameter(dbCommand, "@p_Privados", DbType.Decimal, Privados)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function ExisteValorizacionFechasPosteriores(ByVal CodigoPortafolioSBS As String, ByVal FechaValoracion As Decimal) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("dbo.SP_SIT_OBT_ExisteValorizacionFechasPosteriores")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_FechaValoracion", DbType.Decimal, FechaValoracion)
            db.AddOutParameter(dbCommand, "@p_Resultado", DbType.Decimal, 10)
            db.ExecuteNonQuery(dbCommand)
            Return CType(db.GetParameterValue(dbCommand, "@p_Resultado"), Decimal)
        End Using
    End Function
    Public Function Valida_PRECIERRE_CONSOLIDADO_OPERACIONES(ByVal FechaInicio As Date, ByVal FechaFinal As Date, ByVal idFondoOperaciones As Decimal) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Dim objeto As New DataSet
        Using dbCommand As DbCommand = db.GetStoredProcCommand("INGF_LIS_PRECIERRE_CONSOLIDADO_OPERACIONES")
            db.AddInParameter(dbCommand, "@fechaini", DbType.DateTime, FechaInicio)
            db.AddInParameter(dbCommand, "@fechafinal", DbType.DateTime, FechaFinal)
            db.AddInParameter(dbCommand, "@idFondo", DbType.Decimal, idFondoOperaciones)
            db.LoadDataSet(dbCommand, objeto, "Precierre")
            Return objeto.Tables(0).Rows.Count
        End Using
    End Function
    Public Function ReporteVLDiferencia(ByVal FechaHoy As Decimal, ByVal FechaAyer As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "sp_SIT_Lis_ReporteDiferenciaVL"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@p_FechaHoy", DbType.Decimal, FechaHoy)
            db.AddInParameter(dbCommand, "@p_FechaAyer", DbType.Decimal, FechaAyer)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'Inicio OT10902 - 23/10/2017 - Jorge Benites
    'Obtener diferencias en el proceso de valorización
    Public Function ObtenerDiferenciaReporteVL(ByVal FechaHoy As Decimal, ByVal Privado As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "ObtenerDiferenciaReporteVL"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@fecha", DbType.Decimal, FechaHoy)
            db.AddInParameter(dbCommand, "@p_Privados", DbType.Decimal, Privado)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'Inicio OT10902
    Public Function ObtenerValorCuotaPreCierreOperaciones(ByVal Fecha As String, ByVal CodigoPortafolioSisOpe As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Using DbCommand As DbCommand = db.GetStoredProcCommand("FOND_OBT_VALOR_CUOTA_FECHA")
            db.AddInParameter(DbCommand, "@fecha", DbType.String, Fecha)
            db.AddInParameter(DbCommand, "@idfondo", DbType.String, CodigoPortafolioSisOpe)
            ObtenerValorCuotaPreCierreOperaciones = db.ExecuteDataSet(DbCommand).Tables(0)
        End Using
    End Function
End Class