Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class FeriadoBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub
#Region " /* Funciones Seleccionar */ "

    Public Function Seleccionar(ByVal fechaFeriado As Integer, ByVal StrMercado As String, ByVal dataRequest As DataSet) As FeriadoBE

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {fechaFeriado, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New FeriadoDAM().Seleccionar(fechaFeriado, StrMercado, dataRequest)
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

    Public Function SeleccionarPorFiltro(ByVal anio As Decimal, ByVal situacion As String, ByVal dataRequest As DataSet) As FeriadoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {anio, situacion, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New FeriadoDAM().SeleccionarPorFiltro(anio, situacion, dataRequest)
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

    Public Function SeleccionarPorFiltro(ByVal fechaFeriado As Integer, ByVal numeroAnio As String, ByVal situacion As String, ByVal dataRequest As DataSet) As FeriadoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {fechaFeriado, numeroAnio, situacion, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New FeriadoDAM().SeleccionarPorFiltro(fechaFeriado, numeroAnio, situacion, dataRequest)
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

    Public Function Insertar(ByVal oFeriado As FeriadoBE, ByVal dataRequest As DataSet) As String
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oFeriado, dataRequest}
        Try
            Dim oFeriadoDAM As New FeriadoDAM
            oFeriadoDAM.Insertar(oFeriado, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function Modificar(ByVal oFeriado As FeriadoBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oFeriado, dataRequest}
        Try
            Dim oFeriadoDAM As New FeriadoDAM
            actualizado = oFeriadoDAM.Modificar(oFeriado, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return actualizado
    End Function

    Public Function Eliminar(ByVal codigoFeriado As String, ByVal Mercado As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoFeriado, Mercado, dataRequest}
        Try
            Dim oFeriadoDAM As New FeriadoDAM
            eliminado = oFeriadoDAM.Eliminar(codigoFeriado, Mercado, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado

    End Function

#Region " /* Funciones Eliminar */ "

    Public Function EliminarPorNumeroAnio(ByVal numeroAnio As String)

    End Function

    Public Function EliminarPorCodigoPais(ByVal codigoPais As Decimal)


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
#Region " /* Funciones Alberto */ "
    Public Function BuscarPorFecha(ByVal fecha As Decimal, Optional ByVal sInd As String = "") As Boolean   'HDG 20130730
        Dim blnexiste As Boolean
        Try
            Dim oFeriadoDAM As New FeriadoDAM
            blnexiste = oFeriadoDAM.BuscarPorFecha(fecha, sInd) 'HDG 20130730

        Catch ex As Exception

            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return blnexiste
    End Function
#End Region

    Public Function VerificaDia(ByVal fecha As Decimal, ByVal Mercado As String) As Boolean
        Dim _dia As Boolean
        Dim oFeriadoDAM As New FeriadoDAM
        Try
            _dia = oFeriadoDAM.VerificaDia(fecha, Mercado)

        Catch ex As Exception

            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return _dia
    End Function
#Region "Funciones nuevas 02/11/2018"
    Public Function Feriado_ValidarFecha(ByVal fecha As Decimal, ByVal Mercado As String) As Boolean
        Dim _dia As Boolean
        Dim oFeriadoDAM As New FeriadoDAM
        Try
            _dia = oFeriadoDAM.Feriado_ValidarFecha(fecha, Mercado)

        Catch ex As Exception

            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return _dia
    End Function
#End Region
End Class

