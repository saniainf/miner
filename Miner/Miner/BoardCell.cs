using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Miner
{
    class BoardCell
    {
        public enum TypeCell
        {
            Num1,
            Num2,
            Num3,
            Num4,
            Num5,
            Num6,
            Num7,
            Num8,
            Flag,
            Mine,
            Close,
            Empty
        }
        public TypeCell type;

        public const int CellHeight = 20;
        public const int CellWidth = 20;

        public const int SheetSellSize = 30;

        public BoardCell(TypeCell type)
        {
            this.type = type;
        }

        public Rectangle GetTileRect()
        {
            int x = 0;
            int y = SheetSellSize * (int)type;

            return new Rectangle(x, y, CellWidth, CellHeight);
        }
    }
}
