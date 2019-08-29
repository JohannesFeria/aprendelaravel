'OT 10090 - 26/07/2017 - Carlos Espejo
'Descripcion: Clase Terceros para metodos WEB
<Serializable()>
Public Class TercerosE
    Sub New()
    End Sub
    Private _Descripcion As String
    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
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
End Class
<Serializable()>
Public Class TercerosEList
    Inherits Generic.List(Of TercerosE)
End Class
