Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class TipoInstrumentoCuentaBCRBM


    Public Function SeleccionarPorFiltro(ByVal CodigoInstrumento As String) As DataTable
        Try
            Dim oTipoInstrumentoCuentaBCRDAM As New TipoInstrumentoCuentaBCRDAM
            Return oTipoInstrumentoCuentaBCRDAM.SeleccionarPorFiltro(CodigoInstrumento)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
End Class
