/*
 * Fecha de Creación    : 30/05/2016
 * Modificado por           : Irene Reyes
 * Numero de OT             : 8829
 * Descripción del cambio   : Creación
 *                            
 * */
using System;
using System.Collections.Generic;
using System.Text;
using Procesos.DA;
using System.Data.SqlClient;
using System.Data;


namespace Procesos.BL
{
    public class EECCIndividualBL : INGFondos.Data.DA
    {

        public EECCIndividualBL() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones){ }

        public DataTable ObtenerFondos()
        {
            EECCIndividualDA da = new EECCIndividualDA();
            return da.ObtenerFondos();
        }

        public decimal ObtenerValorCuota(decimal idFondo, DateTime fecha)
        {
            EECCIndividualDA da = new EECCIndividualDA();
            return da.ObtenerValorCuota(idFondo, fecha);
        }
    }
}
