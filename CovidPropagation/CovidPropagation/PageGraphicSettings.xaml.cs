using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CovidPropagation
{
    /// <summary>
    /// Logique d'interaction pour PageGraphicSettings.xaml
    /// </summary>
    public partial class PageGraphicSettings : Page
    {
        public const int ROW_MIN_HEIGHT = 150;
        private const int COLUMN_MIN_WIDTH = 150;

        Grid dynamicGrid;

        public PageGraphicSettings()
        {
            InitializeComponent();

            dynamicGrid = grdContent;

            // ElementExemple

            //TextBlock txtBlock3 = new TextBlock();
            //txtBlock3.Text = "Book";
            //txtBlock3.FontSize = 14;
            //txtBlock3.FontWeight = FontWeights.Bold;
            //txtBlock3.Foreground = new SolidColorBrush(Colors.Green);
            //txtBlock3.VerticalAlignment = VerticalAlignment.Top;
            //Grid.SetRow(txtBlock3, 0);
            //Grid.SetColumn(txtBlock3, 2);

            //DynamicGrid.Children.Add(txtBlock3);
            scrollerViewer.Content = dynamicGrid;
        }

        private void AddRow_Click(object sender, RoutedEventArgs e)
        {
            RowDefinition newGridRow = new RowDefinition();
            newGridRow.MinHeight = ROW_MIN_HEIGHT;
            dynamicGrid.RowDefinitions.Add(newGridRow);
        }

        private void RemoveRow_Click(object sender, RoutedEventArgs e)
        {
            if (dynamicGrid.RowDefinitions.Count > 1)
                dynamicGrid.RowDefinitions.RemoveAt(dynamicGrid.RowDefinitions.GetLastIndex());
        }

        private void AddColumn_Click(object sender, RoutedEventArgs e)
        {
            ColumnDefinition newGridColumn = new ColumnDefinition();
            newGridColumn.MinWidth = COLUMN_MIN_WIDTH;
            dynamicGrid.ColumnDefinitions.Add(newGridColumn);
        }
        private void RemoveColumn_Click(object sender, RoutedEventArgs e)
        {
            if(dynamicGrid.ColumnDefinitions.Count > 1)
                dynamicGrid.ColumnDefinitions.RemoveAt(dynamicGrid.ColumnDefinitions.GetLastIndex());
        }

        private void AddGraph_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddGUI_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
