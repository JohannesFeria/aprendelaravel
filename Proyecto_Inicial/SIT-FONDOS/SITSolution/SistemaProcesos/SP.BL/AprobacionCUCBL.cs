/*
 * Fecha de Modificación: 06/05/2015
 * Modificado por: Walter Rodríguez
 * Numero de OT: 7235
 * Descripción del cambio: Creación de la clase.
 * */
/*
 * Fecha de Modificación: 06/11/2015
 * Modificado por: Robert Castillo
 * Numero de OT: 7940
 * Descripción del cambio: Se realizan las siguientes modificaciones:
                           Se agrega método ObtenerTablaGeneral que se encarga de llamar al método 
 *                         del mismo nombre de la clase AprobacionCUCDA.cs.
 * */
using System;
using System.Collections.Generic;
using System.Text;
using Procesos.DA;
using System.Data.SqlClient;
using System.Data;

namespace Procesos.BL
{
    public class AprobacionCUCBL : INGFondos.Data.DA
    {
        private string codigoUsuario;

        public AprobacionCUCBL(string codigoUsuario)
            : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones)
		{
			this.codigoUsuario = codigoUsuario;
		}

        public System.Data.DataTable ListarParticipesPendientes()
        {
            AprobacionCUCDA da = new AprobacionCUCDA();
            return da.ListarParticipesPendientes();
        }

        public void registrarAprobacionCuc(System.Data.DataTable dtParticipes)
        {
            AprobacionCUCDA da = new AprobacionCUCDA();
            SqlConnection cn = GetConnection();
            cn.Open();
            SqlTransaction trans = cn.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                foreach (DataRow dr in dtParticipes.Rows)
                {
                    if (dr["ESTADO"] == "MOD")
                    {
                        da.ActualizarAprobacionParticipe(dr, codigoUsuario, cn, trans);
                    }
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                cn.Close();
                cn.Dispose();
            }
        }

        public DataTable ListarUsuariosOperaciones()
        {
            AsignacionCUCDA da = new AsignacionCUCDA();
            return da.ListarTablaGeneral(Constants.ConstantesING.CODIGO_USUARIOS_OPERACIONES);
        }

        public DataTable ListarUsuariosOFC()
        {
            AsignacionCUCDA da = new AsignacionCUCDA();
            return da.ListarTablaGeneral(Constants.ConstantesING.CODIGO_USUARIOS_OFC);
        }

        public DataTable ObtenerDatosBienvenida(string cuc)
        {
            AprobacionCUCDA da = new AprobacionCUCDA();

            DataTable dt = da.ObtenerDatosBienvenida(cuc);
            string contrasena = dt.Rows[0]["CONTRASENA"].ToString().Trim();
            String claveEncriptada = da.ObtenerClaveEncriptada();
            CryptoWS.Crypto wsCrypto = new CryptoWS.Crypto();

            contrasena = wsCrypto.decrypt(contrasena, claveEncriptada);
            dt.Rows[0]["CONTRASENA"] = contrasena;
            return dt;
        }

        public DataTable ObtenerTablaGeneral(string codigoTabla, string llaveTabla)
        {
            AprobacionCUCDA da = new AprobacionCUCDA();
            //return da.ListarTablaGeneral(Constants.ConstantesING.CODIGO_USUARIOS_OFC);
            return da.ObtenerTablaGeneral(codigoTabla, llaveTabla);
        }
    }
}
