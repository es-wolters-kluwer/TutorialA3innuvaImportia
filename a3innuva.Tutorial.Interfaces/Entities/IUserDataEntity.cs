namespace a3innuva.Tutorial.Interfaces
{
    public interface IUserDataEntity : IEntity
    {
        string Code { get; set; }
        string Group { get; set; }
        string Name { get; set; }
        string PostalCode { get; set; }
        string VatNumber { get; set; }
        string Letter { get; set; }
    }
}
