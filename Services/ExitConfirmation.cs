namespace OOP_lr2.Services;

public static class ExitConfirmation
{
    public static bool IsConfirmed { get; private set; }

    public static async Task<bool> AskAsync(Page page)
    {
        IsConfirmed = await page.DisplayAlert(
            "Вихід",
            "Чи дійсно ви хочете завершити роботу з програмою?",
            "Так",
            "Ні");
        return IsConfirmed;
    }
}
