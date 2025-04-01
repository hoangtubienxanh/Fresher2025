using System.Globalization;

using CsvHelper;

using Microsoft.AspNetCore.Mvc;

using RookiesWebApi.Models;
using RookiesWebApi.Services;

namespace RookiesWebApi.Controllers;

[Route("/NashTech/[controller]/[action]")]
[ApiController]
public class RookiesController(IRookiesService rookiesService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Person>> Index()
    {
        return Ok(rookiesService.GetAllMembers());
    }

    [HttpGet]
    public ActionResult<IEnumerable<Person>> GetMales()
    {
        return Ok(rookiesService.GetAllMaleMembers());
    }

    [HttpGet]
    public ActionResult<Person> GetEldestMember()
    {
        var person = rookiesService.GetOldestMember();
        if (person is null)
        {
            return NotFound();
        }

        return Ok(person);
    }

    [HttpGet]
    public ActionResult<Person> GetAllMembersName()
    {
        return Ok(rookiesService.GetAllMembersName());
    }

    [HttpGet]
    public ActionResult<Person> GetMembersByBirthYearRequirements([FromQuery] string opType)
    {
        if (Enum.TryParse(typeof(ComparisonOperatorType), opType, true, out var comparisonOperatorType))
        {
            return Ok(rookiesService.GetAllMembersWithPredicate((ComparisonOperatorType)comparisonOperatorType));
        }

        return BadRequest();
    }

    [HttpGet]
    public ActionResult<Person> ExportAsExcel()
    {
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<PersonCsvMapping>();
        csv.WriteRecords(rookiesService.GetAllMembers());

        // from docs: https://joshclose.github.io/CsvHelper/getting-started/#:~:text=After%20you%20are%20done%20writing%2C%20you%20should%20call%20writer.Flush()%20to%20ensure%20that%20all%20the%20data%20in%20the%20writer%27s%20internal%20buffer%20has%20been%20flushed%20to%20the%20file
        writer.Flush();

        return File(memoryStream.ToArray(), "text/csv", "export.csv");
    }
}