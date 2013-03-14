using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Miner
{
    class SmileButton
    {
        enum SmileState
        {
            Normal,
            Fear,
            Cool,
            Dead
        }

        public const int SmileWidth = 42;
        public const int SmileHeight = 42;

        private SmileState smileState;

        private Point smileTexture = new Point(430, 66);
        
        private bool _suffixPress;
        private bool _suffixSelect;
        private bool _suffixFear;

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
        /// суфикс выделения смайла
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

        public bool smileFear
        {
            get { return _suffixFear; }

            set
            {
                if (value == true && !_suffixFear)
                    _suffixFear = true;

                if (value == false && _suffixFear)
                    _suffixFear = false;
            }
        }

        /// <summary>
        /// button smile
        /// </summary>
        /// <param name="x">X в текстуре</param>
        /// <param name="y">Y в текстуре</param>
        public SmileButton ()
        {
            _suffixPress = false;
            _suffixSelect = false;
            _suffixFear = false;
            smileState = SmileState.Normal;
        }

        /// <summary>
        /// возвращает rectangle смайла в sheet
        /// </summary>
        /// <returns></returns>
        public Rectangle GetSmileRect()
        {
            int x = smileTexture.X;
            int y = smileTexture.Y;

            switch (smileState.ToString())
            {
                case "Normal":
                    x = smileTexture.X;
                    break;

                case "Cool":
                    x = smileTexture.X + SmileWidth * (int)SmileState.Cool;
                    break;

                case "Dead":
                    x = smileTexture.X + SmileWidth * (int)SmileState.Dead;
                    break;
            }

            if (_suffixPress)
            {
                y = smileTexture.Y + SmileWidth;
                x = smileTexture.X;
            }

            if (_suffixFear)
            {
                x = smileTexture.X + SmileWidth * (int)SmileState.Fear;
            }

            return new Rectangle(x, y, SmileWidth, SmileHeight);
        }

        /// <summary>
        /// обнуление 3 суффиксов ячейки Press Select
        /// </summary>
        public void Clear3Suffix()
        {
            if (SuffixPress && !SuffixSelect) // недаст убрать PressOn пока ячейка Select
                SuffixPress = false;
            if (SuffixSelect)
                SuffixSelect = false;
            if (smileFear)
                smileFear = false;
        }
    }
}
