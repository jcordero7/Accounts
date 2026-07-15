using System;
using System.Collections.Generic;
using System.Text;


namespace Accounts.Application.Enumerations
{
    public enum UserState
    {
        Activo = 1,
        Porconfirmar = 2,
        Bloqueado = 3,
      //  CambioContrasena = 4,
        RecobrarContrasena = 4,
      //  NuevaContrasena
    }
}

