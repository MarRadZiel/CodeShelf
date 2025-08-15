/// <summary>Static class containing methods for retrieving information about polish holidays.</summary>
public static class PolishWorkCalendar
{
    /// <summary>Determines type of day based on specified date.<br/>
    /// Day can be either a Holiday, Saturday or a Workday.</summary>
    /// <param name="date">Date to get day type of.</param>
    public static DayType GetDayType(DateOnly date)
    {
        return IsDayAHoliday(date) ? DayType.Holiday : (date.DayOfWeek == DayOfWeek.Saturday ? DayType.Saturday : DayType.Workday);
    }

    /// <summary>Is day a non-working holiday in polish callendar (including all Sundays).</summary>
    /// <param name="date">Date to check.</param>
    /// <param name="ignoreNonHolidaySundays">Ignores non-holiday Sundays.</param>
    /// <returns>True if specified day is a non-working holiday. False otherwise.</returns>
    public static bool IsDayAHoliday(DateOnly date, bool ignoreNonHolidaySundays = false)
    {
        if (!ignoreNonHolidaySundays && date.DayOfWeek == DayOfWeek.Sunday) return true;
        else if (date.Month == 1) // January
        {
            if (date.Day == 1) return true; // 1 Jan
            else if (date.Day == 6) return true; // 6 Jan
        }
        else if (date.Month == 5) // May
        {
            if (date.Day == 1) return true; // 1 May
            else if (date.Day == 3) return true; // 3 May
        }
        else if (date.Month == 8) // August
        {
            if (date.Day == 15) return true; // 15 Aug
        }
        else if (date.Month == 11) // November
        {
            if (date.Day == 1) return true; // 1 Nov
            if (date.Day == 11) return true; // 11 Nov
        }
        else if (date.Month == 12) // December
        {
            if (date.Year >= 2025 && date.Day == 24) return true; // 24 Dec
            if (date.Day == 25) return true; // 25 Dec
            if (date.Day == 26) return true; // 26 Dec
        }

        DateOnly easterDay = GetFirstEasterDay(date.Year);

        if (date.Month == easterDay.Month && date.Day == easterDay.Day) return true; // first Easter day
        DateOnly secondEasterDay = GetSecondEasterDay(easterDay);
        if (date.Month == secondEasterDay.Month && date.Day == secondEasterDay.Day) return true; // second Easter day

        DateOnly zsDay = GetWhitsunDay(easterDay);
        if (date.Month == zsDay.Month && date.Day == zsDay.Day) return true; // Zielone Swiatki day

        DateOnly bcDay = GetCorpusChristiDay(easterDay);
        if (date.Month == bcDay.Month && date.Day == bcDay.Day) return true; // Boze Cialo day

        return false;
    }

