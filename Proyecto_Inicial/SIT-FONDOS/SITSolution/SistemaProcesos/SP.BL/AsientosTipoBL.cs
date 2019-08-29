/*
 * Fecha de Modificación: 17/07/2015
 * Modificado por: Juan Castro
 * Numero de OT: 6986
 * Descripción del cambio: Creación
 * */

using System;
using System.Data;
using Procesos.TD;
using Procesos.DA;
using System.Data.SqlClient;

namespace Procesos.BL
{
    public class AsientosTipoBO
    {
        public DataTable ConsultarAsientos()
        {
            AsientosTipoDA da = new AsientosTipoDA();
            return da.ListarCabeceraAsientos();
        }

        public DataSet ListarAsientosTipo(Int64 CodigoAT)
        {
            AsientosTipoDA da = new AsientosTipoDA();
            DataSet ds = new DataSet();

            ds.Tables.Add(da.ObtenerCabeceraAsiento(CodigoAT));
            ds.Tables.Add(da.ObtenerDetalleAsiento(CodigoAT));

            return ds;
        }

        public DataTable ListarParametro(string CodTabla)
        {
            AsientosTipoDA da = new AsientosTipoDA();
            return da.ListarParametro(CodTabla);
        }

        public DataTable EliminarAsientosTipo(int Codigo, string usuario)
        {
            AsientosTipoDA da = new AsientosTipoDA();
            return da.EliminarAsiento(Codigo, usuario);
        }

        public DataSet ProcesarAsiento(DataSet dsAsiento, int pOpc)
        {
            DataTable dtCab = new DataTable();
            DataTable dtDet = new DataTable();
            DataSet ds = new DataSet();
            AsientosTipoDA asientosTipoDA = new AsientosTipoDA();
            Int64 intCodigo = 0;

            SqlConnection con = asientosTipoDA.GetConnection2();
            con.Open();
            SqlTransaction trans = con.BeginTransaction();
            try
            {
                
                switch (pOpc)
                {
                    case 1:

                        dtCab = asientosTipoDA.RegistrarAsientoCabecera(dsAsiento.Tables[0], con, trans);
                        intCodigo = Convert.ToInt64(dtCab.Rows[0][0].ToString());
                        dtDet = asientosTipoDA.RegistrarAsientoDetalle(dsAsiento.Tables[1], intCodigo, con, trans);
                        break;
                    case 2:
                        dtCab = asientosTipoDA.ActualizarAsientoCabecera(dsAsiento.Tables[0], intCodigo, con, trans);
                        intCodigo = Convert.ToInt64(dtCab.Rows[0][0].ToString());
                        //asientosTipoDA.EliminarAsientoDetalle(intCodigo);
                        dtDet = asientosTipoDA.RegistrarAsientoDetalle(dsAsiento.Tables[1], intCodigo, con, trans);
                        break;
                }

                ds.Tables.Add(dtCab);
                ds.Tables.Add(dtDet);
                trans.Commit();
                return ds;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

    }
}
