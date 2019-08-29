Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class ConceptoIdiBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub
    Public Function SeleccionarPorCodConcepto(ByVal CodConcepto As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodConcepto, dataRequest}
        Dim oDSConceptoIdi As New DataSet
        Try
            oDSConceptoIdi = New ConceptoIdiDAM().SeleccionarPorCodConcepto(CodConcepto, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDSConceptoIdi
    End Function

End Class
