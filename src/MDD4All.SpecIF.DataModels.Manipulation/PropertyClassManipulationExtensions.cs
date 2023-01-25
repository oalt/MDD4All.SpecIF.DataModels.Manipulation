using MDD4All.SpecIF.DataProvider.Contracts;
using System.Collections.Generic;

namespace MDD4All.SpecIF.DataModels.Manipulation
{
    public static class PropertyClassManipulationExtensions
    {
        public static string GetDataTypeTitle(this PropertyClass propertyClass, ISpecIfMetadataReader dataProvider)
        {
            string result = "";

            DataType dataType = dataProvider.GetDataTypeByKey(propertyClass.DataType);

            if (dataType != null)
            {
                result = dataType.Title.ToString();
            }

            return result;
        }

        public static void SetDefaultValue(this PropertyClass propertyClass, string defaultValue)
        {
            Value v = new Value(new MultilanguageText(defaultValue));
            
            if (defaultValue != null)
            {
                if (propertyClass.Values == null)
                {
                    propertyClass.Values = new List<Value>();
                }

                if (propertyClass.Values.Count > 0)
                {
                    propertyClass.Values[0] = v;
                }
                else
                {
                    propertyClass.Values.Add(v);
                }
            }
        }

        public static void SetMultilanguageText(this PropertyClass propertyClass, MultilanguageText multilanguageText)
        {
            Value v = new Value(multilanguageText);

            if (propertyClass.Values == null)
            {
                propertyClass.Values = new List<Value>();
            }

            if (propertyClass.Values.Count > 0)
            {
                propertyClass.Values[0] = v;
            }
            else
            {
                propertyClass.Values.Add(v);
            }
        }

    }
}
