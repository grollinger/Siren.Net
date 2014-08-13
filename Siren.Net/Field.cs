using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContrib.Formatting.Siren.Client
{
    enum HtmlInputType
	{
	    Text, // Default -> First
        Hidden, Search, Tel, Url, Email, Password, Datetime, Date, Month, Week, Time, DatetimeLocal, Number, Range, Color, Checkbox, Radio, File, Image, Button       
	}

    public class Field
    {
        public string Name { get; set; }
        public HtmlInputType Type { get; set; }
        public object Value { get; set; }
    }
}
