using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace MonoUI
{
    /// <summary>
    /// The utility class of the GUI system.
    /// </summary>
    public static class Tool
    {
        /// <summary>
        /// Creates a new single color texture.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device that is going to draw the texture.</param>
        /// <param name="width">The width of the new texture in pixel. This have to be greater than zero.</param>
        /// <param name="height">The height of the new texture in pixel. This have to be greater than zero.</param>
        /// <param name="color">The color of the new texture.</param>
        /// <returns></returns>
        public static Texture2D CreateTexture(in GraphicsDevice graphicsDevice, int width, int height, Color color)
        {
            if (graphicsDevice == null || width < 1 || height < 1)
            {
                Debug.WriteLine("Texture creation failed!");
                return null;
            }

            Texture2D newTexture = new Texture2D(graphicsDevice, width, height);

            int pixelCount = width * height;
            Color[] textureColors = new Color[pixelCount];

            for (int i = 0; i < pixelCount; i++)
            {
                textureColors[i] = color;
            }

            newTexture.SetData(textureColors);
            return newTexture;
        }
    }
}
