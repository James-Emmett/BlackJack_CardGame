using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.IO;
using Microsoft.Xna.Framework;

namespace CardGame
{
    static class SoundManager
    {
        public  const string m_MusicDir = "Sounds\\Music";
        public  const string m_SFXDir   = "Sounds\\SFX";
        private static float m_MasterVolume = 1;
        private static float m_MusicVolume = 1;
        private static float m_SFXVolume = 1;
        private static bool  m_IsInitialised = false;

        private static Dictionary<string, SoundEffect>  m_SoundEffects;
        // I hope to Devs these are streamed and not actually loaded by ContentManager?
        private static Dictionary<string, Song>         m_Songs;
        // I use this so we can pause sound effects in menus etc?
        private static List<SoundEffectInstance>        m_ActiveInstances = new List<SoundEffectInstance>();
        private static Song                             m_CurrentSong = null;

        // Range [0 - 1]
        public static float MasterVolume
        {
            get { return m_MasterVolume; }
            set 
            { 
                m_MasterVolume = MathHelper.Clamp(value, 0.0f, 1.0f);
                MediaPlayer.Volume = m_MusicVolume * m_MasterVolume;
                SoundEffect.MasterVolume = m_SFXVolume * m_MasterVolume;
            }
        }

        // Range [0 - 1]
        public static float MusicVolume
        {
            get {return m_MusicVolume; }
            set { m_MusicVolume = MathHelper.Clamp(value, 0.0f, 1.0f); MediaPlayer.Volume = m_MusicVolume * MasterVolume; } 
        }

        // Range [0 - 1]
        public static float SFXVolume
        {
            get { return m_SFXVolume; }
            set { m_SFXVolume = MathHelper.Clamp(value, 0.0f, 1.0f); SoundEffect.MasterVolume = m_SFXVolume * MasterVolume; }
        }

        public static void Initialize()
        {
            m_Songs = new Dictionary<string, Song>();
            m_SoundEffects = new Dictionary<string, SoundEffect>();

            // Process Top Level Music
            AddSongFromFiles(m_MusicDir);
            // Now load all sound effects
            AddEffectsFromFiles(m_SFXDir);

            m_IsInitialised = true;
        }

        public static void PlaySong(string name, bool loop = true)
        {
            if (m_Songs.ContainsKey(name))
            {
                m_CurrentSong = m_Songs[name];
                MediaPlayer.IsRepeating = loop;
                MediaPlayer.Play(m_CurrentSong);
            }
        }

        public static void PlayEffect(string name)
        {
            if(m_IsInitialised == false || m_SoundEffects == null) { return; }

            if(m_SoundEffects.ContainsKey(name))
            {
                m_SoundEffects[name].Play();
            }
        }

        // Please call SoundEffect.Dispose() too remove the effect!!!!
        public static void PlayEffect(string name, bool looped, out SoundEffectInstance instance)
        {
            instance = null;
            if (m_IsInitialised == false || m_SoundEffects == null) { return; }

            if (m_SoundEffects.ContainsKey(name))
            {
                instance = m_SoundEffects[name].CreateInstance();
                instance.Play();
                instance.IsLooped = looped;

                // We track the sound effect ourselves
                m_ActiveInstances.Add(instance);
            }
        }

        // Pauses all the sound effect instance we currently have playing
        public static void PauseEffects()
        {
            foreach (SoundEffectInstance item in m_ActiveInstances)
            {
                item.Pause();
            }
        }

        public static void ResumeEffects()
        {
            foreach (SoundEffectInstance item in m_ActiveInstances)
            {
                item.Play();
            }
        }

        public static void PauseSong()
        {
            MediaPlayer.Pause();
        }

        public static void ResumeSong()
        {
            MediaPlayer.Resume();
        }

        public static void StopSong()
        {
            MediaPlayer.Stop();
        }

        public static void PauseAllSounds()
        {
            PauseEffects();
            PauseSong();
        }

        public static void ResumeAllSounds()
        {
            ResumeEffects();
            ResumeSong();
        }

        public static void DisposeAll()
        {
            for (int i = 0; i < m_ActiveInstances.Count; i++)
            {
                m_ActiveInstances[i].Dispose();
            }

            m_ActiveInstances.Clear();
        }

        public static void DisposeEffect(SoundEffectInstance instance)
        {
            int index = m_ActiveInstances.FindIndex(a => a.Equals(instance));
            if(index == -1) { instance.Dispose(); return; }

            // Swap the last element with our instance, effectivly rmeoving from list
            m_ActiveInstances[index] = m_ActiveInstances[m_ActiveInstances.Count];
            m_ActiveInstances.RemoveAt(m_ActiveInstances.Count);

            // Finally dispose our instance
            instance.Dispose();
        }

        private static void AddSongFromFiles(string dirPath)
        {
            // Now loop through all sub directories
            DirectoryInfo directory = new DirectoryInfo(Engine.ContentRootDir + dirPath);

            if(directory.Exists == false) { directory.Create(); }

            // Add all the files too the dictionary
            FileInfo[] fileInfo = directory.GetFiles("*.xnb");
            foreach (FileInfo music in fileInfo)
            {
                string path = dirPath + "\\" + Path.GetFileNameWithoutExtension(music.Name);
                m_Songs.Add(path.Substring(m_MusicDir.Length + 1), Engine.Instance.Content.Load<Song>(path));
            }

            // Now do Sub Directories
            DirectoryInfo[] musicSubDirectories = directory.GetDirectories();
            foreach (var item in musicSubDirectories)
            {
                AddSongFromFiles(dirPath + "\\" + item.Name);
            }
        }

        private static void AddEffectsFromFiles(string dirPath)
        {
            // Now loop through all sub directories
            DirectoryInfo directory = new DirectoryInfo(Engine.ContentRootDir + dirPath);
            if (directory.Exists == false) { directory.Create(); }

            // Add all the files too the dictionary
            FileInfo[] fileInfo = directory.GetFiles("*.xnb");
            foreach (FileInfo effects in fileInfo)
            {
                string path = dirPath + "\\" + Path.GetFileNameWithoutExtension(effects.Name);
                m_SoundEffects.Add(path.Substring(m_SFXDir.Length + 1), Engine.Instance.Content.Load<SoundEffect>(path));
            }

            // Now do Sub Directories
            DirectoryInfo[] subDirectories = directory.GetDirectories();
            foreach (var item in subDirectories)
            {
                AddEffectsFromFiles(dirPath + "\\" + item.Name);
            }
        }
    }
}
