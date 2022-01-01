using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardGame
{
    public abstract class Scene
    {
        public bool IsLoaded { get; protected set; }
        private SceneManager m_SceneManager;
        public SceneManager SceneManager
        {
            get { return m_SceneManager; }
            set { m_SceneManager = value; }
        }

        public string m_Name = "DefaultScene";
        public virtual void OnEnter() { }
        public virtual void OnChanged() { }
        // This is useful if you need to react to window size changed, like camera resize etc.
        public virtual void OnSizeChanged() { }
        // Create ContentManager or use global one, and init your content here.
        public virtual void LoadContent() { IsLoaded = true; }
        // This is basiclaly a shutdown/cleanup function
        public virtual void UnloadContent() { }
        public virtual void Update(float deltaTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
        
        public void ExitScene()
        {
            m_SceneManager.RemoveScene(m_Name);
        }
    }
}
