Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class GrupoInstrumentoBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub
#Region "/*Insertar*/"
    Public Function Insertar(ByVal Descripcion As String, ByVal situacion As String, ByVal dtCaracteristicaGrupoNivel As DataTable, ByVal dataRequest As DataSet) As String
        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {Descripcion, situacion, dtCaracteristicaGrupoNivel, dataRequest}
        Try
            Dim oGrupoInstrumentoDAM As New GrupoInstrumentoDAM

            codigo = oGrupoInstrumentoDAM.Insertar(Descripcion, situacion, dtCaracteristicaGrupoNivel, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True 'true
            If (rethrow) Then
                Throw
            End If
        End Try

        Return codigo

    End Function
#End Region

#Region "/*Modificar*/"
    Public Function Modificar(ByVal CodigoGrupoInstrumento As String, ByVal Descripcion As String, ByVal situacion As String, ByVal dtCaracteristicasValores As DataTable, ByVal dataRequest As DataSet) As String
        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoGrupoInstrumento, Descripcion, situacion, dtCaracteristicasValores, dataRequest}
        Try
            Dim oGrupoInstrumentoDAM As New GrupoInstrumentoDAM

            codigo = oGrupoInstrumentoDAM.Modificar(CodigoGrupoInstrumento, Descripcion, situacion, dtCaracteristicasValores, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True 'true
            If (rethrow) Then
                Throw
            End If
        End Try

        Return codigo

    End Function
#End Region

#Region "/*Seleccionar*/"
    Public Function SeleccionarPorFiltro(ByVal CodigoGrupoInstrumento As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoGrupoInstrumento, descripcion, situacion, dataRequest}
        Dim obj As New DataSet
        Try
            RegistrarAuditora(parameters)
            obj = New GrupoInstrumentoDAM().SeleccionarPorFiltro(CodigoGrupoInstrumento, descripcion, situacion, dataRequest)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return obj
    End Function

    Public Function SeleccionarCaracteristicaGrupo(ByVal CodigoGrupoInstrumento As String, ByVal CodigoCaracteristica As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoGrupoInstrumento, CodigoCaracteristica, dataRequest}
        Dim obj As New DataSet
        Try
            RegistrarAuditora(parameters)
            obj = New GrupoInstrumentoDAM().SeleccionarCaracteristicasGrupo(CodigoGrupoInstrumento, CodigoCaracteristica, dataRequest)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return obj
    End Function
    'Public Function SeleccionarCaracteristicaGrupoNivel(ByVal CodigoGrupoInstrumento As String, ByVal CodigoCaracteristica As String, ByVal Vista As String, ByVal dataRequest As DataSet) As DataSet
    '    Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
    '    Dim parameters As Object() = {CodigoGrupoInstrumento, CodigoCaracteristica, Vista, dataRequest}
    '    Dim obj As New DataSet
    '    Try
    '        RegistrarAuditora(parameters)
    '        obj = New GrupoInstrumentoDAM().SeleccionarCaracteristicasGrupoNivel(CodigoGrupoInstrumento, CodigoCaracteristica, Vista, dataRequest)
    '    Catch ex As Exception
    '        'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
    '        RegistrarAuditora(parameters, ex)
    '        'Las siguientes 4 líneas deben agregarse para el Exception app block
    '        Dim rethrow As Boolean = true
    '        If (rethrow) Then
    '            Throw
    '        End If
    '    End Try
    '    Return obj
    'End Function
    Public Function SeleccionarCodigoCaracteristicasGrupoNivel(ByVal CodigoGrupoInstrumento As String, _
    ByVal FlagDetalle As String, ByVal CodigoCaracteristica As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoGrupoInstrumento, FlagDetalle, dataRequest}
        Dim obj As New DataSet
        Try
            RegistrarAuditora(parameters)
            obj = New GrupoInstrumentoDAM().SeleccionarCodigoCaracteristicasGrupoNivel(CodigoGrupoInstrumento, FlagDetalle, CodigoCaracteristica, dataRequest)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return obj
    End Function

    Public Function SeleccionarValoresPorCaracteristica(ByVal CodigoCaracteristica As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoCaracteristica, dataRequest}
        Dim obj As New DataSet
        Try
            RegistrarAuditora(parameters)
            obj = New GrupoInstrumentoDAM().SeleccionarValoresPorCaracteristica(CodigoCaracteristica, dataRequest)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return obj
    End Function
    Public Function SeleccionarDescripcionValoresPorValorVista(ByVal CodigoValor As String, ByVal Vista As String, ByVal dataRequest As DataSet) As String
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoValor, Vista, dataRequest}
        Dim strDescripcion As String
        Try
            RegistrarAuditora(parameters)
            strDescripcion = New GrupoInstrumentoDAM().SeleccionarDescripcionValoresPorValorVista(CodigoValor, Vista, dataRequest)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return strDescripcion
    End Function
#End Region

#Region "/*Eliminar*/"
    Public Function Eliminar(ByVal CodigoGrupoInstrumento As String, ByVal dataRequest As DataSet)

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoGrupoInstrumento, dataRequest}
        Try
            Dim oGrupoInstrumentoDAM As New GrupoInstrumentoDAM

            eliminado = oGrupoInstrumentoDAM.Eliminar(CodigoGrupoInstrumento, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True 'true
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
