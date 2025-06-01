using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAssignmentManager.Data;
using StudentAssignmentManager.Models;

namespace StudentAssignmentManager.Controllers;

[Authorize]
public class StudentsController : Controller
{
    private readonly ApplicationDbContext _context;

    public StudentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Students
    public IActionResult Index()
    {
        return View(_context.Students.ToList());
    }

    // GET: Students/Details/5
    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }
        var student = _context.Students.Find(id);
        if (student == null)
        {
            return NotFound();
        }
        return View(student);
    }

    // GET: Students/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Students/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Id,FirstName,LastName,RegistrationNumber,Email")] Student student)
    {
        if (ModelState.IsValid)
        {
            // Check for uniqueness manually if Index attributes are not sufficient or for better user feedback
            if (_context.Students.Any(s => s.RegistrationNumber == student.RegistrationNumber))
            {
                ModelState.AddModelError("RegistrationNumber", "This registration number already exists.");
            }
            if (_context.Students.Any(s => s.Email == student.Email))
            {
                ModelState.AddModelError("Email", "This email address already exists.");
            }

            if (ModelState.IsValid) // Re-check after custom validation
            {
                _context.Students.Add(student);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Student created successfully.";
                return RedirectToAction("Index");
            }
        }
        return View(student);
    }

    // GET: Students/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }
        Student student = _context.Students.Find(id);
        if (student == null)
        {
            return NotFound();
        }
        return View(student);
    }

    // POST: Students/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit([Bind("Id,FirstName,LastName,RegistrationNumber,Email")] Student student)
    {
        if (ModelState.IsValid)
        {
            // Check for uniqueness on edit, excluding the current student
            if (_context.Students.Any(s => s.RegistrationNumber == student.RegistrationNumber && s.Id != student.Id))
            {
                ModelState.AddModelError("RegistrationNumber", "This registration number already exists for another student.");
            }
            if (_context.Students.Any(s => s.Email == student.Email && s.Id != student.Id))
            {
                ModelState.AddModelError("Email", "This email address already exists for another student.");
            }

            if (ModelState.IsValid) // Re-check
            {
                _context.Entry(student).State = EntityState.Modified;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Student updated successfully.";
                return RedirectToAction("Index");
            }
        }
        return View(student);
    }

    // GET: Students/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }
        Student student = _context.Students.Find(id);
        if (student == null)
        {
            return NotFound();
        }
        return View(student);
    }

    // POST: Students/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        Student student = _context.Students.Find(id);
        try
        {
            _context.Students.Remove(student);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Student deleted successfully.";
        }
        catch (DbUpdateException ex) // Catch potential FK violation
        {
            TempData["ErrorMessage"] = "Could not delete student. They might be part of a group. Remove them from groups first. Error: " + ex.Message;
            return RedirectToAction("Delete", new { id = id });
        }
        return RedirectToAction("Index");
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _context.Dispose();
        }
        base.Dispose(disposing);
    }
}