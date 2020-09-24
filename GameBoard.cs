/* GameBoard.cs
 *  Handles the tic-tac-toe game logic and tiles
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
    public class GameBoard : DrawableGameComponent
    {
        // number of tiles should be a perfect square: 4, 9, 16, 25, etc.
        public const int NUMBER_OF_TILES = 9;
        public const int TILE_PADDING = 10;
        public const int STARTING_POINT = 75;
        public const float TILE_SCALE = .5f;

        Game1 parent;

        public int boardArrayLength;
        public int nextTile;
        
        public int Winner { get; private set; }
        private GameTile[,] tiles;
        private int[,] oldTileValues;
        public GameBoard(Game game) : base(game)
        {
            parent = (Game1)game;
            boardArrayLength = (int)Math.Sqrt(NUMBER_OF_TILES);
            tiles = new GameTile[boardArrayLength, boardArrayLength];
            Initialize();
        }

        /// <summary>
        /// Clears the gameboard and starts a new game
        /// </summary>
        public override void Initialize()
        {
            ClearGameBoard();
            oldTileValues = new int[tiles.GetLength(0), tiles.GetLength(1)];
            Point originalPosition = new Point(STARTING_POINT, STARTING_POINT);
            // Making a seperate tile, used for texture width only
            GameTile referenceTile = new GameTile(parent, originalPosition, TILE_SCALE, false);
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    int dist = referenceTile.textureWidth/2 + TILE_PADDING;
                    Point currentPosition = new Point(
                        x * dist, 
                        y * dist
                    );
                    GameTile tile = new GameTile(
                        parent,
                        originalPosition + currentPosition,
                        TILE_SCALE);

                    tiles[x, y] = tile;
                    parent.Components.Add(tiles[x,y]);
                }
            }
            nextTile = 1;
            Winner = 0;
            base.Initialize();
        }
        /// <summary>
        /// Updates the game logic, detecting if a tile was clicked and to check if there is a winner each time
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                if (Winner != 0) break;
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    if (tiles[x, y] != null && oldTileValues[x, y] == 0)
                        oldTileValues[x, y] = tiles[x, y].Value;

                    tiles[x, y].Update(gameTime);

                    if (oldTileValues[x, y] != tiles[x, y].Value)
                    {
                        Winner = checkWinner();
                        oldTileValues[x, y] = tiles[x, y].Value;

                        if (Winner != 0) parent.DeclareWinner(Winner);
                        else nextTile = (nextTile % 2) + 1;
                    }
                }
            }

            base.Update(gameTime);
        }
        /// <summary>
        /// Draws the tiles of the gameboard
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            foreach (var tile in tiles)
            {
                tile.Draw(gameTime);
            }
            base.Draw(gameTime);
        }
        /// <summary>
        /// Checks for the winner by rows, columns, both diagonal directions, and if the game is a draw
        /// </summary>
        /// <returns></returns>
        private int checkWinner()
        {
            int winner = 0;
            int count = 0;
            int currentPlayer = 0;

            // Check rows
            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                if (winner != 0) break;
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    if (x == 0) currentPlayer = tiles[x, y].Value;
                    if (tiles[x, y].Value == currentPlayer) count++;
                    if (count >= tiles.GetLength(0)) winner = currentPlayer;
                }
                count = 0;
            }

            // Check columns, x and y for loop statements switched
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                if (winner != 0) break;
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    if (y == 0) currentPlayer = tiles[x, y].Value;
                    if (tiles[x, y].Value == currentPlayer) count++;
                    if (count >= tiles.GetLength(0)) winner = currentPlayer;
                }
                count = 0;
            }

            // Check diagonal 1, simply works by replacing y with x
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                if (winner != 0) break;
                if (x == 0) currentPlayer = tiles[x, x].Value;
                if (tiles[x, x].Value == currentPlayer) count++;
                if (count >= tiles.GetLength(0)) winner = currentPlayer;
            }
            count = 0;

            // check diagonal 2, y goes reverse
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                if (winner != 0) break;
                int y = tiles.GetLength(1) - 1 - x;
                if (x == 0 && y == tiles.GetLength(1) - 1) currentPlayer = tiles[x, y].Value;
                if (tiles[x, y].Value == currentPlayer) count++;
                if (count >= tiles.GetLength(0)) winner = currentPlayer;
            }
            count = 0;

            // Check if draw, so if all tiles are assigned a value other than 0, 
            // but do not match any of the above
            foreach (var tile in tiles)
            {
                if (tile.Value == 0) break;
                count++;
            }
            if (count >= NUMBER_OF_TILES && winner == 0) winner = -1;

            return winner;
        }
        /// <summary>
        /// Clears all the tile components of the gameboard and renders the tile objects null for re-initialization
        /// </summary>
        public void ClearGameBoard()
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    GameTile tile = tiles[x, y];
                    if (tile != null) parent.Components.Remove(tile);
                    tile = null;
                }
            }
        }
    }
}
