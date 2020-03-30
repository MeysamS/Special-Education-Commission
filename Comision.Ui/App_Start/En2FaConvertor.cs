using System.IO;
using System.Text;
using System.Web.Mvc;

//add this filter to project in application_start method
//protected void Application_Start()
//{
//    GlobalFilters.Filters.Add(new En2FaConvertor());
//}
namespace CustomActionFilters
{
    public class En2FaConvertor : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var response = filterContext.HttpContext.Response;

            if (response.ContentType == "text/html")
            {
                response.Filter = new En2FaStream(response.Filter);
            }
        }
    }

    public class En2FaStream : Stream
    {
        private Stream stream;

        public En2FaStream(Stream responseStream)
        {
            stream = responseStream;
        }

        #region Properties
        public override bool CanRead
        {
            get { return true; }
        }
        public override bool CanSeek
        {
            get { return true; }
        }
        public override bool CanWrite
        {
            get { return true; }
        }
        public override long Length
        {
            get { return 0; }
        }
        public override long Position { get; set; }
        #endregion Properties

        #region Methods
        public override void Close()
        {
            stream.Close();
        }
        public override void Flush()
        {
            stream.Flush();
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            return stream.Read(buffer, offset, count);
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            return stream.Seek(offset, origin);
        }
        public override void SetLength(long value)
        {
            stream.SetLength(value);
        }
        #endregion

        public override void Write(byte[] buffer, int offset, int count)
        {
            string html = Encoding.UTF8.GetString(buffer, offset, count);
            html = En2Fa(html);
            buffer = Encoding.UTF8.GetBytes(html);
            stream.Write(buffer, 0, buffer.Length);
        }

        public string En2Fa(string str)
        {
            return str.Replace("0", "۰")
                      .Replace("1", "۱")
                      .Replace("2", "۲")
                      .Replace("3", "۳")
                      .Replace("4", "۴")
                      .Replace("5", "۵")
                      .Replace("6", "۶")
                      .Replace("7", "۷")
                      .Replace("8", "۸")
                      .Replace("9", "۹");
        }
    }
}
