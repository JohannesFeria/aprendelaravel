Imports System
Imports System.Data
Imports System.Data.Common
Imports Cartas.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class ParametrosGeneralesDAM
    Public Function ListarRutaGeneracionCartas() As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ParametrosGenerales_ListarRutaGeneracionCartas")
            ListarRutaGeneracionCartas = Convert.ToString(db.ExecuteScalar(dbCommand))
        End Using
    End Function
    Public Function Listar(ByVal clasificacion As String) As TablaGeneralBEList
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim lTablaGeneral As New TablaGeneralBEList
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ParametrosGenerales_Listar")
            db.AddInParameter(dbCommand, "@p_Clasificacion", DbType.String, clasificacion)
            Using oReader As IDataReader = db.ExecuteReader(dbCommand)
                Dim oTablaGeneral As TablaGeneralBE
                While oReader.Read()
                    oTablaGeneral = New TablaGeneralBE
                    oTablaGeneral.Clasificacion = oReader.Item(0)
                    oTablaGeneral.Codigo = oReader.Item(2)
                    oTablaGeneral.Valor = oReader.Item(1)
                    oTablaGeneral.Comentario = oReader.Item(3)
                    lTablaGeneral.Add(oTablaGeneral)
                End While
                oReader.Close()
            End Using
        End Using
        Return lTablaGeneral
    End Function
    'OT 10025 21/02/2017 - Carlos Espejo
    'Descripcion: Se agrega la fecha de operacion al filtro
    Public Function ListarClaveFirmantesCartas(FechaOperacion As Decimal) As TablaGeneralBEList
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim lTablaGeneral As New TablaGeneralBEList
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ListarFirmantesClaves_ClaveFirmantesCarta")
            db.AddInParameter(dbCommand, "@P_FechaOperacion", DbType.Decimal, FechaOperacion)
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
    Public Function SeleccionarPorFiltro(ByVal clasificacion As String, ByVal nombre As String, ByVal valor As String, ByVal comentario As String) As TablaGeneralBEList
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim lTablaGeneral As New TablaGeneralBEList
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ParametrosGenerales_SeleccionarPorFiltro")
            db.AddInParameter(dbCommand, "@p_clasificacion", DbType.String, clasificacion)
            db.AddInParameter(dbCommand, "@p_Nombre", DbType.String, nombre)
            db.AddInParameter(dbCommand, "@p_Valor", DbType.String, valor)
            db.AddInParameter(dbCommand, "@p_Comentario", DbType.String, comentario)
            Using oReader As IDataReader = db.ExecuteReader(dbCommand)
                Dim oTablaGeneral As TablaGeneralBE
                While oReader.Read()
                    oTablaGeneral = New TablaGeneralBE
                    oTablaGeneral.Clasificacion = oReader.Item(0)
                    oTablaGeneral.Codigo = oReader.Item(1)
                    oTablaGeneral.Valor = oReader.Item(2)
                    oTablaGeneral.Comentario = oReader.Item(3)
                    lTablaGeneral.Add(oTablaGeneral)
                End While
                oReader.Close()
            End Using
        End Using
        Return lTablaGeneral
    End Function
End Class