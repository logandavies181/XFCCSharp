namespace XFCCSharp;

/// <summary>
/// Helps build an Element, ensuring that keys are specified exactly once and no unexpected keys are supplied. TODO:
/// </summary>
internal sealed class ElementBuilder
{
    private Dictionary<string, string> element = new();

    /// <summary>
    /// Add a Key-Value pair
    /// </summary>
    public void Add(string key, string value) =>

        // TODO: raise exception if unknown key
        this.element.Add(key, value);

    /// <summary>
    /// Construct an Element with the current keys and values.
    /// </summary>
    public Element Build() => new(
                this.element.GetValueOrDefault(Keys.By),
                this.element.GetValueOrDefault(Keys.Hash),
                this.element.GetValueOrDefault(Keys.Cert),
                this.element.GetValueOrDefault(Keys.Chain),
                this.element.GetValueOrDefault(Keys.Subject),
                this.element.GetValueOrDefault(Keys.URI),
                this.element.GetValueOrDefault(Keys.DNS));

    /// <summary>
    /// Reset to start building a new Element.
    /// </summary>
    public void Reset() => this.element = new Dictionary<string, string>();
}
