using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace CardGame
{
    public enum SpriteOrigin { TopLeft, BottomLeft, Centre }
    public struct Frame
    {
        public Rectangle m_Source;
        public Vector2 m_Origin;

        public Frame(Rectangle source, Vector2 origin)
        {
            m_Source = source;
            m_Origin = origin;
        }
    }

    public class Animation
    {
        List<Frame> m_Frames = null;
        string m_Name = "";
        float m_Delay = 0.0f;
        bool m_Loop = false;

        public Animation(string name, float delay, bool looped, List<Frame> frames = null)
        {
            m_Name = name;
            m_Delay = delay;
            m_Loop = looped;
            m_Frames = frames;
        }

        public string GetName()
        {
            return m_Name;
        }

        public bool IsLooped()
        {
            return m_Loop;
        }

        public float Delay()
        {
            return m_Delay;
        }

        public void AddFrame(Frame frame)
        {
            m_Frames.Add(frame);
        }

        public Frame GetFrame(int frame)
        {
            if (frame >= 0 && frame < m_Frames.Count)
            {
                return m_Frames[frame];
            }

            return new Frame();
        }

        public int FrameCount()
        {
            return m_Frames.Count;
        }
    }

    public class Sprite
    {
        private Dictionary<string, Animation> m_Animations = null;
        private Texture2D       m_Texture;
        private Animation       m_CurrentAnimation = null;
        private Frame           m_CurrentFrame;
        public  SpriteOrigin    m_SpriteOrigin;
        private SpriteEffects   m_SpriteEffect = SpriteEffects.None;
        private float           m_FrameTimer = 0.0f;    // Timer for each frame being played
        private int             m_FrameIndex = 0;       // Current frame index
        public Color            m_Color;
        public float            m_Speed = 1.0f;
        private bool            m_IsAnimating = false;
        public bool             m_UseRawDeltaTime = false;

        // Events that are triggerd during animating sprites, register to recive them.
        public Action<string> OnFinish = null;
        public Action<string> OnLoop = null;
        public Action<string> OnAnimate = null;

        public Sprite(Texture2D texture, Rectangle source = new Rectangle(), SpriteOrigin origin = SpriteOrigin.Centre)
        {
            m_SpriteOrigin = origin;
            m_Texture = texture;

            if (source.IsEmpty)
            {
                source.X = 0;
                source.Y = 0;
                source.Width = m_Texture.Width;
                source.Height = m_Texture.Height;
            }

            m_CurrentFrame = new Frame(source, CalculateOrigin(source, origin));
            m_Color = Color.White;
            m_CurrentAnimation = null;
        }

        public Sprite(Texture2D texture, Rectangle source, Vector2 origin)
        {
            m_Texture = texture;

            if (source.IsEmpty)
            {
                source.X = 0;
                source.Y = 0;
                source.Width = m_Texture.Width;
                source.Height = m_Texture.Height;
            }

            m_CurrentFrame = new Frame(source, origin);
            m_Color = Color.White;
            m_CurrentAnimation = null;
        }

        public bool FlipX
        {
            get { return (m_SpriteEffect & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally; }
            set
            {
                // & with ~ compliment to turn off the bit for FlipHorizontal
                m_SpriteEffect = value ? (m_SpriteEffect | SpriteEffects.FlipHorizontally) : (m_SpriteEffect & ~SpriteEffects.FlipHorizontally);
            }
        }

        public bool FlipY
        {
            get { return (m_SpriteEffect & SpriteEffects.FlipVertically) == SpriteEffects.FlipVertically; }
            set
            {
                // & with ~ compliment to turn off the bit for FlipVertically
                m_SpriteEffect = value ? (m_SpriteEffect | SpriteEffects.FlipVertically) : (m_SpriteEffect & ~SpriteEffects.FlipVertically);
            }
        }

        public void AddAnimation(Animation data)
        {
            if(m_Animations == null) { m_Animations = new Dictionary<string, Animation>(); }
            m_Animations.Add(data.GetName(), data);
        }

        public void ClearAnimations()
        {
            m_Animations.Clear();
            m_CurrentAnimation = null;
            m_FrameTimer = 0.0f;
            m_FrameIndex = 0;
        }

        public bool IsPlaying(string id)
        {
            if (m_CurrentAnimation == null) { return false; }
            return (m_IsAnimating && m_CurrentAnimation.GetName() == id);
        }

        public void Play(string id, bool restart = false)
        {
            if (IsPlaying(id) == false || restart == true)
            {
                if (m_Animations.TryGetValue(id, out m_CurrentAnimation) == false)
                {
                    throw new Exception($"Animation: {id} Not Found for Sprite!");
                }

                // Set data for the animation where playing
                m_FrameIndex = 0;
                m_FrameTimer = 0.0f;
                m_IsAnimating = true;
                m_CurrentFrame = m_CurrentAnimation.GetFrame(m_FrameIndex);
            }
        }

        public void Reverse(string id, bool restart = false)
        {
            Play(id, restart);
            if (m_Speed > 0)
            {
                m_Speed *= -1;
            }
        }

        public void Stop()
        {
            m_IsAnimating = false;
        }

        public void Update()
        {
            if (m_IsAnimating && m_CurrentAnimation != null)
            {
                if (m_UseRawDeltaTime)
                {
                    m_FrameTimer += Engine.Instance.RawDeltaTime * m_Speed;
                }
                else
                {
                    m_FrameTimer += Engine.Instance.DeltaTime * m_Speed;
                }

                if (Math.Abs(m_FrameTimer) >= m_CurrentAnimation.Delay())
                {
                    // Increment or decrement depending on the frame timer direction
                    m_FrameIndex += Math.Sign(m_FrameTimer);
                    if (m_FrameIndex < 0 || m_FrameIndex >= m_CurrentAnimation.FrameCount())
                    {
                        if (m_CurrentAnimation.IsLooped())
                        {
                            m_FrameIndex = 0;
                            OnAnimate?.Invoke(m_CurrentAnimation.GetName());
                            OnLoop?.Invoke(m_CurrentAnimation.GetName());
                        }
                        else
                        {
                            m_FrameIndex = 0;
                            m_IsAnimating = false;
                            OnFinish?.Invoke(m_CurrentAnimation.GetName());
                        }
                    }

                    m_CurrentFrame = m_CurrentAnimation.GetFrame(m_FrameIndex);
                    m_FrameTimer = 0.0f;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float rotation = 0, float scale = 1)
        {
            spriteBatch.Draw(m_Texture, position, m_CurrentFrame.m_Source, m_Color, rotation, m_CurrentFrame.m_Origin, scale, m_SpriteEffect, 0);
        }

        private Vector2 CalculateOrigin(Rectangle source, SpriteOrigin origin)
        {
            if (origin == SpriteOrigin.Centre)
            {
                return new Vector2(source.Width * 0.5f, source.Height * 0.5f);
            }
            else if (origin == SpriteOrigin.BottomLeft)
            {
                return new Vector2(0.0f, source.Height);
            }

            return Vector2.Zero;
        }

        public Rectangle CalcualteBounds(Vector2 position)
        {
            return new Rectangle((int)(position.X - m_CurrentFrame.m_Origin.X), (int)(position.Y - m_CurrentFrame.m_Origin.Y), 
                                                                m_CurrentFrame.m_Source.Width, m_CurrentFrame.m_Source.Height);
        }
    }
}