using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Rectify11Installer.Themes;

namespace Rectify11Installer.Core
{
    /// <summary>
    /// Advanced Theme Manager for Windows 12 2027
    /// Handles dynamic theme switching, persistence, and real-time updates
    /// </summary>
    public class ThemeManager2027
    {
        #region Singleton Implementation

        private static ThemeManager2027 _instance;
        private static readonly object _lock = new object();

        public static ThemeManager2027 Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new ThemeManager2027();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Private Fields

        private ThemeVariant _currentTheme = ThemeVariant.Neural;
        private bool _isSystemThemeEnabled = true;
        private bool _isAdaptiveThemeEnabled = true;
        private Timer _adaptiveTimer;
        private List<Control> _registeredControls = new List<Control>();
        private Dictionary<string, object> _themeSettings = new Dictionary<string, object>();
        private string _settingsPath;

        #endregion

        #region Events

        /// <summary>
        /// Fired when the theme changes
        /// </summary>
        public event EventHandler<ThemeChangedEventArgs> ThemeChanged;

        /// <summary>
        /// Fired when theme settings are updated
        /// </summary>
        public event EventHandler<ThemeSettingsChangedEventArgs> ThemeSettingsChanged;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the current active theme variant
        /// </summary>
        public ThemeVariant CurrentTheme
        {
            get => _currentTheme;
            private set
            {
                if (_currentTheme != value)
                {
                    var oldTheme = _currentTheme;
                    _currentTheme = value;
                    OnThemeChanged(new ThemeChangedEventArgs(oldTheme, value));
                }
            }
        }

