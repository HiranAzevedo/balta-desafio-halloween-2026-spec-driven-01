namespace PasswordGenerator.Tests;

public class ProjectScaffoldTests
{
    [Fact]
    public void ApiProject_HasExecutableEntryPoint()
    {
        Assert.NotNull(typeof(global::Program).Assembly.EntryPoint);
    }
}