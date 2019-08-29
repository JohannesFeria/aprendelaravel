/*
 * Fecha de Modificación: 31/07/2012
 * Modificado por: Cesar Arasaki
 * Numero de OT: 4792
 * Descripción del cambio: Creación
 * */
/*
 * Fecha de Modificación: 28/11/2012
 * Modificado por: Robert Castillo
 * Numero de OT: 5117
 * Descripción del cambio: Se cambia namespace
 * */
/*
 * Fecha Modificación	: 02/05/2013
 * Modificado por		: Davis Rixi
 * Nro de OT			: 5007
 * Descripción del cambio: Se implementa lógica para proceso de registro de excesos en el método ejecutarPrecierre
 * */
/*
 * Fecha Modificación	: 20/05/2013
 * Modificado por		: Davis Rixi
 * Nro de OT			: 5007
 * Descripción del cambio: Se modifica método ListarAlertasExcesos para enviar como parámetro el idFondo
 * */
/*
 * Fecha Modificación	: 18/06/2013
 * Modificado por		: Robert Castillo
 * Nro de OT			: 5526
 * Descripción del cambio: Se modifica el dato que retorna el método PreviewPrecierre a DataSet.
 * */
/*
 * Fecha Modificación	: 11/11/2013
 * Modificado por		: Robert Castillo
 * Nro de OT			: 5948
 * Descripción del cambio: En el método EjecutarPrecierre se realizan los siguientes cambios:
                            1. Se realiza una llamada al método VerificarFondoPadrePrecierre de la clase 
 *                             PrecierreDA, para verificar que todos los fondos hijos del fondo padre en 
 *                             proceso se hayan precerrado para la fecha de proceso. Si se ha precerrado 
 *                             todos los fondos hijos, se realiza el cálculo de los excesos, de lo contrario 
 *                             el mencionado cálculo no se realizará. Para realizar el cálculo de los excesos 
 *                             se llamará al método EjecutarRegistroExcesos.
                           En el método EjecutarRegistroExcesos se reemplaza el parámetro idFondo por el nuevo 
 *                         parámetro codigoPadre (de tipo VARCHAR(3)).
 * */
/*
 * Fecha Modificación	: 15/06/2015
 * Modificado por		: Robert Castillo
 * Nro de OT			: 7370
 * Descripción del cambio: Se agrega el método ObtenerUltimaFechaPrecierreXFondo.
 * */
/*
 * Fecha Modificación	    : 07/01/2016
 * Modificado por		    : Richard Valdez
 * Nro de OT			    : 7968
 * Descripción del cambio   : Agregar los métodos InsertarDepositoAsociadoTraspasoFondos, ObtenerTraspasoFondosPendienteTipoCambio,
 *                            ObtenerTablaGeneral, ListarTablaGeneral
 * */

/*
 * Fecha Modificación	    : 30/05/2016
 * Modificado por		    : Irene Reyes 
 * Nro de OT			    : 8829
 * Descripción del cambio   : implementar transacción con la base de datos TRIBUTACIÓN  para llamar el procedure insertar log diario
 * */

using System;
using System.Data;
using SistemaProcesosTD;
using SistemaProcesosDA;
using System.Data.SqlClient;

namespace SistemaProcesosBL
{
	/// <summary>
	/// Descripción breve de PrecierreBL.
	/// </summary>
	public class PrecierreBO
	{
		private string codigoUsuario;

		public PrecierreBO(string codigoUsuario) 
		{
			this.codigoUsuario = codigoUsuario;
		}
		public PrecierreBO(){}


                /// <summary>
        /// Obtiene el detalle del Valor Cuota Cerrado para una fecha específica
        /// </summary>
        /// <param name="idFondo"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public DataRow ObtenerDetalleValorCuotaCerrado(decimal idFondo, DateTime fecha)
        {
            PrecierreDA precierreDA = new PrecierreDA();
            return precierreDA.ObtenerDetalleValorCuotaCerrado(idFondo, fecha);
        }

