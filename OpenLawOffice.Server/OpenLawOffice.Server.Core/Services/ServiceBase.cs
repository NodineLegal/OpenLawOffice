using System.Data;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System.Reflection;

namespace OpenLawOffice.Server.Core.Services
{
    public abstract class ServiceBase<TModel, TDbo, TRequest, TResponse> 
        : Service
        where TModel : Common.Models.ModelBase
        where TDbo : DBOs.DboBase
        where TRequest : Common.Rest.Requests.RequestBase
        where TResponse : Common.Rest.Responses.ResponseBase, new()
    {
        protected Common.Models.CanFlags _canFlags;

        public string AreaName
        {
            get
            {
                return this.GetType().FullName.Replace("OpenLawOffice.Server.Core.Services.", "");
            }
        }

        public ServiceBase()
        {
            object[] canAttributes = typeof(TModel).GetCustomAttributes(typeof(Common.Models.CanAttribute), true);

            if (canAttributes != null)
            {
                if (canAttributes.Length > 1) throw new AmbiguousMatchException("Can attribute cannot have multiple matches.");
                if (canAttributes.Length <= 0) throw new AmbiguousMatchException("Can attribute must be specified.");

                _canFlags = ((Common.Models.CanAttribute)canAttributes[0]).Can;
            }
        }

        public Common.Rest.Responses.ResponseContainer<TResponse> Authorize(Common.Rest.Requests.RequestBase request, Common.Models.PermissionType permissionRequired)
        {
            Core.Security.Session session;
            Rest.Requests.Security.User userRequest;
            Rest.Requests.Security.Area areaRequest;
            Rest.Requests.Security.SecuredResource securedResourceRequest;
            Common.Models.Security.ISecuredResource imodel;
            Core.Security.AuthorizeResult authResult;

            session = new Core.Security.Session()
            {
                AuthToken = request.AuthToken
            };
            userRequest = new Rest.Requests.Security.User()
            {
                AuthToken = request.AuthToken,
                Session = session
            };
            areaRequest = new Rest.Requests.Security.Area()
            {
                AuthToken = request.AuthToken,
                Name = AreaName,
                Session = session
            };

            authResult = Core.Security.Authorize.AreaAccess(userRequest, areaRequest, permissionRequired);

            session.RequestingUser = authResult.RequestingUser;

            if (typeof(Rest.Requests.IHasSession).IsAssignableFrom(request.GetType()))
                ((Rest.Requests.IHasSession)request).Session = session;

            if (authResult.HasError)
                return new Common.Rest.Responses.ResponseContainer<TResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Error = new Common.Error()
                    {
                        Message = "An error occurred while attempting to authorize the request.  Error message: " + authResult.ErrorMessage
                    }
                };

            if (!authResult.IsAuthorized)
                return new Common.Rest.Responses.ResponseContainer<TResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.Unauthorized,
                    Error = new Common.Error()
                    {
                        Message = "Access denied."
                    }
                };

            // Do we need to test model specific access?
            if (!typeof(Common.Models.Security.ISecuredResource).IsAssignableFrom(this.GetType()))
            {
                if (authResult.IsAuthorized)
                    return new Common.Rest.Responses.ResponseContainer<TResponse>()
                    {
                        HttpStatusCode = System.Net.HttpStatusCode.OK
                    };
                else
                    return new Common.Rest.Responses.ResponseContainer<TResponse>()
                    {
                        HttpStatusCode = System.Net.HttpStatusCode.Unauthorized,
                        Error = new Common.Error()
                        {
                            Message = "Access denied."
                        }
                    };
            }

            imodel = (Common.Models.Security.ISecuredResource)this;
            
            // Verification check
            if (imodel.SecuredResource == null || !imodel.SecuredResource.Id.HasValue)
                return new Common.Rest.Responses.ResponseContainer<TResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Error = new Common.Error()
                    {
                        Message = "Authorization expected, but was not provided a secured resource on which to check permissions."
                    }
                };

            // Setup request
            securedResourceRequest = new Rest.Requests.Security.SecuredResource()
            {
                AuthToken = request.AuthToken,
                Id = imodel.SecuredResource.Id.Value,
                Session = session
            };

            authResult = Core.Security.Authorize.SecuredResourceAccess(userRequest, securedResourceRequest, permissionRequired);

            if (authResult.HasError)
                return new Common.Rest.Responses.ResponseContainer<TResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Error = new Common.Error()
                    {
                        Message = "An error occurred while attempting to authorize the request."
                    }
                };

            if (!authResult.IsAuthorized)
                return new Common.Rest.Responses.ResponseContainer<TResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.Unauthorized,
                    Error = new Common.Error()
                    {
                        Message = "Access denied."
                    }
                };

            return new Common.Rest.Responses.ResponseContainer<TResponse>()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };
        }

        protected object GetIdValue(TRequest request)
        {
            object idValue = null;

            // Gets the Id property value, if it exists
            if (typeof(Common.Rest.Requests.IHasIntId).IsAssignableFrom(typeof(TRequest)))
            {
                if (((Common.Rest.Requests.IHasIntId)request).Id.HasValue)
                    idValue = ((Common.Rest.Requests.IHasIntId)request).Id.Value;
            }
            else if (typeof(Common.Rest.Requests.IHasLongId).IsAssignableFrom(typeof(TRequest)))
            {
                if (((Common.Rest.Requests.IHasLongId)request).Id.HasValue)
                    idValue = ((Common.Rest.Requests.IHasLongId)request).Id.Value;
            }
            else if (typeof(Common.Rest.Requests.IHasGuidId).IsAssignableFrom(typeof(TRequest)))
            {
                if (((Common.Rest.Requests.IHasGuidId)request).Id.HasValue)
                    idValue = ((Common.Rest.Requests.IHasGuidId)request).Id.Value;
            }

            return idValue;
        }
    }
}
