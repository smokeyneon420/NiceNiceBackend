namespace nicenice.Server.Services
{
    public interface ISelectableEntity
    {
        int Id { get; }
        string Name { get; }
    }
    public interface ISelectableEntity2
    {
        Guid Id { get; }
        string Name { get; }
    }
}
