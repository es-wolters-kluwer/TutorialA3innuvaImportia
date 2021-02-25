namespace a3innuva.Tutorial.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using a3innuva.Tutorial.Interfaces;

    public class DataGenerator : IDataGenerator
    {
        private readonly Random random = new Random();

        public List<IUserDataEntity> GenerateRandomDate(int numberOfEntities)
        {
            List<IUserDataEntity> data = new List<IUserDataEntity>();

            for (int i = 0; i < numberOfEntities; i++)
            {
                IUserDataEntity item = new UserDataEntity()
                {
                    Id = Guid.NewGuid(),
                    Code = this.RandomNumberAsString(6),
                    Letter = this.RandomString(1),
                    Name = this.RandomString(12),
                    PostalCode = this.RandomNumberAsString(5),
                    VatNumber = this.RandomString(9),
                    Group = $"{this.random.Next(1, 9).ToString()}{this.RandomNumberAsString(2)}"
                };

                data.Add(item);
            }

            return data;
        }

        private string RandomString(int size)
        {
            StringBuilder stb = new StringBuilder();  
            char letter;  

            for (int i = 0; i < size; i++)
            {
                double flt = this.random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                stb.Append(letter);  
            }

            return stb.ToString();
        }

        private string RandomNumberAsString(int size)
        {
            StringBuilder stb = new StringBuilder();  

            for (int i = 0; i < size; i++)
            {
                int number = this.random.Next(0,9);
                stb.Append(number.ToString());  
            }

            return stb.ToString();
        }
    }
}
