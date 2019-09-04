using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoUI
{
    /// <summary>
    /// Class for GUI drop-down list.
    /// </summary>
    public class DropDownList : Widget
    {
        /// <summary>
        /// The color of the texture of a non-selected item.
        /// </summary>
        public Color ItemBackgroundColor { get; set; }
        /// <summary>
        /// The color of the texture of a selected item.
        /// </summary>
        public Color ItemSelectedColor { get; set; }
        /// <summary>
        /// The color of the text.
        /// </summary>
        public Color TextColor { get; set; }

        /// <summary>
        /// The currently selected item.
        /// </summary>
        public string ActiveItem { get; private set; }

        private bool isActivated;
        private bool isDropped;

        private readonly string[] items;
        private readonly SpriteFont font;

        private Texture2D picture;
        private Color pictureColor;
        private float pictureScale;

        /// <summary>
        /// Creates a new drop-down list.
        /// </summary>
        /// <param name="game">The game instance in that the drop-down list is to be created.</param>
        /// <param name="dock">The location the drop-down list should dock onto.</param>
        /// <param name="offset">The space in pixel between the drop-down list and the dock location.</param>
        /// <param name="items">The selectable items for the drop-down list. The item at index 0 is the first active item.</param>
        /// <param name="textColor">The color of the text.</param>
        /// <param name="font">The font the text is using. The font size should match with the measurements of the drop-down list.</param>
        /// <param name="width">The total width of the drop-down list in pixel. This have to be greater than zero.</param>
        /// <param name="height">The height of the collapsed drop-down list in pixel. This have to be greater than zero.</param>
        public DropDownList(Game game, DockControl dock, Point offset, string[] items, Color textColor, SpriteFont font, int width = 180, int height = 25) : base(game, dock, offset, 1.0f)
        {
            this.items = items;
            this.font = font;

            ActiveItem = items[0];
            TextColor = textColor;

            ItemBackgroundColor = Color.LightGray;
            ItemSelectedColor = Color.SlateGray;

            if (items != null && items.Length >= 1)
            {
                Texture = Tool.CreateTexture(game.GraphicsDevice, width, height, Color.White);
                Position = CalculatePosition(dock, offset, Texture, 1.0f);
            }
        }

        /// <summary>
        /// Places an optional picture on the right side of the drop-down list.
        /// </summary>
        /// <param name="texture">The texture that is to be placed.</param>
        /// <param name="color">The color of the texture.</param>
        /// <param name="scale">The scale of the texture. 1.0 is no scaling.</param>
        public void SetPicture(Texture2D texture, Color color, float scale = 1.0f)
        {
            picture = texture;
            pictureColor = color;
            pictureScale = scale;
        }

        /// <summary>
        /// Draws the drop-down list.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch that draws the texture.</param>
        /// <param name="gameTime">The game time for the elapsed time since the last update call.</param>
        public override void Draw(in SpriteBatch spriteBatch, in GameTime gameTime)
        {
            if (IsPressed == false)
            {
                isActivated = false;
            }
            else if (isActivated == false)
            {
                isDropped = isDropped == false ? true : false;
                isActivated = true;
            }

            if (Texture != null && font != null && IsVisible == true)
            {
                base.Draw(spriteBatch, gameTime);

                if (picture != null)
                {
                    spriteBatch.Draw(picture,
                                     new Vector2(Position.X + Texture.Width * Scale - picture.Width * pictureScale, Position.Y),
                                     null,
                                     pictureColor,
                                     0.0f,
                                     Vector2.Zero,
                                     pictureScale,
                                     SpriteEffects.None,
                                     LayerDepth.middlePicture);
                }

                spriteBatch.DrawString(font,
                                       ActiveItem,
                                       new Vector2(Position.X, Position.Y + Texture.Height * Scale / 2.0f),
                                       TextColor,
                                       0.0f,
                                       new Vector2(0, (font.MeasureString(ActiveItem) / 2.0f).Y),
                                       1.0f,
                                       SpriteEffects.None,
                                       LayerDepth.middleText);

                if (isDropped == true)
                {
                    for (int i = 0; i < items.Length; i++)
                    {
                        Vector2 itemPosition = new Vector2(Position.X, Position.Y + Texture.Height * Scale * (i + 1));
                        Color itemColor = IsTextureSelected(Texture, itemPosition, 1.0f) == true ? ItemSelectedColor : ItemBackgroundColor;

                        ResetPressState();
                        if (IsTexturePressed(Texture, itemPosition, 1.0f) == true)
                        {
                            ActiveItem = items[i];
                            isDropped = false;
                        }

                        spriteBatch.Draw(Texture,
                                         itemPosition,
                                         null,
                                         itemColor,
                                         0.0f,
                                         Vector2.Zero,
                                         Scale,
                                         SpriteEffects.None,
                                         LayerDepth.middleTexture);

                        spriteBatch.DrawString(font,
                                               items[i],
                                               new Vector2(itemPosition.X, itemPosition.Y + Texture.Height * Scale / 2.0f),
                                               TextColor,
                                               0.0f,
                                               new Vector2(0, (font.MeasureString(items[i]) / 2.0f).Y),
                                               1.0f,
                                               SpriteEffects.None,
                                               LayerDepth.middleText);
                    }
                }
            }
        }
    }
}
