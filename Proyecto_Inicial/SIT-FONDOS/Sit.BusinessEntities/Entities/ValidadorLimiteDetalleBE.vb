Imports System.Collections.Generic

Public Class ValidadorLimiteDetalleBE

    Private _Id As Integer
    Private _CodigoValidador As String
    Private _CodigoLimite As String


    Public Property Id() As Integer
        Get
            Return _Id
        End Get
        Set(ByVal value As Integer)
            _Id = value
        End Set
    End Property

    Public Property CodigoValidador() As String
        Get
            Return _CodigoValidador
        End Get
        Set(ByVal value As String)
            _CodigoValidador = value
        End Set
    End Property

    Public Property CodigoLimite() As String
        Get
            Return _CodigoLimite
        End Get
        Set(ByVal value As String)
            _CodigoLimite = value
        End Set
    End Property

    Private _Tipo As String
    Public Property Tipo() As String
        Get
            Return _Tipo
        End Get
        Set(ByVal value As String)
            _Tipo = value
        End Set
    End Property


End Class

Public Class ListValidadorLimite
    Private _objListValidadorLimiteDetalle As List(Of ValidadorLimiteDetalleBE)

    Public Property objListValidadorLimite() As List(Of ValidadorLimiteDetalleBE)
        Get
            Return _objListValidadorLimiteDetalle
        End Get
        Set(ByVal value As List(Of ValidadorLimiteDetalleBE))
            _objListValidadorLimiteDetalle = value
        End Set
    End Property
End Class
