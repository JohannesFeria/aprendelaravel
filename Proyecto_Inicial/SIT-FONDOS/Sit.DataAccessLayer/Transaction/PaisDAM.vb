Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

    Public Class PaisDAM

        Private sqlCommand As String = ""
    Private oPaisRow As PaisBE.PaisRow
    Public Sub New()

    End Sub

#Region "Funciones Seleccionar"

    Public Function SeleccionarPorFiltro(ByVal codigoPais As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As PaisBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Pais_SeleccionarPorFiltro")
        Dim oPaisBE As New PaisBE

        db.AddInParameter(dbCommand, "@p_CodigoPais", DbType.String, codigoPais)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        db.LoadDataSet(dbCommand, oPaisBE, "Pais")

        Return oPaisBE

    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As PaisBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Pais_Listar")
        Dim oPaisBE As New PaisBE

        db.LoadDataSet(dbCommand, oPaisBE, "Pais")

        Return oPaisBE

    End Function

#End Region

    Public Function Insertar(ByVal ob As PaisBE, ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Pais_Insertar")

        oPaisRow = CType(ob.Pais.Rows(0), PaisBE.PaisRow)
        db.AddInParameter(dbCommand, "@p_CodigoPais", DbType.String, oPaisRow.CodigoPais)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oPaisRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oPaisRow.Situacion)

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_Paraiso", DbType.String, oPaisRow.Paraiso) 'JRM 20100518
        db.AddInParameter(dbCommand, "@p_DobleImposicion", DbType.String, oPaisRow.DobleImposicion) 'JLPA 20100602
        db.ExecuteNonQuery(dbCommand)

    End Function

    Public Function Modificar(ByVal ob As PaisBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Pais_Modificar")
        oPaisRow = CType(ob.Pais.Rows(0), PaisBE.PaisRow)

        db.AddInParameter(dbCommand, "@p_CodigoPais", DbType.String, oPaisRow.CodigoPais)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oPaisRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oPaisRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Paraiso", DbType.String, oPaisRow.Paraiso) 'JRM 20100518
        db.AddInParameter(dbCommand, "@p_DobleImposicion", DbType.String, oPaisRow.DobleImposicion) 'JLPA 20100602
        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function

    Public Function Eliminar(ByVal codigoPais As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Pais_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoPais", DbType.String, codigoPais)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function

End Class

