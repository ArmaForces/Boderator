namespace AttendanceExtractor;

public class AttendanceEqualityComparer : IEqualityComparer<Attendance>
{
    public bool Equals(Attendance? x, Attendance? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        return x.GetType() == y.GetType() && x.Id.Equals(y.Id);
    }

    public int GetHashCode(Attendance obj)
    {
        return obj.Id.GetHashCode();
    }
}