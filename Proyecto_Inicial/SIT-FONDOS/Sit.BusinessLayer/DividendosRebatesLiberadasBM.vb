Imports System
Imports System.Data
Imports MotorTransaccionesProxy
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
''' Clase para el acceso de los datos para DividendosRebatesLiberadas tabla.
Public Class DividendosRebatesLiberadasBM
    Inherits InvokerCOM
    'OT10916 - 07/11/2017 - Ian Pastor M. Ordenar y factorizar código
#Region "/* Funciones Seleccionar */"

    Public Function ListarInformacionEntidadExterna(ByVal nFechaInicio As Long, _
                                                    ByVal nFechaFin As Long, _
                                                    ByVal sEntidadExt As String, _
                                                    ByVal dataRequest As DataSet) As DataSet
        Try
            Return New DividendosRebatesLiberadasDAM().ListarInformacionEntidadExterna(nFechaInicio, nFechaFin, sEntidadExt)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    ''' Lista todos los expedientes de DividendosRebatesLiberadas tabla.
    Public Function Listar(ByVal sCodigoSBS As String, ByVal sCodigoNemonico As String, ByVal dataRequest As DataSet) As DataTable
        Try
            Return New DividendosRebatesLiberadasDAM().Listar(sCodigoSBS, sCodigoNemonico).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    ''' Selecciona un solo expediente de DividendosRebatesLiberadas tabla.
    Public Function Seleccionar(ByVal sCodigoSBS As String, ByVal nIdentificador As Decimal, ByVal sSituacion As String, ByVal dataRequest As DataSet) As DataSet
        Try
            Return New DividendosRebatesLiberadasDAM().Seleccionar(sCodigoSBS, nIdentificador, sSituacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    ''' Selecciona un solo expediente de DividendosRebatesLiberadas tabla.
    Public Function SeleccionarImpresion(ByVal sCodigoSBS As String, ByVal nGrupoIdentificador As Decimal, ByVal sSituacion As String, ByVal sMultifondo As String, ByVal dataRequest As DataSet) As DataSet
        Try
            Return New DividendosRebatesLiberadasDAM().SeleccionarImpresion(sCodigoSBS, nGrupoIdentificador, sSituacion, sMultifondo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

#End Region

    'Inserta un registro de DividendosRebatesLiberadas tabla.
    Public Function Insertar(ByVal oDividendosRebatesLiberadas As DividendosRebatesLiberadasBE, ByRef nIdentificador As Decimal, ByRef nGrupoIdentificador As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim Insertado As Boolean = False
        Try
            Dim oDividendosRebatesLiberadasDAM As New DividendosRebatesLiberadasDAM
            Insertado = oDividendosRebatesLiberadasDAM.Insertar(oDividendosRebatesLiberadas, nIdentificador, nGrupoIdentificador, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return Insertado
    End Function

    ''' Modifica un registro de DividendosRebatesLiberadas tabla.
    Public Function Modificar(ByVal oDividendosRebatesLiberadas As DividendosRebatesLiberadasBE, ByVal dataRequest As DataSet) As Boolean
        Dim Actualizado As Boolean = False
        Try
            Dim oDividendosRebatesLiberadasDAM As New DividendosRebatesLiberadasDAM
            Actualizado = oDividendosRebatesLiberadasDAM.Modificar(oDividendosRebatesLiberadas, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return Actualizado
    End Function

    ''' Elimina un registro de DividendosRebatesLiberadas tabla.
    Public Function Eliminar(ByVal CodigoSBS As String, ByVal nIdentificador As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim Eliminado As Boolean = False
        Try
            Dim oDividendosRebatesLiberadasDAM As New DividendosRebatesLiberadasDAM
            Eliminado = oDividendosRebatesLiberadasDAM.Eliminar(CodigoSBS, nIdentificador, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return Eliminado
    End Function

    Public Function ConfirmarDividendoRebateLiberada(ByVal fondo As String, ByVal nemonico As String, ByVal fechavencimiento As String, ByVal identificador As String, ByVal monto As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim cuponera As DataSet
        Dim oDRL As New DividendosRebatesLiberadasDAM
        Dim rpta As Boolean = False
        Try
            rpta = oDRL.ConfirmarDividendoRebateLiberada(fondo, nemonico, fechavencimiento, identificador, monto, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return rpta
    End Function

    Public Function NemonicoFechaidi_VerificaActual(ByVal CodigoNemonico As String, ByVal FechaIDI As String) As String
        Dim cuponera As DataSet
        Dim oDRL As New DividendosRebatesLiberadasDAM
        Dim rpta As String = ""
        Try
            rpta = oDRL.NemonicoFechaidi_VerificaActual(CodigoNemonico, FechaIDI)
        Catch ex As Exception
            Throw ex
        End Try
        Return rpta
    End Function

    Public Function NemonicoFechaidi_Contar(ByVal CodigoNemonico As String, ByVal FechaIDI As String) As String
        Dim cuponera As DataSet
        Dim oDRL As New DividendosRebatesLiberadasDAM
        Dim rpta As String = ""
        Try
            rpta = oDRL.NemonicoFechaidi_Contar(CodigoNemonico, FechaIDI)
        Catch ex As Exception
            Throw ex
        End Try
        Return rpta
    End Function

    Public Function NemonicoFechaidi_NumeroUnidades(ByVal CodigoNemonico As String, ByVal FechaIDI As String) As String
        Dim cuponera As DataSet
        Dim oDRL As New DividendosRebatesLiberadasDAM
        Dim rpta As String = ""
        Try
            rpta = oDRL.NemonicoFechaidi_NumeroUnidades(CodigoNemonico, FechaIDI)
        Catch ex As Exception
            Throw ex
        End Try
        Return rpta
    End Function

    Public Function NemonicoFechaidi_Factor(ByVal CodigoNemonico As String, ByVal FechaIDI As String) As String
        Dim cuponera As DataSet
        Dim oDRL As New DividendosRebatesLiberadasDAM
        Dim rpta As String = ""
        Try
            rpta = oDRL.NemonicoFechaidi_Factor(CodigoNemonico, FechaIDI)
        Catch ex As Exception
            Throw ex
        End Try
        Return rpta
    End Function

    Public Function ObtenerDatosReporteRebates(ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal, ByVal CodigoNemonico As String) As DataSet
        Try
            Return New DividendosRebatesLiberadasDAM().ObtenerDatosReporteRebates(FechaInicio, FechaFin, CodigoNemonico)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ExisteRebate(ByVal codigoRebate As Decimal) As DataTable
        Try
            Return New DividendosRebatesLiberadasDAM().ExisteRebate(codigoRebate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'JZAVALA - OT - 67090 Rebates. - SE AGREGA EL PARAMETRO DE ENTRADA @p_SumatoriaFondos.
    Public Function InsertarRebate(ByVal CodigoNemonico As String, ByVal DiasCalculo As Decimal, ByVal PorcRebate As Decimal, ByVal IndRango As String, ByVal Situacion As String, ByVal UsuarioCreacion As String, ByVal dataRequest As DataSet, ByVal strSumatoriaFondos As String) As Boolean
        Dim Insertado As Boolean = False
        Try
            Dim oDividendosRebatesLiberadasDAM As New DividendosRebatesLiberadasDAM
            Insertado = oDividendosRebatesLiberadasDAM.InsertarRebate(CodigoNemonico, DiasCalculo, PorcRebate, IndRango, Situacion, UsuarioCreacion, strSumatoriaFondos)
        Catch ex As Exception
            Throw ex
        End Try
        Return Insertado
    End Function

    Public Function ObtenerCodigoRebateDetalle(ByVal codigoNemonico As String) As DataTable
        Dim parameters As Object() = {codigoNemonico}

        Try
            Return New DividendosRebatesLiberadasDAM().ObtenerCalculoRebateDetalle(codigoNemonico)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function InsertarRebateDetalle(ByVal CodigoNemonico As String, ByVal ImporteInicio As Decimal, ByVal ImporteFin As Decimal, ByVal PorRebate As Decimal, ByVal Situacion As String, ByVal UsuarioCreacion As String, ByVal CodigoDetalle As Integer, ByVal dataRequest As DataSet) As Boolean
        Dim Insertado As Boolean = False
        Try
            Dim oDividendosRebatesLiberadasDAM As New DividendosRebatesLiberadasDAM
            Insertado = oDividendosRebatesLiberadasDAM.InsertarRebateDetalle(CodigoNemonico, ImporteInicio, ImporteFin, PorRebate, Situacion, UsuarioCreacion, CodigoDetalle)
        Catch ex As Exception
            Throw ex
        End Try
        Return Insertado
    End Function

    Public Function EliminarRebateDetalle(ByVal CodigoDetalle As Integer, ByVal dataRequest As DataSet) As Boolean
        Dim Insertado As Boolean = False
        Try
            Dim oDividendosRebatesLiberadasDAM As New DividendosRebatesLiberadasDAM
            Insertado = oDividendosRebatesLiberadasDAM.EliminarRebateDetalle(CodigoDetalle)
        Catch ex As Exception
            Throw ex
        End Try
        Return Insertado
    End Function

    Public Function ObtenerCabeceraCodigoRebateDetalle(ByVal codigoNemonico As String) As DataTable
        Try
            Return New DividendosRebatesLiberadasDAM().ObtenerCabeceraCalculoRebateDetalle(codigoNemonico)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function EliminarRebateCabecera(ByVal CodigoNemonico As String, ByVal dataRequest As DataSet) As Boolean
        Dim Insertado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoNemonico, dataRequest}
        Try
            Dim oDividendosRebatesLiberadasDAM As New DividendosRebatesLiberadasDAM
            Insertado = oDividendosRebatesLiberadasDAM.EliminarRebateCabecera(CodigoNemonico)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return Insertado
    End Function

    Public Function ObtenerCodigoRebateDetalleCabecera(ByVal codigoNemonico As String) As DataTable
        Try
            Return New DividendosRebatesLiberadasDAM().ObtenerCalculoRebateDetalleCabecera(codigoNemonico)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function DividendosRebatesLiberadas_ObtenerMontoTotalDividendos(ByVal p_CodigoPortafolio As String, ByVal p_FechaOperacion As Decimal) As DataTable
        Try
            Dim objDRLDAM As New DividendosRebatesLiberadasDAM
            Return objDRLDAM.DividendosRebatesLiberadas_ObtenerMontoTotalDividendos(p_CodigoPortafolio, p_FechaOperacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function DividendosRebatesLiberadas_ObtenerMontosDividendosDetallado(ByVal p_CodigoPortafolio As String, ByVal p_FechaOperacion As Decimal) As DataTable
        Try
            Dim objDRLDAM As New DividendosRebatesLiberadasDAM
            Return objDRLDAM.DividendosRebatesLiberadas_ObtenerMontosDividendosDetallado(p_CodigoPortafolio, p_FechaOperacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT10916 Fin
    'OT10927 - 21/11/2017 - Hanz Cocchi. Obtener montos para el reporte de rentabilidad de flujos
    Public Function DividendosRebatesLiberadas_ObtenerMontosDividendosDetallado_Flujos(ByVal p_CodigoPortafolio As String, ByVal p_FechaOperacion As Decimal) As DataTable
        Try
            Dim objDRLDAM As New DividendosRebatesLiberadasDAM
            Return objDRLDAM.DividendosRebatesLiberadas_ObtenerMontosDividendosDetallado_Flujo(p_CodigoPortafolio, p_FechaOperacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ConsultarDistribucionLib(ByVal p_CodigoPortafolio As String, ByVal p_FechaOperacion As Decimal) As DataTable
        Try
            Dim objDRLDAM As New DividendosRebatesLiberadasDAM
            Return objDRLDAM.ConsultarDistribucionLib(p_CodigoPortafolio, p_FechaOperacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT10927 - Fin

    'OT12012
    Public Function ObtenerSaldo_NemonicoPortafolioFecha(ByVal CodigoNemonico As String, ByVal FechaCorte As Decimal, ByVal Portafolio As String) As Decimal
        Dim cuponera As DataSet
        Dim oDRL As New DividendosRebatesLiberadasDAM
        Dim rpta As Decimal = 0
        Try
            rpta = oDRL.ObtenerSaldo_NemonicoPortafolioFecha(CodigoNemonico, FechaCorte, Portafolio)
        Catch ex As Exception
            Throw ex
        End Try
        Return rpta
    End Function
    'Fin OT12012
End Class