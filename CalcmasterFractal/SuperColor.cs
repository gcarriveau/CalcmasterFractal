using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcmasterFractal
{
    public class SuperColor
    {
        public SuperColor()
        {
            m_R = 128;
            m_G = 128;
            m_B = 128;
            UpdateHSV();
        }

        public SuperColor(Color c)
        {
            m_R = c.R;
            m_G = c.G;
            m_B = c.B;
            UpdateHSV();
        }

        public SuperColor Clone()
        {
            return new SuperColor(this.Color);
        }

        public Color Color
        {
            get
            {
                return (Color.FromArgb(m_R, m_G, m_B));
            }
        }

        byte m_R;
        /// <summary>
        /// Red value (0 to 255)
        /// </summary>
        public byte R
        {
            get
            {
                return (m_R);
            }
            set
            {
                m_R = value;
                UpdateHSV();
            }
        }

        private byte m_G;
        /// <summary>
        /// Green value (0 to 255)
        /// </summary>
        public byte G
        {
            get
            {
                return m_G;
            }
            set
            {
                m_G = value;
                UpdateHSV();
            }
        }

        private byte m_B;
        /// <summary>
        /// Blue value (0 to 255)
        /// </summary>
        public byte B
        {
            get
            {
                return m_B;
            }
            set
            {
                m_B = value;
                UpdateHSV();
            }
        }

        private double m_H = 0.0;

        /// <summary>
        /// Hue (0.0 to 360.0 degrees) <br />
        /// It can be assigned a value outside of that range and will be adjusted accordingly.
        /// </summary>
        public double H
        {
            get
            {
                return (m_H);
            }
            set
            {
                // Hue is circular (degree from 0 to 360)
                int degrees = (int)Math.Floor(value);
                double fracpart = value - (double)degrees;
                degrees = degrees % 360;
                if (degrees < 0) degrees += 360;
                m_H = (double)degrees + fracpart;
                UpdateRGB();
            }
        }

        private double m_S = 0;

        /// <summary>
        /// Saturation (from 0.0 to 1.0)
        /// </summary>
        public double S
        {
            get
            {
                return (m_S);
            }
            set
            {
                if (value >= 0.0d && value <= 1.0d)
                {
                    m_S = value;
                    UpdateRGB();
                }
            }
        }

        private double m_V = 0;

        /// <summary>
        /// Value (totally black 0.0 to 1.0 full color)
        /// </summary>
        public double V
        {
            get
            {
                return (m_V);
            }
            set
            {
                if (value >= 0.0d && value <= 1.0d)
                {
                    m_V = value;
                    UpdateRGB();
                }
            }
        }

        private void UpdateHSV()
        {
            int max = Math.Max(m_R, Math.Max(m_G, m_B));
            int min = Math.Min(m_R, Math.Min(m_G, m_B));

            m_H = (double)this.Color.GetHue();
            m_S = (max == 0) ? 0 : 1d - (1d * min / max);
            m_V = max / 255d;
        }

        private void UpdateRGB()
        {
            int hi = Convert.ToInt32(Math.Floor(m_H / 60d)) % 6;
            double f = m_H / 60d - Math.Floor(m_H / 60d);
            double value = m_V * 255d;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1d - m_S));
            int q = Convert.ToInt32(value * (1d - f * m_S));
            int t = Convert.ToInt32(value * (1d - (1d - f) * m_S));
            if (hi == 0)
            {
                m_R = Convert.ToByte(v);
                m_G = Convert.ToByte(t);
                m_B = Convert.ToByte(p);
            }
            else if (hi == 1)
            {
                m_R = Convert.ToByte(q);
                m_G = Convert.ToByte(v);
                m_B = Convert.ToByte(p);
            }
            else if (hi == 2)
            {
                m_R = Convert.ToByte(p);
                m_G = Convert.ToByte(v);
                m_B = Convert.ToByte(t);
            }
            else if (hi == 3)
            {
                m_R = Convert.ToByte(p);
                m_G = Convert.ToByte(q);
                m_B = Convert.ToByte(v);
            }
            else if (hi == 4)
            {
                m_R = Convert.ToByte(t);
                m_G = Convert.ToByte(p);
                m_B = Convert.ToByte(v);
            }
            else
            {
                m_R = Convert.ToByte(v);
                m_G = Convert.ToByte(p);
                m_B = Convert.ToByte(q);
            }
        }
    }
}
