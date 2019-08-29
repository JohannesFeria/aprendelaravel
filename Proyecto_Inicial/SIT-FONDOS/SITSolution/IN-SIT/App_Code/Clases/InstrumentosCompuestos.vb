Imports Microsoft.VisualBasic
Imports System.Data
Imports Sit.BusinessLayer

Public Class InstrumentosCompuestos

    Private _dtDetalle As DataTable
    Private _count As Integer

    Public ReadOnly Property Count() As Integer
        Get
            Return _Count
        End Get
    End Property
    Public ReadOnly Property Datos() As DataTable
        Get
            Return _dtDetalle
        End Get
    End Property
    Public Sub New()
        _dtDetalle = New DataTable()
        _dtDetalle.Columns.Add("CodigoTipoInstrumento")
        _dtDetalle.Columns.Add("DescripcionTipoInstrumento")
        _dtDetalle.Columns.Add("CodigoNemonicoAsociado")
        _dtDetalle.Columns.Add("Emision")
        _dtDetalle.Columns.Add("Monto")
        _dtDetalle.Columns.Add("Cantidad")
        _dtDetalle.Columns.Add("Situacion")
        _dtDetalle.Columns.Add("MonedaPrima")
        _dtDetalle.Columns.Add("Identificador")
    End Sub
    Public Sub CargarRegistro(ByVal CodigoNemo As String)
        Dim oInsEstBM As New InstrumentosEstructuradosBM
        Dim i As Integer
        _dtDetalle = New DataTable
        _dtDetalle = oInsEstBM.SeleccionarInstrumentosEstructurados(CodigoNemo, Nothing).Tables(0)
        _dtDetalle.Columns.Add("Identificador")
        If _dtDetalle.Rows.Count > 0 Then
            For i = 0 To _dtDetalle.Rows.Count - 1
                _dtDetalle.Rows(i)("Identificador") = i
            Next
            _count = _dtDetalle.Rows.Count
        End If
    End Sub
End Class