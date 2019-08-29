Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

	Public class TipoValorizacionDAM
        Private sqlCommand As String = ""
    Private oRow As TipoValorizacionBE.TipoValorizacionRow
		Public Sub New()

		End Sub

#Region " /* Funciones Seleccionar */ "

    Public Function SeleccionarPorFiltroBcrSeriado(ByVal NombreCuenta As String, ByVal dataRequest As DataSet) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("BCRSeriados_SeleccionarPorFiltro")
        Dim oData As New DataSet
        db.AddInParameter(dbCommand, "@p_NombreCuenta", DbType.String, NombreCuenta)
        db.LoadDataSet(dbCommand, oData, "BcrSeriado")
        Return oData.Tables(0)
    End Function

    Public Function SeleccionarBcrSeriado(ByVal codigoMnemonico As String, ByVal dataRequest As DataSet) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("BCRSeriado_Seleccionar")
        db.AddInParameter(dbCommand, "@p_codigoMnemonico", DbType.String, codigoMnemonico)
        Dim oData As New DataSet
        db.LoadDataSet(dbCommand, oData, "TipoValorizacion")
        Return oData.Tables(0)
    End Function

    Public Function SeleccionarPorFiltroBcrUnico(ByVal NombreCuenta As String, ByVal dataRequest As DataSet) As BCRUnicoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("BCRUnico_SeleccionarPorFiltro")
        Dim oBE As New BCRUnicoBE
        db.AddInParameter(dbCommand, "@p_NombreCuenta", DbType.String, NombreCuenta)
        db.LoadDataSet(dbCommand, oBE, "BCRUnico")
        Return oBE
    End Function

    Public Function SeleccionarBcrUnico(ByVal codigoSBS As String, ByVal dataRequest As DataSet) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("BCRUnico_Seleccionar")
        db.AddInParameter(dbCommand, "@p_codigoSBS", DbType.String, codigoSBS)
        Dim oData As New DataSet
        db.LoadDataSet(dbCommand, oData, "TipoValorizacion")
        Return oData.Tables(0)
    End Function

    Public Function SeleccionarPorFiltro(ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As TipoValorizacionBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoValorizacion_SeleccionarPorFiltro")
        Dim oTipoValorizacionBE As New TipoValorizacionBE
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.LoadDataSet(dbCommand, oTipoValorizacionBE, "TipoValorizacion")
        Return oTipoValorizacionBE
    End Function

    Public Function Seleccionar(ByVal codigoTipoValorizacion As String, ByVal dataRequest As DataSet) As TipoValorizacionBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoValorizacion_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoTipoValorizacion", DbType.String, codigoTipoValorizacion)
        Dim oTipoValorizacionBE As New TipoValorizacionBE
        db.LoadDataSet(dbCommand, oTipoValorizacionBE, "TipoValorizacion")
        Return oTipoValorizacionBE
    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As TipoValorizacionBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoValorizacion_Listar")

        Dim oTipoValorizacionBE As New TipoValorizacionBE
        db.LoadDataSet(dbCommand, oTipoValorizacionBE, "TipoValorizacion")
        Return oTipoValorizacionBE
    End Function

#End Region

#Region " /* Funciones Insertar */ "

    Public Function Insertar(ByVal oTipoValorizacionBE As TipoValorizacionBE, ByVal dataRequest As DataSet) As String

        Dim Codigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoValorizacion_Insertar")

        oRow = DirectCast(oTipoValorizacionBE.TipoValorizacion.Rows(0), TipoValorizacionBE.TipoValorizacionRow)

        db.AddInParameter(dbCommand, "@p_CodigoTipoValorizacion", DbType.String, oRow.CodigoTipoValorizacion)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return Codigo

    End Function

    Public Function InsertarBCRSeriado(ByVal oBCRSeriadoBE As BCRSeriadoBE, ByVal dataRequest As DataSet) As Boolean
        Dim Codigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("BCRSeriado_Insertar")
        Dim oRow1 As BCRSeriadoBE.BCRSeriadoRow
        oRow1 = DirectCast(oBCRSeriadoBE.BCRSeriado.Rows(0), BCRSeriadoBE.BCRSeriadoRow)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, oRow1.CodigoMnemonico)
        db.AddInParameter(dbCommand, "@p_CuentaContable", DbType.String, oRow1.CuentaContable)
        db.AddInParameter(dbCommand, "@p_NombreCuenta", DbType.String, oRow1.NombreCuenta)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function InsertarBCRUnico(ByVal oBCRUnicoBE As BCRUnicoBE, ByVal dataRequest As DataSet) As Boolean
        Dim Codigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("BCRUnico_Insertar")
        Dim oRow1 As BCRUnicoBE.BCRUnicoRow
        oRow1 = DirectCast(oBCRUnicoBE.BCRUnico.Rows(0), BCRUnicoBE.BCRUnicoRow)
        db.AddInParameter(dbCommand, "@p_CodigoTipoTitulo", DbType.String, oRow1.CodigoTipoTitulo)
        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, oRow1.CodigoEntidad)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oRow1.CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, oRow1.CodigoSBS)
        db.AddInParameter(dbCommand, "@p_CuentaContable", DbType.String, oRow1.CuentaContable)
        db.AddInParameter(dbCommand, "@p_NombreCuenta", DbType.String, oRow1.NombreCuenta)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

