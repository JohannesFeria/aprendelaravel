/*
 * Fecha de Creación: 05/07/2017
 * Creado por: Anthony Joaquin
 * Número de OT: 10563
 * Descripción del cambio: Creación
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procesos.TD
{
    public class TablaGeneralAdmCuenta
    {
        public TablaGeneralAdmCuenta()
        {
        }

        int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        string codigoTabla;

        public string CodigoTabla
        {
            get { return codigoTabla; }
            set { codigoTabla = value; }
        }

        string llaveTabla;

        public string LlaveTabla
        {
            get { return llaveTabla; }
            set { llaveTabla = value; }
        }


        int numeroOrden;

        public int NumeroOrden
        {
            get { return numeroOrden; }
            set { numeroOrden = value; }
        }


        string descripcionCorta;

        public string DescripcionCorta
        {
            get { return descripcionCorta; }
            set { descripcionCorta = value; }
        }

        string descripcionLarga;

        public string DescripcionLarga
        {
            get { return descripcionLarga; }
            set { descripcionLarga = value; }
        }

        string abreviacion;

        public string Abreviacion
        {
            get { return abreviacion; }
            set { abreviacion = value; }
        }

        string estado;

        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

    }
}
