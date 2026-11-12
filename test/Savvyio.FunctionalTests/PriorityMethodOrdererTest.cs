#nullable enable
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Savvyio;

/// <summary>
/// Verifies that the runner honors priority across methods, including ties and missing priorities.
/// </summary>
[TestMethodOrderer(typeof(PriorityTestMethodOrderer))]
public class PriorityMethodOrdererTest : Test
{
    private static int _completedSteps;

    /// <summary>
    /// Initializes a new instance of the <see cref="PriorityMethodOrdererTest"/> class.
    /// </summary>
    /// <param name="output">The <see cref="ITestOutputHelper"/> for the test.</param>
    public PriorityMethodOrdererTest(ITestOutputHelper output) : base(output)
    {
    }

    /// <summary>Runs first despite sorting last alphabetically.</summary>
    [Fact, Priority(-1)]
    public void Z_FirstPriority()
    {
        Assert.Equal(0, _completedSteps++);
    }

    /// <summary>Runs first among methods with the same priority.</summary>
    [Fact, Priority(0)]
    public void B_SharedPriority()
    {
        Assert.Equal(1, _completedSteps++);
    }

    /// <summary>Runs second among methods with the same priority.</summary>
    [Fact, Priority(0)]
    public void C_SharedPriority()
    {
        Assert.Equal(2, _completedSteps++);
    }

    /// <summary>Runs last despite sorting first alphabetically.</summary>
    [Fact]
    public void A_NoPriority()
    {
        Assert.Equal(3, _completedSteps++);
    }
}

/// <summary>
/// Verifies that explicit method priorities determine execution order.
/// </summary>
[TestMethodOrderer(typeof(PriorityTestMethodOrderer))]
public class PriorityMethodOrdererExplicitPriorityTest : Test
{
    private static int _completedSteps;

    /// <summary>
    /// Initializes a new instance of the <see cref="PriorityMethodOrdererExplicitPriorityTest"/> class.
    /// </summary>
    /// <param name="output">The <see cref="ITestOutputHelper"/> for the test.</param>
    public PriorityMethodOrdererExplicitPriorityTest(ITestOutputHelper output) : base(output)
    {
    }

    /// <summary>Runs before methods with higher numeric priorities.</summary>
    [Fact, Priority(0)]
    public void B_LowerPriority()
    {
        Assert.Equal(0, _completedSteps++);
    }

    /// <summary>Runs after methods with lower numeric priorities.</summary>
    [Fact, Priority(1)]
    public void C_ExplicitPriority()
    {
        Assert.Equal(1, _completedSteps++);
    }
}
