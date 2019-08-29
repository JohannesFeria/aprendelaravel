Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class MontoNegociadoBVLDAM
    Private sqlCommand As String = ""
    Private oMontoNegociadoBVLRow As MontoNegociadoBVLBE.MontoNegociadoBVLRow

    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "

    Public Function Listar() As MontoNegociadoBVLBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MontoNegociadoBVL_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As MontoNegociadoBVLBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MontoNegociadoBVL_Listar")

        Dim objeto As New MontoNegociadoBVLBE
        db.LoadDataSet(dbCommand, objeto, "MontoNegociadoBVL")
        Return objeto
    End Function


    Public Function SeleccionarPorFiltro(ByVal FechaOperacion As Decimal, ByVal NumeroOperacion As Decimal, ByVal CodigoMnemonico As String, ByVal Situacion As String, ByVal dataRequest As DataSet) As MontoNegociadoBVLBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MontoNegociadoBVL_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
        db.AddInParameter(dbCommand, "@p_NumeroOperacion", DbType.Decimal, NumeroOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, CodigoMnemonico)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, Situacion)

        Dim objeto As New MontoNegociadoBVLBE
        db.LoadDataSet(dbCommand, objeto, "MontoNegociadoBVL")
        Return objeto
    End Function

#End Region


    Public Function Insertar(ByVal ob As MontoNegociadoBVLBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MontoNegociadoBVL_Insertar")
        oMontoNegociadoBVLRow = CType(ob.MontoNegociadoBVL.Rows(0), MontoNegociadoBVLBE.MontoNegociadoBVLRow)

        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, oMontoNegociadoBVLRow.FechaOperacion)
        db.AddInParameter(dbCommand, "@p_HoraOperacion", DbType.String, "")
        db.AddInParameter(dbCommand, "@p_NumeroOperacion", DbType.Decimal, oMontoNegociadoBVLRow.NumeroOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, oMontoNegociadoBVLRow.CodigoMnemonico)
        db.AddInParameter(dbCommand, "@p_Precio", DbType.Decimal, oMontoNegociadoBVLRow.Precio)
        db.AddInParameter(dbCommand, "@p_Cantidad", DbType.Decimal, oMontoNegociadoBVLRow.Cantidad)
        db.AddInParameter(dbCommand, "@p_Comprador", DbType.String, oMontoNegociadoBVLRow.Comprador)
        db.AddInParameter(dbCommand, "@p_Vendedor", DbType.String, oMontoNegociadoBVLRow.Vendedor)
        db.AddInParameter(dbCommand, "@p_MontoEfectivo", DbType.Decimal, oMontoNegociadoBVLRow.MontoEfectivo)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oMontoNegociadoBVLRow.Situacion)

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
    End Function


    Public Function Modificar(ByVal ob As MontoNegociadoBVLBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MontoNegociadoBVL_Modificar")
        oMontoNegociadoBVLRow = CType(ob.MontoNegociadoBVL.Rows(0), MontoNegociadoBVLBE.MontoNegociadoBVLRow)

        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, oMontoNegociadoBVLRow.FechaOperacion)
        db.AddInParameter(dbCommand, "@p_HoraOperacion", DbType.String, "")
        db.AddInParameter(dbCommand, "@p_NumeroOperacion", DbType.Decimal, oMontoNegociadoBVLRow.NumeroOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, oMontoNegociadoBVLRow.CodigoMnemonico)
        db.AddInParameter(dbCommand, "@p_Precio", DbType.Decimal, oMontoNegociadoBVLRow.Precio)
        db.AddInParameter(dbCommand, "@p_Cantidad", DbType.Decimal, oMontoNegociadoBVLRow.Cantidad)
        db.AddInParameter(dbCommand, "@p_Comprador", DbType.String, oMontoNegociadoBVLRow.Comprador)
        db.AddInParameter(dbCommand, "@p_Vendedor", DbType.String, oMontoNegociadoBVLRow.Vendedor)
        db.AddInParameter(dbCommand, "@p_MontoEfectivo", DbType.Decimal, oMontoNegociadoBVLRow.MontoEfectivo)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oMontoNegociadoBVLRow.Situacion)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function


    Public Function Eliminar(ByVal FechaOperacion As Decimal, ByVal NumeroOperacion As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MontoNegociadoBVL_Eliminar")

        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.String, FechaOperacion)
        db.AddInParameter(dbCommand, "@p_NumeroOperacion", DbType.String, NumeroOperacion)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
End Class
