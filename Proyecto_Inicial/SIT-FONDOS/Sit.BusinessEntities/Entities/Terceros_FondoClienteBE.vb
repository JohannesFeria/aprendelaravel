Imports System.Collections.Generic

Public Class Terceros_FondoClienteBE

    Private _Id As Integer
    Public Property Id() As Integer
        Get
            Return _Id
        End Get
        Set(ByVal value As Integer)
            _Id = value
        End Set
    End Property


    Private _CodigoTercero As String
    Public Property CodigoTercero() As String
        Get
            Return _CodigoTercero
        End Get
        Set(ByVal value As String)
            _CodigoTercero = value
        End Set
    End Property


    Private _CodigoPortafolio As String
    Public Property CodigoPortafolio() As String
        Get
            Return _CodigoPortafolio
        End Get
        Set(ByVal value As String)
            _CodigoPortafolio = value
        End Set
    End Property


    Private _CodigoTerceroCliente As String
    Public Property CodigoTerceroCliente() As String
        Get
            Return _CodigoTerceroCliente
        End Get
        Set(ByVal value As String)
            _CodigoTerceroCliente = value
        End Set
    End Property

End Class

Public Class ListFondoCliente
    Private _objListFondoCliente As List(Of Terceros_FondoClienteBE)
    Public Property objListFondoCliente() As List(Of Terceros_FondoClienteBE)
        Get
            Return _objListFondoCliente
        End Get
        Set(ByVal value As List(Of Terceros_FondoClienteBE))
            _objListFondoCliente = value
        End Set
    End Property
End Class
