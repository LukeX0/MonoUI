using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoUI
{
    /// <summary>
    /// Class for GUI checkbox.
    /// </summary>
    public class Checkbox : Widget
    {
        /// <summary>
        /// The current On/Off state of the checkbox.
        /// </summary>
        public bool State { get; private set; }

        // Ensures only one activation/deactivation per click.
        private bool wasToggled = false;

        private readonly Texture2D textureOff;
        private readonly Texture2D textureOn;

        /// <summary>
        /// Creates a new checkbox.
        /// </summary>
        /// <param name="game">The game instance in that the control element is to be created.</param>
        /// <param name="dock">The location the control element should dock onto.</param>
        /// <param name="offset">The space in pixel between the control element and the dock location.</param>
        /// <param name="textureOff">The the texture for the of/not checked state.</param>
        /// <param name="textureOn">The the texture for the on/checked state.</param>
        /// <param name="state">The start state of the checkbox. True for checked.</param>
        /// <param name="scale">The value for scaling the texture. 1.0 is no scaling.</param>
        public Checkbox(Game game, DockControl dock, Point offset, Texture2D textureOff, Texture2D textureOn, bool state, float scale = 1.0f) : base(game, dock, offset, scale)
        {
            this.textureOff = textureOff;
            this.textureOn = textureOn;

            State = state;
            Texture = state == false ? textureOff : textureOn;

            SelectionColor = Color.LightGreen;
            Position = CalculatePosition(dock, offset, Texture, scale);
        }

        /// <summary>
        /// Toggles the state and texture of the checkbox.
        /// </summary>
        private void ToggleState()
        {
            if (State == true)
            {
                State = false;
                Texture = textureOff;
            }
            else // State == false
            {
                State = true;
                Texture = textureOn;
            }
        }

        /// <summary>
        /// Draws the checkbox.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch that draws the texture.</param>
        /// <param name="gameTime">The game time for the elapsed time since the last update call.</param>
        public override void Draw(in SpriteBatch spriteBatch, in GameTime gameTime)
        {
            if (IsPressed == false)
            {
                wasToggled = false;
            }
            else if (wasToggled == false)
            {
                ToggleState();
                wasToggled = true;
            }

            base.Draw(spriteBatch, gameTime);
        }
    }
}
