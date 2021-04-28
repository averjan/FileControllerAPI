using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using KsisLab8.Models;

namespace KsisLab8.Controllers
{
    public class FileController : Controller
    {
        // GET: FileController
        public ActionResult Index()
        {
            return View();
        }

        // POST: FileController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //  curl -i -X PUT -F file=@test.txt -k https://localhost:44382/file/upload/files/test
        [HttpPut]
        public ActionResult Upload(IFormFile file, string path)
        {
            if (file != null)
            {
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                using (var fileStream = new FileStream(path + "/" + file.FileName, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                return Ok();
            }

            Response.StatusCode = 404;
            return Content("Not Found");
        }

        [HttpGet]
        public ActionResult Download(string path)
        {
            if (System.IO.File.Exists(path))
            {
                byte[] data = System.IO.File.ReadAllBytes(path);
                var info = new FileInfo(path);
                string fileName = info.Name;
                string fileType = MimeTypes.GetMimeType(fileName);
                return File(data, fileType, fileName);
            }

            Response.StatusCode = 404;
            return Content("Not Found");
        }

        [HttpGet]
        public ActionResult FileList(string path)
        {
            if (System.IO.Directory.Exists(path))
            {
                var fileList = Directory.GetFiles(path).Select(Path.GetFileName).ToArray();
                return new JsonResult(fileList);
            }

            Response.StatusCode = 404;
            return new EmptyResult();
        }

        [HttpHead]
        //[AcceptVerbs(new[] { "GET", "HEAD" })]
        //  curl -X HEAD https://localhost:44382/file/fileinfo/files/example.txt
        public ActionResult FileInfo(string path)
        {
            //Response.Clear();
            if (System.IO.File.Exists(path))
            {
                var info = new FileInfo(path);
                var customInfo = new FileModel();
                Response.ContentLength = info.Length;
                Response.Headers.Add("File-name", info.Name);
                Response.Headers.Add("Full-name", info.FullName);
                Response.Headers.Add("Full-type", Path.GetExtension(path));
                return new JsonResult(customInfo);
            }

            Response.StatusCode = 404;
            return new EmptyResult();
        }

        [HttpDelete]
        public ActionResult Delete(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                return Ok();
            }

            Response.StatusCode = 404;
            return new EmptyResult();
        }
    }
}
