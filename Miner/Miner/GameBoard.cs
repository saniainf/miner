using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Miner
{
    class GameBoard
    {
        public int GameBoardWidth;
        public int GameBoardHeight;

        private BoardCell[,] boardSquares;

        public int NumberMines;
        int NumberCells;

        bool MouseRightButton = true;

        Random rnd = new Random();

        /// <summary>
        /// Игровое поле
        /// </summary>
        /// <param name="width">высота в ячейках</param>
        /// <param name="height">ширина в ячейуах</param>
        /// <param name="numMines">количество мин</param>
        public GameBoard(int width, int height, int numMines)
        {
            GameBoardWidth = width;
            GameBoardHeight = height;
            NumberMines = numMines;
            NumberCells = GameBoardHeight * GameBoardWidth;

            boardSquares = new BoardCell[width, height];

            ClearBoard();
        }

        /// <summary>
        /// очистка всего поля и установка новых мин
        /// </summary>
        public void ClearBoard()
        {
            for (int x = 0; x < GameBoardWidth; x++)
            {
                for (int y = 0; y < GameBoardHeight; y++)
                {
                    boardSquares[x, y] = new BoardCell();
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
            for (int i = NumberMines; i > 0; i--)
            {
                int x = rnd.Next(GameBoardWidth);
                int y = rnd.Next(GameBoardHeight);

                if (!boardSquares[x, y].MineHave)
                    boardSquares[x, y].AddMine();
                else i++;
            }
        }

        /// <summary>
        /// вычесление количества мин вокруг ячейки
        /// </summary>
        private void NumberInCell()
        {
            for (int x = 0; x < GameBoardWidth; x++)
            {
                for (int y = 0; y < GameBoardHeight; y++)
                {
                    int count = 0;
                    //////////////////////////////////////////////////////////////////////////
                    if (!boardSquares[x, y].MineHave)
                    {
                        for (int miniX = x - 1; miniX <= x + 1; miniX++)
                        {
                            for (int miniY = y - 1; miniY <= y + 1; miniY++)
                            {
                                if (miniX >= 0 && miniY >= 0 && miniX < GameBoardWidth && miniY < GameBoardHeight)
                                {
                                    if (boardSquares[miniX, miniY].MineHave)
                                    {
                                        count = count + 1;
                                    }
                                }
                            }
                        }
                    }
                    //////////////////////////////////////////////////////////////////////////
                    if (count > 0)
                    {
                        boardSquares[x, y].AddNumber(count);
                    }
                }
            }
        }

        /// <summary>
        /// обнуление 2 суффиксов ячейки Press Select
        /// </summary>
        /// <param name="x">X ячейки</param>
        /// <param name="y">Y ячейки</param>
        public void Clear2Suffix(int x, int y)
        {
            if (boardSquares[x, y].SuffixPress && !boardSquares[x, y].SuffixSelect) // недаст убрать PressOn пока ячейка Select
                boardSquares[x, y].SuffixPress = false;
            if (boardSquares[x, y].SuffixSelect)
                boardSquares[x, y].SuffixSelect = false;
        }

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
    }
}
