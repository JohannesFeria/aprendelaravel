/*
 * Fecha de Modificación: 16/12/2014
 * Modificado por: Giovana Veliz
 * Numero de OT: 7014
 * Descripción del cambio: Creación
 * */

using System;
using System.Data;
using System.Data.SqlClient;

using INGFondos.Data;
using Procesos.TD;

namespace Procesos.DA
{
	public class SegmentacionRegionalDA : INGFondos.Data.DA
	{
        public SegmentacionRegionalDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) { }

		public void ActualizarParticipeSegmentacion(ParticipeSegmentacionTD.PARTICIPE_SEGMENTACION_REGIONALRow participeSegmentacion, SqlConnection cn, SqlTransaction trans)
		{
            SqlCommand cmd = new SqlCommand("dbo.FMPR_ACT_PARTICIPE_SEGMENTACION_REGIONAL", cn);
			cmd.CommandType = CommandType.StoredProcedure;

			SqlParameter prmNroDocumento = cmd.Parameters.Add("@nroDocumento", SqlDbType.VarChar);
            prmNroDocumento.Value = participeSegmentacion.NUMERO_DOCUMENTO;

			SqlParameter prmCodigoSegmentacionRegional = cmd.Parameters.Add("@codigoSegmentacionRegional", SqlDbType.VarChar);
            prmCodigoSegmentacionRegional.Value =participeSegmentacion.SEGMENTACION_REGIONAL;

			SqlParameter prmUsuarioModificacion = cmd.Parameters.Add("@usuarioModificacion", SqlDbType.VarChar);
            prmUsuarioModificacion.Value = participeSegmentacion.USUARIO;														
	
			cmd.Transaction = trans;
			try
			{
				cmd.ExecuteNonQuery();
			}
			catch(Exception e)
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
