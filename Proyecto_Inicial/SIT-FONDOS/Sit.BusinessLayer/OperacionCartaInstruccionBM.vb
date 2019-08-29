Imports System
Imports System.Data
Imports System.Data.Common




    ''' Clase para el acceso de los datos para OperacionCartaInstruccion tabla.
    Public  Class OperacionCartaInstruccionBM

        Public Sub New()

        End Sub

        ''' Inserta un expediente en OperacionCartaInstruccion tabla.
        Public Function Insertar(ByVal codigoModelo As String, ByVal codigoTipoOperacion As String, ByVal situacion As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal host As String, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String)

        End Function


        ''' Selecciona un solo expediente de OperacionCartaInstruccion tabla.
        Public Function Seleccionar(ByVal codigoModelo As String, ByVal codigoTipoOperacion As String) As DataSet

        End Function

        ''' Selecciona expedientes de OperacionCartaInstruccion tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoModelo(ByVal codigoModelo As String) As DataSet


        End Function

        ''' Selecciona expedientes de OperacionCartaInstruccion tabla por una llave extranjera.
        Public Function SeleccionarPorCodigoTipoOperacion(ByVal codigoTipoOperacion As String) As DataSet


        End Function

        ''' Lista todos los expedientes de OperacionCartaInstruccion tabla.
        Public Function Listar() As DataSet

        End Function

        ''' Midifica un expediente en OperacionCartaInstruccion tabla.
        Public Function Modificar(ByVal codigoModelo As String, ByVal codigoTipoOperacion As String, ByVal situacion As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal host As String, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String)

        End Function

        ''' Elimina un expediente de OperacionCartaInstruccion table por una llave primaria compuesta.
        Public Function Eliminar(ByVal codigoModelo As String, ByVal codigoTipoOperacion As String)

        End Function

        ''' Elimina un expediente de OperacionCartaInstruccion table por una llave extranjera.
        Public Function EliminarPorCodigoModelo(ByVal codigoModelo As String)


        End Function

        ''' Elimina un expediente de OperacionCartaInstruccion table por una llave extranjera.
        Public Function EliminarPorCodigoTipoOperacion(ByVal codigoTipoOperacion As String)


        End Function
    End Class

