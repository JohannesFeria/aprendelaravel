Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class OrdenInversionWorkFlowDAM
    Public Sub New()
    End Sub
    Public Function ValidarNegociacionDiaAnterior(ByVal strCodigoPortafolioSBS As String, ByVal strCodigoMnemonico As String, ByVal strFechaOperacion As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ValidarInstrumentoValorizado")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, strCodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, strCodigoMnemonico)
        db.AddInParameter(dbCommand, "@p_Poliza", DbType.Decimal, strFechaOperacion)
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function EjecutarOI(ByVal StrCodigoOrden As String, ByVal StrCodigoPortafolio As String, ByVal StrNroPoliza As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_EjecutarOI")
            db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, StrCodigoOrden)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, StrCodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_Poliza", DbType.String, StrNroPoliza)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Function
    Public Function ConfirmarOI(ByVal StrCodigoOrden As String, ByVal StrCodigoPortafolio As String, ByVal StrNroPoliza As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ConfirmarOI")
            db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, StrCodigoOrden)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, StrCodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_Poliza", DbType.String, StrNroPoliza)
            db.AddOutParameter(dbCommand, "@p_Result", DbType.String, 12)
            db.ExecuteNonQuery(dbCommand)
            Return CType(db.GetParameterValue(dbCommand, "@p_Result"), Integer)
        End Using
    End Function
    Public Function ActualizaISINOrden(ByVal StrCodigoOrden As String, ByVal StrCodigoPortafolio As String, ByVal ISIN As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("usp_SIT_up_ISINOrden")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, StrCodigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, StrCodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, ISIN)
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function EnviarCXPCDPH(ByVal StrCodigoOrden As String, ByVal StrCodigoPortafolio As String, ByVal StrNroPoliza As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_CertificadoDepoPH")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, StrCodigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, StrCodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_Poliza", DbType.String, StrNroPoliza)
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function AprobarOI(ByVal StrCodigoOrden As String, ByVal StrCodigoPortafolio As String, ByVal dataRequest As DataSet) As String
        Dim strCodigoOI As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("OrdenInversion_AprobarOI")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, StrCodigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, StrCodigoPortafolio)
        db.ExecuteNonQuery(dbCommand)
        strCodigoOI = db.ExecuteScalar(dbCommand)
        Return strCodigoOI
    End Function
    Public Function AprobarExcesoBrokerOI(ByVal StrCodigoOrden As String, ByVal StrCodigoPortafolio As String, ByVal strEstado As String, ByVal dataRequest As DataSet) As String   'HDG OT 61166 20100920
        Dim strCodigoOI As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_upd_AprobarBrokerOI")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, StrCodigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, StrCodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_Estado", DbType.String, strEstado) 'HDG OT 61166 20100920
        db.ExecuteNonQuery(dbCommand)
        strCodigoOI = db.ExecuteScalar(dbCommand)
        Return strCodigoOI
    End Function
    Public Function AutorizarUsuarioAprobacionExcesoOI(ByVal StrUsuario As String, ByVal StrPassword As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_AutorizarAcceso")
        db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, StrUsuario)
        db.AddInParameter(dbCommand, "@p_Password", DbType.String, StrPassword)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ExtornarOIEjecutadas(ByVal StrCodigoPortafolioSBS As String, ByVal StrCodigoOrden As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ExtornarOIEjecutadas")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, StrCodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, StrCodigoOrden)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function ExtornarOIAsignadas(ByVal fechaOperacion As Int32, ByVal codigoPreOrden As String, ByVal StrCodigo As String, ByVal StrCodigoOrdenF1 As String, ByVal StrCodigoOrdenF2 As String, ByVal StrCodigoOrdenF3 As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ExtornarOIAsignadas")
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, StrCodigo)
        db.AddInParameter(dbCommand, "@nvcCodigoPreOrden", DbType.String, codigoPreOrden)
        db.AddInParameter(dbCommand, "@intFechaOperacion", DbType.Int32, fechaOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoOrdenFondo1", DbType.String, StrCodigoOrdenF1)
        db.AddInParameter(dbCommand, "@p_CodigoOrdenFondo2", DbType.String, StrCodigoOrdenF2)
        db.AddInParameter(dbCommand, "@p_CodigoOrdenFondo3", DbType.String, StrCodigoOrdenF3)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function AsignarPOI(ByVal strCodigoPreOrden As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_AsignarPreorden")
        db.AddInParameter(dbCommand, "@p_CodigoPreOrden", DbType.String, strCodigoPreOrden)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.ExecuteDataSet(dbCommand)
    End Function
    Public Function Cancelar_OI_POI(ByVal strCodigoOrdenCancelado As String, ByVal strCodigoOrdenGenerado As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("OrdenInversion_Cancelar_OI_POI")
        db.AddInParameter(dbCommand, "@p_CodigoOrdenCancelado", DbType.String, strCodigoOrdenCancelado)
        db.AddInParameter(dbCommand, "@p_CodigoOrdenGenerado", DbType.String, strCodigoOrdenGenerado)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.ExecuteDataSet(dbCommand)
    End Function
    Public Function EliminarAsignacion(ByVal codigoPreOrden As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("PreOrdenAsignacion_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoPreOrden", DbType.String, codigoPreOrden)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function IngresarAsignacion(ByVal strEsTemporal As String, ByVal strCodigoPortafolioSBS As String, ByVal strCodigoAsignacionPreOrden As String, ByVal dclUnidadesPropuesto As Decimal, ByVal dclUnidadesReal As Decimal, ByVal dclPorcentajePropuesto As Decimal, ByVal dclPorcentajeReal As Decimal, ByVal strTipoAsignacion As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("PreOrdenAsignacion_Ingresar")
        db.AddInParameter(dbCommand, "@p_EsTemporal", DbType.String, strEsTemporal)
        db.AddInParameter(dbCommand, "@p_CodigoAsignacionPreOrden", DbType.String, strCodigoAsignacionPreOrden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, strCodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_UnidadesPropuesto", DbType.Decimal, dclUnidadesPropuesto)
        db.AddInParameter(dbCommand, "@p_UnidadesReal", DbType.Decimal, dclUnidadesReal)
        db.AddInParameter(dbCommand, "@p_PorcentajePropuesto", DbType.Decimal, dclPorcentajePropuesto)
        db.AddInParameter(dbCommand, "@p_PorcentajeReal", DbType.Decimal, dclPorcentajeReal)
        db.AddInParameter(dbCommand, "@p_TipoAsignacion", DbType.String, strTipoAsignacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function IngresarAsignacionAgrupacion(ByVal strEsTemporal As String, ByVal strCodigoAsignacionPreOrden As String, ByVal strCodigoPreOrden As String, ByRef codAsignacionPreOrdenNuevo As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("PreOrdenAsignacionAgrupacion_Ingresar")
        db.AddInParameter(dbCommand, "@p_EsTemporal", DbType.String, strEsTemporal)
        db.AddInParameter(dbCommand, "@p_CodigoAsignacionPreOrden", DbType.String, strCodigoAsignacionPreOrden)
        db.AddInParameter(dbCommand, "@p_CodigoPreOrden", DbType.String, strCodigoPreOrden)
        db.AddInParameter(dbCommand, "@p_FechaAsignacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        codAsignacionPreOrdenNuevo = db.ExecuteScalar(dbCommand)
        If codAsignacionPreOrdenNuevo <> Nothing Or codAsignacionPreOrdenNuevo <> "" Then
            Return True
        End If
    End Function
    Public Function BuscarAsignacion(ByVal strCodigoPreOrden As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PreOrdenAsignacion_Buscar")
        Dim datos As DataTable
        db.AddInParameter(dbCommand, "@p_CodigoPreOrden", DbType.String, strCodigoPreOrden)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function GetOrdenInversionDivisas(ByVal codigoPortafolio As String, ByVal codigoOrden As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("usp_GetOrdenInversionDivisas")
            db.AddInParameter(dbCommand, "@nvcCodigoPortafolioSBS", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@nvcCodigoOrden", DbType.String, codigoOrden)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                GetOrdenInversionDivisas = ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function ModificarEstadoPreOrden(ByVal StrCodigoPreOrden As String, ByVal StrCodigoPortafolio As String, ByVal StrEstado As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PreOrdenInversion_ModificarEstado")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, StrCodigoPreOrden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, StrCodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_estado", DbType.String, StrEstado)
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function ModificarEstadoAsignacionPreOrden(ByVal StrCodigoPreOrden As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("PreOrdenInversion_ActualizarEstadoAsignado")
        db.AddInParameter(dbCommand, "@p_CodigoPreOrden", DbType.String, StrCodigoPreOrden)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function ModificarCodigoAsignacionOI(ByVal StrCodigoOrdenInversion As String, ByVal StrCodigoAsignacionPreOrden As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PreOrdenInversion_ActualizarAsignaconOI")
        db.AddInParameter(dbCommand, "@p_CodigoOrdenInversion", DbType.String, StrCodigoOrdenInversion)
        db.AddInParameter(dbCommand, "@p_CodigoAsignacionPreOrden", DbType.String, StrCodigoAsignacionPreOrden)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function VencimientoDPZ_OR(CodigoPortafolioSBS As String, CodigoOrden As String, FechaNueva As Decimal, CalculaNuevoVencimiento As String, Usuario As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SP_SIT_VencimientoDPZ_OR")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
            db.AddInParameter(dbCommand, "@p_FechaNueva", DbType.Decimal, FechaNueva)
            db.AddInParameter(dbCommand, "@P_CalculaNuevoVencimiento", DbType.String, CalculaNuevoVencimiento)
            db.AddInParameter(dbCommand, "@P_Usuario", DbType.String, Usuario)
            db.AddOutParameter(dbCommand, "@p_OrdenGenera", DbType.String, "10")
            db.ExecuteNonQuery(dbCommand)
            Return CType(db.GetParameterValue(dbCommand, "@p_OrdenGenera"), String)
        End Using
    End Function
    Public Function GeneraOrdenParaVencimientoFuturo(ByVal obj As Hashtable) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VencimientoFuturo_GeneraOrden")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, obj.Item("CodigoPortafolioSBS"))
        db.AddInParameter(dbCommand, "@p_MontoOperacion", DbType.Decimal, obj.Item("MontoOperacion"))
        db.AddInParameter(dbCommand, "@p_CodigoTipoCupon", DbType.String, obj.Item("CodigoTipoCupon"))
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, obj.Item("CodigoMoneda"))
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, obj.Item("CodigoISIN"))
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, obj.Item("CodigoSBS"))
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, obj.Item("CodigoTercero"))
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, obj.Item("CodigoNemonico"))
        db.AddInParameter(dbCommand, "@p_CodigoTipoTitulo", DbType.String, obj.Item("CodigoTipoTitulo"))
        db.AddInParameter(dbCommand, "@p_Categoria", DbType.String, obj.Item("Categoria"))
        db.AddInParameter(dbCommand, "@p_TasaCupon", DbType.Decimal, obj.Item("TasaCupon"))
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, obj.Item("CodigoOperacion"))
        db.AddInParameter(dbCommand, "@p_FechaVencimiento", DbType.Decimal, obj.Item("FechaVencimiento"))
        db.AddInParameter(dbCommand, "@p_CantidadOperacion", DbType.Decimal, obj.Item("CantidadOperacion"))
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, obj.Item("FechaOperacion"))
        db.AddOutParameter(dbCommand, "@p_OrdenGenerada", DbType.String, "12")
        db.ExecuteNonQuery(dbCommand)
        Return CType(db.GetParameterValue(dbCommand, "@p_OrdenGenerada"), String)
    End Function
    Public Function ExisteNroPoliza(ByVal nroPoliza As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ExistePoliza")
        db.AddInParameter(dbCommand, "@p_NroPoliza", DbType.String, nroPoliza)
        db.AddOutParameter(dbCommand, "@p_Result", DbType.String, 12)
        db.ExecuteNonQuery(dbCommand)
        Return CType(db.GetParameterValue(dbCommand, "@p_Result"), String)
    End Function
    Function CalculoDPZBisiesto(FechaInicial As Decimal, FechaFinal As Decimal, MontoNominal As Decimal, Tasa As Decimal, TipoTasa As String) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SP_sit_CalculoDPZBisiesto")
            db.AddInParameter(dbCommand, "@P_FechaInicial", DbType.Decimal, FechaInicial)
            db.AddInParameter(dbCommand, "@P_FechaFinal", DbType.Decimal, FechaFinal)
            db.AddInParameter(dbCommand, "@P_MontoNominal", DbType.Decimal, MontoNominal)
            db.AddInParameter(dbCommand, "@P_Tasa", DbType.Decimal, Tasa)
            db.AddInParameter(dbCommand, "@P_TipoTasa", DbType.String, TipoTasa)
            db.AddOutParameter(dbCommand, "@P_MontoCancelacion", DbType.String, 22)
            db.ExecuteNonQuery(dbCommand)
            Return CType(db.GetParameterValue(dbCommand, "@P_MontoCancelacion"), Decimal)
        End Using
    End Function
    'OT 10238 - 12/04/2017 - Carlos Espejo
    'Descripcion: Si el fondo valorizado no se puede realizar la reversion
    Public Sub GeneraCuponera(CodigoOperacion As String, Recalculo As String, CodigoPortafolio As String, CodigoNemonico As String, FechaVencimiento As Decimal, _
    FechaNueva As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_GeneraCuponera")
            db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, CodigoOperacion)
            db.AddInParameter(dbCommand, "@p_Recalculo", DbType.String, Recalculo)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, CodigoNemonico)
            db.AddInParameter(dbCommand, "@p_FechaVencimiento", DbType.Decimal, FechaVencimiento)
            db.AddInParameter(dbCommand, "@p_FechaNueva", DbType.Decimal, FechaNueva)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    'OT-11033 - 03/01/2017 - Ian Pastor M.
    'Descripcion: Verifica si existen terceros negociados (Ejecutados o confirmados)
    Public Function OrdenInversion_ExisteTercero(ByVal p_CodigoTercero As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using DbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ExisteTercero")
            db.AddInParameter(DbCommand, "@p_CodigoTercero", DbType.String, p_CodigoTercero)
            Using ds As DataSet = db.ExecuteDataSet(DbCommand)
                OrdenInversion_ExisteTercero = ds.Tables(0)
            End Using
        End Using
    End Function

    Public Sub GeneraVencimientosBono_Swap(ByVal CodigoPortafolio As String, ByVal FechaVencimiento As Decimal, ByVal FechaNueva As Decimal, ByVal codigoOperacion As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_GeneraVencimientosBono_Swap")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaVencimiento)
            db.AddInParameter(dbCommand, "@p_FechaApertura", DbType.Decimal, FechaNueva)
            db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codigoOperacion)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub

    Public Function ActualizaDatosCarta(ByVal StrCodigoOrden As String, ByVal StrNumeroCuenta As String, ByVal StrDtc As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_DatosCartas")
            db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, StrCodigoOrden)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, StrNumeroCuenta)
            db.AddInParameter(dbCommand, "@p_DTC", DbType.String, StrDtc)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Function

    Public Function OrdenInversion_ValidaExterior(ByVal p_CodigoOrden As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using DbCommand As DbCommand = db.GetStoredProcCommand("OrdenInversion_ValidaExterior")
            db.AddInParameter(DbCommand, "@p_CodigoOrden", DbType.String, p_CodigoOrden)
            db.AddOutParameter(DbCommand, "@p_CodigoOrigen", DbType.String, 5)
            db.ExecuteNonQuery(DbCommand)
            Return CType(db.GetParameterValue(DbCommand, "@p_CodigoOrigen"), String)
        End Using
    End Function

End Class