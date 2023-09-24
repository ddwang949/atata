﻿namespace Atata.IntegrationTests.Controls;

[TestFixture]
public class ControlDelegateTests : UITestFixture
{
    [Test]
    public void WithoutNavigation() =>
        Go.To<BasicControlsPage>()
            .RawButton.Should().BePresent()
            .RawButton.Should().BeEnabled()
            .RawButton.Content().Should.Equal("Raw Button")
            .RawButton()
            .InputButton.Should().BePresent()
            .InputButton()
            .Do(_ => _.LinkButton, x =>
            {
                x.Should().BePresent();
                x.Content().Should.Equal("Link Button");
                x();
            })
            .DivButton.Should().BePresent()
            .DivButton.Click()
            .DisabledButton.Should().BePresent()
            .DisabledButton.Should().BeDisabled()
            .MissingButton.Should().Not.BePresent();

    [Test]
    public void WithNavigation()
    {
        Go.To<BasicControlsPage>()
            .GoToButton.Should().BePresent()
            .GoToButton.Should().BeEnabled()
            .GoToButton();

        Go.To<BasicControlsPage>()
            .GoToInputButton.Should().BePresent()
            .GoToInputButton.Should().BeEnabled()
            .GoToInputButton();

        Go.To<BasicControlsPage>()
            .GoToLink.Should().BePresent()
            .GoToLink.Should().BeEnabled()
            .GoToLink();

        Go.To<BasicControlsPage>()
            .GoToDivButton.Should().BePresent()
            .GoToDivButton.Should().BeEnabled()
            .GoToDivButton();
    }
}
