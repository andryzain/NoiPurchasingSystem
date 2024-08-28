namespace NoiPurchasingSystem.Repositories
{
    public interface IUnitOfWork
    {
        IAccountRepository User { get; }
        IRoleRepository Role { get; }
    }
}
