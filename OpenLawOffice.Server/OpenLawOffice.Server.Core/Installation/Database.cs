using System;
using System.Data;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.PostgreSQL;

namespace OpenLawOffice.Server.Core.Installation
{
    public class Database
    {
        public void Run()
        {
            using (IDbConnection conn = Core.Database.Instance.OpenConnection())
            {
                conn.CreateTableIfNotExists<DBOs.Security.User>();
                conn.CreateTableIfNotExists<DBOs.Security.Area>();
                conn.CreateTableIfNotExists<DBOs.Security.AreaAcl>();
                conn.CreateTableIfNotExists<DBOs.Security.SecuredResource>();
                conn.CreateTableIfNotExists<DBOs.Security.SecuredResourceAcl>();
                conn.CreateTableIfNotExists<DBOs.Tagging.TagCategory>();
                conn.CreateTableIfNotExists<DBOs.Matters.Matter>();
                conn.CreateTableIfNotExists<DBOs.Matters.MatterTag>();
                conn.CreateTableIfNotExists<DBOs.Matters.ResponsibleUser>();
                conn.CreateTableIfNotExists<DBOs.Contacts.Contact>();
                conn.CreateTableIfNotExists<DBOs.Matters.MatterContact>();
                conn.CreateTableIfNotExists<DBOs.Timing.Time>();
                conn.CreateTableIfNotExists<DBOs.Tasks.Task>();
                conn.CreateTableIfNotExists<DBOs.Tasks.TaskAssignedContact>();
                conn.CreateTableIfNotExists<DBOs.Tasks.TaskMatter>();
                conn.CreateTableIfNotExists<DBOs.Tasks.TaskResponsibleUser>();
                conn.CreateTableIfNotExists<DBOs.Tasks.TaskTag>();
                conn.CreateTableIfNotExists<DBOs.Tasks.TaskTime>();

                DBOs.Security.User dbUser = conn.Single<DBOs.Security.User>(new { Username = "TestUser" });
                // == "a" on before client hash
                string pw = Services.Security.Authentication.ServerHashPassword("1F40FC92DA241694750979EE6CF582F2D5D7D28E18335DE05ABC54D0560E0F5302860C652BF08D560252AA5E74210546F369FBBBCE8C12CFC7957B2652FE9A75", "0123456789");
                
                if (dbUser == null)
                {
                    dbUser = new DBOs.Security.User()
                    {
                        Username = "TestUser",
                        Password = pw,
                        PasswordSalt = "0123456789",
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.User>(dbUser);
                    dbUser.Id = (int)conn.GetLastInsertId();
                }

                #region Areas

                DBOs.Security.Area dbSecurity = conn.QuerySingle<DBOs.Security.Area>(new { Name = "Security" });
                if (dbSecurity == null)
                {
                    dbSecurity = new DBOs.Security.Area()
                    {
                        Name = "Security",
                        Description = "Security Group",
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.Area>(dbSecurity);
                    dbSecurity.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.Area dbAreaUser = conn.QuerySingle<DBOs.Security.Area>(new { Name = "Security.User" });
                if (dbAreaUser == null)
                {
                    dbAreaUser = new DBOs.Security.Area()
                    {
                        ParentId = dbSecurity.Id,
                        Name = "Security.User",
                        Description = "System users",
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.Area>(dbAreaUser);
                    dbAreaUser.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.Area dbAreaArea = conn.QuerySingle<DBOs.Security.Area>(new { Name = "Security.Area" });
                if (dbAreaArea == null)
                {
                    dbAreaArea = new DBOs.Security.Area()
                    {
                        ParentId = dbSecurity.Id,
                        Name = "Security.Area",
                        Description = "System areas",
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.Area>(dbAreaArea);
                    dbAreaArea.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.Area dbAreaAreaAcl = conn.QuerySingle<DBOs.Security.Area>(new { Name = "Security.AreaAcl" });
                if (dbAreaAreaAcl == null)
                {
                    dbAreaAreaAcl = new DBOs.Security.Area()
                    {
                        ParentId = dbSecurity.Id,
                        Name = "Security.AreaAcl",
                        Description = "Acls for system areas",
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.Area>(dbAreaAreaAcl);
                    dbAreaAreaAcl.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.Area dbAreaSecuredResource = conn.QuerySingle<DBOs.Security.Area>(new { Name = "Security.SecuredResource" });
                if (dbAreaSecuredResource == null)
                {
                    dbAreaSecuredResource = new DBOs.Security.Area()
                    {
                        ParentId = dbSecurity.Id,
                        Name = "Security.SecuredResource",
                        Description = "System secured resources",
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.Area>(dbAreaSecuredResource);
                    dbAreaSecuredResource.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.Area dbAreaSecuredResourceAcl = conn.QuerySingle<DBOs.Security.Area>(new { Name = "Security.SecuredResourceAcl" });
                if (dbAreaSecuredResourceAcl == null)
                {
                    dbAreaSecuredResourceAcl = new DBOs.Security.Area()
                    {
                        ParentId = dbSecurity.Id,
                        Name = "Security.SecuredResourceAcl",
                        Description = "Acls for secured system resources",
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.Area>(dbAreaSecuredResourceAcl);
                    dbAreaSecuredResourceAcl.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.Area dbAreaTagging = conn.QuerySingle<DBOs.Security.Area>(new { Name = "Tagging" });
                if (dbAreaTagging == null)
                {
                    dbAreaTagging = new DBOs.Security.Area()
                    {
                        Name = "Tagging",
                        Description = "Tagging",
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.Area>(dbAreaTagging);
                    dbAreaTagging.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.Area dbAreaTaggingTagCategory = conn.QuerySingle<DBOs.Security.Area>(new { Name = "Tagging.TagCategory" });
                if (dbAreaTaggingTagCategory == null)
                {
                    dbAreaTaggingTagCategory = new DBOs.Security.Area()
                    {
                        ParentId = dbAreaTagging.Id,
                        Name = "Tagging.TagCategory",
                        Description = "Categories for tags",
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.Area>(dbAreaTaggingTagCategory);
                    dbAreaTaggingTagCategory.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.Area dbAreaMatters = conn.QuerySingle<DBOs.Security.Area>(new { Name = "Matters" });
                if (dbAreaMatters == null)
                {
                    dbAreaMatters = new DBOs.Security.Area()
                    {
                        Name = "Matters",
                        Description = "Matters",
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.Area>(dbAreaMatters);
                    dbAreaMatters.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.Area dbAreaMattersMatter = conn.QuerySingle<DBOs.Security.Area>(new { Name = "Matters.Matter" });
                if (dbAreaMattersMatter == null)
                {
                    dbAreaMattersMatter = new DBOs.Security.Area()
                    {
                        ParentId = dbAreaMatters.Id,
                        Name = "Matters.Matter",
                        Description = "System matters",
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.Area>(dbAreaMattersMatter);
                    dbAreaMattersMatter.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.Area dbAreaMattersTag = conn.QuerySingle<DBOs.Security.Area>(new { Name = "Matters.MatterTag" });
                if (dbAreaMattersTag == null)
                {
                    dbAreaMattersTag = new DBOs.Security.Area()
                    {
                        ParentId = dbAreaMatters.Id,
                        Name = "Matters.MatterTag",
                        Description = "Tags for matters",
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.Area>(dbAreaMattersTag);
                    dbAreaMattersTag.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.Area dbAreaMattersResponsibleUser = conn.QuerySingle<DBOs.Security.Area>(new { Name = "Matters.ResponsibleUser" });
                if (dbAreaMattersResponsibleUser == null)
                {
                    dbAreaMattersResponsibleUser = new DBOs.Security.Area()
                    {
                        ParentId = dbAreaMatters.Id,
                        Name = "Matters.ResponsibleUser",
                        Description = "User responsibilities for matters",
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.Area>(dbAreaMattersResponsibleUser);
                    dbAreaMattersResponsibleUser.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.Area dbAreaMattersMatterContact = conn.QuerySingle<DBOs.Security.Area>(new { Name = "Matters.MatterContact" });
                if (dbAreaMattersMatterContact == null)
                {
                    dbAreaMattersMatterContact = new DBOs.Security.Area()
                    {
                        ParentId = dbAreaMatters.Id,
                        Name = "Matters.MatterContact",
                        Description = "Contacts for a specific matter",
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.Area>(dbAreaMattersMatterContact);
                    dbAreaMattersMatterContact.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.Area dbAreaContacts = conn.QuerySingle<DBOs.Security.Area>(new { Name = "Contacts" });
                if (dbAreaContacts == null)
                {
                    dbAreaContacts = new DBOs.Security.Area()
                    {
                        Name = "Contacts",
                        Description = "Contacts",
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.Area>(dbAreaContacts);
                    dbAreaContacts.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.Area dbAreaContactsContact = conn.QuerySingle<DBOs.Security.Area>(new { Name = "Contacts.Contact" });
                if (dbAreaContactsContact == null)
                {
                    dbAreaContactsContact = new DBOs.Security.Area()
                    {
                        ParentId = dbAreaContacts.Id,
                        Name = "Contacts.Contact",
                        Description = "System contacts",
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.Area>(dbAreaContactsContact);
                    dbAreaContactsContact.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.Area dbAreaTasks = conn.QuerySingle<DBOs.Security.Area>(new { Name = "Tasks" });
                if (dbAreaTasks == null)
                {
                    dbAreaTasks = new DBOs.Security.Area()
                    {
                        Name = "Tasks",
                        Description = "Tasks",
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.Area>(dbAreaTasks);
                    dbAreaTasks.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.Area dbAreaTasksTask = conn.QuerySingle<DBOs.Security.Area>(new { Name = "Tasks.Task" });
                if (dbAreaTasksTask == null)
                {
                    dbAreaTasksTask = new DBOs.Security.Area()
                    {
                        ParentId = dbAreaTasks.Id,
                        Name = "Tasks.Task",
                        Description = "System tasks",
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.Area>(dbAreaTasksTask);
                    dbAreaTasksTask.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.Area dbAreaTasksTaskAssignedContact = conn.QuerySingle<DBOs.Security.Area>(new { Name = "Tasks.TaskAssignedContact" });
                if (dbAreaTasksTaskAssignedContact == null)
                {
                    dbAreaTasksTaskAssignedContact = new DBOs.Security.Area()
                    {
                        ParentId = dbAreaTasks.Id,
                        Name = "Tasks.TaskAssignedContact",
                        Description = "Contacts assigned to tasks",
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.Area>(dbAreaTasksTaskAssignedContact);
                    dbAreaTasksTaskAssignedContact.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.Area dbAreaTasksTaskMatter = conn.QuerySingle<DBOs.Security.Area>(new { Name = "Tasks.TaskMatter" });
                if (dbAreaTasksTaskMatter == null)
                {
                    dbAreaTasksTaskMatter = new DBOs.Security.Area()
                    {
                        ParentId = dbAreaTasks.Id,
                        Name = "Tasks.TaskMatter",
                        Description = "Tasks assigned to matters",
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.Area>(dbAreaTasksTaskMatter);
                    dbAreaTasksTaskMatter.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.Area dbAreaTasksTaskResponsibleUser = conn.QuerySingle<DBOs.Security.Area>(new { Name = "Tasks.TaskResponsibleUser" });
                if (dbAreaTasksTaskResponsibleUser == null)
                {
                    dbAreaTasksTaskResponsibleUser = new DBOs.Security.Area()
                    {
                        ParentId = dbAreaTasks.Id,
                        Name = "Tasks.TaskResponsibleUser",
                        Description = "User responsibilities for tasks",
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.Area>(dbAreaTasksTaskResponsibleUser);
                    dbAreaTasksTaskResponsibleUser.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.Area dbAreaTasksTaskTag = conn.QuerySingle<DBOs.Security.Area>(new { Name = "Tasks.TaskTag" });
                if (dbAreaTasksTaskTag == null)
                {
                    dbAreaTasksTaskTag = new DBOs.Security.Area()
                    {
                        ParentId = dbAreaTasks.Id,
                        Name = "Tasks.TaskTag",
                        Description = "Task tags",
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.Area>(dbAreaTasksTaskTag);
                    dbAreaTasksTaskTag.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.Area dbAreaTasksTaskTime = conn.QuerySingle<DBOs.Security.Area>(new { Name = "Tasks.TaskTime" });
                if (dbAreaTasksTaskTime == null)
                {
                    dbAreaTasksTaskTime = new DBOs.Security.Area()
                    {
                        ParentId = dbAreaTasks.Id,
                        Name = "Tasks.TaskTime",
                        Description = "Time entries for tasks",
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.Area>(dbAreaTasksTaskTime);
                    dbAreaTasksTaskTime.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.Area dbAreaTiming = conn.QuerySingle<DBOs.Security.Area>(new { Name = "Timing" });
                if (dbAreaTiming == null)
                {
                    dbAreaTiming = new DBOs.Security.Area()
                    {
                        Name = "Timing",
                        Description = "Timing",
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.Area>(dbAreaTiming);
                    dbAreaTiming.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.Area dbAreaTimingTime = conn.QuerySingle<DBOs.Security.Area>(new { Name = "Timing.Time" });
                if (dbAreaTimingTime == null)
                {
                    dbAreaTimingTime = new DBOs.Security.Area()
                    {
                        ParentId = dbAreaTiming.Id,
                        Name = "Timing.Time",
                        Description = "System time entries",
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.Area>(dbAreaTimingTime);
                    dbAreaTimingTime.Id = (int)conn.GetLastInsertId();
                }

                #endregion

                #region Area Acls

                DBOs.Security.AreaAcl dbAAclSecurity = conn.QuerySingle<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbSecurity.Id, UserId = dbUser.Id });
                if (dbAAclSecurity == null)
                {
                    dbAAclSecurity = new DBOs.Security.AreaAcl()
                    {
                        SecurityAreaId = dbSecurity.Id,
                        UserId = dbUser.Id,
                        AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllWrite | Common.Models.PermissionType.AllRead),
                        DenyFlags = 0,
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.AreaAcl>(dbAAclSecurity);
                    dbAAclSecurity.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.AreaAcl dbAAclUser = conn.QuerySingle<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbAreaUser.Id, UserId = dbUser.Id });
                if (dbAAclUser == null)
                {
                    dbAAclUser = new DBOs.Security.AreaAcl()
                    {
                        SecurityAreaId = dbAreaUser.Id,
                        UserId = dbUser.Id,
                        AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllWrite | Common.Models.PermissionType.AllRead),
                        DenyFlags = 0,
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.AreaAcl>(dbAAclUser);
                    dbAAclUser.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.AreaAcl dbAAclArea = conn.QuerySingle<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbAreaArea.Id, UserId = dbUser.Id });
                if (dbAAclArea == null)
                {
                    dbAAclArea = new DBOs.Security.AreaAcl()
                    {
                        SecurityAreaId = dbAreaArea.Id,
                        UserId = dbUser.Id,
                        AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllWrite | Common.Models.PermissionType.AllRead),
                        DenyFlags = 0,
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.AreaAcl>(dbAAclArea);
                    dbAAclArea.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.AreaAcl dbAAclAreaAcl = conn.QuerySingle<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbAreaAreaAcl.Id, UserId = dbUser.Id });
                if (dbAAclAreaAcl == null)
                {
                    dbAAclAreaAcl = new DBOs.Security.AreaAcl()
                    {
                        SecurityAreaId = dbAreaAreaAcl.Id,
                        UserId = dbUser.Id,
                        AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllWrite | Common.Models.PermissionType.AllRead),
                        DenyFlags = 0,
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.AreaAcl>(dbAAclAreaAcl);
                    dbAAclAreaAcl.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.AreaAcl dbAAclSecuredResource = conn.QuerySingle<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbAreaSecuredResource.Id, UserId = dbUser.Id });
                if (dbAAclSecuredResource == null)
                {
                    dbAAclSecuredResource = new DBOs.Security.AreaAcl()
                    {
                        SecurityAreaId = dbAreaSecuredResource.Id,
                        UserId = dbUser.Id,
                        AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllWrite | Common.Models.PermissionType.AllRead),
                        DenyFlags = 0,
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.AreaAcl>(dbAAclSecuredResource);
                    dbAAclSecuredResource.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Security.AreaAcl dbAAclSecuredResourceAcl = conn.QuerySingle<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbAreaSecuredResourceAcl.Id, UserId = dbUser.Id });
                if (dbAAclSecuredResourceAcl == null)
                {
                    dbAAclSecuredResourceAcl = new DBOs.Security.AreaAcl()
                    {
                        SecurityAreaId = dbAreaSecuredResourceAcl.Id,
                        UserId = dbUser.Id,
                        AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllWrite | Common.Models.PermissionType.AllRead),
                        DenyFlags = 0,
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.AreaAcl>(dbAAclSecuredResourceAcl);
                    dbAAclSecuredResourceAcl.Id = (int)conn.GetLastInsertId();
                }

                // Matters
                DBOs.Security.AreaAcl dbAAclMatters = conn.QuerySingle<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbAreaMatters.Id, UserId = dbUser.Id });
                if (dbAAclMatters == null)
                {
                    dbAAclMatters = new DBOs.Security.AreaAcl()
                    {
                        SecurityAreaId = dbAreaMatters.Id,
                        UserId = dbUser.Id,
                        AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllWrite | Common.Models.PermissionType.AllRead),
                        DenyFlags = 0,
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.AreaAcl>(dbAAclMatters);
                    dbAAclMatters.Id = (int)conn.GetLastInsertId();
                }

                // Tagging.TagCategory
                DBOs.Security.AreaAcl dbAAclTaggingTagCategory = conn.QuerySingle<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbAreaTaggingTagCategory.Id, UserId = dbUser.Id });
                if (dbAAclTaggingTagCategory == null)
                {
                    dbAAclTaggingTagCategory = new DBOs.Security.AreaAcl()
                    {
                        SecurityAreaId = dbAreaTaggingTagCategory.Id,
                        UserId = dbUser.Id,
                        AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllWrite | Common.Models.PermissionType.AllRead),
                        DenyFlags = 0,
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.AreaAcl>(dbAAclTaggingTagCategory);
                    dbAAclTaggingTagCategory.Id = (int)conn.GetLastInsertId();
                }

                // Matters.Matter
                DBOs.Security.AreaAcl dbAAclMattersMatter = conn.QuerySingle<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbAreaMattersMatter.Id, UserId = dbUser.Id });
                if (dbAAclMattersMatter == null)
                {
                    dbAAclMattersMatter = new DBOs.Security.AreaAcl()
                    {
                        SecurityAreaId = dbAreaMattersMatter.Id,
                        UserId = dbUser.Id,
                        AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllWrite | Common.Models.PermissionType.AllRead),
                        DenyFlags = 0,
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.AreaAcl>(dbAAclMattersMatter);
                    dbAAclMattersMatter.Id = (int)conn.GetLastInsertId();
                }

                // Matters.MatterTag
                DBOs.Security.AreaAcl dbAAclMattersMatterTag = conn.QuerySingle<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbAreaMattersTag.Id, UserId = dbUser.Id });
                if (dbAAclMattersMatterTag == null)
                {
                    dbAAclMattersMatterTag = new DBOs.Security.AreaAcl()
                    {
                        SecurityAreaId = dbAreaMattersTag.Id,
                        UserId = dbUser.Id,
                        AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllWrite | Common.Models.PermissionType.AllRead),
                        DenyFlags = 0,
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.AreaAcl>(dbAAclMattersMatterTag);
                    dbAAclMattersMatterTag.Id = (int)conn.GetLastInsertId();
                }

                // Matters.ResponsibleUser
                DBOs.Security.AreaAcl dbAAclMattersResponsibleUser = conn.QuerySingle<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbAreaMattersResponsibleUser.Id, UserId = dbUser.Id });
                if (dbAAclMattersResponsibleUser == null)
                {
                    dbAAclMattersResponsibleUser = new DBOs.Security.AreaAcl()
                    {
                        SecurityAreaId = dbAreaMattersResponsibleUser.Id,
                        UserId = dbUser.Id,
                        AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllWrite | Common.Models.PermissionType.AllRead),
                        DenyFlags = 0,
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.AreaAcl>(dbAAclMattersResponsibleUser);
                    dbAAclMattersResponsibleUser.Id = (int)conn.GetLastInsertId();
                }

                // Matters.MatterContact
                DBOs.Security.AreaAcl dbAAclMattersMatterContact = conn.QuerySingle<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbAreaMattersMatterContact.Id, UserId = dbUser.Id });
                if (dbAAclMattersMatterContact == null)
                {
                    dbAAclMattersMatterContact = new DBOs.Security.AreaAcl()
                    {
                        SecurityAreaId = dbAreaMattersMatterContact.Id,
                        UserId = dbUser.Id,
                        AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllWrite | Common.Models.PermissionType.AllRead),
                        DenyFlags = 0,
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.AreaAcl>(dbAAclMattersMatterContact);
                    dbAAclMattersMatterContact.Id = (int)conn.GetLastInsertId();
                }

                // Contacts
                DBOs.Security.AreaAcl dbAAclContacts = conn.QuerySingle<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbAreaContacts.Id, UserId = dbUser.Id });
                if (dbAAclContacts == null)
                {
                    dbAAclContacts = new DBOs.Security.AreaAcl()
                    {
                        SecurityAreaId = dbAreaContacts.Id,
                        UserId = dbUser.Id,
                        AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllWrite | Common.Models.PermissionType.AllRead),
                        DenyFlags = 0,
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.AreaAcl>(dbAAclContacts);
                    dbAAclContacts.Id = (int)conn.GetLastInsertId();
                }

                // Contacts.Contact
                DBOs.Security.AreaAcl dbAAclContactsContact = conn.QuerySingle<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbAreaContactsContact.Id, UserId = dbUser.Id });
                if (dbAAclContactsContact == null)
                {
                    dbAAclContactsContact = new DBOs.Security.AreaAcl()
                    {
                        SecurityAreaId = dbAreaContactsContact.Id,
                        UserId = dbUser.Id,
                        AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllWrite | Common.Models.PermissionType.AllRead),
                        DenyFlags = 0,
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.AreaAcl>(dbAAclContactsContact);
                    dbAAclContactsContact.Id = (int)conn.GetLastInsertId();
                }

                // Tasks
                DBOs.Security.AreaAcl dbAAclTasks = conn.QuerySingle<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbAreaTasks.Id, UserId = dbUser.Id });
                if (dbAAclTasks == null)
                {
                    dbAAclTasks = new DBOs.Security.AreaAcl()
                    {
                        SecurityAreaId = dbAreaTasks.Id,
                        UserId = dbUser.Id,
                        AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllWrite | Common.Models.PermissionType.AllRead),
                        DenyFlags = 0,
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.AreaAcl>(dbAAclTasks);
                    dbAAclTasks.Id = (int)conn.GetLastInsertId();
                }

                // Tasks.Task
                DBOs.Security.AreaAcl dbAAclTasksTask = conn.QuerySingle<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbAreaTasksTask.Id, UserId = dbUser.Id });
                if (dbAAclTasksTask == null)
                {
                    dbAAclTasksTask = new DBOs.Security.AreaAcl()
                    {
                        SecurityAreaId = dbAreaTasksTask.Id,
                        UserId = dbUser.Id,
                        AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllWrite | Common.Models.PermissionType.AllRead),
                        DenyFlags = 0,
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.AreaAcl>(dbAAclTasksTask);
                    dbAAclTasksTask.Id = (int)conn.GetLastInsertId();
                }

                // Tasks.TaskAssignedContact
                DBOs.Security.AreaAcl dbAAclTasksTaskAssignedContact = conn.QuerySingle<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbAreaTasksTask.Id, UserId = dbUser.Id });
                if (dbAAclTasksTask == null)
                {
                    dbAAclTasksTask = new DBOs.Security.AreaAcl()
                    {
                        SecurityAreaId = dbAreaTasksTask.Id,
                        UserId = dbUser.Id,
                        AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllWrite | Common.Models.PermissionType.AllRead),
                        DenyFlags = 0,
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.AreaAcl>(dbAAclTasksTask);
                    dbAAclTasksTask.Id = (int)conn.GetLastInsertId();
                }

                // Tasks.TaskMatter
                DBOs.Security.AreaAcl dbAAclTasksTaskMatter = conn.QuerySingle<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbAreaTasksTaskMatter.Id, UserId = dbUser.Id });
                if (dbAAclTasksTaskMatter == null)
                {
                    dbAAclTasksTaskMatter = new DBOs.Security.AreaAcl()
                    {
                        SecurityAreaId = dbAreaTasksTaskMatter.Id,
                        UserId = dbUser.Id,
                        AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllWrite | Common.Models.PermissionType.AllRead),
                        DenyFlags = 0,
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.AreaAcl>(dbAAclTasksTaskMatter);
                    dbAAclTasksTaskMatter.Id = (int)conn.GetLastInsertId();
                }

                // Tasks.TaskResponsibleUser
                DBOs.Security.AreaAcl dbAAclTasksTaskResponsibleUser = conn.QuerySingle<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbAreaTasksTaskResponsibleUser.Id, UserId = dbUser.Id });
                if (dbAAclTasksTask == null)
                {
                    dbAAclTasksTaskResponsibleUser = new DBOs.Security.AreaAcl()
                    {
                        SecurityAreaId = dbAreaTasksTaskResponsibleUser.Id,
                        UserId = dbUser.Id,
                        AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllWrite | Common.Models.PermissionType.AllRead),
                        DenyFlags = 0,
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.AreaAcl>(dbAAclTasksTaskResponsibleUser);
                    dbAAclTasksTaskResponsibleUser.Id = (int)conn.GetLastInsertId();
                }

                // Tasks.TaskTag
                DBOs.Security.AreaAcl dbAAclTasksTaskTag = conn.QuerySingle<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbAreaTasksTaskTag.Id, UserId = dbUser.Id });
                if (dbAAclTasksTaskTag == null)
                {
                    dbAAclTasksTaskTag = new DBOs.Security.AreaAcl()
                    {
                        SecurityAreaId = dbAreaTasksTaskTag.Id,
                        UserId = dbUser.Id,
                        AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllWrite | Common.Models.PermissionType.AllRead),
                        DenyFlags = 0,
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.AreaAcl>(dbAAclTasksTaskTag);
                    dbAAclTasksTaskTag.Id = (int)conn.GetLastInsertId();
                }

                // Tasks.TaskTime
                DBOs.Security.AreaAcl dbAAclTasksTaskTime = conn.QuerySingle<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbAreaTasksTaskTime.Id, UserId = dbUser.Id });
                if (dbAAclTasksTaskTime == null)
                {
                    dbAAclTasksTaskTime = new DBOs.Security.AreaAcl()
                    {
                        SecurityAreaId = dbAreaTasksTaskTime.Id,
                        UserId = dbUser.Id,
                        AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllWrite | Common.Models.PermissionType.AllRead),
                        DenyFlags = 0,
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.AreaAcl>(dbAAclTasksTaskTime);
                    dbAAclTasksTaskTime.Id = (int)conn.GetLastInsertId();
                }

                // Timing
                DBOs.Security.AreaAcl dbAAclTiming = conn.QuerySingle<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbAreaTiming.Id, UserId = dbUser.Id });
                if (dbAAclTiming == null)
                {
                    dbAAclTiming = new DBOs.Security.AreaAcl()
                    {
                        SecurityAreaId = dbAreaTiming.Id,
                        UserId = dbUser.Id,
                        AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllWrite | Common.Models.PermissionType.AllRead),
                        DenyFlags = 0,
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.AreaAcl>(dbAAclTiming);
                    dbAAclTiming.Id = (int)conn.GetLastInsertId();
                }

                // Timing.Time
                DBOs.Security.AreaAcl dbAAclTimingTime = conn.QuerySingle<DBOs.Security.AreaAcl>(new { SecurityAreaId = dbAreaTimingTime.Id, UserId = dbUser.Id });
                if (dbAAclTimingTime == null)
                {
                    dbAAclTimingTime = new DBOs.Security.AreaAcl()
                    {
                        SecurityAreaId = dbAreaTimingTime.Id,
                        UserId = dbUser.Id,
                        AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllWrite | Common.Models.PermissionType.AllRead),
                        DenyFlags = 0,
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.AreaAcl>(dbAAclTimingTime);
                    dbAAclTimingTime.Id = (int)conn.GetLastInsertId();
                }

                #endregion
                
                #region Contacts

                DBOs.Contacts.Contact dbContact = conn.QuerySingle<DBOs.Contacts.Contact>(new { DisplayName = "Lucas Nodine" });
                if (dbContact == null)
                {
                    dbContact = new DBOs.Contacts.Contact()
                    {
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now,
                        IsOrganization = false,
                        Nickname = "Luke",
                        DisplayNamePrefix = "Mr.",
                        Surname = "Nodine",
                        MiddleName = "James",
                        GivenName = "Lucas",
                        Initials = "LJN",
                        DisplayName = "Lucas Nodine",
                        Email1DisplayName = "Fake Prevent Spambots",
                        Email1EmailAddress = "no@one.com",
                        Fax1DisplayName = "Fake Fax",
                        Fax1FaxNumber = "1-555-555-5555",
                        Address1DisplayName = "Nodine Legal PO Box",
                        Address1AddressCity = "Parsons",
                        Address1AddressStateOrProvince = "Kansas",
                        Address1AddressPostalCode = "67357",
                        Address1AddressCountry = "USA",
                        Address1AddressCountryCode = "1",
                        Address1AddressPostOfficeBox = "1125",
                        Telephone1DisplayName = "Virtual Office",
                        Telephone1TelephoneNumber = "620-577-4271",
                        Birthday = new DateTime(1982, 3, 25),
                        Wedding = new DateTime(2012, 5, 12),
                        Title = "Managing Member",
                        CompanyName = "Nodine Legal, LLC",
                        Profession = "Attorney",
                        Language = "English (American)",
                        BusinessHomePage = "www.nodinelegal.com",
                        Gender = "Male",
                        ReferredByName = "Bob"
                    };
                    conn.Insert<DBOs.Contacts.Contact>(dbContact);
                    dbContact.Id = (int)conn.GetLastInsertId();
                }

                #endregion

                #region Matters
                
                DBOs.Tagging.TagCategory dbTagCategory1 = conn.QuerySingle<DBOs.Tagging.TagCategory>(new { Name = "Status" });
                if (dbTagCategory1 == null)
                {
                    dbTagCategory1 = new DBOs.Tagging.TagCategory()
                    {
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now,
                        Name = "Status"
                    };
                    conn.Insert<DBOs.Tagging.TagCategory>(dbTagCategory1);
                    dbTagCategory1.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Tagging.TagCategory tagCategory2 = conn.QuerySingle<DBOs.Tagging.TagCategory>(new { Name = "Jurisdiction" });
                if (tagCategory2 == null)
                {
                    tagCategory2 = new DBOs.Tagging.TagCategory()
                    {
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now, 
                        Name = "Jurisdiction"
                    };
                    conn.Insert<DBOs.Tagging.TagCategory>(tagCategory2);
                    tagCategory2.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Matters.Matter matter1 = conn.QuerySingle<DBOs.Matters.Matter>(new { Title = "Test Matter 1" });
                if (matter1 == null)
                {
                    matter1 = new DBOs.Matters.Matter()
                    {
                        Id = Guid.NewGuid(),
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now,
                        Title = "Test Matter 1",
                        Synopsis = "This is the synopsis for test matter 1"
                    };
                    conn.Insert<DBOs.Matters.Matter>(matter1);
                }

                DBOs.Security.SecuredResource secRes1 = conn.QuerySingle<DBOs.Security.SecuredResource>(new { Id = matter1.Id });
                if (secRes1 == null)
                {
                    secRes1 = new DBOs.Security.SecuredResource()
                    {
                        Id = matter1.Id,
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now
                    };
                    conn.Insert<DBOs.Security.SecuredResource>(secRes1);
                }

                DBOs.Security.SecuredResourceAcl secResAcl1 = conn.QuerySingle<DBOs.Security.SecuredResourceAcl>(new { SecuredResourceId = secRes1.Id, UserId = dbUser.Id });
                if (secResAcl1 == null)
                {
                    secResAcl1 = new DBOs.Security.SecuredResourceAcl()
                    {
                        Id = Guid.NewGuid(),
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.UtcNow,
                        UtcModified = DateTime.UtcNow,
                        UserId = dbUser.Id,
                        SecuredResourceId = secRes1.Id,
                        AllowFlags = (int)(Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllRead | Common.Models.PermissionType.AllWrite),
                        DenyFlags = (int)Common.Models.PermissionType.None
                    };
                    conn.Insert<DBOs.Security.SecuredResourceAcl>(secResAcl1);
                }                

                DBOs.Matters.MatterTag matterTag1 = conn.QuerySingle<DBOs.Matters.MatterTag>(new { Tag = "Active" });
                if (matterTag1 == null)
                {
                    matterTag1 = new DBOs.Matters.MatterTag()
                    {
                        Id = Guid.NewGuid(),
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now,
                        MatterId = matter1.Id,
                        TagCategoryId = 1,
                        Tag = "Active"
                    };
                    conn.Insert<DBOs.Matters.MatterTag>(matterTag1);
                }

                DBOs.Matters.MatterTag matterTag2 = conn.QuerySingle<DBOs.Matters.MatterTag>(new { Tag = "Labette County, KS" });
                if (matterTag2 == null)
                {
                    matterTag2 = new DBOs.Matters.MatterTag()
                    {
                        Id = Guid.NewGuid(),
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now,
                        MatterId = matter1.Id,
                        TagCategoryId = 2,
                        Tag = "Labette County, KS"
                    };
                    conn.Insert<DBOs.Matters.MatterTag>(matterTag2);
                }

                DBOs.Matters.ResponsibleUser respUser1 = conn.QuerySingle<DBOs.Matters.ResponsibleUser>(new { MatterId = matter1.Id, UserId = dbUser.Id });
                if (respUser1 == null)
                {
                    respUser1 = new DBOs.Matters.ResponsibleUser()
                    {
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now,
                        MatterId = matter1.Id,
                        UserId = dbUser.Id,
                        Responsibility = "Attorney"
                    };
                    conn.Insert<DBOs.Matters.ResponsibleUser>(respUser1);
                    respUser1.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Matters.MatterContact matterContact1 = conn.QuerySingle<DBOs.Matters.MatterContact>(new { MatterId = matter1.Id, ContactId = dbContact.Id });
                if (matterContact1 == null)
                {
                    matterContact1 = new DBOs.Matters.MatterContact()
                    {
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now,
                        MatterId = matter1.Id,
                        ContactId = dbContact.Id,
                        Role = "Head Attorney"
                    };
                    conn.Insert<DBOs.Matters.MatterContact>(matterContact1);
                    matterContact1.Id = (int)conn.GetLastInsertId();
                }

                #endregion

                #region Timing

                DBOs.Timing.Time time1 = conn.QuerySingle<DBOs.Timing.Time>(new { Id = Guid.Parse("f1220521-20d6-4544-9438-d664d8719935") });
                if (time1 == null)
                {
                    time1 = new DBOs.Timing.Time()
                    {
                        Id = Guid.NewGuid(),
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now,
                        Start = DateTime.UtcNow.AddMinutes(-12),
                        Stop = DateTime.UtcNow,
                        WorkerContactId = dbContact.Id
                    };
                    conn.Insert<DBOs.Timing.Time>(time1);
                }

                #endregion

                #region Tasks

                DBOs.Tasks.Task task1 = conn.QuerySingle<DBOs.Tasks.Task>(new { Title = "Test Task 1" });
                if (task1 == null)
                {
                    task1 = new DBOs.Tasks.Task()
                    {
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now,
                        Title = "Test Task 1",
                        Description = "Test Task 1 description",
                        ProjectedStart = DateTime.UtcNow,
                        DueDate = DateTime.UtcNow.AddDays(4),
                        ProjectedEnd = DateTime.UtcNow.AddDays(3),
                        ActualEnd = null,
                        ParentId = null,
                        IsGroupingTask = false,
                        SequentialPredecessorId = null
                    };
                    conn.Insert<DBOs.Tasks.Task>(task1);
                    task1.Id = conn.GetLastInsertId();
                }

                DBOs.Tasks.TaskAssignedContact taskAssignedContact1 = conn.QuerySingle<DBOs.Tasks.TaskAssignedContact>(new { TaskId = task1.Id, ContactId = dbContact.Id });
                if (taskAssignedContact1 == null)
                {
                    taskAssignedContact1 = new DBOs.Tasks.TaskAssignedContact()
                    {
                        Id = Guid.NewGuid(),
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now,
                        TaskId = task1.Id,
                        ContactId = dbContact.Id,
                        AssignmentType = (int)Common.Models.Tasks.AssignmentType.Direct
                    };
                    conn.Insert<DBOs.Tasks.TaskAssignedContact>(taskAssignedContact1);
                }

                DBOs.Tasks.TaskMatter taskMatter1 = conn.QuerySingle<DBOs.Tasks.TaskMatter>(new { TaskId = task1.Id, MatterId = matter1.Id });
                if (taskMatter1 == null)
                {
                    taskMatter1 = new DBOs.Tasks.TaskMatter()
                    {
                        Id = Guid.NewGuid(),
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now,
                        TaskId = task1.Id,
                        MatterId = matter1.Id
                    };
                    conn.Insert<DBOs.Tasks.TaskMatter>(taskMatter1);
                }

                DBOs.Tasks.TaskResponsibleUser taskRespUser1 = conn.QuerySingle<DBOs.Tasks.TaskResponsibleUser>(new { TaskId = task1.Id, UserId = dbUser.Id });
                if (taskRespUser1 == null)
                {
                    taskRespUser1 = new DBOs.Tasks.TaskResponsibleUser()
                    {
                        Id = Guid.NewGuid(),
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now,
                        TaskId = task1.Id,
                        UserId = dbUser.Id,
                        Responsibility = "Attorney"
                    };
                    conn.Insert<DBOs.Tasks.TaskResponsibleUser>(taskRespUser1);
                }

                DBOs.Tagging.TagCategory dbTagCategory3 = conn.QuerySingle<DBOs.Tagging.TagCategory>(new { Name = "Priority" });
                if (dbTagCategory3 == null)
                {
                    dbTagCategory3 = new DBOs.Tagging.TagCategory()
                    {
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now,
                        Name = "Priority"
                    };
                    conn.Insert<DBOs.Tagging.TagCategory>(dbTagCategory3);
                    dbTagCategory3.Id = (int)conn.GetLastInsertId();
                }

                DBOs.Tasks.TaskTag taskTag1 = conn.QuerySingle<DBOs.Tasks.TaskTag>(new { Tag = "Pending" });
                if (taskTag1 == null)
                {
                    taskTag1 = new DBOs.Tasks.TaskTag()
                    {
                        Id = Guid.NewGuid(),
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now,
                        TaskId = task1.Id,
                        TagCategoryId = 1,
                        Tag = "Pending"
                    };
                    conn.Insert<DBOs.Tasks.TaskTag>(taskTag1);
                }

                DBOs.Tasks.TaskTag taskTag2 = conn.QuerySingle<DBOs.Tasks.TaskTag>(new { Tag = "High" });
                if (taskTag2 == null)
                {
                    taskTag2 = new DBOs.Tasks.TaskTag()
                    {
                        Id = Guid.NewGuid(),
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now,
                        TaskId = task1.Id,
                        TagCategoryId = 3,
                        Tag = "High"
                    };
                    conn.Insert<DBOs.Tasks.TaskTag>(taskTag2);
                }

                DBOs.Tasks.TaskTime taskTime1 = conn.QuerySingle<DBOs.Tasks.TaskTime>(new { TaskId = task1.Id, TimeId = time1.Id });
                if (taskTime1 == null)
                {
                    taskTime1 = new DBOs.Tasks.TaskTime()
                    {
                        Id = Guid.NewGuid(),
                        CreatedByUserId = dbUser.Id,
                        ModifiedByUserId = dbUser.Id,
                        UtcCreated = DateTime.Now,
                        UtcModified = DateTime.Now,
                        TaskId = task1.Id,
                        TimeId = time1.Id
                    };
                    conn.Insert<DBOs.Tasks.TaskTime>(taskTime1);
                }

                #endregion

            }
        }
    }
}
