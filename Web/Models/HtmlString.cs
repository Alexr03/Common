using System.Collections.Generic;
using System.Text;
using Alexr03.Common.Misc.Other;
using Random = Alexr03.Common.Misc.Random;

namespace Alexr03.Common.Web.Models
{
    public class HtmlString
    {
        private readonly string _text;
        private readonly string _id = Random.RandomString(8);
        private string _tag = "div";
        private readonly IDictionary<string, object> _htmlAttributes = new Dictionary<string, object>();

        public HtmlString(string text)
        {
            _text = text;
        }

        public HtmlString Tag(string s)
        {
            _tag = s;
            return this;
        }

        public HtmlString HtmlAttributes(object attributes, bool overwrite = false)
        {
            _htmlAttributes.MergeWith(attributes.ToDictionary());
            return this;
        }

        public HtmlString FontColor(string value)
        {
            return HtmlAttributes(new {style = $"color:{value};" });
        }
        
        public HtmlString BackgroundColor(string value)
        {
            return HtmlAttributes(new {style = $"background-color:{value};" });
        }
        
        public HtmlString FontSize(int size)
        {
            return HtmlAttributes(new {style = $"font-size:{size}px;" });
        }
        
        public HtmlString FontWeight(int size)
        {
            return HtmlAttributes(new {style = $"font-weight:{size};" });
        }
        
        public HtmlString FontWeight(FontWeight fontWeight)
        {
            return HtmlAttributes(new {style = $"font-weight:{fontWeight.ToString().ToLower()};" });
        }

        public override string ToString()
        {
            var attributes = new StringBuilder();
            if (_htmlAttributes != null)
            {
                foreach (var htmlAttribute in _htmlAttributes)
                {
                    attributes.Append($"{htmlAttribute.Key}=\"{htmlAttribute.Value}\"");
                }
            }

            var sb = new StringBuilder();
            sb.Append($"<{_tag} id={_id} {attributes}>{_text}</{_tag}>");
            return sb.ToString();
        }
    }

    public enum FontWeight
    {
        Normal,
        Bold
    }
}