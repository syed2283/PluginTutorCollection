using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;
using System.Xml;
namespace PluginTutorCollection
{
    public class CreateTask:IPlugin
    {
        int taxval;
        int gstval;
        string xmlparse = string.Empty;
        public CreateTask(string unsecureval,string secureval)
        {
            taxval = Convert.ToInt32(unsecureval);
             //xmlparse = unsecureval;
            gstval = Convert.ToInt32(secureval);
        }
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
            //Code level impersonating code execution by another user: provide impersonating user Guid in below
            //IOrganizationService adminservice = serviceFactory.CreateOrganizationService(new Guid(""));

            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity entity = (Entity)context.InputParameters["Target"];

                try
                {
                    
                    // Plug-in business logic goes here.  
                    string sharedkey =context.SharedVariables["sharedkey"].ToString();

                    Entity taskrecord = new Entity("task");
                    //single line of text
                    taskrecord.Attributes.Add("subject","Follow up task. This is shared variable value:" + sharedkey);
                    taskrecord.Attributes.Add("description","Please follow up with contact.\n "+
                        " Tax value from unsecure VALUES:"+taxval+"\n" +
                        ". \n GST VALUES from secure VALUES:" + gstval );

                    //Date time
                    taskrecord.Attributes.Add("scheduledend", DateTime.Now.AddDays(2));

                    //option set value as High
                    taskrecord.Attributes.Add("prioritycode",new OptionSetValue(2));

                    //Setting up parent record or lookup
                    //taskrecord.Attributes.Add("regardingobjectid", new EntityReference("contact", entity.Id));
                    //or
                    taskrecord.Attributes.Add("regardingobjectid", entity.ToEntityReference());

                    //Execute by adminservice object for impersonation
                    //serviceadmin.Create(taskrecord);
                    Guid taskGuid = service.Create(taskrecord);

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
