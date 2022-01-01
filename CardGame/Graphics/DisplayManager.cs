using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardGame
{
    public class DisplayManager
    {
        public static DisplayManager  Instance {get;private set;}
        private GraphicsDeviceManager m_GraphicsManager;
        private GameWindow m_Window;
        public Matrix   ScaleMatrix { get; private set; }
        public Matrix   InvScaleMatrix { get; private set; }
        private int     m_Width = 0;
        private int     m_Height = 0;
        private int     m_TargetWidth = 0;
        private int     m_TargetHeight = 0;
        private int     m_PrevWidth = 0;
        private int     m_PrevHeight = 0;
        private bool    m_FullScreen;
        private bool    m_Dirty = true;
        public bool     SizeChanged { get; private set; }

        public DisplayManager(GraphicsDeviceManager graphicsManager, int width, int height, int targetWidth, int targetHeight)
        {
            if(Instance != null) { return; }
            Instance = this;

            m_GraphicsManager = graphicsManager;
            m_Window = Engine.Instance.Window;
            m_Width = width;
            m_Height = height;
            m_TargetWidth = targetWidth;
            m_TargetHeight = targetHeight;
            m_PrevWidth = m_Width;
            m_PrevHeight = m_Height;
            m_Dirty = true;

            // Bind the resize event from the window
            m_Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        public int Width
        {
            get { return m_Width; }
            set { m_Width = value; m_Dirty = true; }
        }

        public int Height
        {
            get { return m_Height; }
            set { m_Height = value; m_Dirty = true; }
        }

        public int TargetWidth
        {
            get { return m_TargetWidth; }
            set { m_TargetWidth = value; m_Dirty = true; }
        }

        public int TargetHeight
        {
            get { return m_TargetHeight; }
            set { m_TargetHeight = value; m_Dirty = true; }
        }

        public bool FullScreen
        {
            get { return m_FullScreen; }
            set { if (m_FullScreen != value) { m_FullScreen = value; m_Dirty = true; } }
        }

        private void ApplySettings()
        {
            m_Window.ClientSizeChanged -= Window_ClientSizeChanged;
            m_GraphicsManager.PreferredBackBufferWidth = m_Width;
            m_GraphicsManager.PreferredBackBufferHeight = m_Height;
            m_GraphicsManager.IsFullScreen = m_FullScreen;
            //--Apply changes to recreate back buffers--
            m_GraphicsManager.ApplyChanges();

            // Monogame Bug? Win32 windows adjust size to account for Window top bar, however they change the position on the x to 8?
            // it should be 0 on maximise and 0 fullscreen but isnt, Y should be 0 too but it seems to be adjusted for top bar.
            if(m_Width == m_TargetWidth)
            {
                m_Window.Position = new Point(0, m_Window.Position.Y);
            }

            m_Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        private void UpdateScale()
        {
            float targetAspect = (float)m_TargetWidth / m_TargetHeight;

            if (m_Width != m_TargetWidth)
            {
                // Did we change width or height?
                if (m_Width != m_PrevWidth)
                {
                    m_Height = (int)(m_Width / targetAspect);
                }
                else if (m_Height != m_PrevHeight)
                {
                    m_Width = (int)(m_Height * targetAspect);
                }
            }

            //--Update the Matrix--
            ScaleMatrix = Matrix.CreateScale((float)m_Width / m_TargetWidth,
                                       (float)m_Height / m_TargetHeight, 1f);

            InvScaleMatrix = Matrix.Invert(ScaleMatrix);
        }

        // Call this at the start of Drawing in the engine
        public void BeginDraw()
        {
            SizeChanged = false;
            if (m_Dirty)
            {
                UpdateScale();
                ApplySettings();
                m_Dirty = false;
                SizeChanged = true;
            }

            m_PrevWidth = m_Width;
            m_PrevHeight = m_Height;

            // Clear the back buffer
            m_GraphicsManager.GraphicsDevice.Viewport = new Viewport(0, 0, m_Width, m_Height);
            m_GraphicsManager.GraphicsDevice.Clear(Color.Black);
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            Width  = m_Window.ClientBounds.Width;
            Height = m_Window.ClientBounds.Height;
        }
    }
}
