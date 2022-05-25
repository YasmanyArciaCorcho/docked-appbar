using SearchBar.UI.SearchBarViewModel;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using Services.Autocomplete;
using Common.Suggestion;
using Common.ExtensionMethods;

namespace SearchBar.UI.Controls.SearchTextBox
{
    /// <summary>
    /// Interaction logic for SearchTextBox.xaml
    /// </summary>
    public partial class SearchTextBox : UserControl
    {
        private bool _canUpdateQuery;

        public string QueryToComplete { get; set; } = "";

        public SearchTextBox()
        {
            InitializeComponent();

            #region Query Text Box

            this.QueryTextBox.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(QueryTextBox_GotKeyboardFocus);
            this.QueryTextBox.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(QueryTextBox_LostKeyboardFocus);
            this.QueryTextBox.PreviewKeyDown += new KeyEventHandler(QueryTextBox_PreviewKeyDown);
            this.QueryTextBox.TextChanged += new TextChangedEventHandler(QueryTextBox_TextChanged);

            #endregion

            this.DataContextChanged += new DependencyPropertyChangedEventHandler(SearchTextBox_DataContextChanged);

            if (SearchBarViewModel != null)
            {
                SearchBarViewModel.PropertyChanged += new PropertyChangedEventHandler(SearchBarViewModel_PropertyChanged);
            }

            #region Autocomplete List Box

            this.AutocompleteListBox.SelectionChanged += new SelectionChangedEventHandler(MoveAutocomplete_SelectionChanged);

            EventManager.RegisterClassHandler(typeof(ListBoxItem),
            ListBoxItem.MouseLeftButtonDownEvent,
            new RoutedEventHandler(MouseLeftButtonDownClassHandler));

            AddLeftClickEvent(AutocompleteListBox, MouseLeftButtonDownClassHandler);

            #endregion
        }

        private ISearchBarViewModel SearchBarViewModel
        {
            get { return (ISearchBarViewModel)DataContext; }
        }

        private void SearchTextBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ISearchBarViewModel searchBarViewModel)
            {
                searchBarViewModel.PropertyChanged += SearchBarViewModel_PropertyChanged;
            }
        }

        private void QueryTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            this.HidePlaceHolderTextBlock();
        }

        private void QueryTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ShowPlaceholderText();
            CloseAutocompletePopup();
        }

        private void QueryTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                if (!AutocompletePopup.IsOpen)
                {
                    OpenAutocompletePopup();
                }
                else
                {
                    MoveAutocompleteListBoxSelectedIndex(1);
                }
            }
            else if (e.Key == Key.Up)
            {
                MoveAutocompleteListBoxSelectedIndex(-1);
            }
            else
            {
                _canUpdateQuery = true;
                switch (e.Key)
                {
                    case Key.Enter:
                        CloseAutocompletePopup();
                        OpenQuery(QueryTextBox.Text.Trim());
                        break;
                    default:
                        break;
                }
            }
        }

        private void QueryTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!AutocompletePopup.IsOpen)
            {
                OpenAutocompletePopup();
            }
            else if (AutocompletePopup.IsOpen && String.IsNullOrEmpty(QueryTextBox.Text))
            {
                CloseAutocompletePopup();
            }

            if (QueryTextBox.IsKeyboardFocused && QueryTextBox.Text.Length > 0)
            {
                if (SearchBarViewModel != null && _canUpdateQuery)
                {
                    SearchBarViewModel.SetQuery(QueryTextBox.Text);
                }
            }
        }

        #region Place Holder Text

        private void ShowPlaceholderText()
        {
            if (!this.QueryTextBox.IsKeyboardFocused || QueryTextBox.Text.Length == 0)
            {
                this.QueryTextBox.Text = "";
                this.PlaceHolderTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void HidePlaceHolderTextBlock()
        {
            this.PlaceHolderTextBlock.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Autocomplete Popup

        private void OpenAutocompletePopup()
        {
            AutocompletePopup.IsOpen = true;
            AutocompletePopup.Visibility = Visibility.Visible;
        }

        private void CloseAutocompletePopup()
        {
            if (AutocompletePopup.IsOpen)
            {
                AutocompletePopup.IsOpen = false;
                AutocompletePopup.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region Autocomplete List Box

        private void MoveAutocompleteListBoxSelectedIndex(int indexDifference)
        {
            int newIndex = AutocompleteListBox.SelectedIndex + indexDifference;
            if (newIndex < 0)
            {
                newIndex = 0;
            }
            else if (newIndex >= AutocompleteListBox.Items.Count)
            {
                newIndex = AutocompleteListBox.Items.Count - 1;
            }

            AutocompleteListBox.SelectedIndex = newIndex;
        }

        private void MoveAutocomplete_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AutocompletePopup.IsOpen)
            {
                if (AutocompleteListBox.SelectedItem != null)
                {
                    _canUpdateQuery = false;

                    if (AutocompleteListBox.SelectedItem is IAutocompleteSuggestion autocomplete)
                    {
                        QueryTextBox.Text = autocomplete.Value;
                    }
                }
            }
        }

        private void AddLeftClickEvent(ListBox listBox, MouseButtonEventHandler mouseButtonEventHandler)
        {
            listBox.ItemContainerStyle.Setters.Add(new EventSetter()
            {
                Event = MouseDoubleClickEvent,
                Handler = mouseButtonEventHandler
            });
        }

        private void MouseLeftButtonDownClassHandler(object sender, RoutedEventArgs e)
        {
            if (sender is ListBoxItem item && item.Content is IAutocompleteSuggestion autocompleteSuggestion)
            {
                OpenQuery(autocompleteSuggestion.Value);
            }
        }

        #endregion

        private void OpenQuery(string query)
        {
            if (SearchBarViewModel != null)
            {
                if (string.IsNullOrEmpty(QueryToComplete))
                {
                    SearchBarViewModel.SearchQuery(query);
                }
                else
                {
                    SearchBarViewModel.OpenDirectlyQuery(string.Format(QueryToComplete, query.RemplaceWhiteSpaceToPercent()));
                }
            }
        }

        private void SearchBarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (SearchBarViewModel != null)
            {
                if (e.PropertyName.Equals(nameof(SearchBarViewModel.ItemsToSelect)))
                {
                    AutocompleteListBox.Items.Refresh();
                    AutocompleteListBox.SelectedIndex = 0;
                }
            }
        }
    }
}
