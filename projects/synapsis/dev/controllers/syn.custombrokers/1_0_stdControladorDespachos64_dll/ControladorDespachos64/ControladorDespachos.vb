Imports gsol.krom
Imports Wma.Exceptions
Imports MongoDB.Bson
Imports Syn.Documento
Imports MongoDB.Driver
Imports Syn.Nucleo.RecursosComercioExterior.SeccionesControlConsolidados
Imports Syn.Nucleo.RecursosComercioExterior.SeccionesReferencias
Imports Syn.Nucleo.RecursosComercioExterior.CamposControlConsolidados
Imports Syn.Nucleo.RecursosComercioExterior.CamposPedimento
Imports Syn.Nucleo.Recursos.CamposClientes
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.CustomBrokers.Controllers.reportes
Imports Syn.Documento.Componentes

Public Class ControladorDespachos
    Implements IControladorDespachos, ICloneable, IDisposable

#Region "Atributos"

    Private _tipoDespacho As IControladorDespachos.TiposDespacho

    Private _sesion As IClientSessionHandle

#End Region

#Region "Propiedades"

    Public Property EstatusEnLinea As Boolean Implements IControladorDespachos.EstatusEnLinea

    Public Property IdUsuario As ObjectId Implements IControladorDespachos.IdUsuario

    Public Property IdEnviroment As ObjectId Implements IControladorDespachos.IdEnviroment

    Public Property DocumentoElectronicoDespacho As DocumentoElectronico Implements IControladorDespachos.DocumentoElectronicoDespacho

#End Region

#Region "Constructores"

    Sub New(idUsuario_ As ObjectId, idEviroment_ As ObjectId)

        Inicializa(idUsuario_, idEviroment_)

    End Sub

    Sub New(idUsuario_ As ObjectId, idEviroment_ As ObjectId, documentoElectronicoDespacho_ As DocumentoElectronico, tipoDespacho_ As IControladorDespachos.TiposDespacho)

        Inicializa(idUsuario_, idEviroment_, documentoElectronicoDespacho_, tipoDespacho_)

    End Sub

    Private Sub Inicializa(idUsuario_ As ObjectId, idEviroment_ As ObjectId)

        IdEnviroment = idEviroment_

        IdUsuario = idUsuario_

    End Sub



    Private Sub Inicializa(idUsuario_ As ObjectId, idEviroment_ As ObjectId, documentoElectronicoDespacho_ As DocumentoElectronico, tipoDespacho_ As IControladorDespachos.TiposDespacho)

        IdEnviroment = idEviroment_

        IdUsuario = idUsuario_

        DocumentoElectronicoDespacho = documentoElectronicoDespacho_

        _tipoDespacho = tipoDespacho_

    End Sub

#End Region

