using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZachJohnson.Promptu.UIModel.Configuration;
using ZachJohnson.Promptu.UIModel.Interfaces;

namespace ZachJohnson.Promptu.WpfUI.Configuration
{
    internal class WpfToolkitSettings : ToolkitSettings
    {
        public WpfToolkitSettings()
            : base(
            new WindowSettings<IAssemblyReferenceEditor>(),
            new FunctionEditorSettings(),
            new ValueListEditorSettings(),
            new ValueListSelectorSettings(new SetupPanelSettings(new List<double>(new double[] { 98, 327, double.NaN, double.NaN }))),
            new FunctionInvocationEditorSettings(),
            new FileSystemSuggestionEditorSettings(),
            new FunctionViewerSettings(),
            new CommandEditorSettings(),
            //new TempObjectSettings<ICollisionResolvingDialog>(),
            new SetupPanelSettings(new List<double>(new double[] { 225, 383, 221, 391 })),
            new SetupPanelSettings(new List<double>(new double[] { 98, 327, double.NaN, double.NaN })),
            new SetupPanelSettings(new List<double>(new double[] { 352, 208, 173, 121 })),
            new SetupPanelSettings(new List<double>(new double[] { 140, 150, 418 })),
            new WindowSettings<ISetupDialog>(),
            new ProfileTabSettings(),
            new WindowSettings<IOptionsDialog>(),
            new WindowSettings<IOptionsDialog>())
        {
        }
    }
}