    /// <summary>Gets all non-working polish holidays with their names.</summary>
    /// <param name="year">Year to get holidays for.</param>
    /// <returns>Dictionary with date-holiday_name pairs.</returns>
    public static Dictionary<DateOnly, string> GetPolishHolidaysWithNames(int year)
    {
        if (year >= 2025)
        {
            return new Dictionary<DateOnly, string>()
            {
                { new DateOnly(year, 1, 1 ), "Nowy Rok" },
                { new DateOnly(year, 1, 6 ), "Trzech Króli" },
                { GetFirstEasterDay(year), "Pierwszy Dzień Wielkiej Nocy" },
                { GetSecondEasterDay(year), "Drugi Dzień Wielkiej Nocy" },
                { new DateOnly(year, 5, 1 ), "Święto Pracy" },
                { new DateOnly(year, 5, 3 ), "Święto Narodowe Trzeciego Maja" },
                { GetWhitsunDay(year), "Zielone Świątki" },
                { GetCorpusChristiDay(year), "Boże Ciało" },
                { new DateOnly(year, 8, 15 ), "Wniebowzięcie Najświętszej Maryi Panny" },
                { new DateOnly(year, 11, 1 ), "Wszystkich Świętych" },
                { new DateOnly(year, 11, 11 ), "Narodowe Święto Niepodległości" },
                {new DateOnly(year, 12, 24), "Wigilia Bożego Narodzenia" },
                { new DateOnly(year, 12, 25 ), "Pierwszy Dzień Bożego Narodzenia" },
                { new DateOnly(year, 12, 26 ), "Drugi Dzień Bożego Narodzenia" },
            };
        }
        else
        {
            return new Dictionary<DateOnly, string>()
            {
                { new DateOnly(year, 1, 1 ), "Nowy Rok" },
                { new DateOnly(year, 1, 6 ), "Trzech Króli" },
                { GetFirstEasterDay(year), "Pierwszy Dzień Wielkiej Nocy" },
                { GetSecondEasterDay(year), "Drugi Dzień Wielkiej Nocy" },
                { new DateOnly(year, 5, 1 ), "Święto Pracy" },
                { new DateOnly(year, 5, 3 ), "Święto Narodowe Trzeciego Maja" },
                { GetWhitsunDay(year), "Zielone Świątki" },
                { GetCorpusChristiDay(year), "Boże Ciało" },
                { new DateOnly(year, 8, 15 ), "Wniebowzięcie Najświętszej Maryi Panny" },
                { new DateOnly(year, 11, 1 ), "Wszystkich Świętych" },
                { new DateOnly(year, 11, 11 ), "Narodowe Święto Niepodległości" },
                { new DateOnly(year, 12, 25 ), "Pierwszy Dzień Bożego Narodzenia" },
                { new DateOnly(year, 12, 26 ), "Drugi Dzień Bożego Narodzenia" },
            };
        }
    }
    /// <summary>Gets all non-working polish holidays.</summary>
    /// <param name="year">Year to get holidays for.</param>
    public static DateOnly[] GetPolishHolidays(int year)
    {
        if (year >= 2025)
        {
            return new DateOnly[]
            {
                new DateOnly(year, 1, 1 ),
                new DateOnly(year, 1, 6 ),
                GetFirstEasterDay(year),
                GetSecondEasterDay(year),
                new DateOnly(year, 5, 1 ),
                new DateOnly(year, 5, 3 ),
                GetWhitsunDay(year),
                GetCorpusChristiDay(year),
                new DateOnly(year, 8, 15 ),
                new DateOnly(year, 11, 1 ),
                new DateOnly(year, 11, 11 ),
                new DateOnly(year, 12, 24),
                new DateOnly(year, 12, 25 ),
                new DateOnly(year, 12, 26 ),
            };
        }
        else
        {
            return new DateOnly[]
            {
                new DateOnly(year, 1, 1 ),
                new DateOnly(year, 1, 6 ),
                GetFirstEasterDay(year),
                GetSecondEasterDay(year),
                new DateOnly(year, 5, 1 ),
                new DateOnly(year, 5, 3 ),
                GetWhitsunDay(year),
                GetCorpusChristiDay(year),
                new DateOnly(year, 8, 15 ),
                new DateOnly(year, 11, 1 ),
                new DateOnly(year, 11, 11 ),
                new DateOnly(year, 12, 25 ),
                new DateOnly(year, 12, 26 ),
            };
        }
    }