        /// <summary>
        /// Gets or sets whether system theme detection is enabled
        /// </summary>
        public bool IsSystemThemeEnabled
        {
            get => _isSystemThemeEnabled;
            set
            {
                if (_isSystemThemeEnabled != value)
                {
                    _isSystemThemeEnabled = value;
                    if (value)
                        DetectSystemTheme();
                    SaveSettings();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether adaptive theme changes are enabled
        /// </summary>
        public bool IsAdaptiveThemeEnabled
        {
            get => _isAdaptiveThemeEnabled;
            set
            {
                if (_isAdaptiveThemeEnabled != value)
                {
                    _isAdaptiveThemeEnabled = value;
                    if (value)
                        StartAdaptiveTimer();
                    else
                        StopAdaptiveTimer();
                    SaveSettings();
                }
            }
        }

        /// <summary>
        /// Gets all available theme variants
        /// </summary>
        public ThemeVariant[] AvailableThemes => Enum.GetValues(typeof(ThemeVariant)).Cast<ThemeVariant>().ToArray();

        #endregion

        #region Constructor

        private ThemeManager2027()
        {
            _settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                                       "Rectify11", "theme_settings.json");
            
            LoadSettings();
            InitializeAdaptiveTimer();
            
            if (_isSystemThemeEnabled)
                DetectSystemTheme();
        }

        #endregion

        #region Theme Management

        /// <summary>
        /// Sets the active theme variant
        /// </summary>
        public void SetTheme(ThemeVariant theme, bool animate = true)
        {
            if (animate)
            {
                AnimateThemeTransition(theme);
            }
            else
            {
                CurrentTheme = theme;
                ApplyThemeToRegisteredControls();
            }
            
            SaveSettings();
        }

        /// <summary>
        /// Cycles to the next available theme
        /// </summary>
        public void CycleTheme()
        {
            var themes = AvailableThemes;
            int currentIndex = Array.IndexOf(themes, _currentTheme);
            int nextIndex = (currentIndex + 1) % themes.Length;
            SetTheme(themes[nextIndex]);
        }

        /// <summary>
        /// Gets the appropriate color for the current theme
        /// </summary>
        public Color GetThemeColor(string colorName)
        {
            return Windows12_2027_Colors.Adaptive.GetAdaptiveColor(colorName, _currentTheme);
        }

        /// <summary>
        /// Gets a theme-appropriate font
        /// </summary>
        public Font GetThemeFont(string fontStyle)
        {
            switch (fontStyle.ToLower())
            {
                case "display_large": return Windows12_2027_Typography.Styles.DisplayLarge;
                case "display_medium": return Windows12_2027_Typography.Styles.DisplayMedium;
                case "display_small": return Windows12_2027_Typography.Styles.DisplaySmall;
                case "headline_large": return Windows12_2027_Typography.Styles.HeadlineLarge;
                case "headline_medium": return Windows12_2027_Typography.Styles.HeadlineMedium;
                case "headline_small": return Windows12_2027_Typography.Styles.HeadlineSmall;
                case "title_large": return Windows12_2027_Typography.Styles.TitleLarge;
                case "title_medium": return Windows12_2027_Typography.Styles.TitleMedium;
                case "title_small": return Windows12_2027_Typography.Styles.TitleSmall;
                case "label_large": return Windows12_2027_Typography.Styles.LabelLarge;
                case "label_medium": return Windows12_2027_Typography.Styles.LabelMedium;
                case "label_small": return Windows12_2027_Typography.Styles.LabelSmall;
                case "body_large": return Windows12_2027_Typography.Styles.BodyLarge;
                case "body_medium": return Windows12_2027_Typography.Styles.BodyMedium;
                case "body_small": return Windows12_2027_Typography.Styles.BodySmall;
                default: return Windows12_2027_Typography.Styles.BodyMedium;
            }
        }

        #endregion

        #region Control Registration

        /// <summary>
        /// Registers a control to receive theme updates
        /// </summary>
        public void RegisterControl(Control control)
        {
            if (control != null && !_registeredControls.Contains(control))
            {
                _registeredControls.Add(control);
                ApplyThemeToControl(control);
                
                // Handle control disposal
                control.Disposed += (s, e) => UnregisterControl(control);
            }
        }

        /// <summary>
        /// Unregisters a control from theme updates
        /// </summary>
        public void UnregisterControl(Control control)
        {
            if (control != null)
            {
                _registeredControls.Remove(control);
            }
        }

        /// <summary>
        /// Registers all controls in a form hierarchy
        /// </summary>
        public void RegisterFormHierarchy(Control parent)
        {
            RegisterControl(parent);
            
            foreach (Control child in parent.Controls)
            {
                RegisterFormHierarchy(child);
            }
        }

        #endregion

        #region Theme Application

        private void ApplyThemeToRegisteredControls()
        {
            foreach (var control in _registeredControls.ToList()) // ToList to avoid modification during enumeration
            {
                if (!control.IsDisposed)
                {
                    ApplyThemeToControl(control);
                }
            }
        }

        private void ApplyThemeToControl(Control control)
        {
            if (control.IsDisposed) return;

            try
            {
                // Apply theme based on control type
                if (control is Windows12Controls.HolographicPanel panel)
                {
                    panel.ThemeVariant = _currentTheme;
                }
                else if (control is Windows12Controls.NeuralProgressBar progressBar)
                {
                    progressBar.ThemeVariant = _currentTheme;
                }
                else if (control is Windows12Controls.QuantumCheckBox checkBox)
                {
                    checkBox.ThemeVariant = _currentTheme;
                }
                else if (control is Windows12Controls.Windows12Button button)
                {
                    button.ThemeVariant = _currentTheme;
                }
                else
                {
                    // Apply basic theming to standard controls
                    ApplyBasicTheming(control);
                }

                // Recursively apply to child controls
                foreach (Control child in control.Controls)
                {
                    ApplyThemeToControl(child);
                }
            }
            catch (Exception ex)
            {
                // Log error but don't crash the application
                System.Diagnostics.Debug.WriteLine($"Error applying theme to control: {ex.Message}");
            }
        }

        private void ApplyBasicTheming(Control control)
        {
            // Apply basic color scheme to standard controls
            control.BackColor = GetThemeColor("surface");
            control.ForeColor = GetThemeColor("text");

            // Special handling for specific control types
            if (control is Form form)
            {
                form.BackColor = GetThemeColor("background");
            }
            else if (control is Panel || control is GroupBox)
            {
                control.BackColor = GetThemeColor("surface");
            }
            else if (control is Label)
            {
                control.BackColor = Color.Transparent;
                control.ForeColor = GetThemeColor("text");
            }
            else if (control is TextBox || control is ComboBox)
            {
                control.BackColor = GetThemeColor("surface");
                control.ForeColor = GetThemeColor("text");
            }
        }

        #endregion

        #region Animation

        private void AnimateThemeTransition(ThemeVariant newTheme)
        {
            // Simple fade transition - could be enhanced with more sophisticated animations
            var transitionTimer = new Timer();
            transitionTimer.Interval = 50;
            
            float progress = 0f;
            var oldTheme = _currentTheme;
            
            transitionTimer.Tick += (s, e) =>
            {
                progress += 0.1f;
                
                if (progress >= 1f)
                {
                    progress = 1f;
                    transitionTimer.Stop();
                    transitionTimer.Dispose();
                }
                
                // Apply intermediate theme state
                CurrentTheme = newTheme;
                ApplyThemeToRegisteredControls();
            };
            
            transitionTimer.Start();
        }

        #endregion

        #region System Integration

        private void DetectSystemTheme()
        {
            try
            {
                // Detect Windows dark/light mode
                using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    if (key?.GetValue("AppsUseLightTheme") is int lightTheme)
                    {
                        // Map system theme to our theme variants
                        if (lightTheme == 0) // Dark mode
                        {
                            SetTheme(ThemeVariant.Neural, false);
                        }
                        else // Light mode
                        {
                            SetTheme(ThemeVariant.Quantum, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error detecting system theme: {ex.Message}");
            }
        }

        #endregion

        #region Adaptive Theming

        private void InitializeAdaptiveTimer()
        {
            _adaptiveTimer = new Timer();
            _adaptiveTimer.Interval = 60000; // Check every minute
            _adaptiveTimer.Tick += AdaptiveTimer_Tick;
            
            if (_isAdaptiveThemeEnabled)
                StartAdaptiveTimer();
        }

        private void StartAdaptiveTimer()
        {
            _adaptiveTimer?.Start();
        }

        private void StopAdaptiveTimer()
        {
            _adaptiveTimer?.Stop();
        }

        private void AdaptiveTimer_Tick(object sender, EventArgs e)
        {
            if (!_isAdaptiveThemeEnabled) return;

            // Time-based theme adaptation
            var now = DateTime.Now;
            
            if (now.Hour >= 6 && now.Hour < 12) // Morning
            {
                if (_currentTheme != ThemeVariant.Quantum)
                    SetTheme(ThemeVariant.Quantum, true);
            }
            else if (now.Hour >= 12 && now.Hour < 18) // Afternoon
            {
                if (_currentTheme != ThemeVariant.Minimal)
                    SetTheme(ThemeVariant.Minimal, true);
            }
            else if (now.Hour >= 18 && now.Hour < 22) // Evening
            {
                if (_currentTheme != ThemeVariant.Holographic)
                    SetTheme(ThemeVariant.Holographic, true);
            }
            else // Night
            {
                if (_currentTheme != ThemeVariant.Neural)
                    SetTheme(ThemeVariant.Neural, true);
            }
        }

        #endregion

        #region Settings Persistence

        private void LoadSettings()
        {
            try
            {
                if (File.Exists(_settingsPath))
                {
                    var json = File.ReadAllText(_settingsPath);
                    var settings = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                    
                    if (settings != null)
                    {
                        _themeSettings = settings;
                        
                        if (settings.ContainsKey("CurrentTheme"))
                        {
                            if (Enum.TryParse<ThemeVariant>(settings["CurrentTheme"].ToString(), out var theme))
                                _currentTheme = theme;
                        }
                        
                        if (settings.ContainsKey("IsSystemThemeEnabled"))
                        {
                            if (bool.TryParse(settings["IsSystemThemeEnabled"].ToString(), out var systemTheme))
                                _isSystemThemeEnabled = systemTheme;
                        }
                        
                        if (settings.ContainsKey("IsAdaptiveThemeEnabled"))
                        {
                            if (bool.TryParse(settings["IsAdaptiveThemeEnabled"].ToString(), out var adaptiveTheme))
                                _isAdaptiveThemeEnabled = adaptiveTheme;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading theme settings: {ex.Message}");
            }
        }

        private void SaveSettings()
        {
            try
            {
                _themeSettings["CurrentTheme"] = _currentTheme.ToString();
                _themeSettings["IsSystemThemeEnabled"] = _isSystemThemeEnabled;
                _themeSettings["IsAdaptiveThemeEnabled"] = _isAdaptiveThemeEnabled;
                
                Directory.CreateDirectory(Path.GetDirectoryName(_settingsPath));
                
                var json = System.Text.Json.JsonSerializer.Serialize(_themeSettings, new System.Text.Json.JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                
                File.WriteAllText(_settingsPath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving theme settings: {ex.Message}");
            }
        }

        #endregion

        #region Event Handlers

        protected virtual void OnThemeChanged(ThemeChangedEventArgs e)
        {
            ThemeChanged?.Invoke(this, e);
            ApplyThemeToRegisteredControls();
        }

        protected virtual void OnThemeSettingsChanged(ThemeSettingsChangedEventArgs e)
        {
            ThemeSettingsChanged?.Invoke(this, e);
        }

        #endregion

        #region Cleanup

        public void Dispose()
        {
            _adaptiveTimer?.Stop();
            _adaptiveTimer?.Dispose();
            _registeredControls.Clear();
            SaveSettings();
        }

        #endregion
    }

    #region Event Args Classes

    /// <summary>
    /// Event arguments for theme change events
    /// </summary>
    public class ThemeChangedEventArgs : EventArgs
    {
        public ThemeVariant OldTheme { get; }
        public ThemeVariant NewTheme { get; }

        public ThemeChangedEventArgs(ThemeVariant oldTheme, ThemeVariant newTheme)
        {
            OldTheme = oldTheme;
            NewTheme = newTheme;
        }
    }

    /// <summary>
    /// Event arguments for theme settings change events
    /// </summary>
    public class ThemeSettingsChangedEventArgs : EventArgs
    {
        public string SettingName { get; }
        public object OldValue { get; }
        public object NewValue { get; }

        public ThemeSettingsChangedEventArgs(string settingName, object oldValue, object newValue)
        {
            SettingName = settingName;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    #endregion
}

