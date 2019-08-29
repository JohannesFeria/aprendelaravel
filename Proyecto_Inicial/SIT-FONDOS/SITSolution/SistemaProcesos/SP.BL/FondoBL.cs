/*
 * Fecha de Modificación: 31/07/2012
 * Modificado por: Cesar Arasaki
 * Numero de OT: 4792
 * Descripción del cambio: Creación
 * */
/*
 * Fecha Modificación:		17/08/2012
 * Modificado por:			Robert Castillo
 * Nro de OT:				4792 PSC2
 * Descripción del cambio:	Se actualizan las referencias a las conexiones en el método obtenerFondos
* */
/*
 * Fecha de Modificación: 28/11/2012
 * Modificado por: Robert Castillo
 * Numero de OT: 5117
 * Descripción del cambio: Se cambia namespace
 * */
/*
 * Fecha de Modificación: 01/07/2013
 * Modificado por: Robert Castillo
 * Numero de OT: 5526
 * Descripción del cambio: En el método obtenerFondos se usa el método ToDateTime para la conversión
 *						  al tipo DateTime.
 * */
/*
 * Fecha de Modificación: 30/05/2016
 * Modificado por: Irene Reyes 
 * Numero de OT: 8829
 * Descripción del cambio: Validar los fondos inmobiliarios 
 * */
/*
 * Fecha de Modificación : 10/06/2016
 * Modificado por        : Juan Castro
 * Nro. Orden de Trabajo : 8844
 * Descripción del cambio: Se modifica la forma de obtener los valores tributarios.
 * */
/*
 * Fecha de Modificación : 20/11/2017
 * Modificado por        : Anthony Joaquin
 * Nro. Orden de Trabajo : 10928
 * Descripción del cambio: Se agrega el método verificarTributacion.
 * */

using System;
using System.Data;
using SistemaProcesosDA;
//////OT 4792 PSC2 Inicio-------
using SistemaProcesosTD.Constantes; //using INGFondos.Constants;
//////OT 4792 PSC2 Fin----------

namespace SistemaProcesosBL
{

    public class FondoBO
    {
        public FondoBO()
        {
        }

        /// <summary>
        /// Retorna DataSet con la tabla LISTA que contiene los tipo fondo y la tabla FONDO
        /// que obtiene la lista de fondos con su fecha de precierre, valor cuota y si se ha ejecutado el
        /// proceso tributario.
        /// </summary>
        public DataTable obtenerSoloFondos()
        {
            FondoDA da = new FondoDA();
            DataTable fondo = da.ObtenerFondosPrecierre();

            return fondo;
        }

