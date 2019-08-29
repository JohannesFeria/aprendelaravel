Imports Microsoft.VisualBasic
Imports System.Data

Public Class ParametrosSIT

#Region "/* Parametria */"

    Public Const CTERCERO_TERCERO As String = "Tercero"
    Public Const CTERCERO_INTERMEDIARIO As String = "Intermediario"

    Public Const ESTADO_ACTIVO As String = "A"
    Public Const ESTADO_INACTIVO As String = "I"

    Public Const CODIGO_NEGOCIO_FONDO As String = "FOND"
    Public Const CODIGO_NEGOCIO_ADMINISTRADOR As String = "ADMI"

    Public Const CLASIFICACIONTERCERO_TERCERO As String = "T"
    Public Const CLASIFICACIONTERCERO_INTERMEDIARIO As String = "I"

    Public Const MERCADO_LOCAL As String = "1"
    Public Const MERCADO_EXTERIOR As String = "2"

    Public Const FECHA_FONDO As String = "FechaFondo"
    Public Const FECHA_NEGOCIO As String = "FechaNegocio"
    Public Const FECHA_CONSTITUCION As String = "FechaConstitucion"

    Public Const GRUPO_FONDO As String = "GrupoFondo"
    Public Const MULTIFONDO As String = "Multifondo"
    Public Const TIPOFONDO As String = "TipoFondo"
    Public Const FONDOFIR As String = "FondoFir"
    Public Const VALORACION_MENSUAL As String = "VALMENSUAL"
    Public Const VECTOR_PRECIO_VALORIZACION As String = "VEP_VALORI"

    Public Const PORTAFOLIO_MULTIFONDOS As String = ""
    Public Const PORTAFOLIO_ADMINISTRA As String = ""

    Public Const PORTAFOLIO_FONDO0 As String = "4"
    Public Const PORTAFOLIO_FONDO1 As String = "1"
    Public Const PORTAFOLIO_FONDO2 As String = "2"
    Public Const PORTAFOLIO_FONDO3 As String = "3"

    Public Const TABLAS_TBL_T01 As String = "TBL_T01" '<TipoValorizacion>
    Public Const TABLAS_TBL_T02 As String = "TBL_T02" '<TipoCupon>
    Public Const TABLAS_TBL_T03 As String = "TBL_T03" '<TipoAmortizacion>
    Public Const TABLAS_TBL_T04 As String = "TBL_T04" '<Bursatilidad>
    Public Const TABLAS_TBL_T05 As String = "TBL_T05" '<PeriodoLibor>
    Public Const TABLAS_TBL_T06 As String = "TBL_T06" '<Periodicidad>

    Public Const TIPO_CUPON As String = "TIPOCUPON"
    Public Const TIPO_CUPON_A_DESCUENTO As String = "A_DESCUENTO"
    Public Const TIPO_CUPON_TASA_FIJA As String = "TASA_FIJA"
    Public Const TIPO_CUPON_TASA_VARIABLE As String = "TASA_VARIABLE"

    Public Const TIPO_AMORTIZACION As String = "TIPOAMORTI"
    Public Const TIPO_AMORTIZACION_A_VENCIMIENTO As String = "AL_VENCIMIENTO"
    Public Const TIPO_AMORTIZACION_MENSUAL As String = "MENSUAL"
    Public Const TIPO_AMORTIZACION_BIMENSUAL As String = "BIMENSUAL"
    Public Const TIPO_AMORTIZACION_TRIMESTRAL As String = "TRIMENSTRAL"
    Public Const TIPO_AMORTIZACION_CUATRIMESTRAL As String = "CUATRIMENSTRAL"
    Public Const TIPO_AMORTIZACION_SEMESTRAL As String = "SEMESTRAL"
    Public Const TIPO_AMORTIZACION_ANUAL As String = "TRIMESTRAL"

    Public Const TIPO_RENTA_FIJA As String = "RENTA_FIJA"
    Public Const TIPO_RENTA_VARIABLE As String = "RENTA_VARIABLE"
    Public Const TIPO_RENTA_DERIVADOS As String = "OPERACIONES_FX"
    Public Const TIPO_OPER_FUTUROS As String = "OPERACIONES FUTUROS"

    Public Const GRUPO_FACTOR_EMISOR As String = "1"
    Public Const GRUPO_FACTOR_EMISION As String = "2"

    Public Const USUARIOS_ENVIO_FASEI As String = "EMAILF01"
    Public Const USUARIOS_ENVIO_FASEII As String = "EMAILF02"

    Public Const SERV_CORREO As String = "SERVMAIL"
    Public Const EMAIL_FORM As String = "EMAIL_FROM"

    Public Const PREV_OI_APROBADO As String = "APR"
    Public Const PREV_OI_INGRESADO As String = "ING"
    Public Const PREV_OI_EJECUTADO As String = "EJE"
    Public Const PREV_OI_PENDIENTE As String = "PND"
    Public Const PREV_OI_ELIMINADO As String = "ELI"

    Public Const ESTADO_PREV_OI As String = "EST_PREVOI"

    Public Const TR_RENTA_VARIABLE As Integer = 2
    Public Const TR_RENTA_FIJA As Integer = 1
    Public Const TR_DERIVADOS As Integer = 3

    Public Const CORREOS_ALTA_INSTRUMENTOS As String = "EMAIL_ALTA"

    Public Const MODALIDAD_FORW As String = "MODFORW"
    Public Const CLASE_INSTRUMENTO_FX As String = "CLASE_IFX"
    Public Const INDICE_PRECIO_TASA As String = "INDICE_PT"
    Public Const TIPO_CARGO_INVERSIONES As String = "CARGINVER"
    Public Const RUTA_APROB_EXC_TRADER As String = "RUTA_AET"

    Public Const VALIDACION_PREVOI As String = "V"
    Public Const EJECUCION_PREVOI As String = "E"

    Public Const ESTADO_CARTA As String = "EST_CARTA"
    Public Const ESTADO_CARTA_IMPRESION As String = "EST_CARTAI"

    Public Const ESTADO_CARTA_IMP As String = "1"
    Public Const ESTADO_CARTA_NIMP As String = "2"
    Public Const ESTADO_CARTA_AIMP As String = "3"
    Public Const ESTADO_CARTA_PIMP As String = "4"

    Public Const ESTADO_CARTA_APR As String = "2"
    Public Const ESTADO_CARTA_PND As String = "1"
    Public Const ESTADO_CARTA_FRM As String = "3"

    Public Const IMPRESION_CARTAS As Decimal = 1
    Public Const RUTA_FIRMA_CARTA As String = "RUTAFIRMA"
    Public Const RUTA_FIRMA_SIT As String = "../../../../Imagenes/"
    Public Const MOTIVO_EXTORNO As String = "MTVO_EXT"
    Public Const ESTADO_EXTORNO As String = "EST_EXT_OC"

    Public Const ROL_CARTA As String = "ROL_CARTA"
    Public Const FRM_CARTA As String = "2"
    Public Const ADM_CARTA As String = "1"
    Public Const SITUACION As String = "Situación"
    Public Const TIPOFIRMA As String = "TipoFirma" 'OT-10795
    Public Const TIPOFIRMA_A As String = "A" 'OT-10795
    Public Const TIPOFIRMA_B As String = "B" 'OT-10795

    Public Const TIPO_TRAMO As String = "TIPOTRAMO"
    Public Const EPU_PERU_CROSSES As String = "1"
    Public Const EPU_US_CROSSES As String = "2"
    Public Const EPU_CASH As String = "3"

    Public Const PROC_INGRESAR As String = "Ingresar"
    Public Const PROC_MODIFICAR As String = "Modificar"
    Public Const PROC_ELIMINAR As String = "Eliminar"

    Public Const OPC_REPORTE_FALLAS_OI As String = "RPFALLS_OI"
    Public Const OPC_OI_MODIFICADA As Decimal = 1
    Public Const OPC_OI_ELIMINADA As Decimal = 2
    Public Const MEDIO_TRANSMISION As String = "MNPREVOI"
    Public Const TIPO_GRUPO_TRADING As String = "TIPOGRPTRD"
    Public Const TIPO_GRUPO_TRADING_TI As String = "TI"
    Public Const TIPO_GRUPO_TRADING_NE As String = "NE"
    Public Const TIPO_GRUPO_APROBADOR As String = "TIPGRPAPR"
    Public Const TIPO_GRUPO_APROBADOR_PRINC As String = "P"
    Public Const RUTA_LOG As String = "\\plutondb\archivosplanos\SIT\Log\LogEventosSITD.xml"
    Public Const GRUPO_LIMTRD_CURRENCY As String = "3"
    Public Const GRUPO_LIMTRD_DEPOSITOSON As String = "5"
    Public Const TIPO_LIMITE_BANDAS As String = "BAN"
    Public Const RATING As String = "RATING"
    Public Const TIPO_INVERSION As String = "TipoInver"
    Public Const PERIODO_INVERSION As String = "PeriodoInv"
    Public Const RATING_NR As String = "34"
    Public Const REPORTE_FIRMA As String = "ReporteSIT"
    Public Const CATEGORIA_REPORTE As String = "OperReport"
    Public Const CARGO_FIRMANTE As String = "CargoFirma"
    Public Const CARGO_FIRMANTE_VB_LIQTES As String = "27"
    Public Const CARGO_FIRMANTE_VB_GERGESINV As String = "24"
    Public Const REPORTE_LLAMADO_OI As String = "3"
    Public Const REPORTE_OPE_EJE As String = "1"
    Public Const REPORTE_OPE_MASIVAS As String = "2"
    Public Const ESTADO_FIRMA_DOC As String = "EstFirmaD"
    Public Const ESTADO_FIRMA_DOC_PF As String = "2"
    Public Const CLASE_INSTRUMENTO_FTO As String = "CLASE_FTO"
    Public Const SERVIDORETL As String = "SERVIDORETL"
    Public Const USUARIO_APRUEBA_SOLICITUD_REVERSION As String = "USUARIO_APRUEBA_SOLICITUD_REVERSION"
    Public Const SOLICITUD_REVERSION_EMAIL As String = "SOLICITUD_REVERSION_EMAIL"
    Public Const APROBACION_SOLICITUD_REVERSION_EMAIL As String = "APROBACION_SOLICITUD_REVERSION_EMAIL"

    Public Enum _REPORTE_OPE_MASIVAS
        RentaFija = 4
        RentaVariable = 5
        OperacionesFx = 6
        AsignaFondos = 7
    End Enum
    Public Enum _REPORTE_OPE_EJECUTADAS
        RentaFija = 1
        RentaVariable = 2
        Divisas = 3
    End Enum

    Public Const CODIGOSBS_FUTUROS As String = "86"
    Public Const INT_CORR_BASE As String = "BaseIC"
    Public Const INT_CORR_NDIAS As String = "ICNDias"
    Public Const VALIDAFERIADO As String = "A"

    ' INICIO --> Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio
    Public Const TIPO_NEGOCIO As String = "TipoNegoc"
    Public Const TIPO_COMISION As String = "TipoComision"
    Public Const ORDEN_VECTOR As String = "OrdVector"
    Public Const TIPO_VALORIZACION As String = "TipoValorizacion"
    ' FIN --> Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio

    Public Const EMAIL_REP_CONSOLIDADO As String = "EMAIL_REP_CONSOLIDADO" ' Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-17 | Envio de Correo

