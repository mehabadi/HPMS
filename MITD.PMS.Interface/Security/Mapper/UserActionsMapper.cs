﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MITD.Core;
using MITD.PMS.Presentation.Contracts;
using MITD.PMSSecurity.Domain;

namespace MITD.PMS.Interface
{

    public class UserActionsMapper : BaseMapper<List<ActionType>, ClaimsPrincipal>, IMapper<List<ActionType>, System.Security.Claims.ClaimsPrincipal>
    {
        public override ClaimsPrincipal MapToModel(List<ActionType> entity)
        {
            throw new NotSupportedException("map List<User> to ClaimsPrincipal not supported  ");
        }

        public override List<ActionType> MapToEntity(ClaimsPrincipal user)
        {
            var res = new List<ActionType>();
            var claimUserActions = user.Claims.SingleOrDefault(c => c.Type == "CurrentUserActions");
            if (claimUserActions == null || string.IsNullOrWhiteSpace(claimUserActions.Value))
                return res;
            // var lNameClaim = user.Claims.SingleOrDefault(c => c.Type == "http://identityserver.thinktecture.com/claims/profileclaims/lastname");

            foreach (var actionCode in claimUserActions.Value.Split(','))
            {
                var actionType = (ActionType)int.Parse(actionCode);
                if (actionType != null)
                    res.Add(actionType);
            }
            return res;
        }
    }



}
