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

        public bool smileFear
        {
            set
            {
                if (value == true && smileState != SmileState.Fear)
                    smileState = SmileState.Fear;

                if (value == false && smileState == SmileState.Fear)
                    smileState = SmileState.Normal;
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

                case "Fear":
                    x = smileTexture.X + SmileWidth * (int)SmileState.Fear;
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

            return new Rectangle(x, y, SmileWidth, SmileHeight);
        }
    }
    
}
