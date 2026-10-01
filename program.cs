====================================================
Session 09 - Assignment Solutions
====================================================

Project structure:
- Console project: Session09-Assignment
- Test project: Session09-Assignment.Tests (xUnit, with a project reference to the console project)


====================================================
Part 1: Primary Constructor & Records
====================================================

-------------------- Q1: Patient class --------------------
class Patient(int Id, string FullName, string PhoneNumber, string MedicalHistory)
{
    public int Id { get; } = Id;
    public string FullName { get; } = FullName;
    public string PhoneNumber { get; } = PhoneNumber;
    public string MedicalHistory { get; } = MedicalHistory;

    public override string ToString()
    {
        return $"Id: {Id} :: FullName: {FullName} :: PhoneNumber: {PhoneNumber} :: MedicalHistory: {MedicalHistory}";
    }
}


-------------------- Q2: compare two Patient objects --------------------
Patient patient01 = new Patient(1, "Ali Hassan", "0100000000", "No allergies");
Patient patient02 = new Patient(1, "Ali Hassan", "0100000000", "No allergies");

Console.WriteLine(patient01.GetHashCode());
Console.WriteLine(patient02.GetHashCode());
// Different hash codes. Patient is a normal class, so GetHashCode() is not overridden,
// and the default implementation is based on the object's identity (reference), not its data.

Console.WriteLine(patient01.Equals(patient02));
// False. The default Equals() for a class also compares references, and patient01/patient02
// are two separate objects in memory, even though their property values are identical.

patient01 = patient02;
Console.WriteLine(patient01.Equals(patient02));
// True now. patient01 was reassigned to point to the same object as patient02,
// so both variables now reference the exact same object.


-------------------- Q3: positional record PatientDto --------------------
record PatientDto(int Id, string FullName, string PhoneNumber);

PatientDto dto01 = new PatientDto(1, "Ali Hassan", "0100000000");
PatientDto dto02 = new PatientDto(1, "Ali Hassan", "0100000000");

Console.WriteLine(dto01.GetHashCode());
Console.WriteLine(dto02.GetHashCode());
// Equal. Records auto-generate GetHashCode() based on the values of their properties.

Console.WriteLine(dto01.Equals(dto02));
// True. Records use value-based equality by default, so two records with the same
// property values are considered equal even though they are different objects.

// Comment - difference between class and record:
// Patient (class) -> Equals/GetHashCode compare references (identity). Two objects with
// the same data are still "not equal" unless they point to the same memory location.
// PatientDto (record) -> Equals/GetHashCode compare the actual property values.
// Two records with identical data are "equal" even as separate objects.


-------------------- Q4: PatientMapper --------------------
static class PatientMapper
{
    public static PatientDto MapFromModelToDto(Patient patient)
    {
        return new PatientDto(patient.Id, patient.FullName, patient.PhoneNumber);
    }
}

// usage in Main:
PatientDto mapped = PatientMapper.MapFromModelToDto(patient01);
Console.WriteLine(mapped);


====================================================
Part 2: Singleton
====================================================

-------------------- Q5: AppLogger --------------------
class AppLogger
{
    private static AppLogger? _instance;

    private AppLogger() { }

    public static AppLogger GetLogger()
    {
        if (_instance == null)
            _instance = new AppLogger();

        return _instance;
    }
}


-------------------- Q6: test the Singleton --------------------
AppLogger logger1 = AppLogger.GetLogger();
AppLogger logger2 = AppLogger.GetLogger();
AppLogger logger3 = AppLogger.GetLogger();
AppLogger logger4 = AppLogger.GetLogger();

Console.WriteLine(logger1.GetHashCode());
Console.WriteLine(logger2.GetHashCode());
Console.WriteLine(logger3.GetHashCode());
Console.WriteLine(logger4.GetHashCode());

// All four hash codes are the same. GetLogger() only creates a new AppLogger the first
// time it's called (when _instance is still null). Every call after that returns the
// same stored instance, so all four variables point to the exact same object.


====================================================
Part 3: var & dynamic
====================================================

-------------------- Q7 --------------------
Patient p1 = new(2, "Mona Adel", "0111111111", "Diabetes");   // target-typed new
var p2 = new Patient(3, "Omar Tarek", "0122222222", "None");  // var

dynamic p3 = new Patient(4, "Laila Fathy", "0133333333", "Asthma");
Console.WriteLine(p3);

// Comment - var vs dynamic:
// var: the compiler figures out the actual type at COMPILE TIME, from whatever is on
// the right side of the assignment. After that, it behaves like a normal, fixed type,
// and the compiler still checks member access for errors.
// dynamic: the type is resolved at RUN TIME. The compiler skips type-checking entirely,
// so errors (like calling a method that doesn't exist) only show up when the code runs.


