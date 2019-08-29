/*
 * Fecha de Modificación: 26/05/2015
 * Modificado por:        Alex Vega
 * Numero de OT:          7349
 * Descripción del cambio: Creación. 
 * */

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Procesos.TD;
using Procesos.DA;
using System.Data.SqlClient;

namespace Procesos.BL
{
    public class ConceptoBO
    {
        public ConceptoBO()
        {
        }

        public DataTable ListarConceptoEstado(string strEstado, string strOrden)
        {
            ConceptoDA da = new ConceptoDA();
            return (da.ListarConceptoEstado(strEstado, strOrden));
        }

        public DataSet ObtenerConcepto(string strCodigo)
        {
            DataSet ds = new DataSet();
            ConceptoDA da = new ConceptoDA();
            ds.Tables.Add(da.ObtenerConcepto(strCodigo));
            return ds;
        }

        public DataTable ListarCuentas()
        {
            ConceptoDA da = new ConceptoDA();
            return (da.ListarCuentas());
        }

        public DataTable EliminarConcepto(string strCodigoConcepto, string strUsuario)
        {
            ConceptoDA da = new ConceptoDA();
            DataTable dt = da.EliminarConcepto(strCodigoConcepto,strUsuario);
            return dt;           
        }

        public string InsertarConcepto(string strCuenta, string strDescripcion, string strTipo, string strMoneda, string strEstado, string strUsuario)
        {
            ConceptoDA da = new ConceptoDA();
            string msg = da.InsertarConcepto(strCuenta, strDescripcion, strTipo, strMoneda, strEstado, strUsuario);
            return msg;
        }

        public int ActualizarConcepto(int intCaso, string strCuenta, string strDescripcion, string strTipo, string strMoneda, string strEstado, string strUsuario, string strCodigo)
        {
            ConceptoDA da = new ConceptoDA();
            int numero = da.ActualizarConcepto(intCaso, strCuenta, strDescripcion, strTipo, strMoneda, strEstado, strUsuario, strCodigo);
            return numero;
        }

        public int CorrelativoConcepto()
        {
            ConceptoDA conceptoDA = new ConceptoDA();
            return (conceptoDA.CorrelativoConcepto());
        }

        public DataTable ListarConceptos(string strTipoComprobante, string strEstado)
        {
            ConceptoDA conceptoDA = new ConceptoDA();
            return (conceptoDA.ListarConceptos(strTipoComprobante, strEstado));
        }

    }
}
