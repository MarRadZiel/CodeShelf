using System.Text;

var today = DateOnly.FromDateTime(DateTime.Today);

StringBuilder todayStringBuilder = new StringBuilder(today.ToString());
todayStringBuilder.Append($" {PolishWorkCalendar.GetPolishDayOfWeekName(today.DayOfWeek)} ({PolishWorkCalendar.GetDayType(today)})");
if (PolishWorkCalendar.IsDayAHoliday(today, ignoreNonHolidaySundays: true))
{
    var polishHolidays = PolishWorkCalendar.GetPolishHolidaysWithNames(today.Year);
    if (polishHolidays.TryGetValue(today, out string? holidayName))
    {
        todayStringBuilder.Append($" - {holidayName!}");
    }

    Console.WriteLine($"All polish non-working holidays this year:");
    foreach(var holiday in polishHolidays)
    {
        Console.WriteLine($"{holiday.Key} - {holiday.Value}");
    }
}
Console.WriteLine();

Console.WriteLine($"Today is: {todayStringBuilder}");
Console.WriteLine($"Number of workdays this month: {PolishWorkCalendar.GetWorkdayCount(today.Year, today.Month)}");

Console.WriteLine();

var firstDayOfEaster = PolishWorkCalendar.GetFirstEasterDay(today.Year);
Console.WriteLine($"Easter this year will be on: {firstDayOfEaster}");
Console.WriteLine($"Whitsun this year will be on: {PolishWorkCalendar.GetWhitsunDay(today.Year)}");
Console.WriteLine($"Corpus Christi this year will be on: {PolishWorkCalendar.GetCorpusChristiDay(firstDayOfEaster)}");

Console.ReadKey();