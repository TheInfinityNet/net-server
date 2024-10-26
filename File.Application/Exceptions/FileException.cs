using System;

namespace InfinityNetServer.Services.File.Application.Exceptions
{
    public class FileException : Exception
    {
        public FileErrorCode ErrorCode { get; }

        public int HttpStatus { get; }

        public object[] MoreInfo { get; }

        public FileException(FileErrorCode errorCode, int httpStatus, params object[] moreInfo) : base()
        {
            ErrorCode = errorCode;
            HttpStatus = httpStatus;
            MoreInfo = moreInfo;
        }

    }
}
