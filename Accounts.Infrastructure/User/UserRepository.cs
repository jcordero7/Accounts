using Accounts.Application.Dtos;
using Accounts.Application.User.Interfaces;
using Accounts.Infrastructure.Helpers;
using aplication.Infrastructure.Common;
using AutoMapper;
using Newtonsoft.Json;
//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UserEntity = Accounts.Application.User.User;

namespace Accounts.Infrastructure.User
{
    public class UserRepository  : IUserRepository
    {

        public ApiResponse<UserEntity> Get(int UserId, string token)
        {
            //  UserLogin
            string url = "https://localhost:44358/api/User/Get/" + UserId;

            var result = WebRequestHelper.Post2<UserEntity>(
            url,
            "GET",
            null,
            token
           );

            return result;

        }


        public Application.User.User Get2(int UserId, string token)
        {
            string url = "https://localhost:44358/api/User/Get/" + UserId;

            //string url = "https://localhost:44358/api/User/" + UserId;

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

            request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            request.Method = "GET";
            request.ContentType = "application/json";

            request.Headers.Add("Authorization", "Bearer " + token);

           // UserResponse userResponse = new UserResponse();

            Application.User.User user = new Application.User.User();


            try
            {
                using (WebResponse response = request.GetResponse())
                {

                    using (Stream strReader = response.GetResponseStream())
                    {
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();

                            user = JsonConvert.DeserializeObject<Application.User.User>(responseBody);

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //ver como tratar la informacion de excepciones
                user = null;
            }

            //return new Application.User.User { 
            //    Id = userResponse.Id,   
            //    Email = userResponse.Email,
            //    Password = userResponse.Password,
            //    Names = userResponse.Names,
            //    SurNames = userResponse.SurNames,
            //    Phone = userResponse.Phone,
            //   BirthDate = userResponse.BirthDate
            //};

            return user;

        }





        //public List<Country> Gets(string nameSearch, string token)
        //{
        //    string url = "https://localhost:44371/api/GetCountry/Gets/" + nameSearch;

        //    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

        //    request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

        //    request.Method = "GET";
        //    request.ContentType = "application/json";

        //    request.Headers.Add("Authorization", "Bearer" + token);

        //    List<Country> respCountry;

        //    try {

        //        using (WebResponse response = request.GetResponse())
        //        {
        //            using (Stream strReader = response.GetResponseStream())
        //            {
        //                using (StreamReader objReader = new StreamReader(strReader))
        //                {
        //                    string responseBody = objReader.ReadToEnd();

        //                    respCountry = JsonConvert.DeserializeObject<List<Country>>(responseBody);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        respCountry = null;
        //    }

        //    return respCountry;
        //}


        public ApiResponse<UserEntity> Create(Application.User.User user, string token)
        {
            //  UserLogin
            string url = "https://localhost:44358/api/User";

            var result = WebRequestHelper.Post2<UserEntity>(
            url,
            "POST",
            user,
            null
           );

           return result;

        }


        public ApiResponse<bool> ChangePassword(ChangePasswordDto changePasswordDto, string token)
        {
            //  UserLogin
            string url = "https://localhost:44358/api/User/changepassword";

            var result = WebRequestHelper.Post2<bool>(
            url,
            "POST",
            changePasswordDto,
            token
           );

            return result;

        }


        public ApiResponse<bool> Edit(Application.User.User user, string token)
        {
            //  UserLogin
            string url = "https://localhost:44358/api/User";

            var result = WebRequestHelper.Post2<bool>(
            url,
            "PUT",
            user,
            token
           );

            return result;

        }


        public bool Edit2(Application.User.User userLogin, string token)
        {
            //  UserLogin
            string url = "https://localhost:44358/api/User";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

            //se ignora la validacion del certificado por cuestiones de pruebas
            request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            request.Method = "POST";
            request.ContentType = "application/json";

            request.Headers.Add("Authorization", "Bearer " + token);

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
                string cod = code.ToString();
                //throw new Exception(ErrorCodes.GetMessage(code.ToString()));

                throw new Exception("Error");
            }

            return resp;
        }



        //public string Create(User user, string token)
        //{
        //    throw new NotImplementedException();
        //}


      

        List<Application.User.User> IUserRepository.Gets(string nameSearch, string token)
        {
            throw new NotImplementedException();
        }
    }

    public class ApiResponses<T>
    {
        public ApiResponses(T data)
        {
            Data = data;
        }

        public T Data { get; set; }

       // public Metadata Meta { get; set; }

    }


}
