Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy
Public Class OrdenInversionTMPBM

    Public Sub New()

    End Sub
    Public Function InsertarOrdenInversionTemporal(ByVal oOrdenInverTMPBE As OrdenInversionTMPBE) As Boolean
        Dim resultado As Boolean
        Try
            resultado = New OrdenInversionTMPDAM().InsertarOrdenInversionTemporal(oOrdenInverTMPBE)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return resultado
    End Function
    Public Function Seleccionar_OrdenInversionTMPValidarSaldo(ByVal codigoOperacion As String, ByVal guid As String) As DataTable
        Dim resultado As DataTable
        Try
            resultado = New OrdenInversionTMPDAM().Seleccionar_OrdenInversionTMPValidarSaldo(codigoOperacion, guid)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return resultado
    End Function
    Public Sub InicializarOrdenInversion(ByVal oRowOI As OrdenInversionTMPBE.OrdenInversionBETMPRow)
        oRowOI.CodigoCustodio = ""
        oRowOI.CodigoNemonico = ""
        oRowOI.CodigoOperacion = ""
        oRowOI.CodigoPortafolioSBS = ""
        oRowOI.CodigoTercero = ""
        oRowOI.CantidadOrdenado = 0
        oRowOI.FechaSaldo = 0
    End Sub
End Class
