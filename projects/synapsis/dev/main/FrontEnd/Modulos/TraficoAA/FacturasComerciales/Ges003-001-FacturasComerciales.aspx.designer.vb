﻿'------------------------------------------------------------------------------
' <generado automáticamente>
'     Este código fue generado por una herramienta.
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código. 
' </generado automáticamente>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class Ges003_001_FacturasComerciales

    '''<summary>
    '''Control __SYSTEM_CONTEXT_FINDER.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents __SYSTEM_CONTEXT_FINDER As Global.Gsol.Web.Components.FindbarControl

    '''<summary>
    '''Control __SYSTEM_ENVIRONMENT.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents __SYSTEM_ENVIRONMENT As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control __SYSTEM_MODULE_FORM.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents __SYSTEM_MODULE_FORM As Global.Gsol.Web.Components.FormControl

    '''<summary>
    '''Control fscGenerales.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fscGenerales As Global.Gsol.Web.Components.FieldsetControl

    '''<summary>
    '''Control lbModoCapturaIA.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbModoCapturaIA As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control lbModoCapturaManual.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbModoCapturaManual As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control lbModoCapturaManualNuevo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbModoCapturaManualNuevo As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control dbcNumFacturaCOVE.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents dbcNumFacturaCOVE As Global.Gsol.Web.Components.DualityBarControl

    '''<summary>
    '''Control icFechaFactura.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icFechaFactura As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icFechaCOVE.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icFechaCOVE As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icSerieFolioFactura.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icSerieFolioFactura As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control fbcCliente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fbcCliente As Global.Gsol.Web.Components.FindboxControl

    '''<summary>
    '''Control fbcPais.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fbcPais As Global.Gsol.Web.Components.FindboxControl

    '''<summary>
    '''Control fbcIncoterm.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fbcIncoterm As Global.Gsol.Web.Components.FindboxControl

    '''<summary>
    '''Control swcEnajenacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents swcEnajenacion As Global.Gsol.Web.Components.SwitchControl

    '''<summary>
    '''Control swcSubdivision.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents swcSubdivision As Global.Gsol.Web.Components.SwitchControl

    '''<summary>
    '''Control icValorFactura.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icValorFactura As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control scMonedaFactura.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scMonedaFactura As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control icValorMercancia.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icValorMercancia As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control scMonedaMercancia.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scMonedaMercancia As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control icPesoTotal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icPesoTotal As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icBultos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icBultos As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icOrdenCompra.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icOrdenCompra As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icReferenciaCliente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icReferenciaCliente As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control fscProveedor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fscProveedor As Global.Gsol.Web.Components.FieldsetControl

    '''<summary>
    '''Control fbcProveedor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fbcProveedor As Global.Gsol.Web.Components.FindboxControl

    '''<summary>
    '''Control scDomiciliosProveedor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scDomiciliosProveedor As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control scVinculacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scVinculacion As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control scMetodoValoracion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scMetodoValoracion As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control swcFungeCertificado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents swcFungeCertificado As Global.Gsol.Web.Components.SwitchControl

    '''<summary>
    '''Control fbcProveedorCertificado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fbcProveedorCertificado As Global.Gsol.Web.Components.FindboxControl

    '''<summary>
    '''Control fscPartidas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fscPartidas As Global.Gsol.Web.Components.FieldsetControl

    '''<summary>
    '''Control pbPartidas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pbPartidas As Global.Gsol.Web.Components.PillboxControl

    '''<summary>
    '''Control lbNumero.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbNumero As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control fbcProducto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fbcProducto As Global.Gsol.Web.Components.FindboxControl

    '''<summary>
    '''Control icCantidadComercial.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icCantidadComercial As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control scUnidadMedidaComercial.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scUnidadMedidaComercial As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control icValorfacturaPartida.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icValorfacturaPartida As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control scMonedaFacturaPartida.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scMonedaFacturaPartida As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control icValorMercanciaPartida.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icValorMercanciaPartida As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control scMonedaMercanciaPartida.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scMonedaMercanciaPartida As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control icPrecioUnitario.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icPrecioUnitario As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control scMonedaPrecioUnitarioPartida.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scMonedaPrecioUnitarioPartida As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control icPesoNeto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icPesoNeto As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control fbcPaisPartida.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fbcPaisPartida As Global.Gsol.Web.Components.FindboxControl

    '''<summary>
    '''Control scMetodoValoracionPartida.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scMetodoValoracionPartida As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control fbcOrdenCompraPartida.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fbcOrdenCompraPartida As Global.Gsol.Web.Components.FindboxControl

    '''<summary>
    '''Control icDescripcionPartida.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icDescripcionPartida As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control swcAplicaCOVE.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents swcAplicaCOVE As Global.Gsol.Web.Components.SwitchControl

    '''<summary>
    '''Control icDescripcionCOVE.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icDescripcionCOVE As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control lbClasificacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbClasificacion As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control icFraccionArancelaria.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icFraccionArancelaria As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icFraccionNico.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icFraccionNico As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icCantidadTarifa.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icCantidadTarifa As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control scUnidadMedidaTarifa.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scUnidadMedidaTarifa As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control lbMercancia.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbMercancia As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control icLote.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icLote As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icNumeroSerie.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icNumeroSerie As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icMarca.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icMarca As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icModelo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icModelo As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icSubmodelo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icSubmodelo As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control icKilometraje.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icKilometraje As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control fscIncrementables.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fscIncrementables As Global.Gsol.Web.Components.FieldsetControl

    '''<summary>
    '''Control icFletes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icFletes As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control scMonedaFletes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scMonedaFletes As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control icSeguros.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icSeguros As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control scMonedaSeguros.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scMonedaSeguros As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control icEmbalajes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icEmbalajes As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control scMonedaEmbalajes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scMonedaEmbalajes As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control icOtrosIncrementables.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icOtrosIncrementables As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control scMonedaOtrosIncrementables.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scMonedaOtrosIncrementables As Global.Gsol.Web.Components.SelectControl

    '''<summary>
    '''Control icDescuentos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents icDescuentos As Global.Gsol.Web.Components.InputControl

    '''<summary>
    '''Control scMonedaDescuentos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents scMonedaDescuentos As Global.Gsol.Web.Components.SelectControl
End Class
