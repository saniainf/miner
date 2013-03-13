using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Miner
{
    class SmileButton
    {
        public const int SmileWidth = 40;
        public const int SmileHeight = 40;
        
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
        /// элемент управления
        /// </summary>
        /// <param name="x">X в текстуре</param>
        /// <param name="y">Y в текстуре</param>
        public SmileButton (int x, int y, string name)
        {
            pixelX = x;
            pixelY = y;
            Name = name;
            _suffixPress = false;
        }

        public Rectangle GetSmileRect()
        {
            int x = pixelX;
            int y = pixelY;

            if (_suffixPress)
                y = y + SmileHeight;

            return new Rectangle(x, y, SmileWidth, SmileHeight);
        }

        public string GetName()
        {
            return Name;
        }
    }
    }
}
