using GPVP.Entities;
using GPVP.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GPVP.Controls
{
    /// <summary>
    /// Interaction logic for FilterControl.xaml
    /// </summary>
    public partial class FilterControl : UserControl
    {
        public FilterControl()
        {
            InitializeComponent();
            //LayoutRoot.DataContext = this;
        }

        #region Tags DP

        public bool TagsVisible
        {
            get { return (bool)GetValue(TagsVisibleProperty); }
            set { SetValue(TagsVisibleProperty, value); }
        }

        public static readonly DependencyProperty TagsVisibleProperty =
            DependencyProperty.Register(nameof(TagsVisible), typeof(bool), typeof(FilterControl), new UIPropertyMetadata(true, (S, E) =>
            {
                var FC = S as FilterControl;
                FC.TagsVisible = (bool)E.NewValue;
            }));


        public List<Tag> TagFilterList
        {
            get { return (List<Tag>)GetValue(TagFilterListProperty); }
            set { SetValue(TagFilterListProperty, value); }
        }

        public static readonly DependencyProperty TagFilterListProperty =
            DependencyProperty.Register(nameof(TagFilterList), typeof(List<Tag>), typeof(FilterControl), new UIPropertyMetadata(null, (S, E) =>
            {
                var FC = S as FilterControl;
                FC.TagFilterList = (List<Tag>)E.NewValue;
            }));

        #endregion

        #region FilterText DP

        public string FirstFilterValue
        {
            get { return (string)GetValue(FirstFilterValueProperty); }
            set { SetValue(FirstFilterValueProperty, value); }
        }

        public static readonly DependencyProperty FirstFilterValueProperty =
            DependencyProperty.Register(nameof(FirstFilterValue), typeof(string), typeof(FilterControl), new UIPropertyMetadata(null, (S, E) =>
            {
                var FC = S as FilterControl;
                FC.FirstFilterValue = (string)E.NewValue;
            }));


        public string SecondFilterValue
        {
            get { return (string)GetValue(SecondFilterValueProperty); }
            set { SetValue(SecondFilterValueProperty, value); }
        }

        public static readonly DependencyProperty SecondFilterValueProperty =
            DependencyProperty.Register(nameof(SecondFilterValue), typeof(string), typeof(FilterControl), new UIPropertyMetadata(null, (S, E) =>
            {
                var FC = S as FilterControl;
                FC.SecondFilterValue = (string)E.NewValue;
            }));

        #endregion

        #region Combobox DP


        public IEnumerable<string> FirstFilterSource
        {
            get { return (IEnumerable<string>)GetValue(FirstFilterSourceProperty); }
            set { SetValue(FirstFilterSourceProperty, value); }
        }

        public static readonly DependencyProperty FirstFilterSourceProperty =
            DependencyProperty.Register(nameof(FirstFilterSource), typeof(IEnumerable<string>), typeof(FilterControl), new UIPropertyMetadata(null, (S, E) =>
            {
                var FC = S as FilterControl;
                FC.FirstFilterSource = (IEnumerable<string>)E.NewValue;
            }));


        public IEnumerable<string> SecondFilterSource
        {
            get { return (IEnumerable<string>)GetValue(SecondFilterSourceProperty); }
            set { SetValue(SecondFilterSourceProperty, value); }
        }

        public static readonly DependencyProperty SecondFilterSourceProperty =
            DependencyProperty.Register(nameof(SecondFilterSource), typeof(IEnumerable<string>), typeof(FilterControl), new UIPropertyMetadata(null, (S, E) =>
            {
                var FC = S as FilterControl;
                FC.SecondFilterSource = (IEnumerable<string>)E.NewValue;
            }));


        public string FirstComboValue
        {
            get { return (string)GetValue(FirstComboValueProperty); }
            set { SetValue(FirstComboValueProperty, value); }
        }

        public static readonly DependencyProperty FirstComboValueProperty =
            DependencyProperty.Register(nameof(FirstComboValue), typeof(string), typeof(FilterControl), new UIPropertyMetadata(null, (S, E) =>
            {
                var FC = S as FilterControl;
                FC.FirstComboValue = (string)E.NewValue;
            }));


        public string SecondComboValue
        {
            get { return (string)GetValue(SecondComboValueProperty); }
            set { SetValue(SecondComboValueProperty, value); }
        }

        public static readonly DependencyProperty SecondComboValueProperty =
            DependencyProperty.Register(nameof(SecondComboValue), typeof(string), typeof(FilterControl), new UIPropertyMetadata(null, (S, E) =>
            {
                var FC = S as FilterControl;
                FC.SecondComboValue = (string)E.NewValue;
            }));

        #endregion

        public string SearchBoxValue
        {
            get { return (string)GetValue(SearchBoxValueProperty); }
            set { SetValue(SearchBoxValueProperty, value); }
        }

        public static readonly DependencyProperty SearchBoxValueProperty =
            DependencyProperty.Register(nameof(SearchBoxValue), typeof(string), typeof(FilterControl), new UIPropertyMetadata(null, (S, E) =>
            {
                var FC = S as FilterControl;
                FC.SearchBoxValue = (string)E.NewValue;
            }));

        public ICommand ClearCommand
        {
            get { return (ICommand)GetValue(ClearCommandProperty); }
            set { SetValue(ClearCommandProperty, value); }
        }

        public static readonly DependencyProperty ClearCommandProperty =
            DependencyProperty.Register(nameof(ClearCommand), typeof(ICommand), typeof(FilterControl), new UIPropertyMetadata(null, (S, E) =>
            {
                var FC = S as FilterControl;
                FC.ClearCommand = (ICommand)E.NewValue;
            }));
    }
}
