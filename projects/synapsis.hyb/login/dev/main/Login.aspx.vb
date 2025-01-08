Imports System.Net
Imports System.Security.Claims
Imports System.Threading.Tasks
Imports AspNet.Identity.MongoDB
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin
Imports Microsoft.Owin.Security
Imports MongoDB.Driver
Imports Wma.Exceptions

Public Class Login

    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            ' Verifica si el usuario ya está autenticado
            If Request.IsAuthenticated Then
                ' Redirige al usuario autenticado a la página principal
                Response.Redirect("http://localhost:14325/Default.aspx")

            End If

        End If

    End Sub

    Sub IniciaSesion_Click()


        Dim tagwatcher_ = New TagWatcher With {.Status = TagWatcher.TypeStatus.Ok}

        Dim tagwatcherTask_ As Task(Of TagWatcher) = LoginALFA(tbEmail.Text,
                                                               tbContrasena.Text,
                                                               tagwatcher_)

        If tagwatcherTask_.Status = TaskStatus.Faulted Then

            MsgBox("Error en el inicio de sesión")

        Else

            tagwatcherTask_.Wait()

            tagwatcher_ = tagwatcherTask_.Result

            If tagwatcher_.Status = TagWatcher.TypeStatus.Ok Then

                Response.Redirect("http://localhost:14325/Default.aspx")

            Else

                MsgBox("Usuario inválido")

            End If

        End If

    End Sub


    Sub Register_Click()


        Response.Redirect("LoginRegister.aspx")

    End Sub

    Public Async Function LoginALFA(email_ As String,
                                    password_ As String,
                                    tagwatcher_ As TagWatcher) As Task(Of TagWatcher)

        Dim context_ = HttpContext.Current.GetOwinContext()

        Dim userManager_ = context_.GetUserManager(Of ApplicationUserManager)()

        Dim user_ = Await userManager_.FindAsync(email_, password_).ConfigureAwait(False)

        If user_ IsNot Nothing Then

            tagwatcher_ = Await SignInAsync(context_, userManager_, user_, True).ConfigureAwait(False)

            tagwatcher_.ObjectReturned = user_

            Return tagwatcher_

        Else

            tagwatcher_.SetError(Me, "Usuario Inválido")

            Return tagwatcher_

        End If

    End Function

    Private Async Function SignInAsync(context_ As IOwinContext,
                                       userManager_ As ApplicationUserManager,
                                       user_ As ApplicationUser,
                                       isPersistent_ As Boolean) As Task(Of TagWatcher)


        Dim authenticationManager = context_.Authentication

        Dim identity_ = Await userManager_.CreateIdentityAsync(user_,
                                                               DefaultAuthenticationTypes.ApplicationCookie)

        Dim principal_ = New ClaimsPrincipal(identity_)

        ' Custom validation logic
        If principal_.HasClaim(Function(c) c.Type.Contains("SYNAPSIS")) Then

            authenticationManager.SignIn(New AuthenticationProperties() With {
            .IsPersistent = isPersistent_,
            .RedirectUri = "http://localhost:14325/Default.aspx"
        }, identity_)

            Return New TagWatcher With {.Status = TagWatcher.TypeStatus.Ok}

        Else

            MsgBox("Insuficientes Privilegios")

            Return New TagWatcher With {.Status = TagWatcher.TypeStatus.OkBut}
            'Exit Sub ' Prevent further execution if not authorized
        End If

        ' Sign-in the user if claim is valid



    End Function


End Class