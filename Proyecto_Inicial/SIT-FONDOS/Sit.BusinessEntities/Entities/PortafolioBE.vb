

Partial Public Class PortafolioBE
    Partial Class PortafolioDataTable

        Private Sub PortafolioDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.VectorPrecioValColumn.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class

End Class
