namespace StdLib
{
    public sealed class ApiManager
    {
        static ApiManager instance;
        HttpClientHelper client;

        public UserRequest UserRequest { get; }

        public static ApiManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ApiManager();

                return instance;
            }
        }

        ApiManager()
        {
            client = new HttpClientHelper("https://jsonplaceholder.typicode.com/");
            UserRequest = new UserRequest(client);
        }


    }
}
