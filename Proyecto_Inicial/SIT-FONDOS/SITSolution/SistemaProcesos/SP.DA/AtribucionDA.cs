/*
 * Fecha de Modificación : 17/06/2016
 * Modificado por        : Juan Castro
 * Nro. Orden de trabajo : 8844
 * Descripción del cambio: Creación.
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

using SistemaProcesosTD.Constantes;

namespace SistemaProcesosDA
{
    public class AtribucionDA : INGFondos.Data.DA // : ConSQL
    {
        public AtribucionDA() : base(INGFondos.Constants.Conexiones.ServidorTributacion, INGFondos.Constants.Conexiones.BaseDeDatosTributacion) { }

        public SqlConnection GetConnection2()
        {
            return base.GetConnection();
        }

        public void RevertirTributacion(int idFondo, DateTime fechaProceso, string usuario, string tipoProceso, SqlConnection con, SqlTransaction trans)
        {
            try
            {
                SqlCommand comm = new SqlCommand("dbo.FMTR_EJE_REVERSION_ATRIBUCION", con, trans);
                comm.CommandType = CommandType.StoredProcedure;

                comm.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fechaProceso;
                comm.Parameters.Add("@idFondo", SqlDbType.Decimal).Value = idFondo;
                comm.Parameters.Add("@usuario", SqlDbType.VarChar).Value = usuario;
                comm.Parameters.Add("@tipoProceso", SqlDbType.VarChar).Value = tipoProceso;

                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GenerarTributacionPrecierre(int idFondo, DateTime fechaProceso, string usuario, decimal valorCuota, string tipoAcceso, SqlConnection con, SqlTransaction trans)
        {
            try
            {
                SqlCommand comm = new SqlCommand("dbo.FMTR_CAL_ATRIBUCION_PRECIERRE", con, trans);
                comm.CommandType = CommandType.StoredProcedure;

                comm.Parameters.Add("@idFondo", SqlDbType.Decimal).Value = idFondo;
                comm.Parameters.Add("@fechaProceso", SqlDbType.DateTime).Value = fechaProceso;
                comm.Parameters.Add("@valorCuota", SqlDbType.Decimal).Value = valorCuota;
                comm.Parameters.Add("@baseDatosOperaciones", SqlDbType.VarChar).Value = ConstantesING.BASE_DATOS_OPERACIONES;
                comm.Parameters.Add("@usuario", SqlDbType.VarChar).Value = ConstantesING.BASE_DATOS_OPERACIONES;
                comm.Parameters.Add("@tipoAcceso", SqlDbType.VarChar).Value = tipoAcceso;

                comm.CommandTimeout = 4000;
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
