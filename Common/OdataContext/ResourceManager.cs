using System.Collections.ObjectModel;

namespace Common.Api.ExigoOData.ResourceManager
{
    /// <summary>
    /// There are no comments for resourcescontext in the schema.
    /// </summary>
    public partial class resourcescontext 
    {
        
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.ResourceManager in the schema.
    /// </summary>
    /// <KeyProperties>
    /// ResourceID
    /// </KeyProperties>
    public partial class ResourceManager
    {
        
        public int ResourceID
        {
            get;
            set;
        }
        
        public string Title
        {
            get;
            set;
        }
        
        public string Description
        {
            get;
            set;
        }
        
        public string Url
        {
            get;
            set;
        }
        
        public int ResourceCategoryID
        {
            get;
            set;
        }
        
        public int ResourceStatusID
        {
            get;
            set;
        }
       
        public global::System.DateTime CreatedDate
        {
            get;
            set;
        }
        
        public int ResourceActionTypeID
        {
            get;
            set;
        }
        
        public string UploadedFilePath
        {
            get;
            set;
        }
        public int ResourceTypeID
        {
            get;
            set;
        }
        public ResourceManagerCategory ResourceManagerCategory
        {
            get;
            set;
        }
        public ResourceType ResourceType
        {
            get;
            set;
        }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.ResourceManagerCategory in the schema.
    /// </summary>
    /// <KeyProperties>
    /// ResourceCategoryID
    /// </KeyProperties>
    public partial class ResourceManagerCategory
    {
        public int ResourceCategoryID
        {
            get;
            set;
        }
        public string ResourceCategoryDescription
        {
            get;
            set;
        }
        public Collection<ResourceManager> ResourceManagement
        {
            get;
            set;
        }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.ResourceType in the schema.
    /// </summary>
    /// <KeyProperties>
    /// ResourceTypeID
    /// </KeyProperties>
    public partial class ResourceType
    {
        public int ResourceTypeID
        {
            get;
            set;
        }
        public string ResourceTypeDescription
        {
            get;
            set;
        }
        public Collection<ResourceManager> ResourceManagement
        {
            get;
            set;
        }
    }
}