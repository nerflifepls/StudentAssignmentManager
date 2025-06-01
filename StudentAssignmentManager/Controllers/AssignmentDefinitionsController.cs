using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAssignmentManager.Data;
using StudentAssignmentManager.Models;

namespace StudentAssignmentManager.Controllers;

[Authorize]
public class AssignmentDefinitionsController : Controller
{
    private readonly ApplicationDbContext _context;

    public AssignmentDefinitionsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: AssignmentDefinitions
    public IActionResult Index()
    {
        return View(_context.AssignmentDefinitions.ToList());
    }

    // GET: AssignmentDefinitions/Details/5
    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }
        AssignmentDefinition assignmentDefinition = _context.AssignmentDefinitions.Find(id);
        if (assignmentDefinition == null)
        {
            return NotFound();
        }
        return View(assignmentDefinition);
    }

    // GET: AssignmentDefinitions/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: AssignmentDefinitions/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Id,Title,Description,MaxPoints")] AssignmentDefinition assignmentDefinition)
    {
        if (ModelState.IsValid)
        {
            _context.AssignmentDefinitions.Add(assignmentDefinition);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Assignment definition created successfully.";
            return RedirectToAction("Index");
        }

        return View(assignmentDefinition);
    }

    // GET: AssignmentDefinitions/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }
        AssignmentDefinition assignmentDefinition = _context.AssignmentDefinitions.Find(id);
        if (assignmentDefinition == null)
        {
            return NotFound();
        }
        return View(assignmentDefinition);
    }

    // POST: AssignmentDefinitions/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit([Bind("Id,Title,Description,MaxPoints")] AssignmentDefinition assignmentDefinition)
    {
        if (ModelState.IsValid)
        {
            _context.Entry(assignmentDefinition).State = EntityState.Modified;
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Assignment definition updated successfully.";
            return RedirectToAction("Index");
        }
        return View(assignmentDefinition);
    }

    // GET: AssignmentDefinitions/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }
        AssignmentDefinition assignmentDefinition = _context.AssignmentDefinitions.Find(id);
        if (assignmentDefinition == null)
        {
            return NotFound();
        }
        return View(assignmentDefinition);
    }

    // POST: AssignmentDefinitions/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        AssignmentDefinition assignmentDefinition = _context.AssignmentDefinitions.Find(id);
        // Submissions related to this assignment will be cascade deleted if configured
        _context.AssignmentDefinitions.Remove(assignmentDefinition);
        _context.SaveChanges();
        TempData["SuccessMessage"] = "Assignment definition deleted successfully.";
        return RedirectToAction("Index");
    }

    //todo dont need this
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _context.Dispose();
        }
        base.Dispose(disposing);
    }
}