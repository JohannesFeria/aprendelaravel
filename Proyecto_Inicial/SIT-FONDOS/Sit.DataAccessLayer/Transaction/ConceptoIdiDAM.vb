Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class ConceptoIdiDAM
    Private oVectorTipoCambio As VectorTipoCambio.VectorTipoCambioRow
    Public Sub New()

    End Sub

    Public Function SeleccionarPorCodConcepto(ByVal CodConcepto As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ConceptoIdi_SeleccionarPorCodConcepto")

        db.AddInParameter(dbCommand, "@p_CodigoConcepto", DbType.String, CodConcepto)
        Dim oConceptoIdi As New DataSet
        db.LoadDataSet(dbCommand, oConceptoIdi, "ConceptoIdi")
        Return oConceptoIdi

    End Function

End Class
