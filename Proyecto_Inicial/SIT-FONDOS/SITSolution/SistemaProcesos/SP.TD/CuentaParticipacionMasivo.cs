/*
 * Fecha de Creación: 07/06/2017
 * Creado por: Anthony Joaquin
 * Número de OT: OT10433
 * Descripción del cambio: Creación
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procesos.TD
{
    public class CuentaParticipacionMasivo
    {
        public CuentaParticipacionMasivo()
        {
        }

        int cuc;

        public int Cuc
        {
            get { return cuc; }
            set { cuc = value; }
        }

        string etiqueta;

        public string Etiqueta
        {
            get { return etiqueta; }
            set { etiqueta = value; }
        }

				string flagBloqueada;

				public string FlagBloqueada
				{
					get { return flagBloqueada; }
					set { flagBloqueada = value; }
				}

        int idParticipe;

        public int IdParticipe 
        {
            get { return idParticipe; }
            set { idParticipe = value; }
        }

        
        string codigoPlan;

        public string CodigoPlan
        {
            get { return codigoPlan; }
            set { codigoPlan = value; }
        }

				string estado;

				public string Estado
				{
					get { return estado; }
					set { estado = value; }
				}
    }
}
