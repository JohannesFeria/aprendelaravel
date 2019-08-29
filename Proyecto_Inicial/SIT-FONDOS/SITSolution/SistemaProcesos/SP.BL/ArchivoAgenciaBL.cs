/*
 * Fecha Modificación:		19/11/2013
 * Modificado por:			Giovana Veliz
 * Nro de OT:				5908
 * Descripción del cambio:	Se crea la clase BL para generar el archivo de posiciones.
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
    /// Descripción breve de ArchivoAgenciaBL.
	/// </summary>
	public class ArchivoAgenciaBL
	{
        public ArchivoAgenciaBL()
		{
			//
			// TODO: agregar aquí la lógica del constructor
			//
		}	

        /// <summary>
        /// Método que genera el archivo plano que contiene los registros de saldos
        /// de promotores, en la carperta enviada como parámetro.        
        /// </summary>
        /// <param name="rutaArchivo">Ruta donde se generará el archivo plano</param>
        /// <param name="codAgencia">Código de Agencia</param>
        /// <param name="fecha">Fecha de corte del reporte</param>
        /// <returns>Cantidad de registros encontrados</returns>
		public int GenerarArchivo(string rutaArchivo,string codAgencia, DateTime fecha){

			FondoDA fondoDA = new FondoDA();			
            DataTable dtPosiciones;

            ArchivoAgenciaDA archivoAgenciaDA = new ArchivoAgenciaDA();	

			StringBuilder sbLinea;
			sbLinea = new StringBuilder();
			StringBuilder ruta = new StringBuilder();
			ruta.Append("");


            dtPosiciones = archivoAgenciaDA.ObtenerPosicionesAgencia(codAgencia, fecha);
			StreamWriter sw;

            if (dtPosiciones.Rows.Count > 0)
            {
				if(ruta.Length > 0)
					ruta.Remove(0,ruta.Length);

                ruta.Append("Posicion_");				
				ruta.Append(fecha.ToString("ddMMyyyy") + ".txt");
				sw = new StreamWriter(rutaArchivo + "\\" + ruta.ToString());

				if(sbLinea.Length > 0)
					sbLinea.Remove(0,sbLinea.Length);

                foreach (DataRow drPosiciones in dtPosiciones.Rows)
				{
                    sbLinea.Append(drPosiciones["CUC"].ToString() + ";");
                    sbLinea.Append(drPosiciones["Nombre"].ToString() + ";");
                    sbLinea.Append(drPosiciones["Tipo_documento"].ToString() + ";");
                    sbLinea.Append(drPosiciones["Num_documento"].ToString() + ";");
                    sbLinea.Append(drPosiciones["Codigo_fondo"].ToString() + ";");
                    sbLinea.Append(drPosiciones["Nombre_fondo"].ToString() + ";");
                    sbLinea.Append(drPosiciones["Descripcion_fondo"].ToString() + ";");
                    sbLinea.Append(drPosiciones["Moneda_fondo"].ToString() + ";");
                    sbLinea.Append(drPosiciones["Numero_cuotas"].ToString() + ";");
                    sbLinea.Append(drPosiciones["Valor_cuota"].ToString() + ";");
                    sbLinea.Append(drPosiciones["Monto"].ToString() + ";");
                    sbLinea.Append(drPosiciones["Fecha_proceso"].ToString() + ";");
                    sbLinea.Append(drPosiciones["Estado_fondo"].ToString());
					sbLinea.Append("\r\n");
				}
				sw.WriteLine(sbLinea);
				sw.Close();
			}

            return dtPosiciones.Rows.Count;
		}
	}
}
