using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CardGame
{
    public class Engine : Game
    {
        // Singleton engine so only 1 copy.!!!
        public static Engine Instance;

        // Main thread Random instance
        public static readonly Random Random = new Random();
        public static readonly string ContentRootDir = "Content\\";

        private GraphicsDeviceManager   m_GraphicsManager;
        private GraphicsDevice          m_GraphicsDevice;
        private SpriteBatch             m_SpriteBatch;
        private DisplayManager          m_DisplayManager;
        public SceneManager             SceneManager { get; private set; }

        private string                  m_Title = "DungeonOfRogue";
        public Color                    m_ClearColor = Color.CornflowerBlue;
        public float                    m_TimeScale = 1.0f;
        public float                    DeltaTime { get; private set; }
        public float                    RawDeltaTime { get; private set; }

        public Engine(int width, int height, string title)
        {
            // Ensure only one Instance exists as this is a singleton
            if (Instance != null) { return; }
            Instance = this;

            // Set the directory where are assets will be loaded from
            Content.RootDirectory   = "Content";
            m_GraphicsManager       = new GraphicsDeviceManager(this);
            m_DisplayManager        = new DisplayManager(m_GraphicsManager, width, height, 0, 0);
            SceneManager            = new SceneManager();

            m_GraphicsManager.SynchronizeWithVerticalRetrace = true;
            m_Title = title;
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        public string Title
        {
            get { return m_Title; }
        }

        public int Width
        {
            get { return m_DisplayManager.Width; }
            set { m_DisplayManager.Width = value; }
        }

        public int Height
        {
            get { return m_DisplayManager.Height; }
            set { m_DisplayManager.Height = value; }
        }

        public GraphicsDeviceManager GraphicsManager
        {
            get { return m_GraphicsManager; }
        }

        public DisplayManager DisplayManager
        {
            get { return m_DisplayManager; }
        }

        public GraphicsDevice Device
        {
            get { return m_GraphicsDevice; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return m_SpriteBatch; }
        }

        public Color ClearColor
        {
            get { return m_ClearColor; }
            set { m_ClearColor = value; }
        }

        protected override void OnActivated(object sender, EventArgs args)
        {
            base.OnActivated(sender, args);
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            base.OnDeactivated(sender, args);
        }

        protected override void Initialize()
        {
            SoundManager.Initialize();
            Input.Initialise();
            m_GraphicsDevice = m_GraphicsManager.GraphicsDevice;
            Debug.Initialize(Device);
            base.Initialize();

            // Load defualt scene
            SceneManager.MakedefaultActive();
        }

        protected override void LoadContent()
        {
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            // Get the raw elapsed delta time and the scaled delta time
            RawDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            DeltaTime = RawDeltaTime * m_TimeScale;
            Input.Update();

            SceneManager.Update(RawDeltaTime);

            if (Input.Keyboard.IsKeyDown(Keys.Up))
            {
                SoundManager.MasterVolume += 0.1f;
            }
            else if (Input.Keyboard.IsKeyDown(Keys.Down))
            {
                SoundManager.MasterVolume -= 0.1f;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Debug.Begin();
            // Display manager handles clearing backbuffer for us.
            m_DisplayManager.BeginDraw();

            if(m_DisplayManager.SizeChanged)
            {
                SceneManager.OnSizeChanged();
            }

            // TODO: Add your drawing code here
            SceneManager.Draw(m_SpriteBatch);

            Debug.End();
            base.Draw(gameTime);
        }
    }
}
