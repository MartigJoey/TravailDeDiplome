using System;
using System.Collections.Generic;
using System.Text;

namespace CovidPropagation
{
    public struct GraphicDataTransfer
    {
        public GraphicDataTransfer(int x, int y, int sizeX, int sizeY, int graphicType, int valueX, int valueY, int[] datas)
        {
            X = x;
            Y = y;
            SizeX = sizeX;
            SizeY = sizeY;
            GraphicType = graphicType;
            ValueX = valueX;
            ValueY = valueY;
            Datas = datas;
        }

        public int X { get; }
        public int Y { get; }
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public int GraphicType { get; }
        public int ValueX { get; }
        public int ValueY { get; }
        public int[] Datas { get; }
    }
}
