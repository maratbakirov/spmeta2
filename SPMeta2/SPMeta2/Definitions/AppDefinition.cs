﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Definitions
{

    /// <summary>
    /// Allows to install SharePoint application to the target web.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPAppInstance", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.AppInstance", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebDefinition))]
    public class AppDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// ProductId of the target application.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Target application content.
        /// </summary>
        public byte[] Content { get; set; }


        /// <summary>
        /// A valid Version string of the target application.
        /// </summary>
        public string Version { get; set; }

        #endregion
    }
}
