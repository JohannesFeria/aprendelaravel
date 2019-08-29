Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports MotorTransaccionesProxy
''' Clase para el acceso de los datos para Portafolio tabla.
Public Class PortafolioBM
    'Inherits InvokerCOM

    Public Sub New()
    End Sub

    ''' Inserta un expediente en Portafolio tabla.
    Public Function Insertar(ByVal oPortafolioBE As PortafolioBE, ByVal dataRequest As DataSet) As String
        Dim codigo As String
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            codigo = oPortafolioDAM.Insertar(oPortafolioBE, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return codigo
    End Function
    'OT11237 - 15/02/2018 - Ian Pastor M.
    'Descripción: Refactorizar código
    Public Function Eliminar_PorcentajeSeries(ByVal Portafolio As String, ByVal FechaProceso As Decimal) As Boolean
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            Return oPortafolioDAM.Eliminar_PorcentajeSeries(Portafolio, FechaProceso)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT11237 - Fin
    Public Function Modificar_PorcentajeSeries(ByVal Portafolio As String, ByVal CodigoSerie As String, ByVal FechaProceso As Decimal, ByVal Porcentaje As Decimal, _
                                               ByVal dataRequest As DataSet) As Boolean
        Dim codigo As String
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            codigo = oPortafolioDAM.Modificar_PorcentajeSeries(Portafolio, CodigoSerie, FechaProceso, Porcentaje, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return codigo
    End Function
    'OT11237 - 15/02/2018 - Ian Pastor M.
    'Descripción: Refactorizar código
    Public Function Insertar_PorcentajeSeries(ByVal Portafolio As String, ByVal CodigoSerie As String, ByVal FechaProceso As Decimal, ByVal Porcentaje As Decimal, _
                                              ByVal dataRequest As DataSet, Optional ByVal ValoresCierre As Decimal = 0, Optional ByVal CuotasCierre As Decimal = 0) As Boolean
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            Return oPortafolioDAM.Insertar_PorcentajeSeries(Portafolio, CodigoSerie, FechaProceso, Porcentaje, dataRequest, ValoresCierre, CuotasCierre)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT11237 - Fin
    Public Sub InsertarSeries(ByVal CodigoPortafolio As String, ByVal dtDetallePortafolio As DataTable)
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            oPortafolioDAM.EliminarSerie(CodigoPortafolio)
            For Each dtRow As DataRow In dtDetallePortafolio.Rows
                oPortafolioDAM.InsertarSeries(CodigoPortafolio, dtRow)
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function ValidarValorizacion(ByVal CodigoPortafolio As String, ByVal FechaValorizacion As Decimal) As Boolean
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            Return oPortafolioDAM.ValidarValorizacion(CodigoPortafolio, FechaValorizacion)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    'OT11237 - 15/02/2018 - Ian Pastor M.
    'Descripción: Refactorizar código
    Public Function Portafolio_Series_Cuotas(ByVal CodigoPortafolio As String, ByVal FechaValorizacion As Decimal) As DataTable
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            Return oPortafolioDAM.Portafolio_Series_Cuotas(CodigoPortafolio, FechaValorizacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT11237 - Fin
    'OT11143 - 05/03/2018 - Ian Pastor M.
    'Descripción: Obtener las series cuotas del fondo FIRBI
    Public Function Portafolio_Series_CuotasFirbi(ByVal CodigoPortafolio As String, ByVal FechaValorizacion As Decimal) As DataTable
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            Return oPortafolioDAM.Portafolio_Series_CuotasFirbi(CodigoPortafolio, FechaValorizacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT11143 - Fin
    Public Function ListarSeries(ByVal CodigoPortafolio As String) As DataTable
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            Return oPortafolioDAM.ListarSeries(CodigoPortafolio)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    ''' Inserta el detalle del expediente en DetallePortafolio tabla.
    Public Function InsertarDetalle(ByVal id As String, ByVal dtDetallePortafolio As DataTable, ByVal dataRequest As DataSet) As String
        Dim codigo As String = String.Empty
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            codigo = oPortafolioDAM.InsertarDetalle(id, dtDetallePortafolio, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return codigo
    End Function
    ''' Selecciona un solo expediente de Portafolio tabla.
    Public Function Seleccionar(ByVal codigoPortafolio As String, ByVal dataRequest As DataSet) As PortafolioBE
        'OT10916 - 06/11/2017 - Ian Pastor M. Refactorizar y ordenar código.
        Try
            Return New PortafolioDAM().Seleccionar(codigoPortafolio, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Aperturar(ByVal codigoPortafolio As String, ByVal fechaApertura As Decimal, ByVal dataRequest As DataSet)
        Try
            Dim oPortafolio As New PortafolioDAM
            oPortafolio.Aperturar(codigoPortafolio, fechaApertura, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub ModificarCierreContable(ByVal codigoPortafolio As String, ByVal fechaCierre As Decimal, ByVal dataRequest As DataSet)
        Try
            Dim oPortafolio As New PortafolioDAM
            oPortafolio.ModificarCierreContable(codigoPortafolio, fechaCierre, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function Cerrar(ByVal codigoPortafolio As String, ByVal fechaApertura As Decimal, ByVal dataRequest As DataSet)
        Try
            Dim oPortafolio As New PortafolioDAM
            oPortafolio.Cerrar(codigoPortafolio, fechaApertura, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CerrarReproceso(ByVal codigoPortafolio As String, ByVal fechaCierre As Decimal, ByVal dataRequest As DataSet)
        Try
            Dim oPortafolio As New PortafolioDAM
            oPortafolio.CerrarReproceso(codigoPortafolio, fechaCierre, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ValidarCierre(ByVal codigoPortafolio As String) As Boolean
        Try
            Dim oPortafolio As New PortafolioDAM
            Return oPortafolio.ValidarCierre(codigoPortafolio)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function ValidarApertura(ByVal codigoPortafolio As String) As Boolean
        Try
            Dim oPortafolio As New PortafolioDAM
            Return oPortafolio.ValidarApertura(codigoPortafolio)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    ''' Selecciona el detalle del expediente de DetallePortafolio tabla.
    Public Function SeleccionarDetalle(ByVal codigoPortafolio As String, ByVal dataRequest As DataSet) As DetallePortafolioBE
        Try
            Return New PortafolioDAM().SeleccionarDetalle(codigoPortafolio, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    ''' Selecciona el detalle del expediente por Codigo de portafolio y por tercero
    Public Function SeleccionarDetallePorTercero(ByVal codigoPortafolio As String, ByVal codigoClaseCuenta As String, ByVal codigoTercero As String, ByVal codigoMercado As String, ByVal dataRequest As DataSet) As DetallePortafolioBE
        Try
            'Return New PortafolioDAM().SeleccionarDetalle(codigoPortafolio, dataRequest)
            Return New PortafolioDAM().SeleccionarDetallePorFiltro(codigoPortafolio, codigoClaseCuenta, codigoTercero, "", codigoMercado)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarDetallePorFiltro(ByVal codigoPortafolio As String, ByVal CodigoClaseCuenta As String, ByVal CodigoTercero As String, ByVal CodigoMoneda As String, ByVal codigoMercado As String) As DetallePortafolioBE
        Try
            Return New PortafolioDAM().SeleccionarDetallePorFiltro(codigoPortafolio, CodigoClaseCuenta, CodigoTercero, CodigoMoneda, codigoMercado)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function SeleccionarBancoPorNumeroCuenta(ByVal NumeroCuenta As String, ByVal codigoPortafolio As String) As TercerosBE
        Try
            Return New PortafolioDAM().SeleccionarBancoPorNumeroCuenta(NumeroCuenta, codigoPortafolio)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    ''' Selecciona expedientes por Filtro
    Public Function SeleccionarPorFiltros(ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As PortafolioBE 'DataSet
        Try
            Return New PortafolioDAM().SeleccionarPorFiltros(descripcion, situacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarPortafolioPorFiltro(ByVal strFondo As String, ByVal dataRequest As DataSet) As DataSet
        Dim oPort As DataSet
        Try
            oPort = New PortafolioDAM().SeleccionarPortafolioPorFiltro(strFondo)
        Catch ex As Exception
            Throw ex
        End Try
        Return oPort
    End Function
    Public Function ObtenerDatosPortafolio(ByVal dataRequest As DataSet, Optional ByVal situacion As String = "") As PortafolioBE
        Dim parameters As Object() = {dataRequest, situacion}
        Try
            Dim dsPortafolio As PortafolioBE = New PortafolioDAM().ObtenerDatosPortafolio(situacion)
            Return dsPortafolio
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function FechaMaximaPortafolio() As DataTable
        Try
            Return New PortafolioDAM().FechaMaximaPortafolio()
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Listar(ByVal dataRequest As DataSet, Optional ByVal situacion As String = "", Optional ByVal CodigoNegocio As String = "") As PortafolioBE
        Try
            Dim dsPortafolio As PortafolioBE = New PortafolioDAM().Listar(situacion, CodigoNegocio)
            Return dsPortafolio
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function PortafolioCodigoListar(ByVal portafolio As String, Optional ByVal s_Parametro As String = "S", Optional ByVal porSerie As String = "", Optional ByVal estado As String = "") As DataTable
        Try
            Return New PortafolioDAM().PortafolioCodigoListar(portafolio, s_Parametro, porSerie, estado)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function PortafolioCodigoListar_ValoresSerie(ByVal portafolio As String) As DataTable
        Try
            Return New PortafolioDAM().PortafolioCodigoListar_ValoresSerie(portafolio)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    ''' Midifica un expediente en Portafolio tabla.
    Public Function Modificar(ByVal oPortafolioBE As PortafolioBE, ByVal dataRequest As DataSet) As String
        Dim actualizado As Boolean = False
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            actualizado = oPortafolioDAM.Modificar(oPortafolioBE, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return actualizado
    End Function
    Public Function ModificarDetalle(ByVal id As String, ByVal dtTablaDetalle As DataTable, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            actualizado = oPortafolioDAM.ModificarDetalle(id, dtTablaDetalle, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return actualizado
    End Function
    ''' Elimina un expediente de Portafolio table por una llave primaria compuesta.
    Public Function Eliminar(ByVal codigoPortafolio As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            eliminado = oPortafolioDAM.Eliminar(codigoPortafolio, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return eliminado
    End Function
    ''' Elimina el detalle de un expediente de DetallePortafolio table por una llave primaria compuesta.
    Public Function EliminarDetalle(ByVal codigoPortafolio As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            eliminado = oPortafolioDAM.EliminarDetalle(codigoPortafolio, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return eliminado
    End Function

    '#Region " /* Funciones Personalizadas*/"
    '    Public Sub Salir(ByVal dataRequest As DataSet)
    '        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
    '        Dim parameters As Object() = {dataRequest}
    '        RegistrarAuditora(parameters)
    '    End Sub
    '    Public Sub Actualizar(ByVal dataRequest As DataSet)
    '        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
    '        Dim parameters As Object() = {dataRequest}
    '        RegistrarAuditora(parameters)
    '    End Sub
    '    Public Sub Ingresar(ByVal dataRequest As DataSet)
    '        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
    '        Dim parameters As Object() = {dataRequest}
    '        RegistrarAuditora(parameters)
    '    End Sub
    '#End Region

    Public Function SeleccionarPortafolioPorCustodioValores(ByVal dataRequest As DataSet) As DataSet
        Try
            Return New PortafolioDAM().SeleccionarPortafolioPorCustodioValores(dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarCustodioPorPortafolio(ByVal CodigoPortafolioSBS As String, ByVal dataRequest As DataSet) As DataSet
        Try
            Return New PortafolioDAM().SeleccionarCustodioPorPortafolio(CodigoPortafolioSBS, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarCodigoNemonicoPorCustodioPortafolio(ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String, ByVal dataRequest As DataSet) As DataSet
        Try
            Return New PortafolioDAM().SeleccionarCodigoNemonicoPorCustodioPortafolio(CodigoPortafolioSBS, CodigoCustodio, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarCodigoISINPorCustodioPortafolio(ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String, ByVal dataRequest As DataSet) As DataSet
        Try
            Return New PortafolioDAM().SeleccionarCodigoISINPorCustodioPortafolio(CodigoPortafolioSBS, CodigoCustodio, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListaOrdenesFaltantes(ByVal codigoPortafolio As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            oreporte = oPortafolioDAM.ListarOrdenesFaltantes(codigoPortafolio)
        Catch ex As Exception
            Throw ex
        End Try
        Return oreporte
    End Function
    Public Function ListarOrdenesFaltantesValoracion(ByVal codigoPortafolio As String, ByVal fecha As Decimal) As DataSet
        Dim oreporte As New DataSet
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            oreporte = oPortafolioDAM.ListarOrdenesFaltantesValoracion(codigoPortafolio, fecha)
        Catch ex As Exception
            Throw ex
        End Try
        Return oreporte
    End Function
    Public Function ListaOrdenesFaltantesApertura(ByVal codigoPortafolio As String, ByVal datarequest As DataSet) As DataSet
        Dim oreporte As New DataSet
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            oreporte = oPortafolioDAM.ListarOrdenesFaltantesApertura(codigoPortafolio)
        Catch ex As Exception
            Throw ex
        End Try
        Return oreporte
    End Function
    Public Function ListarPortafolioPorUsuario(ByVal loginUsuario As String) As DataTable
        Try
            Dim dtPortafolio As DataTable = New PortafolioDAM().ListarPortafolioPorUsuario(loginUsuario)
            Return dtPortafolio
        Catch ex As Exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function ValidarCuponesLibor(ByVal codigoPortafolio As String, ByVal fechaApertura As Decimal) As DataSet
        Try
            Dim dtPortafolio As DataSet = New PortafolioDAM().ValidarCuponesLibor(codigoPortafolio, fechaApertura)
            Return dtPortafolio
        Catch ex As Exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function ValidarFechasLibor(ByVal fechaApertura As Decimal) As String
        Try
            Dim Validacion As String = New PortafolioDAM().ValidarFechasLibor(fechaApertura)
            Return Validacion
        Catch ex As Exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function ListarFechaNegociacion() As Hashtable
        Try
            Dim dtPortafolio As Hashtable = New PortafolioDAM().ListarFechaNegociacion()
            Return dtPortafolio
        Catch ex As Exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    'No debe de existir DRL sin confirmar al momento de realizar la valoracion estimada
    Public Function ListarOrdenesFaltantesValoracionEstimada(ByVal codigoPortafolio As String, ByVal fecha As Decimal) As DataSet
        Try
            Dim dtPortafolio As DataSet = New PortafolioDAM().ListarOrdenesFaltantesValoracionEstimada(codigoPortafolio, fecha)
            Return dtPortafolio
        Catch ex As Exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function SeleccionarDetalleGrupoPortafolio(ByVal codigoPortafolio As String) As DataSet
        Try
            Dim dsPortafolio As DataSet = New PortafolioDAM().SeleccionarDetalleGrupoPortafolio(codigoPortafolio)
            Return dsPortafolio
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function InsertarDetalleGrupoPortafolio(ByVal codigoPortafolioPadre As String, ByVal codigoPortafolioHijo As String)
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            oPortafolioDAM.InsertarDetalleGrupoPortafolio(codigoPortafolioPadre, codigoPortafolioHijo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    '-- INICIO | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio
    Public Sub InsertarSecuenciaVector(ByVal codigoPortafolio As String, ByVal tipoVector As String, ByVal fuenteVector As String, ByVal secuencia As Integer)
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            oPortafolioDAM.InsertarSecuenciaVector(codigoPortafolio, tipoVector, fuenteVector, secuencia)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub EliminarSecuenciaVector(ByVal codigoPortafolio As String, ByVal tipoVector As String)
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            oPortafolioDAM.EliminarSecuenciaVector(codigoPortafolio, tipoVector)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function ListarSecuenciaVector(ByVal codigoPortafolio As String, ByVal tipoVector As String) As DataTable
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            Return oPortafolioDAM.ListarSecuenciaVector(codigoPortafolio, tipoVector)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    '-- FIN | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio

    Public Function EliminarDetalleGrupoPortafolio(ByVal codigoPortafolioPadre As String)
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            oPortafolioDAM.EliminarDetalleGrupoPortafolio(codigoPortafolioPadre)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Actualiza_FechaNegocio(ByVal CodigoPortafolioSBS As String, ByVal FechaNegocio As Decimal)
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            oPortafolioDAM.Actualiza_FechaNegocio(CodigoPortafolioSBS, FechaNegocio)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ValidaIndicadores(ByVal FechaNegocio As Decimal) As DataTable
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            Return oPortafolioDAM.ValidaIndicadores(FechaNegocio)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub AperturaCajaRecaudo(ByVal CodigoPortafolio As String, ByVal CodigoClaseCuenta As String, ByVal FechaCajaOperaciones As Decimal, ByVal dataRequest As DataSet)
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            oPortafolioDAM.AperturaCajaRecaudo(CodigoPortafolio, CodigoClaseCuenta, FechaCajaOperaciones, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub ReversaCajaRecaudo(ByVal CodigoPortafolio As String, ByVal CodigoClaseCuenta As String, ByVal FechaCajaOperaciones As Decimal)
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            oPortafolioDAM.ReversaCajaRecaudo(CodigoPortafolio, CodigoClaseCuenta, FechaCajaOperaciones)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub GeneraSaldoBanco(ByVal FechaSaldo As Decimal, ByVal decFechaAnterior As Decimal, ByVal CodigoPortafolio As String, ByVal ClaseCuenta As String, ByVal dataRequest As DataSet)
        Try
            Dim oPortafolioDAM As New PortafolioDAM
            oPortafolioDAM.GeneraSaldoBanco(FechaSaldo, decFechaAnterior, CodigoPortafolio, ClaseCuenta, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function PortafolioCodAuto() As String
        Dim objPortafolioDAM As New PortafolioDAM
        Dim dsPortafolio As String
        Try
            dsPortafolio = objPortafolioDAM.PortafolioCodAuto()
        Catch ex As Exception
            Throw ex
        End Try
        Return dsPortafolio
    End Function
    'OT 10238 - 08/05/2017 - Carlos Espejo
    'Descripcion: Lista Inconsistencia de valorización
    Public Function InconsisteciasValorizacion(ByVal CodigoPortafolio As String, ByVal FechaOperacion As Decimal) As DataTable
        Try
            Dim objPortafolioDAM As New PortafolioDAM
            Return objPortafolioDAM.InconsisteciasValorizacion(CodigoPortafolio, FechaOperacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Portafolio_ListarPortafolioMensual(ByVal p_CodigoPortafolio As String, ByVal p_ValoracionMensual As String, ByVal p_Estado As String) As DataTable
        Try
            Return New PortafolioDAM().Portafolio_ListarPortafolioMensual(p_CodigoPortafolio, p_ValoracionMensual, p_Estado)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'INICIO | PROYECTO SIT - ZOLUXIONES | DACV | RF009 - DESCRIPCION | 21/05/2018
    Public Function PortafolioSelectById(ByVal Idportafolio As String) As DataTable
        Try
            Return New PortafolioDAM().PortafolioSelectById(Idportafolio)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function


    Public Function PortafolioCodigoListarByNemonico(ByVal portafolio As String, ByVal Nemonico As String, Optional ByVal s_Parametro As String = "S", Optional ByVal porSerie As String = "", Optional ByVal estado As String = "") As DataTable
        Try
            Return New PortafolioDAM().PortafolioCodigoListarByNemonico(portafolio, Nemonico, s_Parametro, porSerie, estado)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    'FIN | PROYECTO SIT - ZOLUXIONES | DACV | RF009 - DESCRIPCION | 21/05/2018

    'INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF014 - DESCRIPCION | 12/06/2018

    Public Function ListarNemonicoXValorizar(ByVal portafolio As String, ByVal fecha As Decimal) As DataTable
        Try
            Return New PortafolioDAM().ListarNemonicoXValorizar(portafolio, fecha)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function VerificarVectorPrecio(ByVal fechaOperacion As Decimal) As DataTable
        Try
            Return New PortafolioDAM().VerificarVectorPrecio(fechaOperacion)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function CargarValorizador(ByVal fechaOperacion As Decimal, ByVal tipoNegocio As String, ByVal estado As String) As DataTable
        Try
            Return New PortafolioDAM().CargarValorizador(fechaOperacion, tipoNegocio, estado)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function CargarDatosComplementarios(ByVal portafolio As String, ByVal fecha As String) As DataSet
        Try
            Return New PortafolioDAM().CargarDatosComplementarios(portafolio, fecha)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function CargarDatos_InversionAnterior(ByVal portafolio As String, ByVal fecha As Decimal) As DataTable
        Try
            Return New PortafolioDAM().CargarDatos_InversionAnterior(portafolio, fecha)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    'FIN | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF014 - DESCRIPCION | 12/06/2018
    'INICIO | PROYECTO AUMENTO CAPITAL | rcolonia | Obtener Portafolio por Aumento de Capital | 18092018
    Public Function PortafolioListarbyAumentoCapital() As DataTable
        Try
            Return New PortafolioDAM().PortafolioListarbyAumentoCapital()
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    'FIN | PROYECTO AUMENTO CAPITAL | rcolonia | Obtener Portafolio por Aumento de Capital | 18092018
    Public Function PortafolioListar(ByVal portafolio As String, ByVal TipoNegocio As String, ByVal estado As String) As DataTable
        Try
            Return New PortafolioDAM().PortafolioListar(portafolio, TipoNegocio, estado)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function PortafolioListar(ByVal portafolio As String, ByVal TipoNegocio As String, ByVal estado As String, ByVal fondoCliente As String) As DataTable
        Try
            Return New PortafolioDAM().PortafolioListar(portafolio, TipoNegocio, estado, fondoCliente)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT12028 | OC11111 - Rcolonia | Zoluxiones | Función para validar cierre de caja con fecha de registro de pago de comisión
    Public Function CierreCajas_ValidarFechaIngresoPagoComision(ByVal CodigoPortafolioSBS As String, ByVal CodigoClaseCuenta As String, ByVal fechaActualCaja As Decimal) As Integer
        Dim objPortafolioDAM As New PortafolioDAM
        Try
            Return objPortafolioDAM.CierreCajas_ValidarFechaIngresoPagoComision(CodigoPortafolioSBS, CodigoClaseCuenta, fechaActualCaja)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT12028 | OC11111 - FIN

    Public Function PortafolioPorDescripcion(ByVal DescripcionPortafolio As String) As String
        Dim strPortafolio As String = ""
        Try
            strPortafolio = New PortafolioDAM().PortafolioPorDescripcion(DescripcionPortafolio)
        Catch ex As Exception
            Throw ex
        End Try
        Return strPortafolio
    End Function


End Class