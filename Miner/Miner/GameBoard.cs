using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
            PlaceMines();
            NumberInCell();
        }

        /// <summary>
        /// просто очистка всего поля
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
        /// установка количества мин вокруг ячейки
        /// </summary>
        private void NumberInCell()
        {
            for (int x = 0; x < GameBoardWidth; x++)
            {
                for (int y = 0; y < GameBoardHeight; y++)
                {
                    int count = 0;
                    //////////////////////////////////////////////////////////////////////////
                    if (!boardSquares[x,y].MineHave)
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
        /// возвращает Rectangle текструры конкретной ячейки в TileSheet
        /// </summary>
        /// <param name="x">измерение массива</param>
        /// <param name="y">измерение массива</param>
        /// <returns></returns>
        public Rectangle GetTileRect(int x, int y)
        {
            return boardSquares[x, y].GetTileRect();
        }

        /* методы изменения суффиксов */
        public void MouseLBPressON(int x, int y)
        {
            boardSquares[x, y].MouseLBPressON();
        }

        public void MouseLBPressOFF(int x, int y)
        {
            boardSquares[x, y].MouseLBPressOFF();
        }

        public void MouseSelectON(int x, int y)
        {
            boardSquares[x, y].MouseSelectON();
        }

        public void MouseSelectOFF(int x, int y)
        {
            boardSquares[x, y].MouseSelectOFF();
        }

        public void CellCloseON(int x, int y)
        {
            boardSquares[x, y].CellCloseON();
        }

        public void CellCloseOFF(int x, int y)
        {
            boardSquares[x, y].CellCloseOFF();
        }

        public void FlagON(int x, int y)
        {
            boardSquares[x, y].FlagON();
        }

        public void FlagOFF(int x, int y)
        {
            boardSquares[x, y].FlagOFF();
        }

        public void MaybeON(int x, int y)
        {
            boardSquares[x, y].MaybeON();
        }

        public void MaybeOFF(int x, int y)
        {
            boardSquares[x, y].MaybeOFF();
        }

        /* методы проверки суффиксов */
        public bool MouseLBPress(int x, int y)
        {
            return boardSquares[x, y].SuffixPress;
        }

        public bool MouseSelect(int x, int y)
        {
            return boardSquares[x, y].SuffixSelect;
        }

        public bool CellClose(int x, int y)
        {
            return boardSquares[x, y].SuffixClose;
        }

        public bool Flag(int x, int y)
        {
            return boardSquares[x, y].SuffixFlag;
        }

        public bool Maybe(int x, int y)
        {
            return boardSquares[x, y].SuffixMaybe;
        }
    }
}
