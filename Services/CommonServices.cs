using Microsoft.AspNetCore.Mvc.Rendering;

namespace nicenice.Server.Services
{
    public class CommonServices
    {
        public List<SelectListItem> ToSelectListItem<T>(List<T> list) where T : ISelectableEntity
        {
            return list.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
            }).ToList();
        }
        public List<SelectListItem> ToSelectListItem2<T>(List<T> list) where T : ISelectableEntity2
        {
            return list.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
            }).ToList();
        }
        public async Task<byte[]?> ConvertFilesToByteArray(IFormFileCollection files)
        {
            if (files == null || files.Count == 0)
                return null;

            using (var ms = new MemoryStream())
            {
                foreach (var file in files)
                {
                    // Check if the file is valid
                    if (file.Length > 0)
                    {
                        await file.CopyToAsync(ms);
                    }
                }
                return ms.ToArray(); // Returns a byte array containing all files' data
            }
        }
    }
}
