/*
 * Fecha de Modificación: 17/10/2013
 * Modificado por:        Carlos Galán
 * Numero de OT:          5924
 * Descripción del cambio: Se crea la clase AsientoPDRetencionBO, con los métodos ConsultarAlerta, InsertarAlerta, ObtenerTipoCambio, ObtenerRetenciones y InsertarAsientos
 */
using System;
using System.Data;
using System.Data.SqlClient;
using Procesos.DA;

namespace Procesos.BL
{
    /// <summary>
    /// Clase de Negocio que contiene los métodos para el llamado de los métodos de acceso a datos
    /// </summary>
    public class AsientoPDRetencionBO
    {
        public AsientoPDRetencionBO()
        {
            //
            // TODO: agregar aquí la lógica del constructor
            //
        }

        /// <summary>
        /// Llama al método ConsultarAlerta de acceso a datos.
        /// </summary>
        /// <param name="datFecha">Fecha de Proceso</param>
        /// <returns>Cantidad de alertas</returns>
        public DataTable ConsultarAlerta(DateTime datFecha)
        {
            AsientoPDRetencionDA da = new AsientoPDRetencionDA();
            DataTable ConsultarAlerta = da.ConsultarAlerta(datFecha);
            return ConsultarAlerta;
        }

        /// <summary>
        /// Llama al método InsertarAlerta de acceso a datos.
        /// </summary>
        /// <param name="datFecha">Fecha de proceso</param>
        /// <param name="strUsuario">Usuario que generó los asientos</param>
        public void InsertarAlerta(DateTime datFecha, string strUsuario)
        {
            AsientoPDRetencionDA da = new AsientoPDRetencionDA();
            da.InsertarAlerta(datFecha, strUsuario);
        }

        /// <summary>
        /// Llama al método ObtenerTipoCambio de acceso a datos.
        /// </summary>
        /// <param name="strFecha">Fecha de proceso</param>
        /// <returns>Registro de tipo de cambio</returns>
        public DataTable ObtenerTipoCambio(string strFecha)
        {
            AsientoPDRetencionDA da = new AsientoPDRetencionDA();
            DataTable ObtenerTipoCambio = da.ObtenerTipoCambio(strFecha);
            return ObtenerTipoCambio;
        }

        /// <summary>
        /// Llama al método ObtenerRetenciones de acceso a datos.
        /// </summary>
        /// <param name="datFecha">Fecha de proceso</param>
        /// <returns>Retenciones por moneda</returns>
        public DataTable ObtenerRetenciones(DateTime datFecha)
        {
            AsientoPDRetencionDA da = new AsientoPDRetencionDA();
            DataTable ObtenerRetenciones = da.ObtenerRetenciones(datFecha);
            return ObtenerRetenciones;
        }

        /// <summary>
        /// Llama al método InsertarAsientos de acceso a datos.
        /// </summary>
        /// <param name="strAnio">Año de asiento</param>
        /// <param name="strMes">Mes de asiento</param>
        /// <param name="strFecha">Fecha de proceso</param>
        /// <param name="strNumDoc">Número de Documento</param>
        /// <param name="strCheque">Número de Cheque</param>
        /// <param name="strMoneda">Moneda</param>
        /// <param name="strTipoCambio">Tipo de cambio de la fecha de proceso</param>
        /// <param name="decMontoSol">Monto en soles</param>
        /// <param name="decMontoDol">Monro en dolares</param>
        public bool InsertarAsientos(string strAnio, string strMes, string strFecha, string strNumDoc, string strCheque, string strMoneda, decimal strTipoCambio, decimal decMontoSol, decimal decMontoDol) 
        {
            AsientoPDRetencionDA da = new AsientoPDRetencionDA();
            return da.InsertarAsientos(strAnio, strMes, strFecha, strNumDoc, strCheque, strMoneda, strTipoCambio, decMontoSol, decMontoDol);
        }
            
    }
}
