using System;

namespace InfinityNetServer.Services.File.Application.Exceptions
{
    public class FileException(FileError error, int httpStatus, params object[] moreInfo) : Exception
    {
        public FileError Error { get; } = error;

        public int HttpStatus { get; } = httpStatus;

        public object[] MoreInfo { get; } = moreInfo;

    }
}
