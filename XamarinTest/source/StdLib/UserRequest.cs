using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StdLib
{
    public class UserRequest
    {
        HttpClientHelper client;
        public UserRequest(HttpClientHelper client)
        {
            this.client = client;
        }

        public async Task<TodoItem> GetTodoItem(int itemId)
        {
            var response = await client.GetObjectFromRequest<TodoItem>($"todos/{itemId}");
            return response.obj;
        }
    }
}
