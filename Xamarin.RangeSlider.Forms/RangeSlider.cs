﻿using System;
using Xamarin.Forms;

namespace Xamarin.RangeSlider.Forms
{
    public class RangeSlider : View
    {
        public const string LowerValuePropertyName = "LowerValue";
        public const string MaximumValuePropertyName = "MaximumValue";
        public const string MinimumValuePropertyName = "MinimumValue";
        public const string UpperValuePropertyName = "UpperValue";
        public const string MinThumbHiddenPropertyName = "MinThumbHidden";
        public const string MaxThumbHiddenPropertyName = "MaxThumbHidden";
        public const string StepValuePropertyName = "StepValue";
        public const string StepValueContinuouslyPropertyName = "StepValueContinuously";
        public const string BarHeightPropertyName = "BarHeight";

        public readonly BindableProperty LowerValueProperty =
            BindableProperty.Create(LowerValuePropertyName, typeof(float), typeof(RangeSlider), 0f);

        public readonly BindableProperty MaximumValueProperty =
            BindableProperty.Create(MaximumValuePropertyName, typeof(float), typeof(RangeSlider), 1f);

        public readonly BindableProperty MaxThumbHiddenProperty =
            BindableProperty.Create(MaxThumbHiddenPropertyName, typeof(bool), typeof(RangeSlider), false);

        public readonly BindableProperty MinimumValueProperty =
            BindableProperty.Create(MinimumValuePropertyName, typeof(float), typeof(RangeSlider), 0f);

        public readonly BindableProperty MinThumbHiddenProperty =
            BindableProperty.Create(MinThumbHiddenPropertyName, typeof(bool), typeof(RangeSlider), false);

        public readonly BindableProperty StepValueContinuouslyProperty =
            BindableProperty.Create(StepValueContinuouslyPropertyName, typeof(bool), typeof(RangeSlider), false);

        public readonly BindableProperty StepValueProperty =
            BindableProperty.Create(StepValuePropertyName, typeof(float), typeof(RangeSlider), 1f);

        public readonly BindableProperty UpperValueProperty =
            BindableProperty.Create(UpperValuePropertyName, typeof(float), typeof(RangeSlider), 1f);

        public readonly BindableProperty BarHeightProperty =
            BindableProperty.Create(BarHeightPropertyName, typeof(int?), typeof(RangeSlider));

        public float MinimumValue
        {
            get { return (float) GetValue(MinimumValueProperty); }
            set { SetValue(MinimumValueProperty, value); }
        }

        public float MaximumValue
        {
            get { return (float) GetValue(MaximumValueProperty); }
            set { SetValue(MaximumValueProperty, value); }
        }

        public float LowerValue
        {
            get { return (float) GetValue(LowerValueProperty); }
            set { SetValue(LowerValueProperty, value); }
        }

        public float UpperValue
        {
            get { return (float) GetValue(UpperValueProperty); }
            set { SetValue(UpperValueProperty, value); }
        }

        public bool MinThumbHidden
        {
            get { return (bool) GetValue(MinThumbHiddenProperty); }
            set { SetValue(MinThumbHiddenProperty, value); }
        }

        public bool MaxThumbHidden
        {
            get { return (bool) GetValue(MaxThumbHiddenProperty); }
            set { SetValue(MaxThumbHiddenProperty, value); }
        }

        public float StepValue
        {
            get { return (float) GetValue(StepValueProperty); }
            set { SetValue(StepValueProperty, value); }
        }

        public bool StepValueContinuously
        {
            get { return (bool) GetValue(StepValueContinuouslyProperty); }
            set { SetValue(StepValueContinuouslyProperty, value); }
        }

        public int? BarHeight
        {
            get { return (int?)GetValue(BarHeightProperty); }
            set { SetValue(BarHeightProperty, value); }
        }

        public event EventHandler LowerValueChanged;
        public event EventHandler UpperValueChanged;

        public void OnLowerValueChanged(float value)
        {
            LowerValue = value;
            LowerValueChanged?.Invoke(this, EventArgs.Empty);
        }

        public void OnUpperValueChanged(float value)
        {
            UpperValue = value;
            UpperValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}