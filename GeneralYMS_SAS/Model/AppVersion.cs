using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneralYMS_SAS.Model
{
    public static class AppVersion
    {
        public static string Version 
        { 
            get 
            {
                if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                {
                    return "Ver"+System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
                }
                else
                    return "";
                
            }
        }
    }
}
