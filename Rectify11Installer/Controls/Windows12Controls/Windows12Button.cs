using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Rectify11Installer.Themes;

namespace Rectify11Installer.Controls.Windows12Controls
{
    /// <summary>
    /// Windows12Button - A futuristic button with advanced visual effects
    /// Features morphing animations, glow effects, and adaptive theming
    /// </summary>
    public class Windows12Button : Button
    {
        #region Private Fields

        private ThemeVariant _themeVariant = ThemeVariant.Neural;
        private Timer _animationTimer;
        private float _hoverProgress = 0f;
        private float _pressProgress = 0f;
        private float _glowIntensity = 0f;
        private bool _isHovered = false;
        private bool _isPressed = false;
        private bool _showGlow = true;
        private bool _showRipple = true;
        private PointF _rippleCenter = PointF.Empty;
        private float _rippleRadius = 0f;
        private bool _isRippling = false;
        private ButtonStyle _buttonStyle = ButtonStyle.Primary;
        private int _borderRadius = 8;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the theme variant
        /// </summary>
        public ThemeVariant ThemeVariant
        {
            get => _themeVariant;
            set
            {
                if (_themeVariant != value)
                {
                    _themeVariant = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the button style
        /// </summary>
        public ButtonStyle ButtonStyle
        {
            get => _buttonStyle;
            set
            {
                if (_buttonStyle != value)
                {
                    _buttonStyle = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether to show glow effects
        /// </summary>
        public bool ShowGlow
        {
            get => _showGlow;
            set
            {
                if (_showGlow != value)
                {
                    _showGlow = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether to show ripple effects
        /// </summary>
        public bool ShowRipple
        {
            get => _showRipple;
            set
            {
                _showRipple = value;
            }
        }

        /// <summary>
        /// Gets or sets the border radius
        /// </summary>
        public int BorderRadius
        {
            get => _borderRadius;
            set
            {
                if (_borderRadius != value && value >= 0)
                {
                    _borderRadius = value;
                    Invalidate();
                }
            }
        }

        #endregion

        #region Constructor

        public Windows12Button()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            ForeColor = Windows12_2027_Colors.Adaptive.GetAdaptiveColor("text", _themeVariant);
            Font = Windows12_2027_Typography.Styles.LabelLarge;
            Size = new Size(120, 40);
            
            InitializeAnimation();
        }

        #endregion

        #region Animation

        private void InitializeAnimation()
        {
            _animationTimer = new Timer();
            _animationTimer.Interval = 16; // ~60 FPS
            _animationTimer.Tick += AnimationTimer_Tick;
            _animationTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            bool needsRedraw = false;

            // Hover animation
            float targetHover = _isHovered ? 1f : 0f;
            if (Math.Abs(_hoverProgress - targetHover) > 0.01f)
            {
                _hoverProgress += (targetHover - _hoverProgress) * 0.15f;
                needsRedraw = true;
            }

            // Press animation
            float targetPress = _isPressed ? 1f : 0f;
            if (Math.Abs(_pressProgress - targetPress) > 0.01f)
            {
                _pressProgress += (targetPress - _pressProgress) * 0.25f;
                needsRedraw = true;
            }

            // Glow animation
            float targetGlow = (_isHovered || _isPressed) ? 1f : 0f;
            if (Math.Abs(_glowIntensity - targetGlow) > 0.01f)
            {
                _glowIntensity += (targetGlow - _glowIntensity) * 0.1f;
                needsRedraw = true;
            }

            // Ripple animation
            if (_isRippling)
            {
                _rippleRadius += 8f;
                if (_rippleRadius > Math.Max(Width, Height) * 1.5f)
                {
                    _isRippling = false;
                    _rippleRadius = 0f;
                }
                needsRedraw = true;
            }

            if (needsRedraw)
                Invalidate();
        }

        #endregion

        #region Event Handlers

        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovered = true;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovered = false;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            _isPressed = true;
            
            if (_showRipple)
            {
                _rippleCenter = e.Location;
                _rippleRadius = 0f;
                _isRippling = true;
            }
            
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _isPressed = false;
            base.OnMouseUp(e);
        }

        #endregion

        #region Painting

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.CompositingQuality = CompositingQuality.HighQuality;

            var bounds = ClientRectangle;
            
            // Apply press transform
            if (_pressProgress > 0)
            {
                float scale = 1f - (_pressProgress * 0.05f);
                g.ScaleTransform(scale, scale);
                
                int offsetX = (int)((bounds.Width * (1f - scale)) / 2f);
                int offsetY = (int)((bounds.Height * (1f - scale)) / 2f);
                g.TranslateTransform(offsetX, offsetY);
            }

            // Draw glow effect
            if (_showGlow && _glowIntensity > 0)
                DrawGlowEffect(g, bounds);

            // Draw button background
            DrawBackground(g, bounds);

            // Draw ripple effect
            if (_showRipple && _isRippling)
                DrawRippleEffect(g, bounds);

            // Draw border
            DrawBorder(g, bounds);

            // Draw text
            DrawText(g, bounds);
        }

        private void DrawGlowEffect(Graphics g, Rectangle bounds)
        {
            Color glowColor = GetButtonColor("accent");
            int maxGlowSize = 20;
            int glowSize = (int)(maxGlowSize * _glowIntensity);

            for (int i = glowSize; i > 0; i--)
            {
                int alpha = (int)(255 * (glowSize - i) / glowSize * _glowIntensity * 0.1f);
                using (var pen = new Pen(Color.FromArgb(alpha, glowColor), 1))
                {
                    var glowBounds = new Rectangle(
                        bounds.X - i,
                        bounds.Y - i,
                        bounds.Width + i * 2,
                        bounds.Height + i * 2
                    );
                    GraphicsExtensions.DrawRoundedRectangle(g, pen, glowBounds, _borderRadius + i);
                }
            }
        }

        private void DrawBackground(Graphics g, Rectangle bounds)
        {
            Color baseColor = GetButtonColor("background");
            Color hoverColor = GetButtonColor("hover");
            
            // Interpolate colors based on hover progress
            Color currentColor = InterpolateColor(baseColor, hoverColor, _hoverProgress);
            
            // Create gradient for depth
            using (var gradient = new LinearGradientBrush(
                bounds,
                Windows12_2027_Colors.Lighten(currentColor, 0.1f),
                Windows12_2027_Colors.Darken(currentColor, 0.1f),
                LinearGradientMode.Vertical))
            {
                GraphicsExtensions.FillRoundedRectangle(g, gradient, bounds, _borderRadius);
            }

            // Add glassmorphism overlay
            if (_buttonStyle == ButtonStyle.Glass)
            {
                using (var overlay = new SolidBrush(Color.FromArgb(30, Color.White)))
                {
                    GraphicsExtensions.FillRoundedRectangle(g, overlay, bounds, _borderRadius);
                }
            }

            // Add neural pattern for neural theme
            if (_themeVariant == ThemeVariant.Neural && _buttonStyle == ButtonStyle.Primary)
            {
                DrawNeuralPattern(g, bounds);
            }
        }

        private void DrawNeuralPattern(Graphics g, Rectangle bounds)
        {
            // Subtle neural network pattern
            using (var pen = new Pen(Color.FromArgb(20, Color.White), 1))
            {
                int nodeCount = 3;
                var nodes = new PointF[nodeCount];
                
                // Generate nodes
                for (int i = 0; i < nodeCount; i++)
                {
                    nodes[i] = new PointF(
                        bounds.X + bounds.Width * (0.2f + i * 0.3f),
                        bounds.Y + bounds.Height * (0.3f + (i % 2) * 0.4f)
                    );
                }

                // Draw connections
                for (int i = 0; i < nodeCount - 1; i++)
                {
                    g.DrawLine(pen, nodes[i], nodes[i + 1]);
                }

                // Draw nodes
                foreach (var node in nodes)
                {
                    g.FillEllipse(Brushes.White, node.X - 1, node.Y - 1, 2, 2);
                }
            }
        }

        private void DrawRippleEffect(Graphics g, Rectangle bounds)
        {
            if (_rippleRadius <= 0) return;

            // Create clipping path
            using (var path = GraphicsExtensions.CreateRoundedRectanglePath(bounds, _borderRadius))
            {
                var oldClip = g.Clip;
                g.SetClip(path);

                // Draw ripple
                float alpha = 1f - (_rippleRadius / (Math.Max(Width, Height) * 1.5f));
                alpha = Math.Max(0f, alpha);
                
                Color rippleColor = GetButtonColor("accent");
                using (var brush = new SolidBrush(Color.FromArgb((int)(100 * alpha), rippleColor)))
                {
                    g.FillEllipse(brush,
                        _rippleCenter.X - _rippleRadius,
                        _rippleCenter.Y - _rippleRadius,
                        _rippleRadius * 2,
                        _rippleRadius * 2);
                }

                g.Clip = oldClip;
            }
        }

        private void DrawBorder(Graphics g, Rectangle bounds)
        {
            Color borderColor = GetButtonColor("border");
            
            // Enhance border on hover
            if (_hoverProgress > 0)
            {
                Color accentColor = GetButtonColor("accent");
                borderColor = InterpolateColor(borderColor, accentColor, _hoverProgress);
            }

            using (var pen = new Pen(borderColor, 1))
            {
                GraphicsExtensions.DrawRoundedRectangle(g, pen, bounds, _borderRadius);
            }
        }

        private void DrawText(Graphics g, Rectangle bounds)
        {
            if (string.IsNullOrEmpty(Text)) return;

            Color textColor = ForeColor;
            
            // Apply hover effect to text
            if (_hoverProgress > 0 && _buttonStyle == ButtonStyle.Ghost)
            {
                Color accentColor = GetButtonColor("accent");
                textColor = InterpolateColor(textColor, accentColor, _hoverProgress);
            }

            using (var brush = new SolidBrush(textColor))
            using (var format = Windows12_2027_Typography.Formatting.CenterFormat)
            {
                // Text shadow for better readability on colored backgrounds
                if (_buttonStyle == ButtonStyle.Primary)
                {
                    using (var shadowBrush = new SolidBrush(Color.FromArgb(100, Color.Black)))
                    {
                        var shadowBounds = new Rectangle(bounds.X + 1, bounds.Y + 1, bounds.Width, bounds.Height);
                        g.DrawString(Text, Font, shadowBrush, shadowBounds, format);
                    }
                }

                g.DrawString(Text, Font, brush, bounds, format);
            }
        }

        #endregion

        #region Helper Methods

        private Color GetButtonColor(string colorType)
        {
            switch (_buttonStyle)
            {
                case ButtonStyle.Primary:
                    switch (colorType)
                    {
                        case "background": return Windows12_2027_Colors.Adaptive.GetAdaptiveColor("accent", _themeVariant);
                        case "hover": return Windows12_2027_Colors.Lighten(Windows12_2027_Colors.Adaptive.GetAdaptiveColor("accent", _themeVariant), 0.1f);
                        case "border": return Windows12_2027_Colors.Adaptive.GetAdaptiveColor("accent", _themeVariant);
                        case "accent": return Windows12_2027_Colors.Lighten(Windows12_2027_Colors.Adaptive.GetAdaptiveColor("accent", _themeVariant), 0.2f);
                        default: return Windows12_2027_Colors.Adaptive.GetAdaptiveColor("accent", _themeVariant);
                    }

                case ButtonStyle.Secondary:
                    switch (colorType)
                    {
                        case "background": return Windows12_2027_Colors.Adaptive.GetAdaptiveColor("surface", _themeVariant);
                        case "hover": return Windows12_2027_Colors.Lighten(Windows12_2027_Colors.Adaptive.GetAdaptiveColor("surface", _themeVariant), 0.1f);
                        case "border": return Windows12_2027_Colors.Adaptive.GetAdaptiveColor("accent", _themeVariant);
                        case "accent": return Windows12_2027_Colors.Adaptive.GetAdaptiveColor("accent", _themeVariant);
                        default: return Windows12_2027_Colors.Adaptive.GetAdaptiveColor("surface", _themeVariant);
                    }

                case ButtonStyle.Ghost:
                    switch (colorType)
                    {
                        case "background": return Color.Transparent;
                        case "hover": return Color.FromArgb(20, Windows12_2027_Colors.Adaptive.GetAdaptiveColor("accent", _themeVariant));
                        case "border": return Color.FromArgb(60, Windows12_2027_Colors.Adaptive.GetAdaptiveColor("accent", _themeVariant));
                        case "accent": return Windows12_2027_Colors.Adaptive.GetAdaptiveColor("accent", _themeVariant);
                        default: return Color.Transparent;
                    }

                case ButtonStyle.Glass:
                    switch (colorType)
                    {
                        case "background": return Color.FromArgb(100, Windows12_2027_Colors.Adaptive.GetAdaptiveColor("surface", _themeVariant));
                        case "hover": return Color.FromArgb(150, Windows12_2027_Colors.Adaptive.GetAdaptiveColor("surface", _themeVariant));
                        case "border": return Color.FromArgb(80, Windows12_2027_Colors.Adaptive.GetAdaptiveColor("accent", _themeVariant));
                        case "accent": return Windows12_2027_Colors.Adaptive.GetAdaptiveColor("accent", _themeVariant);
                        default: return Color.FromArgb(100, Windows12_2027_Colors.Adaptive.GetAdaptiveColor("surface", _themeVariant));
                    }

                default:
                    return Windows12_2027_Colors.Adaptive.GetAdaptiveColor("accent", _themeVariant);
            }
        }

        private Color InterpolateColor(Color color1, Color color2, float factor)
        {
            factor = Math.Max(0f, Math.Min(1f, factor));
            
            return Color.FromArgb(
                (int)(color1.A + (color2.A - color1.A) * factor),
                (int)(color1.R + (color2.R - color1.R) * factor),
                (int)(color1.G + (color2.G - color1.G) * factor),
                (int)(color1.B + (color2.B - color1.B) * factor)
            );
        }

        #endregion

        #region Cleanup

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _animationTimer?.Stop();
                _animationTimer?.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }

    /// <summary>
    /// Button style enumeration
    /// </summary>
    public enum ButtonStyle
    {
        Primary,    // Filled with accent color
        Secondary,  // Outlined with surface background
        Ghost,      // Transparent with border
        Glass       // Glassmorphism effect
    }
}
