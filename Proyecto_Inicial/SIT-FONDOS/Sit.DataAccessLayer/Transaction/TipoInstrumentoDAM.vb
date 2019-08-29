Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class TipoInstrumentoDAM
    Private sqlCommand As String = ""
    Private oRow As TipoInstrumentoBE.TipoInstrumentoRow
    Public Sub New()

    End Sub


#Region " /* Funciones Seleccionar */ "

    Public Function SeleccionarPorFiltro(ByVal codigoClaseInstrumento As String, ByVal sinonimo As String, ByVal TipoRenta As String, ByVal situacion As String, ByVal dataRequest As DataSet) As TipoInstrumentoBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoInstrumento_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoClaseInstrumento", DbType.String, codigoClaseInstrumento)
        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, TipoRenta)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        Dim oTipoInstrumentoBE As New TipoInstrumentoBE
        db.LoadDataSet(dbCommand, oTipoInstrumentoBE, "TipoInstrumento")
        Return oTipoInstrumentoBE

    End Function
    Public Function SeleccionarPorFiltroValores(ByVal codigo As String, ByVal codigoSBS As String, ByVal codigoTipoRenta As String, ByVal dataRequest As DataSet) As TipoInstrumentoBE
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("TipoInstrumento_SeleccionarPorFiltroValores")

            db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, codigo)
            db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumentoSBS", DbType.String, codigoSBS)
            db.AddInParameter(dbCommand, "@p_CodigoTipoRenta", DbType.String, codigoTipoRenta)
            Dim oTipoInstrumentoBE As New TipoInstrumentoBE
            db.LoadDataSet(dbCommand, oTipoInstrumentoBE, "TipoInstrumento")
            Return oTipoInstrumentoBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SeleccionarPorFiltroValores(ByVal codigoTipoRenta As String, ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoInstrumento_SeleccionarPorFiltroValores")
        Dim ds As New DataSet

        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, "")
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumentoSBS", DbType.String, "")
        db.AddInParameter(dbCommand, "@p_CodigoTipoRenta", DbType.String, codigoTipoRenta)

        db.LoadDataSet(dbCommand, ds, "TipoInstrumento")
        Return ds

    End Function
    Public Function SeleccionarPorCodigoyDescripcion(ByVal codigo As String, ByVal Descripcion As String, ByVal dataRequest As DataSet) As TipoInstrumentoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoInstrumento_SeleccionarPorCodigoyDescripcion")

        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, codigo)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, Descripcion)

        Dim oTipoInstrumentoBE As New TipoInstrumentoBE
        db.LoadDataSet(dbCommand, oTipoInstrumentoBE, "TipoInstrumento")

        Return oTipoInstrumentoBE
    End Function

    Public Function Seleccionar(ByVal codigoTipoInstrumento As String, ByVal dataRequest As DataSet) As TipoInstrumentoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoInstrumento_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, codigoTipoInstrumento)
        Dim oTipoInstrumentoBE As New TipoInstrumentoBE
        db.LoadDataSet(dbCommand, oTipoInstrumentoBE, "TipoInstrumento")
        Return oTipoInstrumentoBE

    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoInstrumento_Listar")

        Dim oTipoInstrumentoBE As New DataSet
        db.LoadDataSet(dbCommand, oTipoInstrumentoBE, "TipoInstrumento")
        Return oTipoInstrumentoBE

    End Function

    Public Function Listar_PorTipoRenta(ByVal TipoRenta As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoInstrumento_ListarPorTipoRenta")

        Dim dsTipoInstrumento As New DataSet
        db.AddInParameter(dbCommand, "@p_CodigoTipoRenta", DbType.String, TipoRenta)
        db.LoadDataSet(dbCommand, dsTipoInstrumento, "TipoInstrumento")
        Return dsTipoInstrumento
    End Function
    Public Function Validar_TipoInstrumento(ByVal TipoInstrumento As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoInstrumento_Valida")

        'Dim sValor As String
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, TipoInstrumento)
        Return Convert.ToString(db.ExecuteScalar(dbCommand))
        'Return sValor
    End Function
    Public Function VerificarExistenciaSBS_Nuevo(ByVal tipoInstrumento As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoInstrumento_ValidaCodigoSBS_Nuevo")
        Dim resul As Boolean

        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, tipoInstrumento)
        If CType(db.ExecuteScalar(dbCommand), String) = "0" Then
            Return False
        Else
            Return True
        End If
    End Function
    Public Function VerificarExistenciaSBS_Modificar(ByVal TipoInstrumentoSBS As String, ByVal TipoInstrumento As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoInstrumento_ValidaCodigoSBS_Modificar")
        Dim resul As Boolean
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumentoSBS", DbType.String, TipoInstrumentoSBS)
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, TipoInstrumento)
        If CType(db.ExecuteScalar(dbCommand), String) = "0" Then
            Return False
        Else
            Return True
        End If
    End Function

#End Region

#Region " /* Funciones Insertar */ "

    Public Function Insertar(ByVal oTipoInstrumentoBE As TipoInstrumentoBE, ByVal oTipoInstrumentoCuentaBCRBE As TipoInstrumentoCuentaBCRBE _
       , ByVal dataRequest As DataSet) As String
        'Dim Codigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoInstrumento_Insertar")

        oRow = CType(oTipoInstrumentoBE.TipoInstrumento.Rows(0), TipoInstrumentoBE.TipoInstrumentoRow)

        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, oRow.CodigoTipoInstrumento)
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumentoSBS", DbType.String, oRow.CodigoTipoInstrumentoSBS)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_PlazoLiquidacion", DbType.String, oRow.PlazoLiquidacion)
        db.AddInParameter(dbCommand, "@p_TipoTasa", DbType.String, oRow.TipoTasa)
        db.AddInParameter(dbCommand, "@p_TasaEncaje", DbType.Decimal, oRow.TasaEncaje)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, oRow.CodigoRenta)
        db.AddInParameter(dbCommand, "@p_CodigoClaseInstrumento", DbType.String, oRow.CodigoClaseInstrumento)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_CodigoTipoValorizacion", DbType.String, oRow.CodigoTipoValorizacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_GrupoRiesgo", DbType.String, oRow.GrupoRiesgo) 'HDG OT 58687 20100302
        db.AddInParameter(dbCommand, "@p_PND", DbType.Decimal, oRow.PND) 'JRM 20100427
        db.AddInParameter(dbCommand, "@p_PNND", DbType.Decimal, oRow.PNND) 'JRM 20100427
        db.AddInParameter(dbCommand, "@p_MetodoCalculoRenta", DbType.String, oRow.MetodoCalculoRenta) 'RGF 20101229 OT 61897
        db.ExecuteNonQuery(dbCommand)

        Dim oTipoInstrumentoCuentaBCRDAM As New TipoInstrumentoCuentaBCRDAM
        oTipoInstrumentoCuentaBCRDAM.Insertar(oTipoInstrumentoCuentaBCRBE)
        'Return Codigo

    End Function

