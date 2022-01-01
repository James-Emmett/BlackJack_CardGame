using System;

namespace CardGame
{
	struct Range
	{
		private int m_Min;
		private int m_Max;
		private int m_Length;
		private int m_Radius;
		private int m_Centre;

		public Range(int min, int max)
		{
			if (min > max)
			{
				m_Min = max;
				m_Max = min;
			}
			else
			{
				m_Min = min;
				m_Max = max;
			}

			m_Length = (int)MathF.Abs(m_Max - m_Min);
			m_Radius = (int)(m_Length * 0.5f);
			m_Centre = m_Min + m_Radius;
		}

		public int Min(){ return m_Min; }
		public int Max(){ return m_Max; }
		public int Length(){ return m_Length; }
		public int Radius(){ return m_Radius; }
		public int Centre(){ return m_Centre; }
		public bool Contains(int value){ return (value >= m_Min) && (value <= m_Max); }
	};

	struct Rangef
	{
		private float m_Min;
		private float m_Max;
		private float m_Length;
		private float m_Radius;
		private float m_Centre;

		public Rangef(float min, float max)
		{
			if (min > max)
			{
				m_Min = max;
				m_Max = min;
			}
			else
			{
				m_Min = min;
				m_Max = max;
			}

			m_Length = (int)MathF.Abs(m_Max - m_Min);
			m_Radius = m_Length * 0.5f;
			m_Centre = m_Min + m_Radius;
		}

		public float Min(){ return m_Min; }
		public float Max(){ return m_Max; }
		public float Length(){ return m_Length; }
		public float Radius(){ return m_Radius; }
		public float Centre(){ return m_Centre; }
		public bool Contains(float value){ return ((value >= m_Min) && (value <= m_Max)); }
	};
}
