/*
 * Fecha de Creación: 19/09/2017
 * Creado por: Anthony Joaquin
 * Número de OT: 10592
 * Descripción del cambio: Creación
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procesos.TD
{
    public class PagoFlujoSobranteTD
    {
        public PagoFlujoSobranteTD()
        {
        }

        int idFondo;

        public int IdFondo
        {
            get { return idFondo; }
            set { idFondo = value; }
        }


        int periodo;

        public int Periodo
        {
            get { return periodo; }
            set { periodo = value; }
        }

        DateTime fecha;

        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        string orden;
        public string Orden
        {
            get { return orden; }
            set { orden = value; }
        }

        DateTime liquidacion;
        public DateTime Liquidacion
        {
            get { return liquidacion; }
            set { liquidacion = value; }
        }

        DateTime finContrato;

        public DateTime FinContrato
        {
            get { return finContrato; }
            set { finContrato = value; }
        }


        string moneda;
        public string Moneda
        {
            get { return moneda; }
            set { moneda = value; }
        }

        string mnemonico;
        public string Mnemonico
        {
            get { return mnemonico; }
            set { mnemonico = value; }
        }

        string operacion;
        public string Operacion
        {
            get { return operacion; }
            set { operacion = value; }
        }


        decimal tasa;
        public decimal Tasa
        {
            get { return tasa; }
            set { tasa = value; }
        }


        decimal tipoCambio;
        public decimal TipoCambio
        {
            get { return tipoCambio; }
            set { tipoCambio = value; }
        }


        decimal precio;
        public decimal Precio
        {
            get { return precio; }
            set { precio = value; }
        }


        decimal monto;
        public decimal Monto
        {
            get { return monto; }
            set { monto = value; }
        }

        decimal cantidad;
        public decimal Cantidad
        {
            get { return cantidad; }
            set { cantidad = value; }
        }

        string intermediario;
        public string Intermediario
        {
            get { return intermediario; }
            set { intermediario = value; }
        }

        string estado;
        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        int dias;
        public int Dias
        {
            get { return dias; }
            set { dias = value; }
        }

        decimal factor;
        public decimal Factor
        {
            get { return factor; }
            set { factor = value; }
        }

        decimal montoFinal;
        public decimal MontoFinal
        {
            get { return montoFinal; }
            set { montoFinal = value; }
        }

        decimal interesDPZ;
        public decimal InteresDPZ
        {
            get { return interesDPZ; }
            set { interesDPZ = value; }
        }

        decimal interesCupones;
        public decimal InteresCupones
        {
            get { return interesCupones; }
            set { interesCupones = value; }
        }


        decimal distribucionFlujo;
        public decimal DistribucionFlujo
        {
            get { return distribucionFlujo; }
            set { distribucionFlujo = value; }
        }

        decimal saldoPendiente;
        public decimal SaldoPendiente
        {
            get { return saldoPendiente; }
            set { saldoPendiente = value; }
        }

        string usuario;
        public string Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }

        string area;
        public string Area
        {
            get { return area; }
            set { area = value; }
        }
    }

}
