// -----------------------------------------------------------------------
// <copyright file="Fee.cs" company="Nodine Legal, LLC">
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
    
    public class Fee
    {
        public static List<Common.Models.Billing.Fee> ListForMatter(Guid matterId)
        {
            return DataHelper.List<Common.Models.Billing.Fee, DBOs.Billing.Fee>(
                "SELECT * FROM \"fee\" WHERE \"id\" IN (SELECT \"fee_id\" FROM \"fee_matter\" WHERE \"matter_id\"=@MatterId) AND " +
                "\"utc_disabled\" is null ORDER BY \"incurred\" ASC",
                new { MatterId = matterId });
        }

        public static List<Common.Models.Billing.Fee> ListBilledFeesForMatter(Guid matterId)
        {
            return DataHelper.List<Common.Models.Billing.Fee, DBOs.Billing.Fee>(
                "SELECT * FROM \"fee\" WHERE " +
                "   \"id\" IN (SELECT \"fee_id\" FROM \"fee_matter\" WHERE \"matter_id\"=@MatterId) AND " +
                "   \"id\" IN (SELECT \"fee_id\" FROM \"invoice_fee\" WHERE \"fee_id\"=\"fee\".\"id\") AND " +
                "\"utc_disabled\" is null ORDER BY \"utc_created\" ASC",
                new { MatterId = matterId });
        }

        public static List<Common.Models.Billing.Fee> ListUnbilledFeesForMatter(Guid matterId)
        {
            return DataHelper.List<Common.Models.Billing.Fee, DBOs.Billing.Fee>(
                "SELECT * FROM \"fee\" WHERE " +
                "   \"id\" IN (SELECT \"fee_id\" FROM \"fee_matter\" WHERE \"matter_id\"=@MatterId) AND " +
                "   \"id\" NOT IN (SELECT \"fee_id\" FROM \"invoice_fee\" WHERE \"fee_id\"=\"fee\".\"id\") AND " +
                "\"utc_disabled\" is null ORDER BY \"utc_created\" ASC",
                new { MatterId = matterId });
        }
    }
}
