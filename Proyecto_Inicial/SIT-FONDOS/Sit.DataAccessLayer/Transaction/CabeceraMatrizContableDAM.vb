Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports Sit.BusinessEntities
Public Class CabeceraMatrizContableDAM
    Private sqlCommand As String = ""
    Public oCabeceraMatrizContableRow As CabeceraMatrizContableBE.CabeceraMatrizContableRow
    Public Sub New()
    End Sub
    Public Function Seleccionar(ByVal strCodigoMatriz As String, ByVal dataRequest As DataSet) As CabeceraMatrizContableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CabeceraMatrizContable_Seleccionar")
        db.AddInParameter(dbCommand, "@p_CodigoMatrizContable", DbType.Int16, strCodigoMatriz)
        Dim objeto As New CabeceraMatrizContableBE
        db.LoadDataSet(dbCommand, objeto, "CabeceraMatrizContable")
        Return objeto
    End Function
    Public Function SeleccionarPorFiltro(ByVal ob As CabeceraMatrizContableBE, ByVal MatrizFondo As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CabeceraMatrizContable_SeleccionarPorFiltro")
        oCabeceraMatrizContableRow = CType(ob.CabeceraMatrizContable.Rows(0), CabeceraMatrizContableBE.CabeceraMatrizContableRow)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, oCabeceraMatrizContableRow.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oCabeceraMatrizContableRow.CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoClaseInstrumento", DbType.String, oCabeceraMatrizContableRow.CodigoClaseInstrumento)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, oCabeceraMatrizContableRow.CodigoOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, oCabeceraMatrizContableRow.CodigoTipoInstrumento)
        db.AddInParameter(dbCommand, "@p_CodigoModalidadPago", DbType.String, oCabeceraMatrizContableRow.CodigoModalidadPago)
        db.AddInParameter(dbCommand, "@p_CodigoSectorEmpresarial", DbType.String, oCabeceraMatrizContableRow.CodigoSectorEmpresarial)
        db.AddInParameter(dbCommand, "@p_CodigoMatrizContable", DbType.Decimal, IIf(oCabeceraMatrizContableRow.CodigoMatrizContable = "", "0", oCabeceraMatrizContableRow.CodigoMatrizContable))
        db.AddInParameter(dbCommand, "@p_NumeroCuentaIngreso", DbType.String, oCabeceraMatrizContableRow.NumeroCuentaIngreso)
        db.AddInParameter(dbCommand, "@p_CodigoSBSBanco", DbType.String, oCabeceraMatrizContableRow.CodigoSBSBanco)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oCabeceraMatrizContableRow.Situacion)
        db.AddInParameter(dbCommand, "@p_CodigoCabeceraMatriz", DbType.Decimal, oCabeceraMatrizContableRow.CodigoCabeceraMatriz)
        db.AddInParameter(dbCommand, "@p_MatrizFondo", DbType.String, MatrizFondo)
        db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, oCabeceraMatrizContableRow.CodigoNegocio)
        db.AddInParameter(dbCommand, "@p_CodigoSerie", DbType.String, oCabeceraMatrizContableRow.CodigoSerie)
        Dim objeto As New DataSet
        db.LoadDataSet(dbCommand, objeto, "CabeceraMatrizContable")
        Return objeto
    End Function
    Public Function Listar(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CabeceraMatrizContable_Listar")
        Dim objeto As New CabeceraMatrizContableBE
        db.LoadDataSet(dbCommand, objeto, "CabeceraMatrizContable")
        Return objeto
    End Function
    Public Function Insertar_1(ByVal ob As CabeceraMatrizContableBE, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CabeceraMatrizContable_Insertar_1")
        oCabeceraMatrizContableRow = CType(ob.CabeceraMatrizContable.Rows(0), CabeceraMatrizContableBE.CabeceraMatrizContableRow)
        Dim strCodigoMatrizContable As String
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oCabeceraMatrizContableRow.CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoClaseInstrumento", DbType.String, oCabeceraMatrizContableRow.CodigoClaseInstrumento)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, oCabeceraMatrizContableRow.CodigoOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, oCabeceraMatrizContableRow.CodigoTipoInstrumento)
        db.AddInParameter(dbCommand, "@p_CodigoModalidadPago", DbType.String, oCabeceraMatrizContableRow.CodigoModalidadPago)
        db.AddInParameter(dbCommand, "@p_CodigoSectorEmpresarial", DbType.String, oCabeceraMatrizContableRow.CodigoSectorEmpresarial)
        db.AddInParameter(dbCommand, "@p_CodigoMatrizContable", DbType.String, oCabeceraMatrizContableRow.CodigoMatrizContable)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oCabeceraMatrizContableRow.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_NumeroCuentaIngreso", DbType.String, oCabeceraMatrizContableRow.NumeroCuentaIngreso)
        db.AddInParameter(dbCommand, "@p_CodigoSBSBanco", DbType.String, oCabeceraMatrizContableRow.CodigoSBSBanco)
        db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, oCabeceraMatrizContableRow.CodigoNegocio)
        db.AddInParameter(dbCommand, "@p_CodigoSerie", DbType.String, oCabeceraMatrizContableRow.CodigoSerie)
        strCodigoMatrizContable = db.ExecuteScalar(dbCommand)
        Return strCodigoMatrizContable
    End Function
    Public Function Modificar(ByVal ob As CabeceraMatrizContableBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CabeceraMatrizContable_Modificar")
        oCabeceraMatrizContableRow = CType(ob.CabeceraMatrizContable.Rows(0), CabeceraMatrizContableBE.CabeceraMatrizContableRow)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oCabeceraMatrizContableRow.CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoClaseInstrumento", DbType.String, oCabeceraMatrizContableRow.CodigoClaseInstrumento)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, oCabeceraMatrizContableRow.CodigoOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, oCabeceraMatrizContableRow.CodigoTipoInstrumento)
        db.AddInParameter(dbCommand, "@p_CodigoModalidadPago", DbType.String, oCabeceraMatrizContableRow.CodigoModalidadPago)
        db.AddInParameter(dbCommand, "@p_CodigoSectorEmpresarial", DbType.String, oCabeceraMatrizContableRow.CodigoSectorEmpresarial)
        db.AddInParameter(dbCommand, "@p_CodigoCabeceramatriz", DbType.String, oCabeceraMatrizContableRow.CodigoCabeceraMatriz)
        db.AddInParameter(dbCommand, "@p_CodigoMatrizContable", DbType.String, oCabeceraMatrizContableRow.CodigoMatrizContable)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_NumeroCuentaIngreso", DbType.String, oCabeceraMatrizContableRow.NumeroCuentaIngreso)
        db.AddInParameter(dbCommand, "@p_CodigoSBSBanco", DbType.String, oCabeceraMatrizContableRow.CodigoSBSBanco)
        db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, oCabeceraMatrizContableRow.CodigoNegocio)
        db.AddInParameter(dbCommand, "@p_CodigoSerie", DbType.String, oCabeceraMatrizContableRow.CodigoSerie)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function Eliminar(ByVal codigoMatrizContable As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("CabeceraMatrizContable_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoMatrizContable", DbType.String, codigoMatrizContable)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function ValidaExistenciaMatrizFondo(CodigoMatrizContable As Decimal, ByVal CodigoNegocio As String, CodigoSerie As String) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ValidaExistenciaMatrizFondo")
        db.AddInParameter(dbCommand, "@p_CodigoMatrizContable", DbType.Decimal, CodigoMatrizContable)
        db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, CodigoNegocio)
        db.AddInParameter(dbCommand, "@p_CodigoSerie", DbType.String, CodigoSerie)
        db.AddOutParameter(dbCommand, "@p_Existe", DbType.Decimal, 1)
        db.ExecuteDataSet(dbCommand)
        Return CType(db.GetParameterValue(dbCommand, "@p_Existe"), Integer)
    End Function
End Class