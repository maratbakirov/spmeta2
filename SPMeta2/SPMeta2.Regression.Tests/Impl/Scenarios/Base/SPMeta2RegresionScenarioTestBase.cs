﻿using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Rnd;
using SPMeta2.Regression.Tests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Exceptions;


namespace SPMeta2.Regression.Tests.Impl.Scenarios.Base
{
    public class SPMeta2RegresionScenarioTestBase : SPMeta2ProvisionRegresionTestBase
    {
        #region constructors

        public SPMeta2RegresionScenarioTestBase()
        {
            RegressionService.ProvisionGenerationCount = 2;
            RegressionService.ShowOnlyFalseResults = false;
        }

        #endregion

        

       
    }
}
