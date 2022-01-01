using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardGame
{
    public class SceneManager
    {
        // Currently Active Scene
        private Scene m_ActiveScene;
        private Scene m_DefaultScene;
        public  Scene ActiveScene { get { return m_ActiveScene; } private set { m_ActiveScene = value; } }

        // Note in a real application this can be data driven not hard coded scenes!!
        private Dictionary<string, Scene> m_SceneMap = new Dictionary<string, Scene>();

        public void AddScene(string name, Scene scene, bool makeDefault = false)
        {
            scene.m_Name = name;
            m_SceneMap.Add(name, scene);
            scene.SceneManager = this;

            if (makeDefault) { m_DefaultScene = scene; }
        }

        public void SetDefaultScene(Scene scene)
        {
            m_DefaultScene = scene;
        }

        public void MakedefaultActive()
        {
            LoadScene(m_DefaultScene.m_Name, false);
        }

        public void LoadScene(string name, bool removeCurrent)
        {
            Scene scene;
            if (m_SceneMap.TryGetValue(name, out scene))
            {
                if(scene.IsLoaded == false)
                {
                    scene.LoadContent();
                }

                if (m_ActiveScene != null)
                {
                    if (removeCurrent)
                    {
                        RemoveScene(ActiveScene);
                    }
                    else
                    {
                        m_ActiveScene.OnChanged();
                    }
                }

                m_ActiveScene = scene;
                m_ActiveScene.OnEnter();
            }
        }

        public void RemoveScene(string name)
        {
            m_SceneMap.Remove(name);

            if(m_ActiveScene.m_Name == name)
            {
                m_ActiveScene.UnloadContent();
                m_ActiveScene = null;
            }
        }

        public void RemoveScene(Scene scene)
        {
            RemoveScene(scene.m_Name);
        }

        public void Update(float deltaTime) 
        {
            if (m_ActiveScene != null)
            {
                m_ActiveScene.Update(deltaTime);
            }
        }

        public void OnSizeChanged()
        {
            if(m_ActiveScene != null)
            {
                m_ActiveScene.OnSizeChanged();
            }
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            if(m_ActiveScene != null)
            {
                m_ActiveScene.Draw(spriteBatch);
            }
        }

        public void ShutDown()
        {
            foreach (var scene in m_SceneMap)
            {
                scene.Value.UnloadContent();
            }

            m_SceneMap.Clear();
            m_ActiveScene = null;
        }
    }
}