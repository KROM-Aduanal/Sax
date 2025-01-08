Imports Microsoft.AspNet.Identity
Imports System.Security.Claims
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin
Imports Microsoft.Owin.Security
Imports System.Threading.Tasks
Imports Wma.Exceptions
Imports AspNet.Identity.MongoDB

Public Class WebRegister
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Sub Register_Click()


        If tbContrasena.Text = tbConfirmarContrasena.Text Then

            Dim tagwatcher_ = New TagWatcher With {.Status = TagWatcher.TypeStatus.Ok}

            Dim tagwatcherTask_ As Task(Of TagWatcher) = Register(tbEmail.Text,
                                                                  tbContrasena.Text,
                                                                  tbPhoneNumber.Text,
                                                                  tbName.Text,
                                                                  tbLastName.Text,
                                                                  tagwatcher_)

            tagwatcherTask_.Wait()

            tagwatcher_ = tagwatcherTask_.Result

            If tagwatcher_.Status = TagWatcher.TypeStatus.Ok Then

                MsgBox("Registro Exitoso")

                Response.Redirect("Login.aspx")


            Else

                MsgBox("Error de conexión " & tagwatcher_.ObjectReturned)

            End If

        Else

            MsgBox("error: contraseñas diferentes")


        End If



    End Sub

    Sub LogOut_Click()

        Dim context_ = HttpContext.Current.GetOwinContext()

        Dim authenticationManager = context_.Authentication

        authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie)


        MsgBox("Sesión cerrada")




    End Sub


    Sub GoStartSesion_Click()

        Response.Redirect("Login.aspx")

    End Sub

    Sub AsignarPrivilegios_Click()

        Response.Redirect("AsignarPrivilegios.aspx")

    End Sub

    Public Async Function Register(email_ As String,
                                   password_ As String,
                                   phoneNumber_ As String,
                                   firstName_ As String,
                                   lastName_ As String,
                                   tagwatcher_ As TagWatcher) As Task(Of TagWatcher)

        Dim context_ = HttpContext.Current.GetOwinContext()

        Dim userManager_ = context_.GetUserManager(Of ApplicationUserManager)()

        Dim user_ = New ApplicationUser() With {.UserName = email_,
                                                .Email = email_,
                                                .CreatedOn = DateTime.Now,
                                                .Version = 1,
                                                .PhoneNumber = phoneNumber_}

        user_.AddClaim(New Claim(ClaimTypes.Email,
                                 email_))

        user_.AddClaim(New Claim(ClaimTypes.HomePhone,
                                 phoneNumber_))

        user_.AddClaim(New Claim("FirstName",
                                 firstName_))

        user_.AddClaim(New Claim("LastName",
                                 lastName_))

        If ckAdminSynApsis.Checked Then

            user_.AddRole("ADMINSYNAPSIS")

        End If

        If ckAdminKromBaseWeb.Checked Then

            user_.AddRole("ADMINKROMBASEWEB")

        End If

        Dim result = Await userManager_.CreateAsync(user_,
                                                    password_).
                                        ConfigureAwait(False)

        If result.Succeeded Then

            Return tagwatcher_

        Else

            tagwatcher_.ObjectReturned = result.Errors(0)

            tagwatcher_.SetError(Me, result.Errors(0))

            Return tagwatcher_

        End If

    End Function

End Class