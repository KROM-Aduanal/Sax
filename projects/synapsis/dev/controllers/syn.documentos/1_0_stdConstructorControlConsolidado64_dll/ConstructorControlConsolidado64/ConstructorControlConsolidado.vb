
Imports gsol.krom
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Documento.Componentes.Campo.TiposDato
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior

Namespace Syn.Documento

    <Serializable()>
    Public Class ConstructorControlConsolidado
        Inherits EntidadDatosDocumento
        Implements ICloneable

#Region "Attributes"

        'Aquí debes cambiar el tipo de documento, según el que usaras, parámetro que requiere el constructor.
        'Por ejemplo: TiposDocumentoElectronico.FacturaComercial
        Private _tipoDocumento As TiposDocumentoElectronico = TiposDocumentoElectronico.ControlConsolidado

#End Region

#Region "Builders"

        Sub New()

            Inicializa(Nothing,
                   _tipoDocumento,
                    True)

        End Sub

        Sub New(ByVal construir_ As Boolean,
            Optional ByVal documentoElectronico_ As DocumentoElectronico = Nothing)

            Inicializa(documentoElectronico_,
                   _tipoDocumento,
                   construir_)

        End Sub

        Public Sub New(ByVal folioDocumento_ As String,
                   ByVal folioOperacion_ As String,
                   ByVal propietario_ As Int32,
                   ByVal nombrePropietario_ As String
                   )

            Inicializa(folioDocumento_,
                   folioOperacion_,
                   propietario_,
                   nombrePropietario_,
                   _tipoDocumento)

        End Sub

#End Region

#Region "Methods"

        Public Overrides Sub ConstruyeEncabezado()

            ' Encabezado principal de la manifestación de valor
            _estructuraDocumento(TiposBloque.Encabezado) = New List(Of Nodo)

            ' Construye las secciones 

            ConstruyeSeccion(seccionEnum_:=SeccionesControlConsolidados.SCC1,
                             tipoBloque_:=TiposBloque.Encabezado,
                             conCampos_:=True)

        End Sub

        Public Overrides Sub ConstruyeEncabezadoPaginasSecundarias()

            'Construir la parte encabezado para páginas secundarias
            '_estructuraDocumento(TiposBloque.EncabezadoPaginasSecundarias) = New List(Of Nodo)

            'Construir una sección
            'ConstruyeSeccion(seccionDocumento_:=SeccionesGenericas.SGS1,
            '                 tipoBloque_:=TiposBloque.EncabezadoPaginasSecundarias,
            '                 conCampos_:=True)

        End Sub

        Public Overrides Sub ConstruyeCuerpo()

            'Construir la parte de cuerpo de la manifestación de valor
            _estructuraDocumento(TiposBloque.Cuerpo) = New List(Of Nodo)

            ' Construye las secciones 

            ConstruyeSeccion(seccionEnum_:=SeccionesControlConsolidados.SCC2,
                            tipoBloque_:=TiposBloque.Cuerpo,
                            conCampos_:=False)

        End Sub

        Public Overrides Sub ConstruyePiePagina()

            'Construir la parte pie de página
            '_estructuraDocumento(TiposBloque.PiePagina) = New List(Of Nodo)

            'Construir una sección
            'ConstruyeSeccion(seccionDocumento_:=SeccionesGenericas.SGS1,
            '                 tipoBloque_:=TiposBloque.PiePagina,
            '                 conCampos_:=True)

        End Sub

        'Será usado solo cuando aplique Followers and Friends (suscripciones).
        Public Sub ConfiguracionNotificaciones()

            'SubscriptionsGroup =
            '   New List(Of subscriptionsgroup) _
            '      From {
            '             New subscriptionsgroup With
            '             {
            '              .active = True,
            '              .toresource = "Vt002EjecutivosMiEmpresa",
            '              ._foreignkeyname = "_id",
            '              ._foreignkey = New ObjectId,
            '              .subscriptions =
            '                New subscriptions With
            '                  {
            '                    .namespaces = New List(Of [namespace]) From
            '                    {
            '                     nsp(1, "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente"),
            '                     nsp(2, "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Encabezado.$[].Nodos.$[].Nodos.$[].Nodos.$[elem]")
            '                    },
            '                   .fields = New List(Of fieldInfo) From
            '                    {
            '                    field(CamposClientes.CP_CVE_EMPRESA, nsp:=2, attr:="Valor"),
            '                    field(CamposClientes.CA_RFC_CLIENTE, nsp:=1, attr:="Valor"),
            '                    field(CamposClientes.CA_CURP_CLIENTE, nsp:=1, attr:="Valor")
            '                    }
            '                 }
            '             },
            '             New subscriptionsgroup With
            '             {
            '              .active = True,
            '              .toresource = "[SynapsisN].[dbo].[Vt022AduanaSeccionA01]",
            '              ._foreignkeyname = "_id",
            '              ._foreignkey = New ObjectId,
            '              .subscriptions =
            '                New subscriptions With
            '                  {
            '                    .namespaces = New List(Of [namespace]) From
            '                    {
            '                     nsp(1, "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente"),
            '                     nsp(2, "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Encabezado.$[].Nodos.$[].Nodos.$[].Nodos.$[elem]")
            '                    },
            '                   .fields = New List(Of fieldInfo) From
            '                   {
            '                    field(CamposClientes.CP_CVE_ADUANA_SECCION, nsp:=2, attr:="Valor")
            '                   }
            '                }
            '             }
            '           }

        End Sub

