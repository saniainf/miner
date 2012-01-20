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

        private BoardCell[,] boardCell;

        public GameBoard(int width, int height)
        {
            GameBoardWidth = width;
            GameBoardHeight = height;

            boardCell = new BoardCell[width, height];

            ClearBoard();
        }

        private void ClearBoard()
        {
            for (int x = 0; x < GameBoardWidth; x++)
            {
                for (int y = 0; y < GameBoardHeight; y++)
                {
                    boardCell[x, y] = new BoardCell(BoardCell.TypeCell.Close);
                }
            }
        }

        /*-------*/
        public Rectangle GetTileRect(int x, int y)
        {
            return boardCell[x, y].GetTileRect();
        }

        public void SetTypeCell(int x, int y, BoardCell.TypeCell type)
        {
            boardCell[x, y].type = type;
        }
    }
}
