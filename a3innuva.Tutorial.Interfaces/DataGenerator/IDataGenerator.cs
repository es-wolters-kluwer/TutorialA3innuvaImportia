namespace a3innuva.Tutorial.Interfaces
{
    using System.Collections.Generic;

    public interface IDataGenerator
    {
        List<IUserDataEntity> GenerateRandomDate(int numberOfEntities);
    }
}
