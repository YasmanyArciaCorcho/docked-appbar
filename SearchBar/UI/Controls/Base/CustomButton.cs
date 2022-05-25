using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SearchBar.UI.Controls.Base
{
    public class CustomButton : Button
    {
        public static readonly DependencyProperty OverBorderBrushProperty = DependencyProperty.Register(nameof(OverBorderBrush), typeof(Brush), typeof(CustomButton), (PropertyMetadata)new FrameworkPropertyMetadata(Border.BorderBrushProperty.DefaultMetadata.DefaultValue, FrameworkPropertyMetadataOptions.None));
        public static readonly DependencyProperty OverBackgroundProperty = DependencyProperty.Register(nameof(OverBackground), typeof(Brush), typeof(CustomButton), (PropertyMetadata)new FrameworkPropertyMetadata(Control.BackgroundProperty.DefaultMetadata.DefaultValue, FrameworkPropertyMetadataOptions.None));
        public static readonly DependencyProperty PressedBorderBrushProperty = DependencyProperty.Register(nameof(PressedBorderBrush), typeof(Brush), typeof(CustomButton), (PropertyMetadata)new FrameworkPropertyMetadata(Border.BorderBrushProperty.DefaultMetadata.DefaultValue, FrameworkPropertyMetadataOptions.None));
        public static readonly DependencyProperty PressedBackgroundProperty = DependencyProperty.Register(nameof(PressedBackground), typeof(Brush), typeof(CustomButton), (PropertyMetadata)new FrameworkPropertyMetadata(Control.BackgroundProperty.DefaultMetadata.DefaultValue, FrameworkPropertyMetadataOptions.None));
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(int), typeof(CustomButton), (PropertyMetadata)new FrameworkPropertyMetadata((object)0, FrameworkPropertyMetadataOptions.None));

        static CustomButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomButton), new FrameworkPropertyMetadata((object)typeof(CustomButton)));
        }

        [Bindable(true)]
        [Category("Brush")]
        public Brush OverBorderBrush
        {
            get
            {
                return (Brush)this.GetValue(OverBorderBrushProperty);
            }
            set
            {
                SetValue(OverBorderBrushProperty, (object)value);
            }
        }

        [Bindable(true)]
        [Category("Brush")]
        public Brush OverBackground
        {
            get
            {
                return (Brush)this.GetValue(OverBackgroundProperty);
            }
            set
            {
                SetValue(OverBackgroundProperty, (object)value);
            }
        }

        [Bindable(true)]
        [Category("Brush")]
        public Brush PressedBorderBrush
        {
            get
            {
                return (Brush)this.GetValue(PressedBorderBrushProperty);
            }
            set
            {
                SetValue(PressedBorderBrushProperty, (object)value);
            }
        }

        [Bindable(true)]
        [Category("Brush")]
        public Brush PressedBackground
        {
            get
            {
                return (Brush)this.GetValue(PressedBackgroundProperty);
            }
            set
            {
                SetValue(PressedBackgroundProperty, (object)value);
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        public int CornerRadius
        {
            get
            {
                return (int)GetValue(CornerRadiusProperty);
            }
            set
            {
                SetValue(CornerRadiusProperty, (object)value);
            }
        }
    }
}
