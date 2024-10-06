﻿namespace Atata.IntegrationTests.WebDriver;

public partial class WebDriverSessionTests
{
    public class TakePageSnapshot : WebDriverSessionTestSuiteBase
    {
        [Test]
        public void WhenNavigated()
        {
            var context = BuildAtataContextWithWebDriverSession();
            Go.To<InputPage>();

            context.GetWebDriverSession().TakePageSnapshot();

            context.Artifacts.Should.ContainFile("01 Input page.mhtml");
        }

        [Test]
        public void WhenNoNavigation()
        {
            var context = BuildAtataContextWithWebDriverSession();

            context.GetWebDriverSession().TakePageSnapshot();

            context.Artifacts.Should.ContainFile("01.mhtml");
        }

        [Test]
        public void WhenThrows()
        {
            var context = ConfigureAtataContextWithWebDriverSession(
                session => session.PageSnapshots.UseStrategy(Mock.Of<IPageSnapshotStrategy<WebDriverSession>>(MockBehavior.Strict)))
                .Build();
            Go.To<InputPage>();

            context.GetWebDriverSession().TakePageSnapshot();

            VerifyLastLogMessagesContain(LogLevel.Error, "Page snapshot failed");
            context.Artifacts.Should.Not.Exist();
        }
    }
}