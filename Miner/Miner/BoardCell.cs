using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Miner
{
    class BoardCell
    {
        //типы ячеек
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
            Maybe,
            MaybeSelect,
            FlagSelect
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

            set
            {
                if (value == true && !_suffixPress && _suffixClose)
                {
                    _suffixPress = true;
                }

                if (value == false && _suffixPress && _suffixClose)
                {
                    _suffixPress = false;
                }
            }
        }

        /// <summary>
        /// суффикс состояния закрытия ячейки
        /// </summary>
        public bool SuffixClose
        {
            get { return _suffixClose; }

            set
            {
                if (value == true)
                {
                    _suffixClose = true;
                }

                if (value == false && _suffixClose)
                {
                    _suffixClose = false;
                }
            }
        }

        /// <summary>
        /// суффикс состояния выделения ячейки
        /// </summary>
        public bool SuffixSelect
        {
            get { return _suffixSelect; }

            set
            {
                if (value == true && !_suffixSelect && _suffixClose)
                {
                    _suffixSelect = true;
                }

                if (value == false && _suffixSelect && _suffixClose)
                {
                    _suffixSelect = false;
                }
            }
        }

        /// <summary>
        /// суффикс флажка на клетке
        /// </summary>
        public bool SuffixFlag
        {
            get { return _suffixFlag; }
        }

        /// <summary>
        /// проверка наличия мины
        /// </summary>
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
        /// проверка пустой клетки
        /// </summary>
        public bool EmptyCell
        {
            get
            {
                if (type == TypeCell.Empty)
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
            _suffixClose = true;
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

                if (_suffixFlag && _suffixSelect)
                {
                    y = SheetSellSize * (int)TypeCell.FlagSelect + Offset;
                }

                if (_suffixMaybe)
                {
                    y = SheetSellSize * (int)TypeCell.Maybe + Offset;
                }

                if (_suffixMaybe && _suffixSelect)
                {
                    y = SheetSellSize * (int)TypeCell.MaybeSelect + Offset;
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

        /// <summary>
        /// устанавливает мину в ячейку
        /// </summary>
        public void AddMine()
        {
            type = TypeCell.Mine;
        }

        /// <summary>
        /// смена типа ячейки на цифру количесва мин вокруг ячейки
        /// </summary>
        /// <param name="count">количество мин вокруг</param>
        public void AddNumber(int count)
        {
            type = (TypeCell)count;
        }

        /// <summary>
        /// циклично изменяет флажок - вопрос - пусто
        /// </summary>
        public void ChangeFlag()
        {
            if (_suffixClose && _suffixSelect)
            {
                if (!_suffixFlag && !_suffixMaybe)
                {
                    _suffixFlag = true;
                    return;
                }

                if (_suffixFlag)
                {
                    _suffixFlag = false;
                    _suffixMaybe = true;
                    return;
                }

                if (_suffixMaybe)
                {
                    _suffixMaybe = false;
                    return;
                }
            }
        }
    }
}
