Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

	Public class CotizacionVACDAM
    Private sqlCommand As String = ""
    Private oRow As CotizacionVACBE.CotizacionVACRow
		Public Sub New()

		End Sub

#Region " /* Funciones Seleccionar */ "

    Public Function SeleccionarPorFiltro(ByVal fecha As Decimal, ByVal codigoIndicador As String) As CotizacionVACBE

        Dim obj As New CotizacionVACBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CotizacionVAC_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_Fecha", DbType.String, fecha)
        db.AddInParameter(dbCommand, "@p_CodigoIndicador", DbType.String, codigoIndicador)

        db.LoadDataSet(dbCommand, obj, "CotizacionVAC")

        Return obj

    End Function

    Public Function Seleccionar(ByVal fecha As Decimal, ByVal codigoIndicador As String) As CotizacionVACBE

        Dim oCotizacionVACBE As New CotizacionVACBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CotizacionVAC_Seleccionar")

        db.AddInParameter(dbCommand, "@p_Fecha", DbType.String, fecha)
        db.AddInParameter(dbCommand, "@p_CodigoIndicador", DbType.String, codigoIndicador)

        db.LoadDataSet(dbCommand, oCotizacionVACBE, "CotizacionVAC")

        Return oCotizacionVACBE

    End Function

    Public Function SeleccionarPorUltimaFecha(ByVal codigoIndicador As String) As CotizacionVACBE

        Dim oCotizacionVACBE As New CotizacionVACBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CotizacionVAC_Seleccionar_UltimaFecha")

        db.AddInParameter(dbCommand, "@p_CodigoIndicador", DbType.String, codigoIndicador)

        db.LoadDataSet(dbCommand, oCotizacionVACBE, "CotizacionVAC")

        Return oCotizacionVACBE

    End Function

    Public Function SeleccionarDetalle(ByVal codigoCotizacionVAC As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CotizacionVAC_SeleccionarDetalle")

        db.AddInParameter(dbCommand, "@p_CodigoCotizacionVAC", DbType.String, codigoCotizacionVAC)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As CotizacionVACBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CotizacionVAC_Listar")

        Dim oCotizacionVACBE As New CotizacionVACBE
        db.LoadDataSet(dbCommand, oCotizacionVACBE, "CotizacionVAC")
        Return oCotizacionVACBE

    End Function

#End Region

#Region " /* Funciones Insertar */ "

    Public Function Insertar(ByVal oCotizacionVACBE As CotizacionVACBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand(" ")
        For Each row As DataRow In oCotizacionVACBE.CotizacionVAC.Rows

            db.AddInParameter(dbCommand, "@p_CodigoIndicador", DbType.String, row("CodigoIndicador"))
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, row("Fecha"))
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, IIf(row("CodigoPortafolioSBS") = "", Convert.DBNull, row("CodigoPortafolioSBS")))
            db.AddInParameter(dbCommand, "@p_DiasPeriodo", DbType.Decimal, IIf(Convert.ToDecimal(row("DiasPeriodo")) = -1, Convert.DBNull, row("DiasPeriodo")))
            db.AddInParameter(dbCommand, "@p_Valor", DbType.Decimal, row("Valor"))
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, row("Situacion"))

            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        Next

        Return True
        'Dim intIndice, intNroFilas As Integer

        'intNroFilas = oCotizacionVACBE.CotizacionVAC.Rows.Count
        'For intIndice = 0 To intNroFilas - 1

        '    oCotizacionVACBE.CotizacionVAC.Rows(intIndice)("UsuarioCreacion") = DataUtility.ObtenerValorRequest(dataRequest, "Usuario")
        '    oCotizacionVACBE.CotizacionVAC.Rows(intIndice)("FechaCreacion") = DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha"))
        '    oCotizacionVACBE.CotizacionVAC.Rows(intIndice)("HoraCreacion") = DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha"))
        '    oCotizacionVACBE.CotizacionVAC.Rows(intIndice)("Host") = DataUtility.ObtenerValorRequest(dataRequest, "Host")

        'Next

        'db.AddInParameter(dbCommand, "@p_XmlCotizacion", DbType.String, oCotizacionVACBE.GetXml().Replace("<CotizacionVACBE xmlns=""http://tempuri.org/CotizacionVACBE.xsd"">", "<CotizacionVACBE>"))

        'db.ExecuteNonQuery(dbCommand)

        'Return True

    End Function

#End Region

#Region " /* Funciones Modificar */"

    Public Function Modificar(ByVal CodigoIndicador As String, ByVal oCotizacionVACBE As CotizacionVACBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand

        'Dim intIndice, intNroFilas As Integer

        'intNroFilas = oCotizacionVACBE.CotizacionVAC.Rows.Count
        'For intIndice = 0 To intNroFilas - 1
        For Each row As DataRow In oCotizacionVACBE.CotizacionVAC.Rows
            If row("DiasPeriodo") Is Convert.DBNull Then
                row("DiasPeriodo") = -1
            End If
            If row("CodigoIndicador").ToString = "Nuevo" Then
                dbCommand = db.GetStoredProcCommand("CotizacionVAC_Insertar")
                db.AddInParameter(dbCommand, "@p_CodigoIndicador", DbType.String, CodigoIndicador)
                db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, row("Fecha"))
                db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, IIf(row("CodigoPortafolioSBS") = "", Convert.DBNull, row("CodigoPortafolioSBS")))
                db.AddInParameter(dbCommand, "@p_DiasPeriodo", DbType.Decimal, IIf(Convert.ToDecimal(row("DiasPeriodo")) = -1, Convert.DBNull, row("DiasPeriodo")))
                db.AddInParameter(dbCommand, "@p_Valor", DbType.Decimal, row("Valor"))
                db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, row("Situacion"))

                db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
                db.ExecuteNonQuery(dbCommand)
            Else
                dbCommand = db.GetStoredProcCommand("CotizacionVAC_Modificar")
                db.AddInParameter(dbCommand, "@p_Secuencia", DbType.Decimal, row("Secuencia"))
                'db.AddInParameter(dbCommand, "@p_CodigoIndicador", DbType.String, row("CodigoIndicador"))
                db.AddInParameter(dbCommand, "@p_CodigoIndicador", DbType.String, CodigoIndicador)
                db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, row("Fecha"))
                'HDG OT 59304 20100419
                'db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, IIf(row("CodigoPortafolioSBS") = "", Convert.DBNull, row("CodigoPortafolioSBS")))
                If row("CodigoPortafolioSBS") Is System.DBNull.Value Then
                    db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, "")
                Else
                    db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, row("CodigoPortafolioSBS"))
                End If
                db.AddInParameter(dbCommand, "@p_DiasPeriodo", DbType.Decimal, IIf(row("DiasPeriodo") = -1, Convert.DBNull, row("DiasPeriodo")))
                db.AddInParameter(dbCommand, "@p_Valor", DbType.Decimal, row("Valor"))
                db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, row("Situacion"))

                db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
                db.ExecuteNonQuery(dbCommand)
            End If


        Next

        Return True

    End Function

