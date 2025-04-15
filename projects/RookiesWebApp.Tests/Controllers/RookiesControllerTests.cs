using Microsoft.AspNetCore.Mvc;

using Moq;

using RookiesWebApp.Controllers;
using RookiesWebApp.Models;
using RookiesWebApp.Services;

namespace RookiesWebApp.Tests.Controllers;

public class RookiesControllerTests
{
    private RookiesController _controller;
    private Mock<IRookiesService> _mockService;
    private List<Person> _testData;

    [SetUp]
    public void Setup()
    {
        _mockService = new Mock<IRookiesService>();
        _controller = new RookiesController(_mockService.Object);

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
    }

    [TearDown]
    public void TearDown()
    {
        _controller.Dispose();
    }

    [Test]
    public void Index_ReturnsViewWithPaginatedList()
    {
        // Arrange
        var paginationRequest = new PaginationRequest();
        var paginatedList = new PaginatedList<Person>(_testData, 1, 10, 3);
        _mockService.Setup(s => s.GetAllMembers(It.IsAny<PaginationRequest>()))
            .Returns(paginatedList);

        // Act
        var result = _controller.Index(paginationRequest);

        // Assert
        Assert.That(result, Is.TypeOf<ViewResult>());
        var viewResult = (ViewResult)result;
        Assert.That(viewResult.Model, Is.TypeOf<PaginatedList<Person>>());
        _mockService.Verify(s => s.GetAllMembers(It.IsAny<PaginationRequest>()), Times.Once);
    }

    [Test]
    public void Details_WithValidId_ReturnsViewWithPerson()
    {
        // Arrange
        const int validId = 1;
        _mockService.Setup(s => s.GetMemberById(validId))
            .Returns(_testData[0]);

        // Act
        var result = _controller.Details(validId);

        // Assert
        Assert.That(result, Is.TypeOf<ViewResult>());
        var viewResult = (ViewResult)result;
        Assert.That(viewResult.Model, Is.TypeOf<Person>());
        var person = (Person)viewResult.Model;
        Assert.That(person.Id, Is.EqualTo(validId));
        _mockService.Verify(s => s.GetMemberById(validId), Times.Once);
    }

    [Test]
    public void Details_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        int? invalidId = 999;
        _mockService.Setup(s => s.GetMemberById(invalidId.Value))
            .Returns(default(Person));

        // Act
        var result = _controller.Details(invalidId);

