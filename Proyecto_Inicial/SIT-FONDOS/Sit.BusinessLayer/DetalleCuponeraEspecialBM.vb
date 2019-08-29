Imports System
Imports System.Data
Imports System.Data.Common




    ''' Clase para el acceso de los datos para DetalleCuponeraEspecial tabla.
    Public  Class DetalleCuponeraEspecialBM

        Public Sub New()

        End Sub

        ''' Inserta un expediente en DetalleCuponeraEspecial tabla.
        Public Function Insertar(ByVal codigoISIN As String, ByVal codigoNemonico As String, ByVal fechaInicio As Decimal, ByVal fechaTermino As Decimal, ByVal de As Decimal, ByVal a As Decimal, ByVal diferenciaDias As Decimal, ByVal tasaCupon As Decimal, ByVal base As Decimal, ByVal diasPago As Decimal, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String, ByVal host As String, ByVal codigoAmortizacion As String)

        End Function

        ''' Selecciona un solo expediente de DetalleCuponeraEspecial tabla.
        Public Function Seleccionar(ByVal codigoISIN As String, ByVal codigoNemonico As String) As DataSet

        End Function
        ''' Selecciona expedientes de DetalleCuponeraEspecial tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoAmortizacion(ByVal codigoAmortizacion As String) As DataSet

        End Function


        ''' Selecciona expedientes de DetalleCuponeraEspecial tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String) As DataSet


        End Function

        ''' Lista todos los expedientes de DetalleCuponeraEspecial tabla.
        Public Function Listar() As DataSet

        End Function


        ''' Midifica un expediente en DetalleCuponeraEspecial tabla.
        Public Function Modificar(ByVal codigoISIN As String, ByVal codigoNemonico As String, ByVal fechaInicio As Decimal, ByVal fechaTermino As Decimal, ByVal de As Decimal, ByVal a As Decimal, ByVal diferenciaDias As Decimal, ByVal tasaCupon As Decimal, ByVal base As Decimal, ByVal diasPago As Decimal, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String, ByVal host As String, ByVal codigoAmortizacion As String)

        End Function

        ''' Elimina un expediente de DetalleCuponeraEspecial table por una llave primaria compuesta.
        Public Function Eliminar(ByVal codigoISIN As String, ByVal codigoNemonico As String)

        End Function


        ''' Elimina un expediente de DetalleCuponeraEspecial table por una llave extranjera.
        Public Function EliminarPorCodigoAmortizacion(ByVal codigoAmortizacion As String)

        End Function

        ''' Elimina un expediente de DetalleCuponeraEspecial table por una llave extranjera.
        Public Function EliminarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String)

        End Function
    End Class

