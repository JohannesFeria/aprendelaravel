Imports System.Data
Imports MotorTransaccionesProxy
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class PatrimonioFideicomisoBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

    Public Function SeleccionarPorFiltro(ByVal codigo As String, ByVal descripcion As String, ByVal Situacion As String, ByVal dataRequest As DataSet) As PatrimonioFideicomisoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigo, descripcion, Situacion, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New PatrimonioFideicomisoDAM().SeleccionarPorFiltro(codigo, descripcion, Situacion, dataRequest)
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

    'HDG OT 61045 20101020
    Public Function SeleccionarPorFiltroExportar(ByVal codigo As String, ByVal descripcion As String, ByVal Situacion As String, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigo, descripcion, Situacion, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New PatrimonioFideicomisoDAM().SeleccionarPorFiltroExportar(codigo, descripcion, Situacion, dataRequest)
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

    Public Function Insertar(ByVal oPatriFideiBE As PatrimonioFideicomisoBE, ByVal dataRequest As DataSet) As String
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oPatriFideiBE, dataRequest}
        Dim resul As String = ""
        Try
            Dim oPatriFideiDAM As New PatrimonioFideicomisoDAM
            resul = oPatriFideiDAM.Insertar(oPatriFideiBE, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Return ""
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return resul
    End Function

    Public Function Modificar(ByVal oPatriFideiBE As PatrimonioFideicomisoBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oPatriFideiBE, dataRequest}
        Try
            Dim oPatriFideiDAM As New PatrimonioFideicomisoDAM
            actualizado = oPatriFideiDAM.Modificar(oPatriFideiBE, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return actualizado
    End Function

    Public Function Eliminar(ByVal CodigoFideicomiso As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoFideicomiso, dataRequest}
        Try
            Dim oPatriFideiDAM As New PatrimonioFideicomisoDAM
            eliminado = oPatriFideiDAM.Eliminar(CodigoFideicomiso, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado
    End Function

    'HDG OT 61045 20101019
    Public Function ActualizarPatrimonioFideicomisoPorExcel(ByVal dtPatrimonio As DataTable, ByVal dataRequest As DataSet, ByRef strmensaje As String) As Boolean
        Dim Codigo = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dtPatrimonio, dataRequest}

        Try
            Dim oPatrimonioFideicomisoDAM As New PatrimonioFideicomisoDAM
            Codigo = oPatrimonioFideicomisoDAM.ActualizarPatrimonioFideicomisoPorExcel(dtPatrimonio, dataRequest, strmensaje)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            Dim sms As String = ex.Message
            Dim src As String = ex.Source
            RegistrarAuditora(parameters, ex)
            Return False
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return True
    End Function

    Public Function SeleccionarPorCodigoDetalle(ByVal CodigoPatrimonioFideicomiso As String, ByVal dataRequest As DataSet) As PatrimonioFideicomisoDetalleBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoPatrimonioFideicomiso, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New PatrimonioFideicomisoDAM().SeleccionarPorCodigoDetalle(CodigoPatrimonioFideicomiso)
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

    Public Function SeleccionarNemonicoCaracteristicas(ByVal CodigoNemonico As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoNemonico, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New PatrimonioFideicomisoDAM().SeleccionarNemonicoCaracteristicas(CodigoNemonico)
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

    Public Function InsertarModificarDetalle(ByVal oPatriFideiBE As PatrimonioFideicomisoDetalleBE, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oPatriFideiBE, dataRequest}
        Dim resul As Boolean = False
        Try
            Dim oPatriFideiDAM As New PatrimonioFideicomisoDAM
            resul = oPatriFideiDAM.InsertarModificarDetalle(oPatriFideiBE, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Return False
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return resul
    End Function

    Public Function InsertarDetalle(ByVal oPatriFideiBE As PatrimonioFideicomisoDetalleBE, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oPatriFideiBE, dataRequest}
        Try
            Dim oPatriFideiDAM As New PatrimonioFideicomisoDAM
            oPatriFideiDAM.InsertarDetalle(oPatriFideiBE, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Return False
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return True
    End Function

    Public Function ModificarDetalle(ByVal oPatriFideiBE As PatrimonioFideicomisoDetalleBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oPatriFideiBE, dataRequest}
        Try
            Dim oPatriFideiDAM As New PatrimonioFideicomisoDAM
            actualizado = oPatriFideiDAM.ModificarDetalle(oPatriFideiBE, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return actualizado
    End Function

    Public Function EliminarDetalle(ByVal CodigoFideicomisoDetalle As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoFideicomisoDetalle, dataRequest}
        Try
            Dim oPatriFideiDAM As New PatrimonioFideicomisoDAM
            eliminado = oPatriFideiDAM.EliminarDetalle(CodigoFideicomisoDetalle, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado
    End Function

    Public Function ValidarExistencia(ByVal CodPatriFideiDetalle As String, ByVal CodigoNemonico As String, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoNemonico, dataRequest}
        Dim resul As Boolean = False
        Try
            RegistrarAuditora(parameters)
            resul = New PatrimonioFideicomisoDAM().ValidarExistencia(CodPatriFideiDetalle, CodigoNemonico)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return resul
    End Function

    Public Function ValidarPatrimonio(ByVal codigoPatrimonio As String, ByVal nombre As String, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoPatrimonio, nombre, dataRequest}
        Dim resul As Boolean = False
        Try
            RegistrarAuditora(parameters)
            resul = New PatrimonioFideicomisoDAM().ValidarPatrimonio(codigoPatrimonio, nombre)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return resul
    End Function

    Public Sub Actualizar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
End Class
