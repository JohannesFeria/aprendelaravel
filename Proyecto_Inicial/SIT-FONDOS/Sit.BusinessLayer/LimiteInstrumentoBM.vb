Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

    Public  Class LimiteInstrumentoBM
    Inherits InvokerCOM
        Public Sub New()

        End Sub
#Region " /* Funciones Seleccionar */ "


   
    Public Function Seleccionar(ByVal StrCodigoLimite As String, ByVal StrCodigoPosicion As String, ByVal dataRequest As DataSet) As LimiteInstrumentoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {StrCodigoLimite, StrCodigoPosicion, dataRequest}
        Try

            RegistrarAuditora(parameters)
            Return New LimiteInstrumentoDAM().Seleccionar(StrCodigoLimite, StrCodigoPosicion, dataRequest)


        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As LimiteInstrumentoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New LimiteInstrumentoDAM().Listar(dataRequest)


        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function


    Public Function SeleccionarPorFiltro(ByVal StrCodigoLimite As String, ByVal StrCodigoPosicion As String, ByVal StrSituacion As String, ByVal dataRequest As DataSet) As LimiteInstrumentoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {StrCodigoPosicion, StrCodigoLimite, StrSituacion, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New LimiteInstrumentoDAM().SeleccionarPorFiltro(StrCodigoLimite, StrCodigoPosicion, StrSituacion, dataRequest)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try


    End Function
#End Region

#Region " /* Funciones Insertar */ "

    Public Function Insertar(ByVal oLimiteInstrumentoBE As LimiteInstrumentoBE, ByVal dataRequest As DataSet) As String

        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oLimiteInstrumentoBE, dataRequest}
        Try
            Dim oLimiteInstrumentoDAM As New LimiteInstrumentoDAM
            oLimiteInstrumentoDAM.Insertar(oLimiteInstrumentoBE, dataRequest)
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

#End Region

#Region " /* Funciones Modificar */"


    Public Function Modificar(ByVal oLimiteInstrumentoBE As LimiteInstrumentoBE, ByVal dataRequest As DataSet) As Boolean

        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oLimiteInstrumentoBE, dataRequest}
        Try
            Dim oLimiteInstrumentoDAM As New LimiteInstrumentoDAM
            oLimiteInstrumentoDAM.Modificar(oLimiteInstrumentoBE, dataRequest)
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

#End Region

#Region " /* Funciones Eliminar */"

    Public Function Eliminar(ByVal StrCodigoLimite As String, ByVal StrCodigoPos As String, ByVal dataRequest As DataSet)

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {StrCodigoLimite, StrCodigoPos, dataRequest}
        Try
            Dim oLimiteInstrumentoDAM As New LimiteInstrumentoDAM
            oLimiteInstrumentoDAM.Eliminar(StrCodigoLimite, StrCodigoPos)
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

