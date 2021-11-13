using CaptchaN.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CaptchaN.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CaptchaController : ControllerBase
    {
        private readonly IPainter _painter;
        private readonly ICodeTextGenerator _codeTextGenerator;
        private readonly PainterOption _painterOption;

        public CaptchaController(
            IOptions<PainterOption> painterOpt,
            ICodeTextGenerator codeTextGenerator,
            IPainter painter)
        {
            _painterOption = painterOpt.Value;
            _codeTextGenerator = codeTextGenerator;
            _painter = painter;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string codeText = _codeTextGenerator.Generate(4);
            var image = await _painter.GenerateImageAsync(codeText, _painterOption);
            return File(image, "image/jpeg; charset=UTF-8");
        }
    }
}