// -----------------------------------------------------------------------
// <copyright file="Billing.cs" company="Nodine Legal, LLC">
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
    using Npgsql;

    public class Billing : Base
    {
        public static Common.Models.Billing.Invoice SingleMatterBill(
            Common.Models.Billing.Invoice invoice, 
            List<Common.Models.Billing.InvoiceExpense> invoiceExpenseList,
            List<Common.Models.Billing.InvoiceFee> invoiceFeeList,
            List<Common.Models.Billing.InvoiceTime> invoiceTimeList,
            Common.Models.Account.Users currentUser,
            IDbConnection conn = null, bool closeConnection = true)
        {
            decimal subtotal = 0;
            IDbTransaction trans;

            OpenIfNeeded(conn);

            trans = conn.BeginTransaction();
            
            invoice = Data.Billing.Invoice.Create(invoice, currentUser, conn, false);

            invoiceExpenseList.ForEach(invoiceExpense =>
            {
                subtotal += invoiceExpense.Amount;
                Data.Billing.InvoiceExpense.Create(invoiceExpense, currentUser, conn, false);
            });
            
            invoiceFeeList.ForEach(invoiceFee =>
            {
                subtotal += invoiceFee.Amount;
                Data.Billing.InvoiceFee.Create(invoiceFee, currentUser, conn, false);
            });
            
            invoiceTimeList.ForEach(invoiceTime =>
            {
                subtotal += ((decimal)invoiceTime.Duration.TotalHours * invoiceTime.PricePerHour);
                Data.Billing.InvoiceTime.Create(invoiceTime, currentUser, conn, false);
            });

            invoice.Subtotal = subtotal;
            invoice.Total = invoice.Subtotal + invoice.TaxAmount;

            Data.Billing.Invoice.Edit(invoice, currentUser, conn, false);

            trans.Commit();

            Close(conn, closeConnection);

            return invoice;
        }
    }
}
