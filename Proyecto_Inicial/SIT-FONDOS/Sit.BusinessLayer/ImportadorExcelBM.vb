Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Transactions
Imports System.Text
Imports System.Collections.Generic

Public Class ImportadorExcelBM
    Inherits InvokerCOM
    Dim objImportar As ImportadorExcelDAM

    Public Sub New()

    End Sub

#Region " /* Funciones Insertar */ "

    Public Sub GuardarImportacion(ByVal objImportador As ImportadorExcelBE)
        'Validacion de Duplicidad contra Base de Datos
        For Each infoTabla As DataRow In objImportador.DtListaTablas.Rows
            If infoTabla("RollbackPorDuplicidad").ToString.Equals("SI") Then 'Para las tablas que lo requieran
                'VALIDAR DUPLICIDAD CONTRA LA BASE DE DATOS (FILA X FILA)
                'SE LANZRIA UNA EXCEPCION DE TIPO VALIDACION
            End If
        Next

        Using tran As New TransactionScope
            PrepareTables(objImportador)
            ' Realizamos el Proceso de Registro de cada tabla
            For Each datos As DataTable In objImportador.TablasCargadas
                Try
                    Dim columnasTabla As DataRow() = objImportador.DtEstructuraTablas.Select(" NombreTablaBD = '" & datos.TableName & "' ")
                    Dim infoTabla As DataRow = objImportador.DtListaTablas.Select(" NombreTablaBD = '" & datos.TableName & "' ")(0)

                    If infoTabla(9).ToString = "SI" Then
                        Dim objDAM As New ImportadorExcelDAM
                        objDAM.ImportarHaciaTabla(infoTabla, columnasTabla, datos)
                    Else
                        ImportarInformacion(infoTabla, columnasTabla, datos)
                    End If
                Catch ex As Exception
                    Throw New ImportadorExcelExceptionBE("Error al Guardar los registros en la tabla: [" & datos.TableName & "] => " & ex.Message)
                    '  Throw ex
                End Try
            Next
            tran.Complete()
        End Using
    End Sub

    Private Sub PrepareTables(ByRef objImportador As ImportadorExcelBE)
        For Each dtDatos As DataTable In objImportador.TablasCargadas
            Dim infoTabla As DataRow = objImportador.DtListaTablas.Select(" NombreTablaBD = '" & dtDatos.TableName & "' ")(0)
            If infoTabla(4).ToString() = "SI" Then 'Verificar si existen tablas con PK autogeneradas
                Dim correlativo As Integer = 0
                Dim codigoCorrelativo As Integer = 0
                Dim objImportar As New ImportadorExcelDAM
                codigoCorrelativo = Integer.Parse(objImportar.GenerarCodigoCorrelativos(dtDatos.TableName))
                Dim nomTabForanea() As String = Nothing
                Dim nomClaveForanea() As String = Nothing
                nomTabForanea = infoTabla(7).ToString().Split(",")
                nomClaveForanea = infoTabla(8).ToString().Split(",")
                Dim i As Integer = 0
                For i = 0 To nomTabForanea.Length - 1
                    Dim positionTable As Integer = 0
                    positionTable = ExistTable(objImportador.TablasCargadas, nomTabForanea(i))
                    If positionTable > 0 Then
                        If objImportador.TablasCargadas(positionTable).Rows.Count > 0 Then
                            correlativo = 0
                            For Each drDatos As DataRow In dtDatos.Rows
                                For Each drAux As DataRow In objImportador.TablasCargadas(positionTable).Rows
                                    If drDatos(infoTabla(6).ToString()).ToString() = drAux(nomClaveForanea(i)).ToString() Then
                                        drAux(nomClaveForanea(i)) = (codigoCorrelativo + correlativo).ToString("000000")
                                        'Exit For
                                    End If
                                Next
                                correlativo += 1
                            Next
                        End If
                    End If
                Next
                correlativo = 0
                For Each drDatos As DataRow In dtDatos.Rows
                    drDatos(infoTabla(6).ToString()) = (codigoCorrelativo + correlativo).ToString("000000")
                    correlativo += 1
                Next
            End If
        Next

    End Sub

    Private Function ExistTable(ByVal dt As List(Of DataTable), ByVal nameTable As String) As Integer
        ExistTable = -1
        Dim contador As Integer = 0
        For Each dtAux As DataTable In dt
            If dtAux.TableName = nameTable Then
                ExistTable = contador
                Exit For
            End If
            contador += 1
        Next
    End Function

    'OTXXXX Ian Pastor M. 15/09/2018.
    'Objetivo: Importar la información de la plantilla de migración.
    'Se tomó como base el código desarrollado por CRUMICHE.
    Public Sub ImportarInformacion(ByVal infoTabla As DataRow, ByVal columnasTabla As DataRow(), ByVal datos As DataTable)
        objImportar = New ImportadorExcelDAM
        Dim sqlModelo As New StringBuilder, sqlCOLUMNAS As New StringBuilder, sqlCOLUMNA_IGUAL_VALOR As New StringBuilder

        Dim columnasUnique As List(Of String) = ImportadorExcelBE.ObtenerListaColumnasUniqueValida(infoTabla("ColumnasUnique").ToString(), columnasTabla)

        Dim omitirRegistroExistente As Boolean = (infoTabla("RollbackPorDuplicidad").ToString().ToUpper().Equals("NO") And columnasUnique.Count > 0)

        If omitirRegistroExistente Then sqlModelo.AppendLine("IF NOT EXISTS(SELECT 1 FROM {_NOMBRE_TABLA_} WHERE {_COLUMNA_IGUAL_VALOR_} ) ")

        sqlModelo.AppendLine("INSERT INTO {_NOMBRE_TABLA_} ( ")
        sqlModelo.AppendLine("{_COLUMNAS_}")
        sqlModelo.AppendLine(") VALUES ( ")
        sqlModelo.AppendLine("{_VALORES_}")
        sqlModelo.AppendLine("); ")

        If omitirRegistroExistente Then
            Dim separadorAND As String = "" 'Inicializar el SEPARADOR
            For Each nombreCol As String In columnasUnique
                sqlCOLUMNA_IGUAL_VALOR.Append(" " & separadorAND & " [" & nombreCol & "] = '[" & nombreCol & "]_VAL'")
                separadorAND = "AND"
            Next
        End If

        Dim separadorCOMA As String = "" 'Inicializar el SEPARADOR
        For Each col As DataRow In columnasTabla
            sqlCOLUMNAS.AppendLine(" " & separadorCOMA & "[" & col("NombreColumnaBD") & "] ")
            separadorCOMA = ","
        Next

        sqlModelo.Replace("{_NOMBRE_TABLA_}", infoTabla("NombreTablaBD"))
        sqlModelo.Replace("{_COLUMNAS_}", sqlCOLUMNAS.ToString())

        'Completar query con los VALORES
        Dim sqlInsert As New StringBuilder(), sqlVALORES As New StringBuilder
        Dim acotadorDatos As String = "'" 'Por defaul es el de TEXTOS (No presentaria problemas en ningun tipo de datos)
        Dim numberFila As Integer = 5
        For Each filaActual As DataRow In datos.Rows
            'Inicializar el query INSERT
            sqlInsert.Clear()
            sqlInsert.Append(sqlModelo.ToString())

            'Unificar el query de Omitrir Existentes si se requiere
            If omitirRegistroExistente Then
                Dim auxColumnaIgualValor As String = sqlCOLUMNA_IGUAL_VALOR.ToString()
                For Each colUnique As String In columnasUnique
                    auxColumnaIgualValor = auxColumnaIgualValor.Replace("[" & colUnique & "]_VAL", filaActual(colUnique))
                Next
                sqlInsert.Replace("{_COLUMNA_IGUAL_VALOR_}", auxColumnaIgualValor)
            End If

            'Dim correlativo As String = String.Empty
            'If infoTabla("PK_Autogenerado").ToString() = "SI" Then
            '    Dim objImportar As New ImportadorExcelDAM
            '    correlativo = objImportar.GenerarCodigoCorrelativos("OrdenInversion")
            'End If

            'Unificar el query de VALORES del INSERT
            sqlVALORES.Clear() 'Inicializar el QUERY de VALORES
            separadorCOMA = "" 'Inicializar el SEPARADOR
            For Each col As DataRow In columnasTabla
                'If infoTabla("PK_NombreColumna").ToString() = col(1).ToString Then 'Identificar nombre columna PK
                '    sqlVALORES.AppendLine(" " & separadorCOMA & acotadorDatos & correlativo & acotadorDatos)
                'Else
                '    sqlVALORES.AppendLine(" " & separadorCOMA & acotadorDatos & filaActual(col("NombreColumnaBD")) & acotadorDatos)
                'End If
                sqlVALORES.AppendLine(" " & separadorCOMA & acotadorDatos & filaActual(col("NombreColumnaBD")) & acotadorDatos)
                separadorCOMA = ","
            Next

            sqlInsert.Replace("{_VALORES_}", sqlVALORES.ToString())

            'Dim sqlQueryXML As String = ""
            'sqlQueryXML = "<Querys>"
            'sqlQueryXML &= "<Query>" & sqlInsert.ToString() & "</Query>"
            'sqlQueryXML &= "</Querys>"
            Try
                objImportar.ImportarInformacion(sqlInsert.ToString())
            Catch ex As Exception
                Throw New ImportadorExcelExceptionBE("Error al Guardar los registros en la tabla: [" & infoTabla("NombreTablaBD") & "] Pestaña:[" & infoTabla(1) & "] Fila:[" & numberFila & "] => " & ex.Message)
                'Throw New System.Exception("Error al guardar los registros de la pestaña " & infoTabla(1) & " 
            End Try
            numberFila += 1
        Next
    End Sub

#End Region

End Class

