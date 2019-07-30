using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoUI
{
    /// <summary>
    /// Class for all keyboard and mouse input.
    /// </summary>
    public static class Input
    {
        /// <summary>
        /// Returns the mouse position in screen space coordinates;
        /// </summary>
        public static Point MousePosition
        {
            get { return Mouse.GetState().Position; }
        }

        /// <summary>
        /// Checks if the left mouse button is pressed down.
        /// </summary>
        public static bool IsLeftMouseButtonPressed
        {
            get { return Mouse.GetState().LeftButton == ButtonState.Pressed; }
        }

        /// <summary>
        /// Checks if the right mouse button is pressed down.
        /// </summary>
        public static bool IsRightMouseButtonPressed
        {
            get { return Mouse.GetState().RightButton == ButtonState.Pressed; }
        }

        /// <summary>
        /// Checks if any key is pressed down.
        /// </summary>
        public static bool IsAnyKeyPressed
        {
            get
            {
                Keys[] keys = Keyboard.GetState().GetPressedKeys();
                return keys.Length > 0 ? true : false;
            }
        }

        /// <summary>
        /// Returns all pressed keys;
        /// </summary>
        public static Keys[] PressedKeys
        {
            get { return Keyboard.GetState().GetPressedKeys(); }
        }

        /// <summary>
        /// Checks if a specific key is pressed down.
        /// </summary>
        /// <param name="key">The name of the key that is pressed.</param>
        /// <returns></returns>
        public static bool IsSpecificKeyPressed(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key);
        }
    }
}
