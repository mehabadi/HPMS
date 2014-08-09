﻿using MITD.Domain.Repository;
using MITD.PMS.Domain.Model.Calculations;
using MITD.PMS.Domain.Model.Periods;
using MITD.PMSReport.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MITD.PMS.Domain.Model.JobIndexPoints
{
    public interface IJobIndexPointRepository : IRepository
    {
        void Add(CalculationPoint jobIndexpoint);

        IList<JobIndexPointWithEmployee> Find<T>(Expression<Func<T, bool>> predicate,
            IListFetchStrategy<JobIndexPointWithEmployee> fetchStrategy) where T : EmployeePoint;

        CalculationPointId GetNextId();


        CalculationPoint GetById(CalculationPointId id);
        List<SummaryCalculationPoint> GetCalculationPointBy(CalculationId calcId);
        void ResetAllInquiryPoints(Period period);

    }
}
