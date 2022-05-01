using System;

namespace asardotnetasync
{

    public enum EAsarException
    {
        ASAR_FILE_CANT_FIND,
        ASAR_FILE_CANT_READ,
        ASAR_INVALID_DESCRIPTOR,
        ASAR_INVALID_FILE_SIZE
    };

    public class AsarException : Exception
    {

        private readonly EAsarException _asarException;
        private readonly string _asarMessage;

        public AsarException(EAsarException ex) : this(ex, "") { }

        public AsarException(EAsarException ex, string message)
        {
            _asarException = ex;
            if (message.Length > 0)
            {
                _asarMessage = message;
                return;
            }
            _asarMessage = GetMessage(ex);
        }

        private string GetMessage(EAsarException ex)
        {
            switch (ex)
            {
                case EAsarException.ASAR_FILE_CANT_FIND:
                    return "Error: The specified file couldn't be found.";
                case EAsarException.ASAR_FILE_CANT_READ:
                    return "Error: File can't be read.";
                case EAsarException.ASAR_INVALID_DESCRIPTOR:
                    return "Error: File's header size is not defined on 4 or 8 bytes.";
                case EAsarException.ASAR_INVALID_FILE_SIZE:
                    return "Error: Data table size shorter than the size specified in in the header.";
                default:
                    return "Error: Unhandled exception !";
            }
        }

        public EAsarException ExceptionCode => _asarException;
        public string ExceptionMessage => _asarMessage;

        public override string ToString()
        {
            return $"(Code {ExceptionCode}) {ExceptionMessage}";
        }
    }
}
