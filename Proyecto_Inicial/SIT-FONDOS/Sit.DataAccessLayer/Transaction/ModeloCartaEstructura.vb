Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class ModeloCartaEstructuraDAM
    Private sqlCommand As String = ""
    Public oModeloCartaEstructuraRow As ModeloCartaEstructuraBE.ModeloCartaEstructuraRow
    Public Sub New()

    End Sub
#Region " /* Funciones Seleccionar */ "

    Public Function SeleccionarPorFiltro(ByVal codigoModelo As String, ByVal dataRequest As DataSet) As ModeloCartaEstructuraBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("ModeloCartaEstructura_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, codigoModelo)


        Dim objeto As New Sit.BusinessEntities.ModeloCartaEstructuraBE
        db.LoadDataSet(dbCommand, objeto, "ModeloCartaEstructura")
        Return objeto
    End Function
    Public Function Listar() As ModeloCartaEstructuraBE

        Dim oModeloCartaEstructura As New ModeloCartaEstructuraBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("ModeloCartaEstructura_Listar")

        db.LoadDataSet(dbCommand, oModeloCartaEstructura, "ModeloCartaEstructura")

        Return oModeloCartaEstructura

    End Function

#End Region

    Public Function Insertar(ByVal ob As ModeloCartaEstructuraBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("ModeloCartaEstructura_Insertar")
        oModeloCartaEstructuraRow = CType(ob._ModeloCartaEstructura.Rows(0), ModeloCartaEstructuraBE.ModeloCartaEstructuraRow)

        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, oModeloCartaEstructuraRow.CodigoModelo)
        db.AddInParameter(dbCommand, "@p_NombreCampo", DbType.String, oModeloCartaEstructuraRow.NombreCampo)
        db.AddInParameter(dbCommand, "@p_OrigenCampo", DbType.String, oModeloCartaEstructuraRow.OrigenCampo)

        db.ExecuteNonQuery(dbCommand)
    End Function

    Public Function Modificar(ByVal ob As ModeloCartaEstructuraBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("ModeloCartaEstructura_Modificar")
        oModeloCartaEstructuraRow = CType(ob._ModeloCartaEstructura.Rows(0), ModeloCartaEstructuraBE.ModeloCartaEstructuraRow)

        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, oModeloCartaEstructuraRow.CodigoModelo)
        db.AddInParameter(dbCommand, "@p_NombreCampo", DbType.String, oModeloCartaEstructuraRow.NombreCampo)
        db.AddInParameter(dbCommand, "@p_OrigenCampo", DbType.String, oModeloCartaEstructuraRow.OrigenCampo)
        db.AddInParameter(dbCommand, "@p_NombreCampoNuevo", DbType.String, oModeloCartaEstructuraRow.NombreCampoNuevo)
        db.AddInParameter(dbCommand, "@p_OrigenCampoNuevo", DbType.String, oModeloCartaEstructuraRow.OrigenCampoNuevo)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Eliminar(ByVal ob As ModeloCartaEstructuraBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("ModeloCartaEstructura_Eliminar")
        oModeloCartaEstructuraRow = CType(ob._ModeloCartaEstructura.Rows(0), ModeloCartaEstructuraBE.ModeloCartaEstructuraRow)

        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, oModeloCartaEstructuraRow.CodigoModelo)
        db.AddInParameter(dbCommand, "@p_NombreCampo", DbType.String, oModeloCartaEstructuraRow.NombreCampo)
        db.AddInParameter(dbCommand, "@p_OrigenCampo", DbType.String, oModeloCartaEstructuraRow.OrigenCampo)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

End Class