    /// <summary>Gets date for Easter holiday.<br/>
    /// Calculated based on <see href="https://pl.wikipedia.org/wiki/Wielkanoc#Wyznaczanie_daty_Wielkanocy_w_danym_roku"></see></summary>
    /// <param name="year">Year for which to calculate the holiday date.</param>
    public static DateOnly GetFirstEasterDay(int year)
    {
        const int A = 25;
        const int B = 5;

        int a = year % 19;
        int b = year % 4;
        int c = year % 7;
        int d = (a * 19 + A) % 30;

        int e = ((2 * b) + (4 * c) + (6 * d) + B) % 7;
        int day = 22 + (d + e);
        if (e == 6)
        {
            if (d == 29 || a > 10)
            {
                day -= 7;
            }
        }
        int month = day > 31 ? 4 : 3;
        if (day > 31) day -= 31;

        return new DateOnly(year, month, day);
    }
    /// <summary>Gets date for the second day of Easter in the specified year.<br/>1 day after first day of Easter.</summary>
    /// <param name="year">Year for which to calculate the holiday date.</param>
    public static DateOnly GetSecondEasterDay(int year)
    {
        return GetSecondEasterDay(GetFirstEasterDay(year));
    }
    /// <summary>Gets date for  the second day of Easter based on first day of Easter date.<br/>1 day after first day of Easter.</summary>
    /// <param name="easterDate">Precalculated date of the first day of Easter.</param>
    public static DateOnly GetSecondEasterDay(DateOnly easterDate)
    {
        return easterDate.AddDays(1);
    }
    /// <summary>Gets date for Whitsun holiday in the specified year.<br/>49 days after first day of Easter.</summary>
    /// <param name="year">Year for which to calculate the holiday date.</param>
    public static DateOnly GetWhitsunDay(int year)
    {
        return GetWhitsunDay(GetFirstEasterDay(year));
    }
    /// <summary>Gets date for Whitsun holiday based on first day of Easter date.<br/>49 days after first day of Easter.</summary>
    /// <param name="easterDate">Precalculated date of the first day of Easter.</param>
    public static DateOnly GetWhitsunDay(DateOnly easterDate)
    {
        return easterDate.AddDays(49);
    }
    /// <summary>Gets date for Corpus Christi holiday in the specified year.<br/>60 days after first day of Easter.</summary>
    /// <param name="year">Year for which to calculate the holiday date.</param>
    public static DateOnly GetCorpusChristiDay(int year)
    {
        return GetCorpusChristiDay(GetFirstEasterDay(year));
    }
    /// <summary>Gets date for Corpus Christi holiday based on first day of Easter date.<br/>60 days after first day of Easter.</summary>
    /// <param name="easterDate">Precalculated date of the first day of Easter.</param>
    public static DateOnly GetCorpusChristiDay(DateOnly easterDate)
    {
        return easterDate.AddDays(60);
    }

    /// <summary>Returns the number of actual workdays in the specified month, taking into account non-working holidays that fall on Saturdays.</summary>
    /// <param name="year">Year to get workdays for.</param>
    /// <param name="month">Month to get workdays for.</param>
    /// <returns>Number of work days minus work-free holidays that were on Saturdays.</returns>
    public static int GetWorkdayCount(int year, int month)
    {
        int workdayCount = 0;
        DateOnly currentDay = new DateOnly(year, month, 1);
        do
        {
            if (GetDayType(currentDay) == DayType.Workday)
            {
                ++workdayCount;
            }
            currentDay = currentDay.AddDays(1);
        }
        while (currentDay.Month == month);

        var holidays = GetPolishHolidays(year);
        foreach (var holidayDate in holidays)
        {
            if (holidayDate.Month == month)
            {
                if (holidayDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    --workdayCount;
                }
            }
        }
        return workdayCount;
    }

    /// <summary>Returns the polish name of the specified day of week.</summary>
    /// <param name="dayOfWeek">Day of week to get polish name for.</param>
    public static string GetPolishDayOfWeekName(DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Monday => "Poniedziałek",
            DayOfWeek.Tuesday => "Wtorek",
            DayOfWeek.Wednesday => "Środa",
            DayOfWeek.Thursday => "Czwartek",
            DayOfWeek.Friday => "Piątek",
            DayOfWeek.Saturday => "Sobota",
            _ => "Niedziela",
        };
    }
    /// <summary>Returns the polish name of the specified month.</summary>
    /// <param name="month">Month to get polish name for.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when month parameter value is out of range (1-12).</exception>
    public static string GetPolishMonthName(int month)
    {
        if (month < 1 || month > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 and 12.");
        }
        return month switch
        {
            1 => "Styczeń",
            2 => "Luty",
            3 => "Marzec",
            4 => "Kwiecień",
            5 => "Maj",
            6 => "Czerwiec",
            7 => "Lipiec",
            8 => "Sierpień",
            9 => "Wrzesień",
            10 => "Październik",
            11 => "Listopad",
            _ => "Grudzień",
        };
    }
}

/// <summary>Type of day determining if day is a normal work day or not.</summary>
public enum DayType
{
    /// <summary>Normal working day.</summary>
    Workday,
    /// <summary>Saturday.</summary>
    Saturday,
    /// <summary>Non-working holiday or Sunday.</summary>
    Holiday
}