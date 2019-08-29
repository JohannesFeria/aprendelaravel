#region Descripción
/*
 * Fecha de Modificación: 26/02/2016
 * Modificado por: Irene Reyes
 * Numero de OT: 8365
 * Descripción del cambio: Creación
 * */
 /*
 * Fecha de Modificación: 03/04/2016
 * Modificado por: Irene Reyes
 * Numero de OT: 8540
 * Descripción del cambio: Modificar el tipo de parametro del código del partícipe.
 * */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Procesos.DA;
using System.Data.SqlClient;
using System.Data;

namespace Procesos.BL
{
    public class EnvioCorreoBienvenidaCUCBL : INGFondos.Data.DA
    {
        public EnvioCorreoBienvenidaCUCBL()
            : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones){}

        public System.Data.DataTable ListarParticipesXPrimerNumeroDocumento(string fecha)
        {
            EnvioCorreoBienvenidaCUCDA da = new EnvioCorreoBienvenidaCUCDA();
            return da.ListarParticipesXPrimerNumeroDocumento(fecha);
        }
        public void registrarEnvioCorreoBienvenida(int cuc,string usuario)
        {
            EnvioCorreoBienvenidaCUCDA da = new EnvioCorreoBienvenidaCUCDA();
            da.registrarEnvioCorreoBienvenida(cuc, usuario);
        }
        public DataTable ObtenerDatosMailBienvenida(string cuc)
        {
            EnvioCorreoBienvenidaCUCDA da = new EnvioCorreoBienvenidaCUCDA();

            DataTable dt = da.ObtenerDatosMailBienvenida(cuc);
            string contrasena = dt.Rows[0]["CONTRASENA"].ToString().Trim();
            String claveEncriptada = da.ObtenerClaveEncriptada();
            CryptoWS.Crypto wsCrypto = new CryptoWS.Crypto();

            contrasena = wsCrypto.decrypt(contrasena, claveEncriptada);
            dt.Rows[0]["CONTRASENA"] = contrasena;
            return dt;
        }
        public DataTable ObtenerTablaGeneral(string codigoTabla, string llaveTabla)
        {
            AprobacionCUCDA da = new AprobacionCUCDA();
            //return da.ListarTablaGeneral(Constants.ConstantesING.CODIGO_USUARIOS_OFC);
            return da.ObtenerTablaGeneral(codigoTabla, llaveTabla);
        }

    }
}
