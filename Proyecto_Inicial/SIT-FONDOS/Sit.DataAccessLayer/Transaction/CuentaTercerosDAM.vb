Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports Sit.BusinessEntities
Public Class CuentaTercerosDAM
    Public Function SeleccionarDetallePorFiltro(ByVal codigoPortafolio As String, ByVal CodigoClaseCuenta As String, ByVal CodigoTercero As String, ByVal CodigoMoneda As String, ByVal CodigoMercado As String) As CuentaTercerosBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentaTerceros_SeleccionarPorFiltro")
        Dim objeto As New CuentaTercerosBE
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, CodigoTercero)
        db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, CodigoClaseCuenta)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, CodigoMercado)
        db.LoadDataSet(dbCommand, objeto, "Portafolio")
        Return objeto
    End Function

    Public Function SeleccionarCuentaTerceros(ByVal strCodigoTercero As String, ByVal strLiqAutomatica As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaTerceros_SeleccionarTerceroDet")
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, strCodigoTercero)
            db.AddInParameter(dbCommand, "@p_LiquidacionAutomatica", DbType.String, strLiqAutomatica)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function

    Public Function VerificarCuentaTerceros(ByVal strNumeroCuenta As String, ByVal strCodigoTercero As String, ByVal strCodigoPortafolioSBS As String, ByVal strBanco As String, ByVal strLiqAut As String) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaTerceros_VerificarCodigo")
            Dim lngcontar As Long
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, strNumeroCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, strCodigoTercero)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, strCodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_Banco", DbType.String, strBanco)
            db.AddInParameter(dbCommand, "@p_LiquidacionAutomatica", DbType.String, strLiqAut)
            lngcontar = db.ExecuteScalar(dbCommand)
            Return lngcontar
        End Using
    End Function

    Public Sub Eliminar(ByVal secuencial As Integer)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaTerceros_Eliminar")
            db.AddInParameter(dbCommand, "@p_Secuencial", DbType.String, secuencial)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub

    Public Function Seleccionar(ByVal banco As String, ByVal codigoTercero As String, ByVal codigoMoneda As String, ByVal liqAutomatica As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaTerceros_SeleccionarLiqAutomatica")
            db.AddInParameter(dbCommand, "@p_Banco", DbType.String, banco)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
            db.AddInParameter(dbCommand, "@p_LiquidacionAutomatica", DbType.String, liqAutomatica)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function

    Public Function Seleccionar(ByVal secuencial As Integer) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaTerceros_Seleccionar")
            db.AddInParameter(dbCommand, "@p_Secuencial", DbType.Int32, secuencial)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function

    Public Sub Modificar(ByVal oCuentaTerceros As CuentaTercerosBE, ByVal dataRequest As DataSet)
        Dim oCuentaTerceroRow As CuentaTercerosBE.CuentaTercerosRow
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaTerceros_Modificar")
            oCuentaTerceroRow = DirectCast(oCuentaTerceros.CuentaTerceros.Rows(0), CuentaTercerosBE.CuentaTercerosRow)
            db.AddInParameter(dbCommand, "@p_Secuencial", DbType.Int32, oCuentaTerceroRow.Secuencial)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, oCuentaTerceroRow.NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oCuentaTerceroRow.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_CuentaInterBancario", DbType.String, oCuentaTerceroRow.CuentaInterBancario)
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, oCuentaTerceroRow.CodigoMercado)
            db.AddInParameter(dbCommand, "@p_EntidadFinanciera", DbType.String, oCuentaTerceroRow.EntidadFinanciera)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oCuentaTerceroRow.Situacion)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oCuentaTerceroRow.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, oCuentaTerceroRow.CodigoTercero)
            db.AddInParameter(dbCommand, "@p_LiquidacionAutomatica", DbType.String, oCuentaTerceroRow.LiquidacionAutomatica)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub

End Class
