Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports Cartas.BusinessEntities
Imports Cartas.DataAccessLayer
Public Class TercerosDAM
    Public Sub New()
    End Sub
    Public Function SeleccionarPorFiltro(ByVal clasificacionTercero As String, ByVal tipoTercero As String) As TerceroBEList
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim lTercero As New TerceroBEList
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Tercero_SeleccionarPorFiltro2")
            db.AddInParameter(dbCommand, "@p_ClasificacionTercero", DbType.String, clasificacionTercero)
            db.AddInParameter(dbCommand, "@p_TipoTercero", DbType.String, tipoTercero)
            Using oReader As IDataReader = db.ExecuteReader(dbCommand)
                Dim oTercero As TerceroBE
                While oReader.Read()
                    oTercero = New TerceroBE
                    oTercero.CodigoTercero = oReader.Item("CodigoTercero")
                    oTercero.Situacion = oReader.Item(2)
                    oTercero.Descripcion = oReader.Item(3)
                    oTercero.Direccion = oReader.Item(4)
                    oTercero.CodigoPostal = oReader.Item(5)
                    oTercero.CodigoPais = oReader.Item(6)
                    oTercero.CodigoTipoDocumento = oReader.Item(7)
                    oTercero.CodigoSectorEmpresarial = oReader.Item(8)
                    oTercero.TipoTercero = oReader.Item(9)
                    oTercero.ClasificacionTercero = oReader.Item(10)
                    oTercero.CodigoCustodio = oReader.Item(11)
                    lTercero.Add(oTercero)
                End While
                oReader.Close()
            End Using
        End Using
        Return lTercero
    End Function
    Public Function SeleccionarBancoPorCodigoMercadoYPortafolio(ByVal codigoMercado As String, ByVal codigoPortafolioSBS As String) As TerceroBEList
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim lTercero As New TerceroBEList
        Using dbCommand As DbCommand = db.GetStoredProcCommand("TerceroBanco_SeleccionarPorMercadoYPortafolio")
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
            Using oReader As IDataReader = db.ExecuteReader(dbCommand)
                Dim oTercero As TerceroBE
                While oReader.Read()
                    oTercero = New TerceroBE
                    oTercero.Situacion = oReader.Item(0)
                    oTercero.CodigoTercero = oReader.Item(1)
                    oTercero.Descripcion = oReader.Item(2)
                    oTercero.Direccion = oReader.Item(3)
                    oTercero.CodigoPostal = oReader.Item(4)
                    oTercero.CodigoPais = oReader.Item(5)
                    oTercero.CodigoTipoDocumento = oReader.Item(6)
                    oTercero.CodigoSectorEmpresarial = oReader.Item(7)
                    oTercero.TipoTercero = oReader.Item(8)
                    oTercero.ClasificacionTercero = oReader.Item(9)
                    oTercero.CodigoCustodio = oReader.Item(10)
                    lTercero.Add(oTercero)
                End While
                oReader.Close()
            End Using
        End Using
        Return lTercero
    End Function
End Class