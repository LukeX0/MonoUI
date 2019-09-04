using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoUI
{
    /// <summary>
    /// Interface for ensuring drawing functionality of objects.
    /// </summary>
    interface IDrawable
    {
        /// <summary>
        /// Method for drawing textures.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch for drawing multiple textures together.</param>
        /// <param name="gameTime">The game time for the elapsed time since the last update.</param>
        void Draw(in SpriteBatch spriteBatch, in GameTime gameTime);
    }
}
