using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.Common.Response
{
    public class ResponseViewModel<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }

    public enum ResponseTypeError
    {
        DefaultError = 1,
        ValidationError = 2,
        UncontrolledError = 3,
        CommunicationError = 4,
    }
}
