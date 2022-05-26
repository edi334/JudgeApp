namespace JudgeApp.Core.Utils;

public static class PointsMapper
{
    private static readonly Dictionary<int, int> MapperDict = new Dictionary<int, int>
    {
        { 1, 25 },
        { 2, 18 },
        { 3, 15 },
        { 4, 12 },
        { 5, 10 },
        { 6, 8 },
        { 7, 6 },
        { 8, 4 },
        { 9, 2 },
        { 10, 1 },
    };

    public static int GetPoints(int value) => value <= 10 ? MapperDict[value] : 0;
}