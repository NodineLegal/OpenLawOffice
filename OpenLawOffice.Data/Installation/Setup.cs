// -----------------------------------------------------------------------
// <copyright file="Setup.cs" company="Nodine Legal, LLC">
// Licensed to Nodine Legal, LLC under one
// or more contributor license agreements.  See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership.  Nodine Legal, LLC licenses this file
// to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
// with the License.  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.
// </copyright>
// -----------------------------------------------------------------------

namespace OpenLawOffice.Data.Installation
{
    using System.Configuration;
    using System.IO;
    using Npgsql;
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Setup
    {
        public static void CreateDb(string filepath, bool setupData = false)
        {
            ExecuteScript(filepath);
            RequiredData(setupData);
        }

        private static void ExecuteScript(string filepath)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Postgres"].ToString());
            FileInfo file = new FileInfo(filepath);
            string script = file.OpenText().ReadToEnd();
            NpgsqlCommand cmd = new NpgsqlCommand(script, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
        }

        private static void RequiredData(bool runOptionalData = false)
        {
            Common.Models.Security.User user = Security.User.Get("Administrator");
            if (user == null)
            {
                string pw = ClientHashPassword("password");
                string salt = GetRandomString(10);
                pw = ServerHashPassword(pw, salt);
                user = new Common.Models.Security.User()
                {
                    Username = "Administrator",
                    Password = pw,
                    PasswordSalt = salt
                };
                user = Security.User.Create(user);
            }

            // Areas

            Common.Models.Security.Area areaSecurity, areaUser, areaArea, areaAreaAcl,
                areaSecuredResource, areaSecuredResourceAcl, areaTagging, areaTagCategory,
                areaMatters, areaMatter, areaMatterTag, areaResponsibleUser, areaMatterContact,
                areaContacts, areaContact, areaTasks, areaTask, areaTaskAssignedContact,
                areaTaskMatter, areaTaskResponsibleUser, areaTaskTag, areaTaskTime, areaTiming,
                areaTime, areaNotes, areaNote, areaNoteMatter, areaNoteTask;

            areaSecurity = SetupSecurityArea("Security", null, user);
            areaUser = SetupSecurityArea("Security.User", areaSecurity.Id, user);
            areaArea = SetupSecurityArea("Security.Area", areaSecurity.Id, user);
            areaAreaAcl = SetupSecurityArea("Security.AreaAcl", areaSecurity.Id, user);
            areaSecuredResource = SetupSecurityArea("Security.SecuredResource", areaSecurity.Id, user);
            areaSecuredResourceAcl = SetupSecurityArea("Security.SecuredResourceAcl", areaSecurity.Id, user);
            areaTagging = SetupSecurityArea("Tagging", null, user);
            areaTagCategory = SetupSecurityArea("Tagging.TagCategory", areaTagging.Id, user);
            areaMatters = SetupSecurityArea("Matters", null, user);
            areaMatter = SetupSecurityArea("Matters.Matter", areaMatters.Id, user);
            areaMatterTag = SetupSecurityArea("Matters.MatterTag", areaMatters.Id, user);
            areaResponsibleUser = SetupSecurityArea("Matters.ResponsibleUser", areaMatters.Id, user);
            areaMatterContact = SetupSecurityArea("Matters.MatterContact", areaMatters.Id, user);
            areaContacts = SetupSecurityArea("Contacts", null, user);
            areaContact = SetupSecurityArea("Contacts.Contact", areaContacts.Id, user);
            areaTasks = SetupSecurityArea("Tasks", null, user);
            areaTask = SetupSecurityArea("Tasks.Task", areaTasks.Id, user);
            areaTaskAssignedContact = SetupSecurityArea("Tasks.TaskAssignedContact", areaTasks.Id, user);
            areaTaskMatter = SetupSecurityArea("Tasks.TaskMatter", areaTasks.Id, user);
            areaTaskResponsibleUser = SetupSecurityArea("Tasks.TaskResponsibleUser", areaTasks.Id, user);
            areaTaskTag = SetupSecurityArea("Tasks.TaskTag", areaTasks.Id, user);
            areaTaskTime = SetupSecurityArea("Tasks.TaskTime", areaTasks.Id, user);
            areaTiming = SetupSecurityArea("Timing", null, user);
            areaTime = SetupSecurityArea("Timing.Time", areaTiming.Id, user);
            areaNotes = SetupSecurityArea("Notes", null, user);
            areaNote = SetupSecurityArea("Notes.Note", areaNotes.Id, user);
            areaNoteMatter = SetupSecurityArea("Notes.NoteMatter", areaNotes.Id, user);
            areaNoteTask = SetupSecurityArea("Notes.NoteTask", areaNotes.Id, user);

            // Area Acls
            SetupAreaAcl(areaSecurity, user);
            SetupAreaAcl(areaUser, user);
            SetupAreaAcl(areaArea, user);
            SetupAreaAcl(areaAreaAcl, user);
            SetupAreaAcl(areaSecuredResource, user);
            SetupAreaAcl(areaSecuredResourceAcl, user);
            SetupAreaAcl(areaTagging, user);
            SetupAreaAcl(areaTagCategory, user);
            SetupAreaAcl(areaMatters, user);
            SetupAreaAcl(areaMatter, user);
            SetupAreaAcl(areaMatterTag, user);
            SetupAreaAcl(areaResponsibleUser, user);
            SetupAreaAcl(areaMatterContact, user);
            SetupAreaAcl(areaContacts, user);
            SetupAreaAcl(areaContact, user);
            SetupAreaAcl(areaTasks, user);
            SetupAreaAcl(areaTask, user);
            SetupAreaAcl(areaTaskAssignedContact, user);
            SetupAreaAcl(areaTaskMatter, user);
            SetupAreaAcl(areaTaskResponsibleUser, user);
            SetupAreaAcl(areaTaskTag, user);
            SetupAreaAcl(areaTaskTime, user);
            SetupAreaAcl(areaTiming, user);
            SetupAreaAcl(areaTime, user);
            SetupAreaAcl(areaNotes, user);
            SetupAreaAcl(areaNote, user);
            SetupAreaAcl(areaNoteMatter, user);
            SetupAreaAcl(areaNoteTask, user);

            if (runOptionalData)
                OptionalData(user);
        }

        private static void OptionalData(Common.Models.Security.User user)
        {
            Common.Models.Contacts.Contact contact = Contacts.Contact.Get("Lucas Nodine");
            if (contact == null)
            {
                contact = new Common.Models.Contacts.Contact()
                {
                    IsOrganization = false,
                    IsOurEmployee = true,
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
                contact = Contacts.Contact.Create(contact, user);
            }

            Common.Models.Tagging.TagCategory tagCat1, tagCat2;

            tagCat1 = Tagging.TagCategory.Get("Status");
            if (tagCat1 == null)
            {
                tagCat1 = new Common.Models.Tagging.TagCategory() { Name = "Status" };
                tagCat1 = Data.Tagging.TagCategory.Create(tagCat1, user);
            }

            tagCat2 = Tagging.TagCategory.Get("Jurisdiction");
            if (tagCat2 == null)
            {
                tagCat2 = new Common.Models.Tagging.TagCategory() { Name = "Jurisdiction" };
                tagCat2 = Data.Tagging.TagCategory.Create(tagCat2, user);
            }

            Common.Models.Matters.Matter matter = Matters.Matter.Get(Guid.Parse("30b00c4e-0506-40ea-9e94-58fc2df9b480"));
            if (matter == null)
            {
                matter = new Common.Models.Matters.Matter()
                {
                    Id = Guid.Parse("30b00c4e-0506-40ea-9e94-58fc2df9b480"),
                    Title = "Test Matter 1",
                    Synopsis = "This is the synopsis for test matter 1"
                };
                matter = Matters.Matter.Create(matter, user);
            }

            Common.Models.Matters.MatterTag tag1 = Matters.MatterTag.Get(Guid.Parse("0f0e6ee8-1c27-4707-ba0d-dcf757bd2b82"));
            if (tag1 == null)
            {
                tag1 = new Common.Models.Matters.MatterTag()
                {
                    Id = Guid.Parse("0f0e6ee8-1c27-4707-ba0d-dcf757bd2b82"),
                    Matter = matter,
                    TagCategory = tagCat1,
                    Tag = "Active"
                };
                tag1 = Matters.MatterTag.Create(tag1, user);
            }

            Common.Models.Matters.MatterTag tag2 = Matters.MatterTag.Get(Guid.Parse("75dd343a-b7e0-4094-8933-4e8548169e26"));
            if (tag2 == null)
            {
                tag2 = new Common.Models.Matters.MatterTag()
                {
                    Id = Guid.Parse("75dd343a-b7e0-4094-8933-4e8548169e26"),
                    Matter = matter,
                    TagCategory = tagCat2,
                    Tag = "Labette County, KS"
                };
                tag2 = Matters.MatterTag.Create(tag2, user);
            }

            Common.Models.Matters.ResponsibleUser matterResponsibleUser =
                Matters.ResponsibleUser.Get(matter.Id.Value, user.Id.Value);
            if (matterResponsibleUser == null)
            {
                matterResponsibleUser = new Common.Models.Matters.ResponsibleUser()
                {
                    Matter = matter,
                    User = user,
                    Responsibility = "Attorney"
                };
                matterResponsibleUser = Matters.ResponsibleUser.Create(matterResponsibleUser, user);
            }

            Common.Models.Matters.MatterContact matterContact = Matters.MatterContact.Get(matter.Id.Value, contact.Id.Value);
            if (matterContact == null)
            {
                matterContact = new Common.Models.Matters.MatterContact()
                {
                    Matter = matter,
                    Contact = contact,
                    Role = "Head Attorney"
                };
                matterContact = Matters.MatterContact.Create(matterContact, user);
            }

            Common.Models.Timing.Time time = Timing.Time.Get(Guid.Parse("179f94c4-0b17-4516-82b4-0dbea1875dad"));
            if (time == null)
            {
                time = new Common.Models.Timing.Time()
                {
                    Id = Guid.Parse("179f94c4-0b17-4516-82b4-0dbea1875dad"),
                    Start = DateTime.UtcNow.AddMinutes(-15),
                    Stop = DateTime.UtcNow,
                    Worker = contact
                };
                time = Timing.Time.Create(time, user);
            }

            Common.Models.Tasks.Task task = Tasks.Task.Get(1);
            if (task == null)
            {
                task = new Common.Models.Tasks.Task()
                {
                    ProjectedStart = DateTime.UtcNow.AddMinutes(45),
                    ProjectedEnd = DateTime.UtcNow.AddMinutes(60),
                    DueDate = DateTime.UtcNow.AddDays(1),
                    Title = "Test Task 1",
                    Description = "This is a description for test task 1"
                };
                task = Tasks.Task.Create(task, user);
            }

            Common.Models.Tasks.TaskAssignedContact taskAssigedContact = Tasks.TaskAssignedContact.Get(task.Id.Value, contact.Id.Value);
            if (taskAssigedContact == null)
            {
                taskAssigedContact = new Common.Models.Tasks.TaskAssignedContact()
                {
                    Task = task,
                    Contact = contact,
                    AssignmentType = Common.Models.Tasks.AssignmentType.Direct
                };
                taskAssigedContact = Tasks.TaskAssignedContact.Create(taskAssigedContact, user);
            }

            Common.Models.Tasks.TaskMatter taskMatter = Tasks.TaskMatter.Get(task.Id.Value, matter.Id.Value);
            if (taskMatter == null)
            {
                taskMatter = new Common.Models.Tasks.TaskMatter()
                {
                    Task = task,
                    Matter = matter
                };
                taskMatter = Tasks.TaskMatter.Create(taskMatter, user);
            }

            Common.Models.Tasks.TaskResponsibleUser taskResponsibleUser = Tasks.TaskResponsibleUser.Get(task.Id.Value, user.Id.Value);
            if (taskResponsibleUser == null)
            {
                taskResponsibleUser = new Common.Models.Tasks.TaskResponsibleUser()
                {
                    Task = task,
                    User = user,
                    Responsibility = "Attorney"
                };
                taskResponsibleUser = Tasks.TaskResponsibleUser.Create(taskResponsibleUser, user);
            }

            Common.Models.Tasks.TaskTag taskTag = Tasks.TaskTag.Get(Guid.Parse("62425f5b-b818-4d67-8730-3441a181d98f"));
            if (taskTag == null)
            {
                taskTag = new Common.Models.Tasks.TaskTag()
                {
                    Id = Guid.Parse("62425f5b-b818-4d67-8730-3441a181d98f"),
                    Task = task,
                    TagCategory = tagCat1,
                    Tag = "Pending"
                };
                taskTag = Tasks.TaskTag.Create(taskTag, user);
            }

            Common.Models.Tasks.TaskTime taskTime = Tasks.TaskTime.Get(task.Id.Value, time.Id.Value);
            if (taskTime == null)
            {
                taskTime = new Common.Models.Tasks.TaskTime()
                {
                    Task = task,
                    Time = time
                };
                taskTime = Tasks.TaskTime.Create(taskTime, user);
            }

            Common.Models.Notes.Note note1 = Notes.Note.Get(Guid.Parse("62425f5b-b818-4d67-8730-3441a171d98f"));
            if (note1 == null)
            {
                note1 = new Common.Models.Notes.Note()
                {
                    Id = Guid.Parse("62425f5b-b818-4d67-8730-3441a171d98f"),
                    Title = "Test Note 1",
                    Body = "This is the note for a matter"
                };
                note1 = Notes.Note.Create(note1, user);
                Notes.Note.RelateMatter(note1, matter.Id.Value, user);
            }

            Common.Models.Notes.Note note2 = Notes.Note.Get(Guid.Parse("62425f5b-b118-4d67-8730-3441a171d98f"));
            if (note2 == null)
            {
                note2 = new Common.Models.Notes.Note()
                {
                    Id = Guid.Parse("62425f5b-b118-4d67-8730-3441a171d98f"),
                    Title = "Test Note 2",
                    Body = "This is the note for a task"
                };
                note2 = Notes.Note.Create(note2, user);
                Notes.Note.RelateTask(note2, task.Id.Value, user);
            }

            Common.Models.Documents.Document document1 = Documents.Document.Get(Guid.Parse("d88165ea-ffa9-4bcd-b9ed-950157fac2cf"));
            if (document1 == null)
            {
                document1 = new Common.Models.Documents.Document()
                {
                    Id = Guid.Parse("d88165ea-ffa9-4bcd-b9ed-950157fac2cf"),
                    Title = "Test Document 1"
                };
                document1 = Documents.Document.Create(document1, user);
                Documents.Document.RelateMatter(document1, matter.Id.Value, user);
            }

            Common.Models.Documents.Document document2 = Documents.Document.Get(Guid.Parse("d861cfa7-f446-44d1-8ed1-e7a3f8a495e1"));
            if (document2 == null)
            {
                document2 = new Common.Models.Documents.Document()
                {
                    Id = Guid.Parse("d861cfa7-f446-44d1-8ed1-e7a3f8a495e1"),
                    Title = "Test Document 2"
                };
                document2 = Documents.Document.Create(document2, user);
                Documents.Document.RelateTask(document2, task.Id.Value, user);
            }
        }

        private static Common.Models.Security.Area SetupSecurityArea(string name, int? parentId, 
            Common.Models.Security.User user)
        {
            Common.Models.Security.Area area = Security.Area.Get(name);
            if (area == null)
            {
                area = new Common.Models.Security.Area()
                {
                    Name = name,
                    Description = name
                };
                if (parentId.HasValue)
                    area.Parent = new Common.Models.Security.Area() { Id = parentId.Value };
                area = Security.Area.Create(area, user);
            }
            return area;
        }

        private static Common.Models.Security.AreaAcl SetupAreaAcl(Common.Models.Security.Area area,
            Common.Models.Security.User user)
        {
            Common.Models.Security.AreaAcl areaAcl = Security.AreaAcl.Get(user.Id.Value, area.Id.Value);
            if (areaAcl == null)
            {
                areaAcl = new Common.Models.Security.AreaAcl()
                {
                    Area = area,
                    User = user,
                    AllowFlags = Common.Models.PermissionType.AllAdmin | Common.Models.PermissionType.AllWrite | Common.Models.PermissionType.AllRead,
                    DenyFlags = 0
                };
                areaAcl = Security.AreaAcl.Create(areaAcl, user);
            }
            return areaAcl;
        }

        private static string ClientHashPassword(string plainTextPassword)
        {
            return Hash(plainTextPassword);
        }

        private static string ServerHashPassword(string plainTextPassword, string salt)
        {
            return Hash(salt + plainTextPassword);
        }

        private static string Hash(string str)
        {
            System.Security.Cryptography.SHA512Managed sha512 = new System.Security.Cryptography.SHA512Managed();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
            bytes = sha512.ComputeHash(bytes);
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        private static int GetRandomNumber(int maxNumber)
        {
            if (maxNumber < 1)
                throw new System.Exception("The maxNumber value should be greater than 1");
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            int seed = (b[0] & 0x7f) << 24 | b[1] << 16 | b[2] << 8 | b[3];
            System.Random r = new System.Random(seed);
            return r.Next(1, maxNumber);
        }

        private static string GetRandomString(int length)
        {
            string[] array = new string[54]
	        {
		        "0","2","3","4","5","6","8","9",
		        "a","b","c","d","e","f","g","h","j","k","m","n","p","q","r","s","t","u","v","w","x","y","z",
		        "A","B","C","D","E","F","G","H","J","K","L","M","N","P","R","S","T","U","V","W","X","Y","Z"
	        };

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < length; i++) sb.Append(array[GetRandomNumber(53)]);
            return sb.ToString();
        }
    }
}
