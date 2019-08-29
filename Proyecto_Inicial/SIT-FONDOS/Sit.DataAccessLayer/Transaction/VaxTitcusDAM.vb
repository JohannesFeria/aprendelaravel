Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class VaxTitcusDAM

    Public Function SeleccionarPorCartera(ByVal portafolio As String, ByVal fecha As Decimal, ByVal dataRequest As DataSet) As DataSet

        Me.llenarTabla_VaxTicus(portafolio, fecha)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("VaxTicus_SeleccionarPorCartera")

        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, portafolio)
        Dim oVaxTitcus As New DataSet
        db.LoadDataSet(dbCommand, oVaxTitcus, "VaxTitcus")
        Return oVaxTitcus
    End Function
    Public Function GetCuentasPorCobrarPagarToVAX(ByVal portafolio As String, ByVal fecha As Decimal, ByVal dataRequest As DataSet) As DataSet


        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("usp_GetCuentasPorCobrarPagarToVAX")

        db.AddInParameter(dbCommand, "@nvcCodigoPortafolioSBS", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@intFechaOperacion", DbType.Int32.Int32, fecha)

        Dim oVaxTitcus As New DataSet
        Return db.ExecuteDataSet(dbCommand)

    End Function


    Public Sub llenarTabla_VaxTicus(ByVal portafolio As String, ByVal fecha As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("llenarTabla_VaxTicus")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fecha)
        db.ExecuteNonQuery(dbCommand)
    End Sub

    Public Function Forward_GenerarArchivoCPP(ByVal portafolio As String, ByVal fechaIDI As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Forward_GenerarArchivoCPP")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_fechaIDI", DbType.Int32.Int32, fechaIDI)
        Dim oVaxTitcus As New DataSet
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function


End Class
