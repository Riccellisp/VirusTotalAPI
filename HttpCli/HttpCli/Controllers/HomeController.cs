using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HttpCli.Controllers
{
    public class HomeController : Controller
    {
        //De fato esse método deve ser assincrono, ja que a saida esperada depende dos parametros de rede
        // por isso o uso do async e await
        public async Task<ActionResult> IndexAsync(HttpPostedFileBase file)
        {
            try
            {
            //Por ser um método que envolve abertura de arquivos, um bloco try catch se faz necessario    
            Debug.WriteLine("cheguei");
            //Pegando o nome do arquivo
            string _FileName = Path.GetFileName(file.FileName);
            //Combinando nome do arquivo e concatenando o caminho completo
            //Vale ressaltar que nesse projeto a pasta UploadedFiles foi criada
            string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
            //Salvando arquivo
            file.SaveAs(_path);
            //Enviando para a viewa confirmação
            ViewBag.Message = "File Uploaded Successfully!!";
            //instanciando classe de maanipulação de arquivos
            var fi = new FileInfo(_path);
            //Cria um container codificado para o tipo multipart/form
            var form = new MultipartFormDataContent();
            //Adicionando o Arquivo stream ao form
            form.Add(new StreamContent(fi.OpenRead()), "file", fi.Name);
            //iniciando conexão
            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            string url = "https://www.virustotal.com/vtapi/v2/file/scan?apikey=2c9268876c5563b7b05b1bc9bd00fdcbbe72f7db022e52d9013f75510ae7d356";
            //REALIZANDO A REQUISIÇÃO POST
                var response = await client.PostAsync(url, form);
                Debug.WriteLine(response.ToString());
                Debug.WriteLine("fiim");
                //CONVERTENDO A MENSAGEM DE REQUISIÇÃO PARA RECEBER NO FORMATO PADRÃO DA API(JSON)
                string x = response.Content.ReadAsStringAsync().Result;
                //ViewBag.x1 = x;
                await GetResults();
            }
            catch
            {

            }
           
            return View();
        }

        //De fato esse método deve ser assincrono, ja que a saida esperada depende dos parametros de rede
        // por isso o uso do async e await
        public async Task<ActionResult> GetResults()
        {
            try
            {
                Debug.WriteLine("chegueiResul");

                var values = new Dictionary<string, string>
                {
                    { "apikey", "2c9268876c5563b7b05b1bc9bd00fdcbbe72f7db022e52d9013f75510ae7d356" },
                    { "resource", "3b508cae5debcba928b5bc355517e2e6" }
                };

                var content = new FormUrlEncodedContent(values);
                HttpClientHandler handler = new HttpClientHandler();
                HttpClient client = new HttpClient(handler);

                var response = await client.PostAsync("https://www.virustotal.com/vtapi/v2/file/report", content);
                Debug.WriteLine("pedi");

                var responseString = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("respondeu");

                ViewBag.x2 = responseString;
                return View();

            }
            catch
            {

            }

            return View();
        }

  


    }
}