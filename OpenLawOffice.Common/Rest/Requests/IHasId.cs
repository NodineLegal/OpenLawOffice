namespace OpenLawOffice.Common.Rest.Requests
{
    public interface IHasId<T>
    {
        T Id { get; set; }
    }
}
