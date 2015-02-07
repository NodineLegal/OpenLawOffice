// -----------------------------------------------------------------------
// <copyright file="TimeSpanHelper.cs" company="Nodine Legal, LLC">
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

namespace OpenLawOffice.WebClient.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    public class TimeSpanHelper
    {
        public static IHtmlString TimeSpan(TimeSpan ts, bool showDays)
        {
            if (showDays)
            {
                return new HtmlString(ts.Days + " d " + ts.Hours + ":" + ts.Minutes);
            }
            else
            {
                int hours = (int)Math.Floor(ts.TotalHours);
                return new HtmlString(hours + ":" + ts.Minutes);
            }
        }

        public static IHtmlString TimeSpan(object obj, bool showDays)
        {
            if (obj.GetType() != typeof(TimeSpan))
                throw new InvalidCastException("Cannot cast " + obj.GetType().FullName + " to " + typeof(TimeSpan).FullName);

            return TimeSpan((TimeSpan)obj, showDays);
        }
    }
}