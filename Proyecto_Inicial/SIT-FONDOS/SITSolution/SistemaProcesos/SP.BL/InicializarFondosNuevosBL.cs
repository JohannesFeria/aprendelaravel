/*
 * Fecha de Modificación    : 19/11/2015    
 * Modificado por           : Richard Valdez
 * Numero de OT             : 7961
 * Descripción del cambio   : Creación
 * */
/*
 * Fecha de Modificación    : 29/02/2016    
 * Modificado por           : Irene Reyes
 * Numero de OT             : 8365
 * Descripción del cambio   : Agregar el campo CODIGO CONASEV.
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Procesos.DA;
using System.Data;

namespace Procesos.BL
{
    public class InicializarFondosNuevosBL
    {
        private InicializarFondosNuevosDA da = new InicializarFondosNuevosDA();

        /// <summary>
        /// Lista los Fondos Nuevos.
        /// Un fondo es considerado nuevo si tiene menos de 2 Valores Cuotas.
        /// </summary>
        public DataTable ObtenerFondosNuevos()
        {
            return da.ObtenerFondosNuevos();
        }

        /// <summary>
        /// Inicializar aquellos Fondos Nuevos que NO tienen Valor Cuota.
        /// </summary>
        /// <param name="FondoID"></param>
        /// <param name="FechaPrimerPrecierre"></param>
        /// <param name="ValorCuota"></param>
        /// <param name="NumeroContabilidadNueva"></param>
        /// <param name="NumeroContabilidadModelo"></param>
        /// <param name="FondoRUC"></param>
        /// <param name="FondoDescripcion"></param>
        /// <returns>En caso de éxito retorna una cadena vacía, en caso de error retorna un mensaje de error</returns>
        //OT 8365 INI
        public string InicializarFondosNuevos(int FondoID, DateTime FechaPrimerPrecierre, decimal ValorCuota,
            string NumeroContabilidadNueva, string NumeroContabilidadModelo, string FondoRUC, string FondoDescripcion, string codigoSMV)
        {
            return da.InicializarFondosNuevos(FondoID, FechaPrimerPrecierre, ValorCuota, NumeroContabilidadNueva,
                NumeroContabilidadModelo, FondoRUC, FondoDescripcion,codigoSMV);
        }
        //OT 8365 FIN

        /// <summary>
        /// Modificar la Inicialización de aquellos Fondos Nuevos que tienen 1 Valor Cuota.
        /// </summary>
        /// <param name="FondoID"></param>
        /// <param name="FechaPrimerPrecierre"></param>
        /// <returns>En caso de éxito retorna una cadena vacía, en caso de error retorna un mensaje de error</returns>
        public string ModificarInicializacionFondosNuevos(int FondoID, DateTime FechaPrimerPrecierre, decimal ValorCuota)
        {
            return da.ModificarInicializacionFondosNuevos(FondoID, FechaPrimerPrecierre, ValorCuota);
        }
    }
}
