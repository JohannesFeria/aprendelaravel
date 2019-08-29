/*
 * Fecha de Creación: 07/10/2016
 * Creado por: Irene Reyes
 * Número de OT: 8954
 * Descripción del cambio: Creación
 * 
 */
/*
 * Fecha de Modificación: 20/10/2016
 * Modificado por: Irene Reyes
 * Número de OT: 8954 PSC-001
 * Descripción del cambio: Agregar nuevos campos para la operación traspaso.
 * 
 */
/*
 * Fecha de Modificación : 26/04/2017
 * Modificado por        : Anthony Joaquin
 * Nro. Orden de Trabajo : 10217
 * Descripción del cambio: Se agrega propiedad para id fondo referencia.
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procesos.TD
{
    public class OperacionMasivo
    {
        public OperacionMasivo()
        {
        }

        int idParticipeOrigen;

        public int IdParticipeOrigen
        {
            get { return idParticipeOrigen; }
            set { idParticipeOrigen = value; }
        }
        int idParticipeDestino;

        public int IdParticipeDestino
        {
            get { return idParticipeDestino; }
            set { idParticipeDestino = value; }
        }
        int idFondoOrigen;

        public int IdFondoOrigen
        {
            get { return idFondoOrigen; }
            set { idFondoOrigen = value; }
        }
        int idFondoDestino;

        public int IdFondoDestino
        {
            get { return idFondoDestino; }
            set { idFondoDestino = value; }
        }
        int idCuentaOrigen;

        public int IdCuentaOrigen
        {
            get { return idCuentaOrigen; }
            set { idCuentaOrigen = value; }
        }
        int idCuentaDestino;

        public int IdCuentaDestino
        {
            get { return idCuentaDestino; }
            set { idCuentaDestino = value; }
        }
        string flagMonedaDistinto;

        public string FlagMonedaDistinto
        {
            get { return flagMonedaDistinto; }
            set { flagMonedaDistinto = value; }
        }
        string usuario;

        public string Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }
        DateTime fecha;

        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }
        string area;

        public string Area
        {
            get { return area; }
            set { area = value; }
        }

        //Campos para Operacion Traspaso

        //OT8954 PSC-001 INI

        int Cuc;

        public int CucOrigen
        {
            get { return Cuc; }
            set { Cuc = value; }
        }

        int IdOrigen;

        public int IdParticipeOrigenTraspaso
        {
            get { return IdOrigen; }
            set { IdOrigen = value; }
        }

        int CUC;

        public int CucDestino
        {
            get { return CUC; }
            set { CUC = value; }
        }

        int IdDestino;

        public int IdParticipeDestinoTraspaso
        {
            get { return IdDestino; }
            set { IdDestino = value; }
        }

        string CuentaParticipacionOrigen;

        public string CuentaParticipacionOrigenTraspaso
        {
            get { return CuentaParticipacionOrigen; }
            set { CuentaParticipacionOrigen = value; }
        }

        int IdCuentaParticipacionOrigen;

        public int IdCuentaParticipacionOrigenTraspaso
        {
            get { return IdCuentaParticipacionOrigen; }
            set { IdCuentaParticipacionOrigen = value; }
        }


        string CuentaParticipacionDestino;

        public string CuentaParticipacionDestinoTraspaso
        {
            get { return CuentaParticipacionDestino; }
            set { CuentaParticipacionDestino = value; }
        }

        int IdCuentaParticipacionDestino;

        public int IdCuentaParticipacionDestinoTraspaso
        {
            get { return IdCuentaParticipacionDestino; }
            set { IdCuentaParticipacionDestino = value; }
        }

        string NombreFondoOrigen;

        public string NombreFondoOrigenTraspaso
        {
            get { return NombreFondoOrigen; }
            set { NombreFondoOrigen = value; }
        }

        int IdNombreFondoOrigen;

        public int IdNombreFondoOrigenTraspaso
        {
            get { return IdNombreFondoOrigen; }
            set { IdNombreFondoOrigen = value; }
        }


        DateTime horaHasta;
        public DateTime HoraHasta
        {
            get { return horaHasta; }
            set { horaHasta = value; }
        }

        DateTime fechaProceso;
        public DateTime FechaProceso
        {
            get { return fechaProceso; }
            set { fechaProceso = value; }
        }
        
       
        string tipoTraspaso;

        public string TipoTraspaso
        {
            get { return tipoTraspaso; }
            set { tipoTraspaso = value; }
        }

        decimal monto;

        public decimal Monto
        {
            get { return monto; }
            set { monto = value; }
        }

        decimal cuotas;

        public decimal Cuotas
        {
            get { return cuotas; }
            set { cuotas = value; }
        }

        string codigoPromotorOrigen;

        public string CodigoPromotorOrigen
        {
            get { return codigoPromotorOrigen; }
            set { codigoPromotorOrigen = value; }
        }

        decimal valorCuota;

        public decimal ValorCuota
        {
            get { return valorCuota; }
            set { valorCuota = value; }
        }

        //OT8954 PSC-001 FIN
				int idFondo;

				public int IdFondo
				{
					get { return idFondo; }
					set { idFondo = value; }
				}

				int idOperacionReferencia;

				public int IdOperacionReferencia
				{
					get { return idOperacionReferencia; }
					set { idOperacionReferencia = value; }
				}
    }
}
