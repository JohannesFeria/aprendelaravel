Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class GapsDAM

    Dim DECIMAL_NULO As Decimal = 0.0
    Private oGapsRow As GapsBE.GapsRow

    Public Sub InicializarGaps(ByRef oRow As GapsBE.GapsRow)

        oRow.CodigoSBS = ""
        oRow.Emisor = ""
        oRow.Codigo = ""
        oRow.HO1 = DECIMAL_NULO
        oRow.HO2 = DECIMAL_NULO
        oRow.HO3 = DECIMAL_NULO
        oRow.IN1 = DECIMAL_NULO
        oRow.IN2 = DECIMAL_NULO
        oRow.IN3 = DECIMAL_NULO
        oRow.PRI1 = DECIMAL_NULO
        oRow.PRI2 = DECIMAL_NULO
        oRow.PRI3 = DECIMAL_NULO
        oRow.PF1 = DECIMAL_NULO
        oRow.PF2 = DECIMAL_NULO
        oRow.PF3 = DECIMAL_NULO
        oRow.CodigoEmisor = ""

    End Sub

    'Public Function Insertar(ByVal objGaps As GapsBE, ByVal dataRequest As DataSet) As String
    '    Dim strCodigo As String
    '    Dim db As Database = DatabaseFactory.CreateDatabase()

    '    Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gaps_Insertar")
    '    oGapsRow = CType(objGaps.Gaps.Rows(0), GapsBE.GapsRow)

    '    db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, oGapsRow.CodigoSBS)
    '    db.AddInParameter(dbCommand, "@p_Emisor", DbType.String, oGapsRow.Emisor)
    '    db.AddInParameter(dbCommand, "@p_Codigo", DbType.String, oGapsRow.Codigo)
    '    db.AddInParameter(dbCommand, "@p_HO1", DbType.Decimal, oGapsRow.HO1)
    '    db.AddInParameter(dbCommand, "@p_HO2", DbType.Decimal, oGapsRow.HO2)
    '    db.AddInParameter(dbCommand, "@p_HO3", DbType.Decimal, oGapsRow.HO3)
    '    db.AddInParameter(dbCommand, "@p_IN1", DbType.Decimal, oGapsRow.IN1)
    '    db.AddInParameter(dbCommand, "@p_IN2", DbType.Decimal, oGapsRow.IN2)
    '    db.AddInParameter(dbCommand, "@p_IN3", DbType.Decimal, oGapsRow.IN3)
    '    db.AddInParameter(dbCommand, "@p_PRI1", DbType.Decimal, oGapsRow.PRI1)
    '    db.AddInParameter(dbCommand, "@p_PRI2", DbType.Decimal, oGapsRow.PRI2)
    '    db.AddInParameter(dbCommand, "@p_PRI3", DbType.Decimal, oGapsRow.PRI3)
    '    db.AddInParameter(dbCommand, "@p_PF1", DbType.Decimal, oGapsRow.PF1)
    '    db.AddInParameter(dbCommand, "@p_PF2", DbType.Decimal, oGapsRow.PF2)
    '    db.AddInParameter(dbCommand, "@p_PF3", DbType.Decimal, oGapsRow.PF3)
    '    db.AddInParameter(dbCommand, "@p_CodigoEmisor", DbType.String, oGapsRow.CodigoEmisor)

    '    strCodigo = db.ExecuteScalar(dbCommand)
    '    Return strCodigo

    'End Function

    Public Function InsertarExcel(ByVal dtDetalle As DataTable, ByVal Fecha As Integer, ByVal dataRequest As DataSet) As String
        Dim strCodigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim CodigoSBS, Emisor, Codigo, CodigoEmisor As String
        Dim HO1, HO2, HO3, IN1, IN2, IN3, PRI1, PRI2, PRI3, PF1, PF2, PF3 As Decimal
        Dim i, filaInicioExcel As Integer
        filaInicioExcel = 1
        i = 0

        'Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gaps_CrearTabla")
        'db.ExecuteNonQuery(dbCommand)

        For Each filaLinea As DataRow In dtDetalle.Rows
            If i >= filaInicioExcel Then

                Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gaps_Insertar")

                CodigoSBS = filaLinea(0).ToString().Trim()
                Emisor = filaLinea(1).ToString().Trim()
                Codigo = filaLinea(2).ToString().Trim()
                HO1 = filaLinea(3).ToString().Trim()
                HO2 = filaLinea(4).ToString().Trim()
                HO3 = filaLinea(5).ToString().Trim()
                IN1 = filaLinea(6).ToString().Trim()
                IN2 = filaLinea(7).ToString().Trim()
                IN3 = filaLinea(8).ToString().Trim()
                PRI1 = filaLinea(9).ToString().Trim()
                PRI2 = filaLinea(10).ToString().Trim()
                PRI3 = filaLinea(11).ToString().Trim()
                PF1 = filaLinea(12).ToString().Trim()
                PF2 = filaLinea(13).ToString().Trim()
                PF3 = filaLinea(14).ToString().Trim()
                CodigoEmisor = filaLinea(15).ToString().Trim()

                db.AddInParameter(dbCommand, "@CodigoSBS", DbType.String, CodigoSBS)
                db.AddInParameter(dbCommand, "@Emisor", DbType.String, Emisor)
                db.AddInParameter(dbCommand, "@Codigo", DbType.String, Codigo)
                db.AddInParameter(dbCommand, "@HO1", DbType.Decimal, HO1)
                db.AddInParameter(dbCommand, "@HO2", DbType.Decimal, HO2)
                db.AddInParameter(dbCommand, "@HO3", DbType.Decimal, HO3)
                db.AddInParameter(dbCommand, "@IN1", DbType.Decimal, IN1)
                db.AddInParameter(dbCommand, "@IN2", DbType.Decimal, IN2)
                db.AddInParameter(dbCommand, "@IN3", DbType.Decimal, IN3)
                db.AddInParameter(dbCommand, "@PRI1", DbType.Decimal, PRI1)
                db.AddInParameter(dbCommand, "@PRI2", DbType.Decimal, PRI2)
                db.AddInParameter(dbCommand, "@PRI3", DbType.Decimal, PRI3)
                db.AddInParameter(dbCommand, "@PF1", DbType.Decimal, PF1)
                db.AddInParameter(dbCommand, "@PF2", DbType.Decimal, PF2)
                db.AddInParameter(dbCommand, "@PF3", DbType.Decimal, PF3)
                db.AddInParameter(dbCommand, "@CodigoEmisor", DbType.String, CodigoEmisor)
                db.AddInParameter(dbCommand, "@FechaCarga", DbType.String, Fecha)

                db.ExecuteNonQuery(dbCommand)
            End If
            i = i + 1

        Next

        Return strCodigo

    End Function

    'Public Function EliminarTabla(ByVal dataRequest As DataSet) As String
    '    Dim strCodigo As String = String.Empty
    '    Dim db As Database = DatabaseFactory.CreateDatabase()

    '    Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gaps_EliminarTabla")
    '    db.ExecuteNonQuery(dbCommand)

    '    Return strCodigo

    'End Function

    Public Function InsertarExcelFondo(ByVal dtDetalle As DataTable, ByVal TipoFondo As String, ByVal fecha As Integer, ByVal dataRequest As DataSet) As String
        Dim strCodigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim HO, [IN], PRI, PF As Decimal
        Dim i, filaInicioExcel As Integer

        filaInicioExcel = 18
        i = 0

        For Each filaLinea As DataRow In dtDetalle.Rows
            If i >= filaInicioExcel Then

                Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gaps_InsertarFondo")

                If filaLinea(0).ToString().Trim() = "TOTAL" Then
                    HO = filaLinea(1).ToString().Trim()
                    [IN] = filaLinea(3).ToString().Trim()
                    PRI = filaLinea(5).ToString().Trim()
                    PF = filaLinea(7).ToString().Trim()

                    db.AddInParameter(dbCommand, "@Fondo", DbType.String, TipoFondo)
                    db.AddInParameter(dbCommand, "@HO", DbType.Decimal, HO)
                    db.AddInParameter(dbCommand, "@IN", DbType.Decimal, [IN])
                    db.AddInParameter(dbCommand, "@PRI", DbType.Decimal, PRI)
                    db.AddInParameter(dbCommand, "@PF", DbType.Decimal, PF)
                    db.AddInParameter(dbCommand, "@FechaCarga", DbType.Int32, fecha)

                    db.ExecuteNonQuery(dbCommand)
                End If

            End If
            i = i + 1

        Next

        Return strCodigo

    End Function

    Public Function ConsultaCantidadMoneda(ByVal CodigoMoneda As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ConsultaCantidadMoneda")

        db.AddInParameter(dbCommand, "@CodigoMoneda", DbType.String, CodigoMoneda)

        Return db.ExecuteDataSet(dbCommand)

    End Function

    Public Function ReporteGapsResumen0rdenado(ByVal CodigoMoneda As String, ByVal CodigoTipoInstrumentoSBS As String, ByVal CodigoPais As String, ByVal fecha As Integer) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Reporte_Gaps_Resumen_0rdenado")
        dbCommand.CommandTimeout = 300
        db.AddInParameter(dbCommand, "@CodigoMoneda", DbType.String, CodigoMoneda)
        db.AddInParameter(dbCommand, "@CodigoTipoInstrumentoSBS", DbType.String, CodigoTipoInstrumentoSBS)
        db.AddInParameter(dbCommand, "@codigoPais", DbType.String, CodigoPais)
        db.AddInParameter(dbCommand, "@fecha", DbType.Int32, fecha)

        Return db.ExecuteDataSet(dbCommand)

    End Function


End Class
