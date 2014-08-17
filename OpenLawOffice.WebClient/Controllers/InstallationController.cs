// -----------------------------------------------------------------------
// <copyright file="InstallationController.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.WebClient.Controllers
{
    using System.IO;
    using System.Web.Mvc;
    using System.Web.Security;
    using System;

    [HandleError(View = "Errors/Index", Order = 10)]
    public class InstallationController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        private FileInfo GetScriptPath()
        {
            FileInfo fi;
            string path;

            path = Request.PhysicalApplicationPath;

            fi = new FileInfo(path + Path.DirectorySeparatorChar + "Install.sql");
            if (!fi.Exists)
            {
                fi = new FileInfo(path + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "Install.sql");
                if (!fi.Exists)
                {
                    fi = new FileInfo(path + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "Installation" + Path.DirectorySeparatorChar + "Install.sql");
                    if (!fi.Exists)
                        return null;
                }
            }

            return fi;
        }

        [HttpPost]
        public ActionResult Index(ViewModels.Installation.InstallationViewModel viewModel)
        {
            FileInfo fi;

            fi = GetScriptPath();

            if (!fi.Exists)
                return RedirectToAction("MissingDbInstallScript");

            OpenLawOffice.Data.Installation.Setup.CreateDb(fi.FullName);

            RequiredData(fi.FullName, viewModel.Username, viewModel.Password);

            return View("Complete");
        }

        public ActionResult MissingDbInstallScript()
        {
            return View();
        }

        private static void RequiredData(string installDirPath, string adminUser, string adminPass)
        {
            MembershipCreateStatus createStatus;
            AccountMembershipService membershipService = new AccountMembershipService();
            AccountRoleService roleService = new AccountRoleService();

            Common.Models.Account.Users user = Data.Account.Users.Get("Administrator");
            if (user == null)
            {
                createStatus = membershipService.CreateUser(adminUser, adminPass, 
                    Common.Settings.Manager.Instance.System.AdminEmail, true);
            }

            if (!roleService.RoleExists("Login"))
            {
                roleService.CreateRole("Login");
            }

            if (!roleService.RoleExists("Admin"))
            {
                roleService.CreateRole("Admin");
            }

            if (!roleService.RoleExists("User"))
            {
                roleService.CreateRole("User");
            }

            if (!roleService.RoleExists("Client"))
            {
                roleService.CreateRole("Client");
            }

            if (!roleService.IsUserInRole(adminUser, "Login"))
            {
                roleService.AddUserToRole(adminUser, "Login");
            }

            if (!roleService.IsUserInRole(adminUser, "Admin"))
            {
                roleService.AddUserToRole(adminUser, "Admin");
            }

            if (!roleService.IsUserInRole(adminUser, "User"))
            {
                roleService.AddUserToRole(adminUser, "User");
            }
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


        //private static void OptionalData(string installDirPath, Common.Models.Account.Users user)
        //{
        //    Common.Models.Contacts.Contact contact = Data.Contacts.Contact.Get("Lucas Nodine");
        //    if (contact == null)
        //    {
        //        contact = new Common.Models.Contacts.Contact()
        //        {
        //            IsOrganization = false,
        //            IsOurEmployee = true,
        //            Nickname = "Luke",
        //            DisplayNamePrefix = "Mr.",
        //            Surname = "Nodine",
        //            MiddleName = "James",
        //            GivenName = "Lucas",
        //            Initials = "LJN",
        //            DisplayName = "Lucas Nodine",
        //            Email1DisplayName = "Fake Prevent Spambots",
        //            Email1EmailAddress = "no@one.com",
        //            Fax1DisplayName = "Fake Fax",
        //            Fax1FaxNumber = "1-555-555-5555",
        //            Address1DisplayName = "Nodine Legal PO Box",
        //            Address1AddressCity = "Parsons",
        //            Address1AddressStateOrProvince = "Kansas",
        //            Address1AddressPostalCode = "67357",
        //            Address1AddressCountry = "USA",
        //            Address1AddressCountryCode = "1",
        //            Address1AddressPostOfficeBox = "1125",
        //            Telephone1DisplayName = "Virtual Office",
        //            Telephone1TelephoneNumber = "620-577-4271",
        //            Birthday = new DateTime(1982, 3, 25),
        //            Wedding = new DateTime(2012, 5, 12),
        //            Title = "Managing Member",
        //            CompanyName = "Nodine Legal, LLC",
        //            Profession = "Attorney",
        //            Language = "English (American)",
        //            BusinessHomePage = "www.nodinelegal.com",
        //            Gender = "Male",
        //            ReferredByName = "Bob"
        //        };
        //        contact = Data.Contacts.Contact.Create(contact, user);
        //    }

        //    Common.Models.Tagging.TagCategory tagCat1, tagCat2;

        //    tagCat1 = Data.Tagging.TagCategory.Get("Status");
        //    if (tagCat1 == null)
        //    {
        //        tagCat1 = new Common.Models.Tagging.TagCategory() { Name = "Status" };
        //        tagCat1 = Data.Tagging.TagCategory.Create(tagCat1, user);
        //    }

        //    tagCat2 = Data.Tagging.TagCategory.Get("Jurisdiction");
        //    if (tagCat2 == null)
        //    {
        //        tagCat2 = new Common.Models.Tagging.TagCategory() { Name = "Jurisdiction" };
        //        tagCat2 = Data.Tagging.TagCategory.Create(tagCat2, user);
        //    }

        //    Common.Models.Matters.Matter matter = Data.Matters.Matter.Get(Guid.Parse("30b00c4e-0506-40ea-9e94-58fc2df9b480"));
        //    if (matter == null)
        //    {
        //        matter = new Common.Models.Matters.Matter()
        //        {
        //            Id = Guid.Parse("30b00c4e-0506-40ea-9e94-58fc2df9b480"),
        //            Title = "Test Matter 1",
        //            Synopsis = "This is the synopsis for test matter 1"
        //        };
        //        matter = Data.Matters.Matter.Create(matter, user);
        //    }

        //    Common.Models.Matters.MatterTag tag1 = Data.Matters.MatterTag.Get(Guid.Parse("0f0e6ee8-1c27-4707-ba0d-dcf757bd2b82"));
        //    if (tag1 == null)
        //    {
        //        tag1 = new Common.Models.Matters.MatterTag()
        //        {
        //            Id = Guid.Parse("0f0e6ee8-1c27-4707-ba0d-dcf757bd2b82"),
        //            Matter = matter,
        //            TagCategory = tagCat1,
        //            Tag = "Active"
        //        };
        //        tag1 = Data.Matters.MatterTag.Create(tag1, user);
        //    }

        //    Common.Models.Matters.MatterTag tag2 = Data.Matters.MatterTag.Get(Guid.Parse("75dd343a-b7e0-4094-8933-4e8548169e26"));
        //    if (tag2 == null)
        //    {
        //        tag2 = new Common.Models.Matters.MatterTag()
        //        {
        //            Id = Guid.Parse("75dd343a-b7e0-4094-8933-4e8548169e26"),
        //            Matter = matter,
        //            TagCategory = tagCat2,
        //            Tag = "Labette County, KS"
        //        };
        //        tag2 = Data.Matters.MatterTag.Create(tag2, user);
        //    }

        //    Common.Models.Matters.ResponsibleUser matterResponsibleUser =
        //        Data.Matters.ResponsibleUser.Get(matter.Id.Value, user.PId.Value);
        //    if (matterResponsibleUser == null)
        //    {
        //        matterResponsibleUser = new Common.Models.Matters.ResponsibleUser()
        //        {
        //            Matter = matter,
        //            User = user,
        //            Responsibility = "Attorney"
        //        };
        //        matterResponsibleUser = Data.Matters.ResponsibleUser.Create(matterResponsibleUser, user);
        //    }

        //    Common.Models.Matters.MatterContact matterContact = Data.Matters.MatterContact.Get(matter.Id.Value, contact.Id.Value);
        //    if (matterContact == null)
        //    {
        //        matterContact = new Common.Models.Matters.MatterContact()
        //        {
        //            Matter = matter,
        //            Contact = contact,
        //            Role = "Head Attorney"
        //        };
        //        matterContact = Data.Matters.MatterContact.Create(matterContact, user);
        //    }

        //    Common.Models.Timing.Time time = Data.Timing.Time.Get(Guid.Parse("179f94c4-0b17-4516-82b4-0dbea1875dad"));
        //    if (time == null)
        //    {
        //        time = new Common.Models.Timing.Time()
        //        {
        //            Id = Guid.Parse("179f94c4-0b17-4516-82b4-0dbea1875dad"),
        //            Start = DateTime.UtcNow.AddMinutes(-15),
        //            Stop = DateTime.UtcNow,
        //            Worker = contact
        //        };
        //        time = Data.Timing.Time.Create(time, user);
        //    }

        //    Common.Models.Tasks.Task task = Data.Tasks.Task.Get(1);
        //    if (task == null)
        //    {
        //        task = new Common.Models.Tasks.Task()
        //        {
        //            ProjectedStart = DateTime.UtcNow.AddMinutes(45),
        //            ProjectedEnd = DateTime.UtcNow.AddMinutes(60),
        //            DueDate = DateTime.UtcNow.AddDays(1),
        //            Title = "Test Task 1",
        //            Description = "This is a description for test task 1"
        //        };
        //        task = Data.Tasks.Task.Create(task, user);
        //    }

        //    Common.Models.Tasks.TaskAssignedContact taskAssigedContact = Data.Tasks.TaskAssignedContact.Get(task.Id.Value, contact.Id.Value);
        //    if (taskAssigedContact == null)
        //    {
        //        taskAssigedContact = new Common.Models.Tasks.TaskAssignedContact()
        //        {
        //            Task = task,
        //            Contact = contact,
        //            AssignmentType = Common.Models.Tasks.AssignmentType.Direct
        //        };
        //        taskAssigedContact = Data.Tasks.TaskAssignedContact.Create(taskAssigedContact, user);
        //    }

        //    Common.Models.Tasks.TaskMatter taskMatter = Data.Tasks.TaskMatter.Get(task.Id.Value, matter.Id.Value);
        //    if (taskMatter == null)
        //    {
        //        taskMatter = new Common.Models.Tasks.TaskMatter()
        //        {
        //            Task = task,
        //            Matter = matter
        //        };
        //        taskMatter = Data.Tasks.TaskMatter.Create(taskMatter, user);
        //    }

        //    Common.Models.Tasks.TaskResponsibleUser taskResponsibleUser = Data.Tasks.TaskResponsibleUser.Get(task.Id.Value, user.PId.Value);
        //    if (taskResponsibleUser == null)
        //    {
        //        taskResponsibleUser = new Common.Models.Tasks.TaskResponsibleUser()
        //        {
        //            Task = task,
        //            User = user,
        //            Responsibility = "Attorney"
        //        };
        //        taskResponsibleUser = Data.Tasks.TaskResponsibleUser.Create(taskResponsibleUser, user);
        //    }

        //    Common.Models.Tasks.TaskTag taskTag = Data.Tasks.TaskTag.Get(Guid.Parse("62425f5b-b818-4d67-8730-3441a181d98f"));
        //    if (taskTag == null)
        //    {
        //        taskTag = new Common.Models.Tasks.TaskTag()
        //        {
        //            Id = Guid.Parse("62425f5b-b818-4d67-8730-3441a181d98f"),
        //            Task = task,
        //            TagCategory = tagCat1,
        //            Tag = "Pending"
        //        };
        //        taskTag = Data.Tasks.TaskTag.Create(taskTag, user);
        //    }

        //    Common.Models.Tasks.TaskTime taskTime = Data.Tasks.TaskTime.Get(task.Id.Value, time.Id.Value);
        //    if (taskTime == null)
        //    {
        //        taskTime = new Common.Models.Tasks.TaskTime()
        //        {
        //            Task = task,
        //            Time = time
        //        };
        //        taskTime = Data.Tasks.TaskTime.Create(taskTime, user);
        //    }

        //    Common.Models.Notes.Note note1 = Data.Notes.Note.Get(Guid.Parse("62425f5b-b818-4d67-8730-3441a171d98f"));
        //    if (note1 == null)
        //    {
        //        note1 = new Common.Models.Notes.Note()
        //        {
        //            Id = Guid.Parse("62425f5b-b818-4d67-8730-3441a171d98f"),
        //            Title = "Test Note 1",
        //            Body = "This is the note for a matter"
        //        };
        //        note1 = Data.Notes.Note.Create(note1, user);
        //        Data.Notes.Note.RelateMatter(note1, matter.Id.Value, user);
        //    }

        //    Common.Models.Notes.Note note2 = Data.Notes.Note.Get(Guid.Parse("62425f5b-b118-4d67-8730-3441a171d98f"));
        //    if (note2 == null)
        //    {
        //        note2 = new Common.Models.Notes.Note()
        //        {
        //            Id = Guid.Parse("62425f5b-b118-4d67-8730-3441a171d98f"),
        //            Title = "Test Note 2",
        //            Body = "This is the note for a task"
        //        };
        //        note2 = Data.Notes.Note.Create(note2, user);
        //        Data.Notes.Note.RelateTask(note2, task.Id.Value, user);
        //    }

        //    Common.Models.Documents.Document document1 = Data.Documents.Document.Get(Guid.Parse("d88165ea-ffa9-4bcd-b9ed-950157fac2cf"));
        //    if (document1 == null)
        //    {
        //        document1 = new Common.Models.Documents.Document()
        //        {
        //            Id = Guid.Parse("d88165ea-ffa9-4bcd-b9ed-950157fac2cf"),
        //            Title = "Test Document 1"
        //        };
        //        document1 = Data.Documents.Document.Create(document1, user);
        //        Data.Documents.Document.RelateMatter(document1, matter.Id.Value, user);

        //        Common.Models.Documents.Version version1 = new Common.Models.Documents.Version()
        //        {
        //            Id = Guid.Parse("59879231-2db4-45f2-8b8d-850079bb11fa"),
        //            Document = document1,
        //            VersionNumber = 1,
        //            Mime = "application/pdf",
        //            Filename = "fw4",
        //            Extension = "pdf",
        //            Size = 113881,
        //            Md5 = "C68A7AEE32C45AE945F4DA680347B382"
        //        };

        //        Data.Documents.Document.CreateNewVersion(document1.Id.Value, version1, user);
        //        System.IO.File.Copy(installDirPath + "fw4.pdf", Common.Settings.Manager.Instance.FileStorage.CurrentVersionPath + version1.Id.Value.ToString() + "." + version1.Extension);
        //    }

        //    Common.Models.Documents.Document document2 = Data.Documents.Document.Get(Guid.Parse("d861cfa7-f446-44d1-8ed1-e7a3f8a495e1"));
        //    if (document2 == null)
        //    {
        //        document2 = new Common.Models.Documents.Document()
        //        {
        //            Id = Guid.Parse("d861cfa7-f446-44d1-8ed1-e7a3f8a495e1"),
        //            Title = "Test Document 2"
        //        };
        //        document2 = Data.Documents.Document.Create(document2, user);
        //        Data.Documents.Document.RelateTask(document2, task.Id.Value, user);

        //        Common.Models.Documents.Version version2 = new Common.Models.Documents.Version()
        //        {
        //            Id = Guid.Parse("1955238c-52e1-4f8c-8e14-2b502aa5b492"),
        //            Document = document2,
        //            VersionNumber = 1,
        //            Mime = "application/pdf",
        //            Filename = "bitcoin",
        //            Extension = "pdf",
        //            Size = 184292,
        //            Md5 = "D56D71ECADF2137BE09D8B1D35C6C042"
        //        };

        //        Data.Documents.Document.CreateNewVersion(document2.Id.Value, version2, user);
        //        System.IO.File.Copy(installDirPath + "bitcoin.pdf", Common.Settings.Manager.Instance.FileStorage.CurrentVersionPath + version2.Id.Value.ToString() + "." + version2.Extension);
        //    }
        //}
    }
}