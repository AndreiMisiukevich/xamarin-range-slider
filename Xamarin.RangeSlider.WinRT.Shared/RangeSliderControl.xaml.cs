﻿using System;
using System.Globalization;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Xamarin.RangeSlider
{
    public sealed partial class RangeSliderControl
    {
        public const string MinimumPropertyName = "Minimum";
        public const string MaximumPropertyName = "Maximum";
        public const string RangeminPropertyName = "RangeMin";
        public const string RangemaxPropertyName = "RangeMax";
        public const string MinThumbHiddenPropertyName = "MinThumbHidden";
        public const string MaxThumbHiddenPropertyName = "MaxThumbHidden";
        public const string StepValuePropertyName = "StepValue";
        public const string StepValueContinuouslyPropertyName = "StepValueContinuously";
        public const string ShowTextAboveThumbsPropertyName = "ShowTextAboveThumbs";
        public const string TextSizePropertyName = "TextSize";
        public const string TextFormatPropertyName = "TextFormat";

        public const int ControlHeight = 32;

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(MinimumPropertyName,
            typeof(double), typeof(RangeSliderControl), new PropertyMetadata(0.0));

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(MaximumPropertyName,
            typeof(double), typeof(RangeSliderControl), new PropertyMetadata(1.0));

        public static readonly DependencyProperty RangeMinProperty = DependencyProperty.Register(RangeminPropertyName,
            typeof(double), typeof(RangeSliderControl), new PropertyMetadata(0.0, OnRangeMinPropertyChanged));

        public static readonly DependencyProperty RangeMaxProperty = DependencyProperty.Register(RangemaxPropertyName,
            typeof(double), typeof(RangeSliderControl), new PropertyMetadata(1.0, OnRangeMaxPropertyChanged));

        public static readonly DependencyProperty MinThumbHiddenProperty = DependencyProperty.Register(MinThumbHiddenPropertyName,
            typeof(bool), typeof(RangeSliderControl), new PropertyMetadata(false, MinThumbHiddenPropertyChanged));

        public static readonly DependencyProperty MaxThumbHiddenProperty = DependencyProperty.Register(MaxThumbHiddenPropertyName,
            typeof(bool), typeof(RangeSliderControl), new PropertyMetadata(false, MaxThumbHiddenPropertyChanged));

        public static readonly DependencyProperty StepValueProperty = DependencyProperty.Register(StepValuePropertyName,
            typeof(double), typeof(RangeSliderControl), new PropertyMetadata(0.0));

        public static readonly DependencyProperty StepValueContinuouslyProperty = DependencyProperty.Register(StepValueContinuouslyPropertyName,
            typeof(bool), typeof(RangeSliderControl), new PropertyMetadata(false));

        public static readonly DependencyProperty ShowTextAboveThumbsProperty = DependencyProperty.Register(ShowTextAboveThumbsPropertyName,
            typeof(bool), typeof(RangeSliderControl), new PropertyMetadata(false, ShowTextAboveThumbsPropertyChanged));

        public static readonly DependencyProperty TextSizeProperty = DependencyProperty.Register(TextSizePropertyName,
            typeof(int), typeof(RangeSliderControl), new PropertyMetadata(10, TextSizePropertyChanged));

        public static readonly DependencyProperty TextFormatProperty = DependencyProperty.Register(TextFormatPropertyName,
            typeof(string), typeof(RangeSliderControl), new PropertyMetadata("F0"));

        public RangeSliderControl()
        {
            InitializeComponent();
        }

        public double Minimum
        {
            get { return (double) GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public double Maximum
        {
            get { return (double) GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public double RangeMin
        {
            get { return (double) GetValue(RangeMinProperty); }
            set { SetValue(RangeMinProperty, value); }
        }

        public double RangeMax
        {
            get { return (double) GetValue(RangeMaxProperty); }
            set { SetValue(RangeMaxProperty, value); }
        }

        public bool MinThumbHidden
        {
            get { return (bool)GetValue(MinThumbHiddenProperty); }
            set { SetValue(MinThumbHiddenProperty, value); }
        }

        public bool MaxThumbHidden
        {
            get { return (bool)GetValue(MaxThumbHiddenProperty); }
            set { SetValue(MaxThumbHiddenProperty, value); }
        }

        public double StepValue
        {
            get { return (double)GetValue(StepValueProperty); }
            set { SetValue(StepValueProperty, value); }
        }

        public bool StepValueContinuously
        {
            get { return (bool)GetValue(StepValueContinuouslyProperty); }
            set { SetValue(StepValueContinuouslyProperty, value); }
        }

        public bool ShowTextAboveThumbs
        {
            get { return (bool)GetValue(ShowTextAboveThumbsProperty); }
            set { SetValue(ShowTextAboveThumbsProperty, value); }
        }

        public int TextSize
        {
            get { return (int)GetValue(TextSizeProperty); }
            set { SetValue(TextSizeProperty, value); }
        }

        public string TextFormat
        {
            get { return (string)GetValue(TextFormatProperty); }
            set { SetValue(TextFormatProperty, value); }
        }

        public event EventHandler LowerValueChanged;
        public event EventHandler UpperValueChanged;

        private double _aggregatedDrag = 0;
        private double _initialLeft = 0;

        private static void OnRangeMinPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slider = (RangeSliderControl) d;
            var newValue = (double) e.NewValue;

            if (newValue < slider.Minimum)
            {
                slider.RangeMin = slider.Minimum;
            }
            else if (newValue > slider.Maximum)
            {
                slider.RangeMin = slider.Maximum;
            }
            else
            {
                slider.RangeMin = newValue;
            }

            if (slider.RangeMin > slider.RangeMax)
            {
                slider.RangeMax = slider.RangeMin;
            }

            slider.UpdateMinThumb(slider.RangeMin);

            slider.OnLowerValueChanged();
        }

        private static void OnRangeMaxPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slider = (RangeSliderControl) d;
            var newValue = (double) e.NewValue;

            if (newValue < slider.Minimum)
            {
                slider.RangeMax = slider.Minimum;
            }
            else if (newValue > slider.Maximum)
            {
                slider.RangeMax = slider.Maximum;
            }
            else
            {
                slider.RangeMax = newValue;
            }

            if (slider.RangeMax < slider.RangeMin)
            {
                slider.RangeMin = slider.RangeMax;
            }

            slider.UpdateMaxThumb(slider.RangeMax);

            slider.OnUpperValueChanged();
        }

        private static void MinThumbHiddenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slider = (RangeSliderControl)d;
            slider.MinThumb.Visibility = (bool)e.NewValue ? Visibility.Collapsed : Visibility.Visible;
        }

        private static void MaxThumbHiddenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slider = (RangeSliderControl)d;
            slider.MaxThumb.Visibility = (bool)e.NewValue ? Visibility.Collapsed : Visibility.Visible;
        }

        private static void TextSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slider = (RangeSliderControl)d;
            var newValue = (int?)e.NewValue;
            if (newValue.HasValue)
            {
                slider.MinThumbText.FontSize = newValue.Value;
                slider.MaxThumbText.FontSize = newValue.Value;
            }
        }

        private static void ShowTextAboveThumbsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slider = (RangeSliderControl)d;
            var newValue = (bool)e.NewValue;
            slider.TexContainerCanvas.Visibility = newValue ? Visibility.Visible : Visibility.Collapsed;
            slider.UpdateMinThumb(slider.RangeMin);
            slider.UpdateMaxThumb(slider.RangeMax);
        }

        private void UpdateTextContainerSize()
        {
            TexContainerCanvas.Height = Math.Max(MinThumbText.ActualHeight, MaxThumbText.ActualHeight);
            var height = ControlHeight + (ShowTextAboveThumbs ? TexContainerCanvas.Height : 0);
            Grid.Height = height;
            Height = height;
        }

        public void UpdateMinThumb(double min, bool update = false)
        {
            if (ContainerCanvas == null) return;
            if (!update && MinThumb.IsDragging) return;
            var relativeLeft = ValueToRelativeLeft(min);

            Canvas.SetLeft(MinThumb, relativeLeft);
            Canvas.SetLeft(ActiveRectangle, relativeLeft);

            ActiveRectangle.Width = (RangeMax - min)/(Maximum - Minimum)*ContainerCanvas.ActualWidth;

            MinThumbText.Text = ValueToString(min);
            Canvas.SetLeft(MinThumbText, relativeLeft - MinThumbText.ActualWidth/2);
        }

        public void UpdateMaxThumb(double max, bool update = false)
        {
            if (ContainerCanvas == null) return;
            if (!update && MaxThumb.IsDragging) return;
            var relativeRight = ValueToRelativeLeft(max);

            Canvas.SetLeft(MaxThumb, relativeRight);

            ActiveRectangle.Width = (max - RangeMin)/(Maximum - Minimum)*ContainerCanvas.ActualWidth;

            MaxThumbText.Text = ValueToString(max);
            Canvas.SetLeft(MaxThumbText, relativeRight - MaxThumbText.ActualWidth/2);
        }

        private double ValueToRelativeLeft(double value)
        {
            return (value - Minimum) / (Maximum - Minimum) * ContainerCanvas.ActualWidth;
        }

        private string ValueToString(double max)
        {
            return max.ToString(TextFormat, CultureInfo.InvariantCulture);
        }

        private void ContainerCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var relativeLeft = (RangeMin - Minimum)/(Maximum - Minimum)*ContainerCanvas.ActualWidth;
            var relativeRight = (RangeMax - Minimum)/(Maximum - Minimum)*ContainerCanvas.ActualWidth;

            Canvas.SetLeft(MinThumb, relativeLeft);
            Canvas.SetLeft(ActiveRectangle, relativeLeft);
            Canvas.SetLeft(MaxThumb, relativeRight);

            ActiveRectangle.Width = (RangeMax - RangeMin)/(Maximum - Minimum)*ContainerCanvas.ActualWidth;
        }

        private void MinThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {   
            if (StepValueContinuously)
            {
                _aggregatedDrag += e.HorizontalChange;
                var min = DragThumb(MinThumb, 0, Canvas.GetLeft(MaxThumb), _aggregatedDrag);
                var normalized = Normalize(min);
                UpdateMinThumb(normalized, true);
                RangeMin = normalized;
            }
            else
            {
                var min = DragThumb(MinThumb, 0, Canvas.GetLeft(MaxThumb), e.HorizontalChange);
                UpdateMinThumb(min, true);
                RangeMin = Normalize(min);
            }
        }

        private void MaxThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (StepValueContinuously)
            {
                _aggregatedDrag += e.HorizontalChange;
                var max = DragThumb(MaxThumb, Canvas.GetLeft(MinThumb), ContainerCanvas.ActualWidth, _aggregatedDrag);
                var normalized = Normalize(max);
                UpdateMaxThumb(normalized, true);
                RangeMax = Normalize(normalized);
            }
            else
            {
                var max = DragThumb(MaxThumb, Canvas.GetLeft(MinThumb), ContainerCanvas.ActualWidth, e.HorizontalChange);
                UpdateMaxThumb(max, true);
                RangeMax = Normalize(max);
            }
        }

        private double Normalize(double value)
        {
            if (Math.Abs(StepValue) < float.Epsilon)
                return value;
            return (float) Math.Round(value / StepValue)*StepValue;
        }

        private double DragThumb(Thumb thumb, double min, double max, double offset)
        {
            var currentPos = StepValueContinuously ? _initialLeft : Canvas.GetLeft(thumb);
            var nextPos = currentPos + offset;

            nextPos = Math.Max(min, nextPos);
            nextPos = Math.Min(max, nextPos);

            return Minimum + nextPos/ContainerCanvas.ActualWidth*(Maximum - Minimum);
        }

        private void MinThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            UpdateMinThumb(RangeMin);
            Canvas.SetZIndex(MinThumb, 10);
            Canvas.SetZIndex(MaxThumb, 0);
        }

        private void MaxThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            UpdateMaxThumb(RangeMax);
            Canvas.SetZIndex(MinThumb, 0);
            Canvas.SetZIndex(MaxThumb, 10);
        }

        private void OnLowerValueChanged()
        {
            LowerValueChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnUpperValueChanged()
        {
            UpperValueChanged?.Invoke(this, EventArgs.Empty);
        }

        private void MinThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            _aggregatedDrag = 0;
            _initialLeft = Canvas.GetLeft(MinThumb);
        }

        private void MaxThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            _aggregatedDrag = 0;
            _initialLeft = Canvas.GetLeft(MaxThumb);
        }

        public void SetBarHeight(int barHeight)
        {
            int margin = (ControlHeight - barHeight)/2;
            InactiveRectangle.Margin = new Thickness(0, margin, 0, margin);
            InactiveRectangle.Height = barHeight;
            Canvas.SetTop(ActiveRectangle, margin);
            ActiveRectangle.Height = barHeight;
        }

        private void ThumbTextSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateTextContainerSize();
            Canvas.SetLeft(MinThumbText, ValueToRelativeLeft(RangeMin) - MinThumbText.ActualWidth / 2);
            Canvas.SetLeft(MaxThumbText, ValueToRelativeLeft(RangeMax) - MaxThumbText.ActualWidth / 2);
        }
    }
}