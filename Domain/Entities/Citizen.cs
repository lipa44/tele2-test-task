using Domain.Exceptions;

namespace Domain.Entities;

using Enums;

public class Citizen
{
    public Citizen(Guid id, string name, string surname, uint age, CitizenGender gender)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Tele2Exception("Citizen name can't be empty");
        
        if (string.IsNullOrWhiteSpace(surname))
            throw new Tele2Exception("Citizen surname can't be empty");
        
        if (id == Guid.Empty)
            throw new Tele2Exception("Citizen Id can't be default");

        Id = id;
        Name = name;
        Surname = surname;
        Age = age;
        Gender = gender;
    }

    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Surname { get; init; }
    public uint Age { get; init; }
    public CitizenGender Gender { get; init; }
}