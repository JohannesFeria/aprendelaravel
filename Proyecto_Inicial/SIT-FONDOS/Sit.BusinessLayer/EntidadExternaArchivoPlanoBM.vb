Imports System
Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class EntidadExternaArchivoPlanoBM
    Inherits InvokerCOM

    Public Function Seleccionar(ByVal Valor As String, ByVal dataRequest As DataSet) As DataSet

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New EntidadExternaArchivoPlanoDAM().Seleccionar(Valor, dataRequest)
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

    End Function

    Public Function Listar(ByVal sEntidadExterna As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {sEntidadExterna, dataRequest}
        Try
            Return New EntidadExternaArchivoPlanoDAM().Listar(sEntidadExterna)
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
    End Function

    Public Function CargarArchivo(ByVal sEntidadExterna As String, ByVal sEntidadArchivo As String, ByVal sFechaInformacion As String, ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {sEntidadArchivo, dataRequest}
        Try
            Dim oEntidadExternaArchivoDAM As New EntidadExternaArchivoPlanoDAM
            oEntidadExternaArchivoDAM.CargarArchivo(sEntidadExterna, sEntidadArchivo, sFechaInformacion, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function InsertarDivLibBloomberg(ByVal Fecha As Date, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As String

        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dtDetalle, dataRequest}
        Try
            Dim oInterfaseBloombergDAM As New InterfaseBloombergDAM

            codigo = New EntidadExternaArchivoPlanoDAM().InsertarDivLibBloomberg(Fecha, dtDetalle, dataRequest)
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

        Return codigo

    End Function

    Public Function ImportarArchivo(ByVal sEntidadExterna As String, ByVal nFechaInformacion As Long, ByVal sFlagBorrado As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New EntidadExternaArchivoPlanoDAM().ImportarArchivo(sEntidadExterna, nFechaInformacion, sFlagBorrado)
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
    End Function

    Public Function VerificaPreCarga(ByVal sEntidadExterna As String, ByVal nFechaInformacion As Long, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New EntidadExternaArchivoPlanoDAM().VerificaPreCarga(sEntidadExterna, nFechaInformacion)
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
    End Function

    Public Function Eliminar(ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim oEntidadExternaArchivoDAM As New EntidadExternaArchivoPlanoDAM
            eliminado = oEntidadExternaArchivoDAM.Eliminar(dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado
    End Function

End Class
