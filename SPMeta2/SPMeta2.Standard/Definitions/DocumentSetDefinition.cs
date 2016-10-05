﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using System.Runtime.Serialization;
using SPMeta2.Attributes.Capabilities;

namespace SPMeta2.Standard.Definitions
{
    /// <summary>
    /// Allows to define and deploy document set.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.Office.DocumentManagement.DocumentSets.DocumentSet", "Microsoft.Office.Server.UserProfiles")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.DocumentSet.DocumentSet", "Microsoft.SharePoint.Client.DocumentManagement")]


    [DefaultParentHost(typeof(WebDefinition))]
    [DefaultRootHost(typeof(WebDefinition))]

    [Serializable]
    [DataContract]
    //[ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(ListDefinition))]
    [ParentHostCapability(typeof(FolderDefinition))]


    [ExpectManyInstances]

    public class DocumentSetDefinition : DefinitionBase
    {
        #region properties

        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        [ExpectRequired]
        public string Name { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectRequired(GroupName = "ContentType")]
        public string ContentTypeId { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectRequired(GroupName = "ContentType")]
        public string ContentTypeName { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<DocumentSetDefinition>(this)
                          .AddPropertyValue(p => p.Name)
                          .AddPropertyValue(p => p.ContentTypeId)
                          .AddPropertyValue(p => p.ContentTypeName)
                          .ToString();
        }

        #endregion
    }
}
