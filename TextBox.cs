using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MonoUI
{
    /// <summary>
    /// Class for GUI text box.
    /// </summary>
    public class TextBox : Widget
    {
        /// <summary>
        /// The text inside the text box.
        /// </summary>
        public string Text
        {
            set
            {
                foreach (char item in value.Where(item => font.Characters.Contains<char>(item) == false))
                {
                    return;
                }
                textBuilder.Clear();
                textBuilder.Append(value);
            }
            get { return textBuilder.ToString(); }
        }

        /// <summary>
        /// The color of the text.
        /// </summary>
        public Color TextColor { get; set; }
        /// <summary>
        /// Determines if the text can reach over the text field.
        /// </summary>
        public bool OverFlow { get; set; }
        /// <summary>
        /// The maximum number of characters the text can contain.
        /// </summary>
        public ushort MaxCharacters { get; set; }
        /// <summary>
        /// The empty space in pixel on the left side between the texture and the text.
        /// </summary>
        public ushort Indentation { get; set; }

        private ushort CharacterIndex
        {
            get { return characterIndex; }
            set { if (value < 0) { characterIndex = 0; } else { characterIndex = value; } }
        }
        private ushort characterIndex;

        private int textLineTimer;
        private readonly int maxTextLineTime;

        private int keyTimer;
        private readonly int maxKeyTime;

        private int editTimer;
        private readonly int maxEditTime;

        private Color textLineColor;
        private bool blinkingPipe;
        private bool isEditModeOn;
        private bool isKeyInputReady;

        private readonly Texture2D textLine;
        private readonly StringBuilder textBuilder;
        private readonly SpriteFont font;

        /// <summary>
        /// The private constructor of the text box class.
        /// </summary>
        /// <param name="game">The game instance in that the text box is to be created.</param>
        /// <param name="dock">The location the text box should dock onto.</param>
        /// <param name="offset">The space in pixel between the text box and the dock location.</param>
        /// <param name="text">The initial text that is to be displayed in the text box.</param>
        /// <param name="textColor">The color of the text.</param>
        /// <param name="font">The font of the text. The font size should match with the measurements.</param>
        /// <param name="scale">The value for scaling the texture. 1.0 is no scaling.</param>
        private TextBox(Game game, DockControl dock, Point offset, string text, Color textColor, SpriteFont font, float scale = 1.0f) : base(game, dock, offset, scale)
        {
            game.Window.TextInput += EditText;
            textBuilder = new StringBuilder(text);
            MaxCharacters = ushort.MaxValue;
            Indentation = 0;

            maxTextLineTime = 500;
            maxKeyTime = 400;
            maxEditTime = 25;

            this.font = font;
            TextColor = textColor;

            blinkingPipe = true;
            OverFlow = false;

            textLine = Tool.CreateTexture(game.GraphicsDevice, 1, (int)font.MeasureString("|").Y, Color.Black);
        }

        /// <summary>
        /// Creates a new simple single line text box.
        /// </summary>
        /// <param name="game">The game instance in that the text box is to be created.</param>
        /// <param name="dock">The location the text box should dock onto.</param>
        /// <param name="offset">The space in pixel between the text box and the dock location.</param>
        /// <param name="text">The initial text that is to be displayed in the text box.</param>
        /// <param name="textColor">The color of the text.</param>
        /// <param name="font">The font of the text. The font size should match with the measurements.</param>
        /// <param name="width">The width of the text box in pixel. This have to be greater than zero.</param>
        /// <param name="height">The height of the text box in pixel. This have to be greater than zero.</param>
        public TextBox(Game game, DockControl dock, Point offset, string text, Color textColor, SpriteFont font, int width = 200, int height = 25) : this(game, dock, offset, text, textColor, font, 1.0f)
        {
            Texture = Tool.CreateTexture(game.GraphicsDevice, width, height, Color.White);
            Position = CalculatePosition(dock, offset, Texture, 1.0f);
        }

        /// <summary>
        /// Creates a new single line text box with a texture.
        /// </summary>
        /// <param name="game">The game instance in that the text box is to be created.</param>
        /// <param name="dock">The location the text box should dock onto.</param>
        /// <param name="offset">The space in pixel between the text box and the dock location.</param>
        /// <param name="text">The initial text that is to be displayed in the text box.</param>
        /// <param name="textColor">The color of the text.</param>
        /// <param name="font">The font of the text. The font size should match with the measurements.</param>
        /// <param name="texture">The texture of the text box.</param>
        /// <param name="scale">The value for scaling the texture. 1.0 is no scaling.</param>
        public TextBox(Game game, DockControl dock, Point offset, string text, Color textColor, SpriteFont font, Texture2D texture, float scale = 1.0f) : this(game, dock, offset, text, textColor, font, scale)
        {
            Texture = texture;
            Position = CalculatePosition(dock, offset, Texture, scale);
        }

        /// <summary>
        /// Edits the text of the text field.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event arguments for text input.</param>
        private void EditText(object sender, TextInputEventArgs e)
        {
            if (isEditModeOn == false || IsActive == false || IsVisible == false)
            {
                return;
            }

            // Checks for functionality first and than inserts the characters if no functionality was provided.
            switch (e.Key)
            {
                case Keys.Left:
                    if (CharacterIndex > 0)
                    {
                        CharacterIndex--;
                    }
                    return;
                case Keys.Right:
                    if (CharacterIndex < textBuilder.Length)
                    {
                        CharacterIndex++;
                    }
                    return;
                case Keys.Home:
                    CharacterIndex = 0;
                    return;
                case Keys.End:
                    CharacterIndex = Convert.ToUInt16(textBuilder.Length);
                    return;
                case Keys.Back:
                    if (CharacterIndex > 0 && textBuilder.Length > 0)
                    {
                        textBuilder.Remove(CharacterIndex - 1, 1);
                        CharacterIndex--;
                    }
                    return;
                case Keys.Delete:
                    if (CharacterIndex < textBuilder.Length)
                    {
                        textBuilder.Remove(CharacterIndex, 1);
                    }
                    return;
                default:
                    // No functionality key was pressed.
                    break;
            }

            if (font.Characters.Contains<char>(e.Character) == false || IsSpaceFree(e.Character) == false || textBuilder.Length >= MaxCharacters)
            {
                return;
            }
            else
            {
                textBuilder.Insert(CharacterIndex, e.Character);
                CharacterIndex++;
            }
        }

        /// <summary>
        /// Checks specific keys that are not part of TextInput event.
        /// </summary>
        /// <param name="gameTime">The game time for the elapsed time since the last update call.</param>
        private void AdditionalKeyFunctionality(GameTime gameTime)
        {
            if (Input.IsAnyKeyPressed == false)
            {
                isKeyInputReady = true;
                keyTimer = 0;
            }
            else
            {
                keyTimer += gameTime.ElapsedGameTime.Milliseconds;
            }

            if (isKeyInputReady == true || keyTimer >= maxKeyTime)
            {
                if (keyTimer >= maxKeyTime)
                {
                    editTimer += gameTime.ElapsedGameTime.Milliseconds;
                    if (editTimer < maxEditTime)
                    {
                        return;
                    }
                    editTimer = 0;
                }

                if (Input.IsSpecificKeyPressed(Keys.Left) == true ||
                    Input.IsSpecificKeyPressed(Keys.Right) == true ||
                    Input.IsSpecificKeyPressed(Keys.Home) == true ||
                    Input.IsSpecificKeyPressed(Keys.End) == true)
                {
                    EditText(null, new TextInputEventArgs('\0', Input.PressedKeys.Last()));
                    isKeyInputReady = false;
                }
            }
        }

        /// <summary>
        /// Returns true if space is available in the text box. Otherwise returns false.
        /// </summary>
        /// <param name="character">The new character that is to be added, for measurements.</param>
        /// <returns></returns>
        private bool IsSpaceFree(char character)
        {
            if (OverFlow == true)
            {
                return true;
            }

            return font.MeasureString(Text + character).X + Indentation < Texture.Width * Scale;
        }

        /// <summary>
        /// Draws the the text box.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch that draws the texture.</param>
        /// <param name="gameTime">The game time for the elapsed time since the last update call.</param>
        public override void Draw(in SpriteBatch spriteBatch, in GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            if (Texture != null && font != null && IsVisible == true)
            {
                if (IsPressed == true)
                {
                    isEditModeOn = true;
                }
                else if (Input.IsLeftMouseButtonPressed == true)
                {
                    isEditModeOn = false;
                }

                if (isEditModeOn == true)
                {
                    AdditionalKeyFunctionality(gameTime);

                    if (Input.IsAnyKeyPressed == true)
                    {
                        blinkingPipe = false;
                        textLineColor.A = byte.MaxValue;
                    }
                    else
                    {
                        blinkingPipe = true;
                    }

                    if (blinkingPipe == true)
                    {
                        textLineTimer += gameTime.ElapsedGameTime.Milliseconds;
                        if (textLineTimer >= maxTextLineTime)
                        {
                            textLineTimer = 0;
                            textLineColor.A = textLineColor.A == byte.MinValue ? byte.MaxValue : byte.MinValue;
                        }
                    }

                    spriteBatch.Draw(textLine,
                                     new Vector2(Position.X + Indentation + font.MeasureString(Text.Substring(0, CharacterIndex)).X, Position.Y + Texture.Height * Scale / 2.0f),
                                     null,
                                     textLineColor,
                                     0.0f,
                                     new Vector2(textLine.Width / 2.0f, textLine.Height / 2.0f),
                                     1.0f,
                                     SpriteEffects.None,
                                     LayerDepth.middleText);
                }

                spriteBatch.DrawString(font,
                                       Text,
                                       new Vector2(Position.X + Indentation, Position.Y + Texture.Height * Scale / 2.0f),
                                       TextColor,
                                       0.0f,
                                       new Vector2(0, (font.MeasureString(Text) / 2.0f).Y),
                                       1.0f,
                                       SpriteEffects.None,
                                       LayerDepth.middleText);
            }
        }
    }
}
