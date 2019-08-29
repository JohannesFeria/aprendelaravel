Imports Microsoft.VisualBasic
Imports Sit.BusinessLayer
Imports System.Data

Public Class DepositoPlazos

    Public Function ObtenerDatosOperacion(ByVal request As DataSet, Optional ByVal drOrden As DataRow = Nothing) As DataTable
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)

        drGrilla = dtGrilla.NewRow
        drGrilla("c1") = "Fecha Operación"
        drGrilla("v1") = UIUtility.ConvertirFechaaString(drOrden("FechaOperacion"))
        drGrilla("v2") = UIUtility.ConvertirFechaaString(drOrden("FechaLiquidacion"))
        drGrilla("v3") = UIUtility.ConvertirFechaaString(drOrden("FechaContrato"))
        drGrilla("v4") = drOrden("HoraOperacion")
        drGrilla("v5") = drOrden("Plazo")
        drGrilla("v6") = drOrden("CodigoMoneda")
        drGrilla("v7") = Format(drOrden("MontoNominalOrdenado"), "#,##0.0000000")    'HDG 20120307
        drGrilla("v8") = Format(drOrden("MontoNominalOperacion"), "#,##0.0000000")    'HDG 20120307
        drGrilla("v9") = New TipoCuponBM().Seleccionar(drOrden("CodigoTipoCupon"), request).Tables(0).Rows(0)("Descripcion")
        drGrilla("v10") = Format(drOrden("TasaPorcentaje"), "#,##0.0000000")    'HDG 20120307
        drGrilla("v11") = Format(drOrden("MontoOperacion"), "#,##0.0000000")    'HDG 20120307
        drGrilla("v12") = New TercerosBM().Seleccionar(drOrden("CodigoTercero"), request).Tables(0).Rows(0)("Descripcion")
        drGrilla("v13") = drOrden("Observacion")
        'drGrilla("v19") = drOrden("TotalComisiones")
        drGrilla("v20") = Format(drOrden("MontoNetoOperacion"), "#,##0.0000000")    'HDG 20120307

        drGrilla("c2") = "Fecha Vencimiento"
        drGrilla("c3") = "Fecha Fin de Contrato"
        drGrilla("c4") = "Hora Operación"
        drGrilla("c5") = "Plazo"
        drGrilla("c6") = "Moneda"
        drGrilla("c7") = "Monto Nominal Ordenado"
        drGrilla("c8") = "Monto Nominal Operación"
        drGrilla("c9") = "Tipo Tasa"
        drGrilla("c10") = "Tasa %"
        drGrilla("c11") = "Monto Operación"
        drGrilla("c12") = "Intermediario"
        drGrilla("c13") = "Observación"
        drGrilla("c19") = "Total Comisiones"

        drGrilla("c20") = "Monto Neto Operación"

        drGrilla("c21") = ""
        drGrilla("v21") = ""
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function

End Class
