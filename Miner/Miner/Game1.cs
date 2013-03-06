using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Miner
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D TileSheet;

        GameBoard gameBoard;
        Controls controls;

        Rectangle BackgroundCell = new Rectangle(0, 330, 22, 22);
        Rectangle BackgroundRectangle = new Rectangle(30, 0, 400, 400);
        Rectangle gameBoardRectangle;
        Rectangle ScreenRectangle;

        Point Offset;
        Point ControlsBottom;
        Point SpaceOverGameBoard = new Point(10, 80);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            gameBoard = new GameBoard(30, 20, 50);
            controls = new Controls();

            gameBoardRectangle.Width = (gameBoard.GameBoardWidth * BoardCell.CellWidth) + (gameBoard.GameBoardWidth + BoardCell.Offset);
            gameBoardRectangle.Height = (gameBoard.GameBoardHeight * BoardCell.CellHeight) + (gameBoard.GameBoardHeight + BoardCell.Offset);

            graphics.PreferredBackBufferWidth = gameBoardRectangle.Width + SpaceOverGameBoard.X;
            graphics.PreferredBackBufferHeight = gameBoardRectangle.Height + SpaceOverGameBoard.Y;
            graphics.ApplyChanges();

            ScreenRectangle = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            Offset.X = (ScreenRectangle.Width - gameBoardRectangle.Width) / 2;
            Offset.Y = (ScreenRectangle.Height - gameBoardRectangle.Height) / 2;

            ControlsBottom.X = graphics.PreferredBackBufferWidth / 2;
            ControlsBottom.Y = (graphics.PreferredBackBufferHeight - (SpaceOverGameBoard.Y / 4)) - (ControlsPiece.ControlsHeight / 2);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            TileSheet = Content.Load<Texture2D>(@"Textures\Tile_sheet");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            GameBoardUpdate(Mouse.GetState());

            base.Update(gameTime);
        }

        /// <summary>
        /// отрисовка игры
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);

            spriteBatch.Begin();

            spriteBatch.Draw(TileSheet, ScreenRectangle, BackgroundRectangle, Color.White);

            for (int x = 0; x < gameBoard.GameBoardWidth; x++)
            {
                for (int y = 0; y < gameBoard.GameBoardHeight; y++)
                {
                    //////////////////////////////////////////////////////////////////////////
                    // отрисовка бэкграунда €чеек
                    //////////////////////////////////////////////////////////////////////////
                    int pixelX = ((x * BackgroundCell.Width) - x) + Offset.X;
                    int pixelY = ((y * BackgroundCell.Height) - y) + Offset.Y;

                    spriteBatch.Draw(TileSheet,
                        new Rectangle(pixelX, pixelY, BackgroundCell.Width, BackgroundCell.Height),
                        BackgroundCell,
                        Color.White);
                    //////////////////////////////////////////////////////////////////////////
                    // отрисовка €чеек 
                    //////////////////////////////////////////////////////////////////////////
                    pixelX = ((x * BoardCell.CellWidth) + x + 1) + Offset.X;
                    pixelY = ((y * BoardCell.CellHeight) + y + 1) + Offset.Y;

                    spriteBatch.Draw(TileSheet,
                        new Rectangle(pixelX, pixelY, BoardCell.CellWidth, BoardCell.CellHeight),
                        gameBoard.GetTileRect(x, y), Color.White);
                    //////////////////////////////////////////////////////////////////////////
                }
            }

            for (int i = 0; i < controls.CountControls(); i++)
            {
                int pixelX = 0;
                int pixelY = 0;

                switch (controls.GetName(i))
                {
                    case "New":
                        pixelX = ControlsBottom.X - (5 + ControlsPiece.ControlsWidth);
                        pixelY = ControlsBottom.Y;
                        break;
                    case "Options":
                        pixelX = ControlsBottom.X + 5;
                        pixelY = ControlsBottom.Y;
                        break;
                }

                spriteBatch.Draw(TileSheet,
                    new Rectangle(pixelX, pixelY, ControlsPiece.ControlsWidth, ControlsPiece.ControlsHeight),
                    controls.GetControlRect(i), Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// update инпута
        /// </summary>
        /// <param name="mouseState"></param>
        private void GameBoardUpdate(MouseState mouseState)
        {
            for (int x = 0; x < gameBoard.GameBoardWidth; x++)
            {
                for (int y = 0; y < gameBoard.GameBoardHeight; y++)
                {
                    gameBoard.WinGameOver(x, y);

                    gameBoard.Clear2Suffix(x, y);

                    // rectangle провер€емой €чейки
                    int pixelX = ((x * BoardCell.CellWidth) + x + 1) + Offset.X;
                    int pixelY = ((y * BoardCell.CellHeight) + y + 1) + Offset.Y;
                    Rectangle rect = new Rectangle(pixelX, pixelY, BoardCell.CellWidth, BoardCell.CellHeight);

                    // если hover mouse вызываем проверку суффиксов €чейки
                    if (rect.Contains(mouseState.X, mouseState.Y))
                    {
                        gameBoard.CheckSuffixCell(x, y, mouseState);
                    }
                }
            }

            for (int i = 0; i < controls.CountControls(); i++)
            {
                int pixelX = 0;
                int pixelY = 0;

                controls.Clear2Suffix(i);

                switch (controls.GetName(i))
                {
                    case "New":
                        pixelX = ControlsBottom.X - (5 + ControlsPiece.ControlsWidth);
                        pixelY = ControlsBottom.Y;
                        break;
                    case "Options":
                        pixelX = ControlsBottom.X + 5;
                        pixelY = ControlsBottom.Y;
                        break;
                }

                Rectangle rect = new Rectangle(pixelX, pixelY, ControlsPiece.ControlsWidth, ControlsPiece.ControlsHeight);

                if (rect.Contains(mouseState.X, mouseState.Y))
                {
                    controls.CheckSuffixCell(i, mouseState);
                }
            }
        }
    }
}
