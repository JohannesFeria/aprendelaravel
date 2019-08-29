Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Configuration
Imports System.Reflection
Imports System.Collections.Generic

Public Class BaseDAM
    'Protected SqlConnectionDatabase As String = ConfigurationSettings.AppSettings("BDRuta").ToString()
    'Protected dbBase As SqlDatabase
    'Protected dbCommand As DbCommand
    'Protected sqlCommand As String = ""

    'Public Sub New()
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    'End Sub

    ''' <summary> Crea y retorna una lista Generica a partir de un DataTable  </summary>
    Public Shared Function ConvertToList(Of ClassType As New)(ByVal dt As DataTable) As List(Of ClassType)

        Dim oLista As New List(Of ClassType)()
        Dim itemType As Type = GetType(ClassType) ' Obtiene informacion de propiedades para los item de la Lista
        Dim bindingFlags_1 As BindingFlags = BindingFlags.Instance Or BindingFlags.[Public] ' Solo Publicas no propiedades heredadas

        ' Averigua columnas de la tabla que son propiedades de ClassType
        Dim Disponible As New Dictionary(Of String, PropertyInfo)()
        For Each column As DataColumn In dt.Columns
            Dim prop As PropertyInfo = itemType.GetProperty(column.ColumnName, bindingFlags_1)
            If prop IsNot Nothing Then Disponible.Add(column.ColumnName, prop)
        Next

        For Each row As DataRow In dt.Rows ' Llena la lista generica con data
            Dim item As New ClassType()
            For Each Disp As KeyValuePair(Of String, PropertyInfo) In Disponible
                Dim propValue As Object = row(Disp.Key)
                If propValue IsNot Nothing AndAlso propValue IsNot System.DBNull.Value Then
                    Disp.Value.SetValue(item, propValue, Nothing)
                End If
            Next
            oLista.Add(item)
        Next

        Return oLista
    End Function

    ''' <summary> Crea y retorna una lista Generica a partir de un DataTable  </summary>
    Public Shared Function ConvertToList(Of ClassType As New)(ByVal dt As IDataReader) As List(Of ClassType)

        Dim oLista As New List(Of ClassType)()
        Dim itemType As Type = GetType(ClassType) ' Obtiene informacion de propiedades para los item de la Lista
        Dim bindingFlags_1 As BindingFlags = BindingFlags.Instance Or BindingFlags.[Public] ' Solo Publicas no propiedades heredadas

        ' Averigua columnas de la tabla que son propiedades de ClassType
        Dim Disponible As New Dictionary(Of String, PropertyInfo)()
        Dim i As Integer

        For i = 0 To i < dt.FieldCount
            Dim prop As PropertyInfo = itemType.GetProperty(dt.GetName(i), bindingFlags_1)
            If prop IsNot Nothing Then Disponible.Add(dt.GetName(i), prop)
        Next

        While dt.Read
            Dim item As New ClassType()
            For Each Disp As KeyValuePair(Of String, PropertyInfo) In Disponible
                Dim propValue As Object = dt(Disp.Key) 'row(Disp.Key)
                If propValue IsNot Nothing AndAlso propValue IsNot System.DBNull.Value Then
                    Disp.Value.SetValue(item, propValue, Nothing)
                End If
            Next
            oLista.Add(item)
        End While

        Return oLista
    End Function


End Class