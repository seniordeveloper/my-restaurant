using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using MyRestaurant.Common.Enums;

namespace MyRestaurant.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class MyRestaurantApplicationException : Exception
    {
        protected string _stackTrace;

        public MyRestaurantApplicationException(
            ErrorCode errorCode,
            string message = null,
            string parameter = null,
            Exception innerException = null)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
            Parameter = parameter;
            _stackTrace = new System.Diagnostics.StackTrace().ToString();
        }

        public ErrorCode ErrorCode { get; private set; }

        public string Parameter { get; private set; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue(nameof(ErrorCode), ErrorCode);

            base.GetObjectData(info, context);
        }

        public override string StackTrace => _stackTrace;
    }
}
