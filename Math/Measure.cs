namespace IROM.Util
{
	using System;
	using System.Linq;
	
    /// <summary>
    /// Stores the values of common measurements in the world.
    /// </summary>
    public static class Measure
    {
	    //time
	    public const double Second = 1;
	
	    public const double Nanosecond = Second * 1E-9;
	    public const double Millisecond = Second / 1000D;
	    public const double Minute = Second * 60;
	    public const double Hour = Minute * 60;
	    public const double Day = Hour * 24;
	    public const double Week = Day * 7;
	    public const double Year = Day * 365;
	
	    //time squared
	    public const double SecondSq = Second * Second;
	
	    public const double NanosecondSq = Nanosecond * Nanosecond;
	    public const double MillisecondSq = Millisecond * Millisecond;
	    public const double MinuteSq = Minute * Minute;
	    public const double HourSq = Hour * Hour;
	    public const double DaySq = Day * Day;
	    public const double WeekSq = Week * Week;
	    public const double YearSq = Year * Year;
	
	    //distance
	    public const double Meter = 1;
	
	    public const double Millimeter = Meter / 1000D;
	    public const double Centimeter = Meter / 100D;
	    public const double Kilometer = Meter * 1000;
	
	    public const double Foot = Meter * 0.3048;
	
	    public const double Inch = Foot / 12D;
	    public const double Yard = Foot * 3;
	    public const double Mile = Foot * 5280;
	
	    //area
	    public const double MeterSq = Meter * Meter;
	
	    public const double MillimeterSq = Millimeter * Millimeter;
	    public const double CentimeterSq = Centimeter * Centimeter;
	    public const double KilometerSq = Kilometer * Kilometer;
	
	    public const double FootSq = Foot * Foot;
	
	    public const double InchSq = Inch * Inch;
	    public const double YardSq = Yard * Yard;
	    public const double MileSq = Mile * Mile;
	
	    //volume
	    public const double MeterCu = Meter * Meter * Meter;
	
	    public const double MillimeterCu = Millimeter * Millimeter * Millimeter;
	    public const double CentimeterCu = Centimeter * Centimeter * Centimeter;
	    public const double KilometerCu = Kilometer * Kilometer * Kilometer;
	
	    public const double FootCu = Foot * Foot * Foot;
	
	    public const double InchCu = Inch * Inch * Inch;
	    public const double YardCu = Yard * Yard * Yard;
	    public const double MileCu = Mile * Mile * Mile;
	
	    //mass
	    public const double Gram = 1;
	
	    public const double Kilogram = Gram * 1000;
	    public const double Slug = 14.5939 * Kilogram;
	
	    //force
	    public const double Newton = Kilogram * Meter / SecondSq;
	
	    public const double Pound = Slug * Foot / SecondSq;
    }
}
