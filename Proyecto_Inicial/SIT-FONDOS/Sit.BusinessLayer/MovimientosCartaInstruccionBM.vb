Imports System
Imports System.Data
Imports System.Data.Common




    ''' Clase para el acceso de los datos para MovimientosCartaInstruccion tabla.
    Public  Class MovimientosCartaInstruccionBM

        Public Sub New()

        End Sub

        ''' Inserta un expediente en MovimientosCartaInstruccion tabla.
        Public Function Insertar(ByVal numeroCarta As String, ByVal importe As Decimal, ByVal numeroOperacion As String, ByVal numeroCuenta As String, ByVal fechaRegistro As Decimal, ByVal horaRegistro As String, ByVal estado As String, ByVal situacion As String, ByVal codigoModelo As String, ByVal codigoTipoOperacion As String, ByVal codigoTerceroSBS As Decimal, ByVal codigoPortafolio As String, ByVal codigoRenta As String, ByVal codigoModalidadPago As String, ByVal codigoMoneda As String, ByVal descripcion As String, ByVal codigoNegocio As String, ByVal codigoClaseCuenta As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String, ByVal host As String, ByVal codigoContacto As String, ByVal codigoIntermediario As String)

        End Function

        ''' Selecciona un solo expediente de MovimientosCartaInstruccion tabla.
        Public Function Seleccionar(ByVal numeroCarta As String) As DataSet

        End Function

        ''' Selecciona expedientes de MovimientosCartaInstruccion tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoNegocio(ByVal codigoNegocio As String) As DataSet


        End Function

        ''' Selecciona expedientes de MovimientosCartaInstruccion tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoMoneda(ByVal codigoMoneda As String) As DataSet


        End Function

        ''' Selecciona expedientes de MovimientosCartaInstruccion tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoTerceroSBS(ByVal codigoTerceroSBS As Decimal) As DataSet

        End Function

        ''' Selecciona expedientes de MovimientosCartaInstruccion tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoRenta(ByVal codigoRenta As String) As DataSet


        End Function

        ''' Selecciona expedientes de MovimientosCartaInstruccion tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoIntermediario_CodigoContacto(ByVal codigoIntermediario As String, ByVal codigoContacto As String) As DataSet

        End Function

        ''' Selecciona expedientes de MovimientosCartaInstruccion tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoModalidadPago(ByVal codigoModalidadPago As String) As DataSet


        End Function

        ''' Selecciona expedientes de MovimientosCartaInstruccion tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoPortafolio(ByVal codigoPortafolio As String) As DataSet


        End Function

        ''' Selecciona expedientes de MovimientosCartaInstruccion tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoModelo_CodigoTipoOperacion(ByVal codigoModelo As String, ByVal codigoTipoOperacion As String) As DataSet


        End Function
        ''' Selecciona expedientes de MovimientosCartaInstruccion tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoClaseCuenta(ByVal codigoClaseCuenta As String) As DataSet


        End Function

        ''' Lista todos los expedientes de MovimientosCartaInstruccion tabla.
        Public Function Listar() As DataSet

        End Function

        ''' Midifica un expediente en MovimientosCartaInstruccion tabla.
        Public Function Modificar(ByVal numeroCarta As String, ByVal importe As Decimal, ByVal numeroOperacion As String, ByVal numeroCuenta As String, ByVal fechaRegistro As Decimal, ByVal horaRegistro As String, ByVal estado As String, ByVal situacion As String, ByVal codigoModelo As String, ByVal codigoTipoOperacion As String, ByVal codigoTerceroSBS As Decimal, ByVal codigoPortafolio As String, ByVal codigoRenta As String, ByVal codigoModalidadPago As String, ByVal codigoMoneda As String, ByVal descripcion As String, ByVal codigoNegocio As String, ByVal codigoClaseCuenta As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String, ByVal host As String, ByVal codigoContacto As String, ByVal codigoIntermediario As String)

        End Function

        ''' Elimina un expediente de MovimientosCartaInstruccion table por una llave primaria compuesta.
        Public Function Eliminar(ByVal numeroCarta As String)

        End Function


        ''' Elimina un expediente de MovimientosCartaInstruccion table por una llave extranjera.
        Public Function EliminarPorCodigoNegocio(ByVal codigoNegocio As String)

        End Function


        ''' Elimina un expediente de MovimientosCartaInstruccion table por una llave extranjera.
        Public Function EliminarPorCodigoMoneda(ByVal codigoMoneda As String)


        End Function

        ''' <summary>
        ''' Elimina un expediente de MovimientosCartaInstruccion table por una llave extranjera.
        ''' <summary>
        Public Function EliminarPorCodigoTerceroSBS(ByVal codigoTerceroSBS As Decimal)


        End Function

        ''' Elimina un expediente de MovimientosCartaInstruccion table por una llave extranjera.
        Public Function EliminarPorCodigoRenta(ByVal codigoRenta As String)


        End Function


        ''' Elimina un expediente de MovimientosCartaInstruccion table por una llave extranjera.
        Public Function EliminarPorCodigoIntermediario_CodigoContacto(ByVal codigoIntermediario As String, ByVal codigoContacto As String)


        End Function
        ''' Elimina un expediente de MovimientosCartaInstruccion table por una llave extranjera.
        Public Function EliminarPorCodigoModalidadPago(ByVal codigoModalidadPago As String)


        End Function
        ''' Elimina un expediente de MovimientosCartaInstruccion table por una llave extranjera.
        Public Function EliminarPorCodigoPortafolio(ByVal codigoPortafolio As String)


        End Function
        ''' Elimina un expediente de MovimientosCartaInstruccion table por una llave extranjera.
        Public Function EliminarPorCodigoModelo_CodigoTipoOperacion(ByVal codigoModelo As String, ByVal codigoTipoOperacion As String)


        End Function

        ''' Elimina un expediente de MovimientosCartaInstruccion table por una llave extranjera.
        Public Function EliminarPorCodigoClaseCuenta(ByVal codigoClaseCuenta As String)


        End Function
    End Class

