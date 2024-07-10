using Jose;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System.Text;

namespace PruebaTecnica.Test.Bll.Utils
{
    public class JweServiceCrypto
    {
        /// <summary>
        /// Tamaño de valor de encriptado con GCM.
        /// </summary>
        private static readonly int GCM_TAG_LENGTH = 16;

        /// <summary>
        /// Encriptar string de la petición GCM.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        static public String EncryptString(String message, byte[] key, byte[] iv)
        {
            String sRet = "";
            AeadParameters parameters = new AeadParameters(new KeyParameter(key), GCM_TAG_LENGTH * 8, iv);
            GcmBlockCipher cipher = new GcmBlockCipher(new AesEngine());
            cipher.Init(true, parameters);

            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] output = new byte[cipher.GetOutputSize(messageBytes.Length)];
            int len = cipher.ProcessBytes(messageBytes, 0, messageBytes.Length, output, 0);
            cipher.DoFinal(output, len);

            byte[] encrypted = new byte[iv.Length + output.Length];
            Array.Copy(iv, 0, encrypted, 0, iv.Length);
            Array.Copy(output, 0, encrypted, iv.Length, output.Length);

            sRet = Convert.ToBase64String(encrypted);
            return sRet;
        }

        /// <summary>
        /// Desencriptar string con GCM.
        /// </summary>
        /// <param name="cypher"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        static public String DecryptString(String cypher, byte[] key, byte[] iv)
        {
            String sRet = "";
            AeadParameters parameters = new AeadParameters(new KeyParameter(key), GCM_TAG_LENGTH * 8, iv);
            GcmBlockCipher cipher = new GcmBlockCipher(new AesEngine());
            cipher.Init(false, parameters);

            byte[] encrypted = Convert.FromBase64String(cypher);
            byte[] message = new byte[cipher.GetOutputSize(encrypted.Length - iv.Length)];
            int len = cipher.ProcessBytes(encrypted, iv.Length, encrypted.Length - iv.Length, message, 0);
            cipher.DoFinal(message, len);

            sRet = Encoding.UTF8.GetString(message);
            return sRet;
        }

        /// <summary>
        /// Generar token con JWE, usnado la librería de Jose-JWT.
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GenerateTokenJWE(string payload, byte[] key)
        {
            return JWT.Encode(payload, key, JweAlgorithm.DIR, JweEncryption.A256GCM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string StringfyTokenJWE(string payload, byte[] key)
        {
            return JWT.Decode(payload, key, JweAlgorithm.DIR, JweEncryption.A256GCM);
        }

        /// <summary>
        /// Procesar encriptado de un string.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static String ProcessEncrypt(String message, string key, string iv)
        {
            byte[] keyByteArray = Encoding.UTF8.GetBytes(key);
            byte[] ivByteArray = Encoding.UTF8.GetBytes(iv);
            string encString = EncryptString(message, keyByteArray, ivByteArray);
            string jwe = GenerateTokenJWE(encString, keyByteArray);
            return jwe;
        }

        /// <summary>
        /// Procesar desencriptado de un string.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static String ProcessDencrypt(String message, string key, string iv)
        {
            byte[] keyByteArray = Encoding.UTF8.GetBytes(key);
            byte[] ivByteArray = Encoding.UTF8.GetBytes(iv);
            string base64 = StringfyTokenJWE(message, keyByteArray);
            string stringFinal = DecryptString(base64, keyByteArray, ivByteArray);
            return stringFinal;
        }

        /// <summary>
        /// Procesar encriptado de un objecto.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static String EncryptObject(dynamic obj, string key, string iv)
        {
            var strRequest = JsonConvert.SerializeObject(obj);
            return ProcessEncrypt(strRequest, key, iv);
        }

        /// <summary>
        /// Procesar desencriptado de un string.
        /// </summary>
        /// <param name="strResponse"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static String DecryptResponse(string strResponse, string key, string iv)
        {
            try
            {
                return ProcessDencrypt(strResponse, key, iv);
            }
            catch
            {
                return strResponse;
            }
        }
    }
}