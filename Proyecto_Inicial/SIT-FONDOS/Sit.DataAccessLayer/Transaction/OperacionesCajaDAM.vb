Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
''' <summary>
''' Clase para el acceso de los datos para OperacionesCaja tabla.
''' </summary>
Public Class OperacionesCajaDAM
    Public Sub New()
    End Sub
    ''' <summary>
    ''' Inserta un expediente en OperacionesCaja tabla.
    ''' <summary>
    ''' <param name="codigoOperacion"></param>
    ''' <param name="codigoMercado"></param>
    ''' <param name="codigoPortafolio"></param>
    ''' <param name="codigoTerceros"></param>
    ''' <param name="codigoClaseCuenta"></param>
    ''' <param name="numeroCuenta"></param>
    ''' <param name="codigoTipoOperacion"></param>
    ''' <param name="referencia"></param>
    ''' <param name="importe"></param>
    ''' <param name="usuarioCreacion"></param>
    ''' <param name="fechaCreacion"></param>
    ''' <param name="codigoMoneda"></param>
    ''' <param name="codigoIntermediario"></param>
    ''' <param name="tipoMovimiento"></param>
    ''' <param name="situacion"></param>
    ''' <param name="host"></param>
    ''' <param name="numeroCarta"></param>
    ''' <param name="codigoPortafolioDestino"></param>
    ''' <param name="numeroCuentaDestino"></param>
    ''' <param name="codigoClaseCuentaDestino"></param>
    ''' <param name="hora"></param>
    ''' <param name="codigoContacto"></param>
    ''' <param name="estado"></param>
    ''' <returns></returns>
    Public Sub Insertar(ByVal dsOperacionCaja As OperacionCajaBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim opCaja As OperacionCajaBE.OperacionCajaRow = dsOperacionCaja.OperacionCaja.Rows(0)
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_Insertar")
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, opCaja.CodigoMercado)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, opCaja.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, opCaja.CodigoOperacion)
            db.AddInParameter(dbCommand, "@p_NroCuenta", DbType.String, opCaja.NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoModalidadPago", DbType.String, opCaja.CodigoModalidadPago)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, opCaja.CodigoTerceroOrigen)
            db.AddInParameter(dbCommand, "@p_CodigoTerceroDestino", DbType.String, opCaja.CodigoTerceroDestino)
            db.AddInParameter(dbCommand, "@p_NroCuentaRef", DbType.String, opCaja.NumeroCuentaDestino)
            db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, opCaja.Referencia)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, opCaja.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, opCaja.Importe)
            db.AddInParameter(dbCommand, "@p_usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_CodigoEjecucion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "CodigoEjecucion"))
            db.AddInParameter(dbCommand, "@nvcCodigoModelo", DbType.String, opCaja.CodigoModelo)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    'OT10749 - Refactorizar código
    Public Function Insertar_FechaOperacion(ByVal dsOperacionCaja As OperacionCajaBE, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim opCaja As OperacionCajaBE.OperacionCajaRow = dsOperacionCaja.OperacionCaja.Rows(0)
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SP_SIT_OperacionesCaja_Insertar")
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, opCaja.CodigoMercado)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, opCaja.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, opCaja.CodigoOperacion)
            db.AddInParameter(dbCommand, "@p_NroCuenta", DbType.String, opCaja.NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoModalidadPago", DbType.String, opCaja.CodigoModalidadPago)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, opCaja.CodigoTerceroOrigen)
            db.AddInParameter(dbCommand, "@p_CodigoTerceroDestino", DbType.String, opCaja.CodigoTerceroDestino)
            db.AddInParameter(dbCommand, "@p_NroCuentaRef", DbType.String, opCaja.NumeroCuentaDestino)
            db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, opCaja.Referencia)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, opCaja.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, opCaja.Importe)
            db.AddInParameter(dbCommand, "@p_usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_CodigoEjecucion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "CodigoEjecucion"))
            db.AddInParameter(dbCommand, "@nvcCodigoModelo", DbType.String, opCaja.CodigoModelo)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, opCaja.FechaPago)
            If opCaja.PagoFechaComision <> 0 Then
                db.AddInParameter(dbCommand, "@p_PagoFechaComision", DbType.Int32, opCaja.PagoFechaComision)
            End If
            db.AddOutParameter(dbCommand, "@p_Result", DbType.String, 12)
            db.ExecuteNonQuery(dbCommand)
            Return CType(db.GetParameterValue(dbCommand, "@p_Result"), String)
        End Using
    End Function
    'OT10749 - Fin
    Public Sub InsertarTransferenciaInterna(ByVal dsOperacionCaja As OperacionCajaBE, TipoTransferencia As String, TranFictizia As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim opCaja As OperacionCajaBE.OperacionCajaRow = dsOperacionCaja.OperacionCaja.Rows(0)
        Using dbCommand As DbCommand = db.GetStoredProcCommand("TransferenciaInterna_Insertar")
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, opCaja.CodigoMercado)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, opCaja.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioDestino", DbType.String, opCaja.CodigoPortafolioSBSDestino)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, opCaja.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_NroCuenta", DbType.String, opCaja.NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_NroCuentaDestino", DbType.String, opCaja.NumeroCuentaDestino)
            db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, opCaja.Importe)
            db.AddInParameter(dbCommand, "@p_usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_CodigoEjecucion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "CodigoEjecucion"))
            db.AddInParameter(dbCommand, "@nvcCodigoModelo", DbType.String, opCaja.CodigoModelo)
            db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, opCaja.NumeroCarta)
            db.AddInParameter(dbCommand, "@p_fechaOperacion", DbType.Decimal, opCaja.FechaPago)
            db.AddInParameter(dbCommand, "@P_TipoTransferencia", DbType.String, TipoTransferencia)
            db.AddInParameter(dbCommand, "@P_TranFictizia", DbType.String, TranFictizia)
            db.AddInParameter(dbCommand, "@p_CodigoTerceroDestino", DbType.String, opCaja.CodigoTerceroDestino)
            'OT12012
            db.AddInParameter(dbCommand, "@p_ObservacionCarta", DbType.String, opCaja.ObservacionCartaDestino)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Function ValidarTransferenciaClaseCuenta(ByVal portafolioOrigen As String, ByVal potafolioDestino As String, ByVal numeroCuentaOrigen As String, ByVal numeroCuentaDestino As String) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Transferencia_ValidarClaseCuenta")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioOrigen", DbType.String, portafolioOrigen)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioDestino", DbType.String, potafolioDestino)
            db.AddInParameter(dbCommand, "@p_NumeroCuentaOrigen", DbType.String, numeroCuentaOrigen)
            db.AddInParameter(dbCommand, "@p_NumeroCuentaDestino", DbType.String, numeroCuentaDestino)
            Return CType(db.ExecuteScalar(dbCommand), Integer)
        End Using
    End Function
    Public Function SeleccionarPorFiltro(ByVal dsOperacionCaja As OperacionCajaBE, ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim opCaja As OperacionCajaBE.OperacionCajaRow = dsOperacionCaja.OperacionCaja.Rows(0)
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_SeleccionarPorFiltro")
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, opCaja.CodigoMercado)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, opCaja.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, opCaja.CodigoClaseCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, opCaja.CodigoTerceroOrigen)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, opCaja.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, opCaja.NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, opCaja.CodigoOperacion)
            db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.String, fechaInicio)
            db.AddInParameter(dbCommand, "@p_FechaFin", DbType.String, fechaFin)
            db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, opCaja.CodigoTipoOperacion)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Sub ModificarEstadoCarta(ByVal NumeroCarta As String, ByVal Estado As String, ByVal TipoEmision As String, ByVal abono As Boolean, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_ModificarEstadoCarta")
            db.AddInParameter(dbCommand, "@p_NumeroCarta", DbType.String, NumeroCarta)
            db.AddInParameter(dbCommand, "@p_Estado", DbType.String, Estado)
            db.AddInParameter(dbCommand, "@p_TipoEmision", DbType.String, TipoEmision)
            db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_abono", DbType.Int32, IIf(abono, 1, 0))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Sub AprobarCarta(ByVal NumeroCarta As String, ByVal Usuario As String, ByVal dataRequest As DataSet, ByVal abono As Boolean)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_AprobarCarta")
            db.AddInParameter(dbCommand, "@p_NumeroCarta", DbType.String, NumeroCarta)
            db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, Usuario)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_abono", DbType.Int32, IIf(abono, 1, 0))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Function SeleccionarCartas(ByVal dsOperacionCaja As OperacionCajaBE, ByVal abono As Boolean) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim opCaja As OperacionCajaBE.OperacionCajaRow = dsOperacionCaja.OperacionCaja.Rows(0)
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_SeleccionarCartas")
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, opCaja.CodigoMercado)
            db.AddInParameter(dbCommand, "@p_Estado", DbType.String, opCaja.EstadoCarta)
            db.AddInParameter(dbCommand, "@p_TipoEmision", DbType.String, opCaja.TipoEmisionCarta)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, opCaja.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoTerceroBanco", DbType.String, opCaja.CodigoTerceroOrigen)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, opCaja.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, opCaja.FechaCreacion)
            db.AddInParameter(dbCommand, "@p_abono", DbType.Int32, IIf(abono, 1, 0))
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function SeleccionarPorFiltro2(ByVal codigoMercado As String, ByVal codigoPortafolio As String, ByVal codigoTercero As String, ByVal codigoTerceroBanco As String, _
         ByVal fecha As Decimal, ByVal generar As Boolean, ByVal abono As Boolean, ByVal impreso As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_SeleccionarPorFiltro3")
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
            db.AddInParameter(dbCommand, "@p_CodigoTerceroBanco", DbType.String, codigoTerceroBanco)
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.String, fecha)
            db.AddInParameter(dbCommand, "@p_Generar", DbType.Int32, IIf(generar, 1, 0))
            db.AddInParameter(dbCommand, "@p_abono", DbType.Int32, IIf(abono, 1, 0))
            db.AddInParameter(dbCommand, "@p_Impreso", DbType.String, impreso)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function SeleccionarPorFiltro3(ByVal dsOperacionCaja As OperacionCajaBE, ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim opCaja As OperacionCajaBE.OperacionCajaRow = dsOperacionCaja.OperacionCaja.Rows(0)
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCajaSinonimo_SeleccionarPorFiltro")
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, opCaja.CodigoMercado)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, opCaja.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, opCaja.CodigoClaseCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, opCaja.CodigoTerceroOrigen)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, opCaja.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, opCaja.NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, opCaja.CodigoOperacion)
            db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.String, fechaInicio)
            db.AddInParameter(dbCommand, "@p_FechaFin", DbType.String, fechaFin)
            db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, opCaja.CodigoTipoOperacion)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function SeleccionarAutorizacionCartas(ByVal codigoMercado As String, ByVal codigoPortafolio As String, ByVal codigoTercero As String, ByVal codigoTerceroBanco As String, _
         ByVal fecha As Decimal, ByVal abono As Boolean, ByVal estado As String, ByVal codigoOperacionCaja As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_SeleccionarPorFiltro_AutorizacionCartas")
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
            db.AddInParameter(dbCommand, "@p_CodigoTerceroBanco", DbType.String, codigoTerceroBanco)
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
            db.AddInParameter(dbCommand, "@p_abono", DbType.Boolean, abono)
            db.AddInParameter(dbCommand, "@p_EstadoCarta", DbType.String, estado)
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function SeleccionarCartasFirmadas(ByVal codigoMercado As String, ByVal codigoPortafolio As String, ByVal codigoTercero As String, ByVal codigoTerceroBanco As String, _
        ByVal fecha As Decimal, ByVal abono As Boolean, ByVal estado As String, ByVal codigoOperacionCaja As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_SeleccionarPorFiltro_ImpresionCartas")
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
            db.AddInParameter(dbCommand, "@p_CodigoTerceroBanco", DbType.String, codigoTerceroBanco)
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
            db.AddInParameter(dbCommand, "@p_abono", DbType.Boolean, abono)
            db.AddInParameter(dbCommand, "@p_EstadoCarta", DbType.String, estado)
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function ReporteInventarioCartas(ByVal fechaInicio As Decimal, ByVal fechaFinal As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_InventarioCartas_OperacionesCaja")
            db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, fechaInicio)
            db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, fechaFinal)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                ReporteInventarioCartas = ds
            End Using
        End Using
    End Function
    Public Function SeleccionarExtornos(ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal, ByVal motivo As String, ByVal estado As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_SeleccionarPorFiltro_OperacionesCajaExt")
            db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, fechaInicio)
            db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, fechaFin)
            db.AddInParameter(dbCommand, "@p_MotivoExtorno", DbType.String, motivo)
            db.AddInParameter(dbCommand, "@p_Estado", DbType.String, estado)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                SeleccionarExtornos = ds
            End Using
        End Using
    End Function
    Public Function ReporteExtornos(ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal, ByVal motivo As String, ByVal estado As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Reporte_OperacionesCajaExt")
            db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, fechaInicio)
            db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, fechaFin)
            db.AddInParameter(dbCommand, "@p_MotivoExtorno", DbType.String, motivo)
            db.AddInParameter(dbCommand, "@p_Estado", DbType.String, estado)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                ReporteExtornos = ds
            End Using
        End Using
    End Function
    Public Function Seleccionar(ByVal codigoOperacion As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_Seleccionar")
            db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codigoOperacion)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Seleccionar = ds
            End Using
        End Using
    End Function
    Public Function SeleccionarPorCodigoClaseCuentaDestino(ByVal codigoClaseCuentaDestino As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_SeleccionarPorCodigoClaseCuentaDestino")
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuentaDestino", DbType.String, codigoClaseCuentaDestino)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    ''' <summary>
    ''' Selecciona expedientes de OperacionesCaja tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoPortafolio"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoPortafolio(ByVal codigoPortafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_SeleccionarPorCodigoPortafolio")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    ''' <summary>
    ''' Selecciona expedientes de OperacionesCaja tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoTerceros"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoTerceros(ByVal codigoTerceros As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_SeleccionarPorCodigoTerceros")
            db.AddInParameter(dbCommand, "@p_CodigoTerceros", DbType.Decimal, codigoTerceros)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    ''' <summary>
    ''' Selecciona expedientes de OperacionesCaja tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoPortafolioDestino"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoPortafolioDestino(ByVal codigoPortafolioDestino As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_SeleccionarPorCodigoPortafolioDestino")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioDestino", DbType.String, codigoPortafolioDestino)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    ''' <summary>
    ''' Selecciona expedientes de OperacionesCaja tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoClaseCuenta"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoClaseCuenta(ByVal codigoClaseCuenta As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_SeleccionarPorCodigoClaseCuenta")
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, codigoClaseCuenta)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    ''' <summary>
    ''' Selecciona expedientes de OperacionesCaja tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="numeroCarta"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorNumeroCarta(ByVal numeroCarta As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_SeleccionarPorNumeroCarta")
            db.AddInParameter(dbCommand, "@p_NumeroCarta", DbType.String, numeroCarta)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    ''' <summary>
    ''' Selecciona expedientes de OperacionesCaja tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoIntermediario"></param>
    ''' <param name="codigoContacto"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoIntermediario_CodigoContacto(ByVal codigoIntermediario As String, ByVal codigoContacto As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_SeleccionarPorCodigoIntermediario_CodigoContacto")
            db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, codigoIntermediario)
            db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, codigoContacto)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    ''' <summary>
    ''' Selecciona expedientes de OperacionesCaja tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoMercado"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoMercado(ByVal codigoMercado As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_SeleccionarPorCodigoMercado")
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                SeleccionarPorCodigoMercado = ds
            End Using
        End Using
    End Function
    ''' <summary>
    ''' Selecciona expedientes de OperacionesCaja tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoTipoOperacion"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoTipoOperacion(ByVal codigoTipoOperacion As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_SeleccionarPorCodigoTipoOperacion")
            db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    ''' <summary>
    ''' Selecciona expedientes de OperacionesCaja tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoMoneda"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoMoneda(ByVal codigoMoneda As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_SeleccionarPorCodigoMoneda")
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                SeleccionarPorCodigoMoneda = ds
            End Using
        End Using
    End Function
    ''' <summary>
    ''' Lista todos los expedientes de OperacionesCaja tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_Listar")
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Listar = ds
            End Using
        End Using
    End Function
    ''' <summary>
    ''' Midifica un expediente en OperacionesCaja tabla.
    ''' <summary>
    ''' <param name="codigoOperacion"></param>
    ''' <param name="codigoMercado"></param>
    ''' <param name="codigoPortafolio"></param>
    ''' <param name="codigoTerceros"></param>
    ''' <param name="codigoClaseCuenta"></param>
    ''' <param name="numeroCuenta"></param>
    ''' <param name="codigoTipoOperacion"></param>
    ''' <param name="referencia"></param>
    ''' <param name="importe"></param>
    ''' <param name="usuarioCreacion"></param>
    ''' <param name="fechaCreacion"></param>
    ''' <param name="codigoMoneda"></param>
    ''' <param name="codigoIntermediario"></param>
    ''' <param name="tipoMovimiento"></param>
    ''' <param name="situacion"></param>
    ''' <param name="host"></param>
    ''' <param name="numeroCarta"></param>
    ''' <param name="codigoPortafolioDestino"></param>
    ''' <param name="numeroCuentaDestino"></param>
    ''' <param name="codigoClaseCuentaDestino"></param>
    ''' <param name="hora"></param>
    ''' <param name="codigoContacto"></param>
    ''' <param name="estado"></param>
    Public Function Modificar(ByVal codigoOperacion As String, ByVal codigoMercado As String, ByVal codigoPortafolio As String, ByVal codigoTerceros As Decimal, ByVal codigoClaseCuenta As String, ByVal numeroCuenta As Decimal, ByVal codigoTipoOperacion As String, ByVal referencia As String, ByVal importe As Decimal, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal codigoMoneda As String, ByVal codigoIntermediario As String, ByVal tipoMovimiento As String, ByVal situacion As String, ByVal host As String, ByVal numeroCarta As String, ByVal codigoPortafolioDestino As String, ByVal numeroCuentaDestino As String, ByVal codigoClaseCuentaDestino As String, ByVal hora As String, ByVal codigoContacto As String, ByVal estado As String) As Boolean
        Modificar = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_Modificar")
            db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codigoOperacion)
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_CodigoTerceros", DbType.Decimal, codigoTerceros)
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, codigoClaseCuenta)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.Decimal, numeroCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)
            db.AddInParameter(dbCommand, "@p_Referencia", DbType.String, referencia)
            db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, importe)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
            db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, codigoIntermediario)
            db.AddInParameter(dbCommand, "@p_TipoMovimiento", DbType.String, tipoMovimiento)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)
            db.AddInParameter(dbCommand, "@p_NumeroCarta", DbType.String, numeroCarta)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioDestino", DbType.String, codigoPortafolioDestino)
            db.AddInParameter(dbCommand, "@p_NumeroCuentaDestino", DbType.String, numeroCuentaDestino)
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuentaDestino", DbType.String, codigoClaseCuentaDestino)
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, hora)
            db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, codigoContacto)
            db.AddInParameter(dbCommand, "@p_Estado", DbType.String, estado)
            db.ExecuteNonQuery(dbCommand)
            Modificar = True
        End Using
    End Function
    ''' <summary>
    ''' Elimina un expediente de OperacionesCaja table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoOperacion As String) As Boolean
        Eliminar = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_Eliminar")
            db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codigoOperacion)
            db.ExecuteNonQuery(dbCommand)
            Eliminar = True
        End Using
    End Function
    ''' <summary>
    ''' Elimina un expediente de OperacionesCaja table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoClaseCuentaDestino(ByVal codigoClaseCuentaDestino As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_EliminarPorCodigoClaseCuentaDestino")
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuentaDestino", DbType.String, codigoClaseCuentaDestino)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    ''' <summary>
    ''' Elimina un expediente de OperacionesCaja table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoPortafolio(ByVal codigoPortafolio As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_EliminarPorCodigoPortafolio")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    ''' <summary>
    ''' Elimina un expediente de OperacionesCaja table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoTerceros(ByVal codigoTerceros As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_EliminarPorCodigoTerceros")
            db.AddInParameter(dbCommand, "@p_CodigoTerceros", DbType.Decimal, codigoTerceros)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    ''' <summary>
    ''' Elimina un expediente de OperacionesCaja table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoPortafolioDestino(ByVal codigoPortafolioDestino As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_EliminarPorCodigoPortafolioDestino")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioDestino", DbType.String, codigoPortafolioDestino)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    ''' <summary>
    ''' Elimina un expediente de OperacionesCaja table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoClaseCuenta(ByVal codigoClaseCuenta As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_EliminarPorCodigoClaseCuenta")
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, codigoClaseCuenta)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    ''' <summary>
    ''' Elimina un expediente de OperacionesCaja table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorNumeroCarta(ByVal numeroCarta As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_EliminarPorNumeroCarta")
            db.AddInParameter(dbCommand, "@p_NumeroCarta", DbType.String, numeroCarta)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    ''' <summary>
    ''' Elimina un expediente de OperacionesCaja table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoIntermediario_CodigoContacto(ByVal codigoIntermediario As String, ByVal codigoContacto As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_EliminarPorCodigoIntermediario_CodigoContacto")
            db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, codigoIntermediario)
            db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, codigoContacto)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    ''' <summary>
    ''' Elimina un expediente de OperacionesCaja table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoMercado(ByVal codigoMercado As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_EliminarPorCodigoMercado")
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    ''' <summary>
    ''' Elimina un expediente de OperacionesCaja table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoTipoOperacion(ByVal codigoTipoOperacion As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_EliminarPorCodigoTipoOperacion")
            db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codigoTipoOperacion)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    ''' <summary>
    ''' Elimina un expediente de OperacionesCaja table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoMoneda(ByVal codigoMoneda As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_EliminarPorCodigoMoneda")
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function Limite1() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_Limite1")
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Limite1 = ds
            End Using
        End Using
    End Function
    Public Function AprobarOperacionCaja(ByVal codigoOperacionCaja As String, ByVal dataRequest As DataSet, Optional ByVal indAprobar As String = "") As Boolean    'HDG 20120330
        Dim bolResult As Boolean = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand
        If indAprobar = "1" Then
            dbCommand = db.GetStoredProcCommand("pr_SIT_pro_Aprobar_OperacionesCaja")
        Else
            dbCommand = db.GetStoredProcCommand("pr_SIT_pro_Aprobar_OperacionesCaja2")
        End If
        Using dbCommand
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddOutParameter(dbCommand, "@p_Valida", DbType.Boolean, bolResult)
            db.ExecuteNonQuery(dbCommand)
            bolResult = CType(db.GetParameterValue(dbCommand, "@p_Valida"), Boolean)
            Return bolResult
        End Using
    End Function
    Public Function GenerarClaveFirmantesCarta(ByVal codigoOperacionCaja As String, ByVal codigoInterno As String, ByVal claveFirma As String, ByVal rutaArchivo As String, ByVal indReporte As String, ByVal dataRequest As DataSet) As Boolean    'HDG OT 64016 20111021
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_Insertar_ClaveFirmantesCarta")
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
            db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, codigoInterno)
            db.AddInParameter(dbCommand, "@p_ClaveFirma", DbType.String, claveFirma)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_RutaReporte", DbType.String, rutaArchivo)
            db.AddInParameter(dbCommand, "@p_IndReporte", DbType.String, indReporte)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function InsertarClaveFirmantes(ByVal codigoInterno As String, ByVal claveFirma As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("dbo.SP_SIT_INS_ClaveFirmante")
            db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, codigoInterno)
            db.AddInParameter(dbCommand, "@p_ClaveFirma", DbType.String, claveFirma)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function ObtenerClaveFirmantes(ByVal codigoInterno As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("dbo.SP_SIT_OBT_ClaveFirmante")
            db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, codigoInterno)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                ObtenerClaveFirmantes = ds
            End Using
        End Using
    End Function
    Public Function InicializarAprobacionOperacionesCaja() As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_del_Inicializar_TmpAprobOperacionesCaja")
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function ReporteAprobacionOperacionesCaja(ByVal proceso As String, ByVal codigoMercado As String, ByVal codigoPortafolio As String, ByVal codigoTercero As String, ByVal codigoTerceroBanco As String, _
         ByVal fecha As Decimal, ByVal abono As Boolean, ByVal estado As String, ByVal codigoOperacionCaja As String) As DataSet    'HDG 20111128
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ReporteAprobacion_OperacionesCaja")
            db.AddInParameter(dbCommand, "@p_Proceso", DbType.String, proceso)
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
            db.AddInParameter(dbCommand, "@p_CodigoTerceroBanco", DbType.String, codigoTerceroBanco)
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
            db.AddInParameter(dbCommand, "@p_abono", DbType.Boolean, abono)
            db.AddInParameter(dbCommand, "@p_EstadoCarta", DbType.String, estado)
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function FirmarCarta(ByVal codigoOperacionCaja As String, ByVal claveFirma As String, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_FirmaCarta_OperacionesCaja")
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
            db.AddInParameter(dbCommand, "@p_ClaveFirma", DbType.String, claveFirma)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddOutParameter(dbCommand, "@p_Valida", DbType.Boolean, bolResult)
            db.ExecuteNonQuery(dbCommand)
            bolResult = CType(db.GetParameterValue(dbCommand, "@p_Valida"), Boolean)
            Return bolResult
        End Using
    End Function
    Public Function ActualizarEstadoImpresion(ByVal codigoImpresion As Decimal, ByVal estado As String, ByVal dataRequest As DataSet) As Boolean
        ActualizarEstadoImpresion = False
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_ActualizarEstado_ImpresionCartas")
            db.AddInParameter(dbCommand, "@p_CodigoImpresion", DbType.Decimal, codigoImpresion)
            db.AddInParameter(dbCommand, "@p_Estado", DbType.String, estado)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            ActualizarEstadoImpresion = True
        End Using
    End Function
    Public Function CalcularRangosInicialFinalCartas(ByVal cantidad As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_CalcularRangos_RangoImpresionCartas")
            db.AddInParameter(dbCommand, "@p_Cantidad", DbType.Decimal, cantidad)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function InsertarRangoImpresionCartas(ByVal rangoInicial As Decimal, ByVal rangoFinal As Decimal, ByVal cantidad As Decimal, ByRef codigoRango As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_Insertar_RangoImpresionCartas")
            db.AddInParameter(dbCommand, "@p_RangoInicial", DbType.Decimal, rangoInicial)
            db.AddInParameter(dbCommand, "@p_RangoFinal", DbType.Decimal, rangoFinal)
            db.AddInParameter(dbCommand, "@p_Cantidad", DbType.Decimal, cantidad)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddOutParameter(dbCommand, "@p_Valida", DbType.Boolean, bolResult)
            db.AddOutParameter(dbCommand, "@p_CodigoRango", DbType.Decimal, codigoRango)
            db.ExecuteNonQuery(dbCommand)
            codigoRango = CType(db.GetParameterValue(dbCommand, "@p_CodigoRango"), Decimal)
            bolResult = CType(db.GetParameterValue(dbCommand, "@p_Valida"), Boolean)
            Return bolResult
        End Using
    End Function
    Public Function ImprimirCarta(ByVal codigoImpresion As Decimal, ByVal codigoCarta As Decimal, ByVal codigoRango As Decimal, ByVal dataRequest As DataSet, Optional ByVal EstadoCarta As String = "") As Boolean 'HDG 20120120
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_Imprimir_ImpresionCartas")
            db.AddInParameter(dbCommand, "@p_CodigoImpresion", DbType.Decimal, codigoImpresion)
            db.AddInParameter(dbCommand, "@p_CodigoCarta", DbType.Decimal, codigoCarta)
            db.AddInParameter(dbCommand, "@p_CodigoRango", DbType.Decimal, codigoRango)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_Estado", DbType.String, EstadoCarta)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    'OT10749 - Refactorizar código
    Public Function ExtornarOperacionesCaja(ByVal codigoExtorno As Decimal, ByVal accion As Boolean, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_Extornar_OperacionesCaja")
            db.AddInParameter(dbCommand, "@p_CodigoExtorno", DbType.Decimal, codigoExtorno)
            db.AddInParameter(dbCommand, "@p_Accion", DbType.Boolean, accion)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function InsertarOperacionesCajaExt(ByVal codigoOperacionCaja As String, ByVal motivo As String, ByVal observacion As String, ByVal dataRequest As DataSet, ByVal liqAntFondo As Integer, ByVal codPortafolio As String) As Boolean  'HDG OT 64767 20120222
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_Insertar_OperacionesCajaExt")
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
            db.AddInParameter(dbCommand, "@p_MotivoExtorno", DbType.String, motivo)
            db.AddInParameter(dbCommand, "@p_ObservacionExtorno", DbType.String, observacion)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_LiqAntFondo", DbType.String, liqAntFondo)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codPortafolio)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    'OT10749 - Fin
    Public Function EliminarClavesFirmantesCartas(ByVal codigoOperacionCaja As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_del_Eliminar_ClavesFirmantesCartas")
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function Listar_MonedaBanco(ByVal CodigoPortafolioSBS As String, CodigoEntidad As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_MonedaBanco")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, CodigoEntidad)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function SeleccionCartas(ByVal codigoMercado As String, ByVal codigoPortafolio As String, ByVal codigoTercero As String, ByVal codigoTerceroBanco As String, _
     ByVal fecha As Decimal, ByVal estado As String, ByVal codigoOperacionCaja As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_SeleccionCartas")
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
            db.AddInParameter(dbCommand, "@p_CodigoTerceroBanco", DbType.String, codigoTerceroBanco)
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
            db.AddInParameter(dbCommand, "@p_EstadoCarta", DbType.String, estado)
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function ImpresionCarta_Transferencias(ByVal codigoOperacionCaja As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ImpresionCarta_Transferencias")
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function ImpresionCarta_CancelacionDPZ(ByVal codigoOperacionCaja As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ImpresionCarta_CancelacionDPZ")
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function ImpresionCarta_ConstitucionDPZ(ByVal codigoOperacionCaja As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ImpresionCarta_ConstitucionDPZ")
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Sub ActualizaDatosCancelacionesDPZ(ByVal FechaOperacion As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ActualizaDatosCancelacionesDPZ")
            db.AddInParameter(dbCommand, "@P_FechaOperacion", DbType.Decimal, FechaOperacion)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Function SaldoBancario(ByVal FechaOperacion As Decimal, CodigoClaseCuenta As String, CodigoPortafolioSBS As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_SaldoBancario")
            db.AddInParameter(dbCommand, "@P_FechaOperacion", DbType.Decimal, FechaOperacion)
            db.AddInParameter(dbCommand, "@P_CodigoClaseCuenta", DbType.String, CodigoClaseCuenta)
            db.AddInParameter(dbCommand, "@P_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function SaldoBancarioOperaciones(ByVal FechaOperacion As Decimal, CodigoPortafolioSBS As String, NumeroCuenta As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_SaldoBancarioOperaciones")
            db.AddInParameter(dbCommand, "@P_FechaOperacion", DbType.Decimal, FechaOperacion)
            db.AddInParameter(dbCommand, "@P_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@P_NumeroCuenta", DbType.String, NumeroCuenta)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'OT10749 - Refactorizar código
    Public Function ObtienCodigoExtorno(ByVal CodigoOperacionCaja As String, CodigoPortafolioSBS As String) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ObtienCodigoExtorno")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, CodigoOperacionCaja)
            db.AddOutParameter(dbCommand, "@p_CodigoExtorno", DbType.Decimal, 9)
            db.ExecuteNonQuery(dbCommand)
            Return CType(db.GetParameterValue(dbCommand, "@p_CodigoExtorno"), Decimal)
        End Using
    End Function
    Public Function GeneraInversiones(CodigoPortafolio As String, ByVal FechaOperacion As Decimal, NumeroCuenta As String, ByVal dataRequest As DataSet) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_GeneraInversiones")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, NumeroCuenta)
            db.AddInParameter(dbCommand, "@P_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            Return db.ExecuteNonQuery(dbCommand)
        End Using
    End Function
    Public Sub ActualizaOperacionCaja(CodigoPortafolio As String, ByVal Importe As Decimal, CodigoOperacionCaja As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ActualizaOperacionCaja")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, CodigoOperacionCaja)
            db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, Importe)
            db.AddInParameter(dbCommand, "@P_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Sub RegularizarIntradia(CodigoPortafolio As String, CodigoOperacionCaja As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_RegularizarIntradia")
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, CodigoOperacionCaja)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    'OT10749 - Fin
    Public Function SeleccionBancos(CodigoClaseCuenta As String, CodigoPortafolioSBS As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_SeleccionBancos")
            db.AddInParameter(dbCommand, "@P_CodigoClaseCuenta", DbType.String, CodigoClaseCuenta)
            db.AddInParameter(dbCommand, "@P_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function CuentaEconomica_SeleccionarPorFiltro(ByVal codigoPortafolio As String, ByVal CodigoClaseCuenta As String, ByVal CodigoEntidad As String, _
ByVal CodigoMoneda As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_CuentaEconomica_SeleccionarPorFiltro")
            Dim ds As New DataSet
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, CodigoEntidad)
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, CodigoClaseCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, CodigoMoneda)
            db.LoadDataSet(dbCommand, ds, "CuentaEconomica")
            Return ds.Tables(0)
        End Using
    End Function
    Public Sub InsertarCajaRecaudoPendiente(NumeroCuenta As String, MontoPendiente As Decimal, FechaOperacion As Decimal, FechaVencimiento As Decimal,
    dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_InsertarCajaRecaudoPendiente")
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_MontoPendiente", DbType.Decimal, MontoPendiente)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
            db.AddInParameter(dbCommand, "@p_FechaVencimiento", DbType.Decimal, FechaVencimiento)
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Sub EliminarPendiente(Correlativo As Integer, FechaOperacion As Decimal, dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_EliminarPendiente")
            db.AddInParameter(dbCommand, "@p_Correlativo", DbType.Int32, Correlativo)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Sub ActualizarCajaRecaudoPendiente(Correlativo As Integer, NumeroCuenta As String, MontoPendiente As Decimal, FechaOperacion As Decimal, _
    FechaVencimiento As Decimal, dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ActualizarCajaRecaudoPendiente")
            db.AddInParameter(dbCommand, "@p_Correlativo", DbType.Int32, Correlativo)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_MontoPendiente", DbType.Decimal, MontoPendiente)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
            db.AddInParameter(dbCommand, "@p_FechaVencimiento", DbType.Decimal, FechaVencimiento)
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Function ValidaNumeroCuenta(ByVal NumeroCuenta As String, FechaOperacion As Decimal) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("dbo.sp_SIT_ValidaNumeroCuenta")
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
            db.AddOutParameter(dbCommand, "@p_Cantidad", DbType.Int32, 20)
            db.ExecuteNonQuery(dbCommand)
            Return CType(db.GetParameterValue(dbCommand, "@p_Cantidad"), Integer)
        End Using
    End Function
    Public Function SeleccionarCajaRecaudoPendiente(ByVal codigoPortafolio As String, ByVal CodigoEntidad As String, ByVal CodigoMoneda As String, _
    ByVal NumeroCuenta As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_SeleccionarCajaRecaudoPendiente")
            Dim ds As New DataSet
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, CodigoEntidad)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, NumeroCuenta)
            db.LoadDataSet(dbCommand, ds, "Pendiente")
            Return ds.Tables(0)
        End Using
    End Function
    Public Function CajaRecaudoPendiente(ByVal Correlativo As Integer) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_CajaRecaudoPendiente")
            Dim ds As New DataSet
            db.AddInParameter(dbCommand, "@p_Correlativo", DbType.String, Correlativo)
            db.LoadDataSet(dbCommand, ds, "FPendiente")
            Return ds.Tables(0)
        End Using
    End Function
    Public Function Listar_MonedaBanco_Clase(ByVal CodigoPortafolioSBS As String, CodigoEntidad As String, ClaseCuenta As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_MonedaBanco_Clase")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, CodigoEntidad)
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, ClaseCuenta)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function Ruta_Carta(ByVal CodigoOperacion As String, CodigoModelo As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_obt_Ruta_Carta")
            db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, CodigoOperacion)
            db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, CodigoModelo)
            db.AddOutParameter(dbCommand, "@p_ArchivoPlantilla", DbType.String, 500)
            db.ExecuteNonQuery(dbCommand)
            Return CType(db.GetParameterValue(dbCommand, "@p_ArchivoPlantilla"), String)
        End Using
    End Function
    'OT11008 - 05/01/2018 - Ian Pastor M.
    'Descripción: Obtiene los rescates y retenciones del sistema de operaciones
    Public Function ObtenerMovimientosRescateyRetenciones(ByVal p_CodigoPortafolioSisOpe As Decimal, ByVal p_Fecha As Date) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Using dbCommand As DbCommand = db.GetStoredProcCommand("INGF_OBT_RESCATE_FINAL")
            db.AddInParameter(dbCommand, "@idFondo", DbType.Decimal, p_CodigoPortafolioSisOpe)
            db.AddInParameter(dbCommand, "@fecha", DbType.DateTime, p_Fecha)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function InsertarOperacion(ByVal dsOperacionCaja As OperacionCajaBE, ByVal dataRequest As DataSet) As String
        InsertarOperacion = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim opCaja As OperacionCajaBE.OperacionCajaRow = dsOperacionCaja.OperacionCaja.Rows(0)
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_InsertarOpe")
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, opCaja.CodigoMercado)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, opCaja.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, opCaja.CodigoOperacion)
            db.AddInParameter(dbCommand, "@p_NroCuenta", DbType.String, opCaja.NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoModalidadPago", DbType.String, opCaja.CodigoModalidadPago)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, opCaja.CodigoTerceroOrigen)
            db.AddInParameter(dbCommand, "@p_CodigoTerceroDestino", DbType.String, opCaja.CodigoTerceroDestino)
            db.AddInParameter(dbCommand, "@p_NroCuentaRef", DbType.String, opCaja.NumeroCuentaDestino)
            db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, opCaja.Referencia)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, opCaja.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, opCaja.Importe)
            db.AddInParameter(dbCommand, "@p_usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_CodigoEjecucion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "CodigoEjecucion"))
            db.AddInParameter(dbCommand, "@nvcCodigoModelo", DbType.String, opCaja.CodigoModelo)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, opCaja.FechaPago)
            InsertarOperacion = CType(db.ExecuteScalar(dbCommand), String)
        End Using
    End Function
    'OT11008 - Fin

    'OT11237 - 15/03/2018 - Ian Pastor M.
    'Descripción: Obtiene las suscripciones del sistema de operaciones
    Public Function ObtenerSuscripcionesSisOpe(ByVal p_CodigoPortafolioSisOpe As Decimal, ByVal p_Fecha As Date) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SIT_LIS_RECAUDACION_ABONO_CHEQUEADO")
            db.AddInParameter(dbCommand, "@ID_FONDO", DbType.Decimal, p_CodigoPortafolioSisOpe)
            db.AddInParameter(dbCommand, "@FECHA", DbType.DateTime, p_Fecha)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'OT11237 - Fin

End Class