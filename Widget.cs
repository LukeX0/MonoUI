using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoUI
{
    /// <summary>
    /// Base class for all control elements. Only for inheritance.
    /// </summary>
    public abstract class Widget : GUI, IDrawable
    {
        /// <summary>
        /// Defines if the user can interact with the control element.
        /// </summary>
        public bool IsActive { get; set; } = true;
        /// <summary>
        /// Defines if the control element is visible. An invisible control element is always inactive.
        /// </summary>
        public bool IsVisible { get; set; } = true;
        /// <summary>
        /// The color of the texture of the control element.
        /// </summary>
        public Color Color { get; set; } = Color.White;
        /// <summary>
        /// The color the control element receives when it gets selected. No value results in no color change.
        /// </summary>
        public Color? SelectionColor { get; set; } = null;

        /// <summary>
        /// True if the control element is selected, otherwise false.
        /// </summary>
        public bool IsSelected
        {
            get { return IsTextureSelected(Texture, Position, Scale); }
        }
        /// <summary>
        /// True if the control element is pressed, otherwise false;
        /// </summary>
        public bool IsPressed
        {
            get { return IsTexturePressed(Texture, Position, Scale); }
        }

        /// <summary>
        /// Delegate for managing events.
        /// </summary>
        public delegate void EventHandler();
        /// <summary>
        /// Raises an event during the frame the control element is clicked.
        /// </summary>
        public event EventHandler OnClick
        {
            add { onClick += value; }
            remove { onClick -= value; }
        }

        /// <summary>
        /// The absolut position of the control element.
        /// </summary>
        public Vector2 Position { get; protected set; }
        /// <summary>
        /// The tooltip that is to be displayed when the control element gets selected.
        /// </summary>
        public Tooltip Tooltip { get; set; }

        /// <summary>
        /// The location the control element should dock onto.
        /// </summary>
        protected DockControl Dock { get; set; }
        /// <summary>
        /// The space in pixel between the control element and the dock location.
        /// </summary>
        protected Point Offset { get; set; }
        /// <summary>
        /// The layer on that the texture is drawn.
        /// </summary>
        protected float Layer { get; set; }
        /// <summary>
        /// The scale of the texture.
        /// </summary>
        protected float Scale { get; private set; }
        /// <summary>
        /// The texture of the control element.
        /// </summary>
        protected Texture2D Texture
        {
            get { return texture; }
            set { texture = value ?? throw new NullReferenceException($"A main texture of the {GetType().Name} class cannot be null."); }
        }

        /// <summary>
        /// Ensures that only one control element is pressed at a time.
        /// </summary>
        private static bool isElementPressed;
        /// <summary>
        /// True while the control element is held down.
        /// </summary>
        private bool isHeldDown;
        /// <summary>
        /// Ensures only one activation per press.
        /// </summary>
        private bool isPressDone;
        /// <summary>
        /// True during the frame in that the control element is pressed.
        /// </summary>
        private bool isFirstFrame;

        private Texture2D texture;
        private EventHandler onClick;
        private readonly Game game;

        /// <summary>
        /// Base constructor for all control elements. Only for inheritance.
        /// </summary>
        /// <param name="game">The game instance in that the control element is to be created.</param>
        /// <param name="dock">The location the control element should dock onto.</param>
        /// <param name="offset">The space in pixel between the control element and the dock location.</param>
        /// <param name="scale">The value for scaling the texture. 1.0 is no scaling.</param>
        protected Widget(Game game, DockControl dock, Point offset, float scale = 1.0f) : base()
        {
            this.game = game ?? throw new ArgumentNullException($"The parameter {nameof(game)} in the constructor of the {GetType().Name} class cannot be null.");

            Dock = dock;
            Offset = offset;
            Scale = scale;

            Layer = LayerDepth.middleTexture;
            onClick = delegate { };
        }

        /// <summary>
        /// Toggles the IsActive state of the control element.
        /// </summary>
        public void ToggleIsActive()
        {
            IsActive = IsActive == false ? true : false;
        }

        /// <summary>
        /// Toggles the IsVisible state of the control element.
        /// </summary>
        public void ToggleIsVisible()
        {
            IsVisible = IsVisible == false ? true : false;
        }

        /// <summary>
        /// Gets the relative position for the control element.
        /// </summary>
        /// <param name="dock">The location the control element should dock onto.</param>
        /// <param name="offset">The space in pixel between the control element and the dock location.</param>
        /// <param name="texture">The texture that determines the positioning.</param>
        /// <param name="scale">The scale of the texture.</param>
        /// <returns></returns>
        protected Vector2 CalculatePosition(DockControl dock, Point offset, Texture2D texture, float scale)
        {
            int width;
            int height;

            if (texture != null)
            {
                width = texture.Width;
                height = texture.Height;
            }
            else
            {
                width = 0;
                height = 0;
            }

            switch (dock)
            {
                case DockControl.Center:
                    return new Vector2((game.Window.ClientBounds.Width / 2.0f) - (width * scale / 2.0f) + offset.X, (game.Window.ClientBounds.Height / 2.0f) - (height * scale / 2.0f) + offset.Y);
                case DockControl.CornerTopLeft:
                    return new Vector2(offset.X, offset.Y);
                case DockControl.CornerTopRight:
                    return new Vector2(game.Window.ClientBounds.Width - (width * scale) + offset.X, offset.Y);
                case DockControl.CornerBottomLeft:
                    return new Vector2(offset.X, game.Window.ClientBounds.Height - (height * scale) + offset.Y);
                case DockControl.CornerBottomRight:
                    return new Vector2(game.Window.ClientBounds.Width - (width * scale) + offset.X, game.Window.ClientBounds.Height - (height * scale) + offset.Y);
                case DockControl.MiddleTop:
                    return new Vector2((game.Window.ClientBounds.Width / 2.0f) - (width / 2.0f * scale) + offset.X, offset.Y);
                case DockControl.MiddleBottom:
                    return new Vector2((game.Window.ClientBounds.Width / 2.0f) - (width / 2.0f * scale) + offset.X, game.Window.ClientBounds.Height - (height * scale) + offset.Y);
                case DockControl.MiddleLeft:
                    return new Vector2(offset.X, (game.Window.ClientBounds.Height / 2.0f) - (height / 2.0f * scale) + offset.Y);
                case DockControl.MiddleRight:
                    return new Vector2(game.Window.ClientBounds.Width - (width * scale) + offset.X, (game.Window.ClientBounds.Height / 2.0f) - (height / 2.0f * scale) + offset.Y);
                default:
                    return Vector2.Zero;
            }
        }

        /// <summary>
        /// Sets the position of the control element.
        /// </summary>
        /// <param name="dock">The location the control element should dock onto.</param>
        /// <param name="offset">The space in pixel between the control element and the dock location.</param>
        public virtual void SetPosition(DockControl dock, Point offset)
        {
            Dock = dock;
            Offset = offset;

            Position = CalculatePosition(dock, offset, Texture, Scale);
        }

        /// <summary>
        /// Checks if a texture at a specific position is selected.
        /// </summary>
        /// <param name="texture">The texture that is to be checked.</param>
        /// <param name="position">The absolute position of the texture.</param>
        /// <param name="scale">The scale of the texture. 1 is no scaling.</param>
        /// <returns></returns>
        protected bool IsTextureSelected(Texture2D texture, Vector2 position, float scale)
        {
            if (texture == null || IsActive == false || IsVisible == false)
            {
                return false;
            }
            if (Input.MousePosition.X < position.X || Input.MousePosition.X > (position.X + (texture.Width * scale)))
            {
                return false;
            }
            if (Input.MousePosition.Y < position.Y || Input.MousePosition.Y > (position.Y + (texture.Height * scale)))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if a texture at a specific position is pressed.
        /// </summary>
        /// <param name="texture">The texture that is to be checked.</param>
        /// <param name="position">The absolute position of the texture.</param>
        /// <param name="scale">The scale of the texture. 1 is no scaling.</param>
        /// <returns></returns>
        protected bool IsTexturePressed(Texture2D texture, Vector2 position, float scale)
        {
            if (Input.IsLeftMouseButtonPressed == false)
            {
                isElementPressed = false;
                isHeldDown = false;
                isPressDone = false;
                isFirstFrame = false;
                return false;
            }
            if (IsTextureSelected(texture, position, scale) == true && isPressDone == false && isElementPressed == false || isHeldDown == true)
            {
                isElementPressed = true;
                isHeldDown = true;
                return true;
            }
            isPressDone = true;
            return false;
        }

        /// <summary>
        /// Resets the press state. Use this if a control element has more than one pressable sections.
        /// </summary>
        protected void ResetPressState()
        {
            isElementPressed = false;
            isHeldDown = false;
            isPressDone = false;
        }

        /// <summary>
        /// Draws the the control element.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch that draws the texture.</param>
        /// <param name="gameTime">The game time for the elapsed time since the last update call.</param>
        public virtual void Draw(in SpriteBatch spriteBatch, in GameTime gameTime)
        {
            if (Texture != null && IsVisible == true)
            {
                Color drawColor = SelectionColor != null && IsSelected == true ? (Color)SelectionColor : Color;

                spriteBatch.Draw(Texture,
                                 Position,
                                 null,
                                 drawColor,
                                 0.0f,
                                 Vector2.Zero,
                                 Scale,
                                 SpriteEffects.None,
                                 Layer);

                if (Tooltip != null)
                {
                    Tooltip.Activate(spriteBatch, gameTime, IsSelected);
                }

                if (IsPressed == true && isFirstFrame == false)
                {
                    onClick();
                    isFirstFrame = true;
                }
            }
        }

        /// <summary>
        /// Defines where the control element should dock onto the screen.
        /// </summary>
        public enum DockControl
        {
            /// <summary>
            /// The center of the screen.
            /// </summary>
            Center = 0,
            /// <summary>
            /// The corner at the top left of the screen.
            /// </summary>
            CornerTopLeft = 1,
            /// <summary>
            /// The corner at the top right of the screen.
            /// </summary>
            CornerTopRight = 2,
            /// <summary>
            /// The corner at the bottom left of the screen.
            /// </summary>
            CornerBottomLeft = 3,
            /// <summary>
            /// The corner at the bottom right of the screen.
            /// </summary>
            CornerBottomRight = 4,
            /// <summary>
            /// The middle at the top side of the screen.
            /// </summary>
            MiddleTop = 5,
            /// <summary>
            /// The middle at the bottom side of the screen.
            /// </summary>
            MiddleBottom = 6,
            /// <summary>
            /// The middle at the left side of the screen.
            /// </summary>
            MiddleLeft = 7,
            /// <summary>
            /// The middle at the right side of the screen.
            /// </summary>
            MiddleRight = 8
        }
    }
}
