namespace SchoolManagement.Application.Common;

public interface IAverageCalculator
{
    float GetAverage(IEnumerable<int> values);
    float GetAverage(IEnumerable<float> values);
}

public class AverageCalculator : IAverageCalculator
{
    public float GetAverage(IEnumerable<int> values)
    {
        var list = values as IList<int> ?? values.ToList();
        return list.Count == 0 ? 0f : (float)list.Average();
    }

    public float GetAverage(IEnumerable<float> values)
    {
        var list = values as IList<float> ?? values.ToList();
        return list.Count == 0 ? 0f : list.Average();
    }
}