#End Region

#Region "Functions"

        Public Overrides Function ObtenerCamposSeccion(ByVal idSeccion_ As Integer) As List(Of Nodo)

            Select Case idSeccion_

                ' Aviso consolidado
                Case SeccionesControlConsolidados.SCC1
                    Return New List(Of Nodo) From {
                                             Item(CamposReferencia.CP_REFERENCIA, Texto, longitud_:=40),
                                             Item(CamposPedimento.CA_NUMERO_PEDIMENTO_COMPLETO, Texto, longitud_:=40),
                                             Item(CamposControlConsolidados.CP_PERIODICIDAD, Texto, longitud_:=40),
                                             Item(CamposPedimento.CA_TIPO_OPERACION, Entero, longitud_:=1),
                                             Item(CamposClientes.CP_OBJECTID_CLIENTE, IdObject),
                                             Item(CamposClientes.CA_RAZON_SOCIAL, Texto, longitud_:=120, useAsMetadata_:=True),
                                             Item(CamposClientes.CA_RFC_CLIENTE, Texto, longitud_:=13),
                                             Item(CamposControlConsolidados.CP_FECHA_APERTURA, Fecha),
                                             Item(CamposControlConsolidados.CP_FECHA_CIERRE_ESTIMADO, Fecha),
                                             Item(CamposPedimento.CA_CVE_PEDIMENTO, Texto, longitud_:=2),
                                             Item(CamposReferencia.CP_RECINTO_FISCAL, Entero, longitud_:=3),
                                             Item(CamposPedimento.CP_MODALIDAD_ADUANA_PATENTE, Entero, longitud_:=3),
                                             Item(CamposPedimento.CA_ADUANA_ENTRADA_SALIDA, Texto, longitud_:=3),
                                             Item(CamposPedimento.CA_PATENTE, Texto, longitud_:=4),
                                             Item(CamposControlConsolidados.CP_ESTATUS, Entero, longitud_:=3)
}

                ' Remesas
                Case SeccionesControlConsolidados.SCC2
                    Return New List(Of Nodo) From {
                                             Item(CamposControlConsolidados.CP_NUMERO_REMESA, Texto, longitud_:=120),
                                             Item(CamposAcuseValor.CP_ID_FACTURA_ACUSEVALOR, IdObject),
                                             Item(CamposAcuseValor.CA_NUMERO_ACUSEVALOR, Texto, longitud_:=40),
                                             Item(CamposControlConsolidados.CP_VALOR_MERCANCIA, Texto, longitud_:=13),
                                             Item(CamposControlConsolidados.CP_FECHA_DESPACHO, Texto, longitud_:=450),
                                             Item(CamposControlConsolidados.CP_COLOR_DESADUANAMIENTO, Texto, longitud_:=80),
                                             Item(CamposControlConsolidados.CP_NUMERO_ECONOMICO_VEHICULO, Texto, longitud_:=10),
                                             Item(CamposControlConsolidados.CP_PLACAS, Texto, longitud_:=10),
                                             Item(CamposControlConsolidados.CP_PESO_BRUTO, Texto, longitud_:=10),
                                             Item(CamposControlConsolidados.CP_NUMERO_BULTOS, Texto, longitud_:=20),
                                             Item(CamposControlConsolidados.CP_MARCA, Texto, longitud_:=10),
                                             Item(CamposControlConsolidados.CP_OBSERVACIONES, Texto, longitud_:=80),
                                             Item(CamposControlConsolidados.CP_CREACION, Texto, longitud_:=80),
                                             Item(CamposControlConsolidados.CP_TIPO_CAMBIO, Real, longitud_:=10, cantidadDecimales_:=3),
                                             Item(SeccionesControlConsolidados.SCC3, conCampos_:=False),
                                             Item(SeccionesControlConsolidados.SCC5, conCampos_:=True),
                                             Item(SeccionesControlConsolidados.SCC6, conCampos_:=True),
                                             Item(SeccionesControlConsolidados.SCC4, conCampos_:=False),
                                             Item(CamposGlobales.CP_IDENTITY, Entero)
}

                ' Contenedores
                Case SeccionesControlConsolidados.SCC3
                    Return New List(Of Nodo) From {
                                             Item(CamposControlConsolidados.CP_CONTENEDOR, Texto, longitud_:=120),
                                             Item(CamposControlConsolidados.CP_TIPO_CONTENEDOR, Entero, longitud_:=1),
                                             Item(CamposControlConsolidados.CP_CANDADO, Texto, longitud_:=13),
                                             Item(CamposControlConsolidados.CP_COLOR_CANDADO, Entero, longitud_:=1)
}

                ' Items
                Case SeccionesControlConsolidados.SCC4
                    Return New List(Of Nodo) From {
                                             Item(CamposAcuseValor.CA_DESCRIPCION_PARTIDA_ACUSEVALOR, Texto, longitud_:=250),
                                             Item(CamposFacturaComercial.CA_CANTIDAD_COMERCIAL_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                             Item(CamposAcuseValor.CA_UNIDAD_MEDIDA_FACTURA_PARTIDA_ACUSEVALOR, Texto, longitud_:=80),
                                             Item(CamposFacturaComercial.CA_PRECIO_UNITARIO_PARTIDA, Real, cantidadEnteros_:=15, cantidadDecimales_:=5),
                                             Item(CamposAcuseValor.CA_VALOR_MERCANCIA_PARTIDA_DOLARES_ACUSEVALOR, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
                                             Item(CamposGlobales.CP_IDENTITY, Entero)
}

                ' Proveedor
                Case SeccionesControlConsolidados.SCC5
                    Return New List(Of Nodo) From {
                                             Item(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, Texto, longitud_:=120),
                                             Item(CamposDomicilio.CA_DOMICILIO_FISCAL, Texto, longitud_:=450)
}

                ' Destinatario
                Case SeccionesControlConsolidados.SCC6
                    Return New List(Of Nodo) From {
                                             Item(CamposDestinatario.CA_RAZON_SOCIAL, Texto, longitud_:=120),
                                             Item(CamposDomicilio.CA_DOMICILIO_FISCAL, Texto, longitud_:=450)
}


                Case Else

                    _tagWatcher.SetError(Me,
                        "La sección con clave:" & idSeccion_ & " no esta configurada.")

            End Select

            Return Nothing

        End Function

#End Region

    End Class

End Namespace