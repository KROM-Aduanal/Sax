Imports Gsol.Web.Modulos.Configuracion
Imports Gsol.krom.ControladorAccesoKBW64
Imports Gsol.krom.MenuDinamico
Imports System.Web
Imports Gsol.krom
Imports System.Web.Script.Serialization
Imports System.Web.Services
Imports Gsol.Web.Gsol
Imports System.Security.Claims
Imports Syn.Utils
Imports Gsol.krom.controladores

Public Class Ges025PaginaMaestra
    Inherits System.Web.UI.MasterPage

#Region "Atributos"

    Private _componente As KromComponentes

    Private _nombreUsuario As String

    Private _imagenUsuario As String

    Private _menuDinamico As String

    Const _paginaLogIn As String = "http://localhost:14326/Login.aspx"

    Const _paginaPrincipal As String = "Default.aspx"

    Private _accessControl As ControladorAccesoKBW

#End Region

#Region "Propiedades"

    ReadOnly Property Componente As KromComponentes

        Get

            Return _componente

        End Get

    End Property

    ReadOnly Property NombreUsuario As String

        Get

            Return _nombreUsuario

        End Get

    End Property

    ReadOnly Property ImagenUsuario As String

        Get

            Return _imagenUsuario

        End Get

    End Property

    ReadOnly Property MenuDinamico As String

        Get

            Return _menuDinamico

        End Get

    End Property

#End Region

#Region "Constructores"

    Sub New()

        _componente = New KromComponentes()

        _accessControl = New ControladorAccesoKBW()

        _nombreUsuario = ""

        _imagenUsuario = ""

        _menuDinamico = ""

    End Sub

#End Region

#Region "Metodos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.IsAuthenticated Then

            IntentarConexionDesdeFormularioAutenticado()

            If Session("usuario") Is Nothing Then

                Session("usuario") = _nombreUsuario

                Response.Redirect(_paginaPrincipal)

                Session("usuario") = _nombreUsuario

            Else

                If Not Session("ControladorWeb") Is Nothing Then

                    Dim request_ = HttpContext.Current.Request

                    Dim rutaImagenUsuario_ As String = Server.MapPath("/FrontEnd/Recursos/Imgs/" & Session("DatosUsuario").item("Imagen"))

                    Dim urlImagen_ = request_.Url.GetLeftPart(UriPartial.Authority) & request_.ApplicationPath & "FrontEnd/Recursos/Imgs/" & Session("DatosUsuario").item("Imagen")

                    'urlImagen_ = urlImagen_.Replace(":8083", "")

                    Dim rutaImagenAvatar_ As String = "/FrontEnd/Recursos/Imgs/avatarkrom.png"

                    'Obtiene el nombre del usuario
                    _nombreUsuario = Session("DatosUsuario").item("Nombre")

                    'Valida la imagen de perfil del usuario
                    If Session("DatosUsuario").item("Imagen") <> "" Then

                        If System.IO.File.Exists(rutaImagenUsuario_) Then

                            _imagenUsuario = urlImagen_

                        Else

                            _imagenUsuario = rutaImagenAvatar_

                        End If

                    Else

                        _imagenUsuario = rutaImagenAvatar_

                    End If

                    'Response.AppendHeader("Refresh", ((Session.Timeout * 60) + 5).ToString() + "; Url=http://web.kromaduanal.com/default.aspx")

                    'Se envía el menu dinamico
                    Dim menuDinamico_ As New MenuDinamicoWeb

                    _menuDinamico = menuDinamico_.CrearJSONMenu(Session("EspacioTrabajoExtranet"))


                End If


            End If

        Else


            Response.Redirect(_paginaLogIn)

        End If


    End Sub

    'Obtiene el nombre del usuario con base a la clase PerfilUsuario (Ges003-001-Edicion.PerfilUsuario.aspx)
    Private Sub ObtenerNombreUsuario()

        'Dim perfilUsuario_ As New Ges003_001_PerfilUsuario

        'Dim usuario_ As Dictionary(Of String, String) = perfilUsuario_.InformacionUsuario()

        '_nombreUsuario = If(usuario_.Count > 0, usuario_.Item("nombre_usuario") & " " & usuario_.Item("apellidoPaterno_usuario"), "")

    End Sub

    Private Sub IntentarConexionDesdeFormularioAutenticado()

        Dim datosUsuario = New Dictionary(Of String, String)

        Dim usuario_ = Request.RequestContext.HttpContext.User.Identity.Name

        Dim contrasena_ = "SYNAPSYS" 'Request.Form("password")

        'recordarSesion_ = Request.Form("recordarSesion")

        IniciarSession(usuario_, usuario_, "SI")


    End Sub
    Public Sub IniciarSession(ByVal usuario_ As String,
                              ByVal contrasena_ As String,
                              ByVal recordarSesion_ As String)


        Dim datosUsuario_ As New Dictionary(Of String, String)

        Dim userclaim_ As ClaimsPrincipal = TryCast(Request.RequestContext.HttpContext.User,
                                                    ClaimsPrincipal)

        With datosUsuario_

            .Add("MobilUserID", usuario_)

            .Add("WebServiceUserID", usuario_)

            .Add("WebServicePasswordID", contrasena_)

            .Add("Nombre", userclaim_.Identity.Name)

            .Add("Compañia", "KROM")

            .Add("Telefono", "2299575809")

            .Add("Cumpleaños", "1982/01/12")

            .Add("Correo", usuario_)

            .Add("Imagen", "avatarkrom.png")

        End With

        Dim sistema_ As New Organismo

        Dim espacioTrabajoExtranet_ As IEspacioTrabajo = New EspacioTrabajo

        sistema_.ObtenerEspcioTrabajoUser(espacioTrabajoExtranet_,
                                          usuario_,
                                          contrasena_,
                                          userclaim_)


        'Una vez que autentico correctamente y además tiene su carpeta del perfil correctamente cargada.
        'Si contiene la construcción dinámica, procedemos a crear el sitio

        'Session("SectorEntorno") = _espacioTrabajoExtranet.SectorEntorno(8)

        Session("EspacioTrabajoExtranet") = espacioTrabajoExtranet_

        Session("usuario") = usuario_

        Session("contrasena") = contrasena_

        Session("DatosUsuario") = datosUsuario_


        If recordarSesion_ = "on" Then

            _accessControl.GuardarConexion(New Dictionary(Of String, String) From {{"usuario", usuario_},
                                                                                     {"contrasena", contrasena_}})


        End If

        'Aqui ya está construido el espacio de trabajo
        Session("ControladorWeb") = New ControladorWeb(espacioTrabajoExtranet_, False)

        '_usuario = usuario_

        '_contrasena = contrasena_

    End Sub
#End Region

End Class
