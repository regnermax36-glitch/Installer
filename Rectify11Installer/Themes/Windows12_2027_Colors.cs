using System;
using System.Drawing;

namespace Rectify11Installer.Themes
{
    /// <summary>
    /// Windows 12 2027 Color System - Advanced color management for futuristic UI
    /// </summary>
    public static class Windows12_2027_Colors
    {
        #region Neural Theme Colors
        public static class Neural
        {
            public static readonly Color Primary = Color.FromArgb(0, 120, 215);
            public static readonly Color Secondary = Color.FromArgb(16, 110, 190);
            public static readonly Color Accent = Color.FromArgb(0, 204, 255);
            public static readonly Color Surface = Color.FromArgb(32, 32, 32);
            public static readonly Color Background = Color.FromArgb(16, 16, 16);
            public static readonly Color Text = Color.FromArgb(255, 255, 255);
            public static readonly Color TextSecondary = Color.FromArgb(200, 200, 200);
        }
        #endregion

        #region Holographic Theme Colors
        public static class Holographic
        {
            public static readonly Color Primary = Color.FromArgb(255, 0, 128);
            public static readonly Color Secondary = Color.FromArgb(128, 0, 255);
            public static readonly Color Accent = Color.FromArgb(0, 255, 255);
            public static readonly Color Surface = Color.FromArgb(24, 24, 24);
            public static readonly Color Background = Color.FromArgb(12, 12, 12);
            public static readonly Color Text = Color.FromArgb(255, 255, 255);
            public static readonly Color TextSecondary = Color.FromArgb(220, 220, 220);
        }
        #endregion

        #region Quantum Theme Colors
        public static class Quantum
        {
            public static readonly Color Primary = Color.FromArgb(0, 255, 127);
            public static readonly Color Secondary = Color.FromArgb(0, 191, 255);
            public static readonly Color Accent = Color.FromArgb(127, 255, 212);
            public static readonly Color Surface = Color.FromArgb(20, 20, 20);
            public static readonly Color Background = Color.FromArgb(8, 8, 8);
            public static readonly Color Text = Color.FromArgb(255, 255, 255);
            public static readonly Color TextSecondary = Color.FromArgb(180, 255, 180);
            
            // Quantum-specific energy colors
            public static readonly Color ENERGY = Color.FromArgb(255, 215, 0);      // Quantum energy glow
            public static readonly Color PARTICLE = Color.FromArgb(138, 43, 226);   // Quantum particle effect
        }
        #endregion

        #region Minimal Theme Colors
        public static class Minimal
        {
            public static readonly Color Primary = Color.FromArgb(96, 96, 96);
            public static readonly Color Secondary = Color.FromArgb(128, 128, 128);
            public static readonly Color Accent = Color.FromArgb(64, 64, 64);
            public static readonly Color Surface = Color.FromArgb(248, 248, 248);
            public static readonly Color Background = Color.FromArgb(255, 255, 255);
            public static readonly Color Text = Color.FromArgb(32, 32, 32);
            public static readonly Color TextSecondary = Color.FromArgb(96, 96, 96);
        }
        #endregion

        #region Adaptive Color System
        public static class Adaptive
        {
            /// <summary>
            /// Gets an adaptive color based on the theme variant and color name
            /// </summary>
            public static Color GetAdaptiveColor(string colorName, ThemeVariant variant)
            {
                return variant switch
                {
                    ThemeVariant.Neural => GetNeuralColor(colorName),
                    ThemeVariant.Holographic => GetHolographicColor(colorName),
                    ThemeVariant.Quantum => GetQuantumColor(colorName),
                    ThemeVariant.Minimal => GetMinimalColor(colorName),
                    _ => Neural.Primary
                };
            }

            private static Color GetNeuralColor(string colorName)
            {
                return colorName.ToLower() switch
                {
                    "primary" => Neural.Primary,
                    "secondary" => Neural.Secondary,
                    "accent" => Neural.Accent,
                    "surface" => Neural.Surface,
                    "background" => Neural.Background,
                    "text" => Neural.Text,
                    "textsecondary" => Neural.TextSecondary,
                    _ => Neural.Primary
                };
            }

            private static Color GetHolographicColor(string colorName)
            {
                return colorName.ToLower() switch
                {
                    "primary" => Holographic.Primary,
                    "secondary" => Holographic.Secondary,
                    "accent" => Holographic.Accent,
                    "surface" => Holographic.Surface,
                    "background" => Holographic.Background,
                    "text" => Holographic.Text,
                    "textsecondary" => Holographic.TextSecondary,
                    _ => Holographic.Primary
                };
            }

            private static Color GetQuantumColor(string colorName)
            {
                return colorName.ToLower() switch
                {
                    "primary" => Quantum.Primary,
                    "secondary" => Quantum.Secondary,
                    "accent" => Quantum.Accent,
                    "surface" => Quantum.Surface,
                    "background" => Quantum.Background,
                    "text" => Quantum.Text,
                    "textsecondary" => Quantum.TextSecondary,
                    _ => Quantum.Primary
                };
            }

            private static Color GetMinimalColor(string colorName)
            {
                return colorName.ToLower() switch
                {
                    "primary" => Minimal.Primary,
                    "secondary" => Minimal.Secondary,
                    "accent" => Minimal.Accent,
                    "surface" => Minimal.Surface,
                    "background" => Minimal.Background,
                    "text" => Minimal.Text,
                    "textsecondary" => Minimal.TextSecondary,
                    _ => Minimal.Primary
                };
            }
        }
        #endregion

        #region Color Utilities

        /// <summary>
        /// Lightens a color by the specified amount
        /// </summary>
        public static Color Lighten(Color color, float amount)
        {
            amount = Math.Max(0f, Math.Min(1f, amount));
            return Color.FromArgb(
                color.A,
                Math.Min(255, (int)(color.R + (255 - color.R) * amount)),
                Math.Min(255, (int)(color.G + (255 - color.G) * amount)),
                Math.Min(255, (int)(color.B + (255 - color.B) * amount))
            );
        }

        /// <summary>
        /// Darkens a color by the specified amount
        /// </summary>
        public static Color Darken(Color color, float amount)
        {
            amount = Math.Max(0f, Math.Min(1f, amount));
            return Color.FromArgb(
                color.A,
                (int)(color.R * (1f - amount)),
                (int)(color.G * (1f - amount)),
                (int)(color.B * (1f - amount))
            );
        }

        #endregion
    }
}
