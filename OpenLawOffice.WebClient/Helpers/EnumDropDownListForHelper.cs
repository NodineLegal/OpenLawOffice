namespace OpenLawOffice.WebClient.Helpers
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;
    using System;

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