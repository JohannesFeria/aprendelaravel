/*
 * Fecha de Modificación: 16/07/2013
 * Modificado por: Davis Rixi
 * Numero de OT: 5643
 * Descripción del cambio: Creación
 * */


using System;
using System.Data;
using Procesos.DA;
using INGFondos.Constants;


namespace Procesos.BL
{

	public class EventoCalendarioBO
	{
        public EventoCalendarioBO()
		{
		}


        public DataSet ObtenerPromotoresEvento()
		{
			DataSet ds = new DataSet();

			EventoCalendarioDA da = new EventoCalendarioDA();		
			ds.Tables.Add(da.ObtenerPromotoresEvento());
			return ds;
		}

        public void ActualizarActividadIdCalendar(int idActividad, string idCalendar)
        {
            EventoCalendarioDA da = new EventoCalendarioDA();
            da.ActualizarActividadIdCalendar(idActividad, idCalendar);
        }
	}
}
