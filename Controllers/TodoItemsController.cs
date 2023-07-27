using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlfieTodoList.Models;

namespace AlfieTodoList.Controllers
{
    public class TodoItemsController : Controller
    {
        private readonly TodoItemDbContext _context;

        public TodoItemsController(TodoItemDbContext context)
        {
            _context = context;
        }

        // GET: TodoItems
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> BuildTable()
        {
            int completedCount = 0;
            IEnumerable<TodoItem> items = await _context.TodoItem.ToListAsync();
            foreach (TodoItem item in items)
            {
                if (item.isDone) completedCount++;
            }
            ViewData["Percentage"] = Math.Round(((float)completedCount / items.Count()) * 100);
            return _context.TodoItem != null ?
                        PartialView("_TodoTable", items) :
                        Problem("Entity set 'TodoItemDbContext.TodoItem'  is null.");
        }

        // GET: TodoItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TodoItem == null)
            {
                return NotFound();
            }

            var todoItem = await _context.TodoItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoItem == null)
            {
                return NotFound();
            }

            return View(todoItem);
        }

        // GET: TodoItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TodoItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,isDone")] TodoItem todoItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(todoItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(todoItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AjaxCreate([Bind("Id,Name")] TodoItem todoItem)
        {
            todoItem.isDone = false;
            if (ModelState.IsValid)
            {
                _context.Add(todoItem);
                await _context.SaveChangesAsync();
            }
            IEnumerable<TodoItem> items = await _context.TodoItem.ToListAsync();
            return RedirectToAction("BuildTable");
        }

        // GET: TodoItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TodoItem == null)
            {
                return NotFound();
            }

            var todoItem = await _context.TodoItem.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            return View(todoItem);
        }

        public async Task<IActionResult> AjaxEdit(int? id, bool isDone)
        {
            if (id == null || _context.TodoItem == null)
            {
                return NotFound();
            }

            var todoItem = await _context.TodoItem.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.isDone = isDone;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todoItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoItemExists(todoItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("BuildTable");
            }
            return NotFound();
        }

        // POST: TodoItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,isDone")] TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todoItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoItemExists(todoItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(todoItem);
        }

        // GET: TodoItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TodoItem == null)
            {
                return NotFound();
            }

            var todoItem = await _context.TodoItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoItem == null)
            {
                return NotFound();
            }

            return View(todoItem);
        }

        // POST: TodoItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TodoItem == null)
            {
                return Problem("Entity set 'TodoItemDbContext.TodoItem'  is null.");
            }
            var todoItem = await _context.TodoItem.FindAsync(id);
            if (todoItem != null)
            {
                _context.TodoItem.Remove(todoItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete, ActionName("AjaxDelete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AjaxDeleteConfirmed(int id)
        {
            if (_context.TodoItem == null)
            {
                return Problem("Entity set 'TodoItemDbContext.TodoItem'  is null.");
            }
            var todoItem = await _context.TodoItem.FindAsync(id);
            if (todoItem != null)
            {
                _context.TodoItem.Remove(todoItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("BuildTable");
        }

        private bool TodoItemExists(int id)
        {
          return (_context.TodoItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
