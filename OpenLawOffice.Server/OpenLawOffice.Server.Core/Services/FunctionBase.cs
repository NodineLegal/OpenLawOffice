using System.Reflection;
using ServiceStack;

namespace OpenLawOffice.Server.Core.Services
{
    public abstract class FunctionBase<TModel, TDbo, TRequest, TResponse>
        : ServiceBase<TModel, TDbo, TRequest, TResponse>, IPost<TRequest>
        where TModel : Common.Models.ModelBase
        where TDbo : DBOs.DboBase
        where TRequest : Common.Rest.Requests.RequestBase
        where TResponse : Common.Rest.Responses.ResponseBase, new()
    {
        public bool CanExecute { get { return _canFlags.HasFlag(Common.Models.CanFlags.Execute); } }
        
        public abstract object Post(TRequest request);
    }
}
