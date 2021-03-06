﻿using System.Linq;
using Castle.Core;
using MITD.Core;
using MITD.Core.RuleEngine;
using MITD.Core.RuleEngine.Model;
using MITD.PMS.Presentation.Contracts;
using MITD.PMSAdmin.Application.Contracts;
using MITD.PMSAdmin.Domain.Model.Policies;
using MITD.PMSSecurity.Domain;

namespace MITD.PMS.Interface
{

    [Interceptor(typeof(Interception))]
    public class FunctionFacadeService : IFunctionFacadeService
    {
        #region Fields

        private readonly IMapper<RuleFunctionBase, FunctionDTODescriptionWithActions> functionWithActionMapper;
        private readonly IMapper<RuleFunctionBase, FunctionDTO> functionMapper;
        private readonly IFunctionService functionService;
        private readonly IPolicyRepository policyRep;
        private readonly IRuleService ruleService; 

        #endregion

        #region Ctor
        public FunctionFacadeService(IMapper<RuleFunctionBase, FunctionDTODescriptionWithActions> functionWithActionMapper,
                                        IMapper<RuleFunctionBase, FunctionDTO> functionMapper,
                                        IFunctionService functionService,
                                        IPolicyRepository policyRep, IRuleService ruleService)
        {
            this.functionWithActionMapper = functionWithActionMapper;
            this.functionMapper = functionMapper;
            this.functionService = functionService;
            this.policyRep = policyRep;
            this.ruleService = ruleService;
        } 
        #endregion

        #region Public Methods
        [RequiredPermission(ActionType.ManageFunctions)]
        public PolicyFunctions GetPolicyFunctionsWithPagination(long policyId)
        {
            var ruleBasePolicy = policyRep.GetRuleBasePolicyById(new PolicyId(policyId));
            var res = ruleService.FindWithPagingBy(ruleBasePolicy.RuleFunctions.ToList());
            return new PolicyFunctions
            {
                PolicyName = ruleBasePolicy.Name,
                PolicyId = ruleBasePolicy.Id.Id,
                Functions = res.Select(f => functionWithActionMapper.MapToModel(f)).ToList()
            };


        }

        [RequiredPermission(ActionType.CreateFunction)]
        public FunctionDTO AddFunction(FunctionDTO dto)
        {
            var res = functionService.AddFunction(dto.Name, dto.Content, new PolicyId(dto.PolicyId));
            return functionMapper.MapToModel(res);
        }

        [RequiredPermission(ActionType.ModifyFunction)]
        public FunctionDTO UpdateFunction(FunctionDTO dto)
        {

            var res = functionService.UppdateFunction(new RuleFunctionId(dto.Id), dto.Name, dto.Content);
            return functionMapper.MapToModel(res);
        }

        [RequiredPermission(ActionType.DeleteFunction)]
        public string DeleteFunction(long policyId, long id)
        {
            functionService.DeleteFunction(new PolicyId(policyId), new RuleFunctionId(id));
            return "Function with " + id + "Deleted";
        }

        public FunctionDTO GetFunctionById(long id)
        {
            RuleFunctionBase function = ruleService.GetById(new RuleFunctionId(id));
            return functionMapper.MapToModel(function);
        } 
        #endregion

    }
}
