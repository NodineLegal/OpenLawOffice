// -----------------------------------------------------------------------
// <copyright file="Invoice.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.Data.Billing
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using AutoMapper;
    using Dapper;
    
    public class Invoice
    {
        public static List<Common.Models.Matters.Matter> ListBillableMatters()
        {
            return DataHelper.List<Common.Models.Matters.Matter, DBOs.Matters.Matter>(
                "SELECT \"matter\".*, \"contact\".\"display_name\" FROM \"matter\" LEFT JOIN \"contact\" ON \"matter\".\"bill_to_contact_id\"=\"contact\".\"id\" " +
                "WHERE \"matter\".\"id\" IN ( " +
                "   SELECT \"matter_id\" FROM \"task_matter\" WHERE " +
                "       \"task_id\" IN (SELECT \"task_id\" FROM \"task_time\" WHERE " +
                "           \"time_id\" IN (SELECT \"id\" FROM \"time\" WHERE \"billable\"=true AND \"utc_disabled\" is NULL) AND \"utc_disabled\" is NULL " +
                "       ) AND \"utc_disabled\" is NULL " +
                "	UNION " +
                "   SELECT \"matter_id\" FROM \"expense_matter\" WHERE \"expense_id\" NOT IN (SELECT \"expense_id\" FROM \"invoice_expense\" WHERE \"utc_disabled\" is NULL) AND \"utc_disabled\" is NULL " +
                "   UNION " +
                "   SELECT \"matter_id\" FROM \"fee_matter\" WHERE \"fee_id\" NOT IN (SELECT \"fee_id\" FROM \"invoice_fee\" WHERE \"utc_disabled\" is NULL) AND \"utc_disabled\" is NULL " +
                ") AND \"matter\".\"billing_group_id\" IS NULL ORDER BY \"contact\".\"display_name\" ASC");
        }

        public static List<Common.Models.Billing.BillingGroup> ListBillableBillingGroups()
        {
            return DataHelper.List<Common.Models.Billing.BillingGroup, DBOs.Billing.BillingGroup>(
                "SELECT * FROM \"billing_group\" WHERE \"next_run\" <= now() at time zone 'utc'");
        }
    }
}
