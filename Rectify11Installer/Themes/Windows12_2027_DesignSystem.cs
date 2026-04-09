using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Rectify11Installer.Themes
{
    /// <summary>
    /// Windows 12 2027 Design System - Core design utilities and graphics extensions
    /// </summary>
    public static class Windows12_2027_DesignSystem
    {
        #region Gradient Creation Methods

        /// <summary>
        /// Creates a neural-themed gradient brush
        /// </summary>
        public static LinearGradientBrush CreateNeuralGradient(Rectangle bounds)
        {
            var brush = new LinearGradientBrush(bounds, 
                Windows12_2027_Colors.Neural.Primary, 
                Windows12_2027_Colors.Neural.Secondary, 
                LinearGradientMode.Vertical);
            
            var blend = new ColorBlend();
            blend.Colors = new Color[]
            {
                Color.FromArgb(30, Windows12_2027_Colors.Neural.Primary),
                Color.FromArgb(60, Windows12_2027_Colors.Neural.Accent),
                Color.FromArgb(20, Windows12_2027_Colors.Neural.Secondary)
            };
            blend.Positions = new float[] { 0f, 0.5f, 1f };
            brush.InterpolationColors = blend;
            
            return brush;
        }

        /// <summary>
        /// Creates a holographic-themed gradient brush
        /// </summary>
        public static LinearGradientBrush CreateHolographicGradient(Rectangle bounds)
        {
            var brush = new LinearGradientBrush(bounds, 
                Windows12_2027_Colors.Holographic.Primary, 
                Windows12_2027_Colors.Holographic.Secondary, 
                LinearGradientMode.ForwardDiagonal);
            
            var blend = new ColorBlend();
            blend.Colors = new Color[]
            {
                Color.FromArgb(40, Windows12_2027_Colors.Holographic.Primary),
                Color.FromArgb(80, Windows12_2027_Colors.Holographic.Accent),
                Color.FromArgb(60, Windows12_2027_Colors.Holographic.Secondary),
                Color.FromArgb(30, Windows12_2027_Colors.Holographic.Primary)
            };
            blend.Positions = new float[] { 0f, 0.3f, 0.7f, 1f };
            brush.InterpolationColors = blend;
            
            return brush;
        }

        /// <summary>
        /// Creates a quantum-themed gradient brush
        /// </summary>
        public static LinearGradientBrush CreateQuantumGradient(Rectangle bounds)
        {
            var brush = new LinearGradientBrush(bounds, 
                Windows12_2027_Colors.Quantum.Primary, 
                Windows12_2027_Colors.Quantum.Secondary, 
                LinearGradientMode.BackwardDiagonal);
            
            var blend = new ColorBlend();
            blend.Colors = new Color[]
            {
                Color.FromArgb(25, Windows12_2027_Colors.Quantum.Primary),
                Color.FromArgb(70, Windows12_2027_Colors.Quantum.Accent),
                Color.FromArgb(35, Windows12_2027_Colors.Quantum.Secondary)
            };
            blend.Positions = new float[] { 0f, 0.6f, 1f };
            brush.InterpolationColors = blend;
            
            return brush;
        }

        #endregion



        #region Animation Easing Functions

        /// <summary>
        /// Easing functions for smooth animations
        /// </summary>
        public static class Easing
        {
            public static float EaseInOut(float t)
            {
                return t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t;
            }

            public static float EaseOut(float t)
            {
                return 1 - (1 - t) * (1 - t);
            }

            public static float EaseIn(float t)
            {
                return t * t;
            }

            public static float Bounce(float t)
            {
                if (t < 1 / 2.75f)
                {
                    return 7.5625f * t * t;
                }
                else if (t < 2 / 2.75f)
                {
                    return 7.5625f * (t -= 1.5f / 2.75f) * t + 0.75f;
                }
                else if (t < 2.5 / 2.75)
                {
                    return 7.5625f * (t -= 2.25f / 2.75f) * t + 0.9375f;
                }
                else
                {
                    return 7.5625f * (t -= 2.625f / 2.75f) * t + 0.984375f;
                }
            }
        }

        #endregion

        #region Typography

        /// <summary>
        /// Typography system for Windows 12 2027
        /// </summary>
        public static class Typography
        {
            public static readonly Font HeaderFont = new Font("Segoe UI Variable", 24f, FontStyle.Bold);
            public static readonly Font SubHeaderFont = new Font("Segoe UI Variable", 18f, FontStyle.Regular);
            public static readonly Font BodyFont = new Font("Segoe UI Variable", 14f, FontStyle.Regular);
            public static readonly Font CaptionFont = new Font("Segoe UI Variable", 12f, FontStyle.Regular);
            public static readonly Font ButtonFont = new Font("Segoe UI Variable", 14f, FontStyle.Regular);
        }

        #endregion

        #region Spacing System

        /// <summary>
        /// Consistent spacing system based on 8px grid
        /// </summary>
        public static class Spacing
        {
            public const int XSmall = 4;
            public const int Small = 8;
            public const int Medium = 16;
            public const int Large = 24;
            public const int XLarge = 32;
            public const int XXLarge = 48;
        }

        #endregion

        #region Border Radius System

        /// <summary>
        /// Consistent border radius system
        /// </summary>
        public static class BorderRadius
        {
            public const int SM = 4;
            public const int MD = 8;
            public const int LG = 12;
            public const int XL = 16;
            public const int XXL = 24;
        }

        #endregion
    }
}

namespace Rectify11Installer.Themes
{
    /// <summary>
    /// Graphics extension methods for Windows 12 2027 UI
    /// </summary>
    public static class GraphicsExtensions
    {
        /// <summary>
        /// Fills a rounded rectangle
        /// </summary>
        public static void FillRoundedRectangle(this Graphics graphics, Brush brush, Rectangle bounds, int radius)
        {
            using (var path = CreateRoundedRectanglePath(bounds, radius))
            {
                graphics.FillPath(brush, path);
            }
        }

        /// <summary>
        /// Draws a rounded rectangle outline
        /// </summary>
        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, Rectangle bounds, int radius)
        {
            using (var path = CreateRoundedRectanglePath(bounds, radius))
            {
                graphics.DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Creates a graphics path for a rounded rectangle
        /// </summary>
        public static GraphicsPath CreateRoundedRectanglePath(Rectangle bounds, int radius)
        {
            var path = new GraphicsPath();
            
            if (radius <= 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            int diameter = radius * 2;
            var arc = new Rectangle(bounds.Location, new Size(diameter, diameter));

            // Top left arc
            path.AddArc(arc, 180, 90);

            // Top right arc
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // Bottom right arc
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // Bottom left arc
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }
    }
}
