Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class PreLiquidacionDivisasDAM
    Private oDatosOIRow As DatosOrdenInversionBE.DatosOrdenInversionRow
    Public Sub New()

    End Sub

    Public Function ListarPreLiquidacionDivisasImprimir(ByVal strCodigoOrden As String, ByVal strCodModeloCarta As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PreLiquidacionDivisas_ListarImprimir")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, strCodigoOrden)
        db.AddInParameter(dbCommand, "@p_CodModeloCarta", DbType.String, strCodModeloCarta)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function SeleccionarPorFiltro(ByVal dsCuentasPorCobrar As CuentasPorCobrarPagarBE, ByVal fechaIni As Decimal, ByVal fechaFin As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim cuentaPorCobrar As CuentasPorCobrarPagarBE.CuentasPorCobrarPagarRow = dsCuentasPorCobrar.CuentasPorCobrarPagar.Rows(0)
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PreLiquidacionDivisas_SeleccionarPorFiltro")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, cuentaPorCobrar.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, cuentaPorCobrar.CodigoMercado)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, cuentaPorCobrar.CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, cuentaPorCobrar.CodigoOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, cuentaPorCobrar.CodigoTercero)
        db.AddInParameter(dbCommand, "@p_CodigoClaseInstrumento", DbType.String, "")
        db.AddInParameter(dbCommand, "@p_Egreso", DbType.String, cuentaPorCobrar.Egreso)
        db.AddInParameter(dbCommand, "@p_FechaOperacionIni", DbType.Decimal, fechaIni)
        db.AddInParameter(dbCommand, "@p_FechaOperacionFin", DbType.Decimal, fechaFin)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function SeleccionarPorCodigoOrden(ByVal strCodigoOrden As String) As DatosOrdenInversionBE
        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PreLiquidacionDivisas_SeleccionarPorCodigoOrden")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, strCodigoOrden)
        Dim obj As New DatosOrdenInversionBE

        Dim ds As DataSet = db.ExecuteDataSet(dbCommand)
        For Each row As DataRow In ds.Tables(0).Rows
            Dim objR As DatosOrdenInversionBE.DatosOrdenInversionRow
            objR = CType(obj.DatosOrdenInversion.NewRow, DatosOrdenInversionBE.DatosOrdenInversionRow)
            objR.CodigoOrden = row("CodigoOrden")
            objR.CodigoClaseCuenta = row("CodigoClaseCuenta")
            objR.CodigoClaseCuentaDestino = row("CodigoClaseCuentaDestino")
            objR.NumeroCuenta = row("NumeroCuenta")
            objR.NumeroCuentaDestino = row("NumeroCuentaDestino")
            objR.CodigoModelo = row("CodigoModelo")
            objR.BancoOrigen = row("BancoOrigen")
            objR.BancoDestino = row("BancoDestino")
            objR.CodigoContacto = row("CodigoContacto")
            objR.CodigoContactoIntermediario = row("CodigoContactoIntermediario")
            'RGF 20090708
            objR.CodigoModalidadPago = IIf(row("CodigoModalidadPago") Is DBNull.Value, "", row("CodigoModalidadPago"))

            obj.DatosOrdenInversion.Rows.Add(objR)
        Next

        Return obj
    End Function

    Public Function Insertar(ByVal ob As DatosOrdenInversionBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PreLiquidacionDivisas_Ingresar")
        oDatosOIRow = CType(ob.DatosOrdenInversion.Rows(0), DatosOrdenInversionBE.DatosOrdenInversionRow)

        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, oDatosOIRow.CodigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, oDatosOIRow.CodigoClaseCuenta)
        db.AddInParameter(dbCommand, "@p_CodigoClaseCuentaDestino", DbType.String, oDatosOIRow.CodigoClaseCuentaDestino)
        db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, oDatosOIRow.NumeroCuenta)
        db.AddInParameter(dbCommand, "@p_NumeroCuentaDestino", DbType.String, oDatosOIRow.NumeroCuentaDestino)
        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, oDatosOIRow.CodigoModelo)
        db.AddInParameter(dbCommand, "@p_BancoOrigen", DbType.String, oDatosOIRow.BancoOrigen)
        db.AddInParameter(dbCommand, "@p_BancoDestino", DbType.String, oDatosOIRow.BancoDestino)
        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, oDatosOIRow.CodigoContacto)
        db.AddInParameter(dbCommand, "@p_CodigoContactoIntermediario", DbType.String, oDatosOIRow.CodigoContactoIntermediario)
        db.AddInParameter(dbCommand, "@p_CodigoModalidadPago", DbType.String, oDatosOIRow.CodigoModalidadPago) 'RGF 20090708
        'db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        'db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        'db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        'db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Modificar(ByVal ob As DatosOrdenInversionBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PreLiquidacionDivisas_Modificar")
        oDatosOIRow = CType(ob.DatosOrdenInversion.Rows(0), DatosOrdenInversionBE.DatosOrdenInversionRow)

        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, oDatosOIRow.CodigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, oDatosOIRow.CodigoClaseCuenta)
        db.AddInParameter(dbCommand, "@p_CodigoClaseCuentaDestino", DbType.String, oDatosOIRow.CodigoClaseCuentaDestino)
        db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, oDatosOIRow.NumeroCuenta)
        db.AddInParameter(dbCommand, "@p_NumeroCuentaDestino", DbType.String, oDatosOIRow.NumeroCuentaDestino)
        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, oDatosOIRow.CodigoModelo)
        db.AddInParameter(dbCommand, "@p_BancoOrigen", DbType.String, oDatosOIRow.BancoOrigen)
        db.AddInParameter(dbCommand, "@p_BancoDestino", DbType.String, oDatosOIRow.BancoDestino)
        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, oDatosOIRow.CodigoContacto)
        db.AddInParameter(dbCommand, "@p_CodigoContactoIntermediario", DbType.String, oDatosOIRow.CodigoContactoIntermediario)
        db.AddInParameter(dbCommand, "@p_CodigoModalidadPago", DbType.String, oDatosOIRow.CodigoModalidadPago) 'RGF 20090708
        'db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        'db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        'db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        'db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
End Class
