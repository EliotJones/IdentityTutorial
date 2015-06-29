namespace IdentityTutorial.Store.Mappings
{
    public interface IMap<in TSource, out TDestination>
    {
        TDestination Map(TSource source);
    }
}