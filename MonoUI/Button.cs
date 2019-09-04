using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoUI
{
    /// <summary>
    /// Class for GUI button.
    /// </summary>
    public class Button : Widget
    {
        /// <summary>
        /// The text on the button.
        /// </summary>
        public String Text { get; set; }
        /// <summary>
        /// The color of the text.
        /// </summary>
        public Color TextColor { get; set; }

        private Vector2 positionNormal;
        private Vector2 positionPressed;

        private readonly Texture2D textureNormal;
        private readonly Texture2D texturePressed;

        private readonly Texture2D picture;
        private readonly Vector2 pictureScale;

        private readonly SpriteFont font;

        /// <summary>
        /// The height difference in pixel between the normal state and the pressed state. For text and pictures.
        /// </summary>
        private readonly int textureHeightDifference;
        private readonly float insideScale;

        /// <summary>The private constructor of the button class.
        /// </summary>
        /// <param name="game">The game instance in that the control element is to be created.</param>
        /// <param name="dock">The location the control element should dock onto.</param>
        /// <param name="offset">The space in pixel between the control element and the dock location.</param>
        /// <param name="scale">The value for scaling the texture. 1.0 is no scaling.</param>
        private Button(Game game, DockControl dock, Point offset, float scale = 1.0f) : base(game, dock, offset, scale)
        {
            SelectionColor = Color.LightGreen;
        }

        /// <summary>
        /// Creates a new simple button.
        /// </summary>
        /// <param name="game">The game instance in that the control element is to be created.</param>
        /// <param name="dock">The location the control element should dock onto.</param>
        /// <param name="offset">The space in pixel between the control element and the dock location.</param>
        /// <param name="textureNormal">The texture for the button in normal state.</param>
        /// <param name="texturePressed">The texture for the button in pressed state.</param>
        /// <param name="scale">The value for scaling the textures. 1.0 is no scaling.</param>
        public Button(Game game, DockControl dock, Point offset, Texture2D textureNormal, Texture2D texturePressed, float scale = 1.0f) : this(game, dock, offset, scale)
        {
            this.textureNormal = textureNormal;
            this.texturePressed = texturePressed;

            positionNormal = CalculatePosition(dock, offset, textureNormal, scale);
            textureHeightDifference = (textureNormal.Height - texturePressed.Height);
            positionPressed = new Vector2(positionNormal.X, positionNormal.Y + textureHeightDifference * scale);

            Texture = textureNormal;
            Position = positionNormal;
        }

        /// <summary>
        /// Creates a new button with a picture.
        /// </summary>
        /// <param name="game">The game instance in that the control element is to be created.</param>
        /// <param name="dock">The location the control element should dock onto.</param>
        /// <param name="offset">The offset in pixel between the GUI element and the dock location.</param>
        /// <param name="textureNormal">The texture for the button in normal state.</param>
        /// <param name="texturePressed">The texture for the button in pressed state.</param>
        /// <param name="picture">The name of the picture that is displayed within the button.</param>
        /// <param name="scale">The value for scaling the textures. 1.0 is no scaling.</param>
        /// <param name="insideScale">A value between 0 and 1 for scaling the picture inside the Button. 1 is the original size.</param>
        public Button(Game game, DockControl dock, Point offset, Texture2D textureNormal, Texture2D texturePressed, Texture2D picture, float insideScale, float scale = 1.0f) : this(game, dock, offset, textureNormal, texturePressed, scale)
        {
            this.picture = picture;

            if (insideScale < 0.0f)
            {
                this.insideScale = 0.0f;
            }
            else if (insideScale > 1.0f)
            {
                this.insideScale = 1.0f;
            }
            else
            {
                this.insideScale = insideScale;
            }

            if (picture != null)
            {
                float scaleX;
                float scaleY;

                (scaleX, scaleY) = CalculateInsideSize(picture);

                pictureScale = new Vector2(scaleX, scaleY);
            }
        }

        /// <summary>
        /// Creates a new button with text.
        /// </summary>
        /// <param name="game">The game instance in that the control element is to be created.</param>
        /// <param name="dock">The location the control element should dock onto.</param>
        /// <param name="offset">The offset in pixel between the control element and the dock location.</param>
        /// <param name="textureNormal">The texture for the button in normal state.</param>
        /// <param name="texturePressed">The texture for the button in pressed state.</param>
        /// <param name="text">The text that is to be displayed within the button.</param>
        /// <param name="textColor">The color of the text.</param>
        /// <param name="font">The font of the text.</param>
        /// <param name="scale">The value for scaling the textures. 1.0 is no scaling.</param>
        public Button(Game game, DockControl dock, Point offset, Texture2D textureNormal, Texture2D texturePressed, String text, Color textColor, SpriteFont font, float scale = 1.0f) : this(game, dock, offset, textureNormal, texturePressed, scale)
        {
            Text = text;
            TextColor = textColor;
            this.font = font;
        }

        /// <summary>
        /// Sets the position of the control element.
        /// </summary>
        /// <param name="dock">The location the control element should dock onto.</param>
        /// <param name="offset">The space in pixel between the control element and the dock location.</param>
        public override void SetPosition(DockControl dock, Point offset)
        {
            base.SetPosition(dock, offset);

            positionNormal = Position;
            positionPressed = new Vector2(positionNormal.X, positionNormal.Y + (textureNormal.Height - texturePressed.Height) * Scale);
        }

        /// <summary>
        /// Calculates the size of the content inside the button.
        /// </summary>
        /// <returns></returns>
        private (float, float) CalculateInsideSize(Texture2D insideTexture)
        {
            int width;
            int height;

            // Choose the smaller parent size.
            if (Texture.Width != Texture.Height)
            {
                if (Texture.Width < Texture.Height)
                {
                    width = Texture.Width;
                    height = Texture.Width;
                }
                else // height < width
                {
                    width = Texture.Height;
                    height = Texture.Height;
                }
            }
            else
            {
                width = Texture.Width;
                height = Texture.Height;
            }

            float scaleX = 1.0f;
            float scaleY = 1.0f;

            // If the aspect ratio is not 1:1.
            if (insideTexture.Width != insideTexture.Height)
            {
                float aspectRatio;

                if (insideTexture.Width < insideTexture.Height)
                {
                    aspectRatio = insideTexture.Width / (float)insideTexture.Height;
                    scaleX = (width / (float)insideTexture.Width) * Scale * insideScale * aspectRatio;
                    scaleY = height / (float)insideTexture.Height * Scale * insideScale;
                    return (scaleX, scaleY);
                }
                else // height < width
                {
                    aspectRatio = insideTexture.Height / (float)insideTexture.Width;
                    scaleX = (width / (float)insideTexture.Width) * Scale * insideScale;
                    scaleY = (height / (float)insideTexture.Height) * Scale * insideScale * aspectRatio;
                    return (scaleX, scaleY);
                }
            }
            else
            {
                scaleX = (width / (float)insideTexture.Width) * Scale * insideScale;
                scaleY = (height / (float)insideTexture.Height) * Scale * insideScale;
                return (scaleX, scaleY);
            }
        }

        /// <summary>
        /// Draws the the button.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch that draws the texture.</param>
        /// <param name="gameTime">The game time for the elapsed time since the last update call.</param>
        public override void Draw(in SpriteBatch spriteBatch, in GameTime gameTime)
        {
            if (Texture != null && IsVisible == true)
            {
                // Height difference between pressed and not pressed state.
                int stateHeightDifference;

                if (IsPressed == true)
                {
                    stateHeightDifference = textureHeightDifference;
                    Texture = texturePressed;
                    Position = positionPressed;
                }
                else
                {
                    stateHeightDifference = 0;
                    Texture = textureNormal;
                    Position = positionNormal;
                }

                // Draw all sprites and text.
                base.Draw(spriteBatch, gameTime);

                if (picture != null)
                {
                    spriteBatch.Draw(picture,
                                     new Vector2(Position.X + (Texture.Width * Scale / 2.0f), Position.Y + ((Texture.Height + stateHeightDifference) * Scale / 2.0f)),
                                     null,
                                     Color.White,
                                     0.0f,
                                     new Vector2(picture.Width / 2.0f, picture.Height / 2.0f),
                                     pictureScale,
                                     SpriteEffects.None,
                                     LayerDepth.middlePicture);
                }

                if (Text != null && font != null)
                {
                    spriteBatch.DrawString(font,
                                           Text,
                                           new Vector2(Position.X + (Texture.Width * Scale / 2.0f), Position.Y + ((Texture.Height + stateHeightDifference) * Scale / 2.0f)),
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
}
