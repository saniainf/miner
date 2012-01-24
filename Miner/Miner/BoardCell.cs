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
            Empty,
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
            Select
        }
        private TypeCell type;

        public const int CellHeight = 20;
        public const int CellWidth = 20;

        public const int Offset = 1;

        public const int SheetSellSize = 30;

        private bool _suffixPress;
        private bool _suffixClose;
        private bool _suffixSelect;

        /// <summary>
        /// суффикс состояния нажатия на ячейку
        /// </summary>
        public bool SuffixPress
        {
            get { return _suffixPress; }
        }

        /// <summary>
        /// суффикс состояния закрытия ячейки
        /// </summary>
        public bool SuffixClose
        {
            get { return _suffixClose; }
        }

        /// <summary>
        /// суффикс состояния выделения ячейки
        /// </summary>
        public bool SuffixSelect
        {
            get { return _suffixSelect; }
        }

        /// <summary>
        /// ячейка игрового поля
        /// </summary>
        /// <param name="type">тип ячейки</param>
        public BoardCell(TypeCell type)
        {
            this.type = type;
            _suffixClose = true;
            _suffixPress = false;
            _suffixSelect = false;
        }

        /// <summary>
        /// возвращает Rectangle текструры ячейки в TileSheet
        /// </summary>
        /// <returns></returns>
        public Rectangle GetTileRect()
        {
            int x = Offset;
            int y = SheetSellSize * (int)type + Offset;

            if (_suffixClose)
            {
                y = SheetSellSize * (int)TypeCell.Close + Offset;

                if (_suffixPress)
                {
                    y = Offset;
                }

                else if (_suffixSelect)
                {
                    y = SheetSellSize * (int)TypeCell.Select + Offset;
                }
            }
            return new Rectangle(x, y, CellWidth, CellHeight);
        }

        /* методы изменения суффиксов */
        public void MouseLBPressON()
        {
            _suffixPress = true;
        }

        public void MouseLBPressOFF()
        {
            _suffixPress = false;
        }

        public void MouseSelectON()
        {
            _suffixSelect = true;
        }

        public void MouseSelectOFF()
        {
            _suffixSelect = false;
        }

        public void CellCloseON()
        {
            _suffixClose = true;
        }

        public void CellCloseOFF()
        {
            _suffixClose = false;
        }
    }
}
