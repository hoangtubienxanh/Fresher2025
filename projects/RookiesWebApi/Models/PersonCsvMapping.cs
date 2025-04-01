using CsvHelper.Configuration;

namespace RookiesWebApi.Models;

public sealed class PersonCsvMapping : ClassMap<Person>
{
    public PersonCsvMapping()
    {
        Map(m => m.FirstName).Index(0).Name("firstName");
        Map(m => m.LastName).Index(1).Name("lastName");
        Map(m => m.BirthPlace).Index(2).Name("birthPlace");
        Map(m => m.PhoneNumber).Index(3).Name("phoneNumber");
        Map(m => m.Gender).Index(4).Name("gender");
        Map(m => m.DateOfBirth).Index(5).Name("dateOfBirth");
        Map(m => m.IsGraduated).Index(6).Name("isGraduated");
    }
}