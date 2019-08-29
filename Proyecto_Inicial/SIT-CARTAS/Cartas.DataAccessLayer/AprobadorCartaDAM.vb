Imports System.Data
Imports System.Data.Common
Imports Cartas.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class AprobadorCartaDAM
    Public Sub New()
    End Sub
    Public Function SeleccionarPorFiltro(ByVal codigoInterno As String, ByVal rol As String, ByVal situacion As String) As AprobadorCartaBEList
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim lAprobadorCarta As New AprobadorCartaBEList
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_SeleccionarPorFiltro_AprobadorCarta")
            db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, codigoInterno)
            db.AddInParameter(dbCommand, "@p_Rol", DbType.String, rol)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
            Using oReader As IDataReader = db.ExecuteReader(dbCommand)
                Dim oAprobadorCarta As AprobadorCartaBE
                While oReader.Read()
                    oAprobadorCarta = New AprobadorCartaBE
                    oAprobadorCarta.CodigoInterno = DataUtility.IsDBNull(oReader.Item("CodigoInterno"), "")
                    oAprobadorCarta.Nombre = DataUtility.IsDBNull(oReader.Item("Nombre"), "")
                    oAprobadorCarta.DescripcionRol = DataUtility.IsDBNull(oReader.Item("DescripcionRol"), "")
                    oAprobadorCarta.Rol = DataUtility.IsDBNull(oReader.Item("Rol"), "")
                    oAprobadorCarta.Firma = DataUtility.IsDBNull(oReader.Item("Firma"), "")
                    oAprobadorCarta.DescripcionSituacion = DataUtility.IsDBNull(oReader.Item("DescripcionSituacion"), "")
                    oAprobadorCarta.Situacion = DataUtility.IsDBNull(oReader.Item("Situacion"), "")
                    oAprobadorCarta.email_trabajo = DataUtility.IsDBNull(oReader.Item("email_trabajo"), "")
                    oAprobadorCarta.email_personal = DataUtility.IsDBNull(oReader.Item("email_personal"), "")

                    lAprobadorCarta.Add(oAprobadorCarta)
                End While
                oReader.Close()
            End Using
        End Using
        Return lAprobadorCarta
    End Function
    Public Function GeneraClaves(ByVal longitud As Decimal, ByVal upper As Boolean, FechaConsulta As Decimal) As TablaGeneralBEList
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim lTablaGeneral As New TablaGeneralBEList
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_GeneraClave_AprobadorCarta")
            db.AddInParameter(dbCommand, "@p_Longitud", DbType.Decimal, longitud)
            db.AddInParameter(dbCommand, "@p_Upper", DbType.Boolean, upper)
            'OT 10025 21/02/2017 - Carlos Espejo
            'Descripcion: Nuevo campo para generacion de claves en un solo proceso
            db.AddInParameter(dbCommand, "@P_FechaConsulta", DbType.Decimal, FechaConsulta)
            'OT 10025 Fin
            Using oReader As IDataReader = db.ExecuteReader(dbCommand)
                Dim oTablaGeneral As TablaGeneralBE
                While oReader.Read()
                    oTablaGeneral = New TablaGeneralBE
                    oTablaGeneral.Codigo = oReader.Item(1)
                    oTablaGeneral.Valor = oReader.Item(2)
                    lTablaGeneral.Add(oTablaGeneral)
                End While
                oReader.Close()
            End Using
        End Using
        Return lTablaGeneral
    End Function
    Public Function ObtenerRutaReporteAprobacion(ByVal codigoUsuario As String, ByVal CodigoOperacionCaja As String, ByVal indReporte As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase
        ObtenerRutaReporteAprobacion = ""
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ObtenerRuta_ReporteAprobacion")
            dbCommand.CommandTimeout = 300
            db.AddInParameter(dbCommand, "@p_CodigoUsuario", DbType.String, codigoUsuario)
            db.AddInParameter(dbCommand, "@p_CodigoOperacionCaja", DbType.String, CodigoOperacionCaja)
            db.AddInParameter(dbCommand, "@p_IndReporte", DbType.String, indReporte)
            Dim ds As DataSet
            ds = db.ExecuteDataSet(dbCommand)
            If ds.Tables(0).Rows.Count > 0 Then ObtenerRutaReporteAprobacion = CType(ds.Tables(0).Rows(0)("RutaReporte"), String)
        End Using
    End Function
End Class