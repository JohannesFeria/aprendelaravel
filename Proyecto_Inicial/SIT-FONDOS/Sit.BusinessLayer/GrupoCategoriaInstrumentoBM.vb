Imports System.Data
Imports MotorTransaccionesProxy
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class GrupoCategoriaInstrumentoBM
  Inherits InvokerCOM

#Region " /* Funciones Seleccionar */ "

  Public Function SeleccionarPorFiltro(ByVal GrupoCategoria As String, ByVal Descripcion As String, ByVal Situacion As String, ByVal Tipo As String, ByVal dataRequest As DataSet) As GrupoCategoriaInstrumentoBE
    Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
    Dim parameters As Object() = {GrupoCategoria, Descripcion, Situacion, Tipo, dataRequest}
    Try
      RegistrarAuditora(parameters)
      Return New GrupoCategoriaInstrumentoDAM().SeleccionarPorFiltro(GrupoCategoria, Descripcion, Situacion, Tipo, dataRequest)
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

    Public Function Insertar(ByVal oGrupoCategoriaInstrumentoBE As GrupoCategoriaInstrumentoBE, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oGrupoCategoriaInstrumentoBE, dataRequest}
        Try
            Dim oGrupoCategoriaInstrumentoDAM As New GrupoCategoriaInstrumentoDAM
            oGrupoCategoriaInstrumentoDAM.Insertar(oGrupoCategoriaInstrumentoBE, dataRequest)
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

    Public Function Modificar(ByVal oGrupoCategoriaInstrumentoBE As GrupoCategoriaInstrumentoBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oGrupoCategoriaInstrumentoBE, dataRequest}
        Try
            Dim oGrupoCategoriaInstrumentoDAM As New GrupoCategoriaInstrumentoDAM
            actualizado = oGrupoCategoriaInstrumentoDAM.Modificar(oGrupoCategoriaInstrumentoBE, dataRequest)
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

    Public Function Eliminar(ByVal GrupoCategoria As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {GrupoCategoria, dataRequest}
        Try
            Dim oGrupoCategoriaInstrumentoDAM As New GrupoCategoriaInstrumentoDAM
            eliminado = oGrupoCategoriaInstrumentoDAM.Eliminar(GrupoCategoria, dataRequest)
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

#End Region

End Class
