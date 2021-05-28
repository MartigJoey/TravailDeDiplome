using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CovidPropagation
{
    public struct ChartData
    {
        public ChartData(int x, int y, int sizeX, int sizeY, int[] datas, int graphicType = 0, int valueX = 0, int valueY = 0, int displayInterval = 0, bool autoDisplay = true, int displayWindow = 0)
        {
            X = x;
            Y = y;
            SpanX = sizeX;
            SpanY = sizeY;
            Datas = datas;
            ChartType = graphicType;
            AxisX = valueX;
            AxisY = valueY;
            DisplayInterval = displayInterval;
            AutoDisplay = autoDisplay;
            DisplayWindow = displayWindow;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int SpanX { get; set; }
        public int SpanY { get; set; }
        public int ChartType { get; set; }
        public int AxisX { get; set; }
        public int AxisY { get; set; }
        public int DisplayInterval { get; set; }
        public int[] Datas { get; set; }
        public bool AutoDisplay { get; set; }
        public int DisplayWindow { get; set; }

        public override string ToString()
        {
            string result = $"X: {X}{Environment.NewLine}" +
                   $"Y: {Y}{Environment.NewLine}" +
                   $"SizeX: {SpanX}{Environment.NewLine}" +
                   $"SizeY: {SpanY}{Environment.NewLine}" +
                   $"GraphicType: {ChartType}{Environment.NewLine}" +
                   $"ValueX: {AxisX}{Environment.NewLine}" +
                   $"ValueY: {AxisY}{Environment.NewLine}" +
                   $"UpdateInterval: {DisplayInterval}{Environment.NewLine}" +
                   $"Datas: {Environment.NewLine}";
            foreach (int item in Datas)
            {
                result += $"{item} {Environment.NewLine}";
            }
            return result;
        }

        public void SetAsNull()
        {
            X = 0;
            Y = 0;
            SpanX = 0;
            SpanY = 0;
            ChartType = 0;
            AxisX = 0;
            AxisY = 0;
            Datas = new int[0];
            AutoDisplay = true;
            DisplayWindow = 0;
        }

        public ChartData CloneInNewLocation(int newX, int newY)
        {
            return new ChartData(newX, newY, SpanX, SpanY, Datas, ChartType, AxisX, AxisY, DisplayInterval, AutoDisplay, DisplayWindow);
        }
    }
}
