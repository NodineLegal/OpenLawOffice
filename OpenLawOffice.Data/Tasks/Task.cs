// -----------------------------------------------------------------------
// <copyright file="Task.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using AutoMapper;
    using Dapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Task
    {
        public static Common.Models.Tasks.Task Get(long id)
        {
            return DataHelper.Get<Common.Models.Tasks.Task, DBOs.Tasks.Task>(
                "SELECT * FROM \"task\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static List<Common.Models.Tasks.Task> List()
        {
            return DataHelper.List<Common.Models.Tasks.Task, DBOs.Tasks.Task>(
                "SELECT * FROM \"task\" WHERE \"utc_disabled\" is null");
        }

        public static List<Common.Models.Tasks.Task> ListAllTasksForContact(int contactId)
        {
            List<Common.Models.Tasks.Task> list = new List<Common.Models.Tasks.Task>();
            IEnumerable<DBOs.Tasks.Task> ie = null;
            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                ie = conn.Query<DBOs.Tasks.Task>(
                    "SELECT \"task\".* FROM \"task\" WHERE \"task\".\"utc_disabled\" is null  " +
                    "AND \"task\".\"id\" IN (SELECT \"task_id\" FROM \"task_assigned_contact\" WHERE \"contact_id\"=@ContactId)",
                    new { ContactId = contactId });
            }

            foreach (DBOs.Tasks.Task dbo in ie)
                list.Add(Mapper.Map<Common.Models.Tasks.Task>(dbo));

            return list;
        }

        public static List<Common.Models.Tasks.Task> ListForMatter(Guid matterId)
        {
            /* Standard Tasks are neither hierarchical or sequenced - not is_grouping_task, no parent_id
             *  We do not need to test for sequential followers as we can infer this from the fact that a
             *  parent_id must be set for any sequential member.  This is a design mechanism.
             *
             * Grouping Tasks simply contain other tasks and their task properties should be caclulated, not stored.
             *  Grouping tasks contain standard tasks, sequenced tasks and other grouping tasks.
             *  However, a grouping task containing a sequence may only contain members of the sequence,
             *  but nothing prevents a sequence member from being a grouping task.
             *
             * Sequenced Tasks are members of a sequence and may be either standard tasks or
             *  grouping tasks.
             *
             * Therefore, when loading the root tasks of a matter, we load standard and
             * grouping tasks.  Note, this does not include loading of any task with a non-null
             * parent_id as that means they are grouped and therefore, not root.  However, this
             * also means that we may simply load tasks with null parent_id fields to select
             * all the root tasks.
             *
             *
             * Example:
             * ST1
             * ST2
             * GT1
             *  Seq1
             *  Seq2-GT
             *   ST3
             *   ST4
             *  Seq3
             *   GT2
             *    GT3
             *     Seq4
             *     Seq5
             *     Seq6
             *    ST5
             *    ST6
             *   ST7
             *  Seq4
             *  Seq5
             * ST8
             *
             */
            return DataHelper.List<Common.Models.Tasks.Task, DBOs.Tasks.Task>(
                "SELECT * FROM \"task\" WHERE \"parent_id\" is null AND " +
                "\"id\" in (SELECT \"task_id\" FROM \"task_matter\" WHERE \"matter_id\"=@MatterId) AND " +
                "\"utc_disabled\" is null",
                new { MatterId = matterId });
        }

        public static List<Common.Models.Tasks.Task> GetTodoListFor(Common.Models.Security.User user, List<Common.Models.Settings.TagFilter> tagFilter, DateTime? start = null, DateTime? stop = null)
        {
            string sql;

            List<string> cats = new List<string>();
            List<string> tags = new List<string>();
            List<Npgsql.NpgsqlParameter> parms = new List<Npgsql.NpgsqlParameter>();
            List<Common.Models.Tasks.Task> list = new List<Common.Models.Tasks.Task>();

            tagFilter.ForEach(x =>
            {
                if (!string.IsNullOrWhiteSpace(x.Category))
                    cats.Add(x.Category.ToLower());
                if (!string.IsNullOrWhiteSpace(x.Tag))
                    tags.Add(x.Tag.ToLower());
            });

            sql = "SELECT * FROM \"task\" WHERE \"id\" IN (SELECT \"task_id\" FROM \"task_responsible_user\" WHERE \"user_id\"=@UserId) " +
                "AND \"id\" IN (SELECT \"task_id\" FROM \"task_tag\" WHERE \"tag_category_id\" " +
                "IN (SELECT \"id\" FROM \"tag_category\" WHERE LOWER(\"name\") IN (";

            cats.ForEach(x =>
            {
                string parmName = parms.Count.ToString();
                parms.Add(new Npgsql.NpgsqlParameter(parmName, NpgsqlTypes.NpgsqlDbType.Text) { Value = x });
                sql += ":" + parmName + ",";
            });

            sql = sql.TrimEnd(',');
            sql += ")) AND LOWER(\"tag\") IN (";

            tags.ForEach(x =>
            {
                string parmName = parms.Count.ToString();
                parms.Add(new Npgsql.NpgsqlParameter(parmName, NpgsqlTypes.NpgsqlDbType.Text) { Value = x });
                sql += ":" + parmName + ",";
            });

            sql = sql.TrimEnd(',');
            sql += ")) AND \"utc_disabled\" is null ";

            if (start.HasValue)
            {
                parms.Add(new Npgsql.NpgsqlParameter("Start", DbType.DateTime) { Value = start.Value });
                if (stop.HasValue)
                {
                    sql += "AND \"due_date\" BETWEEN @Start AND @Stop ";
                    parms.Add(new Npgsql.NpgsqlParameter("Stop", DbType.DateTime) { Value = stop.Value });
                }
                else
                {
                    sql += "AND \"due_date\">=@Start ";
                }
            }

            sql += "ORDER BY \"due_date\" ASC";

            using (Npgsql.NpgsqlConnection conn = (Npgsql.NpgsqlConnection)Database.Instance.GetConnection())
            {
                conn.Open();
                using (Npgsql.NpgsqlCommand cmd = new Npgsql.NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(new Npgsql.NpgsqlParameter("UserId", DbType.Int32) { Value = user.Id.Value });
                    parms.ForEach(x => cmd.Parameters.Add(x));
                    using (Npgsql.NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DBOs.Tasks.Task dbo = new DBOs.Tasks.Task();

                            dbo.Id = Database.GetDbColumnValue<long>("id", reader);
                            dbo.Title = Database.GetDbColumnValue<string>("title", reader);
                            dbo.Description = Database.GetDbColumnValue<string>("description", reader);
                            dbo.ProjectedStart = Database.GetDbColumnValue<DateTime?>("projected_start", reader);
                            dbo.DueDate = Database.GetDbColumnValue<DateTime?>("due_date", reader);
                            dbo.ProjectedEnd = Database.GetDbColumnValue<DateTime?>("projected_end", reader);
                            dbo.ActualEnd = Database.GetDbColumnValue<DateTime?>("actual_end", reader);
                            dbo.ParentId = Database.GetDbColumnValue<long?>("parent_id", reader);
                            dbo.IsGroupingTask = Database.GetDbColumnValue<bool>("is_grouping_task", reader);
                            dbo.SequentialPredecessorId = Database.GetDbColumnValue<long?>("sequential_predecessor_id", reader);

                            list.Add(Mapper.Map<Common.Models.Tasks.Task>(dbo));
                        }
                    }
                }
            }

            return list;
        }

        public static List<Common.Models.Tasks.Task> GetTodoListFor(Common.Models.Contacts.Contact contact, List<Common.Models.Settings.TagFilter> tagFilter, DateTime? start = null, DateTime? stop = null)
        {
            string sql;

            List<string> cats = new List<string>();
            List<string> tags = new List<string>();
            List<Npgsql.NpgsqlParameter> parms = new List<Npgsql.NpgsqlParameter>();
            List<Common.Models.Tasks.Task> list = new List<Common.Models.Tasks.Task>();

            tagFilter.ForEach(x =>
            {
                if (!string.IsNullOrWhiteSpace(x.Category))
                    cats.Add(x.Category.ToLower());
                if (!string.IsNullOrWhiteSpace(x.Tag))
                    tags.Add(x.Tag.ToLower());
            });

            sql = "SELECT * FROM \"task\" WHERE \"id\" IN (SELECT \"task_id\" FROM \"task_assigned_contact\" WHERE \"contact_id\"=@ContactId) " +
                "AND \"id\" IN (SELECT \"task_id\" FROM \"task_tag\" WHERE \"tag_category_id\" " +
                "IN (SELECT \"id\" FROM \"tag_category\" WHERE LOWER(\"name\") IN (";

            cats.ForEach(x =>
            {
                string parmName = parms.Count.ToString();
                parms.Add(new Npgsql.NpgsqlParameter(parmName, NpgsqlTypes.NpgsqlDbType.Text) { Value = x });
                sql += ":" + parmName + ",";
            });

            sql = sql.TrimEnd(',');
            sql += ")) AND LOWER(\"tag\") IN (";

            tags.ForEach(x =>
            {
                string parmName = parms.Count.ToString();
                parms.Add(new Npgsql.NpgsqlParameter(parmName, NpgsqlTypes.NpgsqlDbType.Text) { Value = x });
                sql += ":" + parmName + ",";
            });

            sql = sql.TrimEnd(',');
            sql += ")) AND \"utc_disabled\" is null ";

            if (start.HasValue)
            {
                parms.Add(new Npgsql.NpgsqlParameter("Start", DbType.DateTime) { Value = start.Value });
                if (stop.HasValue)
                {
                    sql += "AND \"due_date\" BETWEEN @Start AND @Stop ";
                    parms.Add(new Npgsql.NpgsqlParameter("Stop", DbType.DateTime) { Value = stop.Value });
                }
                else
                {
                    sql += "AND \"due_date\">=@Start ";
                }
            }

            sql += "ORDER BY \"due_date\" ASC";

            using (Npgsql.NpgsqlConnection conn = (Npgsql.NpgsqlConnection)Database.Instance.GetConnection())
            {
                conn.Open();
                using (Npgsql.NpgsqlCommand cmd = new Npgsql.NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(new Npgsql.NpgsqlParameter("ContactId", DbType.Int32) { Value = contact.Id.Value });
                    parms.ForEach(x => cmd.Parameters.Add(x));
                    using (Npgsql.NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DBOs.Tasks.Task dbo = new DBOs.Tasks.Task();

                            dbo.Id = Database.GetDbColumnValue<long>("id", reader);
                            dbo.Title = Database.GetDbColumnValue<string>("title", reader);
                            dbo.Description = Database.GetDbColumnValue<string>("description", reader);
                            dbo.ProjectedStart = Database.GetDbColumnValue<DateTime?>("projected_start", reader);
                            dbo.DueDate = Database.GetDbColumnValue<DateTime?>("due_date", reader);
                            dbo.ProjectedEnd = Database.GetDbColumnValue<DateTime?>("projected_end", reader);
                            dbo.ActualEnd = Database.GetDbColumnValue<DateTime?>("actual_end", reader);
                            dbo.ParentId = Database.GetDbColumnValue<long?>("parent_id", reader);
                            dbo.IsGroupingTask = Database.GetDbColumnValue<bool>("is_grouping_task", reader);
                            dbo.SequentialPredecessorId = Database.GetDbColumnValue<long?>("sequential_predecessor_id", reader);

                            list.Add(Mapper.Map<Common.Models.Tasks.Task>(dbo));
                        }
                    }
                }
            }

            return list;
        }

        public static List<Common.Models.Tasks.Task> ListChildren(long? parentId, List<Tuple<string, string>> filter = null)
        {
            List<Common.Models.Tasks.Task> list = new List<Common.Models.Tasks.Task>();
            IEnumerable<DBOs.Tasks.Task> ie = null;

            //filter = new List<Tuple<string, string>>();
            //filter.Add(new Tuple<string, string>("status", "pending"));

            if (filter != null)
            {
                string filterStr = null;

                List<string> cats = new List<string>();
                List<string> tags = new List<string>();
                List<Npgsql.NpgsqlParameter> parms = new List<Npgsql.NpgsqlParameter>();

                filter.ForEach(x =>
                {
                    if (!string.IsNullOrWhiteSpace(x.Item1))
                        cats.Add(x.Item1.ToLower());
                    if (!string.IsNullOrWhiteSpace(x.Item2))
                        tags.Add(x.Item2.ToLower());
                });

                filterStr = "SELECT * FROM \"task\" WHERE \"id\" IN (SELECT \"task_id\" FROM \"task_tag\" WHERE \"tag_category_id\" " +
                    "IN (SELECT \"id\" FROM \"tag_category\" WHERE LOWER(\"name\") IN (";

                cats.ForEach(x =>
                {
                    string parmName = parms.Count.ToString();
                    parms.Add(new Npgsql.NpgsqlParameter(parmName, NpgsqlTypes.NpgsqlDbType.Text) { Value = x });
                    filterStr += ":" + parmName + ",";
                });

                filterStr = filterStr.TrimEnd(',');
                filterStr += ")) AND LOWER(\"tag\") IN (";

                tags.ForEach(x =>
                {
                    string parmName = parms.Count.ToString();
                    parms.Add(new Npgsql.NpgsqlParameter(parmName, NpgsqlTypes.NpgsqlDbType.Text) { Value = x });
                    filterStr += ":" + parmName + ",";
                });

                filterStr = filterStr.TrimEnd(',');
                filterStr += ")) AND \"parent_id\"";

                if (parentId.HasValue && parentId.Value > 0)
                {
                    filterStr += "=:parentid ";
                    parms.Add(new Npgsql.NpgsqlParameter("parentid", DbType.Int64) { Value = parentId.Value.ToString() });
                }
                else
                    filterStr += " is null ";

                filterStr += "AND \"utc_disabled\" is null";

                using (Npgsql.NpgsqlConnection conn = (Npgsql.NpgsqlConnection)Database.Instance.GetConnection())
                {
                    conn.Open();
                    using (Npgsql.NpgsqlCommand cmd = new Npgsql.NpgsqlCommand(filterStr, conn))
                    {
                        parms.ForEach(x => cmd.Parameters.Add(x));
                        using (Npgsql.NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DBOs.Tasks.Task dbo = new DBOs.Tasks.Task();

                                dbo.Id = Database.GetDbColumnValue<long>("id", reader);
                                dbo.Title = Database.GetDbColumnValue<string>("title", reader);
                                dbo.Description = Database.GetDbColumnValue<string>("description", reader);
                                dbo.ProjectedStart = Database.GetDbColumnValue<DateTime?>("projected_start", reader);
                                dbo.DueDate = Database.GetDbColumnValue<DateTime?>("due_date", reader);
                                dbo.ProjectedEnd = Database.GetDbColumnValue<DateTime?>("projected_end", reader);
                                dbo.ActualEnd = Database.GetDbColumnValue<DateTime?>("actual_end", reader);
                                dbo.ParentId = Database.GetDbColumnValue<long?>("parent_id", reader);
                                dbo.IsGroupingTask = Database.GetDbColumnValue<bool>("is_grouping_task", reader);
                                dbo.SequentialPredecessorId = Database.GetDbColumnValue<long?>("sequential_predecessor_id", reader);

                                list.Add(Mapper.Map<Common.Models.Tasks.Task>(dbo));
                            }
                        }
                    }
                }
            }
            else
            {
                using (IDbConnection conn = Database.Instance.GetConnection())
                {
                    if (parentId.HasValue)
                        ie = conn.Query<DBOs.Tasks.Task>(
                            "SELECT * FROM \"task\" WHERE \"parent_id\"=@ParentId AND \"utc_disabled\" is null",
                            new { ParentId = parentId.Value });
                    else
                        ie = conn.Query<DBOs.Tasks.Task>(
                            "SELECT * FROM \"task\" WHERE \"parent_id\" is null AND \"utc_disabled\" is null");
                }

                foreach (DBOs.Tasks.Task dbo in ie)
                    list.Add(Mapper.Map<Common.Models.Tasks.Task>(dbo));
            }

            return list;
        }

        public static Common.Models.Tasks.Task GetTaskForWhichIAmTheSequentialPredecessor(long id)
        {
            return DataHelper.Get<Common.Models.Tasks.Task, DBOs.Tasks.Task>(
                "SELECT * FROM \"task\" WHERE \"sequential_predecessor_id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static Common.Models.Matters.Matter GetRelatedMatter(long taskId)
        {
            return DataHelper.Get<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT \"matter\".* FROM \"matter\" JOIN \"task_matter\" ON \"matter\".\"id\"=\"task_matter\".\"matter_id\" " +
                "WHERE \"task_id\"=@TaskId AND \"matter\".\"utc_disabled\" is null AND \"task_matter\".\"utc_disabled\" is null",
                new { TaskId = taskId });
        }

        public static Common.Models.Tasks.Task Create(Common.Models.Tasks.Task model,
            Common.Models.Security.User creator)
        {
            model.CreatedBy = model.ModifiedBy = creator;
            model.UtcCreated = model.UtcModified = DateTime.UtcNow;
            DBOs.Tasks.Task dbo = Mapper.Map<DBOs.Tasks.Task>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"task\" (\"title\", \"description\", \"projected_start\", \"due_date\", \"projected_end\", \"actual_end\", \"parent_id\", \"is_grouping_task\", \"sequential_predecessor_id\", \"utc_created\", \"utc_modified\", \"created_by_user_id\", \"modified_by_user_id\") " +
                    "VALUES (@Title, @Description, @ProjectedStart, @DueDate, @ProjectedEnd, @ActualEnd, @ParentId, @IsGroupingTask, @SequentialPredecessorId, @UtcCreated, @UtcModified, @CreatedByUserId, @ModifiedByUserId)",
                    dbo);
                model.Id = conn.Query<DBOs.Security.Area>("SELECT currval(pg_get_serial_sequence('task', 'id')) AS \"id\"").Single().Id;
            }

            return model;
        }

        public static Common.Models.Tasks.TaskMatter RelateMatter(Common.Models.Tasks.Task taskModel,
            Guid matterId, Common.Models.Security.User creator)
        {
            Common.Models.Tasks.TaskMatter taskMatter = new Common.Models.Tasks.TaskMatter()
            {
                Id = Guid.NewGuid(),
                Matter = new Common.Models.Matters.Matter() { Id = matterId },
                Task = taskModel,
                UtcCreated = DateTime.UtcNow,
                UtcModified = DateTime.UtcNow,
                CreatedBy = creator,
                ModifiedBy = creator
            };

            DBOs.Tasks.TaskMatter dbo = Mapper.Map<DBOs.Tasks.TaskMatter>(taskMatter);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"task_matter\" (\"id\", \"task_id\", \"matter_id\", \"utc_created\", \"utc_modified\", \"created_by_user_id\", \"modified_by_user_id\") " +
                    "VALUES (@Id, @TaskId, @MatterId, @UtcCreated, @UtcModified, @CreatedByUserId, @ModifiedByUserId)",
                    dbo);
            }

            return taskMatter;
        }

        public static Common.Models.Tasks.Task Edit(Common.Models.Tasks.Task model,
            Common.Models.Security.User modifier)
        {
            /*
             * We need to consider how to handle the relationship modifications
             *
             * First, basic assumptions:
             * 1) If a task has a sequential predecessor then it cannot independently specify its parent
             *
             *
             * Parent - if the parent is modified
             *
             * Sequential Predecessor
             * 1) If changed, need to cascade changes to all subsequent sequence members
             * 2) If removed from sequence, need to defer to user's parent selection
             * 3) If added to sequence, need to override user's parent selection
             *
             *
             * UI should be like:
             *
             * If sequence member:
             * [Remove from Sequence]
             *
             *
             * If NOT sequence member:
             * This task is not currently part of a task sequence.  If you would like to make this
             * task part of a task sequence, click here.
             * -- OnClick -->
             * Please select the task you wish t
             *
             */

            if (model.Parent != null && model.Parent.Id.HasValue)
            {
                // There is a proposed parent, we need to check and make sure it is not trying
                // to set itself as its parent
                if (model.Parent.Id.Value == model.Id)
                    throw new ArgumentException("Task cannot have itself as its parent.");
            }

            model.ModifiedBy = modifier;
            model.UtcModified = DateTime.UtcNow;
            DBOs.Tasks.Task dbo = Mapper.Map<DBOs.Tasks.Task>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"task\" SET " +
                    "\"title\"=@Title, \"description\"=@Description, \"projected_start\"=@ProjectedStart, " +
                    "\"due_date\"=@DueDate, \"projected_end\"=@ProjectedEnd, \"actual_end\"=@ActualEnd, \"parent_id\"=@ParentId" +
                    "\"utc_modified\"=@UtcModified, \"modified_by_user_id\"=@ModifiedByUserId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            if (model.Parent != null && model.Parent.Id.HasValue)
                UpdateGroupingTaskProperties(OpenLawOffice.Data.Tasks.Task.Get(model.Parent.Id.Value));

            return model;
        }

        private static void UpdateGroupingTaskProperties(Common.Models.Tasks.Task groupingTask)
        {
            bool groupingTaskChanged = false;

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                // Projected Start
                DBOs.Tasks.Task temp = conn.Query<DBOs.Tasks.Task>(
                    "SELECT * FROM \"task\" WHERE \"parent_id\"=@ParentId AND \"utc_disabled\" is null ORDER BY \"projected_start\" DESC limit 1",
                    new { ParentId = groupingTask.Id.Value }).SingleOrDefault();

                // If temp.ProjectedStart has a value then we know that there are no rows
                // with null value and so, we may update the grouping task to be the
                // earliest projected start value.  However, if null, then we need to
                // set the grouping task's projected start value to null.

                if (temp.ProjectedStart.HasValue)
                {
                    temp = conn.Query<DBOs.Tasks.Task>(
                        "SELECT * FROM \"task\" WHERE \"parent_id\"=@ParentId AND \"utc_disabled\" is null ORDER BY \"projected_start\" ASC limit 1",
                        new { ParentId = groupingTask.Id.Value }).SingleOrDefault();
                    if (groupingTask.ProjectedStart != temp.ProjectedStart)
                    {
                        groupingTask.ProjectedStart = temp.ProjectedStart;
                        groupingTaskChanged = true;
                    }
                }
                else
                {
                    if (groupingTask.ProjectedStart.HasValue)
                    {
                        groupingTask.ProjectedStart = null;
                        groupingTaskChanged = true;
                    }
                }

                // Due Date
                temp = conn.Query<DBOs.Tasks.Task>(
                    "SELECT * FROM \"task\" WHERE \"parent_id\"=@ParentId AND \"utc_disabled\" is null ORDER BY \"due_date\" DESC limit 1",
                    new { ParentId = groupingTask.Id.Value }).SingleOrDefault();
                if (temp.DueDate != groupingTask.DueDate)
                {
                    groupingTask.DueDate = temp.DueDate;
                    groupingTaskChanged = true;
                }

                // Projected End
                temp = conn.Query<DBOs.Tasks.Task>(
                    "SELECT * FROM \"task\" WHERE \"parent_id\"=@ParentId AND \"utc_disabled\" is null ORDER BY \"projected_end\" DESC limit 1",
                    new { ParentId = groupingTask.Id.Value }).SingleOrDefault();
                if (temp.ProjectedEnd != groupingTask.ProjectedEnd)
                {
                    groupingTask.ProjectedEnd = temp.ProjectedEnd;
                    groupingTaskChanged = true;
                }

                // Actual End
                temp = conn.Query<DBOs.Tasks.Task>(
                    "SELECT * FROM \"task\" WHERE \"parent_id\"=@ParentId AND \"utc_disabled\" is null ORDER BY \"actual_end\" DESC limit 1",
                    new { ParentId = groupingTask.Id.Value }).SingleOrDefault();
                if (temp.ActualEnd != groupingTask.ActualEnd)
                {
                    groupingTask.ActualEnd = temp.ActualEnd;
                    groupingTaskChanged = true;
                }

                // Update grouping task if needed
                if (groupingTaskChanged)
                {
                    DBOs.Tasks.Task task = Mapper.Map<DBOs.Tasks.Task>(groupingTask);
                    conn.Execute("UPDATE \"task\" SET \"projected_start\"=@ProjectedStart, \"due_date\"=@DueDate, \"projected_end\"=@ProjectedEnd, " +
                        "\"actual_end\"=@ActualEnd WHERE \"id\"=@Id",
                        task);

                    if (groupingTask.Parent != null && groupingTask.Parent.Id.HasValue)
                        UpdateGroupingTaskProperties(OpenLawOffice.Data.Tasks.Task.Get(groupingTask.Parent.Id.Value));
                }
            }
        }
    }
}