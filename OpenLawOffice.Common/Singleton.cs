
namespace OpenLawOffice.Common
{
    public class Singleton<T> where T : class, new()
    {
        private static T _instance = default(T);
        protected bool _isInitialized = false;

        public static T Instance
        {
            get
            {
                lock (typeof(T))
                {
                    if (_instance == default(T))
                        _instance = new T();
                }

                return _instance;
            }
        }

        public bool IsInitialized { get { return _isInitialized; } }
    }
}
