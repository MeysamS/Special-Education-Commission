using System;
using System.Runtime.InteropServices;

namespace Comision.Model.Common
{
    public class MimeTypeDetector
    {
        [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        private extern static UInt32 FindMimeFromData(
            UInt32 pBC,
            [MarshalAs(UnmanagedType.LPStr)] String pwzUrl,
            [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
            UInt32 cbSize,
            [MarshalAs(UnmanagedType.LPStr)] String pwzMimeProposed,
            UInt32 dwMimeFlags,
            out UInt32 ppwzMimeOut,
            UInt32 dwReserverd
        );

        public string GetMimeType(byte[] content)
        {
            var result = "unknown/unknown";
            try
            {
                byte[] buffer = new byte[256];
                var length = (content.Length > 256) ? 256 : content.Length;
                Array.Copy(content, buffer, length);

                UInt32 mimetype;
                FindMimeFromData(0, null, buffer, 256, null, 0, out mimetype, 0);
                IntPtr mimeTypePtr = new IntPtr(mimetype);
                result = Marshal.PtrToStringUni(mimeTypePtr);
                Marshal.FreeCoTaskMem(mimeTypePtr);
            }
            catch (Exception)
            {
                //Log.WriteError(MethodInfo.GetCurrentMethod(), ex);
            }
            return result;
        }
    }
}
