using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ZachJohnson.Promptu.SkinApi;
using System.Threading;
using System.Windows.Controls.Primitives;
using System.Windows;
using ZachJohnson.Promptu.WpfUI.DefaultSkin;
using System.Windows.Media;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class CenteringListBox : ListBox
    {
        private ScrollViewer scrollViewer;
        private double? itemHeight;
        //private bool reentry;
        //private ManualResetEvent itemHeightCalculated;

        static CenteringListBox()
        {
            FontSizeProperty.OverrideMetadata(typeof(CenteringListBox), new FrameworkPropertyMetadata(HandleFontPropertyChanged));
            FontFamilyProperty.OverrideMetadata(typeof(CenteringListBox), new FrameworkPropertyMetadata(HandleFontPropertyChanged));
        }

        public CenteringListBox()
        {
            this.Loaded += this.HandleLoaded;

            //this.ItemContainerGenerator.StatusChanged += this.HandleGeneratorStatusChanged;
            //this.itemHeightCalculated = new ManualResetEvent(false);
        }

        public event EventHandler DesiredIconSizeChanged;

        private static void HandleFontPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CenteringListBox listBox = obj as CenteringListBox;

            //if (listBox.reentry)
            //{
            //    return;
            //}

            //listBox.reentry = true;
            //listBox.FontSize = listBox.FontSize;
            //listBox.reentry = false;

            if (listBox != null)
            {
                listBox.RecalcuateItemHeight();
                //listBox.Dispatcher.Invoke(new ParameterlessVoid(listBox.RecalcuateItemHeight), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            }
        }

        public double DesiredIconSize
        {
            get
            {
                if (this.itemHeight == null)
                {
                    return 16;
                }

                return this.itemHeight.Value - 2;
            }
        }

        //public event EventHandler ScrollFinished;

        public void CenterSelectedItem()
        {
            if (this.scrollViewer == null)
            {
                return;
            }

            this.scrollViewer.ScrollToVerticalOffset(this.SelectedIndex - Math.Floor(this.scrollViewer.ViewportHeight / 2));
        }

        public ListBoxItem FirstVisibleItem
        {
            get
            {
                this.UpdateLayout();
                return (ListBoxItem)this.ItemContainerGenerator.ContainerFromIndex(
                    (int)this.scrollViewer.VerticalOffset);
            }
        }

        public double ItemHeight
        {
            get
            {
                if (this.itemHeight == null)
                {
                    return 10;
                    ////this.UpdateLayout();
                    ////IItemContainerGenerator generator = this.ItemContainerGenerator;
                    //DummyListBox dummyListBox = new DummyListBox(this);

                    ////StackPanel panel = new StackPanel();
                    ////panel.Orientation = Orientation.Vertical;
                    //////ItemsPanelTemplate template = new ItemsPanelTemplate();
                    //////template.
                    ////tempListBox.ItemsPanel = new ItemsPanelTemplate(new FrameworkElementFactory(typeof();

                    //SuggestionItemWrapper tempItem = new SuggestionItemWrapper(
                    //    new SkinsApi.SuggestionItem(SkinsApi.SuggestionItemType.Command, "temp", 0), 
                    //    null);

                    //dummyListBox.Items.Add(tempItem);

                    ////this.Items.Insert(0, tempItem);

                    ////IItemContainerGenerator generator = dummyListBox.ItemContainerGenerator;

                    ////bool isNewlyRealized;

                    ////FrameworkElement child;

                    ////GeneratorPosition position = generator.GeneratorPositionFromIndex(0);
                    ////using (generator.StartAt(position, GeneratorDirection.Forward, true))
                    ////{
                    ////    child = (FrameworkElement)generator.GenerateNext(out isNewlyRealized);
                    ////    generator.PrepareItemContainer(child);
                    ////}

                    ////dummyListBox.UpdateLayout();
                    ////dummyListBox.Dispatcher.Invoke(new Action(Temp), System.Windows.Threading.DispatcherPriority.Loaded);

                    ////dummyListBox.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    //////FrameworkElement child = (FrameworkElement)dummyListBox.ItemContainerGenerator.ContainerFromIndex(0);
                    //////double previousHeight = this.Height;
                    ////MessageBox.Show("pause");
                    //this.itemHeight = dummyListBox.GetHeightOf(0);

                    ////this.Items.Remove(tempItem);


                    ////ListBoxItem lbi = (ListBoxItem)((IItemContainerGenerator)this.ItemContainerGenerator).GenerateNext();
                    ////this.itemHeight = lbi.ActualHeight;
                    ////this.itemHeightCalculated.WaitOne();
                }

                return this.itemHeight.Value;
            }
        }

        //private void Temp()
        //{
        //}

        //private void HandleGeneratorStatusChanged(object sender, EventArgs e)
        //{
        //    if (this.itemHeight == null)
        //    {
        //        if (this.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
        //        {
        //            //ListBoxItem lbi = (ListBoxItem)this.ItemContainerGenerator.ContainerFromIndex(0);
        //            //this.itemHeight = this.ItemContainerGenerator.ContainerFromIndex(

        //            //lbi.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        //            //this.itemHeight = lbi.ActualHeight;
        //            //this.itemHeightCalculated.Set();
        //        }
        //    }
        //    else
        //    {
        //        this.ItemContainerGenerator.StatusChanged -= this.HandleGeneratorStatusChanged;
        //    }
        //}

        public ScrollViewer ScrollViewer
        {
            get { return this.scrollViewer; }
        }

        public void DoPageUpOrDown(Direction direction)
        {
            int numberOfItemsInPage = (int)(this.scrollViewer.ViewportHeight);
            if (numberOfItemsInPage > 1)
            {
                numberOfItemsInPage--;
            }

            int newSelectedIndex = this.SelectedIndex + (direction == Direction.Up ? -numberOfItemsInPage : numberOfItemsInPage);

            if (newSelectedIndex < 0)
            {
                newSelectedIndex = 0;
            }
            else if (newSelectedIndex >= this.Items.Count)
            {
                newSelectedIndex = this.Items.Count - 1;
            }

            if (newSelectedIndex != this.SelectedIndex)
            {
                //this.customListBox.BeginUpdate();

                int newTopIndex = direction == Direction.Up ? newSelectedIndex : newSelectedIndex - numberOfItemsInPage;

                this.SelectedIndex = newSelectedIndex;
                this.scrollViewer.ScrollToVerticalOffset(newTopIndex);
                //this.customListBox.EndUpdate();
                //this.customListBox.TopIndex = newTopIndex;
                //this.customListBox.Refresh();
            }
        }

        public void Scroll(Direction direction)
        {
            int offset = 3 * (direction == Direction.Down ? 1 : -1);

            this.scrollViewer.ScrollToVerticalOffset(this.scrollViewer.VerticalOffset + offset);
        }

        public void AddChildPublic(object item)
        {
            this.AddChild(item);
        }

        private static double DipToEm(double dip)
        {
            return Math.Round(dip * 72 / 96);
        }

        private static double EmToDip(double em)
        {
            return (em * 96 / 72);
        }

        public void RecalcuateItemHeight()
        {
#if NO_WPF
            this.itemHeight = 18.5;
            return;
#endif
            //VirtualizationMode mode = VirtualizingStackPanel.GetVirtualizationMode(this);

            //VirtualizingStackPanel.SetVirtualizationMode(this, VirtualizationMode.Standard);
            //return;
            //this.itemHeight = Math.Round(EmToDip(DipToEm(this.FontSize) * this.FontFamily.LineSpacing), MidpointRounding.AwayFromZero) + 2;
            //this.scrollViewer = (ScrollViewer)this.GetTemplateChild("ScrollViewer");

            SuggestionItemWrapper tempItem = new SuggestionItemWrapper(
                new SkinApi.SuggestionItem(SkinApi.SuggestionItemType.Command, "temp", 0),
                null);

            this.Items.Insert(0, tempItem);

            IItemContainerGenerator generator = this.ItemContainerGenerator;

            bool isNewlyRealized;

            FrameworkElement child;
            GeneratorPosition position = generator.GeneratorPositionFromIndex(0);
            using (generator.StartAt(position, GeneratorDirection.Forward, true))
            {
                child = (FrameworkElement)generator.GenerateNext(out isNewlyRealized);
                if (isNewlyRealized)
                {
                    generator.PrepareItemContainer(child);
                }
            }

            //if (child.Parent != null)
            //{
            //    child.Parent.
            //}

            //if (child.Parent == null)
            //{
            //    this.AddChild(child);
            //}

            this.AddLogicalChild(child);

            //Globals.CurrentSkin.SuggestionProvider.Show();

            //System.Threading.Thread.Sleep(500);

            this.UpdateLayout();
            child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            this.itemHeight = child.DesiredSize.Height;
            this.Items.Remove(tempItem);

            this.RemoveLogicalChild(child);

            //VirtualizingStackPanel.SetVirtualizationMode(this, VirtualizationMode.Recycling);

            this.OnDesiredIconSizeChanged(EventArgs.Empty);
        }

        private void OnDesiredIconSizeChanged(EventArgs e)
        {
            EventHandler handler = this.DesiredIconSizeChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void HandleLoaded(object sender, EventArgs e)
        {
            this.scrollViewer = (ScrollViewer)this.GetTemplateChild("ScrollViewer");
            this.RecalcuateItemHeight();
            //this.scrollViewer = (ScrollViewer)this.GetTemplateChild("ScrollViewer");

            //SuggestionItemWrapper tempItem = new SuggestionItemWrapper(
            //    new SkinApi.SuggestionItem(SkinApi.SuggestionItemType.Command, "temp", 0),
            //    null);

            //this.Items.Insert(0, tempItem);

            //IItemContainerGenerator generator = this.ItemContainerGenerator;

            //bool isNewlyRealized;

            //FrameworkElement child;
            //GeneratorPosition position = generator.GeneratorPositionFromIndex(0);
            //using (generator.StartAt(position, GeneratorDirection.Forward, true))
            //{
            //    child = (FrameworkElement)generator.GenerateNext(out isNewlyRealized);
            //    if (isNewlyRealized)
            //    {
            //        generator.PrepareItemContainer(child);
            //    }
            //}

            //this.AddChild(child);

            //this.UpdateLayout();
            //this.itemHeight = child.ActualHeight;
            //this.Items.Remove(tempItem);

            ////this.UpdateLayout();
            ////IItemContainerGenerator generator = this.ItemContainerGenerator;
            //DummyListBox dummyListBox = new DummyListBox(this);

            ////StackPanel panel = new StackPanel();
            ////panel.Orientation = Orientation.Vertical;
            //////ItemsPanelTemplate template = new ItemsPanelTemplate();
            //////template.
            ////tempListBox.ItemsPanel = new ItemsPanelTemplate(new FrameworkElementFactory(typeof();

            //SuggestionItemWrapper tempItem = new SuggestionItemWrapper(
            //    new SkinsApi.SuggestionItem(SkinsApi.SuggestionItemType.Command, "temp", 0), 
            //    null);

            //dummyListBox.Items.Add(tempItem);

            ////this.Items.Insert(0, tempItem);

            ////IItemContainerGenerator generator = dummyListBox.ItemContainerGenerator;

            ////bool isNewlyRealized;

            ////FrameworkElement child;

            ////GeneratorPosition position = generator.GeneratorPositionFromIndex(0);
            ////using (generator.StartAt(position, GeneratorDirection.Forward, true))
            ////{
            ////    child = (FrameworkElement)generator.GenerateNext(out isNewlyRealized);
            ////    generator.PrepareItemContainer(child);
            ////}

            ////dummyListBox.UpdateLayout();
            ////dummyListBox.Dispatcher.Invoke(new Action(Temp), System.Windows.Threading.DispatcherPriority.Loaded);

            ////dummyListBox.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            //////FrameworkElement child = (FrameworkElement)dummyListBox.ItemContainerGenerator.ContainerFromIndex(0);
            //////double previousHeight = this.Height;
            ////MessageBox.Show("pause");
            //this.itemHeight = dummyListBox.GetHeightOf(0);

            ////this.Items.Remove(tempItem);


            ////ListBoxItem lbi = (ListBoxItem)((IItemContainerGenerator)this.ItemContainerGenerator).GenerateNext();
            ////this.itemHeight = lbi.ActualHeight;
            ////this.itemHeightCalculated.WaitOne();


            //ListBoxItem lbi = (ListBoxItem)((IItemContainerGenerator)this.ItemContainerGenerator).GenerateNext();
            //this.itemHeight = lbi.ActualHeight;
            //this.itemHeightCalculated.WaitOne();
        }

        //private void HandleScrollFinished(object sender, EventArgs e)
        //{
        //    this.OnScrollChanged(EventArgs.Empty);
        //}

        //protected virtual void OnScrollChanged(EventArgs e)
        //{
        //    EventHandler handler = this.ScrollFinished;
        //    if (handler != null)
        //    {
        //        handler(this, e);
        //    }
        //}
    }
}
