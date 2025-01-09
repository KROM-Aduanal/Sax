Imports gsol.krom
Imports Wma.Exceptions
Imports MongoDB.Bson
Imports Syn.Documento
Imports Syn.CustomBrokers.Controllers
Imports System.IO
Imports MongoDB.Driver
Imports Ia.Pln
Imports MongoDB.Driver.Linq.Processors
Imports Wma.Exceptions.TagWatcher
Imports Syn.Utils

Public Interface IControladorDespachos

#Region "Enums"

    Enum TiposDespacho

        Normal = 1
        Consolidado = 2
        PartesII = 3
        CopiaSimple = 4

    End Enum
    Enum TiposProcesamiento

        Apertura = 1
        Cierre = 2

    End Enum
    Enum EstatusAviso

        Nuevo = 1
        Abierto = 2
        Cerrado = 3

    End Enum

#End Region

#Region "Propiedades"

    Property EstatusEnLinea As Boolean

    Property IdUsuario As ObjectId

    Property IdEnviroment As ObjectId

    Property DocumentoElectronicoDespacho As DocumentoElectronico

#End Region

#Region "Funciones"

    Function IniciarDespacho(tipoDespacho_ As TiposDespacho, documentoRaiz_ As DocumentoElectronico, sesion_ As IClientSessionHandle) As TagWatcher

    Function IniciarDespacho(documentoRaiz_ As DocumentoElectronico, sesion_ As IClientSessionHandle) As TagWatcher

    Function ProcesarDespacho(tipoDespacho_ As TiposDespacho, documentoRaiz_ As DocumentoElectronico) As TagWatcher

    Function ProcesarDespacho(documentoRaiz_ As DocumentoElectronico) As TagWatcher

    Function CerrarDespacho(sesion_ As IClientSessionHandle) As TagWatcher

    Function CerrarDespacho(tipoDespacho_ As TiposDespacho, sesion_ As IClientSessionHandle) As TagWatcher

    Function ProcesamientoElectronico(tipoDespacho_ As TiposDespacho, tipoProcesamiento_ As TiposProcesamiento) As TagWatcher

    Function ProcesamientoElectronico(tipoProcesamiento_ As TiposProcesamiento) As TagWatcher

    Function EstatusProcesamientoElectronico(idOperacionProcesamiento_ As ObjectId) As TagWatcher

    Function GeneraRepresentacionImpresa(numeroDocumento As Int32) As TagWatcher

    Function GeneraRepresentacionImpresa(tipoDespacho_ As TiposDespacho, numeroDocumento As Int32) As TagWatcher

    Function EstatusInventario() As TagWatcher

    Function EstatusInventario(tipoDespacho_ As TiposDespacho) As TagWatcher

    Function EstatusInventario(idDespacho As ObjectId) As TagWatcher

    Function EstatusInventario(tipoDespacho_ As TiposDespacho, idDespacho As ObjectId) As TagWatcher

    Function TrasladarItemsDespacho(idDestino As ObjectId, numeroRemesa_ As Int32, operacionGenerica_ As OperacionGenerica) As TagWatcher

    Function DepurarItemsDespacho(operacionGenerica As OperacionGenerica) As TagWatcher

#End Region
End Interface
