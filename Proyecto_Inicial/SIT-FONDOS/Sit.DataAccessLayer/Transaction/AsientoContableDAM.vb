Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports Sit.BusinessEntities
''' <summary>
''' Clase para el acceso de los datos para AsientoContable tabla.
''' </summary>
Public Class AsientoContableDAM
    'OT10783 - refactorización de código para el tipo de dato IDISPOSABLE dbCommand
    Private oAsientoContable As AsientoContableBE.AsientoContableRow
    Public Sub New()
    End Sub
    ''' <summary>
    ''' Inserta un expediente en AsientoContable tabla.
    ''' </summary>
    ''' <param name="codigoAsiento"></param>
    ''' <param name="numeroAsiento"></param>
    ''' <param name="codigoMatriz"></param>
    ''' <param name="fechaOperacion"></param>
    ''' <param name="importe"></param>
    ''' <returns></returns>
    Public Function Insertar(ByVal codigoAsiento As String, ByVal numeroAsiento As String, ByVal codigoMatriz As Decimal, ByVal fechaOperacion As Decimal, ByVal importe As Decimal) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("AsientoContable_Insertar")
            db.AddInParameter(dbCommand, "@p_CodigoAsiento", DbType.String, codigoAsiento)
            db.AddInParameter(dbCommand, "@p_NumeroAsiento", DbType.String, numeroAsiento)
            db.AddInParameter(dbCommand, "@p_CodigoMatriz", DbType.Decimal, codigoMatriz)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaOperacion)
            db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, importe)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Function
    ''' <summary>
    ''' Inserta un expediente en AsientoContable tabla.
    ''' </summary>
    ''' <param name="CodigoPortafolio"></param>
    ''' <param name="numeroAsiento"></param>
    ''' <param name="Secuencia"></param>
    ''' <param name="fechaAsiento"></param>
    ''' <param name="CodigoMoneda"></param>
    ''' <param name="DebeHaber"></param>
    ''' <param name="Importe"></param>
    ''' <param name="CuentaContable"></param>
    ''' <returns></returns>
    Public Function InsertarRevision(ByVal obj As AsientoContableBE.AsientoContableRow, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("AsientoContable_InsertarRevision")
            oAsientoContable = CType(obj, AsientoContableBE.AsientoContableRow)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oAsientoContable.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_NumeroAsiento", DbType.String, oAsientoContable.NumeroAsiento)
            db.AddInParameter(dbCommand, "@p_FechaAsiento", DbType.Decimal, oAsientoContable.FechaAsiento)
            db.AddInParameter(dbCommand, "@p_CodigoMatrizContable", DbType.String, oAsientoContable.CodigoMatrizContable)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oAsientoContable.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_Glosa", DbType.String, oAsientoContable.Glosa)
            db.AddInParameter(dbCommand, "@p_DebeHaber", DbType.String, oAsientoContable.DebeHaber)
            db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, oAsientoContable.Importe)
            db.AddInParameter(dbCommand, "@p_ImporteOrigen", DbType.Decimal, oAsientoContable.ImporteOrigen)
            db.AddInParameter(dbCommand, "@p_CuentaContable", DbType.String, oAsientoContable.CuentaContable)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Function
    ''' <summary>
    ''' Selecciona un solo expediente de AsientoContable tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Seleccionar(ByVal codigoAsiento As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("AsientoContable_Seleccionar")
            db.AddInParameter(dbCommand, "@p_CodigoAsiento", DbType.String, codigoAsiento)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    ''' <summary>
    ''' Selecciona expedientes de AsientoContable tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoMatriz"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoMatriz(ByVal codigoMatriz As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("AsientoContable_SeleccionarPorCodigoMatriz")
            db.AddInParameter(dbCommand, "@p_CodigoMatriz", DbType.Decimal, codigoMatriz)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    ''' <summary>
    ''' Lista todos los expedientes de AsientoContable tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("AsientoContable_Listar")
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    ''' <summary>
    ''' Lista todos los numeros de asiento
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function ListarNumerosAsiento(ByVal IndiceReferencial As String, ByVal portafolio As String, ByVal fechaAsiento As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("AsientoContable_ListarNumeroAsiento")
            db.AddInParameter(dbCommand, "@p_IndiceReferencial", DbType.String, IndiceReferencial)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio) 'RGF 20081203
            db.AddInParameter(dbCommand, "@p_FechaAsiento", DbType.Decimal, fechaAsiento) 'RGF 20081203
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function ModificarRevision(ByVal CodigoPortafolioSBSKey As String, ByVal NumeroAsientoKey As String, ByVal SecuenciaKey As String, ByVal obj As AsientoContableBE.AsientoContableRow, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("AsientoContable_ModificarRevision")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBSKey)
            db.AddInParameter(dbCommand, "@p_NumeroAsiento", DbType.String, NumeroAsientoKey)
            db.AddInParameter(dbCommand, "@p_Secuencia", DbType.String, SecuenciaKey)
            db.AddInParameter(dbCommand, "@p_FechaAsiento", DbType.Decimal, obj.FechaAsiento)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, obj.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_Glosa", DbType.String, obj.Glosa)
            db.AddInParameter(dbCommand, "@p_DebeHaber", DbType.String, obj.DebeHaber)
            db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, obj.Importe)
            db.AddInParameter(dbCommand, "@p_ImporteOrigen", DbType.Decimal, obj.ImporteOrigen)
            db.AddInParameter(dbCommand, "@p_CuentaContable", DbType.String, obj.CuentaContable)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function Modificar(ByVal oAsientoContableBE As AsientoContableBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("AsientoContable_Modificar")
            Dim intIndice, intNroFilas As Integer
            intNroFilas = oAsientoContableBE.AsientoContable.Rows.Count
            For intIndice = 0 To intNroFilas - 1
                oAsientoContableBE.AsientoContable.Rows(intIndice)("UsuarioModificacion") = DataUtility.ObtenerValorRequest(dataRequest, "Usuario")
                oAsientoContableBE.AsientoContable.Rows(intIndice)("FechaModificacion") = DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha"))
                oAsientoContableBE.AsientoContable.Rows(intIndice)("HoraModificacion") = DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha"))
                oAsientoContableBE.AsientoContable.Rows(intIndice)("UsuarioCreacion") = DataUtility.ObtenerValorRequest(dataRequest, "Usuario")
                oAsientoContableBE.AsientoContable.Rows(intIndice)("FechaCreacion") = DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha"))
                oAsientoContableBE.AsientoContable.Rows(intIndice)("HoraCreacion") = DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha"))
                oAsientoContableBE.AsientoContable.Rows(intIndice)("Host") = DataUtility.ObtenerValorRequest(dataRequest, "Host")
            Next
            db.AddInParameter(dbCommand, "@p_XmlAsientoContable", DbType.String, oAsientoContableBE.GetXml().Replace("<AsientoContableBE xmlns=""http://tempuri.org/AsientoContableBE.xsd"">", "<AsientoContableBE>"))
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    ''' <summary>
    ''' Elimina un expediente de AsientoContable table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoAsiento As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("AsientoContable_Eliminar")
            db.AddInParameter(dbCommand, "@p_CodigoAsiento", DbType.String, codigoAsiento)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    ''' <summary>
    ''' Elimina un expediente de AsientoContable table por sus tres llaves.
    ''' <summary>
    Public Function EliminarRevision(ByVal CodigoPortafolioSBSKey As String, ByVal NumeroAsientoKey As String, ByVal SecuenciaKey As String, ByVal fecha As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("AsientoContable_EliminarRevision")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBSKey)
            db.AddInParameter(dbCommand, "@p_NumeroAsiento", DbType.String, NumeroAsientoKey)
            db.AddInParameter(dbCommand, "@p_Secuencia", DbType.String, SecuenciaKey)
            db.AddInParameter(dbCommand, "@p_FechaAsiento", DbType.Decimal, fecha)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    ''' <summary>
    ''' Elimina un expediente de AsientoContable table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoMatriz(ByVal codigoMatriz As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("AsientoContable_EliminarPorCodigoMatriz")
            db.AddInParameter(dbCommand, "@p_CodigoMatriz", DbType.Decimal, codigoMatriz)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function GenerarAsientoContable(ByVal codigoPortafolio As String, ByVal fechaOperacion As Decimal, ByVal tipoLote As String, ByVal dataRequest As DataSet, Optional ByVal CodigoMercado As String = "") As ArrayList
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim num As Integer
        Dim NombreSP As String = ""
        Dim listaNumAsientos As New ArrayList
        Select Case tipoLote
            Case "CVI"
                NombreSP = "AsientosContable_Generar_Inversion"
            Case "VC"
                NombreSP = "AsientosContable_Generar_Valorizacion"
            Case "CCI"
                NombreSP = "AsientosContable_Generar_Tesoreria"
            Case "PP"
                NombreSP = "AsientosContable_Generar_Provision"
            Case ""
                NombreSP = "AsientosContable_Generar"
            Case "PI"
                NombreSP = "AsientosContable_Generar_Provision_Interes"
        End Select
        Using dbCommand As DbCommand = db.GetStoredProcCommand(NombreSP)
            dbCommand.CommandTimeout = 1020  'HDG 20110831
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_fechaOperacion", DbType.String, fechaOperacion)
            db.AddInParameter(dbCommand, "@p_usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            If tipoLote = "CCI" Then
                db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, CodigoMercado)
            End If
            If tipoLote.Equals("") Then
                db.AddOutParameter(dbCommand, "@p_numAsientos_I", DbType.Decimal, 4)
                db.AddOutParameter(dbCommand, "@p_numAsientos_T", DbType.Decimal, 4)
                db.AddOutParameter(dbCommand, "@p_numAsientos_V", DbType.Decimal, 4)
            Else
                db.AddOutParameter(dbCommand, "@p_numAsientos", DbType.Decimal, 4)
            End If
            db.ExecuteNonQuery(dbCommand)
            If tipoLote.Equals("") Then
                num = CType(db.GetParameterValue(dbCommand, "@p_numAsientos_I"), Integer)
                listaNumAsientos.Add(num)
                num = CType(db.GetParameterValue(dbCommand, "@p_numAsientos_T"), Integer)
                listaNumAsientos.Add(num)
                num = CType(db.GetParameterValue(dbCommand, "@p_numAsientos_V"), Integer)
            Else
                num = CType(db.GetParameterValue(dbCommand, "@p_numAsientos"), Integer)
            End If
            listaNumAsientos.Add(num)
            Return listaNumAsientos
        End Using
    End Function
    Public Sub GenerarAsientoContableSAFM(ByVal codigoPortafolio As String, CodigoSerie As String, CodigoMatriz As String, ByVal fechaOperacion As Decimal, _
    ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim num As Integer
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ins_AsientoComisionSAF")
            dbCommand.CommandTimeout = 1020
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_CodigoSerie", DbType.String, CodigoSerie)
            db.AddInParameter(dbCommand, "@p_CodigoMatrizContable", DbType.String, CodigoMatriz)
            db.AddInParameter(dbCommand, "@p_FechaAsiento", DbType.String, fechaOperacion)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@P_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Sub Extornar(ByVal codigoPortafolio As String, ByVal fechaProceso As Decimal, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("AsientoContable_Extornar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FehaProceso", DbType.Decimal, fechaProceso)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Function SeleccionarPorFiltro(ByVal fechaAsiento As Decimal, ByVal codigoPortafolioSBS As String) As AsientoContableBE
        Dim oAsientoContableBE As New AsientoContableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("AsientoContable_SeleccionarPorFiltro")
            db.AddInParameter(dbCommand, "@p_FechaAsiento", DbType.Decimal, fechaAsiento)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
            db.LoadDataSet(dbCommand, oAsientoContableBE, "AsientoContable")
            Return oAsientoContableBE
        End Using
    End Function
    Public Function SeleccionarPorFiltroRevision(ByVal fechaAsiento As Decimal, ByVal codigoPortafolioSBS As String, ByVal codigoMoneda As String, ByVal tipoLote As String, ByVal CodigoMercado As String) As AsientoContableBE
        Dim oAsientoContableBE As New AsientoContableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("AsientoContable_SeleccionarPorFiltroRevision")
            dbCommand.CommandTimeout = 1020
            db.AddInParameter(dbCommand, "@p_FechaAsiento", DbType.Decimal, fechaAsiento)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
            db.AddInParameter(dbCommand, "@p_TipoLote", DbType.String, tipoLote)
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, CodigoMercado)
            db.LoadDataSet(dbCommand, oAsientoContableBE, "AsientoContable")
            Return oAsientoContableBE
        End Using
    End Function
    Public Function RetornarTipoCambio(ByVal strMoneda As String, ByVal strFecha As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Retornar_TipoCambio")
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, strMoneda)
            db.AddInParameter(dbCommand, "@p_fechaValorizacion", DbType.Decimal, strFecha)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function RetornarTipoCambio_T1(ByVal strMoneda As String, ByVal Escenario As String, ByVal Fecha As Decimal) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Retornar_TipoCambioT_1")
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, strMoneda)
            db.AddInParameter(dbCommand, "@p_EntidadExt", DbType.String, Escenario)
            db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, Fecha)
            Return CDec(db.ExecuteScalar(dbCommand))
        End Using
    End Function
    Public Function AsientoContable_SeleccionarPorFiltroRevisionCabecera(ByVal fechaAsiento As Decimal, ByVal codigoPortafolioSBS As String, ByVal codigoMoneda As String) As AsientoContableBE
        Dim oAsientoContableBE As New AsientoContableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("AsientoContable_SeleccionarPorFiltroRevisionCabecera")
            db.AddInParameter(dbCommand, "@p_FechaAsiento", DbType.Decimal, fechaAsiento)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
            db.LoadDataSet(dbCommand, oAsientoContableBE, "AsientoContable")
            Return oAsientoContableBE
        End Using
    End Function
    Public Function Seleccionar_RistraContable(ByVal portafolio As String, ByVal fechaAperturaDesde As Decimal, ByVal fechaAperturaHasta As Decimal, ByVal fechaProceso As Decimal) As DataSet
        Dim dsConsulta As New DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Contabilidad_Seleccionar_RistraContable")
            dbCommand.CommandTimeout = 1020
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
            db.AddInParameter(dbCommand, "@p_FechaAperturaDesde", DbType.Decimal, fechaAperturaDesde)
            db.AddInParameter(dbCommand, "@p_FechaAperturaHasta", DbType.Decimal, fechaAperturaHasta)
            db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fechaProceso)
            db.LoadDataSet(dbCommand, dsConsulta, "RistraContable")
            Return dsConsulta
        End Using
    End Function
    Public Function AsientoContable_Interface(ByVal codigoPortafolio As String, ByVal fechaOperacion As Decimal, ByVal tipoLote As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_AsientoContable_Interface")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaOperacion)
            db.AddInParameter(dbCommand, "@p_Lote", DbType.String, tipoLote)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    'OT10783 - Método que verifica si existen diferencias de operaciones de compra y venta en los asientos contables
    Public Function AsientoContable_GenerarGananciaYperdida(ByVal p_CodigoPortafolio As String, ByVal p_FechaOperacion As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim proc As Boolean = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using DbCommand As DbCommand = db.GetStoredProcCommand("AsientosContables_GenerarGananciaYPerdida")
            db.AddInParameter(DbCommand, "@p_CodigoPortafolio", DbType.String, p_CodigoPortafolio)
            db.AddInParameter(DbCommand, "@p_FechaOperacion", DbType.Decimal, p_FechaOperacion)
            db.AddInParameter(DbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(DbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(DbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(DbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(DbCommand)
            proc = True
        End Using
        Return proc
    End Function
    'OT10783 - FIN
End Class