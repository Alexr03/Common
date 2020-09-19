using System;
using System.Collections.Generic;
using Alexr03.Common.Deployment.Actions;
using Alexr03.Common.Logging;

namespace Alexr03.Common.Deployment
{
    public class DeploymentProfile
    {
        public readonly string Name;

        public readonly List<DeploymentAction> Actions;

        public DeploymentProfile(string name, List<DeploymentAction> actions)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Actions = actions ?? throw new ArgumentNullException(nameof(actions));
        }

        public void Run()
        {
            var logger = Logger.Create<DeploymentProfile>();
            logger.LogMessage("Running " + this.Name);
            foreach (var deploymentAction in this.Actions)
            {
                deploymentAction.Execute();
                logger.LogMessage($"[{deploymentAction.GetType().Name}] - Running!");
            }
        }
    }
}