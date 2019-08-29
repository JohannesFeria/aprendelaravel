Imports System.Threading
Imports System.SerializableAttribute
Imports Sit.BusinessEntities

Public Interface ILimiteBM

    Function SeleccionarPorInstrumento(ByVal codigoNemonico As String, ByVal dataRequest As DataSet) As LimiteBE
    Function LimiteEvaluar(ByVal codigoLimite As String, ByVal dataRequest As DataSet) As DataSet

End Interface
