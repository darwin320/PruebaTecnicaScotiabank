namespace PruebaTecnica.Test.Bll.Utils
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// obtiene la excepcion mas profunda
        /// </summary>
        /// <param name="error">Exception actual</param>
        /// <returns>Exception que causo el error</returns>
        public static Exception ExcepcionInterna(this Exception error)
        {
            Exception current = error;
            while (current.InnerException != null)
            {
                current = current.InnerException;
            }
            return current;
        }
    }
}
