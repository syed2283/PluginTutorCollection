using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;
using Microsoft.Xrm.Sdk.Query;
namespace PluginTutorCollection
{
    public class PreEntityImage:IPlugin
    {
        //[CrmPluginRegistration(MessageNameEnum.Create,
        //"contact", StageEnum.PreValidation,
        //ExecutionModeEnum.Synchronous, "name",
        //"DuplicateCheck-contact", 1000,
        //IsolationModeEnum.Sandbox)]
        public void Execute(IServiceProvider serviceProvider)
        {
            // Extract the tracing service for use in debugging sandboxed plug-ins.  
            // If you are not registering the plug-in in the sandbox, then you do  
            // not have to add any tracing service related code.  
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Obtain the organization service reference which you will need for  
            // web service calls.  
            IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);



            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity lead = (Entity)context.InputParameters["Target"];

                try
                {
                    // Plug-in business logic goes here.  
                    //Read form attribute 
                    string modifiednumber = string.Empty;
                    if (lead.Attributes.Contains("telephone1"))
                        modifiednumber = lead.Attributes["telephone1"].ToString();
                    Entity preimiage = (Entity)context.PreEntityImages["PreImage"];
                    string oldnumber= preimiage.Attributes["telephone1"].ToString();

                    throw new InvalidPluginExecutionException("Business phone number changed from"+oldnumber +"to"+ modifiednumber);

                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in MyPlug-in.", ex);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("MyPlugin: {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}
