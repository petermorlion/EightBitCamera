using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Primitives;
using Microsoft.Phone.Shell;

namespace EightBitCamera
{
    public partial class NumberPicker : PhoneApplicationPage
    {
        public NumberPicker()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ILoopingSelectorDataSource items = new PixelationDataSource();
            loopingSelector.DataSource = items;
        }
    }

    public class PixelationDataSource : ILoopingSelectorDataSource
    {
        private int _minimum = 1;
        private int _maximum = 8;

        public PixelationDataSource()
        {
            SelectedItem = 3;
        }

        public object GetNext(object relativeTo)
        {
            int value = (int)relativeTo;
            if (value == _maximum)
                return _minimum;

            return ++value;
        }

        public object GetPrevious(object relativeTo)
        {
            int value = (int) relativeTo;
            if (value == _minimum)
                return _maximum;

            return --value;
        }

        public object SelectedItem { get; set; }
        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;
    }
}