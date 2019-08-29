﻿'------------------------------------------------------------------------------
' <autogenerated>
'     This code was generated by a tool.
'     Runtime Version: 1.1.4322.573
'
'     Changes to this file may cause incorrect behavior and will be lost if 
'     the code is regenerated.
' </autogenerated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Runtime.Serialization
Imports System.Xml


<Serializable(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.Diagnostics.DebuggerStepThrough(),  _
 System.ComponentModel.ToolboxItem(true)>  _
Public Class ReportesContabilidadBE
    Inherits DataSet
    
    Private tableReporteContabilidad As ReporteContabilidadDataTable
    
    Public Sub New()
        MyBase.New
        Me.InitClass
        Dim schemaChangedHandler As System.ComponentModel.CollectionChangeEventHandler = AddressOf Me.SchemaChanged
        AddHandler Me.Tables.CollectionChanged, schemaChangedHandler
        AddHandler Me.Relations.CollectionChanged, schemaChangedHandler
    End Sub
    
    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.New
        Dim strSchema As String = CType(info.GetValue("XmlSchema", GetType(System.String)),String)
        If (Not (strSchema) Is Nothing) Then
            Dim ds As DataSet = New DataSet
            ds.ReadXmlSchema(New XmlTextReader(New System.IO.StringReader(strSchema)))
            If (Not (ds.Tables("ReporteContabilidad")) Is Nothing) Then
                Me.Tables.Add(New ReporteContabilidadDataTable(ds.Tables("ReporteContabilidad")))
            End If
            Me.DataSetName = ds.DataSetName
            Me.Prefix = ds.Prefix
            Me.Namespace = ds.Namespace
            Me.Locale = ds.Locale
            Me.CaseSensitive = ds.CaseSensitive
            Me.EnforceConstraints = ds.EnforceConstraints
            Me.Merge(ds, false, System.Data.MissingSchemaAction.Add)
            Me.InitVars
        Else
            Me.InitClass
        End If
        Me.GetSerializationData(info, context)
        Dim schemaChangedHandler As System.ComponentModel.CollectionChangeEventHandler = AddressOf Me.SchemaChanged
        AddHandler Me.Tables.CollectionChanged, schemaChangedHandler
        AddHandler Me.Relations.CollectionChanged, schemaChangedHandler
    End Sub
    
    <System.ComponentModel.Browsable(false),  _
     System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)>  _
    Public ReadOnly Property ReporteContabilidad As ReporteContabilidadDataTable
        Get
            Return Me.tableReporteContabilidad
        End Get
    End Property
    
    Public Overrides Function Clone() As DataSet
        Dim cln As ReportesContabilidadBE = CType(MyBase.Clone,ReportesContabilidadBE)
        cln.InitVars
        Return cln
    End Function
    
    Protected Overrides Function ShouldSerializeTables() As Boolean
        Return false
    End Function
    
    Protected Overrides Function ShouldSerializeRelations() As Boolean
        Return false
    End Function
    
    Protected Overrides Sub ReadXmlSerializable(ByVal reader As XmlReader)
        Me.Reset
        Dim ds As DataSet = New DataSet
        ds.ReadXml(reader)
        If (Not (ds.Tables("ReporteContabilidad")) Is Nothing) Then
            Me.Tables.Add(New ReporteContabilidadDataTable(ds.Tables("ReporteContabilidad")))
        End If
        Me.DataSetName = ds.DataSetName
        Me.Prefix = ds.Prefix
        Me.Namespace = ds.Namespace
        Me.Locale = ds.Locale
        Me.CaseSensitive = ds.CaseSensitive
        Me.EnforceConstraints = ds.EnforceConstraints
        Me.Merge(ds, false, System.Data.MissingSchemaAction.Add)
        Me.InitVars
    End Sub
    
    Protected Overrides Function GetSchemaSerializable() As System.Xml.Schema.XmlSchema
        Dim stream As System.IO.MemoryStream = New System.IO.MemoryStream
        Me.WriteXmlSchema(New XmlTextWriter(stream, Nothing))
        stream.Position = 0
        Return System.Xml.Schema.XmlSchema.Read(New XmlTextReader(stream), Nothing)
    End Function
    
    Friend Sub InitVars()
        Me.tableReporteContabilidad = CType(Me.Tables("ReporteContabilidad"),ReporteContabilidadDataTable)
        If (Not (Me.tableReporteContabilidad) Is Nothing) Then
            Me.tableReporteContabilidad.InitVars
        End If
    End Sub
    
    Private Sub InitClass()
        Me.DataSetName = "ReportesContabilidadBE"
        Me.Prefix = ""
        Me.Namespace = "http://tempuri.org/ReportesContabilidadBE.xsd"
        Me.Locale = New System.Globalization.CultureInfo("en-US")
        Me.CaseSensitive = false
        Me.EnforceConstraints = true
        Me.tableReporteContabilidad = New ReporteContabilidadDataTable
        Me.Tables.Add(Me.tableReporteContabilidad)
    End Sub
    
    Private Function ShouldSerializeReporteContabilidad() As Boolean
        Return false
    End Function
    
    Private Sub SchemaChanged(ByVal sender As Object, ByVal e As System.ComponentModel.CollectionChangeEventArgs)
        If (e.Action = System.ComponentModel.CollectionChangeAction.Remove) Then
            Me.InitVars
        End If
    End Sub
    
    Public Delegate Sub ReporteContabilidadRowChangeEventHandler(ByVal sender As Object, ByVal e As ReporteContabilidadRowChangeEvent)
    
    <System.Diagnostics.DebuggerStepThrough()>  _
    Public Class ReporteContabilidadDataTable
        Inherits DataTable
        Implements System.Collections.IEnumerable
        
        Private columnFondo As DataColumn
        
        Private columnAsiento As DataColumn
        
        Private columnAsientoContable As DataColumn
        
        Private columnCuenta As DataColumn
        
        Private columnDescripcionCuenta As DataColumn
        
        Private columnDebe As DataColumn
        
        Private columnHaber As DataColumn
        
        Private columnDescripcionOperacion As DataColumn
        
        Private columnDB_CR As DataColumn
        
        Private columnDivisa As DataColumn
        
        Private columnCentroCostoDestino As DataColumn
        
        Private columnTipoContabilizacion As DataColumn
        
        Private columnCentroCostoOrigen As DataColumn
        
        Private columnEstado As DataColumn
        
        Private columnLoteContable As DataColumn
        
        Friend Sub New()
            MyBase.New("ReporteContabilidad")
            Me.InitClass
        End Sub
        
        Friend Sub New(ByVal table As DataTable)
            MyBase.New(table.TableName)
            If (table.CaseSensitive <> table.DataSet.CaseSensitive) Then
                Me.CaseSensitive = table.CaseSensitive
            End If
            If (table.Locale.ToString <> table.DataSet.Locale.ToString) Then
                Me.Locale = table.Locale
            End If
            If (table.Namespace <> table.DataSet.Namespace) Then
                Me.Namespace = table.Namespace
            End If
            Me.Prefix = table.Prefix
            Me.MinimumCapacity = table.MinimumCapacity
            Me.DisplayExpression = table.DisplayExpression
        End Sub
        
        <System.ComponentModel.Browsable(false)>  _
        Public ReadOnly Property Count As Integer
            Get
                Return Me.Rows.Count
            End Get
        End Property
        
        Friend ReadOnly Property FondoColumn As DataColumn
            Get
                Return Me.columnFondo
            End Get
        End Property
        
        Friend ReadOnly Property AsientoColumn As DataColumn
            Get
                Return Me.columnAsiento
            End Get
        End Property
        
        Friend ReadOnly Property AsientoContableColumn As DataColumn
            Get
                Return Me.columnAsientoContable
            End Get
        End Property
        
        Friend ReadOnly Property CuentaColumn As DataColumn
            Get
                Return Me.columnCuenta
            End Get
        End Property
        
        Friend ReadOnly Property DescripcionCuentaColumn As DataColumn
            Get
                Return Me.columnDescripcionCuenta
            End Get
        End Property
        
        Friend ReadOnly Property DebeColumn As DataColumn
            Get
                Return Me.columnDebe
            End Get
        End Property
        
        Friend ReadOnly Property HaberColumn As DataColumn
            Get
                Return Me.columnHaber
            End Get
        End Property
        
        Friend ReadOnly Property DescripcionOperacionColumn As DataColumn
            Get
                Return Me.columnDescripcionOperacion
            End Get
        End Property
        
        Friend ReadOnly Property DB_CRColumn As DataColumn
            Get
                Return Me.columnDB_CR
            End Get
        End Property
        
        Friend ReadOnly Property DivisaColumn As DataColumn
            Get
                Return Me.columnDivisa
            End Get
        End Property
        
        Friend ReadOnly Property CentroCostoDestinoColumn As DataColumn
            Get
                Return Me.columnCentroCostoDestino
            End Get
        End Property
        
        Friend ReadOnly Property TipoContabilizacionColumn As DataColumn
            Get
                Return Me.columnTipoContabilizacion
            End Get
        End Property
        
        Friend ReadOnly Property CentroCostoOrigenColumn As DataColumn
            Get
                Return Me.columnCentroCostoOrigen
            End Get
        End Property
        
        Friend ReadOnly Property EstadoColumn As DataColumn
            Get
                Return Me.columnEstado
            End Get
        End Property
        
        Friend ReadOnly Property LoteContableColumn As DataColumn
            Get
                Return Me.columnLoteContable
            End Get
        End Property
        
        Public Default ReadOnly Property Item(ByVal index As Integer) As ReporteContabilidadRow
            Get
                Return CType(Me.Rows(index),ReporteContabilidadRow)
            End Get
        End Property
        
        Public Event ReporteContabilidadRowChanged As ReporteContabilidadRowChangeEventHandler
        
        Public Event ReporteContabilidadRowChanging As ReporteContabilidadRowChangeEventHandler
        
        Public Event ReporteContabilidadRowDeleted As ReporteContabilidadRowChangeEventHandler
        
        Public Event ReporteContabilidadRowDeleting As ReporteContabilidadRowChangeEventHandler
        
        Public Overloads Sub AddReporteContabilidadRow(ByVal row As ReporteContabilidadRow)
            Me.Rows.Add(row)
        End Sub
        
        Public Overloads Function AddReporteContabilidadRow(ByVal Fondo As String, ByVal Asiento As String, ByVal AsientoContable As String, ByVal Cuenta As String, ByVal DescripcionCuenta As String, ByVal Debe As Decimal, ByVal Haber As Decimal, ByVal DescripcionOperacion As String, ByVal DB_CR As String, ByVal Divisa As String, ByVal CentroCostoDestino As String, ByVal TipoContabilizacion As String, ByVal CentroCostoOrigen As String, ByVal Estado As String, ByVal LoteContable As String) As ReporteContabilidadRow
            Dim rowReporteContabilidadRow As ReporteContabilidadRow = CType(Me.NewRow,ReporteContabilidadRow)
            rowReporteContabilidadRow.ItemArray = New Object() {Fondo, Asiento, AsientoContable, Cuenta, DescripcionCuenta, Debe, Haber, DescripcionOperacion, DB_CR, Divisa, CentroCostoDestino, TipoContabilizacion, CentroCostoOrigen, Estado, LoteContable}
            Me.Rows.Add(rowReporteContabilidadRow)
            Return rowReporteContabilidadRow
        End Function
        
        Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
            Return Me.Rows.GetEnumerator
        End Function
        
        Public Overrides Function Clone() As DataTable
            Dim cln As ReporteContabilidadDataTable = CType(MyBase.Clone,ReporteContabilidadDataTable)
            cln.InitVars
            Return cln
        End Function
        
        Protected Overrides Function CreateInstance() As DataTable
            Return New ReporteContabilidadDataTable
        End Function
        
        Friend Sub InitVars()
            Me.columnFondo = Me.Columns("Fondo")
            Me.columnAsiento = Me.Columns("Asiento")
            Me.columnAsientoContable = Me.Columns("AsientoContable")
            Me.columnCuenta = Me.Columns("Cuenta")
            Me.columnDescripcionCuenta = Me.Columns("DescripcionCuenta")
            Me.columnDebe = Me.Columns("Debe")
            Me.columnHaber = Me.Columns("Haber")
            Me.columnDescripcionOperacion = Me.Columns("DescripcionOperacion")
            Me.columnDB_CR = Me.Columns("DB_CR")
            Me.columnDivisa = Me.Columns("Divisa")
            Me.columnCentroCostoDestino = Me.Columns("CentroCostoDestino")
            Me.columnTipoContabilizacion = Me.Columns("TipoContabilizacion")
            Me.columnCentroCostoOrigen = Me.Columns("CentroCostoOrigen")
            Me.columnEstado = Me.Columns("Estado")
            Me.columnLoteContable = Me.Columns("LoteContable")
        End Sub
        
        Private Sub InitClass()
            Me.columnFondo = New DataColumn("Fondo", GetType(System.String), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnFondo)
            Me.columnAsiento = New DataColumn("Asiento", GetType(System.String), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnAsiento)
            Me.columnAsientoContable = New DataColumn("AsientoContable", GetType(System.String), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnAsientoContable)
            Me.columnCuenta = New DataColumn("Cuenta", GetType(System.String), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnCuenta)
            Me.columnDescripcionCuenta = New DataColumn("DescripcionCuenta", GetType(System.String), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnDescripcionCuenta)
            Me.columnDebe = New DataColumn("Debe", GetType(System.Decimal), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnDebe)
            Me.columnHaber = New DataColumn("Haber", GetType(System.Decimal), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnHaber)
            Me.columnDescripcionOperacion = New DataColumn("DescripcionOperacion", GetType(System.String), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnDescripcionOperacion)
            Me.columnDB_CR = New DataColumn("DB_CR", GetType(System.String), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnDB_CR)
            Me.columnDivisa = New DataColumn("Divisa", GetType(System.String), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnDivisa)
            Me.columnCentroCostoDestino = New DataColumn("CentroCostoDestino", GetType(System.String), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnCentroCostoDestino)
            Me.columnTipoContabilizacion = New DataColumn("TipoContabilizacion", GetType(System.String), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnTipoContabilizacion)
            Me.columnCentroCostoOrigen = New DataColumn("CentroCostoOrigen", GetType(System.String), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnCentroCostoOrigen)
            Me.columnEstado = New DataColumn("Estado", GetType(System.String), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnEstado)
            Me.columnLoteContable = New DataColumn("LoteContable", GetType(System.String), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnLoteContable)
        End Sub
        
        Public Function NewReporteContabilidadRow() As ReporteContabilidadRow
            Return CType(Me.NewRow,ReporteContabilidadRow)
        End Function
        
        Protected Overrides Function NewRowFromBuilder(ByVal builder As DataRowBuilder) As DataRow
            Return New ReporteContabilidadRow(builder)
        End Function
        
        Protected Overrides Function GetRowType() As System.Type
            Return GetType(ReporteContabilidadRow)
        End Function
        
        Protected Overrides Sub OnRowChanged(ByVal e As DataRowChangeEventArgs)
            MyBase.OnRowChanged(e)
            If (Not (Me.ReporteContabilidadRowChangedEvent) Is Nothing) Then
                RaiseEvent ReporteContabilidadRowChanged(Me, New ReporteContabilidadRowChangeEvent(CType(e.Row,ReporteContabilidadRow), e.Action))
            End If
        End Sub
        
        Protected Overrides Sub OnRowChanging(ByVal e As DataRowChangeEventArgs)
            MyBase.OnRowChanging(e)
            If (Not (Me.ReporteContabilidadRowChangingEvent) Is Nothing) Then
                RaiseEvent ReporteContabilidadRowChanging(Me, New ReporteContabilidadRowChangeEvent(CType(e.Row,ReporteContabilidadRow), e.Action))
            End If
        End Sub
        
        Protected Overrides Sub OnRowDeleted(ByVal e As DataRowChangeEventArgs)
            MyBase.OnRowDeleted(e)
            If (Not (Me.ReporteContabilidadRowDeletedEvent) Is Nothing) Then
                RaiseEvent ReporteContabilidadRowDeleted(Me, New ReporteContabilidadRowChangeEvent(CType(e.Row,ReporteContabilidadRow), e.Action))
            End If
        End Sub
        
        Protected Overrides Sub OnRowDeleting(ByVal e As DataRowChangeEventArgs)
            MyBase.OnRowDeleting(e)
            If (Not (Me.ReporteContabilidadRowDeletingEvent) Is Nothing) Then
                RaiseEvent ReporteContabilidadRowDeleting(Me, New ReporteContabilidadRowChangeEvent(CType(e.Row,ReporteContabilidadRow), e.Action))
            End If
        End Sub
        
        Public Sub RemoveReporteContabilidadRow(ByVal row As ReporteContabilidadRow)
            Me.Rows.Remove(row)
        End Sub
    End Class
    
    <System.Diagnostics.DebuggerStepThrough()>  _
    Public Class ReporteContabilidadRow
        Inherits DataRow
        
        Private tableReporteContabilidad As ReporteContabilidadDataTable
        
        Friend Sub New(ByVal rb As DataRowBuilder)
            MyBase.New(rb)
            Me.tableReporteContabilidad = CType(Me.Table,ReporteContabilidadDataTable)
        End Sub
        
        Public Property Fondo As String
            Get
                Try 
                    Return CType(Me(Me.tableReporteContabilidad.FondoColumn),String)
                Catch e As InvalidCastException
                    Throw New StrongTypingException("No se puede obtener el valor porque es DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableReporteContabilidad.FondoColumn) = value
            End Set
        End Property
        
        Public Property Asiento As String
            Get
                Try 
                    Return CType(Me(Me.tableReporteContabilidad.AsientoColumn),String)
                Catch e As InvalidCastException
                    Throw New StrongTypingException("No se puede obtener el valor porque es DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableReporteContabilidad.AsientoColumn) = value
            End Set
        End Property
        
        Public Property AsientoContable As String
            Get
                Try 
                    Return CType(Me(Me.tableReporteContabilidad.AsientoContableColumn),String)
                Catch e As InvalidCastException
                    Throw New StrongTypingException("No se puede obtener el valor porque es DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableReporteContabilidad.AsientoContableColumn) = value
            End Set
        End Property
        
        Public Property Cuenta As String
            Get
                Try 
                    Return CType(Me(Me.tableReporteContabilidad.CuentaColumn),String)
                Catch e As InvalidCastException
                    Throw New StrongTypingException("No se puede obtener el valor porque es DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableReporteContabilidad.CuentaColumn) = value
            End Set
        End Property
        
        Public Property DescripcionCuenta As String
            Get
                Try 
                    Return CType(Me(Me.tableReporteContabilidad.DescripcionCuentaColumn),String)
                Catch e As InvalidCastException
                    Throw New StrongTypingException("No se puede obtener el valor porque es DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableReporteContabilidad.DescripcionCuentaColumn) = value
            End Set
        End Property
        
        Public Property Debe As Decimal
            Get
                Try 
                    Return CType(Me(Me.tableReporteContabilidad.DebeColumn),Decimal)
                Catch e As InvalidCastException
                    Throw New StrongTypingException("No se puede obtener el valor porque es DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableReporteContabilidad.DebeColumn) = value
            End Set
        End Property
        
        Public Property Haber As Decimal
            Get
                Try 
                    Return CType(Me(Me.tableReporteContabilidad.HaberColumn),Decimal)
                Catch e As InvalidCastException
                    Throw New StrongTypingException("No se puede obtener el valor porque es DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableReporteContabilidad.HaberColumn) = value
            End Set
        End Property
        
        Public Property DescripcionOperacion As String
            Get
                Try 
                    Return CType(Me(Me.tableReporteContabilidad.DescripcionOperacionColumn),String)
                Catch e As InvalidCastException
                    Throw New StrongTypingException("No se puede obtener el valor porque es DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableReporteContabilidad.DescripcionOperacionColumn) = value
            End Set
        End Property
        
        Public Property DB_CR As String
            Get
                Try 
                    Return CType(Me(Me.tableReporteContabilidad.DB_CRColumn),String)
                Catch e As InvalidCastException
                    Throw New StrongTypingException("No se puede obtener el valor porque es DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableReporteContabilidad.DB_CRColumn) = value
            End Set
        End Property
        
        Public Property Divisa As String
            Get
                Try 
                    Return CType(Me(Me.tableReporteContabilidad.DivisaColumn),String)
                Catch e As InvalidCastException
                    Throw New StrongTypingException("No se puede obtener el valor porque es DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableReporteContabilidad.DivisaColumn) = value
            End Set
        End Property
        
        Public Property CentroCostoDestino As String
            Get
                Try 
                    Return CType(Me(Me.tableReporteContabilidad.CentroCostoDestinoColumn),String)
                Catch e As InvalidCastException
                    Throw New StrongTypingException("No se puede obtener el valor porque es DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableReporteContabilidad.CentroCostoDestinoColumn) = value
            End Set
        End Property
        
        Public Property TipoContabilizacion As String
            Get
                Try 
                    Return CType(Me(Me.tableReporteContabilidad.TipoContabilizacionColumn),String)
                Catch e As InvalidCastException
                    Throw New StrongTypingException("No se puede obtener el valor porque es DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableReporteContabilidad.TipoContabilizacionColumn) = value
            End Set
        End Property
        
        Public Property CentroCostoOrigen As String
            Get
                Try 
                    Return CType(Me(Me.tableReporteContabilidad.CentroCostoOrigenColumn),String)
                Catch e As InvalidCastException
                    Throw New StrongTypingException("No se puede obtener el valor porque es DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableReporteContabilidad.CentroCostoOrigenColumn) = value
            End Set
        End Property
        
        Public Property Estado As String
            Get
                Try 
                    Return CType(Me(Me.tableReporteContabilidad.EstadoColumn),String)
                Catch e As InvalidCastException
                    Throw New StrongTypingException("No se puede obtener el valor porque es DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableReporteContabilidad.EstadoColumn) = value
            End Set
        End Property
        
        Public Property LoteContable As String
            Get
                Try 
                    Return CType(Me(Me.tableReporteContabilidad.LoteContableColumn),String)
                Catch e As InvalidCastException
                    Throw New StrongTypingException("No se puede obtener el valor porque es DBNull.", e)
                End Try
            End Get
            Set
                Me(Me.tableReporteContabilidad.LoteContableColumn) = value
            End Set
        End Property
        
        Public Function IsFondoNull() As Boolean
            Return Me.IsNull(Me.tableReporteContabilidad.FondoColumn)
        End Function
        
        Public Sub SetFondoNull()
            Me(Me.tableReporteContabilidad.FondoColumn) = System.Convert.DBNull
        End Sub
        
        Public Function IsAsientoNull() As Boolean
            Return Me.IsNull(Me.tableReporteContabilidad.AsientoColumn)
        End Function
        
        Public Sub SetAsientoNull()
            Me(Me.tableReporteContabilidad.AsientoColumn) = System.Convert.DBNull
        End Sub
        
        Public Function IsAsientoContableNull() As Boolean
            Return Me.IsNull(Me.tableReporteContabilidad.AsientoContableColumn)
        End Function
        
        Public Sub SetAsientoContableNull()
            Me(Me.tableReporteContabilidad.AsientoContableColumn) = System.Convert.DBNull
        End Sub
        
        Public Function IsCuentaNull() As Boolean
            Return Me.IsNull(Me.tableReporteContabilidad.CuentaColumn)
        End Function
        
        Public Sub SetCuentaNull()
            Me(Me.tableReporteContabilidad.CuentaColumn) = System.Convert.DBNull
        End Sub
        
        Public Function IsDescripcionCuentaNull() As Boolean
            Return Me.IsNull(Me.tableReporteContabilidad.DescripcionCuentaColumn)
        End Function
        
        Public Sub SetDescripcionCuentaNull()
            Me(Me.tableReporteContabilidad.DescripcionCuentaColumn) = System.Convert.DBNull
        End Sub
        
        Public Function IsDebeNull() As Boolean
            Return Me.IsNull(Me.tableReporteContabilidad.DebeColumn)
        End Function
        
        Public Sub SetDebeNull()
            Me(Me.tableReporteContabilidad.DebeColumn) = System.Convert.DBNull
        End Sub
        
        Public Function IsHaberNull() As Boolean
            Return Me.IsNull(Me.tableReporteContabilidad.HaberColumn)
        End Function
        
        Public Sub SetHaberNull()
            Me(Me.tableReporteContabilidad.HaberColumn) = System.Convert.DBNull
        End Sub
        
        Public Function IsDescripcionOperacionNull() As Boolean
            Return Me.IsNull(Me.tableReporteContabilidad.DescripcionOperacionColumn)
        End Function
        
        Public Sub SetDescripcionOperacionNull()
            Me(Me.tableReporteContabilidad.DescripcionOperacionColumn) = System.Convert.DBNull
        End Sub
        
        Public Function IsDB_CRNull() As Boolean
            Return Me.IsNull(Me.tableReporteContabilidad.DB_CRColumn)
        End Function
        
        Public Sub SetDB_CRNull()
            Me(Me.tableReporteContabilidad.DB_CRColumn) = System.Convert.DBNull
        End Sub
        
        Public Function IsDivisaNull() As Boolean
            Return Me.IsNull(Me.tableReporteContabilidad.DivisaColumn)
        End Function
        
        Public Sub SetDivisaNull()
            Me(Me.tableReporteContabilidad.DivisaColumn) = System.Convert.DBNull
        End Sub
        
        Public Function IsCentroCostoDestinoNull() As Boolean
            Return Me.IsNull(Me.tableReporteContabilidad.CentroCostoDestinoColumn)
        End Function
        
        Public Sub SetCentroCostoDestinoNull()
            Me(Me.tableReporteContabilidad.CentroCostoDestinoColumn) = System.Convert.DBNull
        End Sub
        
        Public Function IsTipoContabilizacionNull() As Boolean
            Return Me.IsNull(Me.tableReporteContabilidad.TipoContabilizacionColumn)
        End Function
        
        Public Sub SetTipoContabilizacionNull()
            Me(Me.tableReporteContabilidad.TipoContabilizacionColumn) = System.Convert.DBNull
        End Sub
        
        Public Function IsCentroCostoOrigenNull() As Boolean
            Return Me.IsNull(Me.tableReporteContabilidad.CentroCostoOrigenColumn)
        End Function
        
        Public Sub SetCentroCostoOrigenNull()
            Me(Me.tableReporteContabilidad.CentroCostoOrigenColumn) = System.Convert.DBNull
        End Sub
        
        Public Function IsEstadoNull() As Boolean
            Return Me.IsNull(Me.tableReporteContabilidad.EstadoColumn)
        End Function
        
        Public Sub SetEstadoNull()
            Me(Me.tableReporteContabilidad.EstadoColumn) = System.Convert.DBNull
        End Sub
        
        Public Function IsLoteContableNull() As Boolean
            Return Me.IsNull(Me.tableReporteContabilidad.LoteContableColumn)
        End Function
        
        Public Sub SetLoteContableNull()
            Me(Me.tableReporteContabilidad.LoteContableColumn) = System.Convert.DBNull
        End Sub
    End Class
    
    <System.Diagnostics.DebuggerStepThrough()>  _
    Public Class ReporteContabilidadRowChangeEvent
        Inherits EventArgs
        
        Private eventRow As ReporteContabilidadRow
        
        Private eventAction As DataRowAction
        
        Public Sub New(ByVal row As ReporteContabilidadRow, ByVal action As DataRowAction)
            MyBase.New
            Me.eventRow = row
            Me.eventAction = action
        End Sub
        
        Public ReadOnly Property Row As ReporteContabilidadRow
            Get
                Return Me.eventRow
            End Get
        End Property
        
        Public ReadOnly Property Action As DataRowAction
            Get
                Return Me.eventAction
            End Get
        End Property
    End Class
End Class
