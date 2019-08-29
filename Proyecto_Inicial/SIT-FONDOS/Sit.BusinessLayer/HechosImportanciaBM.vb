'Creado por: HDG OT 64769-4 20120404
Imports System.Data
Imports MotorTransaccionesProxy
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class HechosImportanciaBM
    Inherits InvokerCOM

    Public Sub New()

    End Sub

    Public Function Insertar(ByVal oHechosImportanciaBE As HechosImportanciaBE, ByVal dataRequest As DataSet) As Boolean

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oHechosImportanciaBE, dataRequest}

        Try
            Dim oHechosImportanciaDAM As New HechosImportanciaDAM

            oHechosImportanciaDAM.Insertar(oHechosImportanciaBE, dataRequest)

            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function SeleccionarPorFiltro(ByVal codigoPortafolio As String, ByVal fecha As Decimal, ByVal situacion As String, ByVal dataRequest As DataSet) As HechosImportanciaBE

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoPortafolio, fecha, situacion, dataRequest}

        Try
            RegistrarAuditora(parameters)

            Return New HechosImportanciaDAM().SeleccionarPorFiltro(codigoPortafolio, fecha, situacion)

        Catch ex As Exception

            RegistrarAuditora(parameters, ex)

            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function
    Public Function Seleccionar(ByVal CodigoHechos As String, ByVal dataRequest As DataSet) As HechosImportanciaBE

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoHechos, dataRequest}

        Try
            RegistrarAuditora(parameters)

            Return New HechosImportanciaDAM().Seleccionar(CodigoHechos)

        Catch ex As Exception

            RegistrarAuditora(parameters, ex)

            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function Modificar(ByVal oHechosImportanciaBE As HechosImportanciaBE, ByVal dataRequest As DataSet) As Boolean

        Dim actualizado As Boolean = False
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oHechosImportanciaBE, dataRequest}

        Dim oHechosImportanciaDAM As New HechosImportanciaDAM

        Try

            actualizado = oHechosImportanciaDAM.Modificar(oHechosImportanciaBE, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return actualizado

    End Function

    Public Function Eliminar(ByVal CodigoHechos As String, ByVal dataRequest As DataSet) As Boolean

        Dim eliminado As Boolean = False
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoHechos, dataRequest}
        Dim oHechosImportanciaDAM As New HechosImportanciaDAM

        Try

            eliminado = oHechosImportanciaDAM.Eliminar(CodigoHechos, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado

    End Function

    Public Function ListarHechosImportancia() As DataSet
        Try
            Return New HechosImportanciaDAM().ListarHechosImportancia()
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

End Class

