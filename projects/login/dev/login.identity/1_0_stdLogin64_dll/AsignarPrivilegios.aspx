<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AsignarPrivilegios.aspx.vb" Inherits="WebSessionKromTwo.AsignarPrivilegios" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="fWebPrivilegios" runat="server">
        <div>
            <h2>Registarse</h2>
            <label>Email:</label>
            <asp:TextBox ID="tbEmail" runat="server"></asp:TextBox><br />
            <asp:CheckBox ID="ckAdminSynApsis" Text="Administrador Synapsis" runat="server"></asp:CheckBox><br />
            <asp:CheckBox ID="ckAdminKromBaseWeb" Text="Administrador KromBaseWeb" runat="server"></asp:CheckBox><br />
            <asp:CheckBox ID="ckSynapsisPanIzqModPedimentos" Text="Pedimentos" runat="server"></asp:CheckBox><br />
            <asp:CheckBox ID="ckSynapsisPanIzqModApendices" Text="Apendices" runat="server"></asp:CheckBox><br />
            <asp:CheckBox ID="ckSynapsisPanIzqModClientes" Text="Clientes" runat="server"></asp:CheckBox><br />
            <asp:CheckBox ID="ckSynapsisPanIzqModRegistroReferencias" Text="Referencias" runat="server"></asp:CheckBox><br />
            <asp:CheckBox ID="ckSynapsisPanIzqModRegistroFacturas" Text="Facturas" runat="server"></asp:CheckBox><br />
            <asp:CheckBox ID="ckSynapsisPanIzqModTarifaArancelaria" Text="Tarifa Arencelaria" runat="server"></asp:CheckBox><br />
            <asp:CheckBox ID="ckSynapsisPanIzqModRegistroProductos" Text="Productos" runat="server"></asp:CheckBox><br />
            <asp:CheckBox ID="ckSynapsisPanIzqModRegistroProveedores" Text="Proveedores" runat="server"></asp:CheckBox><br />
            <asp:CheckBox ID="ckSynapsisPanIzqModViajes" Text="Viajes" runat="server"></asp:CheckBox><br />
            <asp:CheckBox ID="ckSynapsisPanIzqModBusquedaGeneral" Text="Busqueda general" runat="server"></asp:CheckBox><br />
             <asp:Button ID="btAsignarPrivilegios" Text="Asignar Privilegios" runat="server" OnClick="AsignarPrivilegios_Click" />
             <asp:Button ID="btLogin" Text="Ir a Iniciar Sesión" runat="server" OnClick="GoStartSesion_Click" />
        </div>
    </form>
</body>
</html>
