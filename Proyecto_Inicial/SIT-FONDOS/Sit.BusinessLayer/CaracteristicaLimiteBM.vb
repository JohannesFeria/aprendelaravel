Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

    Public Class CaracteristicaLimiteBM
    Inherits InvokerCOM
        Public Sub New()

        End Sub
#Region " /* Funciones Seleccionar */ "
    Public Function SeleccionarPorFiltro(ByVal StrCodigoNegocio As String, ByVal StrTipoLim As String, ByVal StrCodigoPortafolio As String, ByVal StrClaseLim As String, ByVal dataRequest As DataSet) As CaracteristicaLimiteBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {StrCodigoNegocio, StrTipoLim, StrTipoLim, StrCodigoPortafolio, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New CaracteristicaLimiteDAM().SeleccionarPorFiltro(StrCodigoNegocio, StrTipoLim, StrCodigoPortafolio, StrClaseLim, dataRequest)
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
    Public Function Listar(ByVal dataRequest As DataSet) As GrupoEconomicoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New GrupoEconomicoDAM().Listar(dataRequest)
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




    Public Function Insertar(ByVal codigoLimite As String, ByVal tipoCalculo As String, ByVal tope As String, ByVal unidadPosicion As String, ByVal valorBase As String, ByVal claseLimite As String, ByVal tipoLimite As String, ByVal factor As String, ByVal aplicarCastigo As String, ByVal saldoBanco As String, ByVal codigoPortafolio As String, ByVal situacion As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal host As String, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal codigoNegocio As String, ByVal horaEliminacion As String)

    End Function
    Public Function Modificar(ByVal codigoLimite As String, ByVal tipoCalculo As String, ByVal tope As String, ByVal unidadPosicion As String, ByVal valorBase As String, ByVal claseLimite As String, ByVal tipoLimite As String, ByVal factor As String, ByVal aplicarCastigo As String, ByVal saldoBanco As String, ByVal codigoPortafolio As String, ByVal situacion As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal host As String, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal codigoNegocio As String, ByVal horaEliminacion As String)

    End Function


    Public Function Eliminar(ByVal codigoLimite As String)

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
    Public Sub Ingresar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
#End Region
End Class

