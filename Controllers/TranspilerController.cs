using Microsoft.AspNetCore.Mvc;

namespace TranspilerAPI.Controllers
{
    [ApiController]
    public class TranspilerController : Controller
    {
        [HttpPost]
        [Route("/transpile")]
        public async Task<IActionResult> Transpile(IFormFile file)
        {
            // Verifica se um arquivo foi enviado
            if (file == null || file.Length == 0)
            {
                return BadRequest("Nenhum arquivo foi enviado.");
            }

            // Verifica a extensão do arquivo para garantir que seja um YAML ou TXT
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (fileExtension != ".yaml" && fileExtension != ".txt")
            {
                return BadRequest("O arquivo não é um YAML ou TXT.");
            }

            List<string> lines = new List<string>();

            // Lê o arquivo e armazena cada linha no array
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    lines.Add(line);
                }
            }

            // Converte a lista em um array
            string[] linesArray = lines.ToArray();

            var htmlDoc = HTMLTranspiler.Transpile(linesArray);

            return Ok(htmlDoc);
        }
    }
}
