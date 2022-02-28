using System;
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
    }
}
