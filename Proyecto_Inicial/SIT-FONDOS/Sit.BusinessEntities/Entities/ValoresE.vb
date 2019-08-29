'OT 10090 - 26/07/2017 - Carlos Espejo
'Descripcion: Clase Valores para metodos WEB
<Serializable()>
Public Class ValoresE
    Sub New()
    End Sub
    Private _Nemonico As String
    Public Property Nemonico() As String
        Get
            Return _Nemonico
        End Get
        Set(ByVal value As String)
            _Nemonico = value
        End Set
    End Property
    Private _Categoria As String
    Public Property Categoria() As String
        Get
            Return _Categoria
        End Get
        Set(ByVal value As String)
            _Categoria = value
        End Set
    End Property
End Class
<Serializable()>
Public Class ValoresEList
    Inherits Generic.List(Of ValoresE)
End Class
