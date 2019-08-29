Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class LimiteBM
    'Inherits InvokerCOM

    Public Sub New()

    End Sub


#Region " /* Funciones Seleccionar */ "



    Public Function SeleccionarPorFiltro(ByVal StrCodigoLimite As String, ByVal StrNombreLimite As String, ByVal StrSituacion As String, ByVal dataRequest As DataSet) As LimiteBE
        Try
            Dim oLimite As LimiteBE
            oLimite = New LimiteDAM().SeleccionarPorFiltro(StrCodigoLimite, StrNombreLimite, StrSituacion, dataRequest)
            Return oLimite
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ConsultarLimites(ByVal TipoLimite As String, ByVal Mnemonico As String, ByVal OpcionLimite As String, ByVal Fecha As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim oLimite As New DataSet
        Try
            Dim oLimiteDAM As New LimiteDAM
            oLimite = oLimiteDAM.ConsultarLimites(TipoLimite, Mnemonico, OpcionLimite, Fecha, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return oLimite
    End Function

    Public Function SeleccionarPorInstrumento(ByVal codigoNemonico As String, ByVal dataRequest As DataSet) As LimiteBE 'Implements ILimiteBM.SeleccionarPorInstrumento
        Try
            Dim oLimite As LimiteBE
            oLimite = New LimiteDAM().SeleccionarPorInstrumento(codigoNemonico, dataRequest)
            Return oLimite
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Seleccionar(ByVal codigoLimite As String, ByVal dataRequest As DataSet) As LimiteBE
        Try
            Return New LimiteDAM().Seleccionar(codigoLimite, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub Insertar_Condicion_LimiteNivel(ByVal CodigoNivelLimite As String, ByVal Secuencial As Integer, ByVal Condicion As String, ByVal PorcentajeMin As Decimal, ByVal PorcentajeMax As Decimal)
        Try
            Dim oLimiteDA As New LimiteDAM
            oLimiteDA.Insertar_Condicion_LimiteNivel(CodigoNivelLimite, Secuencial, Condicion, PorcentajeMin, PorcentajeMax)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub Eliminar_Condicion_LimiteNivel(ByVal CodigoNivelLimite As String)
        Try
            Dim oLimiteDA As New LimiteDAM
            oLimiteDA.Eliminar_Condicion_LimiteNivel(CodigoNivelLimite)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function Seleccionar_Condicion_LimiteNIvel(ByVal CodigoNivelLimite As String) As DataTable
        Try
            Dim oLimiteDA As New LimiteDAM
            Return oLimiteDA.Seleccionar_Condicion_LimiteNIvel(CodigoNivelLimite)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SeleccionarCaracteristicas(ByVal codigoLimite As String, ByVal dataRequest As DataSet) As DataTable
        Try
            Return New LimiteDAM().SeleccionarCaracteristicas(codigoLimite, dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarCaracteristicasCompuestas(ByVal codigoLimite As String, ByVal dataRequest As DataSet) As DataTable
        Try
            Return New LimiteDAM().SeleccionarCaracteristicasCompuestas(codigoLimite, dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'Public Function SeleccionarPortafolios(ByVal codigoLimite As String, ByVal dataRequest As DataSet) As DataTable
    '    Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
    '    Dim parameters As Object() = {codigoLimite, dataRequest}
    '    Dim DtPortafolios As New DataTable
    '    Try
    '        RegistrarAuditora(parameters)
    '        DtPortafolios = New LimiteDAM().SeleccionarPortafolios(codigoLimite, dataRequest).Tables(0)


    '    Catch ex As Exception
    '        'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
    '        RegistrarAuditora(parameters, ex)
    '        'Las siguientes 4 líneas deben agregarse para el Exception app block
    '        Dim rethrow As Boolean = True 'true
    '        If (rethrow) Then
    '            Throw
    '        End If
    '    End Try
    '    Return DtPortafolios
    'End Function
    Public Function SeleccionarNegocios(ByVal codigoLimite As String, ByVal dataRequest As DataSet) As DataTable
        Dim DtNegocios As New DataTable
        Try
            DtNegocios = New LimiteDAM().SeleccionarNegocios(codigoLimite, dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
        Return DtNegocios
    End Function
    Public Function SeleccionarCaracteristicasNiveles(ByVal StrCodigoLimiteCaracteristica As String, ByVal dataRequest As DataSet) As DataTable
        Dim obj As DataTable
        Try
            obj = New LimiteDAM().SeleccionarCaracteristicasNiveles(StrCodigoLimiteCaracteristica, dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
        Return obj
    End Function
    Public Function SeleccionarCaracteristicasDetalleNiveles(ByVal StrCodigoNivelLimite As String, ByVal FlagTipoPorcentaje As String, ByVal dataRequest As DataSet) As DataTable
        Dim obj As DataTable
        Try
            obj = New LimiteDAM().SeleccionarCaracteristicasDetalleNiveles(StrCodigoNivelLimite, FlagTipoPorcentaje, dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
        Return obj
    End Function
    Public Function SeleccionarCaracteristicasDetalleNiveles(ByVal StrCodigoNivelLimite As String, ByVal FlagTipoPorcentaje As String, ByVal CodCaracteristica As String) As DataTable
        Dim obj As DataTable
        Try
            obj = New LimiteDAM().SeleccionarCaracteristicasDetalleNiveles(StrCodigoNivelLimite, FlagTipoPorcentaje, CodCaracteristica).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
        Return obj
    End Function
    Public Function SeleccionarGrupoNivel(ByVal StrCodigoGrupoNivel As String, ByVal dataRequest As DataSet) As DataTable
        Dim obj As DataTable
        Try
            obj = New LimiteDAM().SeleccionarGrupoNivel(StrCodigoGrupoNivel, dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
        Return obj
    End Function

    Public Function SeleccionarCaracteristicaGrupo(ByVal dataRequest As DataSet) As DataTable
        Dim obj As DataTable
        Try
            obj = New LimiteDAM().SeleccionarCaracteristicaGrupo(dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
        Return obj
    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As LimiteBE
        Try
            Return New LimiteDAM().Listar(dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'REQ 66768 JCH 20130201
    Public Function SeleccionarCaracteristicasDetalleEstilo(ByVal codNivelLimite As String, ByVal valCaracter As String, ByVal dataRequest As DataSet) As DataTable
        Dim obj As DataTable
        Try
            obj = New LimiteDAM().SeleccionarCaracteristicasDetalleEstilo(codNivelLimite, valCaracter, dataRequest).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
        Return obj
    End Function
#End Region

#Region " /* Funciones Insertar */ "
    Public Function Insertar(ByVal oLimiteBE As LimiteBE, ByVal oValidadorLimiteDetalle As ListValidadorLimite, ByVal dataRequest As DataSet) As String
        Dim codigo As String = String.Empty
        Try
            Dim oLimiteDAM As New LimiteDAM
            codigo = oLimiteDAM.Insertar(oLimiteBE, dataRequest)
            Dim oRow As LimiteBE.LimiteRow
            oRow = CType(oLimiteBE.Limite.Rows(0), LimiteBE.LimiteRow)
            oLimiteDAM.EliminarValidadorLimiteDetalle(oRow.CodigoLimite)
            oLimiteDAM.InsertarValidadorLimiteDetalle(oValidadorLimiteDetalle, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return codigo
    End Function
    Public Function InsertarNegocios(ByVal CodigoLimite As String, ByVal CodigoNegocio As String, ByVal Situacion As String, ByVal dataRequest As DataSet) As String
        Dim codigo As String = String.Empty
        Try
            Dim oLimiteDAM As New LimiteDAM
            codigo = oLimiteDAM.InsertarNegocios(CodigoLimite, CodigoNegocio, Situacion, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return codigo
    End Function
    Public Function ModificarNegocios(ByVal CodigoLimite As String, ByVal CodigoNegocio As String, ByVal Situacion As String, ByVal dataRequest As DataSet) As String
        Dim codigo As String = String.Empty
        Try
            Dim oLimiteDAM As New LimiteDAM
            codigo = oLimiteDAM.ModificarNegocios(CodigoLimite, CodigoNegocio, Situacion, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return codigo
    End Function
    Public Function EliminarNegocios(ByVal CodigoLimite As String, ByVal dataRequest As DataSet) As String
        Dim codigo As String = String.Empty
        Try
            Dim oLimiteDAM As New LimiteDAM
            codigo = oLimiteDAM.EliminarNegocios(CodigoLimite, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return codigo
    End Function
    Public Function InsertarCaracteristicas(ByVal id As String, ByVal situacion As String, _
    ByVal dtDetalle As DataTable, ByVal ListaCaracteristicas As Hashtable, _
    ByVal ListaDetalleNivelLimite As Hashtable, ByVal dataRequest As DataSet, ByVal codigoFondo As String,
    ByVal dtblCondiciones As DataTable) As String
        Dim codigo As String = String.Empty
        Try
            Dim oLimiteDAM As New LimiteDAM
            codigo = oLimiteDAM.InsertarCaracteristicas(id, situacion, dtDetalle, ListaCaracteristicas, ListaDetalleNivelLimite, dataRequest, codigoFondo, dtblCondiciones)
        Catch ex As Exception
            Throw ex
        End Try
        Return codigo
    End Function
#End Region
#Region " /* Funciones Modificar */"


    Public Function Modificar(ByVal oLimiteBE As LimiteBE, ByVal oValidadorLimiteDetalle As ListValidadorLimite, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Try
            Dim oLimiteDAM As New LimiteDAM
            actualizado = oLimiteDAM.Modificar(oLimiteBE, dataRequest)
            Dim oRow As LimiteBE.LimiteRow
            oRow = CType(oLimiteBE.Limite.Rows(0), LimiteBE.LimiteRow)
            oLimiteDAM.EliminarValidadorLimiteDetalle(oRow.CodigoLimite)
            oLimiteDAM.InsertarValidadorLimiteDetalle(oValidadorLimiteDetalle, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return actualizado
    End Function
    Public Function ModificarCaracteristicas(ByVal id As String, ByVal situacion As String, _
    ByVal dtDetalle As DataTable, ByVal ListaCaracteristicas As Hashtable, _
    ByVal ListaDetalleNivelLimite As Hashtable, ByVal dataRequest As DataSet, ByVal codigoFondo As String,
    ByVal dtblCondiciones As DataTable, ByVal CodigoRelacion As String) As Boolean
        Dim actualizado As Boolean = False
        Try
            Dim oLimiteDAM As New LimiteDAM
            actualizado = oLimiteDAM.ModificarCaracteriticas(id, situacion, dtDetalle, ListaCaracteristicas, ListaDetalleNivelLimite, dataRequest, codigoFondo, dtblCondiciones, CodigoRelacion)
        Catch ex As Exception
            Throw ex
        End Try
        Return actualizado
    End Function

    'CMB OT 62087 20110202 Nro 22 
    Public Function ActualizarPorcentajePorTipoInstrumento(ByVal valorCaracteristica As String, ByVal valorPorcentaje As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Try
            Dim oLimiteDAM As New LimiteDAM
            actualizado = oLimiteDAM.ActualizarPorcentajePorTipoInstrumento(valorCaracteristica, valorPorcentaje, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return True
    End Function
#End Region
#Region " /* Funciones Eliminar */"

    Public Function Eliminar(ByVal codigoLimite As String, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet)
        Dim eliminado As Boolean = False
        Try
            Dim oLimiteDAM As New LimiteDAM
            eliminado = oLimiteDAM.Eliminar(codigoLimite, dtDetalle, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return eliminado
    End Function


    Public Sub EliminarDetalleNivelLimite(ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet)
        Try
            Dim oLimiteDAM As New LimiteDAM
            oLimiteDAM.EliminarDetalleNivelLimite(dtDetalle, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "/*Incremento-Disminucion de Patrimonio (Req40) LETV 20090414*/"

    Public Function SeleccionarIncDecPatrimonio(ByVal codigoCategoriaIncDec As String) As DataTable
        Try
            Dim dt As DataTable
            dt = New LimiteDAM().SeleccionarIncDecPatrimonio(codigoCategoriaIncDec)
            Return dt
        Catch ex As Exception
            Dim rethrow As Boolean = True 'true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function SeleccionarPorFondoIncDecPatrimonio(ByVal codigoPortafolio As String, ByVal FechaOperacion As Decimal) As DataTable
        Try
            Dim dt As DataTable
            dt = New LimiteDAM().SeleccionarPorFondoIncDecPatrimonio(codigoPortafolio, FechaOperacion)
            Return dt
        Catch ex As Exception
            Dim rethrow As Boolean = True 'true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function ValidarIncDecPatrimonio(ByVal codigoCategoriaIncDec As String, ByVal codigoPortafolio As String) As Boolean
        Try
            Dim resultado As Boolean
            resultado = New LimiteDAM().ValidarIncDecPatrimonio(codigoCategoriaIncDec, codigoPortafolio)
            Return resultado
        Catch ex As Exception
            Dim rethrow As Boolean = True 'true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function InsertarIncDecPatrimonio(ByVal codigoCategoriaIncDec As String, ByVal codigoPortafolio As String, ByVal valor As Decimal, ByVal tipoIngreso As String) As Boolean
        Dim resultado As Boolean = False
        Try
            Dim oLimiteDAM As New LimiteDAM
            resultado = oLimiteDAM.InsertarIncDecPatrimonio(codigoCategoriaIncDec, codigoPortafolio, valor, tipoIngreso)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True 'true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return resultado
    End Function

    Public Function ModificarIncDecPatrimonio(ByVal CodigoIncDec As String, ByVal codigoPortafolio As String, ByVal valor As Decimal, ByVal tipoingreso As String) As Boolean
        Dim resultado As Boolean = False
        Try
            Dim oLimiteDAM As New LimiteDAM
            resultado = oLimiteDAM.ModificarIncDecPatrimonio(CodigoIncDec, codigoPortafolio, valor, tipoingreso)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True 'true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return resultado
    End Function

    Public Function EliminarIncDecPatrimonio(ByVal CodigoIncDec As String) As Boolean
        Dim resultado As Boolean = False
        Try
            Dim oLimiteDAM As New LimiteDAM
            resultado = oLimiteDAM.EliminarIncDecPatrimonio(CodigoIncDec)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True 'true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return resultado
    End Function
#End Region
#Region "Porcentaje cerca al limite OT 11517 LETV 20091014"
    Public Function ModificarPorcentajeCercaLimite(ByVal codigoLimite As String, ByVal codigoPortafolio As String, ByVal porcentaje As Decimal) As Boolean
        Dim resultado As Boolean = False
        Try
            Dim oLimiteDAM As New LimiteDAM
            resultado = oLimiteDAM.ModificarPorcentajeCercaLimite(codigoLimite, codigoPortafolio, porcentaje)
        Catch ex As Exception
            Throw ex
        End Try
        Return resultado
    End Function

    Public Function SeleccionarPorcentajeCercaLimite(ByVal codigoLimite As String, ByVal CodigoLimiteCaracteristica As String) As Decimal
        Dim resultado As Decimal
        Try
            Dim oLimiteDAM As New LimiteDAM
            resultado = oLimiteDAM.SeleccionarPorcentajeCercaLimite(codigoLimite, CodigoLimiteCaracteristica)
        Catch ex As Exception
            Throw ex
        End Try
        Return resultado
    End Function
#End Region
#Region "/*Usuarios Notificados HDG OT 61566 Nro3 20101027*/"

    Public Function SeleccionarUsuarioNotifica() As DataTable
        Try
            Dim dt As DataTable
            dt = New LimiteDAM().SeleccionarUsuarioNotifica()
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ListarUnidadesPuestos() As DataTable
        Try
            Dim dt As DataTable
            dt = New LimiteDAM().ListarUnidadesPuestos()
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SeleccionarPersonal(ByVal oUsuario As UsuariosNotificaBE) As DataTable
        Try
            Dim dt As DataTable
            dt = New LimiteDAM().SeleccionarPersonal(oUsuario)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function InsertarUsuarioNotifica(ByVal oUsuario As UsuariosNotificaBE, ByVal dataRequest As DataSet) As Boolean
        Dim resultado As Boolean = False
        Try
            Dim oLimiteDAM As New LimiteDAM
            resultado = oLimiteDAM.InsertarUsuarioNotifica(oUsuario, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return resultado
    End Function

    Public Function EliminarUsuarioNotifica(ByVal CodigoInterno As String, ByVal dataRequest As DataSet) As Boolean
        Dim resultado As Boolean = False
        Try
            Dim oLimiteDAM As New LimiteDAM
            resultado = oLimiteDAM.EliminarUsuarioNotifica(CodigoInterno, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return resultado
    End Function
#End Region
    Public Function LimiteMaximoNegociacion_Validar(ByVal Fecha As Decimal) As ArrayList
        Dim resultado As ArrayList
        Try
            Dim oLimiteDAM As New LimiteDAM
            resultado = oLimiteDAM.LimiteMaximoNegociacion_Validar(Fecha)
        Catch ex As Exception
            Throw ex
        End Try
        Return resultado
    End Function

    'HDG OT 62087 Nro5-R10 20110121
    Public Function ObtenerPorcentajeLimiteGen(ByVal CodigoLimite As String, ByVal Nivel As String) As DataTable
        Try
            Dim dt As DataTable
            dt = New LimiteDAM().ObtenerPorcentajeLimiteGen(CodigoLimite, Nivel)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'RGF 20110315 OT 62087 REQ6
    Public Sub RegistrarOrdenesPreviasSeleccionadas(ByVal tipoRenta As String, Optional ByVal decNProceso As Decimal = 0)  'HDG OT 67554 duplicado
        Try
            Dim oLimiteDAM As New LimiteDAM
            oLimiteDAM.RegistrarOrdenesPreviasSeleccionadas(tipoRenta, decNProceso)  'HDG OT 67554 duplicado
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    'CMB OT 62087 20110318 Nro 20
    Public Function ReporteEstructuraLimites(ByVal codigoLimite As String, ByVal dataRequest As DataSet) As DataSet
        Try
            Dim oLimiteDAM As New LimiteDAM
            Return oLimiteDAM.ReporteEstructuraLimites(codigoLimite)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'CMB OT 62679 20110331
    Public Function DefinirPorcentajeLimiteExteriorDetallado(ByVal valorCaracteristica As String, ByVal dataRequest As DataSet) As Boolean
        Try
            Dim oLimiteDAM As New LimiteDAM
            Return oLimiteDAM.DefinirPorcentajeLimiteExteriorDetallado(valorCaracteristica, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'CMB OT 63540 20110714
    Public Function SeleccionarAjusteLimitesEstimados(ByVal fechaOperacion As Decimal, ByVal codigoPortafolio As String, ByVal codigoLimite As String, ByRef totalAjustes As Decimal, ByVal dataRequest As DataSet) As DataSet
        Try
            Dim oLimiteDAM As New LimiteDAM
            Return oLimiteDAM.SeleccionarAjusteLimitesEstimados(fechaOperacion, codigoPortafolio, codigoLimite, totalAjustes)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'PLD OT 67244 20130502
    Public Function ObtenerLiquidezLimite(ByVal CodigoLimite As String) As DataTable
        Try
            Dim dt As DataTable
            dt = New LimiteDAM().ObtenerLiquidezLimite(CodigoLimite)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'PLD OT 67244 20130502
    'PLD OT 67244 20130503
    Public Sub ActualizarParametriaLiquidez(ByVal CodigoLimite As String, ByVal PorcentajeIL As String, ByVal PorcentajeLL As String, ByVal PorcentajeSL As String)
        Try
            Dim oLimiteDAM As New LimiteDAM
            oLimiteDAM.ActualizarParametriaLiquidez(CodigoLimite, PorcentajeIL, PorcentajeLL, PorcentajeSL)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'PLD OT 67244 20130503
    'PLD OT 67244 20130506

    Public Sub ActualizarPorcentajeLiquidez(ByVal CodigoLimite As String)
        Try
            Dim oLimiteDAM As New LimiteDAM
            oLimiteDAM.ActualizarPorcentajeLiquidez(CodigoLimite)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    'PLD OT 67244 20130506

    'BPesantes OT 9567 Inicio 12-09-2016
    Public Function ConsolidadoLimites(ByVal FechaOperacion As Decimal) As DataTable
        Try
            Dim Limites As New LimiteDAM
            Return Limites.ConsolidadoLimites(FechaOperacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ProcesarLimites(ByVal FechaOperacion As Decimal, Optional ByVal CodigoPortafolioSBS As String = "") As String
        Try
            Dim Limites As New LimiteDAM
            Return Limites.ProcesarLimites(FechaOperacion, CodigoPortafolioSBS)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ReporteLimites(ByVal FechaOperacion As Decimal, ByVal FechaCadena As String, ByVal Mandato As String, Optional ByVal CodigoPortafolio As String = "") As DataTable
        Try
            Dim Limites As New LimiteDAM
            Return Limites.ReporteLimites(FechaOperacion, FechaCadena, Mandato, CodigoPortafolio)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SeleccionarValidadorLimiteDetalle(ByVal objValidadorLimiteDetalle As ValidadorLimiteDetalleBE) As DataTable
        Try
            Return New LimiteDAM().SeleccionarValidadorLimiteDetalle(objValidadorLimiteDetalle)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 9755 Fin
End Class

