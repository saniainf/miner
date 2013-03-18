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
    class GameBoard
    {
        private CellGameBoard[,] _board;
        public int _countMines;
        int _numberCells;
        Rectangle _gameBoardBBox;
        Point _countCells;
        Point _cellSize;

        Random rnd = new Random();

        public Rectangle gameBoardBBox
        {
            get { return _gameBoardBBox; }
        }

        /// <summary>
        /// Игровое поле
        /// </summary>
        /// <param name="width">высота в ячейках</param>
        /// <param name="height">ширина в ячейках</param>
        /// <param name="numMines">количество мин</param>
        public GameBoard(int width, int height, int countMines, Point spaceOver, Point cellSize)
        {
            _countMines = countMines;
            _numberCells = width * height;
            _countCells.X = width;
            _countCells.Y = height;
            _board = new CellGameBoard[width, height];
            _gameBoardBBox = new Rectangle(spaceOver.X, spaceOver.Y + spaceOver.X, width * cellSize.X, height * cellSize.Y);
            _cellSize = cellSize;
            ClearBoard();
        }

        public void gameBoardUpdate(MouseState mouseState)
        {
            if (_gameBoardBBox.Contains(mouseState.X, mouseState.Y))
            {
                for (int x = 0; x < _countCells.X; x++)
                {
                    for (int y = 0; y < _countCells.Y; y++)
                    {
                        if (_board[x, y].cellUpdate(mouseState))
                            OpenEmptyCell(1, 2);
                    }
                }
            }
        }

        public void gameBoardDraw(SpriteBatch spriteBatch, Texture2D tileSheet)
        {
            for (int x = 0; x < _countCells.X; x++)
            {
                for (int y = 0; y < _countCells.Y; y++)
                {
                    spriteBatch.Draw(tileSheet,
                        _board[x,y].boundingBox,
                        _board[x,y].GetBBoxSheet(),
                        Color.White);

                }
            }
        }

        /// <summary>
        /// очистка всего поля и установка новых мин
        /// </summary>
        public void ClearBoard()
        {
            for (int x = 0; x < _countCells.X; x++)
            {
                for (int y = 0; y < _countCells.Y; y++)
                {
                    _board[x, y] = new CellGameBoard(
                        new Rectangle(
                            gameBoardBBox.X + _cellSize.X * x,
                            gameBoardBBox.Y + _cellSize.Y * y,
                            _cellSize.X,
                            _cellSize.Y),
                        new Point(0, 0));
                }
            }
            PlaceMines();
            NumberInCell();
        }

        /// <summary>
        /// размещение мин на поле
        /// </summary>
        private void PlaceMines()
        {
            for (int i = _countMines; i > 0; i--)
            {
                int x = rnd.Next(_countCells.X);
                int y = rnd.Next(_countCells.Y);

                if (!_board[x, y].MineHave)
                    _board[x, y].AddMine();
                else i++;
            }
        }

        /// <summary>
        /// вычесление количества мин вокруг ячейки
        /// </summary>
        private void NumberInCell()
        {
            for (int x = 0; x < _countCells.X; x++)
            {
                for (int y = 0; y < _countCells.Y; y++)
                {
                    int count = 0;
                    if (!_board[x, y].MineHave)
                    {
                        for (int miniX = x - 1; miniX <= x + 1; miniX++)
                        {
                            for (int miniY = y - 1; miniY <= y + 1; miniY++)
                            {
                                if (miniX >= 0 && miniY >= 0 && miniX < _countCells.X && miniY < _countCells.Y)
                                {
                                    if (_board[miniX, miniY].MineHave)
                                    {
                                        count = count + 1;
                                    }
                                }
                            }
                        }
                    }
                    if (count > 0)
                    {
                        _board[x, y].AddNumber(count);
                    }
                }
            }
        }

        #region old methods
        
        /// <summary>
        /// открыть все пустые ячейки рядом с начальной
        /// </summary>
        /// <param name="startX">X начальноя ячейки</param>
        /// <param name="startY">Y начальной ячейки</param>
        public void OpenEmptyCell(int startX, int startY)
        {
            for (int miniX = startX - 1; miniX <= startX + 1; miniX++) // цикл по X квадратика
            {
                for (int miniY = startY - 1; miniY <= startY + 1; miniY++) // цикл по Y квадратика
                {
                    if (miniX >= 0 && miniY >= 0 && miniX < GameBoardWidth && miniY < GameBoardHeight) // если невыходит за пределы поля
                    {
                        if (boardSquares[miniX, miniY].SuffixClose && !boardSquares[miniX,miniY].SuffixFlag) // если закрыта
                        {
                            boardSquares[miniX, miniY].SuffixClose = false; // открыть
                            NumberCells--; // удаляем из счетчика ячеек
                            Clear2Suffix(miniX, miniY); // обнулить суффиксы
                            // TODO: разобраться с рекурсией
                            if (boardSquares[miniX, miniY].EmptyCell) // если пустая то вызвать для нее OpenEmptyCell
                                OpenEmptyCell(miniX, miniY);
                        }
                    }
                }
            }
        }
        /*
        /// <summary>
        /// проверка состояния ячейки и установка соответствующих суффиксов
        /// </summary>
        /// <param name="x">X ячейки</param>
        /// <param name="y">Y ячейки</param>
        /// <param name="mouseState"></param>
        public void CheckSuffixCell(int x, int y, MouseState mouseState)
        {
            if (boardSquares[x, y].SuffixClose) // если закрыта
            {
                boardSquares[x, y].SuffixSelect = true; // ставим selectOn

                if (mouseState.LeftButton == ButtonState.Pressed && !boardSquares[x,y].SuffixFlag)
                {
                    boardSquares[x, y].SuffixPress = true; // если нажата кнопка и нет флажка то ставим pressON
                }

                if (boardSquares[x, y].SuffixPress && mouseState.LeftButton == ButtonState.Released) // если PressON и кнопка ненажата то открываем ячейку
                {
                    boardSquares[x, y].SuffixClose = false;  // ячейка открыта
                    boardSquares[x, y].SuffixPress = false;  // обнуляем все 
                    boardSquares[x, y].SuffixSelect = false; // суффиксы
                    NumberCells--; // удаляем из счетчика ячеек

                    if (boardSquares[x, y].EmptyCell) // если пустая то открываем соседние пустые
                        OpenEmptyCell(x, y);
                }

                if (mouseState.RightButton == ButtonState.Pressed && MouseRightButton)
                {
                    boardSquares[x, y].ChangeFlag();
                    MouseRightButton = false;
                }

                if (mouseState.RightButton == ButtonState.Released)
                {
                    MouseRightButton = true;
                }
            }
        }

        /// <summary>
        /// возвращает Rectangle текструры конкретной ячейки в TileSheet
        /// </summary>
        /// <param name="x">измерение массива</param>
        /// <param name="y">измерение массива</param>
        /// <returns></returns>
        public Rectangle GetTileRect(int x, int y)
        {
            return boardSquares[x, y].GetTileRect();
        }

        /// <summary>
        /// проверка Выиграл / Проиграл
        /// </summary>
        /// <param name="x">X ячейка</param>
        /// <param name="y">Y ячейка</param>
        public void WinGameOver(int x, int y)
        {
            // проиграл :=(
            if (boardSquares[x,y].MineHave && !boardSquares[x,y].SuffixClose)
            {
                //ClearBoard();
            }

            // выиграл :=)
            if (NumberMines == NumberCells)
            {
                //ClearBoard();
            }
        }
        */
        #endregion
    }
}
