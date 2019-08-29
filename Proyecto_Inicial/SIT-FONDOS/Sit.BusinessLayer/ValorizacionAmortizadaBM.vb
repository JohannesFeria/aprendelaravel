Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy
Imports System.Collections.Generic

Public Class ValorizacionAmortizadaBM
    Inherits InvokerCOM
    Public Sub New()
    End Sub

    Public Function ComprasReferidasAlStock(ByVal codigoPortafolio As String, ByVal fechaOperacion As Decimal) As DataSet
        Dim oValoresDAM As New ValorizacionAmortizadaDAM
        Return oValoresDAM.ComprasReferidasAlStock(codigoPortafolio, fechaOperacion)
    End Function

    Public Sub GuardarValorizacion(ByVal codigoPortafolio As String, ByVal fechaProceso As Integer, ByVal dtValorizacion As ValorizacionAmortizadaBE.ValorizacionAmortizadaDataTable, ByVal codUsuario As String)
        Dim oValorizacionDAM As New ValorizacionAmortizadaDAM
        Dim idProceso As Integer = oValorizacionDAM.RegNuevoProcesoValorizacion(codUsuario)

        Try
            oValorizacionDAM.EliminarValorizacion(codigoPortafolio, fechaProceso)

            For Each row As ValorizacionAmortizadaBE.ValorizacionAmortizadaRow In dtValorizacion.Rows
                row.CodigoPortafolioSBS = codigoPortafolio
                row.FechaProceso = fechaProceso
                oValorizacionDAM.GuardarValorizacionRow(idProceso, row)
            Next

            oValorizacionDAM.ActProcesoValorizacionOk(idProceso)
        Catch ex As Exception
            oValorizacionDAM.ActProcesoValorizacionError(idProceso, ex.Message)
            Throw ex
        End Try
    End Sub

    Public Sub EliminarValorizacion(ByVal codigoPortafolio As String, ByVal fechaProceso As Integer)
        Dim oValorizacionDAM As New ValorizacionAmortizadaDAM       
        oValorizacionDAM.EliminarValorizacion(codigoPortafolio, fechaProceso)
    End Sub

End Class
