Imports System
Imports System.Data
Imports System.Data.Common




    ''' Clase para el acceso de los datos para CuentasPorPagar tabla.
    Public Class CuentasPorPagarBM

        Public Sub New()

        End Sub
        ''' Inserta un expediente en CuentasPorPagar tabla.
        Public Function Insertar(ByVal codigoClaseCuenta As String, ByVal codigoMoneda As String, ByVal codigoPortafolio As String, ByVal codigoMercado As String, ByVal codigoTipoOperacion As String, ByVal numeroCuenta As Decimal, ByVal codigoCuenta As String, ByVal referencia As String, ByVal situacion As String, ByVal importe As Decimal, ByVal fechaIngreso As Decimal, ByVal fechaOperacion As Decimal, ByVal fechaPago As Decimal, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal host As String, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal codigoOrden As String, ByVal codigoOperacion As String, ByVal tipoMovimiento As String, ByVal horaEliminacion As String)

        End Function

        ''' Selecciona un solo expediente de CuentasPorPagar tabla.
        Public Function Seleccionar(ByVal codigoCuenta As String) As DataSet

        End Function

        ''' Selecciona expedientes de CuentasPorPagar tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoClaseCuenta(ByVal codigoClaseCuenta As String) As DataSet

        End Function

        ''' Selecciona expedientes de CuentasPorPagar tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoMercado(ByVal codigoMercado As String) As DataSet

        End Function

        ''' Selecciona expedientes de CuentasPorPagar tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoTipoOperacion(ByVal codigoTipoOperacion As String) As DataSet

        End Function
        ''' Selecciona expedientes de CuentasPorPagar tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoOperacion(ByVal codigoOperacion As String) As DataSet

        End Function

        ''' Selecciona expedientes de CuentasPorPagar tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoPortafolio(ByVal codigoPortafolio As String) As DataSet

        End Function

        ''' Selecciona expedientes de CuentasPorPagar tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoMoneda(ByVal codigoMoneda As String) As DataSet


        End Function


        ''' Selecciona expedientes de CuentasPorPagar tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoOrden(ByVal codigoOrden As String) As DataSet


        End Function


        ''' Lista todos los expedientes de CuentasPorPagar tabla.
        Public Function Listar() As DataSet

        End Function
        ''' Midifica un expediente en CuentasPorPagar tabla.
        Public Function Modificar(ByVal codigoClaseCuenta As String, ByVal codigoMoneda As String, ByVal codigoPortafolio As String, ByVal codigoMercado As String, ByVal codigoTipoOperacion As String, ByVal numeroCuenta As Decimal, ByVal codigoCuenta As String, ByVal referencia As String, ByVal situacion As String, ByVal importe As Decimal, ByVal fechaIngreso As Decimal, ByVal fechaOperacion As Decimal, ByVal fechaPago As Decimal, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal host As String, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal codigoOrden As String, ByVal codigoOperacion As String, ByVal tipoMovimiento As String, ByVal horaEliminacion As String)

        End Function


        ''' Elimina un expediente de CuentasPorPagar table por una llave primaria compuesta.
        Public Function Eliminar(ByVal codigoCuenta As String)

        End Function

        ''' Elimina un expediente de CuentasPorPagar table por una llave extranjera.
        Public Function EliminarPorCodigoClaseCuenta(ByVal codigoClaseCuenta As String)

        End Function

        ''' Elimina un expediente de CuentasPorPagar table por una llave extranjera.
        Public Function EliminarPorCodigoMercado(ByVal codigoMercado As String)


        End Function


        ''' Elimina un expediente de CuentasPorPagar table por una llave extranjera.

        Public Function EliminarPorCodigoTipoOperacion(ByVal codigoTipoOperacion As String)


        End Function

        ''' Elimina un expediente de CuentasPorPagar table por una llave extranjera.
        Public Function EliminarPorCodigoOperacion(ByVal codigoOperacion As String)

        End Function

        ''' Elimina un expediente de CuentasPorPagar table por una llave extranjera.
        Public Function EliminarPorCodigoPortafolio(ByVal codigoPortafolio As String)

        End Function

        ''' Elimina un expediente de CuentasPorPagar table por una llave extranjera.
        Public Function EliminarPorCodigoMoneda(ByVal codigoMoneda As String)


        End Function

        ''' Elimina un expediente de CuentasPorPagar table por una llave extranjera.
        Public Function EliminarPorCodigoOrden(ByVal codigoOrden As String)


        End Function
    End Class

