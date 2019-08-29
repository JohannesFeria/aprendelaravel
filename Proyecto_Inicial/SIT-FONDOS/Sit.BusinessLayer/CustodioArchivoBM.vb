Imports System
Imports System.Data
Imports MotorTransaccionesProxy
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class CustodioArchivoBM
    Inherits InvokerCOM
    Public Sub New()
    End Sub
#Region "/* Funciones Seleccionar */"

    Public Function Seleccionar(ByVal CodigoPortafolio As String, ByVal CodigoISIN As String, _
                                ByVal FechaCreacion As Decimal, ByVal dataRequest As DataSet) As CustodioArchivoBE

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New CustodioArchivoDAM().Seleccionar(CodigoPortafolio, CodigoISIN, FechaCreacion, dataRequest)
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

    Public Function Listar(ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New CustodioArchivoDAM().Listar()
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
    'Public Sub CargarArchivo(ByVal sCodigoCustodio As String, ByVal sCustodioArchivo As String, ByVal CodigoPortafolio As String, ByVal sFechaCorte As String, ByVal dataRequest As DataSet)
    '    Try
    '        Dim oCustodioArchivoDAM As New CustodioArchivoDAM
    '        oCustodioArchivoDAM.CargarArchivo(sCodigoCustodio, sCustodioArchivo, CodigoPortafolio, sFechaCorte, dataRequest)
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub
    Public Function CargarArchivo(ByVal sCodigoCustodio As String, ByVal sCustodioArchivo As String, ByVal CodigoPortafolio As String, ByVal sFechaCorte As String, ByVal dataRequest As DataSet) As DataTable
        Try
            Dim oCustodioArchivoDAM As New CustodioArchivoDAM
            Return oCustodioArchivoDAM.CargarArchivo(sCodigoCustodio, sCustodioArchivo, CodigoPortafolio, sFechaCorte, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub Insertar(ByVal oCustodioArchivo As CustodioArchivoBE, ByVal dataRequest As DataSet)
        Try
            Dim oCustodioArchivoDAM As New CustodioArchivoDAM
            oCustodioArchivoDAM.Insertar(oCustodioArchivo, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function Modificar(ByVal oCustodioArchivo As CustodioArchivoBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Try
            Dim oCustodioArchivoDAM As New CustodioArchivoDAM
            actualizado = oCustodioArchivoDAM.Modificar(oCustodioArchivo, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return actualizado
    End Function
    Public Function Eliminar(ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Try
            Dim oCustodioArchivoDAM As New CustodioArchivoDAM
            eliminado = oCustodioArchivoDAM.Eliminar(dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return eliminado
    End Function
    Public Function ImportarArchivo(ByVal sCodigoCustodio As String, ByVal sCodigoPortafolio As String, ByVal nFechaCorte As Long, ByVal sFlagBorrado As Decimal, ByVal dataRequest As DataSet) As Boolean
        Try
            Return New CustodioArchivoDAM().ImportarArchivo(sCodigoCustodio, sCodigoPortafolio, nFechaCorte, sFlagBorrado)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function VerificaCustodioInformacion(ByVal nFechaCorte As Long, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New CustodioArchivoDAM().VerificaCustodioInformacion(nFechaCorte)
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
    Public Function VerificaPreCarga(ByVal sCodigoCustodio As String, ByVal CodigoPortafolio As String, ByVal nFechaCorte As Long, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New CustodioArchivoDAM().VerificaPreCarga(sCodigoCustodio, CodigoPortafolio, nFechaCorte)
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
    Public Function InstrumentosNoRegistrados(ByVal nFechaCorte As Long, ByVal sPortafolioCodigo As String, ByVal sCodigoCustodio As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New CustodioArchivoDAM().InstrumentosNoRegistrados(nFechaCorte, sPortafolioCodigo, sCodigoCustodio)
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
    Public Function InstrumentosNoReportados(ByVal nFechaCorte As Long, ByVal sPortafolioCodigo As String, ByVal sCodigoCustodio As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New CustodioArchivoDAM().InstrumentosNoReportados(nFechaCorte, sPortafolioCodigo, sCodigoCustodio)
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
    Public Function InstrumentosConciliados(ByVal nFechaCorte As Long, ByVal sPortafolioCodigo As String, ByVal sCodigoCustodio As String, ByVal dataRequest As DataSet) As DataSet
        Dim parameters As Object() = {dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New CustodioArchivoDAM().InstrumentosConciliados(nFechaCorte, sPortafolioCodigo, sCodigoCustodio)
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
    Public Function GeneraSaldos(ByVal decNewFecha As Decimal, ByVal decOldFecha As Decimal, ByVal strCodigoPortafolioSBS As String, ByVal dataRequest As DataSet) As Boolean
        Try
            Return New CustodioArchivoDAM().GeneraSaldos(decNewFecha, decOldFecha, strCodigoPortafolioSBS, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function InstrumentosPorConciliar(ByVal nFechaCorte As Decimal, ByVal sPortafolioCodigo As String, ByVal sCodigoCustodio As String, ByVal dataRequest As DataSet) As DataSet
        Dim parameters As Object() = {dataRequest}
        Try
            Return New CustodioArchivoDAM().InstrumentosPorConciliar(nFechaCorte, sPortafolioCodigo, sCodigoCustodio)
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
    Public Function ActualizaSaldosInfCustodio(ByVal nFechaCorte As Decimal, ByVal sCodigoMnemonico As String, _
                                                ByVal sCodigoISIN As String, ByVal sCodigoPortafolioSBS As String, _
                                                ByVal sCodigoCustodio As String, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New CustodioArchivoDAM().ActualizaSaldosInfCustodio(nFechaCorte, sCodigoMnemonico, sCodigoISIN, sCodigoPortafolioSBS, sCodigoCustodio)
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
    Public Function InstrumentosDiferencias(ByVal nFechaCorte As Long, ByVal sPortafolioCodigo As String, ByVal sCodigoCustodio As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New CustodioArchivoDAM().InstrumentosDiferencias(nFechaCorte, sPortafolioCodigo, sCodigoCustodio)
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
    Public Function InstrumentosDiferenciasCDet(ByVal nFechaCorte As Long, ByVal sPortafolioCodigo As String, ByVal sCodigoCustodio As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New CustodioArchivoDAM().InstrumentosDiferenciasCDet(nFechaCorte, sPortafolioCodigo, sCodigoCustodio)
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
    Public Function InstrumentosDiferenciasVarios(ByVal nFechaCorte As Long, ByVal sPortafolioCodigo As String, ByVal sCodigoCustodio As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New CustodioArchivoDAM().InstrumentosDiferenciasVarios(nFechaCorte, sPortafolioCodigo, sCodigoCustodio)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function InstrumentosDiferenciasCarteraCustodio(ByVal fecha As Decimal, ByVal sPortafolioCodigo As String, ByVal sCodigoCustodio As String) As DataSet
        Try
            Return New CustodioArchivoDAM().InstrumentosDiferenciasCarteraCustodio(fecha, sPortafolioCodigo, sCodigoCustodio)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function DIFerenciasCustodioResumen_Listar(ByVal fecha As Decimal, ByVal sPortafolioCodigo As String, ByVal sCodigoCustodio As String) As DataSet
        Try
            Return New CustodioArchivoDAM().DIFerenciasCustodioResumen_Listar(fecha, sPortafolioCodigo, sCodigoCustodio)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10238 20/04/2017 - Carlos Espejo
    'Descripcion: Se genera el saldo incial de una fecha
    Public Sub GeneraSaldoBanco(FechaSaldo As Decimal, FechaAnterior As Decimal, CodigoPortafolio As String, ClaseCuenta As String, ByVal dataRequest As DataSet)
        Try
            Dim oCustodioArchivoDAM As New CustodioArchivoDAM
            oCustodioArchivoDAM.GeneraSaldoBanco(FechaSaldo, FechaAnterior, CodigoPortafolio, ClaseCuenta, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class