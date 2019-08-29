Imports System
Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class CustodioBM
    Inherits InvokerCOM

    Public Sub New()

    End Sub

#Region "/* Funciones Seleccionar */"

    Public Function SeleccionarCuentasDepositaria(ByVal codigoCustodio As String, ByVal dataRequest As DataSet) As CustodioCuentaDepositariaBE

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}

        Dim oCuentas As CustodioCuentaDepositariaBE
        Dim oCustodioDAM As New CustodioDAM

        Try

            oCuentas = oCustodioDAM.SeleccionarCuentasDepositaria(codigoCustodio)
            RegistrarAuditora(parameters)

        Catch ex As Exception

            RegistrarAuditora(parameters, ex)

            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

        Return oCuentas

    End Function

    Public Function Seleccionar(ByVal codigoCustodio As String) As CustodioBE

    End Function

    Public Function SeleccionarCustodioArchivoPlano(ByVal codigoCustodio As String, ByVal dataRequest As DataSet) As DataSet
        Try
            Return New CustodioDAM().SeleccionarCustodioArchivoPlano(codigoCustodio)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As DataSet
        Try
            Return New CustodioDAM().Listar(dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ListarCuentasDepositariasPorCustodio(ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New CustodioDAM().ListarCuentasDepositariasPorCustodio(dataRequest)
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

    Public Function ListarTitulosAsociadosCustodios(ByVal sFechaSaldo As String, _
                                                    ByVal sCodigoPortafolioSBS As String, _
                                                    ByVal sCodigoISIN As String, _
                                                    ByVal sCodigoCustodio As String, _
                                                    ByVal sCodigoTipoTitulo As String, _
                                                    ByVal sTipoRenta As String, _
                                                    ByVal sCodigoMnemonico As String, _
                                                    ByVal sCodigoMoneda As String, _
                                                    ByVal sConsulta As String, _
                                                    ByVal dataRequest As DataSet) As DataSet

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New CustodioDAM().ListarTitulosAsociadosCustodios(sFechaSaldo, sCodigoPortafolioSBS, sCodigoISIN, _
                                                                    sCodigoCustodio, sCodigoTipoTitulo, sTipoRenta, _
                                                                    sCodigoMnemonico, sCodigoMoneda, sConsulta, _
                                                                    dataRequest)
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
    Public Function ListarTitulosAsociadosCustodiosC1(ByVal sFechaSaldo As String, _
                                                   ByVal sCodigoPortafolioSBS As String, _
                                                   ByVal sCodigoISIN As String, _
                                                   ByVal sCodigoCustodio As String, _
                                                   ByVal sCodigoTipoTitulo As String, _
                                                   ByVal sTipoRenta As String, _
                                                   ByVal sCodigoMnemonico As String, _
                                                   ByVal sCodigoMoneda As String, _
                                                   ByVal dataRequest As DataSet) As DataSet

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New CustodioDAM().ListarTitulosAsociadosCustodiosC1(sFechaSaldo, sCodigoPortafolioSBS, sCodigoISIN, _
                                                        sCodigoCustodio, sCodigoTipoTitulo, sTipoRenta, _
                                                        sCodigoMnemonico, sCodigoMoneda, dataRequest)
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
    Public Function ListarTitulosAsociadosCustodiosC2(ByVal sFechaSaldo As String, _
                                                   ByVal sCodigoPortafolioSBS As String, _
                                                   ByVal sCodigoISIN As String, _
                                                   ByVal sCodigoCustodio As String, _
                                                   ByVal sCodigoTipoTitulo As String, _
                                                   ByVal sTipoRenta As String, _
                                                   ByVal sCodigoMnemonico As String, _
                                                   ByVal sCodigoMoneda As String, _
                                                  ByVal dataRequest As DataSet) As DataSet

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New CustodioDAM().ListarTitulosAsociadosCustodiosC2(sFechaSaldo, sCodigoPortafolioSBS, sCodigoISIN, _
                                                                    sCodigoCustodio, sCodigoTipoTitulo, sTipoRenta, _
                                                                    sCodigoMnemonico, sCodigoMoneda, _
                                                                    dataRequest)
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

    Public Function SeleccionarPorFiltro(ByVal codigoCustodio As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As CustodioBE

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoCustodio, descripcion, situacion, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New CustodioDAM().SeleccionarPorFiltro(codigoCustodio, descripcion, situacion, dataRequest)
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
    Public Function Seleccionar1(ByVal codigoCustodio As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As CustodioBE

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoCustodio, descripcion, situacion, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New CustodioDAM().Seleccionar1(codigoCustodio, descripcion, situacion, dataRequest)
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

    'OT 10883 27/10/2017 Hanz Cocchi: Obtiene la lista de portafolios asociados a BBH
    Public Function ListarPortafoliosBBH() As ArrayList
        'Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Dim parameters As Object() = {dataRequest}

        Dim oCustodioDAM As New CustodioDAM
        Dim array As New ArrayList
        Try
            Dim dts As DataSet = oCustodioDAM.ListarPortafoliosBBH()

            If Not dts Is Nothing Then
                If dts.Tables.Count > 0 Then
                    For Each fila As DataRow In dts.Tables(0).Rows
                        array.Add(fila(0))
                    Next
                End If
            End If
            'RegistrarAuditora(parameters)

            Return array

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function
    'OT 10883 FIN
#End Region

    Public Function Insertar(ByVal oCustodio As CustodioBE, ByVal oCuentas As CustodioCuentaDepositariaBE, ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oCustodio, dataRequest}
        Try
            Dim oCustodioDAM As New CustodioDAM
            oCustodioDAM.Insertar(oCustodio, oCuentas, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Modificar(ByVal oCustodio As CustodioBE, ByVal oCuentas As CustodioCuentaDepositariaBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oCustodio, dataRequest}
        Try
            Dim oCustodioDAM As New CustodioDAM
            actualizado = oCustodioDAM.Modificar(oCustodio, oCuentas, dataRequest)
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


    Public Function Eliminar(ByVal codigoCustodio As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoCustodio, dataRequest}
        Try
            Dim oCustodioDAM As New CustodioDAM
            eliminado = oCustodioDAM.Eliminar(codigoCustodio, dataRequest)
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

    Public Sub Actualizar(ByVal dataRequest As DataSet)

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)

    End Sub

    Public Function VerificarCustodioCuentaDepositaria(ByVal strCodigoCustodio As String, ByVal strCodigoCuentaDepositaria As String, ByVal strCodigoPortafolioSBS As String) As Integer
        'Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoCustodio, strCodigoCuentaDepositaria, strCodigoPortafolioSBS}
        Dim LngContar As Long
        Try
            LngContar = New CustodioDAM().VerificarCustodioCuentaDepositaria(strCodigoCustodio, strCodigoCuentaDepositaria, strCodigoPortafolioSBS)
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
        Return LngContar
    End Function

    Public Function SeleccionarCustodioValores(ByVal dataRequest As DataSet, ByVal strNemonico As String, ByVal strCodigoPortafolioSBS As String, ByVal fecha As Decimal) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strNemonico, strCodigoPortafolioSBS, dataRequest}
        Dim oDS As New DataSet
        Try
            oDS = New CustodioDAM().SeleccionarCustodioValores(strNemonico, strCodigoPortafolioSBS, fecha)
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
        Return oDS
    End Function

    Public Sub ActualizarCustodioValores(ByVal strCustodio As String, ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {strCustodio, dataRequest}
        Dim strValores() As String = strCustodio.Split("&")
        Dim i As Integer
        Try
            Dim oCustodioDAM As CustodioDAM
            For i = 0 To strValores.Length - 1
                'oCustodioDAM = New CustodioDAM
                'strValores(i)
            Next
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
    End Sub

    Public Function SeleccionarSaldoDisponible(ByVal strNemonico As String, ByVal strCodigoPortafolioSBS As String, ByVal fechaEmision As Decimal) As Decimal

        Dim oDS As New Decimal
        Try
            oDS = New CustodioDAM().SeleccionarSaldoDisponible(strNemonico, strCodigoPortafolioSBS, fechaEmision)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception

            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDS
    End Function

    'HDG INC 64460	20120102
    Public Function SeleccionaSaldosCustodioxNemonico(ByVal Portafolio As String, ByVal codigoCustodio As String, ByVal codigoNemonico As String, ByVal fechaProceso As Decimal) As DataSet
        Dim parameters As Object() = {codigoCustodio}
        Try
            Return New CustodioDAM().SeleccionaSaldosCustodioxNemonico(Portafolio, codigoCustodio, codigoNemonico, fechaProceso)
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

    'HDG INC 64460	20120102
    Public Function ProcesarSaldosCustodioxMnemonico(ByVal Fondo As String, ByVal Custodio As String, ByVal Mnemonico As String, ByVal fechaProceso As Decimal, ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Dim oCustodioDAM As New CustodioDAM
            oCustodioDAM.ProcesarSaldosCustodioxMnemonico(Fondo, Custodio, Mnemonico, fechaProceso, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function Cuentadepositaria_Portafolio(ByVal CodigoPortafolioSBS As String) As DataTable
        Try
            Dim oCustodioDAM As New CustodioDAM
            Return oCustodioDAM.Cuentadepositaria_Portafolio(CodigoPortafolioSBS)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub Insertar_Cuentadepositaria_Portafolio(ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String, ByVal CuentaDepositarias As String, _
    ByVal dataRequest As DataSet)
        Try
            Dim oCustodioDAM As New CustodioDAM
            oCustodioDAM.Insertar_Cuentadepositaria_Portafolio(CodigoPortafolioSBS, CodigoCustodio, CuentaDepositarias, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'OT 10883 20/10/2017 Hanz Cocchi
    'Se agregan los parámetros CodigoCustodio y CuentaDepositarias eliminar correctamente los registros
    Public Sub Del_Cuentadepositaria_Portafolio(ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String, ByVal CuentaDepositarias As String)
        Try
            Dim oCustodioDAM As New CustodioDAM
            'OT 10883 20/10/2017 Hanz Cocchi
            oCustodioDAM.Del_Cuentadepositaria_Portafolio(CodigoPortafolioSBS, CodigoCustodio, CuentaDepositarias)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'OT 10883 FIN 
End Class