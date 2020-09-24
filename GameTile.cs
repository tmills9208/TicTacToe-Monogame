/* Game1.cs
 *  The individual tile objects of the gameboard.
 *  Handles input and sends information back ot the gameboard, 
 *  along with gathering and drawing the appropriate textures
 * 
 * Revision History
 *      Tyler Mills, 2019.12.01: Created
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3
{
    public class GameTile : DrawableGameComponent
    {
        public const string X_LOCATION= "Images/X";
        public const string O_LOCATION = "Images/O";
        public const string HILIGHT_LOCATION = "Images/Highlight";
        public const string BOARD_TILE_LOCATION = "Images/BoardTile";

        Game1 parent;
        Texture2D xTex;
        Texture2D oTex;
        Texture2D highlightTex;
        Texture2D boardTileTex;
        Rectangle destinationRectangle;
        MouseState oldMouseState;
        public int textureWidth { get; private set; }
        public Point Position { get; private set; }
        public int OldValue { get; private set; }
        public int Value { get; private set; }
        public bool IsClicked { get; private set; }
        
        public GameTile(Game game, Point position, float scale) : base(game)
        {
            parent = (Game1)game;
            // Load textures
            xTex = parent.Content.Load<Texture2D>(X_LOCATION);
            oTex = parent.Content.Load<Texture2D>(O_LOCATION);
            highlightTex = parent.Content.Load<Texture2D>(HILIGHT_LOCATION);
            boardTileTex = parent.Content.Load<Texture2D>(BOARD_TILE_LOCATION);
            // Get useful info
            textureWidth = xTex.Width;
            this.Position = position;
            destinationRectangle = new Rectangle(position.X, position.Y, (int)(textureWidth * scale), (int)(textureWidth * scale));
            Value = 0;
            oldMouseState = Mouse.GetState();
        }
        /// <summary>
        /// For debugging purposes for individual tiles
        /// </summary>
        /// <param name="game"></param>
        /// <param name="position"></param>
        /// <param name="scale"></param>
        /// <param name="visible"></param>
        public GameTile(Game game, Point position, float scale, bool visible) : this(game, position, scale)
        {
            if (visible)
                SetVisible(visible);
        }
        /// <summary>
        /// Handles click detection for the tile and updates logic when clicked
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();
            if (InBounds(ms) && Value == 0 && 
                ms.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
            {
                Value = parent.gameBoard.nextTile;
            }
            oldMouseState = ms;
            base.Update(gameTime);
        }
        /// <summary>
        /// Draws the tile
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();
            
            parent.Sprite.Begin();
            if (Value == 1)
            {
                parent.Sprite.Draw(xTex, destinationRectangle, Color.White);
            }
            else if (Value == 2)
            {
                parent.Sprite.Draw(oTex, destinationRectangle, Color.White);
            }
            else
            {
                parent.Sprite.Draw(boardTileTex, destinationRectangle, Color.White);
            }
            // When mouse is hovering over an empty tile, use the highlighting texture
            if (InBounds(ms) && Value == 0)
            {
                parent.Sprite.Draw(highlightTex, destinationRectangle, Color.White);
            }
            parent.Sprite.End();

            base.Draw(gameTime);
        }
        /// <summary>
        /// checks if the mouse position is in the tile
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public bool InBounds(MouseState ms)
        {
            bool result = false;
            if (destinationRectangle.Contains(new Point(ms.X, ms.Y)))
                result = true;
            return result;
        }

        public void SetVisible(bool visible)
        {
            this.Visible = visible;
        }
    }
}
