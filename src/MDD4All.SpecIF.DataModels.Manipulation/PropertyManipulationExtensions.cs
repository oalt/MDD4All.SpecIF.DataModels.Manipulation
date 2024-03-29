﻿/*
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
                        }
                        else if(multilanguageText.Language != null)
                        {
                            if (multilanguageText.Language == language)
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

        public static void SetSingleStringValue(this Property property,
                                                string value,
                                                string language = "en",
                                                string format = TextFormat.Plain)
        {
            if (value != null)
            {
                MultilanguageText multilanguageTextValue = new MultilanguageText
                {
                    Text = value,

                };
                if (language != "en")
                {
                    multilanguageTextValue.Language = language;
                }

                if (format != TextFormat.Plain)
                {
                    multilanguageTextValue.Format = format;
                }

                
                if (property.Values.Count > 0)
                {
                    Value firstValue = property.Values[0];

                    if (firstValue.MultilanguageTexts != null && firstValue.MultilanguageTexts.Any())
                    {
                        bool languageFound = false;
                        foreach (MultilanguageText multilanguageText in firstValue.MultilanguageTexts)
                        {
                            if((multilanguageText.Language == null || multilanguageText.Language == "en") && language == "en")
                            {
                                multilanguageText.Text = value;
                                languageFound = true;
                                break;
                            }
                            else if(multilanguageText.Language != null && multilanguageText.Language == language)
                            {
                                multilanguageText.Text = value;
                                languageFound = true;
                                break;
                            }
                        }
                        if(!languageFound)
                        {
                            firstValue.MultilanguageTexts.Add(multilanguageTextValue);
                        }
                    }
                    else
                    {
                        if(firstValue.MultilanguageTexts == null)
                        {
                            firstValue.MultilanguageTexts = new List<MultilanguageText>();
                            firstValue.MultilanguageTexts.Add(multilanguageTextValue);
                        }
                    }
                }
                else
                {
                    Value val = new Value(multilanguageTextValue);
                    property.Values.Add(val);
                }



            }
        }

        public static void SetSingleNonStringValue(this Property property,
                                                   string value)
        {
            if (property.Values.Count > 0)
            {
                property.Values[0].StringValue = value;
            }
            else
            {
                property.Values.Add(new Value(value));
            }
        }

        public static string GetSingleStringValue(this Property property, string language = "en")
        {
            string result = null;

            if (property.Values.Count > 0)
            {
                Value firstValue = property.Values[0];

                if(firstValue.MultilanguageTexts != null && firstValue.MultilanguageTexts.Any())
                {
                    MultilanguageText multilanguageText = null;
                    if (language == "en")
                    {
                        multilanguageText = firstValue.MultilanguageTexts.Find(element => element.Language == null || element.Language == language);
                    }
                    else
                    {
                        multilanguageText = firstValue.MultilanguageTexts.Find(element => element.Language == language);
                    }

                    if(multilanguageText != null)
                    {
                        result = multilanguageText.Text;
                    }
                }
            }


             return result;
        }

        public static List<string> GetStringValues(this Property property, ISpecIfMetadataReader metadataReader, string language = "en")
        {
            List<string> result = new List<string>();

            DataType dataType = property.GetDataType(metadataReader);

            if (property.Values != null)
            {
                foreach (Value value in property.Values)
                {
                    result.Add(value.ToString(language));
                }
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

                        if (property.Values != null)
                        {
                            foreach (Value value in property.Values)
                            {
                                string id = value.ToSimpleTextString(language);

                                enumTexts.Add(GetEnumTextForIdValue(id, dataType, language));

                                result.Add(enumTexts);
                            }
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

        public static string GetSingleEnumerationValue(this Property property,
                                                       int index = 0)
        {
            string result = null;

            if(property != null && property.Values != null && index <= property.Values.Count - 1)
            {
                Value value = property.Values[index];
                if (value != null)
                {
                    result = value.StringValue;
                }
            }

            return result;
        }

        public static List<string> GetMultipleEnumerationValue(this Property property,
                                                         int index = 0)
        {
            List<string> result = new List<string>();
            if (property != null)
            {
                if (property.Values != null && index <= property.Values.Count - 1)
                {
                    Value value = property.Values[index];
                    if (value != null && value.StringValue != null)
                    {
                        string multiValueString = value.StringValue;

                        string[] tokens = multiValueString.Split(new char[] { ',' });

                        foreach (string token in tokens)
                        {
                            result.Add(token);
                        }
                    }
                }
            }

            return result;
        }

        
        public static bool IsDifferentTo(this Property propertyOne, Property propertyTwo)
        {
            bool result = false;

            if(propertyOne != null && propertyTwo != null)
            {
                // only start compare operation if the classes are equal
                if (propertyOne.Class.Equals(propertyTwo.Class))
                {
                    if (propertyOne.Values != null && propertyTwo.Values != null)
                    {
                        if (propertyOne.Values.Count == propertyTwo.Values.Count)
                        {
                            for (int counter = 0; counter < propertyOne.Values.Count; counter++)
                            {
                                Value valueOne = propertyOne.Values[counter];
                                Value valueTwo = propertyTwo.Values[counter];

                                if (valueOne.StringValue != null && valueTwo.StringValue != null)
                                {
                                    if (valueOne.StringValue != valueTwo.StringValue)
                                    {
                                        result = true;
                                    }
                                }
                                else if (valueOne.MultilanguageTexts != null && valueOne.MultilanguageTexts.Any() &&
                                        valueTwo.MultilanguageTexts != null && valueTwo.MultilanguageTexts.Any())
                                {
                                    if (valueOne.MultilanguageTexts.Count == valueTwo.MultilanguageTexts.Count)
                                    {
                                        foreach (MultilanguageText multilanguageTextOne in valueOne.MultilanguageTexts)
                                        {
                                            if (multilanguageTextOne.Language == null)
                                            {
                                                MultilanguageText multilangiageTextTwo = valueTwo.MultilanguageTexts.Find(mlt => mlt.Language == null);
                                                if (multilangiageTextTwo == null)
                                                {
                                                    result = true;
                                                }
                                                else
                                                {
                                                    if (multilanguageTextOne.Text == multilangiageTextTwo.Text &&
                                                       multilanguageTextOne.Format == multilangiageTextTwo.Format)
                                                    {
                                                        // it is equal
                                                    }
                                                    else
                                                    {
                                                        result = true;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                MultilanguageText multilangiageTextTwo = valueTwo.MultilanguageTexts.Find(mlt => mlt.Language == multilanguageTextOne.Language);
                                                if (multilangiageTextTwo == null)
                                                {
                                                    result = true;
                                                }
                                                else
                                                {
                                                    if (multilanguageTextOne.Text == multilangiageTextTwo.Text &&
                                                       multilanguageTextOne.Format == multilangiageTextTwo.Format)
                                                    {
                                                        // it is equal
                                                    }
                                                    else
                                                    {
                                                        result = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        result = true;
                                    }
                                }
                                else
                                {
                                    result = true;
                                }
                            }
                        }
                        else
                        {
                            result = true;
                        }
                    }
                }
            }
            else if(propertyOne == null && propertyTwo == null)
            {
                result = false;
            }
            else
            {
                result = true;
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
                enumValue = dataType.Enumeration.Find(e => e.ID == idValue);
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
