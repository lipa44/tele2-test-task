namespace WebApi.Filters;

using Domain.Exceptions;

public record AgeRangeFilter
{
    private const int DefaultAgeStart = 0;
    private const int DefaultAgeEnd = int.MaxValue;

    public AgeRangeFilter(int ageStart, int ageEnd)
    {
        if (ageStart < 0) throw new Tele2Exception("Age start must be positive");

        Start = ageStart == 0 ? DefaultAgeStart : ageStart;
        End = ageEnd == 0 ? DefaultAgeEnd : ageEnd;
    }

    public int Start { get; }
    public int End { get; }

    public bool IsValid => End != DefaultAgeEnd || Start != DefaultAgeStart;
}