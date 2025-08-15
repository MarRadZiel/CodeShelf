# Polish Work Calendar
The static class [PolishWorkCalendar](PolishWorkCalendar.cs) provides utility methods for determining workdays, holidays, and other calendar-related information specific to Poland.

These methods can be used to:
- Determine if a given date is a workday, holiday, or weekend
- Retrieve Polish names of weekdays and holidays
- Calculate number of workdays in a given month
- Compute dates of movable feasts like Easter, Whitsun, and Corpus Christi

## Sample
Below are a few code samples demonstrating how to use provided methods.<br/>
Full sample code is available in [Sample.cs](Sample.cs)

### Sample code A
In this code fragment `GetPolishDayOfWeekName(today.DayOfWeek)` is used to retrieve the Polish name of today's day of week.<br/>
Next `GetDayType(today)` is used to return the type of the day which can be either a `Workday`, `Saturday` or a `Holiday`.<br/>
Then `IsDayAHoliday(today, ignoreNonHolidaySundays: true)` is used to determine if today is a holiday.<br/>
`PolishWorkCalendar.GetPolishHolidaysWithNames(today.Year)` returns all official non-working holidays along with their Polish names.<br/>
If today's date is found in the returned collection, the name of the holiday is appended to the output.

```csharp
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
}
Console.WriteLine($"Today is: {todayStringBuilder}");
```

#### Sample Output

<pre>
Today is: 15.08.2025 Pi¹tek (Holiday) - Wniebowziêcie Najœwiêtszej Maryi Panny
</pre>


### Sample code B
In this code fragment `GetWorkdayCount(today.Year, today.Month)` is used to get the number of working days this month.

```csharp
Console.WriteLine($"Number of workdays this month: {PolishWorkCalendar.GetWorkdayCount(today.Year, today.Month)}");
```

#### Sample Output

<pre>
Number of workdays this month: 20
</pre>

### Sample code C
In this code fragment `GetFirstEasterDay(today.Year)` is used to get the date of the first day of Easter this year.<br/>
Then, `GetWhitsunDay(today.Year)` is used to get the date of the Whitsun day. This method internally recalculates the Easter date.<br/>
Then with `GetCorpusChristiDay(firstDayOfEaster)` the date of Corpus Christi is returned based on previously calculated first day of Easter date.

```csharp
var firstDayOfEaster = PolishWorkCalendar.GetFirstEasterDay(today.Year);
Console.WriteLine($"Easter this year will be on: {firstDayOfEaster}");
Console.WriteLine($"Whitsun this year will be on: {PolishWorkCalendar.GetWhitsunDay(today.Year)}");
Console.WriteLine($"Corpus Christi this year will be on: {PolishWorkCalendar.GetCorpusChristiDay(firstDayOfEaster)}");
```

#### Sample Output

<pre>
Easter this year will be on: 20.04.2025
Whitsun this year will be on: 08.06.2025
Corpus Christi this year will be on: 19.06.2025
</pre>

