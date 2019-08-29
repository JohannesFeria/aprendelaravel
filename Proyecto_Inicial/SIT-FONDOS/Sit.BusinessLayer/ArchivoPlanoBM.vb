Imports System
Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class ArchivoPlanoBM
    Inherits InvokerCOM

#Region "/* Funciones Seleccionar */"

    Public Function Seleccionar(ByVal ArchivoCodigo As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New ArchivoPlanoDAM().Seleccionar(ArchivoCodigo, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Listar(ByVal dataRequest As DataSet, Optional ByVal grupoArchivo As String = "") As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New ArchivoPlanoDAM().Listar(grupoArchivo)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function ListarEstructura(ByVal CodigoArchivo As String, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New ArchivoPlanoDAM().ListarEstructura(CodigoArchivo, dataRequest).Tables(0)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function ListarResultado(ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New ArchivoPlanoDAM().ListarResultado(dataRequest).Tables(0)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    '''Publicado por Yanina Pérez
    '''Fecha Creación 10/12/2007
    '''Descripción Inserta registros de orden de inversión desde un txt
    Public Function InsertarDatosArchivo(ByVal dtsRegistro As DataSet, ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim fila As Integer
        Dim parameters As Object()
        Try
            Dim oArchivoPlanoDAM As New ArchivoPlanoDAM
            For fila = 0 To dtsRegistro.Tables(0).Rows.Count - 1
                parameters = New Object() {dtsRegistro.Tables(0).Rows(fila), dataRequest}
                oArchivoPlanoDAM.InsertarDatosArchivo(dtsRegistro.Tables(0).Rows(fila), dataRequest)
                RegistrarAuditora(parameters)
            Next
        Catch ex As Exception
            parameters = New Object() {dtsRegistro.Tables(0).Rows(fila), dataRequest}
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Insertar_OrdenInversionDATATEC(ByVal dtsRegistro As DataSet, ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim fila As Integer
        Dim parameters As Object()

        'Dim ds As New DataSet
        'Dim dtDatos As New DataTable
        'Dim dtResultado As DataTable
        'Dim ColumnaResultado As Integer

        Try
            Dim oArchivoPlanoDAM As New ArchivoPlanoDAM
            For fila = 0 To dtsRegistro.Tables(0).Rows.Count - 1
                parameters = New Object() {dtsRegistro.Tables(0).Rows(fila), dataRequest}

                'ds = oArchivoPlanoDAM.Insertar_OrdenInversionDATATEC(dtsRegistro.Tables(0).Rows(fila), dataRequest)
                oArchivoPlanoDAM.Insertar_OrdenInversionDATATEC(dtsRegistro.Tables(0).Rows(fila), dataRequest)
                RegistrarAuditora(parameters)

                'For ColumnaResultado = 0 To ds.Tables(0).Columns.Count - 1
                '    dtDatos.Columns.Add(dtResultado.Rows(fila).Item(ColumnaResultado))
                'Next

            Next
            'Return dtDatos
        Catch ex As Exception
            parameters = New Object() {dtsRegistro.Tables(0).Rows(fila), dataRequest}
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

#End Region

    Public Function Insertar(ByVal oArchivoPlano As ArchivoPlanoBE, ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oArchivoPlano, dataRequest}
        Try
            Dim oArchivoPlanoDAM As New ArchivoPlanoDAM
            oArchivoPlanoDAM.Insertar(oArchivoPlano, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Modificar(ByVal oArchivoPlano As ArchivoPlanoBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oArchivoPlano, dataRequest}
        Try
            Dim oArchivoPlanoDAM As New ArchivoPlanoDAM
            actualizado = oArchivoPlanoDAM.Modificar(oArchivoPlano, dataRequest)
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

    Public Function Eliminar(ByVal ArchivoCodigo As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {ArchivoCodigo, dataRequest}
        Try
            Dim oArchivoPlanoDAM As New ArchivoPlanoDAM
            eliminado = oArchivoPlanoDAM.Eliminar(ArchivoCodigo, dataRequest)
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

End Class
