Public Class ClienteMandatoBE

    Private sCodigoPortafolio As String
    Public Property CodigoPortafolio() As String
        Get
            Return sCodigoPortafolio
        End Get
        Set(ByVal value As String)
            sCodigoPortafolio = value
        End Set
    End Property

    Private sCodigoClienteMandato As String
    Public Property CodigoClienteMandato() As String
        Get
            Return sCodigoClienteMandato
        End Get
        Set(ByVal value As String)
            sCodigoClienteMandato = value
        End Set
    End Property

    Private sClienteMandato As String
    Public Property ClienteMandato() As String
        Get
            Return sClienteMandato
        End Get
        Set(ByVal value As String)
            sClienteMandato = value
        End Set
    End Property

End Class
