Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Imports System.Text
Imports System.Collections.Generic

Public Class ImportadorExcelDAM

    Private sqlCommand As String = ""
    Private oRow As TipoValorizacionBE.TipoValorizacionRow

    Public Sub New()

    End Sub

#Region " /* Funciones Insertar */ "

    ''' <summary>
    ''' CRumiche: Guarda la tabla en Base de Datos según la configuración especificada
    ''' </summary>
    ''' <param name="infoTabla">Información de la Metadata de la Tabla (Proviene de ImportadorExcelBE.DtListaTablas)</param>
    ''' <param name="columnasTabla">Definición de Columnas de la Tabla</param>
    ''' <param name="datos">Datos a guardar en el repositorio</param>
    Public Sub ImportarHaciaTabla(ByVal infoTabla As DataRow, ByVal columnasTabla As DataRow(), ByVal datos As DataTable)
        Dim sqlModelo As New StringBuilder, sqlCOLUMNAS As New StringBuilder, sqlCOLUMNA_IGUAL_VALOR As New StringBuilder
        '1. CRumiche: Iniciamos elaborando las plantillas del QUERY
        Dim columnasUnique As List(Of String) = ImportadorExcelBE.ObtenerListaColumnasUniqueValida(infoTabla("ColumnasUnique").ToString(), columnasTabla)
        'Si no hay "RollbackPorDuplicidad" elaboramos la query necesaria para OMITIR los existentes
        Dim omitirRegistroExistente As Boolean = (infoTabla("RollbackPorDuplicidad").ToString().ToUpper().Equals("NO") And columnasUnique.Count > 0)

        If omitirRegistroExistente Then sqlModelo.AppendLine("IF NOT EXISTS(SELECT 1 FROM {_NOMBRE_TABLA_} WHERE {_COLUMNA_IGUAL_VALOR_} ) ")

        sqlModelo.AppendLine("INSERT INTO {_NOMBRE_TABLA_} ( ")
        sqlModelo.AppendLine("{_COLUMNAS_}")
        sqlModelo.AppendLine(") VALUES ( ")
        sqlModelo.AppendLine("{_VALORES_}")
        sqlModelo.AppendLine("); ")

        '2. CRumiche: Hasta aqui ya tenemos las plantillas del QUERY, ahora pasamos a poblarlo con definiciones de la tabla        
        If omitirRegistroExistente Then
            Dim separadorAND As String = "" 'Inicializamos el SEPARADOR (Solo para la primera ITERACIÓN)
            For Each nombreCol As String In columnasUnique
                sqlCOLUMNA_IGUAL_VALOR.Append(" " & separadorAND & " [" & nombreCol & "] = '[" & nombreCol & "]_VAL'")
                separadorAND = "AND" 'Se mantiene constante despues de la PRIMERA LINEA
            Next
        End If

        Dim separadorCOMA As String = "" 'Inicializamos el SEPARADOR (Solo para la primera ITERACIÓN)
        For Each col As DataRow In columnasTabla
            sqlCOLUMNAS.AppendLine(" " & separadorCOMA & "[" & col("NombreColumnaBD") & "] ")
            separadorCOMA = "," 'Se mantiene constante despues de la PRIMERA LINEA
        Next

        sqlModelo.Replace("{_NOMBRE_TABLA_}", infoTabla("NombreTablaBD"))
        sqlModelo.Replace("{_COLUMNAS_}", sqlCOLUMNAS.ToString())

        '3. CRumiche: Completamos el query con los VALORES y pasamos a su EJECUCION
        Dim sqlInsert As New StringBuilder(), sqlVALORES As New StringBuilder
        Dim acotadorDatos As String = "'" 'Por defaul es el de TEXTOS (No presentaria problemas en ningun tipo de datos)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim numberFila As Integer = 5

        Using dbCommand As DbCommand = db.GetSqlStringCommand("SELECT 1")
            For Each filaActual As DataRow In datos.Rows
                'Inicializamos el query INSERT
                sqlInsert.Clear()
                sqlInsert.Append(sqlModelo.ToString())

                'Unificamos el query de Omitrir Existentes si se requiere
                If omitirRegistroExistente Then
                    Dim auxColumnaIgualValor As String = sqlCOLUMNA_IGUAL_VALOR.ToString()
                    For Each colUnique As String In columnasUnique
                        auxColumnaIgualValor = auxColumnaIgualValor.Replace("[" & colUnique & "]_VAL", filaActual(colUnique))
                    Next
                    sqlInsert.Replace("{_COLUMNA_IGUAL_VALOR_}", auxColumnaIgualValor)
                End If

                'Unificamos el query de VALORES del INSERT
                sqlVALORES.Clear() 'Inicializamos el QUERY de VALORES
                separadorCOMA = "" 'Inicializamos el SEPARADOR (Solo para la primera ITERACIÓN)
                For Each col As DataRow In columnasTabla
                    'Dim tipoDatoCol As String = filaActual(col("TipoDato"))
                    sqlVALORES.AppendLine(" " & separadorCOMA & acotadorDatos & filaActual(col("NombreColumnaBD")) & acotadorDatos)
                    separadorCOMA = "," 'Se mantiene constante despues de la PRIMERA LINEA
                Next
                sqlInsert.Replace("{_VALORES_}", sqlVALORES.ToString())

                'Realizamos la EJECUCION
                dbCommand.CommandText = sqlInsert.ToString()
                'db.ExecuteNonQuery(dbCommand, trans) ' // Ejecutamos con transacción
                Try
                    db.ExecuteNonQuery(dbCommand) ' // Ejecutamos con transacción
                Catch ex As Exception
                    Throw New ImportadorExcelExceptionBE("Error al Guardar los registros en la tabla: [" & infoTabla("NombreTablaBD") & "] Pestaña:[" & infoTabla(1) & "] Fila:[" & numberFila & "] => " & ex.Message)
                End Try
                numberFila += 1
            Next
        End Using


            'Dim sqlModelo As New StringBuilder, sqlCOLUMNAS As New StringBuilder, sqlCOLUMNA_IGUAL_VALOR As New StringBuilder
            ''1. CRumiche: Iniciamos elaborando las plantillas del QUERY
            'Dim columnasUnique As List(Of String) = ImportadorExcelBE.ObtenerListaColumnasUniqueValida(infoTabla("ColumnasUnique").ToString(), columnasTabla)
            ''Si no hay "RollbackPorDuplicidad" elaboramos la query necesaria para OMITIR los existentes
            'Dim omitirRegistroExistente As Boolean = (infoTabla("RollbackPorDuplicidad").ToString().ToUpper().Equals("NO") And columnasUnique.Count > 0)

            'If omitirRegistroExistente Then sqlModelo.AppendLine("IF NOT EXISTS(SELECT 1 FROM {_NOMBRE_TABLA_} WHERE {_COLUMNA_IGUAL_VALOR_} ) ")

            'sqlModelo.AppendLine("INSERT INTO {_NOMBRE_TABLA_} ( ")
            'sqlModelo.AppendLine("{_COLUMNAS_}")
            'sqlModelo.AppendLine(") VALUES ( ")
            'sqlModelo.AppendLine("{_VALORES_}")
            'sqlModelo.AppendLine("); ")

            ''2. CRumiche: Hasta aqui ya tenemos las plantillas del QUERY, ahora pasamos a poblarlo con definiciones de la tabla        
            'If omitirRegistroExistente Then
            '    Dim separadorAND As String = "" 'Inicializamos el SEPARADOR (Solo para la primera ITERACIÓN)
            '    For Each nombreCol As String In columnasUnique
            '        sqlCOLUMNA_IGUAL_VALOR.Append(" " & separadorAND & " [" & nombreCol & "] = '[" & nombreCol & "]_VAL'")
            '        separadorAND = "AND" 'Se mantiene constante despues de la PRIMERA LINEA
            '    Next
            'End If

            'Dim separadorCOMA As String = "" 'Inicializamos el SEPARADOR (Solo para la primera ITERACIÓN)
            'For Each col As DataRow In columnasTabla
            '    sqlCOLUMNAS.AppendLine(" " & separadorCOMA & "[" & col("NombreColumnaBD") & "] ")
            '    separadorCOMA = "," 'Se mantiene constante despues de la PRIMERA LINEA
            'Next

            'sqlModelo.Replace("{_NOMBRE_TABLA_}", infoTabla("NombreTablaBD"))
            'sqlModelo.Replace("{_COLUMNAS_}", sqlCOLUMNAS.ToString())

            ''3. CRumiche: Completamos el query con los VALORES y pasamos a su EJECUCION
            'Dim sqlInsert As New StringBuilder(), sqlVALORES As New StringBuilder
            'Dim acotadorDatos As String = "'" 'Por defaul es el de TEXTOS (No presentaria problemas en ningun tipo de datos)
            'Dim db As Database = DatabaseFactory.CreateDatabase()

            'Using conn As DbConnection = db.CreateConnection()
            '    conn.Open()
            '    Dim trans As DbTransaction = conn.BeginTransaction()

            '    Try
            '        Using dbCommand As DbCommand = db.GetSqlStringCommand("SELECT 1")
            '            For Each filaActual As DataRow In datos.Rows
            '                'Inicializamos el query INSERT
            '                sqlInsert.Clear()
            '                sqlInsert.Append(sqlModelo.ToString())

            '                'Unificamos el query de Omitrir Existentes si se requiere
            '                If omitirRegistroExistente Then
            '                    Dim auxColumnaIgualValor As String = sqlCOLUMNA_IGUAL_VALOR.ToString()
            '                    For Each colUnique As String In columnasUnique
            '                        auxColumnaIgualValor = auxColumnaIgualValor.Replace("[" & colUnique & "]_VAL", filaActual(colUnique))
            '                    Next
            '                    sqlInsert.Replace("{_COLUMNA_IGUAL_VALOR_}", auxColumnaIgualValor)
            '                End If

            '                'Unificamos el query de VALORES del INSERT
            '                sqlVALORES.Clear() 'Inicializamos el QUERY de VALORES
            '                separadorCOMA = "" 'Inicializamos el SEPARADOR (Solo para la primera ITERACIÓN)
            '                For Each col As DataRow In columnasTabla
            '                    'Dim tipoDatoCol As String = filaActual(col("TipoDato"))
            '                    sqlVALORES.AppendLine(" " & separadorCOMA & acotadorDatos & filaActual(col("NombreColumnaBD")) & acotadorDatos)
            '                    separadorCOMA = "," 'Se mantiene constante despues de la PRIMERA LINEA
            '                Next
            '                sqlInsert.Replace("{_VALORES_}", sqlVALORES.ToString())

            '                'Realizamos la EJECUCION
            '                dbCommand.CommandText = sqlInsert.ToString()
            '                db.ExecuteNonQuery(dbCommand, trans) ' // Ejecutamos con transacción
            '            Next

            '            trans.Commit()
            '        End Using
            '    Catch ex As Exception
            '        trans.Rollback()
            '        Throw ex
            '    End Try
            'End Using

    End Sub

    Public Sub ImportarInformacion(ByVal sqlQuery As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ImportarExcel_Migracion")
            db.AddInParameter(dbCommand, "@p_Query", DbType.String, sqlQuery)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Function GenerarCodigoCorrelativos(ByVal p_NameTable As String) As String
        GenerarCodigoCorrelativos = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ImportarMigracion_GenerarCorrelativoSegunTabla")
            db.AddInParameter(dbCommand, "@p_TableName", DbType.String, p_NameTable)
            GenerarCodigoCorrelativos = CType(db.ExecuteScalar(dbCommand), String)
        End Using
    End Function
#End Region

End Class