1. Archivo README.md Genérico para Accounts (Frontend / Cliente SSO)
Markdown
# 🔑 Centralized Accounts Portal (SSO Client)

Aplicación web de autenticación y gestión de cuentas de usuario para un ecosistema de aplicaciones distribuidas. Funciona como el portal unificado de acceso (inicio de sesión, registro, recuperación de contraseña y gestión de perfil) que consumen los distintos módulos de negocio integrados para autenticar usuarios mediante una **cookie de sesión compartida (Single Sign-On - SSO)**.

Este proyecto **no tiene lógica de negocio ni base de datos propia**: es la capa de presentación (MVC) que consume la API de identidad [`UserControl`](#repositorio-relacionado) para todo lo relacionado con credenciales, roles y verificación.

---

## 🗺️ Arquitectura de Autenticación y SSO

El sistema implementa un flujo de Single Sign-On (SSO) ligero basado en cookies de dominio compartido:

```text
  [ Usuario ] ----( Intenta acceder )----> [ Apps Satélite (Módulo Ventas, Reportes, etc.) ]
      |                                                |
(No autenticado)                                (Detecta Cookie en .midominio.com)
      |                                                |
      v                                                v
[ Portal Accounts (Este Proyecto) ]             [ Acceso Concedido ]
      |
 (Llama a UserControl, valida credenciales y
  emite Cookie cifrada con Data Protection)
      |
      v
  [ Cliente ] <---( Almacena Cookie en dominio compartido )
Autenticación Federada: Accounts actúa como el portal de inicio de sesión unificado. Tras validar las credenciales del usuario contra la API UserControl, esta le retorna un token JWT de sesión.

Generación de la Cookie: Accounts procesa ese JWT y emite una Cookie de Autenticación (CookieAuthentication) configurada con un dominio raíz común (ej. .midominio.com).

SSO por Cookie Sharing: Al compartir el mismo dominio raíz, cualquier subdominio del ecosistema (ej. ventas.midominio.com, reportes.midominio.com) tiene acceso de lectura a la cookie. Usando el anillo de claves de ASP.NET Core Data Protection, las aplicaciones satélite descifran la cookie de forma local y conceden acceso inmediato sin requerir redirecciones adicionales de login.

Estructura del Proyecto
Plaintext
Accounts/
├── Accounts/                 # Proyecto ASP.NET Core MVC (Controllers, Views, wwwroot) — punto de entrada
├── Accounts.Application/     # Casos de uso e interfaces (Login, User)
└── Accounts.Infrastructure/  # Implementaciones que llaman por HTTP a la API UserControl
ASP.NET Core MVC (Razor Views) sobre .NET 8.

Interfaz modular propia (sidebar + tarjetas de usuario) construida con CSS nativo (wwwroot/css/site.css) sin frameworks de terceros.

Requisitos previos
.NET SDK 8.0

Servidor web IIS o IIS Express (el proyecto local está configurado para correr bajo un host local mapeado, ej. cuentas.midominio.local).

La API de identidad UserControl en ejecución y accesible.

Configuración del Entorno Local
Agrega al archivo hosts (C:\Windows\System32\drivers\etc\hosts en Windows) la redirección para simular el entorno distribuido en local:

Plaintext
127.0.0.1   cuentas.midominio.local
Modifica el perfil de ejecución en Properties/launchSettings.json para que apunte a dicho host de desarrollo.

Asegúrate de que UserControl.Api esté levantada antes de iniciar sesión o registrar usuarios.

Funcionalidades principales
Iniciar sesión / cerrar sesión (/Login/UserLogin)

Crear cuenta de usuario nueva (/User/Create)

Confirmar cuenta activa mediante código de verificación (/Login/ConfirmAccount)

Recuperar y restablecer credenciales (/Login/ForgottenPassword)

Dashboard de usuario: visualización, edición de datos personales y actualización de contraseña.

Redirección automática (SSO) al módulo de destino correspondiente tras un login exitoso.

Notas conocidas / deuda técnica
Accounts.Application y Accounts.Infrastructure apuntan temporalmente a netcoreapp3.1 (fuera de soporte); el proyecto principal Accounts ya corre de manera estable en net8.0. Pendiente unificar el target framework en toda la solución.

Las URLs de conexión con la API se encuentran configuradas directamente en la capa de infraestructura. Pendiente moverlas a archivos de configuración appsettings.json para facilitar despliegues multi-entorno.

No cuenta con cobertura de pruebas automatizadas actualmente.

Repositorio relacionado
UserControl — API REST de identidad que gestiona usuarios, credenciales, roles y emite los tokens consumidos por esta interfaz.