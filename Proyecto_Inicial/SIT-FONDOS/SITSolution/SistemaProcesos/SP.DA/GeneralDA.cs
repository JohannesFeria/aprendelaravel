/*
 * Fecha de Modificación: 17/07/2015
 * Modificado por:        Juan Castro
 * Numero de OT:          6986
 * Descripción del cambio: Se crea la clase GeneralDA
 */
using System;
using System.Collections.Generic;
using System.Text;

using INGFondos.Data;
using System.Data;
using System.Data.SqlClient;

namespace Procesos.DA
{
    public class GeneralDA : INGFondos.Data.DA
    {
        public GeneralDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) { }

        public DataTable ListarTabla(string codigoTabla, SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_TABLA_GENERAL", cn);
            cmd.Transaction = trans;
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmCodigo = cmd.Parameters.Add("@codigoTabla", SqlDbType.VarChar);
                prmCodigo.Value = codigoTabla;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("TABLA");
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public DataTable ObtenerValorTabla(string codigoTabla, string llaveTabla, SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_TABLA_GENERAL", cn);
            cmd.Transaction = trans;
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmCodigo = cmd.Parameters.Add("@codigoTabla", SqlDbType.VarChar);
                prmCodigo.Value = codigoTabla;

                SqlParameter prmLlave = cmd.Parameters.Add("@llaveTabla", SqlDbType.VarChar);
                prmLlave.Value = llaveTabla;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("TABLA");
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Dispose();
            }
        }
    }
}
