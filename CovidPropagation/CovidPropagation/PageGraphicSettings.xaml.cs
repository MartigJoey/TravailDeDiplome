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
        private bool[,] gridHasContent = new bool[MAX_GRID_SIZE, MAX_GRID_SIZE];
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
                btnG.PreviewMouseDown += GraphDragOn_MouseDown;
                btnG.PreviewMouseUp += GraphDragOff_MouseUp;
                btnG.KeyDown += GraphSizeUp_KeyDown;
                btnG.Click += RemoveGraph_Click;

                Grid.SetColumn(btnG, emptyIndex[0]);
                Grid.SetRow(btnG, emptyIndex[1]);

                gridHasContent[emptyIndex[0], emptyIndex[1]] = true;
                dynamicGrid.Children.Add(btnG);
            }
        }

        private void RemoveGraph_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            int x = Grid.GetColumn(btn), y = Grid.GetRow(btn);
            int columnSpan = Grid.GetColumnSpan(btn), rowSpan = Grid.GetRowSpan(btn);
            SetCellsContent(x, y, x + columnSpan, y + rowSpan, false);
            dynamicGrid.Children.Remove(btn);
        }

        private void GraphDragOn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Button btn = (Button)sender;
            oldX = Grid.GetColumn(btn);
            oldY = Grid.GetRow(btn);
            int columnSpan = Grid.GetColumnSpan(btn), rowSpan = Grid.GetRowSpan(btn);
            SetCellsContent(oldX, oldY, oldX + columnSpan, oldY + rowSpan, false); // Libère l'espace anciennement occupé
        }

        private void GraphDragOff_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Button btn = (Button)sender;
            int[] newCoordinates = GetCoordinateWithSize(Mouse.GetPosition(dynamicGrid).X, Mouse.GetPosition(dynamicGrid).Y);

            //if (!gridHasContent[newCoordinates[0], newCoordinates[1]]) // ChechIfCellsEmpty
            int x = newCoordinates[0], y = newCoordinates[1];
            int columnSpan = Grid.GetColumnSpan(btn), rowSpan = Grid.GetRowSpan(btn);
            if (ChechIfCellsEmpty(x, y, x + columnSpan, y + rowSpan)) 
            { 
                // repositionne l'élément
                Grid.SetColumn(btn, newCoordinates[0]);
                Grid.SetRow(btn, newCoordinates[1]);
                SetCellsContent(x, y, x + columnSpan, y + rowSpan, true); // Bloque le nouvel espace occupé
            }
            else
            {
                SetCellsContent(oldX, oldY, oldX + columnSpan, oldY + rowSpan, true); // Aucun nouvel emplacement trouvé, on rebloque l'ancienne espace liberé
            }
            oldX = MAX_GRID_SIZE + 1;
            oldY = MAX_GRID_SIZE + 1;
        }

        private void GraphSizeUp_KeyDown(object sender, KeyEventArgs e)
        {
            Button btn = (Button)sender;

            int x = Grid.GetColumn(btn);
            int y = Grid.GetRow(btn);
            int columnSpan = Grid.GetColumnSpan(btn);
            int rowSpan = Grid.GetRowSpan(btn);

            if (e.Key == Key.Right)
            {
                if (ChechIfCellsEmpty(x + columnSpan, y, x + columnSpan + 1, y + rowSpan))
                {
                    Grid.SetColumnSpan(btn, columnSpan + 1);
                    SetCellsContent(x, y, x + columnSpan + 1, y + rowSpan, true);
                }
            }

            if (e.Key == Key.Left && Grid.GetColumnSpan(btn) > 1)
            {
                Grid.SetColumnSpan(btn, columnSpan - 1);
                SetCellsContent(x + 1, y, x + columnSpan + 1, y + rowSpan, false);
            }

            if (e.Key == Key.Up)
            {
                if (ChechIfCellsEmpty(x, y + rowSpan, x + columnSpan, y + rowSpan + 1))
                {
                    Grid.SetRowSpan(btn, rowSpan + 1);
                    SetCellsContent(x, y, x + columnSpan, y + rowSpan + 1, true);
                }
            }

            if (e.Key == Key.Down && Grid.GetRowSpan(btn) > 1)
            {
                Grid.SetRowSpan(btn, rowSpan - 1);
                SetCellsContent(x, y + 1, x + columnSpan, y + rowSpan + 1, false);
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

        private void SetCellsContent(int xStart, int yStart, int xStop, int yStop, bool hasContent)
        {
            for (int y = yStart; y < yStop; y++)
            {
                for (int x = xStart; x < xStop; x++)
                {
                    gridHasContent[x, y] = hasContent;
                }
            }
        }

        private bool ChechIfCellsEmpty(int xStart, int yStart, int xStop, int yStop)
        {
            // check si entre x, y et x+width, y+height est vide
            bool result = true;
            for (int y = yStart; y < yStop; y++)
            {
                for (int x = xStart; x < xStop; x++)
                {
                    if (gridHasContent[x, y])
                    {
                        result = false;
                        y = yStop;
                        break;
                    }
                }
            }
            return result;
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
                    if (!gridHasContent[x, y])
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
