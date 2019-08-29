Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports MotorTransaccionesProxy

Public Class EventoAutomaticoBM
    Inherits InvokerCOM

    Public Sub New()
    End Sub

    Public Function Insertar(ByVal evAuto As EventosAutomaticosBE, ByVal dataRequest As DataSet) As Boolean
        Try
            Dim daEvAuto As New EventosAutomaticosDAM
            Return daEvAuto.Insertar(evAuto, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
