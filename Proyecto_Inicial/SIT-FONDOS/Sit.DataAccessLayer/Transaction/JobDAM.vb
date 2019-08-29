'Creado por: HDG OT 61566 Nro3-C 20101104
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports Microsoft.Practices.EnterpriseLibrary.Common

Public Class JobDAM
    'Private dbBase As SqlDatabase
    'Private dbCommand As DbCommand
    Private sqlCommand As String = ""

    Public Sub New()

    End Sub

    Public Function EjecutarJob(ByVal sNombreDTs As String, _
                                ByVal sDescripcionJob As String, _
                                ByVal sVariables As String, _
                                ByVal sParametros As String, _
                                ByVal sCorreoUsuario As String, _
                                ByVal sCorreoCopia As String, _
                                ByVal sServidorETL As String) As String

        'Dim cnxDB As Database
        Dim sEjecutarJob As String
        Dim sNombreJob As String
        Dim sTmpResultadoEjecucion As String

        'sNombreJob = "JOB_" + sNombreDTs
        sNombreJob = sNombreDTs 'RGF 20110315 OT 62087 REQ6

        ' sParametros = sNombreJob + "," + sParametros
        sTmpResultadoEjecucion = ""
        sServidorETL = """" & sServidorETL & """"

        If ExisteJob(sNombreJob, sServidorETL) = 1 Then
            sTmpResultadoEjecucion = "El Proceso se encuentra en ejecución (" + sNombreDTs + ")"

        Else
            sParametros = CadenaEjecucionDTS(sVariables, sParametros, sNombreDTs)
            ' de ahi pasar el correo usando el usuario

            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_JobLanzaDTS")
            dbCommand.CommandTimeout = 1020  'HDG 20110905
            db.AddInParameter(dbCommand, "@vc_NombreJob", DbType.String, sNombreJob)
            db.AddInParameter(dbCommand, "@vc_DescripcionJob", DbType.String, sDescripcionJob)
            db.AddInParameter(dbCommand, "@vc_TramaEjecucionSQL", DbType.String, sParametros)
            db.AddInParameter(dbCommand, "@vc_LoginUsuario", DbType.String, sCorreoUsuario)
            db.AddInParameter(dbCommand, "@vc_CorreoCopia", DbType.String, sCorreoCopia)
            db.AddInParameter(dbCommand, "@vc_ServidorETL", DbType.String, sServidorETL)
            sEjecutarJob = db.ExecuteScalar(dbCommand)

            If sEjecutarJob <> "" Then
                sTmpResultadoEjecucion = "El Proceso se envió para su ejecución (" + sNombreDTs + ")"
            Else
                sTmpResultadoEjecucion = "El Proceso no fue enviado por un error (" + sNombreDTs + ")"
            End If
        End If
        'cnxDB = Nothing
        Return sTmpResultadoEjecucion
    End Function

    Private Function ExisteJob(ByVal pivNombreDTs As String, ByVal sServidorETL As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_JobVerificaExiste")
        db.AddInParameter(dbCommand, "@vc_NombreJOB", DbType.String, pivNombreDTs)
        db.AddInParameter(dbCommand, "@vc_ServidorETL", DbType.String, sServidorETL)
        Return db.ExecuteScalar(dbCommand)
    End Function

    Private Function CadenaEjecucionDTS(ByVal pivVariablesDTS As String, ByVal pivParametrosDTS As String, ByVal pivNombreDTs As String) As String
        Dim bFlg As Boolean
        Dim nPosVariableDTS As Integer
        Dim nPosParametroDTS As Integer
        Dim sParametroDTS As String
        Dim sComilla As String
        Dim sTmpNomVariable As String
        Dim sTmpNomParametro As String

        sComilla = Chr(34)
        bFlg = True
        sParametroDTS = ""
        sTmpNomVariable = ""
        sTmpNomParametro = ""

        While bFlg
            nPosVariableDTS = InStr(pivVariablesDTS, ",")
            nPosParametroDTS = InStr(pivParametrosDTS, ",")

            If Len(pivVariablesDTS) > 0 Then
                If nPosVariableDTS = 0 Then
                    sTmpNomVariable = pivVariablesDTS
                Else
                    sTmpNomVariable = Mid(pivVariablesDTS, 1, nPosVariableDTS - 1)
                End If

                pivVariablesDTS = Right(pivVariablesDTS, Len(pivVariablesDTS) - nPosVariableDTS)

                If Len(pivParametrosDTS) > 0 Then
                    If nPosParametroDTS = 0 Then
                        sTmpNomParametro = pivParametrosDTS
                    Else
                        sTmpNomParametro = Mid(pivParametrosDTS, 1, nPosParametroDTS - 1)
                    End If
                    pivParametrosDTS = Right(pivParametrosDTS, Len(pivParametrosDTS) - nPosParametroDTS)
                Else
                    sTmpNomParametro = ""
                End If


                '***************************************************************************
                '            La CadenaEjecucionDTS DEL DTS TENDRA LA SIGUIENTE FORMA
                '***************************************************************************
                ' CadenaEjecucionDTS =
                '        /a "Variable1":"8"="Valor1" /a "Variable2":"8"="Valor2"..." +
                '        /n NOMBRE_DTS
                '***************************************************************************
                'Migra HDG 20120718
                If sTmpNomParametro <> "" Then
                    sParametroDTS = sParametroDTS + _
                                        " /set \package.variables[User::" + sTmpNomVariable + "].Value;" + _
                                        sComilla + sTmpNomParametro + sComilla
                End If
                '" /a " + sComilla + sTmpNomVariable + sComilla + ":" + _
                'sComilla + "8" + sComilla + "=" + _
                'sComilla + sTmpNomParametro + sComilla
                'sParametroDTS = sParametroDTS + _
                '                    " /a " + sTmpNomVariable + sComilla + ":" + _
                '                    sComilla + "8" + sComilla + "=" + _
                '                    sComilla + sTmpNomParametro
                'sParametroDTS = sParametroDTS + _
                '                    " /a " + sComilla + sTmpNomVariable + sComilla + ":" + _
                '                    sComilla + "8" + sComilla + "=" + _
                '                    sComilla + sTmpNomParametro + sComilla
                'sParametroDTS = sParametroDTS + _
                '                    " /a " + sTmpNomVariable + sComilla + ":" + _
                '                    sComilla + "8" + sComilla + "=" + _
                '                    sComilla + sTmpNomParametro
            End If

            If (Len(pivVariablesDTS) <= 0) Or (nPosVariableDTS = 0) Then
                bFlg = False
            End If

        End While

        'sParametroDTS = sParametroDTS + " /n " + pivNombreDTs
        sParametroDTS = sParametroDTS '+ " /n " + pivNombreDTs  'Migra HDG 20120718
        Return sParametroDTS
    End Function

End Class
