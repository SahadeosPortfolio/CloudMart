namespace Products.Domain.Entities;

public class Brand
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    private Brand() { }

    public Brand(string name)
    {
        Id = Guid.NewGuid();
        SetName(name);
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Brand name is required.");

        Name = name.Trim();
    }

    public override string ToString() => Name;
}
