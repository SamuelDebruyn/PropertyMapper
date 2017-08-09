using PropertyMapper.Sample.Destinations;
using PropertyMapper.Sample.Sources;

namespace PropertyMapper.Sample.Results
{
    public interface IMapper : IMapper<Person, PersonExample1>
    {
        PersonExample1 Map(Person instance);
    }

    public interface IMapper<TSource, TDestination>
    {
        TDestination Map(TSource instance);
    }
}