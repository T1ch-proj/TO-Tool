using Xunit;

public class PlayerInfoControlTests
{
    [Fact]
    public void UpdateInfo_UpdatesAllValues()
    {
        // Arrange
        var control = new PlayerInfoControl();
        
        // Act
        control.UpdateInfo(100, 200, 50, 100, 1000, 2000);

        // Assert
        Assert.Equal(50, control.HPPercentage);
        Assert.Equal("100/200", control.HPText);
        Assert.Equal(50, control.MPPercentage);
        Assert.Equal("50/100", control.MPText);
        Assert.Equal(50, control.EXPPercentage);
        Assert.Equal("1000/2000", control.EXPText);
    }
} 