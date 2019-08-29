Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class FactorDAM

    Private sqlCommand As String = ""
    Private oFactorRow As FactorBE.FactorRow

    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "

    Public Function Listar() As FactorBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Factor_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As FactorBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Factor_Listar")

        Dim objeto As New FactorBE
        db.LoadDataSet(dbCommand, objeto, "Factor")
        Return objeto
    End Function


    Public Function SeleccionarPorFiltro(ByVal TipoFactor As String, ByVal CodigoMnemonico As String, ByVal CodigoEntidad As String, ByVal Situacion As String, ByVal GrupoFactor As String, ByVal dataRequest As DataSet) As FactorBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Factor_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_TipoFactor", DbType.String, TipoFactor)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, CodigoMnemonico)
        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, CodigoEntidad)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, Situacion)
        'CMB 20101028 - REQ 30 - OT 61566
        db.AddInParameter(dbCommand, "@p_GrupoFactor", DbType.String, GrupoFactor)
        Dim objeto As New FactorBE
        db.LoadDataSet(dbCommand, objeto, "Factor")
        Return objeto
    End Function

#End Region


    Public Function Insertar(ByVal ob As FactorBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Factor_Insertar")
        oFactorRow = CType(ob.Factor.Rows(0), FactorBE.FactorRow)
        'INI CMB 20101028 - REQ 30 - OT 61566
        db.AddInParameter(dbCommand, "@p_TipoFactor", DbType.String, oFactorRow.TipoFactor)
        If oFactorRow.GrupoFactor = "1" Then
            db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, oFactorRow.CodigoEntidad)
            db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, DBNull.Value)
            db.AddInParameter(dbCommand, "@p_Factor", DbType.Decimal, oFactorRow.Factor)
            db.AddInParameter(dbCommand, "@p_FloatOficioMultiple", DbType.Decimal, DBNull.Value)
        Else
            If oFactorRow.GrupoFactor = "2" Then
                db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, DBNull.Value)
                db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, oFactorRow.CodigoMnemonico)
                db.AddInParameter(dbCommand, "@p_Factor", DbType.Decimal, DBNull.Value)
                db.AddInParameter(dbCommand, "@p_FloatOficioMultiple", DbType.Decimal, oFactorRow.FloatOficioMultiple)
            End If
        End If
        db.AddInParameter(dbCommand, "@p_Alerta", DbType.Decimal, DBNull.Value)
        db.AddInParameter(dbCommand, "@p_AccionesCirculacionRV", DbType.Decimal, DBNull.Value)
        db.AddInParameter(dbCommand, "@p_MarketShare", DbType.Decimal, DBNull.Value)

        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oFactorRow.Situacion)
        db.AddInParameter(dbCommand, "@p_FechaVigencia", DbType.Decimal, oFactorRow.FechaVigencia)
        db.AddInParameter(dbCommand, "@p_GrupoFactor", DbType.String, oFactorRow.GrupoFactor)

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Function


    Public Function Modificar(ByVal ob As FactorBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Factor_Modificar")
        oFactorRow = CType(ob.Factor.Rows(0), FactorBE.FactorRow)
        'INI CMB 20101028 - REQ 30 - OT 61566
        db.AddInParameter(dbCommand, "@p_TipoFactor", DbType.String, oFactorRow.TipoFactor)

        If oFactorRow.GrupoFactor = "1" Then
            db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, oFactorRow.CodigoEntidad)
            db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, DBNull.Value)
            db.AddInParameter(dbCommand, "@p_Factor", DbType.Decimal, oFactorRow.Factor)
            db.AddInParameter(dbCommand, "@p_FloatOficioMultiple", DbType.Decimal, DBNull.Value)
        Else
            If oFactorRow.GrupoFactor = "2" Then
                db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, DBNull.Value)
                db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, oFactorRow.CodigoMnemonico)
                db.AddInParameter(dbCommand, "@p_Factor", DbType.Decimal, DBNull.Value)
                db.AddInParameter(dbCommand, "@p_FloatOficioMultiple", DbType.Decimal, oFactorRow.FloatOficioMultiple)
            End If
        End If
        db.AddInParameter(dbCommand, "@p_Alerta", DbType.Decimal, DBNull.Value)
        db.AddInParameter(dbCommand, "@p_AccionesCirculacionRV", DbType.Decimal, DBNull.Value)
        db.AddInParameter(dbCommand, "@p_MarketShare", DbType.Decimal, DBNull.Value)

        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oFactorRow.Situacion)
        db.AddInParameter(dbCommand, "@p_FechaVigencia", DbType.Decimal, oFactorRow.FechaVigencia)
        db.AddInParameter(dbCommand, "@p_GrupoFactor", DbType.String, oFactorRow.GrupoFactor)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function


    Public Function Eliminar(ByVal TipoFactor As String, ByVal CodigoEntidad As String, ByVal CodigoMnemonico As String, ByVal GrupoFactor As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Factor_Eliminar")

        db.AddInParameter(dbCommand, "@p_TipoFactor", DbType.String, TipoFactor)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, CodigoMnemonico)
        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, CodigoEntidad)

        db.AddInParameter(dbCommand, "@p_GrupoFactor", DbType.String, GrupoFactor) 'CMB 20101028 - REQ 30 - OT 61566

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    'INI CMB OT 61566 20101102
    Public Function ActualizarFactorPorExcel(ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet, ByRef strmensaje As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sCodMnemo As String, sCodEntidad As String
        Dim sSituacion As String, sTipoFactor As String
        Dim nFechaCreacion As Decimal, nFactor As Decimal
        Dim nFechaVigencia As Decimal, nFechaMod As Decimal
        Dim sGrupoFactor As String, sUsuarioMod As String
        Dim sHoraMod As String, sHost As String
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_upd_ActualizaFactorxExcel")

        sUsuarioMod = DataUtility.ObtenerValorRequest(dataRequest, "Usuario")
        nFechaMod = DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha"))
        sHost = DataUtility.ObtenerValorRequest(dataRequest, "Host")

        db.AddInParameter(dbCommand, "@p_TipoFactor", DbType.String)
        db.AddInParameter(dbCommand, "@p_Codigoentidad", DbType.String)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_Factor", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_FechaVigencia", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_GrupoFactor", DbType.String)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String)

        For Each filaLinea As DataRow In dtDetalle.Rows
            sGrupoFactor = filaLinea(0).ToString().Trim()
            If (Not sGrupoFactor.Equals("")) Then
                If sGrupoFactor = "1" Then
                    sTipoFactor = filaLinea(1).ToString().Trim()
                    sCodEntidad = filaLinea(3).ToString().Trim()
                    nFactor = IIf(filaLinea(5).ToString() = "", Nothing, Val(filaLinea(5).ToString().Trim()))
                    sSituacion = filaLinea(6).ToString().Trim()

                    'nFechaCreacion = IIf(filaLinea(7).ToString() = "", Nothing, Val(filaLinea(7).ToString().Trim()))
                    Try
                        nFechaCreacion = IIf(filaLinea(7).ToString() = "", 0, DataUtility.ObtenerFecha(filaLinea(7).ToString().Trim()))
                    Catch ex As Exception
                        nFechaCreacion = 0
                    End Try

                    'nFechaVigencia = IIf(filaLinea(8).ToString() = "", Nothing, Val(filaLinea(8).ToString().Trim()))
                    Try
                        nFechaVigencia = IIf(filaLinea(8).ToString() = "", 0, DataUtility.ObtenerFecha(filaLinea(8).ToString().Trim()))
                    Catch ex As Exception
                        nFechaVigencia = 0
                    End Try

                Else
                    If sGrupoFactor = "2" Then
                        sTipoFactor = filaLinea(1).ToString().Trim()
                        sCodMnemo = filaLinea(5).ToString().Trim()
                        sCodEntidad = filaLinea(7).ToString().Trim()
                        nFactor = IIf(filaLinea(9).ToString() = "", Nothing, Val(filaLinea(9).ToString().Trim()))
                        sSituacion = filaLinea(10).ToString().Trim()
                        'nFechaCreacion = IIf(filaLinea(11).ToString() = "", Nothing, Val(filaLinea(11).ToString().Trim()))
                        Try
                            nFechaCreacion = IIf(filaLinea(11).ToString() = "", 0, DataUtility.ObtenerFecha(filaLinea(11).ToString().Trim()))
                        Catch ex As Exception
                            nFechaCreacion = 0
                        End Try

                        'nFechaVigencia = IIf(filaLinea(12).ToString() = "", Nothing, Val(filaLinea(12).ToString().Trim()))
                        Try
                            nFechaVigencia = IIf(filaLinea(12).ToString() = "", 0, DataUtility.ObtenerFecha(filaLinea(12).ToString().Trim()))
                        Catch ex As Exception
                            nFechaVigencia = 0
                        End Try

                    End If
                End If
                db.SetParameterValue(dbCommand, "@p_TipoFactor", sTipoFactor)
                db.SetParameterValue(dbCommand, "@p_Codigoentidad", sCodEntidad)
                sCodMnemo = IIf(sCodMnemo Is Nothing, "", sCodMnemo)
                db.SetParameterValue(dbCommand, "@p_CodigoMnemonico", sCodMnemo)
                db.SetParameterValue(dbCommand, "@p_Situacion", sSituacion)
                db.SetParameterValue(dbCommand, "@p_FechaCreacion", nFechaCreacion)
                If nFactor > 1 Then
                    db.SetParameterValue(dbCommand, "@p_Factor", DBNull.Value)
                Else
                    db.SetParameterValue(dbCommand, "@p_Factor", nFactor)
                End If

                If nFechaVigencia = 0 Then
                    db.SetParameterValue(dbCommand, "@p_FechaVigencia", DBNull.Value)
                Else
                    db.SetParameterValue(dbCommand, "@p_FechaVigencia", nFechaVigencia)
                End If
                db.SetParameterValue(dbCommand, "@p_GrupoFactor", sGrupoFactor)
                db.SetParameterValue(dbCommand, "@p_UsuarioModificacion", sUsuarioMod)
                db.SetParameterValue(dbCommand, "@p_FechaModificacion", nFechaMod)
                db.SetParameterValue(dbCommand, "@p_HoraModificacion", DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.SetParameterValue(dbCommand, "@p_Host", sHost)
                If nFechaCreacion <> 0 And sTipoFactor <> "" And (sCodEntidad <> "" Or sCodMnemo <> "") Then
                    db.ExecuteNonQuery(dbCommand)
                End If
            End If
        Next
        strmensaje &= "Los datos se cargaron correctamente\n"
        Return True
    End Function
    'FIN CMB OT 61566 20101102
End Class
