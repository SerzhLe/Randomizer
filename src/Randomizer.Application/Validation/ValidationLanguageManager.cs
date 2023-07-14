using FluentValidation.Resources;
using System.Text.Json;

namespace Randomizer.Application.Validation;

public class ValidationLanguageManager : LanguageManager
{
    public ValidationLanguageManager()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (directory is not null && !directory.GetFiles("*.sln").Any())
        {
            directory = directory.Parent;
        }

        if (directory is null)
        {
            throw new InvalidOperationException("Unable to find solution file.");
        }

        var jsonFilePath = directory.GetFiles("./Randomizer.Application/Validation/ValidationErrorMessagesStore.json", SearchOption.TopDirectoryOnly);

        var jsonFile = jsonFilePath.SingleOrDefault();

        if (jsonFile is null)
        {
            throw new InvalidOperationException("Unable to find validation error message store.");
        }

        var json = File.ReadAllText(jsonFile.FullName);

        var store = JsonSerializer.Deserialize<ValidationErrorMessagesStore>(json);

        if (store is null)
        {
            return;
        }

        foreach (var message in store.ErrorMessages)
        {
            AddTranslation(store.Language, message.Key, message.Value);
        }
    }

    private record ValidationErrorMessagesStore(string Language, Dictionary<string, string> ErrorMessages);
}