        // Assert
        Assert.That(result, Is.TypeOf<NotFoundResult>());
        _mockService.Verify(s => s.GetMemberById(invalidId.Value), Times.Once);
    }

    [Test]
    public void Details_WithNullId_ReturnsNotFound()
    {
        // Act
        var result = _controller.Details(null);

        // Assert
        Assert.That(result, Is.TypeOf<NotFoundResult>());
        _mockService.Verify(s => s.GetMemberById(It.IsAny<int>()), Times.Never);
    }

    [Test]
    public void Create_Get_ReturnsView()
    {
        // Act
        var result = _controller.Create();

        // Assert
        Assert.That(result, Is.TypeOf<ViewResult>());
    }

    [Test]
    public void Create_Post_WithValidModel_RedirectsToIndex()
    {
        // Arrange
        var person = new Person
        {
            FirstName = "Test",
            LastName = "User",
            Gender = "Male",
            DateOfBirth = new DateTime(2000, 1, 1),
            PhoneNumber = "123-456-7890",
            BirthPlace = "Test City",
            IsGraduated = true
        };
        _mockService.Setup(s => s.CreateMember(It.IsAny<Person>()));

        // Act
        var result = _controller.Create(person);

        // Assert
        Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        var redirectResult = (RedirectToActionResult)result;
        Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
        _mockService.Verify(s => s.CreateMember(It.Is<Person>(p =>
            p.FirstName == person.FirstName &&
            p.LastName == person.LastName)), Times.Once);
    }

    [Test]
    public void GetMales_ReturnsViewWithMaleMembers()
    {
        // Arrange
        var paginationRequest = new PaginationRequest();
        var malesList = new PaginatedList<Person>(_testData.Where(p => p.Gender == "Male").ToList(), 1, 10, 2);
        _mockService.Setup(s => s.GetAllMaleMembers(It.IsAny<PaginationRequest>()))
            .Returns(malesList);

        // Act
        var result = _controller.GetMales(paginationRequest);

        // Assert
        Assert.That(result, Is.TypeOf<ViewResult>());
        var viewResult = (ViewResult)result;
        Assert.Multiple(() =>
        {
            Assert.That(viewResult.ViewName, Is.EqualTo("Index"));
            Assert.That(viewResult.Model, Is.TypeOf<PaginatedList<Person>>());
        });
        _mockService.Verify(s => s.GetAllMaleMembers(It.IsAny<PaginationRequest>()), Times.Once);
    }

    [Test]
    public void GetEldestMember_ReturnsViewWithEldestPerson()
    {
        // Arrange
        var oldestPerson = _testData.MinBy(p => p.DateOfBirth);
        _mockService.Setup(s => s.GetOldestMember())
            .Returns(oldestPerson);

        // Act
        var result = _controller.GetEldestMember();

        // Assert
        Assert.That(result, Is.TypeOf<ViewResult>());
        var viewResult = (ViewResult)result;
        Assert.Multiple(() =>
        {
            Assert.That(viewResult.ViewName, Is.EqualTo("Details"));
            Assert.That(viewResult.Model, Is.TypeOf<Person>());
        });
        _mockService.Verify(s => s.GetOldestMember(), Times.Once);
    }

    [Test]
    public void GetEldestMember_NoMembersInStore_ReturnsNull()
    {
        // Arrange
        var emptyList = new List<Person>();
        var service = new RookiesService(emptyList);

        // Act
        var result = service.GetOldestMember();

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetAllMembersName_ReturnsViewWithFormattedNames()
    {
        // Arrange
        const string formattedNames = "John Doe, Jane Smith, Bob Johnson";
        _mockService.Setup(s => s.GetAllMembersName()).Returns(formattedNames);

        // Act
        var result = _controller.GetAllMembersName();

        // Assert
        Assert.That(result, Is.TypeOf<ViewResult>());
        var viewResult = (ViewResult)result;
        Assert.That(viewResult.ViewName, Is.EqualTo("Names"));
        Assert.That(viewResult.Model, Is.EqualTo(formattedNames));
    }

    [Test]
    public void GetMembersByBirthYearRequirements_WithValidOperator_ReturnsView()
    {
        // Arrange
        const string opType = "Equals";
        var paginationRequest = new PaginationRequest();
        var filteredList = new PaginatedList<Person>(
            _testData.Where(p => p.DateOfBirth.Year == 2000).ToList(), 1, 10, 1);
        _mockService.Setup(s => s.GetAllMembersWithPredicate(ComparisonOperatorType.Equals,
                It.IsAny<PaginationRequest>()))
            .Returns(filteredList);

        // Act
        var result = _controller.GetMembersByBirthYearRequirements(opType, paginationRequest);

        // Assert
        Assert.That(result, Is.TypeOf<ViewResult>());
        var viewResult = (ViewResult)result;
        Assert.That(viewResult.ViewName, Is.EqualTo("Index"));
        _mockService.Verify(s => s.GetAllMembersWithPredicate(ComparisonOperatorType.Equals,
            It.IsAny<PaginationRequest>()), Times.Once);
    }

    [Test]
    public void GetMembersByBirthYearRequirements_WithInvalidOperator_ReturnsBadRequest()
    {
        // Arrange
        const string opType = "InvalidOperator";
        var paginationRequest = new PaginationRequest();

        // Act
        var result = _controller.GetMembersByBirthYearRequirements(opType, paginationRequest);

        // Assert
        Assert.That(result, Is.TypeOf<BadRequestResult>());
        _mockService.Verify(s => s.GetAllMembersWithPredicate(
            It.IsAny<ComparisonOperatorType>(),
            It.IsAny<PaginationRequest>()), Times.Never);
    }

    [Test]
    public void ExportAsExcel_ReturnsFileWithCorrectContentType()
    {
        // Arrange
        var paginationRequest = new PaginationRequest { PageIndex = 1, PageSize = 10 };
        var csvData = "firstName,lastName\nJohn,Doe"u8.ToArray();
        _mockService.Setup(s => s.ExportMembersToExcel(paginationRequest)).Returns(csvData);

        // Act
        var result = _controller.ExportAsExcel(paginationRequest);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<FileContentResult>());
        var fileResult = result.Result as FileContentResult;
        Assert.That(fileResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(fileResult.ContentType, Is.EqualTo("text/csv"));
            Assert.That(fileResult.FileDownloadName, Is.EqualTo("export.csv"));
            Assert.That(fileResult.FileContents, Is.EqualTo(csvData));
        });
        _mockService.Verify(s => s.ExportMembersToExcel(paginationRequest), Times.Once);
    }

    [Test]
    public void Delete_WithNullId_ReturnsNotFound()
    {
        // Act
        var result = _controller.Delete(null);

        // Assert
        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public void Delete_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        const int invalidId = 999;
        _mockService.Setup(s => s.GetMemberById(invalidId)).Returns(default(Person));

        // Act
        var result = _controller.Delete(invalidId);

        // Assert
        Assert.That(result, Is.TypeOf<NotFoundResult>());
        _mockService.Verify(s => s.GetMemberById(invalidId), Times.Once);
    }

    [Test]
    public void Delete_WithValidId_ReturnsViewWithPerson()
    {
        // Arrange
        const int validId = 1;
        _mockService.Setup(s => s.GetMemberById(validId)).Returns(_testData[0]);

        // Act
        var result = _controller.Delete(validId);

        // Assert
        Assert.That(result, Is.TypeOf<ViewResult>());
        var viewResult = (ViewResult)result;
        Assert.That(viewResult.Model, Is.TypeOf<Person>());
        var person = (Person)viewResult.Model;
        Assert.That(person.Id, Is.EqualTo(validId));
        _mockService.Verify(s => s.GetMemberById(validId), Times.Once);
    }

    [Test]
    public void DeleteConfirmed_WithExistingPerson_DeletesPerson()
    {
        // Arrange
        const int validId = 1;
        var personToDelete = _testData[0];
        _mockService.Setup(s => s.GetMemberById(validId)).Returns(personToDelete);

        // Act
        var result = _controller.DeleteConfirmed(validId);

        // Assert
        Assert.That(result, Is.TypeOf<ViewResult>());
        var viewResult = (ViewResult)result;
        Assert.Multiple(() =>
        {
            Assert.That(viewResult.ViewName, Is.EqualTo("DeleteConfirmed"));
            Assert.That(viewResult.Model, Is.SameAs(personToDelete));
        });
        _mockService.Verify(s => s.DeleteMember(personToDelete), Times.Once);
    }

    [Test]
    public void DeleteConfirmed_WithNonExistingPerson_ReturnsViewWithNullModel()
    {
        // Arrange
        const int invalidId = 999;
        _mockService.Setup(s => s.GetMemberById(invalidId)).Returns(default(Person));

        // Act
        var result = _controller.DeleteConfirmed(invalidId);

        // Assert
        Assert.That(result, Is.TypeOf<ViewResult>());
        var viewResult = (ViewResult)result;
        Assert.Multiple(() =>
        {
            Assert.That(viewResult.ViewName, Is.EqualTo("DeleteConfirmed"));
            Assert.That(viewResult.Model, Is.Null);
        });
        _mockService.Verify(s => s.DeleteMember(It.IsAny<Person>()), Times.Never);
    }

    [Test]
    public void Edit_Get_WithNullId_ReturnsNotFound()
    {
        // Act
        var result = _controller.Edit(null);

        // Assert
        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public void Edit_Get_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        const int invalidId = 999;
        _mockService.Setup(s => s.GetMemberById(invalidId)).Returns(default(Person));

        // Act
        var result = _controller.Edit(invalidId);

        // Assert
        Assert.That(result, Is.TypeOf<NotFoundResult>());
        _mockService.Verify(s => s.GetMemberById(invalidId), Times.Once);
    }

    [Test]
    public void Edit_Get_WithValidId_ReturnsViewWithPerson()
    {
        // Arrange
        const int validId = 1;
        _mockService.Setup(s => s.GetMemberById(validId)).Returns(_testData[0]);

        // Act
        var result = _controller.Edit(validId);

        // Assert
        Assert.That(result, Is.TypeOf<ViewResult>());
        var viewResult = (ViewResult)result;
        Assert.That(viewResult.Model, Is.TypeOf<Person>());
        var person = (Person)viewResult.Model;
        Assert.That(person.Id, Is.EqualTo(validId));
        _mockService.Verify(s => s.GetMemberById(validId), Times.Once);
    }

    [Test]
    public void Edit_Post_WithNonExistingPerson_ReturnsNotFound()
    {
        // Arrange
        const int invalidId = 999;
        var person = new Person
        {
            Id = invalidId,
            FirstName = "Test",
            LastName = "User",
            Gender = "Male",
            DateOfBirth = new DateTime(2000, 1, 1),
            PhoneNumber = "123-456-7890",
            BirthPlace = "Test City",
            IsGraduated = true
        };
        _mockService.Setup(s => s.GetMemberById(invalidId)).Returns(default(Person));

        // Act
        var result = _controller.Edit(invalidId, person);

        // Assert
        Assert.That(result, Is.TypeOf<NotFoundResult>());
        _mockService.Verify(s => s.UpdateMemberById(It.IsAny<int>(), It.IsAny<Person>()), Times.Never);
    }

    [Test]
    public void Edit_Post_WithInvalidModelState_ReturnsView()
    {
        // Arrange
        const int validId = 1;
        var person = new Person
        {
            Id = validId,
            FirstName = "Test",
            LastName = "User",
            Gender = "Male",
            DateOfBirth = new DateTime(2000, 1, 1),
            PhoneNumber = "123-456-7890",
            BirthPlace = "Test City",
            IsGraduated = true
        };
        _mockService.Setup(s => s.GetMemberById(validId)).Returns(_testData[0]);
        _controller.ModelState.AddModelError("Error", "Model error");

        // Act
        var result = _controller.Edit(validId, person);

        // Assert
        Assert.That(result, Is.TypeOf<ViewResult>());
        var viewResult = (ViewResult)result;
        Assert.That(viewResult.Model, Is.SameAs(person));
        _mockService.Verify(s => s.UpdateMemberById(It.IsAny<int>(), It.IsAny<Person>()), Times.Never);
    }

    [Test]
    public void Edit_Post_WithValidModelState_UpdatesPersonAndRedirects()
    {
        // Arrange
        const int validId = 1;
        var person = new Person
        {
            Id = validId,
            FirstName = "Updated",
            LastName = "Person",
            Gender = "Male",
            DateOfBirth = new DateTime(2000, 1, 1),
            PhoneNumber = "123-456-7890",
            BirthPlace = "Test City",
            IsGraduated = true
        };
        _mockService.Setup(s => s.GetMemberById(validId)).Returns(_testData[0]);

        // Act
        var result = _controller.Edit(validId, person);

        // Assert
        Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        var redirectResult = (RedirectToActionResult)result;
        Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
        _mockService.Verify(s => s.UpdateMemberById(validId, person), Times.Once);
    }
}