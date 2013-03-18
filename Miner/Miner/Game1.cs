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
    /// main class
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D TileSheet;

        GameBoard gameBoard;

        SmileButton smileButton;

        Button smlButton;

        /// <summary>
        /// rectangle всего экрана
        /// </summary>
        Rectangle ScreenRectangle;

        /// <summary>
        /// отступы вокруг игрового пол€ (over gameBoard, top)
        /// </summary>
        Point SpaceOverGameBoard;

        /// <summary>
        /// rectangle игрового пол€
        /// </summary>
        Rectangle gameBoardRectangle;

        Rectangle BackgroundRectangle = new Rectangle(31, 1, 100, 100);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// initialize item
        /// </summary>
        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            gameBoard = new GameBoard(10, 10, 15);
            smileButton = new SmileButton();
            SpaceOverGameBoard = new Point(10, 60); // TODO заменить на размеры текстур

            gameBoardRectangle = new Rectangle(
                SpaceOverGameBoard.X,
                SpaceOverGameBoard.X + SpaceOverGameBoard.Y,
                gameBoard.GameBoardWidth * BoardCell.CellWidth,
                gameBoard.GameBoardHeight * BoardCell.CellHeight);

            graphics.PreferredBackBufferWidth = gameBoardRectangle.Width + SpaceOverGameBoard.X * 2;
            graphics.PreferredBackBufferHeight = gameBoardRectangle.Height + SpaceOverGameBoard.X * 2 + SpaceOverGameBoard.Y;
            graphics.ApplyChanges();

            ScreenRectangle = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            smlButton = new Button(new Rectangle((ScreenRectangle.Width - 42) / 2, (SpaceOverGameBoard.Y - 42) / 2, 42, 42), new Point(388, 0));

            smlButton.Action += () =>
            {
                gameBoard.ClearBoard();
            };

            base.Initialize();
        }

        /// <summary>
        /// LoadContent
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            TileSheet = Content.Load<Texture2D>(@"Textures\Tile_sheet");
        }

        /// <summary>
        /// UnloadContent 
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// main update
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            SmileUpdate(mouseState);

            smlButton.btnUpdate(mouseState);

            GameBoardUpdate(mouseState);

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
            
            // игровое поле
            for (int x = 0; x < gameBoard.GameBoardWidth; x++)
            {
                for (int y = 0; y < gameBoard.GameBoardHeight; y++)
                {
                    int pixelX = gameBoardRectangle.X + BoardCell.CellWidth * x;
                    int pixelY = gameBoardRectangle.Y + BoardCell.CellHeight * y;

                    spriteBatch.Draw(TileSheet,
                        new Rectangle(pixelX, pixelY, BoardCell.CellWidth, BoardCell.CellHeight),
                        gameBoard.GetTileRect(x, y), Color.White);
                }
            }

            // smile button
            spriteBatch.Draw(TileSheet,
                smlButton.boundingBox,
                smlButton.GetBBoxSheet(),
                Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// update gameboard инпута
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
                    Rectangle rect = new Rectangle(
                        gameBoardRectangle.X + BoardCell.CellWidth * x,
                        gameBoardRectangle.Y + BoardCell.CellHeight * y,
                        BoardCell.CellWidth, BoardCell.CellHeight);

                    // если hover mouse вызываем проверку суффиксов €чейки и пугаем смайл
                    if (rect.Contains(mouseState.X, mouseState.Y))
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed)
                            smileButton.smileFear = true;

                        gameBoard.CheckSuffixCell(x, y, mouseState);
                    }
                }
            }
        }

        /// <summary>
        /// update smile input
        /// </summary>
        /// <param name="mouseState"></param>
        private void SmileUpdate(MouseState mouseState)
        {
            smileButton.Clear3Suffix();

            Rectangle rect = new Rectangle(
                    (ScreenRectangle.Width - SmileButton.SmileWidth) / 2,
                    (SpaceOverGameBoard.Y - SmileButton.SmileHeight) / 2,
                    SmileButton.SmileWidth, SmileButton.SmileHeight);

            if (rect.Contains(mouseState.X, mouseState.Y))
            {
                smileButton.SuffixSelect = true;

                if (mouseState.LeftButton == ButtonState.Pressed)
                    smileButton.SuffixPress = true;

                if (smileButton.SuffixPress && mouseState.LeftButton == ButtonState.Released)
                {
                    smileButton.SuffixPress = false;
                    gameBoard.ClearBoard();
                }
            }
        }
    }
}
