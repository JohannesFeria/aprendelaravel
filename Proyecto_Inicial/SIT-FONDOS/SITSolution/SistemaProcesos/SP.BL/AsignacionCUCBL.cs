/*
 * Fecha de Modificación: 06/05/2015
 * Modificado por: Walter Rodríguez
 * Numero de OT: 7235
 * Descripción del cambio: Creación de la clase.
 * */
using System;
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
using System.Data;
using Procesos.Constants;
using System.Windows.Forms;

namespace Procesos.BL
{
    public class AsignacionCUCBL : INGFondos.Data.DA
    {
        private string codigoUsuario;

        public AsignacionCUCBL(string codigoUsuario) : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones)
		{
			this.codigoUsuario = codigoUsuario;
		}

        public DataTable ListarProspectosPorLote(string codigoLote)
        {
            AsignacionCUCDA da = new AsignacionCUCDA();
            return da.ListarProspectosPorLote(codigoLote);
        }

        public DataTable ListarLotes()
        {
            AsignacionCUCDA da = new AsignacionCUCDA();
            return da.ListarLotes();
        }

        public DataTable ListarAgencias()
        {
            AsignacionCUCDA da = new AsignacionCUCDA();
            return da.ListarTablaGeneral(ConstantesING.CODIGO_TABLA_AGENCIA_ORIGEN);
        }

        public DataTable ListarPromotores()
        {
            AsignacionCUCDA da = new AsignacionCUCDA();
            return da.ListarPromotores();
        }
        
        public DataTable ListarPromotoresPorAgencia(string codigoAgencia)
        {
            AsignacionCUCDA da = new AsignacionCUCDA();
            return da.ListarPromotoresPorAgencia(codigoAgencia);
        }

        public string GrabarAsignacionMasiva(System.Windows.Forms.ListView.CheckedListViewItemCollection listaAgenciasSeleccionadas, int cucInicio, int cucFin)
        {
            AsignacionCUCDA da = new AsignacionCUCDA();
            SqlConnection cn = GetConnection();
			cn.Open();
			SqlTransaction trans = cn.BeginTransaction(IsolationLevel.ReadUncommitted);
            
            try
            {
                da.EliminarAgenciasOrigenTemp(cn, trans);
                foreach (ListViewItem item in listaAgenciasSeleccionadas)
                {                    
                    da.InsertarAgenciaOrigenTemp(item.Tag.ToString(), cn, trans);
                }

                da.GrabarAsignacionMasiva(cucInicio, cucFin, codigoUsuario, cn, trans);
                trans.Commit();
                return "";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                return "No se pudo realizar la asignación masiva.\r\nDetalle: " + ex.Message + "\r\n" + ex.StackTrace + "\r\n";
            }
            finally
            {
                cn.Close();
                cn.Dispose();
            }
        }

        public string LimpiarAsignacionMasiva(ListView.CheckedListViewItemCollection listaAgenciasSeleccionadas, int cucInicio, int cucFin)
        {
            AsignacionCUCDA da = new AsignacionCUCDA();
            SqlConnection cn = GetConnection();
            cn.Open();
            SqlTransaction trans = cn.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                /* da.EliminarAgenciasOrigenTemp(cn, trans);
                foreach (ListViewItem item in listaAgenciasSeleccionadas)
                {
                    da.InsertarAgenciaOrigenTemp(item.Tag.ToString(), cn, trans);
                }
                */
                da.LimpiarAsignacionMasiva(cucInicio, cucFin, codigoUsuario, cn, trans);
                trans.Commit();
                return "";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                return "No se pudo limpiar la asignación masiva.\r\nDetalle: " + ex.Message + "\r\n" + ex.StackTrace + "\r\n";
            }
            finally
            {
                cn.Close();
                cn.Dispose();
            }
        }

        public string GrabarAsignacionAsesor(string codigoAsesor, int cucInicio, int cucFin)
        {
            AsignacionCUCDA da = new AsignacionCUCDA();
            SqlConnection cn = GetConnection();
            cn.Open();
            SqlTransaction trans = cn.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                da.GrabarAsignacionAsesor(codigoAsesor, cucInicio, cucFin, codigoUsuario, cn, trans);
                trans.Commit();
                return "";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                return "No se pudo realizar la asignación masiva.\r\nDetalle: " + ex.Message + "\r\n" + ex.StackTrace + "\r\n";
            }
            finally
            {
                cn.Close();
                cn.Dispose();
            }
        }

        public string LimpiarAsignacionAsesor(string codigoAsesor, int cucInicio, int cucFin)
        {
            AsignacionCUCDA da = new AsignacionCUCDA();
            SqlConnection cn = GetConnection();
            cn.Open();
            SqlTransaction trans = cn.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                da.LimpiarAsignacionAsesor(codigoAsesor, cucInicio, cucFin, codigoUsuario, cn, trans);
                trans.Commit();
                return "";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                return "No se pudo limpiar la asignación masiva.\r\nDetalle: " + ex.Message + "\r\n" + ex.StackTrace + "\r\n";
            }
            finally
            {
                cn.Close();
                cn.Dispose();
            }
        }

        public bool ValidarRangoCucEnLote(string codigoLote, int cucInicio, int cucFin)
        {
            AsignacionCUCDA da = new AsignacionCUCDA();
            return da.ValidarRangoCucEnLote(codigoLote, cucInicio, cucFin);
        }
    }
}
