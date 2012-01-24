﻿using System;
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

            boardSquares = new BoardCell[width, height];

            ClearBoard();
        }

        /// <summary>
        /// просто очистка всего поля и установка новых мин
        /// </summary>
        private void ClearBoard()
        {
            for (int x = 0; x < GameBoardWidth; x++)
            {
                for (int y = 0; y < GameBoardHeight; y++)
                {
                    boardSquares[x, y] = new BoardCell(BoardCell.TypeCell.Empty);
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
        /// обнуление 2 суффиксов ячейки, Press & Select
        /// </summary>
        /// <param name="x">X ячейки</param>
        /// <param name="y">Y ячейки</param>
        public void Clear2Suffix(int x, int y)
        {
            if (boardSquares[x, y].SuffixPress && !boardSquares[x, y].SuffixSelect) // недаст убрать PressOn пока ячейка выделена
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
            List<Point> ListCells = new List<Point>();
            ListCells.Add(new Point(startX, startY));
            int preCount;

            do
            {
                preCount = ListCells.Count - 1;
                int X = (int)ListCells[ListCells.Count - 1].X;
                int Y = (int)ListCells[ListCells.Count - 1].Y;

                for (int miniX = X - 1; miniX <= X + 1; miniX++)
                {
                    for (int miniY = Y - 1; miniY <= Y + 1; miniY++)
                    {
                        if (miniX >= 0 && miniY >= 0 && miniX < GameBoardWidth && miniY < GameBoardHeight)
                        {
                            if (boardSquares[miniX, miniY].SuffixClose)
                            {
                                if (boardSquares[miniX, miniY].EmptyCell)
                                {
                                    ListCells.Add(new Point(miniX, miniY));
                                }
                                boardSquares[miniX, miniY].SuffixClose = false;
                                // TODO: разобраться с рекурсией
                                if (boardSquares[miniX, miniY].EmptyCell)
                                    OpenEmptyCell(miniX, miniY);
                                Clear2Suffix(miniX, miniY);
                            }
                        }
                    }
                }
            } while (ListCells.Count - 1 != preCount);
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

                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    boardSquares[x, y].SuffixPress = true; // если нажата кнопка то ставим pressON
                }

                if (boardSquares[x, y].SuffixPress && mouseState.LeftButton == ButtonState.Released) // если PressON и кнопка ненажата то открываем ячейку
                {
                    boardSquares[x, y].SuffixClose = false;  // ячейка открыта
                    boardSquares[x, y].SuffixPress = false;  // обнуляем все 
                    boardSquares[x, y].SuffixSelect = false; // суффиксы

                    if (boardSquares[x, y].EmptyCell) // если пустая то открываем соседнии пустые
                        OpenEmptyCell(x, y);
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
    }
}
