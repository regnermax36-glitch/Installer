using System;
using System.Drawing;

namespace Rectify11Installer.Themes
{
    /// <summary>
    /// Windows 12 2027 Typography System
    /// Provides comprehensive font styles and formatting for the futuristic UI
    /// </summary>
    public static class Windows12_2027_Typography
    {
        /// <summary>
        /// Font styles for different UI elements
        /// </summary>
        public static class Styles
        {
            // Display fonts - largest text
            public static Font DisplayLarge => new Font("Segoe UI Variable Display", 57f, FontStyle.Regular);
            public static Font DisplayMedium => new Font("Segoe UI Variable Display", 45f, FontStyle.Regular);
            public static Font DisplaySmall => new Font("Segoe UI Variable Display", 36f, FontStyle.Regular);

            // Headline fonts - section headers
            public static Font HeadlineLarge => new Font("Segoe UI Variable Display", 32f, FontStyle.Regular);
            public static Font HeadlineMedium => new Font("Segoe UI Variable Display", 28f, FontStyle.Regular);
            public static Font HeadlineSmall => new Font("Segoe UI Variable Display", 24f, FontStyle.Regular);

            // Title fonts - component titles
            public static Font TitleLarge => new Font("Segoe UI Variable Text", 22f, FontStyle.Regular);
            public static Font TitleMedium => new Font("Segoe UI Variable Text", 16f, FontStyle.Medium);
            public static Font TitleSmall => new Font("Segoe UI Variable Text", 14f, FontStyle.Medium);

            // Label fonts - UI labels
            public static Font LabelLarge => new Font("Segoe UI Variable Text", 14f, FontStyle.Medium);
            public static Font LabelMedium => new Font("Segoe UI Variable Text", 12f, FontStyle.Medium);
            public static Font LabelSmall => new Font("Segoe UI Variable Text", 11f, FontStyle.Medium);

            // Body fonts - content text
            public static Font BodyLarge => new Font("Segoe UI Variable Text", 16f, FontStyle.Regular);
            public static Font BodyMedium => new Font("Segoe UI Variable Text", 14f, FontStyle.Regular);
            public static Font BodySmall => new Font("Segoe UI Variable Text", 12f, FontStyle.Regular);
        }

        /// <summary>
        /// Text formatting options
        /// </summary>
        public static class Formatting
        {
            /// <summary>
            /// Center-aligned text format
            /// </summary>
            public static StringFormat CenterFormat
            {
                get
                {
                    var format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    format.Trimming = StringTrimming.EllipsisCharacter;
                    return format;
                }
            }

            /// <summary>
            /// Left-aligned text format
            /// </summary>
            public static StringFormat LeftFormat
            {
                get
                {
                    var format = new StringFormat();
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;
                    format.Trimming = StringTrimming.EllipsisCharacter;
                    return format;
                }
            }

            /// <summary>
            /// Right-aligned text format
            /// </summary>
            public static StringFormat RightFormat
            {
                get
                {
                    var format = new StringFormat();
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Center;
                    format.Trimming = StringTrimming.EllipsisCharacter;
                    return format;
                }
            }
        }

        /// <summary>
        /// Font weights for dynamic styling
        /// </summary>
        public static class Weights
        {
            public const int Light = 300;
            public const int Regular = 400;
            public const int Medium = 500;
            public const int SemiBold = 600;
            public const int Bold = 700;
        }

        /// <summary>
        /// Line heights for consistent spacing
        /// </summary>
        public static class LineHeights
        {
            public const float Tight = 1.2f;
            public const float Normal = 1.4f;
            public const float Relaxed = 1.6f;
            public const float Loose = 1.8f;
        }

        /// <summary>
        /// Letter spacing values for enhanced readability
        /// </summary>
        public static class LetterSpacing
        {
            public const float Tight = -0.5f;
            public const float Normal = 0f;
            public const float Wide = 0.5f;
            public const float ExtraWide = 1f;
        }
    }
}
