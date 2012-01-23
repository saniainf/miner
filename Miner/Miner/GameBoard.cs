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

        public GameBoard(int width, int height)
        {
            GameBoardWidth = width;
            GameBoardHeight = height;

            boardSquares = new BoardCell[width, height];

            RandomBoard();
        }

        /// <summary>
        /// заполнение игрового поля
        /// </summary>
        private void RandomBoard()
        {
            for (int x = 0; x < GameBoardWidth; x++)
            {
                for (int y = 0; y < GameBoardHeight; y++)
                {
                    boardSquares[x, y] = new BoardCell(BoardCell.TypeCell.Mine);

                    // TODO: добавить обработчик заполнения игрового поля, пока везде мины
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
    }
}
