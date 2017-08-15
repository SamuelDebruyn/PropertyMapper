using System.Collections.Generic;
using System.Linq;
using PropertyMapper.Sample.Destinations;
using PropertyMapper.Sample.Sources;
using PersonExample1 = PropertyMapper.Sample.Sources.PersonExample1;
using PersonExample2 = PropertyMapper.Sample.Destinations.PersonExample2;
using PersonExample3 = PropertyMapper.Sample.Destinations.PersonExample3;

namespace PropertyMapper.Sample.Results
{
    public class PropertyMapper : IMapper
    {
        Destinations.PersonExample1 IMapper<PersonExample1, Destinations.PersonExample1>.Map(PersonExample1 instance) =>
            new Destinations.PersonExample1
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

        PersonExample2 IMapper<PersonExample1, PersonExample2>.Map(PersonExample1 instance) => new PersonExample2
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

        PersonExample3 IMapper<PersonExample1, PersonExample3>.Map(PersonExample1 instance) => new PersonExample3
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

        PersonExample4 IMapper<PersonExample1, PersonExample4>.Map(PersonExample1 instance) => new PersonExample4
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

        PersonExample5 IMapper<PersonExample1, PersonExample5>.Map(PersonExample1 instance) => new PersonExample5
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

        PersonExample6 IMapper<Sources.PersonExample2, PersonExample6>.Map(Sources.PersonExample2 instance) =>
            new PersonExample6
            {
                Gender = (GenderExample1) (int) instance.Gender,
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

        PersonExample7 IMapper<Sources.PersonExample3, PersonExample7>.Map(Sources.PersonExample3 instance) =>
            new PersonExample7
            {
                Gender = instance.Gender,
                Locations = new List<LocationExample1>(instance.Locations.Select(x => ((IMapper<Location, LocationExample1>) this).Map(x))),
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