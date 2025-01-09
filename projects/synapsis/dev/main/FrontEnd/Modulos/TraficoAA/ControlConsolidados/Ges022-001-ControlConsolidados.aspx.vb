
#Region "├┴┘├┴┘├┴┘├┴┘├┴┘|├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘   DEPENDENCIAS   ├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘├┴┘"

'RECURSOS DEL CMF
Imports MongoDB.Driver
Imports Syn.Documento
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Operaciones
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher
Imports Wma.Exceptions.TagWatcher.TypeStatus
Imports Syn.Nucleo.RecursosComercioExterior.CamposReferencia
Imports Syn.Nucleo.RecursosComercioExterior.CamposProveedorOperativo
Imports Syn.Nucleo.RecursosComercioExterior.SeccionesControlConsolidados

'OBJETOS DIMENSIONALES (ODS's) Dependencias en MongoDB
Imports Rec.Globals.Controllers
Imports gsol

'UTILERIAS/RECURSOS ADICIONALES
Imports Sax.Web
Imports Rec.Globals.Utils
Imports Syn.CustomBrokers.Controllers.ControladorRecursosAduanales
Imports Syn.CustomBrokers.Controllers
Imports gsol.krom
Imports MongoDB.Bson
Imports SharpCompress.Common
Imports System.IO
Imports System.Web.Hosting
Imports Syn.CustomBrokers.Controllers.ControladorUnidadesMedida
Imports System.Windows.Forms
Imports System.Linq
Imports gsol.Web.Components
Imports Syn.Documento.Componentes
Imports MongoDB.Bson.Serialization.Attributes
Imports System.Drawing.Imaging
Imports gsol.Web.Template
Imports Sax.Web.ControladorBackend
Imports System.Diagnostics.Eventing.Reader
Imports Rec.Globals
Imports System.Threading




#End Region

Public Class Ges022_001_ControlConsolidados
    Inherits ControladorBackend

#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Private _controladorDocumentos As New ControladorDocumento

    Private _idDocumento As ObjectId

    Private _icontroladorMonedas As IControladorMonedas

    Public Property _semV As Boolean

#End Region

