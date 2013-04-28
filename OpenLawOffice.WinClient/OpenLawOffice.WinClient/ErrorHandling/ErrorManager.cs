using System;
using System.Collections.Generic;

namespace OpenLawOffice.WinClient.ErrorHandling
{
    public class ErrorManager : Common.Singleton<ErrorManager>
    {
        private int _nextId;

        public static T Create<T>(T error)
            where T : ErrorBase
        {
            error.Init(Instance._nextId++);
            return error;
        }

        public static T CreateAndThrow<T>(T error)
            where T : ErrorBase
        {
            error.Init(Instance._nextId++);
            error.Throw(null);
            return error;
        }

        public static T CreateAndThrow<T>(T error, object data)
            where T : ErrorBase
        {
            error.Init(Instance._nextId++);
            error.Throw(data);
            return error;
        }

        public ErrorManager()
        {
            _nextId = 1;
        }
    }
}
