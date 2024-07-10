using System.Web;

namespace PruebaTecnica.Test.Bll.Utils
{
    public static class EncodeDecodePrueba
    {
        public static string EncodeString(string value)
        {
            return HttpUtility.UrlEncode(value);
        }

        public static string DecodeString(string value)
        {
            return HttpUtility.UrlDecode(value);
        }
    }
}