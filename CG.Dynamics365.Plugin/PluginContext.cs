using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CG.Dynamics365.Plugins.Integration
{
    public class PluginContextMessage
    {
        public const string CREATE = "CREATE";
        public const string UPDATE = "UPDATE";
        public const string DELETE = "DELETE";
        public const string GRANT_ACCESS = "GRANTACCESS";
        public const string MODIFY_ACCESS = "MODIFYACCESS";
        public const string RETRIEVE = "RETRIEVE";
        public const string RETRIEVE_MULTIPLE = "RETRIEVEMULTIPLE";
        public const string RETRIEVE_SHARED_PRINCIPALS_AND_ACCESS = "RETRIEVESHAREDPRINCIPALSANDACCESS";
        public const string REVOKE_ACCESS = "REVOKEACCESS";
        public const string SET_STATE = "SETSTATE";
        public const string ADD_ITEM = "ADDITEM";
        public const string ADD_LIST_MEMBERS= "ADDLISTMEMBERS";
        public const string ADD_MEMBER= "ADDMEMBER";
        public const string ADD_MEMBERS= "ADDMEMBERS";
        public const string ADD_PRINCIPAL_TO_QUEUE= "ADDPRINCIPALTOQUEUE";
    }

    public class PluginContext : IPluginExecutionContext
    {
        private readonly IServiceProvider serviceProvider;

        public PluginContext(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        #region Properties

        private ITracingService tracingService;
        public ITracingService TracingService
        {
            get
            {
                return tracingService ??
                                  (tracingService = (ITracingService)this.serviceProvider.GetService(typeof(ITracingService)));
            }
        }

        private IPluginExecutionContext context;
        public IPluginExecutionContext Context
        {
            get
            {
                return context ??
                                  (context = (IPluginExecutionContext)this.serviceProvider.GetService(typeof(IPluginExecutionContext)));
            }
        }

        private IOrganizationServiceFactory factory;
        public IOrganizationServiceFactory Factory
        {
            get
            {
                return factory ??
                                  (factory = (IOrganizationServiceFactory)this.serviceProvider.GetService(typeof(IOrganizationServiceFactory)));
            }
        }
        #endregion

        #region OrgService

        private IOrganizationService crmServiceUser;
        public IOrganizationService CrmServiceUser
        {
            get { return crmServiceUser ?? (crmServiceUser = Factory.CreateOrganizationService(this.Context.UserId)); }
        }

        private IOrganizationService crmServiceSystem;
        public IOrganizationService ServiceSystem
        {
            get { return crmServiceSystem ?? (crmServiceSystem = Factory.CreateOrganizationService(null)); }
        }

        #endregion

        #region Implementation of IExecutionContext

        public int Mode
        {
            get { return this.Context.Mode; }
        }

        public int IsolationMode
        {
            get { return this.Context.IsolationMode; }
        }

        public int Depth
        {
            get { return this.Context.Depth; }
        }

        public string MessageName
        {
            get { return this.Context.MessageName; }
        }

        public string PrimaryEntityName
        {
            get { return this.Context.PrimaryEntityName; }
        }

        public Guid? RequestId
        {
            get { return this.Context.RequestId; }
        }

        public string SecondaryEntityName
        {
            get { return this.Context.SecondaryEntityName; }
        }

        public ParameterCollection InputParameters
        {
            get { return this.Context.InputParameters; }
        }

        public ParameterCollection OutputParameters
        {
            get { return this.Context.OutputParameters; }
        }

        public ParameterCollection SharedVariables
        {
            get { return this.Context.SharedVariables; }
        }

        public Guid UserId
        {
            get { return this.Context.UserId; }
        }

        public Guid InitiatingUserId
        {
            get { return this.Context.InitiatingUserId; }
        }

        public Guid BusinessUnitId
        {
            get { return this.Context.BusinessUnitId; }
        }

        public Guid OrganizationId
        {
            get { return this.Context.OrganizationId; }
        }

        public string OrganizationName
        {
            get { return this.Context.OrganizationName; }
        }

        public Guid PrimaryEntityId
        {
            get { return this.Context.PrimaryEntityId; }
        }

        public EntityImageCollection PreEntityImages
        {
            get { return this.Context.PreEntityImages; }
        }

        public EntityImageCollection PostEntityImages
        {
            get { return this.Context.PostEntityImages; }
        }

        public EntityReference OwningExtension
        {
            get { return this.Context.OwningExtension; }
        }

        public Guid CorrelationId
        {
            get { return this.Context.CorrelationId; }
        }

        public bool IsExecutingOffline
        {
            get { return this.Context.IsExecutingOffline; }
        }

        public bool IsOfflinePlayback
        {
            get { return this.Context.IsOfflinePlayback; }
        }

        public bool IsInTransaction
        {
            get { return this.Context.IsInTransaction; }
        }

        public Guid OperationId
        {
            get { return this.Context.OperationId; }
        }

        public DateTime OperationCreatedOn
        {
            get { return this.Context.OperationCreatedOn; }
        }

        #endregion

        #region Implementation of IPluginExecutionContext

        public int Stage
        {
            get { return this.Context.Stage; }
        }

        public IPluginExecutionContext ParentContext
        {
            get { return this.Context.ParentContext; }
        }

        #endregion

        public Entity GetTarget()
        {
            return this.InputParameters["Target"] as Entity;
        }

        public Entity GetPostImage()
        {
            var imageName = string.Format("{0}_post", this.Context.PrimaryEntityName);
            return this.PostEntityImages.Contains(imageName) ? this.PostEntityImages[imageName] : null;
        }

        public Entity GetPreImage()
        {
            var imageName = string.Format("{0}_pre", this.Context.PrimaryEntityName);
            return this.PreEntityImages.Contains(imageName) ? this.PreEntityImages[imageName] : null;
        }

        public T GetAttribute<T>(EntityReference entityReference, string fieldName)
        {
            var entity = this.ServiceSystem.Retrieve(entityReference.LogicalName, entityReference.Id, new ColumnSet(fieldName));
            return entity.GetAttributeValue<T>(fieldName);
        }

        public Tuple<T1, T2> GetAttribute<T1, T2>(EntityReference entityReference, string fieldName1, string fieldName2)
        {
            var entity = this.ServiceSystem.Retrieve(entityReference.LogicalName, entityReference.Id, new ColumnSet(fieldName1, fieldName2));
            var f1 = entity.GetAttributeValue<T1>(fieldName1);
            var f2 = entity.GetAttributeValue<T2>(fieldName2);
            return Tuple.Create(f1, f2);
        }

        public bool IsMessage(string message)
        {
            return String.Equals(this.context.MessageName, message, StringComparison.OrdinalIgnoreCase);
        }
    }
}
