/*
 * Fecha de Modificación : 01/06/2016
 * Modificado por        : Juan Castro
 * Nro. Orden de Trabajo : 8844
 * Descripción del cambio: Creación.
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using SistemaProcesosDA;
using System.Data;

using SistemaProcesosTD.Constantes;

namespace SistemaProcesosBL
{
    public class AtribucionBL
    {
        string usuario;
        public AtribucionBL(string codUsuario)
        {
            usuario = codUsuario;
        }

        public string GenerarAtribucionPrecierre(int idFondo, DateTime fechaProceso, string usuario, bool blnProAlterno, string desFondo, string tipoAcceso)
        {
            AtribucionDA atribucionDA = new AtribucionDA();
            ValorCuotaDA valorCuotaDA = new ValorCuotaDA();
            ConsultaContabilidadDA consultaContabilidadDA = new ConsultaContabilidadDA();

            SqlConnection conTributacion = atribucionDA.GetConnection2();
            SqlTransaction transaccionTributacion = null;

            try
            {
                conTributacion.Open();
                transaccionTributacion = conTributacion.BeginTransaction(IsolationLevel.ReadUncommitted);

                DataTable dtValorCuota = valorCuotaDA.obtenerValorCuota(fechaProceso.ToString("yyyy-MM-dd"), idFondo);

                decimal valorCuota = Convert.ToDecimal(dtValorCuota.Rows[0]["VALOR_CUOTA"]);

                DataTable tipoCambio = consultaContabilidadDA.ObtenerTipoCambioContable(fechaProceso);
                if (tipoCambio.Rows.Count == 0)
                {
                    throw new Exception("No se ha registrado el tipo de cambio para la fecha " + fechaProceso.ToString("dd/MM/yyyy"));
                }
                else
                {
                    if (Convert.ToDecimal(tipoCambio.Rows[0]["VENSBS"]) == 1)
                        throw new Exception("El tipo de cambio para la fecha " + fechaProceso.ToString("dd/MM/yyyy") + " tiene valor 1.");
                }

                TablaGeneralDA mantenimientoDA = new TablaGeneralDA();
                DataTable dtTipoCalculo;
                dtTipoCalculo = mantenimientoDA.ListarTablaGeneral(ConstantesING.CODIGO_TABLA_TIPO_CALCULO);

                DataRow drTipoCalculo = dtTipoCalculo.Rows[0];
                string tipoCalculo = drTipoCalculo["DESCRIPCION_LARGA"].ToString();

                DataTable dtTipoAccesoCalculo;
                dtTipoAccesoCalculo = mantenimientoDA.ListarTablaGeneral(ConstantesING.CODIGO_TABLA_TIPO_ACCESO_FONDO);

                string tipoAcceso1;
                string tipoAccesoCalculo = "";

                foreach (DataRow dato in dtTipoAccesoCalculo.Rows)
                {
                    tipoAcceso1 = dato["LLAVE_TABLA"].ToString();
                    if (tipoAcceso1.Equals(tipoAcceso))
                    {
                        tipoAccesoCalculo = dato["DESCRIPCION_CORTA"].ToString();
                    }

                }

                /*Se comenta codigo temporalmente para no tener que implementar hasta que se implemente para fondos privados
                if (tipoCalculo == "PEPS" || tipoAccesoCalculo == "ANTIGUO")
                {
                    if (!blnProAlterno)
                    {
                        if (!ExistenDifereciasVC(fechaProceso, idFondo))
                        {
                            throw new Exception("Existen diferencias entre los Valores Cuota de Tributación y Operaciones para la fecha y fondo indicados.");
                        }
                    }
                    //////////////////////////////////////////////////////////////////////////////
                    Int32 iCantReg = procesoDA.RegistrosRentabilidad(idFondo.ToString(), fechaProceso, dbTributacion);
                    if (iCantReg <= 0)
                    {
                        if (blnProAlterno)
                        {
                            procesoDA.RegistrarRentabilidadAlterno(desFondo, fechaProceso, usuario, dbTributacion, transaccionTributacion);
                            procesoDA.ActualizarDatosArchivo(fechaProceso, desFondo, usuario, dbTributacion, transaccionTributacion);
                        }
                        else
                        {
                            throw new Exception("No se ha cargado el archivo de Rentabilidad MIDAS del día " + fechaProceso.ToString("dd/MM/yyyy") + " para este Fondo.");
                        }
                    }
                }
                */

                atribucionDA.GenerarTributacionPrecierre(idFondo, fechaProceso, usuario, valorCuota, tipoAcceso, conTributacion, transaccionTributacion);
                transaccionTributacion.Commit();

                return tipoAccesoCalculo;

            }
            catch (Exception ex)
            {
                transaccionTributacion.Rollback();
                throw ex;
            }
            finally
            {
                transaccionTributacion.Dispose();
                conTributacion.Close();
                conTributacion.Dispose();
            }
        }

        public void RevertirTributacion(int idFondo, DateTime fechaProceso, string usuario, string tipoProceso)
        {
            AtribucionDA atribucionDA = new AtribucionDA();
            SqlConnection conTributacion = atribucionDA.GetConnection2();
            SqlTransaction transaccionTributacion = null;
            try
            {
                conTributacion.Open();
                transaccionTributacion = conTributacion.BeginTransaction(IsolationLevel.ReadUncommitted);
                atribucionDA.RevertirTributacion(idFondo, fechaProceso, usuario, tipoProceso, conTributacion, transaccionTributacion);
                transaccionTributacion.Commit();
            }
            catch (Exception ex)
            {
                transaccionTributacion.Rollback();
                throw ex;
            }
            finally
            {
                transaccionTributacion.Dispose();
                conTributacion.Close();
                conTributacion.Dispose();
            }
        }

        public DataTable ObtenerTipoCambioContable(DateTime fechaProceso)
        {
            ConsultaContabilidadDA consultaContabilidadDA = new ConsultaContabilidadDA();
            return consultaContabilidadDA.ObtenerTipoCambioContable(fechaProceso);
        }
    }
}
