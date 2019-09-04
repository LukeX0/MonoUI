using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoUI
{
    /// <summary>
    /// Class for GUI tooltip.
    /// </summary>
    public class Tooltip : GUI
    {
        /// <summary>
        /// The space in pixel between the tooltip and the active mouse cursor.
        /// </summary>
        public static Point MouseCursorOffset { get; set; } = new Point(0, 20);

        /// <summary>
        /// The color of the text.
        /// </summary>
        public Color TextColor { get; set; }
        /// <summary>
        /// The color of the background texture.
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// The time in milliseconds that the control have to be selected before the tooltip is shown.
        /// </summary>
        public int ShowTime { get; set; }
        /// <summary>
        /// The time in milliseconds the fade in effect lasts.
        /// </summary>
        public int FadeInTime { get; set; }

        private string text;
        private float scale;
        private Texture2D texture;
        private Texture2D picture;

        /// <summary>
        /// The time in milliseconds that the linked control element is selected.
        /// </summary>
        private int selectionTime;

        private readonly Game game;
        private readonly SpriteFont font;

        /// <summary>
        /// The private constructor of the tooltip class.
        /// </summary>
        /// <param name="game">The game instance in that the tooltip is to be created.</param>
        private Tooltip(Game game) : base()
        {
            this.game = game;

            selectionTime = 0;
            FadeInTime = 200;
            ShowTime = 600;
        }

        /// <summary>
        /// Creates a text tooltip that displays additional informations in combination with a control element.
        /// </summary>
        /// <param name="game">The game instance in that the tooltip is to be created.</param>
        /// <param name="text">The text that is to be displayed within the tooltip.</param>
        /// <param name="textColor">The color of the text.</param>
        /// <param name="font">The font of the text.</param>
        /// <param name="backgroundColor">The color of the text background.</param>
        public Tooltip(Game game, string text, Color textColor, SpriteFont font, Color backgroundColor) : this(game)
        {
            this.font = font;

            TextColor = textColor;
            BackgroundColor = backgroundColor;

            SetText(text);
        }

        /// <summary>
        /// Creates a picture tooltip that displays additional informations in combination with a control element.
        /// </summary>
        /// <param name="game">The game instance in that the tooltip is to be created.</param>
        /// <param name="picture">The picture that is to be displayed within the tooltip.</param>
        /// <param name="scale">The scale of the picture.</param>
        public Tooltip(Game game, Texture2D picture, float scale = 1.0f) : this(game)
        {
            this.picture = picture;
            this.scale = scale;
        }

        /// <summary>
        /// Sets the text of the tooltip. (Doesn't work in combination with a picture.)
        /// </summary>
        /// <param name="text">The text that is to be displayed within the tooltip.</param>
        public void SetText(string text)
        {
            this.text = text;

            if (font != null && text != null && text != string.Empty)
            {
                int sizeX = (int)font.MeasureString(text).X;
                int sizeY = (int)font.MeasureString(text).Y;

                texture = Tool.CreateTexture(game.GraphicsDevice, sizeX, sizeY, Color.White);
            }
        }

        /// <summary>
        /// Sets the picture of the tooltip. (Doesn't work in combination with text.)
        /// </summary>
        /// <param name="picture">The picture that is to be displayed within the tooltip.</param>
        /// <param name="scale">The scale of the picture.</param>
        public void SetPicture(Texture2D picture, float scale)
        {
            this.picture = picture;
            this.scale = scale;
        }

        /// <summary>
        /// Determines the current status of the tooltip.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch that draws the texture.</param>
        /// <param name="gameTime">The game time for the elapsed time since the last update call.</param>
        /// <param name="isSelected">The isSelected state of the connected control element.</param>
        public void Activate(SpriteBatch spriteBatch, GameTime gameTime, bool isSelected)
        {
            if (isSelected == true)
            {
                selectionTime += gameTime.ElapsedGameTime.Milliseconds;
                if (selectionTime >= ShowTime)
                {
                    int currentTime = selectionTime - ShowTime;
                    float alpha = currentTime / (float)FadeInTime;
                    Show(spriteBatch, gameTime, alpha);
                }
            }
            else
            {
                selectionTime = 0;
            }
        }

        /// <summary>
        /// Shows the tooltip for the related control element.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch that draws the texture.</param>
        /// <param name="gameTime">The game time for the elapsed time since the last update call.</param>
        /// <param name="alpha">The alpha value of the tooltip textures and colors.</param>
        private void Show(SpriteBatch spriteBatch, GameTime gameTime, float alpha)
        {
            Vector2 position = new Vector2(Input.MousePosition.X + MouseCursorOffset.X, Input.MousePosition.Y + MouseCursorOffset.Y);

            if (picture != null)
            {
                spriteBatch.Draw(picture,
                                 position,
                                 null,
                                 new Color(Color.White, alpha),
                                 0.0f,
                                 Vector2.Zero,
                                 scale,
                                 SpriteEffects.None,
                                 LayerDepth.upperPicture);
            }
            else if (texture != null && font != null && text != null && text != string.Empty)
            {
                spriteBatch.Draw(texture,
                                 position,
                                 null,
                                 new Color(BackgroundColor, alpha),
                                 0.0f,
                                 Vector2.Zero,
                                 1.0f,
                                 SpriteEffects.None,
                                 LayerDepth.upperTexture);

                spriteBatch.DrawString(font,
                                       text,
                                       position,
                                       new Color(TextColor, alpha),
                                       0.0f,
                                       Vector2.Zero,
                                       1.0f,
                                       SpriteEffects.None,
                                       LayerDepth.upperText);
            }
        }
    }
}
