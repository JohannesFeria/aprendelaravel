/*
----------------------------------------------------------
* Fecha de Creación	: 14/01/2013
* Modificado por	: Michell Cornejo
* Numero de OT		: 5187
* Descripción		: Creación.
----------------------------------------------------------
*/
using System;
using System.Data;
using Procesos.DA;
using INGFondos.Support.Interop;
using System.Data.SqlClient;
using System.Collections;

namespace Procesos.BL
{
	/// <summary>
	/// Descripción breve de MigracionCarteraBL.
	/// </summary>
	public class MigracionCarteraBL: INGFondos.Data.DA
	{

		public MigracionCarteraBL():base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones)
		{
		}

		public void Procesar(string sourcePath,string codigoUsuario)
		{
			ExcelApplication app = new ExcelApplication();
			ExcelWorkBook wb = app.OpenWorkBook(@sourcePath, ExcelMode.Full);
			ExcelWorkSheet sheet = wb.GetSheet(1);

			MigracionCarteraDA operacionesDA = new MigracionCarteraDA();
			SqlConnection cn = operacionesDA.getConexion();
			MigracionCarteraDA comercialDA = new MigracionCarteraDA();
			comercialDA.Server = INGFondos.Constants.Conexiones.ServidorComercial;
			comercialDA.Database = INGFondos.Constants.Conexiones.BaseDeDatosComercial;
			
			SqlConnection cnComercial = comercialDA.getConexion();

			int rowIndex = 3;
			try
			{
				cn.Open();
				cnComercial.Open();

				int contador = 0;
				
				object[] fila = sheet.GetRow(rowIndex ++, 'B', 'H');
	
				while (fila[0] != null)
				{			
					operacionesDA.ActualizarAsignacionPromotor(fila,codigoUsuario,cn);
					comercialDA.ActualizarProspecto(fila,codigoUsuario,cnComercial);
					fila = sheet.GetRow(rowIndex ++, 'B', 'H');
					contador++;
				}
			}
			catch(Exception ex)
			{
				throw new Exception("Error en la fila " + (rowIndex-1) + ".\r\nMensaje de sistema: " + ex.Message, ex);
			}
			finally
			{
				wb.Close();
				app.Close();
				cn.Close();
				cnComercial.Close();
			}
		}
	}
}
