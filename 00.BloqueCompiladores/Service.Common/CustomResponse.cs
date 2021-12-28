using ViewModel.Common.Response;

namespace Service.Common
{
    public static class CustomResponse
    {
        public static ResponseViewModel<T> GetOkByResponse<T>(T result, string message = null)
        {
            return new ResponseViewModel<T>
            {
                IsSuccess = true,
                Message = string.IsNullOrWhiteSpace(message) ? "Ok" : message.Trim(),
                Result = result
            };
        }

        public static ResponseViewModel<T> GetErrorByResponse<T>(string message = null, ResponseTypeError errorType = ResponseTypeError.DefaultError)
        {
            return new ResponseViewModel<T>
            {
                IsSuccess = false,
                Message = GetMessageError(errorType, message)
            };
        }

        public static ResponseViewModel<T> GetErrorByResponse<T>(T result, string message = null, ResponseTypeError errorType = ResponseTypeError.DefaultError)
        {
            return new ResponseViewModel<T>
            {
                IsSuccess = false,
                Message = GetMessageError(errorType, message),
                Result = result
            };
        }

        private static string GetMessageError(ResponseTypeError errorType, string message)
        {
            string msgDefaultError = "Ha ocurrido un error:";
            string msgValidationError = "Ha ocurrido un error de validación:";
            string msgUncontrolledError = "Ha ocurrido un error no controlado:";
            string msgCommunicationError = "Ha ocurrido un error de comunicación:";

            string _message = string.IsNullOrWhiteSpace(message) ? "No especificado" : message;

            var index = _message.IndexOf(msgDefaultError);

            if (_message.IndexOf(msgDefaultError) >= 0 ||
                _message.IndexOf(msgValidationError) >= 0 ||
                _message.IndexOf(msgUncontrolledError) >= 0 ||
                _message.IndexOf(msgCommunicationError) >= 0)
                return _message;
            else
            {
                string errorMessage = string.Empty;

                switch (errorType)
                {
                    case ResponseTypeError.DefaultError:
                        errorMessage = string.Format(msgDefaultError + " {0}", _message);
                        break;
                    case ResponseTypeError.ValidationError:
                        errorMessage = string.Format(msgValidationError + " {0}", _message);
                        break;
                    case ResponseTypeError.UncontrolledError:
                        errorMessage = string.Format(msgUncontrolledError + " {0}", _message);
                        break;
                    case ResponseTypeError.CommunicationError:
                        errorMessage = string.Format(msgCommunicationError + " {0}", _message);
                        break;
                    default:
                        errorMessage = string.Format(msgDefaultError + " {0}", _message);
                        break;
                }

                return errorMessage;
            }
        }
    }
}
