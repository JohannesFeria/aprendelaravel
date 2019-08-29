Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql


Public Class LimiteDAM
    Private sqlCommand As String = ""
    Private oRow As LimiteBE.LimiteRow
    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "


    Public Function SeleccionarPorFiltro(ByVal codigoLimite As String, ByVal sinonimo As String, ByVal situacion As String, ByVal dataRequest As DataSet) As LimiteBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Limite_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, codigoLimite)
        db.AddInParameter(dbCommand, "@p_nombreLimite", DbType.String, sinonimo)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        Dim oLimiteBE As New LimiteBE
        db.LoadDataSet(dbCommand, oLimiteBE, "Limite")
        Return oLimiteBE

    End Function
    Public Function ConsultarLimites(ByVal TipoLimite As String, ByVal Mnemonico As String, ByVal OpcionLimite As String, ByVal Fecha As Decimal, ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Limite_SeleccionarEvaluados")

        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, TipoLimite)
        db.AddInParameter(dbCommand, "@p_OpcionLimite", DbType.String, OpcionLimite)
        db.AddInParameter(dbCommand, "@p_Mnemonico", DbType.String, Mnemonico)
        db.AddInParameter(dbCommand, "@p_FechaReporte", DbType.Decimal, Fecha)


        Return db.ExecuteDataSet(dbCommand)

    End Function


    Public Function SeleccionarPorInstrumento(ByVal codigoNemonico As String, ByVal dataRequest As DataSet) As LimiteBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Limite_SeleccionarPorInstrumento")

        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)

        Dim oLimiteBE As New LimiteBE
        db.LoadDataSet(dbCommand, oLimiteBE, "Limite")
        Return oLimiteBE

    End Function
    Public Function SeleccionarCaracteristicas(ByVal StrCodigoLimite As String, ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Limite_SeleccionarCaracteristicas")

        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, StrCodigoLimite)


        Dim DstTabla As New DataSet
        DstTabla = db.ExecuteDataSet(dbCommand)

        Return DstTabla
    End Function
    Public Function SeleccionarCaracteristicasCompuestas(ByVal StrCodigoLimite As String, ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Limite_SeleccionarCaracteristicasCompuestas")

        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, StrCodigoLimite)


        Dim DstTabla As New DataSet
        DstTabla = db.ExecuteDataSet(dbCommand)

        Return DstTabla
    End Function
    'Public Function SeleccionarPortafolios(ByVal StrCodigoLimite As String, ByVal dataRequest As DataSet) As DataSet

    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim dbCommand As dbCommand = db.GetStoredProcCommand("Limite_SeleccionarPortafolios")

    '    db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, StrCodigoLimite)


    '    Dim DsPortafolios As New DataSet
    '    DsPortafolios = db.ExecuteDataSet(dbCommand)

    '    Return DsPortafolios
    'End Function
    Public Function SeleccionarNegocios(ByVal StrCodigoLimite As String, ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Limite_SeleccionarNegocios")

        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, StrCodigoLimite)


        Dim DsPortafolios As New DataSet
        DsPortafolios = db.ExecuteDataSet(dbCommand)

        Return DsPortafolios
    End Function
    Public Function SeleccionarCaracteristicasNiveles(ByVal StrCodigoLimiteCaracteristica As String, ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Limite_SeleccionarCaracteristicasNiveles")

        db.AddInParameter(dbCommand, "@p_CodigoLimiteCaracteristica", DbType.String, StrCodigoLimiteCaracteristica)

        Dim DstTabla As New DataSet
        DstTabla = db.ExecuteDataSet(dbCommand)

        Return DstTabla
    End Function

    Public Function SeleccionarCaracteristicasDetalleNiveles(ByVal StrCodigoNivelLimite As String, ByVal FlagTipoPorcentaje As String, ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Limite_SeleccionarCaracteristicaDetalleNiveles")

        db.AddInParameter(dbCommand, "@p_CodigoNivelLimite", DbType.String, StrCodigoNivelLimite)
        db.AddInParameter(dbCommand, "@p_FlagTipoPorcentaje", DbType.String, FlagTipoPorcentaje)

        Dim DstTabla As New DataSet
        DstTabla = db.ExecuteDataSet(dbCommand)

        Return DstTabla
    End Function
    Public Function SeleccionarCaracteristicasDetalleNiveles(ByVal StrCodigoNivelLimite As String, ByVal FlagTipoPorcentaje As String, ByVal CodCarcteristica As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Limite_SeleccionarCaracteristicaDetalleNiveles")

        db.AddInParameter(dbCommand, "@p_CodigoNivelLimite", DbType.String, StrCodigoNivelLimite)
        db.AddInParameter(dbCommand, "@p_FlagTipoPorcentaje", DbType.String, FlagTipoPorcentaje)
        db.AddInParameter(dbCommand, "@P_CodigoCaracteristica", DbType.String, CodCarcteristica)

        Dim DstTabla As New DataSet
        DstTabla = db.ExecuteDataSet(dbCommand)

        Return DstTabla
    End Function
    Public Function SeleccionarGrupoNivel(ByVal StrCodigoGrupoInstrumento As String, ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Limite_SeleccionarGrupoNivel")

        db.AddInParameter(dbCommand, "@p_CodigoGrupoInstrumento", DbType.String, StrCodigoGrupoInstrumento)

        Dim DstTabla As New DataSet
        DstTabla = db.ExecuteDataSet(dbCommand)

        Return DstTabla
    End Function

    Public Function SeleccionarCaracteristicaGrupo(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Limite_SeleccionarCaracteristicaGrupo")
        Dim DstTabla As New DataSet
        DstTabla = db.ExecuteDataSet(dbCommand)

        Return DstTabla
    End Function

    Public Function Seleccionar(ByVal codigoLimite As String, ByVal dataRequest As DataSet) As LimiteBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Limite_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, codigoLimite)
        Dim oLimiteBE As New LimiteBE
        Dim oRow As LimiteBE.LimiteRow
        ' db.LoadDataSet(dbCommand, oLimiteBE, "Limite")
        Dim ds As New DataSet
        ds = db.ExecuteDataSet(dbCommand)

        For Each dr As DataRow In ds.Tables(0).Rows
            oRow = CType(oLimiteBE.Limite.NewRow(), LimiteBE.LimiteRow)
            oRow.CodigoLimite = dr("CodigoLimite")
            oRow.NombreLimite = dr("NombreLimite")
            'RGF 20080819
            If dr("MarketShare") Is DBNull.Value Then
                oRow.MarketShare = 0
            Else
                oRow.MarketShare = dr("MarketShare")
            End If

            oRow.TipoCalculo = IIf(dr("TipoCalculo") Is DBNull.Value, "", dr("TipoCalculo"))
            oRow.Tope = IIf(dr("Tope") Is DBNull.Value, "", dr("Tope"))
            oRow.UnidadPosicion = IIf(dr("UnidadPosicion") Is DBNull.Value, "", dr("UnidadPosicion"))
            oRow.ValorBase = IIf(dr("ValorBase") Is DBNull.Value, "", dr("ValorBase"))
            oRow.ClaseLimite = IIf(dr("ClaseLimite") Is DBNull.Value, "", dr("ClaseLimite"))
            'oRow.FactorCastigo = IIf(dr("FactorCastigo") Is DBNull.Value, -1, dr("FactorCastigo"))
            'oRow.FactorCastigo = dr("FactorCastigo") 
            oRow.TipoLimite = IIf(dr("TipoLimite") Is DBNull.Value, "", dr("TipoLimite"))
            oRow.AplicarCastigo = IIf(dr("AplicarCastigo") Is DBNull.Value, "", dr("AplicarCastigo"))
            oRow.TipoFactor = IIf(dr("TipoFactor") Is DBNull.Value, "", dr("TipoFactor"))
            oRow.SaldoBanco = IIf(dr("SaldoBanco") Is DBNull.Value, "", dr("SaldoBanco"))
            oRow.Situacion = dr("Situacion")
            oRow.Posicion = IIf(dr("Posicion") Is DBNull.Value, "", dr("Posicion"))
            oRow.IsAgrupadoPorcentaje = IIf(dr("IsAgrupadoPorcentaje") Is DBNull.Value, False, dr("IsAgrupadoPorcentaje"))
            oRow.ValorAgrupadoPorcentaje = IIf(dr("ValorAgrupadoPorcentaje") Is DBNull.Value, Nothing, dr("ValorAgrupadoPorcentaje"))
            oRow.Cuadrar = IIf(dr("Cuadrar") Is DBNull.Value, 0, dr("Cuadrar"))
            oRow.IsLimiteVinculado = IIf(dr("IsLimiteVinculado") Is DBNull.Value, "N", dr("IsLimiteVinculado"))
            oRow.PorcLimiteVinculado = IIf(dr("PorcLimiteVinculado") Is DBNull.Value, 0, dr("PorcLimiteVinculado"))
            oRow.CastigoRating = IIf(dr("CastigoRating") Is DBNull.Value, 0, dr("CastigoRating"))
            oRow.TieneCastigo = IIf(dr("TieneCastigo") Is DBNull.Value, "N", dr("TieneCastigo"))
            oRow.AplicaForward = IIf(dr("AplicaForward") Is DBNull.Value, "N", dr("AplicaForward"))
            oLimiteBE.Limite.AddLimiteRow(oRow)
            oLimiteBE.Limite.AcceptChanges()
        Next


        Return oLimiteBE

    End Function
    ''' <summary>
    ''' Lista todos los expedientes de LimiteBE tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar(ByVal dataRequest As DataSet) As LimiteBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Limite_Listar")

        Dim oLimiteBE As New LimiteBE
        db.LoadDataSet(dbCommand, oLimiteBE, "Limite")
        Return oLimiteBE

    End Function

    'REQ 66768 JCH 20130201
    Public Function SeleccionarCaracteristicasDetalleEstilo(ByVal codNivelLimite As String, ByVal valCaracter As String, ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_DetalleNivelLimite2_SelCodigoEstilo")

        db.AddInParameter(dbCommand, "@p_CodNivelLimite", DbType.String, codNivelLimite)
        db.AddInParameter(dbCommand, "@p_ValCaracter", DbType.String, valCaracter)

        Dim DstTabla As New DataSet
        DstTabla = db.ExecuteDataSet(dbCommand)

        Return DstTabla
    End Function
#End Region

#Region " /* Funciones Insertar */ "

    Public Function Insertar(ByVal oLimiteBE As LimiteBE, ByVal dataRequest As DataSet) As String
        Dim StrCodigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Limite_Insertar")
            oRow = CType(oLimiteBE.Limite.Rows(0), LimiteBE.LimiteRow)
            db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, oRow.CodigoLimite)
            db.AddInParameter(dbCommand, "@p_NombreLimite", DbType.String, oRow.NombreLimite)
            db.AddInParameter(dbCommand, "@p_Tope", DbType.String, oRow.Tope)
            db.AddInParameter(dbCommand, "@p_TipoCalculo", DbType.String, oRow.TipoCalculo)
            db.AddInParameter(dbCommand, "@p_UnidadPosicion", DbType.String, oRow.UnidadPosicion)
            db.AddInParameter(dbCommand, "@p_ValorBase", DbType.String, oRow.ValorBase)
            db.AddInParameter(dbCommand, "@p_ClaseLimite", DbType.String, oRow.ClaseLimite)
            db.AddInParameter(dbCommand, "@p_AplicarCastigo", DbType.String, oRow.AplicarCastigo)
            'If (oRow.FactorCastigo = -1) Then
            '    db.AddInParameter(dbCommand, "@p_FactorCastigo", DbType.Decimal, Convert.DBNull)
            'Else
            '    db.AddInParameter(dbCommand, "@p_FactorCastigo", DbType.Decimal, oRow.FactorCastigo)
            'End If
            db.AddInParameter(dbCommand, "@p_TipoFactor", DbType.String, oRow.TipoFactor)
            db.AddInParameter(dbCommand, "@p_Tipo", DbType.String, oRow.TipoLimite)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, oRow.CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_SaldoBanco", DbType.String, oRow.SaldoBanco)
            db.AddInParameter(dbCommand, "@p_Posicion", DbType.String, oRow.Posicion)
            'RGF 20080819
            db.AddInParameter(dbCommand, "@p_MarketShare", DbType.Decimal, oRow.MarketShare)

            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            '' INI MPENAL - 13/09/16
            db.AddInParameter(dbCommand, "@p_IsAgrupadoPorcentaje", DbType.Boolean, oRow.IsAgrupadoPorcentaje)
            db.AddInParameter(dbCommand, "@p_ValorAgrupadoPorcentaje", DbType.Decimal, oRow.ValorAgrupadoPorcentaje)
            '' FIN MPENAL - 13/09/16
            db.AddInParameter(dbCommand, "@p_Cuadrar", DbType.Int32, oRow.Cuadrar) ' MPENAL - 22/09/16}
            db.AddInParameter(dbCommand, "@p_IsLimiteVinculado", DbType.String, oRow.IsLimiteVinculado)
            db.AddInParameter(dbCommand, "@p_PorcLimiteVinculado", DbType.Decimal, oRow.PorcLimiteVinculado)
            db.AddInParameter(dbCommand, "@p_CastigoRating", DbType.Int32, oRow.CastigoRating) ' GTueros - 25/10/18
            db.AddInParameter(dbCommand, "@p_TieneCastigo", DbType.String, oRow.TieneCastigo)
            db.AddInParameter(dbCommand, "@p_AplicaForward", DbType.String, oRow.AplicaForward)
            StrCodigo = db.ExecuteScalar(dbCommand)
            Return StrCodigo
        End Using
    End Function
    Public Function InsertarNegocios(ByVal CodigoLimite As String, ByVal CodigoNegocio As String, ByVal Situacion As String, ByVal dataRequest As DataSet) As String
        Dim StrCodigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Limite_InsertarNegocios")
        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, CodigoLimite)
        db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, CodigoNegocio)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        StrCodigo = db.ExecuteScalar(dbCommand)
        Return StrCodigo
    End Function
    Public Function InsertarCaracteristicas(ByVal CodigoLimite As String, ByVal situacion As String, ByVal dtDetalle As DataTable, ByVal ListaCaracteristicas As Hashtable, _
    ByVal ListaDetalleNivelLimite As Hashtable, ByVal dataRequest As DataSet, ByVal codigoFondo As String, ByVal dtblCondiciones As DataTable) As String
        Dim CodigoLimiteCaracteristica As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        For Each filaLinea As DataRow In dtDetalle.Rows
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("Limite_InsertarCaracteristicas")
            Dim CodigoPortafolio As String = filaLinea("CodigoPortafolioSBS").ToString().Trim()
            db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, CodigoLimite)
            db.AddInParameter(dbCommand, "@p_TipoLimite", DbType.String, filaLinea("Tipo").ToString().Trim())
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, filaLinea("Situacion").ToString().Trim())
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            CodigoLimiteCaracteristica = db.ExecuteScalar(dbCommand)
            If Not ListaCaracteristicas Is Nothing Then
                Dim dtt As DataTable
                Dim partClaveDetalle As String
                partClaveDetalle = CodigoPortafolio
                Dim array(ListaCaracteristicas.Count - 1) As Object
                ListaCaracteristicas.CopyTo(array, 0)
                For Each elemento As DictionaryEntry In array
                    If elemento.Key = CodigoPortafolio Then
                        dtt = CType(elemento.Value, DataTable)
                    End If
                Next
                If (Not dtt Is Nothing) Then
                    InsertarCaracteristicasNiveles(CodigoLimiteCaracteristica, situacion, dtt, partClaveDetalle, ListaDetalleNivelLimite, dataRequest, codigoFondo, dtblCondiciones)
                End If
            End If
        Next
        Return CodigoLimiteCaracteristica
    End Function
    Public Function InsertarCaracteristicasNiveles(ByVal CodigoLimiteCaracteristica As String, _
    ByVal situacion As String, ByVal dtDetalle As DataTable, ByVal partClaveDetalle As String, _
        ByVal ListaDetalleNivelLimite As Hashtable, ByVal dataRequest As DataSet, ByVal codigoFondo As String,
        ByVal dtblCondiciones As DataTable) As String
        'Dim CodigoNivelAnterior As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim cont As Integer
        Dim CodigoNivel As String = String.Empty
        Dim Secuencial As String
        cont = 0
        Dim FlagTipoPorcentaje, valor As String
        Dim ValorPorcentaje As Decimal

        For Each filaLinea As DataRow In dtDetalle.Rows
            Secuencial = filaLinea("Secuencial").ToString.Trim
            FlagTipoPorcentaje = filaLinea("FlagTipoPorcentaje").ToString().Trim()
            If FlagTipoPorcentaje = "GENERAL" Then
                FlagTipoPorcentaje = "G"
            ElseIf FlagTipoPorcentaje = "AGRUPACION" Then
                FlagTipoPorcentaje = "A"
            ElseIf FlagTipoPorcentaje = "DETALLE" Then
                FlagTipoPorcentaje = "D"
            End If

            valor = filaLinea("ValorPorcentaje").ToString().Trim()
            ValorPorcentaje = Convert.ToDecimal(valor.Replace(".", Separador))
            'CodigoNivel = cont + 1
            'If (cont <> 0) Then
            '    CodigoNivelAnterior = CodigoNivel
            'End If

            'If situacion = "A" Then
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("Limite_InsertarCaracteristicasNiveles")
            db.AddInParameter(dbCommand, "@p_Secuencial", DbType.String, Secuencial)
            db.AddInParameter(dbCommand, "@p_CodigoLimiteCaracteristica", DbType.String, CodigoLimiteCaracteristica)
            db.AddInParameter(dbCommand, "@p_CodigoCaracteristica", DbType.String, filaLinea("CodigoCaracteristica").ToString().Trim())
            db.AddInParameter(dbCommand, "@p_FlagTipoPorcentaje", DbType.String, FlagTipoPorcentaje)
            'db.AddInParameter(dbCommand, "@p_ValorPorcentaje", DbType.String, ValorPorcentaje)
            db.AddInParameter(dbCommand, "@p_ValorPorcentaje", DbType.String, IIf(ValorPorcentaje = -1, Convert.DBNull, ValorPorcentaje))
            db.AddInParameter(dbCommand, "@p_situacion", DbType.String, situacion)
            'db.AddInParameter(dbCommand, "@p_CodigoNivelAnterior", DbType.String, CodigoNivelAnterior)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

            db.AddInParameter(dbCommand, "@p_TieneLimiteNivel", DbType.String, filaLinea("TieneLimiteNivel").ToString().Trim())
            db.AddInParameter(dbCommand, "@p_LimiteNivelMin", DbType.Decimal, IIf(filaLinea("LimiteNivelMin").ToString().Trim() = "", 0, filaLinea("LimiteNivelMin").ToString().Trim()))
            db.AddInParameter(dbCommand, "@p_LimiteNivelMax", DbType.Decimal, IIf(filaLinea("LimiteNivelMax").ToString().Trim() = "", 0, filaLinea("LimiteNivelMax").ToString().Trim()))
            db.AddInParameter(dbCommand, "@p_TieneLimiteFijoEnDetalle", DbType.String, filaLinea("TieneLimiteFijoEnDetalle").ToString().Trim())
            db.AddInParameter(dbCommand, "@p_LimiteFijoEnDetalleMin", DbType.Decimal, IIf(filaLinea("LimiteFijoEnDetalleMin").ToString().Trim() = "", 0, filaLinea("LimiteFijoEnDetalleMin").ToString().Trim()))
            db.AddInParameter(dbCommand, "@p_LimiteFijoEnDetalleMax", DbType.Decimal, IIf(filaLinea("LimiteFijoEnDetalleMax").ToString().Trim() = "", 0, filaLinea("LimiteFijoEnDetalleMax").ToString().Trim()))

            CodigoNivel = db.ExecuteScalar(dbCommand)

            If Not ListaDetalleNivelLimite Is Nothing Then

                Dim dtt As DataTable
                Dim clave As String
                clave = partClaveDetalle + "," + Secuencial

                dtt = CType(ListaDetalleNivelLimite(clave), DataTable)
                If (Not dtt Is Nothing) Then
                    InsertarDetalleNivelLimite(CodigoNivel, FlagTipoPorcentaje, situacion, dtt, dataRequest, codigoFondo, dtblCondiciones)
                End If

            End If
            cont += 1
        Next
        Return CodigoNivel
    End Function

    Public Function InsertarDetalleNivelLimite(ByVal CodigoNivelLimite As String, ByVal FlagTipoPorcentaje As String, _
    ByVal situacion As String, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet, ByVal codigoFondo As String,
    ByVal dtblCondiciones As DataTable) As String
        Dim Codigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim cont As Integer
        cont = 0
        Dim valor As String
        Dim ValorPorcentaje As Decimal
        Dim valorM As String    'HDG INC 66580 20121112
        Dim ValorPorcentajeM As Decimal 'HDG INC 66580 20121112
        If dtDetalle.Rows.Count > 0 Then
            Dim dbCommand1 As DbCommand = db.GetStoredProcCommand("Limite_EliminarTodoDetalleNivelLimite")
            db.AddInParameter(dbCommand1, "@p_CodigoNivelLimite", DbType.String, CodigoNivelLimite)
            db.AddInParameter(dbCommand1, "@p_CodigoCaracteristica", DbType.String, dtDetalle.Rows(0)("CodigoCaracteristica").ToString)
            db.ExecuteNonQuery(dbCommand1)
        End If

        Dim dbCommand3 As DbCommand = db.GetStoredProcCommand("NivelLimite_Condicion_Eliminar")
        db.AddInParameter(dbCommand3, "@p_CodigoNivelLimite", DbType.String, CodigoNivelLimite)
        db.ExecuteNonQuery(dbCommand3)

        For Each filaLinea As DataRow In dtblCondiciones.Rows
            If filaLinea("CodigoNivelLimite") = CodigoNivelLimite Then
                Dim dbCommand4 As DbCommand = db.GetStoredProcCommand("NivelLimite_Condicion_Insertar")

                db.AddInParameter(dbCommand4, "@p_CodigoNivelLimite", DbType.String, CodigoNivelLimite)
                db.AddInParameter(dbCommand4, "@p_Secuencial", DbType.String, filaLinea("Secuencial"))
                db.AddInParameter(dbCommand4, "@p_Condicion", DbType.String, filaLinea("Condicion"))
                db.AddInParameter(dbCommand4, "@p_PorcentajeMin", DbType.Decimal, filaLinea("PorcentajeMin"))
                db.AddInParameter(dbCommand4, "@p_PorcentajeMax", DbType.Decimal, filaLinea("PorcentajeMax"))

                db.ExecuteScalar(dbCommand4)
            End If
        Next

        For Each filaLinea As DataRow In dtDetalle.Rows
            valor = filaLinea("ValorPorcentaje").ToString
            If valor = "" Then
                ValorPorcentaje = -1
            Else
                ValorPorcentaje = Convert.ToDecimal(valor.Replace(".", Separador))
            End If
            'ini HDG INC 66580 20121112
            valorM = filaLinea("ValorPorcentajeM").ToString
            If valorM = "" Then
                ValorPorcentajeM = -1
            Else
                ValorPorcentajeM = Convert.ToDecimal(valorM.Replace(".", Separador))
            End If
            'fin HDG INC 66580 20121112
            'If situacion = "A" Then
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("Limite_InsertarDetalleNivelLimite")

            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoFondo)
            db.AddInParameter(dbCommand, "@p_FlagTipoPorcentaje", DbType.String, FlagTipoPorcentaje)
            db.AddInParameter(dbCommand, "@p_CodigoNivelLimite", DbType.String, CodigoNivelLimite)
            db.AddInParameter(dbCommand, "@p_CodigoCaracteristica", DbType.String, filaLinea("CodigoCaracteristica").ToString)
            db.AddInParameter(dbCommand, "@p_ValorCaracteristica", DbType.String, filaLinea("ValorCaracteristica").ToString)
            db.AddInParameter(dbCommand, "@p_ClaseNormativa", DbType.String, filaLinea("ClaseNormativa").ToString)
            db.AddInParameter(dbCommand, "@p_ValorPorcentaje", DbType.Decimal, IIf(ValorPorcentaje = -1, Convert.DBNull, ValorPorcentaje))
            db.AddInParameter(dbCommand, "@p_ValorPorcentajeM", DbType.Decimal, IIf(ValorPorcentajeM = -1, Convert.DBNull, ValorPorcentajeM))  'HDG INC 66580 20121112
            db.AddInParameter(dbCommand, "@p_situacion", DbType.String, situacion)
            db.AddInParameter(dbCommand, "@p_ValorEspecifico", DbType.String, filaLinea("ValorEspecifico").ToString)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_CodigoCaracteristicaRelacion", DbType.String, filaLinea("CodigoCaracteristicaRelacion").ToString)
            db.AddInParameter(dbCommand, "@p_CodigoRelacion", DbType.String, filaLinea("CodigoRelacion").ToString)

            Codigo = db.ExecuteScalar(dbCommand)
            'End If
            cont += 1
        Next
        Return Codigo
    End Function
