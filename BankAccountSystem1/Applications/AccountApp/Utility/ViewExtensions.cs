using AccountApp.Models.Operation;
using System.Web.Mvc;

namespace AccountApp.Utility
{
    public static class ViewExtensions
    {
        public static MvcHtmlString GetUiAlertBox(this HtmlHelper helper, object viewBag)
        {
            if (((dynamic)viewBag).OperationResult != null)
            {
                OperationResult result = ((dynamic)viewBag).OperationResult as OperationResult;

                TagBuilder tagBuilder = new TagBuilder("div");

                if (result.IsSuccess)
                {
                    tagBuilder.AddCssClass("alert alert-success");
                    tagBuilder.SetInnerText(result.ResultMessage);
                }
                else
                {
                    tagBuilder.AddCssClass("alert alert-danger");
                    tagBuilder.SetInnerText($"Error Code: {result.ResultCode} - Error: {result.ResultMessage}");
                }

                return MvcHtmlString.Create(tagBuilder.ToString());
            }

            return MvcHtmlString.Create(string.Empty);
        }
    }
}