Imports Microsoft.AspNet.Identity

Public Class SignOut
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        CerrarSesion()

    End Sub


    Sub CerrarSesion()

        Dim context_ = HttpContext.Current.GetOwinContext()

        Dim authenticationManager = context_.Authentication

        authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie)

        'Dim accessControl_ = New ControladorAccesoKBW()

        'accessControl_.Desconectar()

        HttpContext.Current.Session.Clear()

        HttpContext.Current.Session.Abandon()

        'HttpContext.Current.Request.Cookies.Clear()

        Response.Redirect("http://localhost:14326/Login.aspx")

    End Sub
End Class