namespace Marilo.Demo.Components;

public class PageSectionTracker
{
    public record SubSection(string Id, string Title);

    public class Section(string id, string title)
    {
        public string Id { get; } = id;
        public string Title { get; } = title;
        public List<SubSection> SubSections { get; } = [];
    }

    private readonly List<Section> _sections = [];

    public IReadOnlyList<Section> Sections => _sections;

    public event Action? OnChanged;

    public void RegisterSection(string id, string title)
    {
        if (_sections.All(s => s.Id != id))
        {
            _sections.Add(new Section(id, title));
            OnChanged?.Invoke();
        }
    }

    public void RegisterSubSection(string sectionId, string subId, string subTitle)
    {
        var section = _sections.FirstOrDefault(s => s.Id == sectionId);
        if (section is not null && section.SubSections.All(ss => ss.Id != subId))
        {
            section.SubSections.Add(new SubSection(subId, subTitle));
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
