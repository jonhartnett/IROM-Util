namespace IROM.Util
{
	using System;
	using System.Collections.Generic;
	
	/// <summary>
	/// Stores the state of every key and mouse button.
	/// </summary>
	public class InputState
	{	
		/// <summary>
		/// The state of each mouse button.
		/// </summary>
		private readonly Dictionary<MouseButton, bool> BaseMouseStates = new Dictionary<MouseButton, bool>();
		
		/// <summary>
		/// The state of each key.
		/// </summary>
		private readonly Dictionary<KeyboardButton, bool> BaseKeyStates = new Dictionary<KeyboardButton, bool>();
		
		/// <summary>
		/// The current location of the mouse.
		/// </summary>
		public Point2D MousePosition
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Returns the state of the given <see cref="MouseButton"/>.
		/// </summary>
		public bool this[MouseButton button]
		{
			get
			{
				bool result;
				BaseMouseStates.TryGetValue(button, out result);
				return result;
			}
			private set
			{
				BaseMouseStates[button] = value;
			}
		}
		
		/// <summary>
		/// Returns the state of the given <see cref="KeyboardButton"/>.
		/// </summary>
		public bool this[KeyboardButton button]
		{
			get
			{
				bool result;
				BaseKeyStates.TryGetValue(button, out result);
				return result;
			}
			private set
			{
				BaseKeyStates[button] = value;
			}
		}
		
		public InputState(Window window)
		{
			//register with window
			window.OnMouseMove += (coords, delta) => this.MousePosition = coords;
			window.OnMousePress += button => this[button] = true;
			window.OnMouseRelease += button => this[button] = false;
			window.OnKeyPress += button => this[button] = true;
			window.OnKeyRelease += button => this[button] = false;
		}
	}
}
