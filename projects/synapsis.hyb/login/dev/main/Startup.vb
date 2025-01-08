
'Imports Microsoft.Owin

'Imports Microsoft.Owin.Security.Cookies
'Imports Microsoft.Owin.Security.Google
'Imports Owin
'Imports System.Configuration

'<Assembly: OwinStartup(GetType(Startup))>

'Public Class Startup
'    Public Sub Configuration(app As IAppBuilder)
'        ' Configuramos la autenticación basada en cookies primero
'        app.UseCookieAuthentication(New CookieAuthenticationOptions With {
'            .AuthenticationType = "ApplicationCookie",
'            .LoginPath = New PathString("/Login.aspx")
'        })

'        ' Luego, configura la autenticación con Google
'        Dim googleOAuth2AuthenticationOptions = New GoogleOAuth2AuthenticationOptions With {
'            .ClientId = ConfigurationManager.AppSettings("GoogleClientId"),
'            .ClientSecret = ConfigurationManager.AppSettings("GoogleClientSecret"),
'            .CallbackPath = New PathString("/signin-google"),
'            .SignInAsAuthenticationType = "ApplicationCookie", ' Asegúrate de especificar el tipo de autenticación
'            .Provider = New GoogleOAuth2AuthenticationProvider With {
'                .OnAuthenticated = Function(context)
'                                       ' Opcional: accede a información del usuario autenticado si es necesario
'                                       Return Threading.Tasks.Task.FromResult(0)
'                                   End Function
'            }
'        }

'        app.UseGoogleAuthentication(googleOAuth2AuthenticationOptions)

'    End Sub
'End Class

Imports Owin
Imports Microsoft.Owin.Security.Cookies
Imports Microsoft.AspNet.Identity
Imports Microsoft.Owin

Public Class Startup
    Public Sub Configuration(app As IAppBuilder)
        app.UseCookieAuthentication(New CookieAuthenticationOptions() With {
            .AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            .LoginPath = New PathString("/Login.Aspx")
        })

        app.CreatePerOwinContext(Of ApplicationDbContext)(Function(options, context)
                                                              Return New ApplicationDbContext()
                                                          End Function)

        app.CreatePerOwinContext(Of ApplicationUserManager)(Function(options, context)
                                                                Return ApplicationUserManager.Create(options, context)
                                                            End Function)
    End Sub

    'Public void ConfigureServices(IServiceCollection services)
    '{
    '    // ... otros servicios

    '    services.AddAuthorization(options =>
    '    {
    '        options.AddPolicy("RequireHomePhone", policy =>
    '        {
    '            policy.RequireClaim(ClaimTypes.HomePhone, "2299575809")
    '        })
    '    })
    '}

End Class