Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports Sit.BusinessEntities
Public Class CuponeraDAM
    Private oCuponNormalRow As CuponeraNormalBE.CuponeraNormalRow
    Private oCuponEspecialRow As CuponeraEspecialBE.CuponeraEspecialRow
#Region " /* Constructor */ "
    Public Sub New()
    End Sub
#End Region
#Region " /* Find Methods */ "
    ''' <summary>
    ''' Selecciona la cuponera generada por una orden de inversion.
    ''' <summary>
    ''' <param name="codigoOrdenInversion"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorOrdenInversion(ByVal strCodigoMnemonico As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Cuponera_SeleccionarPorOrdenInversion")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, strCodigoMnemonico)
        Dim ds As New DataSet
        ds = db.ExecuteDataSet(dbCommand)
        Return ds
    End Function
    ''' <summary>
    ''' Selecciona la cuponeras vencidas y no vencidas
    ''' <summary>
    ''' <param name="codigoOrdenInversion"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarVencimientos(ByVal codigoNemonico As String, ByVal valorNominal As Decimal, _
        ByVal fecha As String, ByVal codigoPortafolio As String, ByRef saldo As Decimal, _
        ByVal codigoOrden As String, ByRef codigoISIN As String, ByRef codigoSBS As String, ByRef fechaVencimiento As Decimal, secuencial As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CuponeraOI_BuscarVencimientos")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_ValorNominal", DbType.Decimal, valorNominal)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@nvcCodigoPortafolio", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)
        db.AddOutParameter(dbCommand, "@p_Saldo", DbType.Decimal, saldo)
        db.AddOutParameter(dbCommand, "@p_CodigoISIN", DbType.String, 12)
        db.AddOutParameter(dbCommand, "@p_CodigoSBS", DbType.String, 12)
        db.AddOutParameter(dbCommand, "@p_FechaVencimiento", DbType.Decimal, 8)
        db.AddInParameter(dbCommand, "@p_Secuencial", DbType.String, secuencial)
        Dim ds As New DataSet
        ds = db.ExecuteDataSet(dbCommand)
        saldo = db.GetParameterValue(dbCommand, "@p_Saldo")
        codigoISIN = db.GetParameterValue(dbCommand, "@p_CodigoISIN")
        codigoSBS = db.GetParameterValue(dbCommand, "@p_CodigoSBS")
        fechaVencimiento = db.GetParameterValue(dbCommand, "@p_FechaVencimiento")
        Return ds
    End Function
    ''' <summary>
    ''' Selecciona la cuponeras amortizaciones vencidas y no vencidas
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarAmortizaciones(ByVal codigoOrden As String, ByVal codigoNemonico As String, _
        ByVal valorNominal As Decimal, ByVal fecha As String, ByVal codigoPortafolio As String, _
        ByRef cantidadOperacion As Decimal, ByRef codigoISIN As String, ByRef codigoSBS As String, _
        ByRef fechaVencimiento As Decimal, Secuencial As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CuponeraOI_BuscarAmortizacion")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_ValorNominal", DbType.Decimal, valorNominal)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@nvcCodigoPortafolio", DbType.String, codigoPortafolio)
        db.AddOutParameter(dbCommand, "@p_CantidadOperacion", DbType.Decimal, cantidadOperacion)
        db.AddOutParameter(dbCommand, "@p_CodigoISIN", DbType.String, 12)
        db.AddOutParameter(dbCommand, "@p_CodigoSBS", DbType.String, 12)
        db.AddOutParameter(dbCommand, "@p_FechaVencimiento", DbType.Decimal, 8)
        db.AddInParameter(dbCommand, "@p_Secuencial", DbType.String, Secuencial)
        Dim ds As DataSet = db.ExecuteDataSet(dbCommand)
        cantidadOperacion = db.GetParameterValue(dbCommand, "@p_CantidadOperacion")
        codigoISIN = db.GetParameterValue(dbCommand, "@p_CodigoISIN")
        codigoSBS = db.GetParameterValue(dbCommand, "@p_CodigoSBS")
        fechaVencimiento = db.GetParameterValue(dbCommand, "@p_FechaVencimiento")
        Return ds
    End Function
    ''' <summary>
    ''' Devuelve si tiene cupones vencidos
    ''' <summary>
    ''' <param name="codigoOrdenInversion"></param>
    ''' <returns>DataSet</returns>
    Public Function CuponeraTieneCuponVencido(ByVal codigoOrden As String) As Boolean
        Dim rpta As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CuponeraOI_TieneCuponVencido")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)
        If Convert.ToInt32(db.ExecuteScalar(dbCommand)) > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function SeleccionarPorOrdenInversionVPN(ByVal codigoMnemonico As String, ByVal GUID As String, ByVal codigoOrden As String, ByVal codigoPortafolioSBS As String, ByVal fechaOperacion As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Cuponera_SeleccionarPorOrdenInversionVPN")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoMnemonico)
        db.AddInParameter(dbCommand, "@p_GUID", DbType.String, GUID)
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, codigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaOperacion)
        Return db.ExecuteDataSet(dbCommand)
    End Function
#End Region
    Public Function GenerarCuponeraNormal(ByVal strFlagAmortiza As String, ByVal strFechaEmision As String, ByVal strFechaVcto As String, ByVal strFechaPriCupon As String, ByVal decTasaCupon As String, ByVal decBaseCupon As String, ByVal intPeriodicidad As String, ByVal decTasaSpread As String, ByVal numDias As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CuponeraNormal_Obtener")
        dbCommand.CommandTimeout = 1020  'HDG 20110831
        db.AddInParameter(dbCommand, "@p_CodigoTipoAmortizacion", DbType.String, strFlagAmortiza)
        db.AddInParameter(dbCommand, "@p_fechaEmision", DbType.Decimal, strFechaEmision)
        db.AddInParameter(dbCommand, "@p_fechaVcto", DbType.Decimal, strFechaVcto)
        db.AddInParameter(dbCommand, "@p_fechaPriCupon", DbType.Decimal, strFechaPriCupon)
        db.AddInParameter(dbCommand, "@p_tasaCupon", DbType.Decimal, decTasaCupon)
        db.AddInParameter(dbCommand, "@p_baseCupon", DbType.Decimal, decBaseCupon)
        db.AddInParameter(dbCommand, "@p_periodicidad", DbType.Decimal, intPeriodicidad)
        db.AddInParameter(dbCommand, "@p_tasaSpread", DbType.Decimal, decTasaSpread)
        db.AddInParameter(dbCommand, "@p_numeroDias", DbType.Decimal, numDias)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function CalcularCuponeraNormal(ByVal codigoNemonico As String, _
                                           ByVal existeCuponera As Boolean, _
                                           ByVal codigoPeriodicidad As String, _
                                           ByVal valorNominal As Decimal, _
                                           ByVal codigoTipoAmortizacion As String, _
                                           ByVal strFechaEmision As String, _
                                           ByVal strFechaVencimiento As String, _
                                           ByVal strFechaPrimerCupon As String, _
                                           ByVal decTasaCupon As String, _
                                           ByVal decBaseCupon As String, _
                                           ByVal decTasaSpread As String, _
                                           ByVal numDias As String, _
                                           ByVal FlagSinCalcular As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Calcular_CuponeraNormal")
        dbCommand.CommandTimeout = 1020  'HDG 20110831
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, strFechaEmision)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, strFechaVencimiento)
        db.AddInParameter(dbCommand, "@p_FechaFinPrimerCupon", DbType.Decimal, strFechaPrimerCupon)
        db.AddInParameter(dbCommand, "@p_ValorNominal", DbType.Decimal, valorNominal)
        db.AddInParameter(dbCommand, "@p_CodigoTipoAmortizacion", DbType.String, codigoTipoAmortizacion)
        db.AddInParameter(dbCommand, "@p_CodigoPeriocidad", DbType.String, codigoPeriodicidad)
        db.AddInParameter(dbCommand, "@p_TasaCupon", DbType.Decimal, decTasaCupon)
        db.AddInParameter(dbCommand, "@p_TasaSpread", DbType.Decimal, decTasaSpread)
        db.AddInParameter(dbCommand, "@p_BaseCupon", DbType.String, decBaseCupon)
        db.AddInParameter(dbCommand, "@p_NumeroDias", DbType.String, numDias)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_FlagSinCalcular", DbType.String, FlagSinCalcular)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function Obtener_CuponActual(ByVal codigoNemonico As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Obtener_CuponActual")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        Return db.ExecuteDataSet(dbCommand)
    End Function


    Public Function ObtenerCuponera(ByVal CodigoTipoAmortizacionOriginal As String, ByVal fechaEmisionOriginal As Decimal, ByVal fechaVctoOriginal As Decimal, _
    ByVal fechaPriCuponOriginal As Decimal, ByVal CodigoPeriodicidadOriginal As String, ByVal tasaCuponOriginal As Decimal, ByVal baseCuponOriginal As String, _
    ByVal tasaSpreadOriginal As Decimal, ByVal numeroDiasOriginal As String, ByVal MontoNominalOriginal As Decimal, ByVal CadenaNemonico As String, ByVal ImporteVentaTotal As Decimal, _
    ByVal tipoCuponera As String, ByVal CodigoTipoAmortizacion As String, ByVal fechaEmision As Decimal, ByVal fechaVcto As Decimal, ByVal fechaPriCupon As Decimal, _
    ByVal CodigoPeriodicidad As String, ByVal tasaCupon As Decimal, ByVal baseCupon As String, ByVal tasaSpread As Decimal, ByVal numeroDias As String, ByVal MontoNominal As Decimal, _
    ByVal fechaOperacion As Decimal, ByVal diaTOriginal As Decimal, ByVal diaT As Decimal, ByVal portafolio As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ObtenerCuponera")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_CodigoTipoAmortizacionOriginal", DbType.String, CodigoTipoAmortizacionOriginal)
        db.AddInParameter(dbCommand, "@p_fechaEmisionOriginal", DbType.Decimal, fechaEmisionOriginal)
        db.AddInParameter(dbCommand, "@p_fechaVctoOriginal", DbType.Decimal, fechaVctoOriginal)
        db.AddInParameter(dbCommand, "@p_fechaPriCuponOriginal", DbType.Decimal, fechaPriCuponOriginal)
        db.AddInParameter(dbCommand, "@p_CodigoPeriodicidadOriginal", DbType.String, CodigoPeriodicidadOriginal)
        db.AddInParameter(dbCommand, "@p_tasaCuponOriginal", DbType.Decimal, tasaCuponOriginal)
        db.AddInParameter(dbCommand, "@p_baseCuponOriginal", DbType.String, baseCuponOriginal)
        db.AddInParameter(dbCommand, "@p_tasaSpreadOriginal", DbType.Decimal, tasaSpreadOriginal)
        db.AddInParameter(dbCommand, "@p_numeroDiasOriginal", DbType.String, numeroDiasOriginal)
        db.AddInParameter(dbCommand, "@p_MontoNominalOriginal", DbType.Decimal, MontoNominalOriginal)

        db.AddInParameter(dbCommand, "@p_CadenaNemonico", DbType.String, CadenaNemonico)
        db.AddInParameter(dbCommand, "@p_ImporteVentaTotal", DbType.Decimal, ImporteVentaTotal)
        db.AddInParameter(dbCommand, "@p_tipoCuponera", DbType.String, tipoCuponera)

        db.AddInParameter(dbCommand, "@p_CodigoTipoAmortizacion", DbType.String, CodigoTipoAmortizacion)
        db.AddInParameter(dbCommand, "@p_fechaEmision", DbType.Decimal, fechaEmision)
        db.AddInParameter(dbCommand, "@p_fechaVcto", DbType.Decimal, fechaVcto)
        db.AddInParameter(dbCommand, "@p_fechaPriCupon", DbType.Decimal, fechaPriCupon)
        db.AddInParameter(dbCommand, "@p_CodigoPeriodicidad", DbType.String, CodigoPeriodicidad)
        db.AddInParameter(dbCommand, "@p_tasaCupon", DbType.Decimal, tasaCupon)
        db.AddInParameter(dbCommand, "@p_baseCupon", DbType.String, baseCupon)
        db.AddInParameter(dbCommand, "@p_tasaSpread", DbType.Decimal, tasaSpread)
        db.AddInParameter(dbCommand, "@p_numeroDias", DbType.String, numeroDias)
        db.AddInParameter(dbCommand, "@p_MontoNominal", DbType.Decimal, MontoNominal)

        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaOperacion)

        db.AddInParameter(dbCommand, "@p_DiaTOriginal", DbType.Decimal, diaTOriginal)
        db.AddInParameter(dbCommand, "@p_DiaT", DbType.Decimal, diaT)

        db.AddInParameter(dbCommand, "@p_PortafolioSBS", DbType.String, portafolio)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
    Public Function EliminarCuponeraNormal(ByVal strNemonico As String, ByVal datarequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CuponeraNormal_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodNemonico", DbType.String, strNemonico)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(datarequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(datarequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(datarequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(datarequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function EliminarCuponeraNormal_Cupon(ByVal strNemonico As String, ByVal strSecuencia As String, ByVal datarequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CuponeraNormal_EliminarCupon")
        db.AddInParameter(dbCommand, "@p_CodNemonico", DbType.String, strNemonico)
        db.AddInParameter(dbCommand, "@p_Secuencia", DbType.String, strNemonico)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function GenerarCuponeraEspecial(ByVal strFlagAmortiza As String, ByVal strNroCupones As String, ByVal strFechaPriCupon As String, ByVal decTasaCupon As String, ByVal decBaseCupon As String, ByVal intPeriodicidad As String, ByVal decTasaSpread As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CuponeraEspecial_Obtener")
        db.AddInParameter(dbCommand, "@p_FlagAmortiza", DbType.String, strFlagAmortiza)
        db.AddInParameter(dbCommand, "@p_NroCupones", DbType.Decimal, strNroCupones)
        db.AddInParameter(dbCommand, "@p_fechaPriCupon", DbType.Decimal, strFechaPriCupon)
        db.AddInParameter(dbCommand, "@p_tasaCupon", DbType.Decimal, decTasaCupon)
        db.AddInParameter(dbCommand, "@p_baseCupon", DbType.Decimal, decBaseCupon)
        db.AddInParameter(dbCommand, "@p_periodicidad", DbType.Decimal, intPeriodicidad)
        db.AddInParameter(dbCommand, "@p_tasaSpread", DbType.Decimal, decTasaSpread)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function EliminarCuponeraEspecial(ByVal strNemonico As String, ByVal datarequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CuponeraEspecial_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodNemonico", DbType.String, strNemonico)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(datarequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(datarequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(datarequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(datarequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function EliminarCuponeraEspecial_Cupon(ByVal strNemonico As String, ByVal strSecuencia As String, ByVal datarequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CuponeraEspecial_EliminarCupon")
        db.AddInParameter(dbCommand, "@p_CodNemonico", DbType.String, strNemonico)
        db.AddInParameter(dbCommand, "@p_Secuencia", DbType.String, strNemonico)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function Valores_ActualizarTasaVariable(ByVal codigoNemonico As String, ByVal tipoTasaVariable As String, ByVal tasaVariable As Decimal, ByVal periodicidadTasaVariable As Integer, ByVal cuponReferencial As Integer) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Valores_ActualizarTasaVariable")
            db.AddInParameter(dbCommand, "@p_CodNemonico", DbType.String, codigoNemonico)
            db.AddInParameter(dbCommand, "@p_tipoTasaVariable", DbType.String, tipoTasaVariable)
            db.AddInParameter(dbCommand, "@p_tasaVariable", DbType.Decimal, tasaVariable)
            db.AddInParameter(dbCommand, "@p_periodicidadTasaVariable", DbType.Int32, periodicidadTasaVariable)
            db.AddInParameter(dbCommand, "@p_cuponTasaVariableReferencial", DbType.Int32, cuponReferencial)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function RegistrarCuponeraNormal(ByVal oBECuponeraNormal As CuponeraNormalBE, ByVal amortizacionTotal As String, ByVal datarequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuponeraNormal_Ingresar")
            oCuponNormalRow = CType(oBECuponeraNormal.CuponeraNormal.Rows(0), CuponeraNormalBE.CuponeraNormalRow)
            db.AddInParameter(dbCommand, "@p_CodNemonico", DbType.String, oCuponNormalRow.CodigoNemonico.ToUpper)
            db.AddInParameter(dbCommand, "@p_Secuencia", DbType.String, oCuponNormalRow.Secuencia)
            db.AddInParameter(dbCommand, "@p_fechaInicio", DbType.Decimal, oCuponNormalRow.FechaInicio)
            db.AddInParameter(dbCommand, "@p_fechaTermino", DbType.Decimal, oCuponNormalRow.FechaTermino)
            db.AddInParameter(dbCommand, "@p_diferenciaDias", DbType.Decimal, oCuponNormalRow.DiferenciaDias)
            db.AddInParameter(dbCommand, "@p_amortizacion", DbType.Decimal, oCuponNormalRow.Amortizacion)
            db.AddInParameter(dbCommand, "@p_tasaCupon", DbType.Decimal, oCuponNormalRow.TasaCupon)
            db.AddInParameter(dbCommand, "@p_tasaVariable", DbType.Decimal, oCuponNormalRow.TasaVariable)
            db.AddInParameter(dbCommand, "@p_base", DbType.Decimal, oCuponNormalRow.Base)
            db.AddInParameter(dbCommand, "@p_diasPago", DbType.Decimal, oCuponNormalRow.DiasPago)
            db.AddInParameter(dbCommand, "@p_situacion", DbType.String, oCuponNormalRow.Situacion)
            db.AddInParameter(dbCommand, "@p_montoNominal", DbType.String, amortizacionTotal)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(datarequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(datarequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(datarequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(datarequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function RegistrarCuponeraEspecial(ByVal oBECuponeraEspecial As CuponeraEspecialBE, ByVal datarequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CuponeraEspecial_Ingresar")
        oCuponEspecialRow = CType(oBECuponeraEspecial.CuponeraEspecial.Rows(0), CuponeraEspecialBE.CuponeraEspecialRow)
        db.AddInParameter(dbCommand, "@p_CodNemonico", DbType.String, oCuponEspecialRow.CodigoNemonico.ToUpper)
        db.AddInParameter(dbCommand, "@p_Secuencia", DbType.String, oCuponEspecialRow.Secuencia)
        db.AddInParameter(dbCommand, "@p_fechaInicio", DbType.Decimal, oCuponEspecialRow.FechaInicio)
        db.AddInParameter(dbCommand, "@p_fechaTermino", DbType.Decimal, oCuponEspecialRow.FechaTermino)
        db.AddInParameter(dbCommand, "@p_diferenciaDias", DbType.Decimal, oCuponEspecialRow.DiferenciaDias)
        db.AddInParameter(dbCommand, "@p_amortizacion", DbType.Decimal, oCuponEspecialRow.Amortizacion)
        db.AddInParameter(dbCommand, "@p_tasaCupon", DbType.Decimal, oCuponEspecialRow.TasaCupon)
        db.AddInParameter(dbCommand, "@p_base", DbType.Decimal, oCuponEspecialRow.Base)
        db.AddInParameter(dbCommand, "@p_diasPago", DbType.Decimal, oCuponEspecialRow.DiasPago)
        db.AddInParameter(dbCommand, "@p_situacion", DbType.String, oCuponEspecialRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(datarequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(datarequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(datarequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(datarequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function LeerCuponeraEspecial(ByVal strNemonico As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CuponeraEspecial_Leer")
        db.AddInParameter(dbCommand, "@p_CodNemonico", DbType.String, strNemonico)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function LeerCuponeraNormal(ByVal strNemonico As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CuponeraNormal_Leer")
        db.AddInParameter(dbCommand, "@p_CodNemonico", DbType.String, strNemonico)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function InsertarCuponeraOI(ByVal flag As Integer, ByVal GUID As String, ByVal Istemporal As Boolean, ByVal strCodigoOrden As String, ByVal strCodigoPortafolioSBS As String, ByVal dclFechaInicio As Decimal, ByVal dclFechaFin As Decimal, ByVal dclAmortizacion As Decimal, ByVal dclValorNominalOrigen As Decimal, ByVal strModalidad As String, ByVal strPeriodicidad As String, ByVal strRecalcula As String, ByVal dclTasaNominal As Decimal, ByVal dclVPN As Decimal, ByVal dclMontoNominal As Decimal, ByVal tipoTasa As String, ByVal diferenciaDias As Decimal, ByVal periodicidad As Decimal, ByVal base As Decimal, ByVal codigoTipoAmortizacion As String, ByVal codigoNemonico As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand
        If Istemporal Then
            dbCommand = db.GetStoredProcCommand("CuponeraOI_InsertarTemporal")
            db.AddInParameter(dbCommand, "@p_Guid", DbType.String, GUID)
            db.AddInParameter(dbCommand, "@p_MontoNominal", DbType.Decimal, dclMontoNominal)
            db.AddInParameter(dbCommand, "@p_TipoTasa", DbType.String, tipoTasa)
            db.AddInParameter(dbCommand, "@p_DiferenciaDias", DbType.Decimal, diferenciaDias)
            db.AddInParameter(dbCommand, "@p_NumPeriodicidad", DbType.Decimal, periodicidad)
            db.AddInParameter(dbCommand, "@p_Base", DbType.Decimal, base)
            db.AddInParameter(dbCommand, "@p_TipoAmortizacion", DbType.String, codigoTipoAmortizacion)
            db.AddInParameter(dbCommand, "@p_Flag", DbType.Decimal, flag)
        Else
            dbCommand = db.GetStoredProcCommand("CuponeraOI_Insertar")
        End If
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, strCodigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, strCodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, dclFechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, dclFechaFin)
        db.AddInParameter(dbCommand, "@p_Amortizacion", DbType.Decimal, dclAmortizacion)
        db.AddInParameter(dbCommand, "@p_ValorNominalOrigen", DbType.Decimal, dclValorNominalOrigen)
        db.AddInParameter(dbCommand, "@p_Modalidad", DbType.String, strModalidad)
        db.AddInParameter(dbCommand, "@p_Periodicidad", DbType.String, strPeriodicidad)
        db.AddInParameter(dbCommand, "@p_Recalcula", DbType.String, strRecalcula)
        db.AddInParameter(dbCommand, "@p_TasaNominal", DbType.String, dclTasaNominal)
        db.AddInParameter(dbCommand, "@p_VPN", DbType.String, dclVPN)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        Try
            db.ExecuteNonQuery(dbCommand)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function EliminarCuponeraOI(ByVal GUID As String, ByVal IsTemporal As String, ByVal strCodigoOrden As String, ByVal strCodigoPortafolioSBS As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand
        If IsTemporal Then
            dbCommand = db.GetStoredProcCommand("CuponeraOI_EliminarTemporal")
            db.AddInParameter(dbCommand, "@p_Guid", DbType.String, GUID)
        Else
            dbCommand = db.GetStoredProcCommand("CuponeraOI_Eliminar")
            db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, strCodigoOrden)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, strCodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        End If
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function ConfirmarVencimientoCuponeraOI(ByVal portafolio As String, ByVal codigoNemonico As String, ByVal fechaVencimiento As Decimal, ByVal montoNominalLocal As Decimal, ByVal secuencial As String, ByVal fechaIDI As Decimal, ByVal fechaPago As Decimal, ByVal ordenInversion As String, ByVal monedaDestino As String, ByVal montoOrigen As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim rpta As Boolean = False
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuponeraOI_ConfirmarVencimiento")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
            db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
            db.AddInParameter(dbCommand, "@p_OrdenInversion", DbType.String, ordenInversion)
            db.AddInParameter(dbCommand, "@p_FechaVencimiento", DbType.Decimal, fechaVencimiento)
            db.AddInParameter(dbCommand, "@p_MontoNominalLocal", DbType.Decimal, montoNominalLocal)
            db.AddInParameter(dbCommand, "@p_MonedaDestino", DbType.String, monedaDestino) 'Agregado por LC 20080911 para moneda diferente de pago
            db.AddInParameter(dbCommand, "@p_Secuencial", DbType.Decimal, secuencial)
            db.AddInParameter(dbCommand, "@decFechaPago", DbType.Decimal, fechaPago)
            db.AddInParameter(dbCommand, "@decFechaIDI", DbType.Decimal, fechaIDI)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_MontoOrigen", DbType.Decimal, montoOrigen) 'RGF 20090403
            db.ExecuteNonQuery(dbCommand)
            rpta = True
            Return rpta
        End Using
    End Function
    Public Function SecuenciaCuponera(ByVal CodigoOrden As String, ByVal Portafolio As String) As String
        SecuenciaCuponera = ""
        Dim dt As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_SecuenciaCuponera")
            db.AddInParameter(dbCommand, "@P_CodigoOrden", DbType.String, CodigoOrden)
            db.AddInParameter(dbCommand, "@P_CodigoPortafolioSBS", DbType.String, Portafolio)
            dt = db.ExecuteDataSet(dbCommand).Tables(0)
            If dt.Rows.Count > 0 Then
                SecuenciaCuponera = dt.Rows(0)(0)
            End If
        End Using
    End Function
    Public Function ListarCuponesPorVencer() As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_HechosImportancia_Listar")
            dbCommand.CommandTimeout = 1020
            db.AddInParameter(dbCommand, "@p_periodoDias", DbType.Decimal, 8)
            Dim dt As DataTable = db.ExecuteDataSet(dbCommand).Tables(0)
            Return dt
        End Using
    End Function
    Public Function CuponeraNormal_ObtenerPorcentajeParticipacion(ByVal CodigoNemonico As String, ByVal FechaIni As Decimal, ByVal FechaFin As Decimal, _
                                                                  ByVal Consecutivo As Integer, ByVal Estado As Integer, ByVal MontoNominalTotal As Decimal, _
                                                                  ByVal TasaCupon As Decimal, ByVal DifDias As Decimal, ByVal BaseCupon As Integer, _
                                                                  ByVal Amortizac As Decimal, ByVal SumaMontoAmortizacion As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuponeraNormal_ObtenerPorcentajeParticipacion")
            dbCommand.CommandTimeout = 1020
            db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, CodigoNemonico)
            db.AddInParameter(dbCommand, "@p_FechaIni", DbType.Decimal, FechaIni)
            db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, FechaFin)
            db.AddInParameter(dbCommand, "@p_Consecutivo", DbType.Int16, Consecutivo)
            db.AddInParameter(dbCommand, "@p_Estado", DbType.Int16, Estado)
            db.AddInParameter(dbCommand, "@p_MontoNominalTotal", DbType.Decimal, MontoNominalTotal)
            db.AddInParameter(dbCommand, "@p_TasaCupon", DbType.Decimal, TasaCupon)
            db.AddInParameter(dbCommand, "@p_DifDias", DbType.Decimal, DifDias)
            db.AddInParameter(dbCommand, "@p_BaseCupon", DbType.Int16, BaseCupon)
            db.AddInParameter(dbCommand, "@p_Amortizac", DbType.Decimal, Amortizac)
            db.AddInParameter(dbCommand, "@p_SumaMontoAmortizacion", DbType.Decimal, SumaMontoAmortizacion)
            Dim dt As DataTable = db.ExecuteDataSet(dbCommand).Tables(0)
            Return dt
        End Using
    End Function
End Class