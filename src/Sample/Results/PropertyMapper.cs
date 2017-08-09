using PropertyMapper.Sample.Destinations;
using PropertyMapper.Sample.Sources;

namespace PropertyMapper.Sample.Results
{
    public class PropertyMapper : IMapper
    {
        public PersonExample1 Map(Person instance) => new PersonExample1
        {
            Gender = instance.Gender,
            Email = instance.Email,
            Dob = instance.Dob,
            Registered = instance.Registered,
            Phone = instance.Phone,
            Cell = instance.Cell,
            Nat = instance.Nat
        };
    }
}