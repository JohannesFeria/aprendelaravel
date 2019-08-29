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
using Procesos.DA;
using System.Data;
using System.Data.SqlClient;

using Procesos.TD;

namespace Procesos.BL
{
	public class RescateReversionBL
	{
		RescateReversionDA rescateReversionDA = new RescateReversionDA();

		public void reversionRescatePreliminar(int idFondo, string usuario)
		{
			RescateReversionDA rescateReversionDA = new RescateReversionDA();
			rescateReversionDA.reversionRescatePreliminar(idFondo, usuario);
		}

		public DataTable validarPrecierreEjecutadoUltimaFechaCierre(int idFondo)
		{
			RescateReversionDA rescateReversionDA = new RescateReversionDA();
			return rescateReversionDA.validarPrecierreEjecutadoUltimaFechaCierre(idFondo);
		}

		public DataTable listarFondosReversionCierre()
		{
			RescateReversionDA rescateReversionDA = new RescateReversionDA();
			return rescateReversionDA.listarFondosReversionCierre();
		}
	}
}