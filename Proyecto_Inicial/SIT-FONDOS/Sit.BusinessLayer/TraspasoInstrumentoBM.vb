Imports System
Imports System.Data
Imports System.Data.Common
'




    Public Class TraspasoInstrumentoBM

        Public Sub New()

        End Sub

        Public Function Insertar(ByVal codigoTraspaso As String, ByVal fondoOrigen As String, ByVal fondoDestino As String, ByVal codigoMoneda As String, ByVal codigoSBS As Decimal, ByVal horaCreacion As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal usuarioModificacion As String, ByVal horaModificacion As String, ByVal fechaModificacion As Decimal, ByVal usuarioEliminacion As String, ByVal codigoISIN As String, ByVal fechaEliminacion As Decimal, ByVal codigoNemonico As String, ByVal horaEliminacion As String, ByVal host As String, ByVal codigoTipoOperacion As String)

        End Function


        Public Function Seleccionar(ByVal codigoTraspaso As String) As DataSet

        End Function


        Public Function SeleccionarPorFondoOrigen(ByVal fondoOrigen As String) As DataSet

        End Function

        ''' <summary>
        ''' Selecciona expedientes de TraspasoInstrumento tabla por una llave extranjera.
        ''' <summary>
        ''' <param name="codigoTipoOperacion"></param>
        ''' <returns>DataSet</returns>
        Public Function SeleccionarPorCodigoTipoOperacion(ByVal codigoTipoOperacion As String) As DataSet

        End Function

        ''' <summary>
        ''' Selecciona expedientes de TraspasoInstrumento tabla por una llave extranjera.
        ''' <summary>
        ''' <param name="codigoMoneda"></param>
        ''' <returns>DataSet</returns>
        Public Function SeleccionarPorCodigoMoneda(ByVal codigoMoneda As String) As DataSet

        End Function

        ''' <summary>
        ''' Selecciona expedientes de TraspasoInstrumento tabla por una llave extranjera.
        ''' <summary>
        ''' <param name="codigoISIN"></param>
        ''' <param name="codigoNemonico"></param>
        ''' <returns>DataSet</returns>
        Public Function SeleccionarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String) As DataSet

        End Function

        ''' <summary>
        ''' Selecciona expedientes de TraspasoInstrumento tabla por una llave extranjera.
        ''' <summary>
        ''' <param name="fondoDestino"></param>
        ''' <returns>DataSet</returns>
        Public Function SeleccionarPorFondoDestino(ByVal fondoDestino As String) As DataSet

        End Function

        ''' <summary>
        ''' Lista todos los expedientes de TraspasoInstrumento tabla.
        ''' <summary>
        ''' <returns>DataSet</returns>
        Public Function Listar() As DataSet

        End Function

        ''' <summary>
        ''' Midifica un expediente en TraspasoInstrumento tabla.
        ''' <summary>
        ''' <param name="codigoTraspaso"></param>
        ''' <param name="fondoOrigen"></param>
        ''' <param name="fondoDestino"></param>
        ''' <param name="codigoMoneda"></param>
        ''' <param name="codigoSBS"></param>
        ''' <param name="horaCreacion"></param>
        ''' <param name="usuarioCreacion"></param>
        ''' <param name="fechaCreacion"></param>
        ''' <param name="usuarioModificacion"></param>
        ''' <param name="horaModificacion"></param>
        ''' <param name="fechaModificacion"></param>
        ''' <param name="usuarioEliminacion"></param>
        ''' <param name="codigoISIN"></param>
        ''' <param name="fechaEliminacion"></param>
        ''' <param name="codigoNemonico"></param>
        ''' <param name="horaEliminacion"></param>
        ''' <param name="host"></param>
        ''' <param name="codigoTipoOperacion"></param>
        Public Function Modificar(ByVal codigoTraspaso As String, ByVal fondoOrigen As String, ByVal fondoDestino As String, ByVal codigoMoneda As String, ByVal codigoSBS As Decimal, ByVal horaCreacion As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal usuarioModificacion As String, ByVal horaModificacion As String, ByVal fechaModificacion As Decimal, ByVal usuarioEliminacion As String, ByVal codigoISIN As String, ByVal fechaEliminacion As Decimal, ByVal codigoNemonico As String, ByVal horaEliminacion As String, ByVal host As String, ByVal codigoTipoOperacion As String)

        End Function

        ''' <summary>
        ''' Elimina un expediente de TraspasoInstrumento table por una llave primaria compuesta.
        ''' <summary>
        Public Function Eliminar(ByVal codigoTraspaso As String)

        End Function

        ''' <summary>
        ''' Elimina un expediente de TraspasoInstrumento table por una llave extranjera.
        ''' <summary>
        Public Function EliminarPorFondoOrigen(ByVal fondoOrigen As String)

        End Function

        ''' <summary>
        ''' Elimina un expediente de TraspasoInstrumento table por una llave extranjera.
        ''' <summary>
        Public Function EliminarPorCodigoTipoOperacion(ByVal codigoTipoOperacion As String)

        End Function

        ''' <summary>
        ''' Elimina un expediente de TraspasoInstrumento table por una llave extranjera.
        ''' <summary>
        Public Function EliminarPorCodigoMoneda(ByVal codigoMoneda As String)


        End Function

        ''' <summary>
        ''' Elimina un expediente de TraspasoInstrumento table por una llave extranjera.
        ''' <summary>
        Public Function EliminarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String)


        End Function

        ''' <summary>
        ''' Elimina un expediente de TraspasoInstrumento table por una llave extranjera.
        ''' <summary>
        Public Function EliminarPorFondoDestino(ByVal fondoDestino As String)


        End Function
    End Class

