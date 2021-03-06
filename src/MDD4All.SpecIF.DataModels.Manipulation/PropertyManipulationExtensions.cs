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
                Value firstValue = property.Values[0];

                if (firstValue.StringValue != null)
                {
                    result = firstValue.StringValue;
                }
                else if (firstValue.MultilanguageTexts != null && firstValue.MultilanguageTexts.Count > 0)
                {
                    foreach (MultilanguageText multilanguageText in firstValue.MultilanguageTexts)
                    {
                        if (multilanguageText.Language == null || multilanguageText.Language == "en")
                        {
                            if (language == null || language == "en")
                            {
                                result = multilanguageText.Text;
                                break;
                            }
                            else if (multilanguageText.Language == language)
                            {
                                result = multilanguageText.Text;
                                break;
                            }
                        }
                    }
                }
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
        public static List<List<string>> GetEnumerationValues(this Property property,
                                                              ISpecIfMetadataReader metadataReader,
                                                              string language = "en")
        {
            List<List<string>> result = new List<List<string>>();

            PropertyClass propertyClass = metadataReader.GetPropertyClassByKey(property.Class);

            DataType dataType = property.GetDataType(metadataReader);

            if (propertyClass != null && dataType != null)
            {
                // enumeration type
                if (dataType.Enumeration != null && dataType.Enumeration.Count > 0)
                {
                    // property with multiple values
                    if (propertyClass.Multiple.HasValue && propertyClass.Multiple.Value)
                    {
                        List<string> enumTexts = new List<string>();

                        foreach (Value value in property.Values)
                        {
                            string id = value.ToSimpleTextString(language);

                            enumTexts.Add(GetEnumTextForIdValue(id, dataType, language));

                            result.Add(enumTexts);
                        }
                    }
                    else // property with single values
                    {
                        string id = property.GetStringValue(metadataReader, language);

                        List<string> enumValues = new List<string>();

                        enumValues.Add(GetEnumTextForIdValue(id, dataType, language));

                        result.Add(enumValues);
                    }

                }
            }

            return result;

        }

        private static string GetEnumTextForIdValue(string idValue,
                                                    DataType dataType,
                                                    string language = "en")
        {
            string result = "";

            EnumerationValue enumValue = null;
            try
            {
                enumValue = dataType.Enumeration.First(e => e.ID == idValue);
            }
            catch
            { }

            if (enumValue != null)
            {
                MultilanguageText enumText = null;

                foreach (MultilanguageText multilanguageText in enumValue.Value)
                {
                    if (language == null || language == "en")
                    {
                        if (multilanguageText.Language == null || multilanguageText.Language == "en")
                        {
                            enumText = multilanguageText;
                            break;
                        }
                    }
                    else
                    {
                        if (multilanguageText.Language != null && multilanguageText.Language == language)
                        {
                            enumText = multilanguageText;
                            break;
                        }
                    }
                }

                // language not found, take first value as default
                if (enumText == null && enumValue.Value.Count > 0)
                {
                    enumText = enumValue.Value[0];
                }

                if (enumText != null)
                {
                    result = enumText.Text;
                }
            }

            return result;
        }
    }
}
