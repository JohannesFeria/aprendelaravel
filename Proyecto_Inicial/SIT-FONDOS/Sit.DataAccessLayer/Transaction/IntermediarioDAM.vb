Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql


	Public class IntermediarioDAM

    Private sqlCommand As String = ""
    Private oIntermediarioRow As IntermediarioBE.IntermediarioRow
		Public Sub New()

		End Sub
#Region " /* Funciones Seleccionar */ "
    Public Function Seleccionar(ByVal codigoIntermediario As String, ByVal dataRequest As DataSet) As IntermediarioBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Intermediario_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, codigoIntermediario)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function SeleccionarPorCodigoMoneda(ByVal codigoMoneda As String, ByVal dataRequest As DataSet) As IntermediarioBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Intermediario_SeleccionarPorCodigoMoneda")

        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function SeleccionarPorCodigoMercado(ByVal codigoMercado As String, ByVal dataRequest As DataSet) As IntermediarioBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Intermediario_SeleccionarPorCodigoMercado")

        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function SeleccionarPorCodigoCustodio(ByVal codigoCustodio As String, ByVal dataRequest As DataSet) As IntermediarioBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Intermediario_SeleccionarPorCodigoCustodio")

        db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, codigoCustodio)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function SeleccionarPorCodigoPortafolio(ByVal codigoPortafolio As String, ByVal dataRequest As DataSet) As IntermediarioBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Intermediario_SeleccionarPorCodigoPortafolio")

        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As IntermediarioBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Intermediario_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarPorFiltro(ByVal codigoIntermediario As String, _
ByVal tipoIntermediario As String, _
ByVal descripcion As String, _
ByVal codigoMercado As String, _
ByVal codigoTercero As String, _
ByVal codigoPortafolio As String, _
ByVal codigoMoneda As String, _
ByVal numeroCuenta As String, _
ByVal codigoCustodio As String, _
ByVal situacion As String, _
ByVal dataRequest As DataSet) As IntermediarioBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Intermediario_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, codigoIntermediario)
        db.AddInParameter(dbCommand, "@p_TipoIntermediario", DbType.String, tipoIntermediario)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
        db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, numeroCuenta)
        db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, codigoCustodio)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        Dim objeto As New IntermediarioBE
        db.LoadDataSet(dbCommand, objeto, "Intermediario")
        Return objeto
    End Function
#End Region

    Public Function Insertar(ByVal ob As IntermediarioBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Intermediario_Insertar")
        oIntermediarioRow = CType(ob.Intermediario.Rows(0), IntermediarioBE.IntermediarioRow)

        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, oIntermediarioRow.CodigoIntermediario)
        db.AddInParameter(dbCommand, "@p_TipoIntermediario", DbType.String, oIntermediarioRow.TipoIntermediario)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oIntermediarioRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.Decimal, oIntermediarioRow.NumeroCuenta)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oIntermediarioRow.CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, oIntermediarioRow.CodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, oIntermediarioRow.CodigoMercado)
        db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, oIntermediarioRow.CodigoCustodio)

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
    End Function

    Public Function Modificar(ByVal ob As IntermediarioBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Intermediario_Modificar")

        oIntermediarioRow = CType(ob.Intermediario.Rows(0), IntermediarioBE.IntermediarioRow)

        db.AddInParameter(dbCommand, "@p_TipoIntermediario", DbType.String, oIntermediarioRow.TipoIntermediario)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oIntermediarioRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.Decimal, oIntermediarioRow.NumeroCuenta)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oIntermediarioRow.CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, oIntermediarioRow.CodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, oIntermediarioRow.CodigoMercado)
        db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, oIntermediarioRow.CodigoCustodio)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oIntermediarioRow.Situacion)

        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, oIntermediarioRow.CodigoIntermediario)

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Eliminar(ByVal codigoIntermediario As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Intermediario_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, codigoIntermediario)

        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

End Class

