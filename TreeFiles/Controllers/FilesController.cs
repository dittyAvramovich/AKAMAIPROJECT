using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System;


namespace TreeFiles.Controllers
{
    [ApiController]
    public class FilesController : ControllerBase
    {
        [HttpGet]
        [Route("api/[controller]")]

        public IEnumerable<Directory> Get()
        {
            return GetAllData();
        }

        [HttpGet]
        [Route("api/[controller]/{prefix}")]
        public IEnumerable<Directory> Search([FromRoute]string prefix)
        {
            var data = GetAllData().ToList();
            var dataFilter = new List<Directory>();
            for (int i = 0; i < data.ToList().Count; i++)
            {
                data[i] = FilterByPrefix(prefix, data[i]);
                if (!data[i].Directories.Any())
                    data.RemoveAt(i);
            }
            return data;
        }


        private IEnumerable<Directory> GetAllData()
        {
            string text = System.IO.File.ReadAllText(@"./data.json");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var directories = JsonSerializer.Deserialize<List<Directory>>(text, options);
            return directories;
        }

        private Directory FilterByPrefix(string prefix, Directory? directory)
        {
            directory.Files = directory.Files.ToList().Where(p => p.EndsWith(prefix)).ToList();
            if (directory.Directories.Any())
            {
                for (int i = 0; i < directory.Directories.Count; i++)
                {
                    var result = FilterByPrefix(prefix, directory.Directories[i]);
                    if (result.Files.Any())
                        directory.Directories[i] = result;
                    else directory.Directories.RemoveAt(i);
                }
            }
            return directory;
        }

    }

}
