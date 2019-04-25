using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Logo.Api.ApiControllers.Dto;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace Logo.Api.ApiControllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    public class FilesController : Controller
    {
        private readonly IHostingEnvironment environment;
        private readonly string uploadRootDirectory = "uploads";

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="environment"></param>        
        public FilesController(IHostingEnvironment environment)
        {
            this.environment = environment;
        }

        /// <summary>
        /// Upload a file.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> File(IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var data = new byte[file.Length];
            using (Stream stream = file.OpenReadStream())
            {
                await stream.ReadAsync(data, 0, (int)file.Length);
            }
            var dto = new UploadedFileDto();
            dto.Url = await SaveDataToFileAsync(data, Path.GetExtension(file.FileName));
            return Ok(dto);
        }

        /// <summary>
        /// Upload base64 encoded file.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Base64([FromBody]Base64Dto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            const string base64prefix = "base64,";
            int prefixIndex = model.Data.IndexOf(base64prefix);
            // Remove prefix
            if (prefixIndex > -1)
            {
                model.Data = model.Data.Substring(prefixIndex + base64prefix.Length);
            }
            
            byte[] fileData = Convert.FromBase64String(model.Data);
            var dto = new UploadedFileDto();
            dto.Url = await SaveDataToFileAsync(fileData, model.Format);

            return Ok(dto);
        }

        /// <summary>
        /// Upload SVG file.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Svg([FromBody]SvgRawDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            const string fileFormat = "svg";
            byte[] buffer = Encoding.UTF8.GetBytes(model.Data);
            var dto = new UploadedFileDto();
            dto.Url = await SaveDataToFileAsync(buffer, fileFormat);

            return Ok(dto);
        }

        /// <summary>
        /// Save data to a file.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="format"></param>
        /// <returns>Url to created file.</returns>
        private async Task<string> SaveDataToFileAsync(byte[] data, string format)
        {
            if (format.First() != '.')
            {
                format = $".{format}";
            }
            string fileName = $"{Guid.NewGuid()}{format}";
            // Create directories from first 4 symbols, 2 per level
            string directory = Path.Combine(uploadRootDirectory, new string(fileName.Take(2).ToArray()), new string(fileName.Skip(2).Take(2).ToArray()));
            string absoluteDirectoryPath = Path.Combine(environment.WebRootPath, directory);
            Directory.CreateDirectory(absoluteDirectoryPath);
            string filePath = Path.Combine(absoluteDirectoryPath, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.WriteAsync(data, 0, data.Length);
            }
            directory = directory.Replace("\\", "/");
            return $"{Request.Scheme}://{Request.Host}/{directory}/{fileName}";
        }
    }
}
