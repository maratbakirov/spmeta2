﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Utils;

namespace SPMeta2.Services.ServiceModelHandlers
{
    public class DefaultRequiredPropertiesModelHandler : ServiceModelHandlerBase
    {
        #region classes

        protected class PropResult
        {
            public string Name { get; set; }
            public bool IsValid { get; set; }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            //var aggregateException = new c();
            var exceptions = new List<SPMeta2ModelValidationException>();

            var props = ReflectionUtils.GetPropertiesWithCustomAttribute<ExpectRequired>(model, true);

            var requiredPropsGroups = props
                             .GroupBy(g => (g.GetCustomAttributes(typeof(ExpectRequired), true).First() as ExpectRequired).GroupName);

            foreach (var group in requiredPropsGroups)
            {
                // all set is requred
                if (string.IsNullOrEmpty(group.Key))
                {
                    var isAllValid = AllOfThem(model, group.ToList());

                    if (isAllValid.Count > 0)
                    {
                        exceptions.AddRange(isAllValid);
                    }
                }
                else
                {
                    // skip 'Web part content' for typed web part definitions
                    //  a big todo

                    if (group.Key == "Web part content" && (model.GetType() != typeof(WebPartDefinition)))
                        continue;

                    var oneOfThem = OneOfThem(model, group.ToList());

                    if (!oneOfThem)
                    {
                        var ex = new SPMeta2ModelValidationException(
                            string.Format("One of the properties with [{0}] attribute should be set. Definition:[{1}]",
                            group.Key, model))
                        {
                            Definition = model
                        };

                        exceptions.Add(ex);
                    }
                }
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException("Required properties validation error", exceptions);
            }
        }

        private bool OneOfThem(object obj, List<PropertyInfo> props)
        {
            var result = ValidateProps(obj, props);

            return result.Any(p => p.IsValid);
        }

        private List<SPMeta2ModelValidationException> AllOfThem(object obj, List<PropertyInfo> props)
        {
            var r = new List<SPMeta2ModelValidationException>();

            var result = ValidateProps(obj, props);

            foreach (var res in result.Where(rr => !rr.IsValid))
            {
                r.Add(new SPMeta2ModelValidationException(string.Format("Property [{0}] is not valid.",
                    res.Name))
                {
                    Definition = obj as DefinitionBase
                });
            }

            return r;
        }

        protected List<PropResult> ValidateProps(object obj, List<PropertyInfo> props)
        {
            var results = new List<PropResult>();

            foreach (var prop in props)
            {
                var result = new PropResult();

                result.Name = prop.Name;
                result.IsValid = true;

                if (prop.PropertyType == typeof(string))
                {
                    var value = prop.GetValue(obj, null) as string;

                    if (!string.IsNullOrEmpty(value))
                    { }
                    else
                    {
                        result.IsValid = false;
                    }
                }
                else if (prop.PropertyType == typeof(Guid) ||
                   prop.PropertyType == typeof(Guid?))
                {
                    var value = prop.GetValue(obj, null) as Guid?;

                    if (value.HasValue && value.Value != default(Guid))
                    { }
                    else
                    {
                        result.IsValid = false;
                    }
                }
                else if (prop.PropertyType == typeof(int) ||
                   prop.PropertyType == typeof(int?))
                {
                    var value = prop.GetValue(obj, null) as int?;

                    if (value.HasValue && value.Value > 0)
                    { }
                    else
                    {
                        result.IsValid = false;
                    }
                }
                else if (prop.PropertyType == typeof(byte[]))
                {
                    var value = prop.GetValue(obj, null) as byte[];

                    if (value != null && value.Count() > 0)
                    { }
                    else
                    {
                        result.IsValid = false;
                    }
                }
                else if (prop.PropertyType == typeof(object))
                {
                    var value = prop.GetValue(obj, null) as object;

                    if (value != null)
                    { }
                    else
                    {
                        result.IsValid = false;
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }

                results.Add(result);
            }

            return results;
        }

        #endregion
    }
}
