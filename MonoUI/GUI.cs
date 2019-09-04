using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace MonoUI
{
    /// <summary>
    /// Base class for GUI tree.
    /// </summary>
    public abstract class GUI
    {
        /// <summary>
        /// Delegate for managing draw calls.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch that draws the textures.</param>
        /// <param name="gameTime">The game time of the game instance.</param>
        public delegate void DrawHandler(in SpriteBatch spriteBatch, in GameTime gameTime);
        /// <summary>
        /// Draws all GUI elements.
        /// </summary>
        public static DrawHandler DrawAll { get; private set; } = delegate { };

        /// <summary>
        /// For storing additional informations about the GUI element.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Collection for all types of GUI items.
        /// </summary>
        private readonly static List<GUI> guiItems = new List<GUI>();

        /// <summary>
        /// Base constructor for inheritance.
        /// </summary>
        protected GUI()
        {
            guiItems.Add(this);

            IDrawable instance = this as IDrawable;
            if (instance != null)
            {
                DrawAll += instance.Draw;
            }
        }

        /// <summary>
        /// Returns items of a specific GUI type or type that derived from it.
        /// </summary>
        /// <typeparam name="T">The type of the GUI items, which are searched for.</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> Display<T>() where T : GUI
        {
            foreach (T item in guiItems.Where(value => value.GetType() == typeof(T) || value.GetType().IsSubclassOf(typeof(T)) == true))
            {
                yield return item;
            }
        }

        /// <summary>
        /// Removes the item from the GUI collection.
        /// </summary>
        public void RemoveElement()
        {
            IDrawable instance = this as IDrawable;
            if (instance != null)
            {
                DrawAll -= instance.Draw;
            }

            guiItems.Remove(this);
        }

        /// <summary>
        /// Values between 0.9 and 1.0 for all the different layers of drawable elements. (Exclusive)
        /// </summary>
        protected struct LayerDepth
        {
            /// <summary>
            /// Lower layer for textures.
            /// </summary>
            public const float lowerTexture = 0.91f;
            /// <summary>
            /// Lower layer for pictures.
            /// </summary>
            public const float lowerPicture = 0.92f;
            /// <summary>
            /// Lower layer for texts.
            /// </summary>
            public const float lowerText = 0.93f;

            /// <summary>
            /// Middle layer for textures.
            /// </summary>
            public const float middleTexture = 0.94f;
            /// <summary>
            /// Middle layer for pictures.
            /// </summary>
            public const float middlePicture = 0.95f;
            /// <summary>
            /// Middle layer for texts.
            /// </summary>
            public const float middleText = 0.96f;

            /// <summary>
            /// Upper layer for textures.
            /// </summary>
            public const float upperTexture = 0.97f;
            /// <summary>
            /// Upper layer for pictures.
            /// </summary>
            public const float upperPicture = 0.98f;
            /// <summary>
            /// Upper layer for texts.
            /// </summary>
            public const float upperText = 0.99f;
        }
    }
}