====================================================
Part 4: Anonymous Types
====================================================

-------------------- Q8 --------------------
var doctor01 = new { Name = "Sara", Specialty = "Cardiology", ExperienceYears = 8, Salary = 25_000 };
var doctor02 = new { Name = "Sara", Specialty = "Cardiology", ExperienceYears = 8, Salary = 25_000 };

Console.WriteLine(doctor01.Name);
Console.WriteLine(doctor01.Specialty);

Console.WriteLine(doctor01.GetHashCode());
Console.WriteLine(doctor02.GetHashCode());
// Equal, because anonymous types generate value-based GetHashCode(), similar to records.

Console.WriteLine(doctor01.GetType());
// Shows a compiler-generated type name, something like:
// <>f__AnonymousType0`4[System.String,System.String,System.Int32,System.Int32]

Console.WriteLine(doctor01.Equals(doctor02));
// True, because both objects have the same anonymous type (same property names, types,
// and order) and the same values.

Console.WriteLine(doctor01.ToString());
// Prints something like: { Name = Sara, Specialty = Cardiology, ExperienceYears = 8, Salary = 25000 }

// Comment - anonymous type equality vs class vs record:
// Class (default): Equals/GetHashCode compare references. Two objects with the same data
// are "not equal" unless they are literally the same object.
// Record: Equals/GetHashCode compare property values automatically (value equality).
// Anonymous type: behaves the same as a record here - equality and hash code are based
// on the values of its properties, as long as the two anonymous objects have the same
// property names/types/order.


====================================================
Part 5: Extension Methods
====================================================

-------------------- Q9 --------------------
static class TextHelper
{
    // plain static method version (bonus step, before converting to extension method)
    public static bool IsShorterThanStatic(string value, int length)
    {
        return value.Length < length;
    }

    // extension method version - the "this" keyword on the first parameter
    // is what turns it into an extension method on string
    public static bool IsShorterThan(this string value, int length)
    {
        return value.Length < length;
    }

    public static string Repeat(this string value, int times)
    {
        string result = "";
        for (int i = 0; i < times; i++)
        {
            result += value;
        }
        return result;
    }
}


-------------------- Q10 --------------------
Console.WriteLine("Stethoscope".IsShorterThan(5)); // false
Console.WriteLine("Ab".Repeat(4));                  // AbAbAbAb


====================================================
Part 6: Unit Testing (xUnit)
====================================================

-------------------- Q11: FineCalculator (console project) --------------------
public class FineCalculator
{
    public int Add(int a, int b) => a + b;

    public int Subtract(int a, int b) => a - b;

    public int Multiply(int a, int b) => a * b;

    public int Divide(int a, int b)
    {
        if (b == 0)
            throw new DivideByZeroException();

        return a / b;
    }
}


-------------------- Q12: FineCalculatorTests (xUnit test project) --------------------
using Xunit;

public class FineCalculatorTests
{
    private readonly FineCalculator _calculator;

    public FineCalculatorTests()
    {
        _calculator = new FineCalculator();
    }

    [Fact]
    public void Add_ShouldReturnCorrectSum()
    {
        // Arrange
        int a = 5, b = 3;

        // Act
        int result = _calculator.Add(a, b);

        // Assert
        Assert.Equal(8, result);
    }

    [Fact]
    public void Add_ShouldReturnNotEqual()
    {
        // Arrange
        int a = 5, b = 3;

        // Act
        int result = _calculator.Add(a, b);

        // Assert
        Assert.NotEqual(10, result);
    }

    [Fact]
    public void Subtract_ShouldReturnCorrectDifference()
    {
        // Arrange
        int a = 10, b = 4;

        // Act
        int result = _calculator.Subtract(a, b);

        // Assert
        Assert.Equal(6, result);
    }

    [Theory]
    [InlineData(2, 3, 6)]
    [InlineData(5, 5, 25)]
    [InlineData(-2, 4, -8)]
    public void Multiply_ShouldReturnCorrectResult(int a, int b, int expected)
    {
        // Act
        int result = _calculator.Multiply(a, b);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(2, 3, 10)]
    [InlineData(5, 5, 20)]
    [InlineData(-2, 4, 0)]
    public void Multiply_ShouldReturnNotEqual(int a, int b, int notExpected)
    {
        // Act
        int result = _calculator.Multiply(a, b);

        // Assert
        Assert.NotEqual(notExpected, result);
    }

    [Fact]
    public void Divide_ByZero_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => _calculator.Divide(10, 0));
    }
}
