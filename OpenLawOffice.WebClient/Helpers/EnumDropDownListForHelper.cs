// -----------------------------------------------------------------------
// <copyright file="EnumDropDownListForHelper.cs" company="Nodine Legal, LLC">
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

    public static class EnumDropDownListForHelper
    {
        public static IHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> html, Expression<Func<TModel, TEnum>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            Type enumType = Nullable.GetUnderlyingType(metadata.ModelType) ?? metadata.ModelType;

            List<SelectListItem> list = new List<SelectListItem>();

            foreach (ViewModels.AssignmentTypeViewModel vm in Enum.GetValues(enumType))
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = vm.ToString(),
                    Value = vm.ToString(),
                    Selected = vm.Equals(metadata.Model)
                };
                list.Add(item);
            }

            return (IHtmlString)html.DropDownListFor(expression, list);
        }
    }
}