#End Region

#Region " /* Funciones Modificar */"


    Public Function Modificar(ByVal oLimiteBE As LimiteBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Limite_Modificar")
            oRow = CType(oLimiteBE.Limite.Rows(0), LimiteBE.LimiteRow)
            db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, oRow.CodigoLimite)
            db.AddInParameter(dbCommand, "@p_NombreLimite", DbType.String, oRow.NombreLimite)
            db.AddInParameter(dbCommand, "@p_Tope", DbType.String, oRow.Tope)
            db.AddInParameter(dbCommand, "@p_TipoCalculo", DbType.String, oRow.TipoCalculo)
            db.AddInParameter(dbCommand, "@p_UnidadPosicion", DbType.String, oRow.UnidadPosicion)
            db.AddInParameter(dbCommand, "@p_ValorBase", DbType.String, oRow.ValorBase)
            db.AddInParameter(dbCommand, "@p_ClaseLimite", DbType.String, oRow.ClaseLimite)
            'If (oRow.FactorCastigo = -1) Then
            '    db.AddInParameter(dbCommand, "@p_FactorCastigo", DbType.Decimal, Convert.DBNull)
            'Else
            '    db.AddInParameter(dbCommand, "@p_FactorCastigo", DbType.Decimal, oRow.FactorCastigo)
            'End If @
            db.AddInParameter(dbCommand, "@p_AplicarCastigo", DbType.String, oRow.AplicarCastigo)
            db.AddInParameter(dbCommand, "@p_TipoFactor", DbType.String, oRow.TipoFactor)
            db.AddInParameter(dbCommand, "@p_Tipo", DbType.String, oRow.TipoLimite)
            db.AddInParameter(dbCommand, "@p_SaldoBanco", DbType.String, oRow.SaldoBanco)
            db.AddInParameter(dbCommand, "@p_Posicion", DbType.String, oRow.Posicion)
            'RGF 20080819
            db.AddInParameter(dbCommand, "@p_MarketShare", DbType.Decimal, oRow.MarketShare)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

            '' INI MPENAL - 13/09/16
            db.AddInParameter(dbCommand, "@p_IsAgrupadoPorcentaje", DbType.Boolean, oRow.IsAgrupadoPorcentaje)
            db.AddInParameter(dbCommand, "@p_ValorAgrupadoPorcentaje", DbType.Decimal, oRow.ValorAgrupadoPorcentaje)
            '' FIN MPENAL - 13/09/16 
            db.AddInParameter(dbCommand, "@p_Cuadrar", DbType.Int32, oRow.Cuadrar) '' MPENAL - 22/09/16
            db.AddInParameter(dbCommand, "@p_IsLimiteVinculado", DbType.String, oRow.IsLimiteVinculado)
            db.AddInParameter(dbCommand, "@p_PorcLimiteVinculado", DbType.Int32, oRow.PorcLimiteVinculado)
            db.AddInParameter(dbCommand, "@p_CastigoRating", DbType.Int32, oRow.CastigoRating) ' GTueros - 25/10/18
            db.AddInParameter(dbCommand, "@p_TieneCastigo", DbType.String, oRow.TieneCastigo) ' GTueros - 25/10/18
            db.AddInParameter(dbCommand, "@p_AplicaForward", DbType.String, oRow.AplicaForward) ' GTueros - 25/10/18
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function ModificarNegocios(ByVal CodigoLimite As String, ByVal CodigoNegocio As String, ByVal Situacion As String, ByVal dataRequest As DataSet) As String
        Dim StrCodigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Limite_ModificarNegocios")
        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, CodigoLimite)
        db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, CodigoNegocio)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        StrCodigo = db.ExecuteScalar(dbCommand)
        Return StrCodigo
    End Function
    Public Function ModificarCaracteriticas(ByVal CodigoLimite As String, ByVal situacion As String, _
    ByVal dtDetalle As DataTable, ByVal ListaCaracteristicas As Hashtable, _
    ByVal ListaDetalleNivelLimite As Hashtable, ByVal dataRequest As DataSet, ByVal codigoFondo As String, ByVal dtblCondiciones As DataTable, ByVal CodigoRelacion As String) As Boolean
        Dim codigoLimiteCaracteristica, StrTipoLimite, StrCodigoPortafolioSBS As String
        Dim strSituacion As String
        Dim dtt As DataTable
        dtt = dtDetalle
        Dim cont As Integer
        cont = 0
        If (dtt.Rows.Count > 0) Then
            For Each filaLinea As DataRow In dtt.Rows
                codigoLimiteCaracteristica = filaLinea("CodigoLimiteCaracteristica").ToString().Trim()
                StrTipoLimite = filaLinea("Tipo").ToString().Trim()
                StrCodigoPortafolioSBS = filaLinea("CodigoPortafolioSBS").ToString().Trim()
                strSituacion = filaLinea("Situacion").ToString.Trim()
                If codigoLimiteCaracteristica <> "Nuevo" And codigoLimiteCaracteristica <> "" Then
                    ModificarCaracteristicasAux(ListaCaracteristicas, ListaDetalleNivelLimite, StrCodigoPortafolioSBS, CodigoLimite, codigoLimiteCaracteristica, StrTipoLimite, StrCodigoPortafolioSBS, strSituacion, dataRequest, codigoFondo, dtblCondiciones, CodigoRelacion)
                Else
                    InsertarCaracteristicasAux(ListaCaracteristicas, ListaDetalleNivelLimite, StrCodigoPortafolioSBS, CodigoLimite, StrTipoLimite, StrCodigoPortafolioSBS, situacion, dataRequest, codigoFondo, dtblCondiciones, CodigoRelacion)
                End If
                cont += 1
            Next
        End If
    End Function
