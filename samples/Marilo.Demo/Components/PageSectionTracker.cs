namespace Marilo.Demo.Components;

public class PageSectionTracker
{
    private readonly List<(string Id, string Title)> _sections = new();

    public IReadOnlyList<(string Id, string Title)> Sections => _sections;

    public event Action? OnChanged;

    public void Register(string id, string title)
    {
        if (_sections.All(s => s.Id != id))
        {
            _sections.Add((id, title));
            OnChanged?.Invoke();
        }
    }

    public void Clear()
    {
        _sections.Clear();
    }

    public static string ToAnchorId(string title) =>
        title.ToLowerInvariant()
            .Replace(' ', '-')
            .Replace("(", "")
            .Replace(")", "")
            .Replace("&", "")
            .Replace("'", "");
}
