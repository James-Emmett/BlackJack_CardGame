using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardGame
{
    public static class Debug
    {
        public static SpriteBatch   SpriteBatch { get; private set; }
        public static SpriteFont    DefaultFont { get; private set; }
        public static Texture2D     Texture;
        public static Color         Color;
        private static Rectangle    Source;

        // Maybe we should add logging?
        internal static void Initialize(GraphicsDevice device)
        {
            SpriteBatch = new SpriteBatch(device);
            DefaultFont = Engine.Instance.Content.Load<SpriteFont>("Fonts\\Default");

            // Create a single 1 white pixel texture
            Texture = new Texture2D(device, 1, 1);
            Texture.SetData(new Color[] { Color.White });
            Source = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Color = Color.Green;
        }

        public static void Begin()
        {
            SpriteBatch.Begin();
        }

        public static void End()
        {
            SpriteBatch.End();
        }

        public static void DrawLine(Vector2 start, Vector2 end)
        {
            DrawLine(start, MathExtra.AngleDiffernce(start, end), Vector2.Distance(start, end));
        }

        public static void DrawLine(Vector2 start, float angle, float length)
        {
            SpriteBatch.Draw(Texture, start, Source, Color, angle, Vector2.Zero, new Vector2(length, 1), SpriteEffects.None, 0);
        }

        public static void DrawPoint(Vector2 position, float size = 10)
        {
            float halfSize = size * 0.5f;
            DrawLine(position - new Vector2(halfSize, 0), position + new Vector2(halfSize, 0));
            DrawLine(position - new Vector2(0, halfSize), position + new Vector2(0, halfSize));
        }

        public static void DrawWireCircle(Vector2 center, float radius, int steps)
        {
            float arc = MathHelper.TwoPi / steps;
            Vector2 start = new Vector2(center.X, center.Y + radius);
            for (int i = 1; i <= steps; ++i)
            {
                Vector2 end = new Vector2(center.X + MathF.Sin(i * arc) * radius, center.Y + MathF.Cos(i * arc) * radius);
                DrawLine(start, end);
                start = end;
            }
        }

        public static void DrawWireSquare(float x, float y, float width, float height)
        {
            DrawLine(new Vector2(x, y), new Vector2(x + width, y));
            DrawLine(new Vector2(x, y), new Vector2(x, y + height));
            DrawLine(new Vector2(x, y + height), new Vector2(x + width, y + height));
            DrawLine(new Vector2(x + width, y), new Vector2(x + width, y + height));
        }

        public static void DrawSolidSquare(int x, int y, int width, int height, Color color)
        {
            SpriteBatch.Draw(Texture, new Rectangle(x, y, width, height), color);
        }

        public static void DrawText(string text, Vector2 position, float rotation = 0, float scale = 1)
        {
            SpriteBatch.DrawString(DefaultFont, text, position, Color, rotation, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
    }
}
