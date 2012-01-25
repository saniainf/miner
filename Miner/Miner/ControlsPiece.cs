using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Miner
{
    class ControlsPiece
    {
        public const int ControlsWidth = 80;
        public const int ControlsHeight = 22;

        private bool _suffixSelect;
        private bool _suffixPress;

        private int pixelX;
        private int pixelY;

        public string Name;

        /// <summary>
        /// суффикс состояния нажатия на кнопку
        /// </summary>
        public bool SuffixPress
        {
            get { return _suffixPress; }

            set
            {
                if (value == true && !_suffixPress)
                {
                    _suffixPress = true;
                }

                if (value == false && _suffixPress)
                {
                    _suffixPress = false;
                }
            }
        }

        /// <summary>
        /// суффикс состояния выделения кнопки
        /// </summary>
        public bool SuffixSelect
        {
            get { return _suffixSelect; }

            set
            {
                if (value == true && !_suffixSelect)
                {
                    _suffixSelect = true;
                }

                if (value == false && _suffixSelect)
                {
                    _suffixSelect = false;
                }
            }
        }

        /// <summary>
        /// элимент управления
        /// </summary>
        /// <param name="x">X в текстуре</param>
        /// <param name="y">Y в текстуре</param>
        public ControlsPiece(int x, int y, string name)
        {
            pixelX = x;
            pixelY = y;
            Name = name;
            _suffixPress = false;
            _suffixSelect = false;
        }

        public Rectangle GetControlRect()
        {
            int x = pixelX;
            int y = pixelY;

            if (_suffixSelect)
                y = y + ControlsHeight;

            if (_suffixPress)
                y = y + ControlsHeight;

            return new Rectangle(x, y, ControlsWidth, ControlsHeight);
        }

        public string GetName()
        {
            return Name;
        }
    }
}
