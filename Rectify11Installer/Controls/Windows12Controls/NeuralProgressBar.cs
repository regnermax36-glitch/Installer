using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Rectify11Installer.Themes;

namespace Rectify11Installer.Controls.Windows12Controls
{
    /// <summary>
    /// NeuralProgressBar - A futuristic progress bar with neural network visualization
    /// Features animated progress, particle effects, and adaptive theming
    /// </summary>
    public class NeuralProgressBar : Control
    {
        #region Private Fields

        private int _value = 0;
        private int _maximum = 100;
        private int _minimum = 0;
        private ThemeVariant _themeVariant = ThemeVariant.Neural;
        private Timer _animationTimer;
        private float _animationOffset = 0f;
        private bool _showParticles = true;
        private bool _showPulse = true;
        private string _text = "";
        private bool _showText = true;
        private ProgressBarStyle _style = ProgressBarStyle.Continuous;
        private Color _progressColor = Color.Empty;
        private Color _backgroundColor = Color.Empty;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the current value of the progress bar
        /// </summary>
        public int Value
        {
            get => _value;
            set
            {
                int newValue = Math.Max(_minimum, Math.Min(_maximum, value));
                if (_value != newValue)
                {
                    _value = newValue;
                    Invalidate();
                    OnValueChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum value of the progress bar
        /// </summary>
        public int Maximum
        {
            get => _maximum;
            set
            {
                if (_maximum != value && value >= _minimum)
                {
                    _maximum = value;
                    if (_value > _maximum)
                        _value = _maximum;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the minimum value of the progress bar
        /// </summary>
        public int Minimum
        {
            get => _minimum;
            set
            {
                if (_minimum != value && value <= _maximum)
                {
                    _minimum = value;
                    if (_value < _minimum)
                        _value = _minimum;
                    Invalidate();
                }
            }
        }

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
        /// Gets or sets whether to show particle effects
        /// </summary>
        public bool ShowParticles
        {
            get => _showParticles;
            set
            {
                if (_showParticles != value)
                {
                    _showParticles = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether to show pulse effect
        /// </summary>
        public bool ShowPulse
        {
            get => _showPulse;
            set
            {
                if (_showPulse != value)
                {
                    _showPulse = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the text to display on the progress bar
        /// </summary>
        public override string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether to show text
        /// </summary>
        public bool ShowText
        {
            get => _showText;
            set
            {
                if (_showText != value)
                {
                    _showText = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the progress bar style
        /// </summary>
        public ProgressBarStyle Style
        {
            get => _style;
            set
            {
                if (_style != value)
                {
                    _style = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the progress color (overrides theme color if set)
        /// </summary>
        public Color ProgressColor
        {
            get => _progressColor;
            set
            {
                if (_progressColor != value)
                {
                    _progressColor = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the background color (overrides theme color if set)
        /// </summary>
        public new Color BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                if (_backgroundColor != value)
                {
                    _backgroundColor = value;
                    Invalidate();
                }
            }
        }

        #endregion

        #region Events

        public event EventHandler ValueChanged;

        protected virtual void OnValueChanged(EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        #endregion

        #region Constructor

        public NeuralProgressBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            Size = new Size(200, 30);
            BackColor = Color.Transparent;
            
            InitializeAnimation();
        }

        #endregion

        #region Animation

        private void InitializeAnimation()
        {
            _animationTimer = new Timer();
            _animationTimer.Interval = 50; // 20 FPS
            _animationTimer.Tick += AnimationTimer_Tick;
            _animationTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            _animationOffset += 2f;
            if (_animationOffset > Width)
                _animationOffset = -50f;
            
            Invalidate();
        }

        #endregion

        #region Painting

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.CompositingQuality = CompositingQuality.HighQuality;

            var bounds = ClientRectangle;
            
            // Draw background
            DrawBackground(g, bounds);
            
            // Draw progress
            DrawProgress(g, bounds);
            
            // Draw effects
            if (_showParticles)
                DrawParticleEffects(g, bounds);
            
            if (_showPulse && _value > 0)
                DrawPulseEffect(g, bounds);
            
            // Draw text
            if (_showText && !string.IsNullOrEmpty(_text))
                DrawText(g, bounds);

            base.OnPaint(e);
        }

        private void DrawBackground(Graphics g, Rectangle bounds)
        {
            Color bgColor = _backgroundColor.IsEmpty ? GetThemeColor("surface") : _backgroundColor;
            
            // Background with glassmorphism
            using (var brush = new SolidBrush(Color.FromArgb(100, bgColor)))
            {
                Windows12_2027_DesignSystem.GraphicsExtensions.FillRoundedRectangle(g, brush, bounds, Windows12_2027_DesignSystem.BorderRadius.MD);
            }
            
            // Subtle border
            using (var pen = new Pen(Color.FromArgb(40, GetThemeColor("accent")), 1))
            {
                Windows12_2027_DesignSystem.GraphicsExtensions.DrawRoundedRectangle(g, pen, bounds, Windows12_2027_DesignSystem.BorderRadius.MD);
            }
        }

        private void DrawProgress(Graphics g, Rectangle bounds)
        {
            if (_value <= _minimum) return;

            float percentage = (float)(_value - _minimum) / (_maximum - _minimum);
            int progressWidth = (int)(bounds.Width * percentage);
            
            if (progressWidth <= 0) return;

            var progressBounds = new Rectangle(bounds.X, bounds.Y, progressWidth, bounds.Height);
            
            if (_style == ProgressBarStyle.Continuous)
            {
                DrawContinuousProgress(g, progressBounds, bounds);
            }
            else
            {
                DrawMarqueeProgress(g, bounds);
            }
        }

        private void DrawContinuousProgress(Graphics g, Rectangle progressBounds, Rectangle totalBounds)
        {
            Color progressColor = _progressColor.IsEmpty ? GetThemeColor("accent") : _progressColor;
            
            // Main progress fill with gradient
            using (var gradient = new LinearGradientBrush(
                progressBounds,
                progressColor,
                Windows12_2027_Colors.Lighten(progressColor, 0.2f),
                LinearGradientMode.Vertical))
            {
                Windows12_2027_DesignSystem.GraphicsExtensions.FillRoundedRectangle(g, gradient, progressBounds, Windows12_2027_DesignSystem.BorderRadius.MD);
            }
            
            // Neural network pattern overlay
            if (_themeVariant == ThemeVariant.Neural)
            {
                DrawNeuralPattern(g, progressBounds);
            }
            
            // Animated highlight
            DrawAnimatedHighlight(g, progressBounds);
        }

        private void DrawMarqueeProgress(Graphics g, Rectangle bounds)
        {
            Color progressColor = _progressColor.IsEmpty ? GetThemeColor("accent") : _progressColor;
            
            // Marquee blocks
            int blockWidth = 20;
            int spacing = 5;
            
            for (float x = _animationOffset; x < bounds.Width + blockWidth; x += blockWidth + spacing)
            {
                if (x + blockWidth > 0) // Only draw visible blocks
                {
                    var blockBounds = new Rectangle(
                        (int)Math.Max(bounds.X, x),
                        bounds.Y + 2,
                        (int)Math.Min(blockWidth, bounds.Width - Math.Max(0, x - bounds.X)),
                        bounds.Height - 4
                    );
                    
                    if (blockBounds.Width > 0)
                    {
                        using (var brush = new SolidBrush(progressColor))
                        {
                            Windows12_2027_DesignSystem.GraphicsExtensions.FillRoundedRectangle(g, brush, blockBounds, Windows12_2027_DesignSystem.BorderRadius.SM);
                        }
                    }
                }
            }
        }

        private void DrawNeuralPattern(Graphics g, Rectangle bounds)
        {
            // Simple neural connection lines
            using (var pen = new Pen(Color.FromArgb(60, Color.White), 1))
            {
                int nodeCount = bounds.Width / 30;
                for (int i = 0; i < nodeCount; i++)
                {
                    int x = bounds.X + (i * bounds.Width / nodeCount);
                    int y1 = bounds.Y + bounds.Height / 3;
                    int y2 = bounds.Y + 2 * bounds.Height / 3;
                    
                    g.DrawLine(pen, x, y1, x + 15, y2);
                    
                    // Small nodes
                    g.FillEllipse(Brushes.White, x - 1, y1 - 1, 2, 2);
                    g.FillEllipse(Brushes.White, x + 14, y2 - 1, 2, 2);
                }
            }
        }

        private void DrawAnimatedHighlight(Graphics g, Rectangle bounds)
        {
            // Moving highlight effect
            float highlightPos = (_animationOffset / Width) * bounds.Width;
            int highlightWidth = 30;
            
            if (highlightPos > -highlightWidth && highlightPos < bounds.Width)
            {
                var highlightBounds = new Rectangle(
                    (int)Math.Max(bounds.X, bounds.X + highlightPos),
                    bounds.Y,
                    (int)Math.Min(highlightWidth, bounds.Width - Math.Max(0, highlightPos)),
                    bounds.Height
                );
                
                if (highlightBounds.Width > 0)
                {
                    using (var brush = new LinearGradientBrush(
                        highlightBounds,
                        Color.FromArgb(0, Color.White),
                        Color.FromArgb(100, Color.White),
                        LinearGradientMode.Horizontal))
                    {
                        Windows12_2027_DesignSystem.GraphicsExtensions.FillRoundedRectangle(g, brush, highlightBounds, Windows12_2027_DesignSystem.BorderRadius.MD);
                    }
                }
            }
        }

        private void DrawParticleEffects(Graphics g, Rectangle bounds)
        {
            if (_value <= _minimum) return;

            // Simple particle effect at the progress edge
            float percentage = (float)(_value - _minimum) / (_maximum - _minimum);
            int progressEnd = (int)(bounds.Width * percentage);
            
            var random = new Random((int)_animationOffset);
            
            for (int i = 0; i < 5; i++)
            {
                int x = progressEnd + random.Next(-10, 10);
                int y = bounds.Y + random.Next(bounds.Height);
                int size = random.Next(1, 3);
                int alpha = random.Next(100, 200);
                
                if (x >= bounds.X && x < bounds.Right)
                {
                    using (var brush = new SolidBrush(Color.FromArgb(alpha, GetThemeColor("accent"))))
                    {
                        g.FillEllipse(brush, x - size, y - size, size * 2, size * 2);
                    }
                }
            }
        }

        private void DrawPulseEffect(Graphics g, Rectangle bounds)
        {
            float percentage = (float)(_value - _minimum) / (_maximum - _minimum);
            int progressEnd = (int)(bounds.Width * percentage);
            
            // Pulsing glow at progress end
            float pulseIntensity = (float)(Math.Sin(_animationOffset * 0.1) + 1) / 2; // 0 to 1
            int glowSize = (int)(10 * pulseIntensity);
            
            for (int i = glowSize; i > 0; i--)
            {
                int alpha = (int)(255 * (glowSize - i) / glowSize * pulseIntensity * 0.3f);
                using (var brush = new SolidBrush(Color.FromArgb(alpha, GetThemeColor("accent"))))
                {
                    g.FillEllipse(brush, 
                        progressEnd - i, bounds.Y + bounds.Height/2 - i,
                        i * 2, i * 2);
                }
            }
        }

        private void DrawText(Graphics g, Rectangle bounds)
        {
            string displayText = _text;
            if (string.IsNullOrEmpty(displayText))
            {
                displayText = $"{_value}%";
            }
            
            using (var font = Windows12_2027_Typography.Styles.LabelMedium)
            using (var brush = new SolidBrush(GetThemeColor("text")))
            {
                var textSize = g.MeasureString(displayText, font);
                var textPos = new PointF(
                    bounds.X + (bounds.Width - textSize.Width) / 2,
                    bounds.Y + (bounds.Height - textSize.Height) / 2
                );
                
                // Text shadow for better readability
                using (var shadowBrush = new SolidBrush(Color.FromArgb(100, Color.Black)))
                {
                    g.DrawString(displayText, font, shadowBrush, textPos.X + 1, textPos.Y + 1);
                }
                
                g.DrawString(displayText, font, brush, textPos);
            }
        }

        #endregion

        #region Helper Methods

        private Color GetThemeColor(string colorName)
        {
            return Windows12_2027_Colors.Adaptive.GetAdaptiveColor(colorName, _themeVariant);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Increments the progress bar value by the specified amount
        /// </summary>
        public void Increment(int value)
        {
            Value += value;
        }

        /// <summary>
        /// Sets the progress bar to marquee style for indeterminate progress
        /// </summary>
        public void SetMarqueeStyle()
        {
            Style = ProgressBarStyle.Marquee;
        }

        /// <summary>
        /// Sets the progress bar to continuous style for determinate progress
        /// </summary>
        public void SetContinuousStyle()
        {
            Style = ProgressBarStyle.Continuous;
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
    /// Progress bar style enumeration
    /// </summary>
    public enum ProgressBarStyle
    {
        Continuous,
        Marquee
    }
}