        /// <summary>
        /// Retorna DataSet con la tabla LISTA que contiene los tipo fondo y la tabla FONDO
        /// que obtiene la lista de fondos con su fecha de precierre, valor cuota y si se ha ejecutado el
        /// proceso tributario.
        /// </summary>
        public DataSet obtenerFondos()
        {
            DataSet ds = new DataSet();

            FondoDA da = new FondoDA();
            ds.Tables.Add(da.ObtenerLista("TIPFON"));

            DataTable fondo = da.ObtenerFondosPrecierre();
            /*INICIO 8844*/
            TablaGeneralDA tgDa = new TablaGeneralDA();
            DataTable dtFondosInversiones = tgDa.ListarTablaGeneral(ConstantesING.CODIGO_TABLA_FONDOS_INVERSIONES);
            /*FIN 8844*/

            if (da.obtenerParametroXCodigo("GENERAR_RETENCIONES").Equals("S"))
            {
                foreach (DataRow rw in fondo.Rows)
                {
                    //OT 4792 PSC2 Inicio------------------------------------------------------
                    /*da.Database = INGFondos.Constants.Conexiones.BaseDeDatosTributacion;
                    da.Server = INGFondos.Constants.Conexiones.ServidorTributacion;*/
                    //OT 4792 PSC2 Fin---------------------------------------------------------

                    //rw["FLAG_EJECUTO_TRIBUTACION"] = da.verificarTributacionEjecutada(Int32.Parse(rw["ID_FONDO"].ToString()),Convert.ToDateTime(rw["FECHA_PRECIERRE"].ToString()))?"S":"N";
                    /*rw["FLAG_EJECUTO_TRIBUTACION"] = da.verificarTributacionEjecutada(Convert.ToInt32(rw["ID_FONDO"].ToString()),Convert.ToDateTime(rw["FECHA_PRECIERRE"]))?"S":"N";*/


                    da.Database = INGFondos.Constants.Conexiones.BaseDeDatosOperaciones;
                    da.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;

                    int resultado = 0;
                    resultado = da.ObtenerCantidadOperacionesTipoFlu(Convert.ToInt32(rw["ID_FONDO"].ToString()), Convert.ToDateTime(rw["FECHA_PRECIERRE"].ToString()));


                    if (resultado == 0)//!= null && pagoCupon.Equals("S"))
                    {
                        //OT 4792 PSC2 Inicio------------------------------------------------------
                        da.Database = INGFondos.Constants.Conexiones.BaseDeDatosTributacion;
                        da.Server = INGFondos.Constants.Conexiones.ServidorTributacion;
                        //OT 4792 PSC2 Fin---------------------------------------------------------

                        //rw["FLAG_EJECUTO_TRIBUTACION"] = da.verificarTributacionEjecutada(Convert.ToInt32(rw["ID_FONDO"].ToString()), Convert.ToDateTime(rw["FECHA_PRECIERRE"])) ? "S" : "S";
                        rw["FLAG_EJECUTO_TRIBUTACION"] = "S";

                    }
                    else
                    {
                        //OT 4792 PSC2 Inicio------------------------------------------------------
                        da.Database = INGFondos.Constants.Conexiones.BaseDeDatosTributacion;
                        da.Server = INGFondos.Constants.Conexiones.ServidorTributacion;
                        //OT 4792 PSC2 Fin---------------------------------------------------------
                        //rw["FLAG_EJECUTO_TRIBUTACION"] = da.verificarTributacionEjecutada(Int32.Parse(rw["ID_FONDO"].ToString()),Convert.ToDateTime(rw["FECHA_PRECIERRE"].ToString()))?"S":"N";
                        rw["FLAG_EJECUTO_TRIBUTACION"] = da.verificarTributacionEjecutada(Convert.ToInt32(rw["ID_FONDO"].ToString()), Convert.ToDateTime(rw["FECHA_PRECIERRE"])) ? "S" : "N";
                    }
                    /*INICIO 8844: Se obtienen los datos tributarios. No se incluyó en el método anterior porque la idea es que ya no se verifique la atribucion ejecutada.*/
                    DataRow[] dr = dtFondosInversiones.Select("LLAVE_TABLA = '" + rw["ID_FONDO"].ToString() + "'");
                    if (dr.Length == 1)
                    {
                        string descripcionLarga = dr[0]["DESCRIPCION_LARGA"].ToString().Trim();
                        string[] portafolioSerie = descripcionLarga.Split("|".ToCharArray());

                        rw["PORTAFOLIO"] = dr[0]["DESCRIPCION_CORTA"].ToString().Trim();
                        rw["SERIE"] = portafolioSerie.Length == 2 ? portafolioSerie[1] : "";
                    }
                    /*FIN 8844*/
                }
            }
            ds.Tables.Add(fondo);

            return ds;
        }

        //OT10928 INI
        public bool verificarTributacion(int idFondo, DateTime fecha)
        {
            FondoDA da = new FondoDA();
            da.Database = INGFondos.Constants.Conexiones.BaseDeDatosTributacion;
            da.Server = INGFondos.Constants.Conexiones.ServidorTributacion;
            return da.verificarTributacion(idFondo, fecha);
        }
        //OT10928 FIN

    }
}

