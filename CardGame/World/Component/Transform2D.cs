using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardGame
{
    public enum Space { Self, Parent, World }
    public class Transform2D
    {
        public readonly Entity m_Entity;
        private List<Transform2D> m_Transforms;
        private Transform2D m_Parent = null;
        private Matrix2D m_World;
        private Matrix2D m_Inverse;
        //--World--
        private Vector2 m_Position;
        private Vector2 m_Scale;
        private float m_Rotation;
        //--Local--
        private Vector2 m_LocalPosition;
        private Vector2 m_LocalScale;
        private float m_LocalRotation;
        private bool m_Dirty;

        //--Dir Vectors--
        private Vector2 m_Forward;
        private Vector2 m_Right;

        public Transform2D Parent
        {
            get { return m_Parent; }
            set
            {
                if (m_Parent == value) { return; }

                if (m_Parent != null)
                {
                    m_Parent.Children.Remove(this);
                }

                // Copy old world first
                Matrix2D world = World;

                // Set the new parent
                m_Parent = value;

                // Only attemt addition if where not being set to parent null.!!!
                if (value != null)
                {
                    m_Parent.Children.Add(this);
                }
                // Get our world location, that way we can translate it too the new parents local space!!!
                Position = world.Translation;
                Rotation = world.Rotation;
                Scale = world.Scale;
                MarkForUpdate();
            }
        }

        public List<Transform2D> Children
        {
            get { return m_Transforms; }
        }

        public Vector2 Forward
        {
            get
            {
                if (m_Dirty)
                {
                    UpdateMatrix();
                }
                return m_Forward;
            }

            set
            {
                if (float.IsNaN(value.X) || float.IsNaN(value.Y))
                {
                    return;
                }

                Rotation = MathF.Atan2(value.Y, value.X) - 1.5708f;
            }
        }

        public Vector2 Right
        {
            get
            {
                if (m_Dirty)
                {
                    UpdateMatrix();
                }
                return m_Right;
            }

            set
            {
                if (float.IsNaN(value.X) || float.IsNaN(value.Y))
                {
                    return;
                }

                Rotation = MathF.Atan2(value.Y, value.X);
            }
        }

        public Vector2 Position
        {
            get
            {
                if (m_Dirty)
                {
                    UpdateMatrix();
                }

                return m_Position;
            }
            set
            {
                if (m_Parent != null)
                {
                    LocalPosition = m_Parent.WorldToLocal.Transform(value);
                }
                else
                {
                    LocalPosition = value;
                }

                MarkForUpdate();
            }
        }

        public Vector2 Scale
        {
            get
            {
                if (m_Dirty)
                {
                    UpdateMatrix();
                }

                return m_Scale;
            }
            set
            {
                if (m_Parent != null)
                {
                    // Just scale by are parents scale?
                    LocalScale = value / m_Parent.m_Scale;
                }
                else
                {
                    LocalScale = value;
                }
            }
        }

        public float Rotation
        {
            get
            {
                if (m_Dirty)
                {
                    UpdateMatrix();
                }
                return m_Rotation;
            }
            set
            {
                if (m_Parent != null)
                {
                    // I think i/we just add rotation as its 2D?
                    // Why am i negating parents rotation?
                    LocalRotation = (m_Parent.Rotation + value);
                }
                else
                {
                    LocalRotation = value;
                }
            }
        }

        public Vector2 LocalPosition
        {
            get { return m_LocalPosition; }
            set
            {
                MarkForUpdate();
                m_LocalPosition = value;
            }
        }

        public Vector2 LocalScale
        {
            get { return m_LocalScale; }
            set
            {
                MarkForUpdate();
                m_LocalScale = value;
            }
        }

        public float LocalRotation
        {
            get { return m_LocalRotation; }
            set
            {
                MarkForUpdate();

                // Keep the rotation value within the 360 range i.e wrap the values!
                m_LocalRotation = MathHelper.WrapAngle(value);
            }
        }

        public Matrix2D World
        {
            get
            {
                if (m_Dirty)
                {
                    UpdateMatrix();
                }
                return m_World;
            }
        }

        public Matrix2D WorldToLocal
        {
            get
            {
                if (m_Dirty)
                {
                    UpdateMatrix();
                }
                return m_Inverse;
            }
        }

        // Simply looks at target
        public void LookAt(Vector2 target)
        {
            Forward = Vector2.Normalize(target - Position);
        }

        public void LookIn(Vector2 direction)
        {
            Forward = Vector2.Normalize(direction);
        }

        public Transform2D(Entity entity)
        {
            m_Entity = entity;
            m_Dirty = true;
            m_Transforms = new List<Transform2D>();
            Position = Vector2.One;
            Scale = Vector2.One;
            Rotation = 0.0f;
        }

        public void Translate(Vector2 delta, Space space)
        {
            switch (space)
            {
                case Space.Self:
                    m_LocalPosition += Matrix2D.CreateRotation(m_LocalRotation).Transform(delta);
                    break;
                case Space.Parent:
                    m_LocalPosition += delta;
                    break;
                case Space.World:
                    if (m_Parent != null)
                    {
                        m_LocalPosition += m_Parent.WorldToLocal.Transform(delta);
                    }
                    else
                    {
                        m_LocalPosition += delta;
                    }
                    break;
            }
            MarkForUpdate();
        }

        public void Rotate(float delta, Space space)
        {
            switch (space)
            {
                case Space.Self:
                case Space.Parent:
                    LocalRotation += delta;
                    break;
                case Space.World:
                    if (m_Parent != null)
                    {
                        LocalRotation += delta;
                    }
                    else
                    {
                        // Some kind of inverse of the parent rotation?
                        LocalRotation += (delta - m_Parent.Rotation);
                    }
                    break;
            }
            MarkForUpdate();
        }


        public Transform2D GetChild(int index)
        {
            if (index > 0 && index < m_Transforms.Count)
            {
                return m_Transforms[index];
            }

            return null;
        }

        private void MarkForUpdate()
        {
            if (m_Dirty == false)
            {
                m_Dirty = true;
                foreach (Transform2D item in m_Transforms)
                {
                    item.MarkForUpdate();
                }
            }
        }

        private void UpdateMatrix()
        {
            // Scale, Rotate then Translate? because its row majour it should work in this order?
            // Reverse polish notation is counteracted?
            Matrix2D localMatrix = Matrix2D.CreateScale(m_LocalScale) *
                                   Matrix2D.CreateRotation(m_LocalRotation) *
                                   Matrix2D.CreateTranslation(m_LocalPosition);

            if (m_Parent == null)
            {
                m_World = localMatrix;
                m_Position = m_LocalPosition;
                m_Rotation = m_LocalRotation;
                m_Scale = m_LocalScale;
            }
            else
            {
                // Get local relative to parent, then construct matrix.
                m_World = localMatrix * Parent.World;
                m_Position = m_World.Translation;
                m_Rotation = MathHelper.WrapAngle(m_Parent.Rotation + m_LocalRotation); // is it an add here?
                m_Scale = m_Parent.Scale * m_LocalScale;
            }

            Matrix2D rot = Matrix2D.CreateRotation(m_Rotation);
            m_Forward = Vector2.Normalize(rot.Transform(new Vector2(0, 1)));
            m_Right = Vector2.Normalize(rot.Transform(new Vector2(1, 0)));
            m_Inverse = Matrix2D.Invert(m_World);
            m_Dirty = false;
        }
    }
}
