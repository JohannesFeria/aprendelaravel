Imports System
Imports System.Data
Imports System.Data.Common




    ''' Clase para el acceso de los datos para NivelLimite tabla.
    Public  Class NivelLimiteBM

        Public Sub New()

        End Sub

        ''' Inserta un expediente en NivelLimite tabla.
        Public Function Insertar(ByVal codigoNivel As Decimal, ByVal porcentaje As Decimal, ByVal descripcion As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal fechaModificacion As Decimal, ByVal horaCreacion As String, ByVal host As String, ByVal usuarioModificacion As String, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String)

        End Function

        ''' Selecciona un solo expediente de NivelLimite tabla.
        Public Function Seleccionar(ByVal codigoNivel As Decimal) As DataSet

        End Function

        ''' Lista todos los expedientes de NivelLimite tabla.
        Public Function Listar() As DataSet

        End Function

        ''' Midifica un expediente en NivelLimite tabla.
        Public Function Modificar(ByVal codigoNivel As Decimal, ByVal porcentaje As Decimal, ByVal descripcion As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal fechaModificacion As Decimal, ByVal horaCreacion As String, ByVal host As String, ByVal usuarioModificacion As String, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String)

        End Function

        ''' Elimina un expediente de NivelLimite table por una llave primaria compuesta.
        Public Function Eliminar(ByVal codigoNivel As Decimal)

        End Function
    End Class

