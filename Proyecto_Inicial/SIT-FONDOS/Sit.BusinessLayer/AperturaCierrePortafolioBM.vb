Imports System
Imports System.Data
Imports System.Data.Common

Public Class AperturaCierrePortafolioBM

    Public Sub New()

    End Sub

    Public Function Insertar(ByVal codigoAperturaCierre As String, ByVal codigoPortafolio As String, ByVal fecha As Decimal, ByVal estado As String, ByVal usuario As String, ByVal fechaOperacion As Decimal, ByVal horaOperacion As String, ByVal host As String, ByVal accion As String)

    End Function


    Public Function Seleccionar(ByVal codigoAperturaCierre As String) As DataSet

    End Function


    Public Function SeleccionarPorCodigoPortafolio(ByVal codigoPortafolio As String) As DataSet

    End Function


    Public Function Listar() As DataSet

    End Function


    Public Function Modificar(ByVal codigoAperturaCierre As String, ByVal codigoPortafolio As String, ByVal fecha As Decimal, ByVal estado As String, ByVal usuario As String, ByVal fechaOperacion As Decimal, ByVal horaOperacion As String, ByVal host As String, ByVal accion As String)

    End Function


    Public Function Eliminar(ByVal codigoAperturaCierre As String)

    End Function


    Public Function EliminarPorCodigoPortafolio(ByVal codigoPortafolio As String)

    End Function
End Class

