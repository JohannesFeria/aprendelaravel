'Creado por: HDG OT 62087 Nro5-R10 20110118
Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class LineasCreditoDAM

    Public Function ActualizarLineasCreditoPorExcel(ByVal dtData As DataTable, ByVal dataRequest As DataSet, ByRef strmensaje As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sEmisor As String
        Dim sClasifica As String
        Dim nLinea As Decimal
        Dim sUsuario As String, sHost As String
        Dim nFecMod As Decimal
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_upd_ActualizaLineasCreditoxExcel")

        sUsuario = DataUtility.ObtenerValorRequest(dataRequest, "Usuario")
        nFecMod = DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha"))
        sHost = DataUtility.ObtenerValorRequest(dataRequest, "Host")

        db.AddInParameter(dbCommand, "@p_CodigoEmisor", DbType.String)
        db.AddInParameter(dbCommand, "@p_Linea", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_Clasificacion", DbType.String)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String)

        For Each filaLinea As DataRow In dtData.Rows

            sEmisor = filaLinea(0).ToString().Trim()
            sClasifica = filaLinea(1).ToString().Trim().ToUpper()

            If (Not sEmisor.Trim.Equals("")) And (Not sClasifica.Trim.Equals("")) Then

                nLinea = Val(filaLinea(2).ToString().Trim())
                sClasifica = IIf(sClasifica = "CORTO PLAZO", "1", IIf(sClasifica = "LARGO PLAZO", "2", "3"))

                db.SetParameterValue(dbCommand, "@p_CodigoEmisor", sEmisor)
                db.SetParameterValue(dbCommand, "@p_Linea", nLinea)
                db.SetParameterValue(dbCommand, "@p_Clasificacion", sClasifica)
                db.SetParameterValue(dbCommand, "@p_UsuarioCreacion", sUsuario)
                db.SetParameterValue(dbCommand, "@p_FechaCreacion", nFecMod)
                db.SetParameterValue(dbCommand, "@p_HoraCreacion", DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.SetParameterValue(dbCommand, "@p_Host", sHost)
                db.ExecuteNonQuery(dbCommand)

            End If
        Next
        strmensaje &= "Los datos se cargaron correctamente\n"
        Return True
    End Function

End Class
