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
    class CellGameBoard
    {
        /// <summary>
        /// состояния ячейки
        /// </summary>
        private enum State
        {
            CELL_CLOSE,
            CELL_HOVER,
            CELL_PRESSED,
            CELL_OPEN
        }

        private enum Type
        {
            EMPTY,
            N_1,
            N_2,
            N_3,
            N_4,
            N_5,
            N_6,
            N_7,
            N_8,
            MINE
        }

        private bool _flag;
        private bool _maybe;
        private State _state;
        private Type _type;
        private Point _texturePoint;
        private Rectangle _boundingBox;
        private bool _MRB = true;

        /// <summary>
        /// ограничивающий прямоугольник
        /// </summary>
        public Rectangle boundingBox
        {
            get { return _boundingBox; }
        }

        /// <summary>
        /// ячейка игрового поля
        /// </summary>
        /// <param name="cellBBox">BBox ячейки</param>
        /// <param name="point">точка начала текстуры в sheet</param>
        public CellGameBoard(Rectangle cellBBox, Point point)
        {
            _flag = false;
            _maybe = false;
            _state = State.CELL_CLOSE;
            _type = Type.EMPTY;
            _texturePoint = point;
            _boundingBox = cellBBox;
        }

        public Action Action { get; set; }

        /// <summary>
        /// проверка наличия мины
        /// </summary>
        public bool MineHave
        {
            get
            {
                if (_type == Type.MINE)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// устанавливает мину в ячейку
        /// </summary>
        public void AddMine()
        {
            _type = Type.MINE;
        }

        /// <summary>
        /// смена типа ячейки на цифру количесва мин вокруг ячейки
        /// </summary>
        /// <param name="count">количество мин вокруг</param>
        public void AddNumber(int count)
        {
            _type = (Type)count;
        }

        /// <summary>
        /// апдейт ячейки с сменой состояний
        /// </summary>
        /// <param name="mouseState">мышь</param>
        public bool cellUpdate(MouseState mouseState)
        {
            if (_state != State.CELL_OPEN)
            {
                if (_boundingBox.Contains(mouseState.X, mouseState.Y))
                {
                    if (_state == State.CELL_CLOSE && mouseState.LeftButton == ButtonState.Released)
                        _state = State.CELL_HOVER;

                    if (_state == State.CELL_HOVER && mouseState.LeftButton == ButtonState.Pressed && !_flag)
                        _state = State.CELL_PRESSED;

                    if (_state == State.CELL_PRESSED && mouseState.LeftButton == ButtonState.Released)
                    {
                        _state = State.CELL_OPEN;
                        if (_type == Type.EMPTY)
                            return true;
                        else if (_type == Type.MINE && Action != null)
                            Action.Invoke();
                        else return false;
                    }

                    if (mouseState.RightButton == ButtonState.Pressed && _MRB)
                    {
                        ChangeSuffix();
                    }
                }
                else if (mouseState.LeftButton == ButtonState.Released)
                {
                    _state = State.CELL_CLOSE;
                    _MRB = false;
                }
                else if (mouseState.LeftButton == ButtonState.Pressed && _state == State.CELL_PRESSED)
                    _state = State.CELL_HOVER;
            }

            if (mouseState.RightButton == ButtonState.Released)
                _MRB = true;

            return false;
        }

        void ChangeSuffix()
        {
            if (!_flag && !_maybe)
            {
                _flag = true;
            }

            else if (_flag)
            {
                _flag = false;
                _maybe = true;
            }

            else if (_maybe)
            {
                _maybe = false;
            }
            _MRB = false;
        }

        public bool OpenEmpty()
        {
            if (_state == State.CELL_CLOSE && !_flag)
            {
                _state = State.CELL_OPEN;
                if (_type == Type.EMPTY)
                    return true;
            }
            else return false;
            return false;
        }

        /// <summary>
        /// возвращает bbox cell в sheet
        /// </summary>
        /// <returns></returns>
        public Rectangle GetBBoxSheet()
        {
            int x = 0;
            int y = 0;

            x = _texturePoint.X;
            y = _boundingBox.Height * (int)_state;

            if (_flag)
            {
                if (_state == State.CELL_CLOSE)
                {
                    x = _texturePoint.X + _boundingBox.Width * 2;
                    y = _texturePoint.Y;
                }

                if (_state == State.CELL_HOVER)
                {
                    x = _texturePoint.X + _boundingBox.Width * 2;
                    y = _texturePoint.Y + _boundingBox.Height;
                }
            }

            if (_maybe)
            {
                if (_state == State.CELL_CLOSE)
                {
                    x = _texturePoint.X + _boundingBox.Width * 3;
                    y = _texturePoint.Y;
                }

                if (_state == State.CELL_HOVER)
                {
                    x = _texturePoint.X + _boundingBox.Width * 3;
                    y = _texturePoint.Y + _boundingBox.Height;
                }
            }


            if (_state == State.CELL_OPEN)
            {
                x = _texturePoint.X + _boundingBox.Width;
                y = _boundingBox.Height * (int)_type;
            }

            return new Rectangle(x, y, _boundingBox.Width, _boundingBox.Height);
        }
    }
}
