Imports System
Imports System.Data
Imports System.Data.Common
Imports Cartas.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class OperacionesCajaDAM
    Public Sub New()
    End Sub
    'OT 10025 21/02/2017 - Carlos Espejo
    'Descripcion: Se adecua al proceso de una firma por dia, se agrega la fecha de operacion al filtro
    Public Function ObtenerClaveFirmantes(ByVal codigoInterno As String, ByVal FechaOperacion As Decimal) As String
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase
            Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_obt_ClaveFirmante")
                db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, codigoInterno)
                db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
                db.AddOutParameter(dbCommand, "@p_ClaveFirma", DbType.String, 10)
                db.ExecuteNonQuery(dbCommand)
                ObtenerClaveFirmantes = CType(db.GetParameterValue(dbCommand, "@p_ClaveFirma"), String)
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function EliminarClavesFirmantesCartas(ByVal codigoOperacionCaja As String) As Boolean
        EliminarClavesFirmantesCartas = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_del_Eliminar_ClavesFirmantesCartas")
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
            db.ExecuteNonQuery(dbCommand)
            EliminarClavesFirmantesCartas = True
        End Using
    End Function
    'OT 10025 27/02/2017 - Carlos Espejo
    'Descripcion: Se agrega la CodigoPortafolio,CodigoInterno al filtro
    Public Function FirmarCarta(ByVal codigoOperacionCaja As String, ByVal CodigoPortafolio As String, ByVal CodigoInterno As String, ByVal claveFirma As String, _
    ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_FirmaCarta_OperacionesCaja")
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_ClaveFirma", DbType.String, claveFirma)
            db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, CodigoInterno)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddOutParameter(dbCommand, "@p_Valida", DbType.Boolean, bolResult)
            db.ExecuteNonQuery(dbCommand)
            bolResult = CType(db.GetParameterValue(dbCommand, "@p_Valida"), Boolean)
        End Using
        Return bolResult
    End Function
    Public Function SeleccionCartas(ByVal codigoMercado As String, ByVal codigoPortafolio As String, ByVal codigoTercero As String, ByVal codigoTerceroBanco As String, _
     ByVal fecha As Decimal, ByVal estado As String, ByVal codigoOperacionCaja As String, ByVal codigoCartaAgrupado As Integer, ByVal Resumen As String) As SeleccionCartaBEList
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim lSeleccionCarta As New SeleccionCartaBEList
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_SeleccionCartas")
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
            db.AddInParameter(dbCommand, "@p_CodigoTerceroBanco", DbType.String, codigoTerceroBanco)
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
            db.AddInParameter(dbCommand, "@p_EstadoCarta", DbType.String, estado)
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
            db.AddInParameter(dbCommand, "@p_CodigoCartaAgrupado", DbType.Int32, codigoCartaAgrupado)
            db.AddInParameter(dbCommand, "@p_Resumen", DbType.String, Resumen)
            Using oReader As IDataReader = db.ExecuteReader(dbCommand)
                Dim oSeleccionCarta As SeleccionCartaBE
                While oReader.Read()
                    oSeleccionCarta = New SeleccionCartaBE
                    oSeleccionCarta.DescripcionPortafolio = oReader.Item(0)
                    oSeleccionCarta.DescripcionOperacion = oReader.Item(1)
                    oSeleccionCarta.ModeloCarta = oReader.Item(2)
                    oSeleccionCarta.DescripcionIntermediario = oReader.Item(3)
                    oSeleccionCarta.CodigoMoneda = oReader.Item(4)
                    oSeleccionCarta.Importe = oReader.Item(5)
                    oSeleccionCarta.NumeroOrden = oReader.Item(6)
                    oSeleccionCarta.VBADMIN = oReader.Item(7)
                    oSeleccionCarta.VBGERF1 = oReader.Item(8)
                    oSeleccionCarta.VBGERF2 = oReader.Item(9)
                    oSeleccionCarta.CodigoOperacion = oReader.Item(10)
                    oSeleccionCarta.CodigoOperacionCaja = oReader.Item(11)
                    oSeleccionCarta.EstadoCarta = oReader.Item(12)
                    oSeleccionCarta.CodigoModelo = oReader.Item(13)
                    'OT 10025 21/02/2017 - Carlos Espejo
                    'Descripcion: Se recupera el fondo de la consulta
                    oSeleccionCarta.CodigoPortafolioSBS = oReader.Item(14)
                    'OT 10025 Fin
                    'OT 10150 21/02/2017 - Carlos Espejo
                    'Descripcion: Se recupera el correlativo de la carta
                    oSeleccionCarta.CorrelativoCartas = oReader.Item(15)
                    'OT 10150 Fin
                    oSeleccionCarta.NumeroCuenta = oReader.Item(16)
                    oSeleccionCarta.Banco = oReader.Item(17)
                    oSeleccionCarta.CodigoCartaAgrupado = oReader.Item(18)
                    oSeleccionCarta.Tipo = oReader.Item(20)
                    oSeleccionCarta.CodigoAgrupado = oReader.Item(22)
                    lSeleccionCarta.Add(oSeleccionCarta)
                End While
                oReader.Close()
            End Using
        End Using
        Return lSeleccionCarta
    End Function
    Public Sub ActualizaDatosCancelacionesDPZ(ByVal FechaOperacion As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ActualizaDatosCancelacionesDPZ")
            db.AddInParameter(dbCommand, "@P_FechaOperacion", DbType.Decimal, FechaOperacion)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Function ReporteAprobacionOperacionesCaja(ByVal proceso As String, ByVal codigoMercado As String, ByVal codigoPortafolio As String, ByVal codigoTercero As String, ByVal codigoTerceroBanco As String, _
         ByVal fecha As Decimal, ByVal abono As Boolean, ByVal estado As String, ByVal codigoOperacionCaja As String) As DataSet
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
                ReporteAprobacionOperacionesCaja = ds
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
                SeleccionarAutorizacionCartas = ds
            End Using
        End Using
    End Function
    'OT 10025 21/02/2017 - Carlos Espejo
    'Descripcion: Se unifica la aprobacion enun solo procedimiento y se agrega el fondo al filtro
    Public Sub AprobarOperacionCaja(ByVal codigoOperacionCaja As String, ByVal CodigoPortafolio As String, ByVal dataRequest As DataSet)
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_Aprobar_OperacionesCaja")
                db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
                db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, CodigoPortafolio)
                db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
                db.ExecuteNonQuery(dbCommand)
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function GenerarClaveFirmantesCarta(ByVal codigoOperacionCaja As String, ByVal codigoInterno As String, ByVal claveFirma As String, _
    ByVal rutaArchivo As String, ByVal indReporte As String, ByVal dataRequest As DataSet) As Boolean
        GenerarClaveFirmantesCarta = False
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
            GenerarClaveFirmantesCarta = True
        End Using
    End Function
    Public Function InsertarClaveFirmantes(ByVal codigoInterno As String, ByVal claveFirma As String, ByVal dataRequest As DataSet) As Boolean
        InsertarClaveFirmantes = False
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("dbo.SP_SIT_INS_ClaveFirmante")
            db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, codigoInterno)
            db.AddInParameter(dbCommand, "@p_ClaveFirma", DbType.String, claveFirma)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            InsertarClaveFirmantes = True
        End Using
    End Function
    Public Function Ruta_Carta(ByVal CodigoOperacion As String, ByVal CodigoModelo As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_obt_Ruta_Carta")
            db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, CodigoOperacion)
            db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, CodigoModelo)
            db.AddOutParameter(dbCommand, "@p_ArchivoPlantilla", DbType.String, 500)
            db.ExecuteNonQuery(dbCommand)
            Ruta_Carta = CType(db.GetParameterValue(dbCommand, "@p_ArchivoPlantilla"), String)
        End Using
    End Function
    Public Function ImpresionCarta_Constitucion_CancelacionDPZ(ByVal codigoOperacionCaja As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ImpresionCarta_Constitucion_CancelacionDPZ")
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                ImpresionCarta_Constitucion_CancelacionDPZ = ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function EsRenovacion(ByVal CodigoOrden As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_obt_Comprueba_Renovacion")
            db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
            db.AddOutParameter(dbCommand, "@p_Existe", DbType.String, 1)
            db.ExecuteNonQuery(dbCommand)
            EsRenovacion = db.GetParameterValue(dbCommand, "@p_Existe").ToString()
        End Using
    End Function
    Public Function Cartas_Deposito_Renovacion(ByVal codigoOperacionCaja As String, ByVal p_Constitucion As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_gen_Cartas_Deposito_Renovacion")
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
            db.AddInParameter(dbCommand, "@p_Constitucion", DbType.String, p_Constitucion)
            Cartas_Deposito_Renovacion = db.ExecuteDataSet(dbCommand).Tables(0)
        End Using
    End Function
    Public Function ImpresionCarta_Transferencias(ByVal codigoOperacionCaja As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ImpresionCarta_Transferencias")
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                ImpresionCarta_Transferencias = ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function CodigoBCR(ByVal CodigoOrden As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_obt_CodigoBCR")
            db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
            db.AddOutParameter(dbCommand, "@p_CodigoOrdenBCR", DbType.String, 10)
            db.ExecuteNonQuery(dbCommand)
            CodigoBCR = db.GetParameterValue(dbCommand, "@p_CodigoOrdenBCR").ToString()
        End Using
    End Function
    Public Function AmpliacionBCR(ByVal CodigoOrden As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_obt_AmpliacionBCR")
            db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
            db.AddOutParameter(dbCommand, "@p_AmpliacionBCR", DbType.String, 1)
            db.ExecuteNonQuery(dbCommand)
            AmpliacionBCR = db.GetParameterValue(dbCommand, "@p_AmpliacionBCR").ToString()
        End Using
    End Function
    Public Function ImpresionCarta_ComVenAcc(ByVal strXML As String, ByVal strCodigoOperacion As String, ByVal strModeloCarta As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ImpresionCarta_CompraVentaAcciones")
            db.AddInParameter(dbCommand, "@p_XML", DbType.Xml, strXML)
            db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, strCodigoOperacion)
            db.AddInParameter(dbCommand, "@p_CodigoModeloCarta", DbType.String, strModeloCarta)
            'db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, strNumeroCuenta)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Sub AgruparOperaciones_ComVenAcc(ByVal p_CodigoPortafolio As String, ByVal p_FechaOperacion As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using DbCommand As DbCommand = db.GetStoredProcCommand("OperacionesCaja_AgruparCompraVentaAcciones")
            db.AddInParameter(DbCommand, "@p_CodigoPortafolio", DbType.String, p_CodigoPortafolio)
            db.AddInParameter(DbCommand, "@p_FechaOperacion", DbType.Decimal, p_FechaOperacion)
            db.ExecuteNonQuery(DbCommand)
        End Using
    End Sub

    Public Function Cartas_Operacion_Cambio(ByVal codigoOperacionCaja As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_Gen_Carta_OperacionCambio")
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
            Cartas_Operacion_Cambio = db.ExecuteDataSet(dbCommand).Tables(0)
        End Using
    End Function

    Public Function Cartas_Operacion_Reporte(ByVal codigoOperacionCaja As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_Gen_Carta_OperacionReporte")
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
            Cartas_Operacion_Reporte = db.ExecuteDataSet(dbCommand).Tables(0)
        End Using
    End Function


    Public Function Cartas_Operacion_Forward(ByVal codigoOperacionCaja As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_Gen_Carta_OperacionForward")
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
            Cartas_Operacion_Forward = db.ExecuteDataSet(dbCommand).Tables(0)
        End Using
    End Function
    Public Function Cartas_Operacion_ForwardVcto(ByVal codigoOperacionCaja As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_Gen_Carta_OperacionForwardVcto")
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
            Cartas_Operacion_ForwardVcto = db.ExecuteDataSet(dbCommand).Tables(0)
        End Using
    End Function

    'Bonos
    Public Function Cartas_Operacion_Bono(ByVal codigoOperacionCaja As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_CompraVentaBonos")
            db.AddInParameter(dbCommand, "@CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
            Cartas_Operacion_Bono = db.ExecuteDataSet(dbCommand).Tables(0)
        End Using
    End Function


    'TransferenciasExterior
    Public Function Cartas_Operacion_TransferenciaExterior(ByVal codigoOperacionCaja As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_TransferenciasExterior")
            db.AddInParameter(dbCommand, "@CodigoOperacionCaja", DbType.String, codigoOperacionCaja)
            Cartas_Operacion_TransferenciaExterior = db.ExecuteDataSet(dbCommand).Tables(0)
        End Using
    End Function

    Public Function ImpresionCarta_ComVenAccNacionales(ByVal strCodigoOrden As String, ByVal strCodigoModelo As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_CompraVentaAcciones")
            db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, strCodigoOrden)
            db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, strCodigoModelo)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function

    Public Function ImpresionCarta_OperacionReporte(ByVal strCodigoOrden As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ListaOperacionesReporte")
            db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, strCodigoOrden)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
End Class