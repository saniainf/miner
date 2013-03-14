using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Miner
{
    class Controls
    {
        List<ControlsPiece> ListControls;

        public Controls()
        {
            ListControls = new List<ControlsPiece>();
            ListControls.Add(new ControlsPiece(430, 0, "New"));
            ListControls.Add(new ControlsPiece(510, 0, "Options"));
            ListControls.Add(new ControlsPiece(66, 430, "SmileButton"));
        }

        public Rectangle GetControlRect(int index)
        {
            return ListControls[index].GetControlRect();
        }

        public string GetName(int index)
        {
            return ListControls[index].GetName();
        }

        public int CountControls()
        {
            return ListControls.Count;
        }

        public void Clear2Suffix(int index)
        {
            if (ListControls[index].SuffixPress)
                ListControls[index].SuffixPress = false;
            if (ListControls[index].SuffixSelect)
                ListControls[index].SuffixSelect = false;
        }

        public void CheckSuffixCell(int index, MouseState mouseState)
        {
            ListControls[index].SuffixSelect = true;

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                ListControls[index].SuffixPress = true;
            }
        }

    }
}
