﻿using System;
using System.IO;
using System.Linq;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.SSOM.Extensions;
using SPMeta2.Utils;
using System.Web.UI.WebControls.WebParts;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Common;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class WebPartModelHandler : ModelHandlerBase
    {
        #region properties

        private const string WebPartPageCmdTemplate =
                                          "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                          "<Method ID=\"0, NewWebPage\">" +
                                          "<SetList Scope=\"Request\">{0}</SetList>" +
                                          "<SetVar Name=\"ID\">New</SetVar>" +
                                          "<SetVar Name=\"Title\">{1}</SetVar>" +
                                          "<SetVar Name=\"Cmd\">NewWebPage</SetVar>" +
                                          "<SetVar Name=\"WebPartPageTemplate\">{2}</SetVar>" +
                                          "<SetVar Name=\"Type\">WebPartPage</SetVar>" +
                                          "<SetVar Name=\"Overwrite\">{3}</SetVar>" +
                                          "</Method>";

        #endregion

        #region methods

        public override Type TargetType
        {
            get { return typeof(WebPartPageDefinition); }
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var host = modelHost.WithAssertAndCast<WebpartPageModelHost>("modelHost", value => value.RequireNotNull());
            var webpartPageModel = model.WithAssertAndCast<WebPartDefinition>("model", value => value.RequireNotNull());

            InvokeOnModelEvents<FieldDefinition, SPField>(null, ModelEventType.OnUpdating);
            WebPartExtensions.DeployWebPartsToPage(host.SPLimitedWebPartManager, new[] { webpartPageModel },
                onUpdatingWebpartInstnce =>
                {
                    InvokeOnModelEvents<WebPartDefinition, WebPart>(onUpdatingWebpartInstnce, ModelEventType.OnUpdating);

                },
                onUpdatedWebpartInstnce =>
                {
                    InvokeOnModelEvents<WebPartDefinition, WebPart>(onUpdatedWebpartInstnce, ModelEventType.OnUpdated);
                });

        }

        #endregion
    }
}
