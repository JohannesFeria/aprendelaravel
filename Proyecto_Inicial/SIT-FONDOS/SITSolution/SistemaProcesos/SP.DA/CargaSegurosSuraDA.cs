/*
 * Fecha de Modificación: 18/09/2014
 * Modificado por: Leslie Valerio
 * Numero de OT: OT 6891
 * Descripción del cambio: Creación
 * */
using System;
using System.Data;
using System.Data.SqlClient;

using INGFondos.Data;
using Procesos.TD;

namespace Procesos.DA
{
    
        public class CargaSegurosSuraDA : INGFondos.Data.DA
        {
            public CargaSegurosSuraDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) { }

            public DataTable ObtenerDataDiferenciaVidaAhorro()
            {
                SqlConnection cn = GetConnection();

                SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_CONCILIACION_SEGUROS_CONVENIO", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 10000;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("LISTA_VIDA AHORRO");
                da.Fill(dt);
                return dt;
            }


        
            public DataSet ObtenerPaginado(int scrollVal)
            {
                
                SqlConnection cn = GetConnection();

                SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_CONCILIACION_SEGUROS_CONVENIO", cn);
                cmd.CommandType = CommandType.StoredProcedure;


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                
                DataSet pagingDS = new DataSet();

                da.Fill(pagingDS, scrollVal, 5, "Vida");
              
                return pagingDS;
            }



            public void CargaRegistroConvenio(ConvenioTD.ConvenioRow convenio,SqlConnection cn, SqlTransaction trans)
            {
                SqlCommand cmd = new SqlCommand("Dbo.FMPR_INS_CONCILIACION_CONVENIO", cn);
                cmd.Transaction = trans;
                cmd.CommandTimeout = 4000000;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter prmCUC = cmd.Parameters.Add("@CUC", SqlDbType.VarChar);
                prmCUC.Value = convenio.CUC;

                SqlParameter prmMATRICULA = cmd.Parameters.Add("@POLIZA", SqlDbType.VarChar);
                prmMATRICULA.Value = convenio.MATRICULA;

                SqlParameter prmNOM_EMPLEAD = cmd.Parameters.Add("@NOM_EMPLEADO", SqlDbType.VarChar);
                prmNOM_EMPLEAD.Value = DBNull.Value;

                SqlParameter prmNOMBRES = cmd.Parameters.Add("@NOMBRES", SqlDbType.VarChar);
                prmNOMBRES.Value = convenio.NOMBRES;

                SqlParameter prmAPELLIDOS = cmd.Parameters.Add("@APELLIDOS", SqlDbType.VarChar);
                prmAPELLIDOS.Value = convenio.APELLIDOS;

                SqlParameter prmMONTO = cmd.Parameters.Add("@MONTO", SqlDbType.Decimal);
                prmMONTO.Value = convenio.MONTO;

                SqlParameter prmFONDO = cmd.Parameters.Add("@FONDO", SqlDbType.VarChar);
                prmFONDO.Value = convenio.FONDO;

                SqlParameter prmESTADO = cmd.Parameters.Add("@ESTADO", SqlDbType.VarChar);
                prmESTADO.Value = convenio.ESTADO;

                SqlParameter prmPARTICIPE = cmd.Parameters.Add("@PARTICIPE", SqlDbType.Decimal);
                prmPARTICIPE.Value = convenio.PARTICIPANTE;

                SqlParameter prmEMPLEADOR = cmd.Parameters.Add("@EMPLEADOR", SqlDbType.Decimal);
                prmEMPLEADOR.Value = convenio.EMPLEADOR;

                SqlParameter prmANTIGUEDAD = cmd.Parameters.Add("@ANTIGUEDAD", SqlDbType.Decimal);
                prmANTIGUEDAD.Value = convenio.ANTIGUEDAD;

                SqlParameter prmSALARIO_PROMEDIO = cmd.Parameters.Add("@SALARIO_PROMEDIO", SqlDbType.Decimal);
                prmSALARIO_PROMEDIO.Value = convenio.SALARIO_PROM;


                SqlParameter prmEDAD = cmd.Parameters.Add("@EDAD", SqlDbType.Decimal);
                prmEDAD.Value = convenio.EDAD;

                SqlParameter prmBONO = cmd.Parameters.Add("@BONO", SqlDbType.Decimal);
                prmBONO.Value = convenio.BONO_ANUAL;


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

            public void TruncarTabla_Convenio(SqlConnection cn, SqlTransaction trans)
            {
                SqlCommand cmd = new SqlCommand("dbo.FMPR_ELI_CONVENIO_SEGUROS", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 10000;
               

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

            public void insertarRegistroConci_vidaAhorro_Convenio(SqlConnection cn, SqlTransaction trans)
            {
                SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_CONCILIACION_CONVENIO_FONDOS", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 100000;

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

