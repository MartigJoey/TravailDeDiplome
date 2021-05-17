using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CovidPropagation
{
    public struct GraphicData
    {
        public GraphicData(int x, int y, int sizeX, int sizeY, int[] datas, int graphicType = 0, int valueX = 0, int valueY = 0)
        {
            X = x;
            Y = y;
            SpanX = sizeX;
            SpanY = sizeY;
            GraphicType = graphicType;
            AxisX = valueX;
            AxisY = valueY;
            Datas = datas;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int SpanX { get; set; }
        public int SpanY { get; set; }
        public int GraphicType { get; set; }
        public int AxisX { get; set; }
        public int AxisY { get; set; }
        public int[] Datas { get; set; }

        public override string ToString()
        {
            string result = $"X: {X}{Environment.NewLine}" +
                   $"Y: {Y}{Environment.NewLine}" +
                   $"SizeX: {SpanX}{Environment.NewLine}" +
                   $"SizeY: {SpanY}{Environment.NewLine}" +
                   $"GraphicType: {GraphicType}{Environment.NewLine}" +
                   $"ValueX: {AxisX}{Environment.NewLine}" +
                   $"ValueY: {AxisY}{Environment.NewLine}" +
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
            GraphicType = 0;
            AxisX = 0;
            AxisY = 0;
            Datas = new int[0];
        }

        public GraphicData CloneInNewLocation(int newX, int newY)
        {
            return new GraphicData(newX, newY, SpanX, SpanY, Datas, GraphicType, AxisX, AxisY);
        }
    }
}