        //Ejecutar Precierre de Depositos
        private bool EjecutarPrecierre(decimal idFondo, DateTime fecha, DateTime horarioOperacion, decimal valorCuota, SqlConnection cn, SqlTransaction trans)
        {
            // Si la fecha esta cerrada no se puede volver a ejecutar el precierre.
            CierreBO cierreBO = new CierreBO(codigoUsuario);
            if (cierreBO.VerificarFechaCerrada(idFondo, fecha)) return false;

            // Si existe un precierre para la fecha y el fondo se marca como eliminado
            PrecierreDA precierreDA = new PrecierreDA();
            PrecierreTD.PrecierreRow drPrecierreAnterior = precierreDA.ObtenerPrecierre(idFondo, fecha);
            if (drPrecierreAnterior != null) drPrecierreAnterior.FLAG_ELIMINADO = "S";

            // Seteando los datos del precierre.			
            DateTime serverDate = precierreDA.GetServerDate();
            PrecierreTD.PrecierreRow drPrecierre = new PrecierreTD().Precierre.NewPrecierreRow();
            drPrecierre.ID_FONDO = idFondo;
            drPrecierre.FECHA = fecha;
            drPrecierre.HORARIO_OPERACION = horarioOperacion;
            drPrecierre.VALOR_CUOTA = valorCuota;
            drPrecierre.FLAG_ELIMINADO = "N";
            drPrecierre.USUARIO = codigoUsuario;
            drPrecierre.FECHA_PROCESO = serverDate;

            DepositoTD.DepositoPrecierreDataTable dtDepositosPrecierre = null;

            // Actualizando los datos de los depositos que entran al precierre 
            if (fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday && !precierreDA.EsFeriado(fecha))
            {
                dtDepositosPrecierre = precierreDA.ObtenerDepositosPrecierre(idFondo, fecha, horarioOperacion);
                foreach (DepositoTD.DepositoPrecierreRow drDeposito in dtDepositosPrecierre.Rows)
                    drDeposito.NUMERO_CUOTAS = Math.Round(drDeposito.MONTO / valorCuota, INGFondos.Constants.Formatos.NumeroDecimalesCuotas);
            }

            // Grabando la información
            precierreDA.EjecutarPrecierre(drPrecierreAnterior, drPrecierre, dtDepositosPrecierre, cn, trans);

            return true;
        }

        public DataSet PreviewPrecierre(decimal idFondo,String flagVerificacion)
        {
            PrecierreDA da = new PrecierreDA();
            da.Database = INGFondos.Constants.Conexiones.BaseDeDatosOperaciones;
            da.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;
            return da.PreviewPrecierre(idFondo, flagVerificacion, codigoUsuario);
        }
        // Inicio | rcolonia | Se agrega nuevo parametro para ejecución de precierre: decimal ComisionSAFM | 25092018
        public DataSet EjecutarPrecierre(decimal comisionSAFM, decimal idFondo, String flagVerificacion, DateTime fecha, DateTime horarioOperacion, decimal valorCuota)
        {
            DataSet dtResult = null;

            PrecierreDA daDepositos = new PrecierreDA();
            PrecierreDA daOperaciones = new PrecierreDA();
            PrecierreDA daTributacion = new PrecierreDA();


            daOperaciones.Database = INGFondos.Constants.Conexiones.BaseDeDatosOperaciones;
            daOperaciones.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;

            daTributacion.Database = INGFondos.Constants.Conexiones.BaseDeDatosTributacion;
            daTributacion.Server = INGFondos.Constants.Conexiones.ServidorTributacion;

            SqlConnection cn = daDepositos.GetConnection2();
            cn.Open();
            SqlTransaction trans = cn.BeginTransaction();


            SqlConnection cnOperaciones = daOperaciones.GetConnection2();
            cnOperaciones.Open();
            SqlTransaction transOperaciones = cnOperaciones.BeginTransaction();

            SqlConnection cnTributacion = daTributacion.GetConnection2();
            cnTributacion.Open();
            SqlTransaction transTributacion = cnTributacion.BeginTransaction();

            try
            {
                // Inicio | rcolonia | Se agrega nuevo parametro para ejecución de precierre: decimal ComisionSAFM | 25092018
                dtResult = daOperaciones.EjecutarPrecierreOperaciones(comisionSAFM, idFondo, flagVerificacion, codigoUsuario, cnOperaciones, transOperaciones);
                // Fin | rcolonia | Se agrega nuevo parametro para ejecución de precierre: decimal ComisionSAFM | 25092018
                daTributacion.insertarLogDiario(idFondo, fecha, cnTributacion, transTributacion);

                EjecutarPrecierre(idFondo, fecha, horarioOperacion, valorCuota, cn, trans);
                //OT 5948


                DataTable dtFondoPadre = daOperaciones.VerificarFondoPadrePrecierre(idFondo, fecha);

                //Verificando que se han precerrado a la fecha de proceso todos los fondos hijos del fondo padre
                if (dtFondoPadre.Rows.Count > 0)
                {
                    string codigoPadre = dtFondoPadre.Rows[0]["CODIGO_PADRE"].ToString();
                    EjecutarRegistroExcesos(codigoPadre, fecha, codigoUsuario, cnOperaciones, transOperaciones);
                }

                //OT7968 INI

                daOperaciones.InsertarDepositoAsociadoTraspasoFondos(idFondo, fecha, codigoUsuario, cnOperaciones, transOperaciones);

                //OT7968 FIN

                trans.Commit();
                transOperaciones.Commit();
                transTributacion.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                transOperaciones.Rollback();
                transTributacion.Rollback();
                throw e;
            }
            finally
            {
                cn.Close();
                cnOperaciones.Close();
                cnTributacion.Close();
            }
            return dtResult;
        }
        // Fin | rcolonia | Se agrega nuevo parametro para ejecución de precierre: decimal ComisionSAFM | 25092018

