/*
* Fecha de Modificación		: 04/10/2017
* Modificado por			: Rosmery Contreras
* Numero de OT				: 10808
* Descripción del cambio	: Creacion.
* */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procesos.TD
{
    public class RescateCierre
    {
        public RescateCierre()
        {
        }

		string tipoProceso;
		public string TipoProceso
		{
			get { return tipoProceso; }
			set { tipoProceso = value; }
		}

		DateTime fechaProceso;
		public DateTime FechaProceso
		{
			get { return fechaProceso; }
			set { fechaProceso = value; }
		}

        int idFondo;
        public int IdFondo
        {
            get { return idFondo; }
            set { idFondo = value; }
        }
        
        decimal var1;
        public decimal Var1
        {
            get { return var1; }
            set { var1 = value; }
        }

		decimal var2;
		public decimal Var2
        {
            get { return var2; }
            set { var2 = value; }
        }

        string usuarioCreacion;
        public string UsuarioCreacion
        {
            get { return usuarioCreacion; }
            set { usuarioCreacion = value; }
        }

        string usuarioModificacion;
        public string UsuarioModificacion
        {
            get { return usuarioModificacion; }
            set { usuarioModificacion = value; }
        }

        DateTime fechaCreacion;
        public DateTime FechaCreacion
        {
            get { return fechaCreacion; }
            set { fechaCreacion = value; }
        }

        DateTime fechaModificacion;
        public DateTime FechaModificacion
        {
            get { return fechaModificacion; }
            set { fechaModificacion = value; }
        }

		string areaModificacion;
		public string AreaModificacion
		{
			get { return areaModificacion; }
			set { areaModificacion = value; }
		}
    
    }
}
