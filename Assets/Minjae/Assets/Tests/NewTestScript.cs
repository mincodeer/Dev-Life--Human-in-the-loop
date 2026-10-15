using NUnit.Framework;

public class NewTestScript
{
    [Test]
    public void AllConditionsMet_HiddenContentIsUnlocked()
    {
        bool result = HiddenContentUnlockRules.CanUnlock(
            true, true, true, true);

        Assert.That(result, Is.True);
    }

    [Test]
    public void SleepNotCompleted_HiddenContentStaysLocked()
    {
        bool result = HiddenContentUnlockRules.CanUnlock(
            true, true, false, true);

        Assert.That(result, Is.False);
    }

    [Test]
    public void NoConditionsMet_HiddenContentStaysLocked()
    {
        bool result = HiddenContentUnlockRules.CanUnlock(
            false, false, false, false);

        Assert.That(result, Is.False);
    }
}