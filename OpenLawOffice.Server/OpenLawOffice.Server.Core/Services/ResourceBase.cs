using System;
using AutoMapper;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
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
                if (idValue == null)
                    return new Common.Rest.Responses.ListResponseContainer<TResponse>()
                    {
                        HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                        Error = new Common.Error()
                        {
                            Message = "An unexpected error occurred while attempting to authorize the request.",
                            Exception = e
                        }
                    };
                else
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

            if (typeof(Common.Models.ModelWithDatesOnly).IsAssignableFrom(typeof(TModel)))
            {
                PropertyInfo utcCreated = typeof(TModel).GetProperty("UtcCreated");
                PropertyInfo utcModified = typeof(TModel).GetProperty("UtcModified");
                PropertyInfo utcDisabled = typeof(TModel).GetProperty("UtcDisabled");

                utcCreated.SetValue(sysModel, DateTime.Now, null);
                utcModified.SetValue(sysModel, DateTime.Now, null);
                utcDisabled.SetValue(sysModel, null, null);
            }

            if (typeof(Common.Models.ModelWithDatesOnly).IsAssignableFrom(typeof(TModel)))
            {
                PropertyInfo createdBy = typeof(TModel).GetProperty("CreatedBy");
                PropertyInfo modifiedBy = typeof(TModel).GetProperty("ModifiedBy");
                PropertyInfo disabledBy = typeof(TModel).GetProperty("DisabledBy");

                createdBy.SetValue(sysModel, request.Session.RequestingUser, null);
                modifiedBy.SetValue(sysModel, request.Session.RequestingUser, null);
                disabledBy.SetValue(sysModel, null, null);
            }
            
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

            /* To do an update we need to
             * 1) Load the database model from the database using the Id of the request
             * 2) Convert to system model 1
             * 3) Load system model 2 from the request
             * 4) If assignable from date base -> copy date base properties from system model 1 to system model 2
             * 5) If assignable from core -> copy core properties from system model 1 to system model 2
             *  5a) Overwrite core's modified properties (this can be simply done by ignoring on copy)
             */
            
            object idValue = null;
            TDbo dbModelCurrent;
            TDbo dbModelNew;
            TModel sysModel1;
            TModel sysModel2;
            
            idValue = GetIdValue(request);

            // Send an error if idValue is null
            if (idValue == null)
                return new Common.Rest.Responses.ResponseContainer<TResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
                    Error = new Common.Error()
                    {
                        Message = "The request must specify an Id."
                    }
                };


            using (IDbConnection db = Database.Instance.OpenConnection())
            {
                dbModelCurrent = db.GetByIdParam<TDbo>(idValue);
                sysModel1 = Mapper.Map<TModel>(dbModelCurrent);
                sysModel2 = Mapper.Map<TModel>(request);

                if (typeof(Common.Models.ModelWithDatesOnly).IsAssignableFrom(typeof(TModel)))
                {
                    PropertyInfo utcCreated = typeof(TModel).GetProperty("UtcCreated");
                    PropertyInfo utcModified = typeof(TModel).GetProperty("UtcModified");
                    PropertyInfo utcDisabled = typeof(TModel).GetProperty("UtcDisabled");
                    
                    utcCreated.SetValue(sysModel2, utcCreated.GetValue(sysModel1, null), null);
                    utcModified.SetValue(sysModel2, DateTime.Now, null);
                    utcDisabled.SetValue(sysModel2, utcDisabled.GetValue(sysModel1, null), null);
                }

                if (typeof(Common.Models.ModelWithDatesOnly).IsAssignableFrom(typeof(TModel)))
                {
                    PropertyInfo createdBy = typeof(TModel).GetProperty("CreatedBy");
                    PropertyInfo modifiedBy = typeof(TModel).GetProperty("ModifiedBy");
                    PropertyInfo disabledBy = typeof(TModel).GetProperty("DisabledBy");
                    
                    createdBy.SetValue(sysModel2, createdBy.GetValue(sysModel1, null), null);
                    modifiedBy.SetValue(sysModel2, request.Session.RequestingUser, null);
                    disabledBy.SetValue(sysModel2, disabledBy.GetValue(sysModel1, null), null);
                }

                dbModelNew = Mapper.Map<TDbo>(sysModel2);

                try
                {
                    db.Update<TDbo>(dbModelNew);
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
            }

            return new Common.Rest.Responses.ResponseContainer<TResponse>()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Data = Mapper.Map<TResponse>(sysModel2)
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
                response = Authorize(request, Common.Models.PermissionType.Disable);
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
            //dbModel = Mapper.Map<TDbo>(sysModel);
            idValue = GetIdValue(request);

            try
            {
                using (IDbConnection db = Database.Instance.OpenConnection())
                {
                    // Load
                    dbModel = db.GetByIdParam<TDbo>(idValue);

                    if (typeof(DBOs.Core).IsAssignableFrom(typeof(TDbo)))
                    {
                        DBOs.Core coreDbModel = (DBOs.Core)(DBOs.DboBase)dbModel;
                        coreDbModel.UtcDisabled = DateTime.Now;
                        coreDbModel.DisabledByUserId = request.Session.RequestingUser.Id.Value;

                        db.Update<TDbo>(
                            set: "\"utc_disabled\" = {0}, \"disabled_by_user_id\" = {1}"
                                .Params(coreDbModel.UtcDisabled, coreDbModel.DisabledByUserId),
                            where: "\"id\" = {0}".Params(idValue));
                    }
                    else if (typeof(DBOs.DboWithDatesOnly).IsAssignableFrom(typeof(TDbo)))
                    {
                        DBOs.DboWithDatesOnly datesDbModel = (DBOs.DboWithDatesOnly)(DBOs.DboBase)dbModel;
                        datesDbModel.UtcDisabled = DateTime.Now;

                        db.Update<TDbo>(
                            set: "\"utc_disabled\" = {0}"
                                .Params(datesDbModel.UtcDisabled),
                            where: "\"id\" = {0}".Params(idValue));
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
