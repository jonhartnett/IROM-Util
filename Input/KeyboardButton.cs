namespace IROM.Util
{
	using System;
	using System.Runtime.InteropServices;
	
	/// <summary>
	/// Represents the available virtual keyboard keys.
	/// </summary>
	public enum KeyboardButton : uint
	{
		BACKSPACE = 0x08,
		TAB = 0x09,
		ENTER = 0x0D,
		SHIFT = 0x10,
		CTRL = 0x11,
		ALT = 0x12,
		PAUSE = 0x13,
		CAPS_LOCK = 0x14, 
		ESCAPE = 0x1B,
		SPACE = 0x20,
		PAGE_UP = 0x21,
		PAGE_DOWN = 0x22,
		END = 0x23,
		HOME = 0x24,
		LEFT = 0x25,
		UP = 0x26,
		RIGHT = 0x27,
		DOWN = 0x28,
		PRINT_SCREEN = 0x2C,
		INSERT = 0x2D,
		DELETE = 0x2E,
		ZERO = 0x30,
		ONE = 0x31,
		TWO = 0x32,
		THREE = 0x33,
		FOUR = 0x34,
		FIVE = 0x35,
		SIX = 0x36,
		SEVEN = 0x37,
		EIGHT = 0x38,
		NINE = 0x39,
		A = 0x41,
		B = 0x42,
		C = 0x43,
		D = 0x44,
		E = 0x45,
		F = 0x46,
		G = 0x47, 
		H = 0x48,
		I = 0x49,
		J = 0x4A,
		K = 0x4B,
		L = 0x4C,
		M = 0x4D,
		N = 0x4E,
		O = 0x4F,
		P = 0x50,
		Q = 0x51,
		R = 0x52,
		S = 0x53,
		T = 0x54,
		U = 0x55,
		V = 0x56,
		W = 0x57,
		X = 0x58,
		Y = 0x59,
		Z = 0x5A,
		LEFT_WINDOWS = 0x5B,
		RIGHT_WINDOWS = 0x5C,
		APPLICATION = 0x5D,
		NUMPAD_ZERO = 0x60,
		NUMPAD_ONE = 0x61,
		NUMPAD_TWO = 0x62,
		NUMPAD_THREE = 0x63,
		NUMPAD_FOUR = 0x64,
		NUMPAD_FIVE = 0x65,
		NUMPAD_SIX = 0x66,
		NUMPAD_SEVEN = 0x67,
		NUMPAD_EIGHT = 0x68,
		NUMPAD_NINE = 0x69,
		MULTIPLY = 0x6A,
		ADD = 0x6B,
		SEPARATOR = 0x6C,
		SUBTRACT = 0x6D,
		DECIMAL = 0x6E,
		DIVIDE = 0x6F,
		F1 = 0x70,
		F2 = 0x71,
		F3 = 0x72,
		F4 = 0x73,
		F5 = 0x74,
		F6 = 0x75,
		F7 = 0x76,
		F8 = 0x77,
		F9 = 0x78,
		F10 = 0x79,
		F11 = 0x7A,
		F12 = 0x7B,
		F13 = 0x7C,
		F14 = 0x7D,
		F15 = 0x7E,
		F16 = 0x7F,
		F17 = 0x80,
		F18 = 0x81,
		F19 = 0x82,
		F20 = 0x83,
		F21 = 0x84,
		F22 = 0x85,
		F23 = 0x86,
		F24 = 0x87,
		NUM_LOCK = 0x90,
		SCROLL_LOCK = 0x91,
		LEFT_SHIFT = 0xA0,
		RIGHT_SHIFT = 0xA1,
		LEFT_CTRL = 0xA2,
		RIGHT_CTRL = 0xA3,
		LEFT_ALT = 0xA4,
		RIGHT_ALT = 0xA5,
		VOLUME_MUTE = 0xAD,
		VOLUME_DOWN = 0xAE,
		VOLUME_UP = 0xAF,
		MEDIA_NEXT = 0xB0,
		MEDIA_PREV = 0xB1,
		MEDIA_STOP = 0xB2,
		MEDIA_PLAY_PAUSE = 0xB3,
		OEM_PLUS = 0xBB,
		OEM_COMMA = 0xBC,
		OEM_MINUS = 0xBD,
		OEM_PERIOD = 0xBE,
		OEM_1 = 0xBA,
		OEM_2 = 0xBF,
		OEM_3 = 0xC0,
		OEM_4 = 0xDB,
		OEM_5 = 0xDC,
		OEM_6 = 0xDD,
		OEM_7 = 0xDE,
		OEM_8 = 0xDF,
		OEM_102 = 0xE2,
	}
	
	public static class KeyboardButtonExtensions
	{
		/// <summary>
		/// Returns the char for the given <see cref="KeyboardButton"/>.
		/// </summary>
		/// <param name="button">The button.</param>
		/// <returns>The char value.</returns>
		public static char GetCharacter(this KeyboardButton button)
		{
			return (char)MapVirtualKey((uint)button, MAPVK_VK_TO_CHAR);
		}
		
		//winapi constants
		private const int MAPVK_VK_TO_CHAR = 2;
		
		//winapi methods
		[DllImport("user32.dll")] 
		private static extern int MapVirtualKey(uint uCode, uint uMapType);
	}
}
