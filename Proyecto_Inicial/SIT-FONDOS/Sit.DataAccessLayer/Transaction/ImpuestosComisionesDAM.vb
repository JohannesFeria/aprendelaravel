Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

	''' <summary>
	''' Clase para el acceso de los datos para ImpuestosComisiones tabla.
	''' </summary>
	Public class ImpuestosComisionesDAM
        Private sqlCommand As String = ""
    Private oRow As ImpuestosComisionesBE.ImpuestosComisionesRow

    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "

    Public Function SeleccionarPorFiltro(ByVal codigoImpuestosComisiones As String, ByVal descripcion As String, ByVal dataRequest As DataSet) As ImpuestosComisionesBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ImpuestosComisiones_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoImpuestosComisiones", DbType.String, codigoImpuestosComisiones)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)



        Dim oImpuestosComisionesBE As New ImpuestosComisionesBE
        db.LoadDataSet(dbCommand, oImpuestosComisionesBE, "ImpuestosComisiones")
        Return oImpuestosComisionesBE

    End Function

    Public Function SeleccionarPorFiltro(ByVal descripcion As String, ByVal tipoRenta As String, ByVal mercado As String, ByVal situacion As String, ByVal dataRequest As DataSet) As ImpuestosComisionesBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ImpuestosComisiones_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoPlaza", DbType.String, mercado)
        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, tipoRenta)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)



        Dim oImpuestosComisionesBE As New ImpuestosComisionesBE
        db.LoadDataSet(dbCommand, oImpuestosComisionesBE, "ImpuestosComisiones")
        Return oImpuestosComisionesBE

    End Function

    Public Function Seleccionar(ByVal codigoComision As String, ByVal codigoMercado As String, ByVal codigoTipoRenta As String, ByVal dataRequest As DataSet) As ImpuestosComisionesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ImpuestosComisiones_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoComision", DbType.String, codigoComision)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
        db.AddInParameter(dbCommand, "@p_CodigoTipoRenta", DbType.String, codigoTipoRenta)

        Dim oImpuestosComisionesBE As New ImpuestosComisionesBE
        db.LoadDataSet(dbCommand, oImpuestosComisionesBE, "ImpuestosComisiones")
        Return oImpuestosComisionesBE

    End Function

    'Public Function Seleccionar(ByVal codigoTarifa As String, ByVal codigoMercado As String, ByVal codigoRenta As String, ByVal dataRequest As DataSet) As ImpuestosComisionesBE
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim dbCommand As dbCommand = db.GetStoredProcCommand("ImpuestosComisiones_Seleccionar")

    '    db.AddInParameter(dbCommand, "@p_CodigoComision", DbType.String, codigoTarifa)

    '    Dim oImpuestosComisionesBE As New ImpuestosComisionesBE
    '    db.LoadDataSet(dbCommand, oImpuestosComisionesBE, "ImpuestosComisiones")
    '    Return oImpuestosComisionesBE

    'End Function

    Public Function Listar(ByVal dataRequest As DataSet) As ImpuestosComisionesBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ImpuestosComisiones_Listar")

        Dim oImpuestosComisionesBE As New ImpuestosComisionesBE
        db.LoadDataSet(dbCommand, oImpuestosComisionesBE, "ImpuestosComisiones")
        Return oImpuestosComisionesBE

    End Function

#End Region

#Region " /* Funciones Insertar */ "

    Public Function Insertar(ByVal oImpuestosComisionesBE As ImpuestosComisionesBE, ByVal dataRequest As DataSet) As String
        Dim Codigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ImpuestosComisiones_Insertar")

        oRow = DirectCast(oImpuestosComisionesBE.ImpuestosComisiones.Rows(0), ImpuestosComisionesBE.ImpuestosComisionesRow)

        db.AddInParameter(dbCommand, "@p_CodigoComision", DbType.String, oRow.CodigoComision)
        db.AddInParameter(dbCommand, "@p_CodigoPlaza", DbType.String, oRow.CodigoMercado)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_BaseCalculo", DbType.Decimal, oRow.BaseCalculo)
        db.AddInParameter(dbCommand, "@p_TipoTarifa", DbType.String, oRow.CodigoTarifa)
        db.AddInParameter(dbCommand, "@p_EsRentaFija", DbType.String, oRow.CodigoRenta)
        db.AddInParameter(dbCommand, "@p_ValorComision", DbType.Decimal, oRow.ValorComision)
        db.AddInParameter(dbCommand, "@p_Indicador", DbType.String, oRow.Indicador)
	'OT11008 - 23/01/2018 - Carlos Rumiche
	'Descripción: Agregar nuevos parámetros para la creación de la parametría de impuestos y comisiones
        db.AddInParameter(dbCommand, "@p_GeneraImpuestos", DbType.String, oRow.GeneraImpuestos)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oRow.CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return Codigo

    End Function

#End Region

#Region " /* Funciones Modificar */"

    Public Function Modificar(ByVal oImpuestosComisionesBE As ImpuestosComisionesBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ImpuestosComisiones_Modificar")
        oRow = CType(oImpuestosComisionesBE.ImpuestosComisiones.Rows(0), ImpuestosComisionesBE.ImpuestosComisionesRow)

        db.AddInParameter(dbCommand, "@p_CodigoComision", DbType.String, oRow.CodigoComision)
        db.AddInParameter(dbCommand, "@p_CodigoPlaza", DbType.String, oRow.CodigoMercado)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_BaseCalculo", DbType.Decimal, oRow.BaseCalculo)
        db.AddInParameter(dbCommand, "@p_TipoTarifa", DbType.String, oRow.CodigoTarifa)
        db.AddInParameter(dbCommand, "@p_EsRentaFija", DbType.String, oRow.CodigoRenta)
        db.AddInParameter(dbCommand, "@p_ValorComision", DbType.Decimal, oRow.ValorComision)
        db.AddInParameter(dbCommand, "@p_Indicador", DbType.String, oRow.Indicador)
	'OT11008 - 23/01/2018 - Carlos Rumiche
	'Descripción: Agregar nuevos parámetros para la creación de la parametría de impuestos y comisiones
        db.AddInParameter(dbCommand, "@p_GeneraImpuestos", DbType.String, oRow.GeneraImpuestos)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oRow.CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True


    End Function

#End Region

#Region " /* Funciones Eliminar */"

    Public Function Eliminar(ByVal codigoComision As String, ByVal codigoMercado As String, ByVal codigoRenta As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ImpuestosComisiones_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoComision", DbType.String, codigoComision)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
        db.AddInParameter(dbCommand, "@p_CodigoTipoRenta", DbType.String, codigoRenta)

        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True
    End Function
#End Region

#Region " /* Funciones Personalizadas Alberto*/"
    Public Function SeleccionarFiltroDinamico(ByVal CodigoTipoRenta As String, ByVal CodigoMercado As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        'Dim dbCommand As dbCommand = db.GetStoredProcCommand("SeleccionarImpuestosComisiones")
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("SeleccionarImpuestosComisionesTabla")
        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, CodigoTipoRenta)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, CodigoMercado)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function SeleccionarFiltroDinamico_Fijos(ByVal strClasificacion As String, ByVal strMercado As String, ByVal strTipoRenta As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("SeleccionarImpuestosComisiones_Fijos")

        'db.AddInParameter(dbCommand, "@p_Clasificacion", DbType.String, strClasificacion)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, strMercado)
        db.AddInParameter(dbCommand, "@p_CodigoTipoRenta", DbType.String, strTipoRenta)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se añade tres Input: fechaOperacion, codigoMnemonico y codigoTercero a la función para calcular Comisiones para una nueva OI individual: Bonos | 13/06/18 
    Public Function GetImpuestosComisiones(ByVal codigoPreOrden As String, ByVal fechaOperacion As Decimal, ByVal codigoMnemonico As String, ByVal codigoTercero As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("usp_GetImpuestosComisiones") '<< -- Cambiar cuando se publique
        'db.AddInParameter(dbCommand, "@nvcTipoRenta", DbType.String, codigoTipoRenta)
        'db.AddInParameter(dbCommand, "@nvcCodigoMercado", DbType.String, codigoMercado)
        db.AddInParameter(dbCommand, "@nvcCodigoOrden", DbType.String, codigoPreOrden)
        db.AddInParameter(dbCommand, "@p_fechaOperacion", DbType.Decimal, fechaOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, codigoMnemonico)
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se añade tres Input: fechaOperacion, codigoMnemonico y codigoTercero a la función para calcular Comisiones para una nueva OI individual: Bonos | 13/06/18 
#End Region

End Class

