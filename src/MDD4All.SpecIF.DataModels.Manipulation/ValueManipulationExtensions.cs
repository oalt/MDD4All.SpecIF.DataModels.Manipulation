using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace MDD4All.SpecIF.DataModels.Manipulation
{
    public static class ValueManipulationExtensions
    {
        public static TimeSpan ToTimeSpan(this string stringValue)
        {
            TimeSpan result = default;
            try
            {
                result = XmlConvert.ToTimeSpan(stringValue);
            }
            catch(Exception exception) 
            {
                ;
            }

            return result;
        }

        public static string GetDefaultStringValue(this List<MultilanguageText> multilanguageTexts)
        {
            string result = default;

            if(multilanguageTexts != null && multilanguageTexts.Count > 0)
            {
                MultilanguageText multilanguageText = multilanguageTexts.First(v => v.Language == null || v.Language == "en");
                
                if(multilanguageText != null)
                {
                    result = multilanguageText.Text;
                }
            }

            return result;
        }
    }
}
