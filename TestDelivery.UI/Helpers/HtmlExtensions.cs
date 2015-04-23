using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc.Html;

namespace System.Web.Mvc
{
    public static class HtmlExtensions
    {
        public static string Disable(this HtmlHelper html, bool disable)
        {
            return disable ? "disabled" : string.Empty;
        }

        public static MvcHtmlString EditableListFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                              Expression<Func<TModel, IEnumerable<TValue>>> selector,
                                                              string itemViewPath, bool canReorderItems = true)
        {

            var collection = html.ViewData.Model == null ? null : selector.Compile().Invoke(html.ViewData.Model);

            var propName = html.NameFor(selector).ToString();
            propName = propName.Substring(propName.LastIndexOf(".") + 1);
            var oldPrefix = html.ViewData.TemplateInfo.HtmlFieldPrefix;
            var prefixStart = string.IsNullOrEmpty(oldPrefix) ? propName : oldPrefix + "." + propName;
            var listId = html.IdFor(selector);
            var sb = new StringBuilder();
            sb.Append("<div class='list editable' data-property-name='");

            sb.Append(propName);
            sb.Append(@"' data-property-level='");
            sb.Append(string.IsNullOrEmpty(oldPrefix) ? 0 : 1 + oldPrefix.Count(c => c == '.'));
            sb.Append(@"' id='");
            sb.Append(listId);
            sb.Append("' >");
            sb.Append(@"<script type='text/javascript'> jQuery(function($){ $('#");
            sb.Append(listId);
            sb.Append(@"').sortable({ update: function(e, ui) { EditableList.renameContainers(e.target); } }); }); </script>");

            sb.Append(@"<span class='options'><button onclick='EditableList.addItemClicked(this)' type='button'>Add</button></span>");



            if (collection != null)
            {
                var index = -1;
                foreach (var item in collection)
                {
                    sb.Append(html.RenderListItem(itemViewPath, new ViewDataDictionary<TValue>(item), prefixStart + "[" + (++index) + "]"));
                }
            }

            sb.Append(@"<div class='template' style='display: none'>");
            {
                sb.Append(html.RenderListItem(itemViewPath, new ViewDataDictionary<TValue>(default(TValue)), prefixStart + "Template"));
            }
            sb.Append(@"</div>");
            sb.Append(@"</div>");

            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString EditableFieldFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                              Expression<Func<TModel, TValue>> selector,
                                                                bool raw = false)
        {
            var sb = new StringBuilder();

            sb.Append("<div class='field' >");
            {
                sb.Append("<div class='editor-label'>");
                {
                    sb.Append(html.LabelFor(selector));
                }
                sb.Append("</div>");

                sb.Append(html.EditableFieldPartFor(selector, raw));
            }
            sb.Append("</div>");

            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString EditableFieldNoLabelFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                              Expression<Func<TModel, TValue>> selector,
                                                                bool raw = false)
        {
            var sb = new StringBuilder();

            sb.Append("<div class='field' >");
            {
                sb.Append(html.EditableFieldPartFor(selector, raw));
            }
            sb.Append("</div>");

            return new MvcHtmlString(sb.ToString());
        }


        internal static MvcHtmlString EditableFieldPartFor<TModel, TValue>(this HtmlHelper<TModel> html, 
                            Expression<Func<TModel, TValue>> selector, bool raw = false)
        {
            var sb = new StringBuilder();

            sb.Append("<div class='editor-field" + (raw ? " html-field" : string.Empty) + "'>");
            {
                //sb.Append("<span class='preview' data-type='editable' data-for='#");
                //sb.Append(html.IdFor(selector));
                //sb.Append("'>");
                //{
                //    if (raw && html.ViewData.Model != null)
                //    {
                //        var value = selector.Compile().Invoke(html.ViewData.Model);
                //        if (value != null)
                //        {
                //            sb.Append(value.ToString());
                //        }
                //    }
                //    else
                //    {
                //        sb.Append(html.ValueFor(selector).ToString());
                //    }
                //}
                //sb.Append("</span>");

                sb.Append(html.EditorFor(selector));
                sb.Append(html.ValidationMessageFor(selector));
            }
            sb.Append("</div>");
            return new MvcHtmlString(sb.ToString());

        }


        public static MvcHtmlString EditableListFieldFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                              Expression<Func<TModel, IEnumerable<TValue>>> selector,
                                                              string itemViewPath, bool canReorderItems = true)
        {
            var sb = new StringBuilder();

            sb.Append("<div class='field' >");
            {
                sb.Append("<div class='editor-label'>");
                {
                    sb.Append(html.LabelFor(selector));
                }
                sb.Append("</div>");

                sb.Append("<div class='editor-field'>");
                {
                    sb.Append(html.EditableListFor(selector, itemViewPath, canReorderItems));
                }
                sb.Append("</div>");
            }
            sb.Append("</div>");

            return new MvcHtmlString(sb.ToString());
        }


        private static string RenderListItem<TModel, TValue>(this HtmlHelper<TModel> html, string itemViewPath, ViewDataDictionary<TValue> viewData, string prefix)
        {
            var sb = new StringBuilder();
            sb.Append(@"<div class='item'>");
            sb.Append(@"<span class='options'><button onclick='EditableList.deleteItemClicked(this)'>Delete</button></span>");

            viewData.TemplateInfo.HtmlFieldPrefix = prefix;
            sb.Append(html.Partial(itemViewPath, viewData));
            sb.Append(@"</div>");
            return sb.ToString();
        }
    }
}