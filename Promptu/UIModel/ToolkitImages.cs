using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel
{
    internal abstract class ToolkitImages
    {
        public ToolkitImages()
        {
        }

        public object Command
        {
            get { return this.CommandCore; } 
        }

        public object History
        {
            get { return this.HistoryCore; }
        }

        public object Namespace
        {
            get { return this.NamespaceCore; }
        }

        public object Function
        {
            get { return this.FunctionCore; }
        }

        public object NativeCommand
        {
            get { return this.NativeCommandCore; }
        }

        public object CommandAndNamespace
        {
            get { return this.CommandAndNamespaceCore; }
        }

        public object FunctionAndNamespace
        {
            get { return this.FunctionAndNamespaceCore; }
        }

        public object NativeCommandAndNamespace
        {
            get { return this.NativeCommandAndNamespaceCore; }
        }

        public object HistoryAndNamespace
        {
            get { return this.HistoryAndNamespaceCore; }
        }

        public object NewCommand
        {
            get { return this.NewCommandCore; }
        }

        public object NewAssemblyReference
        {
            get { return this.NewAssemblyReferenceCore; }
        }

        public object NewFunction
        {
            get { return this.NewFunctionCore; }
        }

        public object NewValueList
        {
            get { return this.NewValueListCore; }
        }

        public object EnableList
        {
            get { return this.EnableListCore; }
        }

        public object DisableList
        {
            get { return this.DisableListCore; }
        }

        public object RenameList
        {
            get { return this.RenameListCore; }
        }

        public object UnsubscribeList
        {
            get { return this.UnsubscribeListCore; }
        }

        public object PublishList
        {
            get { return this.PublishListCore; }
        }

        public object ExportList
        {
            get { return this.ExportListCore; }
        }

        public object Delete
        {
            get { return this.DeleteCore; }
        }

        public object Copy
        {
            get { return this.CopyCore; }
        }

        public object Paste
        {
            get { return this.PasteCore; }
        }

        public object Cut
        {
            get { return this.CutCore; }
        }

        public object Edit
        {
            get { return this.EditCore; }
        }

        public object Plugin
        {
            get { return this.PluginCore; }
        }

        public object GetPlugins
        {
            get { return this.GetPluginsCore; }
        }

        public object CreateCompositeWithNamespaceOverlay(System.Drawing.Bitmap bitmap)
        {
            return this.CreateCompositeWithNamespaceOverlayCore(bitmap);
        }

        protected abstract object CommandCore { get; }

        protected abstract object HistoryCore { get; }

        protected abstract object NamespaceCore { get; }

        protected abstract object FunctionCore { get; }

        protected abstract object NativeCommandCore { get; }

        protected abstract object CommandAndNamespaceCore { get; }

        protected abstract object FunctionAndNamespaceCore { get; }

        protected abstract object NativeCommandAndNamespaceCore { get; }

        protected abstract object HistoryAndNamespaceCore { get; }

        protected abstract object NewCommandCore { get; }

        protected abstract object NewAssemblyReferenceCore { get; }

        protected abstract object NewFunctionCore { get; }

        protected abstract object NewValueListCore { get; }

        protected abstract object EnableListCore { get; }

        protected abstract object DisableListCore { get; }

        protected abstract object RenameListCore { get; }

        protected abstract object UnsubscribeListCore { get; }

        protected abstract object PublishListCore { get; }

        protected abstract object ExportListCore { get; }

        protected abstract object DeleteCore { get; }

        protected abstract object CopyCore { get; }

        protected abstract object PasteCore { get; }

        protected abstract object CutCore { get; }

        protected abstract object EditCore { get; }

        protected abstract object PluginCore { get; }

        protected abstract object GetPluginsCore { get; }

        protected abstract object CreateCompositeWithNamespaceOverlayCore(System.Drawing.Bitmap bitmap);
    }
}
