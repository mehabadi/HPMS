﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using MITD.Core;
using MITD.PMS.Application.Contracts;
using MITD.PMS.Interface;
using MITD.PMSSecurity.Domain;
using MITD.PMSSecurity.Exceptions;

namespace MITD.PMS.Service.Host.App_Start
{
    public class ClaimsTransformer : ClaimsAuthenticationManager
    {
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            if (!incomingPrincipal.Identity.IsAuthenticated)
            {
                var res = base.Authenticate(resourceName, incomingPrincipal);
                return res;
            }
            //throw new Exception("hkjljhlkjhlkjhlkj");
            return configureAuthorizationClaim(incomingPrincipal);

        }

        private ClaimsPrincipal configureAuthorizationClaim(ClaimsPrincipal incomingPrincipal)
        {
            string currentUsername;
            var roles = new List<string>();
            var currentUserActions = new List<string>();

            if (isCurrentUserSameAsLogonUser())
            {
                currentUsername = incomingPrincipal.Identity.Name;
                var claimsRoles = incomingPrincipal.Claims.Where(c => c.Type == ClaimTypes.Role);
                if (claimsRoles != null && claimsRoles.Any())
                    roles = claimsRoles.Select(c => c.Value).ToList();
            }
            else
            {
                currentUsername = HttpContext.Current.Request.QueryString["CurrentWorkListUserName"];
                CheckIsValidWorkListCurrentUser(incomingPrincipal.Identity.Name, currentUsername);
                var userService = ServiceLocator.Current.GetInstance<IUserManagementService>();
                
                roles = userService.GetRolesForUser(currentUsername).ToList();
            }


            incomingPrincipal.Identities.First().AddClaim(new Claim("CurrentUsername", currentUsername));
            var strRolesBuilder =mergStringList(roles);
            incomingPrincipal.Identities.First().AddClaim(new Claim("CurrentUserRoles", strRolesBuilder.ToString()));

            if (isCurrentUserSameAsLogonUser())
                addUpdateLogonUser(incomingPrincipal);

            currentUserActions = getCurrentUserActions(incomingPrincipal);
            var strActionsbuilder = mergStringList(currentUserActions);
            incomingPrincipal.Identities.First().AddClaim(new Claim("CurrentUserActions", strActionsbuilder.ToString()));
            
           

            return incomingPrincipal;
        }

        private void CheckIsValidWorkListCurrentUser(string logonUsername,string currentUsername)
        {
            var securityService = SecurityServiceFacadeFactory.Create();
            try
            {
                var workListUsernames = securityService.GetPermittedWorkListUserNameFor(logonUsername);
                if (logonUsername.ToLower() != currentUsername.ToLower()
                    && !workListUsernames.Contains(currentUsername))
                    throw new PMSSecurityException(" دسترسی به کارتابل کاربر " + currentUsername + " مجاز برای کاربر لاگین مجاز نمی باشد ");

            }
            finally
            {
                SecurityServiceFacadeFactory.Release(securityService);
            }
        }
        private bool isCurrentUserSameAsLogonUser()
        {
            if (HttpContext.Current.Request.QueryString.AllKeys.Contains("CurrentWorkListUserName"))
                return false;
            else
                return true;
        }

        private void addUpdateLogonUser(ClaimsPrincipal  incomingPrincipal)
        {
            var securityService = SecurityServiceFacadeFactory.Create();
            try
            {
                securityService.AddUpdateUser(incomingPrincipal);
            }
            finally
            {
                SecurityServiceFacadeFactory.Release(securityService);
            }
        }

        private List<string> getCurrentUserActions(ClaimsPrincipal incomingPrincipal)
        {
            var currentUserActions = new List<string>();
            var securityService = SecurityServiceFacadeFactory.Create();
            try
            {
                var actions = securityService.GetUserAuthorizedActions(incomingPrincipal);
                if (actions != null && actions.Any())
                {
                    currentUserActions = actions.Select(c => c.Value).ToList();
                }
            }
            finally
            {
                SecurityServiceFacadeFactory.Release(securityService);
            }
            return currentUserActions;
        }

        private StringBuilder mergStringList(IEnumerable<string> listString)
        {
            var builder = new StringBuilder();
            foreach (var str in listString)
            {
                builder.Append(str + ",");
            }

            if (builder.Length > 0)
                builder.Remove(builder.Length - 1, 1);
            return builder;
        }
    }
}