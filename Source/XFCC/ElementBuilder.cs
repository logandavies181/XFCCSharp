namespace XFCC;

///<Summary>
/// Helps build an Element, ensuring that keys are specified exactly once and no unexpected keys are supplied. TODO:
///</Summary>
public class ElementBuilder
{
    private Dictionary<string, string> element = new();

    ///<Summary>
    /// Add a Key-Value pair
    ///</Summary>
    public void Add(string key, string value) =>

        // TODO: raise exception if unknown key
        this.element.Add(key, value);

    ///<Summary>
    /// Construct an Element with the current keys and values.
    ///</Summary>
    public Element Build() => new(
                this.element.GetValueOrDefault(Keys.By),
                this.element.GetValueOrDefault(Keys.Hash),
                this.element.GetValueOrDefault(Keys.Cert),
                this.element.GetValueOrDefault(Keys.Chain),
                this.element.GetValueOrDefault(Keys.Subject),
                this.element.GetValueOrDefault(Keys.URI),
                this.element.GetValueOrDefault(Keys.DNS));

    ///<Summary>
    /// Reset to start building a new Element.
    ///</Summary>
    public void Reset() => this.element = new Dictionary<string, string>();
}
