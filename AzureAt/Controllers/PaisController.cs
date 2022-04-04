using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AzureAt.Data;
using AzureAt.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;

namespace AzureAt.Controllers
{
    public class PaisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pais
        public ActionResult Index()
        {
            var paises = new List<Pais>();
            var connectionString = "Server = (localdb)\\mssqllocaldb; Database = STOREPROCEDURES15; Trusted_Connection = True; MultipleActiveResultSets = true";
            SqlConnection connection = new SqlConnection(connectionString);
            {
                var storedprocedure = "ConsultarPais";
                var sqlCommand = new SqlCommand(storedprocedure, connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                try
                {
                    connection.Open();
                    var reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    {
                        while (reader.Read())
                        {
                            var pais = new Pais
                            {
                                PaisId = (int)reader["PaisId"],
                                Nome = reader["Nome"].ToString(),
                                Foto = reader["Foto"].ToString(),
                            };
                            paises.Add(pais);
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
            return View(paises);
        }

        // GET: Pais/Details/5
        public ActionResult Details(int id, Pais pais)
        {

            var connectionString = "Server = (localdb)\\mssqllocaldb; Database = STOREPROCEDURES15; Trusted_Connection = True; MultipleActiveResultSets = true";
            SqlConnection connection = new SqlConnection(connectionString);
            {
                var storedprocedure = "ConsultarPaisDetails";
                var sqlCommand = new SqlCommand(storedprocedure, connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@PaisId", id);

                try
                {
                    connection.Open();

                    var reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    {
                        if (reader.Read())
                        {
                            pais = new Pais
                            {
                                PaisId = (int)reader["PaisId"],
                                Nome = reader["Nome"].ToString(),
                                Foto = reader["Foto"].ToString()
                               

                            };
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
            return View(pais);
        }

        // GET: Pais/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AmigoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Pais pais)
        {

            var Foto = UploadBlob2(pais.Imagem);

            if (ModelState.IsValid)
            {


                var connectionString = "Server = (localdb)\\mssqllocaldb; Database = STOREPROCEDURES15; Trusted_Connection = True; MultipleActiveResultSets = true";
                SqlConnection connection = new SqlConnection(connectionString);
                {
                    var storedprocedure = "CadastrarPais";
                    var sqlCommand = new SqlCommand(storedprocedure, connection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Nome", pais.Nome);
                    sqlCommand.Parameters.AddWithValue("@Foto", await Foto);
                    try
                    {
                        connection.Open();
                        sqlCommand.ExecuteNonQuery();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pais);
        }


        // GET: Pais/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AmigoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("Nome,Foto")] Pais pais)
        {
            if (ModelState.IsValid)
            {
                var connectionString = "Server = (localdb)\\mssqllocaldb; Database = STOREPROCEDURES15; Trusted_Connection = True; MultipleActiveResultSets = true";
                SqlConnection connection = new SqlConnection(connectionString);
                {
                    var storedprocedure = "AtualizarPais";
                    var sqlCommand = new SqlCommand(storedprocedure, connection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@PaisId", id);
                    sqlCommand.Parameters.AddWithValue("@Nome", pais.Nome);
                    sqlCommand.Parameters.AddWithValue("@Foto", pais.Foto);
                    try
                    {
                        connection.Open();
                        sqlCommand.ExecuteNonQuery();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pais);
        }

        // GET: Pais/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AmigoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var connectionString = "Server = (localdb)\\mssqllocaldb; Database = STOREPROCEDURES15; Trusted_Connection = True; MultipleActiveResultSets = true";

            SqlConnection connection = new SqlConnection(connectionString);
            {
                var storedprocedure = "DeletarPais";
                var sqlCommand = new SqlCommand(storedprocedure, connection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@PaisId", id);

                try
                {
                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
                finally
                {
                    connection.Close();
                }
            }
            return RedirectToAction(nameof(Index));
        }
        private bool PaisExists(int id)
        {
            return _context.Pais.Any(e => e.PaisId == id);
        }


        public async Task<string> UploadBlob2(IFormFile imageFile)
        {

            var reader = imageFile.OpenReadStream();
            var cloundStorageAccount = CloudStorageAccount.Parse(@"DefaultEndpointsProtocol=https;AccountName=igorsouza0489;AccountKey=bvr5caFtow9DNwVRtfPZZetD+WM1XG7dqbG1R/AxscBAkmvT12xoqDRg+T8ZL6DULmBWCd+4U9mS+AStnnryYw==;EndpointSuffix=core.windows.net");
            var blobClient = cloundStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("imagecontainer");
            await container.CreateIfNotExistsAsync();
            var blob = container.GetBlockBlobReference(Guid.NewGuid().ToString());
            await blob.UploadFromStreamAsync(reader);
            var uri = blob.Uri.ToString();
            return uri;
        }

        public ActionResult Estados(int id)
        {
            
            var connectionString = "Server = (localdb)\\mssqllocaldb; Database = STOREPROCEDURES15; Trusted_Connection = True; MultipleActiveResultSets = true";
            var estados = new List<Estado>();
            SqlConnection connection = new SqlConnection(connectionString);
            {
                var storedprocedure = "Listadeestados";
                var sqlCommand = new SqlCommand(storedprocedure, connection);

                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@PaisId", id);
                try
                {
                    connection.Open();

                    using (var reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            var estado = new Estado
                            {
                                PaisId = (int)reader["PaisId"],
                                Nome = reader["Nome"].ToString(),
                                Foto = reader["Foto"].ToString(),
                            };
                            estados.Add(estado);
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
            return View(estados);
        }



     

    }
}