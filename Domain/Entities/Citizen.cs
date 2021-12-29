namespace Domain.Entities;

using Exceptions;
using Enums;

public class Citizen
{
    public Citizen(Guid id, string name, string surname, uint age, CitizenSex sex)
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
        Sex = sex;
    }

    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Surname { get; init; }
    public uint Age { get; private set; }
    public CitizenSex Sex { get; init; }

    public override bool Equals(object? obj) => Equals(obj as Citizen);
    public override int GetHashCode() => HashCode.Combine(Id, Sex, Name, Surname);

    private bool Equals(Citizen? employee) => employee is not null && employee.Id == Id
                                                                   && employee.Name == Name
                                                                   && employee.Surname == Surname
                                                                   && employee.Sex == Sex
                                                                   && employee.Age == Age;
}