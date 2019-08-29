Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class VaxInterfazBM
    Inherits InvokerCOM

    Public Sub New()

    End Sub



    Public Function SeleccionarVaxCustman(ByVal cartera As String, ByVal decFechaVax As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {cartera}
        Dim oDSVaxCustman As New DataSet
        Try
            oDSVaxCustman = New VaxInterfazDAM().SeleccionarVaxCustman(cartera, decFechaVax, dataRequest)
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
        Return oDSVaxCustman
    End Function



    Public Function SeleccionarVaxDvdos(ByVal cartera As String, ByVal fechaProceso As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {cartera}
        Dim oDSVaxDvdos As New DataSet
        Try
            oDSVaxDvdos = New VaxInterfazDAM().SeleccionarVaxDvdos(cartera, fechaProceso, dataRequest)
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
        Return oDSVaxDvdos
    End Function



    Public Function SeleccionarVaxInfodia(ByVal cartera As String, ByVal fechaProceso As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {cartera}
        Dim oDSVaxInfodia As New DataSet
        Try
            oDSVaxInfodia = New VaxInterfazDAM().SeleccionarVaxInfodia(cartera, fechaProceso, dataRequest)
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
        Return oDSVaxInfodia
    End Function



    Public Function SeleccionarVaxTransac(ByVal cartera As String, ByVal fechaProceso As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {cartera}
        Dim oDSVaxTransac As New DataSet
        Try
            oDSVaxTransac = New VaxInterfazDAM().SeleccionarVaxTransac(cartera, fechaProceso, dataRequest)
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
        Return oDSVaxTransac
    End Function



    Public Function SeleccionarVaxComision(ByVal cartera As String, ByVal fechaProceso As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {cartera}
        Dim oDSVaxComision As New DataSet
        Try
            oDSVaxComision = New VaxInterfazDAM().SeleccionarVaxComision(cartera, fechaProceso, dataRequest)
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
        Return oDSVaxComision
    End Function



    Public Function SeleccionarVaxAuxbcr(ByVal cartera As String, ByVal fechaProceso As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {cartera}
        Dim oDSVaxAuxbcr As New DataSet
        Try
            oDSVaxAuxbcr = New VaxInterfazDAM().SeleccionarVaxAuxbcr(cartera, fechaProceso, dataRequest)
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
        Return oDSVaxAuxbcr
    End Function


    '-------------------------------  Interfaz LBTR -------------------------------------

    Public Function Seleccionar_LBTR(ByVal cartera As String, ByVal fechaProceso As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {cartera}
        Dim dsConsulta As New DataSet
        Try
            dsConsulta = New VaxInterfazDAM().Seleccionar_LBTR(cartera, fechaProceso, dataRequest)
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
        Return dsConsulta
    End Function


    '-------------------------------  Interfaz Cuentas Personales  -------------------------------------

    Public Function Seleccionar_CuentasPersonales(ByVal cartera As String, ByVal fechaProceso As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {cartera}
        Dim dsConsulta As New DataSet
        Try
            dsConsulta = New VaxInterfazDAM().Seleccionar_CuentasPersonales(cartera, fechaProceso, dataRequest)
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
        Return dsConsulta
    End Function


End Class

