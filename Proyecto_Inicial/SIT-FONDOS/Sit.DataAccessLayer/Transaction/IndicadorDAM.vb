Option Explicit On 
Option Strict Off

#Region "/* Imports */"

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

#End Region

Public Class IndicadorDAM

#Region "/* Constructor */"

    Public Sub New()

    End Sub

#End Region

#Region "/* Funciones No Transaccionales */"

    'Public Function SeleccionarTipoVac() As IndicadorBE

    '    Dim oIndicadorBE As New IndicadorBE
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim dbCommand As dbCommand = db.GetStoredProcCommand("Indicador_ListarVAC")

    '    db.LoadDataSet(dbCommand, oIndicadorBE, "Indicador")

    '    Return oIndicadorBE

    'End Function

    Public Function SeleccionarPorTipo(ByVal TipoIndicador As String) As DataSet

        Dim oIndicadorBE As New DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Indicador_Seleccionar_PorTipo")

        db.AddInParameter(dbCommand, "@p_TipoIndicador", DbType.String, TipoIndicador)

        db.LoadDataSet(dbCommand, oIndicadorBE, "Indicador")

        Return oIndicadorBE

    End Function
    Public Function SeleccionarPorFiltro(ByVal codigoIndicador As String, ByVal nombreIndicador As String, ByVal situacion As String, ByVal baseTasa As Decimal, ByVal diasPeriodo As Decimal, ByVal fechaVigencia As Decimal, ByVal manejarPeriodo As String, ByVal codigoPeriodicidad As String, ByVal codigoTipoCupon As String, Optional ByVal sTipoIndicador As String = "") As IndicadorBE

        Dim oIndicadorBE As New IndicadorBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Indicador_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoIndicador", DbType.String, codigoIndicador)
        db.AddInParameter(dbCommand, "@p_NombreIndicador", DbType.String, nombreIndicador)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_BaseTasa", DbType.Decimal, baseTasa)
        db.AddInParameter(dbCommand, "@p_DiasPeriodo", DbType.Decimal, diasPeriodo)
        db.AddInParameter(dbCommand, "@p_FechaVigencia", DbType.Decimal, fechaVigencia)
        db.AddInParameter(dbCommand, "@p_ManejaPeriodo", DbType.String, manejarPeriodo)
        db.AddInParameter(dbCommand, "@p_CodigoPeriodicidad", DbType.String, codigoPeriodicidad)
        db.AddInParameter(dbCommand, "@p_CodigoTipoCupon", DbType.String, codigoTipoCupon)
        db.AddInParameter(dbCommand, "@TipoIndicador", DbType.String, sTipoIndicador)

        db.LoadDataSet(dbCommand, oIndicadorBE, "Indicador")

        Return oIndicadorBE

    End Function

    Public Function Seleccionar(ByVal codigoIndicador As String) As IndicadorBE

        Dim oIndicadorBE As New IndicadorBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Indicador_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoIndicador", DbType.String, codigoIndicador)

        db.LoadDataSet(dbCommand, oIndicadorBE, "Indicador")

        Return oIndicadorBE

    End Function

    Public Function Listar() As IndicadorBE

        Dim oIndicadorBE As New IndicadorBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Indicador_Listar")

        db.LoadDataSet(dbCommand, oIndicadorBE, "Indicador")

        Return oIndicadorBE

    End Function

    Public Function SeleccionarValorIndicador(ByVal codigoIndicador As String, ByVal fecha As Decimal) As DataSet

        Dim oDS As New DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Indicador_SeleccionarValorIndicador")

        db.AddInParameter(dbCommand, "@p_CodigoIndicador", DbType.String, codigoIndicador)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.String, fecha)

        db.LoadDataSet(dbCommand, oDS, "Indicador")

        Return oDS

    End Function


    Public Function VerificaPreCarga(ByVal sCodigoIndicador As String, ByVal nFechaInformacion As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ValorIndicadorVerifica_Listar")

        db.AddInParameter(dbCommand, "@CodigoIndicador", DbType.String, sCodigoIndicador)
        db.AddInParameter(dbCommand, "@FechaInformacion", DbType.Decimal, nFechaInformacion)

        Return db.ExecuteDataSet(dbCommand)
    End Function


    Public Function EliminarIndicadorCuponCero(ByVal sCodigoIndicador As String, ByVal nFechaInformacion As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("IndicadorCuponCero_Eliminar")

        db.AddInParameter(dbCommand, "@CodigoIndicador", DbType.String, sCodigoIndicador)
        db.AddInParameter(dbCommand, "@FechaInformacion", DbType.Decimal, nFechaInformacion)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function InsertarIndicadorCuponCero(ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As String
        Dim CodigoCaracteristicas As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sFechaInformacion As String
        Dim nFechaInformacion As Decimal
        Dim sCodigoIndicador As String
        Dim sDiasPeriodo As Decimal
        Dim nDiasPeriodo As Decimal
        Dim sValor As String
        Dim nValor As Decimal

        For Each filaLinea As DataRow In dtDetalle.Rows
            Dim dbCommand As dbCommand = db.GetStoredProcCommand("ValorIndicador_Insertar")

            sFechaInformacion = filaLinea(0).ToString().Trim()

            If sFechaInformacion.Length >= 10 Then 'RGF 20081222 Salia error en las filas en blanco
                nFechaInformacion = System.Convert.ToDecimal(sFechaInformacion.Substring(6, 4) & sFechaInformacion.Substring(3, 2) & sFechaInformacion.Substring(0, 2))

                sCodigoIndicador = filaLinea(1).ToString().Trim()
                sDiasPeriodo = filaLinea(2).ToString().Trim()
                nDiasPeriodo = Convert.ToDecimal(sDiasPeriodo)
                sValor = filaLinea(3).ToString().Trim()
                If sValor = "" Then
                    nValor = 0
                Else
                    nValor = Convert.ToDecimal(sValor)
                End If

                db.AddInParameter(dbCommand, "@CodigoIndicador", DbType.String, sCodigoIndicador)
                db.AddInParameter(dbCommand, "@FechaInformacion", DbType.Decimal, nFechaInformacion)
                db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, "")
                db.AddInParameter(dbCommand, "@DiasPeriodo", DbType.Decimal, nDiasPeriodo)
                db.AddInParameter(dbCommand, "@Valor", DbType.Decimal, nValor)
                db.AddInParameter(dbCommand, "@UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                db.AddInParameter(dbCommand, "@FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

                db.ExecuteNonQuery(dbCommand)
            End If

        Next
        Return CodigoCaracteristicas

    End Function


    'montos negociados BVL

    Public Function EliminarMontoNegociadoBVL(ByVal nFechaInformacion As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MontoNegociadoBVL_EliminarCarga")

        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, nFechaInformacion)

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function


    Public Function InsertarMontoNegociadoBVL(ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As String
        Dim CodigoCaracteristicas As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sFechaOperacion As String
        Dim nFechaOperacion As Decimal
        Dim sHoraOperacion As String
        Dim nNumeroOperacion As Decimal
        Dim sCodigoMnemonico As String
        Dim nPrecio As Decimal
        Dim nCantidad As Decimal
        Dim sComprador As String
        Dim sVendedor As String
        Dim nMontoEfectivo As Decimal

        For Each filaLinea As DataRow In dtDetalle.Rows

            Dim dbCommand As dbCommand = db.GetStoredProcCommand("MontoNegociadoBVL_Insertar")

            If (Not (filaLinea(0) Is DBNull.Value) And filaLinea(0).ToString().Length > 0) Then
                sFechaOperacion = filaLinea(0).ToString().Trim()
                nFechaOperacion = System.Convert.ToDecimal(sFechaOperacion.Substring(6, 4) & sFechaOperacion.Substring(3, 2) & sFechaOperacion.Substring(0, 2))

                sHoraOperacion = filaLinea(1).ToString().Trim() '"30/12/1899 08:38:02 a.m."
                sHoraOperacion = sHoraOperacion.Substring(11, 8)
                nNumeroOperacion = CType(filaLinea(2).ToString().Trim(), Decimal)
                sCodigoMnemonico = filaLinea(3).ToString().Trim()
                nPrecio = CType(filaLinea(4).ToString().Trim(), Decimal)
                nCantidad = CType(filaLinea(5).ToString().Trim(), Decimal)
                sComprador = filaLinea(6).ToString().Trim()
                sVendedor = filaLinea(7).ToString().Trim()
                nMontoEfectivo = CType(filaLinea(8).ToString().Trim(), Decimal)

                db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, nFechaOperacion)
                db.AddInParameter(dbCommand, "@p_HoraOperacion", DbType.String, sHoraOperacion)
                db.AddInParameter(dbCommand, "@p_NumeroOperacion", DbType.Decimal, nNumeroOperacion)
                db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, sCodigoMnemonico)
                db.AddInParameter(dbCommand, "@p_Precio", DbType.Decimal, nPrecio)
                db.AddInParameter(dbCommand, "@p_Cantidad", DbType.Decimal, nCantidad)
                db.AddInParameter(dbCommand, "@p_Comprador", DbType.String, sComprador)
                db.AddInParameter(dbCommand, "@p_Vendedor", DbType.String, sVendedor)
                db.AddInParameter(dbCommand, "@p_MontoEfectivo", DbType.Decimal, nMontoEfectivo)
                db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, "A")

                db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

                db.ExecuteNonQuery(dbCommand)
            End If

        Next
        Return CodigoCaracteristicas

    End Function


    Public Function VerificaPreCargaMontoNegociadoBVL(ByVal nFechaInformacion As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MontoNegociadoBVL_Listar")

        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, nFechaInformacion)

        Return db.ExecuteDataSet(dbCommand)
    End Function

#End Region

#Region "/* Funciones Transaccionales */"

    Public Function Insertar(ByVal oIndicadorBE As IndicadorBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Indicador_Insertar")
        Dim oRow As IndicadorBE.IndicadorRow

        oRow = DirectCast(oIndicadorBE.Indicador.Rows(0), IndicadorBE.IndicadorRow)

        db.AddInParameter(dbCommand, "@p_CodigoIndicador", DbType.String, oRow.CodigoIndicador)
        db.AddInParameter(dbCommand, "@p_NombreIndicador", DbType.String, oRow.NombreIndicador)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_BaseTasa", DbType.String, oRow.BaseTasa)
        db.AddInParameter(dbCommand, "@p_DiasPeriodo", DbType.String, oRow.DiasPeriodo)
        db.AddInParameter(dbCommand, "@p_FechaVigencia", DbType.String, Convert.DBNull)
        db.AddInParameter(dbCommand, "@p_ManejaPeriodo", DbType.String, oRow.ManejaPeriodo)
        db.AddInParameter(dbCommand, "@p_CodigoPeriodicidad", DbType.String, Convert.DBNull)
        db.AddInParameter(dbCommand, "@p_CodigoTipoCupon", DbType.String, Convert.DBNull)
        db.AddInParameter(dbCommand, "@p_TipoIndicador", DbType.String, oRow.TipoIndicador)
        db.AddInParameter(dbCommand, "@p_MostrarLibor", DbType.String, oRow.MostrarLibor)
        db.AddInParameter(dbCommand, "@p_FuenteLibor", DbType.String, IIf(oRow.FuenteLibor = String.Empty, DBNull.Value, oRow.FuenteLibor))
        db.AddInParameter(dbCommand, "@p_MesLibor", DbType.Int32, IIf(oRow.MesLibor = 0, DBNull.Value, oRow.MesLibor))
        'db.AddInParameter(dbCommand, "@p_FechaVigencia", DbType.String, oRow.FechaVigencia)
        'db.AddInParameter(dbCommand, "@p_ManejaPeriodo", DbType.String, oRow.ManejaPeriodo)
        'db.AddInParameter(dbCommand, "@p_CodigoPeriodicidad", DbType.String, oRow.CodigoPeriodicidad)
        'db.AddInParameter(dbCommand, "@p_CodigoTipoCupon", DbType.String, oRow.CodigoTipoCupon)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function


    Public Function Modificar(ByVal oIndicadorBE As IndicadorBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Indicador_Modificar")
        Dim oRow As IndicadorBE.IndicadorRow

        oRow = DirectCast(oIndicadorBE.Indicador.Rows(0), IndicadorBE.IndicadorRow)

        db.AddInParameter(dbCommand, "@p_CodigoIndicador", DbType.String, oRow.CodigoIndicador)
        db.AddInParameter(dbCommand, "@p_NombreIndicador", DbType.String, oRow.NombreIndicador)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_BaseTasa", DbType.String, oRow.BaseTasa)
        db.AddInParameter(dbCommand, "@p_DiasPeriodo", DbType.String, oRow.DiasPeriodo)
        db.AddInParameter(dbCommand, "@p_FechaVigencia", DbType.String, Convert.DBNull)
        db.AddInParameter(dbCommand, "@p_ManejaPeriodo", DbType.String, oRow.ManejaPeriodo)
        db.AddInParameter(dbCommand, "@p_CodigoPeriodicidad", DbType.String, Convert.DBNull)
        db.AddInParameter(dbCommand, "@p_CodigoTipoCupon", DbType.String, Convert.DBNull)
        db.AddInParameter(dbCommand, "@p_TipoIndicador", DbType.String, oRow.TipoIndicador)
        'db.AddInParameter(dbCommand, "@p_FechaVigencia", DbType.String, oRow.FechaVigencia)
        'db.AddInParameter(dbCommand, "@p_ManejaPeriodo", DbType.String, oRow.ManejaPeriodo)
        'db.AddInParameter(dbCommand, "@p_CodigoPeriodicidad", DbType.String, oRow.CodigoPeriodicidad)
        'db.AddInParameter(dbCommand, "@p_CodigoTipoCupon", DbType.String, oRow.CodigoTipoCupon)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

    Public Function Eliminar(ByVal codigoIndicador As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Indicador_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoIndicador", DbType.String, codigoIndicador)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.String, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

#End Region


    '---------------------------------Libor Fecha  de la Tabla Cuponera-------------------
    'LETV 20090702
    Public Function SeleccionarCuponesLibor(ByVal codigoNemonico As String, ByVal fechaInicio As Decimal, ByVal fechaTermino As Decimal) As DataTable
        Dim oDS As New DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Libor_SeleccionarPorFiltro")
        db.AddInParameter(dbCommand, "@p_codigonemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_fechainicio", DbType.Decimal, fechaInicio)
        db.AddInParameter(dbCommand, "@p_fechatermino", DbType.Decimal, fechaTermino)
        db.LoadDataSet(dbCommand, oDS, "Indicador")
        Return oDS.Tables(0)
    End Function

    Public Function ModificarCuponesLibor(ByVal CodigoNemonico As String, ByVal Secuencia As String, ByVal FechaInicio As Decimal, ByVal FechaLibor As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Libor_ModificarCupon")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, CodigoNemonico)
        db.AddInParameter(dbCommand, "@p_Secuencia", DbType.String, Secuencia)
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, FechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaLibor", DbType.Decimal, FechaLibor)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    '-->> Indicadores para negociación SWAP

    Public Function Indicador_SeleccionarSWAP(ByVal codigoIndicador As String) As DataSet
        Dim oIndicadorBE As New IndicadorBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Indicador_SeleccionarSWAP")
            db.AddInParameter(dbCommand, "@p_CodigoIndicador", DbType.String, codigoIndicador)
            Dim ds As DataSet = db.ExecuteDataSet(dbCommand)
            Return ds
        End Using
    End Function

    Public Function Indicador_SeleccionarIndicadorLibor(ByVal codigoIndicador As String, ByVal fuente As String) As DataSet
        Dim oIndicadorBE As New IndicadorBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Indicador_SeleccionarIndicadorLibor")
            db.AddInParameter(dbCommand, "@p_CodigoIndicador", DbType.String, codigoIndicador)
            db.AddInParameter(dbCommand, "@p_Fuente", DbType.String, fuente)
            Dim ds As DataSet = db.ExecuteDataSet(dbCommand)
            Return ds
        End Using
    End Function

    Public Function Indicador_SeleccionarValorLibor(ByVal codigoIndicador As String, ByVal fechaReferencia As Decimal) As Decimal
        Dim oIndicadorBE As New IndicadorBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Indicador_SeleccionarValorLibor")
            db.AddInParameter(dbCommand, "@p_codigoIndicador", DbType.String, codigoIndicador)
            db.AddInParameter(dbCommand, "@p_fechaReferencia", DbType.Decimal, fechaReferencia)
            Return CType(db.ExecuteScalar(dbCommand), Decimal)
        End Using
    End Function
End Class
