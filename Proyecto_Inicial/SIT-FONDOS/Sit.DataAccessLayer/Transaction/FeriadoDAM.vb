Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

	Public class FeriadoDAM

    Private sqlCommand As String = ""
    Private oFeriadoRow As FeriadoBE.FeriadoRow
		Public Sub New()

		End Sub
#Region " /* Funciones Seleccionar */ "

    Public Function Seleccionar(ByVal codigoFeriado As String) As FeriadoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Feriado_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoFeriado", DbType.String, codigoFeriado)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function SeleccionarPorNumeroAnio(ByVal numeroAnio As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Feriado_SeleccionarPorNumeroAnio")

        db.AddInParameter(dbCommand, "@p_NumeroAnio", DbType.String, numeroAnio)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function SeleccionarPorCodigoPais(ByVal codigoPais As Decimal) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Feriado_SeleccionarPorCodigoPais")

        db.AddInParameter(dbCommand, "@p_CodigoPais", DbType.Decimal, codigoPais)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function Listar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Feriado_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function Seleccionar(ByVal fechaFeriado As Integer, ByVal StrMercado As String, ByVal dataRequest As DataSet) As FeriadoBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Feriado_Seleccionar")

        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fechaFeriado)
        db.AddInParameter(dbCommand, "@p_Mercado", DbType.String, StrMercado)

        Dim objeto As New FeriadoBE
        db.LoadDataSet(dbCommand, objeto, "Feriado")
        Return objeto

    End Function

    Public Function SeleccionarPorFiltro(ByVal anio As Decimal, ByVal situacion As String, ByVal dataRequest As DataSet) As FeriadoBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Feriado_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_Anio", DbType.Decimal, anio)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        Dim objeto As New FeriadoBE
        db.LoadDataSet(dbCommand, objeto, "Feriado")
        Return objeto

    End Function

    Public Function SeleccionarPorFiltro(ByVal fechaFeriado As Integer, ByVal numeroAnio As String, ByVal situacion As String, ByVal dataRequest As DataSet) As FeriadoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Feriado_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_Fecha", DbType.String, fechaFeriado)
        db.AddInParameter(dbCommand, "@p_NumeroAnio", DbType.String, numeroAnio)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        Dim objeto As New FeriadoBE
        db.LoadDataSet(dbCommand, objeto, "Feriado")
        Return objeto

    End Function

#End Region

    Public Function Insertar(ByVal oFeriadoBE As FeriadoBE, ByVal dataRequest As DataSet)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Feriado_Insertar")

        oFeriadoRow = DirectCast(oFeriadoBE.Feriado.Rows(0), FeriadoBE.FeriadoRow)

        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, oFeriadoRow.Fecha)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, oFeriadoRow.CodigoMercado)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oFeriadoRow.Situacion)

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
    End Function

    Public Function Modificar(ByVal oFeriadoBE As FeriadoBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Feriado_Modificar")

        oFeriadoRow = DirectCast(oFeriadoBE.Feriado.Rows(0), FeriadoBE.FeriadoRow)

        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, oFeriadoRow.Fecha)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oFeriadoRow.Situacion)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, oFeriadoRow.CodigoMercado)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Eliminar(ByVal fecha As Integer, ByVal Mercado As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Feriado_Eliminar")

        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_Mercado", DbType.String, Mercado)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function VerificaDia(ByVal fecha As Decimal, ByVal Mercado As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim _dia As Boolean
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("usp_VerificarDia")
        db.AddInParameter(dbCommand, "@dtFecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@dtMercado", DbType.String, Mercado)
        _dia = db.ExecuteScalar(dbCommand)
        Return _dia
    End Function
#Region " /* Funciones Eliminar */ "
    Public Function EliminarPorNumeroAnio(ByVal numeroAnio As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Feriado_EliminarPorNumeroAnio")

        db.AddInParameter(dbCommand, "@p_NumeroAnio", DbType.String, numeroAnio)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function EliminarPorCodigoPais(ByVal codigoPais As Decimal) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Feriado_EliminarPorCodigoPais")

        db.AddInParameter(dbCommand, "@p_CodigoPais", DbType.Decimal, codigoPais)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
#End Region

#Region " /* Funciones Alberto */ "
    Public Function BuscarPorFecha(ByVal fecha As Decimal, Optional ByVal sInd As String = "") As Boolean   'HDG 20130730
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Feriado_ValidaFecha")

        db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_Ind", DbType.String, sInd) 'HDG 20130730

        If Convert.ToString(db.ExecuteScalar(dbCommand)) = "0" Then
            Return False
        Else
            Return True
        End If
        Return True
    End Function
#End Region
#Region "Funciones nuevas 02/11/2018"
    Public Function Feriado_ValidarFecha(ByVal fecha As Decimal, ByVal mercado As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Feriado_ValidaFechaporMercado")

        db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_mercado", DbType.String, mercado)

        If Convert.ToString(db.ExecuteScalar(dbCommand)) = "0" Then
            Return False
        Else
            Return True
        End If
        Return True
    End Function
#End Region
End Class