#End Region

#Region " /* Tesoreria */ "

    Public Const ESTADOCARTA_PORAPROBAR As String = "P"
    Public Const ESTADOCARTA_APROBADA As String = "A"
    Public Const ESTADOCARTA_ENVIADA As String = "E"

    Public Const TIPOEMISIONCARTA_AUTOMATICA As String = "A"
    Public Const TIPOEMISIONCARTA_MANUAL As String = "M"

    Public Const ARCHIVOSCARGA_SALDOS As String = "CapturaSaldos"
    Public Const ARCHIVOSINTERFAZ_ABONO As String = "InterfazAbonos"

    Public Const CODIGO_BANCO_CONTINENTAL As String = "Tercero"
    Public Const CODIGO_TIPOPAGO_ABONO As String = "Modalidad"

    Public Const VISTA_REP_APROB1 As String = "1"   'HDG OT 64016 20111021
    Public Const VISTA_REP_APROB2 As String = "2"   'HDG OT 64016 20111021

    Public Const SALDO_ANIO_ANTERIOR As String = "2009"   'HDG 20120126
    Public Const PRIMER_DIA_OPERACION As Decimal = 20100101   'HDG INC 66297 20121025
    Public Const XLIQUIDA_FECHA_ANT_FONDO As String = "1"   'HDG OT 64767 20120222
    Public Const LIQUIDA_FECHA_ANT_FONDO As String = "2"   'HDG OT 64767 20120222
    Public Const CONSTITUCION_FWD_INVERSION As String = "0"   'HDG OT 66471 20130121
    Public Const CONSTITUCION_FWD_DVI As String = "1"   'HDG OT 66471 20130121
    Public Const ANEXO_I_SWAPS As String = "S"   'HDG OT 67944 20130807
    Public Const ANEXO_II_RENOVA_FWD As String = "R"   'HDG OT 67944 20130807

