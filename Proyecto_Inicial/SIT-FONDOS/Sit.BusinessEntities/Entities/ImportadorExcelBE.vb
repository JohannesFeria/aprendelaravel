Imports System.Collections.Generic

' INICIO | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-09-06 | Importador
'Descripcion: Clase Necesaria pare el Importador
<Serializable()>
Public Class ImportadorExcelBE

    Public Class ValoresConfigExcel
        ' /*CRumiche: Columnas para LEER la lista de tablas a cargar del excel*/
        Public DefTablas_FilaInicio As Integer = 17 '/* Según el formato diseñado */
        Public DefTablas_MaxFilas As Integer = 100 '/* Configurable */
        Public DefTablas_ColNombreTabla As Integer = 3 '/* Según el formato diseñado */
        Public DefTablas_ColPestania As Integer = 4 '/* Según el formato diseñado */
        Public DefTablas_ColImportar As Integer = 5 '/* Según el formato diseñado */
        Public DefTablas_ColColumnasUnique As Integer = 6 '/* Según el formato diseñado */
        Public DefTablas_ColRollbackDuplicidad As Integer = 7  '/* Según el formato diseñado */
        Public DefTablas_ColPK_Autogenerado As Integer = 8  '/* Según el formato diseñado */
        Public DefTablas_ColLlavesForaneas As Integer = 9  '/* Según el formato diseñado */
        Public DefTablas_ColPK_NombreColumna As Integer = 10  '/* Según el formato diseñado */
        Public DefTablas_ColNombreTablaForanea As Integer = 11  '/* Según el formato diseñado */
        Public DefTablas_ColNombreClaveForanea As Integer = 12  '/* Según el formato diseñado */
        Public DefTablas_ColCargaPorScripts As Integer = 13  '/* Según el formato diseñado */
        Public DefTablas_ColGrupoDependenciaTablas As Integer = 14  '/* Según el formato diseñado */
        Public DefTablas_ColSecuenciaDependencia As Integer = 15  '/* Según el formato diseñado */

        ' /*CRumiche: Columnas para LEER la estructura de cada tabla y la data (por cada pestaña)*/
        Public TabTabla_FilaNombreColumnaBD As Integer
        Public TabTabla_FilaTipoDato As Integer
        Public TabTabla_FilaInicioData As Integer
        Public TabTabla_MaxColumnasPorTabla As Integer
    End Class

    Public ConfigExcel As ValoresConfigExcel
    Public DtListaTablas As DataTable
    Public DtEstructuraTablas As DataTable
    Public TablasCargadas As List(Of DataTable)

    Public Sub New()
        Me.ConfigExcel = New ValoresConfigExcel()

        Me.DtListaTablas = New DataTable()
        Me.DtListaTablas.Columns.Add("NombreTablaBD")
        Me.DtListaTablas.Columns.Add("Pestania")
        Me.DtListaTablas.Columns.Add("ColumnasUnique")
        Me.DtListaTablas.Columns.Add("RollbackPorDuplicidad")
        Me.DtListaTablas.Columns.Add("PK_Autogenerado")
        Me.DtListaTablas.Columns.Add("LlavesForaneas")
        Me.DtListaTablas.Columns.Add("PK_NombreColumna")
        Me.DtListaTablas.Columns.Add("NombreTablaForanea")
        Me.DtListaTablas.Columns.Add("NombreClaveForanea")
        Me.DtListaTablas.Columns.Add("CargaPorScripts")

        Me.DtEstructuraTablas = New DataTable()
        Me.DtEstructuraTablas.Columns.Add("NombreTablaBD")
        Me.DtEstructuraTablas.Columns.Add("NombreColumnaBD")
        Me.DtEstructuraTablas.Columns.Add("TipoDato")
        Me.DtEstructuraTablas.Columns.Add("IndiceColEnExcel")

        Me.TablasCargadas = New List(Of DataTable)
    End Sub

    ''' <summary>
    ''' Split para obtener columnas validando q cada item devuelto tenga contenido
    ''' </summary>
    ''' <param name="columasUnique"></param>
    Public Shared Function ObtenerListaColumnasUnique(ByVal columasUnique As String) As List(Of String)
        Dim resul As New List(Of String)
        If columasUnique.Trim().Length > 0 Then
            For Each col As String In columasUnique.Split(",".ToCharArray())
                resul.Add(col.Trim())
            Next
        End If
        Return resul
    End Function

    ''' <summary>
    ''' Split para obtener columnas validando q cada item devuelto tenga contenido y cada columna esté en la Lista de Columnas de la Tabla
    ''' </summary>
    ''' <param name="columasUnique"></param>
    Public Shared Function ObtenerListaColumnasUniqueValida(ByVal columasUnique As String, ByVal columnasTabla As DataRow()) As List(Of String)
        Dim resulIni As List(Of String) = ObtenerListaColumnasUnique(columasUnique)
        Dim resulFin As New List(Of String)

        For i As Integer = 0 To resulIni.Count - 1
            resulIni(i) = resulIni(i).ToUpper()
        Next

        For Each colTabla As DataRow In columnasTabla
            If resulIni.Contains(colTabla("NombreColumnaBD").ToString().ToUpper()) Then
                'Intencionalmente agregamos el NOMBRE DE COLUMNA tal cual esté en la lista de columnas y no en el UNIQUE
                resulFin.Add(colTabla("NombreColumnaBD"))
            End If
        Next

        Return resulFin
    End Function

End Class

Public Class ImportadorExcelExceptionBE
    Inherits Exception

    Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub
End Class
' FIN | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-09-06 | Importador

