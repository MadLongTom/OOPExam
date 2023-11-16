// See https://aka.ms/new-console-template for more information

string? input;
List<Employee> employees = new();
while ((input = Console.ReadLine()) != string.Empty)
{
    string[] sp = input.Split('，');
    try
    {
        employees.Add(new Employee(sp[0][2..], double.Parse(sp[1][3..^1]), double.Parse(sp[2][..^3])));
    }
    catch (Exception ex)
    {
        if(ex is IndexOutOfRangeException)
            Console.WriteLine("输入的格式有误，标准格式为\"员工{ID}，每小时{SPH}元，{WH}小时；\"");
        else
        Console.WriteLine(ex.GetType() + ": " + ex.Message);
    }
}
foreach (Employee employee in employees)
{
    Console.WriteLine($"员工 {employee.Name} 所得薪资为 {employee.Salary}");
}

public struct Limitation
{
    public double Start;
    public double End;
    public double Multiply;
    public Limitation(double start, double end, double multiply)
    {
        Start = start; End = end; Multiply = multiply;
    }
    public double CalcPartSalary(double workingHour) => Math.Max(Math.Min(workingHour, End - Start), 0) * Multiply;
}

public static class SalaryLimit
{
    public static List<Limitation> salaryLimits = new() { { new(0, 40, 1) }, { new(40, 50, 1.5) }, { new(50, 60, 2) } };
}

public class Employee
{
    public readonly double WorkingHour;
    public readonly double SalaryPerHour;
    public readonly string Name;
    public double Salary { get => SalaryLimit.salaryLimits.Sum(s => s.CalcPartSalary(WorkingHour) * SalaryPerHour); }
    public Employee(string name, double salaryPerHour, double workingHour)
    {
        if (workingHour > 60) throw new OverWorkedException(name);
        if (salaryPerHour < 50) throw new SalaryTooLowException(name);
        Name = name;
        WorkingHour = workingHour;
        SalaryPerHour = salaryPerHour;
    }
}

public class OverWorkedException : Exception
{
    public OverWorkedException(string name) : base($"员工{name}的工作时间过长") { }
}

public class SalaryTooLowException : Exception
{
    public SalaryTooLowException(string name) : base($"员工{name}的时薪过低") { }
}


