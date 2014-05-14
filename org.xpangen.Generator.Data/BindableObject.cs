using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace org.xpangen.Generator.Data
{
	/// <summary>
	/// Implements the INotifyPropertyChanged interface and 
	/// exposes a RaisePropertyChanged method for derived 
	/// classes to raise the PropertyChange event.  The event 
	/// arguments created by this class are cached to prevent 
	/// managed heap fragmentation.
	/// </summary>
    /// <remarks>
    /// http://joshsmithonwpf.wordpress.com/2007/08/29/a-base-class-which-implements-inotifypropertychanged/
    /// </remarks>
	[Serializable]
	public abstract class BindableObject : INotifyPropertyChanged
	{
		#region Data

		private static readonly Dictionary<string, PropertyChangedEventArgs> EventArgCache;
	    private bool _ignorePropertyValidation = false;
	    private const string ErrorMsg = "{0} is not a public property of {1}";

		#endregion // Data

		#region Constructors

		static BindableObject()
		{
			EventArgCache = new Dictionary<string, PropertyChangedEventArgs>();
		}

		protected BindableObject()
		{
		}

		#endregion // Constructors

		#region Public Members

		/// <summary>
		/// Raised when a public property of this object is set.
		/// </summary>
		[field:NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Returns an instance of PropertyChangedEventArgs for 
		/// the specified property name.
		/// </summary>
		/// <param name="propertyName">
		/// The name of the property to create event args for.
		/// </param>		
		public static PropertyChangedEventArgs GetPropertyChangedEventArgs(string propertyName)
		{
			if (String.IsNullOrEmpty(propertyName))
				throw new ArgumentException(
					"propertyName cannot be null or empty.");

			PropertyChangedEventArgs args;

			// Get the event args from the cache, creating them
			// and adding to the cache if necessary.
			lock (typeof(BindableObject))
			{
				bool isCached = EventArgCache.ContainsKey(propertyName);
				if (!isCached)
				{
					EventArgCache.Add(
						propertyName,
						new PropertyChangedEventArgs(propertyName));
				}

				args = EventArgCache[propertyName];
			}

			return args;
		}

		#endregion // Public Members

		#region Protected Members

		/// <summary>
		/// Derived classes can override this method to
		/// execute logic after a property is set. The 
		/// base implementation does nothing.
		/// </summary>
		/// <param name="propertyName">
		/// The property which was changed.
		/// </param>
		protected virtual void AfterPropertyChanged(string propertyName)
		{
		}

		/// <summary>
		/// Attempts to raise the PropertyChanged event, and 
		/// invokes the virtual AfterPropertyChanged method, 
		/// regardless of whether the event was raised or not.
		/// </summary>
		/// <param name="propertyName">
		/// The property which was changed.
		/// </param>
		protected void RaisePropertyChanged(string propertyName)
		{
			VerifyProperty(propertyName);

			var handler = PropertyChanged;
			if (handler != null)
			{
				// Get the cached event args.
				var args = 
					GetPropertyChangedEventArgs(propertyName);

				// Raise the PropertyChanged event.
				handler(this, args);
			}

			AfterPropertyChanged(propertyName);
		}

		#endregion // Protected Members

		#region Private Helpers

		[Conditional("DEBUG")]
		private void VerifyProperty(string propertyName)
		{
		    if (IgnorePropertyValidation) return;
            var type = GetType();

			// Look for a public property with the specified name.
			var propInfo = type.GetProperty(propertyName);

		    if (propInfo != null) return;
		    
            // The property could not be found,
		    // so alert the developer of the problem.

		    var msg = string.Format(
		        ErrorMsg, 
		        propertyName, 
		        type.FullName);

		    Debug.Fail(msg);
		}

	    protected bool IgnorePropertyValidation
	    {
	        get { return _ignorePropertyValidation; }
	        set { _ignorePropertyValidation = value; }
	    }

	    #endregion // Private Helpers
	}

}
