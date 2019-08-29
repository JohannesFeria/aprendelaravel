/*
 * Fecha de Modificación: 16/12/2014
 * Modificado por: Giovana Veliz
 * Numero de OT: 7014
 * Descripción del cambio: Creación
 * */

using System;
using System.Data;
using Procesos.DA;
using INGFondos.Support.Interop;
using System.Data.SqlClient;
using System.Collections;
using Procesos.TD;

using INGFondos.Data;

namespace Procesos.BL
{

	public class SegmentacionRegionalBL: INGFondos.Data.DA
	{
		private string sourcePath;

		private string codigoUsuario;

        public SegmentacionRegionalBL(string sourcePath, string codigoUsuario)
            : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones)
		{

			this.sourcePath = sourcePath;
			this.codigoUsuario = codigoUsuario;
			
		}


		public void Procesar()
		{
			ExcelApplication excelApplication = new ExcelApplication();			
			SqlConnection cn = GetConnection();
			cn.Open();

			SqlTransaction trans = cn.BeginTransaction(IsolationLevel.ReadUncommitted);
			
			try
			{
		
				// Abriendo el archivo que contiene la información
				//excelApplication.OpenWorkBook(sourcePath, ExcelMode.Full);
                excelApplication.OpenWorkBook(sourcePath, ExcelMode.ReadOnly);
                ExcelWorkSheet sheet = excelApplication.GetWorkBook(1).GetSheet(1);
				
				//Obtiene los prospectos vacios
                SegmentacionRegionalDA segmentacionRegionalDA = new SegmentacionRegionalDA();				

				// Obteniendo la información del archivo
				int rowIndex = 2;
				object[] row = sheet.GetRow(rowIndex ++, 'A', 'B');
				int contador = 0;
				
				while (row[0] != null)
				{
					try
					{									
						//Leer datos del Partícipe
                        ParticipeSegmentacionTD.PARTICIPE_SEGMENTACION_REGIONALRow drParticipeSegmentacion = new ParticipeSegmentacionTD().PARTICIPE_SEGMENTACION_REGIONAL.NewPARTICIPE_SEGMENTACION_REGIONALRow();                        
                        drParticipeSegmentacion.NUMERO_DOCUMENTO = row[0].ToString();
                        drParticipeSegmentacion.SEGMENTACION_REGIONAL = row[1].ToString();
                        drParticipeSegmentacion.USUARIO = codigoUsuario;

                        segmentacionRegionalDA.ActualizarParticipeSegmentacion(drParticipeSegmentacion, cn, trans);

						row = sheet.GetRow(rowIndex ++, 'A', 'B');
						contador++;
					}
					catch(Exception ex)
					{
						throw new Exception(string.Format("Error en la línea {0}. Detalle " + ex.Message, rowIndex - 1));
					}
				}

				trans.Commit();
                //excelApplication.GetWorkBook(1).Save();
				
			}
			catch(Exception ex)
			{
				trans.Rollback();
				excelApplication.GetWorkBook(1).Close();
				excelApplication.Close();
				throw ex;
			}
			finally
			{
				trans.Dispose();
				cn.Close();				
			}
		}

	}
}
