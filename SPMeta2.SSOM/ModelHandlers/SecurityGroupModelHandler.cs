﻿using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class SecurityGroupModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(SecurityGroupDefinition); }
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var web = ExtractWeb(modelHost);

            if (web != null)
            {
                string securityGroupName;

                if (model is SecurityGroupLinkDefinition)
                    securityGroupName = (model as SecurityGroupLinkDefinition).SecurityGroupName;
                else if (model is SecurityGroupDefinition)
                    securityGroupName = (model as SecurityGroupDefinition).Name;
                else
                {
                    throw new ArgumentException("model has to be SecurityGroupDefinition or SecurityGroupLinkDefinition");
                }

                var securityGroup = web.SiteGroups[securityGroupName];

                var newModelHost = new SecurityGroupModelHost
                {
                    SecurityGroup = securityGroup,
                    SecurableObject = modelHost as SPSecurableObject
                };

                action(newModelHost);
            }
            else
            {
                action(modelHost);
            }
        }

        private SPWeb ExtractWeb(object modelHost)
        {
            if (modelHost is SPWeb)
                return modelHost as SPWeb;

            if (modelHost is SPList)
                return (modelHost as SPList).ParentWeb;

            if (modelHost is SPListItem)
                return (modelHost as SPListItem).ParentList.ParentWeb;

            throw new Exception(string.Format("modelHost with type [{0}] is not supported.", modelHost.GetType()));
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var web = modelHost.WithAssertAndCast<SPWeb>("modelHost", value => value.RequireNotNull());
            var securityGroupModel = model.WithAssertAndCast<SecurityGroupDefinition>("model", value => value.RequireNotNull());

            //var site = web.Site;
            var currentGroup = (SPGroup)null;

            try
            {
                currentGroup = web.SiteGroups[securityGroupModel.Name];
            }
            catch (SPException)
            {
                var ownerUser = EnsureOwnerUser(web, securityGroupModel);
                var defaultUser = EnsureDefaultUser(web, securityGroupModel);

                web.SiteGroups.Add(securityGroupModel.Name, ownerUser, defaultUser, securityGroupModel.Description);
                currentGroup = web.SiteGroups[securityGroupModel.Name];
            }

            currentGroup.Owner = EnsureOwnerUser(web, securityGroupModel);
            currentGroup.Description = securityGroupModel.Description;

            currentGroup.Update();
        }

        protected virtual SPUser EnsureOwnerUser(SPWeb web, SecurityGroupDefinition groupModel)
        {
            if (string.IsNullOrEmpty(groupModel.Owner))
            {
                return web.Site.Owner;
            }
            else
            {
                return web.EnsureUser(groupModel.Owner);
            }
        }

        protected virtual SPUser EnsureDefaultUser(SPWeb web, SecurityGroupDefinition groupModel)
        {
            if (string.IsNullOrEmpty(groupModel.DefaultUser))
            {
                return null;
            }
            else
            {
                return web.EnsureUser(groupModel.DefaultUser);
            }
        }

        #endregion
    }
}
