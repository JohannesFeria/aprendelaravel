Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class ArchivosVAXBMIDASDAM
    Private oArchivosVAXBMIDAS As ArchivosVAXBMIDASBE.ArchivosVAXBMIDASRow
    Public Function Insertar(ByVal objArchivosVAXBMIDAS As ArchivosVAXBMIDASBE.ArchivosVAXBMIDASRow, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ArchivosVAXBMIDAS_Insertar")
        oArchivosVAXBMIDAS = CType(objArchivosVAXBMIDAS, ArchivosVAXBMIDASBE.ArchivosVAXBMIDASRow)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, oArchivosVAXBMIDAS.CodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_NombreBanco", DbType.String, oArchivosVAXBMIDAS.NombreBanco)
        db.AddInParameter(dbCommand, "@p_FechaCarga", DbType.Decimal, oArchivosVAXBMIDAS.FechaCarga)
        If (oArchivosVAXBMIDAS.SaldoSoles = -1) Then
            db.AddInParameter(dbCommand, "@p_SaldoSoles", DbType.Decimal, System.DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_SaldoSoles", DbType.Decimal, oArchivosVAXBMIDAS.SaldoSoles)
        End If

        If (oArchivosVAXBMIDAS.SaldoDolares = -1) Then
            db.AddInParameter(dbCommand, "@p_SaldoDolares", DbType.Decimal, System.DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_SaldoDolares", DbType.Decimal, oArchivosVAXBMIDAS.SaldoDolares)
        End If
        If (oArchivosVAXBMIDAS.SaldoEuros = -1) Then
            db.AddInParameter(dbCommand, "@p_SaldoEuros", DbType.Decimal, System.DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_SaldoEuros", DbType.Decimal, oArchivosVAXBMIDAS.SaldoEuros)
        End If
        If (oArchivosVAXBMIDAS.SaldoYenes = -1) Then
            db.AddInParameter(dbCommand, "@p_SaldoYenes", DbType.Decimal, System.DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_SaldoYenes", DbType.Decimal, oArchivosVAXBMIDAS.SaldoYenes)
        End If
        If (oArchivosVAXBMIDAS.SaldoLibras = -1) Then
            db.AddInParameter(dbCommand, "@p_SaldoLibras", DbType.Decimal, System.DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_SaldoLibras", DbType.Decimal, oArchivosVAXBMIDAS.SaldoLibras)
        End If
        If (oArchivosVAXBMIDAS.SaldoDolCan = -1) Then
            db.AddInParameter(dbCommand, "@p_SaldoDolCan", DbType.Decimal, System.DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_SaldoDolCan", DbType.Decimal, oArchivosVAXBMIDAS.SaldoDolCan)
        End If
        If (oArchivosVAXBMIDAS.SaldoBRL = -1) Then
            db.AddInParameter(dbCommand, "@p_SaldoBRL", DbType.Decimal, System.DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_SaldoBRL", DbType.Decimal, oArchivosVAXBMIDAS.SaldoBRL)
        End If
        If (oArchivosVAXBMIDAS.SaldoMxN = -1) Then
            db.AddInParameter(dbCommand, "@p_SaldoMxN", DbType.Decimal, System.DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_SaldoMxN", DbType.Decimal, oArchivosVAXBMIDAS.SaldoMxN)
        End If
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        'RGF 20080625 nuevas columnas por cambio en formato de archivo a cargar SALSITmmaa.txt
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oArchivosVAXBMIDAS.CodigoMoneda)
        If (oArchivosVAXBMIDAS.Saldo = -1) Then
            db.AddInParameter(dbCommand, "@p_Saldo", DbType.Decimal, System.DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_Saldo", DbType.Decimal, oArchivosVAXBMIDAS.Saldo)
        End If

        db.ExecuteNonQuery(dbCommand)
    End Function

    Public Function SeleccionarPorPortafolioFecha(ByVal PortafolioSBS As String, ByVal fechaCarga As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ArchivosVAXBMIDAS_SeleccionarPorPortafolioFecha")

        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, PortafolioSBS)
        db.AddInParameter(dbCommand, "@p_FechaCarga", DbType.Decimal, fechaCarga)
        Dim oArchivosVAXBMIDAS As New DataSet
        db.LoadDataSet(dbCommand, oArchivosVAXBMIDAS, "ArchivosVAXBMIDAS")
        Return oArchivosVAXBMIDAS

    End Function

End Class
