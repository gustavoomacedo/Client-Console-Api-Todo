using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoConsoleRequisicaoHttp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<Todo> todos = new List<Todo>();
            Todo todo = new Todo();

            Console.WriteLine("\nDescrição do Todo:\n");
            string descricao = Console.ReadLine();
            todo.titulo = descricao;
            
            Carregar(todos);
            EnviaRequisicaoPOST(todo);

            Console.ReadLine();
        }
        
        public static void EnviaRequisicaoPOST(Todo todo)
        {
            string json = JsonConvert.SerializeObject(todo);

            var dados = Encoding.UTF8.GetBytes(json);
            var requisicaoWeb = WebRequest.CreateHttp("http://toodoapp.azurewebsites.net/api/todo");

            requisicaoWeb.Method = "POST";
            requisicaoWeb.ContentType = "application/json";
            requisicaoWeb.ContentLength = json.Length;
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";

            //precisamos escrever os dados post para o stream
            using (var stream = requisicaoWeb.GetRequestStream())
            {
                stream.Write(dados, 0, dados.Length);
                stream.Close();
            }

            //ler e exibir a resposta
            using (var resposta = requisicaoWeb.GetResponse())
            {
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();
                var _todo = JsonConvert.DeserializeObject<Todo>(objResponse.ToString());
                Console.WriteLine("Produto cadastro!\n");
                streamDados.Close();
                resposta.Close();
            }
            Console.ReadLine();
        }
    
        public static void Carregar(List<Todo> todos)
        {
            //HttpClient webClient = new HttpClient();
            //Uri uri = new Uri("http://toodoapp.azurewebsites.net/api/todo/1");
            //HttpResponseMessage response = await webClient.GetAsync(uri);
            //var jsonString = await response.Content.ReadAsStringAsync();
            //var _Data = JsonConvert.DeserializeObject<List<Todo>>(jsonString);

            var requisicaoWeb = WebRequest.CreateHttp("http://toodoapp.azurewebsites.net/api/todo");
            requisicaoWeb.Method = "GET";
            requisicaoWeb.UserAgent = "RequisicaoWebDemo";

            using (var resposta = requisicaoWeb.GetResponse())
            {
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();

                todos = JsonConvert.DeserializeObject<List<Todo>>(objResponse.ToString());
                foreach (var item in todos)
                {
                    Console.WriteLine("Id: " + item.id + "\nTitulo: " + item.titulo + "\nFeito? " + item.feito);
                    Console.WriteLine("\nItens\n");
                    foreach (var itens in item.item.ToList())
                    {
                        Console.WriteLine("Id: " + itens.id + "\nTarefa: " + itens.descricao + "\nFeito? " + itens.feito);
                    }
                }

                streamDados.Close();
                resposta.Close();
            }

        }
    }
}