#End Region
#Region " /* Funciones Eliminar */"

    ''' <summary>
    ''' Elimina un expediente de LimiteBE table oopor una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoLimite As String, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Limite_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, codigoLimite)

        db.ExecuteNonQuery(dbCommand)
        Dim dtt As DataTable = dtDetalle
        Dim CodigoLimiteCaracteristica, Situacion As String
        If Not dtt Is Nothing Then

            If (dtt.Rows.Count > 0) Then
                For Each filaLinea As DataRow In dtt.Rows

                    CodigoLimiteCaracteristica = filaLinea("CodigoLimiteCaracteristica").ToString().Trim()
                    EliminarNiveles(CodigoLimiteCaracteristica, dataRequest)
                Next
            End If
        End If

        Return True
    End Function

    Public Function EliminarNiveles(ByVal codigoLimiteCaracteristica As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Limite_EliminarNiveles")
        db.AddInParameter(dbCommand, "@p_CodigoLimiteCaracteristica", DbType.String, codigoLimiteCaracteristica)

        db.ExecuteNonQuery(dbCommand)

        Return True
    End Function

    Public Function EliminarNegocios(ByVal CodigoLimite As String, ByVal dataRequest As DataSet) As String
        Dim StrCodigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Limite_EliminarNegocios")
        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, CodigoLimite)
        StrCodigo = db.ExecuteScalar(dbCommand)
        Return StrCodigo
    End Function


    'Public Function EliminarPortafolios(ByVal CodigoLimite As String, ByVal dataRequest As DataSet) As String
    '    Dim StrCodigo As String = String.Empty
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim dbCommand As dbCommand = db.GetStoredProcCommand("Limite_EliminarPortafolios")
    '    db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, CodigoLimite)
    '    StrCodigo = db.ExecuteScalar(dbCommand)
    '    Return StrCodigo
    'End Function
