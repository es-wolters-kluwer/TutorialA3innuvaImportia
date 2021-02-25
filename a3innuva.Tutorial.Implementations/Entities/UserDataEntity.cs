namespace a3innuva.Tutorial.Implementations
{
    using System;
    using Interfaces;

    public class UserDataEntity : IUserDataEntity
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Group { get; set; }
        public string Name { get; set; }
        public string PostalCode { get; set; }
        public string VatNumber { get; set; }
        public string Letter { get; set; }
    }
}
