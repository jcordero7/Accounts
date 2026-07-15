namespace Accounts.Models.ViewModels
{
    public class LoginViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        // Identificador del sistema externo que envía al usuario a loguearse (p. ej. "EV", "NP"),
        // usado luego para saber a dónde redirigir tras un login exitoso.
        public string Sistema { get; set; }
    }
}
