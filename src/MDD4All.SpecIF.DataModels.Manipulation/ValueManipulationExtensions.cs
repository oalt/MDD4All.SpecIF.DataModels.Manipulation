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

        public static string GetDefaultLanguageStringValue(this Value value)
        {
            string result = default;

            if(value.IsStringValue())
            {
                result = value.StringValue;
            }
            else
            {
                List<MultilanguageText> multilanguageTexts = value.MultilanguageTexts;

                if (multilanguageTexts != null && multilanguageTexts.Count > 0)
                {
                    MultilanguageText multilanguageText = multilanguageTexts.First(v => v.Language == null || v.Language == "en");

                    if (multilanguageText != null)
                    {
                        result = multilanguageText.Text;
                    }
                }
            }

            

            return result;
        }

        //public static string ToSimpleTextString(this object value)
        //{
        //    string result = "";
        //    if (value is string)
        //    {
        //        result = (string)value;
        //    }
        //    else if (value is object[])
        //    {

        //    }

        //    return result;
        //}

        public static string ToSimpleTextString(this Value value, string language = "en")
        {
            string result = "";

            if (value.StringValue != null)
            {
                result = value.StringValue;
            }
            else if (value.MultilanguageTexts.Count > 0)
            {
                try
                {
                    result = value.MultilanguageTexts.First(mlt => mlt.Language == language).Text;

                    if (result == null)
                    {
                        result = value.MultilanguageTexts[0].Text;
                    }
                }
                catch
                { }

            }

            return result;
        }

        public static bool IsMultilanguageValue(this Value value)
        {
            bool result = false;

            result = !(value.MultilanguageTexts == null || value.MultilanguageTexts.Count == 0);

            return result;
            
        }

        public static bool IsStringValue(this Value value)
        {
            bool result = false;
            
            result = value.StringValue != null;

            return result;
        }

        public static string ToString(this Value value, string language)
        {
            return value.ToSimpleTextString(language);
        }
    }
}
