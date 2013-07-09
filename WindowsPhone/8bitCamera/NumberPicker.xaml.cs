using System;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Primitives;

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
            var minimum = int.Parse(NavigationContext.QueryString["min"]);
            var maximum = int.Parse(NavigationContext.QueryString["max"]);
            var selected = int.Parse(NavigationContext.QueryString["selected"]);
            ILoopingSelectorDataSource items = new IntLoopingDataSource(minimum, maximum, selected);
            loopingSelector.DataSource = items;
        }
        
        private void SaveButtonClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }
    }
}