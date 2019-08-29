/*
 * Fecha de Modificación: 27/11/2014
 * Modificado por: Alejandro Quiñones Rojas
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
    public class ParametrosBO 
    {
        private string codigoUsuario;

        public ParametrosBO(string codigoUsuario) 
		{
			this.codigoUsuario = codigoUsuario;
		}

        public ParametrosBO()
        {
        }

        public DataTable ProcesarParametro(DataTable Parametro, int tpoProceso)
        {
            try
            {
                DataTable dt = new DataTable("PARAMETROSGI");

                ParametrosDA da = new ParametrosDA();
                switch (tpoProceso)
                {
                    case 1: dt = da.RegistrarParametro(Parametro);
                        break;
                    case 2: dt = da.ActualizarParametro(Parametro);
                        break;
                    case 3: dt = da.EliminarParametro(Parametro);
                        break;
                }

                return dt;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public DataTable ListaParametros(string CodTabla)
        {
            ParametrosDA da = new ParametrosDA();

            return (da.ObtenerParametros(CodTabla));

        }

        public DataTable ObtenerValoresParametros(string codigoTabla, string codigoUnico)
        {
            ParametrosDA da = new ParametrosDA();

            return (da.ObtenerValoresParametros(codigoTabla, codigoUnico));

        }

    }
}
