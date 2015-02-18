// -----------------------------------------------------------------------
// <copyright file="Time.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Timing
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using AutoMapper;
    using Dapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Time
    {
        public static Common.Models.Timing.Time Get(Guid id)
        {
            return DataHelper.Get<Common.Models.Timing.Time, DBOs.Timing.Time>(
                "SELECT * FROM \"time\" WHERE \"id\"=@id AND \"utc_disabled\" is null",
                new { id = id });
        }

        public static List<Common.Models.Timing.Time> ListForTask(long taskId)
        {
            return DataHelper.List<Common.Models.Timing.Time, DBOs.Timing.Time>(
                "SELECT \"time\".* FROM \"time\" JOIN \"task_time\" ON \"time\".\"id\"=\"task_time\".\"time_id\" " +
                "WHERE \"task_id\"=@TaskId AND \"time\".\"utc_disabled\" is null AND \"task_time\".\"utc_disabled\" is null " +
                "ORDER BY \"start\" ASC",
                new { TaskId = taskId });
        }

        public static List<Common.Models.Timing.Time> ListForDay(int workerContactId, DateTime date)
        {
            if (date.Kind != DateTimeKind.Utc)
                date = date.ToDbTime();
            return DataHelper.List<Common.Models.Timing.Time, DBOs.Timing.Time>(
                "SELECT * FROM \"time\" WHERE \"worker_contact_id\"=@WorkerContactId AND \"start\" BETWEEN @Start AND @Stop AND \"utc_disabled\" is null ORDER BY \"start\" ASC",
                new { WorkerContactId = workerContactId, Start = date, Stop = date.AddDays(1).AddMilliseconds(-1) });
        }

        public static List<Common.Models.Timing.Time> ListForMatterWithinRange(Guid matterId, DateTime? from = null, DateTime? to = null)
        {
            if (from.HasValue && from.Value.Kind != DateTimeKind.Utc)
                from = from.ToDbTime();
            if (to.HasValue && to.Value.Kind != DateTimeKind.Utc)
                to = to.ToDbTime();

            if (from.HasValue && to.HasValue)
                return DataHelper.List<Common.Models.Timing.Time, DBOs.Timing.Time>(
                    "SELECT * FROM \"time\" WHERE \"id\" IN (SELECT \"time_id\" FROM \"task_time\" WHERE \"task_id\" IN (SELECT \"task_id\" FROM \"task_matter\" WHERE \"matter_id\"=@MatterId)) AND \"start\" BETWEEN @Start AND @Stop AND \"utc_disabled\" is null ORDER BY \"start\" ASC",
                    new { MatterId = matterId, Start = from.Value, Stop = to.Value.AddDays(1).AddMilliseconds(-1) });
            else if (from.HasValue && !to.HasValue)
                return DataHelper.List<Common.Models.Timing.Time, DBOs.Timing.Time>(
                    "SELECT * FROM \"time\" WHERE \"id\" IN (SELECT \"time_id\" FROM \"task_time\" WHERE \"task_id\" IN (SELECT \"task_id\" FROM \"task_matter\" WHERE \"matter_id\"=@MatterId)) AND \"start\" >= @Start AND \"utc_disabled\" is null ORDER BY \"start\" ASC",
                    new { MatterId = matterId, Start = from.Value });
            else if (!from.HasValue && to.HasValue)
                return DataHelper.List<Common.Models.Timing.Time, DBOs.Timing.Time>(
                    "SELECT * FROM \"time\" WHERE \"id\" IN (SELECT \"time_id\" FROM \"task_time\" WHERE \"task_id\" IN (SELECT \"task_id\" FROM \"task_matter\" WHERE \"matter_id\"=@MatterId)) AND \"start\" <= @Stop AND \"utc_disabled\" is null ORDER BY \"start\" ASC",
                    new { MatterId = matterId, Stop = to.Value.AddDays(1).AddMilliseconds(-1) });
            else // !from && !to
                return DataHelper.List<Common.Models.Timing.Time, DBOs.Timing.Time>(
                    "SELECT * FROM \"time\" WHERE \"id\" IN (SELECT \"time_id\" FROM \"task_time\" WHERE \"task_id\" IN (SELECT \"task_id\" FROM \"task_matter\" WHERE \"matter_id\"=@MatterId)) AND \"utc_disabled\" is null ORDER BY \"start\" ASC",
                    new { MatterId = matterId });
        }

        public static List<Common.Models.Timing.Time> ListForMatterWithinRange(Guid matterId, int employeeContactId, DateTime? from = null, DateTime? to = null)
        {
            if (from.HasValue && from.Value.Kind != DateTimeKind.Utc)
                from = from.ToDbTime();
            if (to.HasValue && to.Value.Kind != DateTimeKind.Utc)
                to = to.ToDbTime();

            if (from.HasValue && to.HasValue)
                return DataHelper.List<Common.Models.Timing.Time, DBOs.Timing.Time>(
                    "SELECT * FROM \"time\" WHERE \"worker_contact_id\"=@EmployeeContactId AND \"id\" IN (SELECT \"time_id\" FROM \"task_time\" WHERE \"task_id\" IN (SELECT \"task_id\" FROM \"task_matter\" WHERE \"matter_id\"=@MatterId)) AND \"start\" BETWEEN @Start AND @Stop AND \"utc_disabled\" is null ORDER BY \"start\" ASC",
                    new { MatterId = matterId, EmployeeContactId = employeeContactId, Start = from.Value, Stop = to.Value.AddDays(1).AddMilliseconds(-1) });
            else if (from.HasValue && !to.HasValue)
                return DataHelper.List<Common.Models.Timing.Time, DBOs.Timing.Time>(
                    "SELECT * FROM \"time\" WHERE \"worker_contact_id\"=@EmployeeContactId AND \"id\" IN (SELECT \"time_id\" FROM \"task_time\" WHERE \"task_id\" IN (SELECT \"task_id\" FROM \"task_matter\" WHERE \"matter_id\"=@MatterId)) AND \"start\" >= @Start AND \"utc_disabled\" is null ORDER BY \"start\" ASC",
                    new { MatterId = matterId, EmployeeContactId = employeeContactId, Start = from.Value });
            else if (!from.HasValue && to.HasValue)
                return DataHelper.List<Common.Models.Timing.Time, DBOs.Timing.Time>(
                    "SELECT * FROM \"time\" WHERE \"worker_contact_id\"=@EmployeeContactId AND \"id\" IN (SELECT \"time_id\" FROM \"task_time\" WHERE \"task_id\" IN (SELECT \"task_id\" FROM \"task_matter\" WHERE \"matter_id\"=@MatterId)) AND \"start\" <= @Stop AND \"utc_disabled\" is null ORDER BY \"start\" ASC",
                    new { MatterId = matterId, EmployeeContactId = employeeContactId, Stop = to.Value.AddDays(1).AddMilliseconds(-1) });
            else // !from && !to
                return DataHelper.List<Common.Models.Timing.Time, DBOs.Timing.Time>(
                    "SELECT * FROM \"time\" WHERE \"worker_contact_id\"=@EmployeeContactId AND \"id\" IN (SELECT \"time_id\" FROM \"task_time\" WHERE \"task_id\" IN (SELECT \"task_id\" FROM \"task_matter\" WHERE \"matter_id\"=@MatterId)) AND \"utc_disabled\" is null ORDER BY \"start\" ASC",
                    new { MatterId = matterId, EmployeeContactId = employeeContactId });
        }

        public static Common.Models.Tasks.Task GetRelatedTask(Guid timeId)
        {
            return DataHelper.Get<Common.Models.Tasks.Task, DBOs.Tasks.Task>(
                "SELECT \"task\".* FROM \"task\" JOIN \"task_time\" ON \"task\".\"id\"=\"task_time\".\"task_id\" " +
                "WHERE \"time_id\"=@TimeId AND \"task\".\"utc_disabled\" is null AND \"task_time\".\"utc_disabled\" is null",
                new { TimeId = timeId });
        }

        public static List<Common.Models.Timing.Time> ListConflictingTimes(DateTime start, DateTime stop, int workerContactId)
        {
            // Check for overlap
            // We work in time frames or windows
            // The new time can either (1) be within an existing time, (2) overlap an existing time, (3) encumpas an existing time or (4) be exclusive in reference to other time
            // The ONLY valid entry is #4
            if (start.Kind != DateTimeKind.Utc)
                start = start.ToDbTime();
            if (stop.Kind != DateTimeKind.Utc)
                stop = stop.ToDbTime();
            return DataHelper.List<Common.Models.Timing.Time, DBOs.Timing.Time>(
                "SELECT * FROM time WHERE (@Start > \"start\" AND @Start < \"stop\" AND \"worker_contact_id\"=@WorkerContactId) OR " + // 1 and 2
                "(@Stop > \"start\" AND @Stop < \"stop\" AND \"worker_contact_id\"=@WorkerContactId) OR " + // 1 and 2
                "(@Start <= \"start\" AND @Stop >= \"stop\" AND \"worker_contact_id\"=@WorkerContactId)",
                new { Start = start, Stop = stop, WorkerContactId = workerContactId });
        }

        public static List<Common.Models.Timing.Time> ListBilledTimeForMatter(Guid matterId)
        {
            return DataHelper.List<Common.Models.Timing.Time, DBOs.Timing.Time>(
                "SELECT * FROM \"time\" WHERE \"id\" IN " + 
                "   (SELECT \"time_id\" FROM \"task_time\" WHERE \"task_id\" IN " + 
                "       (SELECT \"task_id\" FROM \"task_matter\" WHERE \"matter_id\"=@MatterId)) AND " +
                "   \"id\" IN (SELECT \"time_id\" FROM \"invoice_time\" WHERE \"time_id\"=\"time\".\"id\") AND " +
                "\"utc_disabled\" is null ORDER BY \"utc_created\" ASC",
                new { MatterId = matterId });
        }

        public static List<Common.Models.Timing.Time> ListUnbilledTimeForMatter(Guid matterId)
        {
            return DataHelper.List<Common.Models.Timing.Time, DBOs.Timing.Time>(
                "SELECT * FROM \"time\" WHERE \"id\" IN " +
                "   (SELECT \"time_id\" FROM \"task_time\" WHERE \"task_id\" IN " +
                "       (SELECT \"task_id\" FROM \"task_matter\" WHERE \"matter_id\"=@MatterId)) AND " +
                "   \"id\" NOT IN (SELECT \"time_id\" FROM \"invoice_time\" WHERE \"time_id\"=\"time\".\"id\") AND " +
                "\"utc_disabled\" is null ORDER BY \"utc_created\" ASC",
                new { MatterId = matterId });
        }

        public static List<Common.Models.Timing.Time> ListUnbilledAndBillableTimeForMatter(Guid matterId)
        {
            return DataHelper.List<Common.Models.Timing.Time, DBOs.Timing.Time>(
                "SELECT * FROM \"time\" WHERE \"billable\"=true AND \"id\" IN " +
                "   (SELECT \"time_id\" FROM \"task_time\" WHERE \"task_id\" IN " +
                "       (SELECT \"task_id\" FROM \"task_matter\" WHERE \"matter_id\"=@MatterId)) AND " +
                "   \"id\" NOT IN (SELECT \"time_id\" FROM \"invoice_time\" WHERE \"time_id\"=\"time\".\"id\") AND " +
                "\"utc_disabled\" is null ORDER BY \"utc_created\" ASC",
                new { MatterId = matterId });
        }

        public static TimeSpan SumUnbilledAndBillableTimeForMatter(Guid matterId)
        {
            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                IEnumerable<dynamic> ie = conn.Query(
                "SELECT SUM(\"stop\" - \"start\") AS \"Interval\" FROM \"time\" WHERE \"id\" IN " +
                "   (SELECT \"time_id\" FROM \"task_time\" WHERE \"task_id\" IN " +
                "       (SELECT \"task_id\" FROM \"task_matter\" WHERE \"matter_id\"=@MatterId)) AND " +
                "   \"id\" NOT IN (SELECT \"time_id\" FROM \"invoice_time\" WHERE \"time_id\"=\"time\".\"id\") AND " +
                "\"billable\"=true AND \"utc_disabled\" is null",
                new { MatterId = matterId });

                IEnumerator<dynamic> enumerator = ie.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    return (TimeSpan)enumerator.Current.Interval;
                }
            }

            return new TimeSpan();
        }

        public static TimeSpan SumUnbilledAndBillableTimeForBillingGroup(int billingGroupId)
        {
            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                IEnumerable<dynamic> ie = conn.Query(
                "SELECT SUM(\"stop\" - \"start\") AS \"Interval\" FROM \"time\" WHERE \"id\" IN " +
                "   (SELECT \"time_id\" FROM \"task_time\" WHERE \"task_id\" IN " +
                "       (SELECT \"task_id\" FROM \"task_matter\" WHERE \"matter_id\" IN " +
                "           (SELECT \"id\" FROM \"matter\" WHERE \"billing_group_id\"=@BillingGroupId) " +
                "       ) " +
                "   ) " +
                "AND " +
                "\"id\" NOT IN (SELECT \"time_id\" FROM \"invoice_time\" WHERE \"time_id\"=\"time\".\"id\") AND " +
                "\"billable\"=true AND \"utc_disabled\" is null",
                new { BillingGroupId = billingGroupId });

                IEnumerator<dynamic> enumerator = ie.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    return (TimeSpan)enumerator.Current.Interval;
                }
            }

            return new TimeSpan();
        }

        public static bool IsFastTime(Guid timeId)
        {
            return (GetRelatedTask(timeId) == null);
        }

        public static List<Common.Models.Timing.Time> FastTimeList()
        {
            return DataHelper.List<Common.Models.Timing.Time, DBOs.Timing.Time>(
                "SELECT * FROM \"time\" WHERE id NOT IN (SELECT \"time_id\" FROM \"task_time\")");
        }

        public static Common.Models.Timing.Time Create(Common.Models.Timing.Time model,
            Common.Models.Account.Users creator)
        {
            if (!model.Id.HasValue) model.Id = Guid.NewGuid();
            model.CreatedBy = model.ModifiedBy = creator;
            model.Created = model.Modified = DateTime.UtcNow;
            DBOs.Timing.Time dbo = Mapper.Map<DBOs.Timing.Time>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"time\" (\"id\", \"start\", \"stop\", \"worker_contact_id\", \"details\", \"billable\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                    "VALUES (@Id, @Start, @Stop, @WorkerContactId, @Details, @Billable, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                    dbo);
            }

            return model;
        }

        public static Common.Models.Timing.Time Edit(Common.Models.Timing.Time model,
            Common.Models.Account.Users modifier)
        {
            model.ModifiedBy = modifier;
            model.Modified = DateTime.UtcNow;
            DBOs.Timing.Time dbo = Mapper.Map<DBOs.Timing.Time>(model);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("UPDATE \"time\" SET " +
                    "\"start\"=@Start, \"stop\"=@Stop, \"worker_contact_id\"=@WorkerContactId, \"details\"=@Details, \"billable\"=@Billable, \"utc_modified\"=@UtcModified, \"modified_by_user_pid\"=@ModifiedByUserPId " +
                    "WHERE \"id\"=@Id", dbo);
            }

            return model;
        }

        public static Common.Models.Tasks.TaskTime RelateTask(Common.Models.Timing.Time timeModel,
            long taskId, Common.Models.Account.Users creator)
        {
            Common.Models.Tasks.TaskTime taskTime = new Common.Models.Tasks.TaskTime()
            {
                Id = Guid.NewGuid(),
                Task = new Common.Models.Tasks.Task() { Id = taskId, IsStub = true },
                Time = timeModel,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
                CreatedBy = creator,
                ModifiedBy = creator
            };

            DBOs.Tasks.TaskTime dbo = Mapper.Map<DBOs.Tasks.TaskTime>(taskTime);

            using (IDbConnection conn = Database.Instance.GetConnection())
            {
                conn.Execute("INSERT INTO \"task_time\" (\"id\", \"task_id\", \"time_id\", \"utc_created\", \"utc_modified\", \"created_by_user_pid\", \"modified_by_user_pid\") " +
                    "VALUES (@Id, @TaskId, @TimeId, @UtcCreated, @UtcModified, @CreatedByUserPId, @ModifiedByUserPId)",
                    dbo);
            }

            return taskTime;
        }
    }
}