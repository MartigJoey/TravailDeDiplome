using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private const int ROW_MIN_HEIGHT = 150;
        private const int COLUMN_MIN_WIDTH = 150;
        private const int MAX_GRID_SIZE = 10;
        private bool[,] gridCellHasContent = new bool[MAX_GRID_SIZE, MAX_GRID_SIZE];
        int oldX = MAX_GRID_SIZE + 1;
        int oldY = MAX_GRID_SIZE + 1;

        Grid dynamicGrid;

        public PageGraphicSettings()
        {
            InitializeComponent();

            dynamicGrid = grdContent;

            // ElementExemple
            scrollerViewer.Content = dynamicGrid;
        }

        private void AddRow_Click(object sender, RoutedEventArgs e)
        {
            if (dynamicGrid.RowDefinitions.Count < MAX_GRID_SIZE)
            {
                RowDefinition newGridRow = new RowDefinition();
                newGridRow.MinHeight = ROW_MIN_HEIGHT;
                dynamicGrid.RowDefinitions.Add(newGridRow);
            }
        }

        private void RemoveRow_Click(object sender, RoutedEventArgs e)
        {
            if (dynamicGrid.RowDefinitions.Count > 1)
                dynamicGrid.RowDefinitions.RemoveAt(dynamicGrid.RowDefinitions.GetLastIndex());
        }

        private void AddColumn_Click(object sender, RoutedEventArgs e)
        {
            if (dynamicGrid.ColumnDefinitions.Count < MAX_GRID_SIZE)
            {
                ColumnDefinition newGridColumn = new ColumnDefinition();
                newGridColumn.MinWidth = COLUMN_MIN_WIDTH;
                dynamicGrid.ColumnDefinitions.Add(newGridColumn);
            }
        }
        private void RemoveColumn_Click(object sender, RoutedEventArgs e)
        {
            if(dynamicGrid.ColumnDefinitions.Count > 1)
                dynamicGrid.ColumnDefinitions.RemoveAt(dynamicGrid.ColumnDefinitions.GetLastIndex());
        }

        private void AddGraph_Click(object sender, RoutedEventArgs e)
        {
            int[] emptyIndex = GetFirstEmptyCell();

            if (emptyIndex[0] < MAX_GRID_SIZE)
            {
                Button btnG = new Button();
                btnG.Content = "Book";
                btnG.FontSize = 14;
                btnG.FontWeight = FontWeights.Bold;
                btnG.Foreground = new SolidColorBrush(Colors.Green);
                btnG.VerticalAlignment = VerticalAlignment.Stretch;
                btnG.HorizontalAlignment = HorizontalAlignment.Stretch;
                btnG.Click += RemoveGraph_Click;
                btnG.PreviewMouseDown += GraphDragOn_MouseDown;
                btnG.PreviewMouseUp += GraphDragOff_MouseUp;
                btnG.KeyDown += GraphSizeUp_KeyDown;


                Grid.SetColumn(btnG, emptyIndex[0]);
                Grid.SetRow(btnG, emptyIndex[1]);

                gridCellHasContent[emptyIndex[0], emptyIndex[1]] = true;
                dynamicGrid.Children.Add(btnG);
            }
        }

        private void RemoveGraph_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            gridCellHasContent[Grid.GetColumn(btn), Grid.GetRow(btn)] = false;
            dynamicGrid.Children.Remove(btn);
        }

        private void GraphDragOn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Button btn = (Button)sender;
            oldX = Grid.GetColumn(btn);
            oldY = Grid.GetRow(btn);
        }

        private void GraphDragOff_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Button btn = (Button)sender;
            int[] newCoordinates = GetCoordinateWithSize(Mouse.GetPosition(dynamicGrid).X, Mouse.GetPosition(dynamicGrid).Y);

            if (!gridCellHasContent[newCoordinates[0], newCoordinates[1]]) // ChechIfCellsEmpty
            {
                gridCellHasContent[oldX, oldY] = false;
                Grid.SetColumn(btn, newCoordinates[0]);
                Grid.SetRow(btn, newCoordinates[1]);
                gridCellHasContent[newCoordinates[0], newCoordinates[1]] = true;
            }
            oldX = MAX_GRID_SIZE + 1;
            oldY = MAX_GRID_SIZE + 1;

            //Button btn = (Button)sender;
            //
            //gridCellHasContent[Grid.GetRow(btn), Grid.GetColumn(btn)] = false;
            //dynamicGrid.Children.Remove(btn);
        }

        private void GraphSizeUp_KeyDown(object sender, KeyEventArgs e)
        {
            Button btn = (Button)sender;
            if (e.Key == Key.Up)
            {
                int x = Grid.GetColumn(btn);
                int y = Grid.GetRow(btn);

                if (!gridCellHasContent[x + 1, y])
                {
                    Grid.SetColumnSpan(btn, 2);
                    gridCellHasContent[x + 1, y] = true;
                }
            }

            if (e.Key == Key.Down && Grid.GetColumnSpan(btn) > 1)
            {
                int x = Grid.GetColumn(btn);
                int y = Grid.GetRow(btn);
                Grid.SetColumnSpan(btn, 1);
                gridCellHasContent[x + 1, y] = false;
            }
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


        private bool ChechIfCellsEmpty(int x, int y, int width, int height)
        {
            // check si entre x, y et x+width, y+height est vide
            return true
        }
        private int[] GetCoordinateWithSize(double x, double y)
        {
            int cellCountX = 1;
            int cellCountY = 1;

            double cellWidth = dynamicGrid.ActualWidth / dynamicGrid.ColumnDefinitions.Count;
            double cellHeight = dynamicGrid.ActualHeight / dynamicGrid.RowDefinitions.Count;

            while (x > cellWidth * cellCountX)
                cellCountX++;

            while (y > cellHeight * cellCountY)
                cellCountY++;

            return new int[] { cellCountX-1, cellCountY-1 };
        }

        private int[] GetFirstEmptyCell()
        {
            int[] indexes = new int[] { MAX_GRID_SIZE + 1, MAX_GRID_SIZE + 1 };

            for (int y = 0; y < dynamicGrid.RowDefinitions.Count; y++)
            {
                for (int x = 0; x < dynamicGrid.ColumnDefinitions.Count; x++)
                {
                    if (!gridCellHasContent[x, y])
                    {
                        indexes[0] = x;
                        indexes[1] = y;
                        y = dynamicGrid.RowDefinitions.Count;
                        break;
                    }
                }
            }
            return indexes;
        }

    }
}
