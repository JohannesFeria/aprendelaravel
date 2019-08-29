Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class PlanDeCuentasDAM

    Private sqlCommand As String = ""
    Private oPlanDeCuentas As PlanDeCuentasBE.PlanDeCuentasRow

    Public Sub New()

    End Sub
#Region "Funciones Seleccionar"
    Public Function SeleccionarPorFiltro(ByVal StrCodigoPortafolioSBS As String, ByVal periodo As Integer, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PlanCuenta_SeleccionarPorFiltro")
        Dim objeto As New DataSet

        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, StrCodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@intPeriodo", DbType.Int32, periodo)
        objeto = db.ExecuteDataSet(dbCommand)
        Return objeto
    End Function

    Public Function Seleccionar(ByVal strCodigoPlanCuenta As String, ByVal dataRequest As DataSet) As PlanDeCuentasBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PlanCuenta_Seleccionar")
        Dim objeto As New PlanDeCuentasBE

        db.AddInParameter(dbCommand, "@p_CuentaContable", DbType.String, strCodigoPlanCuenta)
        'db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, StrCodigoPortafolioSBS)
        db.LoadDataSet(dbCommand, objeto, "PlanDeCuentas")
        Return objeto
    End Function
    Public Function Buscar(ByVal CuentaContable As String, ByVal DescripcionCuenta As String, ByVal portafolio As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PlanCuenta_Buscar")
        Dim objeto As New DataSet

        db.AddInParameter(dbCommand, "@p_CuentaContable", DbType.String, CuentaContable)
        db.AddInParameter(dbCommand, "@p_DescripcionCuenta", DbType.String, DescripcionCuenta)
        'RGF 20080815
        db.AddInParameter(dbCommand, "@p_portafolio", DbType.String, portafolio)
        db.LoadDataSet(dbCommand, objeto, "PlanDeCuentas")
        Return objeto
    End Function

#End Region
End Class

