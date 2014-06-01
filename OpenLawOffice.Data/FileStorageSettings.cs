// -----------------------------------------------------------------------
// <copyright file="FileStorageSettings.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data
{
    using System.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FileStorageSettings : ConfigurationSection
    {
        [ConfigurationProperty("currentVersionPath", IsRequired = true)]
        public string CurrentVersionPath
        {
            get { return (string)this["currentVersionPath"]; }
            set { this["currentVersionPath"] = value; }
        }

        [ConfigurationProperty("previousVersionsPath", IsRequired = true)]
        public string PreviousVersionsPath
        {
            get { return (string)this["previousVersionsPath"]; }
            set { this["previousVersionsPath"] = value; }
        }

        public static FileStorageSettings Load()
        {
            return (FileStorageSettings)System.Configuration.ConfigurationManager.GetSection("fileStorageSettings");
        }
    }
}