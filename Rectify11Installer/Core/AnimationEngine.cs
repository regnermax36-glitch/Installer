using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Rectify11Installer.Core
{
    /// <summary>
    /// Advanced Animation Engine for Windows 12 2027
    /// Provides smooth animations, transitions, and micro-interactions
    /// </summary>
    public class AnimationEngine
    {
        #region Singleton Implementation

        private static AnimationEngine _instance;
        private static readonly object _lock = new object();

        public static AnimationEngine Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new AnimationEngine();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Private Fields

        private Timer _masterTimer;
        private List<Animation> _activeAnimations = new List<Animation>();
        private bool _isRunning = false;
        private const int TARGET_FPS = 60;
        private const int TIMER_INTERVAL = 1000 / TARGET_FPS; // ~16ms for 60 FPS

        #endregion

        #region Constructor

        private AnimationEngine()
        {
            InitializeTimer();
        }

        #endregion

        #region Initialization

        private void InitializeTimer()
        {
            _masterTimer = new Timer();
            _masterTimer.Interval = TIMER_INTERVAL;
            _masterTimer.Tick += MasterTimer_Tick;
        }

        #endregion

        #region Animation Management

        /// <summary>
        /// Starts the animation engine
        /// </summary>
        public void Start()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                _masterTimer.Start();
            }
        }

        /// <summary>
        /// Stops the animation engine
        /// </summary>
        public void Stop()
        {
            if (_isRunning)
            {
                _isRunning = false;
                _masterTimer.Stop();
                _activeAnimations.Clear();
            }
        }

        /// <summary>
        /// Animates a property from one value to another
        /// </summary>
        public void AnimateProperty<T>(object target, string propertyName, T fromValue, T toValue, 
            int duration, EasingFunction easing = null, Action onComplete = null)
        {
            var animation = new PropertyAnimation<T>(target, propertyName, fromValue, toValue, duration, easing, onComplete);
            AddAnimation(animation);
        }

        /// <summary>
        /// Animates a control's location
        /// </summary>
        public void AnimateLocation(Control control, Point fromLocation, Point toLocation, 
            int duration, EasingFunction easing = null, Action onComplete = null)
        {
            var animation = new LocationAnimation(control, fromLocation, toLocation, duration, easing, onComplete);
            AddAnimation(animation);
        }

        /// <summary>
        /// Animates a control's size
        /// </summary>
        public void AnimateSize(Control control, Size fromSize, Size toSize, 
            int duration, EasingFunction easing = null, Action onComplete = null)
        {
            var animation = new SizeAnimation(control, fromSize, toSize, duration, easing, onComplete);
            AddAnimation(animation);
        }

        /// <summary>
        /// Animates a control's opacity (requires custom implementation)
        /// </summary>
        public void AnimateOpacity(Control control, float fromOpacity, float toOpacity, 
            int duration, EasingFunction easing = null, Action onComplete = null)
        {
            var animation = new OpacityAnimation(control, fromOpacity, toOpacity, duration, easing, onComplete);
            AddAnimation(animation);
        }

        /// <summary>
        /// Creates a fade-in animation
        /// </summary>
        public void FadeIn(Control control, int duration = 300, Action onComplete = null)
        {
            AnimateOpacity(control, 0f, 1f, duration, EasingFunctions.EaseOutQuart, onComplete);
        }

        /// <summary>
        /// Creates a fade-out animation
        /// </summary>
        public void FadeOut(Control control, int duration = 300, Action onComplete = null)
        {
            AnimateOpacity(control, 1f, 0f, duration, EasingFunctions.EaseOutQuart, onComplete);
        }

        /// <summary>
        /// Creates a slide-in animation from the left
        /// </summary>
        public void SlideInFromLeft(Control control, int duration = 400, Action onComplete = null)
        {
            var targetLocation = control.Location;
            var startLocation = new Point(targetLocation.X - control.Width, targetLocation.Y);
            control.Location = startLocation;
            AnimateLocation(control, startLocation, targetLocation, duration, EasingFunctions.EaseOutBack, onComplete);
        }

        /// <summary>
        /// Creates a slide-in animation from the right
        /// </summary>
        public void SlideInFromRight(Control control, int duration = 400, Action onComplete = null)
        {
            var targetLocation = control.Location;
            var startLocation = new Point(targetLocation.X + control.Width, targetLocation.Y);
            control.Location = startLocation;
            AnimateLocation(control, startLocation, targetLocation, duration, EasingFunctions.EaseOutBack, onComplete);
        }

        /// <summary>
        /// Creates a scale animation
        /// </summary>
        public void Scale(Control control, float fromScale, float toScale, int duration = 300, Action onComplete = null)
        {
            var originalSize = control.Size;
            var fromSize = new Size((int)(originalSize.Width * fromScale), (int)(originalSize.Height * fromScale));
            var toSize = new Size((int)(originalSize.Width * toScale), (int)(originalSize.Height * toScale));
            
            AnimateSize(control, fromSize, toSize, duration, EasingFunctions.EaseOutBack, onComplete);
        }

        /// <summary>
        /// Creates a bounce animation
        /// </summary>
        public void Bounce(Control control, int intensity = 10, int duration = 600, Action onComplete = null)
        {
            var originalLocation = control.Location;
            var bounceLocation = new Point(originalLocation.X, originalLocation.Y - intensity);
            
            // Bounce up
            AnimateLocation(control, originalLocation, bounceLocation, duration / 4, EasingFunctions.EaseOutQuart, () =>
            {
                // Bounce down
                AnimateLocation(control, bounceLocation, originalLocation, duration * 3 / 4, EasingFunctions.EaseOutBounce, onComplete);
            });
        }

        /// <summary>
        /// Creates a shake animation
        /// </summary>
        public void Shake(Control control, int intensity = 5, int duration = 500, Action onComplete = null)
        {
            var originalLocation = control.Location;
            var shakeAnimation = new ShakeAnimation(control, originalLocation, intensity, duration, onComplete);
            AddAnimation(shakeAnimation);
        }

        /// <summary>
        /// Stops all animations for a specific control
        /// </summary>
        public void StopAnimations(Control control)
        {
            _activeAnimations.RemoveAll(a => a.Target == control);
        }

        /// <summary>
        /// Stops all animations
        /// </summary>
        public void StopAllAnimations()
        {
            _activeAnimations.Clear();
        }

        #endregion

        #region Private Methods

        private void AddAnimation(Animation animation)
        {
            _activeAnimations.Add(animation);
            
            if (!_isRunning)
                Start();
        }

        private void MasterTimer_Tick(object sender, EventArgs e)
        {
            if (_activeAnimations.Count == 0)
            {
                Stop();
                return;
            }

            var completedAnimations = new List<Animation>();

            foreach (var animation in _activeAnimations.ToList())
            {
                try
                {
                    animation.Update();
                    
                    if (animation.IsCompleted)
                    {
                        completedAnimations.Add(animation);
                        animation.OnComplete?.Invoke();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Animation error: {ex.Message}");
                    completedAnimations.Add(animation);
                }
            }

            // Remove completed animations
            foreach (var completed in completedAnimations)
            {
                _activeAnimations.Remove(completed);
            }
        }

        #endregion

        #region Cleanup

        public void Dispose()
        {
            Stop();
            _masterTimer?.Dispose();
        }

        #endregion
    }

    #region Animation Classes

    /// <summary>
    /// Base animation class
    /// </summary>
    public abstract class Animation
    {
        public object Target { get; protected set; }
        public int Duration { get; protected set; }
        public EasingFunction Easing { get; protected set; }
        public Action OnComplete { get; protected set; }
        public bool IsCompleted { get; protected set; }
        
        protected DateTime StartTime { get; set; }
        protected float Progress => Math.Min(1f, (float)(DateTime.Now - StartTime).TotalMilliseconds / Duration);

        protected Animation(object target, int duration, EasingFunction easing, Action onComplete)
        {
            Target = target;
            Duration = duration;
            Easing = easing ?? EasingFunctions.Linear;
            OnComplete = onComplete;
            StartTime = DateTime.Now;
        }

        public abstract void Update();
    }

    /// <summary>
    /// Generic property animation
    /// </summary>
    public class PropertyAnimation<T> : Animation
    {
        private string PropertyName { get; }
        private T FromValue { get; }
        private T ToValue { get; }

        public PropertyAnimation(object target, string propertyName, T fromValue, T toValue, 
            int duration, EasingFunction easing, Action onComplete)
            : base(target, duration, easing, onComplete)
        {
            PropertyName = propertyName;
            FromValue = fromValue;
            ToValue = toValue;
        }

        public override void Update()
        {
            if (IsCompleted) return;

            var progress = Easing(Progress);
            var currentValue = InterpolateValue(FromValue, ToValue, progress);
            
            var property = Target.GetType().GetProperty(PropertyName);
            property?.SetValue(Target, currentValue);

            if (Progress >= 1f)
                IsCompleted = true;
        }

        private T InterpolateValue(T from, T to, float progress)
        {
            if (typeof(T) == typeof(int))
            {
                var fromInt = Convert.ToInt32(from);
                var toInt = Convert.ToInt32(to);
                var result = fromInt + (int)((toInt - fromInt) * progress);
                return (T)(object)result;
            }
            else if (typeof(T) == typeof(float))
            {
                var fromFloat = Convert.ToSingle(from);
                var toFloat = Convert.ToSingle(to);
                var result = fromFloat + (toFloat - fromFloat) * progress;
                return (T)(object)result;
            }
            else if (typeof(T) == typeof(Color))
            {
                var fromColor = (Color)(object)from;
                var toColor = (Color)(object)to;
                var result = Color.FromArgb(
                    (int)(fromColor.A + (toColor.A - fromColor.A) * progress),
                    (int)(fromColor.R + (toColor.R - fromColor.R) * progress),
                    (int)(fromColor.G + (toColor.G - fromColor.G) * progress),
                    (int)(fromColor.B + (toColor.B - fromColor.B) * progress)
                );
                return (T)(object)result;
            }
            
            return to; // Fallback
        }
    }

    /// <summary>
    /// Location animation for controls
    /// </summary>
    public class LocationAnimation : Animation
    {
        private Point FromLocation { get; }
        private Point ToLocation { get; }

        public LocationAnimation(Control control, Point fromLocation, Point toLocation, 
            int duration, EasingFunction easing, Action onComplete)
            : base(control, duration, easing, onComplete)
        {
            FromLocation = fromLocation;
            ToLocation = toLocation;
        }

        public override void Update()
        {
            if (IsCompleted || Target is not Control control || control.IsDisposed) return;

            var progress = Easing(Progress);
            var currentLocation = new Point(
                FromLocation.X + (int)((ToLocation.X - FromLocation.X) * progress),
                FromLocation.Y + (int)((ToLocation.Y - FromLocation.Y) * progress)
            );
            
            control.Location = currentLocation;

            if (Progress >= 1f)
                IsCompleted = true;
        }
    }

    /// <summary>
    /// Size animation for controls
    /// </summary>
    public class SizeAnimation : Animation
    {
        private Size FromSize { get; }
        private Size ToSize { get; }

        public SizeAnimation(Control control, Size fromSize, Size toSize, 
            int duration, EasingFunction easing, Action onComplete)
            : base(control, duration, easing, onComplete)
        {
            FromSize = fromSize;
            ToSize = toSize;
        }

        public override void Update()
        {
            if (IsCompleted || Target is not Control control || control.IsDisposed) return;

            var progress = Easing(Progress);
            var currentSize = new Size(
                FromSize.Width + (int)((ToSize.Width - FromSize.Width) * progress),
                FromSize.Height + (int)((ToSize.Height - FromSize.Height) * progress)
            );
            
            control.Size = currentSize;

            if (Progress >= 1f)
                IsCompleted = true;
        }
    }

    /// <summary>
    /// Opacity animation for controls (requires custom opacity implementation)
    /// </summary>
    public class OpacityAnimation : Animation
    {
        private float FromOpacity { get; }
        private float ToOpacity { get; }

        public OpacityAnimation(Control control, float fromOpacity, float toOpacity, 
            int duration, EasingFunction easing, Action onComplete)
            : base(control, duration, easing, onComplete)
        {
            FromOpacity = fromOpacity;
            ToOpacity = toOpacity;
        }

        public override void Update()
        {
            if (IsCompleted || Target is not Control control || control.IsDisposed) return;

            var progress = Easing(Progress);
            var currentOpacity = FromOpacity + (ToOpacity - FromOpacity) * progress;
            
            // Set opacity through custom property or method if available
            // This would require implementing IOpacitySupport interface on controls
            if (control is IOpacitySupport opacityControl)
            {
                opacityControl.Opacity = currentOpacity;
            }

            if (Progress >= 1f)
                IsCompleted = true;
        }
    }

    /// <summary>
    /// Shake animation for controls
    /// </summary>
    public class ShakeAnimation : Animation
    {
        private Point OriginalLocation { get; }
        private int Intensity { get; }
        private Random Random { get; } = new Random();

        public ShakeAnimation(Control control, Point originalLocation, int intensity, 
            int duration, Action onComplete)
            : base(control, duration, EasingFunctions.Linear, onComplete)
        {
            OriginalLocation = originalLocation;
            Intensity = intensity;
        }

        public override void Update()
        {
            if (IsCompleted || Target is not Control control || control.IsDisposed) return;

            var progress = Progress;
            var currentIntensity = Intensity * (1f - progress); // Decrease intensity over time
            
            var offsetX = (Random.Next(-1, 2) * currentIntensity);
            var offsetY = (Random.Next(-1, 2) * currentIntensity);
            
            control.Location = new Point(
                OriginalLocation.X + (int)offsetX,
                OriginalLocation.Y + (int)offsetY
            );

            if (progress >= 1f)
            {
                control.Location = OriginalLocation; // Reset to original position
                IsCompleted = true;
            }
        }
    }

    #endregion

    #region Easing Functions

    /// <summary>
    /// Easing function delegate
    /// </summary>
    public delegate float EasingFunction(float t);

    /// <summary>
    /// Collection of easing functions
    /// </summary>
    public static class EasingFunctions
    {
        public static float Linear(float t) => t;

        public static float EaseInQuad(float t) => t * t;
        public static float EaseOutQuad(float t) => 1 - (1 - t) * (1 - t);
        public static float EaseInOutQuad(float t) => t < 0.5f ? 2 * t * t : 1 - (float)Math.Pow(-2 * t + 2, 2) / 2;

        public static float EaseInCubic(float t) => t * t * t;
        public static float EaseOutCubic(float t) => 1 - (float)Math.Pow(1 - t, 3);
        public static float EaseInOutCubic(float t) => t < 0.5f ? 4 * t * t * t : 1 - (float)Math.Pow(-2 * t + 2, 3) / 2;

        public static float EaseInQuart(float t) => t * t * t * t;
        public static float EaseOutQuart(float t) => 1 - (float)Math.Pow(1 - t, 4);
        public static float EaseInOutQuart(float t) => t < 0.5f ? 8 * t * t * t * t : 1 - (float)Math.Pow(-2 * t + 2, 4) / 2;

        public static float EaseInBack(float t)
        {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1;
            return c3 * t * t * t - c1 * t * t;
        }

        public static float EaseOutBack(float t)
        {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1;
            return 1 + c3 * (float)Math.Pow(t - 1, 3) + c1 * (float)Math.Pow(t - 1, 2);
        }

        public static float EaseOutBounce(float t)
        {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;

            if (t < 1 / d1)
            {
                return n1 * t * t;
            }
            else if (t < 2 / d1)
            {
                return n1 * (t -= 1.5f / d1) * t + 0.75f;
            }
            else if (t < 2.5 / d1)
            {
                return n1 * (t -= 2.25f / d1) * t + 0.9375f;
            }
            else
            {
                return n1 * (t -= 2.625f / d1) * t + 0.984375f;
            }
        }
    }

    #endregion

    #region Interfaces

    /// <summary>
    /// Interface for controls that support opacity animation
    /// </summary>
    public interface IOpacitySupport
    {
        float Opacity { get; set; }
    }

    #endregion
}

