'Creado por: HDG OT 62087 Nro6-R14 20110201
Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class LineasContraparteFWDDAM

    Public Function ActualizarLineasContraparteFWDPorExcel(ByVal dtData As DataTable, ByVal dataRequest As DataSet, ByRef strmensaje As String, ByVal tipo As Integer) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sCodFwd As String
        Dim sCodEmi As String
        Dim nFactor As Decimal
        Dim nLineaAsig As Decimal
        Dim sRating As String
        Dim sUsuario As String, sHost As String
        Dim nFecMod As Decimal
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_upd_ActualizaparaLinEmisorFWDxExcel")

        sUsuario = DataUtility.ObtenerValorRequest(dataRequest, "Usuario")
        nFecMod = DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha"))
        sHost = DataUtility.ObtenerValorRequest(dataRequest, "Host")

        db.AddInParameter(dbCommand, "@p_NumeroPoliza", DbType.String)
        db.AddInParameter(dbCommand, "@p_CodigoEmisor", DbType.String)
        db.AddInParameter(dbCommand, "@p_Factor", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_LineaAsignada", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_Rating", DbType.String)
        db.AddInParameter(dbCommand, "@p_Tipo", DbType.Int16)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String)

        For Each filaLinea As DataRow In dtData.Rows

            sCodFwd = filaLinea(0).ToString().Trim()

            If (Not sCodFwd.Trim.Equals("")) Then
                If tipo = 1 Then
                    sCodEmi = ""
                    nFactor = Val(filaLinea(1).ToString().Trim())
                    sRating = ""
                    nLineaAsig = 0
                Else
                    sCodEmi = sCodFwd
                    sCodFwd = ""
                    nFactor = 0
                    sRating = filaLinea(1).ToString().Trim().ToUpper()
                    nLineaAsig = Val(filaLinea(2).ToString().Trim())
                End If

                db.SetParameterValue(dbCommand, "@p_NumeroPoliza", sCodFwd)
                db.SetParameterValue(dbCommand, "@p_CodigoEmisor", sCodEmi)
                db.SetParameterValue(dbCommand, "@p_Factor", nFactor)
                db.SetParameterValue(dbCommand, "@p_LineaAsignada", nLineaAsig)
                db.SetParameterValue(dbCommand, "@p_Rating", sRating)
                db.SetParameterValue(dbCommand, "@p_Tipo", tipo)
                db.SetParameterValue(dbCommand, "@p_UsuarioCreacion", sUsuario)
                db.SetParameterValue(dbCommand, "@p_FechaCreacion", nFecMod)
                db.SetParameterValue(dbCommand, "@p_HoraCreacion", DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.SetParameterValue(dbCommand, "@p_Host", sHost)
                db.ExecuteNonQuery(dbCommand)

            End If
        Next
        Return True
    End Function

End Class
