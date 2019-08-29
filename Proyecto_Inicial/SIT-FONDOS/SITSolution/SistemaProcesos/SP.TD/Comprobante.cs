/*
 * Fecha de Modificación: 20/11/2015
 * Modificado por: Robert Castillo
 * Numero de OT: 7999
 * Descripción del cambio: Se agrega porcentajeDetraccion y montoDetraccion.
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procesos.TD
{
    public class Comprobante
    {
        private int idComprobante;

        public int IdComprobante
        {
            get { return idComprobante; }
            set { idComprobante = value; }
        }

        private string tipoDocumento;

        public string TipoDocumento
        {
            get { return tipoDocumento; }
            set { tipoDocumento = value; }
        }

        private string serie;

        public string Serie
        {
            get { return serie; }
            set { serie = value; }
        }

        private string numero;

        public string Numero
        {
            get { return numero; }
            set { numero = value; }
        }

        private string moneda;

        public string Moneda
        {
            get { return moneda; }
            set { moneda = value; }
        }

        private DateTime fechaEmision;

        public DateTime FechaEmision
        {
            get { return fechaEmision; }
            set { fechaEmision = value; }
        }

        private Cliente cliente;

        public Cliente Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }

        private string proceso;

        public string Proceso
        {
            get { return proceso; }
            set { proceso = value; }
        }

        private string expediente;

        public string Expediente
        {
            get { return expediente; }
            set { expediente = value; }
        }

        private string codigoConcepto;

        public string CodigoConcepto
        {
            get { return codigoConcepto; }
            set { codigoConcepto = value; }
        }

        private string tipoDocumentoRelacionado;

        public string TipoDocumentoRelacionado
        {
            get { return tipoDocumentoRelacionado; }
            set { tipoDocumentoRelacionado = value; }
        }

        private string numeroDocumentoRelacionado;

        public string NumeroDocumentoRelacionado
        {
            get { return numeroDocumentoRelacionado; }
            set { numeroDocumentoRelacionado = value; }
        }

        private DateTime fechaDocumentoRelacionado;

        public DateTime FechaDocumentoRelacionado
        {
            get { return fechaDocumentoRelacionado; }
            set { fechaDocumentoRelacionado = value; }
        }

        private string estado;

        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        private string flagImpresion;

        public string FlagImpresion
        {
            get { return flagImpresion; }
            set { flagImpresion = value; }
        }

        private string flagEnviado;

        public string FlagEnviado
        {
            get { return flagEnviado; }
            set { flagEnviado = value; }
        }

        private string flagAfectoIgv;

        public string FlagAfectoIgv
        {
            get { return flagAfectoIgv; }
            set { flagAfectoIgv = value; }
        }

        private string glosa;

        public string Glosa
        {
            get { return glosa; }
            set { glosa = value; }
        }

        private int cantidad;

        public int Cantidad
        {
            get { return cantidad; }
            set { cantidad = value; }
        }

        private double precioUnitario;

        public double PrecioUnitario
        {
            get { return precioUnitario; }
            set { precioUnitario = value; }
        }

        private double valorVenta;

        public double ValorVenta
        {
            get { return valorVenta; }
            set { valorVenta = value; }
        }

        private double igv;

        public double Igv
        {
            get { return igv; }
            set { igv = value; }
        }

        private double total;

        public double Total
        {
            get { return total; }
            set { total = value; }
        }

        private string usuario;

        public string Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }

        private string uuid;

        public string Uuid
        {
            get { return uuid; }
            set { uuid = value; }
        }

        private string tipoNotaCredito;

        public string TipoNotaCredito
        {
            get { return tipoNotaCredito; }
            set { tipoNotaCredito = value; }
        }

        private List<DetalleFactura> listaDetalle;

        public List<DetalleFactura> ListaDetalle
        {
            get { return listaDetalle; }
            set { listaDetalle = value; }
        }

        /* Inicio OT7999*/
        private double porcentajeDetraccion;

        public double PorcentajeDetraccion
        {
            get { return porcentajeDetraccion; }
            set { porcentajeDetraccion = value; }
        }

        private double montoDetraccion;

        public double MontoDetraccion
        {
            get { return montoDetraccion; }
            set { montoDetraccion = value; }
        }

        /* Fin OT7999*/
    }

}