#Region "██████ Vinculación c/capas inf █████████       SAX      ████████████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████


    '------ Sobreescrituras del framework Sax ------------
    Public Overrides Sub Inicializa()

        With Buscador

            .DataObject = New ConstructorControlConsolidado()


            .addFilter(SeccionesControlConsolidados.SCC1, CamposReferencia.CP_REFERENCIA, "Referencia")
            .addFilter(SeccionesControlConsolidados.SCC1, CamposPedimento.CA_NUMERO_PEDIMENTO_COMPLETO, "Pedimento")
            .addFilter(SeccionesControlConsolidados.SCC1, CamposClientes.CA_RAZON_SOCIAL, "Cliente")


        End With

        If Not Page.IsPostBack Then

            SetVars("_pbRemesas", PillboxControl.ToolbarModality.Advanced)

        End If

        pbRemesas.Modality = PillboxControl.ToolbarModality.Advanced

        scClaveDocumento.DataEntity = New krom.Anexo22()

        scRecintoFiscal.DataEntity = New krom.Anexo22()

        scTipoContenedor.DataEntity = New krom.Anexo22()


    End Sub

    Public Overrides Sub BotoneraClicNuevo()

        If OperacionGenerica IsNot Nothing Then

            '_empresa = Nothing

        End If
        swcTipoOperacion.Checked = True
        swcTipoOperacion.Enabled = True

        PreparaControles()

        dbcReferencia.EnabledButton = False

        Remesas.Visible = "False"

    End Sub

    Public Overrides Sub BotoneraClicGuardar()

        If Not ProcesarTransaccion(Of ConstructorControlConsolidado)().Status = TypeStatus.Errors Then : End If

    End Sub

    Public Overrides Sub BotoneraClicEditar()

        dbcReferencia.EnabledButton = True

        ccContenedores.UserInteraction = True

        If Remesas.Visible Then

            PreparaTarjetero(PillboxControl.ToolbarModality.Basic, pbRemesas)

        End If

    End Sub

    Public Overrides Sub BotoneraClicBorrar()


    End Sub


    Public Overrides Sub BotoneraClicOtros(ByVal IndexSelected_ As Integer)

        Dim controladorDespachos_ = New ControladorDespachos(New ObjectId, New ObjectId, OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, IControladorDespachos.TiposDespacho.Consolidado)

        Dim estado_ = New TagWatcher

        Select Case IndexSelected_

            Case 10 ' Archivar remesas abiertas

                Dim listaDesaduanamiento_ As Dictionary(Of Int32, Dictionary(Of String, String)) = GetVars("Desaduanamiento")

                Dim listaDesaduanamientoNuevo_ As New Dictionary(Of Int32, Dictionary(Of String, String))

                Dim contador_ = 1

                listaDesaduanamiento_.Remove(pbRemesas.PageIndex)

                For Each lista_ In listaDesaduanamiento_

                    listaDesaduanamientoNuevo_.Add(contador_, lista_.Value)

                    contador_ += 1

                Next

                SetVars("Desaduanamiento", listaDesaduanamientoNuevo_)

                pbRemesas.FiledPillbox(controladorDespachos_.DepurarItemsDespacho(OperacionGenerica).ObjectReturned)

            Case 11 'Trasladar Remesa

                Dim session_ As IClientSessionHandle = Nothing

                '||||====== No se implementó aú la selección de la nueva referencia

                estado_ = controladorDespachos_.TrasladarItemsDespacho(New ObjectId("66abb577ba7327b94c77c1fc"), pbRemesas.PageIndex, OperacionGenerica)

                Dim listaDesaduanamiento_ As Dictionary(Of Int32, Dictionary(Of String, String)) = GetVars("Desaduanamiento")

                Dim listaDesaduanamientoNuevo_ As New Dictionary(Of Int32, Dictionary(Of String, String))

                Dim contador_ = 1

                listaDesaduanamiento_.Remove(pbRemesas.PageIndex)

                For Each lista_ In listaDesaduanamiento_

                    listaDesaduanamientoNuevo_.Add(contador_, lista_.Value)

                    contador_ += 1

                Next

                SetVars("Desaduanamiento", listaDesaduanamientoNuevo_)

                pbRemesas.FiledPillbox()


        End Select

    End Sub

    Public Overrides Function AgregarComponentesBloqueadosInicial() As List(Of WebControl)

        Dim bloqueados_ As New List(Of WebControl)

        bloqueados_.Add(icApertura)

        Return bloqueados_

    End Function

    Public Overrides Function AgregarComponentesBloqueadosEdicion() As List(Of WebControl)

        Dim bloqueadosEdicion_ As New List(Of WebControl)

        bloqueadosEdicion_.Add(swcTipoOperacion)
        bloqueadosEdicion_.Add(scPatente)
        bloqueadosEdicion_.Add(scClaveDocumento)
        bloqueadosEdicion_.Add(dbcReferencia)
        bloqueadosEdicion_.Add(fbcCliente)

        Return bloqueadosEdicion_

    End Function


    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher

        Dim tipoOp_ As Int32 = IIf(swcTipoOperacion.Checked, ControladorRecursosAduanales.TiposOperacionAduanal.Importacion, ControladorRecursosAduanales.TiposOperacionAduanal.Exportacion)


        [Set](dbcReferencia, CP_REFERENCIA, propiedadDelControl_:=PropiedadesControl.Valor)
        [Set](dbcReferencia, CamposPedimento.CA_NUMERO_PEDIMENTO_COMPLETO, propiedadDelControl_:=PropiedadesControl.ValueDetail)
        [Set](IIf(gcPeriodicidad.SelectedIndex = 0, "Semanal", "Mensual"), CamposControlConsolidados.CP_PERIODICIDAD)
        [Set](swcTipoOperacion, CamposPedimento.CA_TIPO_OPERACION, asignarA_:=TiposAsignacion.ValorPresentacion, propiedadDelControl_:=PropiedadesControl.Checked)
        [Set](tipoOp_, CamposPedimento.CA_TIPO_OPERACION, tipoDato_:=Campo.TiposDato.Entero)
        [Set](fbcCliente, CamposClientes.CP_OBJECTID_CLIENTE)
        [Set](fbcCliente, CamposClientes.CA_RAZON_SOCIAL, propiedadDelControl_:=PropiedadesControl.Text)
        [Set](icApertura, CamposControlConsolidados.CP_FECHA_APERTURA)
        [Set](icCierre, CamposControlConsolidados.CP_FECHA_CIERRE_ESTIMADO)
        [Set](scClaveDocumento, CamposPedimento.CA_CVE_PEDIMENTO)
        [Set](scRecintoFiscal, CP_RECINTO_FISCAL)
        [Set](scPatente, CamposPedimento.CP_MODALIDAD_ADUANA_PATENTE)

        [Set](fbcAcuseValor, CamposAcuseValor.CP_ID_FACTURA_ACUSEVALOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](fbcAcuseValor, CamposAcuseValor.CA_NUMERO_ACUSEVALOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icValorMercancia, CamposControlConsolidados.CP_VALOR_MERCANCIA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icNumEconomico, CamposControlConsolidados.CP_NUMERO_ECONOMICO_VEHICULO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPlacas, CamposControlConsolidados.CP_PLACAS, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icPesoBruto, CamposControlConsolidados.CP_PESO_BRUTO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icNumBultos, CamposControlConsolidados.CP_NUMERO_BULTOS, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icMarca, CamposControlConsolidados.CP_MARCA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icObservaciones, CamposControlConsolidados.CP_OBSERVACIONES, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icContenedor, CamposControlConsolidados.CP_CONTENEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scTipoContenedor, CamposControlConsolidados.CP_TIPO_CONTENEDOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](icCandado, CamposControlConsolidados.CP_CANDADO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](scColor, CamposControlConsolidados.CP_COLOR_CANDADO, propiedadDelControl_:=PropiedadesControl.Ninguno)
        [Set](ccContenedores, Nothing, seccion_:=SeccionesControlConsolidados.SCC3, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](pbRemesas, Nothing, seccion_:=SeccionesControlConsolidados.SCC2)

        Return New TagWatcher(1)

    End Function

    Protected Sub PreparaControles()

    End Sub


    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Function AntesRealizarInsercion(ByVal session_ As IClientSessionHandle) As TagWatcher

        [Set](IControladorDespachos.EstatusAviso.Nuevo, CamposControlConsolidados.CP_ESTATUS, tipoDato_:=Campo.TiposDato.Entero)

        Return New TagWatcher(Ok)

    End Function

    Public Overrides Sub RealizarInsercion(ByRef documentoElectronico_ As DocumentoElectronico)

    End Sub

    Public Overrides Function DespuesRealizarInsercion() As TagWatcher

        Return New TagWatcher(Ok)

    End Function


    'EVENTOS PARA MODIFICACIÓN DE DATOS
    Public Overrides Function AntesRealizarModificacion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim x = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

        If GetVars("EstatusAviso") IsNot Nothing Then

            [Set](GetVars("EstatusAviso"), CamposControlConsolidados.CP_ESTATUS, tipoDato_:=Campo.TiposDato.Entero)

        End If

        Return New TagWatcher(Ok) 'tagwatcher_

    End Function

    Public Overrides Sub RealizarModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

        Dim relacionPillboxRemesa_ As Dictionary(Of Int32, Int32) = GetVars("relacionPillboxRemesa")

        Dim seccionRemesas_ = documentoElectronico_.Seccion(SeccionesControlConsolidados.SCC2)

        Dim remesasCambio As DocumentoElectronico = GetVars("RemesasAcuseValor")

        Dim contador = 0

        If GetVars("Desaduanamiento") IsNot Nothing Then

                Dim dicDesaduanamiento_ As Dictionary(Of Int32, Dictionary(Of String, String)) = GetVars("Desaduanamiento")

                For Each despacho_ In dicDesaduanamiento_

                For Each remesa_ In seccionRemesas_.Nodos

                    If remesa_.Attribute(CamposGlobales.CP_IDENTITY).Valor.Equals(despacho_.Key) Then

                        remesa_.Attribute(CamposControlConsolidados.CP_FECHA_DESPACHO).Valor = despacho_.Value.ElementAt(0).Key

                        remesa_.Attribute(CamposControlConsolidados.CP_COLOR_DESADUANAMIENTO).Valor = despacho_.Value.ElementAt(0).Value

                    End If

                Next
            Next

            End If

            If GetVars("FechaCreacion") IsNot Nothing Then

                Dim dicCreacionTipoCambio_ As Dictionary(Of Int32, Dictionary(Of String, String)) = GetVars("FechaCreacion")

                For Each despacho_ In dicCreacionTipoCambio_

                For Each remesa_ In seccionRemesas_.Nodos

                    If remesa_.Attribute(CamposGlobales.CP_IDENTITY).Valor.Equals(relacionPillboxRemesa_.ElementAt(despacho_.Key - 1).Value) Then

                        remesa_.Attribute(CamposControlConsolidados.CP_CREACION).Valor = despacho_.Value.ElementAt(0).Key

                        remesa_.Attribute(CamposControlConsolidados.CP_TIPO_CAMBIO).Valor = despacho_.Value.ElementAt(0).Value

                    End If

                Next
            Next

            End If

            If remesasCambio IsNot Nothing Then

            For Each remesa In seccionRemesas_.Nodos

                With remesasCambio.Seccion(SCC2)

                    remesa.Seccion(SCC5).Attribute(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).Valor = .Nodos(contador).Seccion(SCC5).Attribute(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR).Valor

                    remesa.Seccion(SCC5).Attribute(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor = .Nodos(contador).Seccion(SCC5).Attribute(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor

                    remesa.Seccion(SCC6).Attribute(CamposDestinatario.CA_RAZON_SOCIAL).Valor = .Nodos(contador).Seccion(SCC6).Attribute(CamposDestinatario.CA_RAZON_SOCIAL).Valor

                    remesa.Seccion(SCC6).Attribute(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor = .Nodos(contador).Seccion(SCC6).Attribute(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor
                End With

            Next

            seccionRemesas_.Seccion(SCC4).Nodos = remesasCambio.Seccion(SCC2).Seccion(SCC4).Nodos

        End If

    End Sub

    Public Overrides Function DespuesRealizarModificacion() As TagWatcher

        Return New TagWatcher(Ok)

    End Function


    'EVENTOS PARA PRESENTACIÓN DE DATOS EN FRONTEND
    Public Overrides Sub PreparaModificacion(ByRef documentoElectronico_ As DocumentoElectronico)

    End Sub


    'EVENTO PARA DATOS EXTRA/AUTOMATICOS

    Public Overrides Sub DespuesBuquedaGeneralConDatos()

        With OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

            Dim tipoOperacion_ = .Attribute(CamposPedimento.CA_TIPO_OPERACION).Valor

            Dim estatus_ = .Attribute(CamposControlConsolidados.CP_ESTATUS).Valor

            Dim periodicidad_ = .Attribute(CamposControlConsolidados.CP_PERIODICIDAD).Valor

            Dim relacionPillBoxRemesa_ As New Dictionary(Of Int32, Int32)

            'btSemaforoGris.ToolTip = "Prueba de tootip!!"

            If tipoOperacion_ = 1 Then

                swcTipoOperacion.Checked = True

            Else

                swcTipoOperacion.Checked = False

            End If

            Select Case estatus_

                Case IControladorDespachos.EstatusAviso.Nuevo

                    btEnviarAviso.Visible = True

                    btCerrarConsolidado.Visible = False

                    Remesas.Visible = False

                    SetVars("EstatusAviso", estatus_)

                    imgCandadoAbierto.Visible = True

                    imgCandadoCerrado.Visible = False

                Case IControladorDespachos.EstatusAviso.Abierto

                    btEnviarAviso.Visible = False

                    btCerrarConsolidado.Visible = True

                    Remesas.Visible = True

                    SetVars("EstatusAviso", estatus_)

                    imgCandadoAbierto.Visible = True

                    imgCandadoCerrado.Visible = False

                Case IControladorDespachos.EstatusAviso.Cerrado

                    btEnviarAviso.Visible = False

                    btCerrarConsolidado.Visible = False

                    Remesas.Visible = True

                    SetVars("EstatusAviso", estatus_)

                    imgCandadoAbierto.Visible = False

                    imgCandadoCerrado.Visible = True

            End Select

            lbNumero.Text = pbRemesas.PageIndex.ToString()

            Dim datosDesaduanamiento_ As Dictionary(Of Int32, Dictionary(Of String, String)) = Nothing

            Dim contador_ = 1

            For Each remesa_ In .Seccion(SeccionesControlConsolidados.SCC2).Nodos

                Dim colorFecha_ As Dictionary(Of String, String)

                If DirectCast(remesa_, Partida).archivado = False Then

                    If remesa_.Attribute(CamposControlConsolidados.CP_COLOR_DESADUANAMIENTO).Valor = "VERDE" Then

                        colorFecha_ = New Dictionary(Of String, String) From {{remesa_.Attribute(CamposControlConsolidados.CP_FECHA_DESPACHO).Valor, "VERDE"}}

                        If datosDesaduanamiento_ Is Nothing Then

                            datosDesaduanamiento_ = New Dictionary(Of Integer, Dictionary(Of String, String)) From {{remesa_.Attribute(CamposGlobales.CP_IDENTITY).Valor, colorFecha_}}

                        Else

                            datosDesaduanamiento_.Add(remesa_.Attribute(CamposGlobales.CP_IDENTITY).Valor, colorFecha_)

                        End If

                        SetVars("Desaduanamiento", datosDesaduanamiento_)

                        btSemaforoVerde.Enabled = False

                    ElseIf remesa_.Attribute(CamposControlConsolidados.CP_COLOR_DESADUANAMIENTO).Valor = "ROJO" Then

                        colorFecha_ = New Dictionary(Of String, String) From {{remesa_.Attribute(CamposControlConsolidados.CP_FECHA_DESPACHO).Valor, "ROJO"}}

                        If datosDesaduanamiento_ Is Nothing Then

                            datosDesaduanamiento_ = New Dictionary(Of Integer, Dictionary(Of String, String)) From {{remesa_.Attribute(CamposGlobales.CP_IDENTITY).Valor, colorFecha_}}

                        Else

                            datosDesaduanamiento_.Add(remesa_.Attribute(CamposGlobales.CP_IDENTITY).Valor, colorFecha_)

                        End If

                        SetVars("Desaduanamiento", datosDesaduanamiento_)

                        btSemaforoRojo.Enabled = False

                    End If

                    relacionPillBoxRemesa_.Add(contador_, remesa_.Attribute(CamposGlobales.CP_IDENTITY).Valor)

                    If contador_ = 1 Then

                        If remesa_.Attribute(CamposControlConsolidados.CP_COLOR_DESADUANAMIENTO).Valor = "VERDE" Then

                            btSemaforoGris.Visible = False

                            btSemaforoRojo.Visible = False

                            btSemaforoVerde.Visible = True

                            btSemaforoVerde.ToolTip = "Despacho: " & remesa_.Attribute(CamposControlConsolidados.CP_FECHA_DESPACHO).Valor

                            _semV = True

                        ElseIf remesa_.Attribute(CamposControlConsolidados.CP_COLOR_DESADUANAMIENTO).Valor = "ROJO" Then

                            btSemaforoGris.Visible = False

                            btSemaforoRojo.Visible = True

                            btSemaforoVerde.Visible = False

                            btSemaforoRojo.ToolTip = "Despacho: " & remesa_.Attribute(CamposControlConsolidados.CP_FECHA_DESPACHO).Valor

                        Else


                            btSemaforoGris.Visible = True

                            btSemaforoRojo.Visible = False

                            btSemaforoVerde.Visible = False

                        End If

                    End If

                    contador_ += 1

                End If

            Next

            If relacionPillBoxRemesa_.Count > 0 Then

                SetVars("relacionPillboxRemesa", relacionPillBoxRemesa_)

            End If

            Dim checkedItems_ = New List(Of Integer)

            If periodicidad_ = "Semanal" Then

                checkedItems_.Add(0)

            Else

                checkedItems_.Add(1)

            End If

            gcPeriodicidad.CheckedItems = checkedItems_

        End With

    End Sub

    Public Overrides Sub DespuesBuquedaGeneralSinDatos()

        PreparaTarjetero(PillboxControl.ToolbarModality.Default, pbRemesas)

        Remesas.Visible = False

    End Sub


    'EVENTOS DE MANTENIMIENTO
    Public Overrides Sub LimpiaSesion()

    End Sub

    Public Overrides Sub Limpiar()

    End Sub
    Public Sub pruebachafa(sender_ As Object, e As EventArgs)

        Dim x = sender_.ID

    End Sub

#End Region

#Region "████████████████  QUINTA CAPA  █████████       Reglas locales         ██████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ███████████████████████████████████████████████████████████████████████████████████████████████████

    Protected Sub btGuardarDesaduanamiento_OnClick(sender_ As Object, e As EventArgs)

        Dim colorFecha_ As Dictionary(Of String, String)

        Dim datosDesaduanamiento_ As Dictionary(Of Int32, Dictionary(Of String, String)) = GetVars("Desaduanamiento")

        Dim relacionPillBoxRemesa_ As Dictionary(Of Int32, Int32) = GetVars("relacionPillboxRemesa")


        If scColorSemaforo.Value = "1" Then

            colorFecha_ = New Dictionary(Of String, String) From {{icFechaDespacho.Value.ToString, "VERDE"}}

            btSemaforoGris.Visible = False

            btSemaforoRojo.Visible = False

            btSemaforoVerde.Visible = True

            btSemaforoVerde.ToolTip = "Despacho: " & icFechaDespacho.Value.ToString

        Else

            colorFecha_ = New Dictionary(Of String, String) From {{icFechaDespacho.Value.ToString, "ROJO"}}

            btSemaforoGris.Visible = False

            btSemaforoRojo.Visible = True

            btSemaforoVerde.Visible = False

            btSemaforoRojo.ToolTip = "Despacho: " & icFechaDespacho.Value.ToString

        End If

        If datosDesaduanamiento_ Is Nothing Then

            datosDesaduanamiento_ = New Dictionary(Of Integer, Dictionary(Of String, String)) From {{relacionPillBoxRemesa_(pbRemesas.PageIndex), colorFecha_}}

        Else

            datosDesaduanamiento_.Add(relacionPillBoxRemesa_(pbRemesas.PageIndex), colorFecha_)

        End If

        SetVars("Desaduanamiento", datosDesaduanamiento_)

        icFechaDespacho.Value = ""

        ccDesaduanamiento.Visible = False

    End Sub
    Protected Sub btSemaforoGris_click(sender_ As Object, e As EventArgs)

        ccDesaduanamiento.Visible = True

    End Sub
    Protected Sub gcPeriodicidad_OnCheckedChanged(sender_ As Object, e As EventArgs)

    End Sub
    Protected Sub btSemaforoVerde_click(sender_ As Object, e As EventArgs)

        ccDesaduanamiento.Visible = True

    End Sub
    Protected Sub btSemaforoRojo_click(sender_ As Object, e As EventArgs)

        ccDesaduanamiento.Visible = True

    End Sub
    Protected Sub btImprimeRemesa_click(sender_ As Object, e As EventArgs)

        Dim relacionPillboxRemesa_ As Dictionary(Of Int32, Int32) = GetVars("relacionPillboxRemesa")

        Dim x = relacionPillboxRemesa_(pbRemesas.PageIndex)

        Dim documento_ = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

        Dim despachos_ = New ControladorDespachos(New ObjectId, New ObjectId, documento_, IControladorDespachos.TiposDespacho.Consolidado)

        Dim respuesta_ = despachos_.GeneraRepresentacionImpresa(IControladorDespachos.TiposDespacho.Consolidado, relacionPillboxRemesa_(pbRemesas.PageIndex) - 1).ObjectReturned.ToString

        Dim pdfstring_ = "data:Application/pdf;base64, " + respuesta_

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "openPDF('" & pdfstring_ & "','" & OperacionGenerica.FolioOperacion & "')", True)

    End Sub

    Protected Sub swcTipoOperacion_CheckedChanged(sender_ As Object, e As EventArgs)

    End Sub

    Protected Sub btEnviarAviso_OnClick(sender_ As Object, e As EventArgs)

        btEnviarAviso.Visible = False

        SetVars("EstatusAviso", IControladorDespachos.EstatusAviso.Abierto)

        icApertura.Value = Now

        btCerrarConsolidado.Visible = True

        Remesas.Visible = True

        If Not ProcesarTransaccion(Of ConstructorControlConsolidado)().Status = TypeStatus.Errors Then : End If

    End Sub

    Protected Sub btCerrarConsolidado_OnClick(sender_ As Object, e As EventArgs)

        Dim validaDespachos_ = ValidaDespachos()


        If validaDespachos_.Status = TypeStatus.Errors Then

            Dim faltanDespachar_ As String = Nothing

            Dim lista_ As List(Of Int32) = validaDespachos_.ObjectReturned

            If lista_.Count = 1 Then

                faltanDespachar_ = lista_(0)

                DisplayAlert("Cuidado", "Tiene la remesa" & faltanDespachar_ & " sin despacho, eliminela o pásela a una nueva referencia", "Ok")

            Else

                Dim contador_ = 1

                For Each numeroRemesa_ In lista_

                    If contador_ = 1 Then

                        faltanDespachar_ = numeroRemesa_

                    Else

                        faltanDespachar_ = faltanDespachar_ & ", " & numeroRemesa_

                    End If

                    contador_ += 1

                Next

                DisplayAlert("Cuidado", "Tiene las remesas" & faltanDespachar_ & " sin despacho, eliminelas o páselas a una nueva referencia", "Ok")

            End If

        Else

            Dim sesion_ As IClientSessionHandle = Nothing

            Dim controladorDespacho_ As IControladorDespachos = New ControladorDespachos(New ObjectId, New ObjectId, OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, IControladorDespachos.TiposDespacho.Consolidado)

            Dim resultado_ = controladorDespacho_.CerrarDespacho(sesion_)

            If resultado_.Status = Ok Then

                btEnviarAviso.Visible = False

                SetVars("EstatusAviso", IControladorDespachos.EstatusAviso.Cerrado)

                icCierre.Value = Now

                btCerrarConsolidado.Visible = False

                imgCandadoAbierto.Visible = False

                imgCandadoCerrado.Visible = True

            End If

        End If

    End Sub

    Private Function ValidaDespachos() As TagWatcher

        Dim estado_ = New TagWatcher

        Dim listaSinFecha_ = New List(Of Int32)

        For Each remesa_ In pbRemesas.DataSource

            With OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

                If .Seccion(SeccionesControlConsolidados.SCC2).Nodos(remesa_.Values(0) - 1).Attribute(CamposControlConsolidados.CP_FECHA_DESPACHO).Valor Is Nothing Then

                    estado_.SetError()

                    listaSinFecha_.Add(remesa_.Values(0))

                End If

            End With

        Next

        If estado_.Status = TypeStatus.Empty Then

            estado_.SetOK()

        Else

            estado_.ObjectReturned = listaSinFecha_

        End If

        Return estado_

    End Function

    Protected Sub pbRemesas_CheckedChange(sender_ As Object, e As EventArgs)

        lbNumero.Text = pbRemesas.PageIndex.ToString()

        Dim dicDesaduanamiento_ As Dictionary(Of Int32, Dictionary(Of String, String)) = GetVars("Desaduanamiento")

        Dim relacionPillBoxRemesa_ As Dictionary(Of Int32, Int32) = GetVars("relacionPillboxRemesa")

        If dicDesaduanamiento_ Is Nothing Then

            btSemaforoGris.Visible = True

            btSemaforoRojo.Visible = False

            btSemaforoVerde.Visible = False

        Else

            Try

                Dim num = relacionPillBoxRemesa_(pbRemesas.PageIndex)

                Dim colorDesaduana_ = dicDesaduanamiento_(num).ElementAt(0).Value

                Dim fechaDesaduana_ = dicDesaduanamiento_(relacionPillBoxRemesa_(pbRemesas.PageIndex)).ElementAt(0).Key

                If colorDesaduana_ = "VERDE" Then

                    btSemaforoGris.Visible = False

                    btSemaforoRojo.Visible = False

                    btSemaforoVerde.Visible = True

                    btSemaforoVerde.ToolTip = "Despacho: " & fechaDesaduana_

                Else

                    btSemaforoGris.Visible = False

                    btSemaforoRojo.Visible = True

                    btSemaforoVerde.Visible = False

                    btSemaforoRojo.ToolTip = "Despacho: " & fechaDesaduana_

                End If

            Catch ex As Exception

                btSemaforoGris.Visible = True

                btSemaforoRojo.Visible = False

                btSemaforoVerde.Visible = False

            End Try

        End If

    End Sub

    Protected Sub pbRemesas_Click(sender_ As Object, e As EventArgs)

    End Sub

    Protected Sub AntesDeCambiarEmpresa(ByVal sender_ As FindbarControl, ByVal e As EventArgs)

        'MsgBox(OperacionGenerica.Id.ToString)

        BusquedaGeneral(sender_, e)

    End Sub


#End Region

#Region "██████ Vinculación sexta capa  █████████       SAX      ████████████████████████████████████████████"
    '    ██████   Controladores utilizados                     Documentos por coding para MongoDB      ██████
    '    ██████    1.ControladorClientes                        1. En Empresa:                         ██████
    '    ██████    2.ControladorRecursosAduanales                  a). Domicilios                      ██████
    '    ██████    3.ControladorSecuencias                         b). Contactos                       ██████
    '    ██████                                                                                        ██████
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Private Function TipoDocumento() As List(Of SelectOption)

        Dim recursos_ As ControladorRecursosAduanalesGral = ControladorRecursosAduanalesGral.Buscar(ControladorRecursosAduanalesGral.TiposRecurso.Generales)

        Dim tipoDocumento_ = From data In recursos_.tiposdocumento
                             Where data.archivado = False And data.estado = 1
                             Select data._idtipodocumento, data.descripcion, data.descripcioncorta

        If tipoDocumento_.Count > 0 Then

            Dim dataSource_ As New List(Of SelectOption)

            For index_ As Int32 = 0 To tipoDocumento_.Count - 1

                dataSource_.Add(New SelectOption With
                             {.Value = tipoDocumento_(index_)._idtipodocumento,
                              .Text = tipoDocumento_(index_).descripcioncorta.ToString})

            Next

            Return dataSource_

        End If

        Return Nothing

    End Function


    Protected Sub scModalidadAduanaPatente_Click(sender_ As Object, e As EventArgs)

        scPatente.DataSource = ModalidadSeccionPatente()

    End Sub

    Private Function ModalidadSeccionPatente() As List(Of SelectOption)

        Dim modalidadSeccionPatente_ = ControladorRecursosAduanales.BuscarRecursosAduanales(ControladorRecursosAduanales.TiposRecurso.Generales)

        SetVars("modalidadSeccionPatente", modalidadSeccionPatente_)

        Dim aduanasSeccionesPatente_ = From data In modalidadSeccionPatente_.aduanaspatentes
                                       Where data.archivado = False And data.estado = 1
                                       Select data._idmodalidadaduanapatente, data.modalidad, data.ciudad, data._idaduanaseccion, data.agenteaduanal, data._idpatente

        If aduanasSeccionesPatente_.Count > 0 Then

            Dim dataSource_ As New List(Of SelectOption)

            For index_ As Int32 = 0 To aduanasSeccionesPatente_.Count - 1

                dataSource_.Add(New SelectOption With
                             {.Value = aduanasSeccionesPatente_(index_)._idmodalidadaduanapatente,
                              .Text = aduanasSeccionesPatente_(index_).modalidad & "|" & aduanasSeccionesPatente_(index_).ciudad & "-" & aduanasSeccionesPatente_(index_)._idaduanaseccion &
                              "|" & aduanasSeccionesPatente_(index_).agenteaduanal & "-" & aduanasSeccionesPatente_(index_)._idpatente})

            Next

            Return dataSource_

        End If

        Return Nothing

    End Function

    Protected Sub scTipoContenedor_Click(sender_ As Object, e As EventArgs)

        scPatente.DataSource = TipoContenedor()

    End Sub

    Private Function TipoContenedor() As List(Of SelectOption)

        Dim tipoContenedor_ = New krom.Anexo22()

        'tipoContenedor_.Dimension = IEnlaceDatos.TiposDimension.Vt022TiposContenedoresVehiculosTransporteA10

        Dim aduanasSeccionesPatente_ = From data In tipoContenedor_.Atributos
                                       Where data.Visible = False And data.Orden = 1
                                       Select data.Descripcion

        If aduanasSeccionesPatente_.Count > 0 Then

            Dim dataSource_ As New List(Of SelectOption)

            'For index_ As Int32 = 0 To aduanasSeccionesPatente_.Count - 1

            '    dataSource_.Add(New SelectOption With
            '                 {.Value = aduanasSeccionesPatente_(index_)._idmodalidadaduanapatente,
            '                  .Text = aduanasSeccionesPatente_(index_).modalidad & "|" & aduanasSeccionesPatente_(index_).ciudad & "-" & aduanasSeccionesPatente_(index_)._idaduanaseccion &
            '                  "|" & aduanasSeccionesPatente_(index_).agenteaduanal & "-" & aduanasSeccionesPatente_(index_)._idpatente})

            'Next

            Return dataSource_

        End If

        Return Nothing

    End Function

    Protected Sub fbcAcuseValor_TextChanged(sender_ As Object, e As EventArgs)

        Using controlador_ = New ControladorBusqueda(Of ConstructorAcuseValor)

            Dim lista_ As List(Of SelectOption) = controlador_.Buscar(fbcAcuseValor.Text, New Filtro With {.IdSeccion = SeccionesAcuseValor.SAcuseValor1, .IdCampo = CamposFacturaComercial.CA_NUMERO_FACTURA})

            fbcAcuseValor.DataSource = lista_

        End Using

    End Sub

    Protected Sub fbcAcuseValor_Click(sender_ As Object, e As EventArgs)

        Dim tipoCambio_ = Nothing

        Dim controladorMonedas_ = New ControladorMonedas

        Dim documentoElectronico_ As DocumentoElectronico = GetVars("RemesasAcuseValor")

        If documentoElectronico_ Is Nothing Then

            documentoElectronico_ = OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Clone

        End If

        Using controlador_ = New ControladorBusqueda(Of ConstructorAcuseValor)

            Dim tagwatcher_ = controlador_.ObtenerDocumento(fbcAcuseValor.Value)

            If tagwatcher_.ObjectReturned IsNot Nothing Then

                Dim relacionPillBoxRemesa_ As New Dictionary(Of Int32, Int32)

                relacionPillBoxRemesa_ = GetVars("relacionPillboxRemesa")

                With documentoElectronico_.Seccion(SCC2).Nodos(relacionPillBoxRemesa_(pbRemesas.PageIndex) - 1)

                    Dim documentoAcuseValor_ = tagwatcher_.ObjectReturned

                    Dim partidas_ = documentoAcuseValor_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Seccion(SeccionesAcuseValor.SAcuseValor4)

                    Dim proveedor_ = documentoAcuseValor_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Seccion(SeccionesAcuseValor.SAcuseValor2)

                    Dim destinatario_ = documentoAcuseValor_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Seccion(SeccionesAcuseValor.SAcuseValor3)

                    icValorMercancia.Value = IIf(partidas_ IsNot Nothing, 0, icValorMercancia.Value)

                    For Each partida_ In partidas_.Nodos

                        icValorMercancia.Value += IIf(partida_.Attribute(CamposAcuseValor.CA_VALOR_MERCANCIA_PARTIDA_DOLARES_ACUSEVALOR).Valor IsNot Nothing,
                                                      partida_.Attribute(CamposAcuseValor.CA_VALOR_MERCANCIA_PARTIDA_DOLARES_ACUSEVALOR).Valor, 0)

                        With .Seccion(SCC4).Partida(documentoElectronico_)

                            .Attribute(CamposAcuseValor.CA_DESCRIPCION_PARTIDA_ACUSEVALOR).Valor = partida_.Attribute(CamposAcuseValor.CA_DESCRIPCION_PARTIDA_ACUSEVALOR).Valor

                            .Attribute(CamposFacturaComercial.CA_CANTIDAD_COMERCIAL_PARTIDA).Valor = partida_.Attribute(CamposFacturaComercial.CA_CANTIDAD_COMERCIAL_PARTIDA).Valor

                            .Attribute(CamposAcuseValor.CA_UNIDAD_MEDIDA_FACTURA_PARTIDA_ACUSEVALOR).Valor = partida_.Attribute(CamposAcuseValor.CA_UNIDAD_MEDIDA_FACTURA_PARTIDA_ACUSEVALOR).Valor

                            .Attribute(CamposFacturaComercial.CA_PRECIO_UNITARIO_PARTIDA).Valor = partida_.Attribute(CamposFacturaComercial.CA_PRECIO_UNITARIO_PARTIDA).Valor

                            .Attribute(CamposAcuseValor.CA_VALOR_MERCANCIA_PARTIDA_DOLARES_ACUSEVALOR).Valor = partida_.Attribute(CamposAcuseValor.CA_VALOR_MERCANCIA_PARTIDA_DOLARES_ACUSEVALOR).Valor

                            .Attribute(CamposGlobales.CP_IDENTITY).Valor = .NumeroSecuencia

                        End With

                    Next

                    .Seccion(SCC5).Attribute(CA_RAZON_SOCIAL_PROVEEDOR).Valor = proveedor_.Attribute(CA_RAZON_SOCIAL_PROVEEDOR).Valor

                    .Seccion(SCC5).Attribute(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor = proveedor_.Attribute(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor

                    .Seccion(SCC6).Attribute(CamposDestinatario.CA_RAZON_SOCIAL).Valor = destinatario_.Attribute(CamposDestinatario.CA_RAZON_SOCIAL).Valor

                    .Seccion(SCC6).Attribute(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor = destinatario_.Attribute(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor

                    SetVars("RemesasAcuseValor", documentoElectronico_)

                End With

                OperadorDatos(documentoElectronico_, TiposFlujo.Entrada)

                Dim x As IEntidadDatos = OperacionCopia


            End If

        End Using

        Dim factorTipoCambio_ = controladorMonedas_.ObtenerFactorTipodeCambio("MXN", FechaCambio_:=Now.AddDays(-8))

        If factorTipoCambio_.Status = TypeStatus.Ok Then

            tipoCambio_ = IIf(factorTipoCambio_.ObjectReturned(1) IsNot Nothing, factorTipoCambio_.ObjectReturned(1).tipocambio, Nothing)

        End If

        Dim dicCreacionTipoCambio_ = GetVars("FechaCreacion")

        lbNumero.ToolTip = "Creación: " & Now.AddDays(-2).ToString & Chr(13) & "Tipo de cambio: " & tipoCambio_.ToString

    End Sub

    Protected Sub fbcCliente_TextChanged(sender_ As Object, e As EventArgs)

        Using controlador_ = New ControladorBusqueda(Of ConstructorCliente)

            Dim lista_ As List(Of SelectOption) = controlador_.Buscar(fbcCliente.Text, New Filtro With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})

            fbcCliente.DataSource = lista_

        End Using

    End Sub

    Protected Sub fbcCliente_Click(sender_ As Object, e As EventArgs)

        Using controlador_ = New ControladorBusqueda(Of ConstructorCliente)

            Dim tagwatcher_ = controlador_.ObtenerDocumento(fbcCliente.Value)

            If tagwatcher_.ObjectReturned IsNot Nothing Then

                Dim documentoCliente_ = tagwatcher_.ObjectReturned

            End If

        End Using

    End Sub

#End Region

End Class

