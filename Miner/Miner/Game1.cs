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

        Rectangle BackgroundCell = new Rectangle(0, 330, 22, 22);

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
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            gameBoard = new GameBoard(20, 16);

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
            HandleMouseInput(Mouse.GetState());

            base.Update(gameTime);
        }

        /// <summary>
        /// отрисовка игры
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            for (int x = 0; x < gameBoard.GameBoardWidth; x++)
            {
                for (int y = 0; y < gameBoard.GameBoardHeight; y++)
                {
                    //////////////////////////////////////////////////////////////////////////
                    // отрисовка бэкграунда
                    //////////////////////////////////////////////////////////////////////////
                    int pixelX = (x * BackgroundCell.Width) - x;
                    int pixelY = (y * BackgroundCell.Height) - y;

                    spriteBatch.Draw(TileSheet,
                        new Rectangle(pixelX, pixelY, BackgroundCell.Width, BackgroundCell.Height),
                        BackgroundCell,
                        Color.White);
                    //////////////////////////////////////////////////////////////////////////
                    // отрисовка €чеек 
                    //////////////////////////////////////////////////////////////////////////
                    pixelX = (x * BoardCell.CellWidth) + x + 1;
                    pixelY = (y * BoardCell.CellHeight) + y + 1;

                    spriteBatch.Draw(TileSheet,
                        new Rectangle(pixelX, pixelY, BoardCell.CellWidth, BoardCell.CellHeight),
                        gameBoard.GetTileRect(x, y), Color.White);
                    //////////////////////////////////////////////////////////////////////////
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// update инпута
        /// </summary>
        /// <param name="mouseState"></param>
        private void HandleMouseInput(MouseState mouseState)
        {
            for (int x = 0; x < gameBoard.GameBoardWidth; x++)
            {
                for (int y = 0; y < gameBoard.GameBoardHeight; y++)
                {
                    //////////////////////////////////////////////////////////////////////////
                    // rectangle провер€емой €чейки
                    //////////////////////////////////////////////////////////////////////////
                    int pixelX = (x * BoardCell.CellWidth) + x + 1;
                    int pixelY = (y * BoardCell.CellHeight) + y + 1;
                    Rectangle rect = new Rectangle(pixelX, pixelY, BoardCell.CellWidth, BoardCell.CellHeight);
                    //////////////////////////////////////////////////////////////////////////

                    if (gameBoard.MouseSelect(x, y))
                    {
                        gameBoard.MouseSelectOFF(x, y); // обнул€ем селект
                    }

                    if (rect.Contains(mouseState.X, mouseState.Y))
                    {
                        gameBoard.MouseSelectON(x, y); // если выделена то ставим selectOn
                    }

                    //////////////////////////////////////////////////////////////////////////

                    if (!(gameBoard.MouseSelect(x,y)) && gameBoard.MouseLBPress(x, y))
                    {
                        gameBoard.MouseLBPressOFF(x, y); // обнул€ем pressON если невыделена
                    }

                    if (gameBoard.MouseSelect(x, y) && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        gameBoard.MouseLBPressON(x, y); // если нажата кнопка то ставим pressON
                    }

                    if (gameBoard.MouseLBPress(x, y) && mouseState.LeftButton == ButtonState.Released)
                    {
                        gameBoard.CellCloseOFF(x, y);
                    }

                    //////////////////////////////////////////////////////////////////////////
                }
            }
        }
    }
}
