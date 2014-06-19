//
// Copyright © 2006 - 2011 Nauck IT KG		http://www.nauck-it.de
//
// Author:
//	Daniel Nauck		<d.nauck(at)nauck-it.de>
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace OpenLawOffice.Data.PostgreSQLProvider
{
    using System;
    using System.Configuration;
    using System.Configuration.Provider;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;
    using System.Web.Profile;
    using System.Web.Hosting;
    using Npgsql;
    using NpgsqlTypes;

    public class PgProfileProvider : ProfileProvider
    {
        private const string s_profilesTableName = "Profiles";
        private const string s_profileDataTableName = "ProfileData";
        private const string s_serializationNamespace = "http://schemas.nauck-it.de/PostgreSQLProvider/1.0/";
        private string m_connectionString = string.Empty;

        /// <summary>
        /// System.Configuration.Provider.ProviderBase.Initialize Method
        /// </summary>
        public override void Initialize(string name, NameValueCollection config)
        {
            // Initialize values from web.config.
            if (config == null)
                throw new ArgumentNullException("config", Properties.Resources.ErrArgumentNull);

            if (string.IsNullOrEmpty(name))
                name = Properties.Resources.ProfileProviderDefaultName;

            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", Properties.Resources.ProfileProviderDefaultDescription);
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);

            m_applicationName = PgMembershipProvider.GetConfigValue(config["applicationName"], HostingEnvironment.ApplicationVirtualPath);

            // Get connection string.
            m_connectionString = PgMembershipProvider.GetConnectionString(config["connectionStringName"]);
        }

        /// <summary>
        /// System.Web.Profile.ProfileProvider properties.
        /// </summary>
        #region System.Web.Security.ProfileProvider properties
        private string m_applicationName = string.Empty;

        public override string ApplicationName
        {
            get { return m_applicationName; }
            set { m_applicationName = value; }
        }
        #endregion

        /// <summary>
        /// System.Web.Profile.ProfileProvider methods.
        /// </summary>
        #region System.Web.Security.ProfileProvider methods

        /// <summary>
        /// ProfileProvider.DeleteInactiveProfiles
        /// </summary>
        public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            throw new NotImplementedException("DeleteInactiveProfiles: The method or operation is not implemented.");
        }

        public override int DeleteProfiles(string[] usernames)
        {
            throw new NotImplementedException("DeleteProfiles1: The method or operation is not implemented.");
        }

        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            throw new NotImplementedException("DeleteProfiles2: The method or operation is not implemented.");
        }

        public override ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException("FindInactiveProfilesByUserName: The method or operation is not implemented.");
        }

        public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException("FindProfilesByUserName: The method or operation is not implemented.");
        }

        public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException("GetAllInactiveProfiles: The method or operation is not implemented.");
        }

        public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException("GetAllProfiles: The method or operation is not implemented.");
        }

        public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            throw new NotImplementedException("GetNumberOfInactiveProfiles: The method or operation is not implemented.");
        }
        #endregion

        /// <summary>
        /// System.Configuration.SettingsProvider methods.
        /// </summary>
        #region System.Web.Security.SettingsProvider methods

        /// <summary>
        /// 
        /// </summary>
        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            SettingsPropertyValueCollection result = new SettingsPropertyValueCollection();
            string username = (string)context["UserName"];
            bool isAuthenticated = (bool)context["IsAuthenticated"];
            Dictionary<string, object> databaseResult = new Dictionary<string, object>();

            using (NpgsqlConnection dbConn = new NpgsqlConnection(m_connectionString))
            {
                using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                {
                    dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "SELECT \"Name\", \"ValueString\", \"ValueBinary\" FROM \"{0}\" WHERE \"Profile\" = (SELECT \"pId\" FROM \"{1}\" WHERE \"Username\" = @Username AND \"ApplicationName\" = @ApplicationName AND \"IsAnonymous\" = @IsAuthenticated)", s_profileDataTableName, s_profilesTableName);

                    dbCommand.Parameters.Add("@Username", NpgsqlDbType.Varchar, 255).Value = username;
                    dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;
                    dbCommand.Parameters.Add("@IsAuthenticated", NpgsqlDbType.Boolean).Value = !isAuthenticated;

                    try
                    {
                        dbConn.Open();
                        dbCommand.Prepare();

                        using (NpgsqlDataReader reader = dbCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                object resultData = null;
                                if (!reader.IsDBNull(1))
                                    resultData = reader.GetValue(1);
                                else if (!reader.IsDBNull(2))
                                    resultData = reader.GetValue(2);

                                databaseResult.Add(reader.GetString(0), resultData);
                            }
                        }
                    }
                    catch (NpgsqlException e)
                    {
                        Trace.WriteLine(e.ToString());
                        throw new ProviderException(Properties.Resources.ErrOperationAborted);
                    }
                    finally
                    {
                        if (dbConn != null)
                            dbConn.Close();
                    }
                }
            }

            foreach (SettingsProperty item in collection)
            {
                if (item.SerializeAs == SettingsSerializeAs.ProviderSpecific)
                {
                    if (item.PropertyType.IsPrimitive || item.PropertyType.Equals(typeof(string)))
                        item.SerializeAs = SettingsSerializeAs.String;
                    else
                        item.SerializeAs = SettingsSerializeAs.Xml;
                }

                SettingsPropertyValue itemValue = new SettingsPropertyValue(item);

                if ((databaseResult.ContainsKey(item.Name)) && (databaseResult[item.Name] != null))
                {
                    if (item.SerializeAs == SettingsSerializeAs.String)
                        itemValue.PropertyValue = SerializationHelper.DeserializeFromBase64<object>((string)databaseResult[item.Name]);

                    else if (item.SerializeAs == SettingsSerializeAs.Xml)
                        itemValue.PropertyValue = SerializationHelper.DeserializeFromXml<object>((string)databaseResult[item.Name], s_serializationNamespace);

                    else if (item.SerializeAs == SettingsSerializeAs.Binary)
                        itemValue.PropertyValue = SerializationHelper.DeserializeFromBinary<object>((byte[])databaseResult[item.Name]);
                }
                itemValue.IsDirty = false;
                result.Add(itemValue);
            }

            UpdateActivityDates(username, isAuthenticated, true);

            return result;
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            string username = (string)context["UserName"];
            bool isAuthenticated = (bool)context["IsAuthenticated"];

            if (string.IsNullOrEmpty(username))
                return;

            if (collection.Count < 1)
                return;

            if (!ProfileExists(username))
                CreateProfileForUser(username, isAuthenticated);

            using (NpgsqlConnection dbConn = new NpgsqlConnection(m_connectionString))
            {
                using (NpgsqlCommand deleteCommand = dbConn.CreateCommand(),
                    insertCommand = dbConn.CreateCommand())
                {
                    deleteCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "DELETE FROM \"{0}\" WHERE \"Name\" = @Name AND \"Profile\" = (SELECT \"pId\" FROM \"{1}\" WHERE \"Username\" = @Username AND \"ApplicationName\" = @ApplicationName AND \"IsAnonymous\" = @IsAuthenticated)", s_profileDataTableName, s_profilesTableName);

                    deleteCommand.Parameters.Add("@Name", NpgsqlDbType.Varchar, 255);
                    deleteCommand.Parameters.Add("@Username", NpgsqlDbType.Varchar, 255).Value = username;
                    deleteCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;
                    deleteCommand.Parameters.Add("@IsAuthenticated", NpgsqlDbType.Boolean).Value = !isAuthenticated;


                    insertCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "INSERT INTO \"{0}\" (\"pId\", \"Profile\", \"Name\", \"ValueString\", \"ValueBinary\") VALUES (@pId, (SELECT \"pId\" FROM \"{1}\" WHERE \"Username\" = @Username AND \"ApplicationName\" = @ApplicationName AND \"IsAnonymous\" = @IsAuthenticated), @Name, @ValueString, @ValueBinary)", s_profileDataTableName, s_profilesTableName);

                    insertCommand.Parameters.Add("@pId", NpgsqlDbType.Varchar, 36);
                    insertCommand.Parameters.Add("@Name", NpgsqlDbType.Varchar, 255);
                    insertCommand.Parameters.Add("@ValueString", NpgsqlDbType.Text);
                    insertCommand.Parameters["@ValueString"].IsNullable = true;
                    insertCommand.Parameters.Add("@ValueBinary", NpgsqlDbType.Bytea);
                    insertCommand.Parameters["@ValueBinary"].IsNullable = true;
                    insertCommand.Parameters.Add("@Username", NpgsqlDbType.Varchar, 255).Value = username;
                    insertCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;
                    insertCommand.Parameters.Add("@IsAuthenticated", NpgsqlDbType.Boolean).Value = !isAuthenticated;

                    NpgsqlTransaction dbTrans = null;

                    try
                    {
                        dbConn.Open();
                        deleteCommand.Prepare();
                        insertCommand.Prepare();

                        using (dbTrans = dbConn.BeginTransaction())
                        {

                            foreach (SettingsPropertyValue item in collection)
                            {
                                if (!item.IsDirty)
                                    continue;

                                deleteCommand.Parameters["@Name"].Value = item.Name;

                                insertCommand.Parameters["@pId"].Value = Guid.NewGuid().ToString();
                                insertCommand.Parameters["@Name"].Value = item.Name;

                                if (item.Property.SerializeAs == SettingsSerializeAs.String)
                                {
                                    insertCommand.Parameters["@ValueString"].Value = SerializationHelper.SerializeToBase64(item.PropertyValue);
                                    insertCommand.Parameters["@ValueBinary"].Value = DBNull.Value;
                                }
                                else if (item.Property.SerializeAs == SettingsSerializeAs.Xml)
                                {
                                    item.SerializedValue = SerializationHelper.SerializeToXml<object>(item.PropertyValue, s_serializationNamespace);
                                    insertCommand.Parameters["@ValueString"].Value = item.SerializedValue;
                                    insertCommand.Parameters["@ValueBinary"].Value = DBNull.Value;
                                }
                                else if (item.Property.SerializeAs == SettingsSerializeAs.Binary)
                                {
                                    item.SerializedValue = SerializationHelper.SerializeToBinary(item.PropertyValue);
                                    insertCommand.Parameters["@ValueString"].Value = DBNull.Value;
                                    insertCommand.Parameters["@ValueBinary"].Value = item.SerializedValue;
                                }

                                deleteCommand.ExecuteNonQuery();
                                insertCommand.ExecuteNonQuery();
                            }

                            // Attempt to commit the transaction
                            dbTrans.Commit();
                        }
                    }
                    catch (NpgsqlException e)
                    {
                        Trace.WriteLine(e.ToString());

                        try
                        {
                            // Attempt to roll back the transaction
                            Trace.WriteLine(Properties.Resources.LogRollbackAttempt);
                            dbTrans.Rollback();
                        }
                        catch (NpgsqlException re)
                        {
                            // Rollback failed
                            Trace.WriteLine(Properties.Resources.ErrRollbackFailed);
                            Trace.WriteLine(re.ToString());
                        }

                        throw new ProviderException(Properties.Resources.ErrOperationAborted);
                    }
                    finally
                    {
                        if (dbConn != null)
                            dbConn.Close();
                    }
                }
            }

            UpdateActivityDates(username, isAuthenticated, false);
        }
        #endregion

        #region private methods
        /// <summary>
        /// Create a empty user profile
        /// </summary>
        /// <param name="username"></param>
        /// <param name="isAuthenticated"></param>
        private void CreateProfileForUser(string username, bool isAuthenticated)
        {
            if (ProfileExists(username))
                throw new ProviderException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.ErrProfileAlreadyExist, username));

            using (NpgsqlConnection dbConn = new NpgsqlConnection(m_connectionString))
            {
                using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                {
                    dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "INSERT INTO \"{0}\" (\"pId\", \"Username\", \"ApplicationName\", \"IsAnonymous\", \"LastActivityDate\", \"LastUpdatedDate\") Values (@pId, @Username, @ApplicationName, @IsAuthenticated, @LastActivityDate, @LastUpdatedDate)", s_profilesTableName);

                    dbCommand.Parameters.Add("@pId", NpgsqlDbType.Varchar, 36).Value = Guid.NewGuid().ToString();
                    dbCommand.Parameters.Add("@Username", NpgsqlDbType.Varchar, 255).Value = username;
                    dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;
                    dbCommand.Parameters.Add("@IsAuthenticated", NpgsqlDbType.Boolean).Value = !isAuthenticated;
                    dbCommand.Parameters.Add("@LastActivityDate", NpgsqlDbType.TimestampTZ).Value = DateTime.Now;
                    dbCommand.Parameters.Add("@LastUpdatedDate", NpgsqlDbType.TimestampTZ).Value = DateTime.Now;

                    try
                    {
                        dbConn.Open();
                        dbCommand.Prepare();

                        dbCommand.ExecuteNonQuery();
                    }
                    catch (NpgsqlException e)
                    {
                        Trace.WriteLine(e.ToString());
                        throw new ProviderException(Properties.Resources.ErrOperationAborted);
                    }
                    finally
                    {
                        if (dbConn != null)
                            dbConn.Close();
                    }
                }
            }
        }


        private bool ProfileExists(string username)
        {
            using (NpgsqlConnection dbConn = new NpgsqlConnection(m_connectionString))
            {
                using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                {
                    dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "SELECT COUNT(*) FROM \"{0}\" WHERE \"Username\" = @Username AND \"ApplicationName\" = @ApplicationName", s_profilesTableName);

                    dbCommand.Parameters.Add("@Username", NpgsqlDbType.Varchar, 255).Value = username;
                    dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;

                    try
                    {
                        dbConn.Open();
                        dbCommand.Prepare();

                        int numRecs = 0;
                        if (!Int32.TryParse(dbCommand.ExecuteScalar().ToString(), out numRecs))
                            return false;

                        if (numRecs > 0)
                            return true;
                    }
                    catch (NpgsqlException e)
                    {
                        Trace.WriteLine(e.ToString());
                        throw new ProviderException(Properties.Resources.ErrOperationAborted);
                    }
                    finally
                    {
                        if (dbConn != null)
                            dbConn.Close();
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Updates the LastActivityDate and LastUpdatedDate values when profile properties are accessed by the
        /// GetPropertyValues and SetPropertyValues methods.
        /// Passing true as the activityOnly parameter will update only the LastActivityDate.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="isAuthenticated"></param>
        /// <param name="activityOnly"></param>
        private void UpdateActivityDates(string username, bool isAuthenticated, bool activityOnly)
        {
            using (NpgsqlConnection dbConn = new NpgsqlConnection(m_connectionString))
            {
                using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                {
                    if (activityOnly)
                    {
                        dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "UPDATE \"{0}\" SET \"LastActivityDate\" = @LastActivityDate WHERE \"Username\" = @Username AND \"ApplicationName\" = @ApplicationName AND \"IsAnonymous\" = @IsAuthenticated", s_profilesTableName);

                        dbCommand.Parameters.Add("@LastActivityDate", NpgsqlDbType.TimestampTZ).Value = DateTime.Now;
                    }
                    else
                    {
                        dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "UPDATE \"{0}\" SET \"LastActivityDate\" = @LastActivityDate, \"LastUpdatedDate\" = @LastActivityDate WHERE \"Username\" = @Username AND \"ApplicationName\" = @ApplicationName AND \"IsAnonymous\" = @IsAuthenticated", s_profilesTableName);

                        dbCommand.Parameters.Add("@LastActivityDate", NpgsqlDbType.TimestampTZ).Value = DateTime.Now;
                        //dbCommand.Parameters.Add("@LastUpdatedDate", NpgsqlDbType.TimestampTZ).Value = DateTime.Now;
                    }

                    dbCommand.Parameters.Add("@Username", NpgsqlDbType.Varchar, 255).Value = username;
                    dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;
                    dbCommand.Parameters.Add("@IsAuthenticated", NpgsqlDbType.Boolean).Value = !isAuthenticated;

                    try
                    {
                        dbConn.Open();
                        dbCommand.Prepare();

                        dbCommand.ExecuteNonQuery();
                    }
                    catch (NpgsqlException e)
                    {
                        Trace.WriteLine(e.ToString());
                        throw new ProviderException(Properties.Resources.ErrOperationAborted);
                    }
                    finally
                    {
                        if (dbConn != null)
                            dbConn.Close();
                    }
                }
            }
        }
        #endregion
    }
}