#End Region

#Region " /* Inversiones */ "

    Public Const ESTADO_OI_ANULADA As String = "A"
    Public Const CLASE_INSTRUMENTO_DEPOSITOPLAZO As String = "DP"
    Public Const CLASE_INSTRUMENTO_FORWARD As String = "FD" 'CMB OT 62087 20110224 Nro 6
    Public Const CLASE_INSTRUMENTO_CVME As String = "CV" 'CMB OT 62087 20110224 Nro 6
    Public Const CLASE_INSTRUMENTO_FONDOMUTUO As String = "FM" 'HDG OT 64034 04 20111111
    Public Const CLASE_INSTRUMENTO_FONDOINVERSION As String = "FI" 'HDG OT 64034 04 20111111
    Public Const CLASE_INSTRUMENTO_BONO As String = "BO"    'HDG 20120224
    Public Const CLASE_INSTRUMENTO_ACCION As String = "AC"    'HDG 20120224

    '/*INICIO: Proy SIT- FONDOS II - CRumiche: 2018-05-30 */
    Public Const CLASE_INSTRUMENTO_CERTIFICADO_DEPOSITO As String = "CD"
    Public Const CLASE_INSTRUMENTO_PAPELES_COMERCIALES As String = "PC"
    Public Const CLASE_INSTRUMENTO_LETRAS_HIPOTECARIAS As String = "LH"
    '/*FIN: Proy SIT- FONDOS II - CRumiche: 2018-05-30 */

    '/*INICIO: Proy SIT- FONDOS II - rcolonia: 06072018 */
    Public Const CLASE_INSTRUMENTO_OPERACIONES_REPORTE As String = "OR"
    '/*FIN: Proy SIT- FONDOS II - rcolonia: 06072018 */

    Public Const TIPOFONDO_ETF As String = "ETF" 'HDG OT 64034 04 20111111
    Public Const DDL_ITEM_SELECCIONE As String = "--Seleccione--" 'HDG OT 64034 04 20111111
    Public Const MEDIO_TRANS_TELF As String = "T" 'HDG OT 64291 20111128
    Public Const TIPO_TRAMO_PRINCIPAL As String = "PRINCIPAL" 'HDG OT 64291 20111128
    Public Const TIPO_TRAMO_AGENCIA As String = "AGENCIA"   'HDG OT 64480 20120119
    Public Const IMP_COM_FIJOS As String = "ImpComFijo"
    Public Const ARCHIVOSINTERFAZ_DATATEC As String = "DATATEC"
    Public Const SEPARADOR_OI As String = "&"

    Public Const EXCESO_LIMITES As String = "E-EXC"
    Public Const EXCESO_BROKER As String = "E-ENV"
    Public Const EXCESO_LIM_BRK As String = "E-EBL"
    Public Const PRINCIPAL_TRADE As String = "PT"   'HDG OT 64480 20120119
    Public Const OPERACION_VENTA As String = "2"   'HDG 20120224
    Public Const OPERACION_COMPRA As String = "1"   'HDG 20120224
    Public Const GRUPO_BROKER As String = "BRO"   'HDG 20120224
    Public Const IMP_COMIS_P_SEC_FEES As String = "P SEC FEES"   'HDG 20120224
    Public Const IMP_COMIS_COM_EXT As String = "COM EXT"   'HDG 20120224
    Public Const CLASE_LLAMADO_DEPOSITOPLAZO As String = "DEPOSITOS A PLAZO"   'HDG 20120307
    Public Const CLASE_LLAMADO_FORWARD As String = "OPERACIONES DERIVADAS - FORWARD DIVISAS" 'HDG 20120307
    Public Const CLASE_LLAMADO_CVME As String = "COMPRA/VENTA MONEDA EXTRANJERA"    'HDG 20120307
    Public Const LIQUIDADO As String = "L"    'HDG OT 65471 20120703
    Public Const FUTURO As String = "FT" 'JHC REQ 66056: Implementacion Futuros
    Public Const FIRMA_ADMINISTRADOR As String = "V B Liquidación Tesorería"   'HDG OT 66471 20130222

    'Ini HDG OT 67627 20130531
    Public Const TIPO_OPER_PREVORDEN As String = "1"
    Public Const TIPO_OPER_ORDEN As String = "2"
    Public Const MODO_ING_MASIVO As String = "1"
    Public Const MODO_ING_IND As String = "2"
    Public Const PROCESO_TRAZA1 As String = "1"
    Public Const PROCESO_TRAZA2 As String = "2"
    Public Const MOTIVO_MODIFICAR_TRAZA As String = "18"
    Public Const COMENTARIO_MODIFICA_TRAZA As String = "Error por Cambio de Datos Principales"
    Public Const COMENTARIO_MODIFICA_FONDO As String = "Error Cambio de Asignación de Fondo"
    Public Const TASA_NOMINAL As String = "1"

    Public CONTADOR_DETALLE_INVERSIONES As Integer
    Public DT_DETALLE_INVERSIONES As New DataTable
    Public Enum _PROCESO_TRAZA
        Grabar = 1
        Validar = 2
        Agregar = 3
        Ejecutar = 4
        Extorno = 5
        Eliminar = 6
        EjecucionPrevia = 7
        ConfirmacionOrdenes = 8
        EliminarConfirmacion = 9
        EjecutarOrdenes = 10
        Modificar = 11
    End Enum
    'Fin HDG OT 67627 20130531
