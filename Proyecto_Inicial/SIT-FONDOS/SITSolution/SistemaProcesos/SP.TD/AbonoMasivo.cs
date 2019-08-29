/*
 * Fecha de Creación: 07/10/2016
 * Creado por: Irene Reyes
 * Número de OT: 8954
 * Descripción del cambio: Creación
 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procesos.TD
{
    public class AbonoMasivo
    {

        int cuc;
        public int Cuc
        {
            get { return cuc; }
            set { cuc = value; }
        }

        string numOperacionBancaria;
        public string NumOperacionBancaria
        {
            get { return numOperacionBancaria; }
            set { numOperacionBancaria = value; }
        }

        string fondo;
        public string Fondo
        {
            get { return fondo; }
            set { fondo = value; }
        }
        decimal monto;
        public decimal Monto
        {
            get { return monto; }
            set { monto = value; }
        }

        DateTime fechaContable;

        public DateTime FechaContable
        {
            get { return fechaContable; }
            set { fechaContable = value; }
        }

        DateTime fechaDisponible;

        public DateTime FechaDisponible
        {
            get { return fechaDisponible; }
            set { fechaDisponible = value; }
        }

        string referencia;
        public string Referencia
        {
            get { return referencia; }
            set { referencia = value; }
        }
        int idFondo;

        public int IdFondo
        {
            get { return idFondo; }
            set { idFondo = value; }
        }


    }
}
