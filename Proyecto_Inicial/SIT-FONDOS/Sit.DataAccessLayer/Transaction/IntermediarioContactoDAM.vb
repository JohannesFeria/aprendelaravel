Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class IntermediarioContactoDAM

    Public Sub New()

    End Sub

    Public Function Insertar(ByVal oIntermediarioContactoBE As IntermediarioContactoBE, ByVal dataRequest As DataSet) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("IntermediarioContacto_Insertar")
        Dim oRow As IntermediarioContactoBE.IntermediarioContactoRow

        oRow = DirectCast(oIntermediarioContactoBE.IntermediarioContacto.Rows(0), IntermediarioContactoBE.IntermediarioContactoRow)

        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, oRow.CodigoTercero)
        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, oRow.CodigoContacto)
        db.AddInParameter(dbCommand, "@p_Telefono1", DbType.String, oRow.Telefono1)
        db.AddInParameter(dbCommand, "@p_Anexo1", DbType.String, oRow.Anexo1)
        db.AddInParameter(dbCommand, "@p_Telefono2", DbType.String, oRow.Telefono2)
        db.AddInParameter(dbCommand, "@p_anexo2", DbType.String, oRow.Anexo2)
        db.AddInParameter(dbCommand, "@p_Telefono3", DbType.String, oRow.Telefono3)
        db.AddInParameter(dbCommand, "@p_Anexo3", DbType.String, oRow.Anexo3)

        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

    End Function

    Public Function SeleccionarPorFiltro(ByVal codigoTercero As String, ByVal codigoContacto As String, ByVal situacion As String) As IntermediarioContactoBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("IntermediarioContacto_SeleccionarPorFiltro")
        Dim oIntermediarioContactoBE As New IntermediarioContactoBE

        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, codigoContacto)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        db.LoadDataSet(dbCommand, oIntermediarioContactoBE, "IntermediarioContacto")

        Return oIntermediarioContactoBE

    End Function
    Public Function ExistenciaIntermediarioContacto(ByVal codigoTercero As String, ByVal codigoContacto As String) As IntermediarioContactoBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("IntermediarioContacto_Existencia")
        Dim oIntermediarioContactoBE As New IntermediarioContactoBE

        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, codigoContacto)

        db.LoadDataSet(dbCommand, oIntermediarioContactoBE, "IntermediarioContacto")

        Return oIntermediarioContactoBE

    End Function

    Public Function Seleccionar(ByVal codigoIntermediario As String, ByVal codigoContacto As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("IntermediarioContacto_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, codigoIntermediario)
        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, codigoContacto)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function SeleccionarPorCodigoIntermediario(ByVal codigoIntermediario As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("IntermediarioContacto_SeleccionarPorCodigoIntermediario")

        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, codigoIntermediario)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function SeleccionarPorCodigoContacto(ByVal codigoContacto As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("IntermediarioContacto_SeleccionarPorCodigoContacto")

        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, codigoContacto)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function Listar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("IntermediarioContacto_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function Modificar(ByVal oIntermediarioContactoBE As IntermediarioContactoBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("IntermediarioContacto_Modificar")
        Dim oRow As IntermediarioContactoBE.IntermediarioContactoRow

        oRow = DirectCast(oIntermediarioContactoBE.IntermediarioContacto.Rows(0), IntermediarioContactoBE.IntermediarioContactoRow)

        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, oRow.CodigoTercero)
        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, oRow.CodigoContacto)
        db.AddInParameter(dbCommand, "@p_Telefono1", DbType.String, oRow.Telefono1)
        db.AddInParameter(dbCommand, "@p_Anexo1", DbType.String, oRow.Anexo1)
        db.AddInParameter(dbCommand, "@p_Telefono2", DbType.String, oRow.Telefono2)
        db.AddInParameter(dbCommand, "@p_anexo2", DbType.String, oRow.Anexo2)
        db.AddInParameter(dbCommand, "@p_Telefono3", DbType.String, oRow.Telefono3)
        db.AddInParameter(dbCommand, "@p_Anexo3", DbType.String, oRow.Anexo3)

        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

    Public Function Eliminar(ByVal codigoTercero As String, ByVal codigoContacto As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("IntermediarioContacto_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, codigoContacto)

        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

    Public Function EliminarPorCodigoIntermediario(ByVal codigoIntermediario As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("IntermediarioContacto_EliminarPorCodigoIntermediario")

        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, codigoIntermediario)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function EliminarPorCodigoContacto(ByVal codigoContacto As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("IntermediarioContacto_EliminarPorCodigoContacto")

        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, codigoContacto)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

#Region "Funciones Alberto"
    Public Function ListarContactosXIntermediario(ByVal CodigoTercero As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ListarComboContactosxIntermediario")
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, CodigoTercero)
        Return db.ExecuteDataSet(dbCommand)
    End Function
#End Region

End Class

