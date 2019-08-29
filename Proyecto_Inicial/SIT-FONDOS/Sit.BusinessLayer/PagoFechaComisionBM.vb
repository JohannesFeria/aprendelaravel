Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports System.Collections.Generic
Imports System.Transactions

Public Class PagoFechaComisionBM
    Public Function ListarPortafoliosCustodio(ByVal codigoFondo As String, ByVal fecha As Decimal) As DataTable
        Try
            Dim pagoFechaComisionDAM As New PagoFechaComisionDAM
            Return pagoFechaComisionDAM.ListarPortafoliosCustodio(codigoFondo, fecha)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ValidarExistenciaIngresados(ByVal codigoFondo As String) As DataTable
        Try
            Dim pagoFechaComisionDAM As New PagoFechaComisionDAM
            Return pagoFechaComisionDAM.ValidarExistenciaIngresados(codigoFondo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Seleccionar(ByVal id As Int32, ByVal codigoFondo As String, ByVal NumeroCuenta As String) As DataTable
        Try
            Dim pagoFechaComisionDAM As New PagoFechaComisionDAM
            Return pagoFechaComisionDAM.Seleccionar(id, codigoFondo, NumeroCuenta)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarPortafolios(ByVal fecha As Decimal) As DataTable
        Try
            Dim pagoFechaComisionDAM As New PagoFechaComisionDAM
            Return pagoFechaComisionDAM.ListarPortafolios(fecha)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function EliminarConfirmado(ByVal id As Int32, ByVal codigoFondo As String, ByVal dataRequest As DataSet) As String
        Try
            Dim pagoFechaComisionDAM As New PagoFechaComisionDAM
            Return pagoFechaComisionDAM.EliminarConfirmado(id, codigoFondo, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Insertar(ByVal listaPagoComisionBE As List(Of PagoFechaComisionBE), ByVal dataRequest As DataSet) As Boolean
        Try
            Dim pagoFechaComisionDAM As New PagoFechaComisionDAM
            Dim i As Integer
            For i = 0 To listaPagoComisionBE.Count - 1
                pagoFechaComisionDAM.Insertar(listaPagoComisionBE(i), dataRequest)
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Actualizar(ByVal listaPagoComisionBE As List(Of PagoFechaComisionBE), ByVal dataRequest As DataSet) As Boolean
        Try
            Dim pagoFechaComisionDAM As New PagoFechaComisionDAM
            Dim i As Integer
            For i = 0 To listaPagoComisionBE.Count - 1
                pagoFechaComisionDAM.Actualizar(listaPagoComisionBE(i), dataRequest)
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ActualizarEstado(ByVal listaPagoComisionBE As List(Of PagoFechaComisionBE), ByVal dataRequest As DataSet) As Boolean
        Try
            Dim pagoFechaComisionDAM As New PagoFechaComisionDAM
            Dim i As Integer
            For i = 0 To listaPagoComisionBE.Count - 1
                pagoFechaComisionDAM.ActualizarEstado(listaPagoComisionBE(i), dataRequest)
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Confirmar(ByVal listaPagoComisionBE As List(Of PagoFechaComisionBE), ByVal dataRequest As DataSet) As Boolean
        Try
            Dim pagoFechaComisionDAM As New PagoFechaComisionDAM
            Dim operacionesCajaDAM As New OperacionesCajaDAM
            Dim i As Integer

            Using transaction As New TransactionScope
                For i = 0 To listaPagoComisionBE.Count - 1
                    Dim dsOpCaja As New OperacionCajaBE
                    Dim opCaja As OperacionCajaBE.OperacionCajaRow = dsOpCaja.OperacionCaja.NewOperacionCajaRow()


                    opCaja.CodigoMercado = "1"
                    opCaja.CodigoPortafolioSBS = listaPagoComisionBE(i).CodigoPortafolioSBS
                    opCaja.CodigoClaseCuenta = "20"
                    opCaja.NumeroCuenta = listaPagoComisionBE(i).NumeroCuenta
                    opCaja.CodigoModalidadPago = "CPAG"
                    opCaja.CodigoTerceroDestino = ""
                    opCaja.CodigoTerceroOrigen = listaPagoComisionBE(i).CodigoBanco ' Cambiar
                    opCaja.NumeroCuentaDestino = ""
                    opCaja.CodigoOperacion = "CSAF"
                    opCaja.Referencia = ""
                    opCaja.CodigoMoneda = listaPagoComisionBE(i).CodigoMoneda
                    opCaja.Importe = listaPagoComisionBE(i).ComisionAcumulada
                    opCaja.CodigoModelo = "SC01"
                    opCaja.CodigoOperacionCaja = ""
                    opCaja.PagoFechaComision = listaPagoComisionBE(i).Id
                    opCaja.FechaPago = listaPagoComisionBE(i).FechaPago
                    dsOpCaja.OperacionCaja.AddOperacionCajaRow(opCaja)
                    pagoFechaComisionDAM.Actualizar(listaPagoComisionBE(i), dataRequest)
                    operacionesCajaDAM.Insertar_FechaOperacion(dsOpCaja, dataRequest)
                Next
                transaction.Complete()
            End Using

            


        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Eliminar(ByVal identificador As Integer) As Boolean
        Try
            Dim pagoFechaComisionDAM As New PagoFechaComisionDAM
            Return pagoFechaComisionDAM.Eliminar(identificador)

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ListarBancos(ByVal codigoFondo As String, ByVal codigoMoneda As String) As DataTable
        Try
            Dim pagoFechaComisionDAM As New PagoFechaComisionDAM
            Return pagoFechaComisionDAM.ListarBancos(codigoFondo, codigoMoneda)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarNumeroDeCuentas(ByVal codigoFondo As String, ByVal codigoMoneda As String, ByVal codigoBanco As String) As DataTable
        Try
            Dim pagoFechaComisionDAM As New PagoFechaComisionDAM
            Return pagoFechaComisionDAM.ListarNumeroDeCuentas(codigoFondo, codigoMoneda, codigoBanco)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ObtenerFechaComision(ByVal fechaOperacion As Decimal, ByVal codigoFondo As String) As DataTable
        Try
            Dim pagoFechaComisionDAM As New PagoFechaComisionDAM
            Return pagoFechaComisionDAM.ObtenerFechaComision(fechaOperacion, codigoFondo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function


End Class