#End Region

#Region " /* Gestión */ "

    Public Const TIPOS_REP_CONTROL_LIMITES As String = "TIP_REPLIM"     'HDG OT 63063 R02 20110504
    Public Const REP_CONSOLIDADO As String = "CONSOLIDADO"  'HDG OT 64926 20120320
    Public Const INTERFASE_BBH As String = "INTERFBBH"  'HDG OT 64765 20120312
    Public Const INTERFASE_CUSTODY As String = "C"  'HDG OT 64765 20120312
    Public Const INTERFASE_TRANSACTIONS As String = "T" 'HDG OT 64765 20120312
    Public Const COD_ARCH_CUSTODIO_BBH As String = "024" 'HDG OT 64765 20120312
    Public Const IND_ELI_TRANSCUSTODIO_BBH As String = "E" 'HDG OT 64765 20120312
    Public Const ANEXO_A3A_03 As String = "A3A"    'HDG INC 67327 20130408
    Public Const ANEXO_A3B_04 As String = "A3B"    'HDG INC 67327 20130408
    Public Const ANEXO_A6_08 As String = "A6"    'HDG INC 67327 20130408
    Public Const ANEXO_A7_09 As String = "A7"    'HDG INC 67327 20130408
    Public Const ANEXO_A8_10 As String = "A8"    'HDG INC 67327 20130408
    Public Const ANEXO_A9_11 As String = "A9"    'HDG INC 67327 20130408

#End Region

#Region " /* Riesgos */ "

    Public Const EXCEP_LIMITE_NEGOCIA As String = "EXCEPCION"     'HDG OT 63063 R04 20110523

#End Region

#Region " /* Contabilidad */ "

    Public Const COD_ARCHIVO_LOTES As String = "026"

#End Region

#Region " /* Valorizacion y Custodia */ "

    Public Const IGV As Decimal = 0.18

#End Region

End Class
