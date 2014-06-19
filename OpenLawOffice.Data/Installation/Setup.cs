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
    using System;
    using System.Configuration;
    using System.IO;
    using Npgsql;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Setup
    {
        public static void CreateDb(string filepath, bool setupData = false)
        {
            FileInfo fi = new FileInfo(filepath);
            string dirName = fi.DirectoryName;

            if (!dirName.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                dirName += System.IO.Path.DirectorySeparatorChar;

            ExecuteScript(filepath);
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
    }
}