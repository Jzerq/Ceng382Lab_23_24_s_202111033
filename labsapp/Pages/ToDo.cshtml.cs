using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{

    
    public class ToDoModel : PageModel
    {
        [BindProperty]
        public TblTodo NewToDo { get; set; } = default!;
        public void OnGet()
        {
            // LINQ query to retrieve items where IsDeleted is false
                    ToDoList = (from item in ToDoDb.TblTodos
                                where item.IsDeleted == false
                                select item).ToList();
        }

        public ToDoDatabaseContext ToDo= new();
        
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid || NewToDo == null)
            {
                return Page();
            }
            NewToDo.IsDeleted = false;
            ToDo.Add(NewToDo);
            ToDo.SaveChanges();
            return RedirectToAction("Get");
        }
        public IActionResult OnPostDelete(int id)
        {
           // var itemToUpdate = ToDoList.FirstOrDefault(item => item.Id == id);
            if (ToDoDb.TblTodos != null)
            {
                var todo = ToDoDb.TblTodos.Find(id);
                if (todo != null)
                {
                    todo.IsDeleted = true;
                    ToDoDb.SaveChanges();
                }
            }            

            return RedirectToAction("Get");
        }


    }
}
