using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ProjetoConsoleRequisicaoHttp
{
    public class Program
    {
        private const string BaseUrl = "http://toodoapp.azurewebsites.net/api/";
        public static void Main(string[] args)
        {
            List<Todo> todos = new List<Todo>();
            Todo todo = new Todo();
            
            Carregar(todos);

            Console.WriteLine("\nDescrição do Todo:\n");
            string descricao = Console.ReadLine();
            todo.titulo = descricao;

            SalvarTodo(todo);

            Console.WriteLine("\nId para editar:\n");
            string idEdicao = Console.ReadLine();
            EditarTodo(idEdicao);

            Console.WriteLine("\nId para excluir:\n");
            string idExclusao = Console.ReadLine();

            ExcluirTodo(idExclusao);

            Console.ReadLine();
        }

        public static void Carregar(List<Todo> todos)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = httpClient.GetAsync($"{BaseUrl}todo").Result;

            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = response.Content.ReadAsStreamAsync())
                {
                    todos = JsonConvert.DeserializeObject<List<Todo>>(response.Content.ReadAsStringAsync().Result);
                    foreach (var item in todos)
                    {
                        Console.WriteLine("Id: " + item.id + "\nTitulo: " + item.titulo + "\nFeito? " + item.feito);
                        Console.WriteLine("\nItens\n");
                        foreach (var itens in item.item.ToList())
                        {
                            Console.WriteLine("Id: " + itens.id + "\nTarefa: " + itens.descricao + "\nFeito? " + itens.feito);
                        }
                    }
                }
            }
        }

        public static void SalvarTodo(Todo todo)
        {
            using (var client = new HttpClient())
            {
                var httpContent = new StringContent(JsonConvert.SerializeObject(todo).ToString(), Encoding.UTF8, "application/json");

                var response = client.PostAsync($"{BaseUrl}todo", httpContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    using (var responseStream = response.Content.ReadAsStreamAsync())
                    {
                        var content = JsonConvert.DeserializeObject<Todo>(response.Content.ReadAsStringAsync().Result);
                        Console.WriteLine("\nTodo Cadastrado:\n");
                        Console.WriteLine("Id: " + content.id + "\nTitulo: " + content.titulo + "\nFeito? " + content.feito);
                    }
                }
            }
            Console.ReadLine();
        }

        private static void EditarTodo(string id)
        {
            Todo _todo = new Todo();
            using (var client = new HttpClient())
            {
                var responseGet = client.GetAsync($"{BaseUrl}todo/{id}").Result;

                if (responseGet.IsSuccessStatusCode)
                {
                    using (var responseStream = responseGet.Content.ReadAsStreamAsync())
                    {
                        _todo = JsonConvert.DeserializeObject<Todo>(responseGet.Content.ReadAsStringAsync().Result);
                        Console.WriteLine("Id: " + _todo.id + "\nTitulo: " + _todo.titulo + "\nFeito? " + _todo.feito);
                    }
                }

                Console.WriteLine("\n Edicao de todo:\n");
                string titulo = Console.ReadLine();
                _todo.titulo = titulo;

                var httpContent = new StringContent(JsonConvert.SerializeObject(_todo).ToString(), Encoding.UTF8, "application/json");

                var response = client.PutAsync($"{BaseUrl}todo", httpContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    Console.Write("Todo Editado com sucesso.");
                }
                else
                    Console.Write("Erro ao editar todo.");
            }
        }

        private static void ExcluirTodo(string id)
        {
            var httpClient = new HttpClient();

            var response = httpClient.DeleteAsync($"{BaseUrl}todo/{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("\nFoi deletado o Todo com Id " + id + " \n");
            }else
            {
                Console.WriteLine("\n Não Foi deletado o Todo com Id " + id + " \n");
            }
        }
    }
}
