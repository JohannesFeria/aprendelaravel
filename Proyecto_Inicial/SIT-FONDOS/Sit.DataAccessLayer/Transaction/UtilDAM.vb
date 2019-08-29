Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class UtilDAM

    Public Function RetornarFechaHoraSistema() As Date
        Dim strFecha As Date
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("RetornarFechaHoraSistema")
        Dim oValoresBE As New ValoresBE
        strFecha = db.ExecuteScalar(dbCommand)
        Return strFecha

    End Function

    Public Function RetornarFechaSistema() As String
        Dim strFecha As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("RetornarFechaSistema")
        Dim oValoresBE As New ValoresBE
        strFecha = Convert.ToString(db.ExecuteScalar(dbCommand))
        Return strFecha

    End Function
    Public Function RetornarFechaNegocio() As String
        Dim strFecha As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("RetornarFechaNegocio")
        Dim oValoresBE As New ValoresBE
        strFecha = Convert.ToString(db.ExecuteScalar(dbCommand))
        Return strFecha

    End Function
    Public Function RetornarHoraSistema() As String
        Dim strFecha As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("RetornarHoraSistema")
        Dim oValoresBE As New ValoresBE
        strFecha = Convert.ToString(db.ExecuteScalar(dbCommand))
        Return strFecha

    End Function

    Public Function RetornarMensajeConfirmacion(ByVal codigoMensaje As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Mensaje_RetornarDescripcion")
        db.AddInParameter(dbCommand, "@p_CodigoMensaje", DbType.String, codigoMensaje)
        Return Convert.ToString(db.ExecuteScalar(dbCommand))
    End Function
    Public Function RetornarFechaUtilAnterior(ByVal fecha As Decimal) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ObtenerFechaUtilAnterior")
        db.AddInParameter(dbCommand, "@p_fechaproceso", DbType.String, fecha)
        Return Convert.ToDecimal(db.ExecuteScalar(dbCommand))
    End Function

    'RGF 20090121
    Public Function RetornarFechaValoradaAnterior(ByVal fecha As Decimal, ByVal portafolio As String) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ObtenerFechaValoradaAnterior")
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, portafolio)
        Return Convert.ToDecimal(db.ExecuteScalar(dbCommand))
    End Function

    'RGF 20090218 Verifica si la Fecha enviada es un dia laborable
    Public Function ValidarFechaHabil(ByVal fecha As Decimal, ByVal codigoTercero As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ValidarFechaHabil")
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
        Return db.ExecuteScalar(dbCommand)
    End Function
End Class
