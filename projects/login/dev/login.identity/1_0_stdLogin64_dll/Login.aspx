<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="WebSessionKromTwo.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="fwebsesion" runat="server">
        <div>
            <h2>Iniciar Sesión</h2>
            <label>Email:</label>
            <asp:TextBox ID="tbEmail" runat="server"></asp:TextBox><br />
            <label>Contraseña:</label>
            <asp:TextBox ID="tbContrasena" TextMode="Password" runat="server"></asp:TextBox><br />
            <asp:Button ID="btIniciaSesion" Text="Iniciar Sesión" runat="server" OnClick="IniciaSesion_Click" />
            <asp:Button ID="btRegister" Text="Registrarse" runat="server" OnClick="Register_Click" />
        </div>
    </form>
</body>
</html>
