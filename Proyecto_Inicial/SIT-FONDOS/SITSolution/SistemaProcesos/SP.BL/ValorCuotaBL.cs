/*
 * Fecha Modificaci�n:		15/08/2012
 * Modificado por:			Robert Castillo
 * Nro de OT:				4792 PSC1
 * Descripci�n del cambio:	Se crea la clase BO para actualizar el Valor Cuota de los fondos.
 * */
/*
 * Fecha Modificaci�n:		17/08/2012
 * Modificado por:			Robert Castillo
 * Nro de OT:				4792 PSC2
 * Descripci�n del cambio:	Se crea el m�todo obtenerCorreos y se adiciona una referencia
* */
/*
 * Fecha de Modificaci�n: 28/11/2012
 * Modificado por: Robert Castillo
 * Numero de OT: 5117
 * Descripci�n del cambio: Se cambia namespace
 * */
/*
 * Fecha de Modificaci�n : 14/06/2016
 * Modificado por        : Juan Castro
 * Nro. Orden de Trabajo : 8844
 * Descripci�n del cambio: Se sobrecarga los m�todos de inserci�n y actualizaci�n del VC.
 * */
using System;
using System.Data;
using SistemaProcesosTD;
using SistemaProcesosDA;
using System.Data.SqlClient;
//OT 4792 PSC2 Inicio--------
using INGFondos.Constants;
//OT 4792 PSC2 Fin-----------

namespace SistemaProcesosBL
{
	/// <summary>
	/// Descripci�n breve de ValorCuotaBL.
	/// </summary>
	public class ValorCuotaBO
	{

        //public DataTable obtenerFondos()
        //{
        //    ValorCuotaDA da = new ValorCuotaDA();
        //    DataTable fondo = da.ObtenerFondosPrecierre();
        //    return fondo;
        //}

        public DataTable obtenerValorCuota(string fecha, int idFondo)
        {
            ValorCuotaDA da = new ValorCuotaDA();
            DataTable valorCuota = da.obtenerValorCuota(fecha, idFondo);
            return valorCuota;
        }
        /*INICIO 8844*/
        public void actualizarValorCuota(string idFondo, double valorCuota, string fecha, string usuario)
        {
            actualizarValorCuota(idFondo, valorCuota, fecha, usuario, false);
        }
        public void actualizarValorCuota(string idFondo, double valorCuota, string fecha, string usuario, bool aprobado)
        {
            ValorCuotaDA da = new ValorCuotaDA();
            DataTable dtUsuario = da.obtenerUsuario(usuario);
            da.actualizarValorCuota(idFondo, valorCuota, fecha, usuario, "", aprobado);
        }

        public void insertarValorCuota(string fecha, double valorCuota, string usuario, int idFondo)
        {
            insertarValorCuota(fecha, valorCuota, usuario, idFondo, false);
        }

        public void insertarValorCuota(string fecha, double valorCuota, string usuario, int idFondo, bool aprobado)
        {
            ValorCuotaDA da = new ValorCuotaDA();
            DataTable dtUsuario = da.obtenerUsuario(usuario);
            da.insertarValorCuota(fecha, valorCuota, usuario, null, idFondo, aprobado);
        }
        /*FIN 8844*/
        ////OT 4792 PSC2 Inicio--------------------------------------------------------------------
        //public DataTable obtenerCorreos()
        //{
        //    FondoDA da = new FondoDA();
        //    DataTable dtCorreos = da.ObtenerLista(Constants.ConstantesING.CORREO_VALOR_CUOTA);
        //    return dtCorreos;
        //}
        ////OT 4792 PSC2 Fin-----------------------------------------------------------------------

	}
}
