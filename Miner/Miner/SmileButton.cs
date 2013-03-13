using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner
{
    class SmileButton
    {
        public const int SmileWidth = 40;
        public const int SmileHeight = 40;
        
        private bool _suffixPress;

        private int pixelX;
        private int pixelY;

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
    }
}
