namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Arguments for an input event.
	/// </summary>
	public class InputEventArgs : EventArgs
	{
		/// <summary>
		/// True if the event should be consumed.
		/// </summary>
		public bool Consumed;
		
		public InputEventArgs()
		{
			Consumed = false;
		}
		
		public InputEventArgs(InputEventArgs args)
		{
			Consumed = args.Consumed;
		}
	}
	
	/// <summary>
	/// Arguments for a mouse button event.
	/// </summary>
	public class MouseButtonEventArgs : InputEventArgs
	{
		/// <summary>
		/// The coordinates of the mouse at the time of the event.
		/// </summary>
		public Point2D Coords;
		
		/// <summary>
		/// The button pressed or released.
		/// </summary>
		public MouseButton Button;
		
		/// <summary>
		/// True for press, false for release.
		/// </summary>
		public bool Pressed;
		
		public MouseButtonEventArgs(Point2D coords, MouseButton button, bool pressed)
		{
			Coords = coords;
			Button = button;
			Pressed = pressed;
		}
		
		public MouseButtonEventArgs(MouseButtonEventArgs args) : base(args)
		{
			Coords = args.Coords;
			Button = args.Button;
			Pressed = args.Pressed;
		}
	}
	
	/// <summary>
	/// Arguments for a mouse move event.
	/// </summary>
	public class MouseMoveEventArgs : InputEventArgs
	{
		/// <summary>
		/// The coordinates of the mouse at the time of the event.
		/// </summary>
		public Point2D Coords;
		/// <summary>
		/// The amount the mouse moved to get to the current position.
		/// </summary>
		public Point2D Delta;
		
		public MouseMoveEventArgs(Point2D coords, Point2D delta)
		{
			Coords = coords;
			Delta = delta;
		}
		
		public MouseMoveEventArgs(MouseMoveEventArgs args) : base(args)
		{
			Coords = args.Coords;
			Delta = args.Delta;
		}
	}

	/// <summary>
	/// Arguments for a mouse wheel event.
	/// </summary>
	public class MouseWheelEventArgs : InputEventArgs
	{
		/// <summary>
		/// The coordinates of the mouse at the time of the event.
		/// </summary>
		public Point2D Coords;
		/// <summary>
		/// The "amount" the wheel rotated. Positive is up, negative is down.
		/// </summary>
		public int Delta;
		
		public MouseWheelEventArgs(Point2D coords, int delta)
		{
			Coords = coords;
			Delta = delta;
		}
		
		public MouseWheelEventArgs(MouseWheelEventArgs args) : base(args)
		{
			Coords = args.Coords;
			Delta = args.Delta;
		}
	}
	
	/// <summary>
	/// Arguments for a key event.
	/// </summary>
	public class KeyEventArgs : InputEventArgs
	{
		/// <summary>
		/// The button pressed or released.
		/// </summary>
		public KeyboardButton Button;
		
		/// <summary>
		/// True for press, false for release.
		/// </summary>
		public bool Pressed;
		
		public KeyEventArgs(KeyboardButton button, bool pressed)
		{
			Button = button;
			Pressed = pressed;
		}
		
		public KeyEventArgs(KeyEventArgs args) : base(args)
		{
			Button = args.Button;
			Pressed = args.Pressed;
		}
	}
	
	/// <summary>
	/// Arguments for a character event.
	/// </summary>
	public class CharEventArgs : InputEventArgs
	{
		/// <summary>
		/// The character typed.
		/// </summary>
		public char Character;
		
		/// <summary>
		/// True if this event is a repeat event due to holding down the key.
		/// </summary>
		public bool IsRepeat;
		
		public CharEventArgs(char character, bool repeat)
		{
			Character = character;
			IsRepeat = repeat;
		}
		
		public CharEventArgs(CharEventArgs args) : base(args)
		{
			Character = args.Character;
		}
	}
}
