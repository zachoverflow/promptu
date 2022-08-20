using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using ZachJohnson.Promptu.PluginModel;
using ZachJohnson.Promptu.UIModel;
using System.Security;
using System.Windows.Media;

namespace ZachJohnson.Promptu.WpfUI.UIComponents
{
    internal class OptionsTemplateSelector : DataTemplateSelector
    {
        //private DataTemplate defaultTemplate;
        //private DataTemplate booleanTemplate;
        //private DataTemplate hotkeyTemplate;

        public OptionsTemplateSelector()
        {
        }

        public DataTemplate DefaultTemplate
        {
            get;
            set;
            //get { return this.defaultTemplate; }
            //set { this.defaultTemplate = value; }
        }

        public DataTemplate BooleanTemplate
        {
            get;
            set;
            //get { return this.booleanTemplate; }
            //set { this.booleanTemplate = value; }
        }

        public DataTemplate RadioButtonTemplate
        {
            get;
            set;
        }

        public DataTemplate EnumTemplate
        {
            get;
            set;
        }

        public DataTemplate FontFamilyTemplate
        {
            get;
            set;
        }

        public DataTemplate ContentHostTemplate
        {
            get;
            set;
        }

        public DataTemplate HotkeyTemplate
        {
            get;
            set;
            //get { return this.hotkeyTemplate; }
            //set { this.hotkeyTemplate = value; }
        }

        public DataTemplate LabelTemplate
        {
            get;
            set;
        }

        public DataTemplate PasswordTemplate
        {
            get;
            set;
        }

        public DataTemplate ColorTemplate
        {
            get;
            set;
        }

        public DataTemplate FileSystemFileTemplate
        {
            get;
            set;
        }

        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            ObjectPropertyBase property = item as ObjectPropertyBase;

            if (property != null)
            {
                IPropertyEditorFactory factory = property.EditorFactory;

                if (factory != null)
                {
                    IPropertyEditor editor = factory.CreateEditor();
                    DataTemplate editorAsTemplate = editor as DataTemplate;
                    if (editorAsTemplate != null)
                    {
                        //editor.Context = property;
                        return editorAsTemplate;
                    }
                }

                object value = property.ObjectValue;

                if (value is bool)
                {
                    BooleanConversionInfo booleanInfo = property.ConversionInfo as BooleanConversionInfo;

                    if (booleanInfo != null && booleanInfo.RadioGroup != null)
                    {
                        return this.RadioButtonTemplate;
                    }

                    return this.BooleanTemplate;
                }
                else if (value is GlobalHotkey)
                {
                    return this.HotkeyTemplate;
                }
                else if (value is Enum)
                {
                    return this.EnumTemplate;
                }
                else if (value is Color)
                {
                    return this.ColorTemplate;
                }
                else if (value is FontFamily)
                {
                    return this.FontFamilyTemplate;
                }
                else if (value is FileSystemFile)
                {
                    return this.FileSystemFileTemplate;
                }
                else if (value is SecureString)
                {
                    return this.PasswordTemplate;
                    //TextConversionInfo textConversionInfo = property.ConversionInfo as TextConversionInfo;
                    //if (textConversionInfo != null && textConversionInfo.IsPassword)
                    //{
                    //    return this.PasswordTemplate;
                    //}
                }
            }
            else if (item is UIElement)
            {
                return this.ContentHostTemplate;
            }
            else if (item is OptionsTextEntry)
            {
                return this.LabelTemplate;
            }

            return this.DefaultTemplate;
        }
    }
}
