/*
 * Fecha de Modificación: 27/11/2012
 * Modificado por: Robert Castillo
 * Numero de OT: 5117
 * Descripción del cambio: Creación
 * */
/*
 * Fecha Modificación	: 02/05/2013
 * Modificado por		: Davis Rixi
 * Nro de OT			: 5007
 * Descripción del cambio: Se implementa lógica para proceso de reversión de excesos en el método RevertirPrecierre
 * ********************************************************************************************************************
 * Fecha Modificación	: 02/07/2015
 * Modificado por		: Robert Castillo
 * Nro de OT			: 7370 - PSC001
 * Descripción del cambio: Se crea ejecuta la llamada a EjecutarReversionRescatesSignificativos 
 *                         en el método RevertirPrecierre
 * */
/*
 * Fecha de Modificación    : 14/01/2016
 * Modificado por           : Richard Valdez
 * Numero de OT             : 7968
 * Descripción del cambio   : Modificar el método RevertirPrecierre para revertir los depósitos asociados a un 
 *                            traspaso entre fondos.   
 * */
/*
 * Fecha de Modificación    : 10/02/2016
 * Modificado por           : Richard Valdez
 * Numero de OT             : 8292
 * Descripción del cambio   : - Método RevertirPrecierre: Agregar una variable que almacena el valor de retorno 
 *                              del método RevertirPrecierre de la clase ReversionDA.
 *                            - Método RevertirPrecierre: Enviar un parámetro adicional al método EjecutarReversionDepositos 
 *                              de la clase ReversionDA. El parámetro será la variable creada en el paso anterior.  
 * */
/*
 * Fecha Modificación	 : 16/06/2016
 * Modificado por		 : Juan Castro
 * Nro. Orden de Trabajo : 8844
 * Descripción del cambio: Se implementa reversión de la atribución de renta junto con la reversión del precierre.
 * */
/*
 * Fecha Modificación	 : 21/03/2017
 * Modificado por		 : Héctor Mendoza Rosales
 * Nro. Orden de Trabajo : 10110
 * Descripción del cambio:
                           - Se modifica el método RevertirPrecierre() donde se agrega y se usa el parámetro de motivo de la Reversión.
                           - Se agrega el método ObtenerUsuario() donde se obtiene los datos personales del usuario.
 * */
/*
 * Fecha de Modificación : 26/04/2017
 * Modificado por        : Anthony Joaquin
 * Nro. Orden de Trabajo : 10217 - PSC001
 * Descripción del cambio: Homologación.
 * */
/*
 * Fecha de Modificación : 25/05/2018
 * Modificado por        : Walter Albites
 * Nro. Orden de Trabajo : OT11264 PSC002
 * Descripción del cambio: Se agrega el método ReversionAlertaActividadCliente para el cambio de estado en alertas.
 * */
/*
 * Fecha de Modificación : 25/07/2019
 * Modificado por        : Robert Castillo
 * Nro. Orden de Trabajo : OT11777
 * Descripción del cambio: Se agrega el método ObtenerSolicitudReversionAprobada.
 * */
using System;
using System.Data;
using SistemaProcesosTD;
using SistemaProcesosDA;
using System.Data.SqlClient;

namespace SistemaProcesosBL
{
    /// <summary>
    /// Clase que controla la lógica de negocio del proceso de reversión.
    /// </summary>
    public class ReversionBL
    {
        private string codigoUsuario;

        public ReversionBL(string codigoUsuario)
        {
            this.codigoUsuario = codigoUsuario;
        }

