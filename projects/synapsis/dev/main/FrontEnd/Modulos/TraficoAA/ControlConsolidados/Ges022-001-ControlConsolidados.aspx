<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022-001-ControlConsolidados.aspx.vb" Inherits=".Ges022_001_ControlConsolidados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">

    <% If IsPopup = False Then%>

    <GWC:FindbarControl Label="Buscar Consolidado" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="BusquedaGeneral"  />

    <% End If %>

    <style>

        .cl_Secciones {
            opacity: .6;
            color: #757575;      
            font-size: 24px;
            font-weight: bold;
        }
    
        .cl_Tarjeta {      
            font-size: 24px;
            font-weight: bold;   
            color: #432776;               
            display: flex;        
            justify-content: center;
            align-items: center;   
        
        }

        .cl_Num__Tarjeta {
            background-color: #432776;            
            color: #fff;
            display: flex;        
            border-radius: 50%;           
            justify-content: center;            
            align-items: center;
            width: 60px;
            height: 60px;
        }

        .cl_Num__Tarjeta {
            font-size: 2.4em;
            font-weight: bold;
        }
    
        .cl_Titulo {      
            font-size: 20px;
            font-weight:normal;   
            color: #757575;               
            display: flex;        
            justify-content: center;
            align-items: center;   
        
        }
    
        .cl_SubTitulo {      
            font-size: 35px;
            font-weight: bold;   
            color: #757575;               
            display: block;        
            justify-content: center;
            align-items: center;   
        
        }
    
        .cl__SubTitulo {      
            font-size: 35px;
            font-weight: normal;   
            color: #757575;               
            display: block;        
            justify-content: center;
            align-items: center;   
        
        }
    
        .cl_Sub__Tarjeta {   
            font-size: 70px;
            font-weight: bold;   
            color: #432776;               
            display: flex;        
            justify-content: center;
            align-items: center;  
        }
    
        .cl_Sub__TarjetaNormal {   
            font-size: 60px;
            font-weight: normal;   
            color: #432776;               
            display: flex;        
            justify-content: center;
            align-items: center;  
        }
    
        .cl_Sub___Tarjeta {   
            font-size: 90px;
            font-weight: bold;   
            color: #432776;               
            display: flex;        
            justify-content: center;
            align-items: center;  
        }
    </style>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentCompanyList" runat="server">

    <% If IsPopup = False Then %>

    <GWC:SelectControl CssClass="col-auto company-list-select" runat="server" SearchBarEnabled="false" ID="__SYSTEM_ENVIRONMENT" OnSelectedIndexChanged="CambiarEmpresa" />

    <% End If %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentBody" runat="server">
    <div class="d-flex">
        <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM" HasAutoSave="true" Label="Control consolidados" OnCheckedChanged="MarcarPagina" >
            <Buttonbar runat="server" OnClick="EventosBotonera" >
                <DropdownButtons>
                    <GWC:ButtonItem Text="Descargar"/>
                    <GWC:ButtonItem Text="Imprimir"/>
                    <GWC:ButtonItem Text="Mandar por Correo"/>
                    <GWC:ButtonItem Text="Cancelar remesas abiertas"/>
                    <GWC:ButtonItem Text="Trasladar remesa"/>
                </DropdownButtons>
            </Buttonbar>   

            <Fieldsets>
                <GWC:FieldsetControl runat="server" ID="AvisoConsolidado" Label="Aviso consolidado">
                    <ListControls>
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6">
                            <asp:Panel runat="server" CssClass="col-xs-12 col-md-1">
                                <asp:Image runat="server" ID="imgCandadoAbierto" ImageUrl="/FrontEnd/Recursos/Imgs/CandadoAb.jpg" Visible="true" />
                                <asp:Image runat="server" ID="imgCandadoCerrado" ImageUrl="/FrontEnd/Recursos/Imgs/CandadoCerrado.jpg" Visible="false" />
                            </asp:Panel>
                            <GWC:DualityBarControl runat="server" CssClass="col-xs-12 col-md-11 mb-5" ID="dbcReferencia" Label="Referencia Aduanal" LabelDetail="Pedimento Aduanal" />
                        </asp:Panel> 
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 p-0 d-flex align-items-center">
                            <GWC:GroupControl runat="server" CssClass="col-xs-12 col-md-5 mb-5" Bordered="true" Columns="X2" type="Radio" OnCheckedChanged="gcPeriodicidad_OnCheckedChanged" ID="gcPeriodicidad" Label="Periodicidad">
                                <ListItems>
                                    <GWC:Item Text="Semanal"/>
                                    <GWC:Item Text="Mensual"/>
                                </ListItems>
                            </GWC:GroupControl>    
                            <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-7 mb-5 ml-3" ID="swcTipoOperacion" Label="Tipo de operación" OnText="Importación" OffText="Exportación" Checked="true" Rules="required" OnCheckedChanged="swcTipoOperacion_CheckedChanged"></GWC:SwitchControl>                       
                        </asp:Panel>
                        <GWC:FindboxControl runat="server" CssClass ="col-xs-12 col-md-6 mb-5" ID="fbcCliente" Label ="Cliente" KeyField ="_id" DisplayField ="CA_RAZON_SOCIAL" OnTextChanged ="fbcCliente_TextChanged" OnClick ="fbcCliente_Click" Rules="required"/>
                                <GWC:InputControl runat="server" cssclass="col-xs-12 col-md-3 mb-5" ID="icApertura" Label ="Apertura" Type ="Text" Format="Calendar"/>
                                <GWC:InputControl runat="server" cssclass="col-xs-12 col-md-3 mb-5" ID="icCierre" Label ="Cierre" Type ="Text" Format="Calendar"/>
                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scClaveDocumento" Label="Clave pedimento" KeyField ="t_Cve_Pedimento" DisplayField ="t_ClaveDescripcion" Dimension ="Vt022ClavesPedimentoA02" Rules="required">
                        </GWC:SelectControl>
                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scRecintoFiscal" Label="Recinto fiscal" KeyField ="i_Cve_RecintoFiscalizado" DisplayField ="t_RecintoFiscalizado" Dimension ="Vt022RecintosFiscalizadosA06"/>
                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scPatente" Label="Modalidad | Aduana | Patente" SearchBarEnabled="true" OnClick="scModalidadAduanaPatente_Click"/>
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 mb-5 p-0 d-flex align-items-center jc-center" > 
                            <GWC:ButtonControl runat="server" ID="btEnviarAviso" Label="Envíar aviso" OnClick="btEnviarAviso_OnClick" Visible="false"/>
                            <GWC:ButtonControl runat="server" ID="btCerrarConsolidado" Label="Cerrar consolidado" OnClick="btCerrarConsolidado_OnClick" Visible="false"/>
                        </asp:Panel>
                    </ListControls>
                </GWC:FieldsetControl>        

                <GWC:FieldsetControl runat="server" ID="Remesas" Label="Remesas" Visible="true">
                    <ListControls>   
                        
                        <GWC:CardControl runat="server" ID="ccResumen" Visible="true" CssClass="col-xs-12 col-md-6 mb-5 d-flex align-items-center jc-center">
                            <listcontrols>
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 d-flex align-items-center jc-center">
                                    <asp:Panel runat="server" CssClass="col-xs-12 col-md-1 d-flex align-items-center jc-center">
                                        <asp:Image runat="server" ID="Image1" ImageUrl="/FrontEnd/Recursos/Imgs/notification.jpg" Visible="true" Width="80px" />
                                    </asp:Panel>
                                    <asp:Panel runat="server" CssClass="col-md-4 d-flex align-items-center pr-0 flex-column margin-bottom brt-3">
                                        <asp:Panel runat="server" CssClass="col-md-12 d-flex align-items-center flex-column margin-bottom">                                            
                                            <asp:Panel runat="server" CssClass="col-md-5 d-flex align-items-center align-content-center mb-0 pb-0">
                                                <asp:Label runat="server" ID="lbDiasCerrar" Text="Cierre" class="cl_SubTitulo col-xs-12 p-0 align-items-center col-md-8"></asp:Label>
                                                <asp:Label runat="server" ID="lbDiasCerrar2" Text="en" class="cl__SubTitulo col-xs-12  p-0 col-md-3"></asp:Label>
                                            </asp:Panel>                                             
                                            <asp:Panel runat="server" CssClass="col-md-5 d-flex align-items-center align-content-center">
                                                <asp:Label runat="server" ID="lbDiasCerrarNum" class="col-xs-12 col-md-3 cl_Sub__Tarjeta" Text="0"></asp:Label>
                                                <asp:Label runat="server" ID="lbDiasCerrarNum2" class="col-xs-12 col-md-9 cl_Sub__TarjetaNormal" Text="días"></asp:Label>
                                            </asp:Panel>  
                                        </asp:Panel>     
                                    </asp:Panel>
                                    <asp:Panel runat="server" CssClass="col-md-5 d-flex ml-5 pl-5 align-items-center flex-column margin-bottom">
                                        <asp:Panel runat="server" CssClass="col-md-12 d-flex align-items-center  ml-5 pl-5">
                                            <asp:Panel runat="server" CssClass="col-md-4 d-flex align-items-center margin-bottom">
                                                <asp:Label runat="server" ID="lbRemesasdespachoNum" class="col-xs-12 col-md-3 cl_Sub___Tarjeta" Text="0"></asp:Label>
                                                <asp:Label runat="server" ID="lbRemesasdespachoNum2" class="col-xs-12 col-md-6 cl_Sub__TarjetaNormal" Text="/0"></asp:Label>
                                            </asp:Panel>
                                            <asp:Panel runat="server" CssClass="col-md-5 d-flex p-0 align-items-center flex-column margin-bottom">
                                                <asp:Label runat="server" ID="lbRemesasdespacho" Text="Remesas" class="cl_SubTitulo col-xs-12 col-md-12 mb-0 h-25"></asp:Label>
                                                <asp:Label runat="server" ID="lbRemesasdespacho2" Text="despachadas" class="cl__SubTitulo col-xs-12 col-md-12 mt-0 h-25"></asp:Label>
                                            </asp:Panel>
                                        </asp:Panel>     
                                    </asp:Panel>
                                </asp:Panel>
                            </listcontrols>
                        </GWC:CardControl> 
                        
                        <GWC:PillboxControl runat="server" ID="pbRemesas" KeyField="indice" CssClass="w-100 mt-5 mb-5"  OnCheckedChange="pbRemesas_CheckedChange" OnClick="pbRemesas_Click">
                            <ListControls>                                                                  
                                <asp:Panel runat="server" ID="panel1" CssClass="col-xs-12 col-md-12 d-flex align-items-center jc-center p-0" > 
                                    <asp:Panel runat="server" ID="panel2" CssClass="col-md-1 d-flex align-items-center flex-column margin-bottom" >
                                        <asp:Panel runat="server" ID="panel3" CssClass="col-md-1 d-flex align-items-center flex-column margin-bottom">
                                            <asp:Label runat="server" ID="lbTarjeta" Text="No." class="cl_Tarjeta col-xs-12 col-md-1"></asp:Label>
                                            <asp:Label runat="server" ID="lbNumero" class="cl_Num__Tarjeta col-xs-12 col-md-1" Text="1"></asp:Label>
                                        </asp:Panel>     
                                    </asp:Panel>                                
                                    <GWC:FindboxControl runat="server" CssClass ="col-xs-12 col-md-2 mb-5 pr-0" ID="fbcAcuseValor" Label ="Acuse de valor" KeyField ="_id" DisplayField ="CP_ACUSE_VALOR"  OnTextChanged ="fbcAcuseValor_TextChanged" OnClick ="fbcAcuseValor_Click"/>
                                    <GWC:InputControl runat="server" ID="icValorMercancia" CssClass="col-xs-12 col-md-3 ml-5 pr-0" Label="Valor mercancia (dolares)" />
                                    <asp:Panel runat="server"  ID="panel4" CssClass="col-xs-12 col-md-7 pb-5 d-flex align-items-center justify-content-end">
                                            <GWC:ButtonControl runat="server" CssClass ="mr-5" ID="btImprimeRemesa" OnClick="btImprimeRemesa_click" Icon="Impresora.png"/> 
                                            <GWC:ButtonControl runat="server" ID="btSemaforoGris" OnClick="btSemaforoGris_click" Icon="semaforo_gris.jpg" Visible="false"/>
                                            <GWC:ButtonControl runat="server" ID="btSemaforoVerde" OnClick="btSemaforoVerde_click" Icon="semaforo_verde.png" Visible="false"/>
                                            <GWC:ButtonControl runat="server" ID="btSemaforoRojo" OnClick="btSemaforoRojo_click" Icon="semaforo_rojo.jpg"  Visible="false"/>                                   
                                    </asp:Panel>
                                </asp:Panel> 
                                <asp:Panel runat="server"  ID="panel7" CssClass="col-xs-12 col-md-12 mb-5 d-flex align-items-center jc-center">
                                    <asp:Panel runat="server" ID="panel8" CssClass="col-xs-12 col-md-7 d-flex align-items-center jc-center">
                                        <GWC:CardControl runat="server" ID="ccDesaduanamiento" Visible="false" CssClass="col-xs-12 col-md-6 mb-5">
                                            <listcontrols>
                                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" ID="icFechaDespacho" Label ="Despacho" Type ="Text" Format="Calendar"/>
                                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6" ID="scColorSemaforo" Label="Color">
                                                    <Options>                                                        
                                                        <GWC:SelectOption Value="1" Text="VERDE"/>
                                                        <GWC:SelectOption Value="2" Text="ROJO"/>
                                                    </Options>
                                                </GWC:SelectControl>
                                                <GWC:ButtonControl runat="server" CssClass="col-xs-12 col-md-12" ID="btGuardarDesaduanamiento" Label="Guardar" OnClick="btGuardarDesaduanamiento_OnClick"/>
                                            </listcontrols>
                                        </GWC:CardControl> 
                                    </asp:Panel>
                                </asp:Panel>
                                <GWC:InputControl runat="server" cssclass="col-xs-12 col-md-3 mb-5" ID="icNumEconomico" Label ="Número económico" Type ="Text"/>
                                <GWC:InputControl runat="server" cssclass="col-xs-12 col-md-3 mb-5" ID="icPlacas" Label ="Placas" Type ="Text"/>
                                <GWC:InputControl runat="server" cssclass="col-xs-12 col-md-6 mb-5" ID="icMarca" Label ="Marca" Type ="Text"/>
                                <GWC:InputControl runat="server" cssclass="col-xs-12 col-md-3 mb-5" ID="icPesoBruto" Label ="Peso bruto" Type ="Text"/>
                                <GWC:InputControl runat="server" cssclass="col-xs-12 col-md-3 mb-5" ID="icNumBultos" Label ="Número de bultos" Type ="Text"/>
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 solid-textarea" Type="TextArea" ID="icObservaciones" Label="Observaciones"/>

                                <GWC:FieldsetControl runat="server" Label="Contenedores" Priority="false" CssClass="mt-4">
                                    <ListControls>
                                        <GWC:CatalogControl ID="ccContenedores" runat="server" KeyField="indice" CssClass="w-100 mt-5 mb-5 p-0" OnLoad="pruebachafa">
                                            <Columns>
                                                <GWC:InputControl runat="server" ID="icContenedor" Label="Contenedor" OnLoad="pruebachafa"/>                                    
                                                <GWC:SelectControl runat="server" ID="scTipoContenedor" Label="Tipo" SearchBarEnabled="true" KeyField ="i_ClaveTipoContenedorVehiculoTransporte" DisplayField ="t_DescripcionTipoContenedorVehiculoTransporte" Dimension ="Vt022TiposContenedoresVehiculosTransporteA10"/>
                                                <GWC:InputControl runat="server" ID="icCandado" Type="Text" Label="Candado" />
                                                <GWC:SelectControl runat="server" ID="scColor" Label="Color" OnLoad="pruebachafa">
                                                    <Options>
                                                        <GWC:SelectOption Value="1" Text="VERDE"/>
                                                        <GWC:SelectOption Value="2" Text="ROJO"/>
                                                    </Options>
                                                </GWC:SelectControl>
                                            </Columns>
                                        </GWC:CatalogControl>
                                    </ListControls>
                                </GWC:FieldsetControl>
                            </ListControls> 
                        </GWC:PillboxControl>
                   
                    </ListControls>
                </GWC:FieldsetControl>
            </Fieldsets>
        </GWC:FormControl>
    </div>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="footer" runat="server" OnLoad="pruebachafa">
</asp:Content>
