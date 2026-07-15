using System;
using System.Collections.Generic;
using System.Text;

namespace UserControl.Core.Constants
{
    public static class MsnUserStatus
    {
        /// <summary>
        /// REVIDAR SI SE OCUPA
        /// </summary>

        //activo
        public const int activeCode = 1;
        public const string activeSubject = "Cuenta activada";
        public const string activeBody = "Su cuenta se ha activado";

        //Porconfirmar
        public const int confirmCode = 2;
        public const string confirmSubject = "Confirmar cuenta";
        public const string confirmBody = "Con este código habilite su cuenta. Código: {0}";

        //bloqueado
        public const int blockCode = 3;
        public const string blockSubject = "Cuenta bloqueada";
        public const string blockBody = "Su cuenta se ha bloqueado, ingresa este código para habilitarla. Código: {0}";

        //RecobrarContrasena
        public const int recoverCode = 4;
        public const string recoverSubject = "Recobrar contraseña";
        public const string recoverBody = "Con el fin de crear una nueva contraseña, se le " +
            "envía este código: {0}";


        //cambio contrasena
        public const int changeCode = 5;
        public const string changeSubject = "Contraseña cambiada";
        public const string changeBody = "Su contraseña ha sido cambiada";

        //nueva contrasena
        public const int newCode = 6;
        public const string newSubject = "Contraseña creada exitosamente";
        public const string newBody = "Su contraseña ha sido creada exitosamente";

        public static (string, string) GetEmailMessage(int state)
        {
            switch (state)
            {
                case recoverCode:
                    return (recoverSubject, recoverBody);

                case confirmCode:
                    return (confirmSubject, confirmBody);

                case blockCode:
                    return (blockSubject, blockBody);

                case changeCode:
                    return (changeSubject, changeBody);

                case activeCode:
                    return (activeSubject, activeBody);

                case newCode:
                    return (newSubject, newBody);

                default:
                    return (recoverSubject, recoverBody);

            }
        }


    }
}
