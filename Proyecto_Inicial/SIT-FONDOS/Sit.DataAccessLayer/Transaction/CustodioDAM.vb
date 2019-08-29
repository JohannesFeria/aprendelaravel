Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

	Public class CustodioDAM

    Private sqlCommand As String = ""
    Private oCustodioRow As CustodioBE.CustodioRow
		Public Sub New()

    End Sub

#Region "/* Funciones Seleccionar */"

    Public Function SeleccionarCuentasDepositaria(ByVal codigoCustodio As String) As CustodioCuentaDepositariaBE

        Dim oCuentas As New CustodioCuentaDepositariaBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CustodioCuentaDepositaria_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, codigoCustodio)

        db.LoadDataSet(dbCommand, oCuentas, "CustodioCuentaDepositaria")

        Return oCuentas

    End Function

    Public Function Seleccionar(ByVal codigoCustodio As String) As CustodioBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Custodio_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, codigoCustodio)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function SeleccionarCustodioArchivoPlano(ByVal codigoCustodio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CustodioArchivoPlano_Seleccion")
            db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, codigoCustodio)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Custodio_Listar")
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function

    Public Function ListarCuentasDepositariasPorCustodio(ByVal datarequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Custodio_ListarCuentasDepositarias")

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function ListarTitulosAsociadosCustodios(ByVal sFechaSaldo As String, _
                                                    ByVal sCodigoPortafolioSBS As String, _
                                                    ByVal sCodigoISIN As String, _
                                                    ByVal sCodigoCustodio As String, _
                                                    ByVal sCodigoTipoTitulo As String, _
                                                    ByVal sTipoRenta As String, _
                                                    ByVal sCodigoMnemonico As String, _
                                                    ByVal sCodigoMoneda As String, _
                                                    ByVal sConsulta As String, _
                                                    ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TitulosAsociadosACustodios_Listar")
        dbCommand.CommandTimeout = 120
        db.AddInParameter(dbCommand, "@FechaSaldo", DbType.Decimal, sFechaSaldo)
        db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, sCodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@CodigoISIN", DbType.String, sCodigoISIN)
        db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, sCodigoCustodio)
        db.AddInParameter(dbCommand, "@CodigoTipoTitulo", DbType.String, sCodigoTipoTitulo)
        db.AddInParameter(dbCommand, "@CodigoTipoRenta", DbType.String, sTipoRenta)
        db.AddInParameter(dbCommand, "@CodigoMnemonico", DbType.String, sCodigoMnemonico)
        db.AddInParameter(dbCommand, "@CodigoMoneda", DbType.String, sCodigoMoneda)
        db.AddInParameter(dbCommand, "@Consulta", DbType.String, sConsulta)

        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarTitulosAsociadosCustodiosC1(ByVal sFechaSaldo As String, _
                                                ByVal sCodigoPortafolioSBS As String, _
                                                ByVal sCodigoISIN As String, _
                                                ByVal sCodigoCustodio As String, _
                                                ByVal sCodigoTipoTitulo As String, _
                                                ByVal sTipoRenta As String, _
                                                ByVal sCodigoMnemonico As String, _
                                                ByVal sCodigoMoneda As String, _
                                               ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TitulosAsociadosACustodios_ListarC1")
        dbCommand.CommandTimeout = 120
        db.AddInParameter(dbCommand, "@FechaSaldo", DbType.Decimal, sFechaSaldo)
        db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, sCodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@CodigoISIN", DbType.String, sCodigoISIN)
        db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, sCodigoCustodio)
        db.AddInParameter(dbCommand, "@CodigoTipoTitulo", DbType.String, sCodigoTipoTitulo)
        db.AddInParameter(dbCommand, "@CodigoTipoRenta", DbType.String, sTipoRenta)
        db.AddInParameter(dbCommand, "@CodigoMnemonico", DbType.String, sCodigoMnemonico)
        db.AddInParameter(dbCommand, "@CodigoMoneda", DbType.String, sCodigoMoneda)

        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarTitulosAsociadosCustodiosC2(ByVal sFechaSaldo As String, _
                                                ByVal sCodigoPortafolioSBS As String, _
                                                ByVal sCodigoISIN As String, _
                                                ByVal sCodigoCustodio As String, _
                                                ByVal sCodigoTipoTitulo As String, _
                                                ByVal sTipoRenta As String, _
                                                ByVal sCodigoMnemonico As String, _
                                                ByVal sCodigoMoneda As String, _
                                                ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TitulosAsociadosACustodios_ListarC2")
        dbCommand.CommandTimeout = 120
        db.AddInParameter(dbCommand, "@FechaSaldo", DbType.Decimal, sFechaSaldo)
        db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, sCodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@CodigoISIN", DbType.String, sCodigoISIN)
        db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, sCodigoCustodio)
        db.AddInParameter(dbCommand, "@CodigoTipoTitulo", DbType.String, sCodigoTipoTitulo)
        db.AddInParameter(dbCommand, "@CodigoTipoRenta", DbType.String, sTipoRenta)
        db.AddInParameter(dbCommand, "@CodigoMnemonico", DbType.String, sCodigoMnemonico)
        db.AddInParameter(dbCommand, "@CodigoMoneda", DbType.String, sCodigoMoneda)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function SeleccionarPorFiltro(ByVal codigoCustodio As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As CustodioBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Custodio_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, codigoCustodio)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        Dim objeto As New CustodioBE
        db.LoadDataSet(dbCommand, objeto, "Custodio")
        Return objeto
    End Function
    Public Function Seleccionar1(ByVal codigoCustodio As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As CustodioBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Custodio_Seleccionar1")

        db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, codigoCustodio)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        Dim objeto As New CustodioBE
        db.LoadDataSet(dbCommand, objeto, "Custodio")
        Return objeto
    End Function


    Public Function ListarPortafoliosBBH() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("ListarPortafoliosBBH")

        Return db.ExecuteDataSet(dbCommand)
    End Function
#End Region

    Public Function Insertar(ByVal oCustodioBE As CustodioBE, ByVal oCuentas As CustodioCuentaDepositariaBE, ByVal dataRequest As DataSet)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Custodio_Insertar")

        Dim intNroFilas, intIndice As Integer

        intNroFilas = oCuentas.CustodioCuentaDepositaria.Rows.Count
        For intIndice = 0 To intNroFilas - 1

            oCuentas.CustodioCuentaDepositaria.Rows(intIndice)("UsuarioCreacion") = DataUtility.ObtenerValorRequest(dataRequest, "Usuario")
            oCuentas.CustodioCuentaDepositaria.Rows(intIndice)("FechaCreacion") = DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha"))
            oCuentas.CustodioCuentaDepositaria.Rows(intIndice)("HoraCreacion") = DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha"))
            oCuentas.CustodioCuentaDepositaria.Rows(intIndice)("Host") = DataUtility.ObtenerValorRequest(dataRequest, "Host")

        Next

        oCustodioRow = DirectCast(oCustodioBE.Custodio.Rows(0), CustodioBE.CustodioRow)

        db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, oCustodioRow.CodigoCustodio)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oCustodioRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oCustodioRow.Situacion)

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        'db.AddInParameter(dbCommand, "@p_xmlCustodioCuentaDepositaria", DbType.String, oCuentas.GetXml().Replace("<CustodioCuentaDepositariaBE xmlns=""http://tempuri.org/CustodioCuentaDepositariaBE.xsd"">", "<CustodioCuentaDepositariaBE>"))

        db.ExecuteNonQuery(dbCommand)
        InsertarDet(oCuentas, dataRequest)
    End Function

    Public Function Modificar(ByVal oCustodioBE As CustodioBE, ByVal oCuentas As CustodioCuentaDepositariaBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Custodio_Modificar")

        Dim intNroFilas, intIndice As Integer

        intNroFilas = oCuentas.CustodioCuentaDepositaria.Rows.Count
        For intIndice = 0 To intNroFilas - 1

            oCuentas.CustodioCuentaDepositaria.Rows(intIndice)("UsuarioCreacion") = DataUtility.ObtenerValorRequest(dataRequest, "Usuario")
            oCuentas.CustodioCuentaDepositaria.Rows(intIndice)("FechaCreacion") = DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha"))
            oCuentas.CustodioCuentaDepositaria.Rows(intIndice)("HoraCreacion") = DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha"))
            oCuentas.CustodioCuentaDepositaria.Rows(intIndice)("UsuarioModificacion") = DataUtility.ObtenerValorRequest(dataRequest, "Usuario")
            oCuentas.CustodioCuentaDepositaria.Rows(intIndice)("FechaModificacion") = DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha"))
            oCuentas.CustodioCuentaDepositaria.Rows(intIndice)("HoraModificacion") = DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha"))
            oCuentas.CustodioCuentaDepositaria.Rows(intIndice)("Host") = DataUtility.ObtenerValorRequest(dataRequest, "Host")

        Next

        oCustodioRow = DirectCast(oCustodioBE.Custodio.Rows(0), CustodioBE.CustodioRow)

        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oCustodioRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oCustodioRow.Situacion)

        db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, oCustodioRow.CodigoCustodio)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        'db.AddInParameter(dbCommand, "@p_xmlCustodioCuentaDepositaria", DbType.String, oCuentas.GetXml().Replace("<CustodioCuentaDepositariaBE xmlns=""http://tempuri.org/CustodioCuentaDepositariaBE.xsd"">", "<CustodioCuentaDepositariaBE>"))
        'db.AddInParameter(dbCommand, "@p_xmlCustodioCuentaDepositaria", DbType.String, oCuentas.GetXml().Replace("<CustodioCuentaDepositariaBE xmlns=""http://tempuri.org/CustodioCuentaDepositariaBE.xsd"">", "<CustodioCuentaDepositariaBE>"))
        '        db.AddInParameter(dbCommand, "@p_xmlCustodioCuentaDepositaria", DbType.String, oCuentas.GetXml().Replace("<CustodioCuentaDepositariaBE xmlns=http://tempuri.org/CuentaDepositariaBE.xsd>", "<CustodioCuentaDepositariaBE>"))

        db.ExecuteNonQuery(dbCommand)

        InsertarDet(oCuentas, dataRequest)

        Return True

    End Function


    Public Function InsertarDet(ByVal oCuentas As CustodioCuentaDepositariaBE, ByVal dataRequest As DataSet)

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim oCustodioCuentaDepositariaRow As CustodioCuentaDepositariaBE.CustodioCuentaDepositariaRow
        Dim intNroFilas, intIndice As Integer

        intNroFilas = oCuentas.CustodioCuentaDepositaria.Rows.Count
        For intIndice = 0 To intNroFilas - 1
            'oCuentaTerceros.CuentaTerceros.Rows(intIndice)("UsuarioCreacion") = DataUtility.ObtenerValorRequest(dataRequest, "Usuario")
            ' oCuentaTerceros.CuentaTerceros.Rows(intIndice)("FechaCreacion") = DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha"))
            'oCuentaTerceros.CuentaTerceros.Rows(intIndice)("HoraCreacion") = DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha"))
            'oCuentaTerceros.CuentaTerceros.Rows(intIndice)("Host") = DataUtility.ObtenerValorRequest(dataRequest, "Host")

            oCustodioCuentaDepositariaRow = DirectCast(oCuentas.CustodioCuentaDepositaria.Rows(intIndice), CustodioCuentaDepositariaBE.CustodioCuentaDepositariaRow)

            '  db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, oCuentaTercerosRow.CodigoCustodio)
            '   db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oCuentaTercerosRow.Descripcion)
            '  db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oCuentaTercerosRow.Situacion)
            Dim dbCommand As dbCommand = db.GetStoredProcCommand("CustodioCuentaDepositaria_Insertar")

            db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, oCustodioCuentaDepositariaRow.CodigoCustodio)
            db.AddInParameter(dbCommand, "@p_CodigoCuentaDepositaria", DbType.String, oCustodioCuentaDepositariaRow.CodigoCuentaDepositaria)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, IIf(oCustodioCuentaDepositariaRow.CodigoPortafolioSBS = "", DBNull.Value, oCustodioCuentaDepositariaRow.CodigoPortafolioSBS))
            db.AddInParameter(dbCommand, "@p_FisicoAnotacion", DbType.String, oCustodioCuentaDepositariaRow.FisicoAnotacion)
            db.AddInParameter(dbCommand, "@p_NombreCuenta", DbType.String, oCustodioCuentaDepositariaRow.NombreCuenta)
            db.AddInParameter(dbCommand, "@p_Observaciones", DbType.String, oCustodioCuentaDepositariaRow.Observaciones)
            db.AddInParameter(dbCommand, "@p_Estado", DbType.String, oCustodioCuentaDepositariaRow.Estado)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oCustodioCuentaDepositariaRow.Situacion)

            db.ExecuteNonQuery(dbCommand)
        Next

    End Function


    Public Function Eliminar(ByVal codigoCustodio As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Custodio_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, codigoCustodio)

        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function VerificarCustodioCuentaDepositaria(ByVal strCodigoCustodio As String, ByVal strCodigoCuentaDepositaria As String, ByVal strCodigoPortafolioSBS As String) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CustodioCuentaDepositaria_VerificarCodigo")
        'Dim DstTabla As New DataSet
        Dim lngcontar As Long
        Dim IntI As Integer

        db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, strCodigoCustodio)
        db.AddInParameter(dbCommand, "@p_CodigoCuentaDepositaria", DbType.String, strCodigoCuentaDepositaria)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, strCodigoPortafolioSBS)

        lngcontar = db.ExecuteScalar(dbCommand)

        Return lngcontar
    End Function

    Public Function SeleccionarCustodioValores(ByVal strNemonico As String, ByVal strCodigoPortafolioSBS As String, ByVal fecha As Decimal) As DataSet
        Dim oValoresBE As New ValoresBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Valores_SeleccionarCustodios")

        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, strNemonico)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, strCodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, fecha)
        db.LoadDataSet(dbCommand, oValoresBE, "Valor")

        Return oValoresBE
    End Function

    Public Function SeleccionarSaldoDisponible(ByVal strNemonico As String, ByVal strCodigoPortafolioSBS As String, ByVal fechaEmision As Decimal) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Custodio_SeleccionarSaldoDisponible")
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, strNemonico)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, strCodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_FechaEmision", DbType.Decimal, fechaEmision)
        Return CDec(db.ExecuteScalar(dbCommand))
    End Function


    'Public Function ActualizarCustodioValores(ByVal codigoCustodio As String,byval codigoNemonico as String,byval codigoPortafolioSBS as String,byval fechaOperacion as Decimal,byval codigoOrdenPreOrden as String,byval horaOperacion as String,byval codigoOperacion as String, ) As Boolean
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim dbCommand As dbCommand = db.GetStoredProcCommand("Custodio_Eliminar")

    '    db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, codigoCustodio)

    '    db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
    '    db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
    '    db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
    '    db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

    '    db.ExecuteNonQuery(dbCommand)
    '    Return True
    'End Function

    'HDG INC 64460	20120102
    Public Function SeleccionaSaldosCustodioxNemonico(ByVal Portafolio As String, ByVal codigoCustodio As String, ByVal codigoNemonico As String, ByVal fechaProceso As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_ConsultaSaldosCustodioxNemonico")
        dbCommand.CommandTimeout = 1020

        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio)
        db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, codigoCustodio)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fechaProceso)

        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ProcesarSaldosCustodioxMnemonico(ByVal Fondo As String, ByVal Custodio As String, ByVal Mnemonico As String, ByVal fechaProceso As Decimal, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("GeneraSaldosCustodioPorNemonico")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_FechaSaldo", DbType.Decimal, fechaProceso)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, Mnemonico)
        db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, Custodio)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, Fondo)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function Cuentadepositaria_Portafolio(ByVal CodigoPortafolioSBS As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_Cuentadepositaria_Portafolio")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
    Public Sub Insertar_Cuentadepositaria_Portafolio(ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String, ByVal CuentaDepositarias As String, _
    ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_InsertarCuentadepositaria_Portafolio")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, CodigoCustodio)
        db.AddInParameter(dbCommand, "@p_CuentaDepositaria", DbType.String, CuentaDepositarias)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Sub
    ''' <summary>
    ''' Elimina un registro de la tabla CustodioCuentaDepositaria por su clave principal.
    ''' </summary>
    ''' <param name="CodigoPortafolioSBS">Código del Portafolio</param>
    ''' <param name="CodigoCustodio">Código del Custodio</param>
    ''' <param name="CuentaDepositarias">Código de la Cuenta Depositaria</param>
    ''' <returns></returns>
    ''' <remarks>Modificado por HCOCCHI 20171019</remarks>
    Public Sub Del_Cuentadepositaria_Portafolio(ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String, ByVal CuentaDepositarias As String)
        'OT 10883 20/10/2017 Hanz Cocchi
        'Se agregan los parámetros CodigoCustodio y CuentaDepositarias eliminar correctamente los registros
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_del_Cuentadepositaria_Portafolio")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, CodigoCustodio)            'OT 10883 20/10/2017 Hanz Cocchi
        db.AddInParameter(dbCommand, "@p_CuentaDepositaria", DbType.String, CuentaDepositarias)     'OT 10883 20/10/2017 Hanz Cocchi
        db.ExecuteNonQuery(dbCommand)
        'OT 10883 FIN 
    End Sub
End Class