#End Region

#Region " /* Funciones Eliminar */"

    Public Function Eliminar(ByVal fecha As Decimal, ByVal codigoIndicador As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CotizacionVAC_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoIndicador", DbType.String, codigoIndicador)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)

        Return True
    End Function

#End Region

#Region " /* Funciones ActualizarporExcel */"

    'HDG OT 59304 20100419
    Public Function ActualizarCotizacionVACPorExcel(ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet, ByRef strmensaje As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sCodigo As String
        Dim nFecha As Decimal, nValor As Decimal
        Dim sUsuario As String, sHost As String
        Dim nFecCre As Decimal
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_upd_ActualizarPorExcel")

        sUsuario = DataUtility.ObtenerValorRequest(dataRequest, "Usuario")
        nFecCre = DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha"))
        sHost = DataUtility.ObtenerValorRequest(dataRequest, "Host")

        db.AddInParameter(dbCommand, "@p_CodigoIndicador", DbType.String)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_Valor", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String)

        For Each filaLinea As DataRow In dtDetalle.Rows

            sCodigo = filaLinea(0).ToString().Trim()
            nFecha = DataUtility.ObtenerFecha(filaLinea(1).ToString().Trim())
            nValor = filaLinea(2).ToString().Trim()

            If (Not sCodigo.Equals("")) And _
                (Not nFecha.ToString.Equals("0")) And _
                (Not nValor.ToString.Equals("0")) Then

                db.SetParameterValue(dbCommand, "@p_CodigoIndicador", sCodigo)
                db.SetParameterValue(dbCommand, "@p_Fecha", nFecha)
                db.SetParameterValue(dbCommand, "@p_Valor", nValor)
                db.SetParameterValue(dbCommand, "@p_UsuarioCreacion", sUsuario)
                db.SetParameterValue(dbCommand, "@p_FechaCreacion", nFecCre)
                db.SetParameterValue(dbCommand, "@p_HoraCreacion", DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.SetParameterValue(dbCommand, "@p_Host", sHost)
                db.ExecuteNonQuery(dbCommand)

            End If
        Next
        strmensaje &= "Los datos de los Indicadores se cargaron correctamente\n"
        Return True
    End Function
#End Region

#Region " /* Funciones Personalizadas*/"
    Public Sub InsertarDetalleAux(ByVal CodigoCotizacionVAC As String, ByVal fecha As String, ByVal valorVAC As String, ByVal Situacion As String, ByVal dataRequest As DataSet)


        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CotizacionVAC_InsertarDetalle")

        db.AddInParameter(dbCommand, "@p_CodigoCotizacionVAC", DbType.String, CodigoCotizacionVAC)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, Convert.ToDecimal(fecha))
        db.AddInParameter(dbCommand, "@p_ValorCotizacionVAC", DbType.Decimal, Convert.ToDecimal(valorVAC))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)


    End Sub
    Public Sub ModificarDetalleAux(ByVal CodigoCotizacionVAC As String, ByVal CodigoDetalleCotizacionVAC As String, ByVal fecha As String, ByVal valorVAC As String, ByVal Situacion As String, ByVal dataRequest As DataSet)


        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CotizacionVAC_ModificarDetalle")

        db.AddInParameter(dbCommand, "@p_CodigoCotizacionVAC", DbType.String, CodigoCotizacionVAC)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, Convert.ToDecimal(fecha))
        db.AddInParameter(dbCommand, "@p_ValorCotizacionVAC", DbType.Decimal, Convert.ToDecimal(valorVAC))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Sub EliminarDetalle(ByVal codFila As String, ByVal CodCotizacion As String, ByVal dataRequest As DataSet)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CotizacionVAC_EliminarDetalle")

        db.AddInParameter(dbCommand, "@p_CodigoCotizacionVAC", DbType.String, CodCotizacion)
        db.AddInParameter(dbCommand, "@p_CodigoDetalleCotizacionVAC", DbType.String, codFila)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)

    End Sub
    Public Function ObtenerCodigoCotizacionVAC(ByVal codigoMes As String, ByVal CodigoAnio As String, ByVal dataRequest As DataSet) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CotizacionVAC_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_Mes", DbType.String, codigoMes)
        db.AddInParameter(dbCommand, "@p_Anio", DbType.String, CodigoAnio)



        Dim codigoCotizacion As String
        codigoCotizacion = db.ExecuteScalar(dbCommand)
        Return codigoCotizacion

    End Function
#End Region

End Class