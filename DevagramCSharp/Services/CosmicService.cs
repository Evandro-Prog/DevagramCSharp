using DevagramCSharp.Dtos;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.InteropServices;

namespace DevagramCSharp.Services
{
    public class CosmicService
    {
        public string EnviarImagem(ImagemDto imagemdto)
        {
            Stream imagem = imagemdto.Imagem.OpenReadStream();             

            var client = new HttpClient(); // classe para requisicoes hhtp

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eWae8y85jo7cBZ1XMs6ytEoaZSxpAJgdwRQ4VkAKxWuhp9nlBd");

            var request = new HttpRequestMessage(HttpMethod.Post, "file");
            var content = new MultipartFormDataContent
            {
                {new StreamContent(imagem), "media", imagemdto.Nome }
            };

            request.Content = content;
            var returnRequest = client.PostAsync("https://workers.cosmicjs.com/v3/buckets/devagramc-devagram/media", request.Content).Result;

            var urlreturn = returnRequest.Content.ReadFromJsonAsync<CosmicRespostaDto>();

            return urlreturn.Result.media.url;
        }
    }
}
