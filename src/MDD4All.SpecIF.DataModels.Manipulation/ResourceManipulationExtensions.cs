﻿/*
 * Copyright (c) MDD4All.de, Dr. Oliver Alt
 */
using MDD4All.SpecIF.DataModels.Helpers;
using MDD4All.SpecIF.DataProvider.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MDD4All.SpecIF.DataModels.Manipulation
{
    public static class ResourceManipulationExtensions
    {
        public static string GetTypeName(this Resource resource, ISpecIfMetadataReader dataProvider)
        {
            string result = "";

            try
            {
                ResourceClass resourceType = null;

                if (resource.GetType() == typeof(Resource))
                {
                    resourceType = dataProvider.GetResourceClassByKey(resource.Class);
                }
                else if (resource.GetType() == typeof(Statement))
                {
                    resourceType = dataProvider.GetStatementClassByKey(resource.Class);
                }


                if (resourceType != null)
                {
                    if (resourceType.Title is string)
                    {
                        result = resourceType.Title.ToString();
                    }
                    //result = resourceType.Title.LanguageValues[0];
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Error with getTypeName() " + exception);
            }
            return result;
        }

        public static string GetClassRevision(this Resource resource, ISpecIfMetadataReader metadataReader)
        {
            string result = "";

            try
            {
                ResourceClass resourceType = null;

                if (resource.GetType() == typeof(Resource))
                {
                    resourceType = metadataReader.GetResourceClassByKey(resource.Class);
                }
                else if (resource.GetType() == typeof(Statement))
                {
                    resourceType = metadataReader.GetStatementClassByKey(resource.Class);
                }


                if (resourceType != null)
                {
                    if (!string.IsNullOrEmpty(resourceType.Revision))
                    {
                        result = resourceType.Revision;
                    }

                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Error with getTypeName() " + exception);
            }
            return result;
        }

        public static ResourceClass GetResourceType(this Resource resource, ISpecIfMetadataReader dataProvider)
        {
            ResourceClass result = null;

            result = dataProvider.GetResourceClassByKey(resource.Class);

            return result;
        }

        public static void SetPropertyValue(this Resource resource,
                                            string propertyTitle,
                                            string stringValue,
                                            ISpecIfMetadataReader metadataProvider,
                                            string format = "plain")
        {
            ResourceClass resourceClass = metadataProvider.GetResourceClassByKey(resource.Class);

            Value value = new Value();

            foreach (Key propertyClassKey in resourceClass.PropertyClasses)
            {
                PropertyClass propertyClass = metadataProvider.GetPropertyClassByKey(propertyClassKey);

                if (propertyClass.Title == propertyTitle)
                {
                    DataType dataType = metadataProvider.GetDataTypeByKey(propertyClass.DataType);

                    if (dataType.Type == "xs:string")
                    {
                        MultilanguageText multilanguageText = new MultilanguageText
                        {
                            Text = stringValue,
                            Format = format
                        };

                        value = new Value(multilanguageText);
                    }
                    else
                    {
                        value = new Value(stringValue);
                    }
                }
            }

            SetPropertyValue(resource, propertyTitle, value, metadataProvider);
        }

        public static void SetPropertyValue(this Resource resource,
                                            string propertyTitle,
                                            Value value,
                                            ISpecIfMetadataReader metadataProvider)
        {
            bool propertyFound = false;

            foreach (Property property in resource.Properties)
            {

                PropertyClass propertyClass = metadataProvider.GetPropertyClassByKey(property.Class);

                if (propertyClass != null)
                {
                    if (propertyClass.Title == propertyTitle)
                    {
                        if (property.Values == null)
                        {
                            property.Values = new List<Value>();
                        }
                        if (property.Values.Count == 0)
                        {
                            property.Values.Add(value);
                        }
                        else
                        {
                            property.Values[0] = value;
                        }
                        propertyFound = true;
                        break;
                    }
                }

            }

            if (!propertyFound)
            {
                ResourceClass resourceType = metadataProvider.GetResourceClassByKey(resource.Class);

                if (resourceType != null)
                {
                    PropertyClass matchingPropertyClass = null;
                    Key matchingPropertyKey = null;


                    foreach (Key propertyKey in resourceType.PropertyClasses)
                    {
                        PropertyClass propertyClass = metadataProvider.GetPropertyClassByKey(propertyKey);

                        if (propertyClass.Title == propertyTitle)
                        {
                            matchingPropertyClass = propertyClass;
                            matchingPropertyKey = propertyKey;
                            break;
                        }

                    }

                    if (matchingPropertyClass != null)
                    {
                        Property property = new Property()
                        {
                            Class = matchingPropertyKey,
                            Values = new List<Value> { value }
                        };

                        resource.Properties.Add(property);
                    }
                }


            }
        }

        public static void SetPropertyValue(this Resource resource, Key propertyClassKey, Value value)
        {
            bool propertyFound = false;

            foreach (Property property in resource.Properties)
            {
                if (property.Class.ID == propertyClassKey.ID && property.Class.Revision == propertyClassKey.Revision)
                {
                    if (property.Values == null)
                    {
                        property.Values = new List<Value>();
                    }
                    if (property.Values.Count == 0)
                    {
                        property.Values.Add(value);
                    }
                    else
                    {
                        property.Values[0] = value;
                    }
                    propertyFound = true;
                    break;
                }
            }

            if (!propertyFound)
            {

                Property property = new Property()
                {

                    Class = propertyClassKey,
                    Values = new List<Value> { value }
                };

                resource.Properties.Add(property);
            }

        }


        public static string GetPropertyValue(this Resource resource, string propertyTitle, ISpecIfMetadataReader dataProvider,
                                              string language = "en")
        {
            string result = "";

            if (resource != null && resource.Properties != null)
            {
                foreach (Property property in resource.Properties)
                {
                    PropertyClass propertyClass = dataProvider.GetPropertyClassByKey(property.Class);

                    if (propertyClass != null)
                    {

                        if (propertyClass.Title == propertyTitle)
                        {
                            result = property.GetStringValue(dataProvider, language);
                            break;
                        }
                    }
                }
            }

            return result;
        }

        public static List<Value> GetPropertyValue(this Resource resource, Key propertyClassKey)
        {
            List<Value> result = new List<Value>();

            if (resource != null && resource.Properties != null)
            {
                foreach (Property property in resource.Properties)
                {


                    if (propertyClassKey.ID == property.Class.ID && propertyClassKey.Revision == property.Class.Revision)
                    {
                        result = property.Values;
                        break;
                    }
                }
            }

            return result;
        }

        public static string GetSingleStringPropertyValue(this Resource resource, Key propertyClassKey, string language = "en")
        {
            string result = null;

            if (resource != null && resource.Properties != null)
            {
                foreach (Property property in resource.Properties)
                {
                    if (propertyClassKey.ID == property.Class.ID && propertyClassKey.Revision == property.Class.Revision)
                    {
                        List<Value> values = property.Values;

                        if(values.Count > 0)
                        {
                            Value firstValue = values[0];
                            result = firstValue.GetDefaultLanguageStringValue();
                        }

                        break;
                    }
                }
            }

            return result;
        }

        public static void SetResourceTitle(this Resource resource, string title)
        {
            //if(resource.Title.LanguageValues.Count > 0)
            //{
            //	foreach(LanguageValue languageValue in resource.Title.LanguageValues)
            //	{
            //		languageValue.Text = title;
            //	}
            //}
            //else
            //{
            //	resource.Title.LanguageValues.Add(new LanguageValue(title));
            //}
        }

        public static Key Key(this Resource resource)
        {
            return new Key(resource.ID, resource.Revision);
        }

        public static Resource CreateNewRevisionForEdit(this Resource resource, ISpecIfMetadataReader metadataReader)
        {
            Resource result = new Resource()
            {
                ID = resource.ID,
                Revision = SpecIfGuidGenerator.CreateNewRevsionGUID(),
                Replaces = new List<string>()
                {
                    resource.Revision
                },
                Class = new Key(resource.Class.ID, resource.Class.Revision),
                ProjectID = resource.ProjectID,
            };

            if (resource.AlternativeIDs != null)
            {
                foreach (AlternativeId alternativeID in resource.AlternativeIDs)
                {
                    result.AlternativeIDs.Add(new AlternativeId()
                    {
                        ID = alternativeID.ID,
                        Project = alternativeID.Project,
                        Revision = alternativeID.Revision
                    });
                }
            }

            foreach (Property property in resource.Properties)
            {
                if (property.Values != null && property.Values.Count > 0)
                {
                    Property clonedProperty = new Property
                    {
                        Class = new Key(property.Class.ID, property.Class.Revision),
                        Values = new List<Value>()
                    };
                    

                    foreach (Value value in property.Values)
                    {
                        Value clonedValue = new Value();
                        clonedValue.StringValue = value.StringValue;
                        foreach (MultilanguageText text in value.MultilanguageTexts)
                        {
                            clonedValue.MultilanguageTexts.Add(text);
                        }
                        clonedProperty.Values.Add(clonedValue);
                    }

                    result.Properties.Add(property);
                }
            }

            return result;
        }
    }
}
