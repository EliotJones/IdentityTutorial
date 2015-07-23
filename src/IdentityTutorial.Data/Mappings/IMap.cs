namespace IdentityTutorial.Data.Mappings
{
    public interface IMap<in TSource, out TDestination>
    {
        TDestination Map(TSource source);
    }
}