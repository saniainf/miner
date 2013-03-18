using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Miner
{
    class Button
    {
        /// <summary>
        /// состояния кнопки
        /// </summary>
        private enum State
        {
            BTN_NORMAL,
            BTN_HOVER,
            BTN_PRESSED,
        }

        private Rectangle _boundingBox;
        private State _btnState;
        private Point _texturePoint;
        private bool _suffixOne;

        /// <summary>
        /// ограничивающий прямоугольник
        /// </summary>
        public Rectangle boundingBox
        {
            get { return _boundingBox; }
        }

        /// <summary>
        /// класс кнопки
        /// </summary>
        /// <param name="box">ограничивающий прямоугольник</param>
        /// <param name="point">точка начала текстуры в sheet</param>
        /// <param name="suffix">суфикс (можно не указывать)</param>
        public Button(Rectangle box, Point point, bool suffix = false)
        {
            _boundingBox = box;
            _texturePoint = point;
            _btnState = State.BTN_NORMAL;
            _suffixOne = suffix;
        }

        public Action Action { get; set; }

        /// <summary>
        /// апдейт кнопки с сменой состояний
        /// </summary>
        /// <param name="mouseState">мышь</param>
        public void btnUpdate(MouseState mouseState)
        {
            if (_boundingBox.Contains(mouseState.X, mouseState.Y))
            {
                if (_btnState == State.BTN_NORMAL && mouseState.LeftButton == ButtonState.Released)
                    _btnState = State.BTN_HOVER;

                if (_btnState == State.BTN_HOVER && mouseState.LeftButton == ButtonState.Pressed)
                    _btnState = State.BTN_PRESSED;

                if (_btnState == State.BTN_PRESSED && mouseState.LeftButton == ButtonState.Released)
                {
                    if (Action != null)
                        Action.Invoke();
                    _btnState = State.BTN_NORMAL;
                }
            }
            else if (mouseState.LeftButton == ButtonState.Released)
                _btnState = State.BTN_NORMAL;
            else if (mouseState.LeftButton == ButtonState.Pressed && _btnState == State.BTN_PRESSED)
                _btnState = State.BTN_HOVER;

        }

        /// <summary>
        /// возвращает bbox смайла в sheet
        /// </summary>
        /// <returns></returns>
        public Rectangle GetBBoxSheet()
        {
            int x = 0;
            int y = 0;
            switch (_btnState.ToString())
            {
                case "BTN_NORMAL":
                    x = _texturePoint.X;
                    y = _texturePoint.Y;
                    break;

                case "BTN_HOVER":
                    x = _texturePoint.X;
                    y = _texturePoint.Y + _boundingBox.Height;
                    break;

                case "BTN_PRESSED":
                    x = _texturePoint.X;
                    y = _texturePoint.Y + _boundingBox.Height * 2;
                    break;
            }

            if (_suffixOne)
            {
                x = _texturePoint.X;
                y = _texturePoint.Y + _boundingBox.Height * 3;
            }

            return new Rectangle(x, y, _boundingBox.Width, _boundingBox.Height);
        }
    }
}
