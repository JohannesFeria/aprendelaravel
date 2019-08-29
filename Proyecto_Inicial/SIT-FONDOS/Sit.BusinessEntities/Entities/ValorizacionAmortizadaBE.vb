

Partial Public Class ValorizacionAmortizadaBE
    Partial Class ValorizacionAmortizadaDataTable

        Private Sub ValorizacionAmortizadaDataTable_ColumnChanging(sender As System.Object, e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.NEG_InteresCorridoColumn.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class

End Class
