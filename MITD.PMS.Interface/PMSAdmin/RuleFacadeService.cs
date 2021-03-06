﻿using System.Linq;
using Castle.Core;
using MITD.Core;
using MITD.Core.RuleEngine;
using MITD.Core.RuleEngine.Model;
using MITD.Domain.Repository;
using MITD.PMS.Presentation.Contracts;
using MITD.PMSAdmin.Application.Contracts;
using MITD.PMSAdmin.Domain.Model.Policies;
using MITD.PMSAdminReport.Domain.Model;
using MITD.PMSSecurity.Domain;

namespace MITD.PMS.Interface
{
    [Interceptor(typeof(Interception))]
    public class RuleFacadeService : IRuleFacadeService
    {
        #region Fields
        private readonly IMapper<RuleBase, RuleDTOWithAction> ruleWithActionMapper;
        private readonly IMapper<RuleWithPolicyData, RuleDTO> ruleMapper;
        private readonly IMapper<RuleTrail, RuleTrailDTO> ruleTrailMapper;
        private readonly IMapper<RuleTrail, RuleTrailDTOWithAction> ruleTrailWithActionsMapper;
        private readonly IPMSRuleService pmsRuleService;
        private readonly IRuleService ruleService;
        private readonly IPolicyRepository policyRep; 
        #endregion

        #region Ctor
        public RuleFacadeService(IMapper<RuleBase, RuleDTOWithAction> ruleWithActionMapper,
                                IMapper<RuleWithPolicyData, RuleDTO> ruleMapper,
                                IMapper<RuleTrail, RuleTrailDTOWithAction> ruleTrailWithActionsMapper,
                                IMapper<RuleTrail, RuleTrailDTO> ruleTrailMapper,
                                IPMSRuleService pmsRuleService,
                                IPolicyRepository policyRep, IRuleService ruleService)
        {
            this.ruleWithActionMapper = ruleWithActionMapper;
            this.ruleTrailWithActionsMapper = ruleTrailWithActionsMapper;
            this.ruleTrailMapper = ruleTrailMapper;
            this.ruleMapper = ruleMapper;
            this.pmsRuleService = pmsRuleService;
            this.policyRep = policyRep;
            this.ruleService = ruleService;
        } 
        #endregion

        #region Public methods

        [RequiredPermission(ActionType.ManageRules)]
        public PolicyRules GetPolicyRulesWithPagination(long policyId)
        {

            //todo:(LOW) check for correctness
            var ruleBasePolicy = policyRep.GetRuleBasePolicyById(new PolicyId(policyId));
            var res = ruleService.FindWithPagingBy(ruleBasePolicy.Rules.ToList());
            return new PolicyRules
            {
                PolicyName = ruleBasePolicy.Name,
                PolicyId = ruleBasePolicy.Id.Id,
                Rules = res.Select(f => ruleWithActionMapper.MapToModel(f)).ToList()
            };


        }

        [RequiredPermission(ActionType.ManageRules)]
        [RequiredPermission(ActionType.ShowAllRuleTrails)]
        public PageResultDTO<RuleTrailDTOWithAction> GetRuleTrailsWithPagination(long ruleId, int pageSize, int pageIndex)
        {
            //todo:(LOW) check for correctness
            //var fs = new ListFetchStrategy<RuleTrail>();
            //fs.OrderByDescending(f => f.EffectiveDate).WithPaging(pageSize, pageIndex);
            //ruleService.GetRuleTrails(new RuleId(ruleId), fs);
            //var res = new PageResultDTO<RuleTrailDTOWithAction>();
            //res.InjectFrom(fs.PageCriteria.PageResult);
            //res.Result = fs.PageCriteria.PageResult.Result.Select(r => ruleTrailWithActionsMapper.MapToModel(r));
            //return res;

            var res = new PageResultDTO<RuleTrailDTOWithAction>();
            var ruleTrailList = ruleService.GetRuleTrails(new RuleId(ruleId), new ListFetchStrategy<RuleTrail>()).OrderByDescending(r => r.EffectiveDate);
            res.PageSize = 100;
            res.CurrentPage = 1;
            res.Result = ruleTrailList.Select(r => ruleTrailWithActionsMapper.MapToModel(r)).ToList();
            return res;
        }

        [RequiredPermission(ActionType.AddRule)]
        public RuleDTO AddRule(RuleDTO dto)
        {
            var res = pmsRuleService.AddRule(dto.Name, dto.RuleText, Enumeration.FromValue<RuleType>(dto.ExcuteTime.ToString()), new PolicyId(dto.PolicyId), dto.ExcuteOrder);
            return ruleMapper.MapToModel(res);
        }

        [RequiredPermission(ActionType.ModifyRule)]
        public RuleDTO UpdateRule(RuleDTO dto)
        {

            var res = pmsRuleService.UpdateRule(new PolicyId(dto.PolicyId), new RuleId(dto.Id), dto.Name, dto.RuleText, Enumeration.FromValue<RuleType>(dto.ExcuteTime.ToString()), dto.ExcuteOrder);
            return ruleMapper.MapToModel(res);
        }

        [RequiredPermission(ActionType.DeleteRule)]
        public string DeleteRule(long policyId, long id)
        {
            pmsRuleService.DeleteRule(new PolicyId(policyId), new RuleId(id));
            return "Rule with " + id + "Deleted";
        }

        public RuleDTO GetRuleById(long policyId, long id)
        {
            var rule = pmsRuleService.GetRuleById(new PolicyId(policyId), new RuleId(id));
            return ruleMapper.MapToModel(rule);
        }

        public RuleTrailDTO GetRuleTrail(long ruleTrailId)
        {
            var ruleTrail = (RuleTrail)ruleService.GetById(new RuleId(ruleTrailId));
            return ruleTrailMapper.MapToModel(ruleTrail);
        }

        [RequiredPermission(ActionType.ManageRules)]
        public string CompileRule(long policyId, string ruleContent)
        {
            pmsRuleService.CompileRule(new PolicyId(policyId), ruleContent);
            return "compiled successfully";
        } 

        #endregion

       
    }
}
