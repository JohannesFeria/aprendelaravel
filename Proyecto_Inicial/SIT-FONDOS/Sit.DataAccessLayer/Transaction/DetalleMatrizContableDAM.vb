Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports Sit.BusinessEntities
Public Class DetalleMatrizContableDAM
    Private sqlCommand As String = ""
    Private strVacio As String = ""
    Private decVacio As String = 0
    Public oDetalleMatrizContable As DetalleMatrizContableBE.DetalleMatrizContableRow

    Public Sub New()

    End Sub

    Public Sub InicializarValores(ByRef oRow As DetalleMatrizContableBE.DetalleMatrizContableRow)

        oRow.CodigoCabeceraMatriz = decVacio
        oRow.CodigoCentroCosto = strVacio
        oRow.CodigoTercero = strVacio
        oRow.DebeHaber = strVacio
        oRow.Glosa = strVacio
        oRow.NumeroCuentaContable = strVacio
        oRow.TipoContabilidad = strVacio
        oRow.Aplicar = strVacio
        oRow.Tercero = strVacio
        oRow.CentroCosto = strVacio
    End Sub
    Public Function Seleccionar(ByVal strCodigoCabeceraMatriz As String, ByVal dataRequest As DataSet) As DetalleMatrizContableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleMatrizContable_Seleccionar")
        db.AddInParameter(dbCommand, "@p_CodigoCabeceraMatriz", DbType.Int16, strCodigoCabeceraMatriz)
        Dim objeto As New DetalleMatrizContableBE



        Dim oDS As New DataSet
        oDS = db.ExecuteDataSet(dbCommand)

        Dim oDetalleMatrizContable As DetalleMatrizContableBE.DetalleMatrizContableRow
        For Each dr As DataRow In oDS.Tables(0).Rows
            oDetalleMatrizContable = CType(objeto.DetalleMatrizContable.NewRow(), DetalleMatrizContableBE.DetalleMatrizContableRow)
            InicializarValores(oDetalleMatrizContable)
            oDetalleMatrizContable.CodigoCabeceraMatriz = IIf(dr("CodigoCabeceraMatriz") Is DBNull.Value, decVacio, dr("CodigoCabeceraMatriz"))
            oDetalleMatrizContable.CodigoCentroCosto = IIf(dr("CodigoCentroCosto") Is DBNull.Value, strVacio, dr("CodigoCentroCosto"))
            oDetalleMatrizContable.CodigoTercero = IIf(dr("CodigoTercero") Is DBNull.Value, strVacio, dr("CodigoTercero"))
            oDetalleMatrizContable.DebeHaber = IIf(dr("DebeHaber") Is DBNull.Value, strVacio, dr("DebeHaber"))
            oDetalleMatrizContable.Glosa = IIf(dr("Glosa") Is DBNull.Value, strVacio, dr("Glosa"))

            'RGF 20080717
            oDetalleMatrizContable.IndicaNroDocumento = IIf(dr("IndicaNroDocumento") Is DBNull.Value, strVacio, dr("IndicaNroDocumento"))
            'RGF 20080820
            oDetalleMatrizContable.Secuencia = dr("Secuencia")

            oDetalleMatrizContable.NumeroCuentaContable = IIf(dr("NumeroCuentaContable") Is DBNull.Value, strVacio, dr("NumeroCuentaContable"))
            oDetalleMatrizContable.TipoContabilidad = IIf(dr("TipoContabilidad") Is DBNull.Value, strVacio, dr("TipoContabilidad"))
            oDetalleMatrizContable.Aplicar = IIf(dr("Aplicar") Is DBNull.Value, strVacio, dr("Aplicar"))
            oDetalleMatrizContable.Tercero = IIf(dr("Tercero") Is DBNull.Value, strVacio, dr("Tercero"))
            oDetalleMatrizContable.CentroCosto = IIf(dr("CentroCosto") Is DBNull.Value, strVacio, dr("CentroCosto"))
            objeto.DetalleMatrizContable.AddDetalleMatrizContableRow(oDetalleMatrizContable)
            objeto.DetalleMatrizContable.AcceptChanges()
        Next

        Return objeto
    End Function
    Public Function SeleccionarReporteMatrizContable(ByVal Fondo As String, ByVal Matriz As String, ByVal CodigoMoneda As String, ByVal CodigoClaseInstrumento As String, ByVal CodigoOperacion As String, ByVal CodigoTipoInstrumento As String, ByVal CodigoModalidadPago As String, ByVal CodigoSectorEmpresarial As String) As DataSet
        Dim nMatriz As Integer = Val(Matriz)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MatrizContable_SeleccionarReporte")

        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, Fondo)
        db.AddInParameter(dbCommand, "@p_CodigoMatriz", DbType.String, nMatriz)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoClaseInstrumento", DbType.String, CodigoClaseInstrumento)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, CodigoOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, CodigoTipoInstrumento)
        db.AddInParameter(dbCommand, "@p_CodigoModalidadPago", DbType.String, CodigoModalidadPago)
        db.AddInParameter(dbCommand, "@p_CodigoSectorEmpresarial", DbType.String, CodigoSectorEmpresarial)

        Return db.ExecuteDataSet(dbCommand)

    End Function

    Public Function Insertar(ByVal ob As DetalleMatrizContableBE, ByVal Portafolio As String, ByVal dataRequest As DataSet) As String
        Dim oRow As DetalleMatrizContableBE.DetalleMatrizContableRow
        For Each oRow In ob.DetalleMatrizContable.Rows
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("DetalleMatrizContable_Insertar")
            oDetalleMatrizContable = CType(oRow, DetalleMatrizContableBE.DetalleMatrizContableRow)
            db.AddInParameter(dbCommand, "@p_CodigoCabeceraMatriz", DbType.Int16, oDetalleMatrizContable.CodigoCabeceraMatriz)
            db.AddInParameter(dbCommand, "@p_DebeHaber", DbType.String, oDetalleMatrizContable.DebeHaber)
            db.AddInParameter(dbCommand, "@p_NumeroCuentaContable", DbType.String, oDetalleMatrizContable.NumeroCuentaContable)
            db.AddInParameter(dbCommand, "@p_TipoContabilidad", DbType.String, oDetalleMatrizContable.TipoContabilidad)
            db.AddInParameter(dbCommand, "@p_Glosa", DbType.String, oDetalleMatrizContable.Glosa)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, IIf(oDetalleMatrizContable.CodigoTercero = "", DBNull.Value, oDetalleMatrizContable.CodigoTercero))
            db.AddInParameter(dbCommand, "@p_Aplicar", DbType.String, oDetalleMatrizContable.Aplicar)
            db.AddInParameter(dbCommand, "@p_CodigoCentroCosto", DbType.String, oDetalleMatrizContable.CodigoCentroCosto)
            db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, Portafolio)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

            'RGF 20080717
            db.AddInParameter(dbCommand, "@p_IndicaNroDocumento", DbType.String, oDetalleMatrizContable.IndicaNroDocumento)

            db.ExecuteNonQuery(dbCommand)
        Next


    End Function

    Public Function Eliminar(ByVal ob As DetalleMatrizContableBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        oDetalleMatrizContable = CType(ob.DetalleMatrizContable.Rows(0), DetalleMatrizContableBE.DetalleMatrizContableRow)
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleMatrizContable_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoCabeceraMatriz", DbType.String, oDetalleMatrizContable.CodigoCabeceraMatriz)
        db.AddInParameter(dbCommand, "@p_NumeroCuentaContable", DbType.String, oDetalleMatrizContable.NumeroCuentaContable)
        db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function Duplicar(ByVal strCodigoDetalleMatriz As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleMatrizContable_Duplicar")
        db.AddInParameter(dbCommand, "@p_CodigoDetalleMatriz", DbType.String, strCodigoDetalleMatriz)
        db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function Modificar(ByVal ob As DetalleMatrizContableBE, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("DetalleMatrizContable_Modificar")
        oDetalleMatrizContable = CType(ob.DetalleMatrizContable.Rows(0), DetalleMatrizContableBE.DetalleMatrizContableRow)
        db.AddInParameter(dbCommand, "@p_CodigoCabeceraMatriz", DbType.Int16, oDetalleMatrizContable.CodigoCabeceraMatriz)
        db.AddInParameter(dbCommand, "@p_DebeHaber", DbType.String, oDetalleMatrizContable.DebeHaber)
        db.AddInParameter(dbCommand, "@p_NumeroCuentaContable", DbType.String, oDetalleMatrizContable.NumeroCuentaContable)
        db.AddInParameter(dbCommand, "@p_TipoContabilidad", DbType.String, oDetalleMatrizContable.TipoContabilidad)
        db.AddInParameter(dbCommand, "@p_Glosa", DbType.String, oDetalleMatrizContable.Glosa)
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, oDetalleMatrizContable.CodigoTercero)
        db.AddInParameter(dbCommand, "@p_Aplicar", DbType.String, oDetalleMatrizContable.Aplicar)
        db.AddInParameter(dbCommand, "@p_CodigoCentroCosto", DbType.String, oDetalleMatrizContable.CodigoCentroCosto)
        db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        'RGF 20080717
        db.AddInParameter(dbCommand, "@p_IndicaNroDocumento", DbType.String, oDetalleMatrizContable.IndicaNroDocumento)
        'RGF 20080820
        db.AddInParameter(dbCommand, "@p_secuencia", DbType.Int32, oDetalleMatrizContable.Secuencia)

        db.ExecuteNonQuery(dbCommand)
    End Function
    'HSP 20151015
    Public Function EliminarDetalle(ByVal sCodigoCabeceraMatriz As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("DetalleMatrizContable_EliminarDetalle")
        db.AddInParameter(dbCommand, "@p_CodigoCabeceraMatriz", DbType.Int32, Convert.ToInt32(sCodigoCabeceraMatriz))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
End Class

