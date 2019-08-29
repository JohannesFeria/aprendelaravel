Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

''' Clase para el acceso de los datos para KardexCartera tabla.
    Public  Class KardexCarteraBM
    Inherits InvokerCOM

        Public Sub New()

        End Sub

        ''' Inserta un expediente en KardexCartera tabla.



    ''' Selecciona un solo expediente de KardexCartera tabla.
    Public Function Seleccionar(ByVal codigoKardex As String) As DataSet

    End Function


    ''' Selecciona expedientes de KardexCartera tabla por una llave extranjera.
    Public Function SeleccionarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String) As DataSet


    End Function

    ''' Selecciona expedientes de KardexCartera tabla por una llave extranjera.
    Public Function SeleccionarPorCodigoIntermediario_CodigoContacto(ByVal codigoIntermediario As String, ByVal codigoContacto As String) As DataSet


    End Function

    ''' Selecciona expedientes de KardexCartera tabla por una llave extranjera.
    Public Function SeleccionarPorCodigoTipoOperacion(ByVal codigoTipoOperacion As String) As DataSet


    End Function

    ''' Lista todos los expedientes de KardexCartera tabla.
    Public Function Listar() As DataSet

    End Function


    ''' Midifica un expediente en KardexCartera tabla.

    Public Function Modificar(ByVal codigoKardex As String, ByVal codigoISIN As String, ByVal codigoNemonico As String, ByVal codigoTipoOperacion As String, ByVal fechaOperacion As Decimal, ByVal cantidadCompra As Decimal, ByVal montoBrutoCompra As Decimal, ByVal montoNetoCompra As Decimal, ByVal precioPromedioCompra As Decimal, ByVal cantidadVenta As Decimal, ByVal montoNetoVenta As Decimal, ByVal precioPromedioVenta As Decimal, ByVal codigoIntermediario As String, ByVal cantidadAcumulado As Decimal, ByVal montoBrutoAcumulado As Decimal, ByVal montoNetoAcumulado As Decimal, ByVal precioPromedioNetoAcumulado As Decimal, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String, ByVal codigoContacto As String, ByVal host As String, ByVal situacion As String)

    End Function

    ''' Elimina un expediente de KardexCartera table por una llave primaria compuesta.
    Public Function Eliminar(ByVal codigoKardex As String)

    End Function

    ''' Elimina un expediente de KardexCartera table por una llave extranjera.
    Public Function EliminarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String)


    End Function

    ''' Elimina un expediente de KardexCartera table por una llave extranjera.
    Public Function EliminarPorCodigoIntermediario_CodigoContacto(ByVal codigoIntermediario As String, ByVal codigoContacto As String)

    End Function

    ''' Elimina un expediente de KardexCartera table por una llave extranjera.
    Public Function EliminarPorCodigoTipoOperacion(ByVal codigoTipoOperacion As String)


    End Function
End Class

