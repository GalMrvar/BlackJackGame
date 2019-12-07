using BlackjackDBLib;
using GameCardLib;
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
using System.Windows.Shapes;

namespace Blackjack
{
    /// <summary>
    /// Interaction logic for ResultsWindow.xaml
    /// </summary>
    public partial class ResultsWindow : Window
    {
        private ResultsController resultsController = new ResultsController();

        public ResultsWindow()
        {
            InitializeComponent();
            DataGrid.AutoGenerateColumns = false;
            DataGrid.CanUserAddRows = false;
            UpdateWindow();
        }

        private void UpdateWindow()
        {
            DataGrid.ItemsSource = resultsController.GetResultData();
        }

        /// <summary>
        /// On delete klick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            foreach (DataGridCellInfo cellInfo in DataGrid.SelectedCells)
            {
                if (i == 6)
                    i = 0;
                if (i == 0)
                {
                    if (cellInfo.IsValid)
                    {
                        //GetCellContent returns FrameworkElement
                        ResultData content = (ResultData)cellInfo.Item;
                        resultsController.DeletePlayer(content);
                    }
                }
                i++;
            }
            if (TextBoxSearch.Text != "")
            {
                UpdateBySearchString(TextBoxSearch.Text);
            }
            else
                UpdateWindow();
        }

        /// <summary>
        /// On change/edit click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            foreach (ResultData item in DataGrid.Items)
            {
                resultsController.UpdatePlayer(item);
            }
            if (TextBoxSearch.Text != "")
            {
                UpdateBySearchString(TextBoxSearch.Text);
            }
            else
                UpdateWindow();
        }

        /// <summary>
        /// every time text changes in textBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxSearch.Text != "")
            {
                UpdateBySearchString(TextBoxSearch.Text);
            }
            else
                UpdateWindow();
        }

        private void UpdateBySearchString(string str)
        {
            DataGrid.ItemsSource = resultsController.SearchByString(str);
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
