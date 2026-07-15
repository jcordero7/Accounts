using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Accounts.Application.Common;
using aplication.Infrastructure.Common;
using Newtonsoft.Json;

namespace Accounts.Infrastructure.Helpers
{
    public static class WebRequestHelper
    {


        public static ApiResponse<T> Post2<T>(string url, string tipOpe, object payload, string token = null)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);

                // Ignorar certificado solo en pruebas
                request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                request.Method = tipOpe;
                request.ContentType = "application/json";

                // 🚀 Agregar Bearer Token si viene
                if (!string.IsNullOrWhiteSpace(token))
                {
                    request.Headers.Add("Authorization", $"Bearer {token}");
                }

                if (payload != null)
                {
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        string json = JsonConvert.SerializeObject(payload);
                        streamWriter.Write(json);
                    }
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();

                    //el que se usa actuamente
                    //var data = JsonConvert.DeserializeObject<T>(result);

                    var data = JsonConvert.DeserializeObject<ResponseWrapper<T>>(result);

                    

                    return new ApiResponse<T>
                    {
                        Success = true,
                        Data = data.Data,
                        StatusCode = (int)response.StatusCode,
                        Error = data.ResponseCode
                        
                    };


                }
            }
            catch (WebException ex)
            {
                return ApiErrorHandler.Handle<T>(ex);
            }

        }


        //public static async Task <ApiResponse<T>> Post3<T>(string url, string tipOpe, object payload, string token = null)
        //{
        //    try
        //    {
        //        var request = (HttpWebRequest)WebRequest.Create(url);

        //        // Ignorar certificado solo en pruebas
        //        request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

        //        request.Method = tipOpe;
        //        request.ContentType = "application/json";

        //        // 🚀 Agregar Bearer Token si viene
        //        if (!string.IsNullOrWhiteSpace(token))
        //        {
        //            request.Headers.Add("Authorization", $"Bearer {token}");
        //        }

        //        if (payload != null)
        //        {
        //            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        //            {
        //                string json = JsonConvert.SerializeObject(payload);
        //                streamWriter.Write(json);
        //            }
        //        }

        //        using (var response = (HttpWebResponse)await request.GetResponseAsync())
        //        using (var reader = new StreamReader(response.GetResponseStream()))
        //        {
        //            string result = reader.ReadToEnd();
        //            var data = JsonConvert.DeserializeObject<T>(result);

        //            return new ApiResponse<T>
        //            {
        //                Success = true,
        //                Data = data,
        //                StatusCode = (int)response.StatusCode
        //            };
        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //        return ApiErrorHandler.Handle<T>(ex);
        //    }

        //}



    }

    public class ResponseWrapper<T>
    {
        public T Data { get; set; }

        public ErrorResponse ResponseCode { get; set; }
    }


}


