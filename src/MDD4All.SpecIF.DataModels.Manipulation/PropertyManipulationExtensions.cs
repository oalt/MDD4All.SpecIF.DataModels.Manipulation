/*
 * Copyright (c) MDD4All.de, Dr. Oliver Alt
 */
using MDD4All.SpecIF.DataProvider.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace MDD4All.SpecIF.DataModels.Manipulation
{
    public static class PropertyManipulationExtensions
    {
        public static string GetDataTypeType(this Property property, ISpecIfMetadataReader dataProvider)
        {
            string result = "";

            PropertyClass propertyClass = dataProvider.GetPropertyClassByKey(property.Class);

            if (propertyClass != null)
            {
                DataType dataType = dataProvider.GetDataTypeByKey(propertyClass.DataType);
                result = dataType.Type;
            }
            return result;
        }

        public static DataType GetDataType(this Property property, ISpecIfMetadataReader dataProvider)
        {
            DataType result = null;

            PropertyClass propertyClass = dataProvider.GetPropertyClassByKey(property.Class);

            if (propertyClass != null)
            {
                DataType dataType = dataProvider.GetDataTypeByKey(propertyClass.DataType);
                result = dataType;
            }
            return result;
        }

        public static string GetClassTitle(this Property property, ISpecIfMetadataReader dataProvider)
        {
            string result = null;

            PropertyClass propertyClass = dataProvider.GetPropertyClassByKey(property.Class);

            if (propertyClass != null)
            {
                result = propertyClass.Title;
            }
            return result;
        }

        public static string GetStringValue(this Property property, ISpecIfMetadataReader metadataReader, string language = "en")
        {
            string result = "";

            DataType dataType = property.GetDataType(metadataReader);

            if (property.Values != null && property.Values.Count > 0)
            {
                result = property.Values[0].ToString(language);
            }

            return result;
        }

        public static List<string> GetStringValues(this Property property, ISpecIfMetadataReader metadataReader, string language = "en")
        {
            List<string> result = new List<string>();

            DataType dataType = property.GetDataType(metadataReader);

            foreach (Value value in property.Values)
            {
                result.Add(value.ToString(language));
            }
            
            return result;
        }

        public static bool IsEnumeration(this Property property, ISpecIfMetadataReader metadataReader)
        {
            bool result = false;

            DataType dataType = property.GetDataType(metadataReader);

            if (dataType != null)
            {
                // enumeration type
                if (dataType.Enumeration != null && dataType.Enumeration.Count > 0)
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the user visible enumeration values.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="metadataReader"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static List<string> GetEnumerationValues(this Property property, 
                                                        ISpecIfMetadataReader metadataReader,
                                                        string language = "en")
        {
            List<string> result = new List<string>();

            DataType dataType = property.GetDataType(metadataReader);

            if (dataType != null)
            {
                // enumeration type
                if (dataType.Enumeration != null && dataType.Enumeration.Count > 0)
                {
                    foreach (Value value in property.Values)
                    {
                        EnumerationValue enumValue = null;
                        try
                        {
                            enumValue = dataType.Enumeration.First(e => e.ID == value.ToString(language));
                        }
                        catch
                        { }

                        if (enumValue != null)
                        {
                            MultilanguageText enumText = enumValue.Value.First(v => v.Language == language);
                            if (enumText == null && enumValue.Value.Count > 0)
                            {
                                enumText = enumValue.Value[0];
                            }

                            if (enumText != null)
                            {
                                result.Add(enumText.Text);
                            }
                        }


                    }
                } 
            }

            return result;
        
        }
    }
}
