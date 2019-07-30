using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoUI
{
    /// <summary>
    /// Class for GUI slider.
    /// </summary>
    public class Slider : StatusBar
    {
        /// <summary>
        /// True is horizontal alignment, false is vertical alignment.
        /// </summary>
        public override bool Alignment
        {
            get { return alignment; }
            set { alignment = value; if (value == true) { barOffset = Texture.Width * Scale * 0.5f; } else { barOffset = Texture.Height * Scale * 0.5f; } }
        }

        /// <summary>
        /// The color of the background bar.
        /// </summary>
        public Color BackgroundColor { get; set; }
        /// <summary>
        /// The color of the status bar.
        /// </summary>
        public Color StatusColor { get; set; }

        /// <summary>
        /// The minimum value of the slider. (Inclusive)
        /// </summary>
        public float MinValue { get; set; }
        /// <summary>
        /// The maximum value of the slider. (Inclusive)
        /// </summary>
        public float MaxValue { get; set; }
        /// <summary>
        /// The current value of the slider. This value is clamped between MinValue and MaxValue. (Inclusive)
        /// </summary>
        public float Value
        {
            get { return value; }
            set { if (value < MinValue) { this.value = MinValue; } else if (value > MaxValue) { this.value = MaxValue; } else { this.value = value; } }
        }

        /// <summary>
        /// The step between each value when using the handle. No value results in no steps.
        /// </summary>
        public float? Step { get; set; } = null;
        /// <summary>
        /// The status of the slider, 0 means 0% filled and 1 means 100% filled.
        /// </summary>
        public override float Status
        {
            get { return (Value - MinValue) / (MaxValue - MinValue); }
        }

        private bool alignment;
        private bool firstPress;
        private float value;
        private float handleOffset;
        private float barOffset;
        private Vector2 originalPosition;

        private readonly Texture2D backgroundTexture;
        private readonly float barScale;

        /// <summary>
        /// The private constructor of the slider class.
        /// </summary>
        /// <param name="game">The game instance in that the control element is to be created.</param>
        /// <param name="dock">The location the control element should dock onto.</param>
        /// <param name="offset">The space in pixel between the control element and the dock location.</param>
        /// <param name="minValue">The minimum value of the slider. (Inclusive)</param>
        /// <param name="maxValue">The maximum value of the slider. (Inclusive)</param>
        /// <param name="value">The current value of the slider. This value is clamped between minValue and maxValue.</param>
        /// <param name="scale">The value for scaling the textures.</param>
        private Slider(Game game, DockControl dock, Point offset, float minValue, float maxValue, float value, float scale = 1.0f) : base(game, dock, offset, scale)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            Value = value;
        }

        /// <summary>
        /// Creates a new simple slider.
        /// </summary>
        /// <param name="game">The game instance in that the slider is to be created.</param>
        /// <param name="dock">The location the slider should dock onto.</param>
        /// <param name="offset">The space in pixel between the slider and the dock location.</param>
        /// <param name="minValue">The minimum value of the slider. (Inclusive)</param>
        /// <param name="maxValue">The maximum value of the slider. (Inclusive)</param>
        /// <param name="value">The current value of the slider. This value is clamped between minValue and maxValue.</param>
        /// <param name="width">The width of the slider bar in pixel. This have to be greater than zero.</param>
        /// <param name="height">The height of the slider bar in pixel. This have to be greater than zero.</param>
        /// <param name="textureHandle">The texture for the slider handle.</param>
        /// <param name="scale">The scale of the handle texture. 1.0 is no scaling.</param>
        public Slider(Game game, DockControl dock, Point offset, float minValue, float maxValue, float value, int width, int height, Texture2D textureHandle, float scale = 1.0f) : this(game, dock, offset, minValue, maxValue, value, scale)
        {
            BackgroundColor = Color.White;
            StatusColor = Color.Black;

            Texture = textureHandle;
            backgroundTexture = Tool.CreateTexture(game.GraphicsDevice, width, height, Color.White);
            StatusTexture = Tool.CreateTexture(game.GraphicsDevice, width, height, Color.White);

            barScale = 1.0f;
            Alignment = true;

            originalPosition = CalculatePosition(dock, offset, backgroundTexture, barScale);
            Position = InitialHandlePosition();
        }

        /// <summary>
        /// Creates a new slider with textures.
        /// </summary>
        /// <param name="game">The game instance in that the slider is to be created.</param>
        /// <param name="dock">The location the slider should dock onto.</param>
        /// <param name="offset">The space in pixel between the slider and the dock location.</param>
        /// <param name="minValue">The minimum value of the slider. (Inclusive)</param>
        /// <param name="maxValue">The maximum value of the slider. (Inclusive)</param>
        /// <param name="value">The current value of the slider. This value is clamped between minValue and maxValue.</param>
        /// <param name="textureBackground">The texture for the background bar.</param>
        /// <param name="textureStaus">The texture for the foreground bar.</param>
        /// <param name="textureHandle">The texture for the slider handle.</param>
        /// <param name="scale">The value for scaling the textures. 1.0 is no scaling.</param>
        public Slider(Game game, DockControl dock, Point offset, float minValue, float maxValue, float value, Texture2D textureBackground, Texture2D textureStaus, Texture2D textureHandle, float scale = 1.0f) : this(game, dock, offset, minValue, maxValue, value, scale)
        {
            BackgroundColor = Color.White;
            StatusColor = Color.White;

            Texture = textureHandle;
            backgroundTexture = textureBackground;
            StatusTexture = textureStaus;

            barScale = scale;
            Alignment = true;

            originalPosition = CalculatePosition(dock, offset, backgroundTexture, barScale);
            Position = InitialHandlePosition();
        }

        /// <summary>
        /// Sets the position of the control element.
        /// </summary>
        /// <param name="dock">The location the control element should dock onto.</param>
        /// <param name="offset">The space in pixel between the control element and the dock location.</param>
        public override void SetPosition(DockControl dock, Point offset)
        {
            Dock = dock;
            Offset = offset;

            originalPosition = CalculatePosition(dock, offset, backgroundTexture, barScale);
            Position = InitialHandlePosition();
        }

        /// <summary>
        /// Returns the initial position for the slider handle.
        /// </summary>
        /// <returns></returns>
        private Vector2 InitialHandlePosition()
        {
            if (Texture == null || backgroundTexture == null)
            {
                return Vector2.Zero;
            }
            else if (Alignment == true) // Horizontal slider
            {
                Rectangle statusBar = StatusBarSize();
                return new Vector2(originalPosition.X + statusBar.Width * barScale - (Texture.Width * Scale * 0.5f),
                                   originalPosition.Y + backgroundTexture.Height * barScale * 0.5f - Texture.Height * Scale * 0.5f);
            }
            else // Vertical slider
            {
                Rectangle statusBar = StatusBarSize();
                return new Vector2(originalPosition.X + backgroundTexture.Width * barScale * 0.5f - Texture.Width * Scale * 0.5f,
                                   originalPosition.Y + statusBar.Height * barScale - (Texture.Height * Scale * 0.5f));
            }
        }

        /// <summary>
        /// Sets the value and the handle position of the slider, when an interaction occurs.
        /// </summary>
        private void Interact()
        {
            if (IsPressed == true) // Use of Handle
            {
                float backgroundTextureSize;
                float mousePosition;
                float oldPosition;
                float defaultPosition;
                float newPosition;

                if (Alignment == true) // Horizontal slider
                {
                    backgroundTextureSize = backgroundTexture.Width * barScale;
                    mousePosition = Input.MousePosition.X;
                    oldPosition = Position.X;
                    defaultPosition = originalPosition.X;
                }
                else // Vertical slider
                {
                    backgroundTextureSize = backgroundTexture.Height * barScale;
                    mousePosition = Input.MousePosition.Y;
                    oldPosition = Position.Y;
                    defaultPosition = originalPosition.Y;
                }

                if (firstPress == false)
                {
                    handleOffset = mousePosition - oldPosition;
                    firstPress = true;
                }
                if (handleOffset > 0)
                {
                    mousePosition -= handleOffset;
                }

                if (Step != null) // Steps
                {
                    if (mousePosition >= defaultPosition - barOffset + (backgroundTextureSize * (Value + Step - MinValue) / (MaxValue - MinValue)))
                    {
                        Value += (float)Step;
                    }
                    else if (mousePosition <= defaultPosition - barOffset + (backgroundTextureSize * (Value - Step - MinValue) / (MaxValue - MinValue)))
                    {
                        Value -= (float)Step;
                    }

                    newPosition = defaultPosition - barOffset + (backgroundTextureSize * Status);
                }
                else // No steps
                {
                    newPosition = MathHelper.Clamp(mousePosition, defaultPosition - barOffset, defaultPosition - barOffset + backgroundTextureSize);
                    float percentage = (mousePosition + barOffset - defaultPosition) / backgroundTextureSize;
                    Value = MathHelper.Lerp(MinValue, MaxValue, percentage);
                }

                if (Alignment == true) // Horizontal slider
                {
                    Position = new Vector2(newPosition, originalPosition.Y + backgroundTexture.Height * barScale * 0.5f - Texture.Height * Scale * 0.5f);
                }
                else // Vertical slider
                {
                    Position = new Vector2(originalPosition.X + backgroundTexture.Width * barScale * 0.5f - Texture.Width * Scale * 0.5f, newPosition);
                }
            }
            else if (Step == null)
            {
                ResetPressState();

                if (IsTexturePressed(backgroundTexture, originalPosition, barScale) == true) // Use of bar click
                {
                    if (Alignment == true) // Horizontal slider
                    {
                        float percentage = (Input.MousePosition.X - originalPosition.X) / backgroundTexture.Width * barScale;
                        Position = new Vector2(Input.MousePosition.X - barOffset, originalPosition.Y + backgroundTexture.Height * barScale * 0.5f - Texture.Height * Scale * 0.5f);
                        Value = MathHelper.Lerp(MinValue, MaxValue, percentage);
                    }
                    else // Vertical slider
                    {
                        float percentage = (Input.MousePosition.Y - originalPosition.Y) / backgroundTexture.Height * barScale;
                        Position = new Vector2(originalPosition.X + backgroundTexture.Width * barScale * 0.5f - Texture.Width * Scale * 0.5f, Input.MousePosition.Y - barOffset);
                        Value = MathHelper.Lerp(MinValue, MaxValue, percentage);
                    }
                }
            }

            if (IsPressed == false)
            {
                firstPress = false;
            }
        }

        /// <summary>
        /// Draws the slider.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch that draws the texture.</param>
        /// <param name="gameTime">The game time for the elapsed time since the last update call.</param>
        public override void Draw(in SpriteBatch spriteBatch, in GameTime gameTime)
        {
            if (backgroundTexture != null && StatusTexture != null && IsVisible == true)
            {
                Interact();

                // Background texture
                spriteBatch.Draw(backgroundTexture,
                                 originalPosition,
                                 null,
                                 BackgroundColor,
                                 0.0f,
                                 Vector2.Zero,
                                 barScale,
                                 SpriteEffects.None,
                                 LayerDepth.lowerTexture);

                // Status texture
                spriteBatch.Draw(StatusTexture,
                                 originalPosition,
                                 StatusBarSize(),
                                 StatusColor,
                                 0.0f,
                                 Vector2.Zero,
                                 barScale,
                                 SpriteEffects.None,
                                 LayerDepth.lowerPicture);

                // handle texture
                base.Draw(spriteBatch, gameTime);
            }
        }
    }
}
