/*
 * Fecha de Modificación: 25/08/2016
 * Modificado por: Robert Castillo
 * Numero de OT: 8895
 * Descripción del cambio: Creación de clase que se encarga del acceso a datos de las retenciones
 *                         asociadas a pagos de flujos.
 * */
using System;
using System.Data;
using System.Data.SqlClient;

using INGFondos.Data;
using Procesos.TD;

namespace Procesos.DA
{
    public class CargarRetencionPagoFlujosDA : INGFondos.Data.DA
    {
        public CargarRetencionPagoFlujosDA() : base(INGFondos.Constants.Conexiones.ServidorTributacion, INGFondos.Constants.Conexiones.BaseDeDatosTributacion) { }

        public void registrarPagoFlujos(PagoFlujosTD.PagoFlujosRow pagoFlujos, SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_RETENCION_PAGO_FLUJOS", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 10000;

            SqlParameter prmIdOperacion = cmd.Parameters.Add("@ID_OPERACION", SqlDbType.Int);
            prmIdOperacion.Value = pagoFlujos.ID_OPERACION;

            SqlParameter prmIdParticipe = cmd.Parameters.Add("@ID_PARTICIPE", SqlDbType.Int);
            prmIdParticipe.Value = pagoFlujos.ID_PARTICIPE;

            SqlParameter prmMontoRetencion = cmd.Parameters.Add("@MONTO_RETENCION", SqlDbType.Decimal);
            prmMontoRetencion.Value = pagoFlujos.RETENCION;

            SqlParameter prmUsuarioCreacion = cmd.Parameters.Add("@USUARIO_CREACION", SqlDbType.VarChar);
            prmUsuarioCreacion.Value = pagoFlujos.USUARIO_CREACION;

            cmd.Transaction = trans;
            
            try
            {

                cmd.ExecuteNonQuery();
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