        private void EjecutarRegistroExcesos(string codigoPadre, DateTime fecha, string codigoUsuario, SqlConnection cn, SqlTransaction trans)
        {
            PrecierreDA precierreDA = new PrecierreDA();
            precierreDA.EjecutarRegistroExcesos(codigoPadre, fecha, codigoUsuario, cn, trans);


        }

        public DataSet ListarAlertasExcesos(decimal idFondo)
        {
            PrecierreDA da = new PrecierreDA();
            da.Database = INGFondos.Constants.Conexiones.BaseDeDatosOperaciones;
            da.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;
            return da.ListarAlertasExcesos(idFondo);
        }

        //OT7370 INI
        public DataTable ObtenerUltimaFechaPrecierreXFondo(decimal idFondo)
        {
            PrecierreDA da = new PrecierreDA();
            da.Database = INGFondos.Constants.Conexiones.BaseDeDatosOperaciones;
            da.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;
            return da.ObtenerUltimaFechaPrecierreXFondo(idFondo);
        }

        public DataTable VerificarFondoPadrePrecierre(decimal idFondo, DateTime fecha)
        {
            PrecierreDA da = new PrecierreDA();
            da.Database = INGFondos.Constants.Conexiones.BaseDeDatosOperaciones;
            da.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;
            return da.VerificarFondoPadrePrecierre(idFondo, fecha);
        }
        //OT7370 FIN


        //OT7968 INI
        /// <summary>
        /// Se encarga de llamar al método del mismo nombre de la clase PrecierreDA
        /// </summary>
        /// <param name="idFondo"></param>
        /// <param name="fecha"></param>
        /// <param name="usuario"></param>
        /// <param name="cn"></param>
        /// <param name="trans"></param>
        public void InsertarDepositoAsociadoTraspasoFondos(decimal idFondo, DateTime fecha, string usuario,
            SqlConnection cn, SqlTransaction trans)
        {
            PrecierreDA da = new PrecierreDA();
            da.Database = INGFondos.Constants.Conexiones.BaseDeDatosOperaciones;
            da.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;

            da.InsertarDepositoAsociadoTraspasoFondos(idFondo, fecha, usuario, cn, trans);
        }

        /// <summary>
        /// Se encarga de llamar al método del mismo nombre de la clase PrecierreDA
        /// </summary>
        /// <param name="idFondo"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public DataTable ObtenerTraspasoFondosPendienteTipoCambio(decimal idFondo, DateTime fecha)
        {
            PrecierreDA da = new PrecierreDA();
            da.Database = INGFondos.Constants.Conexiones.BaseDeDatosOperaciones;
            da.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;

            return da.ObtenerTraspasoFondosPendienteTipoCambio(idFondo, fecha);
        }

        ///// <summary>
        ///// Se encarga de llamar al método del mismo nombre de la clase PrecierreDA
        ///// </summary>
        ///// <param name="codigoTabla"></param>
        ///// <param name="llaveTabla"></param>
        ///// <returns></returns>
        //public DataTable ObtenerTablaGeneral(string codigoTabla, string llaveTabla)
        //{
        //    PrecierreDA da = new PrecierreDA();

        //    return da.ObtenerTablaGeneral(codigoTabla, llaveTabla);
        //}

        public DataTable ListarTablaGeneral(string codigoTabla)
        {
            PrecierreDA da = new PrecierreDA();

            da.Database = INGFondos.Constants.Conexiones.BaseDeDatosOperaciones;
            da.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;

            return da.ListarTablaGeneral(codigoTabla);
        }

        ////OT7968 FIN

        //OT11264 PSC002 INI
        public void InsertarAlertaActividadCliente(int idParticipe, int idFondo, int idOperacion, DateTime fechaProceso)
        {
            PrecierreDA da = new PrecierreDA();

            da.Database = INGFondos.Constants.Conexiones.BaseDeDatosOperaciones;
            da.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;

            SqlConnection cnOperaciones = da.GetConnection2();
            cnOperaciones.Open();
            SqlTransaction transOperaciones = cnOperaciones.BeginTransaction();

            try
            {
                da.InsertarAlertaActividadCliente(idParticipe, idFondo, idOperacion, codigoUsuario, fechaProceso, cnOperaciones, transOperaciones);
                transOperaciones.Commit();
            }
            catch (Exception e)
            {
                transOperaciones.Rollback();
                throw e;
            }
            finally
            {
                cnOperaciones.Close();
            }
        }
        //OT11264 PSC002 FIN

        //OT11264 PSC003 INI
        public DataTable ValidarFondoPrecerradoXFondo(int idFondo, DateTime fecha)
        {
            PrecierreDA da = new PrecierreDA();
            da.Database = INGFondos.Constants.Conexiones.BaseDeDatosOperaciones;
            da.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;
            return da.ValidarFondoPrecerradoXFondo(idFondo, fecha);
        }
        //OT11264 PSC003 FIN
	}
}
