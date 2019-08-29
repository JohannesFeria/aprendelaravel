Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class EntidadExternaArchivoPlanoDAM

    Public Function Seleccionar(ByVal Valor As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("EntidadExternaArchivoPlano_Seleccionar")

        db.AddInParameter(dbCommand, "@ParametroValor", DbType.String, Valor)

        Return db.ExecuteDataSet(dbCommand)

    End Function

    Public Function Listar(ByVal sEntidadExterna As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DividendosRebatesLiberadasCarga_Listar")

        db.AddInParameter(dbCommand, "@EntidadExterna", DbType.String, sEntidadExterna)

        Return db.ExecuteDataSet(dbCommand)

    End Function

    Public Function CargarArchivo(ByVal sEntidadExterna As String, ByVal sEntidadArchivo As String, ByVal sFechaInformacion As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "EntidadExternaCargaTemporal_Carga"
        Dim dbCommand As dbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbCommand, "@EntidadExterna", DbType.String, sEntidadExterna)
        db.AddInParameter(dbCommand, "@CadenaArchivo", DbType.String, sEntidadArchivo)
        db.AddInParameter(dbCommand, "@FechaInformacion", DbType.String, sFechaInformacion)
        db.AddInParameter(dbCommand, "@UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))


        db.ExecuteNonQuery(dbCommand)

    End Function

    Public Function Separador() As String
        Return System.Globalization.NumberFormatInfo.CurrentInfo.PercentDecimalSeparator
    End Function

    Public Function InsertarDivLibBloomberg(ByVal Fecha As Date, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As String
        Dim CodigoCaracteristicas As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim cont As Integer
        Dim CodigoSBS, Descripcion, CodigoMnemonico, Moneda, Factor, Tipo As String
        Dim FechaA, FechaC, FechaE, FechaR As Date
        Dim FechaA2, FechaC2, FechaE2, FechaR2 As Decimal
        Dim DecFactor As Decimal

        cont = 0
        For Each filaLinea As DataRow In dtDetalle.Rows
            Dim dbCommand As dbCommand = db.GetStoredProcCommand("DivLibBloombergTemporal_Insertar")
            If cont <> 0 Then

                If (filaLinea(0).ToString().Trim() <> "") And (filaLinea(9).ToString().Trim() <> "") And ((filaLinea(9).ToString().ToUpper.Trim() = "INCOME" Or (filaLinea(9).ToString().ToUpper.Trim = "SPLIT"))) Then

                    CodigoSBS = filaLinea(0).ToString().Trim()
                    CodigoMnemonico = filaLinea(1).ToString().Trim()

                    If filaLinea(2).ToString().Trim() = "" Then
                        Descripcion = ""
                    Else
                        Descripcion = filaLinea(2).ToString().Trim()
                    End If

                    If ("" & filaLinea(3)) = "" Or ("" & filaLinea(3)) = "#N/A N Ap" Then
                        FechaA2 = 0
                    Else
                        FechaA = filaLinea(3)
                        'FechaA2 = System.Convert.ToDecimal(FechaA)
                        FechaA2 = CType(FechaA.ToString("yyyyMMdd"), Decimal)
                    End If

                    'FechaC = filaLinea(4)

                    If ("" & filaLinea(4)) = "" Or ("" & filaLinea(4)) = "#N/A N Ap" Then
                        FechaC2 = 0
                    Else
                        FechaC = filaLinea(3)
                        FechaC2 = CType(FechaC.ToString("yyyyMMdd"), Decimal)
                    End If

                    'FechaR = filaLinea(5)

                    If ("" & filaLinea(5)) = "" Or ("" & filaLinea(5)) = "#N/A N Ap" Then
                        FechaR2 = 0
                    Else
                        FechaR = filaLinea(5)
                        FechaR2 = CType(FechaR.ToString("yyyyMMdd"), Decimal)
                    End If

                    'FechaE = filaLinea(6)

                    If ("" & filaLinea(6)) = "" Or ("" & filaLinea(6)) = "#N/A N Ap" Then
                        FechaE2 = 0
                    Else
                        FechaE = filaLinea(6)
                        FechaE2 = CType(FechaE.ToString("yyyyMMdd"), Decimal)
                    End If

                    'Moneda = filaLinea(7).ToString().Trim()

                    If filaLinea(7).ToString().Trim() = "" Or ("" & filaLinea(7)) = "#N/A N Ap" Then
                        Moneda = ""
                    Else
                        Moneda = filaLinea(7).ToString().Trim()
                    End If

                    'Factor = filaLinea(8).ToString().Trim()

                    If filaLinea(8).ToString().Trim() = "" Or ("" & filaLinea(8)) = "#N/A N Ap" Then
                        Factor = ""
                    Else
                        Factor = filaLinea(8).ToString().Trim()
                    End If

                    'Tipo = filaLinea(9).ToString().Trim()

                    If filaLinea(9).ToString().Trim() = "" Or ("" & filaLinea(9)) = "#N/A N Ap" Then
                        Tipo = ""
                    Else
                        Tipo = filaLinea(9).ToString().Trim()
                    End If

                    '                If (Factor <> "") Then
                    If Factor = "" Or Factor = "#N/A N Ap" Then
                        DecFactor = 0
                    Else
                        DecFactor = Convert.ToDecimal(Factor.Replace(".", Separador))
                    End If

                    db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(Fecha))
                    db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, CodigoSBS)

                    db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, CodigoMnemonico)
                    db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, Descripcion)

                    'db.AddInParameter(dbCommand, "@p_FechaAcuerdo", DbType.Decimal, DataUtility.ObtenerFecha(FechaA2))
                    'db.AddInParameter(dbCommand, "@p_FechaCorte", DbType.Decimal, DataUtility.ObtenerFecha(FechaC2))
                    'db.AddInParameter(dbCommand, "@p_FechaRegistro", DbType.Decimal, DataUtility.ObtenerFecha(FechaR2))
                    'db.AddInParameter(dbCommand, "@p_FechaEntrega", DbType.Decimal, DataUtility.ObtenerFecha(FechaE2))

                    db.AddInParameter(dbCommand, "@p_FechaAcuerdo", DbType.Decimal, FechaA2)
                    db.AddInParameter(dbCommand, "@p_FechaCorte", DbType.Decimal, FechaC2)
                    db.AddInParameter(dbCommand, "@p_FechaRegistro", DbType.Decimal, FechaR2)
                    db.AddInParameter(dbCommand, "@p_FechaEntrega", DbType.Decimal, FechaE2)

                    db.AddInParameter(dbCommand, "@p_Moneda", DbType.String, Moneda)
                    db.AddInParameter(dbCommand, "@p_Factor", DbType.Decimal, DecFactor)
                    db.AddInParameter(dbCommand, "@p_Tipo", DbType.String, Tipo)

                    db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                    db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                    db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                    db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

                    db.ExecuteNonQuery(dbCommand)

                End If
            End If

            cont += 1

        Next
        Return CodigoCaracteristicas

    End Function

    Public Function ImportarArchivo(ByVal sEntidadExterna As String, ByVal nFechaInformacion As Long, ByVal sFlagBorrado As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("EntidadExternaInformacion_Importar")

        db.AddInParameter(dbCommand, "@EntidadExterna", DbType.String, sEntidadExterna)
        db.AddInParameter(dbCommand, "@FechaInformacion", DbType.Decimal, nFechaInformacion)
        db.AddInParameter(dbCommand, "@FlagBorrado", DbType.Decimal, sFlagBorrado)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function VerificaPreCarga(ByVal sEntidadExterna As String, ByVal nFechaInformacion As Long) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("EntidadExternaCargaTemporal_VerificaPreCarga")

        db.AddInParameter(dbCommand, "@EntidadExterna", DbType.String, sEntidadExterna)
        db.AddInParameter(dbCommand, "@FechaInformacion", DbType.Decimal, nFechaInformacion)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function Eliminar(ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("EntidadExternaCargaTemporal_Eliminar")
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

End Class
