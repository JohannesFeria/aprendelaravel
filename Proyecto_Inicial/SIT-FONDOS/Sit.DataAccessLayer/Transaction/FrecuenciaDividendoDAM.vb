Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql


'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Creación de Objeto DataAccessLayer para tabla FrecuenciaDividendo| 18/05/18

Public Class FrecuenciaDividendoDAM
    Private sqlCommand As String = ""
    Private oRow As FrecuenciaDividendoBE.FrecuenciaDividendoRow
    Public Sub New()

    End Sub
#Region " /* Funciones Seleccionar */ "

    Public Function Listar(ByVal dataRequest As DataSet) As FrecuenciaDividendoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("FrecuenciaDividendo_Listar")
            Dim oFrecuenciaDividendoBE As New FrecuenciaDividendoBE
            db.LoadDataSet(dbCommand, oFrecuenciaDividendoBE, "FrecuenciaDividendo")
            Return oFrecuenciaDividendoBE
        End Using
    End Function
#End Region

#Region " /* Funciones Insertar */ "

#End Region

#Region " /* Funciones Modificar */"

#End Region

#Region " /* Funciones Eliminar */"

#End Region

#Region " /* Funciones Personalizadas*/"

#End Region


End Class
'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Creación de Objeto DataAccessLayer para tabla FrecuenciaDividendo| 18/05/18

