<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LoginRegister.aspx.vb" Inherits="WebRegister" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="fWebRegister" runat="server">
        <div>
            <h2>Registarse</h2>
            <label>Email:</label>
            <asp:TextBox ID="tbEmail" runat="server"></asp:TextBox><br />
            <label>Contraseña:</label>
            <asp:TextBox ID="tbContrasena" TextMode="Password" runat="server"></asp:TextBox><br />
            <label>Contraseña:</label>
            <asp:TextBox ID="tbConfirmarContrasena" TextMode="Password" runat="server"></asp:TextBox><br />
            <label>Teléfono:</label>
            <asp:TextBox ID="tbPhoneNumber" TextMode="Phone" runat="server"></asp:TextBox><br />
            <label>Nombre:</label>
            <asp:TextBox ID="tbName" runat="server"></asp:TextBox><br />
            <label>Apellidos:</label>
            <asp:TextBox ID="tbLastName" runat="server"></asp:TextBox><br />
            <asp:CheckBox ID="ckAdminSynApsis" Text="Administrador Synapsis" runat="server"></asp:CheckBox><br />
            <asp:CheckBox ID="ckAdminKromBaseWeb" Text="Administrador KromBaseWeb" runat="server"></asp:CheckBox><br />
            <asp:Button ID="btRegister" Text="Registrar" runat="server" OnClick="Register_Click" />
            <asp:Button ID="btAsignarPrivilegios" Text="Asignar Privilegios" runat="server" OnClick="AsignarPrivilegios_Click" />
            <asp:Button ID="btLogOut" Text="Cerrar Sesión" runat="server" OnClick="LogOut_Click" />
           <asp:Button ID="btLogin" Text="Ir a Iniciar Sesión" runat="server" OnClick="GoStartSesion_Click" />
        </div>
    </form>
</body>
</html>
