using PropertyMapper.Sample.Destinations;
using PropertyMapper.Sample.Sources;

namespace PropertyMapper.Sample.Results
{
    public interface IMapper<in TSource, out TDestination> 
    {
        TDestination Map(TSource instance);
    }
    
    public interface IMapper : IMapper<Person, PersonExample1>
    {
    }
}