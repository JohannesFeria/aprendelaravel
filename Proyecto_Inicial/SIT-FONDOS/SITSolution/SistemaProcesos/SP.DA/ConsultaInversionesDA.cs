/*
 * Fecha de Modificación : 02/06/2016
 * Modificado por        : Juan Castro
 * Nro. Orden de Trabajo : 8844
 * Descripción del cambio: Creación.
 * */
/*
 * Fecha de Modificación : 19/06/2017
 * Modificado por        : Anthony Joaquin
 * Nro. Orden de Trabajo : 10478
 * Descripción del cambio: Se agrega el método listarOrdenesPreOrdenesInversion que hace referencia a la base de datos SIT.
 * */
/*
 * Fecha de Modificación : 12/09/2017
 * Modificado por        : Rosmery Contreras
 * Nro. Orden de Trabajo : 10571
 * Descripción del cambio: Se agrega los métodos DevolucionComisiones_Seleccionar y ListarComisionUnificada.
 * */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using INGFondos.Data;
using SistemaProcesosTD.Constantes; // using Procesos.Constants;
using System.IO;//OT10571

namespace SistemaProcesosDA
{
    public class ConsultaInversionesDA : INGFondos.Data.DA // : ConSQL
    {
        public ConsultaInversionesDA() : base(Conexion.ServidorInversiones, Conexion.BDInversiones) { }

		//OT10571 INI
		public SqlConnection GetConnection2()
		{
			return GetConnection();
		}
		//OT10571 FIN

        public DataTable ObtenerValorCuota(DateTime fecha, string portafolio, string serie)
        {
            try
            {
                SqlConnection cn = GetConnection();
                SqlCommand cmd = new SqlCommand("dbo.sp_SIT_sel_ValorCuota", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter prmPortafolio = cmd.Parameters.Add("@CodigoPortafolioSBS", SqlDbType.VarChar);
                prmPortafolio.Value = portafolio;

                SqlParameter prmSerie = cmd.Parameters.Add("@CodigoSerie", SqlDbType.VarChar);
                prmSerie.Value = serie;

                SqlParameter prmFecha = cmd.Parameters.Add("@FechaProceso", SqlDbType.Int);
                //Las fechas en sit se graban en dato numerico y con el formato indicado
                prmFecha.Value = Convert.ToInt32(fecha.ToString("yyyyMMdd"));

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("VALOR_CUOTA");
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable ListarPortafolios()
        {
            try
            {
                SqlConnection cn = GetConnection();
                SqlCommand cmd = new SqlCommand("dbo.Portafolio_Listar", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                //TIENE UN PARAMETRO MAS PERO NO SE SABE DE QUE ES Y NO ES OBLIGATORIO
                //ESTE PARAMETRO TAMPOCO ES OBLIGATORIO
                SqlParameter prmCodigoNegocio = cmd.Parameters.Add("@p_CodigoNegocio", SqlDbType.VarChar);
                prmCodigoNegocio.Value = "";

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("PORTAFOLIOS");
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        ////OT10478 INI
        //public DataTable listarOrdenesPreOrdenesInversion(int fechaInicio, int fechaFin, int idFondoSit)
        //{

        //    SqlConnection cn = GetConnection();
        //    SqlCommand cmd = new SqlCommand("dbo.OrdenInversion_consultarOrdenesPreordenes", cn);
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    try
        //    {
        //        SqlParameter prmFechaInicio = cmd.Parameters.Add("@p_FechaInicio", SqlDbType.Int);
        //        prmFechaInicio.Value = fechaInicio;

        //        SqlParameter prmfechafin = cmd.Parameters.Add("@p_fechafin", SqlDbType.Int);
        //        prmfechafin.Value = fechaFin;

        //        SqlParameter prmCodigoOperacion = cmd.Parameters.Add("@p_CodigoOperacion", SqlDbType.VarChar);
        //        prmCodigoOperacion.Value = null;

        //        SqlParameter prmCodigoTabla = cmd.Parameters.Add("@p_CodigotipoInstrumentoSBS", SqlDbType.VarChar);
        //        prmCodigoTabla.Value = null;

        //        SqlParameter prmCodigoMnemonico = cmd.Parameters.Add("@p_CodigoMnemonico", SqlDbType.VarChar);
        //        prmCodigoMnemonico.Value = null;

        //        SqlParameter prmCodigoISIN = cmd.Parameters.Add("@p_CodigoISIN", SqlDbType.VarChar);
        //        prmCodigoISIN.Value = null;

        //        SqlParameter prmCodigoSBS = cmd.Parameters.Add("@p_CodigoSBS", SqlDbType.VarChar);
        //        prmCodigoSBS.Value = null;

        //        SqlParameter prmCodigotipoRenta = cmd.Parameters.Add("@p_codigotipoRenta", SqlDbType.VarChar);
        //        prmCodigotipoRenta.Value = null;

        //        SqlParameter prmPortafolio = cmd.Parameters.Add("@p_Portafolio", SqlDbType.VarChar);
        //        prmPortafolio.Value = idFondoSit;

        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        DataTable dt = new DataTable("LISTA_ORDENES_PREORDENES_SIT");
        //        da.Fill(dt);
        //        return dt;
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        cmd.Dispose();
        //    }

        //}
        ////OT10478 INI


        ////OT10571 INI
        //public DataTable DevolucionComisiones_Seleccionar(string codigoPortafolio, DateTime fecha)
        //{
        //    try
        //    {
        //        SqlConnection cn = GetConnection();
        //        SqlCommand cmd = new SqlCommand("dbo.DevolucionComisiones_Seleccionar", cn);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        SqlParameter prmCodigoPortafolio = cmd.Parameters.Add("@p_CodigoPortafolioSBS", SqlDbType.VarChar);
        //        prmCodigoPortafolio.Value = codigoPortafolio;

        //        SqlParameter prmFechaOperacion = cmd.Parameters.Add("@p_FechaOperacion", SqlDbType.Int);
        //        prmFechaOperacion.Value = fecha.Year * 10000 + fecha.Month * 100 + fecha.Day;

        //        SqlParameter prmSituacion = cmd.Parameters.Add("@p_Situacion", SqlDbType.VarChar);
        //        prmSituacion.Value = "A";

        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        DataTable dt = new DataTable("DEVOLUCION");
        //        da.Fill(dt);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public DataTable ListarComisionUnificada(DateTime fechaCalculo)
        //{
        //    this.Server = Conexion.ServidorInversiones;
        //    this.Database = Conexion.BDInversiones;

        //    SqlConnection cn = GetConnection();
        //    SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_COMISION_UNIFICADA_x_FECHA", cn);
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    try
        //    {
        //        SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.Int);
        //        prmFecha.Value = fechaCalculo.Year * 10000 + fechaCalculo.Month * 100 + fechaCalculo.Day;

        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        DataTable dt = new DataTable("COMISION");
        //        da.Fill(dt);
        //        return dt;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    finally
        //    {
        //        cmd.Dispose();
        //    }
        //}
        ////OT10571 FIN
    }
}