#End Region

#Region " /* Funciones Modificar */"

    Public Function Modificar(ByVal oTipoInstrumentoBE As TipoInstrumentoBE, ByVal oTipoInstrumentoCuentaBCRBE As TipoInstrumentoCuentaBCRBE _
   , ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoInstrumento_Modificar")
        oRow = CType(oTipoInstrumentoBE.TipoInstrumento.Rows(0), TipoInstrumentoBE.TipoInstrumentoRow)

        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, oRow.CodigoTipoInstrumento)
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumentoSBS", DbType.String, oRow.CodigoTipoInstrumentoSBS)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_PlazoLiquidacion", DbType.String, oRow.PlazoLiquidacion)
        db.AddInParameter(dbCommand, "@p_TipoTasa", DbType.String, oRow.TipoTasa)
        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, oRow.CodigoRenta)
        db.AddInParameter(dbCommand, "@p_CodigoClaseInstrumento", DbType.String, oRow.CodigoClaseInstrumento)
        db.AddInParameter(dbCommand, "@p_TasaEncaje", DbType.Decimal, oRow.TasaEncaje)
        db.AddInParameter(dbCommand, "@p_CodigoTipoValorizacion", DbType.String, oRow.CodigoTipoValorizacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_GrupoRiesgo", DbType.String, oRow.GrupoRiesgo) 'HDG OT 58687 20100302
        db.AddInParameter(dbCommand, "@p_PND", DbType.Decimal, oRow.PND) 'JRM 20100427
        db.AddInParameter(dbCommand, "@p_PNND", DbType.Decimal, oRow.PNND) 'JRM 20100427
        db.AddInParameter(dbCommand, "@p_MetodoCalculoRenta", DbType.String, oRow.MetodoCalculoRenta) 'RGF 20101229 OT 61897
        db.ExecuteNonQuery(dbCommand)

        Dim oTipoInstrumentoCuentaBCRDAM As New TipoInstrumentoCuentaBCRDAM
        oTipoInstrumentoCuentaBCRDAM.Modificar(oTipoInstrumentoCuentaBCRBE)
        Return True

    End Function



#End Region

#Region " /* Funciones Eliminar */"

    Public Function Eliminar(ByVal codigoTipoInstrumento As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoInstrumento_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, codigoTipoInstrumento)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

#End Region

#Region " /* Funciones Personalizadas*/"

#End Region

    Public Function ListarTipoInstrumento(ByVal strCodigoClaseInstrumento As String, ByVal dataRequest As DataSet) As TipoInstrumentoBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ClaseInstrumento_ListarTipoInstrumento")

        db.AddInParameter(dbCommand, "@p_CodigoClaseInstrumento", DbType.String, strCodigoClaseInstrumento)

        Dim oTipoInstrumentoBE As New TipoInstrumentoBE
        db.LoadDataSet(dbCommand, oTipoInstrumentoBE, "TipoInstrumento")
        Return oTipoInstrumentoBE

    End Function

    Public Function ListarTipoInstrumentoPorCategoria(ByVal strCategoria As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ClaseInstrumento_ListarTipoInstrumentoPorCategoria")
        db.AddInParameter(dbCommand, "@p_Categoria", DbType.String, strCategoria)
        Return db.ExecuteDataSet(dbCommand)
    End Function
End Class

