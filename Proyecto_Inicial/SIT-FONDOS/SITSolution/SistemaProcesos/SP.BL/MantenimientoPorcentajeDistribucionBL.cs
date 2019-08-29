/*
 * Fecha de Modificación : 12/06/2017
 * Modificado por        : Anthony Joaquin
 * Nro. Orden de Trabajo : 10478
 * Descripción del cambio: Creación.
 * */

using System;
using System.Data;
using Procesos.TD;
using Procesos.DA;
using System.Data.SqlClient;
using INGFondos.Constants;
using INGFondos.Data;
using System.Configuration;


namespace Procesos.BL
{
    public class MantenimientoPorcentajeDistribucionBO
    {
        public MantenimientoPorcentajeDistribucionBO()
		{
			//
			// TODO: agregar aquí la lógica del constructor
			//
		}


        public DataTable ListarFondo()
        {
            MantenimientoPorcentajeDistribucionDA da = new MantenimientoPorcentajeDistribucionDA();
            return da.ListarFondo();
        }


        public DataTable ListaPorcentajeXFondo(int idFondo)
        {
            MantenimientoPorcentajeDistribucionDA da = new MantenimientoPorcentajeDistribucionDA();
            return da.ListaPorcentajeXFondo(idFondo);
        }


        public DataTable ListarMes(string codTabla)
        {
            MantenimientoPorcentajeDistribucionDA da = new MantenimientoPorcentajeDistribucionDA();
            return da.ListarMes(codTabla);
        }


        public void ActualizarDistribucion(DataTable dtDistribucionXPeriodo, string codigoUsuario)
        {
            MantenimientoPorcentajeDistribucionDA da = new MantenimientoPorcentajeDistribucionDA();
            da.Database = INGFondos.Constants.Conexiones.BaseDeDatosOperaciones;
            da.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;

            SqlConnection cn = da.GetConnection2();

            cn.Open();
            SqlTransaction trans = cn.BeginTransaction();

            DistribucionXPeriodo dxp = new DistribucionXPeriodo();

            try
            {               
                foreach (DataRow drDistribucionXPeriodo in dtDistribucionXPeriodo.Rows)
                {
                    dxp.ID = Convert.ToInt32(drDistribucionXPeriodo["ID"].ToString().Trim());
                    dxp.Periodo = Convert.ToInt32(drDistribucionXPeriodo["PERIODO"].ToString().Trim());
                    dxp.Porcentaje = Convert.ToDecimal(drDistribucionXPeriodo["PORCENTAJE"].ToString().Trim());
                    dxp.FechaCorte = drDistribucionXPeriodo["FECHA_CORTE"].ToString().Trim();
                }

                da.ActualizarDistribucion(dxp, codigoUsuario, cn, trans);
                trans.Commit();
                
            }
            catch (Exception x1)
            {
                trans.Rollback();
                throw x1;
            }
            finally
            {
                trans.Dispose();
                cn.Close();
               
            }

        }

        public void RegistarDistribucion(DataTable dtDistribucionXPeriodo, string codigoUsuario)
        {
            MantenimientoPorcentajeDistribucionDA da = new MantenimientoPorcentajeDistribucionDA();
            da.Database = INGFondos.Constants.Conexiones.BaseDeDatosOperaciones;
            da.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;

            SqlConnection cn = da.GetConnection2();

            cn.Open();
            SqlTransaction trans = cn.BeginTransaction();

            DistribucionXPeriodo dxp = new DistribucionXPeriodo();

            try
            {
                foreach (DataRow drDistribucionXPeriodo in dtDistribucionXPeriodo.Rows)
                {
                    dxp.IdFondo = Convert.ToInt32(drDistribucionXPeriodo["ID_FONDO"].ToString().Trim());
                    dxp.Periodo = Convert.ToInt32(drDistribucionXPeriodo["PERIODO"].ToString().Trim());
                    dxp.Porcentaje = Convert.ToDecimal(drDistribucionXPeriodo["PORCENTAJE"].ToString().Trim());
                    dxp.FechaCorte = drDistribucionXPeriodo["FECHA_CORTE"].ToString().Trim();
                }

                da.RegistarDistribucion(dxp, codigoUsuario, cn, trans);

                trans.Commit();
               
            }
            catch (Exception x2)
            {
                trans.Rollback();
                throw x2;
            }
            finally
            {
                trans.Dispose();
                cn.Close();
                
            }

        }


        public void EliminarDistribucion(int idDistribucionxPeriodo, string codigoUsuario)
        {
            MantenimientoPorcentajeDistribucionDA da = new MantenimientoPorcentajeDistribucionDA();
            da.Database = INGFondos.Constants.Conexiones.BaseDeDatosOperaciones;
            da.Server = INGFondos.Constants.Conexiones.ServidorOperaciones;

            SqlConnection cn = da.GetConnection2();

            cn.Open();
            SqlTransaction trans = cn.BeginTransaction();

            DistribucionXPeriodo dxp = new DistribucionXPeriodo();

            try
            {

                da.EliminarDistribucion(idDistribucionxPeriodo, codigoUsuario, cn, trans);
                trans.Commit();
               
            }
            catch (Exception x3)
            {
                trans.Rollback();
                throw x3;
            }
            finally
            {
                trans.Dispose();
                cn.Close();
                
            }

        }

    }
}
