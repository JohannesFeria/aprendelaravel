Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class ParametrosGeneralesDAM
#Region "/* Funciones CRUD */"
    Public Function Insertar(ByVal clasificacion As String, ByVal nombre As String, ByVal valor As String, ByVal comentario As String, ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_Insertar")
        db.AddInParameter(dbCommand, "@p_clasificacion", DbType.String, clasificacion)
        db.AddInParameter(dbCommand, "@p_Nombre", DbType.String, nombre)
        db.AddInParameter(dbCommand, "@p_Valor", DbType.String, valor)
        db.AddInParameter(dbCommand, "@p_Comentario", DbType.String, comentario)
        db.ExecuteNonQuery(dbCommand)

    End Function
    'CMB OT 63063 20110419 REQ 15
    Public Function Eliminar(ByVal clasificacion As String, ByVal valor As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_del_Eliminar_ParametrosGenerales")
        db.AddInParameter(dbCommand, "@p_Clasificacion", DbType.String, clasificacion)
        db.AddInParameter(dbCommand, "@p_Valor", DbType.String, valor)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    'CMB OT 63063 20110523 REQ 15
    Public Function InsertarMotivoExtorno(ByVal nombre As String, ByVal descripcion As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_ins_InsertarMotivoExt_ParametrosGenerales")
        db.AddInParameter(dbCommand, "@p_Nombre", DbType.String, nombre)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    'CMB OT 63063 20110523 REQ 15
    Public Function Actualizar(ByVal clasificacion As String, ByVal valor As String, ByVal nombre As String, ByVal descripcion As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_upd_Actualizar_ParametrosGenerales")
        db.AddInParameter(dbCommand, "@p_Clasificacion", DbType.String, clasificacion)
        db.AddInParameter(dbCommand, "@p_Valor", DbType.String, valor)
        db.AddInParameter(dbCommand, "@p_Nombre", DbType.String, nombre)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
#End Region

#Region "/* Funciones personalizadas para Rating */"
    'OAB 20091109
    'Public Function InsertarRating(ByVal nombre As String, ByVal comentario As String, ByVal dataRequest As DataSet) As Integer    'HDG OT 62087 Nro14-R23 20110223
    Public Function InsertarRating(ByVal nombre As String, ByVal comentario As String, ByVal Factor As String, ByVal dataRequest As DataSet) As Integer    'HDG OT 62087 Nro14-R23 20110223
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_InsertarRating")
        db.AddInParameter(dbCommand, "@p_Nombre", DbType.String, nombre)
        db.AddInParameter(dbCommand, "@p_Comentario", DbType.String, IIf(comentario.Equals(""), DBNull.Value, comentario))
        db.AddOutParameter(dbCommand, "@p_Valor", DbType.Int16, 4)
        db.AddInParameter(dbCommand, "@p_Valor2", DbType.String, Factor)    'HDG OT 62087 Nro14-R23 20110223
        db.ExecuteNonQuery(dbCommand)
        Return CType(db.GetParameterValue(dbCommand, "@p_Valor"), Integer)
    End Function

    'OAB 20091105
    'HDG OT 62087 Nro14-R23 20110223
    'Public Function ModificarRating(ByVal nombre As String, ByVal valor As String, ByVal comentario As String, ByVal dataRequest As DataSet) As Integer
    Public Function ModificarRating(ByVal nombre As String, ByVal valor As String, ByVal comentario As String, ByVal factor As String, ByVal dataRequest As DataSet) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ModificarRating")
        db.AddInParameter(dbCommand, "@p_Nombre", DbType.String, nombre)
        db.AddInParameter(dbCommand, "@p_Valor", DbType.String, valor)
        db.AddInParameter(dbCommand, "@p_Comentario", DbType.String, IIf(comentario.Equals(""), DBNull.Value, comentario))
        db.AddInParameter(dbCommand, "@p_Valor2", DbType.String, factor)   'HDG OT 62087 Nro14-R23 20110223
        db.AddOutParameter(dbCommand, "@p_Out", DbType.Int16, 4)
        db.ExecuteNonQuery(dbCommand)
        Return CType(db.GetParameterValue(dbCommand, "@p_Out"), Integer)
    End Function

    'OAB 20091105
    Public Function EliminarRating(ByVal valor As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_EliminarRating")
        db.AddInParameter(dbCommand, "@p_Valor", DbType.String, valor)
        db.ExecuteNonQuery(dbCommand)
    End Function
    'OAB 20091109
    'Se implmenta para poder insertar registros a traves de un archivo excel
    Public Function ActualizarRatingPorExcel(ByVal dtDetalle As DataTable, ByVal tipo As Integer, ByVal dataRequest As DataSet, ByRef strmensaje As String) As String
        Dim strCodigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim Codigo, Rating As String
        Dim i, filaInicioExcel As Integer
        filaInicioExcel = 0
        i = 0


        Dim strTipoCodigo As String
        Dim flag As Boolean = True
        Dim strPestania As String
        Select Case tipo
            Case 2 'Emisiones
                strTipoCodigo = "Codigo SBS"
                strPestania = "Emisiones"
            Case 1 'Emisores
                strTipoCodigo = "Codigo de Emisor"
                strPestania = "Emisores"
        End Select

        For Each filaLinea As DataRow In dtDetalle.Rows
            If i >= filaInicioExcel Then

                Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ActualizarRatingPorExcel")

                Codigo = filaLinea(0).ToString().Trim()
                Rating = filaLinea(1).ToString().Trim()

                If (Not Codigo.Equals("")) And _
                    (Not Rating.Equals("")) Then

                    db.AddInParameter(dbCommand, "@p_Codigo", DbType.String, Codigo)
                    db.AddInParameter(dbCommand, "@p_Rating", DbType.String, Rating)
                    db.AddInParameter(dbCommand, "@p_Tipo", DbType.Int16, tipo)
                    db.AddOutParameter(dbCommand, "@p_Out", DbType.Int16, 4)

                    db.ExecuteNonQuery(dbCommand)

                    Dim intResult As Integer = CType(db.GetParameterValue(dbCommand, "@p_Out"), Integer)
                    If intResult <= 3 And intResult > 0 And flag Then
                        strmensaje &= "Los datos de " & strPestania & " cargaron con inconvenientes.\n"
                        flag = False
                    End If

                    Select Case intResult
                        Case 1
                            If strmensaje.IndexOf("Rating no existe: " & Rating) < 0 Then
                                strmensaje &= "\tRating no existe: " & Rating & "\n"
                            End If
                        Case 2
                            If strmensaje.IndexOf(strTipoCodigo & " no existe: " & Codigo) < 0 Then
                                strmensaje &= "\t" & strTipoCodigo & " no existe: " & Codigo & "\n"
                            End If
                        Case 3
                            If strmensaje.IndexOf(strTipoCodigo & " no existe: " & Codigo) < 0 Then
                                strmensaje &= "\t" & strTipoCodigo & " no existe: " & Codigo & "\n"
                            End If
                    End Select
                End If
            End If
            i = i + 1
        Next
        If flag Then
            strmensaje &= "Los datos de " & strPestania & " cargaron correctamente\n"
        End If
        Return strCodigo
    End Function
#End Region

#Region " /* Funciones Seleccionar */ "
    Public Function SeleccionarPorFiltro(ByVal clasificacion As String, ByVal nombre As String, ByVal valor As String, ByVal comentario As String, ByVal datarequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ParametrosGenerales_SeleccionarPorFiltro")
            db.AddInParameter(dbCommand, "@p_clasificacion", DbType.String, clasificacion)
            db.AddInParameter(dbCommand, "@p_Nombre", DbType.String, nombre)
            db.AddInParameter(dbCommand, "@p_Valor", DbType.String, valor)
            db.AddInParameter(dbCommand, "@p_Comentario", DbType.String, comentario)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function Listar(ByVal clasificacion As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ParametrosGenerales_Listar")
            db.AddInParameter(dbCommand, "@p_Clasificacion", DbType.String, clasificacion)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function ListarAgrupacion(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarAgrupacion")

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarSaldoBanco(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarCondicional")

        Return db.ExecuteDataSet(dbCommand)

    End Function

    'JHC REQ 66056: Implementacion Futuros
    Public Function ListarVencimiento(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ParametrosGenerales_ListarVencimiento")

        Return db.ExecuteDataSet(dbCommand)

    End Function

    Public Function ListarDefaultCentroCosto(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarDefaultCentroCosto")

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarDefaultTipoContabilidad(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarDefaultTipoContabilidad")

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarTipoFactor(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarFactor")

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarAplicarCastigo(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarCondicional")

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarValorBase(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarValorBase")

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarTopeLimite(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarTopeLimite")

        Return db.ExecuteDataSet(dbCommand)

    End Function

    Public Function ListarFondosInversion(ByVal dataRequest As DataSet, ByVal ordenFondo As String) As DataSet 'CMB OT 64769 20120323

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarTipoFondosInversion")
        db.AddInParameter(dbCommand, "@p_OrdenFondo", SqlDbType.VarChar, ordenFondo) 'CMB OT 64769 20120323

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarUnidadPosicion(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarUnidadesPosicion")

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarNaturaleza(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarNaturaleza")

        Return db.ExecuteDataSet(dbCommand)

    End Function

    Public Function ListarSituacion(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarSituacion")
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function

    'HDG OT 62087 Nro14-R23 20110223
    Public Function Listar_TipoFactor(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_ParametrosGenerales_TipoFactor")

        Return db.ExecuteDataSet(dbCommand)

    End Function

    Public Function ListarInterfaseBloomberg(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarHojasBloomberg")

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarRutaBloomberg(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarRutaBloomberg")

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarPosicion(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarPosicion")

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarTablaInstrumento(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarTablaInstrumento")

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarCampoInstrumento(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarCampoInstrumento")

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarSecuencial(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarSecuencial")

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarRelacion(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarRelacion")

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarCriterioTabla(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarCriterioTabla")

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarCriterioCampo(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarCriterioCampo")

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarMaduracion(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarMaduracion")

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarPeriodos(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarPeriodos")

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarTipoTasa(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarTipoTasa")

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarBaseCalculo(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarBaseCalculo")

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarTipoPrecio(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarTipoPrecio")

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarTipoTarifa(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarTipoTarifa")

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarTipoCalculo(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarTipoCalculoLimite")

        Return db.ExecuteDataSet(dbCommand)

    End Function

    Public Function ListarTipoPersona(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarTipoPersona")

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarRutaParametria(ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("RutaParametria_Listar")

        Return db.ExecuteDataSet(dbCommand)

    End Function
    Public Function ListarTipoIntermediario(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarTipoIntermediario")

        Return db.ExecuteDataSet(dbCommand)

    End Function

    Public Function ListarEntidadesExternas(ByVal Clasificacion As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGeneralesEntidadExt_listar")

        db.AddInParameter(dbCommand, "@Clasificacion", DbType.String, Clasificacion)

        Return db.ExecuteDataSet(dbCommand)
    End Function
    'OAB 20091103
    Public Function ListarParametros(ByVal Clasificacion As String, _
                                    ByVal Nombre As String, _
                                    ByVal Valor As String, _
                                    ByVal Comentario As String, _
                                    ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarParametros")

        db.AddInParameter(dbCommand, "@p_Clasificacion", DbType.String, IIf(Clasificacion.Equals(""), DBNull.Value, Clasificacion))
        db.AddInParameter(dbCommand, "@p_Nombre", DbType.String, IIf(Nombre.Equals(""), DBNull.Value, Nombre))
        db.AddInParameter(dbCommand, "@p_Valor", DbType.String, IIf(Valor.Equals(""), DBNull.Value, Valor))
        db.AddInParameter(dbCommand, "@p_Comentario", DbType.String, IIf(Comentario.Equals(""), DBNull.Value, Comentario))

        Return db.ExecuteDataSet(dbCommand)
    End Function
    'OAB 20091103
    Public Function ListarRating(ByVal Nombre As String, _
                                    ByVal Valor As String, _
                                    ByVal Comentario As String, _
                                    ByVal Local As String,
                                    ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarRating")

        db.AddInParameter(dbCommand, "@p_Nombre", DbType.String, IIf(Nombre.Equals(""), DBNull.Value, Nombre))
        db.AddInParameter(dbCommand, "@p_Valor", DbType.String, IIf(Valor.Equals(""), DBNull.Value, Valor))
        db.AddInParameter(dbCommand, "@p_Comentario", DbType.String, IIf(Comentario.Equals(""), DBNull.Value, Comentario))
        db.AddInParameter(dbCommand, "@p_Local", DbType.String, IIf(Local.Equals(""), DBNull.Value, Local))

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function ListarMatrizContable(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarMatrizContable")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarTipoContabilidad(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarTipoContabilidad")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarCompraVentaME_Mnemonico() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarCompraVentaME_Mnemonico")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarOrdenFondo(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarOrdenFondo")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarClaseLimite(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarClaseLimite")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarTipoLimite(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarTipoLimite")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarCalifInstr(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarCalifInstr")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarBaseTir(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarBaseTir")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarBaseTirNDias(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarBaseTirNDias")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarBaseCupon(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarBaseCupon")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarBaseCuponNDias(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarBaseCuponNDias")
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function SeleccionarTipoContabilidad(ByVal strCodigo As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_SelecccionarTipoContabilidad")
        db.AddInParameter(dbCommand, "@p_Valor", DbType.String, strCodigo)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarTipoAsignacion(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarTipoAsignacion")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    'CMB OT 61566 20101209
    Public Function ListarTipoAsignacionPorImporte(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_ParametrosGenerales_ListarTipoAsignacionPorImporte")
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function ListarMatriz(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGeneralaes_ListarMatriz")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarEncaje(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarEncaje")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarEstadosOI(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarEstadoOI")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarBaseImp(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarBaseImpuestosComisiones")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarAmortizacionVencimiento(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarTipoAmortizacionVencimiento")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarGrupoNormativo(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarGrupoNormativo")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ObtenerRutaModeloCarta(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ObtenerRutaModeloCarta")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarRutaGeneracionCartas() As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarRutaGeneracionCartas")
        Return Convert.ToString(db.ExecuteScalar(dbCommand))
    End Function

    Public Function ListarGrupoContable(ByVal CodigoTipoInstrumentoSBS As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_GrupoContable")
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumentoSBS", DbType.String, CodigoTipoInstrumentoSBS)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function ListarEntidadVinculadaGrupoEconomico(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_EntidadVinculada_GrupoEconomico")
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function ListarEntidadVinculadaEmisor(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_EntidadVinculada_Emisor")
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function ObtenerProximoSecuencial(ByVal tabla As String) As String

        Dim secuencial As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ObtenerProximoSecuencial")
        db.AddInParameter(dbCommand, "@m_Tabla", DbType.String, tabla)
        db.AddOutParameter(dbCommand, "@m_Secuencial", DbType.String, 50)
        Try
            db.ExecuteNonQuery(dbCommand)
            secuencial = db.GetParameterValue(dbCommand, "m_Secuencial").ToString()
        Catch ex As Exception
            Throw ex
        End Try

        Return secuencial
    End Function

    Public Function ListarGrupoIntermediario() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarGrupoIntermediario")
        Return db.ExecuteDataSet(dbCommand)
    End Function

    'CMB OT 62087 20110119 Nro 6
    Public Function ListarCondicionPrevOI() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_ParametrosGenerales_ListarCondicionPrevOI")
        Return db.ExecuteDataSet(dbCommand)
    End Function

    'CMB OT 62087 20110127 Nro 6
    Public Function ListarMedioNegociacionPrevOI(ByVal TipoRenta As String) As DataSet  'HDG OT 64291 20111021
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_ParametrosGenerales_ListarMedioNegPrevOI")
        db.AddInParameter(dbCommand, "@p_Valor2", DbType.String, TipoRenta)   'HDG OT 64291 20111021
        Return db.ExecuteDataSet(dbCommand)
    End Function

    'PLD 20110316 Nro 21
    Public Function ListarDerivadasLimitesInstrumentos() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_ParametrosGenerales_ListarDerivadasLimitesInstrumentos")
        Return db.ExecuteDataSet(dbCommand)
    End Function

    'CMB OT 63063 20110419 REQ 15
    Public Function ListarAprobadorCarta(ByVal codigoInterno As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_ListarAprobadorCarta_ParametrosGenerales")
        db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, codigoInterno)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    'CMB OT 63063 20110504 REQ 15
    'Public Function ListarFirmantesCartas() As DataSet
    '    Dim db As Database = DatabaseFactory.CreateDatabase
    '    Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_ListarFirmantes_ParametrosGenerales")
    '    Return db.ExecuteDataSet(dbCommand)
    'End Function

    'CMB OT 63063 20110504 REQ 15
    Public Function ListarClaveFirmantesCartas() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_ListarFirmantesClaves_ClaveFirmantesCarta")
        Return db.ExecuteDataSet(dbCommand)
    End Function

    'CMB OT 63063 20110523 REQ 15
    Public Function SeleccionarMotivoExtorno(ByVal Nombre As String, ByVal Valor As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_SeleccionarMotivoExtorno_ParametrosGenerales")
        db.AddInParameter(dbCommand, "@p_Nombre", DbType.String, Nombre)
        db.AddInParameter(dbCommand, "@p_Valor", DbType.String, Valor)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    'HDG OT 64291 20111128
    Public Function SeleccionarMedioTransmision(ByVal Nombre As String, ByVal Valor As String, ByVal tiporenta As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_SeleccionarMedioTransmision_ParametrosGenerales")
        db.AddInParameter(dbCommand, "@p_Nombre", DbType.String, Nombre)
        db.AddInParameter(dbCommand, "@p_Valor", DbType.String, Valor)
        db.AddInParameter(dbCommand, "@p_Valor2", DbType.String, tiporenta)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    'HDG OT 64291 20111128
    Public Function InsertarMedioTransmision(ByVal codigo As String, ByVal descripcion As String, ByVal tiporenta As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_ins_InsertarMedioTransmision_ParametrosGenerales")
        db.AddInParameter(dbCommand, "@p_Valor", DbType.String, codigo)
        db.AddInParameter(dbCommand, "@p_Nombre", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Valor2", DbType.String, tiporenta)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    'HDG OT 64291 20111128
    Public Function ActualizarMedioTransmision(ByVal codigo As String, ByVal descripcion As String, ByVal tiporenta As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_upd_ActualizarMedioTransmision_ParametrosGenerales")
        db.AddInParameter(dbCommand, "@p_Valor", DbType.String, codigo)
        db.AddInParameter(dbCommand, "@p_Nombre", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Valor2", DbType.String, tiporenta)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    'CMB OT 65473 20120618
    Public Function SeleccionarCategReporte(ByVal codCategReporte As String, ByVal codReporte As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ParametrosGenerales_SeleccionarCategReporte")
        db.AddInParameter(dbCommand, "@p_codCategReporte", DbType.String, codCategReporte)
        db.AddInParameter(dbCommand, "@p_codReporte", DbType.String, codReporte)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    'REQ 66768 JCH 20130201
    Public Function ListarSubLista(ByVal clasificacion As String, ByVal valor2 As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ParametrosGenerales_ListarSubLista")
        db.AddInParameter(dbCommand, "@p_clasificacion", DbType.String, clasificacion)
        db.AddInParameter(dbCommand, "@p_valor2", DbType.String, valor2)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se implementa función para capturar valores de tabla Parametros Generales | 17/05/18 
    Public Function ListarTipoRentaRiesgo(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarTipoRentaRiesgo")
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Se implementa función para capturar valores de tabla Parametros Generales | 17/05/18 


#End Region
End Class