using System.Reflection;

namespace Randomizer.Application;

public static class ApplicationAssemblyReference
{
    public static Assembly Reference => typeof(ApplicationAssemblyReference).Assembly;
}
