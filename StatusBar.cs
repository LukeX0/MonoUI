using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoUI
{
    /// <summary>
    /// Base class for status bars. Only for inheritance.
    /// </summary>
    public abstract class StatusBar : Widget
    {
        /// <summary>
        /// True is horizontal alignment, false is vertical alignment.
        /// </summary>
        public virtual bool Alignment { get; set; }
        /// <summary>
        /// The status of the status bar, 0 means 0% filled and 1 means 100% filled.
        /// </summary>
        public virtual float Status { get; set; }

        /// <summary>
        /// The texture that represents the status.
        /// </summary>
        protected Texture2D StatusTexture { get; set; }

        /// <summary>
        /// Base constructor for status bars. Only for inheritance.
        /// </summary>
        /// <param name="game">The game instance in that the control element is to be created.</param>
        /// <param name="dock">The location the control element should dock onto.</param>
        /// <param name="offset">The space in pixel between the control element and the dock location.</param>
        /// <param name="scale">The value for scaling the texture. 1.0 is no scaling.</param>
        protected StatusBar(Game game, DockControl dock, Point offset, float scale = 1.0f) : base(game, dock, offset, scale) { }

        /// <summary>
        /// Returns the rectangle for the status bar.
        /// </summary>
        /// <returns></returns>
        protected Rectangle StatusBarSize()
        {
            if (StatusTexture == null)
            {
                return Rectangle.Empty;
            }
            else if (Alignment == true) // Horizontal status bar
            {
                return new Rectangle(0, 0, (int)(StatusTexture.Width * Status), StatusTexture.Height);
            }
            else // Vertical status bar
            {
                return new Rectangle(0, 0, StatusTexture.Width, (int)(StatusTexture.Height * Status));
            }
        }
    }
}
