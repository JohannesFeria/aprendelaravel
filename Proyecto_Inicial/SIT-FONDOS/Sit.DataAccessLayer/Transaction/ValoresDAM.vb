Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports Generic
Imports System.Collections.Generic
''' <summary>
''' Clase para el acceso de los datos para Valores tabla.
''' </summary>
Public Class ValoresDAM
    Private sqlCommand As String = ""
    Private oRow As ValoresBE.ValorRow
    Private strVacio As String = ""
    Private decVacio As String = 0
    Private decimalVacio As String = ""
    Public Sub New()
    End Sub
#Region " /* Funciones Seleccionar */ "
    Public Function SeleccionarDetalleCustodios(ByVal codigoMnemonico As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_SeleccionarDetalleCustodios")
        Dim DstTabla As New DataSet
        Dim IntI As Integer
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoMnemonico)
        DstTabla = db.ExecuteDataSet(dbCommand)
        Return DstTabla
    End Function
    Public Function SeleccionarDetalleCapitalCompro(ByVal codigoMnemonico As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Valores_SeleccionarCapitalFondoAlternativo")
        Dim DstTabla As New DataSet
        Dim IntI As Integer
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoMnemonico)
        DstTabla = db.ExecuteDataSet(dbCommand)
        Return DstTabla
    End Function
    Public Function BuscarCustodioValor(ByVal codigoMnemonico As String, ByVal codigoPortafolioSBS As String, ByVal codigoCustodio As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_BuscarCustodioValores")
        Dim intResultado As Integer
        db.AddInParameter(dbCommand, "@p_codigoNemonico", DbType.String, codigoMnemonico)
        db.AddInParameter(dbCommand, "@p_codigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_codigoCustodio", DbType.String, codigoCustodio)
        intResultado = CType(db.ExecuteScalar(dbCommand), Integer)
        If intResultado >= 1 Then
            Return True
        ElseIf intResultado = 0 Then
            Return False
        End If
        Return False
    End Function
    Public Function ValidarExistenciaValor(ByVal codigoMnemonico As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_ValidarExistenciaValor")
        Dim intResultado As Integer
        db.AddInParameter(dbCommand, "@p_codigoNemonico", DbType.String, codigoMnemonico)
        intResultado = CType(db.ExecuteScalar(dbCommand), Integer)
        If intResultado >= 1 Then
            Return True
        ElseIf intResultado = 0 Then
            Return False
        End If
        Return False
    End Function
    Public Function ValidarExistenciaValorModificar(ByVal codigoMnemonico As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_ValidarExistenciaValor")
        Dim intResultado As Integer
        db.AddInParameter(dbCommand, "@p_codigoNemonico", DbType.String, codigoMnemonico)
        intResultado = CType(db.ExecuteScalar(dbCommand), Integer)
        If intResultado >= 2 Then
            Return True
        ElseIf intResultado <= 1 Then
            Return False
        End If
        Return False
    End Function
    Public Function ValidarExistenciaValorISIN(ByVal codigoISIN As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_ValidarExistenciaValorISIN")
        Dim intResultado As Integer
        db.AddInParameter(dbCommand, "@p_codigoISIN", DbType.String, codigoISIN)
        intResultado = CType(db.ExecuteScalar(dbCommand), Integer)
        If intResultado >= 1 Then
            Return True
        ElseIf intResultado = 0 Then
            Return False
        End If
        Return False
    End Function
    Public Function ValidarExistenciaValorISINModificar(ByVal codigoISIN As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_ValidarExistenciaValorISIN")
        Dim intResultado As Integer
        db.AddInParameter(dbCommand, "@p_codigoISIN", DbType.String, codigoISIN)
        intResultado = CType(db.ExecuteScalar(dbCommand), Integer)
        If intResultado >= 2 Then
            Return True
        ElseIf intResultado <= 1 Then
            Return False
        End If
        Return False
    End Function
    Public Function ValidarExistenciaValorSBS(ByVal codigoSBS As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_ValidarExistenciaValorSBS")
        Dim intResultado As Integer
        db.AddInParameter(dbCommand, "@p_codigoSBS", DbType.String, codigoSBS)
        intResultado = CType(db.ExecuteScalar(dbCommand), Integer)
        If intResultado >= 1 Then
            Return True
        ElseIf intResultado = 0 Then
            Return False
        End If
        Return False
    End Function
    Public Function ValidarExistenciaValorSBSModificar(ByVal codigoSBS As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_ValidarExistenciaValorSBS")
        Dim intResultado As Integer
        db.AddInParameter(dbCommand, "@p_codigoSBS", DbType.String, codigoSBS)
        intResultado = CType(db.ExecuteScalar(dbCommand), Integer)
        If intResultado >= 2 Then
            Return True
        ElseIf intResultado <= 1 Then
            Return False
        End If
        Return False
    End Function
    Public Function VerificarCodigoPortafolioSBS(ByVal codigoPortafolioSBS As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_VerificarCodigoPortafolioSBS")
        Dim strResultado As String
        db.AddInParameter(dbCommand, "@codigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
        strResultado = CType(db.ExecuteScalar(dbCommand), String)
        Return strResultado
    End Function
    Public Function Seleccionar(ByVal codigoNemonico As String, ByVal dataRequest As DataSet) As ValoresBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_Seleccionar")
        Dim oValoresBE As New ValoresBE
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        Dim oDS As New DataSet
        oDS = db.ExecuteDataSet(dbCommand)
        Dim oRow As ValoresBE.ValorRow
        For Each dr As DataRow In oDS.Tables(0).Rows
            oRow = CType(oValoresBE.Valor.NewRow(), ValoresBE.ValorRow)
            InicializarValores(oRow)
            oRow.DescripcionTipoInstrumento = Convert.ToString(dr("DescripcionTipoInstrumento"))
            oRow.CodigoNemonico = IIf(dr("CodigoNemonico") Is DBNull.Value, strVacio, dr("CodigoNemonico"))
            oRow.CodigoTipoTitulo = IIf(dr("CodigoTipoTitulo") Is DBNull.Value, strVacio, dr("CodigoTipoTitulo"))
            oRow.CodigoTipoInstrumentoSBS = IIf(dr("CodigoTipoInstrumentoSBS") Is DBNull.Value, strVacio, dr("CodigoTipoInstrumentoSBS"))
            oRow.Descripcion = IIf(dr("Descripcion") Is DBNull.Value, strVacio, dr("Descripcion"))
            oRow.Agrupacion = IIf(dr("Agrupacion") Is DBNull.Value, strVacio, dr("Agrupacion"))
            oRow.CodigoEmisor = IIf(dr("CodigoEmisor") Is DBNull.Value, strVacio, dr("CodigoEmisor"))
            oRow.CodigoBursatilidad = IIf(dr("CodigoBursatilidad") Is DBNull.Value, strVacio, dr("CodigoBursatilidad"))
            oRow.CodigoMoneda = IIf(dr("CodigoMoneda") Is DBNull.Value, strVacio, dr("CodigoMoneda"))
            oRow.NumeroUnidades = IIf(dr("NumeroUnidades") Is DBNull.Value, decVacio, dr("NumeroUnidades"))
            oRow.CodigoISIN = IIf(dr("CodigoISIN") Is DBNull.Value, strVacio, dr("CodigoISIN"))
            oRow.ValorUnitario = IIf(dr("ValorUnitario") Is DBNull.Value, decVacio, dr("ValorUnitario"))
            oRow.CodigoSBS = IIf(dr("CodigoSBS") Is DBNull.Value, strVacio, dr("CodigoSBS"))
            oRow.ValorNominal = IIf(dr("ValorNominal") Is DBNull.Value, decVacio, dr("ValorNominal"))
            oRow.CodigoCalificacion = IIf(dr("CodigoCalificacion") Is DBNull.Value, strVacio, dr("CodigoCalificacion"))
            oRow.ValorEfectivoColocado = IIf(dr("ValorEfectivoColocado") Is DBNull.Value, decVacio, dr("ValorEfectivoColocado"))
            oRow.TasaEncaje = IIf(dr("TasaEncaje") Is DBNull.Value, decVacio, dr("TasaEncaje"))
            oRow.FechaEmision = IIf(dr("FechaEmision") Is DBNull.Value, decVacio, dr("FechaEmision"))
            oRow.CodigoTipoCupon = IIf(dr("CodigoTipoCupon") Is DBNull.Value, strVacio, dr("CodigoTipoCupon"))
            oRow.FechaVencimiento = IIf(dr("FechaVencimiento") Is DBNull.Value, decVacio, dr("FechaVencimiento"))
            oRow.TasaCupon = IIf(dr("TasaCupon") Is DBNull.Value, decVacio, dr("TasaCupon"))
            oRow.FechaPrimerCupon = IIf(dr("FechaPrimerCupon") Is DBNull.Value, decVacio, dr("FechaPrimerCupon"))
            oRow.CodigoPeriodicidad = IIf(dr("CodigoPeriodicidad") Is DBNull.Value, strVacio, dr("CodigoPeriodicidad"))
            oRow.CodigoTipoAmortizacion = IIf(dr("CodigoTipoAmortizacion") Is DBNull.Value, strVacio, dr("CodigoTipoAmortizacion"))
            oRow.TasaSpread = IIf(dr("TasaSpread") Is DBNull.Value, decVacio, dr("TasaSpread"))
            oRow.CodigoIndicador = IIf(dr("CodigoIndicador") Is DBNull.Value, strVacio, dr("CodigoIndicador"))
            oRow.ValorIndicador = IIf(dr("ValorIndicador") Is DBNull.Value, decVacio, dr("ValorIndicador"))
            oRow.Situacion = IIf(dr("Situacion") Is DBNull.Value, strVacio, dr("Situacion"))
            oRow.BaseTir = IIf(dr("BaseTir") Is DBNull.Value, strVacio, dr("BaseTir"))
            oRow.BaseTirDias = IIf(dr("BaseTirDias") Is DBNull.Value, strVacio, dr("BaseTirDias"))
            oRow.BaseCupon = IIf(dr("BaseCupon") Is DBNull.Value, strVacio, dr("BaseCupon"))
            oRow.BaseCuponDias = IIf(dr("BaseCuponDias") Is DBNull.Value, strVacio, dr("BaseCuponDias"))
            oRow.Observacion = IIf(dr("Observacion") Is DBNull.Value, strVacio, dr("Observacion"))
            oRow.CodigoRenta = IIf(dr("TipoRenta") Is DBNull.Value, strVacio, dr("TipoRenta"))
            oRow.TipoCuponera = IIf(dr("TipoCuponera") Is DBNull.Value, strVacio, dr("TipoCuponera"))
            oRow.cantidadIE = IIf(dr("CantidadIE") Is DBNull.Value, decVacio, dr("CantidadIE"))
            oRow.rentaFijaIE = IIf(dr("RentaFijaIE") Is DBNull.Value, decVacio, dr("RentaFijaIE"))
            oRow.rentaVarIE = IIf(dr("RentaVarIE") Is DBNull.Value, decVacio, dr("RentaVarIE"))
            oRow.MonedaPago = IIf(dr("MonedaPago") Is DBNull.Value, decVacio, dr("MonedaPago"))
            oRow.MonedaCupon = IIf(dr("MonedaCupon") Is DBNull.Value, decVacio, dr("MonedaCupon"))
            oRow.GrupoContable = IIf(dr("GrupoContable") Is DBNull.Value, strVacio, dr("GrupoContable"))
            oRow.Rescate = IIf(dr("Rescate") Is DBNull.Value, strVacio, dr("Rescate"))
            oRow.TipoRentaFija = IIf(dr("TipoRentaFija") Is DBNull.Value, strVacio, dr("TipoRentaFija"))
            oRow.CodigoClaseInstrumento = IIf(dr("CodigoClaseInstrumento") Is DBNull.Value, strVacio, dr("CodigoClaseInstrumento"))
            oRow.AplicaReduccionUnidades = IIf(dr("AplicaReduccionUnidades") Is DBNull.Value, strVacio, dr("AplicaReduccionUnidades"))
            oRow.MontoDividendo = IIf(dr("MontoDividendo") Is DBNull.Value, decVacio, dr("MontoDividendo"))
            oRow.Rating = IIf(dr("Rating") Is DBNull.Value, strVacio, dr("Rating"))
            oRow.TipoDerivado = IIf(dr("TipoDerivado") Is DBNull.Value, strVacio, dr("TipoDerivado"))
            oRow.CodigoTipoDerivado = IIf(dr("CodigoTipoDerivado") Is DBNull.Value, strVacio, dr("CodigoTipoDerivado"))
            oRow.FactorFlotante = IIf(dr("FactorFlotante") Is DBNull.Value, decVacio, dr("FactorFlotante"))
            oRow.SpreadFlotante = IIf(dr("SpreadFlotante") Is DBNull.Value, decVacio, dr("SpreadFlotante"))
            oRow.CodigoEmisorIE = IIf(dr("CodigoEmisorIE") Is DBNull.Value, strVacio, dr("CodigoEmisorIE"))
            oRow.CondicionImpuesto = IIf(dr("CondicionImpuesto") Is DBNull.Value, strVacio, dr("CondicionImpuesto"))
            oRow.PosicionAct = IIf(dr("PosicionAct") Is DBNull.Value, decVacio, dr("PosicionAct"))
            oRow.PorcPosicion = IIf(dr("PorcPosicion") Is DBNull.Value, decVacio, dr("PorcPosicion"))
            oRow.Categoria = IIf(dr("Categoria") Is DBNull.Value, strVacio, dr("Categoria"))
            oRow.Liberada = IIf(dr("Liberada") Is DBNull.Value, strVacio, dr("Liberada"))
            oRow.FechaLiberada = IIf(dr("FechaLiberada") Is DBNull.Value, decVacio, dr("FechaLiberada"))
            oRow.BaseIC = IIf(dr("BaseIC") Is DBNull.Value, strVacio, dr("BaseIC"))
            oRow.BaseICDias = IIf(dr("BaseICDias") Is DBNull.Value, strVacio, dr("BaseICDias"))
            oRow.SituacionEmi = IIf(dr("SituacionEmi") Is DBNull.Value, strVacio, dr("SituacionEmi"))
            oRow.MargenInicial = IIf(dr("MargenInicial") Is DBNull.Value, decVacio, dr("MargenInicial"))
            oRow.MargenMantenimiento = IIf(dr("MargenMantenimiento") Is DBNull.Value, decVacio, dr("MargenMantenimiento"))
            oRow.ContractSize = IIf(dr("ContractSize") Is DBNull.Value, decVacio, dr("ContractSize"))
            oRow.FirmaLlamado = IIf(dr("FirmaLlamado") Is DBNull.Value, strVacio, dr("FirmaLlamado"))
            oRow.ValorUnitarioActual = IIf(dr("ValorUnitarioActual") Is DBNull.Value, decVacio, dr("ValorUnitarioActual"))
            oRow.GeneraInteres = IIf(dr("GeneraInteres") Is DBNull.Value, strVacio, dr("GeneraInteres"))
            oRow.TipoCodigoValor = IIf(dr("TipoCodigoValor") Is DBNull.Value, decVacio, dr("TipoCodigoValor"))
            oRow.Estilo = IIf(dr("Estilo") Is DBNull.Value, strVacio, dr("Estilo"))
            oRow.CodigoMercado = IIf(dr("CodigoMercado") Is DBNull.Value, strVacio, dr("CodigoMercado"))
            oRow.MontoPiso = IIf(dr("MontoPiso") Is DBNull.Value, 0, dr("MontoPiso"))
            oRow.MontoTecho = IIf(dr("MontoTecho") Is DBNull.Value, 0, dr("MontoTecho"))
            oRow.Garante = IIf(dr("Garante") Is DBNull.Value, strVacio, dr("Garante"))
            oRow.Subyacente = IIf(dr("Subyacente") Is DBNull.Value, strVacio, dr("Subyacente"))
            oRow.PrecioEjercicio = IIf(dr("PrecioEjercicio") Is DBNull.Value, 0, dr("PrecioEjercicio"))
            oRow.TamanoEmision = IIf(dr("TamanoEmision") Is DBNull.Value, 0, dr("TamanoEmision"))
            oRow.CodigoPortafolioSBS = dr("CodigoPortafolioSBS")
            'OT 10090 - 21/03/2017 - Carlos Espejo
            'Descripcion: Se agrega moneda al objeto
            oRow.CodigoMonedaSBS = dr("CodigoMonedaSBS")
            'OT 10090 Fin

            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se agrega nuevo campo tiporentariesgo | 17/05/18
            oRow.TipoRentaRiesgo = dr("TipoRentaRiesgo")
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se agrega nuevo campo tiporentariesgo | 17/05/18

            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se agrega nuevo campo frecuencia de pagos (dividendos)| 18/05/18
            oRow.CodigoFrecuenciaDividendo = dr("CodigoFrecuenciaDividendo")
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se agrega nuevo campo frecuencia de pagos (dividendos)| 18/05/18

            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se agrega nuevo campo flag para habilitar Estado de Base IC,Base Interes Corrido, Base Interes Corrido Dias| 18/05/18
            oRow.EstadoBaseIC = dr("EstadoBaseIC")
            oRow.BaseInteresCorrido = IIf(dr("BaseInteresCorrido") Is DBNull.Value, "0", dr("BaseInteresCorrido"))
            oRow.BaseInteresCorridoDias = IIf(dr("BaseInteresCorridoDias") Is DBNull.Value, "0", dr("BaseInteresCorridoDias"))
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se agrega nuevo campo flag  para habilitar Estado de Base IC,Base Interes Corrido, Base Interes Corrido Dias | 18/05/18

            'INICIO | ZOLUXIONES | RCE | RU022 - Se agrega nuevo atributo a ValoresBE para campo Subordinario y Precio Devengado | 23/10/18
            oRow.Subordinado = dr("Subordinado")
            oRow.PrecioDevengado = dr("PrecioDevengado")
            'FIN | ZOLUXIONES | RCE | RU022 - Se agrega nuevo atributo a ValoresBE para campo Subordinario y Precio Devengado | 23/10/18
            oRow.TipoTasaVariable = dr("TipoTasaVariable")
            oRow.DiasTTasaVariable = dr("DiasTTasaVariable")
            oRow.TasaVariable = dr("TasaVariable")
            '--
            oRow.RatingMandato = IIf(dr("RatingMandato") Is DBNull.Value, strVacio, dr("RatingMandato"))
            '--
            oValoresBE.Valor.AddValorRow(oRow)
            oValoresBE.Valor.AcceptChanges()
        Next
        Return oValoresBE
    End Function
    ''' <summary>
    ''' Lista todos los expedientes de ValoresBE tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar(ByVal dataRequest As DataSet) As ValoresBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_Listar")
        Dim oValoresBE As New ValoresBE
        db.LoadDataSet(dbCommand, oValoresBE, "Valor")
        Return oValoresBE
    End Function
    Public Function SeleccionarBono(ByVal codigoNemonico As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_SeleccionarBono")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        Dim oValoresBono As New DataSet
        db.LoadDataSet(dbCommand, oValoresBono, "Valor")
        Return oValoresBono
    End Function
    Public Function SeleccionarCodigoMnemonicoPorCodigoSBS(ByVal codigoSBS As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_SeleccionarCodigoMnemonicoPorCodigoSBS")
        db.AddInParameter(dbCommand, "@codigoSBS", DbType.String, codigoSBS)
        Dim oValores As New DataSet
        db.LoadDataSet(dbCommand, oValores, "Valor")
        Return oValores
    End Function
    Public Function SeleccionarInstrumentoPorCodigoSBS(ByVal CodigoSBS As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("DatosValorPorCodigoSBS_seleccionar")
        db.AddInParameter(dbCommand, "@CodigoSBS", DbType.String, CodigoSBS)
        Dim oValores As New DataSet
        db.LoadDataSet(dbCommand, oValores, "Valor")
        Return oValores
    End Function
    Public Function SeleccionarInstrumento(ByVal CodigoISIN As String, ByVal CodigoNemonico As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("DatosValorPorMnemonico_seleccionar")
        db.AddInParameter(dbCommand, "@CodigoNemonico", DbType.String, CodigoNemonico)
        db.AddInParameter(dbCommand, "@CodigoISIN", DbType.String, CodigoISIN)
        Dim oValores As New DataSet
        db.LoadDataSet(dbCommand, oValores, "Valor")
        Return oValores
    End Function
    Public Function CarteraTituloVerifica(ByVal CodigoSBS As String, ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String,
    ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CarteraTituloVerifica_Seleccionar")
        db.AddInParameter(dbCommand, "@CodigoSBS", DbType.String, CodigoSBS)
        db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, CodigoCustodio)
        Dim oValores As New DataSet
        db.LoadDataSet(dbCommand, oValores, "Valor")
        Return oValores
    End Function
    Public Function VerificaRelacionInstrumentoCustodio(ByVal CodigoMnemonico As String, ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VerificaRelacionInstrumentoCustodio_seleccionar")
        db.AddInParameter(dbCommand, "@CodigoMnemonico", DbType.String, CodigoMnemonico)
        db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, CodigoCustodio)
        Dim oValores As New DataSet
        db.LoadDataSet(dbCommand, oValores, "Valor")
        Return oValores
    End Function
    Public Function RecuperaSaldoTransferencia(ByVal CodigoSBS As String, ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String,
    ByVal FechaOperacion As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("RecuperaSaldosTransferenciaRecompile_Seleccionar")
        db.AddInParameter(dbCommand, "@CodigoSBS", DbType.String, CodigoSBS)
        db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, CodigoCustodio)
        db.AddInParameter(dbCommand, "@FechaSaldo", DbType.String, FechaOperacion)
        Dim oValores As New DataSet
        db.LoadDataSet(dbCommand, oValores, "Valor")
        Return oValores
    End Function
    Public Function RecuperaSaldosInstrumento(ByVal CodigoSBS As String, ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String,
    ByVal FechaOperacion As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("RecuperaSaldos_Seleccionar")
        db.AddInParameter(dbCommand, "@CodigoSBS", DbType.String, CodigoSBS)
        db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, CodigoCustodio)
        db.AddInParameter(dbCommand, "@FechaSaldo", DbType.String, FechaOperacion)
        Dim oValores As New DataSet
        db.LoadDataSet(dbCommand, oValores, "Valor")
        Return oValores
    End Function
    Public Function SeleccionarPagare(ByVal codigoNemonico As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_SeleccionarPagare")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        Dim oValores As New DataSet
        db.LoadDataSet(dbCommand, oValores, "Valor")
        Return oValores
    End Function
    Public Function InstrumentosBuscarPorFiltro(ByVal strSBS As String, ByVal strISIN As String, ByVal strMnemonico As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("InstrumentosBuscar_ListarPorFiltro")
        db.AddInParameter(dbCommand, "@CodigoSBS", DbType.String, strSBS)
        db.AddInParameter(dbCommand, "@CodigoISIN", DbType.String, strISIN)
        db.AddInParameter(dbCommand, "@CodigoMnemonico", DbType.String, strMnemonico)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function InstrumentosBuscarPorFiltroPreorden(ByVal strSBS As String, ByVal strISIN As String, ByVal strMnemonico As String,
    ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("InstrumentosBuscar_ListarPorFiltroPreorden")
        db.AddInParameter(dbCommand, "@CodigoSBS", DbType.String, strSBS)
        db.AddInParameter(dbCommand, "@CodigoISIN", DbType.String, strISIN)
        db.AddInParameter(dbCommand, "@CodigoMnemonico", DbType.String, strMnemonico)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function InstrumentosBuscarPorFiltroConsultarOrdenesPreordenes(ByVal strISIN As String, ByVal strSBS As String, _
    ByVal strMnemonico As String, ByVal correlativo As String, ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal, _
    ByVal TipoOperacion As String, ByVal TipoInstrumento As String, ByVal TipoRenta As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("InstrumentosBuscar_ListarPorFiltroConsultarOrdenesPreordenes")
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, strISIN)
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, strSBS)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, strMnemonico)
        db.AddInParameter(dbCommand, "@p_Correlativo", DbType.String, correlativo)
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, FechaInicio)
        db.AddInParameter(dbCommand, "@p_fechafin", DbType.Decimal, FechaFin)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, TipoOperacion)
        db.AddInParameter(dbCommand, "@p_CodigotipoInstrumentoSBS", DbType.String, TipoInstrumento)
        db.AddInParameter(dbCommand, "@p_codigotipoRenta", DbType.String, TipoRenta)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function InstrumentosBuscarPorFiltroKardex(ByVal TipoInstrumento As String, ByVal strISIN As String, ByVal strMnemonico As String,
    ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("InstrumentosBuscar_ListarPorFiltroKardex")
        db.AddInParameter(dbCommand, "@TipoInstrumento", DbType.String, TipoInstrumento)
        db.AddInParameter(dbCommand, "@CodigoISIN", DbType.String, strISIN)
        db.AddInParameter(dbCommand, "@CodigoMnemonico", DbType.String, strMnemonico)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarPorFiltro(ByVal dataRequest As DataSet, ByVal strCategoriaInstrumento As String, ByVal strSBS As String, ByVal strISIN As String,
    ByVal strMnemonico As String, ByVal strFondo As String, ByVal strCodigoTipoInstrumentoSBS As String, ByVal operacion As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_ListarPorFiltro")
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, strSBS)
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, strISIN)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, strMnemonico)
        db.AddInParameter(dbCommand, "@p_CategoriaInstrumento", DbType.String, strCategoriaInstrumento)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, strFondo)
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumentoSBS", DbType.String, strCodigoTipoInstrumentoSBS)
        db.AddInParameter(dbCommand, "@p_Operacion", DbType.String, operacion)
        Dim oDS As New DataSet
        db.LoadDataSet(dbCommand, oDS, "Valor")
        Return oDS
    End Function
    Public Function ListarPorTipoInstrumentoSBS(ByVal datarequest As DataSet, ByVal strTipoInstSBS As String) As ValoresBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_ListarPorTipoInstrumentoSBS")
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumentoSBS", DbType.String, strTipoInstSBS)
        Dim oValoresBE As New ValoresBE
        db.LoadDataSet(dbCommand, oValoresBE, "Valor")
        Return oValoresBE
    End Function
    Public Function SeleccionarPorFiltro(ByVal StrCodigoSBS As String, ByVal StrCodigoIsin As String, ByVal StrCodigoMnemonico As String, ByVal StrMoneda As String,
    ByVal StrTipoRenta As String, ByVal StrEmisor As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_SeleccionarPorFiltro")
        db.AddInParameter(dbCommand, "@p_TipoRenta", DbType.String, StrTipoRenta)
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, StrCodigoIsin)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, StrCodigoMnemonico)
        db.AddInParameter(dbCommand, "@p_CodigoEmisor", DbType.String, StrEmisor)
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, StrCodigoSBS)
        db.AddInParameter(dbCommand, "@p_Moneda", DbType.String, StrMoneda)
        Dim oDS As New DataSet
        db.LoadDataSet(dbCommand, oDS, "Valor")
        Return oDS
    End Function
    Public Function SeleccionarPorFiltro2(ByVal StrCodigoSBS As String, ByVal StrCodigoIsin As String, ByVal StrCodigoMnemonico As String, ByVal StrMoneda As String,
    ByVal StrTipoRenta As String, ByVal StrEmisor As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_SeleccionarPorFiltro2")
        db.AddInParameter(dbCommand, "@p_TipoRenta", DbType.String, StrTipoRenta)
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, StrCodigoIsin)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, StrCodigoMnemonico)
        db.AddInParameter(dbCommand, "@p_CodigoEmisor", DbType.String, StrEmisor)
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, StrCodigoSBS)
        db.AddInParameter(dbCommand, "@p_Moneda", DbType.String, StrMoneda)
        Dim oDS As New DataSet
        db.LoadDataSet(dbCommand, oDS, "Valor")
        Return oDS
    End Function
    Public Function SeleccionarPorFiltro3(ByVal StrCodigoSBS As String, ByVal StrCodigoIsin As String, ByVal StrCodigoMnemonico As String, ByVal StrMoneda As String,
    ByVal StrTipoRenta As String, ByVal StrEmisor As String, ByVal StrEstado As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Valores_SeleccionarPorFiltro")
        dbCommand.CommandTimeout = 300
        db.AddInParameter(dbCommand, "@p_TipoRenta", DbType.String, StrTipoRenta)
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, StrCodigoIsin)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, StrCodigoMnemonico)
        db.AddInParameter(dbCommand, "@p_CodigoEmisor", DbType.String, StrEmisor)
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, StrCodigoSBS)
        db.AddInParameter(dbCommand, "@p_Moneda", DbType.String, StrMoneda)
        db.AddInParameter(dbCommand, "@p_Estado", DbType.String, StrEstado)
        Dim oDS As New DataSet
        db.LoadDataSet(dbCommand, oDS, "Valor")
        Return oDS
    End Function
    Public Function Custodio_ListarCuentasDepositariasPorCustodio(ByVal codigoCustodio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Custodio_ListarCuentasDepositariasPorCustodio")
        db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, codigoCustodio)
        Dim oDS As New DataSet
        db.LoadDataSet(dbCommand, oDS, "Valor")
        Return oDS
    End Function
    Public Function SeleccionarCorrelativoMnemonicoOR(ByVal codigoNemonico As String, ByVal codigoTercero As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_SeleccionarCorrelativoMnemonicoOR")
        db.AddInParameter(dbCommand, "@p_codigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_codigoTercero", DbType.String, codigoTercero)
        Dim oDS As New DataSet
        db.LoadDataSet(dbCommand, oDS, "Valor")
        Return oDS
    End Function
    Public Function SeleccionarCorrelativoMnemonicoDP(ByVal tipoTitulo As String, ByVal codigoTercero As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_SeleccionarCorrelativoMnemonicoDP")
        db.AddInParameter(dbCommand, "@p_tipoTitulo", DbType.String, tipoTitulo)
        db.AddInParameter(dbCommand, "@p_codigoTercero", DbType.String, codigoTercero)
        Dim oDS As New DataSet
        db.LoadDataSet(dbCommand, oDS, "Valor")
        Return oDS
    End Function
    Public Function SeleccionarPorCodigoFactura(ByVal pCodigoFactura As String, ByVal pCodigoMnemonico As String) As ValoresBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_SeleccionarPorCodigoFactura")
        Dim oValoresBE As New ValoresBE
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, pCodigoMnemonico)
        db.AddInParameter(dbCommand, "@p_CodigoFactura", DbType.String, pCodigoFactura)

        Dim oDS As New DataSet
        oDS = db.ExecuteDataSet(dbCommand)
        Dim oRow As ValoresBE.ValorRow
        For Each dr As DataRow In oDS.Tables(0).Rows
            oRow = CType(oValoresBE.Valor.NewRow(), ValoresBE.ValorRow)
            InicializarValores(oRow)
            oRow.CodigoNemonico = Convert.ToString(dr("CodigoNemonico"))
            oRow.CodigoFactura = Convert.ToString(dr("CodigoFactura"))
            oRow.CodigoISIN = Convert.ToString(dr("CodigoISIN"))
            oRow.Situacion = Convert.ToString(dr("Situacion"))
            oValoresBE.Valor.AddValorRow(oRow)
            oValoresBE.Valor.AcceptChanges()
        Next
        Return oValoresBE
    End Function
#End Region
    Private Sub AgregarParametros(ByRef db As Database, ByVal dbCommand As DbCommand, ByVal dataRequest As DataSet)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, IIf(oRow.CodigoNemonico = strVacio, DBNull.Value, oRow.CodigoNemonico.ToUpper))
        db.AddInParameter(dbCommand, "@p_CodigoTipoTitulo", DbType.String, IIf(oRow.CodigoTipoTitulo = strVacio, DBNull.Value, oRow.CodigoTipoTitulo.ToUpper))
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, IIf(oRow.Descripcion = strVacio, DBNull.Value, oRow.Descripcion.ToUpper))
        db.AddInParameter(dbCommand, "@p_Agrupacion", DbType.String, IIf(oRow.Agrupacion = strVacio, DBNull.Value, oRow.Agrupacion.ToUpper))
        db.AddInParameter(dbCommand, "@p_CodigoEmisor", DbType.String, IIf(oRow.CodigoEmisor = strVacio, DBNull.Value, oRow.CodigoEmisor.ToUpper))
        db.AddInParameter(dbCommand, "@p_CodigoBursatilidad", DbType.String, IIf(oRow.CodigoBursatilidad = strVacio, DBNull.Value, oRow.CodigoBursatilidad.ToUpper))
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, IIf(oRow.CodigoMoneda = strVacio, DBNull.Value, oRow.CodigoMoneda.ToUpper))
        db.AddInParameter(dbCommand, "@p_NumeroUnidades", DbType.Decimal, oRow.NumeroUnidades)
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, IIf(oRow.CodigoISIN = strVacio, DBNull.Value, oRow.CodigoISIN.ToUpper))
        db.AddInParameter(dbCommand, "@p_ValorUnitario", DbType.Decimal, oRow.ValorUnitario)
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, IIf(oRow.CodigoSBS = strVacio, DBNull.Value, oRow.CodigoSBS.ToUpper))
        db.AddInParameter(dbCommand, "@p_ValorNominal", DbType.Decimal, oRow.ValorNominal)
        db.AddInParameter(dbCommand, "@p_CodigoCalificacion", DbType.String, IIf(oRow.CodigoCalificacion = strVacio, DBNull.Value, oRow.CodigoCalificacion.ToUpper))
        db.AddInParameter(dbCommand, "@p_ValorEfectivoColocado", DbType.Decimal, oRow.ValorEfectivoColocado)
        db.AddInParameter(dbCommand, "@p_TasaEncaje", DbType.Decimal, oRow.TasaEncaje)
        db.AddInParameter(dbCommand, "@p_FechaEmision", DbType.Decimal, oRow.FechaEmision)
        db.AddInParameter(dbCommand, "@p_CodigoTipoCupon", DbType.String, IIf(oRow.CodigoTipoCupon = strVacio, DBNull.Value, oRow.CodigoTipoCupon.ToUpper))
        db.AddInParameter(dbCommand, "@p_FechaVencimiento", DbType.Decimal, oRow.FechaVencimiento)
        db.AddInParameter(dbCommand, "@p_TasaCupon", DbType.Decimal, oRow.TasaCupon)
        db.AddInParameter(dbCommand, "@p_FechaPrimerCupon", DbType.Decimal, oRow.FechaPrimerCupon)
        db.AddInParameter(dbCommand, "@p_CodigoPeriodicidad", DbType.String, IIf(oRow.CodigoPeriodicidad = strVacio, DBNull.Value, oRow.CodigoPeriodicidad.ToUpper))
        db.AddInParameter(dbCommand, "@p_CodigoTipoAmortizacion", DbType.String, IIf(oRow.CodigoTipoAmortizacion = strVacio, DBNull.Value, oRow.CodigoTipoAmortizacion.ToUpper))
        db.AddInParameter(dbCommand, "@p_TasaSpread", DbType.Decimal, oRow.TasaSpread)
        db.AddInParameter(dbCommand, "@p_CodigoIndicador", DbType.String, IIf(oRow.CodigoIndicador = strVacio, DBNull.Value, oRow.CodigoIndicador.ToUpper))
        db.AddInParameter(dbCommand, "@p_ValorIndicador", DbType.Decimal, oRow.ValorIndicador)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, IIf(oRow.Situacion = strVacio, DBNull.Value, oRow.Situacion.ToUpper))
        db.AddInParameter(dbCommand, "@p_TirBase", DbType.Decimal, oRow.BaseTir)
        db.AddInParameter(dbCommand, "@p_TirNDias", DbType.Decimal, oRow.BaseTirDias)
        db.AddInParameter(dbCommand, "@p_CuponBase", DbType.Decimal, oRow.BaseCupon)
        db.AddInParameter(dbCommand, "@p_CuponNDias", DbType.Decimal, oRow.BaseCuponDias)
        db.AddInParameter(dbCommand, "@p_Observacion", DbType.String, IIf(oRow.Observacion = strVacio, DBNull.Value, oRow.Observacion.ToUpper))
        db.AddInParameter(dbCommand, "@p_TipoRenta", DbType.String, IIf(oRow.CodigoRenta = strVacio, DBNull.Value, oRow.CodigoRenta))
        db.AddInParameter(dbCommand, "@p_TipoCuponera", DbType.String, IIf(oRow.TipoCuponera = strVacio, DBNull.Value, oRow.TipoCuponera))
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumentoSBS", DbType.String, IIf(oRow.CodigoTipoInstrumentoSBS = strVacio, DBNull.Value, oRow.CodigoTipoInstrumentoSBS))
        db.AddInParameter(dbCommand, "@p_NemonicoTemporal", DbType.String, IIf(oRow.NemonicoTemporal = strVacio, DBNull.Value, oRow.NemonicoTemporal))
        db.AddInParameter(dbCommand, "@p_CantidadIE", DbType.Decimal, oRow.cantidadIE)
        db.AddInParameter(dbCommand, "@p_RentaFijaIE", DbType.Decimal, oRow.rentaFijaIE)
        db.AddInParameter(dbCommand, "@p_RentaVarIE", DbType.Decimal, oRow.rentaVarIE)
        db.AddInParameter(dbCommand, "@p_GrupoContable", DbType.String, IIf(oRow.GrupoContable = strVacio, DBNull.Value, oRow.GrupoContable))
        db.AddInParameter(dbCommand, "@p_Rescate", DbType.String, IIf(oRow.Rescate = strVacio, DBNull.Value, oRow.Rescate))
        db.AddInParameter(dbCommand, "@p_MonedaCupon", DbType.String, IIf(oRow.MonedaCupon = strVacio, DBNull.Value, oRow.MonedaCupon))
        db.AddInParameter(dbCommand, "@p_MonedaPago", DbType.String, IIf(oRow.MonedaPago = strVacio, DBNull.Value, oRow.MonedaPago))
        db.AddInParameter(dbCommand, "@p_TipoRentaFija", DbType.String, IIf(oRow.TipoRentaFija = strVacio, DBNull.Value, oRow.TipoRentaFija))
        db.AddInParameter(dbCommand, "@p_GeneraInteres", DbType.String, IIf(oRow.GeneraInteres = strVacio, DBNull.Value, oRow.GeneraInteres))
        db.AddInParameter(dbCommand, "@p_TipoCodigoValor", DbType.String, IIf(oRow.TipoCodigoValor = decimalVacio, DBNull.Value, oRow.TipoCodigoValor))
        db.AddInParameter(dbCommand, "@p_AplicaReduccionUnidades", DbType.String, IIf(oRow.AplicaReduccionUnidades = strVacio, DBNull.Value, oRow.AplicaReduccionUnidades))
        db.AddInParameter(dbCommand, "@p_MontoDividendo", DbType.Decimal, oRow.MontoDividendo)
        db.AddInParameter(dbCommand, "@p_Rating", DbType.String, IIf(oRow.Rating = strVacio, DBNull.Value, oRow.Rating))
        db.AddInParameter(dbCommand, "@p_TipoDerivado", DbType.String, IIf(oRow.TipoDerivado = strVacio, DBNull.Value, oRow.TipoDerivado))
        db.AddInParameter(dbCommand, "@p_CodigoTipoDerivado", DbType.String, IIf(oRow.CodigoTipoDerivado = strVacio, DBNull.Value, oRow.CodigoTipoDerivado))
        db.AddInParameter(dbCommand, "@p_FactorFlotante", DbType.Decimal, oRow.FactorFlotante)
        db.AddInParameter(dbCommand, "@p_SpreadFlotante", DbType.Decimal, oRow.SpreadFlotante)
        db.AddInParameter(dbCommand, "@p_CodigoEmisorIE", DbType.String, IIf(oRow.CodigoEmisorIE = strVacio, DBNull.Value, oRow.CodigoEmisorIE))
        db.AddInParameter(dbCommand, "@p_CondicionImpuesto", DbType.String, IIf(oRow.CondicionImpuesto = strVacio, DBNull.Value, oRow.CondicionImpuesto))
        If oRow.IsPosicionActNull Then
            db.AddInParameter(dbCommand, "@p_PosicionAct", DbType.Decimal, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_PosicionAct", DbType.Decimal, oRow.PosicionAct)
        End If
        If oRow.IsPorcPosicionNull Then
            db.AddInParameter(dbCommand, "@p_PorcPosicion", DbType.Decimal, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_PorcPosicion", DbType.Decimal, oRow.PorcPosicion)
        End If
        If oRow.IsCategoriaNull Then
            db.AddInParameter(dbCommand, "@p_Categoria", DbType.String, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_Categoria", DbType.String, oRow.Categoria)
        End If

        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Cambio de tipo de dato a decimal a varchar por nuevos valores en Base interés corrido y nueva columna de estado | 22/05/18
        db.AddInParameter(dbCommand, "@P_BaseInteresCorridoDias", DbType.String, IIf(oRow.BaseInteresCorridoDias = strVacio, "0", oRow.BaseInteresCorridoDias))
        db.AddInParameter(dbCommand, "@P_BaseInteresCorrido", DbType.String, IIf(oRow.BaseInteresCorrido = strVacio, "0", oRow.BaseInteresCorrido))
        db.AddInParameter(dbCommand, "@P_EstadoBaseIC", DbType.String, oRow.EstadoBaseIC)
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Cambio de tipo de dato a decimal a varchar por nuevos valores en Base interés corrido y nueva columna de estado | 22/05/18

        If oRow.IsBaseICNull Then
            db.AddInParameter(dbCommand, "@p_ICBase", DbType.Decimal, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_ICBase", DbType.Decimal, oRow.BaseIC)
        End If
        If oRow.IsBaseICDiasNull Then
            db.AddInParameter(dbCommand, "@p_ICNDias", DbType.Decimal, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_ICNDias", DbType.Decimal, oRow.BaseICDias)
        End If


        If oRow.IsMargenInicialNull Then
            db.AddInParameter(dbCommand, "@p_MargenInicial", DbType.Decimal, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_MargenInicial", DbType.Decimal, oRow.MargenInicial)
        End If
        If oRow.IsMargenMantenimientoNull Then
            db.AddInParameter(dbCommand, "@p_MargenMantenimiento", DbType.Decimal, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_MargenMantenimiento", DbType.Decimal, oRow.MargenMantenimiento)
        End If
        If oRow.IsContractSizeNull Then
            db.AddInParameter(dbCommand, "@p_ContractSize", DbType.Decimal, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_ContractSize", DbType.Decimal, oRow.ContractSize)
        End If
        If oRow.IsFirmaLlamadoNull Then
            db.AddInParameter(dbCommand, "@p_FirmaLlamado", DbType.String, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_FirmaLlamado", DbType.String, IIf(oRow.FirmaLlamado = "", DBNull.Value, oRow.FirmaLlamado))
        End If
        If oRow.IsEstiloNull Then
            db.AddInParameter(dbCommand, "@p_Estilo", DbType.String, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_Estilo", DbType.String, oRow.Estilo)
        End If
        db.AddInParameter(dbCommand, "@P_CodigoMercado", DbType.String, oRow.CodigoMercado)
        db.AddInParameter(dbCommand, "@P_MontoPiso", DbType.Decimal, oRow.MontoPiso)
        db.AddInParameter(dbCommand, "@P_MontoTecho", DbType.Decimal, oRow.MontoTecho)
        db.AddInParameter(dbCommand, "@P_Garante", DbType.String, oRow.Garante)
        db.AddInParameter(dbCommand, "@P_Subyacente", DbType.String, oRow.Subyacente)
        db.AddInParameter(dbCommand, "@P_PrecioEjercicio", DbType.Decimal, oRow.PrecioEjercicio)
        db.AddInParameter(dbCommand, "@P_TamanoEmision", DbType.Decimal, oRow.TamanoEmision)
        db.AddInParameter(dbCommand, "@P_CodigoPortafolioSBS", DbType.String, oRow.CodigoPortafolioSBS)

        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se agrega data a nuevo campo TipoRentaRiesgo | 17/05/18
        db.AddInParameter(dbCommand, "@P_TipoRentaRiesgo", DbType.String, oRow.TipoRentaRiesgo)
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se agrega data a nuevo campo TipoRentaRiesgo | 17/05/18

        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se agrega data a nuevo campo Frecuencia Dividendo | 18/05/18
        db.AddInParameter(dbCommand, "@P_CodigoFrecuenciaPago", DbType.String, oRow.CodigoFrecuenciaDividendo)
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se agrega data a nuevo campo Frecuencia Dividendo | 18/05/18

        'INICIO | ZOLUXIONES | RCE | RU022 - Se agrega nuevo atributo a ValoresBE para campo Subordinario y Precio Devengado | 23/10/18
        db.AddInParameter(dbCommand, "@p_Subordinado", DbType.String, oRow.Subordinado)
        db.AddInParameter(dbCommand, "@p_PrecioDevengado", DbType.String, oRow.PrecioDevengado)
        'FIN | ZOLUXIONES | RCE | RU022 - Se agrega nuevo atributo a ValoresBE para campo Subordinario y Precio Devengado | 23/10/18

        'INICIO | CDA | Ernesto Galarza | Se agrega nuevo atributo a ValoresBE para campo Codigo de factura | 16/01/19
        db.AddInParameter(dbCommand, "@p_CodigoFactura", DbType.String, IIf(oRow.CodigoFactura = strVacio, DBNull.Value, oRow.CodigoFactura))
        'FIN | CDA | Ernesto Galarza | Se agrega nuevo atributo a ValoresBE para campo Codigo de factura  | 16/01/19
        db.AddInParameter(dbCommand, "@p_RatingMandato", DbType.String, IIf(oRow.RatingMandato = strVacio, DBNull.Value, oRow.RatingMandato))
    End Sub
#Region " /* Funciones Insertar */ "
    Public Function Insertar(ByVal oValoresBE As ValoresBE, ByVal dataRequest As DataSet) As String
        Dim Codigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_Insertar")
        oRow = CType(oValoresBE.Valor.Rows(0), ValoresBE.ValorRow)
        AgregarParametros(db, dbCommand, dataRequest)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return Codigo
    End Function
    Public Sub CambiarCodigoTemporal(ByVal codigoActual As String, ByVal nuevoCodigo As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("usp_CambiarCodigoTemporalValor")
        db.AddInParameter(dbCommand, "@nvcCodigoNemonico", DbType.String, codigoActual)
        db.AddInParameter(dbCommand, "@nvcNewCodigoNemonico", DbType.String, nuevoCodigo)
        dbCommand.CommandTimeout = 400
        db.ExecuteNonQuery(dbCommand)
    End Sub
    'OT10952 - 17/11/2017 - Ian Pastor M. Refactorizar código
    Public Sub InsertarDetalleAux(ByVal StrCodigoNemo As String, ByVal StrCodigoSBS As String, ByVal StrCodigoCustodio As String, ByVal StrCodigoCuentaDepositaria As String,
    ByVal StrSituacion As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Valores_InsertarDetalleCustodios")
            db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, StrCodigoNemo)
            db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, StrCodigoSBS)
            db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, StrCodigoCustodio)
            db.AddInParameter(dbCommand, "@p_CodigoCuentaDepositaria", DbType.String, StrCodigoCuentaDepositaria)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, StrSituacion)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Sub ModificarDetalleAux(ByVal StrCodigoNemo As String, ByVal StrCodigoSBS As String, ByVal StrCodigoCustodio As String,
    ByVal StrCodigoCuentaDepositaria As String, ByVal StrSituacion As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Valores_ModificarDetalleCustodios")
            db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, StrCodigoNemo)
            db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, StrCodigoSBS)
            db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, StrCodigoCustodio)
            db.AddInParameter(dbCommand, "@p_CodigoCuentaDepositaria", DbType.String, StrCodigoCuentaDepositaria)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, StrSituacion)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    'OT10952 - Fin
    Public Sub InsertarDetalleCapComp(ByVal StrCodigoNemo As String, ByVal StrCodigoPortafolioSBS As String, ByVal numCapitalCompro As Decimal, ByVal StrSituacion As String,
    ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_Valores_InsertarCapitalFondoAlternativo")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, StrCodigoNemo)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, StrCodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CapitalCompro", DbType.Decimal, numCapitalCompro)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, StrSituacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Sub ModificarDetalleCapComp(ByVal StrCodigoNemo As String, ByVal StrCodigoPortafolioSBS As String, ByVal numCapitalCompro As Decimal,
    ByVal StrSituacion As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_Valores_ModificarCapitalFondoAlternativo")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, StrCodigoNemo)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, StrCodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CapitalCompro", DbType.Decimal, numCapitalCompro)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, StrSituacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Function verificarExistencia(ByVal StrCodigoNemo As String, ByVal StrCodigoSBS As String, ByVal StrCodigoCustodio As String,
    ByVal StrCodigoCuentaDepositaria As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_VerificarExistencia")
        Dim DtTabla As New DataTable
        Dim IntI As Integer
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, StrCodigoNemo)
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, StrCodigoSBS)
        db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, StrCodigoCustodio)
        DtTabla = db.ExecuteDataSet(dbCommand).Tables(0)
        If DtTabla.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function verificarExistenciaCapitalCompro(ByVal StrCodigoNemo As String, ByVal StrCodigoSBS As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Valores_VerificarExistenciaCapitalFondoAlt")
        Dim DtTabla As New DataTable
        Dim IntI As Integer
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, StrCodigoNemo)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, StrCodigoSBS)
        DtTabla = db.ExecuteDataSet(dbCommand).Tables(0)
        If DtTabla.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
#End Region
#Region " /* Funciones Modificar */"
    Public Function Modificar(ByVal oValoresBE As ValoresBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_Modificar")
        oRow = CType(oValoresBE.Valor.Rows(0), ValoresBE.ValorRow)
        AgregarParametros(db, dbCommand, dataRequest)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function ModificarDetalleCustodios(ByVal StrCodigoNemo As String, ByVal dtDetalleCotizacion As DataTable, ByVal dataRequest As DataSet) As Boolean
        Dim StrCodigoCustodio As String
        Dim StrSituacion As String
        Dim StrCodigoCuentaDepositaria As String
        Dim StrCodigoSBS As String
        If (dtDetalleCotizacion.Rows.Count > 0) Then
            For Each filaLinea As DataRow In dtDetalleCotizacion.Rows
                StrCodigoCuentaDepositaria = ""
                StrCodigoCustodio = filaLinea("CodigoCustodio").ToString().Trim()
                StrSituacion = filaLinea("Situacion").ToString().Trim()
                StrCodigoSBS = filaLinea("CodigoPortafolioSBS")
                If verificarExistencia(StrCodigoNemo, StrCodigoSBS, StrCodigoCustodio, StrCodigoCuentaDepositaria) Then
                    ModificarDetalleAux(StrCodigoNemo, StrCodigoSBS, StrCodigoCustodio, StrCodigoCuentaDepositaria, StrSituacion, dataRequest)
                Else
                    InsertarDetalleAux(StrCodigoNemo, StrCodigoSBS, StrCodigoCustodio, StrCodigoCuentaDepositaria, StrSituacion, dataRequest)
                End If
            Next
        End If
    End Function
    Public Function ModificarDetalleCapitalCompro(ByVal StrCodigoNemo As String, ByVal dtDetalleCapitalCompro As DataTable, ByVal dataRequest As DataSet) As Boolean
        Dim StrCodigoCustodio As String
        Dim StrSituacion As String
        Dim numCapitalCompro As Decimal
        Dim StrCodigoPortafolioSBS As String
        If (dtDetalleCapitalCompro.Rows.Count > 0) Then
            For Each filaLinea As DataRow In dtDetalleCapitalCompro.Rows
                StrCodigoPortafolioSBS = filaLinea("CodigoPortafolioSBS")
                numCapitalCompro = filaLinea("CapitalCompro").ToString().Trim()
                StrSituacion = filaLinea("Situacion").ToString().Trim()
                If verificarExistenciaCapitalCompro(StrCodigoNemo, StrCodigoPortafolioSBS) Then
                    ModificarDetalleCapComp(StrCodigoNemo, StrCodigoPortafolioSBS, numCapitalCompro, StrSituacion, dataRequest)
                Else
                    InsertarDetalleCapComp(StrCodigoNemo, StrCodigoPortafolioSBS, numCapitalCompro, StrSituacion, dataRequest)
                End If
            Next
        End If
    End Function
#End Region
#Region " /* Funciones Eliminar */"
    ''' <summary>
    ''' Elimina un expediente de ValoresBE table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoNemonico As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_Eliminar")
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function EliminarDetalleCustodios(ByVal codigoNemonico As String, ByVal dataRequest As DataSet) As Boolean
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_EliminarDetalleCustodios")
            db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
            db.ExecuteNonQuery(dbCommand)
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region
    Public Sub InicializarValores(ByRef oRow As ValoresBE.ValorRow)
        oRow.CodigoNemonico = strVacio
        oRow.CodigoTipoTitulo = strVacio
        oRow.Descripcion = strVacio
        oRow.Agrupacion = strVacio
        oRow.CodigoEmisor = strVacio
        oRow.CodigoBursatilidad = strVacio
        oRow.CodigoMoneda = strVacio
        oRow.NumeroUnidades = decVacio
        oRow.CodigoISIN = strVacio
        oRow.ValorUnitario = decVacio
        oRow.CodigoSBS = strVacio
        oRow.ValorNominal = decVacio
        oRow.CodigoCalificacion = strVacio
        oRow.ValorEfectivoColocado = decVacio
        oRow.TasaEncaje = decVacio
        oRow.FechaEmision = decVacio
        oRow.CodigoTipoCupon = strVacio
        oRow.FechaVencimiento = decVacio
        oRow.TasaCupon = decVacio
        oRow.FechaPrimerCupon = decVacio
        oRow.CodigoPeriodicidad = strVacio
        oRow.CodigoTipoAmortizacion = strVacio
        oRow.TasaSpread = decVacio
        oRow.CodigoIndicador = strVacio
        oRow.ValorIndicador = decVacio
        oRow.Situacion = strVacio
        oRow.BaseTir = strVacio
        oRow.BaseTirDias = strVacio
        oRow.BaseCupon = strVacio
        oRow.BaseCuponDias = strVacio
        oRow.CodigoRenta = strVacio
        oRow.TipoCuponera = strVacio
        oRow.Observacion = strVacio
        oRow.CodigoTipoInstrumentoSBS = strVacio
        oRow.cantidadIE = decVacio
        oRow.rentaFijaIE = decVacio
        oRow.rentaVarIE = decVacio
        oRow.GrupoContable = strVacio
        oRow.Rescate = strVacio
        oRow.MonedaCupon = strVacio
        oRow.MonedaPago = strVacio
        oRow.AplicaReduccionUnidades = strVacio
        oRow.MontoDividendo = decVacio
        oRow.Rating = strVacio
        oRow.TipoDerivado = strVacio


        oRow.TipoCodigoValor = strVacio
        oRow.NemonicoTemporal = strVacio
        oRow.CodigoFactura = strVacio
        oRow.CondicionImpuesto = strVacio
        oRow.EstadoBaseIC = strVacio
        oRow.CodigoMercado = strVacio
        oRow.CodigoEmisorIE = strVacio
        oRow.TipoRentaRiesgo = strVacio
        oRow.CodigoFrecuenciaDividendo = strVacio
        oRow.Garante = strVacio
        oRow.Subyacente = strVacio
        oRow.CodigoPortafolioSBS = strVacio
        oRow.CodigoTipoDerivado = strVacio
        oRow.FactorFlotante = decVacio
        oRow.SpreadFlotante = decVacio
        oRow.MargenInicial = decVacio
        oRow.MargenMantenimiento = decVacio
        oRow.ContractSize = decVacio
        oRow.FirmaLlamado = decVacio
        oRow.ValorUnitarioActual = decVacio
        oRow.MontoPiso = decVacio
        oRow.MontoTecho = decVacio
        oRow.TamanoEmision = decVacio
        oRow.PrecioEjercicio = decVacio
        oRow.BaseIC = decVacio
        oRow.BaseICDias = decVacio
        oRow.BaseTir = decVacio
        oRow.BaseTirDias = decVacio
        oRow.Subordinado = decVacio
        oRow.PrecioDevengado = decVacio
        oRow.BaseInteresCorridoDias = decVacio
        oRow.BaseInteresCorrido = decVacio
        oRow.DiasTTasaVariable = decVacio
        oRow.TipoTasaVariable = strVacio
        oRow.TasaVariable = decVacio
        oRow.RatingMandato = strVacio
    End Sub
#Region " /* Funciones Personalizadas de Alberto*/"
    Public Function ListarValoresOI(ByVal CodigoISIN As String, ByVal dataRequest As DataSet) As DataSet
        Dim ds As New DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("SeleccionarValorIO")
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, CodigoISIN)
        Dim oValoresBE As New ValoresBE
        db.LoadDataSet(dbCommand, ds, "ValoresOI")
        Return ds
    End Function
#End Region
    Public Function GenerarValorizacionCartera(ByVal codigoPortafolio As String, ByVal fechaOperacion As Decimal, ByVal EntExtPrecio As String,
    ByVal EntExtTipoCambio As String, ByVal TipoValorizacion As String, ByVal dataRequest As DataSet) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ValorizacionCartera_Generar2")
            dbCommand.CommandTimeout = 1020
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaOperacion)
            db.AddInParameter(dbCommand, "@p_EntidadExtPrecio", DbType.String, EntExtPrecio)
            db.AddInParameter(dbCommand, "@p_EntidadExtTipoCambio", DbType.String, EntExtTipoCambio)
            db.AddInParameter(dbCommand, "@p_TipoValorizacion", DbType.String, TipoValorizacion)
            db.AddInParameter(dbCommand, "@p_usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            Return Convert.ToInt32(db.ExecuteScalar(dbCommand))
        End Using
    End Function
    Public Function GenerarValorizacionCurvaCuponCero(ByVal codigoPortafolio As String, ByVal fechaOperacion As Decimal, ByVal datarequest As DataSet) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valorizacion_GenerarCurvaCuponCero")
        db.AddInParameter(dbCommand, "@portafolio", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@FechaProceso", DbType.Decimal, fechaOperacion)
        db.AddInParameter(dbCommand, "@usuario", DbType.String, DataUtility.ObtenerValorRequest(datarequest, "Usuario"))
        db.AddInParameter(dbCommand, "@fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(datarequest, "Fecha")))
        db.AddInParameter(dbCommand, "@Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(datarequest, "Fecha")))
        db.AddInParameter(dbCommand, "@Host", DbType.String, DataUtility.ObtenerValorRequest(datarequest, "Host"))
        Return Convert.ToInt32(db.ExecuteScalar(dbCommand))
    End Function
    Public Function ExtornarValorizacionCartera(ByVal codigoPortafolio As String, ByVal fechaOperacion As Decimal, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("ValorizacionCartera_Extornar")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaOperacion)
        db.AddInParameter(dbCommand, "@p_usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function VerificarExtornoValorizacionCartera(ByVal codigoPortafolio As String, ByVal fechaOperacion As Decimal, ByVal dataRequest As DataSet) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("ValorizacionCartera_VerificarExtorno")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaOperacion)
        Return db.ExecuteScalar(dbCommand)
    End Function
    Public Function SeleccionarPorFiltroValores(ByVal codigoInterno As String, ByVal nombreCompleto As String) As ValoresBE
        Dim oValoresBE As New ValoresBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_SeleccionarPorFiltroValor")
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, codigoInterno)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, nombreCompleto)
        db.LoadDataSet(dbCommand, oValoresBE, "Valor")
        Return oValoresBE
    End Function
    Public Function SeleccionarPorFiltroValoresAprob(ByVal codigoInterno As String, ByVal nombreCompleto As String) As ValoresBE
        Dim oValoresBE As New ValoresBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_SeleccionarPorFiltroValoresAprob")
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, codigoInterno)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, nombreCompleto)
        db.LoadDataSet(dbCommand, oValoresBE, "Valor")
        Return oValoresBE
    End Function
    Public Function SeleccionarPorFiltroValoresFuturo(ByVal codigoInterno As String, ByVal nombreCompleto As String) As ValoresBE
        Dim oValoresBE As New ValoresBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_SeleccionFiltroValorAprobadoFuturo")
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, codigoInterno)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, nombreCompleto)
        db.LoadDataSet(dbCommand, oValoresBE, "Valor")
        Return oValoresBE
    End Function
    Public Function SeleccionarValorizacionPromedio(ByVal codigoPortafolio As String, ByVal codigoNemonico As String, ByVal codigoISIN As String,
    ByVal diasBase As Integer) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("ValorizacionCarteraPromedio_Listar")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_DiasBase", DbType.Int32, diasBase)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
        If IsDBNull(db.ExecuteScalar(dbCommand)) = True Then
            Return 0
        Else
            Return Convert.ToDecimal(db.ExecuteScalar(dbCommand))
        End If
    End Function
    Public Function SeleccionarValorizacion(ByVal codigoPortafolio As String, ByVal fechaOperacion As Decimal, ByVal TipoValorizacion As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("ValorizacionCartera_Listar")
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.String, fechaOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_TipoValorizacion", DbType.String, TipoValorizacion)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ReporteSeleccionarValorizacion(ByVal codigoPortafolio As String, ByVal fechaOperacion As Decimal, ByVal TipoValorizacion As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("ReporteValorizacionCartera_Listar")
        dbCommand.CommandTimeout = 300
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.String, fechaOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_TipoValorizacion", DbType.String, TipoValorizacion)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarPorFiltroNemonico(ByVal codigoNemonico As String, ByVal descripcion As String) As ValoresBE
        Dim oValoresBE As New ValoresBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_SeleccionarPorFiltroNemonico")
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.LoadDataSet(dbCommand, oValoresBE, "Valor")
        Return oValoresBE
    End Function
    Public Function VerificarCustodia(ByVal codigoMnemonico As String) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_VerificarDetalleCustodios")
        Dim lngcontar As Long
        Dim IntI As Integer
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoMnemonico)
        lngcontar = db.ExecuteScalar(dbCommand)
        Return lngcontar
    End Function
    Public Function ObtenerTemporal() As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_SeleccionarCodigoTemporal")
        Dim COD As String
        COD = db.ExecuteScalar(dbCommand)
        Return COD
    End Function
    Public Function VerificarCuponeraNormal(ByVal codigoMnemonico As String) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_VerificarCuponeraNormal")
        Dim lngcontar As Long
        Dim IntI As Integer
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoMnemonico)
        lngcontar = db.ExecuteScalar(dbCommand)
        Return lngcontar
    End Function
    Public Function VerificarCuponeraEspecial(ByVal codigoMnemonico As String) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_VerificarCuponeraEspecial")
        Dim lngcontar As Long
        Dim IntI As Integer
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoMnemonico)
        lngcontar = db.ExecuteScalar(dbCommand)
        Return lngcontar
    End Function
    Public Function ActualizarMnemonicoValores(ByVal strCodigoMnemonicoReal As String, ByVal strCodigoMnemonicoTemporal As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_VerificarCuponeraEspecial")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, strCodigoMnemonicoReal)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, strCodigoMnemonicoTemporal)
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function ObtenerUltimaFechaValorizacion(ByVal portafolio As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valorizacion_ObtenerUltimaFecha")
        db.AddInParameter(dbCommand, "@portafolio", DbType.String, portafolio)
        Return Convert.ToString(db.ExecuteScalar(dbCommand))
    End Function
    Public Function BuscarPrecioUnitario(ByVal codigoNemonico As String) As Decimal
        Dim precioUnitario As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_BuscarPrecioUnitario")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        precioUnitario = Convert.ToDecimal(db.ExecuteScalar(dbCommand))
        Return Convert.ToDecimal(precioUnitario)
    End Function
    Public Function SeleccionarSaldoNemonicos(ByVal fecha As Decimal, ByVal portafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("ValorizacionCartera_SeleccionarNemonicoSaldo")
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.String, fecha)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, portafolio)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ObtenerNemonicosError() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("ValorizacionCartera_SeleccionarNemonicoError")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Sub EliminarCuponera()
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("ValorizacionCartera_Eliminarcuponera")
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Function VerificaNemonicosValorizacion() As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valorizacion_VerificaNemonicos")
        Return Convert.ToInt32(db.ExecuteScalar(dbCommand))
    End Function
    Public Function VerificarTasasCurva(ByVal fecha As Decimal) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valorizacion_VeificarTasasCurva")
        db.AddInParameter(dbCommand, "@fechaProceso", DbType.String, fecha)
        Return Convert.ToInt32(db.ExecuteScalar(dbCommand))
    End Function
    Public Function VerificarFondoInversion(ByVal codigonemonico As String) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VerificarFondoInversion")
        db.AddInParameter(dbCommand, "@p_codigonemonico", DbType.String, codigonemonico)
        Return Convert.ToInt32(db.ExecuteScalar(dbCommand))
    End Function
    Public Function VerificarCalculoComisiones(ByVal codigonemonico As String, ByVal codigotercero As String, ByVal fechaoperacion As Decimal,
    ByVal tipooperacion As String) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VerificarCalculoComisiones")
        db.AddInParameter(dbCommand, "Codigonemonico", DbType.String, codigonemonico)
        db.AddInParameter(dbCommand, "CodigoTercero", DbType.String, codigotercero)
        db.AddInParameter(dbCommand, "FechaOperacion", DbType.Decimal, fechaoperacion)
        db.AddInParameter(dbCommand, "TipoOperacion", DbType.String, tipooperacion)
        Return Convert.ToInt32(db.ExecuteScalar(dbCommand))
    End Function
    Public Sub EliminarValoracionCartera(ByVal TipoValoracion As String, ByVal Portafolio As String, ByVal fechavaloracion As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valorizacion_EliminarCartera")
        db.AddInParameter(dbCommand, "@p_TipoValorizacion", DbType.String, TipoValoracion)
        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, Portafolio)
        db.AddInParameter(dbCommand, "@p_fechavaloracion", DbType.Decimal, fechavaloracion)
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Function ExisteValoracion(ByVal Portafolio As String, ByVal fechavaloracion As Decimal) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valorizacion_ExisteCartera")
        db.AddInParameter(dbCommand, "p_Portafolio", DbType.String, Portafolio)
        db.AddInParameter(dbCommand, "p_fechavaloracion", DbType.Decimal, fechavaloracion)
        Return Convert.ToInt32(db.ExecuteScalar(dbCommand))
    End Function
    Public Function ValoracionValidarOperaciones(ByVal codigoPortafolio As String, ByVal fecha As Decimal) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "ValorizacionCartera_ValidarGeneracion"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_fechavaloracion", DbType.Decimal, fecha)
        Return CInt(db.ExecuteScalar(dbCommand))
    End Function
    Public Function ValoresRetornarFechaVencimiento(ByVal codigonemonico As String) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "Valores_RetornarFechaVencimiento"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "@p_Codigonemonico", DbType.String, codigonemonico)
        Return Convert.ToDecimal(db.ExecuteScalar(dbCommand))
    End Function
    Public Function ValidarFueNemonicoTemporal(ByVal codigoNemonico As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dtDatos As DataTable
        Dim result As Boolean = False
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_ValidarFueAlgunaVezTemporal")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        dtDatos = db.ExecuteDataSet(dbCommand).Tables(0)
        If dtDatos.Rows(0)(0).ToString = "" Then
            result = False
        Else
            result = True
        End If
        Return result
    End Function
    Public Function OrdenInversionRetornarPH(ByVal codigoorden As String, ByVal portafolio As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_RetornarPH")
        db.AddInParameter(dbCommand, "@codigoorden", DbType.String, codigoorden)
        db.AddInParameter(dbCommand, "@codigoportafoliosbs", DbType.String, portafolio)
        Return Convert.ToString(db.ExecuteScalar(dbCommand))
    End Function
    Public Function ObtenerDatosHipervalorizador(ByVal codigoNemonico As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_SeleccionarDatosHipervalorizador")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        Dim oValoresBono As New DataSet
        db.LoadDataSet(dbCommand, oValoresBono, "Valor")
        Return oValoresBono
    End Function
    Public Function ExistenciaCuponera(ByVal dataRequest As DataSet, ByVal codigoNemonico As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("ValoresCuponera_Existencia")
        Dim oValoresBE As New ValoresBE
        db.AddInParameter(dbCommand, "@p_Nemonico", DbType.String, codigoNemonico)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarTipoRentaPorCodigoNemonico(ByVal CodigoNemonico As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Valores_SeleccionarTipoRentaPorNemonico")
        Dim oValoresBE As New ValoresBE
        db.AddInParameter(dbCommand, "@p_Nemonico", DbType.String, CodigoNemonico)
        Return Convert.ToString(db.ExecuteScalar(dbCommand))
    End Function
    Public Function ValidarDRL_ParaValoracionEstimada(ByVal CodigoPortafolio As String, ByVal FechaProceso As Decimal) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_ValidarDRL")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, FechaProceso)
        Return Convert.ToInt32(db.ExecuteScalar(dbCommand))
    End Function
    Public Function Aprobacion_InstrumentosRiesgo(ByVal CodigoNemonico As String, ByVal Fecha As Decimal, ByVal Obs As String, ByVal Operacion As String,
    ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_Aprobacion_InstrumentosRiesgo")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, CodigoNemonico)
        db.AddInParameter(dbCommand, "@p_FechaAprobacion", DbType.Decimal, Fecha)
        db.AddInParameter(dbCommand, "@p_ObsAprobacion", DbType.String, Obs)
        db.AddInParameter(dbCommand, "@p_Operacion", DbType.String, Operacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        Dim dsValor As New DataSet
        db.LoadDataSet(dbCommand, dsValor, "Valor")
        Return dsValor
    End Function
    Public Function Reporte_AutorizacionRiesgo(ByVal codigoMnemonico As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Reporte_AutorizacionRiesgo")
        Dim DstTabla As New DataSet
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoMnemonico)
        DstTabla = db.ExecuteDataSet(dbCommand)
        Return DstTabla.Tables(0)
    End Function
    Public Sub CalculaMontoInversion(ByVal fechavaloracion As Decimal, CodigoPortafolio As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_calculaMontoInversion")
            db.AddInParameter(dbCommand, "@p_FechaOperacionActual", DbType.Decimal, fechavaloracion)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.Decimal, CodigoPortafolio)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Sub BorraMontoInversion(ByVal fechavaloracion As Decimal, CodigoPortafolio As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_borraMontoInversion")
        db.AddInParameter(dbCommand, "@p_FechaOperacionActual", DbType.Decimal, fechavaloracion)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.Decimal, CodigoPortafolio)
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Function InsertaTraspasoValores(ByVal CodigoIsinOrigen As String, CodigoIsinDestino As String, FechaProceso As Decimal, dataRequest As DataSet) As DataTable
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_InsertaTraspasoValores")
            db.AddInParameter(dbCommand, "@P_CodigoIsinOrigen", DbType.String, CodigoIsinOrigen)
            db.AddInParameter(dbCommand, "@P_CodigoIsinDestino", DbType.String, CodigoIsinDestino)
            db.AddInParameter(dbCommand, "@P_FechaProceso", DbType.Decimal, FechaProceso)
            db.AddInParameter(dbCommand, "@P_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@P_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            Return db.ExecuteDataSet(dbCommand).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ValidaTraspaso(ByVal CodigoIsinOrigen As String, CodigoIsinDestino As String) As String
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ValidaTraspaso")
            db.AddInParameter(dbCommand, "@P_CodigoIsinOrigen", DbType.String, CodigoIsinOrigen)
            db.AddInParameter(dbCommand, "@P_CodigoIsinDestino", DbType.String, CodigoIsinDestino)
            db.AddOutParameter(dbCommand, "@P_Mensaje", DbType.String, 100)
            db.ExecuteNonQuery(dbCommand)
            Return CType(db.GetParameterValue(dbCommand, "@P_Mensaje"), String)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ActualizaNemomicoIsin(ByVal TipoActualizacion As String, CodigoMnemonico As String, CodigoIsin As String) As String
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ActualizaNemomicoIsin")
            db.AddInParameter(dbCommand, "@p_TipoActualizacion", DbType.String, TipoActualizacion)
            db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, CodigoMnemonico)
            db.AddInParameter(dbCommand, "@p_CodigoIsin", DbType.String, CodigoIsin)
            db.ExecuteNonQuery(dbCommand)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function NominalBono(ByVal CodigoPortafolio As String, ByVal CodigoIsin As String, ByVal Fecha As Decimal, ByVal Unidades As Decimal) As Decimal
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_NominalBonoporUnidad")
            db.AddInParameter(dbCommand, "@p_Unidades", DbType.Decimal, Unidades)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_CodigoIsin", DbType.String, CodigoIsin)
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, Fecha)
            db.AddOutParameter(dbCommand, "@p_MontoNominal", DbType.Decimal, 100)
            db.ExecuteNonQuery(dbCommand)
            Return CType(db.GetParameterValue(dbCommand, "@p_MontoNominal"), Decimal)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function TasaBono(CodigoIsin As String) As Decimal
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_TasaBono")
            db.AddInParameter(dbCommand, "@p_CodigoIsin", DbType.String, CodigoIsin)
            db.AddOutParameter(dbCommand, "@p_Tasa", DbType.String, 22)
            db.ExecuteNonQuery(dbCommand)
            Return CType(db.GetParameterValue(dbCommand, "@p_Tasa"), Decimal)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CuponeraSWAP(ByVal CadenaNemonico As String, Fecha As Decimal, TasaCupon As Decimal, MontoNominal As Decimal,
    TasaCuponOriginal As Decimal, MontoNominalOriginal As Decimal) As DataTable
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_CuponeraSWAP")
            db.AddInParameter(dbCommand, "@p_CadenaNemonico", DbType.String, CadenaNemonico)
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, Fecha)
            db.AddInParameter(dbCommand, "@p_TasaCupon", DbType.Decimal, TasaCupon)
            db.AddInParameter(dbCommand, "@P_MontoNominal", DbType.Decimal, MontoNominal)
            db.AddInParameter(dbCommand, "@p_TasaCuponOriginal", DbType.Decimal, TasaCuponOriginal)
            db.AddInParameter(dbCommand, "@P_MontoNominalOriginal", DbType.Decimal, MontoNominalOriginal)
            Return db.ExecuteDataSet(dbCommand).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10362 - 18/05/2017 - Carlos Espejo
    'Descripcion: Inserta los instrumentos temporales
    Public Function InsertarPrecioISINDetalle(ByVal CodigoUsuario As String, CodigoIsin As String, CodigoNemonico As String)
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ins_PrecioISINDetalle")
            db.AddInParameter(dbCommand, "@p_CodigoUsuario", DbType.String, CodigoUsuario)
            db.AddInParameter(dbCommand, "@p_CodigoIsin", DbType.String, CodigoIsin)
            db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, CodigoNemonico)
            db.ExecuteNonQuery(dbCommand)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10362 - 18/05/2017 - Carlos Espejo
    'Descripcion: Borrar los instrumentos temporales
    Public Function BorrarPrecioISINDetalle(ByVal CodigoUsuario As String)
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_del_PrecioISINDetalle")
            db.AddInParameter(dbCommand, "@p_CodigoUsuario", DbType.String, CodigoUsuario)
            db.ExecuteNonQuery(dbCommand)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10362 - 18/05/2017 - Carlos Espejo
    'Descripcion: Lista los datos del reporte
    Public Function ListaPrecioInstrumento(ByVal FechaInicio As Decimal, FechaFin As Decimal, CodigoUsuario As String) As DataTable
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_lis_PrecioInstrumento")
            db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.String, FechaInicio)
            db.AddInParameter(dbCommand, "@p_FechaFinal", DbType.Decimal, FechaFin)
            db.AddInParameter(dbCommand, "@p_CodigoUsuario", DbType.String, CodigoUsuario)
            Return db.ExecuteDataSet(dbCommand).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10362 - 18/05/2017 - Carlos Espejo
    'Descripcion: Lista los instrumento del reporte por cantidad de registros en VP
    Public Function ListaOrdenPrecioInstrumento(CodigoUsuario As String) As DataTable
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_lis_OrdenPrecioInstrumento")
            db.AddInParameter(dbCommand, "@p_CodigoUsuario", DbType.String, CodigoUsuario)
            Return db.ExecuteDataSet(dbCommand).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Listar valores por tipo de renta
    Public Function ListarValores(ByVal CodigoNemonico As String, ByVal FechaConsulta As Decimal, TipoRenta As String) As ValoresEList
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim oReader As IDataReader
        Dim lValorList As New ValoresEList
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_lis_Valores")
        db.AddInParameter(dbCommand, "@P_CodigoNemonico", DbType.String, CodigoNemonico)
        db.AddInParameter(dbCommand, "@p_FechaConsulta", DbType.Decimal, FechaConsulta)
        db.AddInParameter(dbCommand, "@p_TipoRenta", DbType.String, TipoRenta)
        oReader = db.ExecuteReader(dbCommand)
        While oReader.Read()
            Dim lValor As New ValoresE
            lValor.Nemonico = oReader.GetString(0)
            lValor.Categoria = oReader.GetString(1)
            lValorList.Add(lValor)
        End While
        oReader.Close()
        Return lValorList
    End Function
End Class
