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
        Button smlButton;

        int _width = 20;
        int _height = 20;
        int _count = 50;

        Point smlButtonSize = new Point(42,42);
        Point cellGameboardSize = new Point(21,21);

        /// <summary>
        /// bounding box всего экрана
        /// </summary>
        Rectangle ScreenBBox;

        /// <summary>
        /// отступы вокруг игрового пол€ (over gameBoard, top)
        /// </summary>
        Point spaceOverGameBoard;

        /// <summary>
        /// текстура бэкграунда
        /// </summary>
        Rectangle Background = new Rectangle(499, 1, 100, 100);

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
            
            spaceOverGameBoard = new Point(10, 60); // TODO заменить на размеры текстур

            NewGame(_width, _height, _count);

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

            smlButton.btnUpdate(mouseState);
            gameBoard.gameBoardUpdate(mouseState);

            base.Update(gameTime);
        }

        /// <summary>
        /// main draw
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin();
            
            spriteBatch.Draw(TileSheet, ScreenBBox, Background, Color.White);
            
            smlButton.btnDraw(spriteBatch, TileSheet);
 
            gameBoard.gameBoardDraw(spriteBatch, TileSheet);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// нова€ игра
        /// </summary>
        /// <param name="width">количество €чеек по ширине</param>
        /// <param name="height">количество €чеек по высоте</param>
        /// <param name="count">количество мин</param>
        private void NewGame(int width, int height, int count)
        {
            gameBoard = new GameBoard(width, height, count, spaceOverGameBoard, cellGameboardSize);

            ScreenBBox = new Rectangle(0, 0,
                gameBoard.gameBoardBBox.Width + spaceOverGameBoard.X * 2,
                gameBoard.gameBoardBBox.Height + spaceOverGameBoard.Y + spaceOverGameBoard.X * 2);

            graphics.PreferredBackBufferWidth = ScreenBBox.Width;
            graphics.PreferredBackBufferHeight = ScreenBBox.Height;
            graphics.ApplyChanges();

            smlButton = new Button(new Rectangle(
                (ScreenBBox.Width - smlButtonSize.X) / 2,
                (spaceOverGameBoard.Y - smlButtonSize.Y) / 2,
                smlButtonSize.X, smlButtonSize.Y),
                new Point(456, 0));

            smlButton.Action += () =>
            {
                NewGame(_width, _height, _count);
            };
        }
    }
}
