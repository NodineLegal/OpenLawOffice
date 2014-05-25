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

namespace OpenLawOffice.Common.Settings
{
    using System.Configuration;
    using System.IO;
    using System.Security.Cryptography;
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FileStorageSettings : ConfigurationElement
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

        [ConfigurationProperty("tempPath", IsRequired = true)]
        public string TempPath
        {
            get { return (string)this["tempPath"]; }
            set { this["tempPath"] = value; }
        }

        public static string CalculateMd5(string path)
        {
            string output = null;
            using (MD5 md5 = MD5.Create())
            {
                using (FileStream stream = File.OpenRead(path))
                {
                    output = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToUpper();
                }
            }
            return output;
        }

        public string GetCurrentVersionFilepathFor(string filename)
        {
            string[] Split = filename.Split('.');
            return CurrentVersionPath + Split[0] + "." + Split[1];
        }

        public string GetPreviousVersionFilepathFor(string filename)
        {
            string[] Split = filename.Split('.');
            return PreviousVersionsPath + Split[0] + "." + Split[1];
        }

        public void MoveCurrentToPrevious(string filename)
        {
            string currentFilePath = GetCurrentVersionFilepathFor(filename);
            File.Move(currentFilePath, GetPreviousVersionFilepathFor(filename));
        }
    }
}