        /// <summary>
        /// Ejecuta la reversión del precierre
        /// </summary>
        /// <param name="idFondo">Fondo que se desea revertir</param>
        /// <param name="fecha">Fecha de reversión</param>
        /// <param name="usuario">Usuario responsable de la reversión</param>
        /*INI 10110*/
        /// <param name="motivo">Motivo de la reversión</param>
        //public void RevertirPrecierre(int idFondo,DateTime fecha, string usuario)
        public void RevertirPrecierre(int idFondo, DateTime fecha, string usuario, string motivo)
        /*FIN 10110*/
        {
            ReversionDA da = new ReversionDA();
            /*INICIO 8844*/
            AtribucionDA atribucionDA = new AtribucionDA();
            /*FIN 8844*/

            da.Database = INGFondos.Constants.Conexiones.BaseDeDatosOperaciones;
            da.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;

            //OT 7968 INI
            ReversionDA daDepositos = new ReversionDA();

            daDepositos.Database = INGFondos.Constants.Conexiones.BaseDeDatosBancos;
            daDepositos.Server = INGFondos.Constants.Conexiones.ServidorBancos;
            //OT 7968 FIN

            SqlConnection cn = da.GetConnection2();

            //OT 7968 INI
            //SqlConnection cnDepositos = daDepositos.GetConnection2();
            SqlConnection cnDepositos = daDepositos.GetConnection2();
            //OT 7968 FIN

            //INICIO OT8844
            SqlConnection cnTributacion = atribucionDA.GetConnection2();
            cnTributacion.Open();
            SqlTransaction transTributacion = cnTributacion.BeginTransaction(IsolationLevel.ReadUncommitted);
            //FIN OT8844

            cn.Open();

            //OT 7968 INI
            cnDepositos.Open();
            //OT 7968 FIN

            SqlTransaction trans = cn.BeginTransaction();

            //OT 7968 INI
            SqlTransaction transDepositos = cnDepositos.BeginTransaction();
            //OT 7968 FIN

            try
            {
                //OT 8292 INI

                //da.RevertirPrecierre(cn, trans, idFondo, fecha, usuario);

                /*INI 10110*/
                //decimal idPrecierre = da.RevertirPrecierre(cn, trans, idFondo, fecha, usuario);
                decimal idPrecierre = da.RevertirPrecierre(cn, trans, idFondo, fecha, usuario, motivo);
                /*FIN 10110*/

                //OT 8292 FIN

                da.EjecutarReversionExcesos(idFondo, fecha, usuario, cn, trans);
                // 7370 - PSC001
                da.EjecutarReversionRescatesSignificativos(idFondo, fecha, usuario, cn, trans);
                // 7370 - PSC001

                //OT 7968 INI

                //OT 8292 INI

                //daDepositos.EjecutarReversionDepositos(cnDepositos, transDepositos, idFondo, fecha, usuario);
                daDepositos.EjecutarReversionDepositos(cnDepositos, transDepositos, idFondo, fecha, idPrecierre, usuario);

                //OT 8292 FIN

                //OT 7968 FIN

                //INICIO OT8844
                if (fecha.Year < fecha.AddDays(1).Year) // Si al día siguiente es otro año
                {
                    atribucionDA.RevertirTributacion(idFondo, fecha, usuario, "S", cnTributacion, transTributacion);
                }
                atribucionDA.RevertirTributacion(idFondo, fecha, usuario, "D", cnTributacion, transTributacion);
                transTributacion.Commit();
                //FIN OT8844
                trans.Commit();

                //OT 7968 INI
                transDepositos.Commit();
                //OT 7968 FIN
            }
            catch (Exception e)
            {
                trans.Rollback();

                //OT 7968 INI
                transDepositos.Rollback();
                //OT 7968 FIN

                throw e;
            }
            finally
            {
                trans.Dispose();
                cn.Close();

                //OT 7968 INI
                transDepositos.Dispose();
                cnDepositos.Close();
                //OT 7968 FIN
            }
        }

        /// <summary>
        /// Obtiene la lista de fondos que el usuario puede elegir para revertir
        /// </summary>
        /// <returns>Tabla con los datos de los fondos que se puede seleccionar</returns>
        public DataTable ListarFondos()
        {
            ReversionDA da = new ReversionDA();
            return da.ObtenerFondos();
        }

        /// <summary>
        /// Obtiene la fecha que se podría revertir un fondo
        /// </summary>
        /// <param name="idFondo">Fondo que se desea revertir</param>
        /// <returns>Devuelve la fecha que se podría revertir el fondo seleccionado</returns>
        public string ObtenerFechaReversion(int idFondo)
        {
            ReversionDA da = new ReversionDA();
            return da.ObtenerFechaReversion(idFondo);

        }

        /*INI 10110*/
        public DataTable ObtenerUsuario(string codUsuario)
        {
            ReversionDA da = new ReversionDA();
            da.Database = INGFondos.Constants.Conexiones.BaseDeDatosAccesos;
            da.Server = INGFondos.Constants.Conexiones.ServidorAccesos;
            SqlConnection cn = da.GetConnection2();
            cn.Open();
            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                ProspectoMasivoDA prospectoMasivoDA = new ProspectoMasivoDA();
                DataTable dtUsuario = prospectoMasivoDA.ObtenerUsuario(codigoUsuario, cn, trans);
                trans.Commit();
                return dtUsuario;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                trans.Rollback();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                cn.Close();
            }
        }
        /*FIN 10110*/
        //INI OT11264 PSC002
        public void ReversionAlertaActividadCliente(int idFondo, DateTime fecha, string usuario)
        {
            ReversionDA da = new ReversionDA();

            da.Database = INGFondos.Constants.Conexiones.BaseDeDatosOperaciones;
            da.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;
            SqlConnection cn = da.GetConnection2();
            cn.Open();
            SqlTransaction trans = cn.BeginTransaction();
            try
            {

                da.ReversionAlertaActividadCliente(cn, trans, idFondo, fecha, usuario);
                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw e;
            }
            finally
            {
                trans.Dispose();
                cn.Close();
            }
        }
        //FIN OT11264 PSC002

        //OT11777 INI
        public DataTable ObtenerSolicitudReversionAprobada(int idFondo, int fecha)
        {
            ReversionDA da = new ReversionDA();
            return da.ObtenerSolicitudReversionAprobada(idFondo, fecha);
        }
        //OT11777 FIN
    }
}