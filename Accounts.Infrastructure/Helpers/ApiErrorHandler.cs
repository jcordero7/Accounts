using System.IO;
using System.Net;
using Accounts.Application.Common;
using Accounts.Application.Constants;
using aplication.Infrastructure.Common;
using Newtonsoft.Json;

namespace Accounts.Infrastructure.Helpers
{
  
    public static class ApiErrorHandler
    {
        public static ApiResponse<T> Handle<T>(WebException ex)
        {
            if (ex.Response is HttpWebResponse errorResponse)
            {
                int statusCode = (int)errorResponse.StatusCode;

                switch (ex.Status)
                {
                    case WebExceptionStatus.Timeout:
                        return new ApiResponse<T>
                        {
                            Success = false,
                            Error = new ErrorResponse(ErrorCodes.TIMEOUT, "La solicitud excedió el tiempo de espera."),
                            TipoError = ErrorType.Technical,
                            StatusCode = 0
                        };

                    case WebExceptionStatus.ConnectFailure:
                        return new ApiResponse<T>
                        {
                            Success = false,
                            Error = new ErrorResponse(ErrorCodes.NETWORK_ERROR, "No se pudo conectar al servidor."),
                            TipoError = ErrorType.Technical,
                            StatusCode = 0
                        };

                    case WebExceptionStatus.ProtocolError:
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string errorBody = reader.ReadToEnd();

                            ErrorResponse? error = null;
                            string codeError;
                            string message;
                            ErrorType tipoError;

                            try
                            {
                                error = JsonConvert.DeserializeObject<ErrorResponse>(errorBody);
                                
                                codeError = error?.Code ?? $"HTTP_{statusCode}";
                                message = error?.Message ?? ex.Message;
                                error = new ErrorResponse(codeError, message);
                                    tipoError = (statusCode >= 400 && statusCode < 500)
                                        ? ErrorType.Business
                                        : ErrorType.Technical;
                               
                            }
                            catch
                            {
                                codeError = $"HTTP_{statusCode}";
                                error = new ErrorResponse(codeError, ex.Message);
                                tipoError = ErrorType.Technical;
                            }

                            return new ApiResponse<T>
                            {
                                Success = false,
                                Error = error,
                                TipoError = tipoError,
                                StatusCode = statusCode
                            };
                        }

                    default:
                        return new ApiResponse<T>
                        {
                            Success = false,
                            Error = new ErrorResponse(ErrorCodes.NETWORK_ERROR, "Ocurrió un error de red no especificado."),
                            TipoError = ErrorType.Technical,
                            StatusCode = statusCode
                        };
                }
            }

            // ❌ No hubo respuesta HTTP (timeout fatal, sin red, etc.)
            return new ApiResponse<T>
            {
                Success = false,
                Error = new ErrorResponse(ErrorCodes.NETWORK_ERROR, ex.Message),
                TipoError = ErrorType.Technical,
                StatusCode = 0
            };
        }
    }

}
