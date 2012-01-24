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
            Select,
            Maybe
        }
        private TypeCell type;

        public const int CellHeight = 20;
        public const int CellWidth = 20;

        public const int Offset = 1;

        public const int SheetSellSize = 30;

        private bool _suffixPress;
        private bool _suffixClose;
        private bool _suffixSelect;
        private bool _suffixFlag;
        private bool _suffixMaybe;

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
        /// суффикс флажка
        /// </summary>
        public bool SuffixFlag
        {
            get { return _suffixFlag; }
        }

        /// <summary>
        /// суффикс вопроса
        /// </summary>
        public bool SuffixMaybe
        {
            get { return _suffixMaybe; }
        }

        public bool MineHave
        {
            get
            {
                if (type == TypeCell.Mine)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// ячейка игрового поля
        /// </summary>
        /// <param name="type">тип ячейки</param>
        public BoardCell(TypeCell type)
        {
            this.type = type;
            _suffixClose = false;
            _suffixPress = false;
            _suffixSelect = false;
            _suffixFlag = false;
            _suffixMaybe = false;
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

                if (_suffixFlag)
                {
                    y = SheetSellSize * (int)TypeCell.Flag + Offset;
                }

                if (_suffixMaybe)
                {
                    y = SheetSellSize * (int)TypeCell.Maybe + Offset;
                }

                if (_suffixPress && !(_suffixFlag || _suffixMaybe))
                {
                    y = SheetSellSize * (int)TypeCell.Empty + Offset;
                }

                else if (_suffixSelect && !(_suffixFlag || _suffixMaybe))
                {
                    y = SheetSellSize * (int)TypeCell.Select + Offset;
                }
            }
            return new Rectangle(x, y, CellWidth, CellHeight);
        }

        /* установка мины */
        public void AddMine()
        {
            type = TypeCell.Mine;
        }

        /* установка цифры количесва мин вокруг ячейки */
        public void AddNumber(int count)
        {
            type = (TypeCell)count;
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

        public void FlagON()
        {
            _suffixFlag = true;
        }

        public void FlagOFF()
        {
            _suffixFlag = false;
        }

        public void MaybeON()
        {
            _suffixMaybe = true;
        }

        public void MaybeOFF()
        {
            _suffixMaybe = false;
        }
    }
}
