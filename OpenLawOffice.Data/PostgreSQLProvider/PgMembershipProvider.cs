//
// Copyright © 2006 - 2013 Nauck IT KG		http://www.nauck-it.de
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
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Text;
    using System.Security.Cryptography;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Configuration;
    using System.Web.Security;
    using System.Configuration;
    using System.Configuration.Provider;
    using Npgsql;
    using NpgsqlTypes;
    using System.Diagnostics;

    public class PgMembershipProvider : MembershipProvider
    {
        private const string s_tableName = "Users";
        private const int s_newPasswordLength = 8;
        private string m_connectionString = string.Empty;

        // Used when determining encryption key values.
        private MachineKeySection m_machineKeyConfig = null;

        /// <summary>
        /// System.Configuration.Provider.ProviderBase.Initialize Method.
        /// </summary>
        public override void Initialize(string name, NameValueCollection config)
        {
            // Initialize values from web.config.
            if (config == null)
                throw new ArgumentNullException("config", Properties.Resources.ErrArgumentNull);

            if (string.IsNullOrEmpty(name))
                name = Properties.Resources.MembershipProviderDefaultName;

            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", Properties.Resources.MembershipProviderDefaultDescription);
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);

            m_applicationName = GetConfigValue(config["applicationName"], HostingEnvironment.ApplicationVirtualPath);
            m_maxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"), CultureInfo.InvariantCulture);
            m_passwordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"), CultureInfo.InvariantCulture);
            m_minRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredNonAlphanumericCharacters"], "1"), CultureInfo.InvariantCulture);
            m_minRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "7"), CultureInfo.InvariantCulture);
            m_passwordStrengthRegularExpression = GetConfigValue(config["passwordStrengthRegularExpression"], "");
            m_enablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"), CultureInfo.InvariantCulture);
            m_enablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"), CultureInfo.InvariantCulture);
            m_requiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"), CultureInfo.InvariantCulture);
            m_requiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"), CultureInfo.InvariantCulture);

            // Get password encryption type.
            string pwFormat = GetConfigValue(config["passwordFormat"], "Hashed");
            switch (pwFormat)
            {
                case "Hashed":
                    m_passwordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    m_passwordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    m_passwordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new ProviderException(Properties.Resources.ErrPwFormatNotSupported);
            }

            // Get connection string.
            m_connectionString = GetConnectionString(config["connectionStringName"]);


            // Check whether we are on a web hosted application or not
            // If we're web hosted use the Web.config; otherwise the application's config file.
            // Then get encryption and decryption key information from the configuration.
            m_machineKeyConfig = HostingEnvironment.IsHosted
                                    ? WebConfigurationManager.GetSection("system.web/machineKey") as MachineKeySection
                                    : ConfigurationManager.GetSection("system.web/machineKey") as MachineKeySection;

            if (!m_passwordFormat.Equals(MembershipPasswordFormat.Clear))
            {
                if (m_machineKeyConfig == null)
                    throw new ProviderException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.ErrConfigSectionNotFound, "system.web/machineKey"));

                if (m_machineKeyConfig.ValidationKey.Contains("AutoGenerate"))
                    throw new ProviderException(Properties.Resources.ErrAutoGeneratedKeyNotSupported);
            }
        }

        /// <summary>
        /// System.Web.Security.MembershipProvider properties.
        /// </summary>
        #region System.Web.Security.MembershipProvider properties
        private string m_applicationName = string.Empty;
        private bool m_enablePasswordReset = false;
        private bool m_enablePasswordRetrieval = false;
        private bool m_requiresQuestionAndAnswer = false;
        private bool m_requiresUniqueEmail = false;
        private int m_maxInvalidPasswordAttempts = 0;
        private int m_passwordAttemptWindow = 0;
        private MembershipPasswordFormat m_passwordFormat = MembershipPasswordFormat.Clear;
        private int m_minRequiredNonAlphanumericCharacters = 0;
        private int m_minRequiredPasswordLength = 0;
        private string m_passwordStrengthRegularExpression = string.Empty;

        public override string ApplicationName
        {
            get { return m_applicationName; }
            set { m_applicationName = value; }
        }

        public override bool EnablePasswordReset
        {
            get { return m_enablePasswordReset; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return m_enablePasswordRetrieval; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return m_requiresQuestionAndAnswer; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return m_requiresUniqueEmail; }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return m_maxInvalidPasswordAttempts; }
        }

        public override int PasswordAttemptWindow
        {
            get { return m_passwordAttemptWindow; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return m_passwordFormat; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return m_minRequiredNonAlphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return m_minRequiredPasswordLength; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return m_passwordStrengthRegularExpression; }
        }

        #endregion


        /// <summary>
        /// System.Web.Security.MembershipProvider methods.
        /// </summary>
        #region System.Web.Security.MembershipProvider methods

        /// <summary>
        /// MembershipProvider.ChangePassword
        /// </summary>
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (!ValidateUser(username, oldPassword))
                return false;

            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPassword, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                if (args.FailureInformation != null)
                    throw args.FailureInformation;
                else
                    throw new MembershipPasswordException(Properties.Resources.ErrPasswordChangeCanceled);
            }

            int rowsAffected = 0;

            using (NpgsqlConnection dbConn = new NpgsqlConnection(m_connectionString))
            {
                using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                {
                    dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "UPDATE \"{0}\" SET \"Password\" = @Password, \"LastPasswordChangedDate\" = @LastPasswordChangedDate WHERE \"Username\" = @Username AND  \"ApplicationName\" = @ApplicationName", s_tableName);

                    dbCommand.Parameters.Add("@Password", NpgsqlDbType.Varchar, 128).Value = EncodePassword(newPassword);
                    dbCommand.Parameters.Add("@LastPasswordChangedDate", NpgsqlDbType.TimestampTZ).Value = DateTime.Now;
                    dbCommand.Parameters.Add("@Username", NpgsqlDbType.Varchar, 255).Value = username;
                    dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;

                    try
                    {
                        dbConn.Open();
                        dbCommand.Prepare();

                        rowsAffected = dbCommand.ExecuteNonQuery();
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

            if (rowsAffected > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// MembershipProvider.ChangePasswordQuestionAndAnswer
        /// </summary>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            if (!ValidateUser(username, password))
                return false;

            int rowsAffected = 0;

            using (NpgsqlConnection dbConn = new NpgsqlConnection(m_connectionString))
            {
                using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                {
                    dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "UPDATE \"{0}\" SET \"PasswordQuestion\" = @PasswordQuestion, \"PasswordAnswer\" = @PasswordAnswer WHERE \"Username\" = @Username AND  \"ApplicationName\" = @ApplicationName", s_tableName);

                    dbCommand.Parameters.Add("@PasswordQuestion", NpgsqlDbType.Varchar, 255).Value = newPasswordQuestion;
                    dbCommand.Parameters.Add("@PasswordAnswer", NpgsqlDbType.Varchar, 255).Value = EncodePassword(newPasswordAnswer);
                    dbCommand.Parameters.Add("@Username", NpgsqlDbType.Varchar, 255).Value = username;
                    dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;

                    try
                    {
                        dbConn.Open();
                        dbCommand.Prepare();

                        rowsAffected = dbCommand.ExecuteNonQuery();
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

            if (rowsAffected > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// MembershipProvider.CreateUser
        /// </summary>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (RequiresUniqueEmail && string.IsNullOrEmpty(email))
            {
                status = MembershipCreateStatus.InvalidEmail;
                return null;
            }

            if (RequiresUniqueEmail && !string.IsNullOrEmpty(GetUserNameByEmail(email)))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            if (GetUser(username, false) == null)
            {
                DateTime createDate = DateTime.Now;

                if (providerUserKey == null)
                {
                    providerUserKey = Guid.NewGuid();
                }
                else
                {
                    if (!(providerUserKey is Guid))
                    {
                        status = MembershipCreateStatus.InvalidProviderUserKey;
                        return null;
                    }
                }

                // Create user in database
                using (NpgsqlConnection dbConn = new NpgsqlConnection(m_connectionString))
                {
                    using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                    {
                        dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "INSERT INTO \"{0}\" (\"pId\", \"Username\", \"Password\", \"Email\", \"PasswordQuestion\", \"PasswordAnswer\", \"IsApproved\", \"CreationDate\", \"LastPasswordChangedDate\", \"LastActivityDate\", \"ApplicationName\", \"IsLockedOut\", \"LastLockedOutDate\", \"FailedPasswordAttemptCount\", \"FailedPasswordAttemptWindowStart\", \"FailedPasswordAnswerAttemptCount\", \"FailedPasswordAnswerAttemptWindowStart\") Values (@pId, @Username, @Password, @Email, @PasswordQuestion, @PasswordAnswer, @IsApproved, @CreationDate, @LastPasswordChangedDate, @LastActivityDate, @ApplicationName, @IsLockedOut, @LastLockedOutDate, @FailedPasswordAttemptCount, @FailedPasswordAttemptWindowStart, @FailedPasswordAnswerAttemptCount, @FailedPasswordAnswerAttemptWindowStart)", s_tableName);

                        dbCommand.Parameters.Add("@pId", NpgsqlDbType.Uuid).Value = providerUserKey;
                        dbCommand.Parameters.Add("@Username", NpgsqlDbType.Varchar, 255).Value = username;
                        dbCommand.Parameters.Add("@Password", NpgsqlDbType.Varchar, 255).Value = EncodePassword(password);
                        dbCommand.Parameters.Add("@Email", NpgsqlDbType.Varchar, 128).Value = email;
                        dbCommand.Parameters.Add("@PasswordQuestion", NpgsqlDbType.Varchar, 255).Value = passwordQuestion;
                        dbCommand.Parameters.Add("@PasswordAnswer", NpgsqlDbType.Varchar, 255).Value = EncodePassword(passwordAnswer);
                        dbCommand.Parameters.Add("@IsApproved", NpgsqlDbType.Boolean).Value = isApproved;
                        dbCommand.Parameters.Add("@CreationDate", NpgsqlDbType.TimestampTZ).Value = createDate;
                        dbCommand.Parameters.Add("@LastPasswordChangedDate", NpgsqlDbType.TimestampTZ).Value = createDate;
                        dbCommand.Parameters.Add("@LastActivityDate", NpgsqlDbType.TimestampTZ).Value = createDate;
                        dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;
                        dbCommand.Parameters.Add("@IsLockedOut", NpgsqlDbType.Boolean).Value = false;
                        dbCommand.Parameters.Add("@LastLockedOutDate", NpgsqlDbType.TimestampTZ).Value = createDate;
                        dbCommand.Parameters.Add("@FailedPasswordAttemptCount", NpgsqlDbType.Integer).Value = 0;
                        dbCommand.Parameters.Add("@FailedPasswordAttemptWindowStart", NpgsqlDbType.TimestampTZ).Value = createDate;
                        dbCommand.Parameters.Add("@FailedPasswordAnswerAttemptCount", NpgsqlDbType.Integer).Value = 0;
                        dbCommand.Parameters.Add("@FailedPasswordAnswerAttemptWindowStart", NpgsqlDbType.TimestampTZ).Value = createDate;

                        try
                        {
                            dbConn.Open();
                            dbCommand.Prepare();

                            if (dbCommand.ExecuteNonQuery() > 0)
                            {
                                status = MembershipCreateStatus.Success;
                            }
                            else
                            {
                                status = MembershipCreateStatus.UserRejected;
                            }
                        }
                        catch (NpgsqlException e)
                        {
                            status = MembershipCreateStatus.ProviderError;
                            Trace.WriteLine(e.ToString());
                            throw new ProviderException(Properties.Resources.ErrOperationAborted);
                        }
                        finally
                        {
                            if (dbConn != null)
                                dbConn.Close();
                        }

                        return GetUser(username, false);
                    }
                }
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }
            return null;
        }

        /// <summary>
        /// MembershipProvider.DeleteUser
        /// </summary>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            int rowsAffected = 0;

            using (NpgsqlConnection dbConn = new NpgsqlConnection(m_connectionString))
            {
                using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                {
                    dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "DELETE FROM \"{0}\" WHERE \"Username\" = @Username AND  \"ApplicationName\" = @ApplicationName", s_tableName);

                    dbCommand.Parameters.Add("@Username", NpgsqlDbType.Varchar, 255).Value = username;
                    dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;

                    try
                    {
                        dbConn.Open();
                        dbCommand.Prepare();

                        rowsAffected = dbCommand.ExecuteNonQuery();

                        if (deleteAllRelatedData)
                        {
                            // Process commands to delete all data for the user in the database.
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

            if (rowsAffected > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// MembershipProvider.FindUsersByEmail
        /// </summary>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            MembershipUserCollection users = new MembershipUserCollection();

            if (string.IsNullOrEmpty(emailToMatch))
                return users;

            // replace permitted wildcard characters 
            emailToMatch = emailToMatch.Replace('*', '%');
            emailToMatch = emailToMatch.Replace('?', '_');

            using (NpgsqlConnection dbConn = new NpgsqlConnection(m_connectionString))
            {
                // Get user count
                using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                {
                    dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "SELECT Count(*) FROM \"{0}\" WHERE \"Email\" ILIKE @Email AND  \"ApplicationName\" = @ApplicationName", s_tableName);

                    dbCommand.Parameters.Add("@Email", NpgsqlDbType.Varchar, 128).Value = emailToMatch;
                    dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;

                    try
                    {
                        dbConn.Open();
                        dbCommand.Prepare();

                        if (!Int32.TryParse(dbCommand.ExecuteScalar().ToString(), out totalRecords))
                            return users;

                        if (totalRecords <= 0) { return users; }
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

                // Fetch user from database
                using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                {
                    dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "SELECT \"pId\", \"Username\", \"Email\", \"PasswordQuestion\", \"Comment\", \"IsApproved\", \"IsLockedOut\", \"CreationDate\", \"LastLoginDate\", \"LastActivityDate\", \"LastPasswordChangedDate\", \"LastLockedOutDate\" FROM \"{0}\" WHERE \"Email\" ILIKE @Email AND \"ApplicationName\" = @ApplicationName ORDER BY \"Username\" ASC LIMIT @MaxCount OFFSET @StartIndex", s_tableName);

                    dbCommand.Parameters.Add("@Email", NpgsqlDbType.Varchar, 128).Value = emailToMatch;
                    dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;
                    dbCommand.Parameters.Add("@MaxCount", NpgsqlDbType.Integer).Value = pageSize;
                    dbCommand.Parameters.Add("@StartIndex", NpgsqlDbType.Integer).Value = pageSize * pageIndex;

                    try
                    {
                        dbConn.Open();
                        dbCommand.Prepare();

                        using (NpgsqlDataReader reader = dbCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MembershipUser u = GetUserFromReader(reader);
                                users.Add(u);
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

            return users;
        }

        /// <summary>
        /// MembershipProvider.FindUsersByName
        /// </summary>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            MembershipUserCollection users = new MembershipUserCollection();

            // replace permitted wildcard characters 
            usernameToMatch = usernameToMatch.Replace('*', '%');
            usernameToMatch = usernameToMatch.Replace('?', '_');

            using (NpgsqlConnection dbConn = new NpgsqlConnection(m_connectionString))
            {
                // Get user count
                using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                {
                    dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "SELECT Count(*) FROM \"{0}\" WHERE \"Username\" ILIKE @Username AND  \"ApplicationName\" = @ApplicationName", s_tableName);

                    dbCommand.Parameters.Add("@Username", NpgsqlDbType.Varchar, 255).Value = usernameToMatch;
                    dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;

                    try
                    {
                        dbConn.Open();
                        dbCommand.Prepare();

                        if (!Int32.TryParse(dbCommand.ExecuteScalar().ToString(), out totalRecords))
                            return users;

                        if (totalRecords <= 0) { return users; }
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

                // Fetch user from database
                using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                {
                    dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "SELECT \"pId\", \"Username\", \"Email\", \"PasswordQuestion\", \"Comment\", \"IsApproved\", \"IsLockedOut\", \"CreationDate\", \"LastLoginDate\", \"LastActivityDate\", \"LastPasswordChangedDate\", \"LastLockedOutDate\" FROM \"{0}\" WHERE \"Username\" ILIKE @Username AND \"ApplicationName\" = @ApplicationName ORDER BY \"Username\" ASC LIMIT @MaxCount OFFSET @StartIndex", s_tableName);

                    dbCommand.Parameters.Add("@Username", NpgsqlDbType.Varchar, 255).Value = usernameToMatch;
                    dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;
                    dbCommand.Parameters.Add("@MaxCount", NpgsqlDbType.Integer).Value = pageSize;
                    dbCommand.Parameters.Add("@StartIndex", NpgsqlDbType.Integer).Value = pageSize * pageIndex;

                    try
                    {
                        dbConn.Open();
                        dbCommand.Prepare();

                        using (NpgsqlDataReader reader = dbCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MembershipUser u = GetUserFromReader(reader);
                                users.Add(u);
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

            return users;
        }

        /// <summary>
        /// MembershipProvider.GetAllUsers
        /// </summary>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            MembershipUserCollection users = new MembershipUserCollection();

            using (NpgsqlConnection dbConn = new NpgsqlConnection(m_connectionString))
            {
                // Get user count
                using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                {
                    dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "SELECT Count(*) FROM \"{0}\" WHERE \"ApplicationName\" = @ApplicationName", s_tableName);

                    dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;

                    try
                    {
                        dbConn.Open();
                        dbCommand.Prepare();

                        if (!Int32.TryParse(dbCommand.ExecuteScalar().ToString(), out totalRecords))
                            return users;

                        if (totalRecords <= 0) { return users; }
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

                // Fetch user from database
                using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                {
                    dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "SELECT \"pId\", \"Username\", \"Email\", \"PasswordQuestion\", \"Comment\", \"IsApproved\", \"IsLockedOut\", \"CreationDate\", \"LastLoginDate\", \"LastActivityDate\", \"LastPasswordChangedDate\", \"LastLockedOutDate\" FROM \"{0}\" WHERE \"ApplicationName\" = @ApplicationName ORDER BY \"Username\" ASC LIMIT @MaxCount OFFSET @StartIndex", s_tableName);

                    dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;
                    dbCommand.Parameters.Add("@MaxCount", NpgsqlDbType.Integer).Value = pageSize;
                    dbCommand.Parameters.Add("@StartIndex", NpgsqlDbType.Integer).Value = pageSize * pageIndex;

                    try
                    {
                        dbConn.Open();
                        dbCommand.Prepare();

                        using (NpgsqlDataReader reader = dbCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MembershipUser u = GetUserFromReader(reader);
                                users.Add(u);
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

            return users;
        }

        /// <summary>
        /// MembershipProvider.GetNumberOfUsersOnline
        /// </summary>
        public override int GetNumberOfUsersOnline()
        {
            int numOnline = 0;

            using (NpgsqlConnection dbConn = new NpgsqlConnection(m_connectionString))
            {
                using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                {
                    TimeSpan onlineSpan = new TimeSpan(0, System.Web.Security.Membership.UserIsOnlineTimeWindow, 0);
                    DateTime compareTime = DateTime.Now.Subtract(onlineSpan);

                    dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "SELECT Count(*) FROM \"{0}\" WHERE \"LastActivityDate\" > @CompareTime AND  \"ApplicationName\" = @ApplicationName", s_tableName);

                    dbCommand.Parameters.Add("@CompareTime", NpgsqlDbType.TimestampTZ, 255).Value = compareTime;
                    dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;

                    try
                    {
                        dbConn.Open();
                        dbCommand.Prepare();

                        numOnline = (int)dbCommand.ExecuteScalar();
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

            return numOnline;
        }

        /// <summary>
        /// MembershipProvider.GetPassword
        /// </summary>
        public override string GetPassword(string username, string answer)
        {
            if (!EnablePasswordRetrieval)
            {
                throw new ProviderException(Properties.Resources.ErrPasswordRetrievalNotEnabled);
            }

            if (PasswordFormat == MembershipPasswordFormat.Hashed)
            {
                throw new ProviderException(Properties.Resources.ErrCantRetrieveHashedPw);
            }

            using (NpgsqlConnection dbConn = new NpgsqlConnection(m_connectionString))
            {
                using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                {
                    dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "SELECT \"Password\", \"PasswordAnswer\", \"IsLockedOut\" FROM \"{0}\" WHERE \"Username\" = @Username AND \"ApplicationName\" = @ApplicationName", s_tableName);

                    dbCommand.Parameters.Add("@Username", NpgsqlDbType.Varchar, 255).Value = username;
                    dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;

                    try
                    {
                        dbConn.Open();
                        dbCommand.Prepare();

                        using (NpgsqlDataReader reader = dbCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();

                                string password = reader.GetString(0);
                                string passwordAnswer = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                                bool isLockedOut = reader.IsDBNull(2) ? false : reader.GetBoolean(2);

                                reader.Close();

                                if (isLockedOut)
                                    throw new MembershipPasswordException(Properties.Resources.ErrUserIsLoggedOut);

                                if (m_requiresQuestionAndAnswer && !CheckPassword(answer, passwordAnswer))
                                {
                                    UpdateFailureCount(username, FailureType.PasswordAnswer);

                                    throw new MembershipPasswordException(Properties.Resources.ErrIncorrectPasswordAnswer);
                                }

                                if (m_passwordFormat == MembershipPasswordFormat.Encrypted)
                                {
                                    password = UnEncodePassword(password);
                                }

                                return password;
                            }
                            else
                            {
                                throw new MembershipPasswordException(Properties.Resources.ErrUserNotFound);
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
        }

        /// <summary>
        /// MembershipProvider.GetUser
        /// </summary>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            MembershipUser u = null;

            using (NpgsqlConnection dbConn = new NpgsqlConnection(m_connectionString))
            {
                using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                {
                    dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "SELECT \"pId\", \"Username\", \"Email\", \"PasswordQuestion\", \"Comment\", \"IsApproved\", \"IsLockedOut\", \"CreationDate\", \"LastLoginDate\", \"LastActivityDate\", \"LastPasswordChangedDate\", \"LastLockedOutDate\" FROM \"{0}\" WHERE \"Username\" = @Username AND \"ApplicationName\" = @ApplicationName", s_tableName);

                    dbCommand.Parameters.Add("@Username", NpgsqlDbType.Varchar, 255).Value = username;
                    dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;

                    try
                    {
                        dbConn.Open();
                        dbCommand.Prepare();

                        using (NpgsqlDataReader reader = dbCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                u = GetUserFromReader(reader);
                                reader.Close();

                                if (userIsOnline)
                                {
                                    // Update user online status
                                    using (NpgsqlCommand dbUpdateCommand = dbConn.CreateCommand())
                                    {
                                        dbUpdateCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "UPDATE \"{0}\" SET \"LastActivityDate\" = @LastActivityDate WHERE \"pId\" = @pId", s_tableName);

                                        dbUpdateCommand.Parameters.Add("@LastActivityDate", NpgsqlDbType.TimestampTZ).Value = DateTime.Now;
                                        dbUpdateCommand.Parameters.Add("@pId", NpgsqlDbType.Uuid).Value = u.ProviderUserKey;

                                        dbUpdateCommand.Prepare();

                                        dbUpdateCommand.ExecuteNonQuery();
                                    }
                                }
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

            return u;
        }

        /// <summary>
        /// MembershipProvider.GetUser
        /// </summary>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            MembershipUser u = null;

            using (NpgsqlConnection dbConn = new NpgsqlConnection(m_connectionString))
            {
                using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                {
                    dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "SELECT \"pId\", \"Username\", \"Email\", \"PasswordQuestion\", \"Comment\", \"IsApproved\", \"IsLockedOut\", \"CreationDate\", \"LastLoginDate\", \"LastActivityDate\", \"LastPasswordChangedDate\", \"LastLockedOutDate\" FROM \"{0}\" WHERE \"pId\" = @pId", s_tableName);

                    dbCommand.Parameters.Add("@pId", NpgsqlDbType.Uuid).Value = providerUserKey;

                    try
                    {
                        dbConn.Open();
                        dbCommand.Prepare();

                        using (NpgsqlDataReader reader = dbCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                u = GetUserFromReader(reader);
                                reader.Close();

                                if (userIsOnline)
                                {
                                    // Update user online status
                                    using (NpgsqlCommand dbUpdateCommand = dbConn.CreateCommand())
                                    {
                                        dbUpdateCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "UPDATE \"{0}\" SET \"LastActivityDate\" = @LastActivityDate WHERE \"pId\" = @pId", s_tableName);

                                        dbUpdateCommand.Parameters.Add("@LastActivityDate", NpgsqlDbType.TimestampTZ).Value = DateTime.Now;
                                        dbUpdateCommand.Parameters.Add("@pId", NpgsqlDbType.Uuid).Value = u.ProviderUserKey;

                                        dbUpdateCommand.Prepare();

                                        dbUpdateCommand.ExecuteNonQuery();
                                    }
                                }
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

            return u;
        }

        /// <summary>
        /// MembershipProvider.GetUserNameByEmail
        /// </summary>
        public override string GetUserNameByEmail(string email)
        {
            string username = string.Empty;

            using (NpgsqlConnection dbConn = new NpgsqlConnection(m_connectionString))
            {
                using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                {
                    dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "SELECT \"Username\" FROM \"{0}\" WHERE \"Email\" = @Email AND \"ApplicationName\" = @ApplicationName", s_tableName);

                    dbCommand.Parameters.Add("@Email", NpgsqlDbType.Varchar, 128).Value = email;
                    dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;

                    try
                    {
                        dbConn.Open();
                        dbCommand.Prepare();

                        username = (dbCommand.ExecuteScalar() as string) ?? string.Empty;
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

            return username;
        }

        /// <summary>
        /// MembershipProvider.ResetPassword
        /// </summary>
        public override string ResetPassword(string username, string answer)
        {
            if (!m_enablePasswordReset)
            {
                throw new NotSupportedException(Properties.Resources.ErrPasswordResetNotEnabled);
            }

            if (string.IsNullOrEmpty(answer) && m_requiresQuestionAndAnswer)
            {
                UpdateFailureCount(username, FailureType.PasswordAnswer);

                throw new ProviderException(Properties.Resources.ErrPasswordAnswerRequired);
            }

            string newPassword = Membership.GeneratePassword(s_newPasswordLength, m_minRequiredNonAlphanumericCharacters);


            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPassword, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                if (args.FailureInformation != null)
                    throw args.FailureInformation;
                else
                    throw new MembershipPasswordException(Properties.Resources.ErrPasswordResetCanceled);
            }

            int rowsAffected = 0;

            using (NpgsqlConnection dbConn = new NpgsqlConnection(m_connectionString))
            {
                using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                {
                    dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "SELECT \"PasswordAnswer\", \"IsLockedOut\" FROM \"{0}\" WHERE \"Username\" = @Username AND \"ApplicationName\" = @ApplicationName", s_tableName);

                    dbCommand.Parameters.Add("@Username", NpgsqlDbType.Varchar, 255).Value = username;
                    dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;

                    try
                    {
                        string passwordAnswer = string.Empty;

                        dbConn.Open();
                        dbCommand.Prepare();

                        using (NpgsqlDataReader reader = dbCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();

                                passwordAnswer = reader.IsDBNull(0) ? string.Empty : reader.GetString(0); ;
                                bool isLockedOut = reader.IsDBNull(1) ? false : reader.GetBoolean(1);

                                reader.Close();

                                if (isLockedOut)
                                    throw new MembershipPasswordException(Properties.Resources.ErrUserIsLoggedOut);

                                if (m_requiresQuestionAndAnswer && !CheckPassword(answer, passwordAnswer))
                                {
                                    UpdateFailureCount(username, FailureType.PasswordAnswer);

                                    throw new MembershipPasswordException(Properties.Resources.ErrIncorrectPasswordAnswer);
                                }
                            }
                            else
                            {
                                throw new MembershipPasswordException(Properties.Resources.ErrUserNotFound);
                            }
                        }

                        // Reset Password
                        using (NpgsqlCommand dbUpdateCommand = dbConn.CreateCommand())
                        {
                            dbUpdateCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "UPDATE \"{0}\" SET \"Password\" = @Password, \"LastPasswordChangedDate\" = @LastPasswordChangedDate WHERE \"Username\" = @Username AND \"ApplicationName\" = @ApplicationName AND \"IsLockedOut\" = @IsLockedOut", s_tableName);

                            dbUpdateCommand.Parameters.Add("@Password", NpgsqlDbType.Varchar, 128).Value = EncodePassword(newPassword);
                            dbUpdateCommand.Parameters.Add("@LastPasswordChangedDate", NpgsqlDbType.TimestampTZ).Value = DateTime.Now;
                            dbUpdateCommand.Parameters.Add("@Username", NpgsqlDbType.Varchar, 255).Value = username;
                            dbUpdateCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;
                            dbUpdateCommand.Parameters.Add("@IsLockedOut", NpgsqlDbType.Boolean).Value = false;

                            dbUpdateCommand.Prepare();

                            rowsAffected = dbUpdateCommand.ExecuteNonQuery();
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

            if (rowsAffected > 0)
                return newPassword;

            else
                throw new MembershipPasswordException(Properties.Resources.ErrPasswordResetAborted);
        }

        /// <summary>
        /// MembershipProvider.UnlockUser
        /// </summary>
        public override bool UnlockUser(string userName)
        {
            int rowsAffected = 0;

            using (NpgsqlConnection dbConn = new NpgsqlConnection(m_connectionString))
            {
                using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                {
                    dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "UPDATE  \"{0}\" SET \"IsLockedOut\" = @IsLockedOut, \"LastLockedOutDate\" = @LastLockedOutDate WHERE \"Username\" = @Username AND \"ApplicationName\" = @ApplicationName", s_tableName);

                    dbCommand.Parameters.Add("@IsLockedOut", NpgsqlDbType.Boolean).Value = false;
                    dbCommand.Parameters.Add("@LastLockedOutDate", NpgsqlDbType.TimestampTZ).Value = DateTime.Now;
                    dbCommand.Parameters.Add("@Username", NpgsqlDbType.Varchar, 255).Value = userName;
                    dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;

                    try
                    {
                        dbConn.Open();
                        dbCommand.Prepare();

                        rowsAffected = dbCommand.ExecuteNonQuery();
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

            if (rowsAffected > 0)
                return true;

            else
                return false;
        }

        /// <summary>
        /// MembershipProvider.UpdateUser
        /// </summary>
        public override void UpdateUser(MembershipUser user)
        {
            if (RequiresUniqueEmail)
            {
                if (string.IsNullOrEmpty(user.Email))
                    throw new ProviderException("Email address required.");
                string foundUsername = GetUserNameByEmail(user.Email);
                if (!string.IsNullOrEmpty(foundUsername) &&
                    foundUsername != user.UserName)
                    throw new ProviderException("Duplicate E-mail address. The E-mail supplied is invalid.");
            }

            using (NpgsqlConnection dbConn = new NpgsqlConnection(m_connectionString))
            {
                using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                {
                    dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "UPDATE  \"{0}\" SET \"Email\" = @Email, \"Comment\" = @Comment, \"IsApproved\" = @IsApproved WHERE \"Username\" = @Username AND \"ApplicationName\" = @ApplicationName", s_tableName);

                    dbCommand.Parameters.Add("@Email", NpgsqlDbType.Varchar, 128).Value = user.Email;
                    dbCommand.Parameters.Add("@Comment", NpgsqlDbType.Varchar, 255).Value = user.Comment;
                    dbCommand.Parameters.Add("@IsApproved", NpgsqlDbType.Boolean).Value = user.IsApproved;
                    dbCommand.Parameters.Add("@Username", NpgsqlDbType.Varchar, 255).Value = user.UserName;
                    dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;

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

        /// <summary>
        /// MembershipProvider.ValidateUser
        /// </summary>
        public override bool ValidateUser(string username, string password)
        {
            string dbPassword = string.Empty;
            bool dbIsApproved = false;

            using (NpgsqlConnection dbConn = new NpgsqlConnection(m_connectionString))
            {
                // Fetch user data from database
                using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                {
                    dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "SELECT \"Password\", \"IsApproved\" FROM \"{0}\" WHERE \"Username\" = @Username AND \"ApplicationName\" = @ApplicationName AND \"IsLockedOut\" = @IsLockedOut", s_tableName);

                    dbCommand.Parameters.Add("@Username", NpgsqlDbType.Varchar, 255).Value = username;
                    dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;
                    dbCommand.Parameters.Add("@IsLockedOut", NpgsqlDbType.Boolean).Value = false;

                    try
                    {
                        dbConn.Open();
                        dbCommand.Prepare();

                        using (NpgsqlDataReader reader = dbCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                dbPassword = reader.GetString(0);
                                dbIsApproved = reader.IsDBNull(1) ? false : reader.GetBoolean(1);
                            }
                            else
                            {
                                return false;
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

                if (CheckPassword(password, dbPassword))
                {
                    if (dbIsApproved)
                    {
                        // Update last login date
                        using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                        {
                            dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "UPDATE \"{0}\" SET \"LastLoginDate\" = @LastLoginDate WHERE \"Username\" = @Username AND \"ApplicationName\" = @ApplicationName", s_tableName);

                            dbCommand.Parameters.Add("@LastLoginDate", NpgsqlDbType.TimestampTZ).Value = DateTime.Now;
                            dbCommand.Parameters.Add("@Username", NpgsqlDbType.Varchar, 255).Value = username;
                            dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;

                            try
                            {
                                dbConn.Open();
                                dbCommand.Prepare();

                                dbCommand.ExecuteNonQuery();

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
                }

                return false;
            }
        }
        #endregion

        #region private methods
        /// <summary>
        /// A helper function to retrieve config values from the configuration file.
        /// </summary>
        /// <param name="configValue"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        internal static string GetConfigValue(string configValue, string defaultValue)
        {
            if (string.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

        /// <summary>
        /// A helper function to retrieve the connecion string from the configuration file
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string</param>
        /// <returns></returns>
        internal static string GetConnectionString(string connectionStringName)
        {
            if (string.IsNullOrEmpty(connectionStringName))
                throw new ArgumentException(Properties.Resources.ErrArgumentNullOrEmpty, "connectionStringName");

            ConnectionStringSettings ConnectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];

            if (ConnectionStringSettings == null || string.IsNullOrEmpty(ConnectionStringSettings.ConnectionString.Trim()))
                throw new ProviderException(Properties.Resources.ErrConnectionStringNullOrEmpty);

            return ConnectionStringSettings.ConnectionString;
        }

        /// <summary>
        /// A helper function that takes the current row from the NpgsqlDataReader
        /// and hydrates a MembershipUser from the values. Called by the 
        /// MembershipUser.GetUser implementation.
        /// </summary>
        /// <param name="reader">NpgsqlDataReader object</param>
        /// <returns>MembershipUser object</returns>
        private MembershipUser GetUserFromReader(NpgsqlDataReader reader)
        {
            object providerUserKey = reader.GetValue(0);
            string username = reader.GetString(1);
            string email = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
            string passwordQuestion = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
            string comment = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
            bool isApproved = reader.IsDBNull(5) ? false : reader.GetBoolean(5);
            bool isLockedOut = reader.IsDBNull(6) ? false : reader.GetBoolean(6);
            DateTime creationDate = reader.IsDBNull(7) ? DateTime.MinValue : reader.GetDateTime(7);
            DateTime lastLoginDate = reader.IsDBNull(8) ? DateTime.MinValue : reader.GetDateTime(8);
            DateTime lastActivityDate = reader.IsDBNull(9) ? DateTime.MinValue : reader.GetDateTime(9);
            DateTime lastPasswordChangedDate = reader.IsDBNull(10) ? DateTime.MinValue : reader.GetDateTime(10);
            DateTime lastLockedOutDate = reader.IsDBNull(11) ? DateTime.MinValue : reader.GetDateTime(11);

            return new MembershipUser(this.Name,
                                                  username,
                                                  providerUserKey,
                                                  email,
                                                  passwordQuestion,
                                                  comment,
                                                  isApproved,
                                                  isLockedOut,
                                                  creationDate,
                                                  lastLoginDate,
                                                  lastActivityDate,
                                                  lastPasswordChangedDate,
                                                  lastLockedOutDate);
        }

        /// <summary>
        /// Compares password values based on the MembershipPasswordFormat.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="dbpassword"></param>
        /// <returns></returns>
        private bool CheckPassword(string password, string dbpassword)
        {
            string pass1 = password;
            string pass2 = dbpassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Encrypted:
                    pass2 = UnEncodePassword(dbpassword);
                    break;

                case MembershipPasswordFormat.Hashed:
                    pass1 = EncodePassword(password);
                    break;

                default:
                    break;
            }

            if (pass1.Equals(pass2))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Encrypts, Hashes, or leaves the password clear based on the PasswordFormat.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string EncodePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return password;

            string encodedPassword = password;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;

                case MembershipPasswordFormat.Encrypted:
                    encodedPassword = Convert.ToBase64String(EncryptPassword(Encoding.Unicode.GetBytes(password)));
                    break;

                case MembershipPasswordFormat.Hashed:
                    HMACSHA1 hash = new HMACSHA1();
                    hash.Key = HexToByte(m_machineKeyConfig.ValidationKey);
                    encodedPassword = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                    break;

                default:
                    throw new ProviderException(Properties.Resources.ErrPwFormatNotSupported);
            }

            return encodedPassword;
        }

        /// <summary>
        /// Decrypts or leaves the password clear based on the PasswordFormat.
        /// </summary>
        /// <param name="encodedPassword"></param>
        /// <returns></returns>
        private string UnEncodePassword(string encodedPassword)
        {
            string password = encodedPassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;

                case MembershipPasswordFormat.Encrypted:
                    password = Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)));
                    break;

                case MembershipPasswordFormat.Hashed:
                    throw new ProviderException(Properties.Resources.ErrCantDecodeHashedPw);

                default:
                    throw new ProviderException(Properties.Resources.ErrPwFormatNotSupported);
            }

            return password;
        }

        /// <summary>
        /// Converts a hexadecimal string to a byte array. Used to convert encryption
        /// key values from the configuration.
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private static byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

            return returnBytes;
        }

        /// <summary>
        /// A helper method that performs the checks and updates associated with
        /// password failure tracking.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="failType"></param>
        private void UpdateFailureCount(string username, FailureType failType)
        {
            DateTime windowStart = new DateTime();
            int failureCount = 0;

            using (NpgsqlConnection dbConn = new NpgsqlConnection(m_connectionString))
            {
                // Fetch user data from database
                using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                {
                    dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "SELECT \"FailedPasswordAttemptCount\", \"FailedPasswordAttemptWindowStart\", \"FailedPasswordAnswerAttemptCount\", \"FailedPasswordAnswerAttemptWindowStart\" FROM \"{0}\" WHERE \"Username\" = @Username AND \"ApplicationName\" = @ApplicationName", s_tableName);

                    dbCommand.Parameters.Add("@Username", NpgsqlDbType.Varchar, 255).Value = username;
                    dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;

                    try
                    {
                        dbConn.Open();
                        dbCommand.Prepare();

                        using (NpgsqlDataReader reader = dbCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();

                                if (failType.Equals(FailureType.Password))
                                {
                                    failureCount = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                    windowStart = reader.IsDBNull(1) ? DateTime.MinValue : reader.GetDateTime(1);
                                }
                                else if (failType.Equals(FailureType.PasswordAnswer))
                                {
                                    failureCount = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                                    windowStart = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3);
                                }
                            }
                        }
                    }
                    catch (NpgsqlException e)
                    {
                        Trace.WriteLine(e.ToString());

                        if (dbConn != null)
                            dbConn.Close();

                        throw new ProviderException(Properties.Resources.ErrOperationAborted);
                    }
                }

                // Calculate failture count and update database
                using (NpgsqlCommand dbCommand = dbConn.CreateCommand())
                {
                    DateTime windowEnd = windowStart.AddMinutes(m_passwordAttemptWindow);

                    try
                    {
                        if (failureCount == 0 || DateTime.Now > windowEnd)
                        {
                            // First password failure or outside of PasswordAttemptWindow. 
                            // Start a new password failure count from 1 and a new window starting now.

                            if (failType.Equals(FailureType.Password))
                            {
                                dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "UPDATE \"{0}\" SET \"FailedPasswordAttemptCount\" = @Count, \"FailedPasswordAttemptWindowStart\" = @WindowStart WHERE \"Username\" = @Username AND \"ApplicationName\" = @ApplicationName", s_tableName);
                            }
                            else if (failType.Equals(FailureType.PasswordAnswer))
                            {
                                dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "UPDATE \"{0}\" SET \"FailedPasswordAnswerAttemptCount\" = @Count, \"FailedPasswordAnswerAttemptWindowStart\" = @WindowStart WHERE \"Username\" = @Username AND \"ApplicationName\" = @ApplicationName", s_tableName);
                            }

                            dbCommand.Parameters.Add("@Count", NpgsqlDbType.Integer).Value = 1;
                            dbCommand.Parameters.Add("@WindowStart", NpgsqlDbType.TimestampTZ).Value = DateTime.Now;
                            dbCommand.Parameters.Add("@Username", NpgsqlDbType.Varchar, 255).Value = username;
                            dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;

                            if (dbCommand.ExecuteNonQuery() < 0)
                                throw new ProviderException(Properties.Resources.ErrCantUpdateFailtureCountAndWindowStart);
                        }
                        else
                        {
                            failureCount++;

                            if (failureCount >= m_maxInvalidPasswordAttempts)
                            {
                                // Password attempts have exceeded the failure threshold. Lock out
                                // the user.
                                dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "UPDATE \"{0}\" SET \"IsLockedOut\" = @IsLockedOut, \"LastLockedOutDate\" = @LastLockedOutDate WHERE \"Username\" = @Username AND \"ApplicationName\" = @ApplicationName", s_tableName);

                                dbCommand.Parameters.Add("@IsLockedOut", NpgsqlDbType.Boolean).Value = true;
                                dbCommand.Parameters.Add("@LastLockedOutDate", NpgsqlDbType.TimestampTZ).Value = DateTime.Now;
                                dbCommand.Parameters.Add("@Username", NpgsqlDbType.Varchar, 255).Value = username;
                                dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;

                                if (dbCommand.ExecuteNonQuery() < 0)
                                    throw new ProviderException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.ErrCantLogoutUser, username));
                            }
                            else
                            {
                                // Password attempts have not exceeded the failure threshold. Update
                                // the failure counts. Leave the window the same.
                                if (failType.Equals(FailureType.Password))
                                {
                                    dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "UPDATE \"{0}\" SET \"FailedPasswordAttemptCount\" = @Count WHERE \"Username\" = @Username AND \"ApplicationName\" = @ApplicationName", s_tableName);
                                }
                                else if (failType.Equals(FailureType.PasswordAnswer))
                                {
                                    dbCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "UPDATE \"{0}\" SET \"FailedPasswordAnswerAttemptCount\" = @Count WHERE \"Username\" = @Username AND \"ApplicationName\" = @ApplicationName", s_tableName);
                                }

                                dbCommand.Parameters.Add("@Count", NpgsqlDbType.Integer).Value = failureCount;
                                dbCommand.Parameters.Add("@Username", NpgsqlDbType.Varchar, 255).Value = username;
                                dbCommand.Parameters.Add("@ApplicationName", NpgsqlDbType.Varchar, 255).Value = m_applicationName;

                                if (dbCommand.ExecuteNonQuery() < 0)
                                    throw new ProviderException(Properties.Resources.ErrCantUpdateFailtureCount);
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
        }

        private enum FailureType
        {
            Password,
            PasswordAnswer
        }
        #endregion
    }
}
