Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class InventarioCartasDAM
    Private sqlCommand As String = ""
    Private oInventarioCartasRow As InventarioCartasBE.InventarioCartasRow
    Dim DECIMAL_NULO As Decimal = -1
    Public Sub New()

    End Sub

    Public Function Insertar(ByVal oInventarioCartas As InventarioCartasBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        oInventarioCartasRow = CType(oInventarioCartas.InventarioCartas.Rows(0), InventarioCartasBE.InventarioCartasRow)
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_ins_Insertar_InventarioCartas")
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, oInventarioCartasRow.Fecha)
        db.AddInParameter(dbCommand, "@p_RangoInicial", DbType.Decimal, oInventarioCartasRow.RangoInicial)
        db.AddInParameter(dbCommand, "@p_RangoFinal", DbType.Decimal, oInventarioCartasRow.RangoFinal)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Eliminar(ByVal codigoInventario As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_del_Eliminar_InventarioCartas")
        db.AddInParameter(dbCommand, "@p_CodigoInventario", DbType.Decimal, codigoInventario)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Seleccionar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_Seleccionar_InventarioCartas")
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Sub InicializarInventarioCartas(ByRef oRow As InventarioCartasBE.InventarioCartasRow)
        oRow.CodigoInventario = DECIMAL_NULO
        oRow.Fecha = DECIMAL_NULO
        oRow.RangoInicial = DECIMAL_NULO
        oRow.RangoFinal = DECIMAL_NULO
        oRow.UsuarioCreacion = ""
        oRow.FechaCreacion = DECIMAL_NULO
        oRow.HoraCreacion = ""
        oRow.Host = ""
    End Sub

End Class
