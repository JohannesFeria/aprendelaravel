Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class ArchivosVAXBCOSBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

    Public Function Insertar(ByVal objArchivosVAXBCOS As ArchivosVAXBCOSBE.ArchivosVAXBCOSRow, ByVal dataRequest As DataSet) As String
        Dim strCodigo As String
        'Se solicita un c�digo de ejecuci�n al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Se agrupan los par�metros enviados en el mismo orden
        Dim parameters As Object() = {objArchivosVAXBCOS, dataRequest}
        Try
            Dim daArchivosVAXBCOS As New ArchivosVAXBCOSDAM
            strCodigo = daArchivosVAXBCOS.Insertar(objArchivosVAXBCOS, dataRequest)
            'Luego de terminar la ejecuci�n de m�todos(sin errores) 
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo m�todo pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 l�neas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return strCodigo
    End Function

    'Public Function SeleccionarPortafolioFecha(ByVal portafolioSBS As String, ByVal fecha As Decimal, ByVal dataRequest As DataSet) As DataSet
    '    Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
    '    Dim parameters As Object() = {portafolioSBS, fecha, dataRequest}
    '    Dim oDSArchivosVAXBCOS As New DataSet
    '    Try
    '        oDSArchivosVAXBCOS = New ArchivosVAXBCOSDAM().SeleccionarPorPortafolioFecha(portafolioSBS, fecha, dataRequest)
    '        RegistrarAuditora(parameters)
    '    Catch ex As Exception
    '        'Si ocurre un error se invoca el mismo m�todo pero se agrega el obj exception
    '        RegistrarAuditora(parameters, ex)
    '        'Las siguientes 4 l�neas deben agregarse para el Exception app block
    '        Dim rethrow As Boolean = true
    '        If (rethrow) Then
    '            Throw
    '        End If
    '    End Try
    '    Return oDSArchivosVAXBCOS
    'End Function

    Public Function SeleccionarPorFiltro(ByVal codigoIndicador As String, ByVal portafolioSBS As String, ByVal fecha As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoIndicador, portafolioSBS, fecha, dataRequest}
        Dim oDSArchivosVAXBCOS As New DataSet
        Try
            oDSArchivosVAXBCOS = New ArchivosVAXBCOSDAM().SeleccionarPorFiltro(codigoIndicador, portafolioSBS, fecha, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo m�todo pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 l�neas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDSArchivosVAXBCOS
    End Function

End Class
