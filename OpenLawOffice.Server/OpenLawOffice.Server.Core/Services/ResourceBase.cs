using System;
using AutoMapper;
using System.Data;
using System.Collections.Generic;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using ServiceStack.DesignPatterns.Model;

namespace OpenLawOffice.Server.Core.Services
{
    public abstract class ResourceBase<TModel, TDbo, TRequest, TResponse>
        : ServiceBase<TModel, TDbo, TRequest, TResponse>, IGet<TRequest>, IPost<TRequest>, IPut<TRequest>, IDelete<TRequest>
        where TModel : Common.Models.ModelBase
        where TDbo : DBOs.DboBase, new()
        where TRequest : Common.Rest.Requests.RequestBase, Rest.Requests.IHasSession
        where TResponse : Common.Rest.Responses.ResponseBase, new()
    {
        public bool CanGet { get { return _canFlags.HasFlag(Common.Models.CanFlags.Get); } }
        public bool CanUpdate { get { return _canFlags.HasFlag(Common.Models.CanFlags.Update); } }
        public bool CanCreate { get { return _canFlags.HasFlag(Common.Models.CanFlags.Create); } }
        public bool CanDelete { get { return _canFlags.HasFlag(Common.Models.CanFlags.Delete); } }

        public abstract List<TDbo> GetList(TRequest request, IDbConnection db);
        
        public virtual object Get(TRequest request)
        {
            TModel sysModel;
            TDbo dbModel;
            object idValue = null;
            List<TDbo> dbModelList;
            Common.Rest.Responses.ResponseContainer<TResponse> response;

            if (!CanGet)
            {
                return new Common.Rest.Responses.ResponseContainer<TResponse>()
                {
                    Error = new Common.Error()
                    {
                        Message = "Get verb not enabled."
                    }
                };
            }

            // Null indicates list, a value indicates a single object
            idValue = GetIdValue(request);

            try
            {
                if (idValue == null)
                    response = Authorize(request, Common.Models.PermissionType.List);
                else
                    response = Authorize(request, Common.Models.PermissionType.Read);
            }
            catch (Exception e)
            {
                return new Common.Rest.Responses.ResponseContainer<TResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Error = new Common.Error()
                    {
                        Message = "An unexpected error occurred while attempting to authorize the request.",
                        Exception = e
                    }
                };
            }

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                return response;

            try
            {
                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    if (idValue == null)
                    {
                        List<TResponse> responseModelList = new List<TResponse>();
                        dbModelList = GetList(request, db);

                        foreach (TDbo item in dbModelList)
                        {
                            sysModel = Mapper.Map<TModel>(item);
                            responseModelList.Add(Mapper.Map<TResponse>(sysModel));
                        }

                        return new Common.Rest.Responses.ListResponseContainer<TResponse>()
                        {
                            HttpStatusCode = System.Net.HttpStatusCode.OK,
                            Data = responseModelList
                        };
                    }
                    else
                    {
                        dbModel = db.GetByIdParam<TDbo>(idValue);
                        sysModel = Mapper.Map<TModel>(dbModel);

                        return new Common.Rest.Responses.ResponseContainer<TResponse>()
                        {
                            HttpStatusCode = System.Net.HttpStatusCode.OK,
                            Data = Mapper.Map<TResponse>(sysModel)
                        };
                    }
                }
            }
            catch (Exception e)
            {
                return new Common.Rest.Responses.ResponseContainer<TResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Error = new Common.Error()
                    {
                        Message = "An error occurred while attempting to create the object in the database.",
                        Exception = e
                    }
                };
            }
        }

        public virtual object Post(TRequest request)
        {
            TModel sysModel;
            TDbo dbModel;
            Common.Rest.Responses.ResponseContainer<TResponse> response;

            if (!CanCreate)
            {
                return new Common.Rest.Responses.ResponseContainer<TResponse>()
                {
                    Error = new Common.Error()
                    {
                        Message = "Post verb not enabled."
                    }
                };
            }

            try
            {
                response = Authorize(request, Common.Models.PermissionType.Create);
            }
            catch (Exception e)
            {
                return new Common.Rest.Responses.ResponseContainer<TResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Error = new Common.Error()
                    {
                        Message = "An unexpected error occurred while attempting to authorize the request.",
                        Exception = e
                    }
                };
            }

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                return response;

            sysModel = Mapper.Map<TModel>(request);
            dbModel = Mapper.Map<TDbo>(sysModel);

            try
            {
                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    // Reflection below should probably be done through interfaces
                    if (typeof(IHasIntId).IsAssignableFrom(typeof(TDbo)))
                    {
                        // AutoIncrementing Ids
                        db.Insert<TDbo>(dbModel);
                        System.Reflection.PropertyInfo idPropInfo = typeof(TDbo).GetProperty("Id");
                        idPropInfo.SetValue(dbModel, (int)db.GetLastInsertId(), null);
                    }
                    else if (typeof(IHasLongId).IsAssignableFrom(typeof(TDbo)))
                    {
                        // AutoIncrementing Ids
                        db.Insert<TDbo>(dbModel);
                        System.Reflection.PropertyInfo idPropInfo = typeof(TDbo).GetProperty("Id");
                        idPropInfo.SetValue(dbModel, db.GetLastInsertId(), null);
                    }
                    else if (typeof(IHasGuidId).IsAssignableFrom(typeof(TDbo)))
                    {
                        // If no Guid -> create one
                        System.Reflection.PropertyInfo idPropInfo = typeof(TDbo).GetProperty("Id");
                        Guid tempGuid = (Guid)idPropInfo.GetValue(dbModel, null);
                        if (tempGuid == Guid.Empty)
                            idPropInfo.SetValue(dbModel, Guid.NewGuid(), null);
                        db.Insert<TDbo>(dbModel);
                    }

                }
            }
            catch (Exception e)
            {
                return new Common.Rest.Responses.ResponseContainer<TResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Error = new Common.Error()
                    {
                        Message = "An error occurred while attempting to create the object in the database.",
                        Exception = e
                    }
                };
            }

            sysModel = Mapper.Map<TModel>(dbModel);

            return new Common.Rest.Responses.ResponseContainer<TResponse>()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Data = Mapper.Map<TResponse>(sysModel)
            };
        }

        public virtual object Put(TRequest request)
        {
            TModel sysModel;
            TDbo dbModel;
            Common.Rest.Responses.ResponseContainer<TResponse> response;

            if (!CanUpdate)
            {
                return new Common.Rest.Responses.ResponseContainer<TResponse>()
                {
                    Error = new Common.Error()
                    {
                        Message = "Put verb not enabled."
                    }
                };
            }

            try
            {
                response = Authorize(request, Common.Models.PermissionType.Modify);
            }
            catch (Exception e)
            {
                return new Common.Rest.Responses.ResponseContainer<TResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Error = new Common.Error()
                    {
                        Message = "An unexpected error occurred while attempting to authorize the request.",
                        Exception = e
                    }
                };
            }

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                return response;

            sysModel = Mapper.Map<TModel>(request);
            dbModel = Mapper.Map<TDbo>(sysModel);

            try
            {
                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    db.Update<TDbo>(dbModel);
                }
            }
            catch (Exception e)
            {
                return new Common.Rest.Responses.ResponseContainer<TResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Error = new Common.Error()
                    {
                        Message = "An error occurred while attempting to update the object in the database.",
                        Exception = e
                    }
                };
            }

            sysModel = Mapper.Map<TModel>(dbModel);

            return new Common.Rest.Responses.ResponseContainer<TResponse>()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Data = Mapper.Map<TResponse>(sysModel)
            };
        }

        public virtual object Delete(TRequest request)
        {
            TModel sysModel;
            TDbo dbModel;
            object idValue = null;
            Common.Rest.Responses.ResponseContainer<TResponse> response;

            if (!CanDelete)
            {
                return new Common.Rest.Responses.ResponseContainer<TResponse>()
                {
                    Error = new Common.Error()
                    {
                        Message = "Delete verb not enabled."
                    }
                };
            }

            try
            {
                response = Authorize(request, Common.Models.PermissionType.Modify);
            }
            catch (Exception e)
            {
                return new Common.Rest.Responses.ResponseContainer<TResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Error = new Common.Error()
                    {
                        Message = "An unexpected error occurred while attempting to authorize the request.",
                        Exception = e
                    }
                };
            }

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                return response;

            sysModel = Mapper.Map<TModel>(request);
            dbModel = Mapper.Map<TDbo>(sysModel);
            idValue = GetIdValue(request);

            try
            {
                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    if (typeof(TDbo).IsAssignableFrom(typeof(DBOs.Core)))
                    {
                        DBOs.Core coreDbModel = (DBOs.Core)(DBOs.DboBase)dbModel;
                        coreDbModel.UtcDisabled = DateTime.Now;
                        coreDbModel.DisabledByUserId = request.Session.RequestingUser.Id.Value;

                        db.Update<TDbo>(
                            set: "\"UtcDisabled\" = '{0}', \"DisabledByUserId\" = {1}"
                                .Params(coreDbModel.UtcDisabled, coreDbModel.DisabledByUserId),
                            where: "\"Id\" = {0}".Params(idValue));
                    }
                    else if (typeof(TDbo).IsAssignableFrom(typeof(DBOs.DboWithDatesOnly)))
                    {
                        DBOs.DboWithDatesOnly datesDbModel = (DBOs.DboWithDatesOnly)(DBOs.DboBase)dbModel;
                        datesDbModel.UtcDisabled = DateTime.Now;

                        db.Update<TDbo>(
                            set: "\"UtcDisabled\" = '{0}'"
                                .Params(datesDbModel.UtcDisabled),
                            where: "\"Id\" = {0}".Params(idValue));
                    }
                    else
                        throw new InvalidObjectException("To disable, the DBO class must inherit from DboWithDatesOnly as it needs at least the UtcDisabled property.");
                }
            }
            catch (Exception e)
            {
                return new Common.Rest.Responses.ResponseContainer<TResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Error = new Common.Error()
                    {
                        Message = "An error occurred while attempting to update the object in the database.",
                        Exception = e
                    }
                };
            }

            sysModel = Mapper.Map<TModel>(dbModel);

            return new Common.Rest.Responses.ResponseContainer<TResponse>()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Data = Mapper.Map<TResponse>(sysModel)
            };
        }
    }
}
