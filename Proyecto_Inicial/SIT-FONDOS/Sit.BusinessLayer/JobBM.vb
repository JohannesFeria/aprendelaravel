'Creado por: HDG OT 61566 Nro3-C 20101104
Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports System.Data.Common
Imports MotorTransaccionesProxy

Public Class JobBM
    Inherits InvokerCOM

    Public Sub New()

    End Sub

    Public Function EjecutarJob(ByVal sNombreDTS As String, ByVal sDescripcionJob As String, ByVal sVariables As String, ByVal sParametros As String, ByVal sCorreoUsuario As String, ByVal sCorreoCopia As String, ByVal sServidorETL As String) As String
        Dim tx As New JobDAM
        Return tx.EjecutarJob(sNombreDTS, sDescripcionJob, sVariables, sParametros, sCorreoUsuario, sCorreoCopia, sServidorETL)
    End Function

End Class
