Imports System.Data
Imports MotorTransaccionesProxy
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class CuentaEconomicaBM
    Inherits InvokerCOM
    Public Sub New()
    End Sub
#Region " /* Funciones Seleccionar */ "
    Public Function SeleccionarPorFiltroMant(ByVal codigoPortafolio As String, ByVal CodigoClaseCuenta As String, ByVal entidadFinanciera As String, _
    ByVal CodigoMoneda As String, ByVal CodigoMercado As String, ByVal NumeroCuenta As String, ByVal dataRequest As DataSet) As CuentaEconomicaBE
        Try
            Return New CuentaEconomicaDAM().SeleccionarPorFiltroMant(codigoPortafolio, CodigoClaseCuenta, entidadFinanciera, CodigoMoneda, CodigoMercado, _
            NumeroCuenta, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarPorFiltro(ByVal codigoPortafolio As String, ByVal codigoClaseCuenta As String, ByVal codigoTercero As String, ByVal codigoMoneda As String, _
    ByVal codigoMercado As String, ByVal dataRequest As DataSet) As CuentaEconomicaBE
        Try
            Return New CuentaEconomicaDAM().SeleccionarPorFiltro(codigoPortafolio, codigoClaseCuenta, codigoTercero, codigoMoneda, codigoMercado)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function SeleccionarPorFiltro_CAD(ByVal codigoPortafolio As String, ByVal codigoClaseCuenta As String, ByVal codigoTercero As String, _
    ByVal codigoMercado As String) As CuentaEconomicaBE
        Try
            Return New CuentaEconomicaDAM().SeleccionarPorFiltro_CAD(codigoPortafolio, codigoClaseCuenta, codigoTercero, codigoMercado)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarPorFiltro() As DataTable
        Try
            Return New CuentaEconomicaDAM().SeleccionarPorFiltro()
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function SeleccionarPorFiltro2(ByVal codigoPortafolio As String, ByVal codigoTercero As String, ByVal codigoClaseCuenta As String, ByVal codigoMoneda As String, _
    ByVal CodigoMercado As String, ByVal dataRequest As DataSet) As DataTable
        Try
            Return New CuentaEconomicaDAM().SeleccionarPorFiltro2(codigoPortafolio, codigoTercero, codigoClaseCuenta, codigoMoneda, CodigoMercado, dataRequest)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function SeleccionarPorFiltro(ByVal codigoCuentaEconomica As String, ByVal codigoTercero As String, ByVal codigoMoneda As String, _
    ByVal codigoClaseCuenta As String, ByVal codigoPortafolio As String, ByVal situacion As String, ByVal dataRequest As DataSet) As CuentaEconomicaBE
        Try
            Return New CuentaEconomicaDAM().SeleccionarPorFiltro(codigoCuentaEconomica, codigoTercero, codigoMoneda, codigoClaseCuenta, codigoPortafolio, _
            situacion, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Seleccionar(ByVal codigoCuentaEconomica As String, ByVal dataRequest As DataSet) As CuentaEconomicaBE
        Try
            Return New CuentaEconomicaDAM().Seleccionar(codigoCuentaEconomica, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function EncontrarCuentasExistentes(ByVal strEntidadFinanciera As String, ByVal dataRequest As DataSet) As DataSet
        Try
            Return New CuentaEconomicaDAM().EncontrarCuentasExistentes(strEntidadFinanciera)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Listar(ByVal dataRequest As DataSet) As SectorEmpresarialBE
        Try
            Return New SectorEmpresarialDAM().Listar(dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarPorCodigoTercero(ByVal CodigoTercero As String, ByVal CodigoBanco As String, ByVal CodigoMercado As String, _
    ByVal CodigoMoneda As String) As DataSet
        Try
            Return New CuentaEconomicaDAM().SeleccionarPorCodigoTercero(CodigoTercero, CodigoBanco, CodigoMercado, CodigoMoneda)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarCuentaEconomica(ByVal dataRequest As DataSet) As DataTable
        Try
            Return New CuentaEconomicaDAM().Listar(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarCuentaEconomica(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal,
     ByVal CodigoMoneda As String, ByVal CodBanco As String, ByVal CodigoMercado As String, ByVal CodigoClasecuenta As String) As DataTable
        Try
            Return New CuentaEconomicaDAM().SeleccionarCuentaEconomica(CodigoPortafolioSBS, FechaOperacion, CodigoMoneda,
            CodBanco, CodigoMercado, CodigoClasecuenta)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarCuentaEconomica_Movimientos(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal, ByVal NumeroCuenta As String) As DataTable
        Try
            Return New CuentaEconomicaDAM().SeleccionarCuentaEconomica_Movimientos(CodigoPortafolioSBS, FechaOperacion, NumeroCuenta)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ObtenerCuentaContable(ByVal CodigoEntidad As String, ByVal CodigoMoneda As String) As String
        Try
            Return New CuentaEconomicaDAM().ObtenerCuentaContable(CodigoEntidad, CodigoMoneda)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ObtenerSituacion(ByVal CodigoPortafolioSBS As String, ByVal CuentaContable As String, ByVal EntidadFinanciera As String, _
    ByVal CodigoMoneda As String) As String
        Try
            Return New CuentaEconomicaDAM().ObtenerSituacion(CodigoPortafolioSBS, CuentaContable, EntidadFinanciera, CodigoMoneda)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    ' Inicio de Cambio OT-10795
    Public Function ObtenerSituacionNuevo(ByVal CodigoPortafolioSBS As String, ByVal CuentaContable As String, ByVal EntidadFinanciera As String, _
    ByVal CodigoMoneda As String, ByVal NuevoNroCuenta As String) As String
        Try
            Return New CuentaEconomicaDAM().ObtenerSituacionNuevo(CodigoPortafolioSBS, CuentaContable, EntidadFinanciera, CodigoMoneda, NuevoNroCuenta)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    ' Fin de Cambio OT-10795
#End Region

#Region " /* Funciones Insertar */ "
    Public Function Insertar(ByVal oCuentaEconomica As CuentaEconomicaBE, ByVal dataRequest As DataSet) As String
        Try
            Return New CuentaEconomicaDAM().Insertar(oCuentaEconomica, dataRequest) ' Modificación por OT-10795
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region

#Region " /* Funciones CambiarCuenta */ "
    Public Function CambiarCuenta(ByVal oCuentaEconomica As CuentaEconomicaBE, ByVal dataRequest As DataSet) As String
        Try
            Dim oCuentaEconomicaDAM As New CuentaEconomicaDAM
            Return oCuentaEconomicaDAM.CambiarCuenta(oCuentaEconomica, dataRequest) 'Modificación por OT-10795
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region

#Region " /* Funciones Modificar */ "
    Public Function Modificar(ByVal oCuentaEconomica As CuentaEconomicaBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Try
            Dim oCuentaEconomicaDAM As New CuentaEconomicaDAM
            actualizado = oCuentaEconomicaDAM.Modificar(oCuentaEconomica, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return actualizado
    End Function

    Public Function Modificar(ByVal oCuentaEconomica As CuentaEconomicaBE, ByVal p_CodigoPortafolioSBS As String, ByVal p_CuentaContable As String, ByVal p_NumeroCuenta As String, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Try
            Dim oCuentaEconomicaDAM As New CuentaEconomicaDAM
            actualizado = oCuentaEconomicaDAM.Modificar(oCuentaEconomica, p_CodigoPortafolioSBS, p_CuentaContable, p_NumeroCuenta, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return actualizado
    End Function

#End Region

#Region " /* Funciones Eliminar */ "
    Public Function Eliminar(ByVal codigoSectorEmpresarial As Integer, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Try
            Dim oSectorEmpresarialDAM As New SectorEmpresarialDAM
            eliminado = oSectorEmpresarialDAM.Eliminar(codigoSectorEmpresarial, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return eliminado
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

    Public Function Eliminar(ByVal codigoPortafolio As String, ByVal cuentaContable As String, ByVal NumeroCuenta As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Try
            Dim oCuentaEconomicaDAM As New CuentaEconomicaDAM
            eliminado = oCuentaEconomicaDAM.Eliminar(codigoPortafolio, cuentaContable, NumeroCuenta, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return eliminado
    End Function
    'OT 10328 - 28/04/2017 - Carlos Espejo
    'Descripcion: Lista el reporte de comisiones - Recaudo
    Public Function ReporteComisionesRecaudo(ByVal CodigoPortafolio As String, ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal) As DataTable
        Try
            Dim oCuentaEconomicaDAM As New CuentaEconomicaDAM
            Return oCuentaEconomicaDAM.ReporteComisionesRecaudo(CodigoPortafolio, FechaInicio, FechaFin)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'OT11237 - 22/02/2018 - Ian Pastor M.
    'Descripción: Obtener los rescates preliminares de un fondo, especificando su fecha y su entidad financiera
    Public Function ObtenerRescatesPreliminaresSisOpe(ByVal p_CodigoPortafolio As String, ByVal p_FechaOperacion As String, _
                                                      ByVal p_CodigoTercero As String, ByVal p_CodigoMonedaCuenta As String, _
                                                      ByVal p_dataRequest As DataSet) As Decimal
        Try
            ObtenerRescatesPreliminaresSisOpe = 0
            Dim oCuentaEconomicaDAM As New CuentaEconomicaDAM
            'Dim oEntidadDAM As New EntidadDAM
            Dim objPortafolioBM As New PortafolioBM
            Dim objValorCuotaDAM As New ValorCuotaDAM

            'Obtener propiedas del portafolio
            Dim objPortafolioBE As PortafolioBE
            Dim objPortafolioRow As PortafolioBE.PortafolioRow
            objPortafolioBE = objPortafolioBM.Seleccionar(p_CodigoPortafolio, p_dataRequest)
            objPortafolioRow = CType(objPortafolioBE.Portafolio.Rows(0), PortafolioBE.PortafolioRow)

            If objPortafolioRow.CodigoMoneda <> p_CodigoMonedaCuenta Then
                Exit Function
            End If

            'Obtener el último valor cuota
            Dim valorCuota As Decimal = 0.0
            Dim dtValorCuota As DataTable = objValorCuotaDAM.ValorCuota_ObtenerUltimoValorCuota(p_CodigoPortafolio, "")
            If dtValorCuota.Rows.Count > 0 Then
                valorCuota = Decimal.Parse(dtValorCuota.Rows(0)("ValCuotaValoresCierre"))
            End If

            'Obtener código de entidad
            'Dim codigoEntidad As String = oEntidadDAM.SeleccionarPorCodigoTercero(p_CodigoTercero).Tables(0).Rows(0)(0)
            'Obtener vía de pagos
            Dim drViaPago() As DataRow = oCuentaEconomicaDAM.ObtenerViaPagoSisOpe("VIAPAGRES"). _
                Select("DESCRIPCION_CORTA='" & p_CodigoTercero & "'")

            'Verificar si el portafolio es seriado
            Dim rescatePreliminar As Decimal = 0
            If objPortafolioRow.CodigoPortafolioSisOpe <> "" Then
                If objPortafolioRow.PorSerie = "N" Then
                    For Each dtRowViapago As DataRow In drViaPago
                        Dim dtRescates As DataTable = oCuentaEconomicaDAM.ObtenerRescatesPreliminaresSisOpe(objPortafolioRow.CodigoPortafolioSisOpe, _
                                                                                                            p_FechaOperacion, _
                                                                                                            dtRowViapago("LLAVE_TABLA").ToString())
                        For Each dtRowRescates As DataRow In dtRescates.Rows
                            If dtRowRescates("FLAG_RESCATE_EN_CUOTAS") = "N" Then
                                'rescatePreliminar = rescatePreliminar + Decimal.Parse(dtRowRescates("MONTO_NETO"))
                                rescatePreliminar = rescatePreliminar + Decimal.Parse(dtRowRescates("MONTO"))
                            Else
                                rescatePreliminar = rescatePreliminar + (Decimal.Parse(dtRowRescates("NUMERO_CUOTAS")) * valorCuota)
                            End If
                        Next
                    Next
                Else
                    Dim DtValoresSerie As DataTable
                    DtValoresSerie = objPortafolioBM.PortafolioCodigoListar_ValoresSerie(p_CodigoPortafolio)
                    For Each dtRowPortafolioSisOpe As DataRow In DtValoresSerie.Rows
                        For Each dtRowViapago As DataRow In drViaPago
                            Dim dtRescates As DataTable = oCuentaEconomicaDAM.ObtenerRescatesPreliminaresSisOpe(dtRowPortafolioSisOpe("CodigoPortafolioSO"), _
                                                                                                            p_FechaOperacion, _
                                                                                                            dtRowViapago("LLAVE_TABLA").ToString())
                            For Each dtRowRescates As DataRow In dtRescates.Rows
                                If dtRowRescates("FLAG_RESCATE_EN_CUOTAS") = "N" Then
                                    'rescatePreliminar = rescatePreliminar + Decimal.Parse(dtRowRescates("MONTO_NETO"))
                                    rescatePreliminar = rescatePreliminar + Decimal.Parse(dtRowRescates("MONTO"))
                                Else
                                    rescatePreliminar = rescatePreliminar + (Decimal.Parse(dtRowRescates("NUMERO_CUOTAS")) * valorCuota)
                                End If

                            Next
                        Next
                    Next
                End If
            End If
            ObtenerRescatesPreliminaresSisOpe = rescatePreliminar * (-1)
        Catch ex As Exception
            'Throw ex
            ObtenerRescatesPreliminaresSisOpe = 0.0
        End Try
    End Function
    'OT11237 - Fin

End Class