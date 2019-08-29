Imports System
Imports System.Data
Imports System.Data.Common




    ''' Clase para el acceso de los datos para Tasas tabla.
    Public  Class TasasBM

        Public Sub New()

        End Sub


        ''' Inserta un expediente en Tasas tabla.
        Public Function Insertar(ByVal codigoTasa As String, ByVal codigoEmisor As Decimal, ByVal codigoMercado As String, ByVal codigoCalificacion As String, ByVal vigencia As Decimal, ByVal tasaEncaje As Decimal, ByVal referencia As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal codigoMoneda As String, ByVal codigoISIN As String, ByVal usuarioEliminacion As String, ByVal codigoNemonico As String, ByVal fechaEliminacion As Decimal, ByVal horaModificacion As String, ByVal horaEliminacion As String, ByVal usuarioModificacion As String, ByVal host As String, ByVal fechaModificacion As Decimal)

        End Function

        ''' Selecciona un solo expediente de Tasas tabla.
        Public Function Seleccionar(ByVal codigoTasa As String) As DataSet

        End Function

        ''' Selecciona expedientes de Tasas tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoMoneda(ByVal codigoMoneda As String) As DataSet


        End Function

        ''' Selecciona expedientes de Tasas tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoEmisor(ByVal codigoEmisor As Decimal) As DataSet

        End Function

        ''' Selecciona expedientes de Tasas tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String) As DataSet


        End Function

        ''' Selecciona expedientes de Tasas tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoMercado(ByVal codigoMercado As String) As DataSet


        End Function

        ''' Selecciona expedientes de Tasas tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoCalificacion(ByVal codigoCalificacion As String) As DataSet


        End Function

        ''' Lista todos los expedientes de Tasas tabla.
        Public Function Listar() As DataSet

        End Function

        ''' Midifica un expediente en Tasas tabla.
        Public Function Modificar(ByVal codigoTasa As String, ByVal codigoEmisor As Decimal, ByVal codigoMercado As String, ByVal codigoCalificacion As String, ByVal vigencia As Decimal, ByVal tasaEncaje As Decimal, ByVal referencia As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal codigoMoneda As String, ByVal codigoISIN As String, ByVal usuarioEliminacion As String, ByVal codigoNemonico As String, ByVal fechaEliminacion As Decimal, ByVal horaModificacion As String, ByVal horaEliminacion As String, ByVal usuarioModificacion As String, ByVal host As String, ByVal fechaModificacion As Decimal)

        End Function

        ''' Elimina un expediente de Tasas table por una llave primaria compuesta.
        Public Function Eliminar(ByVal codigoTasa As String)

        End Function

        ''' Elimina un expediente de Tasas table por una llave extranjera.
        Public Function EliminarPorCodigoMoneda(ByVal codigoMoneda As String)


        End Function

        ''' Elimina un expediente de Tasas table por una llave extranjera.
        Public Function EliminarPorCodigoEmisor(ByVal codigoEmisor As Decimal)


        End Function

        ''' Elimina un expediente de Tasas table por una llave extranjera.
        Public Function EliminarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String)


        End Function

        ''' Elimina un expediente de Tasas table por una llave extranjera.
        Public Function EliminarPorCodigoMercado(ByVal codigoMercado As String)

        End Function

        ''' Elimina un expediente de Tasas table por una llave extranjera.
        Public Function EliminarPorCodigoCalificacion(ByVal codigoCalificacion As String)


        End Function
    End Class

