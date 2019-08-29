Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class OrdenInversionFormulasDAM
    Public Function SeleccionarCaracValor_Acciones(ByVal codigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OI_SeleccionarCaracValor_Acciones")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarCaracValor_Acciones(ByVal portafolio As String, ByVal codigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OI_SeleccionarCaracValor_Acciones")
        db.AddInParameter(dbCommand, "@nvcCodigoPortafolio", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarCaracValor_Bonos(ByVal codigoOrden As String, ByVal fondo As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OI_SeleccionarCaracValor_Bonos")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, fondo)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    'OT 10090 - 21/03/2017 - Carlos Espejo
    'Descripcion: Se elimian el parametro obsoleto
    Public Function SeleccionarCaracValor_CertificadoDeposito(ByVal CodigoNemonico As String, ByVal fondo As String, ByVal strCodigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OI_SeleccionarCaracValor_CertificadoDeposito")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, CodigoNemonico)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, fondo)
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, strCodigoOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarCaracValor_CertificadoSuscripcion(ByVal CodigoPortafolioSBS As String, ByVal codigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OI_SeleccionarCaracValor_CertificadoSuscripcion")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarCaracValor_InstCoberturados(ByVal codigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OI_SeleccionarCaracValor_InstCoberturados")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarCaracValor_InstEstructurados(ByVal codigoOrden As String, ByVal fondo As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OI_SeleccionarCaracValor_InstEstructurados")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, fondo)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarCaracValor_LetrasHipotecarias(ByVal codigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OI_SeleccionarCaracValor_LetrasHipotecarias")
            db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoOrden)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function SeleccionarCaracValor_LetrasHipotecarias(ByVal CodigoNemonico As String, ByVal codigoPortafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OI_SeleccionarCaracteristicaValor_LetrasHipotecarias")
            db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, CodigoNemonico)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolio)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function SeleccionarCaracValor_OperacionesReporte(ByVal codigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OI_SeleccionarCaracValor_OperacionesReporte")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarCaracValor_DepositoPlazos(ByVal codigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OI_SeleccionarCaracValor_DepositoPlazos")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarCaracValor_ForwardDivisas(ByVal codigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OI_SeleccionarCaracValor_ForwardDivisas")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarCaracValor_Futuros(ByVal codigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_SeleccionarCaracteristicas_Valores")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarCaracValor_OrdenesFondo(ByVal codigoOrden As String, ByVal fondo As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OI_SeleccionarCaracValor_OrdenesFondo")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, fondo)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarCaracValor_Pagares(ByVal codigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OI_SeleccionarCaracValor_Pagares")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarCaracValor_PapelesComerciales(ByVal codigoOrden As String, ByVal fondo As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OI_SeleccionarCaracValor_PapelesComerciales")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, fondo)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function CalcularMontoOperacion2(ByVal guid As String, ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal valornominal As Decimal,
    ByVal YMT As Decimal, ByVal tipotasa As String) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_CalcularMontoOperacion2")
        db.AddInParameter(dbCommand, "@p_guid", DbType.String, guid)
        db.AddInParameter(dbCommand, "@p_codigoMnemonico", DbType.String, codigonemonico)
        db.AddInParameter(dbCommand, "@p_fechaoperacion", DbType.Decimal, fechaoperacion)
        db.AddInParameter(dbCommand, "@p_dblValNom", DbType.Decimal, valornominal)
        db.AddInParameter(dbCommand, "@p_YMT", DbType.Decimal, YMT)
        db.AddInParameter(dbCommand, "@p_tipoTasa", DbType.String, tipotasa)
        Return Convert.ToDecimal(db.ExecuteScalar(dbCommand))
    End Function
    Public Function CalcularDuracionOI(ByVal guid As String, ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal valornominal As Decimal,
    ByVal YMT As Decimal, ByVal tipotasa As String) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_CalcularDuracion")
        db.AddInParameter(dbCommand, "@p_guid", DbType.String, guid)
        db.AddInParameter(dbCommand, "@p_codigoMnemonico", DbType.String, codigonemonico)
        db.AddInParameter(dbCommand, "@p_fechaoperacion", DbType.Decimal, fechaoperacion)
        db.AddInParameter(dbCommand, "@p_dblValNom", DbType.Decimal, valornominal)
        db.AddInParameter(dbCommand, "@p_YMT", DbType.Decimal, YMT)
        db.AddInParameter(dbCommand, "@p_tipoTasa", DbType.String, tipotasa)
        Return Convert.ToDecimal(db.ExecuteScalar(dbCommand))
    End Function
    Public Function CalcularInteresesCorridos(ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal valornominal As Decimal, ByVal tasainteres As Decimal,
    ByVal TipoTasa As String, Optional ByVal FlagOperacionReporte As Integer = 0) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_CalcularInteresCorridos")
            db.AddInParameter(dbCommand, "@p_CodNemonico", DbType.String, codigonemonico)
            db.AddInParameter(dbCommand, "@p_FchOperacion", DbType.Decimal, fechaoperacion)
            db.AddInParameter(dbCommand, "@p_ImpNominal", DbType.Decimal, valornominal)
            db.AddInParameter(dbCommand, "@p_TasaInt", DbType.Decimal, tasainteres)
            db.AddInParameter(dbCommand, "@TipoTasa", DbType.String, TipoTasa)
            db.AddInParameter(dbCommand, "@flagOperacionReporte", DbType.Int32, FlagOperacionReporte)
            db.AddOutParameter(dbCommand, "@IntCorrido", DbType.Double, 7)
            db.ExecuteNonQuery(dbCommand)
            Dim dato As Decimal
            dato = Convert.ToDecimal(db.GetParameterValue(dbCommand, "@IntCorrido"))
            Return dato
        End Using
    End Function
    Public Function OntenerMontoCupon(ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal valornominal As Decimal) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("ObtenerMontoCupon")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigonemonico)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaoperacion)
        db.AddInParameter(dbCommand, "@p_ValorNominal", DbType.Decimal, valornominal)
        Dim dato As Decimal = CDec(db.ExecuteScalar(dbCommand))
        Return dato
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Se retira en parametro GUID
    Public Function CalcularPrecioLimpioN(ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal valornominal As Decimal, ByVal tasainteres As Decimal,
    ByVal TipoTasa As String) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_CalcularPrecioLimpioOI")
            db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, codigonemonico)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaoperacion)
            db.AddInParameter(dbCommand, "p_dblValNom", DbType.Decimal, valornominal)
            db.AddInParameter(dbCommand, "@p_YMT", DbType.Decimal, tasainteres)
            db.AddInParameter(dbCommand, "@p_TipoTasa", DbType.String, TipoTasa)
            db.AddOutParameter(dbCommand, "@p_PrecioLimpio", DbType.Double, 7)
            db.ExecuteNonQuery(dbCommand)
            Dim dato As Decimal
            dato = Convert.ToDecimal(db.GetParameterValue(dbCommand, "@p_PrecioLimpio"))
            Return dato
        End Using
    End Function
    Public Function CalcularTIR(ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal guid As String, ByVal valornominal As Decimal) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_CalcularTIR")
        db.AddInParameter(dbCommand, "@p_codigoMnemonico", DbType.String, codigonemonico)
        db.AddInParameter(dbCommand, "@p_guid", DbType.String, guid)
        db.AddInParameter(dbCommand, "@p_fechaoperacion", DbType.Decimal, fechaoperacion)
        db.AddInParameter(dbCommand, "@dblValOpe_p", DbType.Decimal, valornominal)
        Return Convert.ToDecimal(db.ExecuteScalar(dbCommand))
    End Function
    Public Function CalcularVPN(ByVal manejaflujo As String, ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal guid As String, _
    ByVal valornominal As Decimal, ByVal tasanegociacion As Decimal, ByVal tipotasa As String) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_CalcularVPN")
            db.AddInParameter(dbCommand, "@chrManTfi_p", DbType.String, manejaflujo)
            db.AddInParameter(dbCommand, "@p_codigoMnemonico", DbType.String, codigonemonico)
            db.AddInParameter(dbCommand, "@p_guid", DbType.String, guid)
            db.AddInParameter(dbCommand, "@p_fechaoperacion", DbType.Decimal, fechaoperacion)
            db.AddInParameter(dbCommand, "@dblValOpe_p", DbType.Decimal, valornominal)
            db.AddInParameter(dbCommand, "@tasanegociacion", DbType.Decimal, tasanegociacion)
            db.AddInParameter(dbCommand, "@TipoTasa", DbType.String, tipotasa)
            db.AddOutParameter(dbCommand, "@VPN", DbType.Double, 7)
            db.ExecuteNonQuery(dbCommand)
            Dim dato As Decimal = Convert.ToDecimal(db.GetParameterValue(dbCommand, "@VPN"))
            Return dato
        End Using
    End Function
    Public Function CalcularPrecioBono(ByVal CodigoPortafolioSBS As String, ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, _
    ByVal FechaLiquidacion As Decimal, ByVal TasaAnual As Decimal, MontoNominal As Decimal, TipoTasa As String, Operacion As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_CalcularPrecioBono")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_codigoMnemonico", DbType.String, codigonemonico)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.String, fechaoperacion)
            db.AddInParameter(dbCommand, "@p_FechaLiquidacion", DbType.Decimal, FechaLiquidacion)
            db.AddInParameter(dbCommand, "@p_TasaAnual", DbType.Decimal, TasaAnual)
            db.AddInParameter(dbCommand, "@P_MontoNominal", DbType.Decimal, MontoNominal)
            db.AddInParameter(dbCommand, "@p_TipoTasa", DbType.String, TipoTasa)
            db.AddInParameter(dbCommand, "@p_Operacion", DbType.String, Operacion)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'OT 9856 - 24/01/2017 - Carlos Espejo
    'Descripcion: Forma de calculo para CD a la par
    Public Function CalcularPrecioBono_ALAPAR(ByVal CodigoPortafolioSBS As String, ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, _
    ByVal FechaLiquidacion As Decimal, ByVal TasaAnual As Decimal, MontoNominal As Decimal, TipoTasa As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_CalcularPrecioBono_ALAPAR")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_codigoMnemonico", DbType.String, codigonemonico)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.String, fechaoperacion)
        db.AddInParameter(dbCommand, "@p_FechaLiquidacion", DbType.Decimal, FechaLiquidacion)
        db.AddInParameter(dbCommand, "@p_TasaAnual", DbType.Decimal, TasaAnual)
        db.AddInParameter(dbCommand, "@P_MontoNominal", DbType.Decimal, MontoNominal)
        db.AddInParameter(dbCommand, "@p_TipoTasa", DbType.String, TipoTasa)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
    Public Function CalcularVPN2(ByVal manejaflujo As String, ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal guid As String,
    ByVal valornominal As Decimal, ByVal tasanegociacion As Decimal, ByVal tipotasa As String) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_CalcularVPN2")
        db.AddInParameter(dbCommand, "@chrManTfi_p", DbType.String, manejaflujo)
        db.AddInParameter(dbCommand, "@p_codigoMnemonico", DbType.String, codigonemonico)
        db.AddInParameter(dbCommand, "@p_guid", DbType.String, guid)
        db.AddInParameter(dbCommand, "@p_fechaoperacion", DbType.Decimal, fechaoperacion)
        db.AddInParameter(dbCommand, "@dblValOpe_p", DbType.Decimal, valornominal)
        db.AddInParameter(dbCommand, "@tasanegociacion", DbType.Decimal, tasanegociacion)
        db.AddInParameter(dbCommand, "@TipoTasa", DbType.String, tipotasa)
        Return Convert.ToDecimal(db.ExecuteScalar(dbCommand))
    End Function
    Public Function CalcularPrecioLimpio2(ByVal MontoNegociacion As Decimal, ByVal MontoNominal As Decimal, ByVal interescorrido As Decimal) As Decimal
        Dim dblprecioLimpio As Decimal
        dblprecioLimpio = Math.Round((MontoNegociacion - interescorrido) / MontoNominal, 7)
        Return dblprecioLimpio
    End Function
    Public Function CalcularPrecioLimpio(ByVal ValorPresenteNegociacion As Decimal, ByVal ValorPresenteCupon As Decimal, ByVal Interescorrido As Decimal) As Decimal
        Dim dblprecioLimpio As Decimal
        dblprecioLimpio = Math.Round((ValorPresenteNegociacion - Interescorrido) / ValorPresenteCupon, 7)
        Return dblprecioLimpio
    End Function
    Public Function CalcularPrecioSucio(ByVal ValorPresenteNegociacion As Decimal, ByVal ValorPresenteCupon As Decimal) As Decimal
        Dim dblprecioLimpio As Decimal
        dblprecioLimpio = Math.Round(ValorPresenteNegociacion / ValorPresenteCupon, 7)
        Return dblprecioLimpio
    End Function
    Public Function CalcularPrecioSucio3(ByVal MontoOperacion As Decimal, ByVal MontoNominal As Decimal) As Decimal
        Dim dblprecioLimpio As Decimal
        dblprecioLimpio = Math.Round(MontoOperacion / MontoNominal, 7)
        Return dblprecioLimpio
    End Function
    Public Function CalcularNumeroPapeles(ByVal Montonominaloperacion As Decimal, ByVal montoNominalesunidad As Decimal) As Decimal
        Dim dblNumeroPapeles As Decimal
        dblNumeroPapeles = Math.Round(Montonominaloperacion / montoNominalesunidad, 7)
        Return dblNumeroPapeles
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Se agrega el tipo decimal a la funcion
    Public Function CalularDiferencial(ByVal tcvcto As Decimal, ByVal tcspot As Decimal, ByVal fechavcto As Decimal, ByVal fechaconst As Decimal) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_calculodiferencial")
        dbCommand.CommandTimeout = 120
        db.AddInParameter(dbCommand, "@p_tccvto", DbType.Decimal, tcvcto)
        db.AddInParameter(dbCommand, "@p_tcspot", DbType.Decimal, tcspot)
        db.AddInParameter(dbCommand, "@p_fechaVcto", DbType.Decimal, fechavcto)
        db.AddInParameter(dbCommand, "@p_fechaconst", DbType.Decimal, fechaconst)
        Return Convert.ToDecimal(db.ExecuteScalar(dbCommand))
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Se retira en parametro GUID
    Public Function calcularTasanegociacion(ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal montonominal As Decimal, ByVal tipotasa As String,
    ByVal montooperacion As Decimal) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_CalculoTasaNegociacionIteracion")
        dbCommand.CommandTimeout = 120
        db.AddInParameter(dbCommand, "@p_codigoMnemonico", DbType.String, codigonemonico)
        db.AddInParameter(dbCommand, "@p_fechaoperacion", DbType.Decimal, fechaoperacion)
        db.AddInParameter(dbCommand, "@p_dblValNom", DbType.Decimal, montonominal)
        db.AddInParameter(dbCommand, "@p_tipoTasa", DbType.String, tipotasa)
        db.AddInParameter(dbCommand, "@p_montooperacion", DbType.String, montooperacion)
        Return Convert.ToDecimal(db.ExecuteScalar(dbCommand))
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Se retira en parametro GUID
    Public Function calcularTasanegociacionPrecio(ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal montonominal As Decimal, ByVal tipotasa As String,
    ByVal preciolimpio As Decimal) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_CalculoTasaNegociacionIteracionPrecio")
            dbCommand.CommandTimeout = 120
            db.AddInParameter(dbCommand, "@p_codigoMnemonico", DbType.String, codigonemonico)
            db.AddInParameter(dbCommand, "@p_fechaoperacion", DbType.Decimal, fechaoperacion)
            db.AddInParameter(dbCommand, "@p_dblValNom", DbType.Decimal, montonominal)
            db.AddInParameter(dbCommand, "@p_tipoTasa", DbType.String, tipotasa)
            db.AddInParameter(dbCommand, "@p_PrecioLimpio", DbType.String, preciolimpio)
            Return Convert.ToDecimal(db.ExecuteScalar(dbCommand))
        End Using
    End Function
    Public Function CalularValorOperacionDPCD(ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal, ByVal ValorNominal As Decimal, ByVal Tasa As Decimal,
    ByVal nemonico As String) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_CalculoCDPP")
        db.AddInParameter(dbCommand, "@p_fechainicio", DbType.Decimal, FechaInicio)
        db.AddInParameter(dbCommand, "@p_fechafinal", DbType.Decimal, FechaFin)
        db.AddInParameter(dbCommand, "@p_ValorNominal", DbType.Decimal, ValorNominal)
        db.AddInParameter(dbCommand, "@p_Tasa", DbType.Decimal, Tasa)
        db.AddInParameter(dbCommand, "@p_Nemonico", DbType.String, nemonico)
        Return Convert.ToDecimal(db.ExecuteScalar(dbCommand))
    End Function
    Public Function CalularValorOperacionDPCD(ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal, ByVal ValorNominal As Decimal, ByVal Tasa As Decimal,
    TipoTasa As String, ByVal nemonico As String) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_CalculoCDPP")
        db.AddInParameter(dbCommand, "@p_fechainicio", DbType.Decimal, FechaInicio)
        db.AddInParameter(dbCommand, "@p_fechafinal", DbType.Decimal, FechaFin)
        db.AddInParameter(dbCommand, "@p_ValorNominal", DbType.Decimal, ValorNominal)
        db.AddInParameter(dbCommand, "@p_Tasa", DbType.Decimal, Tasa)
        db.AddInParameter(dbCommand, "@p_Nemonico", DbType.String, nemonico)
        db.AddInParameter(dbCommand, "@p_TipoTasa", DbType.String, TipoTasa)
        Return Convert.ToDecimal(db.ExecuteScalar(dbCommand))
    End Function
    Public Function CalularValorOperacionDPCDPrecio(ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal, ByVal ValorNominal As Decimal, ByVal Precio As Decimal,
    ByVal nemonico As String) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_CalculoCDPPPrecio")
        db.AddInParameter(dbCommand, "@p_fechainicio", DbType.Decimal, FechaInicio)
        db.AddInParameter(dbCommand, "@p_fechafinal", DbType.Decimal, FechaFin)
        db.AddInParameter(dbCommand, "@p_ValorNominal", DbType.Decimal, ValorNominal)
        db.AddInParameter(dbCommand, "@p_precio2", DbType.Decimal, Precio)
        db.AddInParameter(dbCommand, "@p_Nemonico", DbType.String, nemonico)
        Return Convert.ToDecimal(db.ExecuteScalar(dbCommand))
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Se retira en parametro GUID
    Public Function CalcularMontoNominalVAC(ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal valornominal As Decimal) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_CalcularMontoNominalVAC")
            db.AddInParameter(dbCommand, "@p_CodNemonico", DbType.String, codigonemonico)
            db.AddInParameter(dbCommand, "@p_FchOperacion", DbType.Decimal, fechaoperacion)
            db.AddInParameter(dbCommand, "@p_ImpNominal", DbType.Decimal, valornominal)
            db.AddOutParameter(dbCommand, "@ImpNominal", DbType.Double, 7)
            db.ExecuteNonQuery(dbCommand)
            Dim dato As Decimal
            dato = Convert.ToDecimal(db.GetParameterValue(dbCommand, "@ImpNominal"))
            Return dato
        End Using
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Se retira en parametro GUID
    Public Function CalculoNroPapelesBonoAmortPrin(ByVal codigonemonico As String, ByVal fechaoperacion As Decimal, ByVal montooperacion As Decimal,
    ByVal valorunidades As Decimal) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_CalculoNroPapelesBonoAmortPrin")
        db.AddInParameter(dbCommand, "@p_CodNemonico", DbType.String, codigonemonico)
        db.AddInParameter(dbCommand, "@p_FchOperacion", DbType.Decimal, fechaoperacion)
        db.AddInParameter(dbCommand, "@p_MNomOper", DbType.Decimal, montooperacion)
        db.AddInParameter(dbCommand, "@p_VUnidades", DbType.Decimal, valorunidades)
        db.AddOutParameter(dbCommand, "@p_NroPapeles", DbType.Double, 7)
        db.ExecuteNonQuery(dbCommand)
        Dim dato As Decimal
        dato = Convert.ToDecimal(db.GetParameterValue(dbCommand, "@p_NroPapeles"))
        Return dato
    End Function
    Public Function RestriccionExcesosBroker(ByVal fechaoperacion As Decimal, ByVal codigotercero As String, ByVal montooperacion As Decimal,
    ByVal codigonemonico As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_RestriccionExcesosBroker")
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaoperacion)
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigotercero)
        db.AddInParameter(dbCommand, "@p_MontoOperacion", DbType.Decimal, montooperacion)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigonemonico)
        db.AddOutParameter(dbCommand, "@p_IndExceso", DbType.String, 1)
        db.ExecuteNonQuery(dbCommand)
        Dim dato As String
        dato = db.GetParameterValue(dbCommand, "@p_IndExceso")
        Return dato
    End Function
    Public Function ValidarTresuryADescuento(ByVal codigonemonico As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_ValidarTreasuryADescuento")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigonemonico)
        db.AddOutParameter(dbCommand, "@p_IndTreasury", DbType.String, 1)
        db.ExecuteNonQuery(dbCommand)
        Dim dato As String
        dato = db.GetParameterValue(dbCommand, "@p_IndTreasury")
        Return dato
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Se presenta el valor de mercado del instrumento
    Public Function ValorMercado(ByVal codigoMnemonico As String) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_obt_ValorMercado")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoMnemonico)
        db.AddOutParameter(dbCommand, "@p_ValorMercado", DbType.String, 29)
        db.ExecuteNonQuery(dbCommand)
        Return CDec(db.GetParameterValue(dbCommand, "@p_ValorMercado").ToString())
    End Function
End Class