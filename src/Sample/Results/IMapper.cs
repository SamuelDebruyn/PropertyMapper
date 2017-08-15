using PropertyMapper.Sample.Destinations;
using PropertyMapper.Sample.Sources;
using PersonExample1 = PropertyMapper.Sample.Sources.PersonExample1;
using PersonExample2 = PropertyMapper.Sample.Destinations.PersonExample2;
using PersonExample3 = PropertyMapper.Sample.Destinations.PersonExample3;

namespace PropertyMapper.Sample.Results
{
    public interface IMapper<in TSource, out TDestination>
    {
        TDestination Map(TSource instance);
    }
    
    public interface IMapper : IMapper<PersonExample1, Destinations.PersonExample1>,
        IMapper<Location, LocationExample1>, IMapper<PersonExample1, PersonExample2>,
        IMapper<PersonExample1, PersonExample3>, IMapper<Id, IdExample1>, IMapper<PersonExample1, PersonExample4>,
        IMapper<PersonExample1, PersonExample5>, IMapper<Sources.PersonExample2, PersonExample6>,
        IMapper<Sources.PersonExample3, PersonExample7>
    {
    }
}