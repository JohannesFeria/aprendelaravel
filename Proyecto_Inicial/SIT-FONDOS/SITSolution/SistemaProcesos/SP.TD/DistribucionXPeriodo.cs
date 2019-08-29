/*
 * Fecha de Creación: 12/06/2017
 * Creado por: Anthony Joaquin
 * Número de OT: 10478
 * Descripción del cambio: Creación
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procesos.TD
{
    public class DistribucionXPeriodo
    {
        public DistribucionXPeriodo()
        {
        }

        int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
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

		decimal porcentaje;

		public decimal Porcentaje
		{
            get { return porcentaje; }
            set { porcentaje = value; }
		}

        string fechaCorte;

        public string FechaCorte 
        {
            get { return fechaCorte; }
            set { fechaCorte = value; }
        }

    }
}
