Imports System
Imports System.Data
Imports System.Data.Common





	''' Clase para el acceso de los datos para Precio tabla.
    Public  Class PrecioBM

        Public Sub New()

        End Sub


        ''' Inserta un expediente en Precio tabla.

        Public Function Insertar(ByVal codigoPrecio As String, ByVal tipoPrecio As String, ByVal fecha As Decimal, ByVal ultimoPrecio As Decimal, ByVal precioActual As Decimal, ByVal codigoISIN As String, ByVal codigoNemonico As String, ByVal usuarioCreacion As String, ByVal codigoClaseInstrumento As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String, ByVal host As String)

        End Function


        ''' Selecciona un solo expediente de Precio tabla.
        Public Function Seleccionar(ByVal codigoPrecio As String) As DataSet

        End Function


        ''' Selecciona expedientes de Precio tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoClaseInstrumento(ByVal codigoClaseInstrumento As String) As DataSet


        End Function


        ''' Selecciona expedientes de Precio tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String) As DataSet


        End Function


        ''' Lista todos los expedientes de Precio tabla.
        Public Function Listar() As DataSet

        End Function

        ''' Midifica un expediente en Precio tabla.
        Public Function Modificar(ByVal codigoPrecio As String, ByVal tipoPrecio As String, ByVal fecha As Decimal, ByVal ultimoPrecio As Decimal, ByVal precioActual As Decimal, ByVal codigoISIN As String, ByVal codigoNemonico As String, ByVal usuarioCreacion As String, ByVal codigoClaseInstrumento As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String, ByVal host As String)

        End Function

        ''' Elimina un expediente de Precio table por una llave primaria compuesta.
        Public Function Eliminar(ByVal codigoPrecio As String)

        End Function


        ''' Elimina un expediente de Precio table por una llave extranjera.
        Public Function EliminarPorCodigoClaseInstrumento(ByVal codigoClaseInstrumento As String)


        End Function


        ''' Elimina un expediente de Precio table por una llave extranjera.
        Public Function EliminarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String)

        End Function
    End Class

