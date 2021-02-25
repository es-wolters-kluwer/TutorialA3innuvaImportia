namespace a3innuva.Tutorial.Persistence
{
    using a3innuva.Tutorial.Implementations;
    using a3innuva.Tutorial.Interfaces;

    public class UserDataRepository : BaseRepository<IUserDataEntity, UserDataEntity>
    {
        public UserDataRepository(AppDbContext context) : base(context)
        {
        }
    }
}
