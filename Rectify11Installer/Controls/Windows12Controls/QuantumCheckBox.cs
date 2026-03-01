using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Rectify11Installer.Themes;

namespace Rectify11Installer.Controls.Windows12Controls
{
    /// <summary>
    /// QuantumCheckBox - A futuristic checkbox with quantum field effects
    /// Features morphing animations, energy field visualization, and adaptive theming
    /// </summary>
    public class QuantumCheckBox : CheckBox
    {
        #region Private Fields

        private ThemeVariant _themeVariant = ThemeVariant.Quantum;
        private Timer _animationTimer;
        private float _animationProgress = 0f;
        private bool _isAnimating = false;
        private float _hoverIntensity = 0f;
        private bool _isHovered = false;
        private float _energyFieldOffset = 0f;
        private bool _showEnergyField = true;
        private int _checkBoxSize = 20;
        private int _spacing = 8;

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
        /// Gets or sets whether to show the energy field effect
        /// </summary>
        public bool ShowEnergyField
        {
            get => _showEnergyField;
            set
            {
                if (_showEnergyField != value)
                {
                    _showEnergyField = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the size of the checkbox
        /// </summary>
        public int CheckBoxSize
        {
            get => _checkBoxSize;
            set
            {
                if (_checkBoxSize != value && value > 0)
                {
                    _checkBoxSize = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the spacing between checkbox and text
        /// </summary>
        public int Spacing
        {
            get => _spacing;
            set
            {
                if (_spacing != value && value >= 0)
                {
                    _spacing = value;
                    Invalidate();
                }
            }
        }

        #endregion

        #region Constructor

        public QuantumCheckBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            ForeColor = Windows12_2027_Colors.Adaptive.GetAdaptiveColor("text", _themeVariant);
            Font = Windows12_2027_Typography.Styles.LabelMedium;
            
            InitializeAnimation();
        }

        #endregion

        #region Animation

        private void InitializeAnimation()
        {
            _animationTimer = new Timer();
            _animationTimer.Interval = 16; // ~60 FPS
            _animationTimer.Tick += AnimationTimer_Tick;
            _animationTimer.Start(); // Always running for energy field effect
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            bool needsRedraw = false;
            
            // Handle check/uncheck animation
            if (_isAnimating)
            {
                _animationProgress += 0.1f;
                if (_animationProgress >= 1f)
                {
                    _animationProgress = 1f;
                    _isAnimating = false;
                }
                needsRedraw = true;
            }
            
            // Handle hover animation
            float targetHover = _isHovered ? 1f : 0f;
            if (Math.Abs(_hoverIntensity - targetHover) > 0.01f)
            {
                _hoverIntensity += (targetHover - _hoverIntensity) * 0.15f;
                needsRedraw = true;
            }
            
            // Energy field animation
            if (_showEnergyField)
            {
                _energyFieldOffset += 2f;
                if (_energyFieldOffset > 360f)
                    _energyFieldOffset = 0f;
                needsRedraw = true;
            }
            
            if (needsRedraw)
                Invalidate();
        }

        #endregion

        #region Event Handlers

        protected override void OnCheckedChanged(EventArgs e)
        {
            _animationProgress = 0f;
            _isAnimating = true;
            base.OnCheckedChanged(e);
        }

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

        #endregion

        #region Painting

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.CompositingQuality = CompositingQuality.HighQuality;

            // Calculate layout
            var checkBoxBounds = new Rectangle(0, (Height - _checkBoxSize) / 2, _checkBoxSize, _checkBoxSize);
            var textBounds = new Rectangle(_checkBoxSize + _spacing, 0, Width - _checkBoxSize - _spacing, Height);

            // Draw energy field background
            if (_showEnergyField)
                DrawEnergyField(g, checkBoxBounds);

            // Draw checkbox
            DrawCheckBox(g, checkBoxBounds);

            // Draw text
            if (!string.IsNullOrEmpty(Text))
                DrawText(g, textBounds);
        }

        private void DrawEnergyField(Graphics g, Rectangle checkBoxBounds)
        {
            if (_themeVariant != ThemeVariant.Quantum) return;

            // Expand bounds for energy field
            var fieldBounds = new Rectangle(
                checkBoxBounds.X - 10,
                checkBoxBounds.Y - 10,
                checkBoxBounds.Width + 20,
                checkBoxBounds.Height + 20
            );

            // Draw quantum field waves
            using (var pen = new Pen(Color.FromArgb(30, Windows12_2027_Colors.Quantum.ENERGY), 1))
            {
                for (int i = 0; i < 3; i++)
                {
                    float phase = _energyFieldOffset + (i * 120f);
                    int radius = (int)(15 + 5 * Math.Sin(Math.PI * phase / 180f));
                    
                    var waveBounds = new Rectangle(
                        checkBoxBounds.X + checkBoxBounds.Width/2 - radius,
                        checkBoxBounds.Y + checkBoxBounds.Height/2 - radius,
                        radius * 2,
                        radius * 2
                    );
                    
                    g.DrawEllipse(pen, waveBounds);
                }
            }

            // Draw energy particles
            var random = new Random((int)_energyFieldOffset);
            for (int i = 0; i < 8; i++)
            {
                float angle = (i * 45f + _energyFieldOffset) * (float)Math.PI / 180f;
                int distance = 25 + (int)(5 * Math.Sin(_energyFieldOffset * Math.PI / 180f));
                
                int x = checkBoxBounds.X + checkBoxBounds.Width/2 + (int)(distance * Math.Cos(angle));
                int y = checkBoxBounds.Y + checkBoxBounds.Height/2 + (int)(distance * Math.Sin(angle));
                
                int alpha = (int)(255 * (0.3f + 0.2f * Math.Sin((i * 30f + _energyFieldOffset) * Math.PI / 180f)));
                using (var brush = new SolidBrush(Color.FromArgb(alpha, Windows12_2027_Colors.Quantum.PARTICLE)))
                {
                    g.FillEllipse(brush, x - 1, y - 1, 2, 2);
                }
            }
        }

        private void DrawCheckBox(Graphics g, Rectangle bounds)
        {
            // Background with glassmorphism
            Color bgColor = GetThemeColor("surface");
            using (var bgBrush = new SolidBrush(Color.FromArgb(100, bgColor)))
            {
                g.FillRoundedRectangle(bgBrush, bounds, Windows12_2027_DesignSystem.BorderRadius.SM);
            }

            // Hover glow effect
            if (_hoverIntensity > 0)
            {
                int glowSize = (int)(8 * _hoverIntensity);
                for (int i = glowSize; i > 0; i--)
                {
                    int alpha = (int)(255 * (glowSize - i) / glowSize * _hoverIntensity * 0.1f);
                    using (var pen = new Pen(Color.FromArgb(alpha, GetThemeColor("accent")), 1))
                    {
                        var glowBounds = new Rectangle(
                            bounds.X - i/2,
                            bounds.Y - i/2,
                            bounds.Width + i,
                            bounds.Height + i
                        );
                        g.DrawRoundedRectangle(pen, glowBounds, Windows12_2027_DesignSystem.BorderRadius.SM + i/2);
                    }
                }
            }

            // Border
            Color borderColor = _isHovered ? GetThemeColor("accent") : Color.FromArgb(80, GetThemeColor("accent"));
            using (var pen = new Pen(borderColor, 2))
            {
                g.DrawRoundedRectangle(pen, bounds, Windows12_2027_DesignSystem.BorderRadius.SM);
            }

            // Check mark or intermediate state
            if (Checked || _isAnimating)
            {
                DrawCheckMark(g, bounds);
            }
        }

        private void DrawCheckMark(Graphics g, Rectangle bounds)
        {
            float progress = Checked ? (_isAnimating ? _animationProgress : 1f) : (1f - _animationProgress);
            
            if (progress <= 0) return;

            // Quantum-style check mark
            var center = new PointF(bounds.X + bounds.Width / 2f, bounds.Y + bounds.Height / 2f);
            
            if (_themeVariant == ThemeVariant.Quantum)
            {
                // Quantum energy burst effect
                DrawQuantumCheckMark(g, center, bounds.Width * 0.3f, progress);
            }
            else
            {
                // Traditional check mark with animation
                DrawAnimatedCheckMark(g, bounds, progress);
            }
        }

        private void DrawQuantumCheckMark(Graphics g, PointF center, float size, float progress)
        {
            // Central energy core
            float coreSize = size * progress;
            using (var brush = new SolidBrush(GetThemeColor("accent")))
            {
                g.FillEllipse(brush, center.X - coreSize/2, center.Y - coreSize/2, coreSize, coreSize);
            }

            // Energy rays
            int rayCount = 8;
            for (int i = 0; i < rayCount; i++)
            {
                float angle = (i * 360f / rayCount + _energyFieldOffset) * (float)Math.PI / 180f;
                float rayLength = size * progress * 0.8f;
                
                var endPoint = new PointF(
                    center.X + rayLength * (float)Math.Cos(angle),
                    center.Y + rayLength * (float)Math.Sin(angle)
                );

                int alpha = (int)(255 * progress * (0.5f + 0.5f * Math.Sin((i * 45f + _energyFieldOffset) * Math.PI / 180f)));
                using (var pen = new Pen(Color.FromArgb(alpha, GetThemeColor("accent")), 2))
                {
                    g.DrawLine(pen, center, endPoint);
                }
            }
        }

        private void DrawAnimatedCheckMark(Graphics g, Rectangle bounds, float progress)
        {
            // Traditional animated check mark
            var checkPoints = new PointF[]
            {
                new PointF(bounds.X + bounds.Width * 0.25f, bounds.Y + bounds.Height * 0.5f),
                new PointF(bounds.X + bounds.Width * 0.45f, bounds.Y + bounds.Height * 0.7f),
                new PointF(bounds.X + bounds.Width * 0.75f, bounds.Y + bounds.Height * 0.3f)
            };

            using (var pen = new Pen(GetThemeColor("accent"), 3))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;

                // First line (animated)
                if (progress > 0f)
                {
                    float firstLineProgress = Math.Min(1f, progress * 2f);
                    var firstEnd = new PointF(
                        checkPoints[0].X + (checkPoints[1].X - checkPoints[0].X) * firstLineProgress,
                        checkPoints[0].Y + (checkPoints[1].Y - checkPoints[0].Y) * firstLineProgress
                    );
                    g.DrawLine(pen, checkPoints[0], firstEnd);
                }

                // Second line (animated)
                if (progress > 0.5f)
                {
                    float secondLineProgress = Math.Min(1f, (progress - 0.5f) * 2f);
                    var secondEnd = new PointF(
                        checkPoints[1].X + (checkPoints[2].X - checkPoints[1].X) * secondLineProgress,
                        checkPoints[1].Y + (checkPoints[2].Y - checkPoints[1].Y) * secondLineProgress
                    );
                    g.DrawLine(pen, checkPoints[1], secondEnd);
                }
            }
        }

        private void DrawText(Graphics g, Rectangle bounds)
        {
            Color textColor = ForeColor;
            
            // Apply hover effect to text
            if (_hoverIntensity > 0)
            {
                Color accentColor = GetThemeColor("accent");
                textColor = Color.FromArgb(
                    textColor.A,
                    (int)(textColor.R + (accentColor.R - textColor.R) * _hoverIntensity * 0.3f),
                    (int)(textColor.G + (accentColor.G - textColor.G) * _hoverIntensity * 0.3f),
                    (int)(textColor.B + (accentColor.B - textColor.B) * _hoverIntensity * 0.3f)
                );
            }

            using (var brush = new SolidBrush(textColor))
            using (var format = new StringFormat { LineAlignment = StringAlignment.Center })
            {
                g.DrawString(Text, Font, brush, bounds, format);
            }
        }

        #endregion

        #region Helper Methods

        private Color GetThemeColor(string colorName)
        {
            return Windows12_2027_Colors.Adaptive.GetAdaptiveColor(colorName, _themeVariant);
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
}