#End Region
#Region " /* Funciones de Procesos */ "
    Public Function LimiteEvaluar(ByVal codigoOperacion As String, _
                                ByVal codigoNemonico As String, _
                                ByVal CantidadValor As Decimal, _
                                ByVal MontoNominal As Decimal, _
                                ByVal CodigoPortafolio As String, _
                                ByVal FechaOperacion As String, _
                                  ByVal dataRequest As DataSet) As System.Data.DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Limite_Evaluar")
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codigoOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_CantidadValor", DbType.Decimal, CantidadValor)
        db.AddInParameter(dbCommand, "@p_MontoNominal", DbType.Decimal, MontoNominal)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, Convert.ToDecimal(FechaOperacion))
        Return db.ExecuteDataSet(dbCommand)
    End Function
    'RGF 20081009 Evalua el impacto de una negociacion OnLine, retornando los limites excedidos como 
    'consecuencia de dicha operacion
    Public Function ListarExcesosLimitesOnLine(ByVal codigoNemonico As String, ByVal CantidadOperacion As Decimal, _
                ByVal CodigoPortafolio As String, ByVal codigoOperacion As String, ByVal dataRequest As DataSet, Optional ByVal codigoTercero As String = "") As System.Data.DataSet    'HDG 20120112
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("LimiteOnLine_ListarExcesos")
        dbCommand.CommandTimeout = 1020   'HDG INC 61360	20101014
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_CantidadOperacion", DbType.Decimal, CantidadOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codigoOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)   'HDG 20120112

        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function DefinirPorcentajeLimiteExteriorDetallado(ByVal valorCaracteristica As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_InsertarPorcentaje_DetalleNivelLimite2")
        db.AddInParameter(dbCommand, "@p_ValorCaracteristica", DbType.String, valorCaracteristica)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
#End Region
#Region " /* Funciones Personalizadas*/"
    Public Sub InsertarCaracteristicasAux(ByVal ListaCaraceristicas As Hashtable, _
    ByVal ListaDetalleNivelLimite As Hashtable, _
    ByVal Indice As Integer, ByVal CodigoLimite As String, ByVal StrTipoLimite As String, _
    ByVal StrCodigoPortafolioSBS As String, ByVal Situacion As String, ByVal dataRequest As DataSet, ByVal codigoFondo As String,
    ByVal dtblCondiciones As DataTable, ByVal CodigoRelacion As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Limite_InsertarCaracteristicas")
        Dim codigoLimiteCaracteristica As String
        Dim Secuencial, CodigoNivelLimite, CodigoCaracteristica, FlagTipoPorcentaje, ValorPorcentaje As String
        Dim TieneLimiteNivel, TieneLimiteFijoEnDetalle As String
        Dim LimiteNivelMin, LimiteNivelMax, LimiteFijoEnDetalleMin, LimiteFijoEnDetalleMax As Decimal
        Dim codigoAux As String
        Dim SituacionNivel As String
        Dim cont As Integer
        Dim dtt As DataTable
        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, CodigoLimite)
        db.AddInParameter(dbCommand, "@p_TipoLimite", DbType.String, StrTipoLimite)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, StrCodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        codigoLimiteCaracteristica = db.ExecuteScalar(dbCommand)
        If Not ListaCaraceristicas Is Nothing Then
            dtt = CType(ListaCaraceristicas(Indice), DataTable)
        End If
        If Not dtt Is Nothing Then
            dtt = CType(ListaCaraceristicas(Indice), DataTable)
            If (dtt.Rows.Count > 0) Then
                cont = 0
                For Each filaLinea As DataRow In dtt.Rows
                    Secuencial = filaLinea("Secuencial").ToString().Trim()
                    CodigoNivelLimite = filaLinea("CodigoNivelLimite").ToString().Trim()
                    CodigoCaracteristica = filaLinea("CodigoCaracteristica").ToString().Trim()
                    FlagTipoPorcentaje = filaLinea("FlagTipoPorcentaje").ToString().Trim()
                    If FlagTipoPorcentaje.ToUpper = "GENERAL" Then
                        FlagTipoPorcentaje = "G"
                    ElseIf FlagTipoPorcentaje.ToUpper = "AGRUPACION" Then
                        FlagTipoPorcentaje = "A"
                    ElseIf FlagTipoPorcentaje.ToUpper = "DETALLE" Then
                        FlagTipoPorcentaje = "D"
                    End If

                    TieneLimiteNivel = filaLinea("TieneLimiteNivel").ToString().Trim()
                    TieneLimiteFijoEnDetalle = filaLinea("TieneLimiteFijoEnDetalle").ToString().Trim()
                    LimiteNivelMin = IIf(filaLinea("LimiteNivelMin").ToString().Trim() = "", 0, filaLinea("LimiteNivelMin").ToString().Trim())
                    LimiteNivelMax = IIf(filaLinea("LimiteNivelMax").ToString().Trim() = "", 0, filaLinea("LimiteNivelMax").ToString().Trim())
                    LimiteFijoEnDetalleMin = IIf(filaLinea("LimiteFijoEnDetalleMin").ToString().Trim() = "", 0, filaLinea("LimiteFijoEnDetalleMin").ToString().Trim())
                    LimiteFijoEnDetalleMax = IIf(filaLinea("LimiteFijoEnDetalleMax").ToString().Trim() = "", 0, filaLinea("LimiteFijoEnDetalleMax").ToString().Trim())

                    ValorPorcentaje = Convert.ToDecimal(filaLinea("ValorPorcentaje").ToString().Trim())
                    Dim ClaveDetalleNivel As String
                    ClaveDetalleNivel = Indice.ToString() + "," + Secuencial + "," + CodigoCaracteristica
                    If CodigoNivelLimite <> "Nuevo" Then
                        codigoAux = ModificarCaracteristicasNivelesAux(ClaveDetalleNivel, ListaDetalleNivelLimite, Secuencial, codigoLimiteCaracteristica, CodigoNivelLimite, CodigoCaracteristica, FlagTipoPorcentaje, ValorPorcentaje, Situacion, dataRequest, codigoFondo, TieneLimiteNivel, TieneLimiteFijoEnDetalle, LimiteNivelMin, LimiteNivelMax, LimiteFijoEnDetalleMin, LimiteFijoEnDetalleMax, dtblCondiciones, CodigoRelacion)
                    Else
                        codigoAux = InsertarCaracteristicasNivelesAux(ClaveDetalleNivel, ListaDetalleNivelLimite, Secuencial, codigoLimiteCaracteristica, CodigoNivelLimite, CodigoCaracteristica, FlagTipoPorcentaje, ValorPorcentaje, Situacion, dataRequest, codigoFondo, TieneLimiteNivel, TieneLimiteFijoEnDetalle, LimiteNivelMin, LimiteNivelMax, LimiteFijoEnDetalleMin, LimiteFijoEnDetalleMax, dtblCondiciones, CodigoRelacion)
                    End If
                    cont = cont + 1
                Next
            End If
        End If
    End Sub
    Public Sub ModificarCaracteristicasAux(ByVal ListaCaracteristicas As Hashtable, ByVal ListaDetalleNivelLimite As Hashtable, ByVal Indice As Integer, _
    ByVal CodigoLimite As String, ByVal codigoLimiteCaracteristica As String, ByVal StrTipoLimite As String, _
    ByVal StrCodigoPortafolioSBS As String, ByVal Situacion As String, ByVal dataRequest As DataSet, ByVal codigoFondo As String,
    ByVal dtblCondiciones As DataTable, ByVal CodigoRelacion As String)
        Dim Secuencial, CodigoNivelLimite, Valor, CodigoCaracteristica, DescripcionNivel, FlagTipoPorcentaje, CodNivel As String
        Dim TieneLimiteNivel, TieneLimiteFijoEnDetalle As String
        Dim LimiteNivelMin, LimiteNivelMax, LimiteFijoEnDetalleMin, LimiteFijoEnDetalleMax As Decimal
        Dim CodigoAux As String
        Dim ValorPorcentaje As Decimal
        Dim SituacionNivel As String
        Dim cont As Integer
        Dim dtt As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Limite_ModificarCaracteristicas")
        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, CodigoLimite)
        db.AddInParameter(dbCommand, "@p_CodigoLimiteCaracteristica", DbType.String, codigoLimiteCaracteristica)
        db.AddInParameter(dbCommand, "@p_TipoLimite", DbType.String, StrTipoLimite)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, StrCodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        cont = 0
        If Not ListaCaracteristicas Is Nothing Then
            dtt = CType(ListaCaracteristicas(Indice), DataTable)
        End If
        If Not dtt Is Nothing Then
            If (dtt.Rows.Count > 0) Then
                For Each filaLinea As DataRow In dtt.Rows
                    Secuencial = filaLinea("Secuencial").ToString().Trim()
                    CodigoNivelLimite = filaLinea("CodigoNivelLimite").ToString().Trim()
                    CodigoCaracteristica = filaLinea("CodigoCaracteristica").ToString().Trim()
                    FlagTipoPorcentaje = filaLinea("FlagTipoPorcentaje").ToString().Trim()
                    If FlagTipoPorcentaje.ToUpper = "GENERAL" Then
                        FlagTipoPorcentaje = "G"
                    ElseIf FlagTipoPorcentaje.ToUpper = "AGRUPACION" Then
                        FlagTipoPorcentaje = "A"
                    ElseIf FlagTipoPorcentaje.ToUpper = "DETALLE" Then
                        FlagTipoPorcentaje = "D"
                    End If

                    Valor = filaLinea("ValorPorcentaje").ToString().Trim()
                    If Valor = "" Then
                        ValorPorcentaje = -1
                    Else
                        ValorPorcentaje = Convert.ToDecimal(Valor.Replace(".", Separador))
                    End If

                    TieneLimiteNivel = filaLinea("TieneLimiteNivel").ToString().Trim()
                    TieneLimiteFijoEnDetalle = filaLinea("TieneLimiteFijoEnDetalle").ToString().Trim()
                    LimiteNivelMin = IIf(filaLinea("LimiteNivelMin").ToString().Trim() = "", 0, filaLinea("LimiteNivelMin").ToString().Trim())
                    LimiteNivelMax = IIf(filaLinea("LimiteNivelMax").ToString().Trim() = "", 0, filaLinea("LimiteNivelMax").ToString().Trim())
                    LimiteFijoEnDetalleMin = IIf(filaLinea("LimiteFijoEnDetalleMin").ToString().Trim() = "", 0, filaLinea("LimiteFijoEnDetalleMin").ToString().Trim())
                    LimiteFijoEnDetalleMax = IIf(filaLinea("LimiteFijoEnDetalleMax").ToString().Trim() = "", 0, filaLinea("LimiteFijoEnDetalleMax").ToString().Trim())
                    CodigoRelacion = filaLinea("CodigoRelacion").ToString().Trim()

                    Dim ClaveDetalleNivel As String
                    ClaveDetalleNivel = Indice.ToString + "," + Secuencial + "," + CodigoCaracteristica
                    If CodigoNivelLimite <> "Nuevo" Then
                        If filaLinea("situacion") = "INACTIVO" Then
                            CodigoAux = EliminarCaracteristicasNivelesAux(codigoLimiteCaracteristica, CodigoNivelLimite, dataRequest)
                        Else
                            CodigoAux = ModificarCaracteristicasNivelesAux(ClaveDetalleNivel, ListaDetalleNivelLimite, Secuencial, codigoLimiteCaracteristica, CodigoNivelLimite, CodigoCaracteristica, FlagTipoPorcentaje, ValorPorcentaje, "A", dataRequest, codigoFondo, TieneLimiteNivel, TieneLimiteFijoEnDetalle, LimiteNivelMin, LimiteNivelMax, LimiteFijoEnDetalleMin, LimiteFijoEnDetalleMax, dtblCondiciones, CodigoRelacion)
                        End If
                    Else
                        CodigoAux = InsertarCaracteristicasNivelesAux(ClaveDetalleNivel, ListaDetalleNivelLimite, Secuencial, codigoLimiteCaracteristica, CodigoNivelLimite, CodigoCaracteristica, FlagTipoPorcentaje, ValorPorcentaje, Situacion, dataRequest, codigoFondo, TieneLimiteNivel, TieneLimiteFijoEnDetalle, LimiteNivelMin, LimiteNivelMax, LimiteFijoEnDetalleMin, LimiteFijoEnDetalleMax, dtblCondiciones, CodigoRelacion)
                    End If
                    cont = cont + 1
                Next
            End If
        End If
    End Sub
    Public Function Separador() As String
        Return System.Globalization.NumberFormatInfo.CurrentInfo.PercentDecimalSeparator
    End Function

    Public Sub Insertar_Condicion_LimiteNivel(ByVal CodigoNivelLimite As String, ByVal Secuencial As Integer, ByVal Condicion As String, ByVal PorcentajeMin As Decimal, ByVal PorcentajeMax As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("NivelLimite_Condicion_Insertar")

        db.AddInParameter(dbCommand, "@p_CodigoNivelLimite", DbType.String, CodigoNivelLimite)
        db.AddInParameter(dbCommand, "@p_Secuencial", DbType.String, Secuencial)
        db.AddInParameter(dbCommand, "@p_Condicion", DbType.String, Condicion)
        db.AddInParameter(dbCommand, "@p_PorcentajeMin", DbType.Decimal, IIf(PorcentajeMin = -1, Convert.DBNull, PorcentajeMin))
        db.AddInParameter(dbCommand, "@p_PorcentajeMax", DbType.Decimal, IIf(PorcentajeMax = -1, Convert.DBNull, PorcentajeMax))

        db.ExecuteScalar(dbCommand)
    End Sub

    Public Sub Eliminar_Condicion_LimiteNivel(ByVal CodigoNivelLimite As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("NivelLimite_Condicion_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoNivelLimite", DbType.String, CodigoNivelLimite)

        db.ExecuteScalar(dbCommand)
    End Sub

    Public Function Seleccionar_Condicion_LimiteNIvel(ByVal CodigoNivelLimite As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("NivelLimite_Condicion_Seleccionar")
        db.AddInParameter(dbCommand, "@p_CodigoNivelLimite", DbType.String, CodigoNivelLimite)

        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function

    Public Function InsertarCaracteristicasNivelesAux(ByVal clave As String, _
    ByVal ListaDetalleNivelLimite As Hashtable, ByVal Secuencial As String, _
    ByVal codigoLimiteCaracteristica As String, _
    ByVal CodigoNivelLimite As String, ByVal CodigoCaracteristica As String, ByVal FlagTipoPorcentaje As String, _
    ByVal ValorPorcentaje As Decimal, ByVal Situacion As String, _
    ByVal dataRequest As DataSet,
    ByVal codigoFondo As String,
    ByVal TieneLimiteNivel As String, ByVal TieneLimiteFijoEnDetalle As String, ByVal LimiteNivelMin As Decimal, ByVal LimiteNivelMax As Decimal, ByVal LimiteFijoEnDetalleMin As Decimal, ByVal LimiteFijoEnDetalleMax As Decimal,
    ByVal dtblCondiciones As DataTable,
    ByVal CodigoRelacion As String) As String

        Dim CodigoNivel As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Limite_InsertarCaracteristicasNiveles")

        db.AddInParameter(dbCommand, "@p_Secuencial", DbType.String, Secuencial)
        db.AddInParameter(dbCommand, "@p_CodigoLimiteCaracteristica", DbType.String, codigoLimiteCaracteristica)
        db.AddInParameter(dbCommand, "@p_CodigoCaracteristica", DbType.String, CodigoCaracteristica)
        db.AddInParameter(dbCommand, "@p_FlagTipoPorcentaje", DbType.String, FlagTipoPorcentaje)
        db.AddInParameter(dbCommand, "@p_ValorPorcentaje", DbType.Decimal, IIf(ValorPorcentaje = -1, Convert.DBNull, ValorPorcentaje))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, Situacion)

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.AddInParameter(dbCommand, "@p_TieneLimiteNivel", DbType.String, TieneLimiteNivel)
        db.AddInParameter(dbCommand, "@p_LimiteNivelMin", DbType.Decimal, LimiteNivelMin)
        db.AddInParameter(dbCommand, "@p_LimiteNivelMax", DbType.Decimal, LimiteNivelMax)
        db.AddInParameter(dbCommand, "@p_TieneLimiteFijoEnDetalle", DbType.String, TieneLimiteFijoEnDetalle)
        db.AddInParameter(dbCommand, "@p_LimiteFijoEnDetalleMin", DbType.Decimal, LimiteFijoEnDetalleMin)
        db.AddInParameter(dbCommand, "@p_LimiteFijoEnDetalleMax", DbType.Decimal, LimiteFijoEnDetalleMax)
        CodigoNivel = db.ExecuteScalar(dbCommand)

        If Not ListaDetalleNivelLimite Is Nothing Then

            Dim dtt As DataTable


            dtt = CType(ListaDetalleNivelLimite(clave), DataTable)
            If (Not dtt Is Nothing) Then
                EliminarDetalleNivelLimite(dtt, dataRequest)
                InsertarDetalleNivelLimite(CodigoNivel, FlagTipoPorcentaje, Situacion, dtt, dataRequest, codigoFondo, dtblCondiciones)
            End If

        End If
        Return CodigoNivel
    End Function
    Public Function ModificarCaracteristicasNivelesAux(ByVal clave As String, _
    ByVal ListaDetalleNivelLimite As Hashtable, ByVal Secuencial As String, ByVal codigoLimiteCaracteristica As String, _
    ByVal CodigoNivelLimite As String, ByVal CodigoCaracteristica As String, ByVal FlagTipoPorcentaje As String, _
    ByVal ValorPorcentaje As Decimal, ByVal Situacion As String, _
    ByVal dataRequest As DataSet, ByVal codigoFondo As String,
    ByVal TieneLimiteNivel As String, ByVal TieneLimiteFijoEnDetalle As String, ByVal LimiteNivelMin As Decimal, ByVal LimiteNivelMax As Decimal, ByVal LimiteFijoEnDetalleMin As Decimal, ByVal LimiteFijoEnDetalleMax As Decimal,
    ByVal dtblCondiciones As DataTable,
    ByVal CodigoRelacion As String) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Limite_ModificarCaracteristicasNiveles")

        db.AddInParameter(dbCommand, "@p_Secuencial", DbType.String, Secuencial)
        db.AddInParameter(dbCommand, "@p_CodigoLimiteCaracteristica", DbType.String, codigoLimiteCaracteristica)
        db.AddInParameter(dbCommand, "@p_CodigoNivelLimite", DbType.String, CodigoNivelLimite)
        db.AddInParameter(dbCommand, "@p_CodigoCaracteristica", DbType.String, CodigoCaracteristica)
        db.AddInParameter(dbCommand, "@p_FlagTipoPorcentaje", DbType.String, FlagTipoPorcentaje)
        db.AddInParameter(dbCommand, "@p_ValorPorcentaje", DbType.Decimal, IIf(ValorPorcentaje = -1, Convert.DBNull, ValorPorcentaje))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.AddInParameter(dbCommand, "@p_TieneLimiteNivel", DbType.String, TieneLimiteNivel)
        db.AddInParameter(dbCommand, "@p_LimiteNivelMin", DbType.Decimal, LimiteNivelMin)
        db.AddInParameter(dbCommand, "@p_LimiteNivelMax", DbType.Decimal, LimiteNivelMax)
        db.AddInParameter(dbCommand, "@p_TieneLimiteFijoEnDetalle", DbType.String, TieneLimiteFijoEnDetalle)
        db.AddInParameter(dbCommand, "@p_LimiteFijoEnDetalleMin", DbType.Decimal, LimiteFijoEnDetalleMin)
        db.AddInParameter(dbCommand, "@p_LimiteFijoEnDetalleMax", DbType.Decimal, LimiteFijoEnDetalleMax)
        db.AddInParameter(dbCommand, "@p_ValorPorcentajeM", DbType.Decimal, ValorPorcentaje)
        db.AddInParameter(dbCommand, "@p_CodigoRelacion", DbType.String, CodigoRelacion)
        db.ExecuteScalar(dbCommand)

        If Not ListaDetalleNivelLimite Is Nothing Then

            Dim dtt As DataTable
            Try
                dtt = CType(ListaDetalleNivelLimite(clave), DataTable)
                If (Not dtt Is Nothing) Then
                    For Each filaLinea As DataRow In dtt.Rows
                        filaLinea("CodigoNivelLimite") = CodigoNivelLimite
                    Next
                    EliminarDetalleNivelLimite(dtt, dataRequest)
                    InsertarDetalleNivelLimite(CodigoNivelLimite, FlagTipoPorcentaje, Situacion, dtt, dataRequest, codigoFondo, dtblCondiciones)
                End If
            Catch ex As Exception
                'Las siguientes 4 líneas deben agregarse para el Exception app block
                Dim rethrow As Boolean = True 'ExceptionPolicy.HandleException(ex, "SITBLPolicy")
                If (rethrow) Then
                    Throw
                End If
            End Try



        End If
        Return CodigoNivelLimite



    End Function

    'RGF 20080709
    Public Function EliminarCaracteristicasNivelesAux(ByVal codigoLimiteCaracteristica As String, _
    ByVal CodigoNivelLimite As String, ByVal dataRequest As DataSet) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Limite_EliminarCaracteristicasNiveles")

        db.AddInParameter(dbCommand, "@p_CodigoLimiteCaracteristica", DbType.String, codigoLimiteCaracteristica)
        db.AddInParameter(dbCommand, "@p_CodigoNivelLimite", DbType.String, CodigoNivelLimite)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteScalar(dbCommand)

        'If Not ListaDetalleNivelLimite Is Nothing Then

        '    Dim dtt As DataTable
        '    Try
        '        dtt = CType(ListaDetalleNivelLimite(clave), DataTable)
        '        If (Not dtt Is Nothing) Then
        '            For Each filaLinea As DataRow In dtt.Rows
        '                filaLinea("CodigoNivelLimite") = CodigoNivelLimite
        '            Next
        '            EliminarDetalleNivelLimite(dtt, dataRequest)
        '            InsertarDetalleNivelLimite(CodigoNivelLimite, FlagTipoPorcentaje, Situacion, dtt, dataRequest)
        '        End If
        '    Catch ex As Exception
        '        'Las siguientes 4 líneas deben agregarse para el Exception app block
        '        Dim rethrow As Boolean = True 'ExceptionPolicy.HandleException(ex, "SITBLPolicy")
        '        If (rethrow) Then
        '            Throw
        '        End If
        '    End Try
        'End If
        Return CodigoNivelLimite
    End Function

    'CMB OT 62087 20110202 Nro 22 
    Public Function ActualizarPorcentajePorTipoInstrumento(ByVal valorCaracteristica As String, ByVal valorPorcentaje As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_upd_ActualizarPorcentajePorTipoInstrumento")

        db.AddInParameter(dbCommand, "@p_ValorCaracteristica", DbType.String, valorCaracteristica)
        db.AddInParameter(dbCommand, "@p_ValorPorcentaje", DbType.Decimal, valorPorcentaje)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

#End Region

#Region "/*Eliminar*/"
    Public Sub EliminarDetalleNivelLimite(ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()


        For Each filaLinea As DataRow In dtDetalle.Rows


            Dim dbCommand As dbCommand = db.GetStoredProcCommand("Limite_EliminarDetalleNivelLimite")

            db.AddInParameter(dbCommand, "@p_CodigoNivelLimite", DbType.String, filaLinea("CodigoNivelLimite").ToString)
            db.AddInParameter(dbCommand, "@p_CodigoCaracteristica", DbType.String, filaLinea("CodigoCaracteristica").ToString)
            db.AddInParameter(dbCommand, "@p_ClaseNormativa", DbType.String, filaLinea("ClaseNormativa").ToString)
            db.AddInParameter(dbCommand, "@p_ValorCaracteristica", DbType.String, filaLinea("ValorCaracteristica").ToString)
            db.ExecuteScalar(dbCommand)
        Next


    End Sub
#End Region

#Region "/*Incremento-Disminucion de Patrimonio (Req40) LETV 20090414*/"

    Public Function SeleccionarIncDecPatrimonio(ByVal codigoCategoriaIncDec As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("LimiteIncrementoDisminucionPatrimonio_Seleccionar")
        db.AddInParameter(dbCommand, "@p_CodigoCategoriaIncDec", DbType.String, codigoCategoriaIncDec)
        Dim ds As New DataSet
        db.LoadDataSet(dbCommand, ds, "LimiteIncDecPatrimonio")
        Return ds.Tables("LimiteIncDecPatrimonio")
    End Function

    Public Function ValidarIncDecPatrimonio(ByVal codigoCategoriaIncDec As String, ByVal codigoPortafolio As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("LimiteIncrementoDisminucionPatrimonio_SeleccionarPorCategoria")
        db.AddInParameter(dbCommand, "@p_CodigoCategoriaIncDec", DbType.String, codigoCategoriaIncDec)
        db.AddInParameter(dbCommand, "@p_codigoPortafolioSBS", DbType.String, codigoPortafolio)
        Dim ds As New DataSet
        Dim resultado As Boolean = False
        db.LoadDataSet(dbCommand, ds, "LimiteIncDecPatrimonio")
        resultado = IIf(ds.Tables("LimiteIncDecPatrimonio").Rows.Count > 0, True, False)
        Return resultado
    End Function

    Public Function SeleccionarPorFondoIncDecPatrimonio(ByVal codigoPortafolio As String, ByVal FechaOperacion As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("LimiteIncrementoDisminucionPatrimonio_SeleccionarPorFondo")
        db.AddInParameter(dbCommand, "@p_codigoPortafolioSBS", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
        Dim ds As New DataSet
        db.LoadDataSet(dbCommand, ds, "LimiteIncDecPatrimonio")
        Return ds.Tables("LimiteIncDecPatrimonio")
    End Function

    Public Function InsertarIncDecPatrimonio(ByVal codigoCategoriaIncDec As String, ByVal codigoPortafolio As String, ByVal valor As Decimal, ByVal TipoIngreso As String) As Boolean
        Dim StrCodigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("LimiteIncrementoDisminucionPatrimonio_Insertar")
        db.AddInParameter(dbCommand, "@p_codigoCategoriaIncDec", DbType.String, codigoCategoriaIncDec)
        db.AddInParameter(dbCommand, "@p_codigoPortafolioSBS", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_valor", DbType.Decimal, valor)
        db.AddInParameter(dbCommand, "@p_tipoIngreso", DbType.String, TipoIngreso)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function ModificarIncDecPatrimonio(ByVal CodigoIncDec As String, ByVal codigoPortafolio As String, ByVal valor As Decimal, ByVal tipoingreso As String) As Boolean
        Dim StrCodigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("LimiteIncrementoDisminucionPatrimonio_Modificar")
        db.AddInParameter(dbCommand, "@p_CodigoIncDec", DbType.String, CodigoIncDec)
        db.AddInParameter(dbCommand, "@p_codigoPortafolioSBS", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_valor", DbType.Decimal, valor)
        db.AddInParameter(dbCommand, "@p_tipoIngreso", DbType.String, tipoingreso)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function EliminarIncDecPatrimonio(ByVal CodigoIncDec As String) As Boolean
        Dim StrCodigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("LimiteIncrementoDisminucionPatrimonio_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoIncDec", DbType.String, CodigoIncDec)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
#End Region

#Region "Porcentaje cerca al limite OT 11517 LETV 20091014"

    Public Function ModificarPorcentajeCercaLimite(ByVal codigoLimite As String, ByVal codigoPortafolio As String, ByVal porcentaje As Decimal) As Boolean
        Dim StrCodigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Limite_ModificarPorcentajeCercaLimite")
        db.AddInParameter(dbCommand, "@p_codigoLimite", DbType.String, codigoLimite)
        db.AddInParameter(dbCommand, "@p_codigoPortafolioSBS", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_porcentaje", DbType.Decimal, porcentaje)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function SeleccionarPorcentajeCercaLimite(ByVal codigoLimite As String, ByVal CodigoLimiteCaracteristica As String) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Limite_SeleccionarPorcentajeCercaLimite")
        db.AddInParameter(dbCommand, "@p_codigoLimite", DbType.String, codigoLimite)
        db.AddInParameter(dbCommand, "@p_CodigoLimiteCaracteristica", DbType.String, CodigoLimiteCaracteristica)
        Return CDec(db.ExecuteScalar(dbCommand))
    End Function
#End Region

#Region "/*Usuarios Notificados HDG OT 61566 Nro3 20101027*/"

    Public Function SeleccionarUsuarioNotifica() As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_ListarUsuariosNotificados")
        Dim ds As New DataSet
        db.LoadDataSet(dbCommand, ds, "LimiteUsuarioNotifica")
        Return ds.Tables("LimiteUsuarioNotifica")
    End Function

    Public Function ListarUnidadesPuestos() As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_ListarUnidadesPuestos")
        Dim ds As New DataSet
        db.LoadDataSet(dbCommand, ds, "UnidadesPuestos")
        Return ds.Tables("UnidadesPuestos")
    End Function

    Public Function SeleccionarPersonal(ByVal oUsuario As UsuariosNotificaBE) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim oUsuarioRow As UsuariosNotificaBE.UsuariosNotificaRow
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_PersonalSeleccionar")
        oUsuarioRow = CType(oUsuario.UsuariosNotifica.Rows(0), UsuariosNotificaBE.UsuariosNotificaRow)

        db.AddInParameter(dbCommand, "@p_CodigoUsuario", DbType.String, oUsuarioRow.CodigoUsuario)
        db.AddInParameter(dbCommand, "@p_Nombre", DbType.String, oUsuarioRow.Nombre)
        db.AddInParameter(dbCommand, "@p_Apellido", DbType.String, oUsuarioRow.Apellido)
        db.AddInParameter(dbCommand, "@p_CodigoCentroCosto", DbType.String, oUsuarioRow.CodigoCentroCosto)
        Dim ds As New DataSet
        db.LoadDataSet(dbCommand, ds, "Personal")
        Return ds.Tables("Personal")
    End Function

    Public Function InsertarUsuarioNotifica(ByVal oUsuario As UsuariosNotificaBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_ins_UsuariosNotificados_insertar")
        Dim oUsuarioRow As UsuariosNotificaBE.UsuariosNotificaRow
        oUsuarioRow = CType(oUsuario.UsuariosNotifica.Rows(0), UsuariosNotificaBE.UsuariosNotificaRow)
        db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, oUsuarioRow.CodigoInterno)
        db.AddInParameter(dbCommand, "@p_CodigoUsuario", DbType.String, oUsuarioRow.CodigoUsuario)
        db.AddInParameter(dbCommand, "@p_CodigoCentroCosto", DbType.String, oUsuarioRow.CodigoCentroCosto)
        db.AddInParameter(dbCommand, "@p_Email", DbType.String, oUsuarioRow.Email)

        db.AddInParameter(dbCommand, "@p_Usuariocreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_Fechacreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Horacreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        Return db.ExecuteScalar(dbCommand)
    End Function

    Public Function EliminarUsuarioNotifica(ByVal CodigoInterno As String, ByVal dataRequest As DataSet) As Boolean
        Dim StrCodigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_del_UsuariosNotificados_eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, CodigoInterno)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
#End Region

    Public Function LimiteMaximoNegociacion_Validar(ByVal Fecha As Decimal) As ArrayList
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("LimiteMaximoNegociacion_Validar")
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.String, Fecha)
        Dim Fechas As New ArrayList
        Dim ir As IDataReader
        ir = db.ExecuteReader(dbCommand)
        While ir.Read
            Fechas.Add(ir("FechaOperacion"))
        End While
        Return Fechas
    End Function

    'HDG OT 62087 Nro5-R10 20110121
    Public Function ObtenerPorcentajeLimiteGen(ByVal CodigoLimite As String, ByVal Nivel As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_PorcentajeLimiteNivelGen")
        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, CodigoLimite)
        db.AddInParameter(dbCommand, "@p_Secuencial", DbType.String, Nivel)

        Dim ds As New DataSet
        db.LoadDataSet(dbCommand, ds, "PorcentajeLimites")
        Return ds.Tables("PorcentajeLimites")
    End Function
    'OT10795 06/10/2017 Refactorizar código
    Public Sub RegistrarOrdenesPreviasSeleccionadas(ByVal tipoRenta As String, Optional ByVal decNProceso As Decimal = 0)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_RegistraOrdenesPreviasSeleccionadas")
            db.AddInParameter(dbCommand, "@p_TipoRenta", DbType.String, tipoRenta)
            db.AddInParameter(dbCommand, "@p_NProceso", DbType.Decimal, decNProceso)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    'OT10795 - Fin
    Public Function ReporteEstructuraLimites(ByVal codigoLimite As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_ReporteEstructuraLimites_Limite")
        dbCommand.CommandTimeout = 300
        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, codigoLimite)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    'CMB OT 63540 20110714
    Public Function SeleccionarAjusteLimitesEstimados(ByVal fechaOperacion As Decimal, ByVal codigoPortafolio As String, ByVal codigoLimite As String, ByRef totalAjustes As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_Ajustes_LimitesEstimados")
        Dim ds As DataSet
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fechaOperacion)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, codigoLimite)
        db.AddOutParameter(dbCommand, "@p_TotalAjustes", DbType.Decimal, totalAjustes)

        ds = db.ExecuteDataSet(dbCommand)
        totalAjustes = Convert.ToDecimal(db.GetParameterValue(dbCommand, "@p_TotalAjustes"))
        Return ds
    End Function

    'PLD OT 67244 20130502
    Public Function ObtenerLiquidezLimite(ByVal CodigoLimite As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_SeleccionarLiquidezLimite")
        db.AddInParameter(dbCommand, "@p_CodLimite", DbType.String, CodigoLimite)

        Dim ds As New DataSet
        db.LoadDataSet(dbCommand, ds, "ParametrosGenerales")
        Return ds.Tables("ParametrosGenerales")
    End Function
    'PLD OT 67244 20130502

    'PLD OT 67244 20130503
    Public Sub ActualizarParametriaLiquidez(ByVal CodigoLimite As String, ByVal PorcentajeIL As String, ByVal PorcentajeLL As String, ByVal PorcentajeSL As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_ActualizarParametriaLiquidez")
        db.AddInParameter(dbCommand, "@p_CodLimite", DbType.String, CodigoLimite)
        db.AddInParameter(dbCommand, "@p_PorcentajeIL", DbType.String, PorcentajeIL)
        db.AddInParameter(dbCommand, "@p_PorcentajeLL", DbType.String, PorcentajeLL)
        db.AddInParameter(dbCommand, "@p_PorcentajeSL", DbType.String, PorcentajeSL)
        db.ExecuteNonQuery(dbCommand)
    End Sub
    'PLD OT 67244 20130503

    'PLD OT 67244 20130506

    Public Sub ActualizarPorcentajeLiquidez(ByVal CodigoLimite As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_ActualizarValorPorcentajeLiquidez")
        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, CodigoLimite)
        db.ExecuteNonQuery(dbCommand)
    End Sub


    'PLD OT 67244 20130506

    'BPesantes OT 9567 Inicio 12-09-2016
    Public Function ProcesarLimites(ByVal FechaOperacion As Decimal, Optional ByVal CodigoPortafolioSBS As String = "") As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ProcesarLimites")
            dbCommand.CommandTimeout = 1800
            Dim Descripcion As String = ""
            db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, FechaOperacion)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddOutParameter(dbCommand, "@p_Descripcion", DbType.String, 40)
            db.ExecuteNonQuery(dbCommand)
            Descripcion = Convert.ToString(db.GetParameterValue(dbCommand, "@p_Descripcion"))
            Return Descripcion
        End Using
    End Function

    Public Function ConsolidadoLimites(ByVal FechaOperacion As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ConsolidadoLimites")
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, FechaOperacion)

        Dim ds As New DataSet
        db.LoadDataSet(dbCommand, ds, "ConsolidadoLimites")
        Return ds.Tables("ConsolidadoLimites")
    End Function

    Public Function ReporteLimites(ByVal FechaOperacion As Decimal, ByVal FechaCadena As String, ByVal CodigoClienteMandato As String, Optional ByVal CodigoPortafolio As String = "") As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        'Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ReporteLimites")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ReporteLimites_New") 'CAMBIOS EN EL REPORTE DE LIMITES - GT 20181114'
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, FechaOperacion)
        db.AddInParameter(dbCommand, "@p_FechaCadena", DbType.String, FechaCadena)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_ClienteMandato", DbType.String, CodigoClienteMandato)

        Dim ds As New DataSet
        db.LoadDataSet(dbCommand, ds, "ReporteLimites")
        Return ds.Tables("ReporteLimites")
    End Function
    'OT 9755 Fin

    Public Sub InsertarValidadorLimiteDetalle(ByVal objListValidadorLimiteDetalle As ListValidadorLimite, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        For Each objValidadorLimiteDetalle As ValidadorLimiteDetalleBE In objListValidadorLimiteDetalle.objListValidadorLimite
            Using dbCommand As DbCommand = db.GetStoredProcCommand("ValidadorLimiteDetalle_Insertar")
                db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, objValidadorLimiteDetalle.CodigoLimite)
                db.AddInParameter(dbCommand, "@p_CodigoValidador", DbType.String, objValidadorLimiteDetalle.CodigoValidador)
                db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
                db.AddInParameter(dbCommand, "@p_Tipo", DbType.String, objValidadorLimiteDetalle.Tipo)
                db.ExecuteNonQuery(dbCommand)
            End Using
        Next
    End Sub

    Public Sub EliminarValidadorLimiteDetalle(ByVal p_CodigoLimite As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ValidadorLimiteDetalle_Eliminar")
            db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, p_CodigoLimite)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub

    Public Function SeleccionarValidadorLimiteDetalle(ByVal objValidadorLimiteDetalle As ValidadorLimiteDetalleBE) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ValidadorLimiteDetalle_Seleccionar")
            db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, objValidadorLimiteDetalle.CodigoLimite)
            db.AddInParameter(dbCommand, "@p_CodigoValidador", DbType.String, objValidadorLimiteDetalle.CodigoValidador)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
End Class


