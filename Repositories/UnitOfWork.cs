namespace NoiPurchasingSystem.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IAccountRepository User { get; }
        public IRoleRepository Role { get; }

        public UnitOfWork(IAccountRepository user, IRoleRepository role)
        {
            User = user;
            Role = role;
        }
    }
}
