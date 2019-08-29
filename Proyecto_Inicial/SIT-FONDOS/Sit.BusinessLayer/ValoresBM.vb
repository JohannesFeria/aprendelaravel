Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy
Imports System.Collections.Generic
Public Class ValoresBM
    Inherits InvokerCOM
    Public Sub New()
    End Sub
#Region " /* Funciones Seleccionar */ "
    Public Function SeleccionarDetalleCustodios(ByVal codigoMnemónico As String, ByVal dataRequest As DataSet) As DataTable
        Dim oTable As New DataTable
        Try
            oTable = New ValoresDAM().SeleccionarDetalleCustodios(codigoMnemónico, dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
        Return oTable
    End Function
    Public Function SeleccionarDetalleCapitalCompro(ByVal codigoMnemónico As String, ByVal dataRequest As DataSet) As DataTable
        Dim oTable As New DataTable
        Try
            oTable = New ValoresDAM().SeleccionarDetalleCapitalCompro(codigoMnemónico, dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
        Return oTable
    End Function
    Public Function ValidarFueNemonicoTemporal(ByVal codigoMnemónico As String, ByVal dataRequest As DataSet) As Boolean
        Dim result As Boolean
        Try
            result = New ValoresDAM().ValidarFueNemonicoTemporal(codigoMnemónico)
        Catch ex As Exception
            Throw ex
        End Try
        Return result
    End Function
    Public Function Seleccionar(ByVal codigoValores As String, ByVal dataRequest As DataSet) As ValoresBE
        Dim oValoresBE As New ValoresBE
        Try
            oValoresBE = New ValoresDAM().Seleccionar(codigoValores, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return oValoresBE
    End Function
    Public Function ValidarExistenciaValor(ByVal codigoMnemonico As String, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoMnemonico, dataRequest}
        Dim oresult As Boolean = False
        Try
            oresult = New ValoresDAM().ValidarExistenciaValor(codigoMnemonico)
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
        Return oresult
    End Function
    Public Function ValidarExistenciaValorModificar(ByVal codigoMnemonico As String, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoMnemonico, dataRequest}
        Dim oresult As Boolean = False
        Try
            oresult = New ValoresDAM().ValidarExistenciaValorModificar(codigoMnemonico)
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
        Return oresult
    End Function
    Public Function ValidarExistenciaValorISIN(ByVal codigoISIN As String, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoISIN, dataRequest}
        Dim oresult As Boolean = False
        Try
            oresult = New ValoresDAM().ValidarExistenciaValorISIN(codigoISIN)
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
        Return oresult
    End Function
    Public Function ValidarExistenciaValorISINModificar(ByVal codigoISIN As String, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoISIN, dataRequest}
        Dim oresult As Boolean = False
        Try
            oresult = New ValoresDAM().ValidarExistenciaValorISINModificar(codigoISIN)
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
        Return oresult
    End Function
    Public Function ValidarExistenciaValorSBS(ByVal codigoSBS As String, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoSBS, dataRequest}
        Dim oresult As Boolean = False
        Try
            oresult = New ValoresDAM().ValidarExistenciaValorSBS(codigoSBS)
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
        Return oresult
    End Function
    Public Function ValidarExistenciaValorSBSModificar(ByVal codigoSBS As String, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoSBS, dataRequest}
        Dim oresult As Boolean = False
        Try
            oresult = New ValoresDAM().ValidarExistenciaValorSBSModificar(codigoSBS)
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
        Return oresult
    End Function
    Public Function VerificarCodigoPortafolioSBS(ByVal codigoPortafolioSBS As String) As String
        Dim oresult As String = String.Empty
        Try
            oresult = New ValoresDAM().VerificarCodigoPortafolioSBS(codigoPortafolioSBS)
        Catch ex As Exception
            Throw ex
        End Try
        Return oresult
    End Function
    Public Function BuscarCustodioValor(ByVal codigoMnemonico As String, ByVal codigoPortafolioSBS As String, ByVal codigoCustodio As String,
    ByVal dataRequest As DataSet) As Boolean
        Dim existe As Boolean
        Try
            existe = New ValoresDAM().BuscarCustodioValor(codigoMnemonico, codigoPortafolioSBS, codigoCustodio)
        Catch ex As Exception
            Throw ex
        End Try
        Return existe
    End Function
    Public Function SeleccionarBono(ByVal codigoNemonico As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoNemonico, dataRequest}
        Dim oDSValores As New DataSet
        Try
            oDSValores = New ValoresDAM().SeleccionarBono(codigoNemonico, dataRequest)
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
        Return oDSValores
    End Function
    Public Function SeleccionarCodigoMnemonicoPorCodigoSBS(ByVal codigoSBS As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoSBS, dataRequest}
        Dim oDSValores As New DataSet
        Try
            oDSValores = New ValoresDAM().SeleccionarCodigoMnemonicoPorCodigoSBS(codigoSBS, dataRequest)
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
        Return oDSValores
    End Function
    Public Function SeleccionarInstrumentoPorCodigoSBS(ByVal CodigoSBS As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoSBS, dataRequest}
        Dim oDSValores As New DataSet
        Try
            oDSValores = New ValoresDAM().SeleccionarInstrumentoPorCodigoSBS(CodigoSBS, dataRequest)
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
        Return oDSValores
    End Function
    Public Function SeleccionarInstrumento(ByVal CodigoISIN As String, ByVal CodigoNemonico As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoISIN, CodigoNemonico, dataRequest}
        Dim oDSValores As New DataSet
        Try
            oDSValores = New ValoresDAM().SeleccionarInstrumento(CodigoISIN, CodigoNemonico, dataRequest)
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
        Return oDSValores
    End Function
    Public Function RecuperaSaldoTransferencia(ByVal CodigoSBS As String, ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String,
    ByVal FechaOperacion As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoSBS, CodigoPortafolioSBS, dataRequest}
        Dim oDSValores As New DataSet
        Try
            oDSValores = New ValoresDAM().RecuperaSaldoTransferencia(CodigoSBS, CodigoPortafolioSBS, CodigoCustodio, FechaOperacion)
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
        Return oDSValores
    End Function
    Public Function RecuperaSaldosInstrumento(ByVal CodigoSBS As String, ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String,
    ByVal FechaOperacion As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoSBS, CodigoPortafolioSBS, dataRequest}
        Dim oDSValores As New DataSet
        Try
            oDSValores = New ValoresDAM().RecuperaSaldosInstrumento(CodigoSBS, CodigoPortafolioSBS, CodigoCustodio, FechaOperacion)
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
        Return oDSValores
    End Function
    Public Function VerificaRelacionInstrumentoCustodio(ByVal CodigoMnemonico As String, ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String,
    ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoMnemonico, CodigoPortafolioSBS, dataRequest}
        Dim oDSValores As New DataSet
        Try
            oDSValores = New ValoresDAM().VerificaRelacionInstrumentoCustodio(CodigoMnemonico, CodigoPortafolioSBS, CodigoCustodio)
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
        Return oDSValores
    End Function
    Public Function CarteraTituloVerifica(ByVal CodigoSBS As String, ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String,
    ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoSBS, CodigoPortafolioSBS, dataRequest}
        Dim oDSValores As New DataSet
        Try
            oDSValores = New ValoresDAM().CarteraTituloVerifica(CodigoSBS, CodigoPortafolioSBS, CodigoCustodio, dataRequest)
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
        Return oDSValores
    End Function
    Public Function SeleccionarPagare(ByVal codigoNemonico As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoNemonico, dataRequest}
        Dim oDSValores As New DataSet
        Try
            oDSValores = New ValoresDAM().SeleccionarPagare(codigoNemonico, dataRequest)
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
        Return oDSValores
    End Function
    Public Function Listar(ByVal dataRequest As DataSet) As ValoresBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim oValoresBE As ValoresBE
        Try
            oValoresBE = New ValoresDAM().Listar(dataRequest)
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
        Return oValoresBE
    End Function
    Public Function InstrumentosBuscarPorFiltro(ByVal strSBS As String, ByVal strISIN As String, ByVal strMnemonico As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim oDS As New DataSet
        Try
            oDS = New ValoresDAM().InstrumentosBuscarPorFiltro(strSBS, strISIN, strMnemonico, dataRequest)
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
        Return oDS
    End Function
    Public Function InstrumentosBuscarPorFiltroPreorden(ByVal strSBS As String, ByVal strISIN As String, ByVal strMnemonico As String,
    ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim oDS As DataSet
        Try
            oDS = New ValoresDAM().InstrumentosBuscarPorFiltroPreorden(strSBS, strISIN, strMnemonico, dataRequest)
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
        Return oDS
    End Function
    Public Function InstrumentosBuscarPorFiltroConsultarOrdenesPreordenes(ByVal strISIN As String, ByVal strSBS As String, ByVal strMnemonico As String,
    ByVal correlativo As String, ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal, ByVal TipoOperacion As String, ByVal TipoInstrumento As String,
    ByVal TipoRenta As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strSBS, strISIN, strMnemonico, correlativo, FechaInicio, FechaFin, TipoOperacion, TipoInstrumento, TipoRenta, dataRequest}
        Dim oDS As DataSet
        Try
            oDS = New ValoresDAM().InstrumentosBuscarPorFiltroConsultarOrdenesPreordenes(strISIN, strSBS, strMnemonico, correlativo, FechaInicio, FechaFin,
            TipoOperacion, TipoInstrumento, TipoRenta, dataRequest)
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
        Return oDS
    End Function
    Public Function InstrumentosBuscarPorFiltroKardex(ByVal TipoInstrumento As String, ByVal strISIN As String, ByVal strMnemonico As String,
    ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim oDS As DataSet
        Try
            oDS = New ValoresDAM().InstrumentosBuscarPorFiltroKardex(TipoInstrumento, strISIN, strMnemonico, dataRequest)
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
        Return oDS
    End Function
    Public Function ListarPorFiltro(ByVal dataRequest As DataSet, ByVal strCategoriaInstrumento As String, ByVal strSBS As String, ByVal strISIN As String,
    ByVal strMnemonico As String, ByVal strFondo As String, ByVal strCodigoTipoInstrumentoSBS As String, ByVal operacion As String) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim oDS As DataSet
        Try
            oDS = New ValoresDAM().ListarPorFiltro(dataRequest, strCategoriaInstrumento, strSBS, strISIN, strMnemonico, strFondo, strCodigoTipoInstrumentoSBS, operacion)
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
        Return oDS
    End Function
    Public Function ListarPorTipoInstrumentoSBS(ByVal dataRequest As DataSet, ByVal strTipoInstSBS As String) As ValoresBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest, strTipoInstSBS}
        Dim oValoresBE As ValoresBE
        Try
            oValoresBE = New ValoresDAM().ListarPorTipoInstrumentoSBS(dataRequest, strTipoInstSBS)
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
        Return oValoresBE
    End Function
    Public Function SeleccionarPorFiltro(ByVal StrCodigoSBS As String, ByVal StrCodigoIsin As String, ByVal StrCodigoMnemonico As String, ByVal StrMoneda As String,
    ByVal StrTipoRenta As String, ByVal StrEmisor As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {StrCodigoSBS, StrCodigoIsin, StrCodigoMnemonico, StrTipoRenta, StrMoneda, StrEmisor, dataRequest}
        Dim oDS As New DataSet
        Try
            oDS = New ValoresDAM().SeleccionarPorFiltro(StrCodigoSBS, StrCodigoIsin, StrCodigoMnemonico, StrMoneda, StrTipoRenta, StrEmisor, dataRequest)
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
        Return oDS
    End Function
    Public Function SeleccionarPorFiltro2(ByVal StrCodigoSBS As String, ByVal StrCodigoIsin As String, ByVal StrCodigoMnemonico As String, ByVal StrMoneda As String,
    ByVal StrTipoRenta As String, ByVal StrEmisor As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {StrCodigoSBS, StrCodigoIsin, StrCodigoMnemonico, StrTipoRenta, StrMoneda, StrEmisor, dataRequest}
        Dim oDS As New DataSet
        Try
            oDS = New ValoresDAM().SeleccionarPorFiltro2(StrCodigoSBS, StrCodigoIsin, StrCodigoMnemonico, StrMoneda, StrTipoRenta, StrEmisor, dataRequest)
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
        Return oDS
    End Function
    Public Function SeleccionarPorFiltro3(ByVal StrCodigoSBS As String, ByVal StrCodigoIsin As String, ByVal StrCodigoMnemonico As String, ByVal StrMoneda As String,
    ByVal StrTipoRenta As String, ByVal StrEmisor As String, ByVal StrEstado As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {StrCodigoSBS, StrCodigoIsin, StrCodigoMnemonico, StrTipoRenta, StrMoneda, StrEmisor, dataRequest}
        Dim oDS As New DataSet
        Try
            oDS = New ValoresDAM().SeleccionarPorFiltro3(StrCodigoSBS, StrCodigoIsin, StrCodigoMnemonico, StrMoneda, StrTipoRenta, StrEmisor, StrEstado, dataRequest)
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
        Return oDS
    End Function
    Public Function SeleccionarCorrelativoMnemonicoOR(ByVal codigoNemonico As String, ByVal codigoTercero As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoNemonico, codigoTercero, dataRequest}
        Dim oDSValores As New DataSet
        Try
            oDSValores = New ValoresDAM().SeleccionarCorrelativoMnemonicoOR(codigoNemonico, codigoTercero)
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
        Return oDSValores
    End Function
    Public Function SeleccionarCorrelativoMnemonicoDP(ByVal tipoTitulo As String, ByVal codigoTercero As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {tipoTitulo, codigoTercero, dataRequest}
        Dim oDSValores As New DataSet
        Try
            oDSValores = New ValoresDAM().SeleccionarCorrelativoMnemonicoDP(tipoTitulo, codigoTercero)
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
        Return oDSValores
    End Function
    Public Function Custodio_ListarCuentasDepositariasPorCustodio(ByVal codigoCustodio As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoCustodio, dataRequest}
        Dim oDSValores As New DataSet
        Try
            oDSValores = New ValoresDAM().Custodio_ListarCuentasDepositariasPorCustodio(codigoCustodio)
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
        Return oDSValores
    End Function
    Public Function BuscarPrecioUnitario(ByVal codigoNemonico As String, ByVal dataRequest As DataSet) As Decimal
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoNemonico, dataRequest}
        Dim precioUnitario As String
        Try
            precioUnitario = New ValoresDAM().BuscarPrecioUnitario(codigoNemonico)
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
        Return precioUnitario
    End Function
    Public Function VerificarFondoInversion(ByVal codigonemonico As String) As Integer
        Dim parameters As Object() = {codigonemonico}
        Dim resultado As Integer
        Try
            resultado = New ValoresDAM().VerificarFondoInversion(codigonemonico)
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
        Return resultado
    End Function
    Public Function ObtenerDatosHipervalorizador(ByVal codigoNemonico As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoNemonico, dataRequest}
        Dim oValoresBE As DataSet
        Try
            oValoresBE = New ValoresDAM().ObtenerDatosHipervalorizador(codigoNemonico, dataRequest)
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
        Return oValoresBE
    End Function
#End Region
#Region " /* Funciones Insertar */ "
    Public Function Insertar(ByVal oValoresBE As ValoresBE, ByVal dataRequest As DataSet) As String
        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oValoresBE, dataRequest}
        Dim oValoresDAM As New ValoresDAM
        Try
            codigo = oValoresDAM.Insertar(oValoresBE, dataRequest)
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
        Return codigo
    End Function
    Public Function CambiarCodigo(ByVal codigoActual As String, ByVal nuevoCodigo As String, ByVal dataRequest As DataSet) As Boolean
        CambiarCodigoTemporal(codigoActual, nuevoCodigo, dataRequest)
    End Function
    Public Function CambiarCodigoTemporal(ByVal codigoActual As String, ByVal nuevoCodigo As String, ByVal dataRequest As DataSet) As Boolean
        Dim ok As Boolean = False
        Dim parameters As Object() = {codigoActual, nuevoCodigo, dataRequest}
        Dim oValoresDAM As New ValoresDAM
        Try
            oValoresDAM.CambiarCodigoTemporal(codigoActual, nuevoCodigo)
            ok = True
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
        Return ok
    End Function
    Public Function VerificarCustodia(ByVal codigoMnemónico As String) As Integer
        Dim parameters As Object() = {codigoMnemónico}
        Dim LngContar As Long
        Try
            LngContar = New ValoresDAM().VerificarCustodia(codigoMnemónico)
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
        Return LngContar
    End Function
    Public Function VerificarCuponeraNormal(ByVal codigoMnemónico As String) As Integer
        Dim parameters As Object() = {codigoMnemónico}
        Dim LngContar As Long
        Try
            LngContar = New ValoresDAM().VerificarCuponeraNormal(codigoMnemónico)
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
        Return LngContar
    End Function
    Public Function VerificarCuponeraEspecial(ByVal codigoMnemónico As String) As Integer
        Dim parameters As Object() = {codigoMnemónico}
        Dim LngContar As Long
        Try
            LngContar = New ValoresDAM().VerificarCuponeraEspecial(codigoMnemónico)
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
        Return LngContar
    End Function
    Public Function ExistenciaCuponera(ByVal codigoNemonico As String, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New ValoresDAM().ExistenciaCuponera(dataRequest, codigoNemonico).Tables(0)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
#End Region
#Region " /* Funciones Modificar */"
    Public Function Modificar(ByVal oValoresBE As ValoresBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oValoresBE, dataRequest}
        Dim oValoresDAM As New ValoresDAM
        Try
            actualizado = oValoresDAM.Modificar(oValoresBE, dataRequest)
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
        Return actualizado
    End Function
    'OT10952 - 17/11/2017 - Ian Pastor M. Refactorizar código
    Public Function ModificarDetalleCustodios(ByVal StrCodigoNemo As String, ByVal dtDetalleCotizacion As DataTable, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim oValoresDAM As New ValoresDAM
        Try
            actualizado = oValoresDAM.ModificarDetalleCustodios(StrCodigoNemo, dtDetalleCotizacion, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return actualizado
    End Function
    'OT10952 - Fin
    Public Function ModificarDetalleCapitalCompro(ByVal StrCodigoNemo As String, ByVal dtDetalleCapitalCompro As DataTable, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {StrCodigoNemo, dtDetalleCapitalCompro, dataRequest}
        Dim oValoresDAM As New ValoresDAM
        Try
            actualizado = oValoresDAM.ModificarDetalleCapitalCompro(StrCodigoNemo, dtDetalleCapitalCompro, dataRequest)
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
        Return actualizado
    End Function
    Public Function ActualizarMnemonicoValores(ByVal strCodigoMnemonicoReal As String, ByVal strCodigoMnemonicoTemporal As String, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoMnemonicoReal, strCodigoMnemonicoTemporal, dataRequest}
        Dim oValoresDAM As New ValoresDAM
        Try
            oValoresDAM.ActualizarMnemonicoValores(strCodigoMnemonicoReal, strCodigoMnemonicoTemporal, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
#End Region
#Region " /* Funciones Eliminar */"
    Public Function Eliminar(ByVal codigoValores As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoValores, dataRequest}
        Dim oValoresDAM As New ValoresDAM
        Try
            eliminado = oValoresDAM.Eliminar(codigoValores, dataRequest)
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
        Return eliminado
    End Function
    Public Function EliminarDetalleCustodios(ByVal strCodNemonico As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodNemonico, dataRequest}
        Dim oValoresDAM As New ValoresDAM
        Try
            eliminado = oValoresDAM.EliminarDetalleCustodios(strCodNemonico, dataRequest)
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
    Public Sub CuponNormal(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
    Public Sub CuponEspecial(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
    Public Sub Custodios(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
#End Region
#Region " /* Funciones Personalizadas*/"
    Public Function ListarValoresOI(ByVal CosigoISIN As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim oDS As DataSet
        Try
            oDS = New ValoresDAM().ListarValoresOI(CosigoISIN, dataRequest)
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
        Return oDS
    End Function
    Public Sub InicializarValores(ByRef oRow As ValoresBE.ValorRow)
        Try
            Dim oValoresDAM As New ValoresDAM
            oValoresDAM.InicializarValores(oRow)
            'Luego de terminar la ejecución de métodos(sin errores)             
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub
    Public Function GenerarValorizacionCartera(ByVal codigoPortafolio As String, ByVal fechaOperacion As Decimal, ByVal EntExtPrecio As String, ByVal EntExtTipoCambio As String, ByVal TipoValorizacion As String, ByVal dataRequest As DataSet) As Integer
        Dim oDS As Integer
        Try
            Dim oValoresDAM As New ValoresDAM
            oDS = oValoresDAM.GenerarValorizacionCartera(codigoPortafolio, fechaOperacion, EntExtPrecio, EntExtTipoCambio, TipoValorizacion, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return oDS
    End Function
    Public Function GenerarValorizacionCurvaCuponCero(ByVal codigoPortafolio As String, ByVal fechaOperacion As Decimal, ByVal datarequest As DataSet) As Integer
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {codigoPortafolio, fechaOperacion, datarequest}
        Dim oDS As Integer
        Try
            Dim oValoresDAM As New ValoresDAM
            oDS = oValoresDAM.GenerarValorizacionCurvaCuponCero(codigoPortafolio, fechaOperacion, datarequest)
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
    Public Function SeleccionarValorizacionPromedio(ByVal codigoPortafolio As String, ByVal codigoNemonico As String, ByVal codigoISIN As String, ByVal diasBase As Integer, ByVal dataRequest As DataSet) As Decimal
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoPortafolio, codigoNemonico, codigoISIN, diasBase, dataRequest}
        Dim oDS As Decimal
        Try
            Dim oValoresDAM As New ValoresDAM
            oDS = oValoresDAM.SeleccionarValorizacionPromedio(codigoPortafolio, codigoNemonico, codigoISIN, diasBase)
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
    Public Function SeleccionarValorizacion(ByVal codigoPortafolio As String, ByVal fechaOperacion As Decimal, ByVal TipoValorizacion As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoPortafolio, fechaOperacion, TipoValorizacion, dataRequest}
        Dim oDS As DataSet
        Try
            Dim oValoresDAM As New ValoresDAM
            Return oValoresDAM.SeleccionarValorizacion(codigoPortafolio, fechaOperacion, TipoValorizacion)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function ReporteSeleccionarValorizacion(ByVal codigoPortafolio As String, ByVal fechaOperacion As Decimal, ByVal TipoValorizacion As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoPortafolio, fechaOperacion, TipoValorizacion, dataRequest}
        Dim oDS As DataSet
        Try
            Dim oValoresDAM As New ValoresDAM
            Return oValoresDAM.ReporteSeleccionarValorizacion(codigoPortafolio, fechaOperacion, TipoValorizacion)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function ExtornarValorizacionCartera(ByVal codigoPortafolio As String, ByVal fechaOperacion As Decimal, ByVal dataRequest As DataSet) As String
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoPortafolio, fechaOperacion, dataRequest}
        Dim oDS As DataSet
        Try
            Dim oValoresDAM As New ValoresDAM
            oValoresDAM.ExtornarValorizacionCartera(codigoPortafolio, fechaOperacion, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function VerificarExtornoValorizacionCartera(ByVal codigoPortafolio As String, ByVal fechaOperacion As Decimal, ByVal dataRequest As DataSet) As Integer
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoPortafolio, fechaOperacion, dataRequest}
        Dim iCantidad As Integer
        Try
            Dim oValoresDAM As New ValoresDAM
            iCantidad = oValoresDAM.VerificarExtornoValorizacionCartera(codigoPortafolio, fechaOperacion, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return iCantidad
    End Function
    Public Function ObtenerTemporal(ByVal dataRequest As DataSet) As String
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim Cod As String
        Try
            Dim oValoresDAM As New ValoresDAM
            Cod = oValoresDAM.ObtenerTemporal()
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return Cod
    End Function
    Public Function SeleccionarPorFiltroValores(ByVal codigoInterno As String, ByVal nombreCompleto As String, ByVal dataRequest As DataSet) As ValoresBE
        Dim oValoresDAM As New ValoresDAM
        Dim oValores As New ValoresBE
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoInterno, nombreCompleto, dataRequest}
        Try
            oValores = oValoresDAM.SeleccionarPorFiltroValores(codigoInterno, nombreCompleto)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oValores
    End Function
    Public Function SeleccionarPorFiltroValoresAprob(ByVal codigoInterno As String, ByVal nombreCompleto As String, ByVal dataRequest As DataSet) As ValoresBE
        Dim oValoresDAM As New ValoresDAM
        Dim oValores As New ValoresBE
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoInterno, nombreCompleto, dataRequest}
        Try
            oValores = oValoresDAM.SeleccionarPorFiltroValoresAprob(codigoInterno, nombreCompleto)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oValores
    End Function
    Public Function SeleccionarPorFiltroValoresFuturo(ByVal codigoInterno As String, ByVal nombreCompleto As String, ByVal dataRequest As DataSet) As ValoresBE
        Dim oValoresDAM As New ValoresDAM
        Dim oValores As New ValoresBE
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoInterno, nombreCompleto, dataRequest}
        Try
            oValores = oValoresDAM.SeleccionarPorFiltroValoresFuturo(codigoInterno, nombreCompleto)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oValores
    End Function
    Public Function SeleccionarPorFiltroNemonico(ByVal codigoNemonico As String, ByVal descripcion As String, ByVal dataRequest As DataSet) As ValoresBE
        Dim oValoresDAM As New ValoresDAM
        Dim oValores As New ValoresBE
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoNemonico, descripcion, dataRequest}
        Try
            oValores = oValoresDAM.SeleccionarPorFiltroValores(codigoNemonico, descripcion)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oValores
    End Function
    Public Function ObtenerUltimaFechaValorizacion(ByVal portafolio As String) As String
        Dim oValoresDAM As New ValoresDAM
        Dim oValores As String
        Try
            oValores = oValoresDAM.ObtenerUltimaFechaValorizacion(portafolio)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oValores
    End Function
    Public Function SeleccionarSaldoNemonicos(ByVal fecha As Decimal, ByVal portafolio As String, ByVal datarequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {fecha, portafolio, datarequest}
        Dim oDS As DataSet
        Try
            Dim oValoresDAM As New ValoresDAM
            oDS = oValoresDAM.SeleccionarSaldoNemonicos(fecha, portafolio)
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
    Public Function ObtenerNemonicosError() As DataSet
        Dim oDS As DataSet
        Try
            Dim oValoresDAM As New ValoresDAM
            oDS = oValoresDAM.ObtenerNemonicosError()
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDS
    End Function
    Public Sub EliminarCuponera()
        Try
            Dim oValoresDAM As New ValoresDAM
            oValoresDAM.EliminarCuponera()
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub
    Public Function VerificaNemonicosValorizacion() As Integer
        Dim oDS As Integer
        Try
            Dim oValoresDAM As New ValoresDAM
            oDS = oValoresDAM.VerificaNemonicosValorizacion
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDS
    End Function
    Public Function VerificarTasasCurva(ByVal fecha As Decimal) As Integer
        Dim oDS As Integer
        Try
            Dim oValoresDAM As New ValoresDAM
            oDS = oValoresDAM.VerificarTasasCurva(fecha)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDS
    End Function
    Public Function VerificarCalculoComisiones(ByVal codigonemonico As String, ByVal codigotercero As String, ByVal fechaoperacion As Decimal, ByVal tipooperacion As String) As Integer
        Dim oDS As Integer
        Try
            Dim oValoresDAM As New ValoresDAM
            oDS = oValoresDAM.VerificarCalculoComisiones(codigonemonico, codigotercero, fechaoperacion, tipooperacion)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDS
    End Function
    Public Sub EliminarValoracionCartera(ByVal TipoValoracion As String, ByVal Portafolio As String, ByVal fechavaloracion As Decimal)
        Try
            Dim oValoresDAM As New ValoresDAM
            oValoresDAM.EliminarValoracionCartera(TipoValoracion, Portafolio, fechavaloracion)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub
    Public Function ExisteValoracion(ByVal Portafolio As String, ByVal fechavaloracion As Decimal) As Integer
        Dim oDS As Integer
        Try
            Dim oValoresDAM As New ValoresDAM
            oDS = oValoresDAM.ExisteValoracion(Portafolio, fechavaloracion)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDS
    End Function
    Public Function ValoracionValidarOperaciones(ByVal codigoPortafolio As String, ByVal fecha As Decimal) As Integer
        Dim oDS As Integer
        Try
            Dim oValoresDAM As New ValoresDAM
            oDS = oValoresDAM.ValoracionValidarOperaciones(codigoPortafolio, fecha)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDS
    End Function
    Public Function ValoresRetornarFechaVencimiento(ByVal codigonemonico As String) As Decimal
        Dim oDS As Decimal
        Try
            Dim oValoresDAM As New ValoresDAM
            oDS = oValoresDAM.ValoresRetornarFechaVencimiento(codigonemonico)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDS
    End Function
    Public Function OrdenInversionRetornarPH(ByVal codigoorden As String, ByVal portafolio As String) As String
        Dim oDS As String
        Try
            Dim oValoresDAM As New ValoresDAM
            oDS = oValoresDAM.OrdenInversionRetornarPH(codigoorden, portafolio)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDS
    End Function
    Public Function SeleccionarPorCodigoFactura(ByVal pCodigoFactura As String, ByVal pCodigoMnemonico As String, ByVal dataRequest As DataSet) As ValoresBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {pCodigoFactura, pCodigoMnemonico, dataRequest}
        Dim oValoresBE As New ValoresBE
        Try
            oValoresBE = New ValoresDAM().SeleccionarPorCodigoFactura(pCodigoFactura, pCodigoMnemonico)
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
        Return oValoresBE
    End Function
#End Region
    Public Function SeleccionarTipoRentaPorCodigoNemonico(ByVal codigoMnemónico As String) As String
        Dim tiporenta As String
        Try
            tiporenta = New ValoresDAM().SeleccionarTipoRentaPorCodigoNemonico(codigoMnemónico)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return tiporenta
    End Function
    Public Function ValidarDRL_ParaValoracionEstimada(ByVal CodigoPortafolio As String, ByVal FechaProceso As Decimal) As Integer
        Dim tiporenta As String
        Try
            tiporenta = New ValoresDAM().ValidarDRL_ParaValoracionEstimada(CodigoPortafolio, FechaProceso)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return tiporenta
    End Function
    Public Function Aprobacion_InstrumentosRiesgo(ByVal CodigoNemonico As String, ByVal Fecha As Decimal, ByVal Obs As String, ByVal Operacion As String, ByVal dataRequest As DataSet) As DataSet
        Try
            Dim oValoresDAM As New ValoresDAM
            Return oValoresDAM.Aprobacion_InstrumentosRiesgo(CodigoNemonico, Fecha, Obs, Operacion, dataRequest)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function Reporte_AutorizacionRiesgo(ByVal codigoMnemónico As String) As DataTable
        Dim parameters As Object() = {codigoMnemónico}
        Dim oTable As New DataTable
        Try
            oTable = New ValoresDAM().Reporte_AutorizacionRiesgo(codigoMnemónico)
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
        Return oTable
    End Function
    Public Sub CalculaMontoInversion(ByVal Fechavaloracion As Decimal, CodigoPortafolioSBS As String)
        Try
            Dim oValoresDAM As New ValoresDAM
            oValoresDAM.CalculaMontoInversion(Fechavaloracion, CodigoPortafolioSBS)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub BorraMontoInversion(ByVal Fechavaloracion As Decimal, CodigoPortafolioSBS As String)
        Try
            Dim oValoresDAM As New ValoresDAM
            oValoresDAM.BorraMontoInversion(Fechavaloracion, CodigoPortafolioSBS)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function InsertaTraspasoValores(ByVal CodigoIsinOrigen As String, CodigoIsinDestino As String, FechaProceso As Decimal, dataRequest As DataSet) As DataTable
        Try
            Dim oValoresDAM As New ValoresDAM
            Return oValoresDAM.InsertaTraspasoValores(CodigoIsinOrigen, CodigoIsinDestino, FechaProceso, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ValidaTraspaso(ByVal CodigoIsinOrigen As String, CodigoIsinDestino As String) As String
        Try
            Dim oValoresDAM As New ValoresDAM
            Return oValoresDAM.ValidaTraspaso(CodigoIsinOrigen, CodigoIsinDestino)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub ActualizaNemomicoIsin(ByVal TipoActualizacion As String, CodigoMnemonico As String, CodigoIsin As String)
        Try
            Dim oValoresDAM As New ValoresDAM
            oValoresDAM.ActualizaNemomicoIsin(TipoActualizacion, CodigoMnemonico, CodigoIsin)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function NominalBono(ByVal CodigoPortafolio As String, ByVal CodigoIsin As String, ByVal Fecha As Decimal, ByVal Unidades As Decimal) As Decimal
        Try
            Dim oValoresDAM As New ValoresDAM
            Return oValoresDAM.NominalBono(CodigoPortafolio, CodigoIsin, Fecha, Unidades)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function TasaBono(CodigoIsin As String) As Decimal
        Try
            Dim oValoresDAM As New ValoresDAM
            Return oValoresDAM.TasaBono(CodigoIsin)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CuponeraSWAP(ByVal CadenaNemonico As String, Fecha As Decimal, TasaCupon As Decimal, MontoNominal As Decimal,
    TasaCuponOriginal As Decimal, MontoNominalOriginal As Decimal) As DataTable
        Try
            Dim oValoresDAM As New ValoresDAM
            Return oValoresDAM.CuponeraSWAP(CadenaNemonico, Fecha, TasaCupon, MontoNominal, TasaCuponOriginal, MontoNominalOriginal)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10362 - 18/05/2017 - Carlos Espejo
    'Descripcion: Inserta los instrumentos temporales
    Public Function InsertarPrecioISINDetalle(ByVal CodigoUsuario As String, CodigoIsin As String, CodigoNemonico As String)
        Try
            Dim oValoresDAM As New ValoresDAM
            Return oValoresDAM.InsertarPrecioISINDetalle(CodigoUsuario, CodigoIsin, CodigoNemonico)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10362 - 18/05/2017 - Carlos Espejo
    'Descripcion: Borrar los instrumentos temporales
    Public Function BorrarPrecioISINDetalle(ByVal CodigoUsuario As String)
        Try
            Dim oValoresDAM As New ValoresDAM
            Return oValoresDAM.BorrarPrecioISINDetalle(CodigoUsuario)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10362 - 18/05/2017 - Carlos Espejo
    'Descripcion: Lista los datos del reporte
    Public Function ListaPrecioInstrumento(ByVal FechaInicio As Decimal, FechaFin As Decimal, CodigoUsuario As String) As DataTable
        Try
            Dim oValoresDAM As New ValoresDAM
            Return oValoresDAM.ListaPrecioInstrumento(FechaInicio, FechaFin, CodigoUsuario)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10362 - 18/05/2017 - Carlos Espejo
    'Descripcion: Lista los instrumento del reporte por cantidad de registros en VP
    Public Function ListaOrdenPrecioInstrumento(CodigoUsuario As String) As DataTable
        Try
            Dim oValoresDAM As New ValoresDAM
            Return oValoresDAM.ListaOrdenPrecioInstrumento(CodigoUsuario)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Lista valores por tipo de renta
    Public Function ListarValores(ByVal CodigoNemonico As String, ByVal FechaConsulta As Decimal, TipoRenta As String) As ValoresEList
        Try
            Dim oValoresDAM As New ValoresDAM
            Return oValoresDAM.ListarValores(CodigoNemonico, FechaConsulta, TipoRenta)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
