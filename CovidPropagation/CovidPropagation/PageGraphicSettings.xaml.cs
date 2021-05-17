using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        public static GraphicData[,] graphicDatas = new GraphicData[MAX_GRID_SIZE, MAX_GRID_SIZE];
        private bool[,] gridHasContent = new bool[MAX_GRID_SIZE, MAX_GRID_SIZE];
        public int oldX = MAX_GRID_SIZE + 1;
        int oldY = MAX_GRID_SIZE + 1;
        int maxCellColumnSpan;
        int maxCellRowSpan;

        Grid dynamicGrid;

        public PageGraphicSettings()
        {
            InitializeComponent();

            dynamicGrid = grdContent;
            scrollerViewer.Content = dynamicGrid;
            maxCellColumnSpan = dynamicGrid.ColumnDefinitions.Count;
            maxCellRowSpan = dynamicGrid.RowDefinitions.Count;
        }

        private void AddRow_Click(object sender, RoutedEventArgs e)
        {
            if (dynamicGrid.RowDefinitions.Count < MAX_GRID_SIZE)
            {
                RowDefinition newGridRow = new RowDefinition();
                newGridRow.MinHeight = ROW_MIN_HEIGHT;
                dynamicGrid.RowDefinitions.Add(newGridRow);
                maxCellRowSpan = dynamicGrid.RowDefinitions.Count;
            }
        }

        private void RemoveRow_Click(object sender, RoutedEventArgs e)
        {
            if (dynamicGrid.RowDefinitions.Count > 1)
            {
                dynamicGrid.RowDefinitions.RemoveAt(dynamicGrid.RowDefinitions.GetLastIndex());
                maxCellRowSpan = dynamicGrid.RowDefinitions.Count;
            }
        }

        private void AddColumn_Click(object sender, RoutedEventArgs e)
        {
            if (dynamicGrid.ColumnDefinitions.Count < MAX_GRID_SIZE)
            {
                ColumnDefinition newGridColumn = new ColumnDefinition();
                newGridColumn.MinWidth = COLUMN_MIN_WIDTH;
                dynamicGrid.ColumnDefinitions.Add(newGridColumn);
                maxCellColumnSpan = dynamicGrid.ColumnDefinitions.Count;
            }
        }

        private void RemoveColumn_Click(object sender, RoutedEventArgs e)
        {
            if(dynamicGrid.ColumnDefinitions.Count > 1)
            {
                dynamicGrid.ColumnDefinitions.RemoveAt(dynamicGrid.ColumnDefinitions.GetLastIndex());
                maxCellColumnSpan = dynamicGrid.ColumnDefinitions.Count;
            }
        }

        private void AddGraph_Click(object sender, RoutedEventArgs e)
        {
            int[] emptyIndex = GetFirstEmptyCell();
            int x = emptyIndex[0];
            int y = emptyIndex[1];

            if (x < MAX_GRID_SIZE)
            {
                Grid cell = new Grid();
                Button btnRemove = CreateGraphButton("GraphCloseStyle", "./Images/close.png");
                Button btnMove = CreateGraphButton("GraphButtonStyle", "./Images/cursor-move.png");

                Button btnWidthPlus = CreateGraphButton("GraphButtonStyle", "./Images/arrow-right.png");
                Button btnWidthMinus = CreateGraphButton("GraphButtonStyle", "./Images/arrow-left.png");

                Button btnHeightPlus = CreateGraphButton("GraphButtonStyle", "./Images/arrow-down.png");
                Button btnHeightMinus = CreateGraphButton("GraphButtonStyle", "./Images/arrow-up.png");

                Button btnGraphSettings = CreateGraphButton("GraphButtonStyle", "./Images/cog.png");

                RowDefinition firstRow = new RowDefinition();
                firstRow.MinHeight = 30;
                firstRow.MaxHeight = 30;
                cell.RowDefinitions.Add(firstRow);
                CreateRows(cell, 4);
                CreateColumns(cell, 6);

                cell.VerticalAlignment = VerticalAlignment.Stretch;
                cell.HorizontalAlignment = HorizontalAlignment.Stretch;

                btnMove.PreviewMouseDown += GraphDragOn_MouseDown;
                btnMove.PreviewMouseUp += GraphDragOff_MouseUp;
                btnRemove.Click += RemoveGraph_Click;

                btnWidthPlus.Click += GrapheWidthUp_Click;
                btnWidthMinus.Click += GrapheWidthDown_Click;
                btnHeightPlus.Click += GrapheHeightUp_Click;
                btnHeightMinus.Click += GrapheHeightDown_Click;

                btnGraphSettings.Click += OpenGraphSettings_Click;

                Grid.SetColumn(btnWidthPlus, 0);
                Grid.SetRow(btnWidthPlus, 0);

                Grid.SetColumn(btnWidthMinus, 1);
                Grid.SetRow(btnWidthMinus, 0);

                Grid.SetColumn(btnHeightPlus, 2);
                Grid.SetRow(btnHeightPlus, 0); 
                
                Grid.SetColumn(btnHeightMinus, 3);
                Grid.SetRow(btnHeightMinus, 0);

                Grid.SetColumn(btnMove, 4);
                Grid.SetRow(btnMove, 0);

                Grid.SetColumn(btnRemove, 5);
                Grid.SetRow(btnRemove, 0);

                Grid.SetColumn(btnGraphSettings, 0);
                Grid.SetColumnSpan(btnGraphSettings, 6);
                Grid.SetRow(btnGraphSettings, 1);
                Grid.SetRowSpan(btnGraphSettings, 4);

                cell.Children.Add(btnMove);
                cell.Children.Add(btnRemove);
                cell.Children.Add(btnWidthPlus);
                cell.Children.Add(btnWidthMinus);
                cell.Children.Add(btnHeightPlus);
                cell.Children.Add(btnHeightMinus);
                cell.Children.Add(btnGraphSettings);

                cell.Background = Brushes.Green;

                Grid.SetColumn(cell, x);
                Grid.SetRow(cell, y);

                gridHasContent[x, y] = true;
                GraphicData grpData = new GraphicData(x, y, 1, 1, new int[] { 0 });
                graphicDatas[x, y] = grpData;
                dynamicGrid.Children.Add(cell);
            }
        }

        private void CreateColumns(Grid cell, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                cell.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        private void CreateRows(Grid cell, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                cell.RowDefinitions.Add(new RowDefinition());
            }
        }

        private Button CreateGraphButton(string style, string imageSource)
        {
            Button btn = new Button();
            btn.Style = this.FindResource(style) as Style;
            Image img = new Image();
            img.Source = new BitmapImage(new Uri(imageSource, UriKind.Relative));
            img.Height = 30;
            btn.Content = img;
            btn.VerticalAlignment = VerticalAlignment.Stretch;
            btn.HorizontalAlignment = HorizontalAlignment.Stretch;
            return btn;
        }

        private void RemoveGraph_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Grid cell = VisualTreeHelper.GetParent(btn) as Grid;

            int x = Grid.GetColumn(cell), y = Grid.GetRow(cell);
            int columnSpan = Grid.GetColumnSpan(cell), rowSpan = Grid.GetRowSpan(cell);
            SetCellsContent(x, y, x + columnSpan, y + rowSpan, false);
            graphicDatas[x, y].SetAsNull();
            dynamicGrid.Children.Remove(cell);
        }

        private void GraphDragOn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Button btn = (Button)sender;
            Grid cell = VisualTreeHelper.GetParent(btn) as Grid;
            oldX = Grid.GetColumn(cell);
            oldY = Grid.GetRow(cell);
            int columnSpan = Grid.GetColumnSpan(cell), rowSpan = Grid.GetRowSpan(cell);
            SetCellsContent(oldX, oldY, oldX + columnSpan, oldY + rowSpan, false); // Libère l'espace anciennement occupé
        }

        private void GraphDragOff_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Button btn = (Button)sender;
            Grid cell = VisualTreeHelper.GetParent(btn) as Grid;
            int[] newCoordinates = GetCoordinateWithSize(Mouse.GetPosition(dynamicGrid).X, Mouse.GetPosition(dynamicGrid).Y);

            //if (!gridHasContent[newCoordinates[0], newCoordinates[1]]) // ChechIfCellsEmpty
            int x = newCoordinates[0], y = newCoordinates[1];
            int columnSpan = Grid.GetColumnSpan(cell), rowSpan = Grid.GetRowSpan(cell);
            if (ChechIfCellsEmpty(x, y, x + columnSpan, y + rowSpan)) 
            { 
                // repositionne l'élément
                Grid.SetColumn(cell, newCoordinates[0]);
                Grid.SetRow(cell, newCoordinates[1]);
                SetCellsContent(x, y, x + columnSpan, y + rowSpan, true); // Bloque le nouvel espace occupé
                graphicDatas[x, y] = graphicDatas[oldX, oldY].CloneInNewLocation(x,y);
                graphicDatas[oldX, oldY].SetAsNull();
            }
            else
            {
                SetCellsContent(oldX, oldY, oldX + columnSpan, oldY + rowSpan, true); // Aucun nouvel emplacement trouvé, on rebloque l'ancienne espace liberé
            }
            oldX = MAX_GRID_SIZE + 1;
            oldY = MAX_GRID_SIZE + 1;
        }

        private void GrapheWidthUp_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Grid cell = VisualTreeHelper.GetParent(btn) as Grid;

            int x = Grid.GetColumn(cell);
            int y = Grid.GetRow(cell);
            int columnSpan = Grid.GetColumnSpan(cell);
            int rowSpan = Grid.GetRowSpan(cell);

            if (ChechIfCellsEmpty(x + columnSpan, y, x + columnSpan + 1, y + rowSpan) && columnSpan < maxCellColumnSpan)
            {
                Grid.SetColumnSpan(cell, columnSpan + 1);
                SetCellsContent(x, y, x + columnSpan + 1, y + rowSpan, true);
                graphicDatas[x, y].SpanX++;
            }
        }

        private void GrapheWidthDown_Click(object sender, RoutedEventArgs e)
        {

            Button btn = (Button)sender;
            Grid cell = VisualTreeHelper.GetParent(btn) as Grid;

            int x = Grid.GetColumn(cell);
            int y = Grid.GetRow(cell);
            int columnSpan = Grid.GetColumnSpan(cell);
            int rowSpan = Grid.GetRowSpan(cell);
            if (Grid.GetColumnSpan(cell) > 1)
            {
                Grid.SetColumnSpan(cell, columnSpan - 1);
                SetCellsContent(x + 1, y, x + columnSpan + 1, y + rowSpan, false);
                graphicDatas[x, y].SpanX--;
            }
        }

        private void GrapheHeightUp_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Grid cell = VisualTreeHelper.GetParent(btn) as Grid;

            int x = Grid.GetColumn(cell);
            int y = Grid.GetRow(cell);
            int columnSpan = Grid.GetColumnSpan(cell);
            int rowSpan = Grid.GetRowSpan(cell);

            if (ChechIfCellsEmpty(x, y + rowSpan, x + columnSpan, y + rowSpan + 1) && rowSpan < maxCellRowSpan)
            {
                Grid.SetRowSpan(cell, rowSpan + 1);
                SetCellsContent(x, y, x + columnSpan, y + rowSpan + 1, true);

                graphicDatas[x, y].SpanY++;
            }
        }

        private void GrapheHeightDown_Click(object sender, RoutedEventArgs e)
        {

            Button btn = (Button)sender;
            Grid cell = VisualTreeHelper.GetParent(btn) as Grid;

            int x = Grid.GetColumn(cell);
            int y = Grid.GetRow(cell);
            int columnSpan = Grid.GetColumnSpan(cell);
            int rowSpan = Grid.GetRowSpan(cell);

            if (Grid.GetRowSpan(cell) > 1)
            {
                Grid.SetRowSpan(cell, rowSpan - 1);
                SetCellsContent(x, y + 1, x + columnSpan, y + rowSpan + 1, false);
                graphicDatas[x, y].SpanY--;
            }
        }

        private void OpenGraphSettings_Click(object sender, RoutedEventArgs e)
        {
            int graphCellX;
            int graphCellY;
            int sizeX;
            int sizeY;
            Button btn = (Button)sender;
            Grid cell = VisualTreeHelper.GetParent(btn) as Grid;
            graphCellX = Grid.GetColumn(cell);
            graphCellY = Grid.GetRow(cell);
            sizeX = Grid.GetColumnSpan(cell);
            sizeY = Grid.GetRowSpan(cell);
            WindowGraph graphicWindow = new WindowGraph(graphCellX, graphCellY, sizeX, sizeY);
            graphicWindow.OnSave += new SaveEventHandler(OnSave);
            graphicWindow.Show();
        }

        static void OnSave(object source, GraphicData e)
        {
            graphicDatas[e.X, e.Y] = e;
        }

        public GraphicData[,] GetGraphicsData()
        {
            return graphicDatas;
        }

        public Grid GetGrid()
        {
            Grid result = new Grid();
            result.VerticalAlignment = VerticalAlignment.Stretch;
            result.HorizontalAlignment = HorizontalAlignment.Stretch;

            for (int i = 0; i < grdContent.ColumnDefinitions.Count; i++)
            {
                ColumnDefinition newGridColumn = new ColumnDefinition();
                newGridColumn.MinWidth = COLUMN_MIN_WIDTH;
                result.ColumnDefinitions.Add(newGridColumn);
            }

            for (int i = 0; i < grdContent.RowDefinitions.Count; i++)
            {
                RowDefinition newGridRow = new RowDefinition();
                newGridRow.MinHeight = ROW_MIN_HEIGHT;
                result.RowDefinitions.Add(newGridRow);
            }
            return result;
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
                    if (x < gridHasContent.GetLength(0) && y < gridHasContent.GetLength(1))
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
                    if (x < gridHasContent.GetLength(0) && y < gridHasContent.GetLength(1) && gridHasContent[x, y])
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
