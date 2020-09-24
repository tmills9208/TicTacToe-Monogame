/* Game1.cs
 *  Handles the gameboard, text and input
 * 
 * Revision History
 *      Tyler Mills, 2019.12.01: Created
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Assignment3
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private const int SCREEN_WIDTH = 550;

        private string textFont = "Font/runescape_chat_bold_2";
        private Color textColor = Color.White;

        GraphicsDeviceManager graphics;
        public GraphicsDeviceManager Graphics { get => graphics; }
        SpriteBatch spriteBatch;
        public SpriteBatch Sprite { get => spriteBatch; }

        SpriteFont text;
        string winnerText;

        public GameBoard gameBoard { get; private set; }

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
            // TODO: Add your initialization logic here
            Graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            Graphics.PreferredBackBufferHeight = SCREEN_WIDTH;
            this.IsMouseVisible = true;
            this.Window.Title = "TIC-TAC-TOE";
            Graphics.ApplyChanges();

            winnerText = "";
            gameBoard = new GameBoard(this);

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

            // TODO: use this.Content to load your game content here
            text = this.Content.Load<SpriteFont>(textFont);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (string.IsNullOrEmpty(winnerText))
                gameBoard.Update(gameTime);

            if (gameBoard.Winner != 0)
                

            if (gameBoard.Winner != 0 && Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                gameBoard.Initialize();
                winnerText = "";
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            if (gameBoard.Winner != 0)
            {
                Sprite.Begin();
                Sprite.DrawString(text, winnerText, new Vector2(5, 0), Color.White);
                Sprite.DrawString(text, "Press Space to Restart", new Vector2(5, 25), Color.White);
                Sprite.End();
            }
            gameBoard.Draw(gameTime);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Sets the text to display the winner or if the game was a draw
        /// </summary>
        /// <param name="winner"></param>
        public void DeclareWinner(int winner)
        {
            if (winner == 1)
            {
                winnerText = "X Wins";
            }
            else if (winner == 2)
            {
                winnerText = "O Wins";
            }
            else if (winner < 0)
            {
                winnerText = "Draw";
            }
        }
    }
}
