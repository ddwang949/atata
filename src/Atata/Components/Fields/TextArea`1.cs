﻿using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the text area control (<c>&lt;textarea&gt;</c>).
    /// Default search is performed by the label.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("textarea", IgnoreNameEndings = "TextArea", ComponentTypeName = "text area")]
    [FindByLabel]
    public class TextArea<TOwner> : EditableField<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Gets the value by executing <see cref="ValueGetBehaviorAttribute"/>.
        /// The default behavior is <see cref="ValueGetFromValueAttribute"/>.
        /// </summary>
        /// <returns>
        /// The value.
        /// </returns>
        protected override string GetValue()
        {
            var behavior = Metadata.Get<ValueGetBehaviorAttribute>()
                ?? new ValueGetFromValueAttribute();

            return behavior.Execute(this);
        }

        /// <summary>
        /// Sets the value by executing <see cref="ValueSetBehaviorAttribute"/> behavior.
        /// The default behavior is <see cref="ValueSetUsingClearAndSendKeysAttribute"/>.
        /// If the value is null or empty, calls <see cref="OnClear()"/> method instead.
        /// </summary>
        /// <param name="value">The value.</param>
        protected override void SetValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                OnClear();
            }
            else
            {
                var behavior = Metadata.Get<ValueSetBehaviorAttribute>()
                ?? new ValueSetUsingClearAndSendKeysAttribute();

                behavior.Execute(this, value);
            }
        }

        /// <summary>
        /// Appends the specified value.
        /// Also executes <see cref="TriggerEvents.BeforeSet" /> and <see cref="TriggerEvents.AfterSet" /> triggers.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The owner page object.</returns>
        public TOwner Append(string value)
        {
            ExecuteTriggers(TriggerEvents.BeforeSet);
            Log.Start(new DataAdditionLogSection(this, value) { ActionText = "Append" });

            Scope.SendKeys(Keys.End + value);

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterSet);

            return Owner;
        }

        /// <summary>
        /// Clears the value.
        /// By default uses <see cref="ValueClearUsingClearMethodAttribute"/> behavior.
        /// Also executes <see cref="TriggerEvents.BeforeSet" /> and <see cref="TriggerEvents.AfterSet" /> triggers.
        /// </summary>
        /// <returns>The owner page object.</returns>
        public TOwner Clear()
        {
            ExecuteTriggers(TriggerEvents.BeforeSet);
            Log.Start(new DataClearingLogSection(this));

            OnClear();

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterSet);

            return Owner;
        }

        /// <summary>
        /// Clears the value by executing <see cref="ValueClearBehaviorAttribute"/> behavior.
        /// The default behavior is <see cref="ValueClearUsingClearMethodAttribute"/>.
        /// </summary>
        protected virtual void OnClear()
        {
            var behavior = Metadata.Get<ValueClearBehaviorAttribute>()
                ?? new ValueClearUsingClearMethodAttribute();

            behavior.Execute(this);
        }
    }
}
