using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoUI
{
    /// <summary>
    /// Class for GUI label.
    /// </summary>
    public class Label : Widget
    {
        /// <summary>
        /// The text of the label.
        /// </summary>
        public String Text { get; set; }
        /// <summary>
        /// The color of the text.
        /// </summary>
        public Color TextColor { get; set; }

        private readonly SpriteFont font;

        /// <summary>
        /// The private constructor of the Label class.
        /// </summary>
        /// <param name="game">The game instance in that the control element is to be created.</param>
        /// <param name="dock">The location the control element should dock onto.</param>
        /// <param name="offset">The space in pixel between the control element and the dock location.</param>
        /// <param name="text">The text that is to be displayed within the label.</param>
        /// <param name="textColor">The color of the text.</param>
        /// <param name="font">The font of the text.</param>
        /// <param name="scale">The value for scaling the texture. 1.0 is no scaling.</param>
        private Label(Game game, DockControl dock, Point offset, String text, Color textColor, SpriteFont font, float scale = 1.0f) : base(game, dock, offset, scale)
        {
            Text = text;
            TextColor = textColor;
            this.font = font;
        }

        /// <summary>
        /// Creates a new simple label.
        /// </summary>
        /// <param name="game">The game instance in that the control element is to be created.</param>
        /// <param name="dock">The location the control element should dock onto.</param>
        /// <param name="offset">The space in pixel between the control element and the dock location.</param>
        /// <param name="text">The text that is to be displayed within the label.</param>
        /// <param name="textColor">The color of the text.</param>
        /// <param name="font">The font of the text.</param>
        public Label(Game game, DockControl dock, Point offset, String text, Color textColor, SpriteFont font) : this(game, dock, offset, text, textColor, font, 1.0f)
        {
            Position = CalculatePosition(dock, offset, Texture, 1.0f);
        }

        /// <summary>
        /// Creates a new label with a background texture.
        /// </summary>
        /// <param name="game">The game instance in that the control element is to be created.</param>
        /// <param name="dock">The location the control element should dock onto.</param>
        /// <param name="offset">The space in pixel between the control element and the dock location.</param>
        /// <param name="text">The text that is to be displayed within the label.</param>
        /// <param name="textColor">The color of the text.</param>
        /// <param name="font">The font of the text.</param>
        /// <param name="texture">The background texture for the label.</param>
        /// <param name="scale">The value for scaling the texture. 1.0 is no scaling.</param>
        public Label(Game game, DockControl dock, Point offset, String text, Color textColor, SpriteFont font, Texture2D texture, float scale = 1.0f) : this(game, dock, offset, text, textColor, font, scale)
        {
            Texture = texture;

            Position = CalculatePosition(dock, offset, Texture, scale);
        }

        /// <summary>
        /// Draws the the Label.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch that draws the texture.</param>
        /// <param name="gameTime">The game time for the elapsed time since the last update call.</param>
        public override void Draw(in SpriteBatch spriteBatch, in GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            if (Texture != null && Text != null && font != null && IsVisible == true)
            {
                spriteBatch.DrawString(font,
                                       Text,
                                       new Vector2(Position.X + ((Texture.Width * Scale) / 2.0f), Position.Y + ((Texture.Height * Scale) / 2.0f)),
                                       TextColor,
                                       0.0f,
                                       font.MeasureString(Text) / 2.0f,
                                       1.0f,
                                       SpriteEffects.None,
                                       LayerDepth.middleText);
            }
            else if (Texture == null && Text != null && font != null && IsVisible == true)
            {
                spriteBatch.DrawString(font,
                                       Text,
                                       Position,
                                       TextColor,
                                       0.0f,
                                       font.MeasureString(Text) / 2.0f,
                                       1.0f,
                                       SpriteEffects.None,
                                       LayerDepth.middleText);
            }
        }
    }
}
