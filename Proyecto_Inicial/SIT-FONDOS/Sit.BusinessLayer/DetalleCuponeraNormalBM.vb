Imports System
Imports System.Data
Imports System.Data.Common




	''' <summary>
	''' Clase para el acceso de los datos para DetalleCuponeraNormal tabla.
	''' </summary>
	Public class DetalleCuponeraNormalBM
	
		Public Sub New()

        End Sub

        ''' Inserta un expediente en DetalleCuponeraNormal tabla.
        Public Function Insertar(ByVal codigoISIN As String, ByVal codigoNemonico As String, ByVal fechaInicio As Decimal, ByVal fechaTermino As Decimal, ByVal diferenciaDias As Decimal, ByVal tasaCupon As Decimal, ByVal base As Decimal, ByVal flujo As Decimal, ByVal diasPago As Decimal, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String, ByVal host As String)

        End Function


        ''' Selecciona un solo expediente de DetalleCuponeraNormal tabla.
        Public Function Seleccionar(ByVal codigoISIN As String, ByVal codigoNemonico As String) As DataSet

        End Function


        ''' Selecciona expedientes de DetalleCuponeraNormal tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String) As DataSet


        End Function

        ''' Lista todos los expedientes de DetalleCuponeraNormal tabla.
        Public Function Listar() As DataSet

        End Function


        ''' Midifica un expediente en DetalleCuponeraNormal tabla.

        Public Function Modificar(ByVal codigoISIN As String, ByVal codigoNemonico As String, ByVal fechaInicio As Decimal, ByVal fechaTermino As Decimal, ByVal diferenciaDias As Decimal, ByVal tasaCupon As Decimal, ByVal base As Decimal, ByVal flujo As Decimal, ByVal diasPago As Decimal, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String, ByVal host As String)

        End Function


        ''' Elimina un expediente de DetalleCuponeraNormal table por una llave primaria compuesta.
        Public Function Eliminar(ByVal codigoISIN As String, ByVal codigoNemonico As String)

        End Function


        ''' Elimina un expediente de DetalleCuponeraNormal table por una llave extranjera.
        Public Function EliminarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String)


        End Function
    End Class