#Region "Funciones"

    Public Function IniciarDespacho(tipoDespacho_ As IControladorDespachos.TiposDespacho, documentoRaiz_ As DocumentoElectronico, sesion_ As IClientSessionHandle) As TagWatcher _
                                                                                                                                Implements IControladorDespachos.IniciarDespacho

        Dim estado_ As TagWatcher = Nothing

        Select Case tipoDespacho_

            Case IControladorDespachos.TiposDespacho.Consolidado

                estado_ = IniciarConsolidado(tipoDespacho_, documentoRaiz_, sesion_)

            Case IControladorDespachos.TiposDespacho.CopiaSimple

                estado_ = IniciarCopiaSimple(tipoDespacho_, documentoRaiz_)

            Case IControladorDespachos.TiposDespacho.PartesII

                estado_ = IniciarPartesII(tipoDespacho_, documentoRaiz_)

        End Select

        Return estado_

    End Function

    Public Function IniciarDespacho(documentoRaiz_ As DocumentoElectronico, sesion_ As IClientSessionHandle) As TagWatcher _
                                                                            Implements IControladorDespachos.IniciarDespacho

        Dim estado_ As TagWatcher = Nothing

        Select Case _tipoDespacho

            Case IControladorDespachos.TiposDespacho.Consolidado

                estado_ = IniciarConsolidado(_tipoDespacho, documentoRaiz_, sesion_)

            Case IControladorDespachos.TiposDespacho.CopiaSimple

                estado_ = IniciarCopiaSimple(_tipoDespacho, documentoRaiz_)

            Case IControladorDespachos.TiposDespacho.PartesII

                estado_ = IniciarPartesII(_tipoDespacho, documentoRaiz_)

        End Select

        Return estado_

    End Function

    Public Function ProcesarDespacho(tipoDespacho_ As IControladorDespachos.TiposDespacho, documentoRaiz_ As DocumentoElectronico) As TagWatcher _
                                                                                                Implements IControladorDespachos.ProcesarDespacho
        Dim estado_ As TagWatcher = New TagWatcher

        With estado_

            If tipoDespacho_ = IControladorDespachos.TiposDespacho.Consolidado Then

                .SetOK()

            Else

                .SetError(Me, "No es el tipo despacho correcto")

            End If

        End With

        Return estado_

    End Function

    Public Function ProcesarDespacho(documentoRaiz_ As DocumentoElectronico) As TagWatcher _
                                        Implements IControladorDespachos.ProcesarDespacho
        Dim estado_ As TagWatcher = New TagWatcher

        With estado_

            If _tipoDespacho = IControladorDespachos.TiposDespacho.Consolidado Then

                .SetOK()

            Else

                .SetError(Me, "No es el tipo despacho correcto")

            End If

        End With

    End Function

    Public Function CerrarDespacho(sesion_ As IClientSessionHandle) As TagWatcher _
                                    Implements IControladorDespachos.CerrarDespacho

        Dim estado_ As TagWatcher = Nothing

        Select Case _tipoDespacho

            Case IControladorDespachos.TiposDespacho.Consolidado

                estado_ = CerrarConsolidado(_tipoDespacho, sesion_)

            Case IControladorDespachos.TiposDespacho.CopiaSimple

                estado_ = CerrarCopiaSimple(_tipoDespacho, sesion_)

        End Select

        Return estado_

    End Function

    Public Function CerrarDespacho(tipoDespacho_ As IControladorDespachos.TiposDespacho, sesion_ As IClientSessionHandle) As TagWatcher _
                                                                                        Implements IControladorDespachos.CerrarDespacho

        Dim estado_ As TagWatcher = Nothing

        Select Case tipoDespacho_

            Case IControladorDespachos.TiposDespacho.Consolidado

                estado_ = CerrarConsolidado(tipoDespacho_, sesion_)

            Case IControladorDespachos.TiposDespacho.CopiaSimple

                estado_ = CerrarCopiaSimple(tipoDespacho_, sesion_)

        End Select

        Return estado_

    End Function

    Public Function ProcesamientoElectronico(tipoDespacho_ As IControladorDespachos.TiposDespacho, tipoProcesamiento_ As IControladorDespachos.TiposProcesamiento) As TagWatcher _
                                                                                                                        Implements IControladorDespachos.ProcesamientoElectronico
        Dim estado_ As TagWatcher = New TagWatcher

        With estado_

            If tipoDespacho_ = IControladorDespachos.TiposDespacho.Consolidado Then

                Select Case tipoProcesamiento_

                    Case IControladorDespachos.TiposProcesamiento.Apertura

                        .SetOK()

                    Case IControladorDespachos.TiposProcesamiento.Cierre

                        .SetOK()

                End Select

            Else

                .SetError(Me, "No es el tipo despacho correcto")

            End If

        End With

    End Function

    Public Function ProcesamientoElectronico(tipoProcesamiento_ As IControladorDespachos.TiposProcesamiento) As TagWatcher _
                                                                Implements IControladorDespachos.ProcesamientoElectronico
        Dim estado_ As TagWatcher = New TagWatcher

        With estado_

            If _tipoDespacho = IControladorDespachos.TiposDespacho.Consolidado Then

                Select Case tipoProcesamiento_

                    Case IControladorDespachos.TiposProcesamiento.Apertura

                        .SetOK()

                    Case IControladorDespachos.TiposProcesamiento.Cierre

                        .SetOK()

                End Select

            Else

                .SetError(Me, "No es el tipo despacho correcto")

            End If

        End With

    End Function

    Public Function EstatusProcesamientoElectronico(idOperacionProcesamiento_ As ObjectId) As TagWatcher _
                                        Implements IControladorDespachos.EstatusProcesamientoElectronico
        Throw New NotImplementedException()
    End Function

    Public Function GeneraRepresentacionImpresa(numeroDocumento_ As Int32) As TagWatcher _
                                                        Implements IControladorDespachos.GeneraRepresentacionImpresa

        Dim respuesta_ = New TagWatcher

        Dim representacionImpresa_ = New RepresentacionPedimento(DocumentoElectronicoDespacho)

        Select Case _tipoDespacho

            Case IControladorDespachos.TiposDespacho.Consolidado

                respuesta_.ObjectReturned = representacionImpresa_.ImprimirFormatoRemesa(numeroDocumento_)

            Case IControladorDespachos.TiposDespacho.PartesII

                respuesta_.ObjectReturned = representacionImpresa_.ImprimirFormatoPartesII(numeroDocumento_)

            Case IControladorDespachos.TiposDespacho.CopiaSimple

                respuesta_.ObjectReturned = representacionImpresa_.ImprimirFormatoCopiaSimple(numeroDocumento_)


        End Select

        Return respuesta_

    End Function

    Public Function GeneraRepresentacionImpresa(tipoDespacho_ As IControladorDespachos.TiposDespacho, numeroDocumento_ As Int32) As TagWatcher _
                                                        Implements IControladorDespachos.GeneraRepresentacionImpresa

        Dim respuesta_ = New TagWatcher

        Dim representacionImpresa_ = New RepresentacionPedimento(DocumentoElectronicoDespacho)

        Select Case tipoDespacho_

            Case IControladorDespachos.TiposDespacho.Consolidado

                respuesta_.ObjectReturned = representacionImpresa_.ImprimirFormatoRemesa(numeroDocumento_)

            Case IControladorDespachos.TiposDespacho.PartesII

                respuesta_.ObjectReturned = representacionImpresa_.ImprimirFormatoPartesII(numeroDocumento_)

            Case IControladorDespachos.TiposDespacho.CopiaSimple

                respuesta_.ObjectReturned = representacionImpresa_.ImprimirFormatoCopiaSimple(numeroDocumento_)


        End Select

        Return respuesta_

    End Function

    Public Function EstatusInventario() As TagWatcher _
        Implements IControladorDespachos.EstatusInventario
        Throw New NotImplementedException()
    End Function

    Public Function EstatusInventario(tipoDespacho_ As IControladorDespachos.TiposDespacho) As TagWatcher _
                                                        Implements IControladorDespachos.EstatusInventario
        Throw New NotImplementedException()
    End Function

    Public Function EstatusInventario(idDespacho As ObjectId) As TagWatcher _
                        Implements IControladorDespachos.EstatusInventario
        Throw New NotImplementedException()
    End Function

    Public Function EstatusInventario(tipoDespacho_ As IControladorDespachos.TiposDespacho, idDespacho As ObjectId) As TagWatcher _
                                                                                Implements IControladorDespachos.EstatusInventario
        Throw New NotImplementedException()
    End Function

    Public Function TrasladarItemsDespacho(idDestino As ObjectId, numeroRemesa_ As Int32, operacionGenerica_ As OperacionGenerica) As TagWatcher _
                        Implements IControladorDespachos.TrasladarItemsDespacho

        Dim estado_ As TagWatcher = New TagWatcher

        With estado_

            If _tipoDespacho = IControladorDespachos.TiposDespacho.Consolidado Then

                Dim enlaceDatos_ = New EnlaceDatos

                Dim operacionesDB_ = enlaceDatos_.GetMongoCollection(Of OperacionGenerica)(New ConstructorControlConsolidado().GetType.Name)

                Dim clon_ = Activator.CreateInstance(New ConstructorControlConsolidado().GetType, New Object() {True, operacionGenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Clone()})

                Dim documentoDestino_ = operacionesDB_.Aggregate().Match(Function(a) idDestino.Equals(a.Id)).ToList()(0).Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Seccion(SCC2)

                Dim remesa_ As Partida = clon_.Seccion(SCC2).Nodos(numeroRemesa_ - 1)

                Dim remesa2_ As Partida = DocumentoElectronicoDespacho.Seccion(SCC2).Nodos(numeroRemesa_ - 1)

                remesa_.NumeroSecuencia = documentoDestino_.Nodos.Count + 1

                remesa_.Attribute(CamposGlobales.CP_IDENTITY).Valor = documentoDestino_.Nodos.Count + 1

                Dim update_ = Builders(Of OperacionGenerica).Update.AddToSet(Function(x) x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts("Cuerpo").Item(0).Nodos(0).Nodos, DirectCast(remesa_, Nodo))

                Dim respuesta_ = operacionesDB_.UpdateOneAsync(Function(e) e.Id = idDestino, update_).Result

                If respuesta_.ModifiedCount <> 0 Then

                    update_ = Builders(Of OperacionGenerica).Update.Set(Function(x) x.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.EstructuraDocumento.Parts("Cuerpo").Item(0).Nodos(0).Nodos(numeroRemesa_ - 1).archivado, True)

                    respuesta_ = operacionesDB_.UpdateOneAsync(Function(e) e.Id = operacionGenerica_.Id, update_).Result

                    If respuesta_.ModifiedCount <> 0 Then

                        .SetOK()

                    Else

                        .SetError(Me, "No se pudo actualizar")

                    End If

                Else

                    .SetError(Me, "No se pudo actualizar")

                End If

            Else

                .SetError(Me, "No es el tipo despacho correcto")

            End If

        End With

        Return estado_

    End Function

    Public Function DepurarItemsDespacho(operacionGenerica As OperacionGenerica) As TagWatcher Implements _
                            IControladorDespachos.DepurarItemsDespacho

        Dim estado_ As TagWatcher = New TagWatcher

        Dim listaArchivar_ = New List(Of Int32)

        Dim contador_ = 1

        With estado_

            If _tipoDespacho = IControladorDespachos.TiposDespacho.Consolidado Then

                For Each documento_ In DocumentoElectronicoDespacho.Seccion(SCC2).Nodos

                    If documento_.archivado = False Then

                        If documento_.Attribute(CP_FECHA_DESPACHO).Valor Is Nothing Then

                            listaArchivar_.Add(contador_)

                        End If

                        contador_ += 1

                    End If

                Next

                .SetOK()

            Else

                .SetError(Me, "No es el tipo despacho correcto")

            End If

            .ObjectReturned = listaArchivar_

        End With

        Return estado_

    End Function

    Public Function Clone() As Object _
            Implements ICloneable.Clone
        Throw New NotImplementedException()
    End Function

    Private Function IniciarConsolidado(tipoDespacho_ As IControladorDespachos.TiposDespacho, documentoRaiz_ As DocumentoElectronico, sesion_ As IClientSessionHandle) As TagWatcher

        Dim estado_ As TagWatcher = New TagWatcher

        Dim controlConsolidados_ = New ConstructorControlConsolidado(True)

        With controlConsolidados_

            .FolioOperacion = documentoRaiz_.Seccion(SREF1).Attribute(CamposReferencia.CP_REFERENCIA).Valor
            .Seccion(SCC1).Attribute(CamposReferencia.CP_REFERENCIA).Valor = documentoRaiz_.Seccion(SREF1).Attribute(CamposReferencia.CP_REFERENCIA).Valor
            .Seccion(SCC1).Attribute(CA_NUMERO_PEDIMENTO_COMPLETO).Valor = documentoRaiz_.Seccion(SREF1).Attribute(CA_NUMERO_PEDIMENTO_COMPLETO).Valor
            .Seccion(SCC1).Attribute(CA_TIPO_OPERACION).Valor = documentoRaiz_.Seccion(SREF1).Attribute(CA_TIPO_OPERACION).Valor
            .Seccion(SCC1).Attribute(CP_OBJECTID_CLIENTE).Valor = documentoRaiz_.Seccion(SREF2).Attribute(CP_OBJECTID_CLIENTE).Valor
            .Seccion(SCC1).Attribute(CA_RAZON_SOCIAL).Valor = documentoRaiz_.Seccion(SREF2).Attribute(CA_RAZON_SOCIAL).Valor
            .Seccion(SCC1).Attribute(CA_RFC_CLIENTE).Valor = documentoRaiz_.Seccion(SREF2).Attribute(CA_RFC_CLIENTE).Valor
            .Seccion(SCC1).Attribute(CA_CVE_PEDIMENTO).Valor = documentoRaiz_.Seccion(SREF1).Attribute(CA_CVE_PEDIMENTO).Valor
            .Seccion(SCC1).Attribute(CA_CVE_PEDIMENTO).ValorPresentacion = documentoRaiz_.Seccion(SREF1).Attribute(CA_CVE_PEDIMENTO).ValorPresentacion
            .Seccion(SCC1).Attribute(CP_MODALIDAD_ADUANA_PATENTE).Valor = documentoRaiz_.Seccion(SREF1).Attribute(CP_MODALIDAD_ADUANA_PATENTE).Valor
            .Seccion(SCC1).Attribute(CP_MODALIDAD_ADUANA_PATENTE).ValorPresentacion = documentoRaiz_.Seccion(SREF1).Attribute(CP_MODALIDAD_ADUANA_PATENTE).ValorPresentacion
            .Seccion(SCC1).Attribute(CA_ADUANA_ENTRADA_SALIDA).Valor = documentoRaiz_.Seccion(SREF1).Attribute(CA_ADUANA_ENTRADA_SALIDA).Valor
            .Seccion(SCC1).Attribute(CA_PATENTE).Valor = documentoRaiz_.Seccion(SREF1).Attribute(CA_PATENTE).Valor
            .Seccion(SCC1).Attribute(CP_ESTATUS).Valor = IControladorDespachos.EstatusAviso.Nuevo

        End With

        Dim operacionesGenerica_ = New OperacionGenerica(controlConsolidados_)

        operacionesGenerica_.FolioOperacion = documentoRaiz_.Seccion(SREF1).Attribute(CamposReferencia.CP_REFERENCIA).Valor

        With estado_

            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With
                {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)(New ConstructorControlConsolidado().GetType.Name)

                Dim result_ = Nothing

                If sesion_ IsNot Nothing Then

                    result_ = operationsDB_.InsertOneAsync(sesion_, operacionesGenerica_).ConfigureAwait(False)

                Else

                    result_ = operationsDB_.InsertOneAsync(operacionesGenerica_).ConfigureAwait(False)

                End If

                Dim consolidadoResult_ = operationsDB_.Aggregate().Match(Function(a) documentoRaiz_.Seccion(SREF1).Attribute(CamposReferencia.CP_REFERENCIA).Valor.Equals(a.FolioOperacion)).ToList()

                If consolidadoResult_.Count > 0 Then

                    .SetOK()
                Else

                    .SetOKBut(Me, "No se guardo todo")

                End If


            End Using

        End With

        Return estado_

    End Function

    Private Function IniciarPartesII(tipoDespacho_ As IControladorDespachos.TiposDespacho, documentoRaiz_ As DocumentoElectronico) As TagWatcher

        Dim estado_ As TagWatcher = Nothing

        Return estado_

    End Function

    Private Function IniciarCopiaSimple(tipoDespacho_ As IControladorDespachos.TiposDespacho, documentoRaiz_ As DocumentoElectronico) As TagWatcher

        Dim estado_ As TagWatcher = Nothing

        Return estado_

    End Function

    Private Function CerrarConsolidado(tipoDespacho_ As IControladorDespachos.TiposDespacho, Optional sesion_ As IClientSessionHandle = Nothing) As TagWatcher

        Dim estado_ As TagWatcher = Nothing

        Return estado_

    End Function

    Private Function CerrarCopiaSimple(tipoDespacho_ As IControladorDespachos.TiposDespacho, Optional sesion_ As IClientSessionHandle = Nothing) As TagWatcher

        Dim estado_ As TagWatcher = Nothing

        Return estado_

    End Function


#End Region

#Region "Métodos"

    Public Sub Dispose() Implements IDisposable.Dispose
        Throw New NotImplementedException()
    End Sub

#End Region

End Class
