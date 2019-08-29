Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


	Public class CotizacionVACBM
    Inherits InvokerCOM

		Public Sub New()

        End Sub


#Region " /* Funciones Seleccionar */ "

    Public Function SeleccionarPorFiltro(ByVal fecha As Decimal, ByVal codigoIndicador As String, ByVal dataRequest As DataSet) As CotizacionVACBE

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {fecha, codigoIndicador, dataRequest}
        Dim ds As New CotizacionVACBE
        Try

            ds = New CotizacionVACDAM().SeleccionarPorFiltro(fecha, codigoIndicador)
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
        Return ds
    End Function

    Public Function Seleccionar(ByVal fecha As Decimal, ByVal codigoIndicador As String, ByVal dataRequest As DataSet) As CotizacionVACBE

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {fecha, codigoIndicador, dataRequest}

        Try

            Return New CotizacionVACDAM().Seleccionar(fecha, codigoIndicador)
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


    End Function

    Public Function SeleccionarPorUltimaFecha(ByVal codigoIndicador As String, ByVal dataRequest As DataSet) As CotizacionVACBE

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoIndicador, dataRequest}

        Try

            Return New CotizacionVACDAM().SeleccionarPorUltimaFecha(codigoIndicador)
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


    End Function

    Public Function SeleccionarDetalle(ByVal codigoCotizacionVAC As String, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoCotizacionVAC, dataRequest}
        Try

            Return New CotizacionVACDAM().SeleccionarDetalle(codigoCotizacionVAC, dataRequest).Tables(0)
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
    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As CotizacionVACBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try

            Return New CotizacionVACDAM().Listar(dataRequest)
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
    End Function

#End Region

#Region " /* Funciones Insertar */ "

    Public Function Insertar(ByVal oCotizacionVACBE As CotizacionVACBE, ByVal dataRequest As DataSet) As String

        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oCotizacionVACBE, dataRequest}
        Try
            Dim oCotizacionVACDAM As New CotizacionVACDAM

            codigo = oCotizacionVACDAM.Insertar(oCotizacionVACBE, dataRequest)
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


    Public Function Modificar(ByVal CodigoIndicador As String, ByVal oCotizacionVACBE As CotizacionVACBE, ByVal dataRequest As DataSet) As Boolean

        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoIndicador, oCotizacionVACBE, dataRequest}
        Try
            Dim oCotizacionVACDAM As New CotizacionVACDAM

            actualizado = oCotizacionVACDAM.Modificar(CodigoIndicador, oCotizacionVACBE, dataRequest)
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

    Public Function Eliminar(ByVal fecha As Decimal, ByVal codigoIndicador As String, ByVal dataRequest As DataSet)

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {fecha, codigoIndicador, dataRequest}
        Try
            Dim oCotizacionVACDAM As New CotizacionVACDAM

            eliminado = oCotizacionVACDAM.Eliminar(fecha, codigoIndicador, dataRequest)
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

#Region " /* Funciones ActualizarporExcel */"

    'HDG OT 59304 20100419
    Public Function ActualizarCotizacionVACPorExcel(ByVal dtRating As DataTable, ByVal dataRequest As DataSet, ByRef strmensaje As String) As Boolean
        Dim Codigo = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dtRating, dataRequest}

        Try
            Dim oCotizacionVACDAM As New CotizacionVACDAM
            Codigo = oCotizacionVACDAM.ActualizarCotizacionVACPorExcel(dtRating, dataRequest, strmensaje)
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
    Public Function ObtenerCodigoCotizacionVAC(ByVal codigoMes As String, ByVal NumeroAnio As String, ByVal dataRequest As DataSet) As String
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoMes, NumeroAnio, dataRequest}
        Try

            Return New CotizacionVACDAM().ObtenerCodigoCotizacionVAC(codigoMes, NumeroAnio, dataRequest)
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


End Class

