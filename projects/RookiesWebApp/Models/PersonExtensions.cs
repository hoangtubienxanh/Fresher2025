﻿namespace RookiesWebApp.Models;

public static class PersonExtensions
{
    public static List<Person> GetDummyPeople()
    {
        return new List<Person>
        {
            new()
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                DateOfBirth = new DateTime(2000, 05, 15),
                PhoneNumber = "123-456-7890",
                BirthPlace = "New York City",
                IsGraduated = true
            },
            new()
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                Gender = "Female",
                DateOfBirth = new DateTime(1985, 12, 01),
                PhoneNumber = "987-654-3210",
                BirthPlace = "Los Angeles",
                IsGraduated = true
            },
            new()
            {
                Id = 3,
                FirstName = "Peter",
                LastName = "Jones",
                Gender = "Male",
                DateOfBirth = new DateTime(2002, 08, 22),
                PhoneNumber = "555-123-4567",
                BirthPlace = "Chicago",
                IsGraduated = false
            },
            new()
            {
                Id = 4,
                FirstName = "Alice",
                LastName = "Wonderland",
                Gender = "Female",
                DateOfBirth = new DateTime(1998, 03, 10),
                PhoneNumber = "111-222-3333",
                BirthPlace = "London",
                IsGraduated = true
            },
            new()
            {
                Id = 5,
                FirstName = "Bob",
                LastName = "Builder",
                Gender = "Male",
                DateOfBirth = new DateTime(1975, 07, 04),
                PhoneNumber = "444-555-6666",
                BirthPlace = "Paris",
                IsGraduated = true
            }
        };
    }
}