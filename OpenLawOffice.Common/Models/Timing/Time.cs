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

namespace OpenLawOffice.Common.Models.Timing
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Time : Core
    {
        public Guid? Id { get; set; }

        public DateTime Start { get; set; }

        public DateTime? Stop { get; set; }

        public Contacts.Contact Worker { get; set; }

        public string Details { get; set; }
    }
}