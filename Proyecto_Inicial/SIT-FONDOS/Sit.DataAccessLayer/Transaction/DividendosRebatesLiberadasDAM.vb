Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class DividendosRebatesLiberadasDAM
    'OT10916 - 07/11/2017 - Ian Pastor M. Ordenar y factorizar código.
    Private sqlCommand As String = ""
    Private oDividendosRebatesLiberadasRow As DividendosRebatesLiberadasBE.DividendosRebatesLiberadasRow
#Region "/* Funciones Seleccionar */"

    Public Function ListarInformacionEntidadExterna(ByVal nFechaInicio As Long, _
                                                    ByVal nFechaFin As Long, _
                                                    ByVal sEntidadExt As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("InformacionEntidadExterna_Listar")
            db.AddInParameter(dbCommand, "@FechaEntregaInicio", DbType.Decimal, nFechaInicio)
            db.AddInParameter(dbCommand, "@FechaEntregaFin", DbType.Decimal, nFechaFin)
            db.AddInParameter(dbCommand, "@EntidadExt", DbType.String, sEntidadExt)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    ' Selecciona un solo expediente de DividendosRebatesLiberadas tabla.
    Public Function SeleccionarImpresion(ByVal sCodigoSBS As String, ByVal nGrupoIdentificador As Decimal, ByVal sSituacion As String, ByVal sMultifondo As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DividendosRebatesLiberadasSeleccion_Imprimir")
            db.AddInParameter(dbCommand, "@CodigoSBS", DbType.String, sCodigoSBS)
            db.AddInParameter(dbCommand, "@GrupoIdentificador", DbType.Decimal, nGrupoIdentificador)
            db.AddInParameter(dbCommand, "@Situacion", DbType.String, sSituacion)
            db.AddInParameter(dbCommand, "@Multifondo", DbType.String, sMultifondo)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    ''' Selecciona un solo expediente de DividendosRebatesLiberadas tabla.
    Public Function Seleccionar(ByVal sCodigoSBS As String, ByVal nIdentificador As Decimal, ByVal sSituacion As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DividendosRebatesLiberadas_Seleccionar")
            db.AddInParameter(dbCommand, "@CodigoSBS", DbType.String, sCodigoSBS)
            db.AddInParameter(dbCommand, "@Identificador", DbType.Decimal, nIdentificador)
            db.AddInParameter(dbCommand, "@Situacion", DbType.String, sSituacion)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function Listar(ByVal sCodigoSBS As String, ByVal sCodigoNemonico As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DividendosRebatesLiberadas_Listar")
            db.AddInParameter(dbCommand, "@CodigoSBS", DbType.String, sCodigoSBS)
            db.AddInParameter(dbCommand, "@CodigoNemonico", DbType.String, sCodigoNemonico)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function SeleccionarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DividendosRebatesLiberadas_SeleccionarPorCodigoISIN_CodigoNemonico")
            db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
            db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function

#End Region
    Public Function Insertar(ByVal ob As DividendosRebatesLiberadasBE, ByRef nIdentificador As Decimal, ByRef nGrupoIdentificador As Decimal, ByVal dataRequest As DataSet) As Boolean
        Insertar = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "DividendosRebatesLiberadas_Insertar"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            oDividendosRebatesLiberadasRow = CType(ob.DividendosRebatesLiberadas.Rows(0), DividendosRebatesLiberadasBE.DividendosRebatesLiberadasRow)
            db.AddInParameter(dbCommand, "@CodigoSBS", DbType.String, oDividendosRebatesLiberadasRow.CodigoSBS)
            db.AddInParameter(dbCommand, "@CodigoISIN", DbType.String, oDividendosRebatesLiberadasRow.CodigoISIN)
            db.AddInParameter(dbCommand, "@CodigoNemonico", DbType.String, oDividendosRebatesLiberadasRow.CodigoNemonico)
            db.AddInParameter(dbCommand, "@FechaIDI", DbType.Decimal, oDividendosRebatesLiberadasRow.FechaIDI)
            db.AddInParameter(dbCommand, "@Factor", DbType.Decimal, oDividendosRebatesLiberadasRow.Factor)
            db.AddInParameter(dbCommand, "@FechaCorte", DbType.Decimal, oDividendosRebatesLiberadasRow.FechaCorte)
            db.AddInParameter(dbCommand, "@FechaEntrega", DbType.Decimal, oDividendosRebatesLiberadasRow.FechaEntrega)
            db.AddInParameter(dbCommand, "@TipoDistribucion", DbType.String, oDividendosRebatesLiberadasRow.TipoDistribucion)
            db.AddInParameter(dbCommand, "@CodigoMoneda", DbType.String, oDividendosRebatesLiberadasRow.CodigoMoneda)
            db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, oDividendosRebatesLiberadasRow.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddOutParameter(dbCommand, "@Identificador", DbType.Decimal, nIdentificador)
            db.AddOutParameter(dbCommand, "@GrupoIdentificador", DbType.Decimal, nGrupoIdentificador)
            db.ExecuteNonQuery(dbCommand)
            nIdentificador = CType(db.GetParameterValue(dbCommand, "@Identificador"), Decimal)
            nGrupoIdentificador = CType(db.GetParameterValue(dbCommand, "@GrupoIdentificador"), Decimal)
            Insertar = True
        End Using
    End Function
    ''' Modifica un expediente en DividendosRebatesLiberadas tabla.
    Public Function Modificar(ByVal ob As DividendosRebatesLiberadasBE, ByVal dataRequest As DataSet) As Boolean
        Modificar = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DividendosRebatesLiberadas_Modificar")
            oDividendosRebatesLiberadasRow = CType(ob.DividendosRebatesLiberadas.Rows(0), DividendosRebatesLiberadasBE.DividendosRebatesLiberadasRow)
            db.AddInParameter(dbCommand, "@CodigoSBS", DbType.String, oDividendosRebatesLiberadasRow.CodigoSBS)
            db.AddInParameter(dbCommand, "@Identificador", DbType.Decimal, oDividendosRebatesLiberadasRow.Identificador)
            db.AddInParameter(dbCommand, "@FechaIDI", DbType.Decimal, oDividendosRebatesLiberadasRow.FechaIDI)
            db.AddInParameter(dbCommand, "@Factor", DbType.Decimal, oDividendosRebatesLiberadasRow.Factor)
            db.AddInParameter(dbCommand, "@FechaCorte", DbType.Decimal, oDividendosRebatesLiberadasRow.FechaCorte)
            db.AddInParameter(dbCommand, "@FechaEntrega", DbType.Decimal, oDividendosRebatesLiberadasRow.FechaEntrega)
            db.AddInParameter(dbCommand, "@TipoDistribucion", DbType.String, oDividendosRebatesLiberadasRow.TipoDistribucion)
            db.AddInParameter(dbCommand, "@CodigoMoneda", DbType.String, oDividendosRebatesLiberadasRow.CodigoMoneda)
            db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, oDividendosRebatesLiberadasRow.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            Modificar = True
        End Using
    End Function
    ' Elimina un expediente de DividendosRebatesLiberadas table por una llave primaria compuesta.
    Public Function Eliminar(ByVal CodigoSBS As String, ByVal nIdentificador As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DividendosRebatesLiberadas_Eliminar")
            db.AddInParameter(dbCommand, "@CodigoSBS", DbType.String, CodigoSBS)
            db.AddInParameter(dbCommand, "@Identificador", DbType.String, nIdentificador)
            db.AddInParameter(dbCommand, "@UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddOutParameter(dbCommand, "@p_Mensaje", DbType.String, 1)
            db.ExecuteNonQuery(dbCommand)
            Dim Mensaje As String = CType(db.GetParameterValue(dbCommand, "@p_Mensaje"), String)
            Return Mensaje
        End Using
    End Function
    ' Elimina un expediente de DividendosRebatesLiberadas table por una llave extranjera.
    Public Function EliminarPorCodigoISIN_CodigoNemonico(ByVal codigoISIN As String, ByVal codigoNemonico As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DividendosRebatesLiberadas_EliminarPorCodigoISIN_CodigoNemonico")
            db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, codigoISIN)
            db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function ConfirmarDividendoRebateLiberada(ByVal fondo As String, ByVal nemonico As String, ByVal fechavencimiento As String, ByVal identificador As String, ByVal monto As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim rpta As Boolean = False
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DividendosRebatesLiberadas_ConfirmarVencimiento")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, fondo)
            db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, nemonico)
            db.AddInParameter(dbCommand, "@p_FechaVencimiento", DbType.Decimal, fechavencimiento)
            db.AddInParameter(dbCommand, "@p_MontoNominalLocal", DbType.Decimal, monto)
            db.AddInParameter(dbCommand, "@p_Identificador", DbType.String, identificador)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            rpta = True
        End Using
        Return rpta
    End Function
    Public Function NemonicoFechaidi_VerificaActual(ByVal CodigoNemonico As String, ByVal FechaIDI As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim rpta As String = ""
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_VerificaActualNemonicoFechaIDI_DividendosRebatesLiberadas")
            db.AddInParameter(dbCommand, "@CodigoNemonico", DbType.String, CodigoNemonico)
            db.AddInParameter(dbCommand, "@FechaIDI", DbType.Decimal, FechaIDI)
            rpta = Convert.ToString(db.ExecuteScalar(dbCommand))
        End Using
        Return rpta
    End Function
    Public Function NemonicoFechaidi_Contar(ByVal CodigoNemonico As String, ByVal FechaIDI As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim rpta As String = ""
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ContarNemonicoFechaIDI_DividendosRebatesLiberadas")
            db.AddInParameter(dbCommand, "@CodigoNemonico", DbType.String, CodigoNemonico)
            db.AddInParameter(dbCommand, "@FechaIDI", DbType.Decimal, FechaIDI)
            rpta = Convert.ToString(db.ExecuteScalar(dbCommand))
        End Using
        Return rpta
    End Function
    Public Function NemonicoFechaidi_NumeroUnidades(ByVal CodigoNemonico As String, ByVal FechaIDI As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim rpta As String = ""
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_NumeroUnidadesNemonicoFechaIDI_DividendosRebatesLiberadas")
            db.AddInParameter(dbCommand, "@CodigoNemonico", DbType.String, CodigoNemonico)
            db.AddInParameter(dbCommand, "@FechaIDI", DbType.Decimal, FechaIDI)
            rpta = Convert.ToString(db.ExecuteScalar(dbCommand))
        End Using
        Return rpta
    End Function
    Public Function NemonicoFechaidi_Factor(ByVal CodigoNemonico As String, ByVal FechaIDI As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim rpta As String = ""
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_FactorNemonicoFechaIDI_DividendosRebatesLiberadas")
            db.AddInParameter(dbCommand, "@CodigoNemonico", DbType.String, CodigoNemonico)
            db.AddInParameter(dbCommand, "@FechaIDI", DbType.Decimal, FechaIDI)
            rpta = Convert.ToString(db.ExecuteScalar(dbCommand))
        End Using
        Return rpta
    End Function
    Public Function ObtenerDatosReporteRebates(ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal, ByVal CodigoNemonico As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Reporte_CalculoRebate")
            dbCommand.CommandTimeout = 1020
            db.AddInParameter(dbCommand, "@p_fechainicio", DbType.String, FechaInicio)
            db.AddInParameter(dbCommand, "@p_fechafin", DbType.String, FechaFin)
            db.AddInParameter(dbCommand, "@p_nemonico", DbType.String, CodigoNemonico)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function ExisteRebate(ByVal codigoRebate As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Existe_Rebate")
            db.AddInParameter(dbCommand, "@p_CodigoRebate", DbType.Decimal, codigoRebate)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'ini 20120524 OT 65289
    'JZAVALA 25/03/2013  - OT - 67090 Rebates. - SE AGREGA EL PARAMETRO DE ENTRADA @p_SumatoriaFondos.
    Public Function InsertarRebate(ByVal CodigoNemonico As String, ByVal DiasCalculo As Decimal, ByVal PorcRebate As Decimal, ByVal IndRango As String, ByVal Situacion As String, ByVal UsuarioCreacion As String, ByVal strSumatoriaFondos As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "pr_SIT_ins_Insertar_Rebates"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@CodigoNemonico", DbType.String, CodigoNemonico)
            db.AddInParameter(dbCommand, "@DiasCalculo", DbType.Decimal, DiasCalculo)
            db.AddInParameter(dbCommand, "@PorcRebate", DbType.Decimal, PorcRebate)
            db.AddInParameter(dbCommand, "@IndRango", DbType.String, IndRango)
            db.AddInParameter(dbCommand, "@Situacion", DbType.String, Situacion)
            db.AddInParameter(dbCommand, "@UsuarioCreacion", DbType.String, UsuarioCreacion)
            db.AddInParameter(dbCommand, "@p_SumatoriaFondos", DbType.String, strSumatoriaFondos)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function ObtenerCalculoRebateDetalle(ByVal CodigoNemonico As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Obtener_CalculoRebatesDetalle")
            db.AddInParameter(dbCommand, "@CodigoNemonico", DbType.String, CodigoNemonico)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function InsertarRebateDetalle(ByVal CodigoNemonico As String, ByVal ImporteInicio As Decimal, ByVal ImporteFin As Decimal, ByVal PorRebate As Decimal, ByVal Situacion As String, ByVal UsuarioCreacion As String, ByVal CodigoDetalle As Integer) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "pr_SIT_ins_Insertar_CalculoRebatesDetalle"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@CodigoNemonico", DbType.String, CodigoNemonico)
            db.AddInParameter(dbCommand, "@ImporteIni", DbType.Decimal, ImporteInicio)
            db.AddInParameter(dbCommand, "@ImporteFin", DbType.Decimal, ImporteFin)
            db.AddInParameter(dbCommand, "@PorcRebate", DbType.String, PorRebate)
            db.AddInParameter(dbCommand, "@Situacion", DbType.String, Situacion)
            db.AddInParameter(dbCommand, "@UsuarioCreacion", DbType.String, UsuarioCreacion)
            db.AddInParameter(dbCommand, "@CodigoDetalle", DbType.Int32, CodigoDetalle)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function EliminarRebateDetalle(ByVal CodigoDetalle As Integer) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "pr_SIT_del_Eliminar_CalculoRebatesDetalle"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@CodigoDetalle", DbType.Int32, CodigoDetalle)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function ObtenerCabeceraCalculoRebateDetalle(ByVal CodigoNemonico As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Obtener_CalculoRebatesCabecera")
            db.AddInParameter(dbCommand, "@CodigoNemonico", DbType.String, CodigoNemonico)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function EliminarRebateCabecera(ByVal CodigoNemonico As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "pr_SIT_del_Eliminar_CalculoRebatesCabecera"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@CodigoNemonico", DbType.String, CodigoNemonico)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function ObtenerCalculoRebateDetalleCabecera(ByVal CodigoNemonico As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Obtener_CalculoRebatesDetalleCabecera")
            db.AddInParameter(dbCommand, "@CodigoNemonico", DbType.String, CodigoNemonico)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function DividendosRebatesLiberadas_ObtenerMontoTotalDividendos(ByVal p_CodigoPortafolio As String, ByVal p_FechaOperacion As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DividendosRebatesLiberadas_ObtenerMontoTotalDividendos")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, p_CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, p_FechaOperacion)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function DividendosRebatesLiberadas_ObtenerMontosDividendosDetallado(ByVal p_CodigoPortafolio As String, ByVal p_FechaOperacion As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DividendosRebatesLiberadas_ObtenerMontosDividendosDetallado")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, p_CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, p_FechaOperacion)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'OT10916 - Fin
    'OT10927 - 21/11/2017 - Hanz Cocchi. Obtener montos para el reporte de rentabilidad de flujos
    Public Function DividendosRebatesLiberadas_ObtenerMontosDividendosDetallado_Flujo(ByVal p_CodigoPortafolio As String, ByVal p_FechaOperacion As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DividendosRebatesLiberadas_ObtMonDivDet_Flujos")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, p_CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, p_FechaOperacion)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function ConsultarDistribucionLib(ByVal p_CodigoPortafolio As String, ByVal p_FechaOperacion As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Sel_CantDistribucionLib")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, p_CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, p_FechaOperacion)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'OT10927 - Fin

    'OT12012
    Public Function ObtenerSaldo_NemonicoPortafolioFecha(ByVal CodigoNemonico As String, ByVal FechaCorte As Decimal, ByVal Portafolio As String) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim strrpta As Decimal = 0, rpta As String
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_ObtenerSaldo_NemonicoPortafolioFecha")
            db.AddInParameter(dbCommand, "@CodigoNemonico", DbType.String, CodigoNemonico)
            db.AddInParameter(dbCommand, "@FechaIDI", DbType.Decimal, FechaCorte)
            db.AddInParameter(dbCommand, "@CodigoPortafolio", DbType.String, Portafolio)
            rpta = Convert.ToString(db.ExecuteScalar(dbCommand))
            strrpta = Convert.ToDecimal(rpta)
        End Using
        Return Convert.ToDecimal(strrpta)
    End Function
    'OT12012 - Fin
End Class

