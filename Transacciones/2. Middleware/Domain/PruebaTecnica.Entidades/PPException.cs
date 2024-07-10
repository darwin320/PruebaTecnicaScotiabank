using System;

namespace PruebaTecnica.Test.Entidades
{
    public class PPException : Exception
    {
        private string _mensajelog;
        public string mensajelog { get { return _mensajelog; } }
        public PPException(string mensajeUsuario, string mensajelog) : base(mensajeUsuario)
        {
            _mensajelog = mensajelog;
        }
    }
}
