Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class OrdenInversionDatosOperacionDAM
    Public Function ObtenerDatoOperacion_PrecioCalculadoPorcentaje(ByVal codigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OI_DatoOperacion_PrecioCalculadoPorcentaje")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ObtenerDatoOperacion_InteresCorrido(ByVal codigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OI_DatoOperacion_InteresCorrido")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ObtenerDatoOperacion_PrecioNegociacionLimpio(ByVal codigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OI_DatoOperacion_PrecNegLimpio")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ObtenerDatoOperacion_PrecioNegociacionSucio(ByVal codigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OI_DatoOperacion_PrecNegSucio")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ObtenerDatoOperacion_InteresAcumulado(ByVal codigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OI_DatoOperacion_InteresAcumulado")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ObtenerDatoOperacion_InteresCastigado(ByVal codigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OI_DatoOperacion_InteresCastigado")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    'VECTOR PRECIO FORWARDS
    Public Function ObtenerDatoOperacion_PorPoliza(ByVal NumeroPoliza As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Orden_Inversion_xPoliza")
        db.AddInParameter(dbCommand, "@p_NumeroPoliza", DbType.String, NumeroPoliza)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
    'VECTOR PRECIO FORWARDS
    Public Sub OrdenInversion_ActualizarPrecioFWD(ByVal NumeroPoliza As String, ByVal PrecioForwards As Decimal, ByVal Mtm As Decimal, _
    ByVal MtmDestino As Decimal, ByVal PrecioVector As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Orden_Inversion_ActualizarPrecioFWD")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_PrecioForwards", DbType.String, PrecioForwards)
        db.AddInParameter(dbCommand, "@p_PrecioVector", DbType.String, PrecioVector)
        db.AddInParameter(dbCommand, "@p_NumeroPoliza", DbType.String, NumeroPoliza)
        db.AddInParameter(dbCommand, "@p_Mtm", DbType.String, Mtm)
        db.AddInParameter(dbCommand, "@p_MtmDestino", DbType.String, MtmDestino)
        db.ExecuteNonQuery(dbCommand)
    End Sub
    'OT 9908 31/01/2017 - Carlos Espejo
    'Descripcion: Devuelve tabla de Interes cobrados en un rango de fechas
    Public Function InteresesCobrados(ByVal CodigoPortafolioSBS As String, ByVal FechaInicio As Decimal, FechaTermino As Decimal) As DataTable
        Try
            Dim oValorCuotaBE As New ValorCuotaBE
            Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
            Dim dbcomand As DbCommand = db.GetStoredProcCommand("sp_SIT_obt_InteresesCobrados")
            db.AddInParameter(dbcomand, "@p_CodigoPortafolio", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbcomand, "@p_FechaInicio", DbType.Decimal, FechaInicio)
            db.AddInParameter(dbcomand, "@p_FechaTermino", DbType.Decimal, FechaTermino)
            Return db.ExecuteDataSet(dbcomand).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10059 02/03/2017 - Carlos Espejo
    'Descripcion: Validar el codigo de Mercado del Emisor
    Public Function ValidarDatosConfirmacion(ByVal CodigoIsin As String) As String
        Try
            Dim oValorCuotaBE As New ValorCuotaBE
            Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
            Dim dbcomand As DbCommand = db.GetStoredProcCommand("sp_SIT_obt_ValidarDatosConfirmacion")
            db.AddInParameter(dbcomand, "@p_CodigoIsin", DbType.String, CodigoIsin)
            db.AddOutParameter(dbcomand, "@p_TerceroDescripcion", DbType.String, 64)
            db.ExecuteNonQuery(dbcomand)
            Return CType(db.GetParameterValue(dbcomand, "@p_TerceroDescripcion"), String)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Datos Adicionales para Cartas
    Public Function ListarValorTipoCarta(ByVal CodigoOrden As String, ByVal CodigoTipoDato As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ListarDatosCarta")
            db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
            db.AddInParameter(dbCommand, "@p_CodigoTipoDato", DbType.String, CodigoTipoDato)

            Return db.ExecuteDataSet(dbCommand)
        End Using
    End Function

    Public Function ListarBancosConfirmacion(ByVal CodigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ListarBancosConfirmacion")
            db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
            Return db.ExecuteDataSet(dbCommand)
        End Using
    End Function

    Public Function ListarCuentasPorBancoConfirmacion(ByVal CodigoTercero As String, ByVal CodigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ListarCuentasBancosConfirmacion")
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, CodigoTercero)
            db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)

            Return db.ExecuteDataSet(dbCommand)
        End Using
    End Function

End Class