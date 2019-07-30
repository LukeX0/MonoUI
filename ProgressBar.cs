using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoUI
{
    /// <summary>
    /// Class for GUI progress bar.
    /// </summary>
    public class ProgressBar : StatusBar
    {
        /// <summary>
        /// The color of the progress bar. This does not affect the background color.
        /// </summary>
        public Color ProgressColor { get; set; }

        /// <summary>
        /// The status of the progress bar, 0 means 0% filled and 1 means 100% filled.
        /// </summary>
        public override float Status
        {
            get { return status; }
            set { if (value < 0.0f) { status = 0.0f; } else if (value > 1.0f) { status = 1.0f; } else { status = value; } }
        }

        private float status;

        /// <summary>
        /// The private constructor of the progress bar class.
        /// </summary>
        /// <param name="game">The game instance in that the control element is to be created.</param>
        /// <param name="dock">The location the control element should dock onto.</param>
        /// <param name="offset">The space in pixel between the control element and the dock location.</param>
        /// <param name="status">The status of the progress bar, 0 means 0% filled and 1 means 100% filled.</param>
        /// <param name="scale">The value for scaling the progress bar. 1.0 is no scaling.</param>
        private ProgressBar(Game game, DockControl dock, Point offset, float status, float scale = 1.0f) : base(game, dock, offset, scale)
        {
            Alignment = true;
            Status = status;
        }

        /// <summary>
        /// Creates a new simple progress bar.
        /// </summary>
        /// <param name="game">The game instance in that the progress bar is to be created.</param>
        /// <param name="dock">The location the progress bar should dock onto.</param>
        /// <param name="offset">The space in pixel between the progress bar and the dock location.</param>
        /// <param name="status">The status of the progress bar, 0 means 0% filled and 1 means 100% filled.</param>
        /// <param name="width">The width of the progress bar in pixel. This have to be greater than zero.</param>
        /// <param name="height">The height of the progress bar in pixel. This have to be greater than zero.</param>
        /// <param name="progressColor">The color of the progress bar. (Foreground)</param>
        /// <param name="backgroundColor">The color behind the progress bar. (Background)</param>
        public ProgressBar(Game game, DockControl dock, Point offset, float status, int width, int height, Color progressColor, Color backgroundColor) : this(game, dock, offset, status, 1.0f)
        {
            ProgressColor = progressColor;
            Color = backgroundColor;

            Texture = Tool.CreateTexture(game.GraphicsDevice, width, height, Color.White);
            StatusTexture = Tool.CreateTexture(game.GraphicsDevice, width, height, Color.White);

            Position = CalculatePosition(dock, offset, Texture, 1.0f);
        }

        /// <summary>
        /// Creates a new progress bar with textures.
        /// </summary>
        /// <param name="game">The game instance in that the progress bar is to be created.</param>
        /// <param name="dock">The location the progress bar should dock onto.</param>
        /// <param name="offset">The space in pixel between the progress bar and the dock location.</param>
        /// <param name="status">The status of the progress bar, 0 means 0% filled and 1 means 100% filled.</param>
        /// <param name="textureBackground">The texture for the background bar.</param>
        /// <param name="textureProgress">The texture for the foreground bar.</param>
        /// <param name="scale">The value for scaling the textures. 1.0 is no scaling.</param>
        public ProgressBar(Game game, DockControl dock, Point offset, float status, Texture2D textureBackground, Texture2D textureProgress, float scale = 1.0f) : this(game, dock, offset, status, scale)
        {
            ProgressColor = Color.White;

            Texture = textureBackground;
            StatusTexture = textureProgress;

            Position = CalculatePosition(dock, offset, Texture, 1.0f);
        }

        /// <summary>
        /// Draws the progress bar.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch that draws the texture.</param>
        /// <param name="gameTime">The game time for the elapsed time since the last update call.</param>
        public override void Draw(in SpriteBatch spriteBatch, in GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            if (StatusTexture != null && IsVisible == true)
            {
                spriteBatch.Draw(StatusTexture,
                                 Position,
                                 StatusBarSize(),
                                 ProgressColor,
                                 0.0f,
                                 Vector2.Zero,
                                 Scale,
                                 SpriteEffects.None,
                                 LayerDepth.middlePicture);
            }
        }
    }
}
