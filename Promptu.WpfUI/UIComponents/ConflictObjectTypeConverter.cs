using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;
using ZachJohnson.Promptu.UIModel;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class ConflictObjectTypeConverter : MarkupExtension, IValueConverter
    {
        private object ifCommand;
        private object ifFunction;
        private object ifAssemblyReference;
        private object ifValueList;

        public ConflictObjectTypeConverter()
        {
        }

        public ConflictObjectTypeConverter(
            object ifCommand, 
            object ifFunction,
            object ifAssemblyReference, 
            object ifValueList)
        {
            this.ifCommand = ifCommand;
            this.ifFunction = ifFunction;
            this.ifAssemblyReference = ifAssemblyReference;
            this.ifValueList = ifValueList;
        }

        public object IfCommand
        {
            get { return this.ifCommand; }
            set { this.ifCommand = value; }
        }

        public object IfFunction
        {
            get { return this.ifFunction; }
            set { this.ifFunction = value; }
        }

        public object IfAssemblyReference
        {
            get { return this.ifAssemblyReference; }
            set { this.ifAssemblyReference = value; }
        }

        public object IfValueList
        {
            get { return this.ifValueList; }
            set { this.ifValueList = value; }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ConflictObjectType type = (ConflictObjectType)value;

            switch (type)
            {
                case ConflictObjectType.Command:
                    return ifCommand;
                case ConflictObjectType.Function:
                    return ifFunction;
                case ConflictObjectType.AssemblyReference:
                    return ifAssemblyReference;
                case ConflictObjectType.ValueList:
                    return ifValueList;
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
