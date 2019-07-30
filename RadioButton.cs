using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace MonoUI
{
    /// <summary>
    /// Class for GUI radio button.
    /// </summary>
    public class RadioButton : Widget
    {
        /// <summary>
        /// The current On/Off state of the radio button.
        /// </summary>
        public bool State { get; private set; }

        private readonly Texture2D textureOff;
        private readonly Texture2D textureOn;

        private readonly List<RadioButton> group;

        /// <summary>
        /// Creates a new radio button.
        /// </summary>
        /// <param name="game">The game instance in that the control element is to be created.</param>
        /// <param name="dock">The location the control element should dock onto.</param>
        /// <param name="offset">The space in pixel between the control element and the dock location.</param>
        /// <param name="textureOff">The texture for the of/not checked state.</param>
        /// <param name="textureOn">The texture for the on/checked state.</param>
        /// <param name="group">A list of this type to group radio buttons together.</param>
        /// <param name="scale">The value for scaling the texture. 1.0 is no scaling.</param>
        public RadioButton(Game game, DockControl dock, Point offset, Texture2D textureOff, Texture2D textureOn, List<RadioButton> group, float scale = 1.0f) : base(game, dock, offset, scale)
        {
            this.textureOff = textureOff;
            this.textureOn = textureOn;

            State = false;
            Texture = this.textureOff;

            this.group = group;
            this.group.Add(this);

            SelectionColor = Color.LightGreen;
            Position = CalculatePosition(dock, offset, Texture, scale);
        }

        /// <summary>
        /// Changes the active radio button within a group of radio buttons.
        /// </summary>
        private void ChangeActiveRadioButton()
        {
            foreach (RadioButton item in group.Where(item => item != null))
            {
                item.State = false;
                item.Texture = item.textureOff;
            }

            State = true;
            Texture = textureOn;
        }

        /// <summary>
        /// Draws the radio button.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch that draws the texture.</param>
        /// <param name="gameTime">The game time for the elapsed time since the last update call.</param>
        public override void Draw(in SpriteBatch spriteBatch, in GameTime gameTime)
        {
            if (IsPressed == true && State == false)
            {
                ChangeActiveRadioButton();
            }

            base.Draw(spriteBatch, gameTime);
        }
    }
}
