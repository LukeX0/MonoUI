using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoUI
{
    /// <summary>
    /// Class for GUI image.
    /// </summary>
    public class Image : Widget
    {
        /// <summary>
        /// Creates a new image within a box.
        /// </summary>
        /// <param name="game">The game instance in that the control element is to be created.</param>
        /// <param name="dock">The location the control element should dock onto.</param>
        /// <param name="offset">The space in pixel between the control element and the dock location.</param>
        /// <param name="image">The texture for the image.</param>
        /// <param name="scale">The value for scaling the texture. 1.0 is no scaling.</param>
        public Image(Game game, DockControl dock, Point offset, Texture2D image, float scale = 1.0f) : base(game, dock, offset, scale)
        {
            Texture = image;
            Layer = LayerDepth.middlePicture;
            Position = CalculatePosition(dock, offset, Texture, scale);
        }

        /// <summary>
        /// Sets a new image.
        /// </summary>
        /// <param name="newImage">The texture for the new image.</param>
        public void SetImage(Texture2D newImage)
        {
            Texture = newImage;
            Position = CalculatePosition(Dock, Offset, Texture, Scale);
        }

        /// <summary>
        /// Draws the image.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch that draws the texture.</param>
        /// <param name="gameTime">The game time for the elapsed time since the last update call.</param>
        public override void Draw(in SpriteBatch spriteBatch, in GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
        }
    }
}
