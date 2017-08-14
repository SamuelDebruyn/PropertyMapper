using PropertyMapper.Sample.Destinations;
using PropertyMapper.Sample.Sources;

namespace PropertyMapper.Sample.Results
{
    public interface IMapper<in TSource, out TDestination>
    {
        TDestination Map(TSource instance);
    }

    public interface IMapper : IMapper<Person, PersonExample1>, IMapper<Location, LocationExample1>,
        IMapper<Person, PersonExample2>, IMapper<Person, PersonExample3>, IMapper<Id, IdExample1>,
        IMapper<Person, PersonExample4>, IMapper<Person, PersonExample5>
    {
    }
}