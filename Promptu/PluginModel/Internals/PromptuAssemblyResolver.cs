//-----------------------------------------------------------------------
// <copyright file="PromptuAssemblyResolver.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.PluginModel.Internals
{
    using Mono.Cecil;

    internal class PromptuAssemblyResolver : DefaultAssemblyResolver
    {
        private AssemblyDefinition promptuAssemblyDefinition;

        public PromptuAssemblyResolver(AssemblyDefinition promptuAssemblyDefinition)
        {
            this.promptuAssemblyDefinition = promptuAssemblyDefinition;
        }

        public override AssemblyDefinition Resolve(AssemblyNameReference name)
        {
            if (name.Name == this.promptuAssemblyDefinition.Name.Name)
            {
                return this.promptuAssemblyDefinition;
            }

            return base.Resolve(name);
        }
    }
}
