using System.Text;

using RookiesWebApp.Models;
using RookiesWebApp.Services;

namespace RookiesWebApp.Tests.Services;

public class RookiesServiceTests
{
    private RookiesService _service;
    private List<Person> _testData;

    [SetUp]
    public void Setup()
    {
        _testData =
        [
            new Person
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "123-456-7890",
                BirthPlace = "New York",
                IsGraduated = true
            },

            new Person
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                Gender = "Female",
                DateOfBirth = new DateTime(2000, 5, 10),
                PhoneNumber = "987-654-3210",
                BirthPlace = "Chicago",
                IsGraduated = false
            },

            new Person
            {
                Id = 3,
                FirstName = "Bob",
                LastName = "Johnson",
                Gender = "Male",
                DateOfBirth = new DateTime(1985, 3, 15),
                PhoneNumber = "555-123-4567",
                BirthPlace = "Los Angeles",
                IsGraduated = true
            }
        ];

        _service = new RookiesService(_testData);
    }

    [Test]
    public void GetAllMembers_ReturnsPaginatedList()
    {
        // Arrange
        var paginationRequest = new PaginationRequest();

        // Act
        var result = _service.GetAllMembers(paginationRequest);

        // Assert
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result, Is.TypeOf<PaginatedList<Person>>());
    }

    [Test]
    public void GetMemberById_WithValidId_ReturnsPerson()
    {
        // Act
        var result = _service.GetMemberById(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.FirstName, Is.EqualTo("John"));
        });
    }

    [Test]
    public void GetMemberById_WithInvalidId_ReturnsNull()
    {
        // Act
        var result = _service.GetMemberById(999);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void CreateMember_AddsPersonToList()
    {
        // Arrange
        var newPerson = new Person
        {
            FirstName = "Test",
            LastName = "User",
            Gender = "Male",
            DateOfBirth = new DateTime(1995, 6, 15),
            PhoneNumber = "555-555-5555",
            BirthPlace = "Test City",
            IsGraduated = true
        };
        var initialCount = _testData.Count;

        // Act
        _service.CreateMember(newPerson);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(_testData, Has.Count.EqualTo(initialCount + 1));
            Assert.That(newPerson.Id, Is.Not.Default);
        });
    }

    [Test]
    public void UpdateMemberById_WithValidId_UpdatesPerson()
    {
        // Arrange
        var updatedPerson = new Person
        {
            Id = 1,
            FirstName = "Updated",
            LastName = "Name",
            Gender = "Male",
            DateOfBirth = new DateTime(1992, 2, 2),
            PhoneNumber = "111-222-3333",
            BirthPlace = "Updated City",
            IsGraduated = false
        };

        // Act
        _service.UpdateMemberById(1, updatedPerson);
        var result = _service.GetMemberById(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.FirstName, Is.EqualTo("Updated"));
            Assert.That(result.LastName, Is.EqualTo("Name"));
            Assert.That(result.DateOfBirth, Is.EqualTo(new DateTime(1992, 2, 2)));
        });
    }

    [Test]
    public void DeleteMember_RemovesPersonFromList()
    {
        // Arrange
        var personToDelete = _service.GetMemberById(1);
        var initialCount = _testData.Count;

        // Act
        _service.DeleteMember(personToDelete!);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(_testData, Has.Count.EqualTo(initialCount - 1));
            Assert.That(_service.GetMemberById(1), Is.Null);
        });
    }

    [Test]
    public void GetAllMaleMembers_ReturnsOnlyMales()
    {
        // Arrange
        var paginationRequest = new PaginationRequest();

        // Act
        var result = _service.GetAllMaleMembers(paginationRequest);

        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.All(p => p.Gender == "Male"), Is.True);
    }

    [Test]
    public void GetOldestMember_WithMembers_ReturnsOldestMember()
    {
        // Arrange
        var oldestPerson = _testData.MinBy(p => p.DateOfBirth);

        // Act
        var result = _service.GetOldestMember();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(oldestPerson!.Id));
            Assert.That(result.FirstName, Is.EqualTo(oldestPerson.FirstName));
            Assert.That(result.DateOfBirth, Is.EqualTo(oldestPerson.DateOfBirth));
        });
    }


    [Test]
    public void GetAllMembersName_ReturnsFormattedNames()
    {
        // Act
        var result = _service.GetAllMembersName();

        // Assert
        Assert.That(result, Is.EqualTo("John Doe, Jane Smith, Bob Johnson"));
    }

    [Test]
    public void GetAllMembersWithPredicate_Equals_FiltersCorrectly()
    {
        // Arrange
        var paginationRequest = new PaginationRequest();

        // Act
        var result = _service.GetAllMembersWithPredicate(ComparisonOperatorType.Equals, paginationRequest);

        // Assert
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].DateOfBirth.Year, Is.EqualTo(2000));
    }

    [Test]
    public void GetAllMembersWithPredicate_GreaterThan_FiltersCorrectly()
    {
        // Arrange
        var paginationRequest = new PaginationRequest();

        // Act
        var result = _service.GetAllMembersWithPredicate(ComparisonOperatorType.GreaterThan, paginationRequest);

        // Assert
        Assert.That(result, Is.Empty); // No one born after 2000 in test data
    }

    [Test]
    public void GetAllMembersWithPredicate_LessThan_FiltersCorrectly()
    {
        // Arrange
        var paginationRequest = new PaginationRequest();

        // Act
        var result = _service.GetAllMembersWithPredicate(ComparisonOperatorType.LessThan, paginationRequest);

        // Assert
        Assert.That(result, Has.Count.EqualTo(2)); // Two people born before 2000
        Assert.That(result.All(p => p.DateOfBirth.Year < 2000), Is.True);
    }

    [Test]
    public void ExportMembersToExcel_ReturnsValidCSVData()
    {
        // Arrange
        var people = new List<Person>
        {
            new()
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890",
                BirthPlace = "New York",
                IsGraduated = true
            }
        };
        var service = new RookiesService(people);
        var paginationRequest = new PaginationRequest();

        // Act
        var result = service.ExportMembersToExcel(paginationRequest);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);

        var fileContent = Encoding.UTF8.GetString(result);
        Assert.That(fileContent, Does.Contain("firstName"));
        Assert.That(fileContent, Does.Contain("lastName"));
        Assert.That(fileContent, Does.Contain("John"));
        Assert.That(fileContent, Does.Contain("Doe"));
    }

    [Test]
    public void ExportMembersToExcel_EmptyList_ReturnsHeadersOnly()
    {
        // Arrange
        var emptyList = new List<Person>();
        var service = new RookiesService(emptyList);
        var paginationRequest = new PaginationRequest { PageIndex = 1, PageSize = 10 };

        // Act
        var result = service.ExportMembersToExcel(paginationRequest);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);

        var fileContent = Encoding.UTF8.GetString(result);
        Assert.That(fileContent, Does.Contain("firstName"));
        Assert.That(fileContent, Does.Contain("lastName"));
        Assert.That(fileContent, Does.Not.Contain("John"));
    }
}