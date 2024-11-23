using System;

namespace InfinityNetServer.Services.File.Application.Exceptions
{
    public class FileException(FileError error, int httpStatus, params object[] moreInfo) : Exception
    {
        public FileError Error { get; }

        public int HttpStatus { get; }

        public object[] MoreInfo { get; }

    }
}
