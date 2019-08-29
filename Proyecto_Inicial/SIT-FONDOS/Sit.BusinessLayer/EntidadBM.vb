Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class EntidadBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

    Public Function SeleccionarPorFiltro(ByVal codigoEntidad As String, ByVal codigoIdentificacion As String, ByVal codigoPostal As String, ByVal codigoMercado As String, ByVal codigoSectorEmpresarial As String, ByVal tipoTercero As String, ByVal situacion As String, ByVal descripcion As String, ByVal dataRequest As DataSet) As EntidadBE

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoIdentificacion, codigoPostal, codigoMercado, codigoSectorEmpresarial, tipoTercero, situacion, dataRequest}
        Dim oEntidadBE As EntidadBE
        Dim oEntidadDAM As New EntidadDAM

        Try
            RegistrarAuditora(parameters)
            oEntidadBE = oEntidadDAM.SeleccionarPorFiltro(codigoEntidad, codigoIdentificacion, codigoPostal, codigoMercado, codigoSectorEmpresarial, tipoTercero, situacion, descripcion)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If

        End Try

        Return oEntidadBE

    End Function
    '==============================================================
    ' CREADO POR  : Zoluxiones Consulting S.A.C (JVC)
    ' NÚMERO DE OT: 
    ' DESCRIPCIÓN : Obtiene datos de intermediarios
    ' FECHA DE CREACIÓN : 09/03/2009
    ' PARÁMETROS ENTRADA: CodigoIntermediario: Código del intermediario
    '  	                  Situacion	        : Situación
    '	                  EntidadBroker      : Entidad de broker
    '================================================================*/
    Public Function Entidad_Listar(ByVal dataRequest As DataSet, _
                                   ByVal CodigoIntermediario As String, _
                                   ByVal Situacion As String, _
                                   ByVal EntidadBroker As String) As EntidadSitBE
        Dim CodigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoIntermediario, Situacion, EntidadBroker, dataRequest}
        Dim oEntidadDAM As New EntidadDAM
        Dim oEntidadBE As EntidadSitBE
        Try
            RegistrarAuditora(parameters)
            oEntidadBE = oEntidadDAM.Entidad_Listar(CodigoIntermediario, Situacion, EntidadBroker)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oEntidadBE
    End Function

    Public Function Insertar(ByVal oEntidadBE As EntidadBE, ByVal dataRequest As DataSet) As Boolean

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oEntidadBE, dataRequest}
        Dim oEntidadDAM As New EntidadDAM

        Try

            oEntidadDAM.Insertar(oEntidadBE, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function Modificar(ByVal oEntidadBE As EntidadBE, ByVal sinonimo As String, ByVal dataRequest As DataSet) As Boolean

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oEntidadBE, dataRequest}
        Dim oEntidadDAM As New EntidadDAM

        Try

            oEntidadDAM.Modificar(oEntidadBE, sinonimo, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function Eliminar(ByVal codigoEntidad As String, ByVal dataRequest As DataSet) As Boolean
        Dim oEntidadDAM As New EntidadDAM
        Try
            oEntidadDAM.Eliminar(codigoEntidad, dataRequest)
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Eliminar_Registro_Fisico(ByVal codigoEntidad As String, ByVal dataRequest As DataSet) As Boolean

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoEntidad, dataRequest}
        Dim oEntidadDAM As New EntidadDAM

        Try

            oEntidadDAM.Eliminar_Registro_Fisico(codigoEntidad, dataRequest)
            RegistrarAuditora(parameters)
            Return True

        Catch exsql As SqlClient.SqlException

            RegistrarAuditora(parameters, exsql)
            'Dim rethrow As Boolean = ExceptionPolicy.HandleException(exsql, "SITBLPolicy")
            'If (rethrow) Then
            '    Throw
            'End If

            Throw exsql

        Catch ex As Exception

            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If

        End Try


    End Function

    Public Function Seleccionar(ByVal codigoEntidad As String, ByVal dataRequest As DataSet) As EntidadBE

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoEntidad, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New EntidadDAM().Seleccionar(codigoEntidad)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function
    Public Function ExisteTercero(ByVal codigoEntidad As String, ByVal codigoTercero As String, ByVal FlagIngreso As String, ByVal dataRequest As DataSet) As EntidadBE

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoEntidad, codigoTercero, FlagIngreso, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New EntidadDAM().ExisteTercero(codigoEntidad, codigoTercero, FlagIngreso, dataRequest)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function ExisteCodigoSBS(ByVal codigoEntidad As String, ByVal codigoSBS As String, ByVal FlagIngreso As String, ByVal dataRequest As DataSet) As EntidadBE

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoEntidad, codigoSBS, FlagIngreso, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New EntidadDAM().ExisteCodigoSBS(codigoEntidad, codigoSBS, FlagIngreso, dataRequest)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As EntidadBE

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New EntidadDAM().Listar()
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function ListarCustodio(ByVal dataRequest As DataSet) As DataSet

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim oDS As DataSet
        Try
            oDS = New EntidadDAM().ListarCustodio()
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDS
    End Function

    Public Function ListarEntidadFinanciera(ByVal dataRequest As DataSet) As EntidadBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New EntidadDAM().ListarEntidadFinanciera(dataRequest)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function SeleccionarPorCodigoTercero(ByVal codigoTercero As String, ByVal dataRequest As DataSet) As DataSet
        Dim oDS As DataSet
        Try
            oDS = New EntidadDAM().SeleccionarPorCodigoTercero(codigoTercero)
        Catch ex As Exception
            Throw ex
        End Try
        Return oDS
    End Function

    Public Function SeleccionarPorReferenciaValores(ByVal codigoEntidad As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim oDS As DataSet
        Try
            RegistrarAuditora(parameters)
            oDS = New EntidadDAM().SeleccionarPorReferenciaValores(codigoEntidad)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDS
    End Function
#Region " /* Broker Comisiones (Req25) LETV 20090401 */ "
    Public Function ListarBroker(ByVal CodigoEntidad As String, ByVal Descripcion As String) As DataTable

        Dim odt As DataTable
        Try
            odt = New EntidadDAM().ListarBroker(CodigoEntidad, Descripcion)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return odt
    End Function

    Public Function ListarTramoBroker(ByVal CodigoEntidad As String, ByVal descripcion As String, ByVal situacion As String, ByVal tipotramo As String) As DataTable

        Dim odt As DataTable
        Try
            odt = New EntidadDAM().ListarTramoBroker(CodigoEntidad, descripcion, situacion, tipotramo)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return odt
    End Function

    Public Function ModificarTramoBroker(ByVal oEntidadBE As EntidadBE, ByVal dataRequest As DataSet) As Boolean

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oEntidadBE, dataRequest}
        Dim oEntidadDAM As New EntidadDAM

        Try

            oEntidadDAM.ModificarTramoBroker(oEntidadBE, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function InsertarTramoBroker(ByVal oEntidadBE As EntidadBE, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oEntidadBE, dataRequest}
        Dim oEntidadDAM As New EntidadDAM

        Try

            oEntidadDAM.InsertarTramoBroker(oEntidadBE, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function ExisteTramo(ByVal Tramo As String) As Boolean
        Try

            Return New EntidadDAM().ExisteTramo(Tramo)
        Catch ex As Exception

            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function SeleccionarTramoBroker(ByVal tramo As String) As DataTable
        Dim dt As DataTable
        Dim oEntidadDAM As New EntidadDAM

        Try
            dt = oEntidadDAM.SeleccionarTramoBroker(tramo)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If

        End Try

        Return dt
    End Function

    Public Function EliminarTramoBroker(ByVal Tramo As String, ByVal dataRequest As DataSet) As Boolean

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {Tramo, dataRequest}
        Dim oEntidadDAM As New EntidadDAM

        Try

            oEntidadDAM.EliminarTramoBroker(Tramo, dataRequest)
            RegistrarAuditora(parameters)
            Return True

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function SeleccionarCostoBroker(ByVal NroAcciones As Decimal, ByVal CodigoIntermediario As String, ByVal TipoTramo As String) As DataTable
        Dim dt As DataTable
        Dim oEntidadDAM As New EntidadDAM

        Try
            dt = oEntidadDAM.SeleccionarCostoBroker(NroAcciones, CodigoIntermediario, TipoTramo)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If

        End Try

        Return dt
    End Function

#End Region

#Region " /* Broker Excesos HDG OT 60022 20100707 */ "

    Public Function SeleccionarExcesosBroker(ByVal CodigoEntidad As String, ByVal descripcion As String, ByVal situacion As String) As DataTable

        Dim odt As DataTable
        Try
            odt = New EntidadDAM().SeleccionarExcesosBroker(CodigoEntidad, descripcion, situacion)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return odt
    End Function

    Public Function ModificarExcesosBroker(ByVal oEntidadExcesosBE As EntidadExcesosBE, ByVal dataRequest As DataSet) As Boolean

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oEntidadExcesosBE, dataRequest}
        Dim oEntidadDAM As New EntidadDAM

        Try

            oEntidadDAM.ModificarExcesosBroker(oEntidadExcesosBE, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function InsertarExcesosBroker(ByVal oEntidadExcesosBE As EntidadExcesosBE, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oEntidadExcesosBE, dataRequest}
        Dim oEntidadDAM As New EntidadDAM

        Try

            oEntidadDAM.InsertarExcesosBroker(oEntidadExcesosBE, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function EliminarExcesosBroker(ByVal CodigoEntidad As String, ByVal dataRequest As DataSet) As Boolean

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoEntidad, dataRequest}
        Dim oEntidadDAM As New EntidadDAM

        Try

            oEntidadDAM.EliminarExcesosBroker(CodigoEntidad, dataRequest)
            RegistrarAuditora(parameters)
            Return True

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

#End Region

#Region " /* Funciones Personalizadas*/"
    Public Sub Ingresar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
    Public Sub Actualizar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub

    Public Sub Salir(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
#End Region

End Class