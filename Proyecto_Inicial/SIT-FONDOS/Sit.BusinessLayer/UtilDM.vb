Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Public Class UtilDM

    Public Function RetornarFechaHoraSistema() As Date
        Return New UtilDAM().RetornarFechaHoraSistema
    End Function

    Public Function RetornarFechaSistema() As String
        Return New UtilDAM().RetornarFechaSistema
    End Function
    Public Function RetornarFechaNegocio() As String
        Return New UtilDAM().RetornarFechaNegocio
    End Function
    Public Function RetornarHoraSistema() As String
        Return New UtilDAM().RetornarHoraSistema
    End Function

    Public Function RetornarMensajeConfirmacion(ByVal codigoMensaje As String) As String
        Return New UtilDAM().RetornarMensajeConfirmacion(codigoMensaje)
    End Function

    'considera los feriados
    Public Function RetornarFechaUtilAnterior(ByVal fecha As Decimal) As Decimal
        Return New UtilDAM().RetornarFechaUtilAnterior(fecha)
    End Function

    'RGF 20090121 no se basa en los feriados, sino en la fecha de valoracion anterior
    Public Function RetornarFechaValoradaAnterior(ByVal fecha As Decimal, ByVal portafolio As String) As Decimal
        Return New UtilDAM().RetornarFechaValoradaAnterior(fecha, portafolio)
    End Function

    'RGF 20090218
    Public Function ValidarFechaHabil(ByVal fecha As Decimal, ByVal codigoTercero As String) As String
        Return New UtilDAM().ValidarFechaHabil(fecha, codigoTercero)
    End Function

    Public Shared Function ConvertirFechaaDecimal(ByVal fecha As String) As Decimal
        If fecha.Length = 0 Then
            Return 0
        ElseIf fecha.Length = 8 Then
            Return fecha
        End If
        Return Convert.ToDecimal(fecha.Substring(6, 4) + fecha.Substring(3, 2) + fecha.Substring(0, 2))
    End Function

    Public Shared Function ConvertirStringaFecha(ByVal fecha As String) As System.DateTime
        Dim strfecha As System.DateTime
        If fecha.Length > 0 Then strfecha = New Date(fecha.Split("/")(2), fecha.Split("/")(1), fecha.Split("/")(0))
        Return strfecha
    End Function

    Public Shared Function ConvertirFechaaString(ByVal fecha As Decimal) As String
        Dim strfecha As String = Convert.ToString(fecha)
        If strfecha.Length = 8 Then Return strfecha.Substring(6, 2) + "/" + strfecha.Substring(4, 2) + "/" + strfecha.Substring(0, 4)
        Return ""
    End Function

    Public Shared Function RetornarFechaAnterior(ByVal p_Fecha As Decimal) As Decimal
        Dim fechaString As String = ConvertirFechaaString(p_Fecha)
        Dim fechaDate As Date = ConvertirStringaFecha(fechaString)
        Dim fechaAnterior As Date = fechaDate.AddDays(-1)
        Dim fechaAnteriorDecimal As Decimal = Convert.ToDecimal(fechaAnterior.ToString("yyyyMMdd"))
        RetornarFechaAnterior = fechaAnteriorDecimal
    End Function
    'OT11103 - 02/02/2018 - Ian Pastor M.
    'Descripción: Devuelve una fecha indicando el número de días que se quiere agregar
    Public Shared Function fnDateAddDays(ByVal p_FechaActual As String, ByVal p_NumeroDias As Integer) As Date
        Dim fechaAnterior As Date
        Dim fechaNueva As Date
        fechaAnterior = Convert.ToDateTime(p_FechaActual)
        fechaNueva = fechaAnterior.AddDays(p_NumeroDias)
        Return fechaNueva
    End Function
    'OT11103 - Ian
End Class
