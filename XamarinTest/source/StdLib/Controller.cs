using Model;
using System.Threading.Tasks;

namespace StdLib
{
    public class Controller
    {
        public async Task<TodoItem> GetTodoItem(int id)
        {
            return await ApiManager.Instance.UserRequest.GetTodoItem(id);
        }
    }
}
