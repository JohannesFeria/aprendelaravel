Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

    Public Class TipoValorizacionBM
    Inherits InvokerCOM
        Public Sub New()

        End Sub



#Region " /* Funciones Seleccionar */ "

    Public Function SeleccionarPorFiltroBcrSeriado(ByVal NombreCuenta As String, ByVal dataRequest As DataSet) As DataTable
        '  Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {NombreCuenta, dataRequest}
        Try

            Return New TipoValorizacionDAM().SeleccionarPorFiltroBcrSeriado(NombreCuenta, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function SeleccionarBcrSeriado(ByVal codigoMnemonico As String, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoMnemonico, dataRequest}
        Try
            Return New TipoValorizacionDAM().SeleccionarBcrSeriado(codigoMnemonico, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function SeleccionarBcrUnico(ByVal codigoSBS As String, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoSBS, dataRequest}
        Try
            Return New TipoValorizacionDAM().SeleccionarBcrUnico(codigoSBS, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function SeleccionarPorFiltroBcrUnico(ByVal codigoMnemonico As String, ByVal dataRequest As DataSet) As BCRUnicoBE
        ' Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoMnemonico, dataRequest}
        Try
            Return New TipoValorizacionDAM().SeleccionarPorFiltroBcrUnico(codigoMnemonico, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function SeleccionarPorFiltro(ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As TipoValorizacionBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {descripcion, situacion, dataRequest}
        Try

            Return New TipoValorizacionDAM().SeleccionarPorFiltro(descripcion, situacion, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try


    End Function
    Public Function Seleccionar(ByVal codigoTipoValorizacion As String, ByVal dataRequest As DataSet) As TipoValorizacionBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoTipoValorizacion, dataRequest}
        Try

            Return New TipoValorizacionDAM().Seleccionar(codigoTipoValorizacion, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function Listar(ByVal dataRequest As DataSet) As TipoValorizacionBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try

            Return New TipoValorizacionDAM().Listar(dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
#End Region

#Region " /* Funciones Insertar */ "

    Public Function Insertar(ByVal oTipoValorizacionBE As TipoValorizacionBE, ByVal dataRequest As DataSet) As String

        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oTipoValorizacionBE, dataRequest}
        Try
            Dim oTipoValorizacionDAM As New TipoValorizacionDAM

            codigo = oTipoValorizacionDAM.Insertar(oTipoValorizacionBE, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return codigo
    End Function

    Public Function InsertarBCRSeriado(ByVal oBCRSeriadoBE As BCRSeriadoBE, ByVal dataRequest As DataSet) As Boolean

        Dim resultado As Boolean = True
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oBCRSeriadoBE, dataRequest}
        Try
            Dim oTipoValorizacionDAM As New TipoValorizacionDAM
            resultado = oTipoValorizacionDAM.InsertarBCRSeriado(oBCRSeriadoBE, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return resultado
    End Function


    Public Function InsertarBCRUnico(ByVal oBCRUnicoBE As BCRUnicoBE, ByVal dataRequest As DataSet) As Boolean
        Dim resultado As Boolean = True
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oBCRUnicoBE, dataRequest}
        Try
            Dim oTipoValorizacionDAM As New TipoValorizacionDAM
            resultado = oTipoValorizacionDAM.InsertarBCRUnico(oBCRUnicoBE, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return resultado
    End Function

#End Region

#Region " /* Funciones Modificar */"


    Public Function Modificar(ByVal oTipoValorizacionBE As TipoValorizacionBE, ByVal dataRequest As DataSet) As Boolean

        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oTipoValorizacionBE, dataRequest}
        Try
            Dim oTipoValorizacionDAM As New TipoValorizacionDAM

            actualizado = oTipoValorizacionDAM.Modificar(oTipoValorizacionBE, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

        Return actualizado

    End Function

    Public Function ModificarBCRSeriado(ByVal oBCRSeriadoBE As BCRSeriadoBE, ByVal dataRequest As DataSet) As Boolean

        Dim actualizado As Boolean = False
        Dim parameters As Object() = {oBCRSeriadoBE, dataRequest}
        Try
            Dim oTipoValorizacionDAM As New TipoValorizacionDAM
            actualizado = oTipoValorizacionDAM.ModificarBCRSeriado(oBCRSeriadoBE, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return actualizado
    End Function

    Public Function ModificarBCRUnico(ByVal oBCRUnicoBE As BCRUnicoBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim parameters As Object() = {oBCRUnicoBE, dataRequest}
        Try
            Dim oTipoValorizacionDAM As New TipoValorizacionDAM
            actualizado = oTipoValorizacionDAM.ModificarBCRUnico(oBCRUnicoBE, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return actualizado
    End Function

#End Region

#Region " /* Funciones Eliminar */"

    Public Function EliminarBCRSeriado(ByVal codigoMnemonico As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoMnemonico, dataRequest}
        Try
            Dim oTipoValorizacionDAM As New TipoValorizacionDAM
            eliminado = oTipoValorizacionDAM.EliminarBCRSeriado(codigoMnemonico, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado
    End Function

    Public Function EliminarBCRUnico(ByVal codigoSBS As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoSBS, dataRequest}
        Try
            Dim oTipoValorizacionDAM As New TipoValorizacionDAM
            eliminado = oTipoValorizacionDAM.EliminarBCRUnico(codigoSBS, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado
    End Function


    Public Function Eliminar(ByVal codigoTipoValorizacion As String, ByVal dataRequest As DataSet)

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoTipoValorizacion, dataRequest}
        Try
            Dim oTipoValorizacionDAM As New TipoValorizacionDAM

            eliminado = oTipoValorizacionDAM.Eliminar(codigoTipoValorizacion, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado
    End Function
#End Region

#Region " /* Funciones Personalizadas*/"
    Public Sub Salir(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
    Public Sub Actualizar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
    Public Sub Ingresar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
#End Region


   
   
End Class

