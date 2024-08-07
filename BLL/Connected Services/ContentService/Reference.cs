﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BLL.ContentService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="urn:Core.service.livelink.opentext.com", ConfigurationName="ContentService.ContentService")]
    public interface ContentService {
        
        // CODEGEN: Generating message contract since message DownloadContentRequest has headers
        [System.ServiceModel.OperationContractAttribute(Action="urn:Core.service.livelink.opentext.com/DownloadContent", ReplyAction="urn:Core.service.livelink.opentext.com/ContentService/DownloadContentResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(ServiceDataObject))]
        BLL.ContentService.DownloadContentResponse DownloadContent(BLL.ContentService.DownloadContentRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:Core.service.livelink.opentext.com/DownloadContent", ReplyAction="urn:Core.service.livelink.opentext.com/ContentService/DownloadContentResponse")]
        System.Threading.Tasks.Task<BLL.ContentService.DownloadContentResponse> DownloadContentAsync(BLL.ContentService.DownloadContentRequest request);
        
        // CODEGEN: Generating message contract since message UploadContentRequest has headers
        [System.ServiceModel.OperationContractAttribute(Action="urn:Core.service.livelink.opentext.com/UploadContent", ReplyAction="urn:Core.service.livelink.opentext.com/ContentService/UploadContentResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(ServiceDataObject))]
        BLL.ContentService.UploadContentResponse UploadContent(BLL.ContentService.UploadContentRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:Core.service.livelink.opentext.com/UploadContent", ReplyAction="urn:Core.service.livelink.opentext.com/ContentService/UploadContentResponse")]
        System.Threading.Tasks.Task<BLL.ContentService.UploadContentResponse> UploadContentAsync(BLL.ContentService.UploadContentRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:api.ecm.opentext.com")]
    public partial class OTAuthentication : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string authenticationTokenField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string AuthenticationToken {
            get {
                return this.authenticationTokenField;
            }
            set {
                this.authenticationTokenField = value;
                this.RaisePropertyChanged("AuthenticationToken");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(FileAtts))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:Core.service.livelink.opentext.com")]
    public abstract partial class ServiceDataObject : object, System.ComponentModel.INotifyPropertyChanged {
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:Core.service.livelink.opentext.com")]
    public partial class FileAtts : ServiceDataObject {
        
        private System.Nullable<System.DateTime> createdDateField;
        
        private string fileNameField;
        
        private System.Nullable<long> fileSizeField;
        
        private System.Nullable<System.DateTime> modifiedDateField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=0)]
        public System.Nullable<System.DateTime> CreatedDate {
            get {
                return this.createdDateField;
            }
            set {
                this.createdDateField = value;
                this.RaisePropertyChanged("CreatedDate");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string FileName {
            get {
                return this.fileNameField;
            }
            set {
                this.fileNameField = value;
                this.RaisePropertyChanged("FileName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=2)]
        public System.Nullable<long> FileSize {
            get {
                return this.fileSizeField;
            }
            set {
                this.fileSizeField = value;
                this.RaisePropertyChanged("FileSize");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=3)]
        public System.Nullable<System.DateTime> ModifiedDate {
            get {
                return this.modifiedDateField;
            }
            set {
                this.modifiedDateField = value;
                this.RaisePropertyChanged("ModifiedDate");
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="DownloadContent", WrapperNamespace="urn:Core.service.livelink.opentext.com", IsWrapped=true)]
    public partial class DownloadContentRequest {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="urn:api.ecm.opentext.com")]
        public BLL.ContentService.OTAuthentication OTAuthentication;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:Core.service.livelink.opentext.com", Order=0)]
        public string contextID;
        
        public DownloadContentRequest() {
        }
        
        public DownloadContentRequest(BLL.ContentService.OTAuthentication OTAuthentication, string contextID) {
            this.OTAuthentication = OTAuthentication;
            this.contextID = contextID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="DownloadContentResponse", WrapperNamespace="urn:Core.service.livelink.opentext.com", IsWrapped=true)]
    public partial class DownloadContentResponse {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="urn:api.ecm.opentext.com")]
        public BLL.ContentService.OTAuthentication OTAuthentication;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:Core.service.livelink.opentext.com", Order=0)]
        public System.IO.Stream DownloadContentResult;
        
        public DownloadContentResponse() {
        }
        
        public DownloadContentResponse(BLL.ContentService.OTAuthentication OTAuthentication, System.IO.Stream DownloadContentResult) {
            this.OTAuthentication = OTAuthentication;
            this.DownloadContentResult = DownloadContentResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="UploadContent", WrapperNamespace="urn:Core.service.livelink.opentext.com", IsWrapped=true)]
    public partial class UploadContentRequest {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="urn:api.ecm.opentext.com")]
        public BLL.ContentService.OTAuthentication OTAuthentication;
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="urn:Core.service.livelink.opentext.com")]
        public string contextID;
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="urn:Core.service.livelink.opentext.com")]
        public BLL.ContentService.FileAtts fileAtts;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:Core.service.livelink.opentext.com", Order=0)]
        public System.IO.Stream contents;
        
        public UploadContentRequest() {
        }
        
        public UploadContentRequest(BLL.ContentService.OTAuthentication OTAuthentication, string contextID, BLL.ContentService.FileAtts fileAtts, System.IO.Stream contents) {
            this.OTAuthentication = OTAuthentication;
            this.contextID = contextID;
            this.fileAtts = fileAtts;
            this.contents = contents;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="UploadContentResponse", WrapperNamespace="urn:Core.service.livelink.opentext.com", IsWrapped=true)]
    public partial class UploadContentResponse {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="urn:api.ecm.opentext.com")]
        public BLL.ContentService.OTAuthentication OTAuthentication;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:Core.service.livelink.opentext.com", Order=0)]
        public string UploadContentResult;
        
        public UploadContentResponse() {
        }
        
        public UploadContentResponse(BLL.ContentService.OTAuthentication OTAuthentication, string UploadContentResult) {
            this.OTAuthentication = OTAuthentication;
            this.UploadContentResult = UploadContentResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ContentServiceChannel : BLL.ContentService.ContentService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ContentServiceClient : System.ServiceModel.ClientBase<BLL.ContentService.ContentService>, BLL.ContentService.ContentService {
        
        public ContentServiceClient() {
        }
        
        public ContentServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ContentServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ContentServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ContentServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        BLL.ContentService.DownloadContentResponse BLL.ContentService.ContentService.DownloadContent(BLL.ContentService.DownloadContentRequest request) {
            return base.Channel.DownloadContent(request);
        }
        
        public System.IO.Stream DownloadContent(ref BLL.ContentService.OTAuthentication OTAuthentication, string contextID) {
            BLL.ContentService.DownloadContentRequest inValue = new BLL.ContentService.DownloadContentRequest();
            inValue.OTAuthentication = OTAuthentication;
            inValue.contextID = contextID;
            BLL.ContentService.DownloadContentResponse retVal = ((BLL.ContentService.ContentService)(this)).DownloadContent(inValue);
            OTAuthentication = retVal.OTAuthentication;
            return retVal.DownloadContentResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<BLL.ContentService.DownloadContentResponse> BLL.ContentService.ContentService.DownloadContentAsync(BLL.ContentService.DownloadContentRequest request) {
            return base.Channel.DownloadContentAsync(request);
        }
        
        public System.Threading.Tasks.Task<BLL.ContentService.DownloadContentResponse> DownloadContentAsync(BLL.ContentService.OTAuthentication OTAuthentication, string contextID) {
            BLL.ContentService.DownloadContentRequest inValue = new BLL.ContentService.DownloadContentRequest();
            inValue.OTAuthentication = OTAuthentication;
            inValue.contextID = contextID;
            return ((BLL.ContentService.ContentService)(this)).DownloadContentAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        BLL.ContentService.UploadContentResponse BLL.ContentService.ContentService.UploadContent(BLL.ContentService.UploadContentRequest request) {
            return base.Channel.UploadContent(request);
        }
        
        public string UploadContent(ref BLL.ContentService.OTAuthentication OTAuthentication, string contextID, BLL.ContentService.FileAtts fileAtts, System.IO.Stream contents) {
            BLL.ContentService.UploadContentRequest inValue = new BLL.ContentService.UploadContentRequest();
            inValue.OTAuthentication = OTAuthentication;
            inValue.contextID = contextID;
            inValue.fileAtts = fileAtts;
            inValue.contents = contents;
            BLL.ContentService.UploadContentResponse retVal = ((BLL.ContentService.ContentService)(this)).UploadContent(inValue);
            OTAuthentication = retVal.OTAuthentication;
            return retVal.UploadContentResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<BLL.ContentService.UploadContentResponse> BLL.ContentService.ContentService.UploadContentAsync(BLL.ContentService.UploadContentRequest request) {
            return base.Channel.UploadContentAsync(request);
        }
        
        public System.Threading.Tasks.Task<BLL.ContentService.UploadContentResponse> UploadContentAsync(BLL.ContentService.OTAuthentication OTAuthentication, string contextID, BLL.ContentService.FileAtts fileAtts, System.IO.Stream contents) {
            BLL.ContentService.UploadContentRequest inValue = new BLL.ContentService.UploadContentRequest();
            inValue.OTAuthentication = OTAuthentication;
            inValue.contextID = contextID;
            inValue.fileAtts = fileAtts;
            inValue.contents = contents;
            return ((BLL.ContentService.ContentService)(this)).UploadContentAsync(inValue);
        }
    }
}
