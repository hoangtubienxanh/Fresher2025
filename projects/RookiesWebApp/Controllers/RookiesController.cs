using System.Globalization;

using CsvHelper;

using Microsoft.AspNetCore.Mvc;

using RookiesWebApp.Models;
using RookiesWebApp.Services;

namespace RookiesWebApp.Controllers;

public class RookiesController(IRookiesService rookiesService) : Controller
{
    // GET: Rookies
    public IActionResult Index([AsParameters] PaginationRequest paginationRequest)
    {
        return View(rookiesService.GetAllMembers(paginationRequest));
    }

    // GET: Rookies/Details/5
    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var person = rookiesService.GetMemberById(id.Value);
        if (person == null)
        {
            return NotFound();
        }

        return View(person);
    }

    // GET: Rookies/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Rookies/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(
        [Bind("Id,FirstName,LastName,Gender,DateOfBirth,PhoneNumber,BirthPlace,IsGraduated")]
        Person person)
    {
        if (ModelState.IsValid)
        {
            rookiesService.CreateMember(person);
            return RedirectToAction(nameof(Index));
        }

        return View(person);
    }

    // GET: Rookies/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var person = rookiesService.GetMemberById(id.Value);
        if (person == null)
        {
            return NotFound();
        }

        return View(person);
    }

    // POST: Rookies/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id,
        [Bind("Id,FirstName,LastName,Gender,DateOfBirth,PhoneNumber,BirthPlace,IsGraduated")]
        Person person)
    {
        if (rookiesService.GetMemberById(id) == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            rookiesService.UpdateMemberById(id, person);
            return RedirectToAction(nameof(Index));
        }

        return View(person);
    }

    // GET: Rookies/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var person = rookiesService.GetMemberById(id.Value);
        if (person == null)
        {
            return NotFound();
        }

        return View(person);
    }

    // POST: Rookies/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var person = rookiesService.GetMemberById(id);
        if (person != null)
        {
            rookiesService.DeleteMember(person);
        }

        return View("DeleteConfirmed", person);
    }

    [HttpGet]
    public IActionResult GetMales([AsParameters] PaginationRequest paginationRequest)
    {
        return View("Index", rookiesService.GetAllMaleMembers(paginationRequest));
    }

    [HttpGet]
    public IActionResult GetEldestMember()
    {
        var person = rookiesService.GetOldestMember();
        if (person is null)
        {
            return NotFound();
        }

        return View("Details", person);
    }

    [HttpGet]
    public IActionResult GetAllMembersName()
    {
        return View("Names", rookiesService.GetAllMembersName());
    }

    [HttpGet]
    public IActionResult GetMembersByBirthYearRequirements([FromQuery] string opType,
        [AsParameters] PaginationRequest paginationRequest)
    {
        if (Enum.TryParse(typeof(ComparisonOperatorType), opType, true, out var comparisonOperatorType))
        {
            var members = rookiesService.GetAllMembersWithPredicate((ComparisonOperatorType)comparisonOperatorType,
                paginationRequest);
            return View("Index", members);
        }

        return BadRequest();
    }

    [HttpGet]
    public ActionResult<Person> ExportAsExcel([AsParameters] PaginationRequest paginationRequest)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<PersonCsvMapping>();
        csv.WriteRecords(rookiesService.GetAllMembers(paginationRequest));

        // from docs: https://joshclose.github.io/CsvHelper/getting-started/#:~:text=After%20you%20are%20done%20writing%2C%20you%20should%20call%20writer.Flush()%20to%20ensure%20that%20all%20the%20data%20in%20the%20writer%27s%20internal%20buffer%20has%20been%20flushed%20to%20the%20file
        writer.Flush();

        return File(memoryStream.ToArray(), "text/csv", "export.csv");
    }
}