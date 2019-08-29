'OT 10025 21/02/2017 - Carlos Espejo
'Descripcion: Se agrega la linea de CLSCompliant
<Assembly: CLSCompliant(True)> 
'OT 10025 Fin
<Serializable()>
Public Class TablaGeneralBE
    Sub New()
    End Sub
    Private _Clasificacion As String
    Public Property Clasificacion() As String
        Get
            Return _Clasificacion
        End Get
        Set(ByVal value As String)
            _Clasificacion = value
        End Set
    End Property
    Private _Codigo As String
    Public Property Codigo() As String
        Get
            Return _Codigo
        End Get
        Set(ByVal value As String)
            _Codigo = value
        End Set
    End Property
    Private _Valor As String
    Public Property Valor() As String
        Get
            Return _Valor
        End Get
        Set(ByVal value As String)
            _Valor = value
        End Set
    End Property
    Private _Comentario As String
    Public Property Comentario() As String
        Get
            Return _Comentario
        End Get
        Set(ByVal value As String)
            _Comentario = value
        End Set
    End Property
End Class
<Serializable()>
Public Class TablaGeneralBEList
    Inherits Generic.List(Of TablaGeneralBE)
End Class
