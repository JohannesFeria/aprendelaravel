Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class CorreccionDepositoPlazoDAM
    Private sqlCommand As String = ""
    Public Sub New()
    End Sub
    Public Function ListarCorreccionDepositoPlazo(ByVal strCodigoPortafolioSBS As String, ByVal strFecha As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CorreccionDepositosPlazo_Listar")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, strCodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, strFecha)
        Dim dsAux As DataSet = db.ExecuteDataSet(dbCommand)
        Dim dsResul As New DataSet
        Dim dtAux As New DataTable
        dtAux.Columns.Add("CodigoPortafolioSBS")
        dtAux.Columns.Add("NroOrden")
        dtAux.Columns.Add("Intermediario")
        dtAux.Columns.Add("MontoOperacion")
        dtAux.Columns.Add("Plazo")
        dtAux.Columns.Add("Tasa")
        dtAux.Columns.Add("CodigoSBS")
        dtAux.Columns.Add("CodigoTipoTitulo")
        dtAux.Columns.Add("CodigoOperacion")
        Dim drAux As DataRow
        Dim i As Integer
        For i = 0 To dsAux.Tables(0).Rows.Count - 1
            drAux = dtAux.NewRow
            drAux("CodigoPortafolioSBS") = dsAux.Tables(0).Rows(i)("CodigoPortafolioSBS")
            drAux("NroOrden") = dsAux.Tables(0).Rows(i)("NroOrden")
            drAux("Intermediario") = dsAux.Tables(0).Rows(i)("Intermediario")
            drAux("MontoOperacion") = Format(dsAux.Tables(0).Rows(i)("MontoOperacion"), "###,###,##0.00")
            drAux("Plazo") = dsAux.Tables(0).Rows(i)("Plazo")
            drAux("Tasa") = Format(dsAux.Tables(0).Rows(i)("Tasa"), "###,###,##0.0000000")
            drAux("CodigoSBS") = dsAux.Tables(0).Rows(i)("CodigoSBS")
            drAux("CodigoTipoTitulo") = dsAux.Tables(0).Rows(i)("CodigoTipoTitulo")
            drAux("CodigoOperacion") = dsAux.Tables(0).Rows(i)("CodigoOperacion")
            dtAux.Rows.Add(drAux)
        Next
        dsResul.Tables.Add(dtAux)
        Return dsResul
    End Function
End Class