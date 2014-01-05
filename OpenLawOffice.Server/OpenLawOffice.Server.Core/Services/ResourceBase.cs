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

        public override Common.Rest.Responses.ResponseContainer<TResponse> Authorize(Common.Rest.Requests.RequestBase request, Common.Models.PermissionType permissionRequired)
        {
            Core.Security.Session session;
            Rest.Requests.Security.User userRequest;
            Core.Security.AuthorizeResult authResult;
            Rest.Requests.Security.SecuredResource securedResourceRequest;
            Common.Models.Security.ISecuredResource imodel;
            Common.Rest.Responses.ResponseContainer<TResponse> result;

            session = new Core.Security.Session()
            {
                AuthToken = request.AuthToken
            };
            userRequest = new Rest.Requests.Security.User()
            {
                AuthToken = request.AuthToken,
                Session = session
            };

            // If we have an unauthorized request detected, go ahead and return now.
            if ((result = base.Authorize(request, permissionRequired)).HttpStatusCode != System.Net.HttpStatusCode.OK)
                return result;
            
            // Do we need to test secured resource access?  If not, return now.
            if (!typeof(Common.Models.Security.ISecuredResource).IsAssignableFrom(typeof(TModel)))
                return result;

            // At this point, if the requested permission type is != List then, 
            // we know we are going to check a specific secured resource
            // this means we MUST have an Id property and it must be of type Guid
            // no exception, ever.
            // Now, if it is a list, then we would have to check every secured resource's permission
            // before showing... serious bottleneck.  So we have to find a way to prevent the server
            // from bogging down when the list grows.  The easiest way to accomplish this is to provide
            // the request to the database server and allow it to filter.  Stepping back, we can realize
            // we are going to ask the server to make the list again when the "GetList" method is called.
            // The list made filtering for secured resource permissions will be a subset of the list 
            // retrieved from the "GetList" method.  Therefore, we can combine the two queries within the
            // "GetList" method.  OOP wise, this may be ugly; however, we are doing it on purpose to
            // resolve a very serious bottleneck issue.  In future version, we may want to review the 
            // pipeline and figure out a prettier way of handling this.  This will create a problem for
            // coding as it REQUIRES testing secured resource permissions in the query of the GetList method.

            if (permissionRequired == Common.Models.PermissionType.List)
                return result;

            Guid id = Guid.Empty;

            PropertyInfo idPi = request.GetType().GetProperty("Id");
            if (idPi == null)
                return new Common.Rest.Responses.ResponseContainer<TResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
                    Error = new Common.Error()
                    {
                        Message = "A request to access a secured resource was received without a mandatory Id property."
                    }
                };

            object tempId = idPi.GetValue(request, null);
            string tempIdStr = null;

            try
            {
                tempIdStr = tempId.ToString();
            }
            catch (Exception e)
            {
                return new Common.Rest.Responses.ResponseContainer<TResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
                    Error = new Common.Error()
                    {
                        Message = "A request to access a secured resource was received but the mandatory Id property was not parseable as a string.",
                        Exception = e
                    }
                };
            }

            if (!Guid.TryParse(tempIdStr, out id))
            {
                return new Common.Rest.Responses.ResponseContainer<TResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
                    Error = new Common.Error()
                    {
                        Message = "A request to access a secured resource was received but the mandatory Id property was not parseable as a Guid."
                    }
                };
            }

            // Now, we have the "id" variable holding the secured resource id

            // Setup request
            securedResourceRequest = new Rest.Requests.Security.SecuredResource()
            {
                AuthToken = request.AuthToken,
                Id = id,
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
                            sysModel = StripNonListDetails(sysModel);

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
            Common.Models.Security.SecuredResource secResSysModel = null;
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

            if (typeof(Common.Models.Security.ISecuredResource).IsAssignableFrom(typeof(TModel)))
            {
                secResSysModel = new Common.Models.Security.SecuredResource();
                secResSysModel.UtcCreated = DateTime.Now;
                secResSysModel.UtcModified = DateTime.Now;
                secResSysModel.CreatedBy = request.Session.RequestingUser;
                secResSysModel.ModifiedBy = request.Session.RequestingUser;
            }

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
                    using (IDbTransaction tran = db.BeginTransaction())
                    {
                        try
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
                                {
                                    tempGuid = Guid.NewGuid();
                                    idPropInfo.SetValue(dbModel, tempGuid, null);
                                }
                                db.Insert<TDbo>(dbModel);

                                if (secResSysModel != null)
                                    secResSysModel.Id = tempGuid;
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

                        try
                        {
                            DBOs.Security.SecuredResource dboSecRes = Mapper.Map<DBOs.Security.SecuredResource>(secResSysModel);
                            db.Insert<DBOs.Security.SecuredResource>(dboSecRes);
                        }
                        catch (Exception e)
                        {
                            return new Common.Rest.Responses.ResponseContainer<TResponse>()
                            {
                                HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                                Error = new Common.Error()
                                {
                                    Message = "An error occurred while attempting to create the secured resource instance in the database.",
                                    Exception = e
                                }
                            };
                        }

                        try
                        {
                            // Creator gets full access rights
                            Common.Models.Security.SecuredResourceAcl secResAclSysModel = new Common.Models.Security.SecuredResourceAcl()
                            {
                                Id = Guid.NewGuid(),
                                UtcCreated = secResSysModel.UtcCreated,
                                UtcModified = secResSysModel.UtcModified,
                                CreatedBy = secResSysModel.CreatedBy,
                                ModifiedBy = secResSysModel.ModifiedBy,
                                SecuredResource = secResSysModel,
                                User = secResSysModel.CreatedBy,
                                AllowFlags = Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllRead | Common.Models.PermissionType.AllWrite,
                                DenyFlags = Common.Models.PermissionType.None
                            };
                            DBOs.Security.SecuredResourceAcl dboSecResAcl = Mapper.Map<DBOs.Security.SecuredResourceAcl>(secResAclSysModel);
                            db.Insert<DBOs.Security.SecuredResourceAcl>(dboSecResAcl);
                        }
                        catch (Exception e)
                        {
                            return new Common.Rest.Responses.ResponseContainer<TResponse>()
                            {
                                HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                                Error = new Common.Error()
                                {
                                    Message = "An error occurred while attempting to create a secured resource acl entry for the creator in the database.",
                                    Exception = e
                                }
                            };
                        }

                        tran.Commit();
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
            Common.Models.Security.SecuredResource secResSysModel = null;
            
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
                using (IDbTransaction tran = db.BeginTransaction())
                {
                    DBOs.Security.SecuredResource dboSecRes = null;
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

                    // If this is a secured resource, we need to load it for updating as well.
                    if (typeof(Common.Models.Security.ISecuredResource).IsAssignableFrom(typeof(TModel)))
                    {
                        dboSecRes = db.GetByIdParam<DBOs.Security.SecuredResource>(
                            typeof(TModel).GetProperty("Id").GetValue(sysModel1, null));
                        secResSysModel = Mapper.Map<Common.Models.Security.SecuredResource>(dboSecRes);
                        secResSysModel.UtcModified = DateTime.Now;
                        secResSysModel.ModifiedBy = request.Session.RequestingUser;
                    }

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

                    try
                    {
                        dboSecRes = Mapper.Map<DBOs.Security.SecuredResource>(secResSysModel);
                        db.Update<DBOs.Security.SecuredResource>(dboSecRes);
                    }
                    catch (Exception e)
                    {
                        return new Common.Rest.Responses.ResponseContainer<TResponse>()
                        {
                            HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                            Error = new Common.Error()
                            {
                                Message = "An error occurred while attempting to update the secured resource object in the database.",
                                Exception = e
                            }
                        };
                    }

                    tran.Commit();
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
                    using (IDbTransaction tran = db.BeginTransaction())
                    {
                        // Load
                        DBOs.Security.SecuredResource dboSecRes = null;
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

                        // If this is a secured resource, we need to load it for updating as well.
                        if (typeof(Common.Models.Security.ISecuredResource).IsAssignableFrom(typeof(TModel)))
                        {
                            dboSecRes = db.GetByIdParam<DBOs.Security.SecuredResource>(idValue);
                            dboSecRes.UtcDisabled = DateTime.Now;
                            dboSecRes.DisabledByUserId = request.Session.RequestingUser.Id;
                            db.Update<DBOs.Security.SecuredResource>(dboSecRes);
                        }

                        tran.Commit();
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

        // TODO : This could be improved by taking a list and determining the properties
        // one time, then just setting everything else to null on all other properties.
        private TModel StripNonListDetails(TModel model)
        {
            PropertyInfo[] props = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

            foreach(PropertyInfo prop in props)
            {
                object[] attributes = prop.GetCustomAttributes(typeof(Common.Models.ShowInListAttribute), true);

                // If it is not marked with the attribute
                if (attributes == null || 
                    attributes.Length <= 0)
                {
                    // If not "Id" - id is ALWAYS given
                    if (prop.Name != "Id")
                    {
                        // Then strip it
                        prop.SetValue(model, null, null);
                    }
                }
            }

            return model;
        }
    }
}
