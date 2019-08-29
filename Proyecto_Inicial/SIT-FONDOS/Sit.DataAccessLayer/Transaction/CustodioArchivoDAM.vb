Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class CustodioArchivoDAM
    Private sqlCommand As String = ""
    Private oCustodioArchivoRow As CustodioArchivoBE.CustodioArchivoRow
    Public Function Seleccionar(ByVal CodigoPortafolio As String, ByVal CodigoISIN As String, _
                                ByVal FechaCreacion As Decimal, ByVal dataRequest As DataSet) As CustodioArchivoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "CustodioCargaTemporal_Seleccionar"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbCommand, "@CodigoPortafolio", DbType.String, CodigoPortafolio)
        db.AddInParameter(dbCommand, "@CodigoISIN", DbType.String, CodigoISIN)
        db.AddInParameter(dbCommand, "@FechaCreacion", DbType.String, FechaCreacion)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function Listar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CustodioCargaTemporal_Listar")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    'Public Sub CargarArchivo(ByVal sCodigoCustodio As String, ByVal sCustodioArchivo As String, ByVal CodigoPortafolio As String, ByVal sFechaCorte As String, _
    'ByVal dataRequest As DataSet)
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim sqlCommand As String = "CustodioCargaTemporal_Carga"
    '    Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
    '        db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, sCodigoCustodio)
    '        db.AddInParameter(dbCommand, "@CadenaArchivo", DbType.String, sCustodioArchivo)
    '        db.AddInParameter(dbCommand, "@CodigoPortafoliosbs", DbType.String, CodigoPortafolio)
    '        db.AddInParameter(dbCommand, "@FechaCorte", DbType.String, sFechaCorte)
    '        db.AddInParameter(dbCommand, "@UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
    '        db.AddInParameter(dbCommand, "@FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
    '        db.AddInParameter(dbCommand, "@HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
    '        db.ExecuteNonQuery(dbCommand)
    '    End Using
    'End Sub
    'OT11112 - 02/02/2018 - Ian Pastor M.
    'Descripción: Carga la información de custodios a una tabla temporal del SIT
    Public Function CargarArchivo(ByVal sCodigoCustodio As String, ByVal sCustodioArchivo As String, ByVal CodigoPortafolio As String, ByVal sFechaCorte As String, _
    ByVal dataRequest As DataSet) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "CustodioCargaTemporal_Carga"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, sCodigoCustodio)
            db.AddInParameter(dbCommand, "@CadenaArchivo", DbType.String, sCustodioArchivo)
            db.AddInParameter(dbCommand, "@CodigoPortafoliosbs", DbType.String, CodigoPortafolio)
            db.AddInParameter(dbCommand, "@FechaCorte", DbType.String, sFechaCorte)
            db.AddInParameter(dbCommand, "@UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Sub Insertar(ByVal ob As CustodioArchivoBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "CustodioCargaTemporal_Insertar"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            oCustodioArchivoRow = CType(ob.CustodioArchivo.Rows(0), CustodioArchivoBE.CustodioArchivoRow)
            db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, oCustodioArchivoRow.CodigoCustodio)
            db.AddInParameter(dbCommand, "@CuentaDepositariaCustodio", DbType.String, oCustodioArchivoRow.CuentaDepositariaCustodio)
            db.AddInParameter(dbCommand, "@CodigoPortafolio", DbType.String, oCustodioArchivoRow.CodigoPortafolio)
            db.AddInParameter(dbCommand, "@CodigoISIN", DbType.String, oCustodioArchivoRow.CodigoISIN)
            db.AddInParameter(dbCommand, "@DescripcionTitulo", DbType.String, oCustodioArchivoRow.DescripcionTitulo)
            db.AddInParameter(dbCommand, "@TipoIntermediario", DbType.String, oCustodioArchivoRow.TipoIntermediario)
            db.AddInParameter(dbCommand, "@CodigoIntermediario", DbType.String, oCustodioArchivoRow.CodigoIntermediario)
            db.AddInParameter(dbCommand, "@ValorNominal", DbType.String, oCustodioArchivoRow.ValorNominal)
            db.AddInParameter(dbCommand, "@SaldoContable", DbType.String, oCustodioArchivoRow.SaldoContable)
            db.AddInParameter(dbCommand, "@SaldoDisponible", DbType.String, oCustodioArchivoRow.SaldoDisponible)
            db.AddInParameter(dbCommand, "@Dato1", DbType.String, oCustodioArchivoRow.Dato1)
            db.AddInParameter(dbCommand, "@Diferencia", DbType.String, oCustodioArchivoRow.Diferencia)
            db.AddInParameter(dbCommand, "@UsuarioCreacion", DbType.String, oCustodioArchivoRow.UsuarioCreacion)
            db.AddInParameter(dbCommand, "@FechaCreacion", DbType.Decimal, oCustodioArchivoRow.FechaCreacion)
            db.AddInParameter(dbCommand, "@HoraCreacion", DbType.String, oCustodioArchivoRow.HoraCreacion)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Function Modificar(ByVal ob As CustodioArchivoBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CustodioCargaTemporal_Modificar")
            oCustodioArchivoRow = CType(ob.CustodioArchivo.Rows(0), CustodioArchivoBE.CustodioArchivoRow)
            db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, oCustodioArchivoRow.CodigoCustodio)
            db.AddInParameter(dbCommand, "@CuentaDepositariaCustodio", DbType.String, oCustodioArchivoRow.CuentaDepositariaCustodio)
            db.AddInParameter(dbCommand, "@CodigoPortafolio", DbType.String, oCustodioArchivoRow.CodigoPortafolio)
            db.AddInParameter(dbCommand, "@CodigoISIN", DbType.String, oCustodioArchivoRow.CodigoISIN)
            db.AddInParameter(dbCommand, "@DescripcionTitulo", DbType.String, oCustodioArchivoRow.DescripcionTitulo)
            db.AddInParameter(dbCommand, "@TipoIntermediario", DbType.String, oCustodioArchivoRow.TipoIntermediario)
            db.AddInParameter(dbCommand, "@CodigoIntermediario", DbType.String, oCustodioArchivoRow.CodigoIntermediario)
            db.AddInParameter(dbCommand, "@ValorNominal", DbType.String, oCustodioArchivoRow.ValorNominal)
            db.AddInParameter(dbCommand, "@SaldoContable", DbType.String, oCustodioArchivoRow.SaldoContable)
            db.AddInParameter(dbCommand, "@SaldoDisponible", DbType.String, oCustodioArchivoRow.SaldoDisponible)
            db.AddInParameter(dbCommand, "@Dato1", DbType.String, oCustodioArchivoRow.Dato1)
            db.AddInParameter(dbCommand, "@Diferencia", DbType.String, oCustodioArchivoRow.Diferencia)
            db.AddInParameter(dbCommand, "@UsuarioModificacion", DbType.String, oCustodioArchivoRow.UsuarioModificacion)
            db.AddInParameter(dbCommand, "@FechaModificacion", DbType.Decimal, oCustodioArchivoRow.FechaModificacion)
            db.AddInParameter(dbCommand, "@HoraModificacion", DbType.String, oCustodioArchivoRow.HoraModificacion)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function Eliminar(ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CustodioCargaTemporal_Eliminar")
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function ImportarArchivo(ByVal sCodigoCustodio As String, ByVal sCodigoPortafolio As String, ByVal nFechaCorte As Long, ByVal sFlagBorrado As Decimal) As Boolean
        ImportarArchivo = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CustodioInformacion_Importar")
            db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, sCodigoCustodio)
            db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, sCodigoPortafolio)
            db.AddInParameter(dbCommand, "@FechaCorte", DbType.Decimal, nFechaCorte)
            db.AddInParameter(dbCommand, "@FlagBorrado", DbType.Decimal, sFlagBorrado)
            db.ExecuteNonQuery(dbCommand)
            ImportarArchivo = True
        End Using
    End Function
    Public Function VerificaCustodioInformacion(ByVal nFechaCorte As Long) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CustodioInformacion_Verifica")
            db.AddInParameter(dbCommand, "@FechaCorte", DbType.Decimal, nFechaCorte)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function VerificaPreCarga(ByVal sCodigoCustodio As String, ByVal sCodigoPortafolio As String, ByVal nFechaCorte As Long) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CustodioCargaTemporal_VerificaPreCarga")
            db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, sCodigoCustodio)
            db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, sCodigoPortafolio)
            db.AddInParameter(dbCommand, "@FechaCorte", DbType.Decimal, nFechaCorte)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function InstrumentosNoRegistrados(ByVal nFechaCorte As Long, ByVal sPortafolioCodigo As String, ByVal sCodigoCustodio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("InstrumentosNoRegistrados_Listar")
            db.AddInParameter(dbCommand, "@FechaCorte", DbType.Decimal, nFechaCorte)
            db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, sPortafolioCodigo)
            db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, sCodigoCustodio)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function InstrumentosNoReportados(ByVal nFechaCorte As Long, ByVal sPortafolioCodigo As String, ByVal sCodigoCustodio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("TitulosCartera_No_Custodio_Listar")
            db.AddInParameter(dbCommand, "@FechaCorte", DbType.Decimal, nFechaCorte)
            db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, sPortafolioCodigo)
            db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, sCodigoCustodio)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function InstrumentosConciliados(ByVal nFechaCorte As Long, ByVal sPortafolioCodigo As String, ByVal sCodigoCustodio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("InstrumentosConciliados_Listar")
            db.AddInParameter(dbCommand, "@FechaCorte", DbType.Decimal, nFechaCorte)
            db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, sPortafolioCodigo)
            db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, sCodigoCustodio)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function GeneraSaldos(ByVal decNewFecha As Decimal, ByVal decOldFecha As Decimal, ByVal strCodigoPortafolioSBS As String, ByVal dataRequest As DataSet) As Boolean
        GeneraSaldos = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_Aperturar")
            dbCommand.CommandTimeout = 1020
            db.AddInParameter(dbCommand, "@FechaSaldo", DbType.Decimal, decNewFecha)
            db.AddInParameter(dbCommand, "@decFechaAnterior", DbType.Decimal, decOldFecha)
            db.AddInParameter(dbCommand, "@CodigoPortafolio", DbType.String, strCodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            GeneraSaldos = True
        End Using
    End Function
    Public Function InstrumentosPorConciliar(ByVal nFechaCorte As Decimal, ByVal sPortafolioCodigo As String, ByVal sCodigoCustodio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("InstrumentosPorConciliar_Listar")
            db.AddInParameter(dbCommand, "@FechaCorte", DbType.Decimal, nFechaCorte)
            db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, sPortafolioCodigo)
            db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, sCodigoCustodio)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function ActualizaSaldosInfCustodio(ByVal nFechaCorte As Decimal, ByVal sCodigoMnemonico As String, _
                                                ByVal sCodigoISIN As String, ByVal sCodigoPortafolioSBS As String, _
                                                ByVal sCodigoCustodio As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ActualizaSaldosInfCustodio_Modificar")
            db.AddInParameter(dbCommand, "@FechaCorte", DbType.Decimal, nFechaCorte)
            db.AddInParameter(dbCommand, "@CodigoMnemonico", DbType.String, sCodigoMnemonico)
            db.AddInParameter(dbCommand, "@CodigoISIN", DbType.String, sCodigoISIN)
            db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, sCodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, sCodigoCustodio)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function InstrumentosDiferencias(ByVal nFechaCorte As Long, ByVal sPortafolioCodigo As String, ByVal sCodigoCustodio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DiferenciasInstrumentos_Listar")
            db.AddInParameter(dbCommand, "@FechaCorte", DbType.Decimal, nFechaCorte)
            db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, sPortafolioCodigo)
            db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, sCodigoCustodio)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function InstrumentosDiferenciasCDet(ByVal nFechaCorte As Long, ByVal sPortafolioCodigo As String, ByVal sCodigoCustodio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DiferenciasInstrumentosRecompile_Listar_ConDetalle")
            db.AddInParameter(dbCommand, "@FechaCorte", DbType.Decimal, nFechaCorte)
            db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, sPortafolioCodigo)
            db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, sCodigoCustodio)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function InstrumentosDiferenciasVarios(ByVal nFechaCorte As Long, ByVal sPortafolioCodigo As String, ByVal sCodigoCustodio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DiferenciasInstrumentosRecompile_ListarVarios")
            db.AddInParameter(dbCommand, "@FechaCorte", DbType.Decimal, nFechaCorte)
            db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, sPortafolioCodigo)
            db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, sCodigoCustodio)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function InstrumentosDiferenciasCarteraCustodio(ByVal fecha As Decimal, ByVal sPortafolioCodigo As String, ByVal sCodigoCustodio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DiferenciasInstrumentosCarteraCustodio_Listar")
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, sPortafolioCodigo)
            db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, sCodigoCustodio)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function DIFerenciasCustodioResumen_Listar(ByVal fecha As Decimal, ByVal sPortafolioCodigo As String, ByVal sCodigoCustodio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DIFerenciasResumen_Listar")
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, sPortafolioCodigo)
            db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, sCodigoCustodio)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    'OT 10238 20/04/2017 - Carlos Espejo
    'Descripcion: Se genera el saldo incial de una fecha
    Public Sub GeneraSaldoBanco(FechaSaldo As Decimal ,FechaAnterior As Decimal,CodigoPortafolio As String ,ClaseCuenta As String , ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "sp_SIT_GeneraSaldoBanco"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@p_FechaSaldo", DbType.Decimal, FechaSaldo)
            db.AddInParameter(dbCommand, "@p_decFechaAnterior", DbType.Decimal, FechaAnterior)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_ClaseCuenta", DbType.String, ClaseCuenta)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
End Class