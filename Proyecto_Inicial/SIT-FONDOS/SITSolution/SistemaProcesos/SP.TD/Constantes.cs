using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SistemaProcesosTD
{
    namespace Constantes
    {
        public struct Tablas
        {
            public static int ID_TABLA_DEPOSITO = 5;
            public static int ID_TABLA_PRECIERRE = 8;
            public static int ID_TABLA_CIERRE = 2;
        }

        public struct ConstantesING
        {
            public static String SI = "S";
            public static String NO = "N";

            public static String CODIGO_AGENCIA_CITIBANK = "CIT";
            //OT5908 INICIO
            public static String CODIGO_AGENCIA_BIF = "BIF";
            //OT5908 FIN

            public static String PERSONA_SEXO_FEMENINO = "FEM";
            public static String PERSONA_SEXO_MASCULINO = "MAS";

            public static String ESTADO_CIVIL_SOL = "SOL";
            //OT 4792 PSC2 Inicio-----------------------------------------------
            public static String CORREO_VALOR_CUOTA = "CORREO_VALOR_CUOTA";
            //OT 4792 PSC2 Fin--------------------------------------------------

            //OT 5012 INI
            public static int ID_PLANTILLA_CONTRATOS = 1;
            //OT 5012 FIN

            public static String TABLA_TIPO_DOCUMENTO = "TIPDOC";
            public static String TIPO_DOCUMENTO_RUC = "RUC";
            public static String TIPO_DOCUMENTO_DNI = "DNI";

            //OT 7349 INI  
            public static String CODIGO_SAP = "031";
            public static String ITEM_SAP = "001";
            public static String ACTIVO = "ACTIVO";
            public static String TIPO_DOCUMENTO_FACTURA = "01";
            public static String TIPO_DOCUMENTO_BOLETA = "03";
            public static String TIPO_DOCUMENTO_NOTA = "07";
            public static String SERIE_ORDEN_ASC = "SERIE ASC";
            public static String NUMERO_ORDEN_ASC = "NUMERO ASC";
            public static String COD_MONEDA = "018";
            public static String ITEM_MONEDA_SOLES = "S/.";
            public static String ITEM_MONEDA_DOLARES = "$";
            public static String CODIGO_TIPO_DOCUMENTO = "022";
            public static String ESTADO_ACTIVO = "N";
            public static String ESTADO_INACTIVO = "S";
            public static String EXITO = "1";
            public static String CERO = "0";
            public static String COD_CONCEPTO_ORDEN_ASC = "COD_CONCEPTO ASC";
            public static String CODIGO_IGV = "023";
            public static String IGV = "IGV";
            public static String AFECTO_IMPUESTO = "S";
            public static String AFECTO_IGV = "S";
            public static String IMPRESO = "S";
            public static string ESTADO_NO_ENVIADO = "N";
            public static string AIM = "AIM";
            public static string MENOS_UNO = "-1";
            //OT 7349 FIN


            public static string DIRECCION_FONDOS = "DireccionFondos";
            public static string DISTRITO_FONDOS = "DistritoFondos";
            public static string PROVINCIA_FONDOS = "ProvinciaFondos";
            public static string DEPARTAMENTO_FONDOS = "DepartamentoFondos";
            public static string FONDO = "FONDO";
            public const string TIPO_ASIENTO_INGRESO = "VT_ING";
            public const string TIPO_ASIENTO_COMISION = "VT_CRA";
            public static string CONCEPTO_INGRESO_SOLES = "01";
            public static string CONCEPTO_INGRESO_DOLARES = "02";
            public static string CONCEPTO_COMISION_SOLES = "03";
            public static string CONCEPTO_COMISION_DOLARES = "04";
            public static string ACCION_ACTUALIZAR = "ACT";
            public static string TABLA_COMPROBANTES = "GLOSAS";
            public static string GLOSA_ASIENTO_INGRESOS = "GLOING";
            public static string GLOSA_ASIENTO_COMISIONES = "GLOCRA";
            public static string DESCRIPCION_ASIENTO_INGRESOS = "DESING";
            public static string DESCRIPCION_ASIENTO_COMISIONES = "DESCRA";

            public static string CODIGO_PAIS_PERU = "173";
            public static string DESC_PAIS_PERU = "PERU";
            public static string CODIGO_PERU = "PE";
            public static string CODIGO_NO_VALOR = "-1";
            public static string DESCRIPCION_NO_VALOR = "--Seleccione--";
            public static string CODIGO_TIPO_NOTA_CREDITO = "034";
            public static string CODIGO_TIPO_DOCUMENTO_IDENTIDAD = "035";

            public static string ID_PAIS_FONDOS = "idPaisFondos";
            public static string ID_DEPARTAMENTO_FONDOS = "idDepartamentoFondos";
            public static string ID_CIUDAD_FONDOS = "idCiudadFondos";
            public static string ID_DISTRITO_FONDOS = "idDistritoFondos";
            public static string UBIGEO_FONDOS = "UbigeoFondos";
            public static string CORREO_FONDOS = "CorreoFondos";

            public static string ACCION_ANULAR = "ANU";

            public static string TABLA_MODELO = "RUTELE";
            public static string RUTA_MODELO = "rutaModelos";
            public static string RUTA_ARCHIVOS = "rutaArchivos";

            //OT7235
            public static string CODIGO_TABLA_AGENCIA_ORIGEN = "AGEPRO";
            public static string CODIGO_USUARIOS_OPERACIONES = "USUOPE";
            public static string CODIGO_USUARIOS_OFC = "USUOFC";

            public static string CODIGO_TABLA_URL_IMAGENES = "URL_IMAGENES";
            public static string LLAVE_TABLA_URL_IMAGENES_BIENVENIDA = "BIENVENIDA";

            public static string CODIGO_TABLA_APROBACION_CUC_CORREO = "APROB_CUC_CORREO";
            public static string LLAVE_TABLA_APROBACION_CUC_CORREO_FLAG_ENVIO = "FLAG_ENVIO";

            /*Inicio OT7999*/
            public static string CODIGO_PORCENTAJE_DETRACCION = "036";
            public static string PORCENTAJE_DETRACCION = "PORDETRAC";

            public static string CODIGO_SCOTIABANK_SOLES_MN = "037";
            public static string SCOTIABANK_SOLES_MN = "SCO_S_MN";

            public static string CODIGO_SCOTIABANK_SOLES_CCI = "038";
            public static string SCOTIABANK_SOLES_CCI = "SCO_S_CC";

            public static string CODIGO_SCOTIABANK_DOLARES_MN = "039";
            public static string SCOTIABANK_DOLARES_MN = "SCO_D_MN";

            public static string CODIGO_SCOTIABANK_DOLARES_CCI = "040";
            public static string SCOTIABANK_DOLARES_CCI = "SCO_D_CC";

            public static string CODIGO_NUMERO_BANCO_NACION = "041";
            public static string NUMERO_BANCO_NACION = "NUM_BN";

            public static string CODIGO_SCOTIABANK_SOLES_MN_TEXTO = "042";
            public static string SCOTIABANK_SOLES_MN_TEXTO = "SC_S_M_T";

            public static string CODIGO_SCOTIABANK_SOLES_CCI_TEXTO = "043";
            public static string SCOTIABANK_SOLES_CCI_TEXTO = "SC_S_C_T";

            public static string CODIGO_SCOTIABANK_DOLARES_MN_TEXTO = "044";
            public static string SCOTIABANK_DOLARES_MN_TEXTO = "SC_D_M_T";

            public static string CODIGO_SCOTIABANK_DOLARES_CCI_TEXTO = "045";
            public static string SCOTIABANK_DOLARES_CCI_TEXTO = "SC_D_C_T";
            /*Fin OT7999*/

            /*Inicio OT7940 PSC001*/
            public static string CODIGO_TABLA_BIENVENIDA_FONDOS_CONVENCIONALES = "BIENVENI_FOND_CONVEN";
            public static string LLAVE_TABLA_URL_CLAUSULAS_CONTRATO = "URL_CLAUSU_CONTRATO";
            public static string LLAVE_TABLA_URL_REGLAMENTO_PARTICIPACION = "URL_REGLAMENT_PARTI";
            public static string LLAVE_TABLA_URL_PROSPECTO_SIMPLIFICADO = "URL_PROSPECTO_SIMPL";

            public static string CODIGO_TABLA_BIENVENIDA_FONDOS_SERIE_B = "BIENV_FONDOS_SERIEB";

            /*Fin OT7940 PSC001*/

            //OT 7968 INI

            public static string MAIL_TC_TRASPASO = "MAIL_TC_TRASPASO";

            //OT 7968 FIN

            //OT 8829 INI

            public static string FormatoCuotas = "0.00000000";
            public static string FormatoValorCuota = "0.00000000";

            //OT 8829 FIN

            /*Inicio OT 8844*/
            public static string TIPO_ACCESO_FONDO_PRIVADO = "PRI";
            public static string CODIGO_TABLA_FONDOS_INVERSIONES = "FONDOS_MIDAS";
            public static string BASE_DATOS_OPERACIONES = "ADMCUENTAS";
            public static string CODIGO_TABLA_TIPO_CALCULO = "TIPO_CALCULO";
            public static string CODIGO_TABLA_TIPO_ACCESO_FONDO = "TIPAFO";
            public static string CODIGO_TABLA_INFO_PRECIERRE = "INFO_PRECIERRE";
            public static string RUTA_LOG = "rutaLog";
            /*Fin OT 8844*/


            //OT8954 INI
            public static string CODIGO_CARGA_MASIVA = "CARGA_MASIVA";
            public static string CODIGO_TABLA_CUENTA_RECAUDO = "CUENTA_RECAUDO";
            public static string AREA_TRASPASO_FONDO = "OPE";
            public static string TIPO_RESCATE_TOTAL = "TOTAL";
            public static string CODIGO_VIA_PAGO_TRASPASO_FONDOS = "015";
            public static string CODIGO_FORMA_PAGO_TRASPASO_FONDOS = "ABO";
            public static string TIPO_OPERACION_RESCATE = "RES";
            public static string ESTADO_OPERACION_PENDIENTE = "PEN";
            public static string FORMATO = "yyyy-MM-dd HH:mm:ss";
            public static string ADICIONAL2 = "T";
            public static string CODIGO_VIA_SOLICITUD_AGENCIA = "AGE";
            public static string CLASE_ALERTA_EVENTO = "EVE";
            public static string TIPO_ALERTA_OBSERVACION = "OBS";
            public static string SUBTIPO_ALERTA_OTROS = "OTR";
            public static string ESTADO_ALERTA = "ACT";
            public static int USUARIO_ALERTA = 183;
            public static string AREA_MODIFICACION_ALERTA = "SER";
            public static string TITULO = "Carga Masiva de ";
            public static int IGV_VALOR = 18;
            public static int ID_FONDO_ABONO_MASIVO = 9;
            public static string TIPO_PAGO_ABONO_MASIVO = "EFE";
            public static string TITULO_REVERSION = "Reversión de carga Masiva";

            //OT8954 FIN

            //OT8954 PSC-001 INI
            public static string TIPO_OPERACION_TRASPASO = "TPS";
            public static string AREA_OPERACION_TRASPASO = "SIS";
            public static string TITULO_ABONO = "Carga Masiva de Abonos Masivos";
            public static string TITULO_RESCATE = "Carga Masiva de Rescates Masivos";
            public static string TITULO_TRASPASO_FONDO = "Carga Masiva de Traspasos Fondos Masivos";
            public static string TITULO_TRASPASO = "Carga Masiva de Traspaso Masivo";

            //OT8954 PSC-001 FIN

            //OT10217 INI
            public static string TIPO_OPERACION_CONVERSION_CUOTAS = "COV";
            public static string ESTADO_OPERACION_CONFIRMADO = "CON";
            public static string AREA_OPERACION_CONVERSION_CUOTAS = "SIS";
            public static string TITULO_CONVERSION = "Carga Masiva de Conversion de Cuotas Masivo";
            //OT10217 FIN

            //OT10433 INI
            public static string AREA_CUENTA_PARTICIPACION = "SIS";
            public static string TITULO_CUENTA_PARTICIPACION = "Carga Masiva de Cuentas de Participación";
            //OT10433 FIN

            //OT10478 INI
            public static string AREA_DISTRIBUCION_X_PERIODO = "SER";
            public static string CODIGO_FONDO_SIT = "CODIGO_PORTAFOLIO";
            //OT10478 FIN

            public static string CODIGO_TABLA_FONDOS_MIDAS = "FONDOS_MIDAS";//OT10604 

            public static string AREA_PAGO_FLUJO_SOBRANTE = "SER";//OT10592

            //OT10808 INICIO
            public static string TIPO_PROCESO = "RES";
            public static string ESTADO_CIERRE_FONDO = "CON";
            public static string ESTADO_CIERRE_FONDO_REVERTIDO = "CAN";
            public static string AREA_MODIFICACION_CIERRE_RESCATE = "SER";
            //OT10808 FIN

            //OT10944 INI	
            public static string TIPO_ACCESO_FONDO_PUBLICO = "PUB";
            public static string TIPO_ACCESO_FONDO_INVERSION = "INV";
            //OT10944 FIN

            public static string ESTADO_OPERACION_SIMULACION = "CON"; //OT10563

            //OT11069 - INI
            public static String tipoProcesoCierre = "SUS";
            public static String cierreEstadoActivo = "ACT";
            public static string AREA_MODIFICACION_CIERRE_SUSCRIPCION = "SER";
            //OT11069 - FIN

            public static string CODIGO_TABLA_FONDOS_FIRBI = "FONDO_FIRBI";//OT11247

            public static string CODIGO_CORREOS_DESTINATARIO_ALERTA = "ALECLIESP";//OT11264

            public static string TITULO_ALERTA = "Alerta clientes especiales";//OT11264 PSC002

            public static string DESCRIPCION_ALERTA = "Alerta clientes especiales";//OT11264 PSC002

            public static string ID_USUARIO_ORIGEN = "183";//OT11264 PSC002

            public static string DESCRIPCION_LARGA_DISTRIBUCION_TOTAL = "T";//OT11264 PSC003

            public static string PLANTILLA_DEVOLUCIONES = "plantillaDevoluciones";//OT10571 

            //OT11142 INI
            public static string SERIE_ORDEN_DESC = "SERIE DESC";
            public static string NUMERO_ORDEN_DESC = "NUMERO DESC";
            public static string EMPRESA_FONDO_SURA = "13";
            public static string EMPRESA_FIR = "55";
            public static string TIPO_SERVICIO_SOAP = "1";
            //OT11142 FIN
        }

        /*Inicio OT 8844*/
        public struct Conexion
        {
            public static string ServidorInversiones = "ServidorInversiones";
            public static string BDInversiones = "BDInversiones";

            public static string ServidorContabilidad = "ServidorPrem_Main";
            public static string BDContabilidad = "BDPrem_Main";

            public static string ServidorPremBanco13 = "ServidorPrem_Banco13";
            public static string BaseDeDatosPremBanco13 = "BDPrem_Banco13";
        }
        /*Fin OT 8844*/

    }
}
