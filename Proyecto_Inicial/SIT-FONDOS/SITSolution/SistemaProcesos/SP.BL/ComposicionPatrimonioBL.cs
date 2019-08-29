/*
 * Fecha Modificación:		19/09/2012
 * Modificado por:			Robert Castillo
 * Nro de OT:				4959
 * Descripción del cambio:	Se crea la clase BO para generar el archivo de Composición Patrimonial.
 * */
/*
 * Fecha de Modificación: 28/11/2012
 * Modificado por: Robert Castillo
 * Numero de OT: 5117
 * Descripción del cambio: Se cambia namespace
 * */
using System;
using System.IO;
using System.Data;
using System.Text;
using Procesos.TD;
using Procesos.DA;

namespace Procesos.BL
{
	/// <summary>
	/// Descripción breve de ValorCuotaBL.
	/// </summary>
	public class ComposicionPatrimonioBO
	{
		public ComposicionPatrimonioBO()
		{
			//
			// TODO: agregar aquí la lógica del constructor
			//
		}	

		public int generarArchivo(string rutaArchivo, DateTime fecha){
			FondoDA fondoDA = new FondoDA();
			DataTable dtPortaPadres = fondoDA.ObtenerLista("PORTA_PADRE");
			DataTable dtProporciones;

			ComposicionPatrimonioDA composicionPatrimonioDA = new ComposicionPatrimonioDA();	

			StringBuilder sbLinea;
			sbLinea = new StringBuilder();
			StringBuilder ruta = new StringBuilder();
			ruta.Append("");

			foreach(DataRow drPortaPadre in dtPortaPadres.Rows)
			{
				dtProporciones = composicionPatrimonioDA.ObtenerProporciones( fecha, drPortaPadre["LLAVE_TABLA"].ToString());
				StreamWriter sw;

				if(dtProporciones.Rows.Count>0){
					if(ruta.Length > 0)
						ruta.Remove(0,ruta.Length);

					ruta.Append("PR_");
					ruta.Append(dtProporciones.Rows[0]["PORTA_PADRE"].ToString() + "_");
					ruta.Append(fecha.ToString("yyyyMMdd") + ".txt");
					sw = new StreamWriter(rutaArchivo + "\\" + ruta.ToString());

					if(sbLinea.Length > 0)
						sbLinea.Remove(0,sbLinea.Length);

					foreach(DataRow drProporcion in dtProporciones.Rows)
					{
						sbLinea.Append(drProporcion["PORTA_PADRE"].ToString() + ";");
						sbLinea.Append(fecha.ToString("dd/MM/yyyy") + ";");
						sbLinea.Append(drProporcion["CODIGO_SERIE"].ToString() + ";");
						sbLinea.Append(drProporcion["PORCENTAJE"].ToString());
						sbLinea.Append("\r\n");
					}
					sw.WriteLine(sbLinea);
					sw.Close();
				}
			}
			return dtPortaPadres.Rows.Count;
		}
	}
}
