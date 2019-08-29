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
using SistemaProcesosTD.Constantes; // using Procesos.Constants;

namespace SistemaProcesosDA
{
    public class ConsultaContabilidadDA : INGFondos.Data.DA // : ConSQL
    {
        public ConsultaContabilidadDA() : base(Conexion.ServidorContabilidad, Conexion.BDContabilidad) { }

        public DataTable ObtenerTipoCambioContable(DateTime fechaProceso)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.sp_Trae_Tipos_Cambio", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmIdFondo = cmd.Parameters.Add("@cOrden", SqlDbType.VarChar);
            prmIdFondo.Value = "fecha";

            SqlParameter prmFechaDesde = cmd.Parameters.Add("@cFechaInicio", SqlDbType.VarChar);
            prmFechaDesde.Value = fechaProceso.ToString("dd/MM/yyyy");

            SqlParameter prmFechaHasta = cmd.Parameters.Add("@cFechaFin", SqlDbType.VarChar);
            prmFechaHasta.Value = fechaProceso.ToString("dd/MM/yyyy");

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("TIPO_CAMBIO");
            da.Fill(dt);
            return dt;
        }
    }
}
