Imports Syn.Documento

Public Interface IRepresentacionPedimento

    Enum TipoOperacion
        Importacion = 1
        Exportacion = 2
    End Enum

#Region "Funciones"
    Function ImprimirPedimentoNormal(Optional ByVal documento_ As DocumentoElectronico = Nothing) As String

    Function ImprimirRectificacion(Optional ByVal documento_ As DocumentoElectronico = Nothing) As String

    Function ImprimirPedimentoComplementario(Optional ByVal documento_ As DocumentoElectronico = Nothing) As String

    Function ImprimirPedimentoGlobal(Optional ByVal documento_ As DocumentoElectronico = Nothing) As String

    Function ImprimirPedimentoSimplificado(Optional ByVal documento_ As DocumentoElectronico = Nothing) As String

    Function ImprimirPedimentoConsolidado(Optional ByVal documento_ As DocumentoElectronico = Nothing) As String

    Function ImprimirFormatoPartesII(numeroRemesa_ As Int32, Optional ByVal documento_ As DocumentoElectronico = Nothing) As String

    Function ImprimirFormatoCopiaSimple(numeroRemesa_ As Int32, Optional ByVal documento_ As DocumentoElectronico = Nothing) As String

    Function ImprimirFormatoRemesa(numeroRemesa_ As Int32, Optional ByVal documento_ As DocumentoElectronico = Nothing) As String

#End Region

End Interface