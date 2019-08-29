'Creado por: CMB OT 66087 20130429
Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class LineasSettlementDAM

    Public Function ActualizarLineasSettlementPorExcel(ByVal dtData As DataTable, ByVal dataRequest As DataSet, ByRef strmensaje As String) As Boolean
        Dim sSettlement As String
        Dim nLinea As Decimal
        Dim sUsuario As String, sHost As String
        Dim nFecMod As Decimal

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_ActualizaLineasSettlementxExcel")
        
        sUsuario = DataUtility.ObtenerValorRequest(dataRequest, "Usuario")
        nFecMod = DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha"))
        sHost = DataUtility.ObtenerValorRequest(dataRequest, "Host")

        db.AddInParameter(dbCommand, "@p_CodigoSettlement", DbType.String)
        db.AddInParameter(dbCommand, "@p_Linea", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String)

         For Each filaLinea As DataRow In dtData.Rows

            sSettlement = filaLinea(0).ToString().Trim()

            If (Not sSettlement.Trim.Equals("")) Then

                nLinea = Val(filaLinea(2).ToString().Trim())

                db.SetParameterValue(dbCommand, "@p_CodigoSettlement", sSettlement)
                db.SetParameterValue(dbCommand, "@p_Linea", nLinea)
                db.SetParameterValue(dbCommand, "@p_UsuarioCreacion", sUsuario)
                db.SetParameterValue(dbCommand, "@p_FechaCreacion", nFecMod)
                db.SetParameterValue(dbCommand, "@p_HoraCreacion", DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.SetParameterValue(dbCommand, "@p_Host", sHost)
                db.ExecuteNonQuery(dbCommand)

            End If
        Next
        strmensaje &= "Los datos se cargaron correctamente\n"
    End Function

End Class
