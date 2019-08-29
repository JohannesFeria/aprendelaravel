Imports System
Imports System.Data
Imports System.Data.Common




	''' <summary>
	''' Clase para el acceso de los datos para DetalleEstructurados tabla.
	''' </summary>
	Public class DetalleEstructuradosBM
	
		Public Sub New()

        End Sub

        ''' Inserta un expediente en DetalleEstructurados tabla.
        Public Function Insertar(ByVal codigoTipoInstrumento As Decimal, ByVal monto As Decimal, ByVal cantidad As Decimal, ByVal usuarioCreacion As String, ByVal horaCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaModificacion As String, ByVal usuarioModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaModificacion As Decimal, ByVal codigoISIN As String, ByVal codigoNemonico As String, ByVal fechaEliminacion As Decimal, ByVal host As String, ByVal horaEliminacion As String)

        End Function

        ''' Selecciona un solo expediente de DetalleEstructurados tabla.
        Public Function Seleccionar(ByVal codigoISIN As String, ByVal codigoNemonico As String) As DataSet

        End Function


        ''' Selecciona expedientes de DetalleEstructurados tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoTipoInstrumento(ByVal codigoTipoInstrumento As Decimal) As DataSet


        End Function

        ''' Selecciona expedientes de DetalleEstructurados tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String) As DataSet

        End Function

        ''' Lista todos los expedientes de DetalleEstructurados tabla.
        Public Function Listar() As DataSet

        End Function

        ''' Midifica un expediente en DetalleEstructurados tabla.
        Public Function Modificar(ByVal codigoTipoInstrumento As Decimal, ByVal monto As Decimal, ByVal cantidad As Decimal, ByVal usuarioCreacion As String, ByVal horaCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaModificacion As String, ByVal usuarioModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaModificacion As Decimal, ByVal codigoISIN As String, ByVal codigoNemonico As String, ByVal fechaEliminacion As Decimal, ByVal host As String, ByVal horaEliminacion As String)

        End Function


        ''' Elimina un expediente de DetalleEstructurados table por una llave primaria compuesta.
        Public Function Eliminar(ByVal codigoISIN As String, ByVal codigoNemonico As String)

        End Function


        ''' Elimina un expediente de DetalleEstructurados table por una llave extranjera.
        Public Function EliminarPorCodigoTipoInstrumento(ByVal codigoTipoInstrumento As Decimal)

        End Function


        ''' Elimina un expediente de DetalleEstructurados table por una llave extranjera.
        Public Function EliminarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String)

        End Function
    End Class

