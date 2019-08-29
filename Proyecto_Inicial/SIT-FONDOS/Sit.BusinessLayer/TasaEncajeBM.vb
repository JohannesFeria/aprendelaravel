Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class TasaEncajeBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub
    Public Function Seleccionar(ByVal SecuenciaTasa As String) As DataSet
        Dim oTasaEncajeBE As DataSet
        Try
            Dim oTasaEncajeDAM As New TasaEncajeDAM
            oTasaEncajeBE = oTasaEncajeDAM.Seleccionar(SecuenciaTasa)
        Catch ex As Exception
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oTasaEncajeBE
    End Function
    Public Function SeleccionarPorFiltro(ByVal codigoEntidad As String, _
     ByVal codigoCalificacion As String, ByVal fechaVigencia As Decimal, _
    ByVal codigoNemonico As String, ByVal TasaVigente As Decimal) As DataSet
        Dim oTasaEncajeBE As DataSet
        Try
            Dim oTasaEncajeDAM As New TasaEncajeDAM
            oTasaEncajeBE = oTasaEncajeDAM.SeleccionarPorFiltro(codigoEntidad, codigoCalificacion, fechaVigencia, codigoNemonico, TasaVigente)
        Catch ex As Exception
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oTasaEncajeBE
    End Function

    Public Function Insertar(ByVal objTasaEncaje As TasaEncajeBE.TasaEncajeRow, ByVal dataRequest As DataSet) As String
        Dim strCodigo As String
        'Se solicita un código de ejecución al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {objTasaEncaje, dataRequest}
        Try
            Dim daTasaEncaje As New TasaEncajeDAM
            strCodigo = daTasaEncaje.Insertar(objTasaEncaje, dataRequest)
            'Luego de terminar la ejecución de métodos(sin errores) 
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
        Return strCodigo

    End Function
    Public Function Modificar(ByVal objTasaEncaje As TasaEncajeBE.TasaEncajeRow, ByVal dataRequest As DataSet) As String
        Dim strCodigo As String
        'Se solicita un código de ejecución al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {objTasaEncaje, dataRequest}
        Try
            Dim daTasaEncaje As New TasaEncajeDAM
            strCodigo = daTasaEncaje.Modificar(objTasaEncaje, dataRequest)
            'Luego de terminar la ejecución de métodos(sin errores) 
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
        Return strCodigo

    End Function
    Public Function Eliminar(ByVal secuencia As String, ByVal dataRequest As DataSet) As String
        Dim strCodigo As String
        'Se solicita un código de ejecución al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {secuencia, dataRequest}
        Try
            Dim daTasaEncaje As New TasaEncajeDAM
            strCodigo = daTasaEncaje.Eliminar(secuencia)
            'Luego de terminar la ejecución de métodos(sin errores) 
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
        Return strCodigo

    End Function
    Public Sub Actualizar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub

End Class
