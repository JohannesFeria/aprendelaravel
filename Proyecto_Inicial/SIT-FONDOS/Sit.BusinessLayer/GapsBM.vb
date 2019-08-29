Imports System.Data
Imports MotorTransaccionesProxy
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class GapsBM
    Inherits InvokerCOM

    Public Sub New()

    End Sub

    Dim daGaps As New GapsDAM

    Public Sub InicializarGaps(ByRef oRow As GapsBE.GapsRow)
        Try
            daGaps.InicializarGaps(oRow)
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

    'Public Function InsertarGaps(ByVal objGaps As GapsBE, ByVal dataRequest As DataSet) As String
    '    Dim strCodigo As String
    '    Dim parameters As Object() = {objGaps, dataRequest}

    '    Try

    '        strCodigo = daGaps.Insertar(objGaps, dataRequest)
    '        RegistrarAuditora(parameters)

    '    Catch ex As Exception
    '        RegistrarAuditora(parameters, ex)
    '        Dim rethrow As Boolean = True
    '        If (rethrow) Then
    '            Throw
    '        End If
    '    End Try
    '    Return strCodigo
    'End Function

    Public Function InsertarExcel(ByVal dtGaps As DataTable, ByVal Fecha As Integer, ByVal dataRequest As DataSet) As String
        Dim strCodigo As String
        Dim parameters As Object() = {dtGaps, dataRequest}

        Try

            strCodigo = daGaps.InsertarExcel(dtGaps, Fecha, dataRequest)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return strCodigo
    End Function

    'Public Function EliminarTabla(ByVal dataRequest As DataSet) As String
    '    Dim strCodigo As String
    '    Dim parameters As Object() = {dataRequest}

    '    Try

    '        strCodigo = daGaps.EliminarTabla(dataRequest)

    '    Catch ex As Exception
    '        RegistrarAuditora(parameters, ex)
    '        Dim rethrow As Boolean = True
    '        If (rethrow) Then
    '            Throw
    '        End If
    '    End Try
    '    Return strCodigo
    'End Function

    Public Function InsertarExcelFondo(ByVal dtGaps As DataTable, ByVal TipoFondo As String, ByVal fecha As Integer, ByVal dataRequest As DataSet) As String
        Dim strCodigo As String
        Dim parameters As Object() = {dtGaps, TipoFondo, fecha, dataRequest}
        Try

            strCodigo = daGaps.InsertarExcelFondo(dtGaps, TipoFondo, fecha, dataRequest)

            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return strCodigo
    End Function

    Public Function ConsultaCantidadMoneda(ByVal Moneda As String, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim parameters As Object() = {Moneda, dataRequest}

        Try
            oreporte = daGaps.ConsultaCantidadMoneda(Moneda)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oreporte
    End Function

    Public Function ReporteGapsResumen0rdenado(ByVal CodigoMoneda As String, ByVal CodigoTipoInstrumentoSBS As String, ByVal CodigoPais As String, ByVal fecha As Integer, ByVal dataRequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Dim parameters As Object() = {CodigoMoneda, dataRequest}

        Try
            oreporte = daGaps.ReporteGapsResumen0rdenado(CodigoMoneda, CodigoTipoInstrumentoSBS, CodigoPais, fecha)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oreporte
    End Function


End Class
