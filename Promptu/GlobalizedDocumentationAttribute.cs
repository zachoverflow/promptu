//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Resources;

//namespace ZachJohnson.Promptu
//{
//    [global::System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
//    public class GlobalizedDocumentationAttribute : DocumentationAttribute
//    {
//        private ResourceManager resourceManager;
//        private string key;

//        public GlobalizedDocumentationAttribute(Type resourceManagerType, string key)
//            : base(String.Empty)
//        {
//            if (resourceManager == null)
//            {
//                throw new ArgumentNullException("resourceManager");
//            }
//            else if (key == null)
//            {
//                throw new ArgumentNullException("key");
//            }

//            this.resourceManager = new ResourceManager(resourceManagerType.FullName, resourceManagerType.Assembly);
//            this.key = key;
//        }

//        protected override string GetDocumentation()
//        {
//            return this.resourceManager.GetObject(this.key).ToString();
//        }
//    }
//}
