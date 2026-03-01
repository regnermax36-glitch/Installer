using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Rectify11Installer.Themes;

namespace Rectify11Installer.Controls.Windows12Controls
{
    /// <summary>
    /// HolographicPanel - A futuristic panel with holographic effects and glassmorphism
    /// Features dynamic gradients, shimmer effects, and adaptive theming
    /// </summary>
    public class HolographicPanel : Panel
    {
        #region Private Fields

        private ThemeVariant _themeVariant = ThemeVariant.Neural;
        private float _shimmerOffset = 0f;
        private Timer _animationTimer;
        private bool _isAnimated = true;
        private float _glowIntensity = 0.5f;
        private int _borderRadius = 12;
        private bool _showBorder = true;
        private float _backgroundOpacity = 0.1f;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the theme variant for the panel
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
        /// Gets or sets whether the panel should animate
        /// </summary>
        public bool IsAnimated
        {
            get => _isAnimated;
            set
            {
                if (_isAnimated != value)
                {
                    _isAnimated = value;
                    if (_isAnimated)
                        StartAnimation();
                    else
                        StopAnimation();
                }
            }
        }

        /// <summary>
        /// Gets or sets the glow intensity (0.0 to 1.0)
        /// </summary>
        public float GlowIntensity
        {
            get => _glowIntensity;
            set
            {
                _glowIntensity = Math.Max(0f, Math.Min(1f, value));
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the border radius for rounded corners
        /// </summary>
        public int BorderRadius
        {
            get => _borderRadius;
            set
            {
                _borderRadius = Math.Max(0, value);
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether to show the border
        /// </summary>
        public bool ShowBorder
        {
            get => _showBorder;
            set
            {
                if (_showBorder != value)
                {
                    _showBorder = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the background opacity (0.0 to 1.0)
        /// </summary>
        public float BackgroundOpacity
        {
            get => _backgroundOpacity;
            set
            {
                _backgroundOpacity = Math.Max(0f, Math.Min(1f, value));
                Invalidate();
            }
        }

        #endregion

        #region Constructor

        public HolographicPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            
            InitializeAnimation();
        }

        #endregion

        #region Animation Methods

        private void InitializeAnimation()
        {
            _animationTimer = new Timer();
            _animationTimer.Interval = 50; // 20 FPS for smooth animation
            _animationTimer.Tick += AnimationTimer_Tick;
            
            if (_isAnimated)
                StartAnimation();
        }

        private void StartAnimation()
        {
            if (_animationTimer != null && !_animationTimer.Enabled)
            {
                _animationTimer.Start();
            }
        }

        private void StopAnimation()
        {
            if (_animationTimer != null && _animationTimer.Enabled)
            {
                _animationTimer.Stop();
            }
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            _shimmerOffset += 2f;
            if (_shimmerOffset > Width + Height)
                _shimmerOffset = -(Width + Height);
            
            Invalidate();
        }

        #endregion

        #region Painting Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.CompositingQuality = CompositingQuality.HighQuality;

            var bounds = new Rectangle(0, 0, Width, Height);
            
            // Draw background with glassmorphism effect
            DrawGlassmorphismBackground(g, bounds);
            
            // Draw holographic shimmer effect
            if (_isAnimated)
                DrawHolographicShimmer(g, bounds);
            
            // Draw glow effect
            if (_glowIntensity > 0)
                DrawGlowEffect(g, bounds);
            
            // Draw border
            if (_showBorder)
                DrawBorder(g, bounds);

            base.OnPaint(e);
        }

        private void DrawGlassmorphismBackground(Graphics g, Rectangle bounds)
        {
            Color baseColor = GetThemeColor("surface");
            
            // Create semi-transparent background
            using (var brush = new SolidBrush(Color.FromArgb((int)(255 * _backgroundOpacity), baseColor)))
            {
                Windows12_2027_DesignSystem.GraphicsExtensions.FillRoundedRectangle(g, brush, bounds, _borderRadius);
            }
            
            // Add gradient overlay for depth
            using (var gradient = CreateThemeGradient(bounds))
            {
                Windows12_2027_DesignSystem.GraphicsExtensions.FillRoundedRectangle(g, gradient, bounds, _borderRadius);
            }
            
            // Add noise texture for glassmorphism
            DrawNoiseTexture(g, bounds);
        }

        private void DrawHolographicShimmer(Graphics g, Rectangle bounds)
        {
            if (_themeVariant != ThemeVariant.Holographic) return;

            // Create shimmer effect
            using (var shimmerBrush = CreateShimmerBrush(bounds))
            {
                // Create clipping path
                using (var path = Windows12_2027_DesignSystem.GraphicsExtensions.CreateRoundedRectanglePath(bounds, _borderRadius))
                {
                    var oldClip = g.Clip;
                    g.SetClip(path);
                    
                    // Draw shimmer line
                    var shimmerBounds = new Rectangle(
                        (int)_shimmerOffset - bounds.Height,
                        0,
                        bounds.Height * 2,
                        bounds.Height
                    );
                    
                    g.FillRectangle(shimmerBrush, shimmerBounds);
                    g.Clip = oldClip;
                }
            }
        }

        private void DrawGlowEffect(Graphics g, Rectangle bounds)
        {
            Color glowColor = GetThemeColor("accent");
            int glowSize = (int)(10 * _glowIntensity);
            
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
                    Windows12_2027_DesignSystem.GraphicsExtensions.DrawRoundedRectangle(g, pen, glowBounds, _borderRadius + i);
                }
            }
        }

        private void DrawBorder(Graphics g, Rectangle bounds)
        {
            Color borderColor = GetThemeColor("accent");
            using (var pen = new Pen(Color.FromArgb(60, borderColor), 1))
            {
                Windows12_2027_DesignSystem.GraphicsExtensions.DrawRoundedRectangle(g, pen, bounds, _borderRadius);
            }
        }

        private void DrawNoiseTexture(Graphics g, Rectangle bounds)
        {
            // Simple noise texture for glassmorphism
            var random = new Random(42); // Fixed seed for consistent pattern
            
            for (int i = 0; i < bounds.Width * bounds.Height / 100; i++)
            {
                int x = random.Next(bounds.Width);
                int y = random.Next(bounds.Height);
                int alpha = random.Next(5, 15);
                
                using (var brush = new SolidBrush(Color.FromArgb(alpha, 255, 255, 255)))
                {
                    g.FillRectangle(brush, x, y, 1, 1);
                }
            }
        }

        #endregion

        #region Helper Methods

        private Color GetThemeColor(string colorName)
        {
            return Windows12_2027_Colors.Adaptive.GetAdaptiveColor(colorName, _themeVariant);
        }

        private LinearGradientBrush CreateThemeGradient(Rectangle bounds)
        {
            switch (_themeVariant)
            {
                case ThemeVariant.Neural:
                    return Windows12_2027_DesignSystem.CreateNeuralGradient(bounds);
                case ThemeVariant.Holographic:
                    return Windows12_2027_DesignSystem.CreateHolographicGradient(bounds);
                case ThemeVariant.Quantum:
                    return Windows12_2027_DesignSystem.CreateQuantumGradient(bounds);
                default:
                    return new LinearGradientBrush(bounds, GetThemeColor("primary"), GetThemeColor("secondary"), LinearGradientMode.Vertical);
            }
        }

        private LinearGradientBrush CreateShimmerBrush(Rectangle bounds)
        {
            var shimmerColors = new Color[]
            {
                Color.FromArgb(0, 255, 255, 255),
                Color.FromArgb(50, 255, 255, 255),
                Color.FromArgb(100, 255, 255, 255),
                Color.FromArgb(50, 255, 255, 255),
                Color.FromArgb(0, 255, 255, 255)
            };

            var brush = new LinearGradientBrush(bounds, Color.Transparent, Color.Transparent, 45f);
            
            var blend = new ColorBlend();
            blend.Colors = shimmerColors;
            blend.Positions = new float[] { 0f, 0.25f, 0.5f, 0.75f, 1f };
            brush.InterpolationColors = blend;
            
            return brush;
        }

        #endregion

        #region Cleanup

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopAnimation();
                _animationTimer?.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
