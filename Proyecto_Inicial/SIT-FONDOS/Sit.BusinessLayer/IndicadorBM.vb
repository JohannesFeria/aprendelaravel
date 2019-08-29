Option Explicit On 
Option Strict On

#Region "/* Imports */"

Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities


#End Region

Public Class IndicadorBM
    '  Inherits InvokerCOM

#Region "/* Constructor */"

    Public Sub New()

    End Sub

#End Region

#Region "/* Funciones Personalizadas */"

    Public Function Insertar(ByVal oIndicadorBE As IndicadorBE, ByVal dataRequest As DataSet) As Boolean

        Dim oIndicadorDAM As New IndicadorDAM

        Try
            oIndicadorDAM.Insertar(oIndicadorBE, dataRequest)

        Catch ex As Exception
            Throw ex
        End Try

        Return True

    End Function

    Public Function Modificar(ByVal oIndicadorBE As IndicadorBE, ByVal dataRequest As DataSet) As Boolean

        Dim oIndicadorDAM As New IndicadorDAM
        Try

            oIndicadorDAM.Modificar(oIndicadorBE, dataRequest)

        Catch ex As Exception
            Throw ex

        End Try

        Return True

    End Function

    Public Function Eliminar(ByVal codigoIndicador As String, ByVal dataRequest As DataSet) As Boolean

        Dim oIndicadorDAM As New IndicadorDAM


        Try

            oIndicadorDAM.Eliminar(codigoIndicador, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try

        Return True

    End Function
    Public Function SeleccionarPorTipo(ByVal TipoIndicador As String, ByVal datarequest As DataSet) As DataSet
        Dim oIndicadorDAM As New IndicadorDAM
        Dim oIndicadorBE As DataSet
        Try

            oIndicadorBE = oIndicadorDAM.SeleccionarPorTipo(TipoIndicador)
        Catch ex As Exception
            Throw ex
        End Try

        Return oIndicadorBE
    End Function

    'Public Function SeleccionarTipoVac(ByVal datarequest As DataSet) As IndicadorBE
    '    Dim oIndicadorDAM As New IndicadorDAM
    '    Dim intCodigoEjecucion As Integer
    '    Dim parameters As Object() = {datarequest}

    '    Dim oIndicadorBE As IndicadorBE

    '    Try

    '        intCodigoEjecucion = ObtenerCodigoEjecucion(datarequest)

    '        oIndicadorBE = oIndicadorDAM.SeleccionarTipoVac()
    '        RegistrarAuditora(parameters)

    '    Catch ex As Exception

    '        RegistrarAuditora(parameters, ex)
    '        Dim rethrow As Boolean = true
    '        If (rethrow) Then
    '            Throw
    '        End If

    '    End Try

    '    Return oIndicadorBE
    'End Function

    Public Function SeleccionarPorFiltro(ByVal codigoIndicador As String, ByVal nombreIndicador As String, ByVal situacion As String, ByVal baseTasa As Decimal, ByVal diasPeriodo As Decimal, ByVal fechaVigencia As Decimal, ByVal manejarPeriodo As String, ByVal codigoPeriodicidad As String, ByVal codigoTipoCupon As String, ByVal dataRequest As DataSet, Optional ByVal sTipoIndicador As String = "") As IndicadorBE

        Dim oIndicadorDAM As New IndicadorDAM
        Dim oIndicadorBE As IndicadorBE

        Try

            oIndicadorBE = oIndicadorDAM.SeleccionarPorFiltro(codigoIndicador, nombreIndicador, situacion, baseTasa, diasPeriodo, fechaVigencia, manejarPeriodo, codigoPeriodicidad, codigoTipoCupon, sTipoIndicador)

        Catch ex As Exception

            Throw ex

        End Try

        Return oIndicadorBE

    End Function

    Public Function Seleccionar(ByVal codigoIndicador As String, ByVal dataRequest As DataSet) As IndicadorBE

        Dim oIndicadorDAM As New IndicadorDAM

        Dim oIndicadorBE As IndicadorBE

        Try

            oIndicadorBE = oIndicadorDAM.Seleccionar(codigoIndicador)


        Catch ex As Exception

            Throw ex

        End Try

        Return oIndicadorBE

    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As IndicadorBE

        Dim oIndicadorDAM As New IndicadorDAM
        Dim oIndicadorBE As IndicadorBE

        Try

            oIndicadorBE = oIndicadorDAM.Listar()


        Catch ex As Exception
            Throw ex

        End Try

        Return oIndicadorBE

    End Function

    Public Function SeleccionarValorIndicador(ByVal codigoIndicador As String, ByVal fecha As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim oIndicadorDAM As New IndicadorDAM
        Dim oDS As DataSet
        Try
            oDS = oIndicadorDAM.SeleccionarValorIndicador(codigoIndicador, fecha)
        Catch ex As Exception
            Throw ex
        End Try
        Return oDS
    End Function

    Public Function VerificaPreCarga(ByVal sCodigoIndicador As String, ByVal nFechaInformacion As Decimal, ByVal dataRequest As DataSet) As DataSet
        Try
            Return New IndicadorDAM().VerificaPreCarga(sCodigoIndicador, nFechaInformacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function EliminarIndicadorCuponCero(ByVal sCodigoIndicador As String, ByVal nFechaInformacion As Decimal, ByVal dataRequest As DataSet) As DataSet
        Try
            Return New IndicadorDAM().EliminarIndicadorCuponCero(sCodigoIndicador, nFechaInformacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function InsertarIndicadorCuponCero(ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As String
        Dim codigo As String = String.Empty
        Try
            Dim oInterfaseBloombergDAM As New InterfaseBloombergDAM
            codigo = New IndicadorDAM().InsertarIndicadorCuponCero(dtDetalle, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try

        Return codigo

    End Function


    '-----------------------------montos negociados BVL


    Public Function EliminarMontoNegociadoBVL(ByVal nFechaInformacion As Decimal, ByVal dataRequest As DataSet) As Boolean
        Try
            Return New IndicadorDAM().EliminarMontoNegociadoBVL(nFechaInformacion)
        Catch ex As Exception
            Throw ex
        End Try
        Return True
    End Function


    Public Function InsertarMontoNegociadoBVL(ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As String
        Dim codigo As String = String.Empty
        Try
            Dim oInterfaseBloombergDAM As New InterfaseBloombergDAM
            codigo = New IndicadorDAM().InsertarMontoNegociadoBVL(dtDetalle, dataRequest)

        Catch ex As Exception
            Throw ex
        End Try

        Return codigo

    End Function


    Public Function VerificaPreCargaMontoNegociadoBVL(ByVal nFechaInformacion As Decimal, ByVal dataRequest As DataSet) As DataSet
        Try
            Return New IndicadorDAM().VerificaPreCargaMontoNegociadoBVL(nFechaInformacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function



    '----------------------------Libor Fecha-------------------------
    'LETV 20090703
    Public Function SeleccionarCuponesLibor(ByVal codigoNemonico As String, ByVal fechaInicio As Decimal, ByVal fechaTermino As Decimal) As DataTable
        Dim oIndicadorDAM As New IndicadorDAM
        Try
            Return oIndicadorDAM.SeleccionarCuponesLibor(codigoNemonico, fechaInicio, fechaTermino)
        Catch ex As Exception

            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function ModificarCuponesLibor(ByVal CodigoNemonico As String, ByVal Secuencia As String, ByVal FechaInicio As Decimal, ByVal FechaLibor As Decimal) As Boolean
        Try
            Dim oIndicadorDAM As New IndicadorDAM
            Return oIndicadorDAM.ModificarCuponesLibor(CodigoNemonico, Secuencia, FechaInicio, FechaLibor)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception

            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw ex
            End If
        End Try
    End Function

    Public Function Indicador_SeleccionarSWAP(ByVal codigoIndicador As String) As DataTable
        Dim oIndicadorDAM As New IndicadorDAM
        Try
            Return oIndicadorDAM.Indicador_SeleccionarSWAP(codigoIndicador).Tables(0)

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Indicador_SeleccionarIndicadorLibor(ByVal codigoIndicador As String, ByVal fuente As String) As DataTable
        Dim oIndicadorDAM As New IndicadorDAM
        Try
            Return oIndicadorDAM.Indicador_SeleccionarIndicadorLibor(codigoIndicador, fuente).Tables(0)

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Indicador_SeleccionarValorLibor(ByVal codigoIndicador As String, ByVal fechaReferencia As Decimal) As Decimal
        Dim oIndicadorDAM As New IndicadorDAM
        Try
            Return oIndicadorDAM.Indicador_SeleccionarValorLibor(codigoIndicador, fechaReferencia)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

#End Region

End Class