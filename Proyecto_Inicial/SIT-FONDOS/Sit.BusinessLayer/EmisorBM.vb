Imports System
Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities



''' Clase para el acceso de los datos para Emisor tabla.
Public Class EmisorBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub
    Public Function Listar(ByVal dataRequest As DataSet) As EmisorBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New EmisorDAM().Listar(dataRequest)
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
    ''' Inserta un expediente en Emisor tabla.
    Public Function Insertar(ByVal codigoEmisor As Decimal, ByVal sinonimoEmisor As String, ByVal descripcion As String, ByVal numeroDocumento As Decimal, ByVal direccion As String, ByVal codigoPais As Decimal, ByVal codigoTipoDocumento As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal codigoMercado As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal codigoPostal As String, ByVal horaCreacion As String, ByVal codigoSectorEmpresarial As String, ByVal situacion As String, ByVal host As String, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String)

    End Function

    ''' Selecciona un solo expediente de Emisor tabla.
    Public Function Seleccionar(ByVal codigoEmisor As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim oEmisorDAM As New EmisorDAM
        Dim oEmisorBE As EmisorBE
        Try
            oEmisorBE = oEmisorDAM.Seleccionar(codigoEmisor)
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
        Return oEmisorBE
    End Function

    ''' Selecciona expedientes de Emisor tabla por una llave extranjera.
    Public Function SeleccionarPorCodigoSectorEmpresarial(ByVal codigoSectorEmpresarial As String) As DataSet

    End Function
    ''' Selecciona expedientes de Emisor tabla por una llave extranjera.
    Public Function SeleccionarPorCodigoTipoDocumento(ByVal codigoTipoDocumento As String) As DataSet


    End Function



    ''' Selecciona expedientes de Emisor tabla por una llave extranjera.
    Public Function SeleccionarPorCodigoPais(ByVal codigoPais As Decimal) As DataSet


    End Function


    ''' Selecciona expedientes de Emisor tabla por una llave extranjera.
    Public Function SeleccionarPorCodigoMercado(ByVal codigoMercado As String) As DataSet


    End Function


    ''' Selecciona expedientes de Emisor tabla por una llave extranjera.
    Public Function SeleccionarPorCodigoPostal(ByVal codigoPostal As String) As DataSet


    End Function


    ''' Midifica un expediente en Emisor tabla.
    Public Function Modificar(ByVal codigoEmisor As Decimal, ByVal sinonimoEmisor As String, ByVal descripcion As String, ByVal numeroDocumento As Decimal, ByVal direccion As String, ByVal codigoPais As Decimal, ByVal codigoTipoDocumento As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal codigoMercado As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal codigoPostal As String, ByVal horaCreacion As String, ByVal codigoSectorEmpresarial As String, ByVal situacion As String, ByVal host As String, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String)

    End Function

    ''' Elimina un expediente de Emisor table por una llave primaria compuesta.
    Public Function Eliminar(ByVal codigoEmisor As Decimal)

    End Function

    ''' Elimina un expediente de Emisor table por una llave extranjera.
    Public Function EliminarPorCodigoSectorEmpresarial(ByVal codigoSectorEmpresarial As String)

    End Function

    ''' Elimina un expediente de Emisor table por una llave extranjera.
    Public Function EliminarPorCodigoTipoDocumento(ByVal codigoTipoDocumento As String)

    End Function

    ''' Elimina un expediente de Emisor table por una llave extranjera.
    Public Function EliminarPorCodigoPais(ByVal codigoPais As Decimal)

    End Function

    ''' Elimina un expediente de Emisor table por una llave extranjera.
    Public Function EliminarPorCodigoMercado(ByVal codigoMercado As String)


    End Function

    ''' Elimina un expediente de Emisor table por una llave extranjera.
    Public Function EliminarPorCodigoPostal(ByVal codigoPostal As String)


    End Function
End Class

