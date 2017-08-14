using PropertyMapper.Sample.Destinations;
using PropertyMapper.Sample.Sources;

namespace PropertyMapper.Sample.Results
{
    public class PropertyMapper : IMapper
    {
        PersonExample1 IMapper<Person, PersonExample1>.Map(Person instance) => new PersonExample1
        {
            Gender = instance.Gender,
            Email = instance.Email,
            Dob = instance.Dob,
            Registered = instance.Registered,
            Phone = instance.Phone,
            Cell = instance.Cell,
            Nat = instance.Nat
        };

        LocationExample1 IMapper<Location, LocationExample1>.Map(Location instance) => new LocationExample1
        {
            Street = instance.Street,
            City = instance.City,
            State = instance.State,
            Postcode = instance.Postcode
        };

        PersonExample2 IMapper<Person, PersonExample2>.Map(Person instance) => new PersonExample2
        {
            Gender = instance.Gender,
            Location = ((IMapper<Location, LocationExample1>) this).Map(instance.Location),
            Email = instance.Email,
            Dob = instance.Dob,
            Registered = instance.Registered,
            Phone = instance.Phone,
            Cell = instance.Cell,
            Nat = instance.Nat
        };

        PersonExample3 IMapper<Person, PersonExample3>.Map(Person instance) => new PersonExample3
        {
            Gender = instance.Gender,
            Location = ((IMapper<Location, LocationExample1>) this).Map(instance.Location),
            Email = instance.Email,
            LoginUsername = instance.Login.Username,
            LoginPassword = instance.Login.Password,
            Dob = instance.Dob,
            Registered = instance.Registered,
            Phone = instance.Phone,
            Cell = instance.Cell,
            Id = ((IMapper<Id, IdExample1>) this).Map(instance.Id),
            Nat = instance.Nat
        };

        IdExample1 IMapper<Id, IdExample1>.Map(Id instance) => new IdExample1
        {
            Name = instance.Name,
            Value = instance.Value
        };

        PersonExample4 IMapper<Person, PersonExample4>.Map(Person instance) => new PersonExample4
        {
            Gender = instance.Gender,
            Location = ((IMapper<Location, LocationExample1>) this).Map(instance.Location),
            Email = instance.Email,
            LoginUsername = instance.Login.Username,
            LoginPassword = instance.Login.Password,
            Dob = instance.Dob,
            Registered = instance.Registered,
            Phone = instance.Phone,
            Cell = instance.Cell,
            Id = ((IMapper<Id, IdExample1>) this).Map(instance.Id),
            Nat = instance.Nat,
            PictureIdName = instance.Picture.Id.Name,
            PictureIdValue = instance.Picture.Id.Value
        };

        PersonExample5 IMapper<Person, PersonExample5>.Map(Person instance) => new PersonExample5
        {
            Gender = instance.Gender,
            Location = ((IMapper<Location, LocationExample1>) this).Map(instance.Location),
            Email = instance.Email,
            LoginUsername = instance.Login.Username,
            LoginPassword = instance.Login.Password,
            Dob = instance.Dob,
            Registered = instance.Registered,
            Phone = instance.Phone,
            Cell = instance.Cell,
            Id = ((IMapper<Id, IdExample1>) this).Map(instance.Id),
            Nat = instance.Nat,
            PictureId = ((IMapper<Id, IdExample1>) this).Map(instance.Picture.Id)
        };
    }
}