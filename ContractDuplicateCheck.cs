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
    [CrmPluginRegistration(MessageNameEnum.Create,
        "new_contract", StageEnum.PreValidation,
        ExecutionModeEnum.Synchronous,"name",
        "DuplicateCheck-contract", 1000,
        IsolationModeEnum.Sandbox)]
    public class ContractDuplicateCheck:IPlugin
    {
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
                Entity entity = (Entity)context.InputParameters["Target"];

                try
                {
                    // Plug-in business logic goes here.  
                    //Read form attribute 
                    //throw new InvalidPluginExecutionException("Contract with already exist!!!");
                    ///Commented
                    #region
                                       
                    string comanyName = string.Empty;
                    string companyAddress = string.Empty;
                    string contractType = string.Empty;
                    string isFirstTime = string.Empty;
                    string contractRegion = string.Empty;
                    if (entity.Attributes.Contains("new_contractcompany"))
                        comanyName = entity.Attributes["new_contractcompany"].ToString();
                    if (entity.Attributes.Contains("new_CompanyAddress"))
                        companyAddress = entity.Attributes["new_CompanyAddress"].ToString();
                    if (entity.Attributes.Contains("new_ContractType"))
                        contractType = entity.Attributes["new_ContractType"].ToString();
                    if (entity.Attributes.Contains("new_ContractFirstTime"))
                        isFirstTime = entity.Attributes["new_ContractFirstTime"].ToString();
                    if (entity.Attributes.Contains("new_ContractRegion"))
                        contractRegion = entity.Attributes["new_ContractRegion"].ToString();

                    QueryExpression query = new QueryExpression("new_contract");
                    query.ColumnSet = new ColumnSet(new string[] { "new_contractcompany" });
                    query.ColumnSet = new ColumnSet(new string[] { "new_companyaddress" });
                    query.ColumnSet = new ColumnSet(new string[] { "new_contracttype" });
                    query.ColumnSet = new ColumnSet(new string[] { "new_contractfirsttime" });
                    query.ColumnSet = new ColumnSet(new string[] { "new_contractregion" });
                    query.Criteria = new FilterExpression();
                    //query.Criteria.FilterOperator = LogicalOperator.And;
                    FilterExpression filter1 = query.Criteria.AddFilter(LogicalOperator.And);
                    filter1.Conditions.Add(new ConditionExpression("new_contractcompany", ConditionOperator.Equal, comanyName));
                    filter1.Conditions.Add(new ConditionExpression("new_companyaddress", ConditionOperator.Equal, companyAddress));
                    FilterExpression filter2 = query.Criteria.AddFilter(LogicalOperator.And);
                    filter2.Conditions.Add(new ConditionExpression("new_contracttype", ConditionOperator.Equal, contractType));
                    filter2.Conditions.Add(new ConditionExpression("new_contractfirsttime", ConditionOperator.Equal, isFirstTime));
                    
                    //filter.Conditions.Add(new ConditionExpression("new_contractregion", ConditionOperator.Equal, contractRegion));
                   
                    if (service.RetrieveMultiple(query).Entities.Count > 0)
                        throw new InvalidPluginExecutionException("Contract with given information already exist!!!"+comanyName);

                     
                    #endregion

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
