using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Controls.Primitives;
using System.Reflection;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    /// <summary>
    /// Interaction logic for DummyListBox.xaml
    /// </summary>
    public partial class DummyListBox : ListBox
    {
        private ManualResetEvent generationFinishedSignal;

        public DummyListBox(ListBox copyFrom)
        {
            InitializeComponent();

            this.ItemTemplate = copyFrom.ItemTemplate;
            this.Template = copyFrom.Template;
            this.ItemContainerStyle = copyFrom.ItemContainerStyle;
            this.generationFinishedSignal = new ManualResetEvent(true);
        }

        public double GetHeightOf(int index)
        {
            //DependencyObject obj = this.ItemContainerGenerator.ContainerFromIndex(0);
            //if (obj == null)
            //{
            //    this.ItemContainerGenerator.StatusChanged += this.HandleGeneratorStatusChanged;
            //    this.generationFinishedSignal.Reset();
            //    this.UpdateLayout();
            //    this.generationFinishedSignal.WaitOne();
            //    obj = this.ItemContainerGenerator.ContainerFromIndex(0);
            //}

            IItemContainerGenerator generator = this.ItemContainerGenerator;

            bool isNewlyRealized;

            FrameworkElement child;

            //StackPanel itemsPanel = VisualTreeHelper.
                
                //this.ItemsPanel.FindName("stackPanel", this.TemplatedParent) as StackPanel;//.FindName("stackPanel") as StackPanel;

            GeneratorPosition position = generator.GeneratorPositionFromIndex(index);
            using (generator.StartAt(position, GeneratorDirection.Forward, true))
            {
                child = (FrameworkElement)generator.GenerateNext(out isNewlyRealized);
                if (isNewlyRealized)
                {
                    //if (index >= itemsPanel.Children.Count)
                    //{
                    //    itemsPanel.GetType().InvokeMember("AddInternalChild",
                    //        BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod,
                    //        Type.DefaultBinder, itemsPanel,
                    //        new object[] { child });
                    //}
                    //else
                    //{
                    //    itemsPanel.GetType().InvokeMember("InsertInternalChild",
                    //        BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod,
                    //        Type.DefaultBinder, itemsPanel,
                    //        new object[] { index, child });
                    //}

                    generator.PrepareItemContainer(child);
                }
            }

            //FrameworkElement element = (FrameworkElement)obj;
            return child.ActualHeight;
        }

        private void HandleGeneratorStatusChanged(object sender, EventArgs e)
        {
                if (this.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
                {
                    //ListBoxItem lbi = (ListBoxItem)this.ItemContainerGenerator.ContainerFromIndex(0);
                    //this.itemHeight = this.ItemContainerGenerator.ContainerFromIndex(

                    //lbi.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    //this.itemHeight = lbi.ActualHeight;
                    //this.itemHeightCalculated.Set();
                    this.generationFinishedSignal.Set();
                    this.ItemContainerGenerator.StatusChanged -= this.HandleGeneratorStatusChanged;
                }
        }
    }
}
