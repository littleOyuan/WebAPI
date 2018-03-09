using System.Runtime.Serialization;

namespace MyAPI.Base
{
    [DataContract]
    public class ResponseBase
    {
        [DataMember]
        public bool IsSuccess { get; set; }

        [DataMember]
        public bool IsBusinessException { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public object Data { get; set; }


        public ResponseBase(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public ResponseBase(bool isSuccess, string errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public ResponseBase(bool isSuccess, bool isBusinessException, string errorMessage)
        {
            IsSuccess = isSuccess;
            IsBusinessException = isBusinessException;
            ErrorMessage = errorMessage;
        }
    }
}