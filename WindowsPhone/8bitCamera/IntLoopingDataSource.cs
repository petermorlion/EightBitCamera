using System;
using System.Windows.Controls;
using Microsoft.Phone.Controls.Primitives;

namespace EightBitCamera
{
    public class IntLoopingDataSource : ILoopingSelectorDataSource
    {
        private readonly int _minimum;
        private readonly int _maximum;

        public IntLoopingDataSource(int minimum, int maximum, int selectedItem)
        {
            _minimum = minimum;
            _maximum = maximum;
            SelectedItem = selectedItem;
        }

        public object GetNext(object relativeTo)
        {
            var value = (int)relativeTo;
            if (value == _maximum)
                return _minimum;

            return ++value;
        }

        public object GetPrevious(object relativeTo)
        {
            var value = (int) relativeTo;
            if (value == _minimum)
                return _maximum;

            return --value;
        }

        public object SelectedItem { get; set; }
        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;
    }
}