#End Region

#Region " /* Funciones Modificar */"

    Public Function ModificarBCRSeriado(ByVal oBCRSeriadoBE As BCRSeriadoBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("BCRSeriado_Modificar")
        Dim oRow1 As BCRSeriadoBE.BCRSeriadoRow
        oRow1 = CType(oBCRSeriadoBE.BCRSeriado.Rows(0), BCRSeriadoBE.BCRSeriadoRow)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, oRow1.CodigoMnemonico)
        db.AddInParameter(dbCommand, "@p_CuentaContable", DbType.String, oRow1.CuentaContable)
        db.AddInParameter(dbCommand, "@p_NombreCuenta", DbType.String, oRow1.NombreCuenta)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function ModificarBCRUnico(ByVal oBCRUnicoBE As BCRUnicoBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("BCRUnico_Modificar")
        Dim oRow1 As BCRUnicoBE.BCRUnicoRow
        oRow1 = DirectCast(oBCRUnicoBE.BCRUnico.Rows(0), BCRUnicoBE.BCRUnicoRow)
        db.AddInParameter(dbCommand, "@p_CodigoTipoTitulo", DbType.String, oRow1.CodigoTipoTitulo)
        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, oRow1.CodigoEntidad)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oRow1.CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, oRow1.CodigoSBS)
        db.AddInParameter(dbCommand, "@p_CuentaContable", DbType.String, oRow1.CuentaContable)
        db.AddInParameter(dbCommand, "@p_NombreCuenta", DbType.String, oRow1.NombreCuenta)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Modificar(ByVal oTipoValorizacionBE As TipoValorizacionBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoValorizacion_Modificar")
        oRow = CType(oTipoValorizacionBE.TipoValorizacion.Rows(0), TipoValorizacionBE.TipoValorizacionRow)

        db.AddInParameter(dbCommand, "@p_CodigoTipoValorizacion", DbType.String, oRow.CodigoTipoValorizacion)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function


#End Region

#Region " /* Funciones Eliminar */"

    Public Function EliminarBCRSeriado(ByVal codigoMnemonico As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("BCRSeriado_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, codigoMnemonico)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function


    Public Function EliminarBCRUnico(ByVal codigoSBS As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("BCRUnico_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, codigoSBS)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Eliminar(ByVal codigoTipoValorizacion As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoValorizacion_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoTipoValorizacion", DbType.String, codigoTipoValorizacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

#End Region

#Region " /* Funciones Personalizadas*/"

#End Region

End Class