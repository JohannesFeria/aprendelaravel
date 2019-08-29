/*
 * Fecha de Modificación: 20/06/2013
 * Modificado por: Robert Castillo
 * Numero de OT: 5526
 * Descripción del cambio: Creación de clase BO para la creación de Prospectos Masivos.
 * */
/*
 * Fecha de Modificación: 06/05/2015
 * Modificado por: Walter Rodríguez
 * Numero de OT: 7235
 * Descripción del cambio: Se captura el primer y último CUC de la generación para registrar el Lote.
 * */
using System;
using System.Data;
//using Procesos.UI;

using Procesos.DA;
//using Procesos.BL;
using INGFondos.Support.Interop;
using System.Data.SqlClient;
using System.Collections;
using Procesos.TD;
using INGFondos.Data;
using System.IO;
using System.Text;
using System.Configuration;


/*
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Procesos.BL;
using Procesos.Constants;
using FondosSura.SistemaProcesos.Util;
using System.Text;
*/


namespace Procesos.BL
{
	/// <summary>
	/// Descripción breve de ProspectoMasivoBL.
	/// </summary>
	public class ProspectoMasivoBO: INGFondos.Data.DA
	{
		private string sourcePath;

		private string codigoUsuario;

		private int idParticipe;
		private string idParticipeAcceso;
		private string codigoParticipe;

		private DataTable dtPrmGrabarDatosParticipe;
		private DataTable dtCodigoParticipe;

		public ProspectoMasivoBO(string sourcePath, string codigoUsuario):base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones)
		{
			this.sourcePath = sourcePath;
			this.codigoUsuario = codigoUsuario;
		}

		public string generarProspectoMasivo(int cantidad, string rutaArchivo){

			ProspectoMasivoDA prospectoMasivoDA = new ProspectoMasivoDA();

                        string codidoLote = DateTime.Now.ToString("yyyyMMdd-hhmm");

			SqlConnection cn = GetConnection();
			cn.Open();
			SqlTransaction trans = cn.BeginTransaction(IsolationLevel.ReadUncommitted);
			
			ConnectionManager cm =  new ConnectionManager();
			
			string server = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.ServidorAccesos];
			string database = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.BaseDeDatosAccesos];

			SqlConnection cn2 = cm.GetTrustedConnection(server, database);
			cn2.Open();
			SqlTransaction trans2 = cn2.BeginTransaction(IsolationLevel.ReadUncommitted);

			StreamWriter sw;

			DataTable dtRuta = prospectoMasivoDA.ObtenerParametroSistema("RUTA_ARCHIVO_CREACION_MASIVA", cn, trans);
			//String rutaArchivo;// = dtRuta.Rows[0]["VALOR"].ToString();
			//aquí debe ir la ruta obtenida x FolderBrowser
			rutaArchivo = rutaArchivo + "\\INGFONDOSPROSPECT_" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss") + ".txt";

			sw = new StreamWriter(rutaArchivo, false, Encoding.Default);

			StringBuilder cabecera = new StringBuilder();

			cabecera.Append("Fecha" + new String(' ', 4) + ": " +  DateTime.Now.ToString("dd/MM/yyyy hh:mm") + "\r\n");
			
			DataTable dtUsuario = prospectoMasivoDA.ObtenerUsuario(codigoUsuario, cn2, trans2);
			string nombreCortoUsuario = dtUsuario.Rows[0]["NOMBRE_CORTO"].ToString();
			
			cabecera.Append("Usuario" + new String(' ', 2) + ": " + nombreCortoUsuario + "\r\n");
			cabecera.Append("Cantidad : " + cantidad.ToString() + "\r\n"); 

			sw.WriteLine(cabecera);

			String contrasena="";
            String contrasenaCifrada="";
			String claveEncriptada = prospectoMasivoDA.ObtenerClaveEncriptada(cn, trans);

			CryptoWS.Crypto wsCrypto = new CryptoWS.Crypto(); 

            int primerCUC = 0;
            int ultimoCUC = 0;

			try
			{
				for (int i = 0; i < cantidad; i++) 
				{
					do 
					{
						Random random = new Random(DateTime.Now.Millisecond);
						double a = random.NextDouble();
						
						if (a == 0 || a == 1){
							continue;
						}
						double b=a*100000000;
						long asd=(long)b;
						contrasena=""+asd;
					}
					while(contrasena.Length!=8);

                    contrasenaCifrada = wsCrypto.encrypt(contrasena,claveEncriptada);

					dtPrmGrabarDatosParticipe = prospectoMasivoDA.grabarDatosParticipe(nombreCortoUsuario, contrasenaCifrada, cn, trans);
					idParticipe = Convert.ToInt32(dtPrmGrabarDatosParticipe.Rows[0]["VALOR_PARAMETRO"].ToString());
					idParticipeAcceso = dtPrmGrabarDatosParticipe.Rows[1]["VALOR_PARAMETRO"].ToString();

					dtCodigoParticipe = prospectoMasivoDA.ObtenerCodigoParticipe(idParticipe, cn, trans);
					codigoParticipe = dtCodigoParticipe.Rows[0]["CODIGO"].ToString();

                    if (i == 0)
                    {
                        primerCUC = Convert.ToInt32(codigoParticipe);
                    }

                    ultimoCUC = Convert.ToInt32(codigoParticipe);

					prospectoMasivoDA.registrarCuentaParticipacionXParticipe( idParticipe.ToString(), codigoParticipe, nombreCortoUsuario, cn, trans);

					sw.WriteLine(codigoParticipe + "," + contrasena);
				}

                prospectoMasivoDA.InsertarLoteGeneracion(codidoLote, primerCUC, ultimoCUC, codigoUsuario, cn, trans);
				sw.Close();
				trans.Commit();
				trans2.Commit();
                return codidoLote;
			}
			catch(Exception ex)
			{
				trans.Rollback();
				trans2.Rollback();
				throw ex;
			}
			finally
			{
				trans.Dispose();
				trans2.Dispose();
				cn.Close();
				cn2.Close();
			}
		}
	}
}
