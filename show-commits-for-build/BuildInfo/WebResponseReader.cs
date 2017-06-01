using System;
using System.IO;
using System.Net;
using PeanutButter.Utils;

namespace ShowCommitsForBuild.BuildInfo
{
    public class WebResponseReader : IDisposable
    {
        private readonly WebRequest _request;
        private AutoDisposer _disposer;
        private readonly object _lock = new object();
        private readonly WebResponse _response;

        public WebResponseReader(WebRequest request)
        {
            _request = request;
            _disposer = new AutoDisposer();
        }

        public WebResponseReader(WebResponse response)
        {
            _response = response;
            _disposer = new AutoDisposer();
        }

        public string ReadFullResponse()
        {
            CheckDisposed();
            var response = _response ?? _disposer.Add(_request.GetResponse());
            var stream = _disposer.Add(response.GetResponseStream());
            return GetText(stream, (int) response.ContentLength);
        }

        private void CheckDisposed()
        {
            lock (_lock)
            {
                if (_disposer == null)
                    throw new ObjectDisposedException("WebResponseReader already disposed");
            }
        }

        private static string GetText(Stream stream, int contentLength)
        {
            if (stream == null)
                throw new Exception("Build query gets null response )':");
            var buffer = new byte[contentLength];
            var offset = 0;
            var toRead = contentLength;
            while (toRead > 0)
            {
                var read = stream.Read(buffer, offset, toRead);
                offset += read;
                toRead -= read;
            }
            var jsonText = buffer.ToUTF8String();
            return jsonText;
        }

        public void Dispose()
        {
            lock (_lock)
            {
                _disposer?.Dispose();
                _disposer = null;
            }
        }
    }
}