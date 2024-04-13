using System;

namespace Immersed.General
{
    public class RequestCallback<T>
    {

        private Action<bool,T,string, string, long> _Success;
        public Action<bool,T,string,string, long> Success
        {
            get { return _Success; }
            set { this._Success = value; }
        }

        private Action<string, long> _Error;

        public Action<string, long> Error
        {
            get { return _Error; }
            set { this._Error = value; }
        }
    }
}
