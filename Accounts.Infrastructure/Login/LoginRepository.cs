using Accounts.Application.Login;
using Accounts.Application.Login.Interfaces;
using Accounts.Application.User.Interfaces;
using Accounts.Infrastructure.Helpers;
using aplication.Infrastructure.Common;
using Newtonsoft.Json;
//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Infrastructure.Login
{
    public class LoginRepository : ILoginRepository
    {
      

        public string Login2(Application.Login.UserLogin userLogin)
        {
            //  UserLogin
            string url = "https://localhost:44358/api/Token";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

            //se ignora la validacion del certificado por cuestiones de pruebas
            request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            request.Method = "POST";
            request.ContentType = "application/json";

            ///
            // request.ContentType = "application/x-www-form-urlencoded

            // Metodo modificado
            //string postData = "username=miUsuraio&password=MiClave&grant_type=password&client_id=Miclient_id"; byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            var jsonObj = JsonConvert.SerializeObject(userLogin);

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonObj);

            request.ContentLength = byteArray.Length;

            using (Stream dataStream = request.GetRequestStream())
            {
                using (StreamWriter stmw = new StreamWriter(dataStream))
                {
                    stmw.Write(jsonObj);
                }
                dataStream.Write(byteArray, 0, byteArray.Length);
            };
            string resp;

            try
            {
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    resp = reader.ReadToEnd();
                }

            }
             catch (Exception ex)

            {
                //int codeError = 0;
                //bool band;
                //string cod;
                //TRATAR LOS DATOS ACA
                var code = ((System.Net.HttpWebResponse)((System.Net.WebException)ex).Response).StatusCode;
                //band = int.TryParse(code.ToString(), out codeError);
                //cod = band ? code.ToString() : "499";   //int.Parse(exception.Message); // 1;
                
                throw new Exception(code.ToString());
            }

            var RespToken = resp.Split(':');
            var token = RespToken[1].Replace('}', ' ').Replace('"', ' ').Trim();
            return token;
        }


        public ApiResponse<string> Login(Application.Login.UserLogin userLogin)
        {
            //  UserLogin
          //  string url = "https://localhost:44358/api/User";

            string url = "https://localhost:44358/api/Token";

            var result = WebRequestHelper.Post2<string>(
            url,
            "POST",
            userLogin,
            null
           );

            return result;

        }

        public bool EnabledAccount(UserEnabledAccount userEnabled)
        {
            string url = "https://localhost:44358/api/User/enableaccount";
            var result = WebRequestHelper.Post2<bool>(
            url,
            "POST",
            userEnabled,
            null
           );

            return result.Data;
        }

        //public bool ReenviarCodigo(string email, string token)
        //{
        //    string url = "https://localhost:44358/api/User/RequestToken";
        //    var result = WebRequestHelper.Post2<bool>(
        //    url,
        //    "GET",
        //    email,
        //    null
        //   );

        //    return result.Data;
        //}


        public bool EnabledAccount2(UserEnabledAccount userEnabled)
        {

            string url = "https://localhost:44358/api/User/enableaccount";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

            //se ignora la validacion del certificado por cuestiones de pruebas
            request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            request.Method = "POST";
            request.ContentType = "application/json";

            //sin autorizacion
          ///  request.Headers.Add("Authorization", "Bearer " + token);

            ///
            // request.ContentType = "application/x-www-form-urlencoded

            // Metodo modificado
            //string postData = "username=miUsuraio&password=MiClave&grant_type=password&client_id=Miclient_id"; byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            var jsonObj = JsonConvert.SerializeObject(userEnabled);

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonObj);

            request.ContentLength = byteArray.Length;

            using (Stream dataStream = request.GetRequestStream())
            {
                using (StreamWriter stmw = new StreamWriter(dataStream))
                {
                    stmw.Write(jsonObj);
                }
                dataStream.Write(byteArray, 0, byteArray.Length);
            };
            bool resp;

            try
            {

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        // if (strReader == null) return;

                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();

                            resp = JsonConvert.DeserializeObject<bool>(responseBody);

                            // Do something with responseBody
                            // Console.WriteLine(responseBody);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                //TRATAR LOS DATOS ACA
                var code = ((System.Net.HttpWebResponse)((System.Net.WebException)ex).Response).StatusCode;
               // string cod = code.ToString();
                //throw new Exception(ErrorCodes.GetMessage(code.ToString()));

                throw new Exception(code.ToString());
            }

            return resp;

        }




        public bool ConfirmAccount(UserEnabledAccount userEnabled)
        {

            string url = "https://localhost:44358/api/User/confirmAccount";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

            //se ignora la validacion del certificado por cuestiones de pruebas
            request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            request.Method = "POST";
            request.ContentType = "application/json";

            //sin autorizacion
            ///  request.Headers.Add("Authorization", "Bearer " + token);

            ///
            // request.ContentType = "application/x-www-form-urlencoded

            // Metodo modificado
            //string postData = "username=miUsuraio&password=MiClave&grant_type=password&client_id=Miclient_id"; byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            var jsonObj = JsonConvert.SerializeObject(userEnabled);

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonObj);

            request.ContentLength = byteArray.Length;

            using (Stream dataStream = request.GetRequestStream())
            {
                using (StreamWriter stmw = new StreamWriter(dataStream))
                {
                    stmw.Write(jsonObj);
                }
                dataStream.Write(byteArray, 0, byteArray.Length);
            };
            bool resp;

            try
            {

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        // if (strReader == null) return;

                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();

                            resp = JsonConvert.DeserializeObject<bool>(responseBody);

                            // Do something with responseBody
                            // Console.WriteLine(responseBody);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                //TRATAR LOS DATOS ACA
                var code = ((System.Net.HttpWebResponse)((System.Net.WebException)ex).Response).StatusCode;
                // string cod = code.ToString();
                //throw new Exception(ErrorCodes.GetMessage(code.ToString()));
                throw new Exception(code.ToString());
            }

            return resp;

        }

        public bool RequestToken2(string email)
        {

            string url = "https://localhost:44358/api/User/RequestToken/" + email;

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

            request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            request.Method = "GET";
            request.ContentType = "application/json";

            // request.Headers.Add("Authorization", "Bearer " + token);

            //  Application.User.User user;

            bool result;

            try
            {
                using (WebResponse response = request.GetResponse())
                {

                    using (Stream strReader = response.GetResponseStream())
                    {
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();

                            result = JsonConvert.DeserializeObject<bool>(responseBody);

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //ver como tratar la informacion de excepciones
                result = false;
            }

            return result;

        }


        public bool RequestToken(string email)
        {
            string url = "https://localhost:44358/api/User/RequestToken/" + email;
            var result = WebRequestHelper.Post2<bool>(
            url,
            "GET",
            null,
            null
           );

            return result.Data;
        }


        public bool ForgottenPassword(string email)
        {

            UserRecoverPassword userRecoverPassword = new UserRecoverPassword { Email = email };

            string url = "https://localhost:44358/api/User/RecoverPassword";
            var result = WebRequestHelper.Post2<bool>(
            url,
            "POST",
            userRecoverPassword,
            null
           );

            return result.Data;

        }

        public bool ResetPassword(string token, string email)
        {

            UserNewPassword userNewPassword = new UserNewPassword { Code = token, Email = email };

            string url = "https://localhost:44358/api/User/NewPassword";
            var result = WebRequestHelper.Post2<bool>(
            url,
            "POST",
            userNewPassword,
            null
           );

            return result.Data;

        }

    }

    public class UserRecoverPassword
    {
        public string Email { get; set; }
    }

    public class UserNewPassword
    {
        public string Email { get; set; }

        public string Code { get; set; }

    }


}
