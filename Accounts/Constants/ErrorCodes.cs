using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Constants
{
    public static class ErrorCodes
    {

        public const string userNotFound = "453";
        public const string msgUserNotFound = "El usuario no existe";

        public const string userBlocked = "454";
        //public const string linkUserBlocked = "sdfsdf";
        //public const string smgUserBlocked = string.Format("El usuario está bloqueado, ingrese a este link para desbloquear: {0}.", linkUserBlocked);

        public const string smgUserBlocked = "El usuario está bloqueado, ingrese a este link para desbloquear: ";

        //public const string smgUserBlocked = "El usuario está bloqueado, ingrese a este link para desbloquear: " + linkUserBlocked;
        public const string linkUserBlocked = "http://account.buscadme.net";


        //Porconfirmar
        public const string confirmAccount = "455";
        public const string msgConfirmAccount = "El usuario debe confirmar su cuenta";

        //usuario ha sido bloqueado
        public const string userWasBlocked = "456";
        public const string msgUserWasBlocked = "El usuario ha sido bloqueado";

        //contraseña incorrecta
        public const string incorrectPassword = "457";
        public const string msgIncorrectPassword = "La contraseña es incorrecta";

        //sin permisos
        public const string noPermits = "458";
        public const string msgNoPermits = "No tiene permisos para ingresar al sistema";

        //cuenta ya existe
        public const string accountExists = "459";
        public const string msgAccountExists = "Ya existe una cuenta registrada con ese correo";

        //codigo incorrecto
        public const string incorrectCode = "460";
        public const string msgIncorrectCode = "El código es incorrecto";

        //accion no permitida
        public const string notAllowed = "461";
        public const string msgNotAllowed = "Acción no permitida";


        public const string missingData = "462";
        public const string msgMissingData = "Faltan datos";


        //error desconocido
        public const string defaultCode = "499";
        public const string msgDefaultCode = "Intento de inicio de sesión no válido";


        public static string GetMessage(string code)
        {
            switch (code)
            {
                case userNotFound:
                    return msgUserNotFound;

                case userBlocked:
                    return smgUserBlocked;

                case confirmAccount:
                    return msgConfirmAccount;

                case userWasBlocked:
                    return msgUserWasBlocked;

                case incorrectPassword:
                    return msgIncorrectPassword;

                case noPermits:
                    return msgNoPermits;

                case accountExists:
                    return msgAccountExists;

                case incorrectCode:
                    return msgConfirmAccount;

                case notAllowed:
                    return msgNotAllowed;

                default:
                    return msgDefaultCode;

            }
        }

    }
}
