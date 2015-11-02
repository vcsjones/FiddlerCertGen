namespace VCSJones.FiddlerCertGen
open System.Runtime.InteropServices

[<StructAttribute; StructLayoutAttribute(LayoutKind.Sequential)>]
type SYSTEMTIME =
    val wYear : uint16
    val wMonth : uint16
    val wDayOfWeek : uint16
    val wDay : uint16
    val wHour : uint16
    val wMinute : uint16
    val wSecond : uint16
    val wMilliseconds : uint16

    new (dt : System.DateTime) = {
            wYear = uint16 dt.Year
            wMonth = uint16 dt.Month
            wDayOfWeek = uint16 dt.DayOfWeek
            wDay = uint16 dt.Day
            wHour = uint16 dt.Day
            wMinute = uint16 dt.Minute
            wSecond = uint16 dt.Second
            wMilliseconds = uint16 dt.Millisecond
        }

