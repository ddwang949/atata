﻿using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for scrolling to control using WebDriver's <see cref="Actions"/>.
    /// Performs <see cref="Actions.MoveToElement(IWebElement)"/> action.
    /// </summary>
    [Obsolete("Use " + nameof(ScrollsUsingActionsAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class ScrollUsingMoveToElementAttribute : ScrollsUsingActionsAttribute
    {
    }
}
