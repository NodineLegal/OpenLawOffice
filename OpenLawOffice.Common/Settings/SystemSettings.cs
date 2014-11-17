// -----------------------------------------------------------------------
// <copyright file="SystemSettings.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Common.Settings
{
    using System;
    using System.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SystemSettings : ConfigurationElement
    {
        //[ConfigurationProperty("fileStorage", IsRequired = true)]
        //public FileStorageSettings FileStorage
        //{
        //    get { return (FileStorageSettings)base["fileStorage"]; }
        //    set { base["fileStorage"] = value; }
        //}

        [ConfigurationProperty("timezone", IsRequired = true)]
        public string Timezone
        {
            get { return (string)base["timezone"]; }
            set { base["timezone"] = value; }
        }

        [ConfigurationProperty("globalTaskTagFilters", IsDefaultCollection = false)]
        public TagFilterCollection GlobalTaskTagFilters
        {
            get { return (TagFilterCollection)base["globalTaskTagFilters"]; }
        }

        [ConfigurationProperty("passwordRetrievalFromEmail", IsRequired = true)]
        public string PasswordRetrievalFromEmail
        {
            get { return (string)base["passwordRetrievalFromEmail"]; }
            set { base["passwordRetrievalFromEmail"] = value; }
        }

        [ConfigurationProperty("websiteUrl", IsRequired = true)]
        public Uri WebsiteUrl
        {
            get { return (Uri)base["websiteUrl"]; }
            set { base["websiteUrl"] = value.ToString(); }
        }

        [ConfigurationProperty("adminEmail", IsRequired = true)]
        public string AdminEmail
        {
            get { return (string)base["adminEmail"]; }
            set { base["adminEmail"] = value.ToString(); }
        }

        [ConfigurationProperty("office365AuthEndpoint", IsRequired = false)]
        public Uri Office365AuthEndpoint
        {
            get { return (Uri)base["office365AuthEndpoint"]; }
            set { base["office365AuthEndpoint"] = value.ToString(); }
        }

        [ConfigurationProperty("office365TokenEndpoint", IsRequired = false)]
        public Uri Office365TokenEndpoint
        {
            get { return (Uri)base["office365TokenEndpoint"]; }
            set { base["office365TokenEndpoint"] = value.ToString(); }
        }

        [ConfigurationProperty("office365ClientId", IsRequired = false)]
        public string Office365ClientId
        {
            get { return (string)base["office365ClientId"]; }
            set { base["office365ClientId"] = value.ToString(); }
        }

        [ConfigurationProperty("office365ClientKey", IsRequired = false)]
        public string Office365ClientKey
        {
            get { return (string)base["office365ClientKey"]; }
            set { base["office365ClientKey"] = value.ToString(); }
        }

        //public static SystemSettings Load()
        //{
        //    return (SystemSettings)ConfigurationManager.GetSection("OpenLawOffice");
        //}
    }
}
