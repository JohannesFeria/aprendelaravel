/*
 * Fecha de Modificación: 26/08/2015
 * Modificado por: Juan Castro
 * Numero de OT: 7584
 * Descripción del cambio: Creación de clase.
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procesos.TD
{
    public class Cliente
    {
        private string codigoCliente;

        public string CodigoCliente
        {
            get { return codigoCliente; }
            set { codigoCliente = value; }
        }

        private string tipoDocumento;

        public string TipoDocumento
        {
            get { return tipoDocumento; }
            set { tipoDocumento = value; }
        }

        private string razonSocial;

        public string RazonSocial
        {
            get { return razonSocial; }
            set { razonSocial = value; }
        }
        private string direccion;

        public string Direccion
        {
            get { return direccion; }
            set { direccion = value; }
        }
        private int idPais;

        public int IdPais
        {
            get { return idPais; }
            set { idPais = value; }
        }
        private int idDepartamento;

        public int IdDepartamento
        {
            get { return idDepartamento; }
            set { idDepartamento = value; }
        }
        private int idCiudad;

        public int IdCiudad
        {
            get { return idCiudad; }
            set { idCiudad = value; }
        }
        private int idDistrito;

        public int IdDistrito
        {
            get { return idDistrito; }
            set { idDistrito = value; }
        }
        private string ubigeo;

        public string Ubigeo
        {
            get { return ubigeo; }
            set { ubigeo = value; }
        }
        private string codigoPostal;

        public string CodigoPostal
        {
            get { return codigoPostal; }
            set { codigoPostal = value; }
        }
        private string correo;

        public string Correo
        {
            get { return correo; }
            set { correo = value; }
        }

        private string usuario;

        public string Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }

        private string pais;

        public string Pais
        {
            get { return pais; }
            set { pais = value; }
        }

        private string departamento;

        public string Departamento
        {
            get { return departamento; }
            set { departamento = value; }
        }

        private string ciudad;

        public string Ciudad
        {
            get { return ciudad; }
            set { ciudad = value; }
        }

        private string distrito;

        public string Distrito
        {
            get { return distrito; }
            set { distrito = value; }
        }
    }
}
